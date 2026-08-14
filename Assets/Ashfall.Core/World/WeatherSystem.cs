using System;
using System.Collections.Generic;

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

        public event Action<WeatherKind> OnWeatherChanged;
        public event Action<WorldWeatherState> OnStateChanged;

        public WeatherSystem(WorldWeatherState state = null)
        {
            _state = state ?? new WorldWeatherState();
        }

        public WorldWeatherState State => _state;
        public WeatherKind Current => ParseKind(_state.currentKind);
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
            SeasonWindowDef current = null;
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
            var rng = new Random(unchecked(_seed * 397 + _state.rollCount));
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
                switch (Current)
                {
                    case WeatherKind.BlackRain: return BlackRainOutdoorRadModifier;
                    case WeatherKind.FalloutStorm: return FalloutStormOutdoorRadModifier;
                    default: return 0f;
                }
            }
        }

        public bool IsScavengingBlocked(bool hasFullSuit) =>
            (Current == WeatherKind.FalloutStorm || Current == WeatherKind.BlackRain) && !hasFullSuit;

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
                restrictToNonHazardWeather = _state.restrictToNonHazardWeather
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
            RaiseChanged();
        }

        public void RestrictToNonHazardWeather(bool restrict)
        {
            _state.restrictToNonHazardWeather = restrict;
            RaiseChanged();
        }

        private static WeatherKind ParseKind(string kind)
        {
            return Enum.TryParse(kind, out WeatherKind parsed) ? parsed : WeatherKind.Clear;
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }

    /// <summary>Engine-agnostic loader for weather_seasons.json.</summary>
    public static class WeatherProfileLoader
    {
        public const string FileName = "weather_seasons.json";

        public static SeasonProfileDef Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
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
            catch
            {
                return null;
            }
        }
    }
}
