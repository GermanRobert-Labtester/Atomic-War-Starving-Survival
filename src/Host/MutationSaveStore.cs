// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : MutationSaveStore
// Core State : Ashfall.Core.Medical.MutationState
// Host Caller: Main.Plans178_181
// Purpose    : Radiation exposure, genetic instability, and mutation trees
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class MutationSaveStore
    {
        public const string FileName = "mutation_save.json";
        public const string SectionName = "mutation_tree";

        private static readonly SaveStore<MutationState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(MutationSaveStore),
            SchemaVersionedEnvelope<MutationState>.Encode,
            SchemaVersionedEnvelope<MutationState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(MutationState state) => s_store.TrySave(state);
        public static MutationState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(MutationState state) => s_store.CapturePersisted(state);
    }
}
