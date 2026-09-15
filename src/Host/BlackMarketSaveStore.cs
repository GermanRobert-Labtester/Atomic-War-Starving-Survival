// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : BlackMarketSaveStore
// Core State : Ashfall.Core.Economy.BlackMarketState
// Host Caller: Main.BlackMarket
// Purpose    : Plan 211 — underworld contacts, stock snapshots, debts, heat
// ============================================================================
using Ashfall.Core.Economy;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class BlackMarketSaveStore
    {
        public const string FileName = "black_market_save.json";
        public const string SectionName = "black_market";

        private static readonly SaveStore<BlackMarketState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(BlackMarketSaveStore),
            SchemaVersionedEnvelope<BlackMarketState>.Encode,
            SchemaVersionedEnvelope<BlackMarketState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(BlackMarketState state) => s_store.TrySave(state);
        public static BlackMarketState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(BlackMarketState state) => s_store.CapturePersisted(state);
    }
}
