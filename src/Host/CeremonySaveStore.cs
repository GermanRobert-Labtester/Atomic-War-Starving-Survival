// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : CeremonySaveStore
// Core State : Ashfall.Core.Narrative.CeremonySaveState
// Host Caller: Main.Plans198_201
// Purpose    : Communal ceremonies, wasteland festivals, preparations & truces
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class CeremonySaveStore
    {
        public const string FileName = "ceremony_save.json";
        public const string SectionName = "ceremony";

        private static readonly SaveStore<CeremonySaveState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(CeremonySaveStore),
            SchemaVersionedEnvelope<CeremonySaveState>.Encode,
            SchemaVersionedEnvelope<CeremonySaveState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(CeremonySaveState state) => s_store.TrySave(state);
        public static CeremonySaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(CeremonySaveState state) => s_store.CapturePersisted(state);
    }
}
