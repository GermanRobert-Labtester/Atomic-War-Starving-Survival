// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : HydraulicExtrusionSaveStore
// Core State : Ashfall.Core.Foundry.HydraulicExtrusionState
// Host Caller: Main.HydraulicExtrusion
// Purpose    : Plan 140 Phase 2 — extrusion machines, tooling condition,
//              in-flight batches, completed quality grades.
// ============================================================================
using Ashfall.Core.Foundry;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class HydraulicExtrusionSaveStore
    {
        public const string FileName = "hydraulic_extrusion_save.json";
        public const string SectionName = "hydraulic_extrusion";

        private static readonly SaveStore<HydraulicExtrusionState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(HydraulicExtrusionSaveStore),
            SchemaVersionedEnvelope<HydraulicExtrusionState>.Encode,
            SchemaVersionedEnvelope<HydraulicExtrusionState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(HydraulicExtrusionState state) => s_store.TrySave(state);
        public static HydraulicExtrusionState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(HydraulicExtrusionState state) => s_store.CapturePersisted(state);
    }
}
