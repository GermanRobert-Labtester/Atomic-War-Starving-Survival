// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.World;

namespace Ashfall.Core.Weather
{
    /// <summary>
    /// Canonical weather hazard index for the Plan 135 weather→gameplay cascade.
    /// <para>
    /// Authority boundary (deliberate): this class owns NO weather fact. Every
    /// input is read from the existing <see cref="WeatherEffectsCatalog"/> — the
    /// same C2 / Plan 20A+20C table that <c>WeatherSystem</c> already consumes
    /// for rad dose, visibility, thermal load, travel speed and encounter
    /// chance. The index therefore cannot disagree with the weather the player
    /// is standing in, and nothing here mutates or caches a weather value.
    /// </para>
    /// <para>
    /// The five axes are the five canonical, already-authored per-kind signals.
    /// Each is normalized to 0..1 against a named reference constant, then
    /// combined with named weights (integer permille, like the rest of the
    /// codebase). There is no per-kind table in this file and no RNG: two calls
    /// with the same kind and catalog return the same severity, which is what
    /// the cascade's determinism contract needs.
    /// </para>
    /// </summary>
    public static class WeatherCascadeSeverity
    {
        // ── Normalization references (per-kind authored values are clamped into [0,1] against these) ──

        /// <summary>Thermal-load delta (°C) that counts as full thermal stress.
        /// <c>weather_effects.json</c> worst authored value is Blizzard at -15 °C.</summary>
        public const float ThermalStressReferenceC = 15f;

        /// <summary>Outdoor rad modifier that counts as full radiological stress.
        /// <c>weather_effects.json</c> worst authored value is BlackRain at 250.</summary>
        public const float RadStressReference = 250f;

        /// <summary>Encounter-chance multiplier above neutral that counts as full
        /// encounter stress. Worst authored value is 1.30 (BlackRain).</summary>
        public const float EncounterStressSpan = 0.30f;

        // ── Axis weights (permille; must sum to 1000) ──

        public const int VisibilityWeightPermille = 300;
        public const int ThermalWeightPermille = 250;
        public const int RadWeightPermille = 200;
        public const int TravelDelayWeightPermille = 150;
        public const int EncounterWeightPermille = 100;

        /// <summary>
        /// Composite weather hazard severity, 0..100 (0 = benign, 100 = worst
        /// conceivable front). Derived entirely from the canonical per-kind
        /// weather-effects row; an unbound catalog yields 0 (no cascade) rather
        /// than a fabricated severity.
        /// </summary>
        public static float SeverityFor(WeatherKind kind, WeatherEffectsCatalog? catalog)
        {
            if (catalog == null) return 0f;
            if (!catalog.TryGetEffects(kind, out var effects) || effects == null) return 0f;

            float visibility = Clamp01(1f - Clamp01(effects.visibility_modifier));
            float thermal = Clamp01(MathF.Abs(effects.thermal_load_additive_c) / ThermalStressReferenceC);
            float rad = Clamp01(effects.outdoor_rad_modifier / RadStressReference);
            float delay = Clamp01(1f - Clamp01(effects.travel_speed_multiplier));
            float encounter = Clamp01((Clamp01(effects.travel_encounter_multiplier) - 1f) / EncounterStressSpan);

            long permille =
                (long)(visibility * VisibilityWeightPermille
                     + thermal * ThermalWeightPermille
                     + rad * RadWeightPermille
                     + delay * TravelDelayWeightPermille
                     + encounter * EncounterWeightPermille);

            return Math.Clamp(permille / 10f, 0f, 100f);
        }

        /// <summary>True when the canonical row marks this kind mechanically
        /// neutral (all-identity effects). Used by the host to skip cascading
        /// benign weather instead of firing effects from a cosmetic front.</summary>
        public static bool IsMechanicallyNeutral(WeatherKind kind, WeatherEffectsCatalog? catalog)
        {
            if (catalog == null) return true;
            if (!catalog.TryGetEffects(kind, out var effects) || effects == null) return true;
            return effects.explicitly_neutral;
        }

        private static float Clamp01(float v) => v < 0f ? 0f : (v > 1f ? 1f : v);
    }
}
