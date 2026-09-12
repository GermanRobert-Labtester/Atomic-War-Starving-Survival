// SPDX-License-Identifier: MIT
using Ashfall.Core.Save;
using Ashfall.Core.Diplomacy;

namespace AtomicWar.GodotApp
{
    /// <summary>Host façade for the diplomatic_summits save section (flagship Task 6).</summary>
    public static class DiplomaticSummitSaveStore
    {
        public const string FileName = "diplomatic_summits_save.json";
        public const string SectionName = "diplomatic_summits";

        private static readonly SaveStore<DiplomaticSummitSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(DiplomaticSummitSaveStore),
            SchemaVersionedEnvelope<DiplomaticSummitSave>.Encode,
            SchemaVersionedEnvelope<DiplomaticSummitSave>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(DiplomaticSummitSave state) => s_store.TrySave(state);
        public static DiplomaticSummitSave? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(DiplomaticSummitSave state) => s_store.CapturePersisted(state);
    }
}
