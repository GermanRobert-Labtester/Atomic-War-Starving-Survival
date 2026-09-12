// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : LowBackgroundMetrologySaveStore
// Core State : Ashfall.Core.Radiation.LowBackgroundMetrologyState
// Host Caller: Main.LowBackgroundMetrology
// Purpose    : Plan 138 Phase 2 — shield installation, detector calibration,
//              smelting batches, bounded assay history.
// ============================================================================
using Ashfall.Core.Radiation;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class LowBackgroundMetrologySaveStore
    {
        public const string FileName = "low_background_metrology_save.json";
        public const string SectionName = "low_background_metrology";

        private static readonly SaveStore<LowBackgroundMetrologyState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(LowBackgroundMetrologySaveStore),
            SchemaVersionedEnvelope<LowBackgroundMetrologyState>.Encode,
            SchemaVersionedEnvelope<LowBackgroundMetrologyState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(LowBackgroundMetrologyState state) => s_store.TrySave(state);
        public static LowBackgroundMetrologyState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(LowBackgroundMetrologyState state) => s_store.CapturePersisted(state);
    }
}
