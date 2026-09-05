// ============================================================================
// Save Store : SolarConcentratorSaveStore
// Core State : Ashfall.Core.Shelter.SolarConcentratorState
// Host Caller: Main.Plans110_113 / SolarConcentratorHostSession
// Purpose    : Plans 110-113 — parabolic solar concentrator, mirror condition,
//              tracking mode, and thermal/electrical output.
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class SolarConcentratorSaveStore
    {
        public const string FileName = "solar_concentrator_save.json";
        public const string SectionName = "solar_concentrator";

        private static readonly SaveStore<SolarConcentratorState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(SolarConcentratorSaveStore),
            SchemaVersionedEnvelope<SolarConcentratorState>.Encode,
            SchemaVersionedEnvelope<SolarConcentratorState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(SolarConcentratorState state) => s_store.CaptureBare(state);
        public static SolarConcentratorState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
        public static string TryCapture(SolarConcentratorState state) => s_store.CaptureBare(state);
        public static SolarConcentratorState? TryRestore(string json) => s_store.RestoreBare(json);
        public static bool TrySave(SolarConcentratorState state) => s_store.TrySave(state);
        public static SolarConcentratorState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(SolarConcentratorState state) => s_store.CapturePersisted(state);
    }
}
