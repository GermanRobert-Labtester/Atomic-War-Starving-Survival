// ============================================================================
// Save Store : ShelterThermalSaveStore
// Core State : Ashfall.Core.ShelterThermalState
// Host Caller: Main.ShelterInfrastructure / ShelterThermalHostSession
// Purpose    : Shelter thermal insulation, heating zones, fuel consumption, and cold exposure
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Shelter thermal save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope, preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter; path resolution, atomic write, and error handling live in the
    /// service.
    /// </summary>
    public static class ShelterThermalSaveStore
    {
        public const string FileName = "shelter_thermal_save.json";
        public const string SectionName = "shelter_thermal";

        private static readonly SaveStore<ShelterThermalState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ShelterThermalSaveStore),
            SchemaVersionedEnvelope<ShelterThermalState>.Encode,
            SchemaVersionedEnvelope<ShelterThermalState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(ShelterThermalState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static ShelterThermalState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(ShelterThermalState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static ShelterThermalState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(ShelterThermalState state) => s_store.TrySave(state);

        public static ShelterThermalState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(ShelterThermalState state) => s_store.CapturePersisted(state);
    }
}
