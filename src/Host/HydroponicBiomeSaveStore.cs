// SPDX-License-Identifier: MIT
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Hydroponic biome persistence facade delegating to the Core SaveStore service.
    /// </summary>
    public static class HydroponicBiomeSaveStore
    {
        public const string FileName = "hydroponic_biomes_save.json";
        public const string SectionName = "hydroponic_biomes";

        private static readonly SaveStore<HydroponicBiomeSave> s_store =
            SaveStoreHub.Checksummed<HydroponicBiomeSave>(
                FileName,
                nameof(HydroponicBiomeSaveStore),
                allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(HydroponicBiomeSave state) => s_store.CaptureBare(state);
        public static HydroponicBiomeSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        public static string TryCapture(HydroponicBiomeSave state) => s_store.CaptureBare(state);
        public static HydroponicBiomeSave? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(HydroponicBiomeSave state) => s_store.TrySave(state);
        public static HydroponicBiomeSave? TryLoad() => s_store.TryLoad();

        public static string TryCapturePersisted(HydroponicBiomeSave state) => s_store.CapturePersisted(state);
    }
}
