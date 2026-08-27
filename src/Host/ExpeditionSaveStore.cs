// ============================================================================
// Save Store : ExpeditionSaveStore
// Core State : List<Ashfall.Core.Expeditions.ExpeditionState>
// Host Caller: Main.Expeditions / ExpeditionHostSession
// Purpose    : Active and completed expedition sorties, route waypoints, and field loot
// ============================================================================
using System.Collections.Generic;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Expedition (Encounters port) save persistence — thin façade over the
    /// Core SaveStore&lt;T&gt; service (via SaveStoreHub). Checksummed envelope,
    /// atomic write, and legacy bare-state loading live in the service.
    /// </summary>
    public static class ExpeditionSaveStore
    {
        public const string FileName = "expedition_save.json";
        public const string SectionName = "expedition";

        private static readonly SaveStore<List<ExpeditionState>> s_store =
            SaveStoreHub.Checksummed<List<ExpeditionState>>(FileName, nameof(ExpeditionSaveStore));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(List<ExpeditionState> state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static List<ExpeditionState>? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(List<ExpeditionState> state) => s_store.TrySave(state);

        public static List<ExpeditionState>? TryLoad() => s_store.TryLoad();
    }
}
