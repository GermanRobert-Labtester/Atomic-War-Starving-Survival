// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// ASHFALL — Authoritative Campaign Calendar Read Model (Plan 38 §38A.3).
    ///
    /// Single immutable domain snapshot for in-game time context:
    /// day, season, seasonal progress, days-to-end, year, chapter, ambient baseline,
    /// and seasonal modifiers. Engine-free.
    /// </summary>
    public sealed class CampaignCalendarReadModel
    {
        public int Day { get; }
        public string SeasonId { get; }
        public string SeasonDisplayName { get; }
        public int SeasonIndex { get; }
        public float SeasonProgress { get; } // 0.0 to 1.0
        public int DaysIntoSeason { get; }
        public int DaysToSeasonEnd { get; }
        public int Year { get; } // 1-based
        public int Chapter { get; } // 1-based
        public float AmbientTemperatureC { get; }
        public float SeasonalSeverity { get; } // 0.0 to 1.0
        public float DayLengthHours { get; }
        public float MigrationBias { get; }
        public float PreservationBias { get; }

        public CampaignCalendarReadModel(
            int day,
            string seasonId,
            string seasonDisplayName,
            int seasonIndex,
            float seasonProgress,
            int daysIntoSeason,
            int daysToSeasonEnd,
            int year,
            int chapter,
            float ambientTemperatureC,
            float seasonalSeverity,
            float dayLengthHours,
            float migrationBias,
            float preservationBias)
        {
            Day = Math.Max(1, day);
            SeasonId = seasonId ?? string.Empty;
            SeasonDisplayName = seasonDisplayName ?? string.Empty;
            SeasonIndex = Math.Max(0, seasonIndex);
            SeasonProgress = Math.Clamp(seasonProgress, 0f, 1f);
            DaysIntoSeason = Math.Max(1, daysIntoSeason);
            DaysToSeasonEnd = Math.Max(0, daysToSeasonEnd);
            Year = Math.Max(1, year);
            Chapter = Math.Max(1, chapter);
            AmbientTemperatureC = ambientTemperatureC;
            SeasonalSeverity = Math.Clamp(seasonalSeverity, 0f, 1f);
            DayLengthHours = Math.Clamp(dayLengthHours, 0f, 24f);
            MigrationBias = migrationBias;
            PreservationBias = preservationBias;
        }
    }
}
