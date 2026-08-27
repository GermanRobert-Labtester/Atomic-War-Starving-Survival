// ============================================================================
// Save Store : CaregivingSaveStore
// Core State : Ashfall.Core.CaregivingSaveState
// Host Caller: Main.ShelterSocial / CaregivingHostSession
// Purpose    : Caregiving assignments, dependent survivor care, and morale buffers
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Caregiving save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope, preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter; path resolution, atomic write, and error handling live in the
    /// service.
    /// </summary>
    public static class CaregivingSaveStore
    {
        public const string FileName = "caregiving_save.json";
        public const string SectionName = "caregiving";

        private static readonly SaveStore<CaregivingSaveState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(CaregivingSaveStore),
            SchemaVersionedEnvelope<CaregivingSaveState>.Encode,
            SchemaVersionedEnvelope<CaregivingSaveState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(CaregivingSaveState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static CaregivingSaveState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(CaregivingSaveState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static CaregivingSaveState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(CaregivingSaveState state) => s_store.TrySave(state);

        public static CaregivingSaveState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(CaregivingSaveState state) => s_store.CapturePersisted(state);
    }
}
