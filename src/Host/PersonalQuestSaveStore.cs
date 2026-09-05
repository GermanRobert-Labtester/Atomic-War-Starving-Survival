// SPDX-License-Identifier: MIT
// ASHFALL survivor personal quest save store facade (Plan 83 / Task B24).

using Ashfall.Core.Quests;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Personal quests save persistence facade delegating to the Core SaveStore service.
    /// </summary>
    public static class PersonalQuestSaveStore
    {
        public const string FileName = "personal_quests_save.json";
        public const string SectionName = "personal_quests";

        private static readonly SaveStore<PersonalQuestSaveState> s_store =
            SaveStoreHub.Checksummed<PersonalQuestSaveState>(
                FileName,
                nameof(PersonalQuestSaveStore),
                allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(PersonalQuestSaveState state) => s_store.CaptureBare(state);
        public static PersonalQuestSaveState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        public static string TryCapture(PersonalQuestSaveState state) => s_store.CaptureBare(state);
        public static PersonalQuestSaveState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(PersonalQuestSaveState state) => s_store.TrySave(state);
        public static PersonalQuestSaveState? TryLoad() => s_store.TryLoad();

        public static string TryCapturePersisted(PersonalQuestSaveState state) => s_store.CapturePersisted(state);
    }
}
