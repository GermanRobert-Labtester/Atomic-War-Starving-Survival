// ============================================================================
// Save Store : UniqueClaimSaveStore
// Core State : Ashfall.Core.UniqueClaimSave
// Host Caller: Main.SetupCollectibles / Main.SaveCollectibles
// Purpose    : Global unique-item claim ledger — which globally unique items
//              have already entered the campaign economy, so no generation
//              channel (scavenging, trade, procedural, scripted) can ever
//              produce a second copy.
// ============================================================================

using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Unique-item claim persistence — thin façade over the Core
    /// <c>SaveStore&lt;T&gt;</c> service (via SaveStoreHub, checksummed
    /// canonical <c>{ State, Checksum }</c> envelope, atomic write, no legacy
    /// bare-state fallback: this section never shipped a pre-envelope format).
    /// Path resolution and error handling live in the service.
    /// </summary>
    public static class UniqueClaimSaveStore
    {
        public const string FileName = "unique_claims_save.json";
        public const string SectionName = "unique_claims";

        private static readonly SaveStore<UniqueClaimSave> s_store =
            SaveStoreHub.Checksummed<UniqueClaimSave>(
                FileName, nameof(UniqueClaimSaveStore), allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCaptureDirect(UniqueClaimSave state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static UniqueClaimSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        public static bool TrySave(UniqueClaimSave state) => s_store.TrySave(state);

        public static UniqueClaimSave? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(UniqueClaimSave state) => s_store.CapturePersisted(state);
    }
}
