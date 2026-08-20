using System;
using System.Collections.Generic;

namespace Ashfall.Core.World
{
    /// <summary>
    /// ASHFALL Atmosphere descriptor (item 8).
    ///
    /// Maps authoritative <see cref="WeatherKind"/> to host-renderable
    /// atmosphere parameters: tint color, particle density, visibility
    /// (0..1), and audio gain. Core owns the deterministic mapping so the
    /// host (Godot) can drive visuals and audio without inventing its own
    /// weather rules. Headless mode caps particle counts at 0.
    /// </summary>
    public static class WeatherAtmosphereMap
    {
        public static WeatherAtmosphere For(WeatherKind kind, bool headless = false)
        {
            switch (kind)
            {
                case WeatherKind.Clear:
                    return new WeatherAtmosphere
                    {
                        Tint = new float[] { 1.0f, 1.0f, 1.0f, 1f },
                        AshParticleCount = headless ? 0 : 0,
                        FogDensity = headless ? 0f : 0f,
                        RainParticleCount = headless ? 0 : 0,
                        Visibility = 1f,
                        AudioGain = 0.15f,
                        AudioCueId = "audio_weather_clear"
                    };
                case WeatherKind.Overcast:
                    return new WeatherAtmosphere
                    {
                        Tint = new float[] { 0.85f, 0.85f, 0.92f, 1f },
                        AshParticleCount = headless ? 0 : 0,
                        FogDensity = headless ? 0.05f : 0.2f,
                        RainParticleCount = headless ? 0 : 0,
                        Visibility = 0.92f,
                        AudioGain = 0.20f,
                        AudioCueId = "audio_weather_overcast"
                    };
                case WeatherKind.Rain:
                    return new WeatherAtmosphere
                    {
                        Tint = new float[] { 0.70f, 0.75f, 0.85f, 1f },
                        AshParticleCount = headless ? 0 : 0,
                        FogDensity = headless ? 0.10f : 0.35f,
                        RainParticleCount = headless ? 0 : 80,
                        Visibility = 0.78f,
                        AudioGain = 0.45f,
                        AudioCueId = "audio_weather_rain"
                    };
                case WeatherKind.Ashfall:
                    return new WeatherAtmosphere
                    {
                        Tint = new float[] { 0.55f, 0.50f, 0.45f, 1f },
                        AshParticleCount = headless ? 0 : 60,
                        FogDensity = headless ? 0.20f : 0.55f,
                        RainParticleCount = headless ? 0 : 0,
                        Visibility = 0.55f,
                        AudioGain = 0.30f,
                        AudioCueId = "audio_weather_ash_fall"
                    };
                case WeatherKind.FalloutStorm:
                    return new WeatherAtmosphere
                    {
                        Tint = new float[] { 0.40f, 0.35f, 0.30f, 1f },
                        AshParticleCount = headless ? 0 : 200,
                        FogDensity = headless ? 0.30f : 0.85f,
                        RainParticleCount = headless ? 0 : 0,
                        Visibility = 0.30f,
                        AudioGain = 0.55f,
                        AudioCueId = "audio_weather_fallout_storm"
                    };
                case WeatherKind.Blizzard:
                    return new WeatherAtmosphere
                    {
                        Tint = new float[] { 0.85f, 0.88f, 0.95f, 1f },
                        AshParticleCount = headless ? 0 : 0,
                        FogDensity = headless ? 0.20f : 0.70f,
                        RainParticleCount = headless ? 0 : 120,
                        Visibility = 0.40f,
                        AudioGain = 0.50f,
                        AudioCueId = "audio_weather_blizzard"
                    };
                case WeatherKind.BlackSnow:
                    return new WeatherAtmosphere
                    {
                        Tint = new float[] { 0.45f, 0.45f, 0.50f, 1f },
                        AshParticleCount = headless ? 0 : 100,
                        FogDensity = headless ? 0.25f : 0.70f,
                        RainParticleCount = headless ? 0 : 0,
                        Visibility = 0.45f,
                        AudioGain = 0.40f,
                        AudioCueId = "audio_weather_black_snow"
                    };
                default:
                    return For(WeatherKind.Clear, headless);
            }
        }
    }

    [Serializable]
    public sealed class WeatherAtmosphere
    {
        public float[] Tint = new float[] { 1f, 1f, 1f, 1f };
        public int AshParticleCount;
        public int RainParticleCount;
        public float FogDensity;
        public float Visibility = 1f;
        public float AudioGain = 0.2f;
        public string AudioCueId = string.Empty;
    }
}
