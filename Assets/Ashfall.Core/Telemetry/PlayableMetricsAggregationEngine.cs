using System;

namespace Ashfall.Core.Telemetry
{
    /// <summary>
    /// Performance and survival readiness evaluation grade.
    /// </summary>
    public enum SessionReadinessGrade
    {
        A_Exemplary = 0,
        B_Resilient = 1,
        C_Strained = 2,
        D_Critical = 3,
        F_Collapsed = 4
    }

    /// <summary>
    /// Raw session metric inputs collected over a campaign run.
    /// Strictly anonymous and privacy-safe.
    /// </summary>
    public readonly struct SessionMetricInputs
    {
        public int DaysSurvived { get; }
        public int PeakPopulation { get; }
        public int CasualtiesCount { get; }
        public int TotalScavengeSorties { get; }
        public int TotalResourcesHarvested { get; }
        public int TotalWaterPurifiedLiters { get; }
        public int CrisesResolved { get; }
        public int CrisesFailed { get; }
        public int DifficultyScalarPermille { get; } // 1000 = standard 1.0x

        public SessionMetricInputs(
            int daysSurvived,
            int peakPopulation,
            int casualtiesCount,
            int totalScavengeSorties,
            int totalResourcesHarvested,
            int totalWaterPurifiedLiters,
            int crisesResolved,
            int crisesFailed,
            int difficultyScalarPermille = 1000)
        {
            DaysSurvived = Math.Max(0, daysSurvived);
            PeakPopulation = Math.Max(1, peakPopulation);
            CasualtiesCount = Math.Max(0, casualtiesCount);
            TotalScavengeSorties = Math.Max(0, totalScavengeSorties);
            TotalResourcesHarvested = Math.Max(0, totalResourcesHarvested);
            TotalWaterPurifiedLiters = Math.Max(0, totalWaterPurifiedLiters);
            CrisesResolved = Math.Max(0, crisesResolved);
            CrisesFailed = Math.Max(0, crisesFailed);
            DifficultyScalarPermille = Math.Max(500, Math.Min(3000, difficultyScalarPermille));
        }
    }

    /// <summary>
    /// Immutable aggregated session telemetry evaluation result produced by <see cref="PlayableMetricsAggregationEngine"/>.
    /// </summary>
    public readonly struct AggregatedMetricsResult
    {
        public SessionReadinessGrade Grade { get; }
        public int HardshipIndexPermille { get; }
        public int EfficiencyRatingPermille { get; }
        public int SurvivalStabilityScorePermille { get; }
        public bool HasCriticalFailure { get; }
        public string SummaryDescription { get; }

        public AggregatedMetricsResult(
            SessionReadinessGrade grade,
            int hardshipIndexPermille,
            int efficiencyRatingPermille,
            int survivalStabilityScorePermille,
            bool hasCriticalFailure,
            string summaryDescription)
        {
            Grade = grade;
            HardshipIndexPermille = hardshipIndexPermille;
            EfficiencyRatingPermille = efficiencyRatingPermille;
            SurvivalStabilityScorePermille = survivalStabilityScorePermille;
            HasCriticalFailure = hasCriticalFailure;
            SummaryDescription = summaryDescription ?? string.Empty;
        }
    }

    /// <summary>
    /// Pure domain engine for aggregating playable telemetry metrics and campaign difficulty ratings (Plan 46 / C2[20]).
    /// Operates with zero network dependencies, strictly anonymous keyed counters, and deterministic scoring.
    /// </summary>
    public static class PlayableMetricsAggregationEngine
    {
        public const int PermilleScale = 1000;

        /// <summary>
        /// Evaluates aggregated session performance, survival stability, and hardship rating.
        /// </summary>
        /// <param name="inputs">Raw session metric counters.</param>
        /// <returns>Immutable <see cref="AggregatedMetricsResult"/>.</returns>
        public static AggregatedMetricsResult Evaluate(SessionMetricInputs inputs)
        {
            // 1. Hardship index permille:
            // Scaled by difficulty modifier, crises faced, and days survived
            int totalCrises = inputs.CrisesResolved + inputs.CrisesFailed;
            int crisisRate = inputs.DaysSurvived > 0 ? (totalCrises * PermilleScale) / inputs.DaysSurvived : 0;
            int hardship = (inputs.DifficultyScalarPermille * 500 + crisisRate * 500) / PermilleScale;
            hardship = Math.Min(PermilleScale * 2, hardship);

            // 2. Efficiency rating permille:
            // Resources + water produced per survivor-day
            int survivorDays = inputs.DaysSurvived * inputs.PeakPopulation;
            int efficiency = 0;
            if (survivorDays > 0)
            {
                long totalProduction = (long)inputs.TotalResourcesHarvested + (inputs.TotalWaterPurifiedLiters / 2);
                efficiency = (int)Math.Min(PermilleScale * 3, (totalProduction * PermilleScale) / survivorDays);
            }

            // 3. Survival stability score permille:
            // Base 1000, penalized heavily by casualties and failed crises, boosted by successful crises
            int casualtyRatePermille = (inputs.CasualtiesCount * PermilleScale) / inputs.PeakPopulation;
            int casualtyPenalty = casualtyRatePermille * 2; // e.g. 50% casualties = -1000

            int crisisPenalty = inputs.CrisesFailed * 150;
            int crisisBonus = inputs.CrisesResolved * 50;

            int stability = PermilleScale - casualtyPenalty - crisisPenalty + crisisBonus;
            stability = Math.Max(0, Math.Min(PermilleScale, stability));

            // 4. Critical failure check
            bool criticalFailure = casualtyRatePermille >= 600 || (inputs.DaysSurvived < 3 && inputs.CasualtiesCount > 0);

            // 5. Readiness Grade
            SessionReadinessGrade grade;
            if (criticalFailure || stability < 200)
            {
                grade = SessionReadinessGrade.F_Collapsed;
            }
            else if (stability < 450)
            {
                grade = SessionReadinessGrade.D_Critical;
            }
            else if (stability < 700)
            {
                grade = SessionReadinessGrade.C_Strained;
            }
            else if (stability < 900)
            {
                grade = SessionReadinessGrade.B_Resilient;
            }
            else
            {
                grade = SessionReadinessGrade.A_Exemplary;
            }

            string summary;
            switch (grade)
            {
                case SessionReadinessGrade.A_Exemplary:
                    summary = "Exemplary shelter cohesion. Near-zero casualties with abundant logistics buffer.";
                    break;
                case SessionReadinessGrade.B_Resilient:
                    summary = "Resilient campaign performance. Stable resource flow and managed casualty rate.";
                    break;
                case SessionReadinessGrade.C_Strained:
                    summary = "Strained survival conditions. Vulnerable to compounding crises.";
                    break;
                case SessionReadinessGrade.D_Critical:
                    summary = "Critical operational degradation. Elevated casualties and severe supply deficits.";
                    break;
                case SessionReadinessGrade.F_Collapsed:
                default:
                    summary = "Catastrophic collapse. Shelter population decimated or unmanageable crisis cascade.";
                    break;
            }

            return new AggregatedMetricsResult(
                grade: grade,
                hardshipIndexPermille: hardship,
                efficiencyRatingPermille: efficiency,
                survivalStabilityScorePermille: stability,
                hasCriticalFailure: criticalFailure,
                summaryDescription: summary
            );
        }
    }
}
