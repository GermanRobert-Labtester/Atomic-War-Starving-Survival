// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : AmputationSaveStore
// Core State : Ashfall.Core.Medical.AmputationSystemState
// Host Caller: Main.Plans190_193
// Purpose    : Infection progression, amputations, prosthetics and bionics
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class AmputationSaveStore
    {
        public const string FileName = "amputation_save.json";
        public const string SectionName = "amputation";

        private static readonly SaveStore<AmputationSystemState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(AmputationSaveStore),
            SchemaVersionedEnvelope<AmputationSystemState>.Encode,
            SchemaVersionedEnvelope<AmputationSystemState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(AmputationSystemState state) => s_store.TrySave(state);
        public static AmputationSystemState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(AmputationSystemState state) => s_store.CapturePersisted(state);
    }
}
