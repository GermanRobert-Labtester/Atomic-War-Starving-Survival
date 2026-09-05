// SPDX-License-Identifier: MIT
// ASHFALL survivor personal quest headless demo & verification suite (Plan 83 / Task B24).

using System;
using System.IO;
using Ashfall.Core;

namespace Ashfall.Core.Quests
{
    public static class PersonalQuestHeadlessDemo
    {
        public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new HeadlessReport();

            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition)
                {
                    report.PassedCount++;
                    log.Info("[PASS] " + name);
                }
                else
                {
                    report.FailedCount++;
                    log.Error("[FAIL] " + name);
                }
            }

            log.Info("[PersonalQuestHeadlessDemo] begin");

            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            string dataDir = !string.IsNullOrEmpty(dataDirectory) && files.DirectoryExists(dataDirectory)
                ? dataDirectory
                : (files.DirectoryExists("Assets/StreamingAssets/Data") ? "Assets/StreamingAssets/Data" : "StreamingAssets/Data");
            string path = Path.Combine(dataDir, "personal_quests.json");

            Check(files.FileExists(path), "personal_quests.json exists");

            var sys = new PersonalQuestSystem(new SeededRng(83), log);
            if (files.FileExists(path))
            {
                sys.LoadCatalog(files.ReadAllText(path), json);
            }

            Check(sys.Catalog.Count >= 10, $"personal quests catalog has >= 10 arcs (found {sys.Catalog.Count})");
            Check(sys.Catalog.ContainsKey("pq_buried_cache"), "pq_buried_cache present in catalog");
            Check(sys.Catalog.ContainsKey("pq_last_confession"), "pq_last_confession present in catalog");
            Check(sys.Catalog.ContainsKey("pq_radio_echoes"), "pq_radio_echoes present in catalog");
            Check(sys.Catalog.ContainsKey("pq_watchers_remorse"), "pq_watchers_remorse present in catalog");

            // Test quest triggering by trait
            bool trig = sys.TryTriggerQuest("survivor_alpha", "scout", 1);
            Check(trig, "TryTriggerQuest succeeded for survivor with scout trait");
            var active = sys.GetActiveQuest("survivor_alpha");
            Check(active != null && active.questId == "pq_buried_cache", "active quest is pq_buried_cache");

            // Test daily tick progression
            sys.TickDay(2);
            active = sys.GetActiveQuest("survivor_alpha");
            Check(active != null && active.progressCount == 1, "days_elapsed requirement incremented on tick");

            // Test choice transition
            bool choiceOk = sys.ChooseOption("survivor_alpha", "study_thoroughly", 2, out var chosen);
            Check(choiceOk && chosen != null && chosen.morale_delta > 0, "ChooseOption transitioned stage with morale buff");
            Check(active != null && active.currentStage == 1, "currentStage advanced to 1");

            // Test terminal choice completion
            bool completeOk = sys.ChooseOption("survivor_alpha", "share_supplies", 4, out var termChoice);
            Check(completeOk && termChoice != null && termChoice.next_stage == -1, "terminal choice completed quest");
            Check(sys.GetActiveQuest("survivor_alpha") == null, "active quest removed after completion");
            Check(sys.CompletedQuests.Count == 1 && sys.CompletedQuests[0].status == PersonalQuestStatus.Completed,
                "quest recorded in CompletedQuests with Completed status");

            // Test save and restore roundtrip
            var saved = sys.CaptureState();
            var sysRestore = new PersonalQuestSystem(new SeededRng(83), log);
            if (files.FileExists(path)) sysRestore.LoadCatalog(files.ReadAllText(path), json);
            sysRestore.RestoreState(saved);
            Check(sysRestore.CompletedQuests.Count == 1 && sysRestore.CompletedQuests[0].questId == "pq_buried_cache",
                "save state restored faithfully");

            // Test determinism
            string hashA = SaveChecksum.Compute(saved);
            string hashB = SaveChecksum.Compute(sysRestore.CaptureState());
            Check(string.Equals(hashA, hashB, StringComparison.Ordinal), "SaveChecksum is identical across restored systems");

            report.Passed = report.FailedCount == 0 && report.PassedCount > 0;
            report.Summary = $"{report.PassedCount}/{report.PassedCount + report.FailedCount} passed";
            log.Info($"[PersonalQuestHeadlessDemo] completed: {report.Summary}");
            return report;
        }
    }
}
