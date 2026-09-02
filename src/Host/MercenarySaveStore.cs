// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : MercenarySaveStore
// Core State : Ashfall.Core.Economy.MercenaryState
// Host Caller: Main.Plans186_189
// Purpose    : Mercenary bounty contracts, target intel, and rival tracking
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class MercenarySaveStore
    {
        public const string FileName = "mercenary_bounties_save.json";
        public const string SectionName = "mercenary_bounties";

        private static readonly SaveStore<MercenaryState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(MercenarySaveStore),
            SchemaVersionedEnvelope<MercenaryState>.Encode,
            SchemaVersionedEnvelope<MercenaryState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(MercenaryState state) => s_store.TrySave(state);
        public static MercenaryState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(MercenaryState state) => s_store.CapturePersisted(state);
    }
}
