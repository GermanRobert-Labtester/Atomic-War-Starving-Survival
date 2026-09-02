// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ForcedLaborSaveStore
// Core State : Ashfall.Core.Factions.ForcedLaborState
// Host Caller: Main.Plans182_185
// Purpose    : Captive forced labor assignments, cruelty index, and rebellion risks
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class ForcedLaborSaveStore
    {
        public const string FileName = "forced_labor_save.json";
        public const string SectionName = "forced_labor";

        private static readonly SaveStore<ForcedLaborState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ForcedLaborSaveStore),
            SchemaVersionedEnvelope<ForcedLaborState>.Encode,
            SchemaVersionedEnvelope<ForcedLaborState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(ForcedLaborState state) => s_store.TrySave(state);
        public static ForcedLaborState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(ForcedLaborState state) => s_store.CapturePersisted(state);
    }
}
