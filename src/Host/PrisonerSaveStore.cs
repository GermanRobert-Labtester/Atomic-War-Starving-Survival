// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : PrisonerSaveStore
// Core State : Ashfall.Core.Factions.PrisonerState
// Host Caller: Main.Plans178_181
// Purpose    : Captive detention, upkeep, interrogation, escape, and recruitment
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class PrisonerSaveStore
    {
        public const string FileName = "prisoner_save.json";
        public const string SectionName = "prisoner_management";

        private static readonly SaveStore<PrisonerState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(PrisonerSaveStore),
            SchemaVersionedEnvelope<PrisonerState>.Encode,
            SchemaVersionedEnvelope<PrisonerState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(PrisonerState state) => s_store.TrySave(state);
        public static PrisonerState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(PrisonerState state) => s_store.CapturePersisted(state);
    }
}
