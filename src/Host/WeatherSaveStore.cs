// ============================================================================
// Save Store : WeatherSaveStore
// Core State : Ashfall.Core.WorldWeatherState
// Host Caller: Main.World / WeatherHostSession
// Purpose    : Atmospheric weather simulation, fallout storms, and temperature forecasts
// ============================================================================
using System;
using Ashfall.Core.Save;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists WorldWeatherState under user://weather_save.json.
    /// Thin façade over the Core SaveStore&lt;T&gt; service (via SaveStoreHub):
    /// checksummed envelope, atomic write, checksum validation, and legacy
    /// bare-state loading live in the service. This class only pins the
    /// section identity and the static call surface used by the host.
    /// </summary>
    public static class WeatherSaveStore
    {
        public const string FileName = "weather_save.json";
        public const string SectionName = "weather";

        private static readonly SaveStore<WorldWeatherState> s_store =
            SaveStoreHub.Checksummed<WorldWeatherState>(FileName, nameof(WeatherSaveStore));

        public static string SavePath => s_store.SavePath;

        public static bool TrySave(WorldWeatherState state) => s_store.TrySave(state);

        public static WorldWeatherState? TryLoad() => s_store.TryLoad();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(WorldWeatherState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static WorldWeatherState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
    }
}
