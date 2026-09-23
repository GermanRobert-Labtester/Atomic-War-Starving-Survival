using System;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Type/complexity class of prosthetic limb.
    /// </summary>
    public enum ProstheticComplexityClass
    {
        SimpleImprovised = 0, // Peg, hook, splint: durable, low efficiency cap
        StandardMechanical = 1, // Pinned, cable-driven: moderate efficiency and wear
        AdvancedArticulated = 2  // Precision bionic/jointed: high efficiency, high upkeep
    }

    /// <summary>
    /// Immutable prosthetic wear evaluation result produced by <see cref="ProstheticConditionWearEngine"/>.
    /// </summary>
    public readonly struct ProstheticWearEvaluationResult
    {
        public int NetConditionPermille { get; }
        public int WearDeltaPermille { get; }
        public int BiomechanicalEfficiencyPermille { get; }
        public int FailureRiskPermille { get; }
        public bool RequiresImmediateMaintenance { get; }
        public string MaintenanceStatus { get; }

        public ProstheticWearEvaluationResult(
            int netConditionPermille,
            int wearDeltaPermille,
            int biomechanicalEfficiencyPermille,
            int failureRiskPermille,
            bool requiresImmediateMaintenance,
            string maintenanceStatus)
        {
            NetConditionPermille = netConditionPermille;
            WearDeltaPermille = wearDeltaPermille;
            BiomechanicalEfficiencyPermille = biomechanicalEfficiencyPermille;
            FailureRiskPermille = failureRiskPermille;
            RequiresImmediateMaintenance = requiresImmediateMaintenance;
            MaintenanceStatus = maintenanceStatus ?? string.Empty;
        }
    }

    /// <summary>
    /// Pure domain engine for prosthetic condition wear, calibration efficiency,
    /// and failure risk evaluation (F14-D / UNBLOCK-01).
    /// Integrates with canonical condition math without introducing a duplicate condition store.
    /// </summary>
    public static class ProstheticConditionWearEngine
    {
        public const int PermilleScale = 1000;

        /// <summary>
        /// Evaluates daily prosthetic condition wear, functional efficiency, and mechanical failure risk.
        /// </summary>
        /// <param name="currentConditionPermille">Current item condition (0..1000).</param>
        /// <param name="complexity">Prosthetic complexity tier.</param>
        /// <param name="laborIntensityPermille">Daily physical labor or expedition stress (0..1000).</param>
        /// <param name="maintenanceQualityPermille">Quality of daily cleaning and calibration (0..1000).</param>
        /// <returns>Immutable <see cref="ProstheticWearEvaluationResult"/>.</returns>
        public static ProstheticWearEvaluationResult EvaluateDailyWear(
            int currentConditionPermille,
            ProstheticComplexityClass complexity,
            int laborIntensityPermille,
            int maintenanceQualityPermille)
        {
            currentConditionPermille = Math.Max(0, Math.Min(PermilleScale, currentConditionPermille));
            laborIntensityPermille = Math.Max(0, Math.Min(PermilleScale, laborIntensityPermille));
            maintenanceQualityPermille = Math.Max(0, Math.Min(PermilleScale, maintenanceQualityPermille));

            // Base daily wear depending on complexity
            int baseWear;
            int maxEfficiency;
            switch (complexity)
            {
                case ProstheticComplexityClass.AdvancedArticulated:
                    baseWear = 30; // 3% base wear/day
                    maxEfficiency = 1000;
                    break;
                case ProstheticComplexityClass.StandardMechanical:
                    baseWear = 20; // 2% base wear/day
                    maxEfficiency = 800;
                    break;
                case ProstheticComplexityClass.SimpleImprovised:
                default:
                    baseWear = 10; // 1% base wear/day
                    maxEfficiency = 600;
                    break;
            }

            // Labor intensity increases wear up to +150%
            int intensityMultiplier = PermilleScale + (laborIntensityPermille * 1500) / PermilleScale;
            int rawWear = (baseWear * intensityMultiplier) / PermilleScale;

            // Maintenance reduces wear up to 70%
            int maintenanceMitigation = (maintenanceQualityPermille * 700) / PermilleScale;
            int effectiveWear = Math.Max(1, (rawWear * (PermilleScale - maintenanceMitigation)) / PermilleScale);

            int netCondition = Math.Max(0, currentConditionPermille - effectiveWear);

            // Functional efficiency scales with condition up to complexity cap
            // Condition in optimal range (>= 800) delivers full rated efficiency for its tier.
            // Below 800, efficiency scales down to 50% at 300, and drops sharply below 300.
            int efficiency;
            if (netCondition >= 800)
            {
                efficiency = maxEfficiency;
            }
            else if (netCondition >= 300)
            {
                int span = ((netCondition - 300) * 500) / 500;
                efficiency = (maxEfficiency * (500 + span)) / PermilleScale;
            }
            else
            {
                efficiency = (maxEfficiency * ((netCondition * 500) / 300)) / PermilleScale;
            }

            // Failure risk when condition drops below 300
            int failureRisk = 0;
            if (netCondition < 300)
            {
                failureRisk = ((300 - netCondition) * PermilleScale) / 300;
            }

            bool needsService = netCondition < 350 || failureRisk > 200;

            string status;
            if (netCondition >= 750)
            {
                status = "Optimal alignment and mechanical integrity.";
            }
            else if (netCondition >= 400)
            {
                status = "Serviceable condition with mild joint wear.";
            }
            else if (netCondition >= 150)
            {
                status = "Degraded mechanism; frequent slippage and calibration drift.";
            }
            else
            {
                status = "Imminent mechanical breakdown; structural failure likely.";
            }

            return new ProstheticWearEvaluationResult(
                netConditionPermille: netCondition,
                wearDeltaPermille: effectiveWear,
                biomechanicalEfficiencyPermille: efficiency,
                failureRiskPermille: failureRisk,
                requiresImmediateMaintenance: needsService,
                maintenanceStatus: status
            );
        }
    }
}
