// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : RunFlatTireSaveStore
// Core State : Ashfall.Core.Expeditions.RunFlatTireState
// Host Caller: Main.RunFlatTire
// Purpose    : Plan 141 Phase 2 — run-flat wheel profiles, integrity, heat,
//              rim/bead condition, puncture counts.
// ============================================================================
using Ashfall.Core.Expeditions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class RunFlatTireSaveStore
    {
        public const string FileName = "runflat_tire_save.json";
        public const string SectionName = "runflat_tire";

        private static readonly SaveStore<RunFlatTireState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(RunFlatTireSaveStore),
            SchemaVersionedEnvelope<RunFlatTireState>.Encode,
            SchemaVersionedEnvelope<RunFlatTireState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(RunFlatTireState state) => s_store.TrySave(state);
        public static RunFlatTireState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(RunFlatTireState state) => s_store.CapturePersisted(state);
    }
}
