// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : OralLoreSaveStore
// Core State : Ashfall.Core.Narrative.OralLoreDiscoveryState
// Host Caller: Main.Plans155 / OralLorePerformanceSystem
// Purpose    : First-heard ledger for the oral-lore corpus (Plan 155 §15) —
//              stable lore IDs only, ordinal ordering, tolerant of unknown
//              future IDs. Lyrics, tempo descriptors and performance
//              contexts are NEVER persisted; they derive from the data
//              authority at runtime.
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Oral lore save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Ships the
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter. No pre-envelope legacy format ever existed for this section.
    /// </summary>
    public static class OralLoreSaveStore
    {
        public const string FileName = "oral_lore_save.json";
        public const string SectionName = "oral_lore";

        private static readonly SaveStore<OralLoreDiscoveryState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(OralLoreSaveStore),
            SchemaVersionedEnvelope<OralLoreDiscoveryState>.Encode,
            SchemaVersionedEnvelope<OralLoreDiscoveryState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Capture the exact persisted envelope bytes for the campaign aggregate without writing to disk.</summary>
        public static string TryCapturePersisted(OralLoreDiscoveryState state) => s_store.CapturePersisted(state);

        public static bool TrySave(OralLoreDiscoveryState state) => s_store.TrySave(state);

        public static OralLoreDiscoveryState? TryLoad() => s_store.TryLoad();
    }
}
