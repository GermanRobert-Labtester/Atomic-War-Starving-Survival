// ============================================================================
// Save Store : WildlifeTrappingSaveStore
// Core State : Ashfall.Core.WildlifeTrappingState
// Host Caller: Main.ShelterSocial / WildlifeTrappingHostSession
// Purpose    : Wildlife snare traps, bait replenishment, and wasteland game yields
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Wildlife trapping save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope, preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter (presence-only checksum, legacy bare-state fallback); path
    /// resolution, atomic write, and error handling live in the service.
    /// </summary>
    public static class WildlifeTrappingSaveStore
    {
        public const string FileName = "wildlife_trapping_save.json";
        public const string SectionName = "wildlife_trapping";

        private static readonly SaveStore<WildlifeTrappingState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(WildlifeTrappingSaveStore),
            SchemaVersionedEnvelope<WildlifeTrappingState>.Encode,
            SchemaVersionedEnvelope<WildlifeTrappingState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static bool TrySave(WildlifeTrappingState state) => s_store.TrySave(state);

        public static WildlifeTrappingState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(WildlifeTrappingState state) => s_store.CapturePersisted(state);

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(WildlifeTrappingState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static WildlifeTrappingState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(WildlifeTrappingState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static WildlifeTrappingState? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
