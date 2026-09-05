// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public static class MicrofluidicDiagnosticSaveStore
    {
        public const string FileName = "microfluidic_diagnostic_save.json";
        public const string SectionName = "microfluidic_diagnostic";

        private static readonly SaveStore<MicrofluidicDiagnosticState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(MicrofluidicDiagnosticSaveStore),
            Encode,
            Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapture(MicrofluidicDiagnosticState state) => s_store.CaptureBare(state);
        public static MicrofluidicDiagnosticState? TryRestore(string json) => s_store.RestoreBare(json);
        public static bool TrySave(MicrofluidicDiagnosticState state) => s_store.TrySave(state);
        public static MicrofluidicDiagnosticState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(MicrofluidicDiagnosticState state) => s_store.CapturePersisted(state);

        private static string Encode(MicrofluidicDiagnosticState state, IJsonSerializer json) => json.Serialize(state);
        private static MicrofluidicDiagnosticState? Decode(string raw, IJsonSerializer json) => json.Deserialize<MicrofluidicDiagnosticState>(raw);
    }
}
