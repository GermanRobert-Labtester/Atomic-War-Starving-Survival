// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Audio
{
    /// <summary>
    /// Snapshot of host survival and environmental facts for ambience evaluation.
    /// </summary>
    public sealed class AmbienceEvaluationContext
    {
        public bool HasPower { get; set; } = true;
        public bool IsBrownout { get; set; } = false;
        public float ExternalRadiation { get; set; } = 0f;
        public float InternalRadiation { get; set; } = 0f;
        public int InfectedSurvivorCount { get; set; } = 0;
        public bool IsSurfaceListening { get; set; } = false;
        public string WeatherKind { get; set; } = "Clear";
    }

    /// <summary>
    /// Resulting state and intensities for continuous audio layers.
    /// </summary>
    public sealed class AmbienceEvaluationResult
    {
        public bool BunkerHumActive { get; set; }
        public bool BunkerLowPowerActive { get; set; }
        public bool RadiationSpikeActive { get; set; }
        public float GeigerIntensity { get; set; }
        public bool SicknessMiseryActive { get; set; }
        public float SicknessMiseryIntensity { get; set; }
        public string SurfaceLoopCue { get; set; } = string.Empty;
    }

    /// <summary>
    /// Deterministic, hysteresis-protected evaluator for shelter and surface ambience.
    /// Eliminates audio fluttering around threshold boundary conditions.
    /// Engine-agnostic C# with zero Godot dependencies.
    /// </summary>
    public sealed class ReactiveAmbienceEvaluator
    {
        // Radiation hysteresis thresholds
        public const float RadiationHighThreshold = 35.0f;
        public const float RadiationLowThreshold = 20.0f;

        // Sickness hysteresis thresholds
        public const int SicknessHighThreshold = 3;
        public const int SicknessLowThreshold = 1;

        private bool _radiationSpikeLatched;
        private bool _sicknessMiseryLatched;

        public bool IsRadiationSpikeLatched => _radiationSpikeLatched;
        public bool IsSicknessMiseryLatched => _sicknessMiseryLatched;

        public void Reset()
        {
            _radiationSpikeLatched = false;
            _sicknessMiseryLatched = false;
        }

        public AmbienceEvaluationResult Evaluate(AmbienceEvaluationContext ctx)
        {
            if (ctx == null) ctx = new AmbienceEvaluationContext();

            var result = new AmbienceEvaluationResult();

            // 1. Power State: Bunker Hum vs Low-Power Creak/Drip
            bool powered = ctx.HasPower && !ctx.IsBrownout;
            result.BunkerHumActive = powered;
            result.BunkerLowPowerActive = !powered;

            // 2. Radiation Spike with Hysteresis
            float effectiveRad = Math.Max(ctx.ExternalRadiation, ctx.InternalRadiation * 1.5f);
            if (!_radiationSpikeLatched)
            {
                if (effectiveRad >= RadiationHighThreshold)
                {
                    _radiationSpikeLatched = true;
                }
            }
            else
            {
                if (effectiveRad < RadiationLowThreshold)
                {
                    _radiationSpikeLatched = false;
                }
            }

            result.RadiationSpikeActive = _radiationSpikeLatched;
            if (_radiationSpikeLatched)
            {
                // Intensity scales smoothly between low threshold and 100+
                result.GeigerIntensity = Math.Clamp((effectiveRad - RadiationLowThreshold) / 60.0f, 0.1f, 1.0f);
            }
            else
            {
                result.GeigerIntensity = 0f;
            }

            // 3. Sickness Misery Layer with Hysteresis
            if (!_sicknessMiseryLatched)
            {
                if (ctx.InfectedSurvivorCount >= SicknessHighThreshold)
                {
                    _sicknessMiseryLatched = true;
                }
            }
            else
            {
                if (ctx.InfectedSurvivorCount <= SicknessLowThreshold)
                {
                    _sicknessMiseryLatched = false;
                }
            }

            result.SicknessMiseryActive = _sicknessMiseryLatched;
            if (_sicknessMiseryLatched)
            {
                result.SicknessMiseryIntensity = Math.Clamp(ctx.InfectedSurvivorCount / 8.0f, 0.25f, 1.0f);
            }
            else
            {
                result.SicknessMiseryIntensity = 0f;
            }

            // 4. Surface Loop Selection
            if (ctx.IsSurfaceListening)
            {
                result.SurfaceLoopCue = ResolveSurfaceCue(ctx.WeatherKind, _radiationSpikeLatched);
            }
            else
            {
                result.SurfaceLoopCue = string.Empty;
            }

            return result;
        }

        private static string ResolveSurfaceCue(string weatherKind, bool radiationSpike)
        {
            if (radiationSpike || string.Equals(weatherKind, "FalloutStorm", StringComparison.OrdinalIgnoreCase))
            {
                return "amb_rad_storm_exterior";
            }

            if (string.Equals(weatherKind, "Ashfall", StringComparison.OrdinalIgnoreCase))
            {
                return "amb_surface_ashfall";
            }

            if (string.Equals(weatherKind, "Blizzard", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(weatherKind, "IceStorm", StringComparison.OrdinalIgnoreCase))
            {
                return "amb_surface_blizzard";
            }

            if (string.Equals(weatherKind, "Silence", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(weatherKind, "SilentSpring", StringComparison.OrdinalIgnoreCase))
            {
                return "amb_surface_dead_silence";
            }

            return "amb_surface_dead_silence";
        }
    }
}
