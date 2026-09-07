// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : CargoAirdropSaveStore
// Core State : Ashfall.Core.World.CargoAirdropState
// Host Caller: Main.Plans202_205
// Purpose    : Airdrop events, crate contents (generated once), beacon and
//              interception state
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static class CargoAirdropSaveStore
    {
        public const string FileName = "cargo_airdrop_save.json";
        public const string SectionName = "cargo_airdrop";

        private static readonly SaveStore<CargoAirdropState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(CargoAirdropSaveStore),
            SchemaVersionedEnvelope<CargoAirdropState>.Encode,
            SchemaVersionedEnvelope<CargoAirdropState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(CargoAirdropState state) => s_store.TrySave(state);
        public static CargoAirdropState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(CargoAirdropState state) => s_store.CapturePersisted(state);
    }
}
