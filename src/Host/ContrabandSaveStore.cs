// ============================================================================
// Save Store : ContrabandSaveStore
// Core State : Ashfall.Core.Narrative.ContrabandStashState
// Host Caller: Main.Plans147 / ContrabandStashSystem
// Purpose    : Bunker contraband stash claim ledger (Plan 147) — once-only
//              discovery bookkeeping; the catalog itself is immutable
//              definition data and is never persisted.
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Contraband stash save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Ships the
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter; path resolution, atomic write, and error handling live in the
    /// service. No pre-envelope legacy format ever existed for this section.
    /// </summary>
    public static class ContrabandSaveStore
    {
        public const string FileName = "contraband_stash_save.json";
        public const string SectionName = "contraband_stash";

        private static readonly SaveStore<ContrabandStashState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ContrabandSaveStore),
            SchemaVersionedEnvelope<ContrabandStashState>.Encode,
            SchemaVersionedEnvelope<ContrabandStashState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Capture the exact persisted envelope bytes for the campaign aggregate without writing to disk.</summary>
        public static string TryCapturePersisted(ContrabandStashState state) => s_store.CapturePersisted(state);

        public static bool TrySave(ContrabandStashState state) => s_store.TrySave(state);

        public static ContrabandStashState? TryLoad() => s_store.TryLoad();

        /// <summary>Restore a state from envelope JSON without touching disk.</summary>
        public static ContrabandStashState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
    }
}
