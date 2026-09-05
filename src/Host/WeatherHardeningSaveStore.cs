// ============================================================================
// Save Store : WeatherHardeningSaveStore
// Core State : Ashfall.Core.World.WeatherHardeningState
// Host Caller: Main.ExpandedShelterSystems / WeatherHardeningHostSession
// Purpose    : Cryo-ash weather hardening, intake ice, pipe freeze, and insulation wear
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static class WeatherHardeningSaveStore
    {
        public const string FileName = "weather_hardening_save.json";
        public const string SectionName = "weather_hardening";

        private static readonly SaveStore<WeatherHardeningState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(WeatherHardeningSaveStore),
            Encode,
            Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapture(WeatherHardeningState state) => s_store.CaptureBare(state);
        public static WeatherHardeningState? TryRestore(string json) => s_store.RestoreBare(json);
        public static bool TrySave(WeatherHardeningState state) => s_store.TrySave(state);
        public static WeatherHardeningState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(WeatherHardeningState state) => s_store.CapturePersisted(state);

        private static string Encode(WeatherHardeningState state, IJsonSerializer json) => json.Serialize(state);
        private static WeatherHardeningState? Decode(string raw, IJsonSerializer json) => json.Deserialize<WeatherHardeningState>(raw);
    }
}
