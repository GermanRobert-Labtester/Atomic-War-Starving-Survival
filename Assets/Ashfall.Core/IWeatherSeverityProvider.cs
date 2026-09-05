// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core
{
    /// <summary>
    /// Provider interface for querying weather severity for an ecological zone and campaign day.
    /// </summary>
    public interface IWeatherSeverityProvider
    {
        float GetSeverity(string zoneId, int campaignDay);
    }

    /// <summary>
    /// Canonical severity calculator mapping weather states to operational degradation severity.
    /// Canonical mapping:
    /// - Clear / Overcast / Calm: 1.0
    /// - Rain / Fog: 1.2
    /// - Storm / Ashfall / Rad-storm: 1.5
    /// - Blizzard / Inversion: 2.0
    /// </summary>
    public static class WeatherSeverityCalculator
    {
        public static float GetSeverity(WeatherKind weather) => weather switch
        {
            WeatherKind.Clear => 1.0f,
            WeatherKind.Rain => 1.2f,
            WeatherKind.Blizzard => 2.0f,
            WeatherKind.FalloutStorm or WeatherKind.EMPStorm or WeatherKind.GlassStorm or
            WeatherKind.RadHail or WeatherKind.AshLightning or WeatherKind.IceStorm or
            WeatherKind.BlackRain or WeatherKind.AcidSnow or WeatherKind.BlackSnow or
            WeatherKind.BloodRain or WeatherKind.Ashfall => 1.5f,
            WeatherKind.Overcast or WeatherKind.ParticulateFog or WeatherKind.BioFog or
            WeatherKind.AlgaeBloom or WeatherKind.Silence or WeatherKind.FalseSpring => 1.0f,
            _ => 1.0f
        };

        public static float GetSeverity(string weatherName)
        {
            if (string.IsNullOrWhiteSpace(weatherName)) return 1.0f;
            string lower = weatherName.Trim().ToLowerInvariant();
            return lower switch
            {
                "clear" => 1.0f,
                "rain" => 1.2f,
                "storm" => 1.5f,
                "blizzard" => 2.0f,
                _ => Enum.TryParse<WeatherKind>(weatherName, true, out var kind) ? GetSeverity(kind) : 1.0f
            };
        }
    }
}
