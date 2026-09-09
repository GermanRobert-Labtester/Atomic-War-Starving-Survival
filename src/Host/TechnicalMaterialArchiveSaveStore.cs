// ============================================================================
// Save Store : TechnicalMaterialArchiveSaveStore
// Core State : Ashfall.Core.Narrative.TechnicalMaterialArchiveState
// Host Caller: Main.Plans158 / TechnicalMaterialArchiveSystem
// Purpose    : Discovered-record ledger for the technical-material archive
//              (Plan 158 §17) — stable record IDs only, ordinal ordering,
//              tolerant of unknown future IDs. Measurements, failure
//              summaries and canonical links are NEVER persisted; they
//              derive from current catalog data at runtime.
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Technical-material archive save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Ships the
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter. No pre-envelope legacy format ever existed for this section.
    /// </summary>
    public static class TechnicalMaterialArchiveSaveStore
    {
        public const string FileName = "technical_material_archive_save.json";
        public const string SectionName = "technical_material_archive";

        private static readonly SaveStore<TechnicalMaterialArchiveState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(TechnicalMaterialArchiveSaveStore),
            SchemaVersionedEnvelope<TechnicalMaterialArchiveState>.Encode,
            SchemaVersionedEnvelope<TechnicalMaterialArchiveState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Capture the exact persisted envelope bytes for the campaign aggregate without writing to disk.</summary>
        public static string TryCapturePersisted(TechnicalMaterialArchiveState state) => s_store.CapturePersisted(state);

        public static bool TrySave(TechnicalMaterialArchiveState state) => s_store.TrySave(state);

        public static TechnicalMaterialArchiveState? TryLoad() => s_store.TryLoad();
    }
}
