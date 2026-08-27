using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.MoralChoice;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        /// <summary>
        /// --moral-choice-selftest: catalog load via the static id class, a
        /// scripted arc through the live system (score, empathy, listener
        /// threshold, overnight reconcile events, ending lock), the journal
        /// resolution hook, and the checksummed save round-trip with tamper
        /// and empty-checksum rejection.
        /// </summary>
        public static int RunMoralChoiceSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            int failures = 0;
            void Check(bool ok, string label)
            {
                GD.Print($"[{(ok ? "PASS" : "FAIL")}] {label}");
                if (!ok) failures++;
            }

            // 1. Catalog loads all 60 quests.
            var defs = MoralChoiceCatalogLoader.Load(dataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            Check(defs.Count == MoralChoiceIds.BaseQuestCount,
                $"catalog loads {MoralChoiceIds.BaseQuestCount} quests (got {defs.Count})");

            MoralChoiceQuestDefinition? Def(string id) => defs.Find(d => d.Id == id);

            // 2. Scripted arc through the live system, resolved by static id.
            var sys = new MoralChoiceSystem(new SeededRng(2026));
            var journal = new JournalSystem();
            int journalAdds = 0;
            journal.OnEntryAdded += _ => journalAdds++;
            sys.OnQuestResolved += r =>
            {
                string arrow = r.impactMark == "up" ? "🔺" : r.impactMark == "down" ? "🔻" : "⚪";
                journal.TryAddRawEntry(r.questId, $"{arrow} {r.epitaph}", null!, r.resolvedDay);
            };

            var arc = new (string id, int choice)[]
            {
                (MoralChoiceIds.ShareChild, 0),      // +10 moral, +1 empathy
                (MoralChoiceIds.DeadUnmarked, 0),    // +12, +2
                (MoralChoiceIds.ListenOldMan, 1),    // +12, +3
                (MoralChoiceIds.ComfortWidow, 0),    // +14, +4
                (MoralChoiceIds.TrustFire, 0),       // +10, +2
                (MoralChoiceIds.TrustMessenger, 0),  // +20, +4
            };
            int day = 4;
            foreach (var step in arc)
            {
                var def = Def(step.id);
                if (def == null)
                {
                    Check(false, $"definition exists for {step.id}");
                    continue;
                }
                sys.Resolve(def, step.choice, def.LocationId, day);
                day += 3;
            }

            Check(sys.MoralScore == 78, $"arc moral score 78 (got {sys.MoralScore})");
            Check(sys.EmpathyPoints == 16, $"arc empathy 16 (got {sys.EmpathyPoints})");
            Check(sys.IsListener, "listener threshold crossed at 16 empathy");
            Check(journalAdds == arc.Length, $"journal hook wrote {arc.Length} entries (got {journalAdds})");

            // 3. Overnight reconcile settles the Positive-band contract exactly once.
            var fired = new List<string>();
            sys.OnThresholdEventFired += fired.Add;
            sys.Reconcile(day);
            Check(fired.Contains(MoralChoiceSystem.EventContractTaken),
                "reconcile settles contract_taken when crossing into Positive");
            sys.Reconcile(day + 1);
            Check(fired.Count == 1, $"one-time events do not re-fire (got {fired.Count})");

            // 4. Ending sanity: 6 resolved quests is below the lock, so the
            // mild positive ending fires regardless of the score.
            Check(sys.SelectEnding() == MoralEndingKind.CommunityBuilder,
                "ending below the 20-quest lock falls to CommunityBuilder");

            // 5. Checksummed save round-trip through the host store.
            string tmpDir = Path.Combine(Path.GetTempPath(), "ashfall_moral_choice_selftest");
            Directory.CreateDirectory(tmpDir);
            string path = Path.Combine(tmpDir, MoralChoiceSaveStore.FileName);
            try { File.Delete(path); } catch { /* fresh run */ }

            MoralChoiceSaveStore.Save(sys.CaptureState(), path);
            var loaded = MoralChoiceSaveStore.TryLoad(path);
            Check(loaded != null, "save round-trip loads");
            if (loaded != null)
            {
                var restored = new MoralChoiceSystem(new SeededRng(1));
                restored.RestoreState(loaded);
                Check(restored.MoralScore == sys.MoralScore
                      && restored.EmpathyPoints == sys.EmpathyPoints
                      && restored.QuestsResolved == sys.QuestsResolved,
                      "restored ledger matches (score/empathy/resolutions)");
            }

            // 6. Tamper rejection: a mutated payload must fail the checksum.
            string json = File.ReadAllText(path);
            string tampered = json.Replace("\"moralScore\":78", "\"moralScore\":79", StringComparison.Ordinal);
            Check(tampered != json, "tamper probe found the moral score field");
            File.WriteAllText(path, tampered);
            Check(MoralChoiceSaveStore.TryLoad(path) == null, "tampered save rejected by checksum");

            // 7. Empty-checksum envelope is corrupt, not legacy.
            File.WriteAllText(path,
                "{\"State\":{\"systemId\":\"moral_choice\",\"schemaVersion\":1,\"moralScore\":5},\"Checksum\":\"\"}");
            Check(MoralChoiceSaveStore.TryLoad(path) == null, "empty-checksum envelope rejected as corrupt");

            return EmitSummary("moral_choice_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }
    }
}
