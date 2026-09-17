// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

using Ashfall.Core.IO;
namespace Ashfall.Core.World
{
    /// <summary>One seasonal weather window (the JSON is the authority).</summary>
    [Serializable]
    public class SeasonWindowDef
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public int startDay = 0;
        public float clearWeight = 1f;
        public float rainWeight = 1f;
        public float overcastWeight = 1f;
        public float ashfallWeight = 1f;
        public float falloutStormWeight = 1f;
        public float blizzardWeight = 1f;
        public float blackRainWeight = 1f;
    }

    /// <summary>The campaign weather profile (mirrors Unity SeasonProfile).</summary>
    [Serializable]
    public class SeasonProfileDef
    {
        public string id = "default_winter";
        public string displayName = "The Long Winter";
        public float weatherCheckIntervalHours = 6f;
        public List<SeasonWindowDef> seasons = new List<SeasonWindowDef>();
    }

    /// <summary>Serialized weather state (save/load safe; rolls resume identically).</summary>
    [Serializable]
    public class WorldWeatherState
    {
        public string systemId = WeatherSystem.SystemId;
        public string currentKind = "Clear";
        public float totalElapsedHours = 0f;
        public float hoursUntilNextCheck = 0f;
        public int rollCount = 0;
        public bool restrictToNonHazardWeather = false;

        // ── Plan 205: deterministic surface wind (single weather authority) ──
        public float wind_direction_deg = 0f;
        public float wind_speed_kph = 0f;
    }

    /// <summary>
    /// Engine-agnostic port of the Unity WeatherSystem (Assets/_Game/Environment/
    /// WeatherSystem.cs): seeded weighted-random state transitions against the
    /// active season window, deterministic for save/load (each roll reseeds fresh
    /// from seed + rollCount instead of persisting RNG state), plus the weather
    /// modifiers (visibility, outdoor rad, hazmat melt, temperature penalty).
    /// </summary>
    public class WeatherSystem
    {
        public const string SystemId = "world_weather_system";

        public const float FalloutStormOutdoorRadModifier = 150f;
        public const float BlackRainOutdoorRadModifier = 250f;
        public const float BlackRainHazmatMeltMultiplier = 5f;
        public const float BlizzardTemperaturePenaltyC = -15f;
        public const float FalloutStormTemperaturePenaltyC = -5f;
        public const float BlackRainTemperaturePenaltyC = -8f;
        public const float BlizzardVisibilityFactor = 0.4f;

        private readonly WorldWeatherState _state;
        private SeasonProfileDef _profile;
        private int _seed;
        private WeatherEffectsCatalog? _effectsCatalog;

        public event Action<WeatherKind> OnWeatherChanged;
        public event Action<WorldWeatherState> OnStateChanged;

        public WeatherSystem(WorldWeatherState? state = null)
        {
            _state = state ?? new WorldWeatherState();
        }

        public WorldWeatherState State => _state;
        public WeatherKind Current => ParseKind(_state.currentKind);

        /// <summary>
        /// C2 / Plan 20A (G1) — binds the data-authored weather-effects
        /// authority. When bound, OutdoorRadModifier and the forecast rad
        /// projection read the one shared table (plan §57.3: forecast and
        /// runtime can never drift). Unbound callers keep the legacy Core
        /// constants byte-for-byte.
        /// </summary>
        public void BindWeatherEffects(WeatherEffectsCatalog? catalog)
        {
            _effectsCatalog = catalog;
        }

        // ── Plan 205: surface wind projections (degrees; kph) ──
        public float WindDirectionDeg => _state.wind_direction_deg;
        public float WindSpeedKph => _state.wind_speed_kph;
        public int Seed => _seed;

        // ── Profile ────────────────────────────────────────────────────

        public void BindProfile(SeasonProfileDef profile, int seed)
        {
            _profile = profile ?? new SeasonProfileDef();
            _seed = seed;
        }

        public SeasonWindowDef GetSeasonForDay(int day)
        {
            if (_profile == null || _profile.seasons == null || _profile.seasons.Count == 0)
                return DefaultWindow;
            SeasonWindowDef? current = null;
            for (int i = 0; i < _profile.seasons.Count; i++)
            {
                if (_profile.seasons[i] != null && _profile.seasons[i].startDay <= day)
                    current = _profile.seasons[i];
            }
            return current ?? DefaultWindow;
        }

        private static readonly SeasonWindowDef DefaultWindow = new SeasonWindowDef
        {
            id = "default",
            displayName = "Default",
            clearWeight = 1f,
            rainWeight = 1f,
            overcastWeight = 1f,
            ashfallWeight = 1f,
            falloutStormWeight = 1f,
            blizzardWeight = 1f,
            blackRainWeight = 0f // Unity parity: SeasonProfile.Default keeps Black Rain rare (0)
        };

        // ── Tick ───────────────────────────────────────────────────────

        public void Tick(float gameHours)
        {
            if (_profile == null || gameHours <= 0f)
                return;

            _state.totalElapsedHours += gameHours;
            _state.hoursUntilNextCheck -= gameHours;

            int safety = 0;
            while (_state.hoursUntilNextCheck <= 0f && safety < 10000)
            {
                _state.hoursUntilNextCheck += NextCheckInterval();
                SetCurrent(RollNextState());
                safety++;
            }
            RaiseChanged();
        }

        /// <summary>Force a specific weather state (debug / scripted events).</summary>
        public void ForceWeather(WeatherKind kind)
        {
            _state.hoursUntilNextCheck = NextCheckInterval();
            SetCurrent(kind);
            RaiseChanged();
        }

        private WeatherKind RollNextState()
        {
            var season = GetSeasonForDay((int)Math.Floor(_state.totalElapsedHours / 24f));
            var rng = new SeededRng(unchecked(_seed * 397 + _state.rollCount));
            _state.rollCount++;

            bool restrict = _state.restrictToNonHazardWeather;
            float clear = Math.Max(0f, season.clearWeight);
            float rain = Math.Max(0f, season.rainWeight);
            float overcast = Math.Max(0f, season.overcastWeight);
            float ashfall = restrict ? 0f : Math.Max(0f, season.ashfallWeight);
            float storm = restrict ? 0f : Math.Max(0f, season.falloutStormWeight);
            float blizzard = restrict ? 0f : Math.Max(0f, season.blizzardWeight);
            float blackRain = restrict ? 0f : Math.Max(0f, season.blackRainWeight);
            float total = clear + rain + overcast + ashfall + storm + blizzard + blackRain;
            if (total <= 0f)
                return WeatherKind.Clear;

            double roll = rng.NextDouble() * total;
            var kind = RollKind(roll, clear, rain, overcast, ashfall, storm, blizzard, blackRain);

            // Plan 205: wind accompanies every weather roll. Storm kinds run a
            // higher base speed band; calm kinds stay light. Deterministic from
            // the same seeded rng instance as the kind draw.
            double windRoll = rng.NextDouble();
            double speedRoll = rng.NextDouble();
            bool severe = kind is WeatherKind.FalloutStorm or WeatherKind.Blizzard
                or WeatherKind.BlackRain or WeatherKind.AcidSnow
                or WeatherKind.BlackSnow or WeatherKind.BloodRain;
            float baseSpeed = severe ? 18f : 6f;
            float bandWidth = severe ? 22f : 10f;
            _state.wind_direction_deg = (float)(windRoll * 360.0);
            _state.wind_speed_kph = baseSpeed + (float)(speedRoll * bandWidth);

            return kind;
        }

        private static WeatherKind RollKind(double roll, float clear, float rain, float overcast,
            float ashfall, float storm, float blizzard, float blackRain)
        {
            if (roll < clear) return WeatherKind.Clear;
            roll -= clear;
            if (roll < rain) return WeatherKind.Rain;
            roll -= rain;
            if (roll < overcast) return WeatherKind.Overcast;
            roll -= overcast;
            if (roll < ashfall) return WeatherKind.Ashfall;
            roll -= ashfall;
            if (roll < storm) return WeatherKind.FalloutStorm;
            roll -= storm;
            if (roll < blizzard) return WeatherKind.Blizzard;
            return WeatherKind.BlackRain;
        }

        private void SetCurrent(WeatherKind next)
        {
            if (next == Current)
                return;
            _state.currentKind = next.ToString();
            OnWeatherChanged?.Invoke(next);
        }

        private float NextCheckInterval()
        {
            return Math.Max(0.01f,
                _profile != null ? _profile.weatherCheckIntervalHours : 6f);
        }

        // ── Modifiers (Unity parity) ───────────────────────────────────

        public float VisibilityFactor
        {
            get
            {
                // C2 / Plan 20C — data authority first (table row); legacy
                // switch fallback when unbound.
                if (_effectsCatalog != null && _effectsCatalog.TryGetEffects(Current, out var effects)
                    && effects != null)
                    return Math.Clamp(effects.visibility_modifier, 0f, 1f);

                switch (Current)
                {
                    case WeatherKind.FalloutStorm:
                    case WeatherKind.BlackRain:
                        return 0f;
                    case WeatherKind.Blizzard:
                        return BlizzardVisibilityFactor;
                    default:
                        return 1f;
                }
            }
        }

        public float OutdoorRadModifier
        {
            get
            {
                // C2 / Plan 20A (G1) — data authority first; the catalog is
                // validated to carry an explicit row for every WeatherKind.
                if (_effectsCatalog != null && _effectsCatalog.TryGetModifier(Current, out float fromData))
                    return fromData;

                switch (Current)
                {
                    case WeatherKind.BlackRain: return BlackRainOutdoorRadModifier;
                    case WeatherKind.FalloutStorm: return FalloutStormOutdoorRadModifier;
                    default: return 0f;
                }
            }
        }

        /// <summary>C2 / Plan 20A (G1) — forecast rad projection for a kind,
        /// from the same weather-effects table the runtime dose consumes.
        /// Legacy fallback preserves the pre-catalog projection constants.</summary>
        public float ForecastRadModifier(WeatherKind kind)
        {
            if (_effectsCatalog != null && _effectsCatalog.TryGetModifier(kind, out float fromData))
                return fromData;

            return kind switch
            {
                WeatherKind.BlackRain => BlackRainOutdoorRadModifier,
                WeatherKind.FalloutStorm => FalloutStormOutdoorRadModifier,
                WeatherKind.Ashfall => 45.0f,
                _ => 0f
            };
        }

        public bool IsScavengingBlocked(bool hasFullSuit) =>
            (Current == WeatherKind.FalloutStorm || Current == WeatherKind.BlackRain) && !hasFullSuit;

        public float GetTemperaturePenaltyCelsius()
        {
            return Current switch
            {
                WeatherKind.Blizzard => BlizzardTemperaturePenaltyC,
                WeatherKind.FalloutStorm => FalloutStormTemperaturePenaltyC,
                WeatherKind.BlackRain => BlackRainTemperaturePenaltyC,
                _ => 0f
            };
        }

        public float HazmatDegradeMultiplier =>
            Current == WeatherKind.BlackRain ? BlackRainHazmatMeltMultiplier : 1f;

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

        /// <summary>
        /// C2 / Plan 20C (§42) — shelter thermal load delta from the bound
        /// weather-effects table (data authority); falls back to the legacy
        /// static constants when unbound. This is THE thermal coupling: cold
        /// weather reaches heating/fuel pressure through the existing
        /// temperature path — never a second fuel drain.
        /// </summary>
        public float TemperaturePenaltyC(WeatherKind kind)
        {
            if (_effectsCatalog != null && _effectsCatalog.TryGetEffects(kind, out var effects)
                && effects != null)
                return effects.thermal_load_additive_c;
            return TemperaturePenaltyForWeather(kind);
        }

        /// <summary>
        /// C2 / Plan 20C (§45) — visibility fraction 0..1 from the bound table;
        /// legacy switch fallback when unbound (same values the forecast
        /// projection previously hardcoded).
        /// </summary>
        public float VisibilityModifier(WeatherKind kind)
        {
            if (_effectsCatalog != null && _effectsCatalog.TryGetEffects(kind, out var effects)
                && effects != null)
                return Math.Clamp(effects.visibility_modifier, 0f, 1f);

            return kind switch
            {
                WeatherKind.FalloutStorm or WeatherKind.BlackRain => 0f,
                WeatherKind.Blizzard => BlizzardVisibilityFactor,
                WeatherKind.Ashfall => 0.65f,
                _ => 1f
            };
        }

        // ── Save / Load ────────────────────────────────────────────────

        public WorldWeatherState CaptureState()
        {
            return new WorldWeatherState
            {
                systemId = _state.systemId,
                currentKind = _state.currentKind,
                totalElapsedHours = _state.totalElapsedHours,
                hoursUntilNextCheck = _state.hoursUntilNextCheck,
                rollCount = _state.rollCount,
                restrictToNonHazardWeather = _state.restrictToNonHazardWeather,
                wind_direction_deg = _state.wind_direction_deg,
                wind_speed_kph = _state.wind_speed_kph
            };
        }

        public void RestoreState(WorldWeatherState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _state.currentKind = saved.currentKind;
            _state.totalElapsedHours = Math.Max(0f, saved.totalElapsedHours);
            _state.hoursUntilNextCheck = saved.hoursUntilNextCheck;
            _state.rollCount = Math.Max(0, saved.rollCount);
            _state.restrictToNonHazardWeather = saved.restrictToNonHazardWeather;
            _state.wind_direction_deg = saved.wind_direction_deg;
            _state.wind_speed_kph = saved.wind_speed_kph;
            RaiseChanged();
        }

        public void RestrictToNonHazardWeather(bool restrict)
        {
            _state.restrictToNonHazardWeather = restrict;
            RaiseChanged();
        }

        /// <summary>
        /// Deterministically peeks upcoming forecast for N days ahead without mutating simulation roll count or RNG state.
        /// </summary>
        public List<WeatherForecastEntry> PeekForecast(int daysAhead = 3)
        {
            var list = new List<WeatherForecastEntry>();
            int currentDay = (int)Math.Floor(_state.totalElapsedHours / 24f);

            for (int i = 0; i < daysAhead; i++)
            {
                int targetDay = currentDay + i;
                var season = GetSeasonForDay(targetDay);
                var rng = new SeededRng(unchecked(_seed * 397 + (_state.rollCount + i)));

                bool restrict = _state.restrictToNonHazardWeather;
                float clear = Math.Max(0f, season.clearWeight);
                float rain = Math.Max(0f, season.rainWeight);
                float overcast = Math.Max(0f, season.overcastWeight);
                float ashfall = restrict ? 0f : Math.Max(0f, season.ashfallWeight);
                float storm = restrict ? 0f : Math.Max(0f, season.falloutStormWeight);
                float blizzard = restrict ? 0f : Math.Max(0f, season.blizzardWeight);
                float blackRain = restrict ? 0f : Math.Max(0f, season.blackRainWeight);
                float total = clear + rain + overcast + ashfall + storm + blizzard + blackRain;

                WeatherKind predicted = WeatherKind.Clear;
                if (i == 0)
                {
                    predicted = Current;
                }
                else if (total > 0f)
                {
                    double roll = rng.NextDouble() * total;
                    if (roll < clear) predicted = WeatherKind.Clear;
                    else if ((roll -= clear) < rain) predicted = WeatherKind.Rain;
                    else if ((roll -= rain) < overcast) predicted = WeatherKind.Overcast;
                    else if ((roll -= overcast) < ashfall) predicted = WeatherKind.Ashfall;
                    else if ((roll -= ashfall) < storm) predicted = WeatherKind.FalloutStorm;
                    else if ((roll -= storm) < blizzard) predicted = WeatherKind.Blizzard;
                    else predicted = WeatherKind.BlackRain;
                }

                float rad = ForecastRadModifier(predicted);

                float vis = VisibilityModifier(predicted);

                list.Add(new WeatherForecastEntry
                {
                    Day = targetDay + 1,
                    Kind = predicted,
                    OutdoorRad = rad,
                    Visibility = vis,
                    Summary = predicted.ToString(),
                    ThermalLoadC = TemperaturePenaltyC(predicted),
                    TravelSpeedMultiplier = EffectsFor(predicted).travel_speed_multiplier,
                    TravelEncounterMultiplier = EffectsFor(predicted).travel_encounter_multiplier,
                    TrapYieldMultiplier = EffectsFor(predicted).trap_yield_multiplier,
                    CaravanAvailabilityMultiplier = EffectsFor(predicted).caravan_availability_multiplier
                });
            }

            return list;
        }

        private static WeatherKind ParseKind(string kind)
        {
            return Enum.TryParse(kind, out WeatherKind parsed) ? parsed : WeatherKind.Clear;
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);

        /// <summary>C2 / Plan 20C (§45) — the full effects row for a kind
        /// (legacy-identity fallback when the catalog is unbound).</summary>
        private WeatherEffectsDef EffectsFor(WeatherKind kind)
        {
            if (_effectsCatalog != null && _effectsCatalog.TryGetEffects(kind, out var fx)
                && fx != null)
                return fx;
            return new WeatherEffectsDef();
        }
    }

    /// <summary>
    /// Deterministic daily weather forecast entry.
    /// </summary>
    [Serializable]
    public class WeatherForecastEntry
    {
        public int Day;
        public WeatherKind Kind;
        public float OutdoorRad;
        public float Visibility;
        public string Summary = string.Empty;

        // C2 / Plan 20C (§45) — decision-relevant effects from the ONE
        // weather-effects table (populated in PeekForecast; legacy defaults
        // when unbound: thermal from the static curve, multipliers neutral).
        public float ThermalLoadC;
        public float TravelSpeedMultiplier = 1f;
        public float TravelEncounterMultiplier = 1f;
        public float TrapYieldMultiplier = 1f;
        public float CaravanAvailabilityMultiplier = 1f;
    }


    /// <summary>Engine-agnostic loader for weather_seasons.json.</summary>
    public static class WeatherProfileLoader
    {
        public const string FileName = "weather_seasons.json";

        public static SeasonProfileDef? Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return null;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return null;

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            try
            {
                var parsed = json.Deserialize<SeasonProfileDef>(raw);
                if (parsed != null && parsed.seasons == null)
                    parsed.seasons = new List<SeasonWindowDef>();
                return parsed;
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(path, "SeasonProfileDef", ex_CATDIAG);
                return null;
            }
        }
    }
}
