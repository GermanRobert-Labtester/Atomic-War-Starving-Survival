// ============================================================================
// Save Store : ReconTelemetrySaveStore
// Core State : Ashfall.Core.Expeditions.ReconTelemetryState
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Expeditions;

namespace AtomicWar.GodotApp
{
    public static class ReconTelemetrySaveStore
    {
        public const string FileName = "recon_telemetry_save.json";
        public const string SectionName = "recon_telemetry";

        private static readonly SaveStore<ReconTelemetryState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ReconTelemetrySaveStore),
            Encode,
            Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapture(ReconTelemetryState state) => s_store.CaptureBare(state);
        public static ReconTelemetryState? TryRestore(string json) => s_store.RestoreBare(json);
        public static bool TrySave(ReconTelemetryState state) => s_store.TrySave(state);
        public static ReconTelemetryState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(ReconTelemetryState state) => s_store.CapturePersisted(state);

        private static string Encode(ReconTelemetryState state, IJsonSerializer json) => json.Serialize(state);
        private static ReconTelemetryState? Decode(string raw, IJsonSerializer json) => json.Deserialize<ReconTelemetryState>(raw);
    }
}
