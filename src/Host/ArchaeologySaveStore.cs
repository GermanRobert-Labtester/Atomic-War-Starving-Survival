// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ArchaeologySaveStore
// Core State : Ashfall.Core.Archaeology.ArchaeologyState
// Host Caller: Main.Plans186_189
// Purpose    : Archaeology excavation ruins, archive decryption, and lore unlocks
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Archaeology;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class ArchaeologySaveStore
    {
        public const string FileName = "archaeology_save.json";
        public const string SectionName = "archaeology";

        private static readonly SaveStore<ArchaeologyState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ArchaeologySaveStore),
            SchemaVersionedEnvelope<ArchaeologyState>.Encode,
            SchemaVersionedEnvelope<ArchaeologyState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(ArchaeologyState state) => s_store.TrySave(state);
        public static ArchaeologyState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(ArchaeologyState state) => s_store.CapturePersisted(state);
    }
}
