// SPDX-License-Identifier: MIT
// ASHFALL campaign endgame headless demo & verification suite (Plan 84 / Task B25).

using System;
using System.IO;
using Ashfall.Core;

namespace Ashfall.Core.Endgame
{
    public static class EndgameHeadlessDemo
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

            log.Info("[EndgameHeadlessDemo] begin");

            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            string dataDir = !string.IsNullOrEmpty(dataDirectory) && files.DirectoryExists(dataDirectory)
                ? dataDirectory
                : (files.DirectoryExists("Assets/StreamingAssets/Data") ? "Assets/StreamingAssets/Data" : "StreamingAssets/Data");
            string path = Path.Combine(dataDir, "endings.json");

            Check(files.FileExists(path), "endings.json exists");

            var sys = new EndgameSystem(new SeededRng(84), log);
            if (files.FileExists(path))
            {
                sys.LoadCatalog(files.ReadAllText(path), json);
            }

            Check(sys.Catalog.Count >= 8, $"endings catalog has >= 8 endings (found {sys.Catalog.Count})");
            Check(sys.Catalog.ContainsKey("ending_dawn_of_thaw"), "ending_dawn_of_thaw present in catalog");
            Check(sys.Catalog.ContainsKey("ending_silent_tombs"), "ending_silent_tombs present in catalog");
            Check(sys.Catalog.ContainsKey("ending_iron_hegemony"), "ending_iron_hegemony present in catalog");
            Check(sys.Catalog.ContainsKey("ending_exodus_to_sea"), "ending_exodus_to_sea present in catalog");

            // Evaluate Extinction
            var ctxExtinct = new CampaignEvaluationContext { CurrentDay = 90, LivingSurvivors = 0 };
            var endingExtinct = sys.EvaluateEnding(ctxExtinct);
            Check(endingExtinct.id == "ending_silent_tombs", "extinction selects ending_silent_tombs");

            // Evaluate Year Two Dawn of Thaw
            var ctxThaw = new CampaignEvaluationContext { CurrentDay = 360, LivingSurvivors = 14 };
            var endingThaw = sys.EvaluateEnding(ctxThaw);
            Check(endingThaw.id == "ending_dawn_of_thaw", "day 360 selects ending_dawn_of_thaw");

            // Evaluate Garrison Dominance
            var ctxGarrison = new CampaignEvaluationContext { CurrentDay = 240, LivingSurvivors = 10, DominantFaction = "garrison" };
            var endingGarrison = sys.EvaluateEnding(ctxGarrison);
            Check(endingGarrison.id == "ending_iron_hegemony", "garrison dominance selects ending_iron_hegemony");

            // Trigger Ending and verify Epilogue
            bool trig = sys.TriggerEnding(ctxThaw);
            Check(trig, "TriggerEnding succeeded");
            Check(sys.Phase == EndgamePhase.Epilogue, "Phase transitioned to Epilogue");
            Check(sys.State.epilogueReport != null && sys.State.epilogueReport.daysSurvived == 360, "Epilogue report generated");

            // Seal Campaign
            bool sealedOk = sys.SealCampaign(360);
            Check(sealedOk && sys.IsSealed && sys.Phase == EndgamePhase.Sealed, "SealCampaign permanently locked campaign");
            Check(!sys.TriggerEnding(ctxThaw), "TriggerEnding blocked after campaign sealed");

            // Save and Restore Roundtrip
            var saved = sys.CaptureState();
            var sysRestore = new EndgameSystem(new SeededRng(84), log);
            if (files.FileExists(path)) sysRestore.LoadCatalog(files.ReadAllText(path), json);
            sysRestore.RestoreState(saved);
            Check(sysRestore.IsSealed && sysRestore.Phase == EndgamePhase.Sealed, "Sealed state restored faithfully");

            // Determinism
            string hashA = SaveChecksum.Compute(saved);
            string hashB = SaveChecksum.Compute(sysRestore.CaptureState());
            Check(string.Equals(hashA, hashB, StringComparison.Ordinal), "SaveChecksum identical across restored systems");

            report.Passed = report.FailedCount == 0 && report.PassedCount > 0;
            report.Summary = $"{report.PassedCount}/{report.PassedCount + report.FailedCount} passed";
            log.Info($"[EndgameHeadlessDemo] completed: {report.Summary}");
            return report;
        }
    }
}
