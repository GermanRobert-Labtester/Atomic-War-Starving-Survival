// ============================================================================
// Save Store : MedicalSaveStore
// Core State : Ashfall.Core.Medical.ChemicalDependencyLedgerState
// Host Caller: Main.Medical / MedicalHostSession
// Purpose    : Medical ward clinic state, medication regimens, and patient triage queues
// ============================================================================
using Ashfall.Core.Medical;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Medical (Chemical Dependency port) save persistence — thin façade over
    /// the Core SaveStore&lt;T&gt; service (via SaveStoreHub). Checksummed
    /// envelope, atomic write, and legacy bare-state loading live in the service.
    /// </summary>
    public static class MedicalSaveStore
    {
        public const string FileName = "medical_save.json";
        public const string SectionName = "medical";

        private static readonly SaveStore<ChemicalDependencyLedgerState> s_store =
            SaveStoreHub.Checksummed<ChemicalDependencyLedgerState>(FileName, nameof(MedicalSaveStore));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(ChemicalDependencyLedgerState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static ChemicalDependencyLedgerState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(ChemicalDependencyLedgerState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static ChemicalDependencyLedgerState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(ChemicalDependencyLedgerState state) => s_store.TrySave(state);

        public static ChemicalDependencyLedgerState? TryLoad() => s_store.TryLoad();
    }
}
