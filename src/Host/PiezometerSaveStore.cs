// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : PiezometerSaveStore
// Core State : Ashfall.Core.Shelter.HydrogeologyNetworkState
// Host Caller: Main.Piezometer
// Purpose    : Plan 189 — aquifer monitoring network state (constructed nodes,
//              drawdown, contamination risk, isolation zones) so the intake
//              advisory bridge survives reload. No liter ledger lives here.
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class PiezometerSaveStore
    {
        public const string FileName = "piezometer_network_save.json";
        public const string SectionName = "piezometer_network";

        private static readonly SaveStore<HydrogeologyNetworkState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(PiezometerSaveStore),
            SchemaVersionedEnvelope<HydrogeologyNetworkState>.Encode,
            SchemaVersionedEnvelope<HydrogeologyNetworkState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(HydrogeologyNetworkState state) => s_store.TrySave(state);
        public static HydrogeologyNetworkState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(HydrogeologyNetworkState state) => s_store.CapturePersisted(state);
    }
}
