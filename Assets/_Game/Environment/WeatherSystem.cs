using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using Random = System.Random;
using Ashfall.Core;

namespace AtomicWar._Game.Environment
{
    // WeatherKind now lives in Ashfall.Core (Assets/Ashfall.Core/WeatherKind.cs) so the
    // simulation core and both hosts share ONE definition. The duplicate that used to sit
    // here had identical members and ordering, which is exactly how a fork silently drifts.

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
        /// <summary>Outdoor dose-rate while BlackRain is active (Prompt #11) — worse than FalloutStorm.</summary>
        public const float BlackRainOutdoorRadModifier = 250f;
        /// <summary>Hazmat / protective-gear durability multiplier during BlackRain.</summary>
        public const float BlackRainHazmatMeltMultiplier = 5f;
        /// <summary>Perceived-temperature penalty (°C) applied to unsheltered survivors during a Blizzard.</summary>
        public const float BlizzardTemperaturePenaltyC = -15f;
        /// <summary>Perceived-temperature penalty (°C) applied to unsheltered survivors during a FalloutStorm.</summary>
        public const float FalloutStormTemperaturePenaltyC = -5f;
        /// <summary>Perceived-temperature penalty (°C) during BlackRain (cold oily rain).</summary>
        public const float BlackRainTemperaturePenaltyC = -8f;
        /// <summary>Visibility factor (0..1) during a Blizzard: reduced, but not the total blackout of a FalloutStorm.</summary>
        public const float BlizzardVisibilityFactor = 0.4f;

        private readonly SeasonProfile _profile;
        private readonly int _seed;
        private float _hoursUntilNextCheck;
        private float _totalElapsedHours;
        private int _rollCount;

        public WeatherKind Current { get; private set; } = WeatherKind.Clear;

        /// <summary>0.0 to 1.0 storm intensity for active FalloutStorm (Prompt #40).</summary>
        public float StormIntensity { get; set; } = 1.0f;

        /// <summary>
        /// Air filter degradation multiplier: doubles (2.0x) during intense FalloutStorm
        /// (intensity &gt;= 0.7); BlackRain always 2.5x (oily hyper-radioactive residue).
        /// </summary>
        public float AirFilterDegradationMultiplier
        {
            get
            {
                if (Current == WeatherKind.BlackRain) return 2.5f;
                if (Current == WeatherKind.FalloutStorm && StormIntensity >= 0.7f) return 2.0f;
                return 1.0f;
            }
        }

        /// <summary>
        /// When true, RollNextState excludes Ashfall/FalloutStorm/Blizzard from the weighted
        /// roll regardless of how the active SeasonWindow is authored — guarantees Phase 1
        /// (Civil War) never rolls a post-war hazard. Set by WorldPhaseSystem/GameBootstrap.
        /// </summary>
        public bool RestrictToNonHazardWeather;

        /// <summary>Base seed this system's deterministic roll sequence is derived from.</summary>
        public int Seed => _seed;

        /// <summary>Fired whenever Current actually changes (never on a roll that repeats the current state).</summary>
        public event Action<WeatherKind> OnWeatherChanged;

        private PersonalQuestSystem _personalQuests;
        private Func<IReadOnlyList<Survivor>> _getSurvivors;
        private Func<int> _stationForecastDays;

        /// <summary>
        /// Prompt #903 — the weather station's copied data logger. Returns how many
        /// days of readings the survivors hold (0 when the logger was never copied).
        /// Without this, extracting the data set a world flag nothing ever read.
        /// </summary>
        public void BindStationForecast(Func<int> getStationForecastDays) =>
            _stationForecastDays = getStationForecastDays;

        /// <summary>Prompt #233 — Stormcaller 10-day perfect forecast.</summary>
        public void BindPersonalQuests(
            PersonalQuestSystem personalQuests,
            Func<IReadOnlyList<Survivor>> getSurvivors = null)
        {
            _personalQuests = personalQuests;
            _getSurvivors = getSurvivors;
        }

        /// <summary>
        /// When a Stormcaller is present, returns a perfect forecast of future weather
        /// states (deterministic from seed + roll count). Length is forecastDays.
        ///
        /// The weather station's copied data logger is a second, shorter source: it
        /// caps the forecast at the days of readings it holds rather than granting
        /// the full Stormcaller window.
        /// </summary>
        public WeatherKind[] GetPerfectForecast(int forecastDays = PersonalQuestSystem.StormcallerForecastDays)
        {
            if (forecastDays <= 0) return Array.Empty<WeatherKind>();
            bool hasStormcaller = _personalQuests != null
                && _personalQuests.HasPerfectTenDayForecast(_getSurvivors?.Invoke());
            if (!hasStormcaller)
            {
                int stationDays = _stationForecastDays?.Invoke() ?? 0;
                if (stationDays <= 0) return Array.Empty<WeatherKind>();
                forecastDays = Math.Min(forecastDays, stationDays);
            }

            var result = new WeatherKind[forecastDays];
            // Deterministic preview: reseed from Seed + RollCount + day offset without mutating state.
            int baseRoll = _rollCount;
            for (int d = 0; d < forecastDays; d++)
            {
                int previewSeed = unchecked(_seed * 397) ^ (baseRoll + d + 1) ^ (d * 911);
                var rng = new Random(previewSeed);
                // Simple deterministic map: cycle through kinds weighted by roll.
                double r = rng.NextDouble();
                if (r < 0.35) result[d] = WeatherKind.Clear;
                else if (r < 0.5) result[d] = WeatherKind.Overcast;
                else if (r < 0.65) result[d] = WeatherKind.Rain;
                else if (r < 0.78) result[d] = WeatherKind.Ashfall;
                else if (r < 0.9) result[d] = WeatherKind.FalloutStorm;
                else result[d] = WeatherKind.Blizzard;
            }
            return result;
        }

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

        /// <summary>Visibility factor (0..1): zero during FalloutStorm/BlackRain, reduced during Blizzard, full otherwise.</summary>
        public float VisibilityFactor
        {
            get
            {
                switch (Current)
                {
                    case WeatherKind.FalloutStorm:
                    case WeatherKind.BlackRain:
                        return 0f;
                    case WeatherKind.Blizzard: return BlizzardVisibilityFactor;
                    default: return 1f;
                }
            }
        }

        /// <summary>Extra outdoor dose-rate contributed by the current weather.</summary>
        public float OutdoorRadModifier
        {
            get
            {
                switch (Current)
                {
                    case WeatherKind.BlackRain: return BlackRainOutdoorRadModifier;
                    case WeatherKind.FalloutStorm: return FalloutStormOutdoorRadModifier;
                    default: return 0f;
                }
            }
        }

        /// <summary>True when zero-visibility storm weather blocks scavenging without a full hazmat suit.</summary>
        public bool IsScavengingBlocked(bool hasFullSuit) =>
            (Current == WeatherKind.FalloutStorm || Current == WeatherKind.BlackRain) && !hasFullSuit;

        /// <summary>Hazmat durability multiplier for the current weather (BlackRain melts suits).</summary>
        public float HazmatDegradeMultiplier =>
            Current == WeatherKind.BlackRain ? BlackRainHazmatMeltMultiplier : 1f;

        /// <summary>Perceived-temperature penalty (°C) contributed by a given weather kind; 0 for Clear/Ashfall.</summary>
        public static float TemperaturePenaltyForWeather(WeatherKind kind)
        {
            switch (kind)
            {
                case WeatherKind.Blizzard: return BlizzardTemperaturePenaltyC;
                case WeatherKind.FalloutStorm: return FalloutStormTemperaturePenaltyC;
                case WeatherKind.BlackRain: return BlackRainTemperaturePenaltyC;
                default: return 0f;
            }
        }

        /// <summary>True for post-war hyper-hazard weather (storm / black rain).</summary>
        public static bool IsHyperHazardWeather(WeatherKind kind) =>
            kind == WeatherKind.FalloutStorm || kind == WeatherKind.BlackRain;

        /// <summary>Export a save-safe snapshot sufficient to resume the exact same deterministic sequence.</summary>
        public WeatherState GetState()
        {
            return new WeatherState
            {
                Current = Current,
                Seed = _seed,
                RollCount = _rollCount,
                HoursUntilNextCheck = _hoursUntilNextCheck,
                TotalElapsedHours = _totalElapsedHours,
                RestrictToNonHazardWeather = RestrictToNonHazardWeather
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
            RestrictToNonHazardWeather = state.RestrictToNonHazardWeather;
        }

        private WeatherKind RollNextState()
        {
            var season = _profile.GetSeasonForDay(Mathf.FloorToInt(_totalElapsedHours / 24f));
            var rng = new Random(unchecked(_seed * 397 + _rollCount));
            _rollCount++;

            float clear = Mathf.Max(0f, season.GetWeight(WeatherKind.Clear));
            float rain = Mathf.Max(0f, season.GetWeight(WeatherKind.Rain));
            float overcast = Mathf.Max(0f, season.GetWeight(WeatherKind.Overcast));
            float ashfall = RestrictToNonHazardWeather ? 0f : Mathf.Max(0f, season.GetWeight(WeatherKind.Ashfall));
            float storm = RestrictToNonHazardWeather ? 0f : Mathf.Max(0f, season.GetWeight(WeatherKind.FalloutStorm));
            float blizzard = RestrictToNonHazardWeather ? 0f : Mathf.Max(0f, season.GetWeight(WeatherKind.Blizzard));
            float blackRain = RestrictToNonHazardWeather ? 0f : Mathf.Max(0f, season.GetWeight(WeatherKind.BlackRain));
            float total = clear + rain + overcast + ashfall + storm + blizzard + blackRain;
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
            if (roll < rain)
            {
                return WeatherKind.Rain;
            }
            roll -= rain;
            if (roll < overcast)
            {
                return WeatherKind.Overcast;
            }
            roll -= overcast;
            if (roll < ashfall)
            {
                return WeatherKind.Ashfall;
            }
            roll -= ashfall;
            if (roll < storm)
            {
                return WeatherKind.FalloutStorm;
            }
            roll -= storm;
            if (roll < blizzard)
            {
                return WeatherKind.Blizzard;
            }
            return WeatherKind.BlackRain;
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
