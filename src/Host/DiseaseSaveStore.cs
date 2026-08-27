// ============================================================================
// Save Store : DiseaseSaveStore
// Core State : Ashfall.Core.Disease.DiseaseSystemState
// Host Caller: Main.Medical / DiseaseHostSession
// Purpose    : Disease contagion tracking, infection spread, symptoms, and outbreak protocols
// ============================================================================
using Ashfall.Core.Disease;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Disease save persistence — thin façade over the Core SaveStore&lt;T&gt;
    /// service (via SaveStoreHub). Checksummed envelope, atomic write, and
    /// legacy bare-state loading live in the service.
    /// </summary>
    public static class DiseaseSaveStore
    {
        public const string FileName = "disease_save.json";
        public const string SectionName = "disease";

        private static readonly SaveStore<DiseaseSystemState> s_store =
            SaveStoreHub.Checksummed<DiseaseSystemState>(FileName, nameof(DiseaseSaveStore));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(DiseaseSystemState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static DiseaseSystemState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(DiseaseSystemState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static DiseaseSystemState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(DiseaseSystemState state) => s_store.TrySave(state);

        public static DiseaseSystemState? TryLoad() => s_store.TryLoad();
    }
}
