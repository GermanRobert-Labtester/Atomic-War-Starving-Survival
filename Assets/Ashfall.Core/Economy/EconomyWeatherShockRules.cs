// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Economy
{
    /// <summary>
    /// Plan 212 — weather→market shock policy. Core owns the mapping rule
    /// (deterministic, bounded); the host adapter only reads the canonical
    /// weather authority and calls <see cref="MarketSystem.ApplyShock"/> with
    /// the returned band. Clear/overcast-class weather applies no shock.
    /// Severity bands follow <see cref="WeatherSeverityCalculator"/>
    /// (blizzard = 2.0, storm class = 1.5, everything calmer = no shock).
    /// </summary>
    public static class EconomyWeatherShockRules
    {
        public sealed class WeatherShockBand
        {
            public string CategoryId { get; }
            public bool IsShortage { get; }
            public float SeverityBp { get; }
            public int DurationDays { get; }
            public string SourceId { get; }

            public WeatherShockBand(string categoryId, bool isShortage, float severityBp, int durationDays, string sourceId)
            {
                CategoryId = categoryId;
                IsShortage = isShortage;
                SeverityBp = severityBp;
                DurationDays = durationDays;
                SourceId = sourceId;
            }
        }

        /// <summary>Blizzard-class: harvests and convoys fail — strongest, longest band.</summary>
        public const float BlizzardSeverityBp = 1500f;
        public const int BlizzardDurationDays = 3;
        /// <summary>Storm class (ashfall, rad-storm, black rain…): two-day squeeze.</summary>
        public const float StormSeverityBp = 1000f;
        public const int StormDurationDays = 2;

        /// <summary>
        /// Map the day's weather severity to a market shock band. Returns
        /// null when the weather applies no shock (severity &lt; 1.5).
        /// </summary>
        public static WeatherShockBand? TryGetWeatherShock(WeatherKind weather)
        {
            float severity = WeatherSeverityCalculator.GetSeverity(weather);
            if (severity >= 2.0f)
                return new WeatherShockBand("food", true, BlizzardSeverityBp, BlizzardDurationDays, "weather_blizzard");
            if (severity >= 1.5f)
                return new WeatherShockBand("food", true, StormSeverityBp, StormDurationDays, "weather_storm");
            return null;
        }
    }
}
