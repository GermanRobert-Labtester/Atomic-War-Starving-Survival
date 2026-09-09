// SPDX-License-Identifier: MIT
// ASHFALL survivor narrative questline save store facade (Plan 104 runtime wiring).

using Ashfall.Core.Quests;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Narrative questline save persistence facade delegating to the Core
    /// SaveStore service (Initiative #41). Checksummed envelope, atomic write,
    /// no legacy bare-state format because this section is new.
    /// </summary>
    public static class NarrativeQuestlineSaveStore
    {
        public const string FileName = "narrative_questlines_save.json";
        public const string SectionName = "narrative_questlines";

        private static readonly SaveStore<NarrativeQuestlineSaveState> s_store =
            SaveStoreHub.Checksummed<NarrativeQuestlineSaveState>(
                FileName,
                nameof(NarrativeQuestlineSaveStore),
                allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(NarrativeQuestlineSaveState state) => s_store.CaptureBare(state);
        public static NarrativeQuestlineSaveState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        public static string TryCapture(NarrativeQuestlineSaveState state) => s_store.CaptureBare(state);
        public static NarrativeQuestlineSaveState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(NarrativeQuestlineSaveState state) => s_store.TrySave(state);
        public static NarrativeQuestlineSaveState? TryLoad() => s_store.TryLoad();

        public static string TryCapturePersisted(NarrativeQuestlineSaveState state) => s_store.CapturePersisted(state);
    }
}
