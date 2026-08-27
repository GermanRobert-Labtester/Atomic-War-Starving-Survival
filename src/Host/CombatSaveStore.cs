// ============================================================================
// Save Store : CombatSaveStore
// Core State : Ashfall.Core.Combat.CombatState
// Host Caller: Main.Expeditions / CombatHostSession
// Purpose    : Tactical combat encounters, ballistics resolution, enemy status, and weapon wear
// ============================================================================
using Ashfall.Core.Combat;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Combat save persistence — thin façade over the Core SaveStore&lt;T&gt;
    /// service (via SaveStoreHub). Checksummed envelope, atomic write, and
    /// legacy bare-state loading live in the service.
    /// </summary>
    public static class CombatSaveStore
    {
        public const string FileName = "combat_save.json";
        public const string SectionName = "combat";

        private static readonly SaveStore<CombatState> s_store =
            SaveStoreHub.Checksummed<CombatState>(FileName, nameof(CombatSaveStore));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(CombatState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static CombatState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(CombatState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static CombatState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(CombatState state) => s_store.TrySave(state);

        public static CombatState? TryLoad() => s_store.TryLoad();
    }
}
