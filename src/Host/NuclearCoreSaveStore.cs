// SPDX-License-Identifier: MIT
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Nuclear core lifecycle persistence facade delegating to the Core SaveStore service.
    /// </summary>
    public static class NuclearCoreSaveStore
    {
        public const string FileName = "nuclear_core_lifecycle_save.json";
        public const string SectionName = "nuclear_core_lifecycle";

        private static readonly SaveStore<NuclearCoreLifecycleSave> s_store =
            SaveStoreHub.Checksummed<NuclearCoreLifecycleSave>(
                FileName,
                nameof(NuclearCoreSaveStore),
                allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(NuclearCoreLifecycleSave state) => s_store.CaptureBare(state);
        public static NuclearCoreLifecycleSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        public static string TryCapture(NuclearCoreLifecycleSave state) => s_store.CaptureBare(state);
        public static NuclearCoreLifecycleSave? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(NuclearCoreLifecycleSave state) => s_store.TrySave(state);
        public static NuclearCoreLifecycleSave? TryLoad() => s_store.TryLoad();

        public static string TryCapturePersisted(NuclearCoreLifecycleSave state) => s_store.CapturePersisted(state);
    }
}
