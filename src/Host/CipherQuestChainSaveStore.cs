// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : CipherQuestChainSaveStore
// Core State : System.Collections.Generic.List<CipherQuestState>
// Host Caller: Main.CipherQuestChain / CipherQuestChainHostSession
// Purpose    : Plan 11 / 251 — cipher chain progress (heard, key, decoded,
//              location revealed, resolved) per authored chain.
// ============================================================================
using System.Collections.Generic;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class CipherQuestChainSaveStore
    {
        public const string FileName = "cipher_quest_chain_save.json";
        public const string SectionName = "cipher_quest_chain";

        private static readonly SaveStore<List<CipherQuestState>> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(CipherQuestChainSaveStore),
            SchemaVersionedEnvelope<List<CipherQuestState>>.Encode,
            SchemaVersionedEnvelope<List<CipherQuestState>>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(List<CipherQuestState> state) => s_store.TrySave(state);
        public static List<CipherQuestState>? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(List<CipherQuestState> state) => s_store.CapturePersisted(state);
    }
}
