// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : InSarMappingSaveStore
// Core State : Ashfall.Core.World.InSarDeformationState
// Host Caller: Main.InSarMapping
// Purpose    : Plan 139 Phase 2 — repeat-pass survey passes and derived
//              deformation intelligence summaries.
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static class InSarMappingSaveStore
    {
        public const string FileName = "insar_deformation_save.json";
        public const string SectionName = "insar_deformation";

        private static readonly SaveStore<InSarDeformationState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(InSarMappingSaveStore),
            SchemaVersionedEnvelope<InSarDeformationState>.Encode,
            SchemaVersionedEnvelope<InSarDeformationState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(InSarDeformationState state) => s_store.TrySave(state);
        public static InSarDeformationState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(InSarDeformationState state) => s_store.CapturePersisted(state);
    }
}
