// SPDX-License-Identifier: MIT
using Ashfall.Core.Save;
using Ashfall.Core.Culture;

namespace AtomicWar.GodotApp
{
    /// <summary>Host façade for the cultural_archives save section (flagship Task 5).</summary>
    public static class CulturalArchiveSaveStore
    {
        public const string FileName = "cultural_archives_save.json";
        public const string SectionName = "cultural_archives";

        private static readonly SaveStore<CulturalArchiveVaultSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(CulturalArchiveSaveStore),
            SchemaVersionedEnvelope<CulturalArchiveVaultSave>.Encode,
            SchemaVersionedEnvelope<CulturalArchiveVaultSave>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(CulturalArchiveVaultSave state) => s_store.TrySave(state);
        public static CulturalArchiveVaultSave? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(CulturalArchiveVaultSave state) => s_store.CapturePersisted(state);
    }
}
