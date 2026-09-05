// ============================================================================
// Save Store : CollectibleDiscoverySaveStore
// Core State : Ashfall.Core.CollectibleDiscoverySave
// Host Caller: Main.SetupCollectibles / Main.SaveCollectibles
// Purpose    : One-time collectible discovery ledger — which collectible
//              effects have already been handled for this campaign, so
//              selling, re-acquiring, or reloading can never replay them.
// ============================================================================

using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Collectible discovery persistence — thin façade over the Core
    /// <c>SaveStore&lt;T&gt;</c> service (via SaveStoreHub, checksummed
    /// canonical <c>{ State, Checksum }</c> envelope, atomic write, no legacy
    /// bare-state fallback: this section never shipped a pre-envelope format).
    /// Path resolution and error handling live in the service.
    /// </summary>
    public static class CollectibleDiscoverySaveStore
    {
        public const string FileName = "collectible_discovery_save.json";
        public const string SectionName = "collectible_discovery";

        private static readonly SaveStore<CollectibleDiscoverySave> s_store =
            SaveStoreHub.Checksummed<CollectibleDiscoverySave>(
                FileName, nameof(CollectibleDiscoverySaveStore), allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCaptureDirect(CollectibleDiscoverySave state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static CollectibleDiscoverySave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        public static bool TrySave(CollectibleDiscoverySave state) => s_store.TrySave(state);

        public static CollectibleDiscoverySave? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(CollectibleDiscoverySave state) => s_store.CapturePersisted(state);
    }
}
