// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI: Expansion 37 — The Quickening Self-Test
// Validation of AntenatalMaternalHealthEngine calculations, ledger persistence,
// trimester progression, and neonatal delivery resolution.
// ============================================================================

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class HostCliAntenatalMaternalHealth
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Antenatal & Maternal Health Self-Test (Expansion 37) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Trimester transitions map accurately across all boundaries
                var t1 = AntenatalMaternalHealthEngine.ResolveTrimester(45);
                var t2 = AntenatalMaternalHealthEngine.ResolveTrimester(140);
                var t3 = AntenatalMaternalHealthEngine.ResolveTrimester(220);
                var tf = AntenatalMaternalHealthEngine.ResolveTrimester(270);
                var tp = AntenatalMaternalHealthEngine.ResolveTrimester(10, isPostpartum: true);

                if (t1 == GestationTrimester.FirstTrimester &&
                    t2 == GestationTrimester.SecondTrimester &&
                    t3 == GestationTrimester.ThirdTrimester &&
                    tf == GestationTrimester.FullTerm &&
                    tp == GestationTrimester.Postpartum)
                {
                    GD.Print("[PASS] Check 1: Trimester transitions map accurately across all boundaries.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 1: Unexpected trimester mapping ({t1}, {t2}, {t3}, {tf}, {tp}).");
                }

                // Check 2: Caloric demand multiplier escalates across trimesters
                var s1 = new MaternalPregnancyState { GestationDays = 50 };
                var s2 = new MaternalPregnancyState { GestationDays = 150 };
                var s3 = new MaternalPregnancyState { GestationDays = 230 };
                var sf = new MaternalPregnancyState { GestationDays = 270 };

                var r1 = AntenatalMaternalHealthEngine.AdvancePregnancyDay(s1, 1000, 8);
                var r2 = AntenatalMaternalHealthEngine.AdvancePregnancyDay(s2, 1000, 8);
                var r3 = AntenatalMaternalHealthEngine.AdvancePregnancyDay(s3, 1000, 8);
                var rf = AntenatalMaternalHealthEngine.AdvancePregnancyDay(sf, 1000, 8);

                if (r1.DailyCaloricDemandMultiplierPermille < r2.DailyCaloricDemandMultiplierPermille &&
                    r2.DailyCaloricDemandMultiplierPermille < r3.DailyCaloricDemandMultiplierPermille &&
                    r3.DailyCaloricDemandMultiplierPermille <= rf.DailyCaloricDemandMultiplierPermille)
                {
                    GD.Print($"[PASS] Check 2: Caloric demand escalates across trimesters ({r1.DailyCaloricDemandMultiplierPermille}\u2030 -> {rf.DailyCaloricDemandMultiplierPermille}\u2030).");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 2: Caloric demand did not scale monotonically.");
                }

                // Check 3: Nutritional deficit depletes maternal reserves
                var deficitState = new MaternalPregnancyState
                {
                    MotherSurvivorId = "probe-mother-1",
                    GestationDays = 100,
                    MaternalNutritionReservePermille = 800
                };
                AntenatalMaternalHealthEngine.AdvancePregnancyDay(deficitState, nutritionIntakePermille: 400, restHoursProvided: 8);

                if (deficitState.MaternalNutritionReservePermille < 800)
                {
                    GD.Print($"[PASS] Check 3: Nutritional deficit depleted reserve (800\u2030 -> {deficitState.MaternalNutritionReservePermille}\u2030).");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 3: Reserve did not deplete under caloric deficit.");
                }

                // Check 4: Rest hours deficit elevates maternal fatigue
                var fatigueState = new MaternalPregnancyState
                {
                    MotherSurvivorId = "probe-mother-2",
                    GestationDays = 80,
                    MaternalFatiguePermille = 100
                };
                AntenatalMaternalHealthEngine.AdvancePregnancyDay(fatigueState, nutritionIntakePermille: 1000, restHoursProvided: 4);

                if (fatigueState.MaternalFatiguePermille > 100)
                {
                    GD.Print($"[PASS] Check 4: Sleep deficit elevated fatigue (100\u2030 -> {fatigueState.MaternalFatiguePermille}\u2030).");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 4: Sleep deficit did not elevate fatigue.");
                }

                // Check 5: Clinic supervision and sanitation mitigations reduce net complication risk
                var poorClinicState = new MaternalPregnancyState
                {
                    GestationDays = 200,
                    MaternalNutritionReservePermille = 600,
                    MaternalFatiguePermille = 400,
                    MedicalSupervisionQualityPermille = 100,
                    ShelterSanitationQualityPermille = 100
                };
                var goodClinicState = new MaternalPregnancyState
                {
                    GestationDays = 200,
                    MaternalNutritionReservePermille = 600,
                    MaternalFatiguePermille = 400,
                    MedicalSupervisionQualityPermille = 900,
                    ShelterSanitationQualityPermille = 900
                };

                var resPoor = AntenatalMaternalHealthEngine.AdvancePregnancyDay(poorClinicState, 1000, 8);
                var resGood = AntenatalMaternalHealthEngine.AdvancePregnancyDay(goodClinicState, 1000, 8);

                if (resGood.ComplicationRiskPermille < resPoor.ComplicationRiskPermille)
                {
                    GD.Print($"[PASS] Check 5: Clinical supervision reduced risk ({resPoor.ComplicationRiskPermille}\u2030 -> {resGood.ComplicationRiskPermille}\u2030).");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 5: Clinic quality did not reduce complication risk.");
                }

                // Check 6: Health status classification accurately detects Critical status
                var criticalState = new MaternalPregnancyState
                {
                    GestationDays = 210,
                    MaternalNutritionReservePermille = 150,
                    MaternalFatiguePermille = 850,
                    MedicalSupervisionQualityPermille = 200,
                    ShelterSanitationQualityPermille = 200
                };
                var critRes = AntenatalMaternalHealthEngine.AdvancePregnancyDay(criticalState, 500, 4);

                if (critRes.HealthStatus == MaternalHealthStatus.Critical)
                {
                    GD.Print("[PASS] Check 6: Depleted vitals accurately classified as Critical.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 6: Expected Critical health status, got {critRes.HealthStatus}.");
                }

                // Check 7: Labor readiness activates at FullTerm threshold (>= 260 days)
                var earlyState = new MaternalPregnancyState { GestationDays = 255 };
                var readyState = new MaternalPregnancyState { GestationDays = 265 };

                var earlyProg = AntenatalMaternalHealthEngine.AdvancePregnancyDay(earlyState, 1000, 8);
                var readyProg = AntenatalMaternalHealthEngine.AdvancePregnancyDay(readyState, 1000, 8);

                if (!earlyProg.IsLaborReady && readyProg.IsLaborReady)
                {
                    GD.Print("[PASS] Check 7: Labor readiness flag correctly activated at threshold.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 7: Labor readiness threshold mismatch.");
                }

                // Check 8: Deterministic birth delivery resolves Healthy outcome under optimal care
                var optimalDeliveryState = new MaternalPregnancyState
                {
                    MotherSurvivorId = "mother_optimal",
                    GestationDays = 280,
                    MaternalNutritionReservePermille = 950,
                    MaternalFatiguePermille = 50,
                    MedicalSupervisionQualityPermille = 950,
                    ShelterSanitationQualityPermille = 950
                };
                var optimalBirth = AntenatalMaternalHealthEngine.ResolveBirthDelivery(optimalDeliveryState, birthSeed: 777);

                if (optimalBirth.Outcome == BirthOutcomeClassification.Healthy && optimalBirth.NeonatalVigorPermille >= 700)
                {
                    GD.Print($"[PASS] Check 8: Optimal conditions yielded Healthy birth (vigor {optimalBirth.NeonatalVigorPermille}\u2030).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 8: Expected Healthy birth, got {optimalBirth.Outcome}.");
                }

                // Check 9: Depleted condition yields Complicated delivery outcome
                var severeDeliveryState = new MaternalPregnancyState
                {
                    MotherSurvivorId = "mother_depleted",
                    GestationDays = 265,
                    MaternalNutritionReservePermille = 100,
                    MaternalFatiguePermille = 950,
                    MedicalSupervisionQualityPermille = 50,
                    ShelterSanitationQualityPermille = 50
                };
                var severeBirth = AntenatalMaternalHealthEngine.ResolveBirthDelivery(severeDeliveryState, birthSeed: 888);

                if (severeBirth.Outcome == BirthOutcomeClassification.Complicated)
                {
                    GD.Print($"[PASS] Check 9: Depleted conditions yielded Complicated birth (recovery {severeBirth.PostpartumRecoveryDaysNeeded} days).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 9: Expected Complicated birth, got {severeBirth.Outcome}.");
                }

                // Check 10: Successful delivery transitions state to Postpartum and ledger records it
                var ledger = new AntenatalMaternalCareLedger();
                ledger.SetClinicQuality(800);
                ledger.SetSanitationQuality(800);
                ledger.RegisterPregnancy("survivor_jane", gestationDays: 275, nutritionReservePermille: 900);

                var deliveryResult = ledger.ResolveDelivery("survivor_jane", birthSeed: 999, childId: "child_jane_jr");

                if (ledger.DeliveryHistory.Count == 1 &&
                    ledger.DeliveryHistory[0].ChildId == "child_jane_jr" &&
                    ledger.GetPregnancy("survivor_jane")?.IsPostpartum == true)
                {
                    GD.Print("[PASS] Check 10: Delivery recorded and mother transitioned to postpartum.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 10: Delivery record or postpartum transition failed.");
                }

                // Check 11: Postpartum recovery rate scales with clinical care quality
                int poorRecovery = AntenatalMaternalHealthEngine.ComputePostpartumRecoveryRate(5, 100, 200);
                int goodRecovery = AntenatalMaternalHealthEngine.ComputePostpartumRecoveryRate(5, 900, 900);

                if (goodRecovery > poorRecovery)
                {
                    GD.Print($"[PASS] Check 11: Postpartum recovery scales with clinic care ({poorRecovery}\u2030/day -> {goodRecovery}\u2030/day).");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 11: Postpartum recovery rate did not scale.");
                }

                // Check 12: Host session lifecycle and state round-trip persistence
                var captured = ledger.CaptureState();
                var restoredSession = AntenatalMaternalHealthHostSession.Create(captured);
                var restoredCensus = restoredSession.Census;

                if (restoredCensus.TotalDeliveriesCount == 1 &&
                    restoredSession.ShelterClinicQualityPermille == 800 &&
                    restoredSession.Ledger.DeliveryHistory[0].MotherSurvivorId == "survivor_jane")
                {
                    GD.Print("[PASS] Check 12: Host session state preserved across capture/restore round-trip.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 12: Host session failed to restore census or history accurately.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] Unexpected exception during Antenatal & Maternal Health self-test: {ex.Message}");
            }

            GD.Print($"=== Expansion 37 Self-Test Complete: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
