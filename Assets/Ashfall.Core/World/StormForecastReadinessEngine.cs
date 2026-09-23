// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 33 — The Weather
// Subsystem    : Storm Forecast Reliability, Warning Issuance & Seasonal Readiness Engine
// Authority    : docs/expansions/wave5/expansion_33_the_weather_plan.md
//                UNBLOCK-05_EXPANSION_WAVES_C3_EN_GATE.md §3.1, §5.3
// ============================================================================
using System;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Forecast confidence tier based on observation post skill and lead time.
    /// </summary>
    public enum ForecastConfidenceTier
    {
        Unknown      = 0,  // no usable forecast; sky-watch offline
        Unreliable   = 1,  // 0–25% confidence; noise
        Indicative   = 2,  // 26–55% confidence; plan with backup
        Reliable     = 3,  // 56–80% confidence; act on warning
        HighConfidence = 4 // 81–100% confidence; full mobilization
    }

    /// <summary>
    /// Storm severity classification for shelter response planning.
    /// </summary>
    public enum StormSeverityClass
    {
        Clear      = 0,
        Overcast   = 1,
        RainSquall = 2,
        AshStorm   = 3,
        FalloutGale = 4,
        BlackRain  = 5  // worst: chemical/radiological precipitation
    }

    /// <summary>
    /// Seasonal shelter readiness band.
    /// </summary>
    public enum SeasonalReadinessBand
    {
        Unprepared   = 0,  // critical gaps; expect casualties
        Marginal     = 1,  // most gaps covered; some risk
        Adequate     = 2,  // baseline readiness; handles routine storms
        WellPrepared = 3,  // hardened; handles severe events
        Fortified    = 4   // optimal; absorbs black rain without loss
    }

    /// <summary>
    /// Immutable result of a storm forecast evaluation.
    /// </summary>
    public readonly struct StormForecastResult
    {
        /// <summary>Forecast confidence (0..1000 permille).</summary>
        public int ConfidencePermille        { get; }
        public ForecastConfidenceTier ConfidenceTier { get; }
        /// <summary>Predicted storm severity class.</summary>
        public StormSeverityClass PredictedSeverity { get; }
        /// <summary>Lead time hours the warning system provides.</summary>
        public int LeadTimeHours             { get; }
        /// <summary>True if a public shelter warning should be issued.</summary>
        public bool IssueWarning             { get; }

        public StormForecastResult(
            int confidencePermille,
            ForecastConfidenceTier confidenceTier,
            StormSeverityClass predictedSeverity,
            int leadTimeHours,
            bool issueWarning)
        {
            ConfidencePermille = Math.Clamp(confidencePermille, 0, 1000);
            ConfidenceTier     = confidenceTier;
            PredictedSeverity  = predictedSeverity;
            LeadTimeHours      = Math.Max(0, leadTimeHours);
            IssueWarning       = issueWarning;
        }
    }

    /// <summary>
    /// Immutable result of a seasonal readiness assessment.
    /// </summary>
    public readonly struct SeasonalReadinessResult
    {
        public SeasonalReadinessBand ReadinessBand { get; }
        /// <summary>Composite readiness score (0..1000 permille).</summary>
        public int ReadinessPermille         { get; }
        /// <summary>True if the shelter can absorb a BlackRain event without mass casualty.</summary>
        public bool CanAbsorbBlackRain       { get; }
        /// <summary>Key gap description (empty if WellPrepared or above).</summary>
        public string PrimaryGap             { get; }

        public SeasonalReadinessResult(
            SeasonalReadinessBand readinessBand,
            int readinessPermille,
            bool canAbsorbBlackRain,
            string primaryGap)
        {
            ReadinessBand     = readinessBand;
            ReadinessPermille = Math.Clamp(readinessPermille, 0, 1000);
            CanAbsorbBlackRain = canAbsorbBlackRain;
            PrimaryGap        = primaryGap ?? string.Empty;
        }
    }

    /// <summary>
    /// Pure domain engine governing storm forecast reliability (observation post skill × lead time),
    /// warning confidence tiers, black-rain response preparation, ash-season readiness,
    /// and weather record almanac contribution.
    /// Extends WeatherSystem (WorldWeatherState, season profiles) and WeatherGate
    /// without duplicating weather authority.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class StormForecastReadinessEngine
    {
        /// <summary>Lead time hours above which forecast skill begins to decay meaningfully.</summary>
        public const int ForecastDecayOnsetHours = 12;

        /// <summary>Confidence permille threshold above which a public warning is issued.</summary>
        public const int WarningIssuanceThreshold = 560; // Reliable tier floor

        /// <summary>Readiness permille required to absorb a BlackRain event.</summary>
        public const int BlackRainAbsorptionThreshold = 800;

        /// <summary>
        /// Evaluates storm forecast reliability for a given lead time and observation post capability.
        /// </summary>
        /// <param name="observationSkillPermille">
        ///     Observation post operator skill and instrument quality (0..1000).
        /// </param>
        /// <param name="leadTimeHours">
        ///     Hours of advance notice being requested (longer = less reliable).
        /// </param>
        /// <param name="stormSeverity">Expected storm class being forecast.</param>
        /// <param name="forecastSeed">Deterministic seed for confidence variance.</param>
        public static StormForecastResult EvaluateForecastReliability(
            int observationSkillPermille,
            int leadTimeHours,
            StormSeverityClass stormSeverity,
            int forecastSeed)
        {
            observationSkillPermille = Math.Clamp(observationSkillPermille, 0, 1000);
            leadTimeHours            = Math.Max(0, leadTimeHours);

            if (observationSkillPermille < 100)
            {
                return new StormForecastResult(0, ForecastConfidenceTier.Unknown,
                    stormSeverity, 0, false);
            }

            // Lead-time decay: each hour beyond onset halves confidence by ~3 permille
            int decayHours = Math.Max(0, leadTimeHours - ForecastDecayOnsetHours);
            int decayFactor = Math.Min(800, decayHours * 28);
            int baseConfidence = Math.Max(0, observationSkillPermille - decayFactor);

            // Severe weather is harder to pin down accurately
            int severityPenalty = stormSeverity switch
            {
                StormSeverityClass.Clear       => 0,
                StormSeverityClass.Overcast     => 20,
                StormSeverityClass.RainSquall   => 50,
                StormSeverityClass.AshStorm     => 100,
                StormSeverityClass.FalloutGale  => 150,
                StormSeverityClass.BlackRain    => 200,
                _                               => 80
            };
            baseConfidence = Math.Max(0, baseConfidence - severityPenalty);

            // Deterministic variance ±5%
            int hash      = HashCode.Combine(forecastSeed, leadTimeHours, (int)stormSeverity);
            int variance  = ((hash & 0x7FFFFFFF) % 11) - 5; // -5..+5
            int confidence = Math.Clamp(baseConfidence + (baseConfidence * variance) / 100, 0, 1000);

            ForecastConfidenceTier tier = confidence switch
            {
                0        => ForecastConfidenceTier.Unknown,
                <= 250   => ForecastConfidenceTier.Unreliable,
                <= 550   => ForecastConfidenceTier.Indicative,
                <= 800   => ForecastConfidenceTier.Reliable,
                _        => ForecastConfidenceTier.HighConfidence
            };

            // Effective lead time degrades with skill
            int effectiveLeadTime = (leadTimeHours * observationSkillPermille) / 1000;
            bool issueWarning = confidence >= WarningIssuanceThreshold &&
                                stormSeverity >= StormSeverityClass.RainSquall;

            return new StormForecastResult(confidence, tier, stormSeverity, effectiveLeadTime, issueWarning);
        }

        /// <summary>
        /// Assesses the shelter's readiness to absorb a given storm severity class.
        /// </summary>
        /// <param name="sealedAirlockPermille">Airlock and shelter seal integrity (0..1000).</param>
        /// <param name="filterStockPermille">Air filter and respirator stock level (0..1000).</param>
        /// <param name="medicalReadinessPermille">Medical bay capacity for storm casualties (0..1000).</param>
        /// <param name="drillRecencyPermille">
        ///     How recently a storm-response drill was run (0..1000; decays over time).
        /// </param>
        /// <param name="targetSeverity">Storm class the shelter is being assessed against.</param>
        public static SeasonalReadinessResult AssessSeasonalReadiness(
            int sealedAirlockPermille,
            int filterStockPermille,
            int medicalReadinessPermille,
            int drillRecencyPermille,
            StormSeverityClass targetSeverity)
        {
            sealedAirlockPermille    = Math.Clamp(sealedAirlockPermille, 0, 1000);
            filterStockPermille      = Math.Clamp(filterStockPermille, 0, 1000);
            medicalReadinessPermille = Math.Clamp(medicalReadinessPermille, 0, 1000);
            drillRecencyPermille     = Math.Clamp(drillRecencyPermille, 0, 1000);

            // Weighted composite score
            int composite = (sealedAirlockPermille * 30 +
                             filterStockPermille    * 30 +
                             medicalReadinessPermille * 25 +
                             drillRecencyPermille   * 15) / 100;

            // Harder storms demand more
            int severityRequirement = targetSeverity switch
            {
                StormSeverityClass.Clear       => 0,
                StormSeverityClass.Overcast     => 100,
                StormSeverityClass.RainSquall   => 300,
                StormSeverityClass.AshStorm     => 500,
                StormSeverityClass.FalloutGale  => 700,
                StormSeverityClass.BlackRain    => 850,
                _                               => 400
            };

            int adjustedScore = Math.Max(0, composite - severityRequirement / 5);
            adjustedScore     = Math.Clamp(adjustedScore, 0, 1000);

            SeasonalReadinessBand band = adjustedScore switch
            {
                <= 100 => SeasonalReadinessBand.Unprepared,
                <= 350 => SeasonalReadinessBand.Marginal,
                <= 600 => SeasonalReadinessBand.Adequate,
                <= 800 => SeasonalReadinessBand.WellPrepared,
                _      => SeasonalReadinessBand.Fortified
            };

            bool canAbsorbBlackRain = composite >= BlackRainAbsorptionThreshold &&
                                      targetSeverity == StormSeverityClass.BlackRain;

            // Identify primary gap
            string primaryGap = string.Empty;
            if (band < SeasonalReadinessBand.WellPrepared)
            {
                if (sealedAirlockPermille < 400)
                    primaryGap = "Airlock seals critically deficient";
                else if (filterStockPermille < 400)
                    primaryGap = "Filter and respirator stock depleted";
                else if (drillRecencyPermille < 200)
                    primaryGap = "Storm-response drills overdue";
                else if (medicalReadinessPermille < 400)
                    primaryGap = "Medical bay capacity insufficient for storm casualties";
            }

            return new SeasonalReadinessResult(band, adjustedScore, canAbsorbBlackRain, primaryGap);
        }

        /// <summary>
        /// Computes the forecast skill decay rate per day for an unmanned observation post.
        /// Used by the host to tick instrument calibration drift.
        /// </summary>
        /// <param name="instrumentQualityPermille">Quality of installed weather instruments (0..1000).</param>
        /// <returns>Daily skill decay permille (subtract from observationSkillPermille each day unmanned).</returns>
        public static int CalculateObservationPostDecay(int instrumentQualityPermille)
        {
            instrumentQualityPermille = Math.Clamp(instrumentQualityPermille, 0, 1000);
            // Better instruments decay slower (self-calibrating); basic instruments drift fast
            return Math.Max(5, 80 - (instrumentQualityPermille * 70) / 1000);
        }
    }
}
