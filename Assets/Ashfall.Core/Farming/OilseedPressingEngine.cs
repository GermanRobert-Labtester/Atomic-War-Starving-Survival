// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Nutrition;

namespace Ashfall.Core.Farming
{
    /// <summary>
    /// Target product classification for oilseed pressing operations.
    /// </summary>
    public enum OilseedPressingMode
    {
        CulinaryCookingOil = 0,
        LampFuelOil = 1,
        MechanicalLubricant = 2
    }

    /// <summary>
    /// Workshop tooling and press apparatus grade determining extraction efficiency.
    /// </summary>
    public enum PressToolGrade
    {
        ImprovisedMortar = 0,      // Hand crushing, low extraction (500 permille), high waste
        ManualScrewPress = 1,      // Standard workshop screw press (750 permille), balanced yield
        HydraulicWorkshopPress = 2  // Precision mechanical/hydraulic press (920 permille), high purity
    }

    /// <summary>
    /// Immutable result of an oil extraction run.
    /// </summary>
    public readonly struct PressingYieldResult
    {
        public int InputSeedCount { get; }
        public string PrimaryOutputItemId { get; }
        public int PrimaryOutputAmount { get; }
        public int ByproductMealAmount { get; }
        public int ExtractionEfficiencyPermille { get; }
        public int WasteLossPermille { get; }

        public PressingYieldResult(
            int inputSeedCount,
            string primaryOutputItemId,
            int primaryOutputAmount,
            int byproductMealAmount,
            int extractionEfficiencyPermille,
            int wasteLossPermille)
        {
            InputSeedCount = inputSeedCount;
            PrimaryOutputItemId = primaryOutputItemId ?? string.Empty;
            PrimaryOutputAmount = primaryOutputAmount;
            ByproductMealAmount = byproductMealAmount;
            ExtractionEfficiencyPermille = extractionEfficiencyPermille;
            WasteLossPermille = wasteLossPermille;
        }
    }

    /// <summary>
    /// Immutable result of a salt-and-oil confit preservation batch.
    /// </summary>
    public readonly struct ConfitPreservationResult
    {
        public string BaseFoodItemId { get; }
        public int BaseFoodUnitsConsumed { get; }
        public int OilUnitsConsumed { get; }
        public int SaltUnitsConsumed { get; }
        public string PreservedItemId { get; }
        public int PreservedUnitsProduced { get; }
        public int ShelfLifeMultiplierPermille { get; }
        public int SpoilageResistancePermille { get; }

        public ConfitPreservationResult(
            string baseFoodItemId,
            int baseFoodUnitsConsumed,
            int oilUnitsConsumed,
            int saltUnitsConsumed,
            string preservedItemId,
            int preservedUnitsProduced,
            int shelfLifeMultiplierPermille,
            int spoilageResistancePermille)
        {
            BaseFoodItemId = baseFoodItemId ?? string.Empty;
            BaseFoodUnitsConsumed = baseFoodUnitsConsumed;
            OilUnitsConsumed = oilUnitsConsumed;
            SaltUnitsConsumed = saltUnitsConsumed;
            PreservedItemId = preservedItemId ?? string.Empty;
            PreservedUnitsProduced = preservedUnitsProduced;
            ShelfLifeMultiplierPermille = shelfLifeMultiplierPermille;
            SpoilageResistancePermille = spoilageResistancePermille;
        }
    }

    /// <summary>
    /// Pure domain engine for agricultural oilseed extraction, confit food preservation,
    /// seed retention planning, and lipid nutritional contribution (Plan 22 / Crop Roster Phase 2).
    /// Operates without engine dependencies or parallel inventory ledgers.
    /// </summary>
    public static class OilseedPressingEngine
    {
        public const int PermilleScale = 1000;
        public const int BaseSeedsPerOilBatch = 2; // Matches canonical recipe craft_press_oilseed
        public const string DefaultCookingOilId = "cooking_oil";
        public const string DefaultLubricantOilId = "lubricant_oil";
        public const string DefaultPreservedConfitId = "item_fat_confit";
        public const string PreservationSaltId = "item_preservation_salt";
        public const string CropOilseedId = "crop_oilseed";

        /// <summary>
        /// Evaluates pressing yield from harvested oilseeds.
        /// </summary>
        /// <param name="seedCount">Raw oilseed crop units available for pressing.</param>
        /// <param name="toolGrade">Available extraction tooling.</param>
        /// <param name="mode">Target product kind.</param>
        /// <returns>Immutable <see cref="PressingYieldResult"/>.</returns>
        public static PressingYieldResult EvaluatePressing(
            int seedCount,
            PressToolGrade toolGrade = PressToolGrade.ManualScrewPress,
            OilseedPressingMode mode = OilseedPressingMode.CulinaryCookingOil)
        {
            if (seedCount < BaseSeedsPerOilBatch)
            {
                string emptyOutput = mode == OilseedPressingMode.MechanicalLubricant
                    ? DefaultLubricantOilId
                    : DefaultCookingOilId;
                return new PressingYieldResult(seedCount, emptyOutput, 0, 0, 0, 0);
            }

            int efficiency;
            int waste;
            int mealYieldPermille;

            switch (toolGrade)
            {
                case PressToolGrade.ImprovisedMortar:
                    efficiency = 500;
                    waste = 300;
                    mealYieldPermille = 200;
                    break;
                case PressToolGrade.HydraulicWorkshopPress:
                    efficiency = 920;
                    waste = 50;
                    mealYieldPermille = 450;
                    break;
                case PressToolGrade.ManualScrewPress:
                default:
                    efficiency = 750;
                    waste = 150;
                    mealYieldPermille = 350;
                    break;
            }

            int fullBatches = seedCount / BaseSeedsPerOilBatch;
            int effectiveSeeds = fullBatches * BaseSeedsPerOilBatch;

            // Output item determination
            string outputItemId = mode switch
            {
                OilseedPressingMode.MechanicalLubricant => DefaultLubricantOilId,
                _ => DefaultCookingOilId
            };

            // Calculated primary yield: batches scaled by efficiency
            int primaryYield = (fullBatches * efficiency + (PermilleScale / 2)) / PermilleScale;
            if (primaryYield < 1 && fullBatches > 0)
                primaryYield = 1;

            // Byproduct meal cake (high-protein ration supplement or animal feed)
            int mealUnits = (effectiveSeeds * mealYieldPermille + (PermilleScale / 2)) / PermilleScale;

            return new PressingYieldResult(
                effectiveSeeds,
                outputItemId,
                primaryYield,
                mealUnits,
                efficiency,
                waste);
        }

        /// <summary>
        /// Evaluates confit preservation batches from base perishables (tubers, meats), culinary oil, and salt.
        /// Standard batch: 3 base food units + 1 oil + 1 salt -> 3 preserved confit units.
        /// </summary>
        public static ConfitPreservationResult EvaluateConfitPreservation(
            string baseFoodItemId,
            int baseUnits,
            int availableOil,
            int availableSalt)
        {
            if (string.IsNullOrEmpty(baseFoodItemId) || baseUnits < 3 || availableOil < 1 || availableSalt < 1)
            {
                return new ConfitPreservationResult(baseFoodItemId, 0, 0, 0, DefaultPreservedConfitId, 0, 1000, 0);
            }

            // Batches are constrained by the minimum of base food (groups of 3), available oil (1:1), and available salt (1:1)
            int possibleFoodBatches = baseUnits / 3;
            int batches = Math.Min(possibleFoodBatches, Math.Min(availableOil, availableSalt));

            int consumedFood = batches * 3;
            int consumedOil = batches;
            int consumedSalt = batches;
            int producedConfit = batches * 3;

            // Confit creates an anaerobic fat barrier that extends shelf-life by 4.0x (4000 permille)
            // and provides 900 permille spoilage resistance against ambient bacterial decay.
            int shelfLifeMultiplier = 4000;
            int spoilageResistance = 900;

            return new ConfitPreservationResult(
                baseFoodItemId,
                consumedFood,
                consumedOil,
                consumedSalt,
                DefaultPreservedConfitId,
                producedConfit,
                shelfLifeMultiplier,
                spoilageResistance);
        }

        /// <summary>
        /// Calculates sustainable seed retention for next season's planting before pressing.
        /// Preserves the core survival loop: never press 100% of the harvest and starve next cycle.
        /// </summary>
        /// <param name="harvestCount">Total harvested oilseed units.</param>
        /// <param name="plannedPlotsNextSeason">Number of greenhouse plots planned for oilseed.</param>
        /// <param name="seedsPerPlot">Seed requirement per plot (default 1).</param>
        /// <returns>Count of seeds that MUST be saved as seed stock.</returns>
        public static int CalculateSustainableSeedRetention(
            int harvestCount,
            int plannedPlotsNextSeason = 4,
            int seedsPerPlot = 1)
        {
            if (harvestCount <= 0) return 0;
            int safetyMarginPlots = plannedPlotsNextSeason + 1; // 1 extra plot buffer against blight/spoilage
            int requiredSeedStock = safetyMarginPlots * seedsPerPlot;
            return Math.Min(harvestCount, requiredSeedStock);
        }

        /// <summary>
        /// Evaluates net caloric intake and absorption bonus when dietary lipids are included in rations.
        /// </summary>
        public static int EvaluateDietaryLipidContribution(bool hasPressedLipids, int baseCaloriesPercent)
        {
            int lipidAbsorptionBonus = CommonTableRationingEngine.EvaluateLipidAbsorptionModifier(hasPressedLipids);
            return baseCaloriesPercent + lipidAbsorptionBonus;
        }
    }
}
