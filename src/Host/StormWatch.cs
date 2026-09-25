// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Alpha feature G3 — storm window read model.
    ///
    /// Answers one question for the shelter systems: is a violent storm active
    /// now, and how many days until one arrives? Reads only the existing
    /// weather authority (<see cref="WeatherSystem"/>); owns no state and
    /// writes nothing, so the forecast window needs no save section.
    /// </summary>
    public static class StormWatch
    {
        /// <summary>Weather kinds that count as a storm for sealing work.</summary>
        public static bool IsStorm(WeatherKind kind) => kind switch
        {
            WeatherKind.Ashfall => true,
            WeatherKind.FalloutStorm => true,
            WeatherKind.Blizzard => true,
            WeatherKind.BlackRain => true,
            WeatherKind.AcidSnow => true,
            WeatherKind.BlackSnow => true,
            WeatherKind.BloodRain => true,
            WeatherKind.EMPStorm => true,
            WeatherKind.GlassStorm => true,
            WeatherKind.RadHail => true,
            WeatherKind.AshLightning => true,
            WeatherKind.IceStorm => true,
            _ => false,
        };

        public readonly struct Snapshot
        {
            public Snapshot(bool stormActive, int daysUntilStorm, string kind)
            {
                StormActive = stormActive;
                DaysUntilStorm = daysUntilStorm;
                Kind = kind;
            }

            public bool StormActive { get; }
            /// <summary>Days until the next storm; <see cref="int.MaxValue"/> when none is forecast.</summary>
            public int DaysUntilStorm { get; }
            public string Kind { get; }
        }

        public static Snapshot Read(WeatherSystem? weather, int horizonDays = 4)
        {
            if (weather == null) return new Snapshot(false, int.MaxValue, "—");

            var current = weather.Current;
            if (IsStorm(current)) return new Snapshot(true, 0, current.ToString());

            int days = int.MaxValue;
            try
            {
                var forecast = weather.PeekForecast(horizonDays);
                for (int i = 0; i < forecast.Count; i++)
                {
                    var entry = forecast[i];
                    if (entry == null || !IsStorm(entry.Kind)) continue;
                    // Entries are consecutive days starting at the current day,
                    // so the loop index (not the absolute Day field) is the
                    // days-until value the panel needs.
                    if (i < days) days = i;
                }
            }
            catch (Exception)
            {
                // A forecast read must never break the sealing panel; an empty
                // forecast simply means "no storm known".
                days = int.MaxValue;
            }

            return new Snapshot(false, days, current.ToString());
        }
    }
}
