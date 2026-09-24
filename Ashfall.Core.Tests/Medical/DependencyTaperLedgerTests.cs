// SPDX-License-Identifier: MIT
// Expansion 35 — The Habit : DependencyTaperLedger focused tests
using System.Collections.Generic;
using Xunit;
using Ashfall.Core.Medical;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class DependencyTaperLedgerTests
    {
        [Fact]
        public void EnrollProgram_RegistersActiveProgram_WithCalculatedStepDown()
        {
            var ledger = new DependencyTaperLedger();
            var program = ledger.EnrollProgram(
                survivorId: "surv_001",
                dependencyPermille: 400,
                isMedicallySupervised: true);

            Assert.NotNull(program);
            Assert.Equal("surv_001", program.SurvivorId);
            Assert.Equal(400, program.DependencyPermille);
            Assert.True(ledger.TryGetProgram("surv_001", out var fetched));
            Assert.Same(program, fetched);
            Assert.True(program.DailyStepDownPermille > 0);
        }

        [Fact]
        public void AdvanceProgramDay_StepsDownDose_AndCompletesWhenThresholdReached()
        {
            var ledger = new DependencyTaperLedger();
            ledger.EnrollProgram("surv_002", dependencyPermille: 100, isMedicallySupervised: false, dailyStepDown: 400);

            var res1 = ledger.AdvanceProgramDay("surv_002", peerSupportRunToday: false);
            Assert.Equal(600, res1.NewSubstituteDosePermille);
            Assert.False(res1.TaperComplete);

            var res2 = ledger.AdvanceProgramDay("surv_002", peerSupportRunToday: false);
            Assert.Equal(200, res2.NewSubstituteDosePermille);
            Assert.False(res2.TaperComplete);

            var res3 = ledger.AdvanceProgramDay("surv_002", peerSupportRunToday: false);
            Assert.Equal(0, res3.NewSubstituteDosePermille);
            Assert.True(res3.TaperComplete);

            // Program should now be removed from ActivePrograms and added to CompletedSurvivorIds
            Assert.False(ledger.TryGetProgram("surv_002", out _));
            Assert.Contains("surv_002", ledger.CompletedSurvivorIds);
        }

        [Fact]
        public void AdvanceAll_StepsDownAllActivePrograms_AndReturnsResults()
        {
            var ledger = new DependencyTaperLedger();
            ledger.EnrollProgram("surv_A", 200, false, 100);
            ledger.EnrollProgram("surv_B", 500, true, 80);

            var results = ledger.AdvanceAll(peerSupportRunToday: true);
            Assert.Equal(2, results.Count);

            Assert.True(ledger.TryGetProgram("surv_A", out var progA));
            Assert.True(ledger.TryGetProgram("surv_B", out var progB));
            Assert.Equal(900, progA!.CurrentSubstituteDosePermille);
            Assert.Equal(920, progB!.CurrentSubstituteDosePermille);
            Assert.Equal(1, progA.PeerSupportSessionsCompleted);
            Assert.Equal(1, progB.PeerSupportSessionsCompleted);
        }

        [Fact]
        public void EvaluateCarePolicy_EscalatesToEmergency_WhenCriticalBurdenHigh()
        {
            var ledger = new DependencyTaperLedger();
            ledger.SetPolicyPosture(CarePolicyPosture.Permissive);

            // 2 critical cases out of population of 10 = 20% >= 10% -> Emergency
            ledger.EnrollProgram("crit_1", dependencyPermille: 900, isMedicallySupervised: true);
            ledger.EnrollProgram("crit_2", dependencyPermille: 850, isMedicallySupervised: false);

            var policy = ledger.EvaluateCarePolicy(totalShelterPopulation: 10);
            Assert.Equal(CarePolicyPosture.Emergency, policy);
        }

        [Fact]
        public void StatePersistence_RoundTrip_PreservesAllFieldsAndPrograms()
        {
            var ledger = new DependencyTaperLedger();
            ledger.SetPolicyPosture(CarePolicyPosture.Controlled);
            ledger.SetSubstituteStock(750);
            ledger.EnrollProgram("surv_save", dependencyPermille: 650, isMedicallySupervised: true, dailyStepDown: 75);
            ledger.AdvanceProgramDay("surv_save", peerSupportRunToday: true);

            var captured = ledger.CaptureState();
            Assert.Equal(1, captured.SchemaVersion);
            Assert.Equal(CarePolicyPosture.Controlled, captured.CurrentPosture);
            Assert.Equal(750, captured.SubstituteMedicineStockPermille);
            Assert.Single(captured.ActivePrograms);

            var restoredLedger = new DependencyTaperLedger();
            restoredLedger.RestoreState(captured);

            Assert.Equal(CarePolicyPosture.Controlled, restoredLedger.CurrentPosture);
            Assert.Equal(750, restoredLedger.SubstituteMedicineStockPermille);
            Assert.True(restoredLedger.TryGetProgram("surv_save", out var restoredProg));
            Assert.Equal(925, restoredProg!.CurrentSubstituteDosePermille);
            Assert.Equal(1, restoredProg.PeerSupportSessionsCompleted);
            Assert.True(restoredProg.IsMedicallySupervised);
        }

        [Fact]
        public void Census_AccuratelyReflectsActiveSupervisedAndCriticalCases()
        {
            var ledger = new DependencyTaperLedger();
            ledger.EnrollProgram("p1", dependencyPermille: 300, isMedicallySupervised: true);
            ledger.EnrollProgram("p2", dependencyPermille: 850, isMedicallySupervised: false); // Critical
            ledger.EnrollProgram("p3", dependencyPermille: 950, isMedicallySupervised: true);  // Critical

            var census = ledger.GetCensus();
            Assert.Equal(3, census.ActiveProgramsCount);
            Assert.Equal(0, census.CompletedProgramsCount);
            Assert.Equal(2, census.MedicallySupervisedCount);
            Assert.Equal(2, census.CriticalBurdenCount);
            Assert.Equal(CarePolicyPosture.Monitored, census.CurrentPosture);
        }
    }
}
