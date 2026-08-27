// ============================================================================
// Save Store : InventorySaveStore
// Core State : Ashfall.Core.Inventory.InventorySaveState
// Host Caller: Main.Inventory / InventoryHostSession
// Purpose    : Shelter storage inventory, container contents, item stacks, and gear durability
// ============================================================================
using Ashfall.Core.Inventory;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Inventory save persistence — thin façade over the Core SaveStore&lt;T&gt;
    /// service (via SaveStoreHub). Checksummed envelope and atomic write live
    /// in the service; this section keeps its historical strictness of NOT
    /// adopting pre-envelope bare-state files.
    /// </summary>
    public static class InventorySaveStore
    {
        public const string FileName = "inventory_save.json";
        public const string SectionName = "inventory";

        private static readonly SaveStore<InventorySaveState> s_store =
            SaveStoreHub.Checksummed<InventorySaveState>(FileName, nameof(InventorySaveStore), allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(InventorySaveState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static InventorySaveState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(InventorySaveState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static InventorySaveState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(InventorySaveState state) => s_store.TrySave(state);

        public static InventorySaveState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(InventorySaveState state) => s_store.CapturePersisted(state);
    }
}
