// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 175 (Meta Progression & Cross-Run Profile Store).

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core.Endgame;

namespace AtomicWar.GodotApp
{
    public static class HostCliMetaProgression
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Meta Progression & Cross-Run Profile Store Self-Test (Plan 175) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Catalog loading
                var host = MetaProgressionHostSession.Create(dataDir);
                var items = host.System.GetAllUnlockables();
                if (items.Count >= 8)
                {
                    GD.Print($"[PASS] Check 1: Catalog loaded successfully ({items.Count} items).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 1: Expected >= 8 unlockables, got {items.Count}.");
                }

                // Check 2: Initial prestige & unlocks
                if (host.TotalPrestige == 0 && host.UnlockedIds.Count == 0 && host.ActiveNgPlusBoons.Count == 0)
                {
                    GD.Print("[PASS] Check 2: Initial meta progression state is cleanly empty.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 2: Initial state not empty. Prestige={host.TotalPrestige}, Unlocked={host.UnlockedIds.Count}.");
                }

                // Check 3: Profile run recording
                var history = new CampaignCompletionHistory
                {
                    schemaVersion = 1,
                    records = new List<CampaignCompletionRecord>
                    {
                        new()
                        {
                            completionId = "comp_test_run_01",
                            runIdentity = "Run Alpha",
                            endingId = "ending_dawn_of_thaw",
                            difficultyPresetId = "difficulty_hardened",
                            daysSurvived = 360,
                            livingDwellers = 12,
                            deathsRecorded = 0,
                            grandTreatySigned = true,
                            tempestDecommissioned = true,
                            debtLedgersBurned = true,
                            childrenSurvived = true,
                            velSecretExposed = true
                        }
                    }
                };

                host.RecordCampaignCompletion(history, achievements: new[] { "first_week_survivor", "two_week_endurance", "month_of_ash", "no_casualties" }, endings: new[] { "ending_dawn_of_thaw" });
                if (host.System.ProfileStore.TotalRunsCompleted == 1)
                {
                    GD.Print("[PASS] Check 3: CrossRunProfileStore successfully ingested campaign completion record.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 3: Profile store did not record run. Count={host.System.ProfileStore.TotalRunsCompleted}.");
                }

                // Check 4: Prestige score calculation
                // Days: 360 * 2 = 720. Living: 12 * 5 = 60. Milestones: 5 * 50 = 250. Total = 1030.
                if (host.TotalPrestige >= 1000)
                {
                    GD.Print($"[PASS] Check 4: Prestige score properly computed ({host.TotalPrestige} points).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 4: Expected prestige >= 1000, got {host.TotalPrestige}.");
                }

                // Check 5: Milestone prestige bonuses included
                if (host.TotalPrestige == 1030)
                {
                    GD.Print("[PASS] Check 5: All 5 signature milestones correctly contributed 250 prestige points.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 5: Milestone calculation unexpected. Expected 1030, got {host.TotalPrestige}.");
                }

                // Check 6: Automatic item unlocking based on prestige and achievements
                if (host.IsUnlocked("meta_veteran_survivor_ribbon") && host.IsUnlocked("meta_emergency_rations_boon"))
                {
                    GD.Print("[PASS] Check 6: Unlocks with satisfied criteria successfully granted.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 6: Expected unlocks missing from unlocked set.");
                }

                // Check 7: Ending-gated unlocking
                if (host.IsUnlocked("meta_dawn_horizon_crest"))
                {
                    GD.Print("[PASS] Check 7: Dawn Horizon Crest unlocked via ending_dawn_of_thaw.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 7: Ending-gated item 'meta_dawn_horizon_crest' not unlocked.");
                }

                // Check 8: Unearned ending items remain locked
                if (!host.IsUnlocked("meta_iron_bastion_crest") && !host.IsUnlocked("meta_flotilla_insignia"))
                {
                    GD.Print("[PASS] Check 8: Items for unachieved endings correctly remain locked.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 8: Unearned ending items were improperly unlocked.");
                }

                // Check 9: New Game+ boon toggle
                bool activated = host.SetNgPlusBoonActive("meta_emergency_rations_boon", true);
                if (activated && host.IsBoonActive("meta_emergency_rations_boon"))
                {
                    GD.Print("[PASS] Check 9: NG+ emergency rations boon successfully activated.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 9: Failed to activate NG+ boon.");
                }

                // Check 10: Census reporting
                var census = host.Census;
                if (census.TotalCatalogItems >= 8 && census.TotalUnlocked > 0 && census.ActiveBoons == 1 && census.RunsRecorded == 1)
                {
                    GD.Print($"[PASS] Check 10: Census accurate (Items={census.TotalCatalogItems}, Unlocked={census.TotalUnlocked}, Boons={census.ActiveBoons}, Runs={census.RunsRecorded}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 10: Census mismatch. Total={census.TotalCatalogItems}, Unlocked={census.TotalUnlocked}, Boons={census.ActiveBoons}.");
                }

                // Check 11: State capture & restore round-trip
                var state = host.CaptureState();
                var freshHost = MetaProgressionHostSession.Create(dataDir);
                freshHost.RestoreState(state);
                if (freshHost.TotalPrestige == host.TotalPrestige && freshHost.IsUnlocked("meta_dawn_horizon_crest") && freshHost.IsBoonActive("meta_emergency_rations_boon"))
                {
                    GD.Print("[PASS] Check 11: Host state capture and restore round-trip verified.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 11: State restore did not retain prestige, unlocked items, or boons.");
                }

                // Check 12: Checksummed SaveStore roundtrip
                string persistedJson = MetaProgressionSaveStore.TryCapturePersisted(state);
                var parsedBack = MetaProgressionSaveStore.TryRestorePersisted(persistedJson);
                if (parsedBack != null && parsedBack.totalPrestigeEarned == state.totalPrestigeEarned && parsedBack.unlockedIds.Count == state.unlockedIds.Count)
                {
                    GD.Print("[PASS] Check 12: MetaProgressionSaveStore serialization and round-trip verified.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 12: MetaProgressionSaveStore round-trip failed.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] Exception in MetaProgression self-test: {ex.Message}\n{ex.StackTrace}");
            }

            GD.Print($"=== Meta Progression Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
