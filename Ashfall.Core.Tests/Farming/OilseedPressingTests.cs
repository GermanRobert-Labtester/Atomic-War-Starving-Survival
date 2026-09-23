// SPDX-License-Identifier: MIT
using Ashfall.Core.Farming;
using Ashfall.Core.Nutrition;
using Xunit;

namespace Ashfall.Core.Tests.Farming
{
    public sealed class OilseedPressingTests
    {
        [Fact]
        public void EvaluatePressing_SubThresholdSeedCount_ReturnsZeroYield()
        {
            var result = OilseedPressingEngine.EvaluatePressing(1, PressToolGrade.ManualScrewPress);

            Assert.Equal(1, result.InputSeedCount);
            Assert.Equal(0, result.PrimaryOutputAmount);
            Assert.Equal(0, result.ByproductMealAmount);
            Assert.Equal(0, result.ExtractionEfficiencyPermille);
            Assert.Equal(OilseedPressingEngine.DefaultCookingOilId, result.PrimaryOutputItemId);
        }

        [Fact]
        public void EvaluatePressing_ToolGrades_ScaleEfficiencyAndWasteMonotonically()
        {
            const int seedCount = 20; // 10 batches of 2

            var improvised = OilseedPressingEngine.EvaluatePressing(seedCount, PressToolGrade.ImprovisedMortar);
            var manual = OilseedPressingEngine.EvaluatePressing(seedCount, PressToolGrade.ManualScrewPress);
            var hydraulic = OilseedPressingEngine.EvaluatePressing(seedCount, PressToolGrade.HydraulicWorkshopPress);

            // Efficiencies: 500 < 750 < 920
            Assert.True(improvised.ExtractionEfficiencyPermille < manual.ExtractionEfficiencyPermille);
            Assert.True(manual.ExtractionEfficiencyPermille < hydraulic.ExtractionEfficiencyPermille);

            // Waste: 300 > 150 > 50
            Assert.True(improvised.WasteLossPermille > manual.WasteLossPermille);
            Assert.True(manual.WasteLossPermille > hydraulic.WasteLossPermille);

            // Yields: improvised <= manual <= hydraulic
            Assert.True(improvised.PrimaryOutputAmount <= manual.PrimaryOutputAmount);
            Assert.True(manual.PrimaryOutputAmount <= hydraulic.PrimaryOutputAmount);

            // Hydraulic press extracts maximum oil and byproduct
            Assert.Equal(9, hydraulic.PrimaryOutputAmount);
            Assert.Equal(9, hydraulic.ByproductMealAmount);
        }

        [Fact]
        public void EvaluatePressing_Modes_SelectCorrectOutputItem()
        {
            var culinary = OilseedPressingEngine.EvaluatePressing(10, PressToolGrade.ManualScrewPress, OilseedPressingMode.CulinaryCookingOil);
            var lamp = OilseedPressingEngine.EvaluatePressing(10, PressToolGrade.ManualScrewPress, OilseedPressingMode.LampFuelOil);
            var lubricant = OilseedPressingEngine.EvaluatePressing(10, PressToolGrade.ManualScrewPress, OilseedPressingMode.MechanicalLubricant);

            Assert.Equal(OilseedPressingEngine.DefaultCookingOilId, culinary.PrimaryOutputItemId);
            Assert.Equal(OilseedPressingEngine.DefaultCookingOilId, lamp.PrimaryOutputItemId);
            Assert.Equal(OilseedPressingEngine.DefaultLubricantOilId, lubricant.PrimaryOutputItemId);
        }

        [Fact]
        public void EvaluateConfitPreservation_InsufficientIngredients_ReturnsZero()
        {
            // Missing salt
            var noSalt = OilseedPressingEngine.EvaluateConfitPreservation("crop_tuber", 6, 2, 0);
            Assert.Equal(0, noSalt.PreservedUnitsProduced);

            // Missing oil
            var noOil = OilseedPressingEngine.EvaluateConfitPreservation("crop_tuber", 6, 0, 2);
            Assert.Equal(0, noOil.PreservedUnitsProduced);

            // Insufficient base perishables (< 3)
            var lowFood = OilseedPressingEngine.EvaluateConfitPreservation("crop_tuber", 2, 2, 2);
            Assert.Equal(0, lowFood.PreservedUnitsProduced);
        }

        [Fact]
        public void EvaluateConfitPreservation_SufficientIngredients_ProducesPreservedConfitAndCalculatesShelfLife()
        {
            // 8 tubers, 3 oil, 2 salt -> bottle-necked by salt (2 batches = 6 tubers, 2 oil, 2 salt)
            var result = OilseedPressingEngine.EvaluateConfitPreservation("crop_tuber", 8, 3, 2);

            Assert.Equal("crop_tuber", result.BaseFoodItemId);
            Assert.Equal(6, result.BaseFoodUnitsConsumed);
            Assert.Equal(2, result.OilUnitsConsumed);
            Assert.Equal(2, result.SaltUnitsConsumed);
            Assert.Equal(OilseedPressingEngine.DefaultPreservedConfitId, result.PreservedItemId);
            Assert.Equal(6, result.PreservedUnitsProduced);
            Assert.Equal(4000, result.ShelfLifeMultiplierPermille); // 4x shelf life
            Assert.Equal(900, result.SpoilageResistancePermille);   // 90% resistance
        }

        [Fact]
        public void CalculateSustainableSeedRetention_ProtectsSeedStockWithSafetyBuffer()
        {
            // 10 seeds harvested, 4 planned plots, 1 seed per plot -> 4 + 1 safety buffer = 5 retained
            int retained = OilseedPressingEngine.CalculateSustainableSeedRetention(10, plannedPlotsNextSeason: 4, seedsPerPlot: 1);
            Assert.Equal(5, retained);

            // If harvest is smaller than buffer, retain all
            int smallHarvest = OilseedPressingEngine.CalculateSustainableSeedRetention(3, plannedPlotsNextSeason: 4, seedsPerPlot: 1);
            Assert.Equal(3, smallHarvest);
        }

        [Fact]
        public void EvaluateDietaryLipidContribution_CalculatesCaloricAbsorptionBonus()
        {
            int baseWithoutLipids = OilseedPressingEngine.EvaluateDietaryLipidContribution(hasPressedLipids: false, baseCaloriesPercent: 100);
            int baseWithLipids = OilseedPressingEngine.EvaluateDietaryLipidContribution(hasPressedLipids: true, baseCaloriesPercent: 100);

            Assert.Equal(100, baseWithoutLipids);
            Assert.Equal(110, baseWithLipids); // +10% absorption bonus
        }
    }
}
