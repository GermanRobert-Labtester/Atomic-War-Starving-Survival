using System.Collections.Generic;
using Ashfall.Core.Nutrition;
using Xunit;

namespace Ashfall.Core.Tests.Nutrition
{
    public class CommonTableRationingEngineTests
    {
        [Fact]
        public void Evaluate_Monoculture_CalculatesElevatedDeficiencyAndMoralePenalty()
        {
            var foods = new[] { FoodCategory.PreservedStarch };
            var result = CommonTableRationingEngine.Evaluate(
                consumedCategories: foods,
                policy: RationLevel.Standard,
                cookSkillPermille: 0,
                populationCount: 10,
                consecutiveLeanDays: 0,
                hasRationInequality: false
            );

            Assert.Equal(DiversityTier.Monoculture, result.DiversityTier);
            Assert.Equal(1, result.UniqueCategoryCount);
            Assert.Equal(400, result.DeficiencyRiskPermille); // Monoculture base deficiency
            Assert.Equal(-100, result.MoraleDeltaPermille);   // Monoculture morale penalty
            Assert.Equal(20, result.RequiredFoodUnits);       // 10 * 2 * 100%
            Assert.Equal(100, result.GrievanceProbabilityPermille); // Monoculture grievance bump
        }

        [Fact]
        public void Evaluate_AbundantSpectrum_ZeroBaseDeficiencyAndHighMorale()
        {
            var foods = new[]
            {
                FoodCategory.PreservedStarch,
                FoodCategory.DriedProtein,
                FoodCategory.ForagedGreens,
                FoodCategory.FreshProduce
            };

            var result = CommonTableRationingEngine.Evaluate(
                consumedCategories: foods,
                policy: RationLevel.Standard,
                cookSkillPermille: 500, // 50% cook skill
                populationCount: 10,
                consecutiveLeanDays: 0,
                hasRationInequality: false
            );

            Assert.Equal(DiversityTier.AbundantSpectrum, result.DiversityTier);
            Assert.Equal(4, result.UniqueCategoryCount);
            Assert.Equal(0, result.DeficiencyRiskPermille);
            // Morale: 120 (Spectrum) + 0 (Standard) + 25 (cook 500/1000 * 50) = 145
            Assert.Equal(145, result.MoraleDeltaPermille);
            // Cook waste reduction: 500 * 250 / 1000 = 125 permille (12.5% reduction)
            Assert.Equal(125, result.CookWasteReductionPermille);
            // Raw base units: 10 * 2 = 20. With 125 permille reduction: ceil(20 * 875 / 1000) = 18
            Assert.Equal(18, result.RequiredFoodUnits);
        }

        [Fact]
        public void Evaluate_StarvationEmergency_CompoundsDeficiencyAndGrievance()
        {
            var foods = new[] { FoodCategory.SyntheticPaste };
            var result = CommonTableRationingEngine.Evaluate(
                consumedCategories: foods,
                policy: RationLevel.StarvationEmergency,
                cookSkillPermille: 200,
                populationCount: 10,
                consecutiveLeanDays: 5,
                hasRationInequality: true
            );

            Assert.Equal(RationLevel.StarvationEmergency, result.RationLevel);
            // Base deficiency 400 + starvation 350 + 5 days * 25 (125) = 875
            Assert.Equal(875, result.DeficiencyRiskPermille);
            // Morale: -100 (Monoculture) - 250 (Starvation) + 10 (cook) = -340
            Assert.Equal(-340, result.MoraleDeltaPermille);
            // Grievance: 500 (Starvation base) + 400 (5 * 80) + 350 (Inequality) + 100 (Monoculture) = 1350 clamped to 1000
            Assert.Equal(1000, result.GrievanceProbabilityPermille);
        }

        [Fact]
        public void Evaluate_Feast_GeneratesMoraleSpikeAndZeroGrievance()
        {
            var foods = new[]
            {
                FoodCategory.DriedProtein,
                FoodCategory.CultivatedFungus,
                FoodCategory.FreshProduce
            };

            var result = CommonTableRationingEngine.Evaluate(
                consumedCategories: foods,
                policy: RationLevel.Feast,
                cookSkillPermille: 800,
                populationCount: 5,
                consecutiveLeanDays: 0,
                hasRationInequality: false
            );

            Assert.Equal(DiversityTier.BalancedTrio, result.DiversityTier);
            Assert.Equal(RationLevel.Feast, result.RationLevel);
            // Morale: 50 (BalancedTrio) + 200 (Feast) + 40 (cook 800*50/1000) = 290
            Assert.Equal(290, result.MoraleDeltaPermille);
            Assert.Equal(0, result.GrievanceProbabilityPermille);
            // Base calories: 150%
            Assert.Equal(150, result.BaseCaloriesPercent);
        }

        [Fact]
        public void Evaluate_ZeroPopulation_ReturnsZeroFoodRequired()
        {
            var result = CommonTableRationingEngine.Evaluate(
                consumedCategories: null,
                policy: RationLevel.Standard,
                cookSkillPermille: 500,
                populationCount: 0
            );

            Assert.Equal(0, result.RequiredFoodUnits);
            Assert.Equal(0, result.UniqueCategoryCount);
            Assert.Equal(DiversityTier.Monoculture, result.DiversityTier);
        }
    }
}
