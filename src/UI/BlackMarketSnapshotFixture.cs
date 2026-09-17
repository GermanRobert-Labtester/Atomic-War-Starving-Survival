// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Godot;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Deterministic populated fixture for the Plan 211 black-market panel.
    /// It uses the production catalog, price, wallet, inventory, host, and
    /// settlement seams so the golden covers the real action states.
    /// </summary>
    internal static class BlackMarketSnapshotFixture
    {
        private const string ContactId = "faction_wasteland_outlaws";

        public static IDisposable? Bind(Node node)
        {
            if (node is not BlackMarketPanel panel)
                return null;

            string dataDir = CatalogPath.ResolveDataDir();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var goodsLoad = GoodsCatalogLoader.Load(dataDir, fileIO, serializer);
            if (goodsLoad.HasErrors)
                throw new InvalidOperationException("Black-market snapshot goods catalog failed: " +
                    string.Join("; ", goodsLoad.Errors));
            var prices = new MarketSystem();
            prices.BindCatalog(GoodsCatalogLoader.ToCatalog(goodsLoad));

            var blackLoad = BlackMarketInventoryCatalogLoader.Load(dataDir, fileIO, serializer);
            if (blackLoad.HasErrors)
                throw new InvalidOperationException("Black-market snapshot catalog failed: " +
                    string.Join("; ", blackLoad.Errors));
            var system = new BlackMarketSystem();
            system.BindCatalog(BlackMarketInventoryCatalogLoader.ToCatalog(blackLoad));
            system.BindMarket(prices);
            if (!system.DiscoverContact(ContactId, day: 1))
                throw new InvalidOperationException("Black-market snapshot contact is missing: " + ContactId);
            system.EnsureStockSnapshot(ContactId, day: 2, new SeededRng(211));

            var items = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);
            var inventory = new InventoryContainer();
            var medicine = items.Get("medical_kit")
                ?? throw new InvalidOperationException("Black-market snapshot medical_kit is missing.");
            if (!inventory.Add(medicine, 2))
                throw new InvalidOperationException("Black-market snapshot inventory seed failed.");

            var wallet = new HoldfastTradeSession(new HoldfastCatalog(), 2_000, inventory);
            var session = new BlackMarketHostSession(system);
            session.BindSettlementOwners(wallet, inventory, items, () => 2);
            panel.Bind(session);
            return new FixtureOwner(panel, session);
        }

        private sealed class FixtureOwner : IDisposable
        {
            private BlackMarketPanel? _panel;
            private BlackMarketHostSession? _session;

            public FixtureOwner(BlackMarketPanel panel, BlackMarketHostSession session)
            {
                _panel = panel;
                _session = session;
            }

            public void Dispose()
            {
                _panel?.Unbind();
                _session?.Dispose();
                _panel = null;
                _session = null;
            }
        }
    }
}
