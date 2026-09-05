using System;
using Ashfall.Core;
using Ashfall.Core.World;

namespace Ashfall.Core.Narrative
{
    /// <summary>
    /// Explicit deterministic selection context for travel encounters (D2).
    /// Weather is passed in — never read from a global singleton inside
    /// eligibility checks — so the same context always yields the same
    /// eligibility answer for the same inputs.
    /// </summary>
    public sealed class TravelEncounterSelectionContext
    {
        public string Region { get; init; } = "";
        public int DangerLevel { get; init; }
        public string Stance { get; init; } = "";
        public string CurrentSeason { get; init; } = "";
        public int CurrentDay { get; init; }
        public WeatherKind CurrentWeather { get; init; }
        public ISeededRng? Rng { get; init; }

        public static TravelEncounterSelectionContext From(
            string region,
            int dangerLevel,
            string stance,
            string currentSeason,
            int currentDay,
            WeatherKind currentWeather,
            ISeededRng? rng = null)
        {
            return new TravelEncounterSelectionContext
            {
                Region = region ?? "",
                DangerLevel = dangerLevel,
                Stance = stance ?? "",
                CurrentSeason = currentSeason ?? "",
                CurrentDay = currentDay,
                CurrentWeather = currentWeather,
                Rng = rng
            };
        }
    }
}
