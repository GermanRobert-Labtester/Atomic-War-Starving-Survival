// SPDX-License-Identifier: MIT
// ASHFALL Power Distribution Subgrids save store facade (Plan 87 / Task 3).

using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Power distribution subgrids persistence facade delegating to the Core SaveStore service.
    /// </summary>
    public static class PowerDistributionSaveStore
    {
        public const string FileName = "power_subgrids_save.json";
        public const string SectionName = "power_subgrids";

        private static readonly SaveStore<PowerDistributionSubgridSave> s_store =
            SaveStoreHub.Checksummed<PowerDistributionSubgridSave>(
                FileName,
                nameof(PowerDistributionSaveStore),
                allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(PowerDistributionSubgridSave state) => s_store.CaptureBare(state);
        public static PowerDistributionSubgridSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        public static string TryCapture(PowerDistributionSubgridSave state) => s_store.CaptureBare(state);
        public static PowerDistributionSubgridSave? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(PowerDistributionSubgridSave state) => s_store.TrySave(state);
        public static PowerDistributionSubgridSave? TryLoad() => s_store.TryLoad();

        public static string TryCapturePersisted(PowerDistributionSubgridSave state) => s_store.CapturePersisted(state);
    }
}
