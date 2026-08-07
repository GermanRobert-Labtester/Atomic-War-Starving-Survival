using System;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Models ambient and perceived temperature (nuclear-winter cold), factoring
    /// campaign-progress drift, shelter insulation, weather, heater modules, and
    /// clothing. Feeds Needs.warmth. Reads Shelter aggregate stats (IndoorTempBonus)
    /// directly.
    ///
    /// Two modes: the parameterless constructor is legacy/manual mode (AmbientCelsius
    /// only moves via SetAmbient, Tick is a no-op) preserved for existing callers and
    /// tests; the (SeasonProfile, WeatherSystem) constructor drives AmbientCelsius from
    /// the nuclear-winter curve as Tick advances campaign time.
    /// </summary>
    public class TemperatureSystem
    {
        private readonly SeasonProfile _seasonProfile;
        private readonly WeatherSystem _weatherSystem;
        private float _totalElapsedHours;
        private SeasonWindow _currentSeason;

        public float AmbientCelsius { get; private set; } = -10f;

        /// <summary>Total in-game hours this system has advanced through Tick. Zero in legacy/manual mode.</summary>
        public float TotalElapsedHours => _totalElapsedHours;

        /// <summary>The active season window for the current elapsed time. Null in legacy/manual mode.</summary>
        public SeasonWindow CurrentSeason => _currentSeason;

        /// <summary>Fired whenever the active season window actually changes.</summary>
        public event Action<SeasonWindow> OnSeasonChanged;

        /// <summary>Legacy/manual mode: AmbientCelsius only moves via SetAmbient; Tick is a no-op.</summary>
        public TemperatureSystem() : this(null, null)
        {
        }

        /// <summary>Nuclear-winter mode: AmbientCelsius drifts along seasonProfile's curve as Tick advances campaign time.</summary>
        public TemperatureSystem(SeasonProfile seasonProfile, WeatherSystem weatherSystem)
        {
            _seasonProfile = seasonProfile;
            _weatherSystem = weatherSystem;
            if (_seasonProfile != null)
            {
                _currentSeason = _seasonProfile.GetSeasonForDay(0);
                AmbientCelsius = _seasonProfile.EvaluateAmbientCelsius(0f);
            }
        }

        public void SetAmbient(float celsius)
        {
            AmbientCelsius = celsius;
        }

        /// <summary>Advance ambient temperature and season over elapsed game hours. No-op in legacy/manual mode.</summary>
        public void Tick(float gameHours)
        {
            if (_seasonProfile == null || gameHours <= 0f)
            {
                return;
            }

            _totalElapsedHours += gameHours;
            AmbientCelsius = _seasonProfile.EvaluateAmbientCelsius(_totalElapsedHours);
            UpdateSeason();
        }

        /// <summary>Calculates indoor temperature factoring shelter heater output.</summary>
        public float GetIndoorTemperature(Shelter.Shelter shelter)
        {
            float bonus = shelter != null ? shelter.IndoorTempBonus : 0f;
            return AmbientCelsius + bonus;
        }

        /// <summary>Perceived temperature for a survivor given gear and shelter: indoors uses shelter bonus, outdoors applies weather penalties (e.g. Blizzard).</summary>
        public float GetPerceivedTemperature(Survivor survivor, Shelter.Shelter shelter = null)
        {
            if (shelter != null)
            {
                return GetIndoorTemperature(shelter);
            }

            float weatherPenalty = _weatherSystem != null
                ? WeatherSystem.TemperaturePenaltyForWeather(_weatherSystem.Current)
                : 0f;
            return AmbientCelsius + weatherPenalty;
        }

        /// <summary>
        /// Perceived temperature (°C) at or above which a survivor counts as "near heat"
        /// and Needs.Warmth recovers instead of draining. Below it the nuclear-winter
        /// cold path runs. Roughly shirtsleeve comfort in a bunker.
        /// </summary>
        public const float WarmthComfortCelsius = 12f;

        /// <summary>
        /// True when it is warm enough where this survivor is standing for warmth to
        /// recover. Pass the shelter when they are indoors and null when they are out
        /// in the open, so weather penalties apply. NeedsSystem's isNearHeatSource
        /// predicate is wired to this.
        /// </summary>
        public bool IsWarmEnoughForRecovery(Survivor survivor, Shelter.Shelter shelter)
        {
            return GetPerceivedTemperature(survivor, shelter) >= WarmthComfortCelsius;
        }

        /// <summary>Restore elapsed campaign time directly (save/load) without re-walking Tick and without firing OnSeasonChanged.</summary>
        public void SetElapsedHours(float hours)
        {
            if (_seasonProfile == null)
            {
                return;
            }

            _totalElapsedHours = Mathf.Max(0f, hours);
            AmbientCelsius = _seasonProfile.EvaluateAmbientCelsius(_totalElapsedHours);
            _currentSeason = _seasonProfile.GetSeasonForDay(Mathf.FloorToInt(_totalElapsedHours / 24f));
        }

        private void UpdateSeason()
        {
            var season = _seasonProfile.GetSeasonForDay(Mathf.FloorToInt(_totalElapsedHours / 24f));
            if (ReferenceEquals(season, _currentSeason))
            {
                return;
            }
            _currentSeason = season;
            OnSeasonChanged?.Invoke(season);
        }
    }
}
