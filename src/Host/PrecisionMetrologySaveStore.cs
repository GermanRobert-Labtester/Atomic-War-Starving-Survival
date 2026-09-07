// ============================================================================
// Save Store : PrecisionMetrologySaveStore
// Core State : Ashfall.Core.Shelter.PrecisionMetrologyState
// Host Caller: Main.PlansB86_B89
// Purpose    : Plan B89 — precision metrology grades, certificates, and
//              instrument calibration state (no global breakdown magic).
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Precision metrology save persistence — thin façade over Core
    /// SaveStore&lt;T&gt; via SaveStoreHub. Ships the legacy
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope.
    /// </summary>
    public static class PrecisionMetrologySaveStore
    {
        public const string FileName = "precision_metrology_save.json";
        public const string SectionName = "precision_metrology";

        private static readonly SaveStore<PrecisionMetrologyState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(PrecisionMetrologySaveStore),
            SchemaVersionedEnvelope<PrecisionMetrologyState>.Encode,
            SchemaVersionedEnvelope<PrecisionMetrologyState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(PrecisionMetrologyState state) => s_store.CaptureBare(state);
        public static PrecisionMetrologyState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
        public static string TryCapture(PrecisionMetrologyState state) => s_store.CaptureBare(state);
        public static PrecisionMetrologyState? TryRestore(string json) => s_store.RestoreBare(json);
        public static bool TrySave(PrecisionMetrologyState state) => s_store.TrySave(state);
        public static PrecisionMetrologyState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(PrecisionMetrologyState state) => s_store.CapturePersisted(state);
    }
}
