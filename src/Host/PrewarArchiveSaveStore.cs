// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : PrewarArchiveSaveStore
// Core State : Ashfall.Core.Research.PrewarArchiveDecryptionState
// ============================================================================
using Ashfall.Core.Research;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>Checksummed campaign-envelope adapter for pre-war archive decryption state.</summary>
    public static class PrewarArchiveSaveStore
    {
        public const string FileName = "prewar_archives_save.json";
        public const string SectionName = "prewar_archives";

        private static readonly SaveStore<PrewarArchiveDecryptionState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(PrewarArchiveSaveStore),
            SchemaVersionedEnvelope<PrewarArchiveDecryptionState>.Encode,
            SchemaVersionedEnvelope<PrewarArchiveDecryptionState>.Decode);

        public static string TryCapturePersisted(PrewarArchiveDecryptionState state) => s_store.CapturePersisted(state);
        public static PrewarArchiveDecryptionState? TryLoad() => s_store.TryLoad();
    }
}
