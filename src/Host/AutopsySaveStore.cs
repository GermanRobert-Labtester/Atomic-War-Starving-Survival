// ============================================================================
// Save Store : AutopsySaveStore
// Core State : Ashfall.Core.AutopsyState
// Host Caller: Main.ShelterInfrastructure / AutopsyHostSession
// Purpose    : Autopsy examination procedures, pathogen samples, and clinical findings
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Autopsy save persistence — thin façade over the Core SaveStore&lt;T&gt;
    /// service (via SaveStoreHub, codec flavor). This shelter-batch section
    /// ships the legacy <c>{ SchemaVersion, State, Checksum }</c> envelope,
    /// preserved byte-for-byte by the Core
    /// <see cref="SchemaVersionedEnvelope{T}"/> adapter; path resolution,
    /// atomic write, and error handling live in the service.
    /// </summary>
    public static class AutopsySaveStore
    {
        public const string FileName = "autopsy_save.json";
        public const string SectionName = "autopsy";

        private static readonly SaveStore<AutopsyState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(AutopsySaveStore),
            SchemaVersionedEnvelope<AutopsyState>.Encode,
            SchemaVersionedEnvelope<AutopsyState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(AutopsyState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static AutopsyState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(AutopsyState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static AutopsyState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(AutopsyState state) => s_store.TrySave(state);

        public static AutopsyState? TryLoad() => s_store.TryLoad();
    }
}
