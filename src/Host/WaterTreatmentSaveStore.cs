// ============================================================================
// Save Store : WaterTreatmentSaveStore
// Core State : Ashfall.Core.WaterTreatmentState
// Host Caller: Main.ShelterInfrastructure / WaterTreatmentHostSession
// Purpose    : Water purification filters, contaminated reservoir levels, and clean water stores
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Water treatment save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope, preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter; path resolution, atomic write, and error handling live in the
    /// service.
    /// </summary>
    public static class WaterTreatmentSaveStore
    {
        public const string FileName = "water_treatment_save.json";
        public const string SectionName = "water_treatment";

        private static readonly SaveStore<WaterTreatmentState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(WaterTreatmentSaveStore),
            SchemaVersionedEnvelope<WaterTreatmentState>.Encode,
            SchemaVersionedEnvelope<WaterTreatmentState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(WaterTreatmentState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static WaterTreatmentState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(WaterTreatmentState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static WaterTreatmentState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(WaterTreatmentState state) => s_store.TrySave(state);

        public static WaterTreatmentState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(WaterTreatmentState state) => s_store.CapturePersisted(state);
    }
}
