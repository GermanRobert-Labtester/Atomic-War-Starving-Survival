// ============================================================================
// Save Store : ExcavationSaveStore
// Core State : Ashfall.Core.ExcavationState
// Host Caller: Main.ShelterSocial / ExcavationHostSession
// Purpose    : Shelter room excavation progress, structural clearance, and expansion rubble
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Excavation save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope, preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter; path resolution, atomic write, and error handling live in the
    /// service.
    /// </summary>
    public static class ExcavationSaveStore
    {
        public const string FileName = "excavation_save.json";
        public const string SectionName = "excavation";

        private static readonly SaveStore<ExcavationState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ExcavationSaveStore),
            SchemaVersionedEnvelope<ExcavationState>.Encode,
            SchemaVersionedEnvelope<ExcavationState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(ExcavationState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static ExcavationState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(ExcavationState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static ExcavationState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(ExcavationState state) => s_store.TrySave(state);

        public static ExcavationState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(ExcavationState state) => s_store.CapturePersisted(state);
    }
}
