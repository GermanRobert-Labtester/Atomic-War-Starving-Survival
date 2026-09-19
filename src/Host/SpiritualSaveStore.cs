// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Spiritual;

namespace AtomicWar.GodotApp
{
    public static class SpiritualSaveStore
    {
        public const string FileName = "spiritual_meaning_save.json";
        public const string SectionName = "spiritual_meaning";

        private static readonly SaveStore<SpiritualCoordinatorSaveState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(SpiritualSaveStore),
            SchemaVersionedEnvelope<SpiritualCoordinatorSaveState>.Encode,
            SchemaVersionedEnvelope<SpiritualCoordinatorSaveState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(SpiritualCoordinatorSaveState state) => s_store.TrySave(state);
        public static SpiritualCoordinatorSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(SpiritualCoordinatorSaveState state) => s_store.CapturePersisted(state);
    }
}
