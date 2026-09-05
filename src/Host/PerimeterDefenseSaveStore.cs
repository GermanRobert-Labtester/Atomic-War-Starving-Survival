// SPDX-License-Identifier: MIT
// ASHFALL Surface Perimeter Defense save store facade (Plan 88 / Task 4).

using Ashfall.Core.Defense;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Perimeter defense persistence facade delegating to the Core SaveStore service.
    /// </summary>
    public static class PerimeterDefenseSaveStore
    {
        public const string FileName = "perimeter_defense_save.json";
        public const string SectionName = "perimeter_defense";

        private static readonly SaveStore<PerimeterDefenseSave> s_store =
            SaveStoreHub.Checksummed<PerimeterDefenseSave>(
                FileName,
                nameof(PerimeterDefenseSaveStore),
                allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(PerimeterDefenseSave state) => s_store.CaptureBare(state);
        public static PerimeterDefenseSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        public static string TryCapture(PerimeterDefenseSave state) => s_store.CaptureBare(state);
        public static PerimeterDefenseSave? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(PerimeterDefenseSave state) => s_store.TrySave(state);
        public static PerimeterDefenseSave? TryLoad() => s_store.TryLoad();

        public static string TryCapturePersisted(PerimeterDefenseSave state) => s_store.CapturePersisted(state);
    }
}
