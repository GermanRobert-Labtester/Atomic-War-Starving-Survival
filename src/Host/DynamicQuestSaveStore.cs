// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : DynamicQuestSaveStore
// Core State : Ashfall.Core.Quests.DynamicQuestSave
// Host Caller: Main.DynamicQuests
// Purpose    : Campaign-wide emergency dynamic quests (rescue, radio depot, armory)
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Quests;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class DynamicQuestSaveStore
    {
        public const string FileName = "dynamic_quests_save.json";
        public const string SectionName = "dynamic_quests";

        private static readonly SaveStore<DynamicQuestSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(DynamicQuestSaveStore),
            SchemaVersionedEnvelope<DynamicQuestSave>.Encode,
            SchemaVersionedEnvelope<DynamicQuestSave>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(DynamicQuestSave state) => s_store.TrySave(state);
        public static DynamicQuestSave? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(DynamicQuestSave state) => s_store.CapturePersisted(state);
    }
}
