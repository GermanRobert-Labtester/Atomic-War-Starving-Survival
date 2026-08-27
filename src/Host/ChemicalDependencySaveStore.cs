// ============================================================================
// Save Store : ChemicalDependencySaveStore
// Core State : Ashfall.Core.Medical.ChemicalDependencyLedgerState
// Host Caller: Main.ShelterBatch3 / ChemicalDependencyHostSession
// Purpose    : Chemical dependency ledger, addiction tolerance, doses, and withdrawal states
// ============================================================================
using Ashfall.Core.Medical;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists ChemicalDependencyLedgerState under
    /// user://chemical_dependency_save.json — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub). Checksummed envelope,
    /// atomic write, and legacy bare-state loading live in the service.
    /// </summary>
    public static class ChemicalDependencySaveStore
    {
        public const string FileName = "chemical_dependency_save.json";
        public const string SectionName = "chemical_dependency";

        private static readonly SaveStore<ChemicalDependencyLedgerState> s_store =
            SaveStoreHub.Checksummed<ChemicalDependencyLedgerState>(FileName, nameof(ChemicalDependencySaveStore));

        public static string SavePath => s_store.SavePath;

        public static bool TrySave(ChemicalDependencyLedgerState state) => s_store.TrySave(state);

        public static ChemicalDependencyLedgerState? TryLoad() => s_store.TryLoad();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(ChemicalDependencyLedgerState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static ChemicalDependencyLedgerState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
    }
}
