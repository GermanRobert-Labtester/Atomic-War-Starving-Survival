// SPDX-License-Identifier: MIT
using Ashfall.Core.Expeditions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Vehicle garage persistence facade delegating to the Core SaveStore service.
    /// </summary>
    public static class VehicleGarageSaveStore
    {
        public const string FileName = "vehicle_garage_save.json";
        public const string SectionName = "vehicle_garage";

        private static readonly SaveStore<VehicleGarageState> s_store =
            SaveStoreHub.Checksummed<VehicleGarageState>(
                FileName,
                nameof(VehicleGarageSaveStore),
                allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(VehicleGarageState state) => s_store.CaptureBare(state);
        public static VehicleGarageState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        public static string TryCapture(VehicleGarageState state) => s_store.CaptureBare(state);
        public static VehicleGarageState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(VehicleGarageState state) => s_store.TrySave(state);
        public static VehicleGarageState? TryLoad() => s_store.TryLoad();

        public static string TryCapturePersisted(VehicleGarageState state) => s_store.CapturePersisted(state);
    }
}
