using System;
using UnityEngine;
using Random = System.Random;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Kinds of weather, including fallout storms that spike radiation exposure.
    /// </summary>
    public enum WeatherKind
    {
        Clear,
        Ashfall,
        FalloutStorm,
        Blizzard
    }

    /// <summary>
    /// Drives weather state transitions over time via seeded weighted-random rolls
    /// against the active SeasonProfile.SeasonWindow, and raises OnWeatherChanged.
    /// FalloutStorm spikes outdoor radiation and drops visibility to zero (scavenging
    /// blocked without a full suit); Blizzard drives nuclear-winter cold via
    /// TemperaturePenaltyForWeather.
    ///
    /// Deterministic for save/load: rather than persisting System.Random's opaque
    /// internal state, each roll reseeds fresh from Seed + a monotonic RollCount, so
    /// GetState()/RestoreState() only need to round-trip plain primitives to resume
    /// the exact same future sequence.
    /// </summary>
    public class WeatherSystem
    {
        /// <summary>Outdoor dose-rate added (same abstract units as RadZoneProfile.radLevel) while a FalloutStorm is active.</summary>
        public const float FalloutStormOutdoorRadModifier = 150f;
        /// <summary>Perceived-temperature penalty (°C) applied to unsheltered survivors during a Blizzard.</summary>
        public const float BlizzardTemperaturePenaltyC = -15f;
        /// <summary>Perceived-temperature penalty (°C) applied to unsheltered survivors during a FalloutStorm.</summary>
        public const float FalloutStormTemperaturePenaltyC = -5f;
        /// <summary>Visibility factor (0..1) during a Blizzard: reduced, but not the total blackout of a FalloutStorm.</summary>
        public const float BlizzardVisibilityFactor = 0.4f;

        private readonly SeasonProfile _profile;
        private readonly int _seed;
        private float _hoursUntilNextCheck;
        private float _totalElapsedHours;
        private int _rollCount;

        public WeatherKind Current { get; private set; } = WeatherKind.Clear;

        /// <summary>Base seed this system's deterministic roll sequence is derived from.</summary>
        public int Seed => _seed;

        /// <summary>Fired whenever Current actually changes (never on a roll that repeats the current state).</summary>
        public event Action<WeatherKind> OnWeatherChanged;

        /// <summary>Legacy/manual mode: no SeasonProfile, Tick is a no-op. Drive Current via ForceWeather only.</summary>
        public WeatherSystem() : this(null, 0)
        {
        }

        /// <summary>Nuclear-winter mode: Current rolls forward on SeasonProfile.weatherCheckIntervalHours using the given seed.</summary>
        public WeatherSystem(SeasonProfile profile, int seed)
        {
            _profile = profile;
            _seed = seed;
            _hoursUntilNextCheck = NextCheckInterval();
        }

        /// <summary>Advance weather state over elapsed game hours; rolls a new state each time the check interval elapses. No-op in legacy/manual mode.</summary>
        public void Tick(float gameHours)
        {
            if (_profile == null || gameHours <= 0f)
            {
                return;
            }

            _totalElapsedHours += gameHours;
            _hoursUntilNextCheck -= gameHours;

            int safety = 0;
            while (_hoursUntilNextCheck <= 0f && safety < 10000)
            {
                _hoursUntilNextCheck += NextCheckInterval();
                SetCurrent(RollNextState());
                safety++;
            }
        }

        /// <summary>Force a specific weather state (debug / scripted events). Works even in legacy/manual mode.</summary>
        public void ForceWeather(WeatherKind kind)
        {
            _hoursUntilNextCheck = NextCheckInterval();
            SetCurrent(kind);
        }

        /// <summary>Visibility factor (0..1): zero during a FalloutStorm, reduced during a Blizzard, full otherwise.</summary>
        public float VisibilityFactor
        {
            get
            {
                switch (Current)
                {
                    case WeatherKind.FalloutStorm: return 0f;
                    case WeatherKind.Blizzard: return BlizzardVisibilityFactor;
                    default: return 1f;
                }
            }
        }

        /// <summary>Extra outdoor dose-rate contributed by the current weather (FalloutStorm only).</summary>
        public float OutdoorRadModifier => Current == WeatherKind.FalloutStorm ? FalloutStormOutdoorRadModifier : 0f;

        /// <summary>True when a FalloutStorm's zero visibility blocks scavenging for a survivor without a full (hazmat) suit.</summary>
        public bool IsScavengingBlocked(bool hasFullSuit) => Current == WeatherKind.FalloutStorm && !hasFullSuit;

        /// <summary>Perceived-temperature penalty (°C) contributed by a given weather kind; 0 for Clear/Ashfall.</summary>
        public static float TemperaturePenaltyForWeather(WeatherKind kind)
        {
            switch (kind)
            {
                case WeatherKind.Blizzard: return BlizzardTemperaturePenaltyC;
                case WeatherKind.FalloutStorm: return FalloutStormTemperaturePenaltyC;
                default: return 0f;
            }
        }

        /// <summary>Export a save-safe snapshot sufficient to resume the exact same deterministic sequence.</summary>
        public WeatherState GetState()
        {
            return new WeatherState
            {
                Current = Current,
                Seed = _seed,
                RollCount = _rollCount,
                HoursUntilNextCheck = _hoursUntilNextCheck,
                TotalElapsedHours = _totalElapsedHours
            };
        }

        /// <summary>Restore from a save-safe snapshot. Construct with the same Seed first; this does not re-seed.</summary>
        public void RestoreState(WeatherState state)
        {
            if (state == null)
            {
                return;
            }

            Current = state.Current;
            _rollCount = state.RollCount;
            _hoursUntilNextCheck = state.HoursUntilNextCheck;
            _totalElapsedHours = state.TotalElapsedHours;
        }

        private WeatherKind RollNextState()
        {
            var season = _profile.GetSeasonForDay(Mathf.FloorToInt(_totalElapsedHours / 24f));
            var rng = new Random(unchecked(_seed * 397 + _rollCount));
            _rollCount++;

            float clear = Mathf.Max(0f, season.GetWeight(WeatherKind.Clear));
            float ashfall = Mathf.Max(0f, season.GetWeight(WeatherKind.Ashfall));
            float storm = Mathf.Max(0f, season.GetWeight(WeatherKind.FalloutStorm));
            float blizzard = Mathf.Max(0f, season.GetWeight(WeatherKind.Blizzard));
            float total = clear + ashfall + storm + blizzard;
            if (total <= 0f)
            {
                return WeatherKind.Clear;
            }

            double roll = rng.NextDouble() * total;
            if (roll < clear)
            {
                return WeatherKind.Clear;
            }
            roll -= clear;
            if (roll < ashfall)
            {
                return WeatherKind.Ashfall;
            }
            roll -= ashfall;
            if (roll < storm)
            {
                return WeatherKind.FalloutStorm;
            }
            return WeatherKind.Blizzard;
        }

        private void SetCurrent(WeatherKind next)
        {
            if (next == Current)
            {
                return;
            }
            Current = next;
            OnWeatherChanged?.Invoke(next);
        }

        private float NextCheckInterval()
        {
            float interval = _profile != null ? _profile.weatherCheckIntervalHours : 6f;
            return Mathf.Max(0.01f, interval);
        }
    }
}
