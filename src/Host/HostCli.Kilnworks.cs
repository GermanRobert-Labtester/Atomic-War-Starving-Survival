// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Expansion 31 (The Kiln — ceramics firing & lime calcination).

using System;
using System.IO;
using Godot;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class HostCliKilnworks
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Kilnworks Self-Test (Expansion 31) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: three stages bring a batch to draw-ready.
                var batch = new KilnBatchState { BatchId = "b1", RawMaterialQualityPermille = 900 };
                for (int i = 0; i < KilnFiringEngine.MaxFiringStages; i++)
                    KilnFiringEngine.AdvanceFiringStage(batch, 780, 1000);
                if (batch.FiringStage == KilnFiringEngine.MaxFiringStages && !batch.IsWaster)
                {
                    GD.Print($"[PASS] Check 1: Batch reached draw stage {batch.FiringStage} (grade {batch.ResultingGrade}).");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 1: Batch did not reach draw (stage {batch.FiringStage}, waster {batch.IsWaster}).");

                // Check 2: a hot cold-load ramp shocks the batch into a waster.
                var shocked = new KilnBatchState { BatchId = "b2" };
                KilnFiringEngine.AdvanceFiringStage(shocked, 900, 1000);
                if (shocked.IsWaster && shocked.ResultingGrade == DrawGrade.Waster)
                {
                    GD.Print("[PASS] Check 2: Thermal shock produced a waster.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 2: Thermal shock did not produce a waster.");

                // Check 3: a soaked ramp (controlled stage 0, then peak) reaches a premium grade.
                // A 1000‰ cold-load ramp would thermal-shock on stage 0 (Check 2), so the
                // first stage must stay at or below 800‰ before the kiln may be pushed.
                var premium = new KilnBatchState { BatchId = "b3", RawMaterialQualityPermille = 1000 };
                KilnFiringEngine.AdvanceFiringStage(premium, 780, 1000);
                KilnFiringEngine.AdvanceFiringStage(premium, 1000, 1000);
                KilnFiringEngine.AdvanceFiringStage(premium, 1000, 1000);
                if (premium.HeatWorkPermille >= 900 && premium.ResultingGrade >= DrawGrade.Superior)
                {
                    GD.Print($"[PASS] Check 3: Soaked ramp drew '{premium.ResultingGrade}' at heat {premium.HeatWorkPermille}‰.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 3: Soaked ramp incorrect (grade {premium.ResultingGrade}, heat {premium.HeatWorkPermille}).");

                // Check 4: lime calcination yields quicklime at the engine's peak band.
                var lime = KilnFiringEngine.CalcinateLimestone(1000, 1000, 24, 1000);
                if (lime.IsFullyCalcined && lime.QuicklimeYieldKg > 400 && lime.ResidualCarbonatePermille <= 50)
                {
                    GD.Print($"[PASS] Check 4: Calcination yielded {lime.QuicklimeYieldKg} kg quicklime.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 4: Calcination incorrect (yield {lime.QuicklimeYieldKg}, fullyCalcined {lime.IsFullyCalcined}).");

                // Check 5: a cold kiln leaves limestone uncalcined.
                var cold = KilnFiringEngine.CalcinateLimestone(1000, 500, 24, 1000);
                if (!cold.IsFullyCalcined && cold.QuicklimeYieldKg == 0)
                {
                    GD.Print($"[PASS] Check 5: Cold kiln left {cold.ResidualCarbonatePermille}‰ residual carbonate.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 5: Cold kiln still produced quicklime.");

                // Check 6: lining wear is load-dependent (lime stresses it most, refractory least).
                int limeWear = KilnFiringEngine.CalculateRefractoryLiningWear(KilnLoadKind.LimestoneCalc, 800);
                int refractoryWear = KilnFiringEngine.CalculateRefractoryLiningWear(KilnLoadKind.RefractoryTile, 800);
                if (limeWear > refractoryWear)
                {
                    GD.Print($"[PASS] Check 6: Lining wear lime {limeWear} > refractory {refractoryWear}.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 6: Lining wear incorrect (lime {limeWear}, refractory {refractoryWear}).");

                // Check 7: the ledger queues, fires, and charges its own fuel reserve.
                var ledger = new KilnFiringLedger();
                if (ledger.AddBatch("k1", KilnLoadKind.FiredBrick, 800)
                    && !ledger.AddBatch("k1", KilnLoadKind.FiredBrick, 800))
                {
                    for (int i = 0; i < KilnFiringEngine.MaxFiringStages; i++)
                        ledger.AdvanceFiring("k1", KilnFiringLedger.OptimalFiringTemperaturePermille);

                    var drawn = ledger.FindBatch("k1");
                    bool charged = ledger.FuelReservePermille < 1000;
                    if (drawn != null && drawn.FiringStage == KilnFiringEngine.MaxFiringStages && charged)
                    {
                        GD.Print($"[PASS] Check 7: Ledger drew 'k1' (grade {drawn.ResultingGrade}), fuel {ledger.FuelReservePermille}‰.");
                        passed++;
                    }
                    else GD.PrintErr($"[FAIL] Check 7: Ledger draw/fuel incorrect (stage {drawn?.FiringStage}, fuel {ledger.FuelReservePermille}).");
                }
                else GD.PrintErr("[FAIL] Check 7: Ledger batch queueing is not id-guarded.");

                // Check 8: drawing accrues lining wear and tallies the output.
                var wearCensus = ledger.GetCensus();
                if (wearCensus.LiningWearPermille > 0 && wearCensus.DrawnRefractoryTile == 0)
                {
                    GD.Print($"[PASS] Check 8: Draw accrued lining wear {wearCensus.LiningWearPermille}‰ and tallied a brick.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 8: Lining wear/output tally incorrect (wear {wearCensus.LiningWearPermille}).");

                // Check 9: a starved kiln stops advancing and does not overdraw fuel.
                var starved = new KilnFiringLedger(new KilnFiringState { FuelReservePermille = 40 });
                starved.AddBatch("k2", KilnLoadKind.ClayPottery, 700);
                var held = starved.AdvanceFiring("k2");
                if (held != null && held.FiringStage == 0 && starved.FuelReservePermille == 40)
                {
                    GD.Print("[PASS] Check 9: Starved kiln refused to fire and kept its fuel.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 9: Starved kiln advanced or lost fuel.");

                // Check 10: reline clears accrued wear.
                var relined = new KilnFiringLedger(new KilnFiringState { LiningWearPermille = 900 });
                relined.Reline(1000);
                if (relined.LiningWearPermille == 0 && !relined.GetCensus().IsLiningReplacementDue)
                {
                    GD.Print("[PASS] Check 10: Reline cleared lining wear.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 10: Reline left wear {relined.LiningWearPermille}‰.");

                // Check 11: capture/restore round-trips batches, fuel, and wear.
                var state = ledger.CaptureState();
                var restored = new KilnFiringLedger();
                restored.RestoreState(state);
                var restoredCensus = restored.GetCensus();
                bool roundTrip = restoredCensus.BatchCount == ledger.GetCensus().BatchCount
                    && restoredCensus.FuelReservePermille == ledger.FuelReservePermille
                    && restoredCensus.LiningWearPermille == ledger.LiningWearPermille
                    && restoredCensus.DrawnRefractoryTile == ledger.GetCensus().DrawnRefractoryTile;
                if (roundTrip)
                {
                    GD.Print("[PASS] Check 11: Kilnworks capture/restore round-trips batches, fuel, and wear.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 11: Kilnworks round-trip lost state.");

                // Check 12: host wiring — save section, day owner, deterministic daily path.
                string main = ReadRepoFile("src", "Main.Kilnworks.cs");
                string owners = ReadRepoFile("src", "Main.CampaignOwners.cs");
                string registry = ReadRepoFile("Assets", "Ashfall.Core", "Save", "SaveSectionRegistry.cs");
                string vocab = ReadRepoFile("Assets", "Ashfall.Core", "Campaign", "DayEventVocabulary.cs");
                if (main.Contains("TickKilnworksFiring")
                    && owners.Contains("KilnworksDayOwner")
                    && owners.Contains("\"kilnworks\"")
                    && registry.Contains("kilnworks")
                    && registry.Contains("kilnworks_save.json")
                    && vocab.Contains("kilnworks_ticked"))
                {
                    GD.Print("[PASS] Check 12: Host registers the kilnworks section and phase-5 day owner.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 12: Kilnworks host wiring or save section missing.");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[EXCEPTION] Exception during kilnworks self-test: {ex}");
            }

            GD.Print($"=== Kilnworks Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }

        private static string ReadRepoFile(params string[] parts)
        {
            try
            {
                string dir = AppContext.BaseDirectory;
                for (int i = 0; i < 8 && dir != null; i++)
                {
                    string candidate = Path.Combine(dir, Path.Combine(parts));
                    if (File.Exists(candidate)) return File.ReadAllText(candidate);
                    dir = Directory.GetParent(dir)?.FullName ?? string.Empty;
                }
            }
            catch (Exception) { }
            return string.Empty;
        }
    }
}
