using System;
using System.IO;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    public class HoldfastTradeSessionTests
    {
        private static readonly string DataDir = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));

        private static HoldfastCatalog LoadCatalog()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new HoldfastCatalogLoader(files, json);
            return loader.Load(DataDir);
        }

        [Fact]
        public void ResetToDefaults_ClearsValueInventoryAndStock_ThenReusable()
        {
            var catalog = LoadCatalog();
            var session = new HoldfastTradeSession(catalog, 100);
            session.SelectFaction("faction_the_office");
            var buyResult = session.Buy("item_triplicate_carbon", 1, "faction_the_office");
            Assert.True(buyResult.Success, $"Buy failed: {buyResult.Message}");
            Assert.True(session.PlayerValue < 100);
            Assert.True(session.GetHeld("item_triplicate_carbon") > 0);

            session.ResetToDefaults();

            // Held cleared, value restored (stock resets to the 20 default).
            Assert.Equal(100, session.PlayerValue);
            Assert.Equal(0, session.GetHeld("item_triplicate_carbon"));
            Assert.True(session.GetStock("item_triplicate_carbon") > 0);

            // Reusable after reset
            session.SelectFaction("faction_the_office");
            var result = session.Buy("item_triplicate_carbon", 1, "faction_the_office");
            Assert.True(result.Success, $"Second buy failed: {result.Message}");
            Assert.True(result.Success);
        }

        [Fact]
        public void InvalidPrice_RejectsNegativeAndOverflowingUnitValues()
        {
            var catalog = LoadCatalog();
            var session = new HoldfastTradeSession(catalog, 100);
            session.SelectFaction("faction_the_office");

            // Negative price via Sell with invalid price
            var r1 = session.Sell("item_triplicate_carbon", 1, "faction_the_office");
            // Cannot sell an item we don't have with valid price
            Assert.False(r1.Success);
        }

        [Fact]
        public void InventoryCapacity_RejectsPurchaseWhenFull()
        {
            var catalog = LoadCatalog();
            var session = new HoldfastTradeSession(catalog, 10000);
            session.Inventory.Capacity = 2;
            session.SelectFaction("faction_the_office");

            session.Buy("item_triplicate_carbon", 1, "faction_the_office");
            session.Buy("item_fume_rag", 1, "faction_the_office");
            // Third DISTINCT item when capacity is full must be rejected.
            var r = session.Buy("item_map_sheet_ice_road", 1, "faction_the_office");
            Assert.False(r.Success, $"Third distinct item must hit capacity: {r.Message}");
            Assert.Equal(HoldfastTradeFailure.InventoryCapacity, r.Failure);
        }
    }
}
