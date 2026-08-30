// ============================================================================
// Save Store : MedicalPipelineSaveStore
// Core State : Ashfall.Core.Medical.MedicalPipelineSaveState
// Host Caller: Main.Medical / MedicalHostSession
// Purpose    : Diagnosis knowledge, reservations, and scheduled procedures
//              (Task #133 unified medical pipeline).
// ============================================================================
using Ashfall.Core.Medical;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Medical pipeline save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub). Checksummed envelope,
    /// atomic write. Missing files load as an empty pipeline (fresh campaigns);
    /// corrupt state fails the load with the store's standard diagnostics.
    /// </summary>
    public static class MedicalPipelineSaveStore
    {
        public const string FileName = "medical_pipeline_save.json";
        public const string SectionName = "medical_pipeline";

        private static readonly SaveStore<MedicalPipelineSaveState> s_store =
            SaveStoreHub.Checksummed<MedicalPipelineSaveState>(FileName, nameof(MedicalPipelineSaveStore));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static bool TrySave(MedicalPipelineSaveState state) => s_store.TrySave(state);

        public static MedicalPipelineSaveState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(MedicalPipelineSaveState state) => s_store.CapturePersisted(state);
    }
}
