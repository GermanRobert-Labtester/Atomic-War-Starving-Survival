// ============================================================================
// Save Store : CraftingSaveStore
// Core State : Ashfall.Core.CraftingSystemSave
// Host Caller: Main.World / CraftingHostSession
// Purpose    : Crafting queue, unlocked workshop recipes, and workbench upgrade levels
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Crafting;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Crafting save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub): checksummed
    /// <c>{ State, Checksum }</c> envelope, atomic write, checksum validation,
    /// and error handling live in the service. This section never shipped a
    /// pre-checksum bare-state format, so legacy bare-state loading is off.
    /// </summary>
    public static class CraftingSaveStore
    {
        public const string FileName = "crafting_save.json";
        public const string SectionName = "crafting";

        private static readonly SaveStore<CraftingSystemSave> s_store =
            SaveStoreHub.Checksummed<CraftingSystemSave>(FileName, nameof(CraftingSaveStore), allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static bool TrySave(CraftingSystemSave state) => s_store.TrySave(state);

        public static CraftingSystemSave? TryLoad() => s_store.TryLoad();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(CraftingSystemSave state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static CraftingSystemSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(CraftingSystemSave state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static CraftingSystemSave? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
