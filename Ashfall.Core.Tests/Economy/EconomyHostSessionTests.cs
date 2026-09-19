// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    /// <summary>
    /// Tests for MarketSystem.TickDays, MarketSystem.LoadCatalog, and the
    /// EconomyHostSession.TickDemo contract (advancing days, updating tick counts,
    /// formatted event strings, and state change notifications).
    /// </summary>
    public class EconomyHostSessionTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        private sealed class TestEconomyHostSession : StatefulSessionBase
        {
            public const int DemoSeed = 2026;
            public MarketSystem Market { get; }
            public string LastEvent { get; private set; } = string.Empty;

            public TestEconomyHostSession(MarketSystem? market = null)
            {
                Market = market ?? new MarketSystem();
            }

            public void LoadData(string dataDir)
            {
                Market.LoadCatalog(dataDir);
            }

            public string TickDemo(int days)
            {
                Market.TickDays(days);
                LastEvent = $"Advanced {days} days to Day {Market.Day}.";
                RaiseStateChanged();
                return LastEvent;
            }

            public string BarterDemo(string giveItemId, int giveQuantity, string takeItemId)
            {
                var result = Market.Barter(giveItemId, giveQuantity, takeItemId, Market.Day);
                LastEvent = result.Accepted
                    ? $"Bartered {giveQuantity}x {giveItemId} for {result.Quantity}x {takeItemId}."
                    : $"Barter rejected: {result.RejectReason}.";
                RaiseStateChanged();
                return LastEvent;
            }
        }

        [Fact]
        public void MarketSystem_TickDays_AdvancesDayAndTickCount()
        {
            var market = new MarketSystem();
            Assert.Equal(0, market.Day);
            Assert.Equal(0, market.TickCount);

            market.TickDays(3);
            Assert.Equal(3, market.Day);
            Assert.Equal(3, market.TickCount);

            market.TickDays(2);
            Assert.Equal(5, market.Day);
            Assert.Equal(5, market.TickCount);
        }

        [Fact]
        public void MarketSystem_TickDays_ZeroOrNegativeDays_IsNoOp()
        {
            var market = new MarketSystem();
            market.TickDays(0);
            Assert.Equal(0, market.Day);
            Assert.Equal(0, market.TickCount);

            market.TickDays(-5);
            Assert.Equal(0, market.Day);
            Assert.Equal(0, market.TickCount);
        }

        [Fact]
        public void MarketSystem_TickDays_DeterministicPricingAcrossInstances()
        {
            var catalogResult = new GoodsCatalogLoadResult();
            catalogResult.Goods.Add(new GoodDefinition
            {
                id = "clean_water",
                displayName = "Clean Water",
                category = "food",
                basePrice = 10f,
                volatility = 0.2f,
                elasticity = 1.0f
            });
            var catalog = GoodsCatalogLoader.ToCatalog(catalogResult);

            var market1 = new MarketSystem();
            market1.BindCatalog(catalog);
            market1.TickDays(10);

            var market2 = new MarketSystem();
            market2.BindCatalog(catalog);
            market2.TickDays(10);

            Assert.Equal(market1.Day, market2.Day);
            Assert.Equal(market1.TickCount, market2.TickCount);
            Assert.Equal(market1.GetPrice("clean_water"), market2.GetPrice("clean_water"));
        }

        [Fact]
        public void MarketSystem_LoadCatalog_ValidDirectory_BindsKnownGoods()
        {
            string dataDir = GetDataDir();
            var market = new MarketSystem();
            market.LoadCatalog(dataDir);

            Assert.NotNull(market.FindGood("clean_water"));
            Assert.NotNull(market.FindGood("scrap_metal"));
        }

        [Fact]
        public void MarketSystem_LoadCatalog_InvalidOrEmpty_DoesNotThrow()
        {
            var market = new MarketSystem();
            market.LoadCatalog(string.Empty);
            market.LoadCatalog("/nonexistent/directory/path/that/cannot/exist");
            Assert.Null(market.FindGood("clean_water"));
        }

        [Fact]
        public void EconomyHostSession_TickDemo_AdvancesDayTicksAndRaisesStateChanged()
        {
            using var session = new TestEconomyHostSession();
            Assert.Equal(0, session.Market.Day);
            Assert.Equal(0, session.Market.TickCount);
            Assert.Equal(string.Empty, session.LastEvent);
            Assert.False(session.IsDirty);
            Assert.Equal(0, session.StateVersion);

            bool stateChangedFired = false;
            session.StateChanged += () => stateChangedFired = true;

            string eventMsg = session.TickDemo(3);

            Assert.Equal("Advanced 3 days to Day 3.", eventMsg);
            Assert.Equal("Advanced 3 days to Day 3.", session.LastEvent);
            Assert.Equal(3, session.Market.Day);
            Assert.Equal(3, session.Market.TickCount);
            Assert.True(session.IsDirty);
            Assert.Equal(1, session.StateVersion);
            Assert.True(stateChangedFired);

            stateChangedFired = false;
            string eventMsg2 = session.TickDemo(2);

            Assert.Equal("Advanced 2 days to Day 5.", eventMsg2);
            Assert.Equal("Advanced 2 days to Day 5.", session.LastEvent);
            Assert.Equal(5, session.Market.Day);
            Assert.Equal(5, session.Market.TickCount);
            Assert.True(session.IsDirty);
            Assert.Equal(2, session.StateVersion);
            Assert.True(stateChangedFired);
        }

        [Fact]
        public void EconomyHostSession_TickDemo_ZeroDays_UpdatesEventWithoutAdvancingDay()
        {
            using var session = new TestEconomyHostSession();
            string eventMsg = session.TickDemo(0);

            Assert.Equal("Advanced 0 days to Day 0.", eventMsg);
            Assert.Equal("Advanced 0 days to Day 0.", session.LastEvent);
            Assert.Equal(0, session.Market.Day);
            Assert.Equal(0, session.Market.TickCount);
            Assert.True(session.IsDirty);
            Assert.Equal(1, session.StateVersion);
        }

        [Fact]
        public void EconomyHostSession_LoadData_BindsCatalogFromDataDir()
        {
            string dataDir = GetDataDir();
            using var session = new TestEconomyHostSession();
            session.LoadData(dataDir);

            Assert.NotNull(session.Market.FindGood("clean_water"));
        }

        [Fact]
        public void EconomyHostSession_BarterDemo_Accepted_ExchangesGoodsUpdatesLastEventAndRaisesStateChanged()
        {
            var catalogResult = new GoodsCatalogLoadResult();
            catalogResult.Goods.Add(new GoodDefinition
            {
                id = "scrap_metal",
                displayName = "Scrap Metal",
                category = "materials",
                basePrice = 5f,
                volatility = 0.1f,
                elasticity = 1.0f
            });
            catalogResult.Goods.Add(new GoodDefinition
            {
                id = "clean_water",
                displayName = "Clean Water",
                category = "food",
                basePrice = 10f,
                volatility = 0.1f,
                elasticity = 1.0f
            });
            var catalog = GoodsCatalogLoader.ToCatalog(catalogResult);

            using var session = new TestEconomyHostSession();
            session.Market.BindCatalog(catalog);

            bool stateChangedFired = false;
            session.StateChanged += () => stateChangedFired = true;

            // 4 scrap_metal @ 5.0 = 20.0 value -> 2 clean_water @ 10.0
            string result = session.BarterDemo("scrap_metal", 4, "clean_water");

            Assert.Equal("Bartered 4x scrap_metal for 2x clean_water.", result);
            Assert.Equal("Bartered 4x scrap_metal for 2x clean_water.", session.LastEvent);
            Assert.True(stateChangedFired);
            Assert.True(session.IsDirty);
            Assert.Equal(1, session.StateVersion);

            // Ledger contains two legs: give leg (-4 scrap_metal) and take leg (+2 clean_water)
            var ledger = session.Market.State.ledger;
            Assert.Equal(2, ledger.Count);
            Assert.Equal("scrap_metal", ledger[1].itemId);
            Assert.Equal(-4, ledger[1].quantity);
            Assert.Equal("clean_water", ledger[0].itemId);
            Assert.Equal(2, ledger[0].quantity);
        }

        [Fact]
        public void EconomyHostSession_BarterDemo_UnknownGiveItem_ReturnsRejectionMessage()
        {
            var catalogResult = new GoodsCatalogLoadResult();
            catalogResult.Goods.Add(new GoodDefinition
            {
                id = "clean_water",
                displayName = "Clean Water",
                category = "food",
                basePrice = 10f
            });
            var catalog = GoodsCatalogLoader.ToCatalog(catalogResult);

            using var session = new TestEconomyHostSession();
            session.Market.BindCatalog(catalog);

            string result = session.BarterDemo("unknown_material", 4, "clean_water");

            Assert.Equal("Barter rejected: unknown give good.", result);
            Assert.Equal("Barter rejected: unknown give good.", session.LastEvent);
            Assert.Empty(session.Market.State.ledger);
        }

        [Fact]
        public void EconomyHostSession_BarterDemo_UnknownTakeItem_ReturnsRejectionMessage()
        {
            var catalogResult = new GoodsCatalogLoadResult();
            catalogResult.Goods.Add(new GoodDefinition
            {
                id = "scrap_metal",
                displayName = "Scrap Metal",
                category = "materials",
                basePrice = 5f
            });
            var catalog = GoodsCatalogLoader.ToCatalog(catalogResult);

            using var session = new TestEconomyHostSession();
            session.Market.BindCatalog(catalog);

            string result = session.BarterDemo("scrap_metal", 4, "unknown_item");

            Assert.Equal("Barter rejected: unknown take good.", result);
            Assert.Equal("Barter rejected: unknown take good.", session.LastEvent);
            Assert.Empty(session.Market.State.ledger);
        }

        [Fact]
        public void EconomyHostSession_BarterDemo_ZeroOrNegativeQuantity_ReturnsRejectionMessage()
        {
            var catalogResult = new GoodsCatalogLoadResult();
            catalogResult.Goods.Add(new GoodDefinition { id = "scrap_metal", displayName = "Scrap", category = "materials", basePrice = 5f });
            catalogResult.Goods.Add(new GoodDefinition { id = "clean_water", displayName = "Water", category = "food", basePrice = 10f });
            var catalog = GoodsCatalogLoader.ToCatalog(catalogResult);

            using var session = new TestEconomyHostSession();
            session.Market.BindCatalog(catalog);

            string zeroResult = session.BarterDemo("scrap_metal", 0, "clean_water");
            Assert.Equal("Barter rejected: quantity must be > 0.", zeroResult);
            Assert.Equal("Barter rejected: quantity must be > 0.", session.LastEvent);

            string negResult = session.BarterDemo("scrap_metal", -2, "clean_water");
            Assert.Equal("Barter rejected: quantity must be > 0.", negResult);
            Assert.Equal("Barter rejected: quantity must be > 0.", session.LastEvent);
        }

        [Fact]
        public void EconomyHostSession_BarterDemo_TakeGoodTooValuable_ReturnsRejectionMessage()
        {
            var catalogResult = new GoodsCatalogLoadResult();
            catalogResult.Goods.Add(new GoodDefinition { id = "scrap_metal", displayName = "Scrap", category = "materials", basePrice = 2f });
            catalogResult.Goods.Add(new GoodDefinition { id = "power_cell", displayName = "Cell", category = "electronics", basePrice = 50f });
            var catalog = GoodsCatalogLoader.ToCatalog(catalogResult);

            using var session = new TestEconomyHostSession();
            session.Market.BindCatalog(catalog);

            // 1 scrap_metal @ 2.0 = 2.0 value < 50.0 value of 1 power_cell -> 0 takeQuantity
            string result = session.BarterDemo("scrap_metal", 1, "power_cell");

            Assert.Equal("Barter rejected: take good too valuable for the offered amount.", result);
            Assert.Equal("Barter rejected: take good too valuable for the offered amount.", session.LastEvent);
            Assert.Empty(session.Market.State.ledger);
        }

        [Fact]
        public void EconomyHostSession_BarterDemo_WithRealCatalog_ExecutesSuccessfully()
        {
            string dataDir = GetDataDir();
            using var session = new TestEconomyHostSession();
            session.LoadData(dataDir);

            // Using real catalog goods: 20 scrap_metal for clean_water
            string result = session.BarterDemo("scrap_metal", 20, "clean_water");

            Assert.StartsWith("Bartered 20x scrap_metal for ", result);
            Assert.EndsWith("x clean_water.", result);
            Assert.Equal(result, session.LastEvent);
            Assert.Equal(2, session.Market.State.ledger.Count);
        }
    }
}
