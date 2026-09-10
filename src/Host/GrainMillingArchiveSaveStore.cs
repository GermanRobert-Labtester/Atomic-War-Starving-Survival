// ============================================================================
// Save Store : GrainMillingArchiveSaveStore
// Core State : Ashfall.Core.Narrative.GrainMillingArchiveState
// Host Caller: Main.Plans157 / GrainMillingDiscoverySystem
// Purpose    : Discovered-record ledger for the Grain Milling knowledge archive
//              (Plan 157) — stable record IDs only, ordinal ordering,
//              tolerant of unknown future IDs. Industrial measurements and
//              relations are derived from current catalog data at runtime.
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Grain milling archive save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Ships the
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/> adapter.
    /// </summary>
    public static class GrainMillingArchiveSaveStore
    {
        public const string FileName = "grain_milling_archive_save.json";
        public const string SectionName = "grain_milling_archive";

        private static readonly SaveStore<GrainMillingArchiveState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(GrainMillingArchiveSaveStore),
            SchemaVersionedEnvelope<GrainMillingArchiveState>.Encode,
            SchemaVersionedEnvelope<GrainMillingArchiveState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Capture the exact persisted envelope bytes for the campaign aggregate without writing to disk.</summary>
        public static string TryCapturePersisted(GrainMillingArchiveState state) => s_store.CapturePersisted(state);

        public static bool TrySave(GrainMillingArchiveState state) => s_store.TrySave(state);

        public static GrainMillingArchiveState? TryLoad() => s_store.TryLoad();
    }
}
