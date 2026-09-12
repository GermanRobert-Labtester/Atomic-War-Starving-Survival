// SPDX-License-Identifier: MIT
using Ashfall.Core.Save;
using Ashfall.Core.Sanatorium;

namespace AtomicWar.GodotApp
{
    /// <summary>Host façade for the psychological_sanatorium save section (flagship Task 8).</summary>
    public static class PsychologicalSanatoriumSaveStore
    {
        public const string FileName = "psychological_sanatorium_save.json";
        public const string SectionName = "psychological_sanatorium";

        private static readonly SaveStore<PsychologicalSanatoriumSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(PsychologicalSanatoriumSaveStore),
            SchemaVersionedEnvelope<PsychologicalSanatoriumSave>.Encode,
            SchemaVersionedEnvelope<PsychologicalSanatoriumSave>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(PsychologicalSanatoriumSave state) => s_store.TrySave(state);
        public static PsychologicalSanatoriumSave? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(PsychologicalSanatoriumSave state) => s_store.CapturePersisted(state);
    }
}
