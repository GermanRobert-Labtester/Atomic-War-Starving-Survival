// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Environmental and topological context for radio signal propagation.
    /// Invariant: Pure C#, zero engine dependencies.
    /// </summary>
    [Serializable]
    public sealed class RadioPropagationContext
    {
        public int Day { get; set; } = 1;
        public string WeatherCondition { get; set; } = "Clear";
        public WeatherKind Weather { get; set; } = WeatherKind.Clear;
        public int DistanceTicks { get; set; } = 4;
        public string TerrainClass { get; set; } = "urban"; // open, urban, mountainous, subterranean, water
        public float AmbientNoise { get; set; } = 0.1f;
        public float OperatorSkill { get; set; } = 0.5f;

        public RadioPropagationContext() { }

        public RadioPropagationContext(int day, WeatherKind weather, int distanceTicks, string terrainClass = "urban", float ambientNoise = 0.1f)
        {
            Day = day;
            Weather = weather;
            WeatherCondition = weather.ToString();
            DistanceTicks = distanceTicks;
            TerrainClass = terrainClass;
            AmbientNoise = ambientNoise;
        }

        public RadioPropagationContext(int day, string weatherCondition, int distanceTicks, string terrainClass = "urban", float ambientNoise = 0.1f)
        {
            Day = day;
            WeatherCondition = weatherCondition ?? "Clear";
            if (Enum.TryParse<WeatherKind>(WeatherCondition, true, out var wk))
                Weather = wk;
            else
                Weather = WeatherKind.Clear;
            DistanceTicks = distanceTicks;
            TerrainClass = terrainClass;
            AmbientNoise = ambientNoise;
        }
    }

    /// <summary>
    /// Evaluated reception result from deterministic signal propagation.
    /// </summary>
    [Serializable]
    public sealed class RadioPropagationResult
    {
        public float CarrierStrength { get; set; }
        public float AttenuationFactor { get; set; }
        public float NoiseFloor { get; set; }
        public float EffectiveVu { get; set; }
        public bool IsLocked { get; set; }
        public float Clarity { get; set; }
        public int AudibleFragmentIndex { get; set; } = -1;
        public string Explanation { get; set; } = string.Empty;
    }

    /// <summary>
    /// Authoritative deterministic propagation engine factoring distance, terrain,
    /// and live post-war atmospheric conditions.
    /// </summary>
    public static class RadioPropagationEngine
    {
        public const float LockVuThreshold = 0.25f;

        /// <summary>
        /// Calculates propagation factor based on weather condition (0..1).
        /// </summary>
        public static float GetWeatherAttenuation(WeatherKind weather)
        {
            return weather switch
            {
                WeatherKind.Clear => 1.0f,
                WeatherKind.Overcast => 0.95f,
                WeatherKind.Rain => 0.90f,
                WeatherKind.Ashfall => 0.75f,
                WeatherKind.AcidSnow => 0.70f,
                WeatherKind.ParticulateFog => 0.65f,
                WeatherKind.BioFog => 0.65f,
                WeatherKind.Blizzard => 0.55f,
                WeatherKind.RadHail => 0.55f,
                WeatherKind.BlackSnow => 0.50f,
                WeatherKind.BlackRain => 0.45f,
                WeatherKind.FalloutStorm => 0.35f,
                WeatherKind.AshLightning => 0.30f,
                WeatherKind.EMPStorm => 0.15f,
                WeatherKind.Silence => 1.0f,
                WeatherKind.ThermalInversion => 1.15f, // Atmospheric ducting enhances propagation
                _ => 0.85f
            };
        }

        public static float GetWeatherAttenuation(string weatherCondition)
        {
            if (string.IsNullOrWhiteSpace(weatherCondition)) return 1.0f;
            if (Enum.TryParse<WeatherKind>(weatherCondition, true, out var wk))
                return GetWeatherAttenuation(wk);

            if (weatherCondition.Equals("Night", StringComparison.OrdinalIgnoreCase) ||
                weatherCondition.Equals("Skywave", StringComparison.OrdinalIgnoreCase))
                return 1.1f;

            return 0.85f;
        }

        /// <summary>
        /// Calculates terrain attenuation factor (0..1).
        /// </summary>
        public static float GetTerrainAttenuation(string terrainClass)
        {
            if (string.IsNullOrWhiteSpace(terrainClass)) return 0.85f;
            return terrainClass.ToLowerInvariant() switch
            {
                "water" or "open" or "plains" => 1.0f,
                "suburban" or "ruins" => 0.90f,
                "urban" or "industrial" => 0.80f,
                "mountainous" or "crater" => 0.65f,
                "subterranean" or "bunker" or "tunnel" => 0.40f,
                _ => 0.80f
            };
        }

        /// <summary>
        /// Calculates distance path-loss factor (0..1).
        /// Inverse logarithmic curve: 1 tick -> 1.0, 5 ticks -> ~0.70, 10 ticks -> ~0.50, 20 ticks -> ~0.33.
        /// </summary>
        public static float GetDistanceAttenuation(int distanceTicks)
        {
            if (distanceTicks <= 1) return 1.0f;
            return (float)Math.Clamp(1.0 / (1.0 + 0.1 * (distanceTicks - 1)), 0.1, 1.0);
        }

        /// <summary>
        /// Evaluates deterministic signal propagation for a distress signal against context.
        /// </summary>
        public static RadioPropagationResult EvaluatePropagation(
            DistressSignalDefinition signal,
            RadioPropagationContext context,
            float frequencyOffsetMhz = 0f)
        {
            if (signal == null)
            {
                return new RadioPropagationResult
                {
                    CarrierStrength = 0f,
                    AttenuationFactor = 0f,
                    NoiseFloor = context?.AmbientNoise ?? 0.1f,
                    EffectiveVu = 0f,
                    IsLocked = false,
                    Clarity = 0f,
                    Explanation = "No signal definition."
                };
            }

            var ctx = context ?? new RadioPropagationContext();

            float weatherFactor = GetWeatherAttenuation(ctx.WeatherCondition);
            float terrainFactor = GetTerrainAttenuation(ctx.TerrainClass);
            float distFactor = GetDistanceAttenuation(ctx.DistanceTicks);

            // Combined attenuation
            float attenuation = weatherFactor * terrainFactor * distFactor;

            // Frequency tuning offset degradation (lock window ±0.5 MHz)
            float tuneToleranceMhz = 0.5f;
            float absOffset = Math.Abs(frequencyOffsetMhz);
            float tuneFactor = Math.Max(0f, 1f - (absOffset / tuneToleranceMhz));

            // Base signal strength (nominal 1.0 for authentic emitters)
            float baseCarrier = 1.0f;
            float carrier = baseCarrier * attenuation * tuneFactor;

            // Noise floor influenced by weather (e.g. EMP or storms raise noise)
            float weatherNoiseSpike = ctx.Weather == WeatherKind.EMPStorm ? 0.45f
                : (ctx.Weather == WeatherKind.FalloutStorm ? 0.25f : 0f);
            float effectiveNoise = Math.Clamp(ctx.AmbientNoise + weatherNoiseSpike, 0.05f, 0.95f);

            // Effective VU strength
            float vu = Math.Clamp(carrier * (1f - effectiveNoise), 0f, 1f);
            bool isLocked = vu >= LockVuThreshold;

            // Resolve audible fragment and clarity
            float clarity = 0.2f;

            // Tasks 9–12 Wave 1: single shared stage resolver (was a duplicated
            // inline loop; contract documented on the resolver). Authored clarity
            // is still attenuated by atmospheric conditions below.
            int fragIdx = DistressStageResolver.ResolveStageIndex(signal, ctx.Day);

            if (fragIdx >= 0)
            {
                var activeFrag = signal.MessageFragments![fragIdx];
                // Base clarity attenuated by atmospheric conditions
                clarity = Math.Clamp(activeFrag.Clarity * attenuation, 0.1f, activeFrag.Clarity);
            }

            string explanation = isLocked
                ? $"Signal locked on {signal.FrequencyMhzStr} MHz (VU: {vu:F2}, Attenuation: {attenuation:P0}, Weather: {ctx.WeatherCondition})."
                : $"Signal below lock threshold ({vu:F2} < {LockVuThreshold:F2}) in {ctx.WeatherCondition}.";

            return new RadioPropagationResult
            {
                CarrierStrength = carrier,
                AttenuationFactor = attenuation,
                NoiseFloor = effectiveNoise,
                EffectiveVu = vu,
                IsLocked = isLocked,
                Clarity = clarity,
                AudibleFragmentIndex = fragIdx,
                Explanation = explanation
            };
        }

        /// <summary>
        /// Updates active signal tracking ensuring monotonic HighestClarity thresholding.
        /// </summary>
        public static void UpdateMonotonicClarity(ActiveDistressSignal activeSignal, float observedClarity)
        {
            if (activeSignal == null) return;
            if (observedClarity > activeSignal.HighestClarity)
            {
                activeSignal.HighestClarity = (float)Math.Round(observedClarity, 2);
            }
        }
    }
}
