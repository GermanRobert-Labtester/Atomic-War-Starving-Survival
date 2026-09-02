// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : FalloutSaveStore
// Core State : Ashfall.Core.World.FalloutSystemState
// Host Caller: Main.Plans186_189
// Purpose    : Radioactive fallout clouds, dispersion, and shelter sealing
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.World;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class FalloutSaveStore
    {
        public const string FileName = "fallout_save.json";
        public const string SectionName = "fallout";

        private static readonly SaveStore<FalloutSystemState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(FalloutSaveStore),
            SchemaVersionedEnvelope<FalloutSystemState>.Encode,
            SchemaVersionedEnvelope<FalloutSystemState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(FalloutSystemState state) => s_store.TrySave(state);
        public static FalloutSystemState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(FalloutSystemState state) => s_store.CapturePersisted(state);
    }
}
