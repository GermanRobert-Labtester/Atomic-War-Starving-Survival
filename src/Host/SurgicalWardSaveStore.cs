// SPDX-License-Identifier: MIT
// ASHFALL Advanced Surgical Ward save store facade (Plan 86 / Task 2).

using Ashfall.Core.Medical;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Surgical ward persistence facade delegating to the Core SaveStore service.
    /// </summary>
    public static class SurgicalWardSaveStore
    {
        public const string FileName = "surgical_ward_save.json";
        public const string SectionName = "surgical_ward";

        private static readonly SaveStore<AdvancedSurgicalWardSave> s_store =
            SaveStoreHub.Checksummed<AdvancedSurgicalWardSave>(
                FileName,
                nameof(SurgicalWardSaveStore),
                allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(AdvancedSurgicalWardSave state) => s_store.CaptureBare(state);
        public static AdvancedSurgicalWardSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        public static string TryCapture(AdvancedSurgicalWardSave state) => s_store.CaptureBare(state);
        public static AdvancedSurgicalWardSave? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(AdvancedSurgicalWardSave state) => s_store.TrySave(state);
        public static AdvancedSurgicalWardSave? TryLoad() => s_store.TryLoad();

        public static string TryCapturePersisted(AdvancedSurgicalWardSave state) => s_store.CapturePersisted(state);
    }
}
