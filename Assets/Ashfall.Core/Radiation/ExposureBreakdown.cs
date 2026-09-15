// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text;

namespace Ashfall.Core.Radiation
{
    /// <summary>
    /// C2 / Plan 20A — read-model breakdown of one survivor's exposure inputs
    /// (plan §16). Every value is copied from the same <see cref="ExposureEnvironment"/>
    /// the RadiationSystem tick consumed, and the effective rate is produced by
    /// the canonical <see cref="RadiationSystem.ComputeEffectiveRate"/>, so a
    /// UI surface can never disagree with the simulation (plan §3.8/§16.1).
    /// Presentation only: this type mutates nothing and owns no balance.
    /// </summary>
    public sealed class ExposureBreakdown
    {
        public SurvivorExposureLocation LocationKind { get; set; } = SurvivorExposureLocation.ShelterInterior;
        public string LocationId { get; set; } = string.Empty;
        public string PositionLabel { get; set; } = string.Empty;

        /// <summary>Effective ambient at the position (base + weather + fallout + anomaly).</summary>
        public float ZoneAmbient { get; set; }
        public float BaseRadRate { get; set; }
        public float WeatherModifier { get; set; }
        public float FalloutContamination { get; set; }
        public float AnomalyRate { get; set; }
        public float ShelterShielding { get; set; }
        public float GearProtection { get; set; }

        /// <summary>Canonical effective rate (mSv/h) after shielding and gear.</summary>
        public float EffectiveExposurePerHour { get; set; }

        public float AccumulatedDose { get; set; }
        public float LifetimeDose { get; set; }

        /// <summary>
        /// Build from the exact environment the tick used. <paramref name="interiorRads"/>
        /// mirrors the ShelterRadQuery path; null means the shielding fallback
        /// path (identical branch logic to RadiationSystem.Tick).
        /// </summary>
        public static ExposureBreakdown Build(
            ExposureEnvironment env,
            float gearProtection,
            float accumulatedDose,
            float lifetimeDose,
            float? interiorRads = null,
            string? locationDisplayName = null)
        {
            if (env == null) throw new ArgumentNullException(nameof(env));
            float zone = env.EffectiveZoneRadLevel;
            return new ExposureBreakdown
            {
                LocationKind = env.LocationKind,
                LocationId = env.LocationId ?? string.Empty,
                PositionLabel = BuildPositionLabel(env.LocationKind, env.LocationId, locationDisplayName),
                ZoneAmbient = zone,
                BaseRadRate = env.BaseRadRate,
                WeatherModifier = env.WeatherRadModifier,
                FalloutContamination = env.FalloutContamination,
                AnomalyRate = env.AnomalyRadRate,
                ShelterShielding = env.ShelterShielding,
                GearProtection = MathF.Max(0f, gearProtection),
                EffectiveExposurePerHour = RadiationSystem.ComputeEffectiveRate(
                    zone, gearProtection, env.ShelterShielding, interiorRads),
                AccumulatedDose = accumulatedDose,
                LifetimeDose = lifetimeDose
            };
        }

        /// <summary>Player-facing position context (plan §16.2): meaning, not raw ids.</summary>
        public static string BuildPositionLabel(
            SurvivorExposureLocation kind, string? locationId, string? locationDisplayName = null)
        {
            string name = !string.IsNullOrEmpty(locationDisplayName)
                ? locationDisplayName!
                : !string.IsNullOrEmpty(locationId) ? locationId! : string.Empty;
            return kind switch
            {
                SurvivorExposureLocation.ShelterInterior => "Shelter interior",
                SurvivorExposureLocation.ShelterPerimeter => "Shelter perimeter",
                SurvivorExposureLocation.WastelandOutdoors => "Surface",
                SurvivorExposureLocation.Expedition =>
                    string.IsNullOrEmpty(name) ? "Expedition" : $"Expedition — {name}",
                _ => "Shelter interior"
            };
        }

        /// <summary>
        /// Compact one-line breakdown for the radiation detail surface. Zero
        /// components are omitted so the line stays readable; the zone, the
        /// effective rate, and accumulated dose always appear.
        /// </summary>
        public string ToDisplayLine()
        {
            var sb = new StringBuilder();
            sb.Append(PositionLabel);
            sb.Append(" · Zone ").Append(ZoneAmbient.ToString("0.0"));
            if (WeatherModifier != 0f) sb.Append(" · weather +").Append(WeatherModifier.ToString("0.#"));
            if (FalloutContamination != 0f) sb.Append(" · fallout +").Append(FalloutContamination.ToString("0.#"));
            if (AnomalyRate != 0f) sb.Append(" · anomaly +").Append(AnomalyRate.ToString("0.#"));
            if (ShelterShielding != 0f) sb.Append(" · shielding −").Append(ShelterShielding.ToString("0.#"));
            if (GearProtection != 0f) sb.Append(" · gear −").Append(GearProtection.ToString("0.#"));
            sb.Append(" → ").Append(EffectiveExposurePerHour.ToString("0.0")).Append(" mSv/h");
            sb.Append(" · dose ").Append(AccumulatedDose.ToString("0.0")).Append("/100");
            sb.Append(" · lifetime ").Append(LifetimeDose.ToString("0.0")).Append(" mSv");
            return sb.ToString();
        }
    }
}