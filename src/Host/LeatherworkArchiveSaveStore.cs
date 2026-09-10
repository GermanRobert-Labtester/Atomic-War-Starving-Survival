// ============================================================================
// Save Store : LeatherworkArchiveSaveStore
// Core State : Ashfall.Core.Narrative.LeatherworkArchiveState
// Host Caller: Main.Plans159 / LeatherworkArchiveSystem
// Purpose    : Discovered-record ledger for the tanning/leatherwork knowledge
//              archive (Plan 159 §19) — stable record IDs only, ordinal
//              ordering, tolerant of unknown future IDs. Measurements,
//              failure summaries, formulas and canonical links are NEVER
//              persisted; they derive from current catalog data at runtime.
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Leatherwork archive save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Ships the
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter. No pre-envelope legacy format ever existed for this section.
    /// </summary>
    public static class LeatherworkArchiveSaveStore
    {
        public const string FileName = "leatherwork_archive_save.json";
        public const string SectionName = "leatherwork_archive";

        private static readonly SaveStore<LeatherworkArchiveState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(LeatherworkArchiveSaveStore),
            SchemaVersionedEnvelope<LeatherworkArchiveState>.Encode,
            SchemaVersionedEnvelope<LeatherworkArchiveState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Capture the exact persisted envelope bytes for the campaign aggregate without writing to disk.</summary>
        public static string TryCapturePersisted(LeatherworkArchiveState state) => s_store.CapturePersisted(state);

        public static bool TrySave(LeatherworkArchiveState state) => s_store.TrySave(state);

        public static LeatherworkArchiveState? TryLoad() => s_store.TryLoad();
    }
}
