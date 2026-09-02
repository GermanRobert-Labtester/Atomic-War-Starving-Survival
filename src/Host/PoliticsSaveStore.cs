// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : PoliticsSaveStore
// Core State : Ashfall.Core.Narrative.PoliticsState
// Host Caller: Main.Plans182_185
// Purpose    : Settlement elections, political policies, approval rating, and coups
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class PoliticsSaveStore
    {
        public const string FileName = "settlement_politics_save.json";
        public const string SectionName = "settlement_politics";

        private static readonly SaveStore<PoliticsState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(PoliticsSaveStore),
            SchemaVersionedEnvelope<PoliticsState>.Encode,
            SchemaVersionedEnvelope<PoliticsState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(PoliticsState state) => s_store.TrySave(state);
        public static PoliticsState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(PoliticsState state) => s_store.CapturePersisted(state);
    }
}
