// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : GenerationalSaveStore
// Core State : Ashfall.Core.Survivors.GenerationalState
// Host Caller: Main.Plans178_181
// Purpose    : Child development phases, education, trauma, and adulthood
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class GenerationalSaveStore
    {
        public const string FileName = "child_development_save.json";
        public const string SectionName = "child_development";

        private static readonly SaveStore<GenerationalState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(GenerationalSaveStore),
            SchemaVersionedEnvelope<GenerationalState>.Encode,
            SchemaVersionedEnvelope<GenerationalState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(GenerationalState state) => s_store.TrySave(state);
        public static GenerationalState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(GenerationalState state) => s_store.CapturePersisted(state);
    }
}
