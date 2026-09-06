// SPDX-License-Identifier: MIT
using Ashfall.Core.Needs;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Survivor mental health persistence facade delegating to the Core SaveStore service.
    /// </summary>
    public static class SurvivorMentalHealthSaveStore
    {
        public const string FileName = "survivor_mental_health_save.json";
        public const string SectionName = "survivor_mental_health";

        private static readonly SaveStore<SurvivorMentalHealthState> s_store =
            SaveStoreHub.Checksummed<SurvivorMentalHealthState>(
                FileName,
                nameof(SurvivorMentalHealthSaveStore),
                allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(SurvivorMentalHealthState state) => s_store.CaptureBare(state);
        public static SurvivorMentalHealthState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        public static string TryCapture(SurvivorMentalHealthState state) => s_store.CaptureBare(state);
        public static SurvivorMentalHealthState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(SurvivorMentalHealthState state) => s_store.TrySave(state);
        public static SurvivorMentalHealthState? TryLoad() => s_store.TryLoad();

        public static string TryCapturePersisted(SurvivorMentalHealthState state) => s_store.CapturePersisted(state);
    }
}
