// ============================================================================
// Save Store : VinylMoraleSaveStore
// Core State : Ashfall.Core.VinylMoraleState
// Host Caller: Main.ShelterSocial / VinylMoraleHostSession
// Purpose    : Vinyl record player collection, broadcast tracks, and shelter morale bonuses
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Vinyl morale save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope, preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter; path resolution, atomic write, and error handling live in the
    /// service.
    /// </summary>
    public static class VinylMoraleSaveStore
    {
        public const string FileName = "vinyl_morale_save.json";
        public const string SectionName = "vinyl_morale";

        private static readonly SaveStore<VinylMoraleState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(VinylMoraleSaveStore),
            SchemaVersionedEnvelope<VinylMoraleState>.Encode,
            SchemaVersionedEnvelope<VinylMoraleState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(VinylMoraleState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static VinylMoraleState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(VinylMoraleState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static VinylMoraleState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(VinylMoraleState state) => s_store.TrySave(state);

        public static VinylMoraleState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(VinylMoraleState state) => s_store.CapturePersisted(state);
    }
}
