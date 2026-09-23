using System;
using System.Collections.Generic;

namespace Ashfall.Core.Nutrition
{
    /// <summary>
    /// Major food categories contributing to dietary diversity.
    /// </summary>
    public enum FoodCategory
    {
        PreservedStarch = 0,
        DriedProtein = 1,
        ForagedGreens = 2,
        CultivatedFungus = 3,
        FreshProduce = 4,
        SyntheticPaste = 5,
        PressedLipids = 6
    }

    /// <summary>
    /// Dietary diversity classification based on unique food categories consumed.
    /// </summary>
    public enum DiversityTier
    {
        Monoculture = 0,     // 1 category: deficiency risks, morale penalty
        BasicDual = 1,       // 2 categories: baseline sustenance
        BalancedTrio = 2,    // 3 categories: vitality bonus, health buffer
        AbundantSpectrum = 3 // 4+ categories: peak vigor and community morale
    }

    /// <summary>
    /// Collective rationing policy governing intake and morale impact.
    /// </summary>
    public enum RationLevel
    {
        StarvationEmergency = 0, // 50% calories, severe grievance and illness risk
        LeanRation = 1,          // 75% calories, mild fatigue, moderate grievance
        Standard = 2,            // 100% calories, neutral
        HeavyDuty = 3,           // 125% calories, labor vigor, higher burn
        Feast = 4                // 150% calories, celebratory morale boost
    }

    /// <summary>
    /// Immutable evaluation result produced by <see cref="CommonTableRationingEngine"/>.
    /// Contains consumption requirements, deficiency risks, and grievance permille.
    /// </summary>
    public readonly struct NutritionEvaluationResult
    {
        public DiversityTier DiversityTier { get; }
        public RationLevel RationLevel { get; }
        public int UniqueCategoryCount { get; }
        public int BaseCaloriesPercent { get; }
        public int NetCalorieIntakePercent { get; }
        public int DeficiencyRiskPermille { get; }
        public int MoraleDeltaPermille { get; }
        public int GrievanceProbabilityPermille { get; }
        public int CookWasteReductionPermille { get; }
        public int RequiredFoodUnits { get; }

        public NutritionEvaluationResult(
            DiversityTier diversityTier,
            RationLevel rationLevel,
            int uniqueCategoryCount,
            int baseCaloriesPercent,
            int netCalorieIntakePercent,
            int deficiencyRiskPermille,
            int moraleDeltaPermille,
            int grievanceProbabilityPermille,
            int cookWasteReductionPermille,
            int requiredFoodUnits)
        {
            DiversityTier = diversityTier;
            RationLevel = rationLevel;
            UniqueCategoryCount = uniqueCategoryCount;
            BaseCaloriesPercent = baseCaloriesPercent;
            NetCalorieIntakePercent = netCalorieIntakePercent;
            DeficiencyRiskPermille = deficiencyRiskPermille;
            MoraleDeltaPermille = moraleDeltaPermille;
            GrievanceProbabilityPermille = grievanceProbabilityPermille;
            CookWasteReductionPermille = cookWasteReductionPermille;
            RequiredFoodUnits = requiredFoodUnits;
        }
    }

    /// <summary>
    /// Pure domain engine for Common Table nutrition diversity, dietary deficiency modeling,
    /// cook duty efficiency, and rationing grievance evaluation (Expansion 26).
    /// Operates without engine dependencies or parallel food stores.
    /// </summary>
    public static class CommonTableRationingEngine
    {
        public const int PermilleScale = 1000;
        public const int BaseFoodUnitsPerPerson = 2;

        /// <summary>
        /// Evaluates dietary lipid absorption factor. When pressed lipids (culinary oils, confit) are present,
        /// digestive absorption efficiency increases by up to 10% (100 permille).
        /// </summary>
        public static int EvaluateLipidAbsorptionModifier(bool hasLipids) => hasLipids ? 10 : 0;

        /// <summary>
        /// Evaluates dietary outcome, deficiency risks, and grievance for a group meal session.
        /// </summary>
        /// <param name="consumedCategories">Distinct categories served or consumed.</param>
        /// <param name="policy">Active community rationing policy.</param>
        /// <param name="cookSkillPermille">Cook duty skill rating (0..1000).</param>
        /// <param name="populationCount">Number of people dining.</param>
        /// <param name="consecutiveLeanDays">Days under Lean or Starvation rationing.</param>
        /// <param name="hasRationInequality">True if certain privileged cohorts received higher rationing.</param>
        /// <returns>Immutable <see cref="NutritionEvaluationResult"/>.</returns>
        public static NutritionEvaluationResult Evaluate(
            IEnumerable<FoodCategory> consumedCategories,
            RationLevel policy,
            int cookSkillPermille,
            int populationCount,
            int consecutiveLeanDays = 0,
            bool hasRationInequality = false)
        {
            if (populationCount < 0) populationCount = 0;
            cookSkillPermille = Math.Max(0, Math.Min(PermilleScale, cookSkillPermille));
            consecutiveLeanDays = Math.Max(0, consecutiveLeanDays);

            // Count unique categories
            var unique = new HashSet<FoodCategory>();
            if (consumedCategories != null)
            {
                foreach (var cat in consumedCategories)
                {
                    unique.Add(cat);
                }
            }

            int count = unique.Count;
            DiversityTier tier;
            if (count <= 1)
            {
                tier = DiversityTier.Monoculture;
            }
            else if (count == 2)
            {
                tier = DiversityTier.BasicDual;
            }
            else if (count == 3)
            {
                tier = DiversityTier.BalancedTrio;
            }
            else
            {
                tier = DiversityTier.AbundantSpectrum;
            }

            // Calorie and consumption factors
            int baseCalPercent;
            switch (policy)
            {
                case RationLevel.StarvationEmergency:
                    baseCalPercent = 50;
                    break;
                case RationLevel.LeanRation:
                    baseCalPercent = 75;
                    break;
                case RationLevel.HeavyDuty:
                    baseCalPercent = 125;
                    break;
                case RationLevel.Feast:
                    baseCalPercent = 150;
                    break;
                case RationLevel.Standard:
                default:
                    baseCalPercent = 100;
                    break;
            }

            // Cook efficiency reduces food waste up to 25% (250 permille)
            int cookWasteReduction = (cookSkillPermille * 250) / PermilleScale;

            // Base food units required: (pop * baseUnits * baseCalPercent) / 100
            // Cook waste reduction decreases required raw ingredients: required * (1000 - reduction) / 1000
            long rawBaseUnits = ((long)populationCount * BaseFoodUnitsPerPerson * baseCalPercent + 99) / 100;
            int finalRequiredUnits = (int)((rawBaseUnits * (PermilleScale - cookWasteReduction) + (PermilleScale - 1)) / PermilleScale);

            // Net calorie intake (efficiency bonus from skilled prep adds up to +10% effective nutrient absorption)
            int netCalorieIntakePercent = baseCalPercent + (cookSkillPermille * 10) / PermilleScale;

            // Deficiency risk permille calculation
            int baseDeficiencyRisk;
            switch (tier)
            {
                case DiversityTier.Monoculture:
                    baseDeficiencyRisk = 400; // 40% risk per period under single-food monotony
                    break;
                case DiversityTier.BasicDual:
                    baseDeficiencyRisk = 150; // 15% risk
                    break;
                case DiversityTier.BalancedTrio:
                    baseDeficiencyRisk = 30;  // 3% risk
                    break;
                case DiversityTier.AbundantSpectrum:
                default:
                    baseDeficiencyRisk = 0;   // Complete nutrition profile
                    break;
            }

            // Starvation / Lean rationing accelerates nutritional deficiency
            if (policy == RationLevel.StarvationEmergency)
            {
                baseDeficiencyRisk += 350;
            }
            else if (policy == RationLevel.LeanRation)
            {
                baseDeficiencyRisk += 120;
            }

            // Long-term consecutive lean periods compound deficiency
            baseDeficiencyRisk += Math.Min(300, consecutiveLeanDays * 25);
            int deficiencyRiskPermille = Math.Max(0, Math.Min(PermilleScale, baseDeficiencyRisk));

            // Morale delta permille calculation
            // Baseline from diversity tier
            int moraleDelta;
            switch (tier)
            {
                case DiversityTier.Monoculture:
                    moraleDelta = -100; // -10% morale from monotonous slop
                    break;
                case DiversityTier.BasicDual:
                    moraleDelta = 0;
                    break;
                case DiversityTier.BalancedTrio:
                    moraleDelta = 50;   // +5% morale
                    break;
                case DiversityTier.AbundantSpectrum:
                default:
                    moraleDelta = 120;  // +12% morale
                    break;
            }

            // Policy effect on morale
            switch (policy)
            {
                case RationLevel.StarvationEmergency:
                    moraleDelta -= 250;
                    break;
                case RationLevel.LeanRation:
                    moraleDelta -= 80;
                    break;
                case RationLevel.HeavyDuty:
                    moraleDelta += 40;
                    break;
                case RationLevel.Feast:
                    moraleDelta += 200;
                    break;
            }

            // Cook quality enhances meal morale (+ up to 50 permille)
            moraleDelta += (cookSkillPermille * 50) / PermilleScale;

            // Grievance probability permille
            int grievance = 0;
            if (policy == RationLevel.StarvationEmergency)
            {
                grievance += 500 + Math.Min(400, consecutiveLeanDays * 80);
            }
            else if (policy == RationLevel.LeanRation)
            {
                grievance += 150 + Math.Min(300, consecutiveLeanDays * 30);
            }

            // Perceived inequality in rationing drastically provokes conflict
            if (hasRationInequality)
            {
                grievance += 350;
            }

            // Monoculture slop increases irritability under stress
            if (tier == DiversityTier.Monoculture && policy != RationLevel.Feast)
            {
                grievance += 100;
            }

            int grievanceProbabilityPermille = Math.Max(0, Math.Min(PermilleScale, grievance));

            return new NutritionEvaluationResult(
                diversityTier: tier,
                rationLevel: policy,
                uniqueCategoryCount: count,
                baseCaloriesPercent: baseCalPercent,
                netCalorieIntakePercent: netCalorieIntakePercent,
                deficiencyRiskPermille: deficiencyRiskPermille,
                moraleDeltaPermille: moraleDelta,
                grievanceProbabilityPermille: grievanceProbabilityPermille,
                cookWasteReductionPermille: cookWasteReduction,
                requiredFoodUnits: finalRequiredUnits
            );
        }
    }
}
