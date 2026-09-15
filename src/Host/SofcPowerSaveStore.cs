// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : SofcPowerSaveStore
// Core State : SolidOxideFuelCellState
// Host Caller: Main.Plans122to125
// Purpose    : Plan 122 — SOFC plant mode, thermal level, stack health, seal integrity, degradation, faults
// ============================================================================
using Ashfall.Core.Shelter;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class SofcPowerSaveStore
    {
        public const string FileName = "sofc_power_save.json";
        public const string SectionName = "sofc_power";

        private static readonly SaveStore<SolidOxideFuelCellState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(SofcPowerSaveStore),
            SchemaVersionedEnvelope<SolidOxideFuelCellState>.Encode,
            SchemaVersionedEnvelope<SolidOxideFuelCellState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(SolidOxideFuelCellState state) => s_store.TrySave(state);
        public static SolidOxideFuelCellState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(SolidOxideFuelCellState state) => s_store.CapturePersisted(state);
    }
}
