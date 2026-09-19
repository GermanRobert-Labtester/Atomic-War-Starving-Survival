// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.World;

namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// Classes of existential crises that can be projected from authoritative read models.
    /// </summary>
    public enum CrisisClass
    {
        FoodDepletion = 0,
        WaterDepletion = 1,
        PowerBlackout = 2,
        RadiationSpike = 3,
        DiseaseOutbreak = 4,
        DistressAmbush = 5,
        DemographicCollapse = 6,
        WeatherExtreme = 7
    }

    /// <summary>
    /// Qualitative confidence band for player-facing readouts.
    /// </summary>
    public enum CrisisConfidenceBand
    {
        Low = 0,
        Moderate = 1,
        High = 2
    }

    /// <summary>
    /// Pure, immutable projection record for a predicted campaign crisis.
    /// Derivable from domain state; never persisted.
    /// </summary>
    public sealed class CrisisPredictionRecord
    {
        public CrisisClass Kind { get; }
        public int ProjectedDay { get; }
        public int HorizonDays { get; }
        public float Confidence { get; }
        public CrisisConfidenceBand ConfidenceBand { get; }
        public string ReasonId { get; }
        public string PreparationAdvice { get; }
        public float SupportingMetric { get; }

        public CrisisPredictionRecord(
            CrisisClass kind,
            int projectedDay,
            int horizonDays,
            float confidence,
            CrisisConfidenceBand confidenceBand,
            string reasonId,
            string preparationAdvice,
            float supportingMetric = 0f)
        {
            Kind = kind;
            ProjectedDay = projectedDay;
            HorizonDays = Math.Max(0, horizonDays);
            Confidence = Math.Clamp(confidence, 0f, 1f);
            ConfidenceBand = confidenceBand;
            ReasonId = reasonId ?? string.Empty;
            PreparationAdvice = preparationAdvice ?? string.Empty;
            SupportingMetric = supportingMetric;
        }
    }

    /// <summary>
    /// Read-only snapshot of inputs passed to the pure crisis predictor.
    /// All values default to neutral/healthy states so missing inputs never fail.
    /// </summary>
    public sealed class CrisisPredictionInputs
    {
        public int CurrentDay { get; set; } = 1;
        public int LivingSurvivorCount { get; set; } = 4;

        // Food & Water
        public float FoodStockUnits { get; set; } = 50f;
        public float DailyFoodBurnRate { get; set; } = 4f;
        public float WaterStockUnits { get; set; } = 50f;
        public float DailyWaterBurnRate { get; set; } = 4f;

        // Power
        public float BatteryReserveWh { get; set; } = 10000f;
        public float BatteryCapacityWh { get; set; } = 10000f;
        public float GenerationWatts { get; set; } = 500f;
        public float TotalDrawWatts { get; set; } = 300f;
        public float FuelUnits { get; set; } = 50f;
        public float FuelRunwayDays { get; set; } = 10f;
        /// <summary>Campaign difficulty multiplier for predicted deadlines.</summary>
        public float DeadlineMultiplier { get; set; } = 1f;

        // Radiation & Sanitation
        public float AverageRadiationDose { get; set; } = 0f;
        public float MaxRadiationDose { get; set; } = 0f;
        public float PathogenExposureModifier { get; set; } = 1f;
        public int ActiveSanitationSpills { get; set; } = 0;

        // Distress & Narrative
        public int PendingHighRiskDistressFollowUps { get; set; } = 0;
        public int NextDistressFollowUpDay { get; set; } = 0;

        // Cohort & Demographics
        public int WorkingAdultCount { get; set; } = 4;
        public int DependentCount { get; set; } = 0;

        // Weather Forecast
        public IReadOnlyList<ForecastEntry>? Forecast { get; set; }
        public float StationAccuracy { get; set; } = 0.8f;
        public bool IsStationCalibrated { get; set; } = true;
        public bool IsStationOperational { get; set; } = true;
    }

    /// <summary>
    /// Pure, deterministic crisis predictor.
    /// Zero RNG, zero mutation, zero persistence. Evaluates authoritative read-only inputs
    /// and classifies impending crises with bounded confidence and stable ordering.
    /// </summary>
    public static class CrisisPredictor
    {
        public const float FoodWarningRunwayDays = 5f;
        public const float WaterWarningRunwayDays = 5f;
        public const float PowerFuelWarningRunwayDays = 3f;
        public const float SevereRadThreshold = 60f;
        public const float CriticalRadThreshold = 80f;
        public const float SeverePathogenThreshold = 1.4f;

        public static IReadOnlyList<CrisisPredictionRecord> Evaluate(CrisisPredictionInputs inputs)
        {
            if (inputs == null)
                return Array.Empty<CrisisPredictionRecord>();

            var predictions = new List<CrisisPredictionRecord>();
            int currentDay = Math.Max(1, inputs.CurrentDay);
            int livingSurvivors = Math.Max(0, inputs.LivingSurvivorCount);
            float deadlineMultiplier = NormalizeDeadlineMultiplier(inputs.DeadlineMultiplier);

            // 1. Food Trajectory (only relevant if roster is alive)
            if (livingSurvivors > 0)
            {
                float burn = inputs.DailyFoodBurnRate > 0f ? inputs.DailyFoodBurnRate : (float)livingSurvivors;
                float stock = Math.Max(0f, inputs.FoodStockUnits);
                float runway = stock / burn;
                float effectiveRunway = runway * deadlineMultiplier;

                if (effectiveRunway <= FoodWarningRunwayDays)
                {
                    int horizon = (int)Math.Floor(effectiveRunway);
                    int projectedDay = currentDay + horizon;
                    float confidence = Math.Clamp(1.0f - (effectiveRunway / (FoodWarningRunwayDays + 1f)) * 0.4f, 0.6f, 1.0f);
                    CrisisConfidenceBand band = confidence >= 0.8f ? CrisisConfidenceBand.High : CrisisConfidenceBand.Moderate;

                    predictions.Add(new CrisisPredictionRecord(
                        CrisisClass.FoodDepletion,
                        projectedDay,
                        horizon,
                        confidence,
                        band,
                        "crisis.reason.food_stock_exhaustion",
                        "Stock non-perishable rations or activate secondary greenhouse crops immediately.",
                        effectiveRunway));
                }
            }

            // 2. Water Trajectory (only relevant if roster is alive)
            if (livingSurvivors > 0)
            {
                float burn = inputs.DailyWaterBurnRate > 0f ? inputs.DailyWaterBurnRate : (float)livingSurvivors;
                float stock = Math.Max(0f, inputs.WaterStockUnits);
                float runway = stock / burn;
                float effectiveRunway = runway * deadlineMultiplier;

                if (effectiveRunway <= WaterWarningRunwayDays)
                {
                    int horizon = (int)Math.Floor(effectiveRunway);
                    int projectedDay = currentDay + horizon;
                    float confidence = Math.Clamp(1.0f - (effectiveRunway / (WaterWarningRunwayDays + 1f)) * 0.4f, 0.6f, 1.0f);
                    CrisisConfidenceBand band = confidence >= 0.8f ? CrisisConfidenceBand.High : CrisisConfidenceBand.Moderate;

                    predictions.Add(new CrisisPredictionRecord(
                        CrisisClass.WaterDepletion,
                        projectedDay,
                        horizon,
                        confidence,
                        band,
                        "crisis.reason.water_stock_exhaustion",
                        "Ration drinking water and service filtration or condensation units.",
                        effectiveRunway));
                }
            }

            // 3. Power Grid Runaway / Blackout
            float netWatts = inputs.GenerationWatts - inputs.TotalDrawWatts;
            if (inputs.FuelRunwayDays <= PowerFuelWarningRunwayDays || (netWatts < 0f && inputs.BatteryReserveWh <= 0f))
            {
                float runway = Math.Max(0f, inputs.FuelRunwayDays);
                float effectiveRunway = runway * deadlineMultiplier;
                int horizon = (int)Math.Floor(effectiveRunway);
                int projectedDay = currentDay + horizon;
                float confidence = runway <= 1f ? 0.95f : 0.75f;
                CrisisConfidenceBand band = confidence >= 0.8f ? CrisisConfidenceBand.High : CrisisConfidenceBand.Moderate;

                predictions.Add(new CrisisPredictionRecord(
                    CrisisClass.PowerBlackout,
                    projectedDay,
                    horizon,
                    confidence,
                    band,
                    "crisis.reason.power_fuel_depletion",
                    "Acquire generator fuel or shed non-critical room subgrids.",
                    effectiveRunway));
            }

            // 4. Radiation Hazard
            if (inputs.MaxRadiationDose >= SevereRadThreshold || inputs.AverageRadiationDose >= SevereRadThreshold)
            {
                int horizon = ScaleDeadlineHorizon(
                    inputs.MaxRadiationDose >= CriticalRadThreshold ? 1 : 2,
                    deadlineMultiplier);
                int projectedDay = currentDay + horizon;
                float confidence = Math.Clamp(inputs.MaxRadiationDose / 100f, 0.65f, 0.99f);
                CrisisConfidenceBand band = confidence >= 0.8f ? CrisisConfidenceBand.High : CrisisConfidenceBand.Moderate;

                predictions.Add(new CrisisPredictionRecord(
                    CrisisClass.RadiationSpike,
                    projectedDay,
                    horizon,
                    confidence,
                    band,
                    "crisis.reason.radiation_critical_exposure",
                    "Administer anti-rad pharmaceuticals and reinforce airlock shielding.",
                    inputs.MaxRadiationDose));
            }

            // 5. Disease & Sanitation Spill Surge
            if (inputs.ActiveSanitationSpills > 0 || inputs.PathogenExposureModifier >= SeverePathogenThreshold)
            {
                int horizon = ScaleDeadlineHorizon(
                    inputs.ActiveSanitationSpills > 1 ? 1 : 2,
                    deadlineMultiplier);
                int projectedDay = currentDay + horizon;
                float confidence = inputs.ActiveSanitationSpills > 0 ? 0.85f : 0.7f;
                CrisisConfidenceBand band = confidence >= 0.8f ? CrisisConfidenceBand.High : CrisisConfidenceBand.Moderate;

                predictions.Add(new CrisisPredictionRecord(
                    CrisisClass.DiseaseOutbreak,
                    projectedDay,
                    horizon,
                    confidence,
                    band,
                    "crisis.reason.sanitation_pathogen_surge",
                    "Clear contaminated spills and quarantine symptomatic survivors.",
                    inputs.PathogenExposureModifier));
            }

            // 6. Distress Signal Ambush / Trap
            if (inputs.PendingHighRiskDistressFollowUps > 0 && inputs.NextDistressFollowUpDay >= currentDay)
            {
                int horizon = ScaleDeadlineHorizon(
                    Math.Max(0, inputs.NextDistressFollowUpDay - currentDay),
                    deadlineMultiplier);
                predictions.Add(new CrisisPredictionRecord(
                    CrisisClass.DistressAmbush,
                    inputs.NextDistressFollowUpDay,
                    horizon,
                    0.80f,
                    CrisisConfidenceBand.High,
                    "crisis.reason.distress_hostile_intercept",
                    "Arm perimeter security and hold expeditions until frequency clears.",
                    inputs.PendingHighRiskDistressFollowUps));
            }

            // 7. Demographic Collapse (Zero working adults while living crew remains)
            if (livingSurvivors > 0 && inputs.WorkingAdultCount <= 0)
            {
                predictions.Add(new CrisisPredictionRecord(
                    CrisisClass.DemographicCollapse,
                    currentDay,
                    0,
                    0.99f,
                    CrisisConfidenceBand.High,
                    "crisis.reason.zero_working_adults",
                    "Incapacitated or child-only roster cannot maintain shelter operations.",
                    inputs.DependentCount));
            }

            // 8. Weather Extreme Forecast
            if (inputs.IsStationOperational && inputs.Forecast != null && inputs.Forecast.Count > 0)
            {
                foreach (var f in inputs.Forecast)
                {
                    if (IsSevereWeather(f.weather) && f.day >= currentDay)
                    {
                        int distance = Math.Max(1, f.day - currentDay);
                        float calibrationBonus = inputs.IsStationCalibrated ? 0.15f : 0.0f;
                        float distFactor = Math.Max(0.5f, 1.0f - (distance - 1) * 0.12f);
                        float baseAcc = Math.Clamp(inputs.StationAccuracy, 0.5f, 0.95f);
                        float conf = Math.Clamp((baseAcc + calibrationBonus) * distFactor * f.confidence, 0.35f, 0.98f);
                        CrisisConfidenceBand band = conf >= 0.75f ? CrisisConfidenceBand.High
                            : conf >= 0.5f ? CrisisConfidenceBand.Moderate
                            : CrisisConfidenceBand.Low;

                        predictions.Add(new CrisisPredictionRecord(
                            CrisisClass.WeatherExtreme,
                            f.day,
                            distance,
                            conf,
                            band,
                            $"crisis.reason.weather_{f.weather.ToString().ToLowerInvariant()}",
                            GetWeatherAdvice(f.weather),
                            (float)f.weather));
                        break; // Pin earliest severe weather crisis
                    }
                }
            }

            // Stable deterministic ordering:
            // 1. Earliest ProjectedDay first
            // 2. Highest Confidence first
            // 3. Lowest CrisisClass enum ordinal
            predictions.Sort((a, b) =>
            {
                int cDay = a.ProjectedDay.CompareTo(b.ProjectedDay);
                if (cDay != 0) return cDay;
                int cConf = b.Confidence.CompareTo(a.Confidence);
                if (cConf != 0) return cConf;
                return a.Kind.CompareTo(b.Kind);
            });

            return predictions;
        }

        private static int ScaleDeadlineHorizon(int horizon, float multiplier)
        {
            return Math.Max(0, (int)Math.Floor(Math.Max(0, horizon) * multiplier));
        }

        private static float NormalizeDeadlineMultiplier(float value)
        {
            return float.IsNaN(value) || float.IsInfinity(value) || value < 0f ? 1f : value;
        }

        private static bool IsSevereWeather(WeatherKind kind)
        {
            return kind switch
            {
                WeatherKind.FalloutStorm => true,
                WeatherKind.Blizzard => true,
                WeatherKind.GlassStorm => true,
                WeatherKind.RadHail => true,
                WeatherKind.EMPStorm => true,
                WeatherKind.AcidSnow => true,
                WeatherKind.BlackRain => true,
                WeatherKind.BioFog => true,
                WeatherKind.IceStorm => true,
                _ => false
            };
        }

        private static string GetWeatherAdvice(WeatherKind kind)
        {
            return kind switch
            {
                WeatherKind.GlassStorm or WeatherKind.RadHail => "Deploy cloud seeding or reinforce ceiling armor against kinetic impacts.",
                WeatherKind.FalloutStorm or WeatherKind.AcidSnow => "Seal outer blast doors and administer anti-rad medication.",
                WeatherKind.EMPStorm => "Disconnect delicate electronics and charge auxiliary battery banks.",
                WeatherKind.Blizzard or WeatherKind.IceStorm => "Stock furnace fuel and cancel outdoor surface expeditions.",
                _ => "Prepare shelter environmental seals and monitor broadcast frequencies."
            };
        }
    }
}
