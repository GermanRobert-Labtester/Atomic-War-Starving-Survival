// SPDX-License-Identifier: MIT
// Expansion 35 — The Habit : DependencyTaperWithdrawalEngine focused tests
using Xunit;
using Ashfall.Core.Medical;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class DependencyTaperWithdrawalEngineTests
    {
        // ── 1. Critical dependency computes a very slow step-down (safe taper) ──
        [Fact]
        public void ComputeRecommendedStepDown_Slow_ForCriticalDependency()
        {
            int criticalStep = DependencyTaperWithdrawalEngine.ComputeRecommendedStepDown(
                dependencyPermille: 900,        // Critical tier
                isMedicallySupervised: false,
                substituteMedicineAvailablePermille: 1000);

            int mildStep = DependencyTaperWithdrawalEngine.ComputeRecommendedStepDown(
                dependencyPermille: 100,        // Mild tier
                isMedicallySupervised: false,
                substituteMedicineAvailablePermille: 1000);

            Assert.True(criticalStep < mildStep,
                "Critical dependency should recommend a slower daily step-down than Mild");
            Assert.True(criticalStep > 0, "Step-down should never be zero");
        }

        // ── 2. Medicine shortage forces a faster step-down (danger signal) ──
        [Fact]
        public void ComputeRecommendedStepDown_Faster_WhenMedicineStockLow()
        {
            int fullStockStep = DependencyTaperWithdrawalEngine.ComputeRecommendedStepDown(
                dependencyPermille: 500,
                isMedicallySupervised: false,
                substituteMedicineAvailablePermille: 1000);

            int lowStockStep = DependencyTaperWithdrawalEngine.ComputeRecommendedStepDown(
                dependencyPermille: 500,
                isMedicallySupervised: false,
                substituteMedicineAvailablePermille: 50);  // very low stock

            Assert.True(lowStockStep > fullStockStep,
                "Low medicine stock should force a faster (more dangerous) step-down");
        }

        // ── 3. Taper completes after sufficient days ──
        [Fact]
        public void AdvanceTaperDay_CompletesProgram_AfterSufficientStepDowns()
        {
            var program = new TaperProgramState
            {
                SurvivorId                   = "survivor-001",
                DependencyPermille           = 200,
                DailyStepDownPermille        = 120,
                CurrentSubstituteDosePermille = 1000,
                IsMedicallySupervised        = true
            };

            TaperDayResult result = new TaperDayResult(1000, WithdrawalSymptomBand.Asymptomatic, false, false, 0);
            for (int i = 0; i < 15 && !result.TaperComplete; i++)
                result = DependencyTaperWithdrawalEngine.AdvanceTaperDay(program, peerSupportRunToday: false);

            Assert.True(result.TaperComplete,
                "Program should complete within 15 days at 120 permille/day step-down");
        }

        // ── 4. Peer support reduces symptom severity ──
        [Fact]
        public void AdvanceTaperDay_ReducesSymptoms_WithPeerSupport()
        {
            var noPeerProgram = new TaperProgramState
            {
                SurvivorId                   = "survivor-002",
                DependencyPermille           = 600,
                DailyStepDownPermille        = 200,
                CurrentSubstituteDosePermille = 600,
                IsMedicallySupervised        = false,
                PeerSupportSessionsCompleted = 0
            };

            var peerProgram = new TaperProgramState
            {
                SurvivorId                   = "survivor-003",
                DependencyPermille           = 600,
                DailyStepDownPermille        = 200,
                CurrentSubstituteDosePermille = 600,
                IsMedicallySupervised        = false,
                PeerSupportSessionsCompleted = 4  // near-full peer support
            };

            var noPeerResult   = DependencyTaperWithdrawalEngine.AdvanceTaperDay(noPeerProgram, false);
            var withPeerResult = DependencyTaperWithdrawalEngine.AdvanceTaperDay(peerProgram, true);

            Assert.True((int)withPeerResult.Symptoms <= (int)noPeerResult.Symptoms,
                "Peer support should yield equal or lower symptom severity");
        }

        // ── 5. High critical case burden triggers Emergency care policy ──
        [Fact]
        public void RecommendCarePolicy_Emergency_WhenCriticalBurdenHigh()
        {
            var recommendation = DependencyTaperWithdrawalEngine.RecommendCarePolicy(
                criticalCaseCount: 12,
                totalShelterPopulation: 100,  // 12% = ≥10% → Emergency
                currentPosture: CarePolicyPosture.Permissive);

            Assert.Equal(CarePolicyPosture.Emergency, recommendation);
        }
    }
}
