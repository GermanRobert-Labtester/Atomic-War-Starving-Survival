// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Expansion 35 (The Habit — Chemical Dependency Taper & Withdrawal).

using System;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public static class HostCliDependencyTaperWithdrawal
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Chemical Dependency Taper & Withdrawal Self-Test (Expansion 35) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Critical dependency recommends slower safe step-down than Mild
                int criticalStep = DependencyTaperWithdrawalEngine.ComputeRecommendedStepDown(900, false, 1000);
                int mildStep = DependencyTaperWithdrawalEngine.ComputeRecommendedStepDown(150, false, 1000);
                if (criticalStep < mildStep && criticalStep > 0)
                {
                    GD.Print($"[PASS] Check 1: Critical step ({criticalStep}\u2030) is slower than Mild ({mildStep}\u2030).");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 1: Step-down rate incorrect (critical={criticalStep}, mild={mildStep}).");

                // Check 2: Medicine shortage (<300 permille) forces accelerated step-down
                int normalStockStep = DependencyTaperWithdrawalEngine.ComputeRecommendedStepDown(500, false, 1000);
                int lowStockStep = DependencyTaperWithdrawalEngine.ComputeRecommendedStepDown(500, false, 100);
                if (lowStockStep > normalStockStep)
                {
                    GD.Print($"[PASS] Check 2: Low stock forces faster taper ({lowStockStep}\u2030 > {normalStockStep}\u2030).");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 2: Stock scarcity did not accelerate step-down.");

                // Check 3: Medical supervision allows faster safe taper
                int unsuperStep = DependencyTaperWithdrawalEngine.ComputeRecommendedStepDown(500, false, 1000);
                int superStep = DependencyTaperWithdrawalEngine.ComputeRecommendedStepDown(500, true, 1000);
                if (superStep > unsuperStep)
                {
                    GD.Print($"[PASS] Check 3: Supervision allows faster step-down ({superStep}\u2030 > {unsuperStep}\u2030).");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 3: Supervision did not increase step-down rate.");

                // Check 4: ClassifyDependencySeverity tiers
                if (DependencyTaperWithdrawalEngine.ClassifyDependencySeverity(0) == DependencySeverityTier.None &&
                    DependencyTaperWithdrawalEngine.ClassifyDependencySeverity(200) == DependencySeverityTier.Mild &&
                    DependencyTaperWithdrawalEngine.ClassifyDependencySeverity(400) == DependencySeverityTier.Moderate &&
                    DependencyTaperWithdrawalEngine.ClassifyDependencySeverity(700) == DependencySeverityTier.Severe &&
                    DependencyTaperWithdrawalEngine.ClassifyDependencySeverity(850) == DependencySeverityTier.Critical)
                {
                    GD.Print("[PASS] Check 4: Dependency severity tiers classified correctly.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 4: Dependency severity classification error.");

                // Check 5: Emergency care policy recommendation on >= 10% critical burden
                var emPolicy = DependencyTaperWithdrawalEngine.RecommendCarePolicy(
                    criticalCaseCount: 15,
                    totalShelterPopulation: 100,
                    currentPosture: CarePolicyPosture.Permissive);
                if (emPolicy == CarePolicyPosture.Emergency)
                {
                    GD.Print("[PASS] Check 5: Burden >= 10% triggers Emergency care posture.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 5: Expected Emergency, got {emPolicy}.");

                // Check 6: Controlled care policy on 5% - 9% critical burden
                var ctrlPolicy = DependencyTaperWithdrawalEngine.RecommendCarePolicy(
                    criticalCaseCount: 6,
                    totalShelterPopulation: 100,
                    currentPosture: CarePolicyPosture.Permissive);
                if (ctrlPolicy == CarePolicyPosture.Controlled)
                {
                    GD.Print("[PASS] Check 6: Burden >= 5% triggers Controlled care posture.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 6: Expected Controlled, got {ctrlPolicy}.");

                // Check 7: AdvanceTaperDay steps down dose and marks complete
                var prog = new TaperProgramState
                {
                    SurvivorId = "probe_s1",
                    DependencyPermille = 200,
                    DailyStepDownPermille = 500,
                    CurrentSubstituteDosePermille = 400,
                    IsMedicallySupervised = true
                };
                var stepResult = DependencyTaperWithdrawalEngine.AdvanceTaperDay(prog, false);
                if (stepResult.NewSubstituteDosePermille == 0 && stepResult.TaperComplete)
                {
                    GD.Print("[PASS] Check 7: Dose stepped down to 0 and program completed.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 7: Step-down complete check failed (dose={stepResult.NewSubstituteDosePermille}, complete={stepResult.TaperComplete}).");

                // Check 8: Peer support reduces symptom severity
                var pNoPeer = new TaperProgramState
                {
                    SurvivorId = "probe_s2",
                    DependencyPermille = 800,
                    DailyStepDownPermille = 300,
                    CurrentSubstituteDosePermille = 500,
                    PeerSupportSessionsCompleted = 0
                };
                var pPeer = new TaperProgramState
                {
                    SurvivorId = "probe_s3",
                    DependencyPermille = 800,
                    DailyStepDownPermille = 300,
                    CurrentSubstituteDosePermille = 500,
                    PeerSupportSessionsCompleted = 4
                };
                var resNoPeer = DependencyTaperWithdrawalEngine.AdvanceTaperDay(pNoPeer, false);
                var resPeer = DependencyTaperWithdrawalEngine.AdvanceTaperDay(pPeer, true);
                if ((int)resPeer.Symptoms <= (int)resNoPeer.Symptoms)
                {
                    GD.Print($"[PASS] Check 8: Peer support mitigated symptoms ({resPeer.Symptoms} vs {resNoPeer.Symptoms}).");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 8: Peer support did not mitigate symptoms.");

                // Check 9: Unsupervised acute symptoms flag medical escalation
                var pEsc = new TaperProgramState
                {
                    SurvivorId = "probe_s4",
                    DependencyPermille = 200,
                    DailyStepDownPermille = 1000,
                    CurrentSubstituteDosePermille = 1000,
                    IsMedicallySupervised = false
                };
                var resEsc = DependencyTaperWithdrawalEngine.AdvanceTaperDay(pEsc, false);
                if (resEsc.RequiresMedicalEscalation && resEsc.Symptoms >= WithdrawalSymptomBand.Acute)
                {
                    GD.Print("[PASS] Check 9: Unsupervised acute symptoms triggered medical escalation.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 9: Escalation flag missing (symptoms={resEsc.Symptoms}, escalation={resEsc.RequiresMedicalEscalation}).");

                // Check 10: Ledger manages enrollment and active program graduation
                var ledger = new DependencyTaperLedger();
                ledger.EnrollProgram("grad_1", 100, true, 500);
                TaperDayResult advRes = default;
                while (!advRes.TaperComplete)
                {
                    advRes = ledger.AdvanceProgramDay("grad_1", false);
                }
                if (advRes.TaperComplete && !ledger.TryGetProgram("grad_1", out _) && ledger.CompletedSurvivorIds.Contains("grad_1"))
                {
                    GD.Print("[PASS] Check 10: Ledger completed and graduated program successfully.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 10: Ledger graduation flow failed.");

                // Check 11: Ledger state capture and restore round-trip
                ledger.Clear();
                ledger.SetPolicyPosture(CarePolicyPosture.Emergency);
                ledger.SetSubstituteStock(450);
                ledger.EnrollProgram("roundtrip_s1", 600, true, 80);
                var captured = ledger.CaptureState();
                var restoredLedger = new DependencyTaperLedger();
                restoredLedger.RestoreState(captured);
                if (restoredLedger.CurrentPosture == CarePolicyPosture.Emergency &&
                    restoredLedger.SubstituteMedicineStockPermille == 450 &&
                    restoredLedger.TryGetProgram("roundtrip_s1", out var rProg) &&
                    rProg!.DependencyPermille == 600)
                {
                    GD.Print("[PASS] Check 11: State round-trip preserved posture, stock, and active programs.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 11: State round-trip failed.");

                // Check 12: Host session creation and census calculation
                var session = DependencyTaperWithdrawalHostSession.Create(captured);
                var census = session.Census;
                if (census.ActiveProgramsCount == 1 &&
                    census.CurrentPosture == CarePolicyPosture.Emergency &&
                    census.SubstituteMedicineStockPermille == 450)
                {
                    GD.Print("[PASS] Check 12: Host session created with matching census.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 12: Host session census mismatch (count={census.ActiveProgramsCount}, posture={census.CurrentPosture}).");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] Self-test exception: {ex.Message}\n{ex.StackTrace}");
            }

            GD.Print($"=== Expansion 35 Self-Test Complete: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
