// ============================================================================
// Save Store : WastelandMapSaveStore
// Core State : Ashfall.Core.World.WastelandMapState
// Host Caller: Main.Expeditions / WorldHostSession
// Purpose    : Wasteland travel map exploration, discovered POIs, and route fog-of-war
// ============================================================================
using System;
using Ashfall.Core.Save;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Wasteland Map save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub). Checksummed envelope and
    /// atomic write live in the service; this section keeps its historical
    /// strictness of NOT adopting pre-envelope bare-state files. The envelope
    /// DTO stays because the deterministic smoke test round-trips it directly.
    /// </summary>
    public static class WastelandMapSaveStore
    {
        public const string FileName = "wasteland_map_save.json";
        public const string SectionName = "wasteland_map";

        private static readonly SaveStore<WastelandMapState> s_store =
            SaveStoreHub.Checksummed<WastelandMapState>(FileName, nameof(WastelandMapSaveStore), allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(WastelandMapState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static WastelandMapState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(WastelandMapState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static WastelandMapState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(WastelandMapState state) => s_store.TrySave(state);

        public static WastelandMapState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(WastelandMapState state) => s_store.CapturePersisted(state);
    }

    [Serializable]
    public sealed class WastelandMapSaveEnvelope
    {
        public WastelandMapState State;
        public string Checksum;
    }
}
