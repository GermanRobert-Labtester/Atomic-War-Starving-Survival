// ============================================================================
// Save Store : ChlorAlkaliSaveStore
// Core State : Ashfall.Core.Shelter.ChlorAlkaliPlantState
// Host Caller: Main.Plans110_113 / ChlorAlkaliHostSession
// Purpose    : Plans 110-113 — chlor-alkali synthesis plant, membrane wear,
//              hazard load, and chemical production.
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class ChlorAlkaliSaveStore
    {
        public const string FileName = "chlor_alkali_synthesis_save.json";
        public const string SectionName = "chlor_alkali_synthesis";

        private static readonly SaveStore<ChlorAlkaliPlantState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ChlorAlkaliSaveStore),
            SchemaVersionedEnvelope<ChlorAlkaliPlantState>.Encode,
            SchemaVersionedEnvelope<ChlorAlkaliPlantState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(ChlorAlkaliPlantState state) => s_store.CaptureBare(state);
        public static ChlorAlkaliPlantState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
        public static string TryCapture(ChlorAlkaliPlantState state) => s_store.CaptureBare(state);
        public static ChlorAlkaliPlantState? TryRestore(string json) => s_store.RestoreBare(json);
        public static bool TrySave(ChlorAlkaliPlantState state) => s_store.TrySave(state);
        public static ChlorAlkaliPlantState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(ChlorAlkaliPlantState state) => s_store.CapturePersisted(state);
    }
}
