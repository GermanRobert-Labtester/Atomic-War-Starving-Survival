// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : ChemicalReagentSynthesisSelfTest
// Subsystem          : Expansion 39 — The Reagent: Chemical Synthesis Safety
// Authority          : docs/expansions/wave6/expansion_39_the_reagent_plan.md
// ============================================================================

using System;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class HostCliChemicalReagentSynthesis
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Chemical Synthesis Safety & Reagent Grade Self-Test (Expansion 39) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Default reactor initialization
                var session = ChemicalReagentSynthesisHostSession.Create();
                var defaultReactor = session.Ledger.GetReactor("primary_synthesis_vessel");
                if (defaultReactor != null &&
                    defaultReactor.OperatingTemperaturePermille == 350 &&
                    defaultReactor.OperatingPressurePermille == 300 &&
                    defaultReactor.HazardState == SynthesisReactorHazardState.Stable)
                {
                    Console.WriteLine("[PASS] Check 1: Default primary synthesis vessel initialized with stable thermal and pressure baseline.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 1: Default reactor initialization failed.");
                }

                // Check 2: Synthesis execution, output yield, and catalyst wear
                int startCatalyst = defaultReactor?.CatalystActivityPermille ?? 0;
                var stepResult = session.ExecuteSynthesisCycle(
                    reactorId: "primary_synthesis_vessel",
                    targetBatchKg: 30,
                    reactantRatioPermille: 950,
                    operatorSkillPermille: 850,
                    timestampTicks: 101);

                if (!stepResult.IsReactionAborted &&
                    stepResult.OutputBatchKg > 0 &&
                    stepResult.CatalystDegradationPermille > 0 &&
                    defaultReactor!.CatalystActivityPermille < startCatalyst)
                {
                    Console.WriteLine($"[PASS] Check 2: Synthesis cycle produced {stepResult.OutputBatchKg}kg reagent with {stepResult.CatalystDegradationPermille}\u2030 catalyst degradation.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: Synthesis execution or catalyst degradation failed.");
                }

                // Check 3: Thermal and pressure shift scaling
                var thermalReactor = new SynthesisReactorState
                {
                    ReactorId = "thermal_test_vessel",
                    OperatingTemperaturePermille = 400,
                    OperatingPressurePermille = 300,
                    CoolingCapacityPermille = 200, // low cooling to allow heating
                    CatalystActivityPermille = 900
                };
                session.RegisterOrUpdateReactor(thermalReactor);
                session.ExecuteSynthesisCycle(
                    reactorId: "thermal_test_vessel",
                    targetBatchKg: 80, // large batch = high heat
                    reactantRatioPermille: 1200,
                    operatorSkillPermille: 500,
                    timestampTicks: 102);

                var updatedThermalReactor = session.Ledger.GetReactor("thermal_test_vessel");
                if (updatedThermalReactor != null &&
                    updatedThermalReactor.OperatingTemperaturePermille > 400 &&
                    updatedThermalReactor.OperatingPressurePermille > 300)
                {
                    Console.WriteLine($"[PASS] Check 3: Thermal shift scaled with reaction batch size ({updatedThermalReactor.OperatingTemperaturePermille}\u2030 temp, {updatedThermalReactor.OperatingPressurePermille}\u2030 pressure).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 3: Thermal/pressure shift failed to scale.");
                }

                // Check 4: High purity variables yield AnalyticalPharma grade
                var pharmaGrade = ChemicalReagentSynthesisEngine.DeterminePurityGrade(
                    feedstockPurityPermille: 950,
                    catalystActivityPermille: 940,
                    processStabilityPermille: 920);

                if (pharmaGrade == ReagentPurityGrade.AnalyticalPharma)
                {
                    Console.WriteLine("[PASS] Check 4: High purity feedstock and catalyst yielded AnalyticalPharma grade (>99% purity).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: Expected AnalyticalPharma but got {pharmaGrade}.");
                }

                // Check 5: Low purity variables yield Technical/Crude grade
                var crudeGrade = ChemicalReagentSynthesisEngine.DeterminePurityGrade(
                    feedstockPurityPermille: 400,
                    catalystActivityPermille: 300,
                    processStabilityPermille: 400);

                if (crudeGrade == ReagentPurityGrade.Crude)
                {
                    Console.WriteLine("[PASS] Check 5: Low purity variables correctly mapped to Crude reagent grade.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: Expected Crude but got {crudeGrade}.");
                }

                // Check 6: Extreme temp and pressure trigger ThermalExcursionRunaway
                var runawayReactor = new SynthesisReactorState
                {
                    ReactorId = "runaway_vessel",
                    OperatingTemperaturePermille = 870,
                    OperatingPressurePermille = 910,
                    CoolingCapacityPermille = 50,
                    CatalystActivityPermille = 800
                };
                session.RegisterOrUpdateReactor(runawayReactor);
                var runawayResult = session.ExecuteSynthesisCycle(
                    reactorId: "runaway_vessel",
                    targetBatchKg: 60,
                    reactantRatioPermille: 1300,
                    operatorSkillPermille: 200,
                    timestampTicks: 103);

                if (runawayResult.IsReactionAborted &&
                    runawayResult.HazardState == SynthesisReactorHazardState.ThermalExcursionRunaway &&
                    runawayResult.OutputBatchKg == 0)
                {
                    Console.WriteLine("[PASS] Check 6: Thermal excursion runaway triggered and aborted reaction on supercritical envelope.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 6: Runaway conditions did not trigger abortion.");
                }

                // Check 7: Runaway increments safety alerts and runaway counters
                var censusAfterRunaway = session.Census;
                if (censusAfterRunaway.ThermalRunawayCount > 0 &&
                    censusAfterRunaway.SafetyAlertCount > 0)
                {
                    Console.WriteLine($"[PASS] Check 7: Safety counters incremented upon thermal runaway (Runaways: {censusAfterRunaway.ThermalRunawayCount}, Alerts: {censusAfterRunaway.SafetyAlertCount}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 7: Safety counters failed to increment.");
                }

                // Check 8: Stoichiometric mass balance calculation
                var massBalance = session.EvaluateMassBalance(
                    precursorInputKg: 120,
                    stoichiometricRatioPermille: 1000,
                    conversionRatePermille: 750);

                if (massBalance.TheoreticalYieldKg == 120 &&
                    massBalance.ActualYieldKg == 90 &&
                    massBalance.UnreactedPrecursorKg == 30)
                {
                    Console.WriteLine("[PASS] Check 8: Mass balance accurately calculated theoretical yield (120kg), actual yield (90kg), and unconverted mass (30kg).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 8: Mass balance calculation incorrect.");
                }

                // Check 9: Neutralization of acidic waste effluent
                int wasteLitres = session.Census.AccumulatedAcidicWasteLitres;
                int alkaliStockBefore = session.Census.NeutralizingAlkaliStockKg;
                bool neutralized = session.NeutralizeAcidicWaste(wasteLitres, acidConcentrationPermille: 250);

                if (neutralized &&
                    session.Census.AccumulatedAcidicWasteLitres == 0 &&
                    session.Census.NeutralizingAlkaliStockKg < alkaliStockBefore)
                {
                    Console.WriteLine("[PASS] Check 9: Acidic waste effluent neutralized successfully using alkali buffer.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 9: Acidic waste neutralization failed.");
                }

                // Check 10: Waste neutralization rejection on insufficient alkali
                var drySession = ChemicalReagentSynthesisHostSession.Create();
                var dryState = drySession.CaptureState();
                dryState.NeutralizingAlkaliStockKg = 1;
                dryState.AccumulatedAcidicWasteLitres = 400;
                drySession.RestoreState(dryState);

                bool dryNeutralized = drySession.NeutralizeAcidicWaste(400, acidConcentrationPermille: 900);
                if (!dryNeutralized && drySession.Census.AccumulatedAcidicWasteLitres == 400)
                {
                    Console.WriteLine("[PASS] Check 10: Waste neutralization correctly rejected when alkali reserve is depleted.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 10: Deficit alkali check failed.");
                }

                // Check 11: Daily advancement ambient cooling and waste alerting
                var daySession = ChemicalReagentSynthesisHostSession.Create();
                var hotVessel = new SynthesisReactorState
                {
                    ReactorId = "hot_idle_vessel",
                    OperatingTemperaturePermille = 650,
                    OperatingPressurePermille = 550,
                    HazardState = SynthesisReactorHazardState.ElevatedPressure
                };
                daySession.RegisterOrUpdateReactor(hotVessel);
                var dayState = daySession.CaptureState();
                dayState.AccumulatedAcidicWasteLitres = 600; // Above 500L threshold
                daySession.RestoreState(dayState);

                int alertsBefore = daySession.Census.SafetyAlertCount;
                daySession.AdvanceDay(15);

                var cooledVessel = daySession.Ledger.GetReactor("hot_idle_vessel");
                if (cooledVessel != null &&
                    cooledVessel.OperatingTemperaturePermille < 650 &&
                    cooledVessel.OperatingPressurePermille < 550 &&
                    daySession.Census.SafetyAlertCount > alertsBefore)
                {
                    Console.WriteLine("[PASS] Check 11: Daily advancement cooled idle reactor and flagged alert for excessive acidic waste accumulation.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 11: Daily advancement cooling or hazard alert failed.");
                }

                // Check 12: Host session capture/restore round-trip fidelity
                var sourceSession = ChemicalReagentSynthesisHostSession.Create();
                sourceSession.RestockNeutralizingAlkali(500);
                sourceSession.ExecuteSynthesisCycle("primary_synthesis_vessel", 40, 1000, 900, 201);

                var captured = sourceSession.CaptureState();
                var targetSession = ChemicalReagentSynthesisHostSession.Create();
                targetSession.RestoreState(captured);

                var cSource = sourceSession.Census;
                var cTarget = targetSession.Census;

                if (cSource.ActiveReactorsCount == cTarget.ActiveReactorsCount &&
                    cSource.AccumulatedAcidicWasteLitres == cTarget.AccumulatedAcidicWasteLitres &&
                    cSource.NeutralizingAlkaliStockKg == cTarget.NeutralizingAlkaliStockKg &&
                    cSource.TotalReagentStockKg == cTarget.TotalReagentStockKg &&
                    cSource.SafetyAlertCount == cTarget.SafetyAlertCount)
                {
                    Console.WriteLine("[PASS] Check 12: Host session state preserved across capture/restore round-trip.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 12: Host session capture/restore round-trip failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Expansion 39 self-test threw exception: {ex}");
                return 1;
            }

            Console.WriteLine($"=== Expansion 39 Self-Test Complete: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
