// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class EbPvdCoatingSaveStore
    {
        public const string FileName = "ebpvd_coating_save.json";
        public const string SectionName = "ebpvd_coating";

        private static readonly SaveStore<EbPvdCoatingState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(EbPvdCoatingSaveStore),
            Encode,
            Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapture(EbPvdCoatingState state) => s_store.CaptureBare(state);
        public static EbPvdCoatingState? TryRestore(string json) => s_store.RestoreBare(json);
        public static bool TrySave(EbPvdCoatingState state) => s_store.TrySave(state);
        public static EbPvdCoatingState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(EbPvdCoatingState state) => s_store.CapturePersisted(state);

        private static string Encode(EbPvdCoatingState state, IJsonSerializer json) => json.Serialize(state);
        private static EbPvdCoatingState? Decode(string raw, IJsonSerializer json) => json.Deserialize<EbPvdCoatingState>(raw);
    }
}
