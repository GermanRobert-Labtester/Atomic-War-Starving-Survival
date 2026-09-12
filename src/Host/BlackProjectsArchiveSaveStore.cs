// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : BlackProjectsArchiveSaveStore
// Core State : Ashfall.Core.Narrative.BlackProjectsArchiveState
// Host Caller: Main.Plans152 / BlackProjectsArchiveSystem
// Purpose    : Discovered-record ledger for the Black Projects intelligence
//              archive (Plan 152 §16) — stable record IDs only, ordinal
//              ordering, tolerant of unknown future IDs. Telemetry values,
//              classification banners and relations are NEVER persisted;
//              they are derived from current catalog data at runtime.
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Black Projects archive save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Ships the
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter. No pre-envelope legacy format ever existed for this section.
    /// </summary>
    public static class BlackProjectsArchiveSaveStore
    {
        public const string FileName = "black_projects_archive_save.json";
        public const string SectionName = "black_projects_archive";

        private static readonly SaveStore<BlackProjectsArchiveState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(BlackProjectsArchiveSaveStore),
            SchemaVersionedEnvelope<BlackProjectsArchiveState>.Encode,
            SchemaVersionedEnvelope<BlackProjectsArchiveState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Capture the exact persisted envelope bytes for the campaign aggregate without writing to disk.</summary>
        public static string TryCapturePersisted(BlackProjectsArchiveState state) => s_store.CapturePersisted(state);

        public static bool TrySave(BlackProjectsArchiveState state) => s_store.TrySave(state);

        public static BlackProjectsArchiveState? TryLoad() => s_store.TryLoad();
    }
}
