using System;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Type of emergency muster drill conducted.
    /// </summary>
    public enum EmergencyDrillType
    {
        None = 0,
        TabletopWalkthrough = 1,     // Low stress, minor readiness improvement
        FullAlarmEvacuation = 2,     // High fidelity, verifies route timing, moderate fatigue
        FireSuppressionBrigade = 3,  // Equipment & extinguisher readiness, smoke clearance
        ToxicBreachLockdown = 4      // Hatch seals, overpressure checks, refuge chamber muster
    }

    /// <summary>
    /// Immutable readiness evaluation result produced by <see cref="EmergencyMusterReadinessEngine"/>.
    /// </summary>
    public readonly struct MusterReadinessEvaluationResult
    {
        public int CompositeReadinessScorePermille { get; }
        public int EstimatedEvacuationMinutes { get; }
        public int MissingSurvivorRiskPermille { get; }
        public int CascadeInterventionMarginMinutes { get; }
        public int ComplianceFatiguePermille { get; }
        public bool IsReadinessCertified { get; }
        public string ReadinessAdvisory { get; }

        public MusterReadinessEvaluationResult(
            int compositeReadinessScorePermille,
            int estimatedEvacuationMinutes,
            int missingSurvivorRiskPermille,
            int cascadeInterventionMarginMinutes,
            int complianceFatiguePermille,
            bool isReadinessCertified,
            string readinessAdvisory)
        {
            CompositeReadinessScorePermille = compositeReadinessScorePermille;
            EstimatedEvacuationMinutes = estimatedEvacuationMinutes;
            MissingSurvivorRiskPermille = missingSurvivorRiskPermille;
            CascadeInterventionMarginMinutes = cascadeInterventionMarginMinutes;
            ComplianceFatiguePermille = complianceFatiguePermille;
            IsReadinessCertified = isReadinessCertified;
            ReadinessAdvisory = readinessAdvisory ?? string.Empty;
        }
    }

    /// <summary>
    /// Pure domain engine for emergency muster readiness, drill fatigue, evacuation timing,
    /// and cascade intervention margins (Expansion 23: The Alarm).
    /// Operates without engine dependencies or duplicate shelter fire/cascade stores.
    /// </summary>
    public static class EmergencyMusterReadinessEngine
    {
        public const int PermilleScale = 1000;
        public const int OptimalWardenRatioPeople = 10; // 1 warden per 10 people

        /// <summary>
        /// Evaluates emergency readiness, evacuation speed, and cascade intervention buffer.
        /// </summary>
        /// <param name="populationCount">Total survivors in shelter.</param>
        /// <param name="activeWardenCount">Trained emergency wardens assigned to muster duty.</param>
        /// <param name="daysSinceLastDrill">Days elapsed since the most recent drill.</param>
        /// <param name="lastDrillType">Type of the last conducted drill.</param>
        /// <param name="routeClearancePermille">Corridor and egress route clearance (0..1000).</param>
        /// <param name="refugeChamberIntegrityPermille">Refuge chamber seals, air, and rations (0..1000).</param>
        /// <param name="recentDrillCountIn14Days">Drills conducted in the last 14 days (fatigue driver).</param>
        /// <returns>Immutable <see cref="MusterReadinessEvaluationResult"/>.</returns>
        public static MusterReadinessEvaluationResult Evaluate(
            int populationCount,
            int activeWardenCount,
            int daysSinceLastDrill,
            EmergencyDrillType lastDrillType,
            int routeClearancePermille,
            int refugeChamberIntegrityPermille,
            int recentDrillCountIn14Days = 0)
        {
            if (populationCount < 1) populationCount = 1;
            activeWardenCount = Math.Max(0, activeWardenCount);
            daysSinceLastDrill = Math.Max(0, daysSinceLastDrill);
            routeClearancePermille = Math.Max(0, Math.Min(PermilleScale, routeClearancePermille));
            refugeChamberIntegrityPermille = Math.Max(0, Math.Min(PermilleScale, refugeChamberIntegrityPermille));
            recentDrillCountIn14Days = Math.Max(0, recentDrillCountIn14Days);

            // 1. Warden coverage factor (permille)
            int requiredWardens = Math.Max(1, (populationCount + (OptimalWardenRatioPeople - 1)) / OptimalWardenRatioPeople);
            int wardenCoveragePermille = Math.Min(PermilleScale, (activeWardenCount * PermilleScale) / requiredWardens);

            // 2. Drill recency and quality baseline
            int drillBaseScore;
            switch (lastDrillType)
            {
                case EmergencyDrillType.FullAlarmEvacuation:
                    drillBaseScore = 850;
                    break;
                case EmergencyDrillType.ToxicBreachLockdown:
                    drillBaseScore = 800;
                    break;
                case EmergencyDrillType.FireSuppressionBrigade:
                    drillBaseScore = 750;
                    break;
                case EmergencyDrillType.TabletopWalkthrough:
                    drillBaseScore = 500;
                    break;
                case EmergencyDrillType.None:
                default:
                    drillBaseScore = 150; // Untrained population
                    break;
            }

            // Drill decay over days: -10 permille per day past day 7, accelerating past day 30
            int drillDecay = 0;
            if (daysSinceLastDrill > 7)
            {
                drillDecay = (daysSinceLastDrill - 7) * 10;
                if (daysSinceLastDrill > 30)
                {
                    drillDecay += (daysSinceLastDrill - 30) * 15;
                }
            }
            int effectiveDrillScore = Math.Max(100, drillBaseScore - drillDecay);

            // 3. Compliance fatigue from over-drilling (>2 drills in 14 days causes annoyance and complacency)
            int fatigue = 0;
            if (recentDrillCountIn14Days > 2)
            {
                fatigue = Math.Min(600, (recentDrillCountIn14Days - 2) * 150);
            }

            // 4. Composite readiness score
            // 35% drill training, 25% warden coverage, 20% route clearance, 20% refuge integrity, minus fatigue
            int compositeScore = (effectiveDrillScore * 350 +
                                  wardenCoveragePermille * 250 +
                                  routeClearancePermille * 200 +
                                  refugeChamberIntegrityPermille * 200) / PermilleScale;

            compositeScore = Math.Max(0, compositeScore - fatigue);
            compositeScore = Math.Min(PermilleScale, compositeScore);

            // 5. Estimated evacuation time in minutes
            // Base time: 4 minutes. Poor routes, low wardens, and low drill add minutes.
            int routeDelay = ((PermilleScale - routeClearancePermille) * 10) / PermilleScale; // up to 10 min
            int wardenDelay = ((PermilleScale - wardenCoveragePermille) * 8) / PermilleScale; // up to 8 min
            int trainingDelay = ((PermilleScale - effectiveDrillScore) * 6) / PermilleScale;  // up to 6 min
            int evacMinutes = 4 + routeDelay + wardenDelay + trainingDelay;

            // 6. Missing survivor risk permille
            // Unaccounted survivors during emergency: driven by low wardens and low route clearance
            int missingRisk = ((PermilleScale - wardenCoveragePermille) * 500 +
                               (PermilleScale - routeClearancePermille) * 350 +
                               (PermilleScale - effectiveDrillScore) * 150) / PermilleScale;
            missingRisk = Math.Max(10, Math.Min(PermilleScale, missingRisk));

            // 7. Cascade intervention margin minutes
            // Time window before a cascade failure becomes irreversible (e.g. fire reaches fuel, or flood reaches substation)
            // Baseline 15 minutes, modified by composite readiness
            int interventionMargin = 5 + (compositeScore * 20) / PermilleScale; // 5 to 25 minutes

            bool isCertified = (compositeScore >= 650) && (activeWardenCount >= requiredWardens / 2);

            string advisory;
            if (compositeScore >= 800)
            {
                advisory = "High emergency readiness. Rapid evacuation and swift muster accountability assured.";
            }
            else if (compositeScore >= 600)
            {
                advisory = "Standard readiness. Evacuation routes passable; minor muster delays expected.";
            }
            else if (compositeScore >= 350)
            {
                advisory = "Substandard readiness. High probability of corridor congestion and missing persons.";
            }
            else
            {
                advisory = "Critical emergency deficit. Evacuation pathways blocked or population untrained.";
            }

            return new MusterReadinessEvaluationResult(
                compositeReadinessScorePermille: compositeScore,
                estimatedEvacuationMinutes: evacMinutes,
                missingSurvivorRiskPermille: missingRisk,
                cascadeInterventionMarginMinutes: interventionMargin,
                complianceFatiguePermille: fatigue,
                isReadinessCertified: isCertified,
                readinessAdvisory: advisory
            );
        }
    }
}
