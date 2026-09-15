// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : CvdDiamondSaveStore
// Core State : CvdDiamondReactorState
// Host Caller: Main.Plans122to125
// Purpose    : Plan 124 — CVD reactor condition, plasma stability, active/finished batches, faults
// ============================================================================
using Ashfall.Core.Shelter;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class CvdDiamondSaveStore
    {
        public const string FileName = "cvd_diamond_save.json";
        public const string SectionName = "cvd_diamond";

        private static readonly SaveStore<CvdDiamondReactorState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(CvdDiamondSaveStore),
            SchemaVersionedEnvelope<CvdDiamondReactorState>.Encode,
            SchemaVersionedEnvelope<CvdDiamondReactorState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(CvdDiamondReactorState state) => s_store.TrySave(state);
        public static CvdDiamondReactorState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(CvdDiamondReactorState state) => s_store.CapturePersisted(state);
    }
}
