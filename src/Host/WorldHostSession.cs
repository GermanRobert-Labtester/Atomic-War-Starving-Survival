using System;
using Ashfall.Core;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the World port (weather core). Loads the
    /// season profile JSON, ticks the weather clock, persists state. No rules
    /// here — hosts only wire and present.
    /// </summary>
    public sealed class WorldHostSession
    {
        public const int DemoSeed = 1234;

        public WeatherSystem Weather { get; }
        public SeasonProfileDef Profile { get; private set; }

        public string LastEvent { get; private set; } = string.Empty;

        public event Action StateChanged;

        public WorldHostSession(WeatherSystem weather = null)
        {
            Weather = weather ?? new WeatherSystem();
            Weather.OnWeatherChanged += kind =>
            {
                LastEvent = $"Weather: {kind}";
                StateChanged?.Invoke();
            };
            Weather.OnStateChanged += _ => StateChanged?.Invoke();
        }

        public static WorldHostSession Create(string dataDir)
        {
            var session = new WorldHostSession();
            var profile = !string.IsNullOrEmpty(dataDir)
                ? WeatherProfileLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer())
                : null;
            if (profile != null)
            {
                session.Profile = profile;
                session.Weather.BindProfile(profile, DemoSeed);
            }
            var save = WorldSaveStore.TryLoad();
            if (save != null)
            {
                session.Weather.RestoreState(save);
                session.LastEvent = "World state restored from save.";
            }
            return session;
        }

        // ── Demo actions ─────────────────────────────────────────────

        public string TickDemo(float hours)
        {
            Weather.Tick(hours);
            return $"Tick {hours}h: {Weather.Current} (rolls {Weather.State.rollCount}).";
        }

        public string ForceDemo(WeatherKind kind)
        {
            Weather.ForceWeather(kind);
            return $"Weather forced to {kind}.";
        }

        public string StatusLine()
        {
            return $"Weather: {Weather.Current} · visibility {Weather.VisibilityFactor:P0} · " +
                   $"outdoor rad {Weather.OutdoorRadModifier:0} · " +
                   $"temp penalty {WeatherSystem.TemperaturePenaltyForWeather(Weather.Current):0}°C";
        }

        // ── Save / Load ──────────────────────────────────────────────

        public WorldWeatherState CaptureSave() => Weather.CaptureState();
        public void RestoreSave(WorldWeatherState state) => Weather.RestoreState(state);
    }
}
