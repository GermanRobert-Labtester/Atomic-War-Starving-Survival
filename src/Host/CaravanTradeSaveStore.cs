// SPDX-License-Identifier: MIT
// ASHFALL Caravan Trade Network save store facade (Plan 85 / Task 1).

using Ashfall.Core.Economy;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Caravan trade network persistence facade delegating to the Core SaveStore service.
    /// </summary>
    public static class CaravanTradeSaveStore
    {
        public const string FileName = "caravan_trade_network_save.json";
        public const string SectionName = "caravan_trade_network";

        private static readonly SaveStore<CaravanTradeNetworkSave> s_store =
            SaveStoreHub.Checksummed<CaravanTradeNetworkSave>(
                FileName,
                nameof(CaravanTradeSaveStore),
                allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(CaravanTradeNetworkSave state) => s_store.CaptureBare(state);
        public static CaravanTradeNetworkSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        public static string TryCapture(CaravanTradeNetworkSave state) => s_store.CaptureBare(state);
        public static CaravanTradeNetworkSave? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(CaravanTradeNetworkSave state) => s_store.TrySave(state);
        public static CaravanTradeNetworkSave? TryLoad() => s_store.TryLoad();

        public static string TryCapturePersisted(CaravanTradeNetworkSave state) => s_store.CapturePersisted(state);
    }
}
