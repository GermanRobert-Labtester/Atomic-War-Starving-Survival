// ============================================================================
// Save Store : GeothermalAquiferSaveStore
// Core State : Ashfall.Core.Shelter.GeothermalAquiferState
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class GeothermalAquiferSaveStore
    {
        public const string FileName = "geothermal_aquifer_save.json";
        public const string SectionName = "geothermal_aquifer";

        private static readonly SaveStore<GeothermalAquiferState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(GeothermalAquiferSaveStore),
            Encode,
            Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapture(GeothermalAquiferState state) => s_store.CaptureBare(state);
        public static GeothermalAquiferState? TryRestore(string json) => s_store.RestoreBare(json);
        public static bool TrySave(GeothermalAquiferState state) => s_store.TrySave(state);
        public static GeothermalAquiferState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(GeothermalAquiferState state) => s_store.CapturePersisted(state);

        private static string Encode(GeothermalAquiferState state, IJsonSerializer json) => json.Serialize(state);
        private static GeothermalAquiferState? Decode(string raw, IJsonSerializer json) => json.Deserialize<GeothermalAquiferState>(raw);
    }
}
