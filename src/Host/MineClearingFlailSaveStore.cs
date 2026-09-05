// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Expeditions;

namespace AtomicWar.GodotApp
{
    public static class MineClearingFlailSaveStore
    {
        public const string FileName = "mine_clearing_flail_save.json";
        public const string SectionName = "mine_clearing_flail";

        private static readonly SaveStore<MineClearingFlailState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(MineClearingFlailSaveStore),
            Encode,
            Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapture(MineClearingFlailState state) => s_store.CaptureBare(state);
        public static MineClearingFlailState? TryRestore(string json) => s_store.RestoreBare(json);
        public static bool TrySave(MineClearingFlailState state) => s_store.TrySave(state);
        public static MineClearingFlailState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(MineClearingFlailState state) => s_store.CapturePersisted(state);

        private static string Encode(MineClearingFlailState state, IJsonSerializer json) => json.Serialize(state);
        private static MineClearingFlailState? Decode(string raw, IJsonSerializer json) => json.Deserialize<MineClearingFlailState>(raw);
    }
}
