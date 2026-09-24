// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI: Expansion 38 — The Ward Self-Test
// Validation of ClinicalWardTriageEngine calculations, ledger persistence,
// surgical suite preparation, sterile supply utilization, and nosocomial risks.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public static class HostCliClinicalWardTriage
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Clinical Ward Triage & Sterile Supply Self-Test (Expansion 38) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Triage categorization accurately maps across all 4 tiers
                var ward = new ClinicalWardState { TotalBeds = 10, IsolationBedsTotal = 2 };
                var tMinimal = ClinicalWardTriageEngine.EvaluatePatientTriage(150, 850, false, ward);
                var tDelayed = ClinicalWardTriageEngine.EvaluatePatientTriage(450, 750, false, ward);
                var tImmediate = ClinicalWardTriageEngine.EvaluatePatientTriage(700, 350, false, ward);
                var tExpectant = ClinicalWardTriageEngine.EvaluatePatientTriage(900, 100, false, ward);

                if (tMinimal.AssignedPriority == TriagePriorityTier.Minimal &&
                    tDelayed.AssignedPriority == TriagePriorityTier.Delayed &&
                    tImmediate.AssignedPriority == TriagePriorityTier.Immediate &&
                    tExpectant.AssignedPriority == TriagePriorityTier.Expectant)
                {
                    GD.Print("[PASS] Check 1: Triage priorities map accurately across all 4 tiers (Minimal, Delayed, Immediate, Expectant).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 1: Unexpected triage priority assignments ({tMinimal.AssignedPriority}, {tDelayed.AssignedPriority}, {tImmediate.AssignedPriority}, {tExpectant.AssignedPriority}).");
                }

                // Check 2: Contagious patients are routed to negative-pressure isolation beds
                var tIso = ClinicalWardTriageEngine.EvaluatePatientTriage(600, 500, isContagious: true, ward);
                if (tIso.RequiresIsolation && tIso.BedAvailable && tIso.RecommendationNotice.Contains("isolation"))
                {
                    GD.Print("[PASS] Check 2: Contagious patients are routed to negative-pressure isolation beds.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 2: Contagious patient routing failed.");
                }

                // Check 3: Bed capacity overflow detection triggers when beds are saturated
                var fullWard = new ClinicalWardState
                {
                    TotalBeds = 2,
                    OccupiedBeds = 2,
                    IsolationBedsTotal = 1,
                    IsolationBedsOccupied = 1
                };
                var tOverGen = ClinicalWardTriageEngine.EvaluatePatientTriage(300, 800, false, fullWard);
                var tOverIso = ClinicalWardTriageEngine.EvaluatePatientTriage(300, 800, true, fullWard);

                if (!tOverGen.BedAvailable && !tOverIso.BedAvailable &&
                    tOverGen.RecommendationNotice.Contains("Ward capacity reached") &&
                    tOverIso.RecommendationNotice.Contains("Isolation ward saturated"))
                {
                    GD.Print("[PASS] Check 3: Bed capacity overflow detection correctly triggers when beds are saturated.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 3: Bed overflow detection failed.");
                }

                // Check 4: Surgical preflight is denied when theater lacks antiseptic standard
                var dirtyWard = new ClinicalWardState
                {
                    Cleanliness = WardCleanlinessGrade.Contaminated,
                    SterileSupplyStockPermille = 800,
                    StaffingReadinessPermille = 800
                };
                var prepDirty = ClinicalWardTriageEngine.EvaluateSurgicalPreparation(dirtyWard, 500, 600, 100);
                if (!prepDirty.IsApprovedForSurgery && prepDirty.BottleneckReason.Contains("antiseptic standard"))
                {
                    GD.Print("[PASS] Check 4: Surgical preflight is denied when operating theater is contaminated.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 4: Surgery was not denied for contaminated theater.");
                }

                // Check 5: Surgical preflight is denied when sterile supplies are below threshold (< 300‰)
                var lowSupplyWard = new ClinicalWardState
                {
                    Cleanliness = WardCleanlinessGrade.SterileField,
                    SterileSupplyStockPermille = 250,
                    StaffingReadinessPermille = 800
                };
                var prepLowSupply = ClinicalWardTriageEngine.EvaluateSurgicalPreparation(lowSupplyWard, 500, 600, 100);
                if (!prepLowSupply.IsApprovedForSurgery && prepLowSupply.BottleneckReason.Contains("safety threshold"))
                {
                    GD.Print("[PASS] Check 5: Surgical preflight is denied when sterile supplies are below safety threshold (<300\u2030).");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 5: Surgery was not denied for sterile supply shortage.");
                }

                // Check 6: Surgical preflight is denied when staffing readiness is below threshold (< 500‰)
                var lowStaffWard = new ClinicalWardState
                {
                    Cleanliness = WardCleanlinessGrade.SterileField,
                    SterileSupplyStockPermille = 800,
                    StaffingReadinessPermille = 400
                };
                var prepLowStaff = ClinicalWardTriageEngine.EvaluateSurgicalPreparation(lowStaffWard, 500, 600, 100);
                if (!prepLowStaff.IsApprovedForSurgery && prepLowStaff.BottleneckReason.Contains("staffing insufficient"))
                {
                    GD.Print("[PASS] Check 6: Surgical preflight is denied when surgical staffing is insufficient (<500\u2030).");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 6: Surgery was not denied for staffing shortage.");
                }

                // Check 7: Surgical preflight succeeds under sterile field conditions with adequate supplies and staff
                var readyWard = new ClinicalWardState
                {
                    Cleanliness = WardCleanlinessGrade.SterileField,
                    SterileSupplyStockPermille = 750,
                    StaffingReadinessPermille = 900
                };
                var prepReady = ClinicalWardTriageEngine.EvaluateSurgicalPreparation(readyWard, 600, 500, 777);
                if (prepReady.IsApprovedForSurgery && prepReady.SterileSuppliesConsumedPermille > 0 &&
                    readyWard.SterileSupplyStockPermille < 750)
                {
                    GD.Print($"[PASS] Check 7: Surgical preflight succeeds and consumes sterile supplies ({prepReady.SterileSuppliesConsumedPermille}\u2030 consumed).");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 7: Surgical preflight failed under optimal conditions.");
                }

                // Check 8: Surgical infection risk derived from cleanliness grades
                var sterilePrep = ClinicalWardTriageEngine.EvaluateSurgicalPreparation(
                    new ClinicalWardState { Cleanliness = WardCleanlinessGrade.SterileField, SterileSupplyStockPermille = 800, StaffingReadinessPermille = 800 },
                    400, 600, 12);
                var antisepticPrep = ClinicalWardTriageEngine.EvaluateSurgicalPreparation(
                    new ClinicalWardState { Cleanliness = WardCleanlinessGrade.AntisepticStandard, SterileSupplyStockPermille = 800, StaffingReadinessPermille = 800 },
                    400, 600, 12);

                if (sterilePrep.InfectionRiskPermille < antisepticPrep.InfectionRiskPermille)
                {
                    GD.Print($"[PASS] Check 8: Surgical infection risk scales with cleanliness ({sterilePrep.InfectionRiskPermille}\u2030 in SterileField vs {antisepticPrep.InfectionRiskPermille}\u2030 in AntisepticStandard).");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 8: Infection risk did not scale with cleanliness.");
                }

                // Check 9: Monthly bed turnover capacity accurately computes throughput
                var turnWard = new ClinicalWardState { TotalBeds = 12 };
                int turns1 = ClinicalWardTriageEngine.ComputeBedTurnoverCapacity(turnWard, 5);  // 30/5 = 6 turns * 12 = 72
                int turns2 = ClinicalWardTriageEngine.ComputeBedTurnoverCapacity(turnWard, 15); // 30/15 = 2 turns * 12 = 24

                if (turns1 == 72 && turns2 == 24)
                {
                    GD.Print($"[PASS] Check 9: Monthly bed turnover capacity accurately computed throughput ({turns1} patients at 5d stay, {turns2} at 15d stay).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 9: Bed turnover capacity calculation mismatch ({turns1}, {turns2}).");
                }

                // Check 10: Nosocomial infection risk rises with crowding and contamination
                int riskCleanLow = ClinicalWardTriageEngine.CalculateNosocomialInfectionRisk(WardCleanlinessGrade.SterileField, 200, 800);
                int riskCrowdedLowSupply = ClinicalWardTriageEngine.CalculateNosocomialInfectionRisk(WardCleanlinessGrade.BasicSanitation, 950, 200);

                if (riskCrowdedLowSupply > riskCleanLow)
                {
                    GD.Print($"[PASS] Check 10: Nosocomial infection risk rises with crowding and contamination ({riskCleanLow}\u2030 -> {riskCrowdedLowSupply}\u2030).");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 10: Nosocomial risk calculation did not penalize crowding or contamination.");
                }

                // Check 11: Ledger daily advance increments patient tenure and sterile supply drain
                var ledger = new ClinicalWardLedger();
                ledger.State.Ward.TotalBeds = 8;
                ledger.State.Ward.SterileSupplyStockPermille = 600;
                ledger.TriageAndAdmitPatient("probe-patient-1", 400, 700, false, 1);
                ledger.AdvanceDay(2, 456);

                var census = ledger.GetCensus();
                if (census.ActiveAdmissionsCount == 1 &&
                    census.OccupiedBeds == 1 &&
                    census.AvailableBeds == 7 &&
                    census.SterileSupplyStockPermille == 595)
                {
                    GD.Print("[PASS] Check 11: Daily clinical advance updated patient tenure and supply drain accurately.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 11: Daily advance state mismatch (occupied={census.OccupiedBeds}, supply={census.SterileSupplyStockPermille}).");
                }

                // Check 12: Host session state and surgical history preserved across capture/restore
                var hostSession = ClinicalWardTriageHostSession.Create();
                hostSession.SetWardCleanliness(WardCleanlinessGrade.SterileField);
                hostSession.RestockSterileSupplies(200);
                hostSession.TriageAndAdmitPatient("patient_probe_2", 800, 250, false, 3);
                hostSession.PreflightAndExecuteSurgery("proc_exploratory", "patient_probe_2", 400, 500, 3, 101);

                var stateSnapshot = hostSession.CaptureState();
                var restoredSession = ClinicalWardTriageHostSession.Create(stateSnapshot);
                var restoredCensus = restoredSession.Census;

                if (restoredCensus.Cleanliness == WardCleanlinessGrade.SterileField &&
                    restoredCensus.ActiveAdmissionsCount == 1 &&
                    restoredCensus.SurgeriesPerformedCount == 1)
                {
                    GD.Print("[PASS] Check 12: Host session state preserved across capture/restore round-trip.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 12: State restoration mismatch.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] Unexpected exception during Expansion 38 self-test: {ex.Message}\n{ex.StackTrace}");
            }

            GD.Print($"=== Expansion 38 Self-Test Complete: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
