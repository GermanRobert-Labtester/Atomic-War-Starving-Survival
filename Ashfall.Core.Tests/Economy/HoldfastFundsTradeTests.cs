// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Economy;

namespace Ashfall.Core.Tests.Economy
{
    public class HoldfastFundsTradeTests
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
        public void ChitsFromSettlementUnits_FloorsValues_AndPreventsNegative()
        {
            Assert.Equal(0, HoldfastTradeSession.ChitsFromSettlementUnits(0f));
            Assert.Equal(0, HoldfastTradeSession.ChitsFromSettlementUnits(-10.5f));
            Assert.Equal(10, HoldfastTradeSession.ChitsFromSettlementUnits(10.9f));
            Assert.Equal(25, HoldfastTradeSession.ChitsFromSettlementUnits(25.0f));
        }

        [Fact]
        public void BuyWithFunds_DebitsLedger_AndUpdatesStockAndInventory()
        {
            var catalog = LoadCatalog();
            var session = new HoldfastTradeSession(catalog, 100);
            session.SelectFaction("faction_the_office");

            var ledger = new FundsLedger(initialBalance: 100);

            var result = session.BuyWithFunds("item_triplicate_carbon", 1, "faction_the_office", ledger, day: 1);

            Assert.True(result.Success, $"BuyWithFunds failed: {result.Message}");
            Assert.True(result.FundsDelta < 0, "FundsDelta should be negative for a purchase");
            Assert.Equal(100 + result.FundsDelta, ledger.Balance);
            Assert.Equal(FundsFailure.None, result.FundsFailure);
            Assert.True(session.GetHeld("item_triplicate_carbon") > 0);
        }

        [Fact]
        public void BuyWithFunds_InsufficientFunds_FailsAndMutatesNothing()
        {
            var catalog = LoadCatalog();
            var session = new HoldfastTradeSession(catalog, 100);
            session.SelectFaction("faction_the_office");

            var ledger = new FundsLedger(initialBalance: 0); // zero balance

            var result = session.BuyWithFunds("item_triplicate_carbon", 1, "faction_the_office", ledger, day: 1);

            Assert.False(result.Success);
            Assert.Equal(HoldfastTradeFailure.InsufficientFunds, result.Failure);
            Assert.Equal(FundsFailure.InsufficientFunds, result.FundsFailure);
            Assert.Equal(0, ledger.Balance);
            Assert.Equal(0, session.GetHeld("item_triplicate_carbon"));
        }

        [Fact]
        public void SellWithFunds_CreditsLedger_AndUpdatesStockAndInventory()
        {
            var catalog = LoadCatalog();
            var session = new HoldfastTradeSession(catalog, 100);
            session.SelectFaction("faction_the_office");

            var ledger = new FundsLedger(initialBalance: 10);

            // First acquire item via standard Buy so it's in inventory
            var buyRes = session.Buy("item_triplicate_carbon", 1, "faction_the_office");
            Assert.True(buyRes.Success);

            int prevHeld = session.GetHeld("item_triplicate_carbon");
            Assert.True(prevHeld > 0);

            var sellResult = session.SellWithFunds("item_triplicate_carbon", 1, "faction_the_office", ledger, day: 2);

            Assert.True(sellResult.Success, $"SellWithFunds failed: {sellResult.Message}");
            Assert.True(sellResult.FundsDelta > 0, "FundsDelta should be positive for a sale");
            Assert.Equal(10 + sellResult.FundsDelta, ledger.Balance);
            Assert.Equal(FundsFailure.None, sellResult.FundsFailure);
            Assert.Equal(prevHeld - 1, session.GetHeld("item_triplicate_carbon"));
        }
    }
}
