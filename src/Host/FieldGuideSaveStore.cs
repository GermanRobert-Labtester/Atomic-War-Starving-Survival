// ============================================================================
// Save Store : FieldGuideSaveStore
// Core State : Ashfall.Core.World.FieldGuideState
// Host Caller: Main.EcologicalInfestations (Plan 28 Phase 5) / Plan 20A
// Purpose    : Field-guide unlocked-entry ledger ("reading the land" and the
//              fauna/flora archive) — closes the Plan 20A GAP row.
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Field-guide save persistence — thin façade over the Core
    /// <see cref="SaveStore{T}"/> service (via SaveStoreHub, checksummed
    /// flavor). Path resolution, atomic write, and error handling live in
    /// the service; unlocks survive campaign save/load.
    /// </summary>
    public static class FieldGuideSaveStore
    {
        public const string FileName = "field_guide_save.json";
        public const string SectionName = "field_guide";

        private static readonly SaveStore<FieldGuideState> s_store = SaveStoreHub.Checksummed<FieldGuideState>(
            FileName,
            nameof(FieldGuideSaveStore));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static bool TrySave(FieldGuideState state) => s_store.TrySave(state);

        public static FieldGuideState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(FieldGuideState state) => s_store.CapturePersisted(state);
    }
}
