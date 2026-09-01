// ============================================================================
// Save Store : EcologicalInfestationSaveStore
// Core State : Ashfall.Core.Ecology.EcologicalInfestationState
// Host Caller: Main.EcologicalInfestations (Plan 28 Phase 4)
// Purpose    : Location & shelter ecological infestations (trigger, clear,
//              tolerate/harvest, terminal states) — Plan 28.
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Ecology;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Ecological infestation save persistence — thin façade over the Core
    /// <see cref="SaveStore{T}"/> service (via SaveStoreHub, codec flavor).
    /// Ships the legacy <c>{ SchemaVersion, State, Checksum }</c> envelope,
    /// preserved byte-for-byte by the Core
    /// <see cref="SchemaVersionedEnvelope{T}"/> adapter; path resolution,
    /// atomic write, and error handling live in the service.
    /// </summary>
    public static class EcologicalInfestationSaveStore
    {
        public const string FileName = "ecological_infestation_save.json";
        public const string SectionName = "ecological_infestation";

        private static readonly SaveStore<EcologicalInfestationState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(EcologicalInfestationSaveStore),
            SchemaVersionedEnvelope<EcologicalInfestationState>.Encode,
            SchemaVersionedEnvelope<EcologicalInfestationState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static bool TrySave(EcologicalInfestationState state) => s_store.TrySave(state);

        public static EcologicalInfestationState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(EcologicalInfestationState state) => s_store.CapturePersisted(state);
    }
}
