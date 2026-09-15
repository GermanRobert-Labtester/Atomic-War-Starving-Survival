// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : RadioProgramProductionSaveStore
// Core State : Ashfall.Core.Radio.RadioProgramProductionState
// Host Caller: Main.RadioProgramProduction
// Purpose    : Plan 173 Phase 2 — player program prep/delivery jobs + follow-ups
// ============================================================================
using Ashfall.Core.Radio;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class RadioProgramProductionSaveStore
    {
        public const string FileName = "radio_program_production_save.json";
        public const string SectionName = "radio_program_production";

        private static readonly SaveStore<RadioProgramProductionState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(RadioProgramProductionSaveStore),
            SchemaVersionedEnvelope<RadioProgramProductionState>.Encode,
            SchemaVersionedEnvelope<RadioProgramProductionState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(RadioProgramProductionState state) => s_store.TrySave(state);
        public static RadioProgramProductionState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(RadioProgramProductionState state) => s_store.CapturePersisted(state);
    }
}
