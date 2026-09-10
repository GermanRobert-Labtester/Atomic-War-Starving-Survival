// ============================================================================
// Save Store : HydroGeologyArchiveSaveStore
// Core State : Ashfall.Core.Narrative.HydroGeologyArchiveState
// Host Caller: Main.Plans154 / HydroGeologyDiscoverySystem
// Purpose    : Discovered-record ledger for the Hydrogeology science archive
//              (Plan 154 Task H) — stable record IDs only, ordinal ordering,
//              tolerant of unknown future IDs. Scientific measurements and
//              relations are derived from current catalog data at runtime.
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Hydrogeology archive save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Ships the
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/> adapter.
    /// </summary>
    public static class HydroGeologyArchiveSaveStore
    {
        public const string FileName = "hydrogeology_archive_save.json";
        public const string SectionName = "hydrogeology_archive";

        private static readonly SaveStore<HydroGeologyArchiveState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(HydroGeologyArchiveSaveStore),
            SchemaVersionedEnvelope<HydroGeologyArchiveState>.Encode,
            SchemaVersionedEnvelope<HydroGeologyArchiveState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Capture the exact persisted envelope bytes for the campaign aggregate without writing to disk.</summary>
        public static string TryCapturePersisted(HydroGeologyArchiveState state) => s_store.CapturePersisted(state);

        public static bool TrySave(HydroGeologyArchiveState state) => s_store.TrySave(state);

        public static HydroGeologyArchiveState? TryLoad() => s_store.TryLoad();
    }
}
