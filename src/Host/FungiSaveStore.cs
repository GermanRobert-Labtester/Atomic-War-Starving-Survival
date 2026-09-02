// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : FungiSaveStore
// Core State : Ashfall.Core.Farming.FungiCultivationState
// Host Caller: Main.Plans190_193
// Purpose    : Subterranean fungi beds, substrate, spores, and blooms
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Farming;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class FungiSaveStore
    {
        public const string FileName = "fungi_cultivation_save.json";
        public const string SectionName = "fungi_cultivation";

        private static readonly SaveStore<FungiCultivationState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(FungiSaveStore),
            SchemaVersionedEnvelope<FungiCultivationState>.Encode,
            SchemaVersionedEnvelope<FungiCultivationState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(FungiCultivationState state) => s_store.TrySave(state);
        public static FungiCultivationState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(FungiCultivationState state) => s_store.CapturePersisted(state);
    }
}
