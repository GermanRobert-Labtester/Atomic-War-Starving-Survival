// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 37 — The Quickening
// Subsystem    : Antenatal Care, Maternal Support & Neonatal Health Engine
// Authority    : docs/expansions/wave6/expansion_37_the_quickening_plan.md
//                WAVE6_INDEX.md
// ============================================================================
using System;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Gestational stage of pregnancy.
    /// </summary>
    public enum GestationTrimester
    {
        FirstTrimester  = 0, // Days 1–90: organogenesis, morning sickness, nutritional sensitivity
        SecondTrimester = 1, // Days 91–180: steady growth, increased rest requirements
        ThirdTrimester  = 2, // Days 181–260: rapid growth, severe maternal mobility/fatigue penalties
        FullTerm        = 3, // Days 261+: labor-ready window
        Postpartum      = 4  // Post-delivery recovery period
    }

    /// <summary>
    /// Maternal physiological stability classification.
    /// </summary>
    public enum MaternalHealthStatus
    {
        Stable   = 0, // Vitals and reserves within normal boundaries
        Strained = 1, // Minor nutritional deficit or elevated fatigue
        AtRisk   = 2, // Substantial deficit, medical monitoring indicated
        Critical = 3  // Severe deficit or complication; hospital bed required
    }

    /// <summary>
    /// Delivery outcome tier. Elided, dignified, non-eugenic classification.
    /// </summary>
    public enum BirthOutcomeClassification
    {
        Healthy     = 0, // Robust neonatal vitality, uncomplicated delivery
        Delicate    = 1, // Low birthweight or mild distress; requires nursery warmth & feeding support
        Complicated = 2  // Maternal hemorrhage or neonatal respiratory distress; hospital care needed
    }

    /// <summary>
    /// Mutable state record for a pregnant survivor.
    /// Extends ChildDevelopmentSystem and CaregivingSystem without creating redundant relation stores.
    /// </summary>
    public sealed class MaternalPregnancyState
    {
        public string MotherSurvivorId                { get; set; } = string.Empty;
        public int GestationDays                      { get; set; } = 1;
        public int MaternalNutritionReservePermille   { get; set; } = 800;
        public int MaternalFatiguePermille            { get; set; } = 200;
        public int ShelterSanitationQualityPermille   { get; set; } = 700;
        public int MedicalSupervisionQualityPermille  { get; set; } = 600;
        public int AccumulatedStressPermille          { get; set; } = 100;
        public bool IsPostpartum                      { get; set; } = false;

        public MaternalPregnancyState Clone() => new MaternalPregnancyState
        {
            MotherSurvivorId               = MotherSurvivorId,
            GestationDays                  = GestationDays,
            MaternalNutritionReservePermille = MaternalNutritionReservePermille,
            MaternalFatiguePermille        = MaternalFatiguePermille,
            ShelterSanitationQualityPermille = ShelterSanitationQualityPermille,
            MedicalSupervisionQualityPermille = MedicalSupervisionQualityPermille,
            AccumulatedStressPermille      = AccumulatedStressPermille,
            IsPostpartum                   = IsPostpartum
        };
    }

    /// <summary>
    /// Immutable result of a daily pregnancy advancement step.
    /// </summary>
    public readonly struct TrimesterProgressionResult
    {
        public GestationTrimester Trimester                     { get; }
        public MaternalHealthStatus HealthStatus                { get; }
        public int DailyCaloricDemandMultiplierPermille         { get; }
        public int ComplicationRiskPermille                     { get; }
        public bool IsLaborReady                                { get; }

        public TrimesterProgressionResult(
            GestationTrimester trimester,
            MaternalHealthStatus healthStatus,
            int dailyCaloricDemandMultiplierPermille,
            int complicationRiskPermille,
            bool isLaborReady)
        {
            Trimester                           = trimester;
            HealthStatus                        = healthStatus;
            DailyCaloricDemandMultiplierPermille = Math.Max(1000, dailyCaloricDemandMultiplierPermille);
            ComplicationRiskPermille            = Math.Clamp(complicationRiskPermille, 0, 1000);
            IsLaborReady                        = isLaborReady;
        }
    }

    /// <summary>
    /// Immutable result of delivery resolution.
    /// </summary>
    public readonly struct BirthResolutionResult
    {
        public BirthOutcomeClassification Outcome   { get; }
        public int NeonatalVigorPermille            { get; }
        public int MaternalExhaustionPermille       { get; }
        public int PostpartumRecoveryDaysNeeded     { get; }

        public BirthResolutionResult(
            BirthOutcomeClassification outcome,
            int neonatalVigorPermille,
            int maternalExhaustionPermille,
            int postpartumRecoveryDaysNeeded)
        {
            Outcome                     = outcome;
            NeonatalVigorPermille       = Math.Clamp(neonatalVigorPermille, 0, 1000);
            MaternalExhaustionPermille  = Math.Clamp(maternalExhaustionPermille, 0, 1000);
            PostpartumRecoveryDaysNeeded = Math.Max(3, postpartumRecoveryDaysNeeded);
        }
    }

    /// <summary>
    /// Pure domain engine governing antenatal trimester progression, maternal nutritional demand,
    /// delivery outcome resolution, and postpartum recovery curves.
    /// Extends ChildDevelopmentSystem, CohortSystem, and CaregivingSystem seams.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class AntenatalMaternalHealthEngine
    {
        public const int FullTermGestationDaysThreshold = 260;
        public const int StandardFullTermDays = 280;

        /// <summary>
        /// Resolves the current gestation trimester from gestation age in days.
        /// </summary>
        public static GestationTrimester ResolveTrimester(int gestationDays, bool isPostpartum = false)
        {
            if (isPostpartum) return GestationTrimester.Postpartum;
            if (gestationDays < 1) return GestationTrimester.FirstTrimester;

            return gestationDays switch
            {
                <= 90  => GestationTrimester.FirstTrimester,
                <= 180 => GestationTrimester.SecondTrimester,
                <= 260 => GestationTrimester.ThirdTrimester,
                _      => GestationTrimester.FullTerm
            };
        }

        /// <summary>
        /// Advances pregnancy by one day, computing nutritional deficit, fatigue, and complication risk.
        /// Mutates MaternalPregnancyState in place.
        /// </summary>
        /// <param name="state">Pregnancy state record.</param>
        /// <param name="nutritionIntakePermille">Nutrition intake provided today (1000 = full recommended intake).</param>
        /// <param name="restHoursProvided">Hours of rest/sleep provided (8 is standard).</param>
        public static TrimesterProgressionResult AdvancePregnancyDay(
            MaternalPregnancyState state,
            int nutritionIntakePermille,
            int restHoursProvided)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            nutritionIntakePermille = Math.Clamp(nutritionIntakePermille, 0, 2000);
            restHoursProvided       = Math.Clamp(restHoursProvided, 0, 24);

            if (!state.IsPostpartum)
            {
                state.GestationDays++;
            }

            GestationTrimester trimester = ResolveTrimester(state.GestationDays, state.IsPostpartum);

            // Caloric/metabolic demand multiplier increases by trimester
            int demandMultiplier = trimester switch
            {
                GestationTrimester.FirstTrimester  => 1050, // +5%
                GestationTrimester.SecondTrimester => 1200, // +20%
                GestationTrimester.ThirdTrimester  => 1350, // +35%
                GestationTrimester.FullTerm        => 1400, // +40%
                GestationTrimester.Postpartum      => 1250, // +25% lactation support
                _                                  => 1000
            };

            // Nutrition delta: shortfall drains reserve; surplus refills it
            int effectiveDemand = demandMultiplier;
            int nutritionRatio  = (nutritionIntakePermille * 1000) / effectiveDemand;
            int nutritionDelta  = (nutritionRatio - 1000) / 10; // -100..+100 per day
            state.MaternalNutritionReservePermille = Math.Clamp(
                state.MaternalNutritionReservePermille + nutritionDelta, 0, 1000);

            // Rest & fatigue adjustment
            int targetRestHours = trimester >= GestationTrimester.ThirdTrimester ? 10 : 8;
            int restDeficit     = targetRestHours - restHoursProvided;
            int fatigueDelta    = restDeficit > 0 ? restDeficit * 60 : restDeficit * 40;
            state.MaternalFatiguePermille = Math.Clamp(
                state.MaternalFatiguePermille + fatigueDelta, 0, 1000);

            // Calculate complication risk permille
            int baseRisk = trimester switch
            {
                GestationTrimester.FirstTrimester  => 120,
                GestationTrimester.SecondTrimester => 80,
                GestationTrimester.ThirdTrimester  => 160,
                GestationTrimester.FullTerm        => 220,
                _                                  => 50
            };

            int nutritionRisk = (1000 - state.MaternalNutritionReservePermille) * 200 / 1000;
            int fatigueRisk   = state.MaternalFatiguePermille * 150 / 1000;
            int clinicMitigation = state.MedicalSupervisionQualityPermille * 180 / 1000;
            int sanitationMitigation = state.ShelterSanitationQualityPermille * 120 / 1000;

            int netRisk = Math.Clamp(
                baseRisk + nutritionRisk + fatigueRisk - clinicMitigation - sanitationMitigation,
                10, 950);

            // Status classification
            MaternalHealthStatus healthStatus = MaternalHealthStatus.Stable;
            if (state.MaternalNutritionReservePermille < 250 || state.MaternalFatiguePermille > 800 || netRisk > 600)
            {
                healthStatus = MaternalHealthStatus.Critical;
            }
            else if (state.MaternalNutritionReservePermille < 500 || state.MaternalFatiguePermille > 600 || netRisk > 350)
            {
                healthStatus = MaternalHealthStatus.AtRisk;
            }
            else if (state.MaternalNutritionReservePermille < 750 || state.MaternalFatiguePermille > 400 || netRisk > 200)
            {
                healthStatus = MaternalHealthStatus.Strained;
            }

            bool isLaborReady = state.GestationDays >= FullTermGestationDaysThreshold && !state.IsPostpartum;

            return new TrimesterProgressionResult(
                trimester,
                healthStatus,
                demandMultiplier,
                netRisk,
                isLaborReady);
        }

        /// <summary>
        /// Resolves birth delivery deterministically from maternal condition, clinic readiness, and seed.
        /// </summary>
        public static BirthResolutionResult ResolveBirthDelivery(
            MaternalPregnancyState state,
            int birthSeed)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            // Composite delivery readiness score
            int compositeScore = (state.MaternalNutritionReservePermille * 30 +
                                  (1000 - state.MaternalFatiguePermille) * 25 +
                                  state.MedicalSupervisionQualityPermille * 30 +
                                  state.ShelterSanitationQualityPermille * 15) / 100;

            // Deterministic variance ±8%
            int hash = HashCode.Combine(birthSeed, state.GestationDays, state.MotherSurvivorId?.GetHashCode() ?? 0);
            int variance = ((hash & 0x7FFFFFFF) % 17) - 8;
            compositeScore = Math.Clamp(compositeScore + (compositeScore * variance) / 100, 0, 1000);

            BirthOutcomeClassification outcome;
            int neonatalVigor;
            int recoveryDays;

            if (compositeScore >= 680)
            {
                outcome = BirthOutcomeClassification.Healthy;
                neonatalVigor = Math.Clamp(compositeScore + 100, 700, 1000);
                recoveryDays = 7 + (1000 - compositeScore) / 80;
            }
            else if (compositeScore >= 380)
            {
                outcome = BirthOutcomeClassification.Delicate;
                neonatalVigor = Math.Clamp(compositeScore, 400, 750);
                recoveryDays = 14 + (1000 - compositeScore) / 50;
            }
            else
            {
                outcome = BirthOutcomeClassification.Complicated;
                neonatalVigor = Math.Clamp(compositeScore - 100, 200, 500);
                recoveryDays = 21 + (1000 - compositeScore) / 40;
            }

            int maternalExhaustion = Math.Clamp(600 + (1000 - compositeScore) / 3, 500, 1000);

            // Mark postpartum transition
            state.IsPostpartum = true;

            return new BirthResolutionResult(
                outcome,
                neonatalVigor,
                maternalExhaustion,
                recoveryDays);
        }

        /// <summary>
        /// Calculates daily postpartum recovery permille.
        /// </summary>
        public static int ComputePostpartumRecoveryRate(
            int daysPostpartum,
            int careQualityPermille,
            int nutritionPermille)
        {
            daysPostpartum      = Math.Max(1, daysPostpartum);
            careQualityPermille = Math.Clamp(careQualityPermille, 0, 1000);
            nutritionPermille   = Math.Clamp(nutritionPermille, 0, 1000);

            int baseRate = 30; // 3% per day baseline
            int careBonus = (careQualityPermille * 25) / 1000;
            int nutritionBonus = (nutritionPermille * 25) / 1000;

            // Early days heal slightly faster with intensive rest
            int earlyBonus = daysPostpartum <= 7 ? 10 : 0;

            return Math.Clamp(baseRate + careBonus + nutritionBonus + earlyBonus, 10, 150);
        }
    }
}
