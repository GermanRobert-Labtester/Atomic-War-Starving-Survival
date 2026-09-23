// SPDX-License-Identifier: MIT
// Expansion 37 — The Quickening : AntenatalMaternalHealthEngine focused tests
using Xunit;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class AntenatalMaternalHealthEngineTests
    {
        // ── 1. Gestation day transitions across all trimester bounds ──
        [Fact]
        public void ResolveTrimester_CorrectTrimesters_AcrossGestationBoundaries()
        {
            Assert.Equal(GestationTrimester.FirstTrimester, AntenatalMaternalHealthEngine.ResolveTrimester(45));
            Assert.Equal(GestationTrimester.SecondTrimester, AntenatalMaternalHealthEngine.ResolveTrimester(140));
            Assert.Equal(GestationTrimester.ThirdTrimester, AntenatalMaternalHealthEngine.ResolveTrimester(220));
            Assert.Equal(GestationTrimester.FullTerm, AntenatalMaternalHealthEngine.ResolveTrimester(270));
            Assert.Equal(GestationTrimester.Postpartum, AntenatalMaternalHealthEngine.ResolveTrimester(10, isPostpartum: true));
        }

        // ── 2. Daily progression advances gestation and drains reserves under caloric deficit ──
        [Fact]
        public void AdvancePregnancyDay_DepletesReserves_WhenUnderfed()
        {
            var state = new MaternalPregnancyState
            {
                MotherSurvivorId = "survivor-101",
                GestationDays = 100, // 2nd trimester: demand 1200
                MaternalNutritionReservePermille = 800,
                MaternalFatiguePermille = 200,
                MedicalSupervisionQualityPermille = 700,
                ShelterSanitationQualityPermille = 700
            };

            // Provide only 600 permille intake (severe deficit)
            var result = AntenatalMaternalHealthEngine.AdvancePregnancyDay(
                state,
                nutritionIntakePermille: 600,
                restHoursProvided: 8);

            Assert.Equal(101, state.GestationDays);
            Assert.True(state.MaternalNutritionReservePermille < 800,
                "Reserve should decline under significant nutritional deficit");
            Assert.Equal(GestationTrimester.SecondTrimester, result.Trimester);
            Assert.False(result.IsLaborReady);
        }

        // ── 3. High medical and nutritional support yields Healthy birth outcome ──
        [Fact]
        public void ResolveBirthDelivery_HealthyOutcome_UnderOptimalCare()
        {
            var state = new MaternalPregnancyState
            {
                MotherSurvivorId = "survivor-102",
                GestationDays = 275,
                MaternalNutritionReservePermille = 950,
                MaternalFatiguePermille = 100,
                MedicalSupervisionQualityPermille = 950,
                ShelterSanitationQualityPermille = 900
            };

            var birth = AntenatalMaternalHealthEngine.ResolveBirthDelivery(state, birthSeed: 12345);

            Assert.Equal(BirthOutcomeClassification.Healthy, birth.Outcome);
            Assert.True(birth.NeonatalVigorPermille >= 700,
                $"Neonatal vigor should be high under optimal care, got {birth.NeonatalVigorPermille}");
            Assert.True(state.IsPostpartum, "State should transition to postpartum");
        }

        // ── 4. Severe maternal depletion leads to Complicated birth ──
        [Fact]
        public void ResolveBirthDelivery_ComplicatedOutcome_UnderDepletedConditions()
        {
            var state = new MaternalPregnancyState
            {
                MotherSurvivorId = "survivor-103",
                GestationDays = 265,
                MaternalNutritionReservePermille = 150,
                MaternalFatiguePermille = 900,
                MedicalSupervisionQualityPermille = 100,
                ShelterSanitationQualityPermille = 200
            };

            var birth = AntenatalMaternalHealthEngine.ResolveBirthDelivery(state, birthSeed: 54321);

            Assert.Equal(BirthOutcomeClassification.Complicated, birth.Outcome);
            Assert.True(birth.PostpartumRecoveryDaysNeeded >= 20,
                "Complicated delivery should require extended recovery");
        }

        // ── 5. Postpartum recovery rate scales with clinical care quality ──
        [Fact]
        public void ComputePostpartumRecoveryRate_ScalesWithCareAndNutrition()
        {
            int poorRecovery = AntenatalMaternalHealthEngine.ComputePostpartumRecoveryRate(
                daysPostpartum: 10,
                careQualityPermille: 100,
                nutritionPermille: 200);

            int excellentRecovery = AntenatalMaternalHealthEngine.ComputePostpartumRecoveryRate(
                daysPostpartum: 10,
                careQualityPermille: 900,
                nutritionPermille: 950);

            Assert.True(excellentRecovery > poorRecovery,
                $"Excellent care ({excellentRecovery}) should recover faster than poor care ({poorRecovery})");
        }
    }
}
