// SPDX-License-Identifier: MIT
using Ashfall.Core.Crafting;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Chemical synthesis persistence facade delegating to the Core SaveStore service.
    /// </summary>
    public static class ChemicalSynthesisSaveStore
    {
        public const string FileName = "chemical_synthesis_save.json";
        public const string SectionName = "chemical_synthesis";

        private static readonly SaveStore<ChemicalSynthesisSave> s_store =
            SaveStoreHub.Checksummed<ChemicalSynthesisSave>(
                FileName,
                nameof(ChemicalSynthesisSaveStore),
                allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(ChemicalSynthesisSave state) => s_store.CaptureBare(state);
        public static ChemicalSynthesisSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        public static string TryCapture(ChemicalSynthesisSave state) => s_store.CaptureBare(state);
        public static ChemicalSynthesisSave? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(ChemicalSynthesisSave state) => s_store.TrySave(state);
        public static ChemicalSynthesisSave? TryLoad() => s_store.TryLoad();

        public static string TryCapturePersisted(ChemicalSynthesisSave state) => s_store.CapturePersisted(state);
    }
}
