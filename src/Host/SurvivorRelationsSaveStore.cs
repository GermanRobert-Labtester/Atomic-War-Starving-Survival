// ============================================================================
// Save Store : SurvivorRelationsSaveStore
// Core State : Ashfall.Core.SurvivorRelationsState
// Host Caller: Main.ShelterSocial / SurvivorRelationsHostSession
// Purpose    : Interpersonal survivor affinities, rivalries, trust bonds, and social friction
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Survivor relations save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope, preserved
    /// byte-for-byte by the Core <see cref="SchemaVersionedEnvelope{T}"/>
    /// adapter; path resolution, atomic write, and error handling live in the
    /// service.
    /// </summary>
    public static class SurvivorRelationsSaveStore
    {
        public const string FileName = "survivor_relations_save.json";
        public const string SectionName = "survivor_relations";

        private static readonly SaveStore<SurvivorRelationsState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(SurvivorRelationsSaveStore),
            SchemaVersionedEnvelope<SurvivorRelationsState>.Encode,
            SchemaVersionedEnvelope<SurvivorRelationsState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(SurvivorRelationsState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static SurvivorRelationsState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(SurvivorRelationsState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static SurvivorRelationsState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(SurvivorRelationsState state) => s_store.TrySave(state);

        public static SurvivorRelationsState? TryLoad() => s_store.TryLoad();
    }
}
