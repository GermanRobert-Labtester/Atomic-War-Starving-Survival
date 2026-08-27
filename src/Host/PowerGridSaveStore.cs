// ============================================================================
// Save Store : PowerGridSaveStore
// Core State : Ashfall.Core.Shelter.PowerGridSave
// Host Caller: Main.ShelterInfrastructure / PowerGridHostSession
// Purpose    : Shelter electrical power grid, generator fuel, battery capacity, and blackout zones
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="PowerGridSave"/> as JSON under
    /// <c>user://power_grid_save.json</c> — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Shape and
    /// checksum live in <see cref="PowerGridSaveCodec"/>; path resolution,
    /// atomic write, and error handling live in the service.
    /// </summary>
    public static class PowerGridSaveStore
    {
        public const string FileName = "power_grid_save.json";
        public const string SectionName = "power_grid";

        private static readonly SaveStore<PowerGridSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(PowerGridSaveStore),
            (save, json) => PowerGridSaveCodec.EncodeToString(save, json),
            (raw, json) => PowerGridSaveCodec.Decode(raw, json));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(PowerGridSave state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static PowerGridSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(PowerGridSave state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static PowerGridSave? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(PowerGridSave save) => s_store.TrySave(save);

        public static PowerGridSave? TryLoad() => s_store.TryLoad();
    }
}
