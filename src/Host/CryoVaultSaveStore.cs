// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : CryoVaultSaveStore
// Core State : Ashfall.Core.Shelter.CryoVaultSaveState
// Host Caller: Main.PlansB68_B69
// Purpose    : Plan B69 — cryo canisters, viability, coolant reserve,
//              insulation level, breach state, released history
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class CryoVaultSaveStore
    {
        public const string FileName = "cryo_vault_save.json";
        public const string SectionName = "cryo_vault";

        private static readonly SaveStore<CryoVaultSaveState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(CryoVaultSaveStore),
            SchemaVersionedEnvelope<CryoVaultSaveState>.Encode,
            SchemaVersionedEnvelope<CryoVaultSaveState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(CryoVaultSaveState state) => s_store.TrySave(state);
        public static CryoVaultSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(CryoVaultSaveState state) => s_store.CapturePersisted(state);
    }
}
