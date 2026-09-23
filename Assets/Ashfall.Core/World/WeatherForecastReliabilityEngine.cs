using System;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Confidence grade of a received weather forecast.
    /// </summary>
    public enum ForecastConfidenceGrade
    {
        High = 0,        // Accurate barometric telemetry; expedition dispatch safe
        Moderate = 1,    // Minor uncertainty; local microclimates may vary
        Speculative = 2, // Significant telemetry noise; storm timing uncertain
        Unusable = 3     // Static and extreme interference; radio report unverified
    }

    /// <summary>
    /// Immutable forecast reliability evaluation result produced by <see cref="WeatherForecastReliabilityEngine"/>.
    /// </summary>
    public readonly struct ForecastReliabilityResult
    {
        public ForecastConfidenceGrade Grade { get; }
        public int ReliabilityScorePermille { get; }
        public int LeadTimeDays { get; }
        public bool IsReliableForDispatch { get; }
        public string ReliabilitySummary { get; }

        public ForecastReliabilityResult(
            ForecastConfidenceGrade grade,
            int reliabilityScorePermille,
            int leadTimeDays,
            bool isReliableForDispatch,
            string reliabilitySummary)
        {
            Grade = grade;
            ReliabilityScorePermille = reliabilityScorePermille;
            LeadTimeDays = leadTimeDays;
            IsReliableForDispatch = isReliableForDispatch;
            ReliabilitySummary = reliabilitySummary ?? string.Empty;
        }
    }

    /// <summary>
    /// Pure domain engine for evaluating weather forecast reliability and radio telemetry decay (D4 / DEC-15 / UNBLOCK-04).
    /// Enforces WeatherStationSystem as single authority while modeling diegetic radio broadcast confidence.
    /// </summary>
    public static class WeatherForecastReliabilityEngine
    {
        public const int PermilleScale = 1000;

        /// <summary>
        /// Evaluates the reliability and confidence grade of a weather forecast received via radio or sensor telemetry.
        /// </summary>
        /// <param name="distanceKmFromStation">Distance from the broadcasting weather station in kilometers.</param>
        /// <param name="leadTimeDays">Forecast horizon in days (1 = tomorrow, 7 = week ahead).</param>
        /// <param name="atmosphericInterferencePermille">Electromagnetic / particulate interference (0..1000).</param>
        /// <param name="stationCalibrationPermille">Station sensor and barometer calibration (0..1000).</param>
        /// <returns>Immutable <see cref="ForecastReliabilityResult"/>.</returns>
        public static ForecastReliabilityResult EvaluateReliability(
            int distanceKmFromStation,
            int leadTimeDays,
            int atmosphericInterferencePermille,
            int stationCalibrationPermille = 800)
        {
            distanceKmFromStation = Math.Max(0, distanceKmFromStation);
            leadTimeDays = Math.Max(1, Math.Min(14, leadTimeDays));
            atmosphericInterferencePermille = Math.Max(0, Math.Min(PermilleScale, atmosphericInterferencePermille));
            stationCalibrationPermille = Math.Max(0, Math.Min(PermilleScale, stationCalibrationPermille));

            // 1. Lead time decay:
            // Day 1: 950 permille base
            // Day 2: 820
            // Day 3: 700
            // Decay accelerates with longer horizons
            int baseAccuracy;
            if (leadTimeDays == 1)
            {
                baseAccuracy = 950;
            }
            else if (leadTimeDays <= 3)
            {
                baseAccuracy = 950 - ((leadTimeDays - 1) * 125);
            }
            else
            {
                baseAccuracy = 700 - ((leadTimeDays - 3) * 90);
            }
            baseAccuracy = Math.Max(100, baseAccuracy);

            // 2. Distance decay: -5 permille per 10 km beyond 20 km
            int distanceDecay = 0;
            if (distanceKmFromStation > 20)
            {
                distanceDecay = ((distanceKmFromStation - 20) * 5) / 10;
            }
            distanceDecay = Math.Min(400, distanceDecay);

            // 3. Atmospheric interference penalty: up to -350 permille
            int interferencePenalty = (atmosphericInterferencePermille * 350) / PermilleScale;

            // 4. Station calibration factor: 800 is baseline. Above adds up to +50, below penalizes up to -200
            int calibrationDelta = ((stationCalibrationPermille - 800) * 250) / PermilleScale;

            int netReliability = baseAccuracy - distanceDecay - interferencePenalty + calibrationDelta;
            netReliability = Math.Max(50, Math.Min(PermilleScale, netReliability));

            // 5. Confidence Grade
            ForecastConfidenceGrade grade;
            bool dispatchSafe;
            string summary;

            if (netReliability >= 800)
            {
                grade = ForecastConfidenceGrade.High;
                dispatchSafe = true;
                summary = "High barometric confidence. Expedition planning and flight windows verified.";
            }
            else if (netReliability >= 600)
            {
                grade = ForecastConfidenceGrade.Moderate;
                dispatchSafe = true;
                summary = "Moderate confidence. Minor storm timing drift possible.";
            }
            else if (netReliability >= 350)
            {
                grade = ForecastConfidenceGrade.Speculative;
                dispatchSafe = false;
                summary = "Speculative forecast. Significant atmospheric noise; unverified weather shifts.";
            }
            else
            {
                grade = ForecastConfidenceGrade.Unusable;
                dispatchSafe = false;
                summary = "Unusable radio telemetry. Severe static or distance attenuation renders data invalid.";
            }

            return new ForecastReliabilityResult(
                grade: grade,
                reliabilityScorePermille: netReliability,
                leadTimeDays: leadTimeDays,
                isReliableForDispatch: dispatchSafe,
                reliabilitySummary: summary
            );
        }
    }
}
