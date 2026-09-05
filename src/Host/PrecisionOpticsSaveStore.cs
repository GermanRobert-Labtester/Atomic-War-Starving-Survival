// ============================================================================
// Save Store : PrecisionOpticsSaveStore
// Core State : Ashfall.Core.Shelter.PrecisionOpticsState
// Host Caller: Main.Plans110_113 / PrecisionOpticsHostSession
// Purpose    : Plans 110-113 — precision optical blank grinding, figure testing,
//              and telescope/shield viewports.
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class PrecisionOpticsSaveStore
    {
        public const string FileName = "precision_optics_save.json";
        public const string SectionName = "precision_optics";

        private static readonly SaveStore<PrecisionOpticsState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(PrecisionOpticsSaveStore),
            SchemaVersionedEnvelope<PrecisionOpticsState>.Encode,
            SchemaVersionedEnvelope<PrecisionOpticsState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(PrecisionOpticsState state) => s_store.CaptureBare(state);
        public static PrecisionOpticsState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
        public static string TryCapture(PrecisionOpticsState state) => s_store.CaptureBare(state);
        public static PrecisionOpticsState? TryRestore(string json) => s_store.RestoreBare(json);
        public static bool TrySave(PrecisionOpticsState state) => s_store.TrySave(state);
        public static PrecisionOpticsState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(PrecisionOpticsState state) => s_store.CapturePersisted(state);
    }
}
