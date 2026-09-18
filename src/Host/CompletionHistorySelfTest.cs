// SPDX-License-Identifier: MIT
// Wave 11 B3 — concrete user-level persistence proof for completion history.

using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Endgame;

namespace AtomicWar.GodotApp.Host
{
    public static class CompletionHistorySelfTest
    {
        private const string TestPath = "user://wave11_b3_completion_history_selftest.json";

        public static int Run()
        {
            string globalPath = ProjectSettings.GlobalizePath(TestPath);
            int failures = 0;

            void Check(bool condition, string message)
            {
                if (condition) GD.Print("[B3 COMPLETION HISTORY PASS] " + message);
                else
                {
                    GD.PrintErr("[B3 COMPLETION HISTORY FAIL] " + message);
                    failures++;
                }
            }

            try
            {
                if (File.Exists(globalPath)) File.Delete(globalPath);

                var endgameHost = new AtomicWar.GodotApp.EndgameHostSession(
                    new SystemTextJsonSerializer(),
                    new FileSystemIO(),
                    string.Empty,
                    new SeededRng(84));
                int sealEvents = 0;
                endgameHost.CampaignSealed += _ => sealEvents++;
                bool triggered = endgameHost.TriggerEnding(new CampaignEvaluationContext
                {
                    CurrentDay = 360,
                    LivingSurvivors = 9,
                    DeceasedSurvivors = 4,
                    AverageMorale = 60f,
                    ExpeditionsCount = 3
                });
                Check(triggered && endgameHost.SealCampaign(360) && sealEvents == 1,
                    "host forwards the existing campaign-sealed observation exactly once");

                var firstStore = CompletionHistoryStore.Load(TestPath);
                Check(firstStore.IsValid && firstStore.Records.Count == 0, "missing user-level history defaults empty");

                var campaignA = Observation("default/slot_1/seed_17", "ending_dawn_of_thaw", 360);
                Check(firstStore.Append(campaignA, out _) == CompletionHistoryAppendResult.Appended,
                    "first campaign completion appends once");

                var restartedStore = CompletionHistoryStore.Load(TestPath);
                Check(restartedStore.IsValid && restartedStore.Records.Count == 1,
                    "fresh store instance retains campaign A history");
                Check(restartedStore.Append(campaignA, out _) == CompletionHistoryAppendResult.AlreadyRecorded,
                    "re-observing campaign A does not duplicate history");

                var campaignB = Observation("default/slot_2/seed_17", "ending_dawn_of_thaw", 361);
                Check(restartedStore.Append(campaignB, out _) == CompletionHistoryAppendResult.Appended,
                    "different campaign with the same ending appends separately");

                var crossCampaignStore = CompletionHistoryStore.Load(TestPath);
                Check(crossCampaignStore.IsValid && crossCampaignStore.Records.Count == 2,
                    "history survives a second fresh store instance");

                string raw = File.ReadAllText(globalPath);
                File.WriteAllText(globalPath, raw.Replace("ending_dawn_of_thaw", "ending_tampered"));
                Check(!CompletionHistoryStore.Load(TestPath).IsValid,
                    "checksum validation rejects tampered user-level history");
            }
            catch (Exception ex)
            {
                Check(false, "selftest threw " + ex);
            }
            finally
            {
                try
                {
                    if (File.Exists(globalPath)) File.Delete(globalPath);
                }
                catch (Exception cleanupEx)
                {
                    GD.PrintErr("[B3 COMPLETION HISTORY] Failed to clean test file: " + cleanupEx.Message);
                    failures++;
                }
            }

            GD.Print(failures == 0
                ? "[B3 COMPLETION HISTORY] PASS"
                : "[B3 COMPLETION HISTORY] FAIL (" + failures + ")");
            return failures == 0 ? 0 : 1;
        }

        private static CampaignCompletionObservation Observation(string runIdentity, string endingId, int days)
        {
            return new CampaignCompletionObservation(
                runIdentity,
                endingId,
                new EpilogueContextInputs(days, 9, 4, true, false, true, true, false));
        }
    }
}
