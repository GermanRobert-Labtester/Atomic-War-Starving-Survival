// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Expeditions;

namespace AtomicWar.GodotApp
{
    public static class RailGrindingSaveStore
    {
        public const string FileName = "rail_grinding_save.json";
        public const string SectionName = "rail_grinding";

        private static readonly SaveStore<RailGrindingEngineState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(RailGrindingSaveStore),
            Encode,
            Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapture(RailGrindingEngineState state) => s_store.CaptureBare(state);
        public static RailGrindingEngineState? TryRestore(string json) => s_store.RestoreBare(json);
        public static bool TrySave(RailGrindingEngineState state) => s_store.TrySave(state);
        public static RailGrindingEngineState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(RailGrindingEngineState state) => s_store.CapturePersisted(state);

        private static string Encode(RailGrindingEngineState state, IJsonSerializer json) => json.Serialize(state);
        private static RailGrindingEngineState? Decode(string raw, IJsonSerializer json) => json.Deserialize<RailGrindingEngineState>(raw);
    }
}
