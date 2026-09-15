// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : BioFermentationSaveStore
// Core State : Ashfall.Core.Shelter.BioFermentationState
// Host Caller: Main.Plans126_129
// Purpose    : Plan 126 — subterranean biological fermentation reactor
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class BioFermentationSaveStore
    {
        public const string FileName = "bio_fermentation_save.json";
        public const string SectionName = "bio_fermentation";

        private static readonly SaveStore<BioFermentationState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(BioFermentationSaveStore),
            SchemaVersionedEnvelope<BioFermentationState>.Encode,
            SchemaVersionedEnvelope<BioFermentationState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(BioFermentationState state) => s_store.TrySave(state);
        public static BioFermentationState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(BioFermentationState state) => s_store.CapturePersisted(state);
    }
}