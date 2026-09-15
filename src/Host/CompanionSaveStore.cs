// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : CompanionSaveStore
// Core State : Ashfall.Core.Ecology.CompanionSystemState
// Host Caller: Main.Companion
// Purpose    : Plan 174 — persistent companion animals (care, bond, training,
//              roles, sickness, assignments). Identity = wildlife animal_id.
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Ecology;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class CompanionSaveStore
    {
        public const string FileName = "companion_animals_save.json";
        public const string SectionName = "companion_animals";

        private static readonly SaveStore<CompanionSystemState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(CompanionSaveStore),
            SchemaVersionedEnvelope<CompanionSystemState>.Encode,
            SchemaVersionedEnvelope<CompanionSystemState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(CompanionSystemState state) => s_store.TrySave(state);
        public static CompanionSystemState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(CompanionSystemState state) => s_store.CapturePersisted(state);
    }
}
