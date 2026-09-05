using System;
using System.IO;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class HoldfastTradeArbitrageTests
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
        public void Buy_Neutral_NominalPrice()
        {
            var catalog = LoadCatalog();
            var session = new HoldfastTradeSession(catalog, 1000);
            session.SelectFaction("faction_the_office");
            session.StanceQuery = _ => HoldfastFactionStance.Neutral;

            long price = session.GetBuyPrice("item_triplicate_carbon", "faction_the_office", 1);
            var item = catalog.GetItem("item_triplicate_carbon")!;
            Assert.Equal((int)item.TradeValue, price);

            var result = session.Buy("item_triplicate_carbon", 1, "faction_the_office");
            Assert.True(result.Success);
            Assert.Equal((int)item.TradeValue, result.TotalValue);
        }

        [Fact]
        public void Buy_Allied_AppliesDiscount()
        {
            var catalog = LoadCatalog();
            var session = new HoldfastTradeSession(catalog, 1000);
            session.SelectFaction("faction_the_office");
            session.StanceQuery = _ => HoldfastFactionStance.Allied;

            var item = catalog.GetItem("item_triplicate_carbon")!;
            int expected = (int)Math.Max(1, Math.Round(item.TradeValue * 0.85f));

            long price = session.GetBuyPrice("item_triplicate_carbon", "faction_the_office", 1);
            Assert.Equal(expected, price);

            var result = session.Buy("item_triplicate_carbon", 1, "faction_the_office");
            Assert.True(result.Success);
            Assert.Equal(expected, result.TotalValue);
            Assert.Contains("[Allied discount applied]", result.WhyLine);
        }

        [Fact]
        public void Buy_Hostile_AppliesSurcharge()
        {
            var catalog = LoadCatalog();
            var session = new HoldfastTradeSession(catalog, 1000);
            session.SelectFaction("faction_the_office");
            session.StanceQuery = _ => HoldfastFactionStance.Hostile;

            var item = catalog.GetItem("item_triplicate_carbon")!;
            int expected = (int)Math.Max(1, Math.Round(item.TradeValue * 1.25f));

            long price = session.GetBuyPrice("item_triplicate_carbon", "faction_the_office", 1);
            Assert.Equal(expected, price);

            var result = session.Buy("item_triplicate_carbon", 1, "faction_the_office");
            Assert.True(result.Success);
            Assert.Equal(expected, result.TotalValue);
            Assert.Contains("Hostile surcharge", result.WhyLine);
        }

        [Fact]
        public void Sell_Neutral_NominalPrice()
        {
            var catalog = LoadCatalog();
            var session = new HoldfastTradeSession(catalog, 1000);
            session.SelectFaction("faction_the_office");
            session.SeedInventory("item_triplicate_carbon", 5);
            session.StanceQuery = _ => HoldfastFactionStance.Neutral;

            var item = catalog.GetItem("item_triplicate_carbon")!;
            long price = session.GetSellPrice("item_triplicate_carbon", "faction_the_office", 1);
            Assert.Equal((int)item.TradeValue, price);

            var result = session.Sell("item_triplicate_carbon", 1, "faction_the_office");
            Assert.True(result.Success);
            Assert.Equal((int)item.TradeValue, result.TotalValue);
        }

        [Fact]
        public void Sell_Allied_AppliesBonus()
        {
            var catalog = LoadCatalog();
            var session = new HoldfastTradeSession(catalog, 1000);
            session.SelectFaction("faction_the_office");
            session.SeedInventory("item_triplicate_carbon", 5);
            session.StanceQuery = _ => HoldfastFactionStance.Allied;

            var item = catalog.GetItem("item_triplicate_carbon")!;
            int expected = (int)Math.Max(1, Math.Round(item.TradeValue * 1.15f));

            long price = session.GetSellPrice("item_triplicate_carbon", "faction_the_office", 1);
            Assert.Equal(expected, price);

            var result = session.Sell("item_triplicate_carbon", 1, "faction_the_office");
            Assert.True(result.Success);
            Assert.Equal(expected, result.TotalValue);
            Assert.Contains("[Allied bonus applied]", result.WhyLine);
        }

        [Fact]
        public void Sell_Hostile_AppliesPenalty()
        {
            var catalog = LoadCatalog();
            var session = new HoldfastTradeSession(catalog, 1000);
            session.SelectFaction("faction_the_office");
            session.SeedInventory("item_triplicate_carbon", 5);
            session.StanceQuery = _ => HoldfastFactionStance.Hostile;

            var item = catalog.GetItem("item_triplicate_carbon")!;
            int expected = (int)Math.Max(1, Math.Round(item.TradeValue * 0.75f));

            long price = session.GetSellPrice("item_triplicate_carbon", "faction_the_office", 1);
            Assert.Equal(expected, price);

            var result = session.Sell("item_triplicate_carbon", 1, "faction_the_office");
            Assert.True(result.Success);
            Assert.Equal(expected, result.TotalValue);
            Assert.Contains("Hostile penalty", result.WhyLine);
        }

        [Fact]
        public void WhyLine_HostileStance_ContainsFactionId()
        {
            var catalog = LoadCatalog();
            var session = new HoldfastTradeSession(catalog, 1000);
            session.SelectFaction("faction_the_office");
            session.StanceQuery = _ => HoldfastFactionStance.Hostile;

            string whyBuy = session.GetWhyLine("item_triplicate_carbon", "faction_the_office", true);
            Assert.Contains("faction_the_office", whyBuy);
            Assert.Contains("Hostile surcharge", whyBuy);

            string whySell = session.GetWhyLine("item_triplicate_carbon", "faction_the_office", false);
            Assert.Contains("faction_the_office", whySell);
            Assert.Contains("Hostile penalty", whySell);
        }

        [Fact]
        public void WhyLine_LowStock_ContainsStockWarning()
        {
            var catalog = LoadCatalog();
            var session = new HoldfastTradeSession(catalog, 1000);
            session.SelectFaction("faction_the_office");
            // Buy down stock until stock < 3
            // Default stock is 20
            session.Buy("item_triplicate_carbon", 18, "faction_the_office");
            Assert.Equal(2, session.GetStock("item_triplicate_carbon"));

            string why = session.GetWhyLine("item_triplicate_carbon", "faction_the_office", true);
            Assert.Contains("[Stock critical — limited availability]", why);
        }
    }
}
