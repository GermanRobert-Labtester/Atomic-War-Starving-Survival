// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 199 (Seasonal Human Migration Engine).

using System;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    public static class HostCliHumanMigration
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Seasonal Human Migration Self-Test (Plan 199) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Initial default weights
                var host = HumanMigrationHostSession.Create(dataDir);
                int settlementWeight = host.GetRegionPopulationWeight("settlement");
                int ironBasinWeight = host.GetRegionPopulationWeight("iron_basin");
                if (settlementWeight == 100 && ironBasinWeight == 100)
                {
                    GD.Print("[PASS] Check 1: Known regions initialize to default base weight 100.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 1: Initial weight mismatch: settlement={settlementWeight}, iron_basin={ironBasinWeight}.");
                }

                // Check 2: Catalog loading
                if (host.RegionWeights.Count >= 5)
                {
                    GD.Print($"[PASS] Check 2: Catalog loaded successfully with {host.RegionWeights.Count} regions tracked.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 2: Tracked region count mismatch: {host.RegionWeights.Count}.");
                }

                // Check 3: Phase transition tick (deep_winter on day 1)
                // In catalog: Compact gives +25 to settlement, +15 to iron_basin from Overlay, +20 to industrial_belt from Flotilla
                bool applied1 = host.TickDay(currentDay: 1, seasonPhase: "deep_winter");
                int settlementWinter = host.GetRegionPopulationWeight("settlement");
                if (applied1 && settlementWinter > 100 && host.LastAppliedPhase == "deep_winter" && host.LastTransitionDay == 1)
                {
                    GD.Print($"[PASS] Check 3: Phase transition 'deep_winter' applied on day 1 (settlement weight: {settlementWinter}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 3: Phase transition failed (applied={applied1}, weight={settlementWinter}).");
                }

                // Check 4: Idempotency of same phase
                bool appliedSame = host.TickDay(currentDay: 2, seasonPhase: "deep_winter");
                if (!appliedSame && host.GetRegionPopulationWeight("settlement") == settlementWinter)
                {
                    GD.Print("[PASS] Check 4: Same-phase re-tick was correctly suppressed.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 4: Same-phase re-tick was not suppressed.");
                }

                // Check 5: 10-day dwell hysteresis enforcement
                // Day 5: 5 - 1 = 4 < 10 -> dwell not satisfied -> suppressed
                bool appliedPremature = host.TickDay(currentDay: 5, seasonPhase: "thaw");
                if (!appliedPremature && host.LastAppliedPhase == "deep_winter")
                {
                    GD.Print("[PASS] Check 5: Premature phase transition at day 5 suppressed by 10-day dwell hysteresis.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 5: Premature phase transition was not suppressed.");
                }

                // Check 6: Mature transition applied after dwell threshold
                // Day 12: 12 - 1 = 11 >= 10 -> applied
                bool appliedMature = host.TickDay(currentDay: 12, seasonPhase: "thaw");
                if (appliedMature && host.LastAppliedPhase == "thaw" && host.LastTransitionDay == 12)
                {
                    GD.Print("[PASS] Check 6: Mature phase transition 'thaw' applied on day 12 after dwell requirement met.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 6: Mature phase transition failed.");
                }

                // Check 7: Minimum weight clamp (>= 10)
                var customCatalog = new SeasonalMigrationCatalog
                {
                    schema_version = 1,
                    DwellDays = 1,
                    Factions = new System.Collections.Generic.List<FactionMigrationSchedule>
                    {
                        new FactionMigrationSchedule
                        {
                            FactionId = "faction_depletion",
                            Schedule = new System.Collections.Generic.List<SeasonalMigrationEntry>
                            {
                                new SeasonalMigrationEntry { Phase = "blight", RegionId = "region_depleted", PopulationDelta = -500 }
                            }
                        }
                    }
                };
                var customEngine = new SeasonalHumanMigrationEngine(customCatalog, new[] { "region_depleted" });
                customEngine.TickDay(currentDay: 1, currentSeasonPhase: "blight");
                if (customEngine.GetRegionPopulationWeight("region_depleted") == 10)
                {
                    GD.Print("[PASS] Check 7: Population weight minimum floor clamped at 10.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 7: Population weight floor mismatch: {customEngine.GetRegionPopulationWeight("region_depleted")}.");
                }

                // Check 8: Unregistered region fallback
                int unknownWeight = host.GetRegionPopulationWeight("region_unmapped_zone");
                if (unknownWeight == 100)
                {
                    GD.Print("[PASS] Check 8: Unregistered region correctly returns default weight 100.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 8: Unregistered region weight mismatch: {unknownWeight}.");
                }

                // Check 9: Census reporting
                var census = host.Census;
                if (census.TotalTrackedRegions >= 5 && census.TotalPopulationWeight > 0 && census.LastTransitionDay == 12)
                {
                    GD.Print($"[PASS] Check 9: Census accurate (TrackedRegions={census.TotalTrackedRegions}, TotalWeight={census.TotalPopulationWeight}, LastTransition={census.LastTransitionDay}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 9: Census mismatch: Tracked={census.TotalTrackedRegions}, Weight={census.TotalPopulationWeight}.");
                }

                // Check 10: State capture and restore round-trip
                var snapshot = host.CaptureState();
                var freshHost = HumanMigrationHostSession.Create(dataDir);
                freshHost.RestoreState(snapshot);
                if (freshHost.LastAppliedPhase == "thaw" && freshHost.LastTransitionDay == 12 &&
                    freshHost.GetRegionPopulationWeight("settlement") == host.GetRegionPopulationWeight("settlement"))
                {
                    GD.Print("[PASS] Check 10: Host state capture and restore round-trip verified.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 10: State restore mismatch.");
                }

                // Check 11: Idempotency preserved after restore
                bool reTickRestored = freshHost.TickDay(currentDay: 15, seasonPhase: "thaw");
                if (!reTickRestored)
                {
                    GD.Print("[PASS] Check 11: Idempotency of current phase maintained across restore.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 11: Re-tick after restore was incorrectly applied.");
                }

                // Check 12: Checksummed HumanMigrationSaveStore bare round-trip
                string persistedJson = HumanMigrationSaveStore.TryCapturePersisted(snapshot);
                var restoredFromBare = HumanMigrationSaveStore.TryRestorePersisted(persistedJson);
                if (restoredFromBare != null && restoredFromBare.LastAppliedPhase == "thaw" &&
                    restoredFromBare.RegionWeights.Count == snapshot.RegionWeights.Count)
                {
                    GD.Print("[PASS] Check 12: Checksummed HumanMigrationSaveStore round-trip verified.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 12: HumanMigrationSaveStore bare round-trip failed.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] Exception in HumanMigration self-test: {ex.Message}\n{ex.StackTrace}");
            }

            GD.Print($"=== Human Migration Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
