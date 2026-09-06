// SPDX-License-Identifier: MIT
using Ashfall.Core.Factions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Shelter espionage persistence facade delegating to the Core SaveStore service.
    /// </summary>
    public static class ShelterEspionageSaveStore
    {
        public const string FileName = "faction_espionage_save.json";
        public const string SectionName = "faction_espionage";

        private static readonly SaveStore<ShelterEspionageState> s_store =
            SaveStoreHub.Checksummed<ShelterEspionageState>(
                FileName,
                nameof(ShelterEspionageSaveStore),
                allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(ShelterEspionageState state) => s_store.CaptureBare(state);
        public static ShelterEspionageState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        public static string TryCapture(ShelterEspionageState state) => s_store.CaptureBare(state);
        public static ShelterEspionageState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(ShelterEspionageState state) => s_store.TrySave(state);
        public static ShelterEspionageState? TryLoad() => s_store.TryLoad();

        public static string TryCapturePersisted(ShelterEspionageState state) => s_store.CapturePersisted(state);
    }
}
