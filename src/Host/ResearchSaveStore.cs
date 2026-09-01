// ============================================================================
// Save Store : ResearchSaveStore
// Core State : Ashfall.Core.ResearchState
// Host Caller: Main.ExpandedShelterSystems (EnsureSharedResearch / SaveResearch)
// Purpose    : Research knowledge progress — unlocked, active, and completed nodes
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Research save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor).
    /// Added by Plan 34: research progress is ID-based and previously was
    /// never persisted at all. Ships the legacy
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope via
    /// <see cref="SchemaVersionedEnvelope{T}"/>; path resolution, atomic
    /// write, and error handling live in the service.
    /// </summary>
    public static class ResearchSaveStore
    {
        public const string FileName = "research_save.json";
        public const string SectionName = "research";

        private static readonly SaveStore<ResearchState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ResearchSaveStore),
            SchemaVersionedEnvelope<ResearchState>.Encode,
            SchemaVersionedEnvelope<ResearchState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(ResearchState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static ResearchState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(ResearchState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static ResearchState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(ResearchState state) => s_store.TrySave(state);

        public static ResearchState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(ResearchState state) => s_store.CapturePersisted(state);
    }
}
