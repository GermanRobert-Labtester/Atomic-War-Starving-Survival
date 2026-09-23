// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class HoldfastTradeIntegrityTests
    {
        private const string Item = "item_triplicate_carbon";
        private const string Faction = "faction_the_office";

        private static HoldfastTradeSession CreateSession(long balance = 100)
        {
            string dataDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,
                "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            var catalog = new HoldfastCatalogLoader(new FileSystemIO(),
                new SystemTextJsonSerializer()).Load(dataDir);
            var session = new HoldfastTradeSession(catalog, balance);
            session.Inventory.MaxWeight = 0;
            return session;
        }

        [Fact]
        public void SeedInventory_RejectedCapacityDoesNotCreateSellableOrSavedItems()
        {
            var session = CreateSession();
            session.Inventory.Capacity = 0;
            int notifications = 0;
            session.StateChanged += () => notifications++;

            session.SeedInventory(Item, 1);

            Assert.Equal(0, session.GetHeld(Item));
            Assert.Empty(session.Inventory.Items);
            Assert.Empty(session.CaptureState().held);
            Assert.Equal(0, notifications);
            Assert.False(session.Sell(Item, 1, Faction).Success);
            Assert.Equal(100, session.PlayerValue);
        }

        [Fact]
        public void SeedInventory_CountOverflowPreservesExistingItems()
        {
            var session = CreateSession();
            session.SeedInventory(Item, int.MaxValue);
            int notifications = 0;
            session.StateChanged += () => notifications++;

            session.SeedInventory(Item, 1);

            Assert.Equal(int.MaxValue, session.GetHeld(Item));
            Assert.Equal(int.MaxValue, session.Inventory.Items[Item]);
            Assert.Equal(0, notifications);
        }

        [Fact]
        public void Buy_CountOverflowIsUnavailableAndMutatesNothing()
        {
            var session = CreateSession();
            session.SeedInventory(Item, int.MaxValue);
            int stock = session.GetStock(Item);

            Assert.False(session.PreviewBuy(Item, 1, Faction).IsAvailable);
            var result = session.Buy(Item, 1, Faction);

            Assert.False(result.Success);
            Assert.Equal(HoldfastTradeFailure.InventoryCapacity, result.Failure);
            Assert.Equal(int.MaxValue, session.GetHeld(Item));
            Assert.Equal(stock, session.GetStock(Item));
            Assert.Equal(100, session.PlayerValue);
        }

        [Fact]
        public void BuyWithFunds_PreservesWholeChitsBeyondFloatPrecision()
        {
            var session = CreateSession();
            const int quantity = 16_777_217;
            session.SetStock(Item, quantity);
            int quote = checked((int)session.GetBuyPrice(Item, Faction, quantity));
            var ledger = new FundsLedger(quote);

            var result = session.BuyWithFunds(Item, quantity, Faction, ledger, 1);

            Assert.True(result.Success, result.Message);
            Assert.Equal(-quote, result.FundsDelta);
            Assert.Equal(quote, result.TotalValue);
            Assert.Equal(0, ledger.Balance);
            Assert.Equal(quantity, session.GetHeld(Item));
            Assert.Equal(-quote, Assert.Single(ledger.Movements).Delta);
        }

        [Fact]
        public void SellWithFunds_PreservesWholeChitsBeyondFloatPrecision()
        {
            var session = CreateSession();
            const int quantity = 16_777_217;
            session.SeedInventory(Item, quantity);
            int quote = checked((int)session.GetSellPrice(Item, Faction, quantity));
            var ledger = new FundsLedger();

            var result = session.SellWithFunds(Item, quantity, Faction, ledger, 1);

            Assert.True(result.Success, result.Message);
            Assert.Equal(quote, result.FundsDelta);
            Assert.Equal(quote, ledger.Balance);
            Assert.Equal(0, session.GetHeld(Item));
            Assert.Equal(quote, Assert.Single(ledger.Movements).Delta);
        }

        [Fact]
        public void BuyWithFunds_UnrepresentableQuoteCannotBecomeOneChit()
        {
            var session = CreateSession();
            int quantity = checked((int)(int.MaxValue / session.GetBuyPrice(Item, Faction) + 1));
            session.SetStock(Item, quantity);
            var ledger = new FundsLedger(int.MaxValue);

            var result = session.BuyWithFunds(Item, quantity, Faction, ledger, 1);

            Assert.False(result.Success);
            Assert.Equal(HoldfastTradeFailure.InvalidPrice, result.Failure);
            Assert.Equal(int.MaxValue, ledger.Balance);
            Assert.Empty(ledger.Movements);
            Assert.Equal(quantity, session.GetStock(Item));
            Assert.Equal(0, session.GetHeld(Item));
        }

        [Fact]
        public void SellWithFunds_UnrepresentableQuotePreservesItemsAndFunds()
        {
            var session = CreateSession();
            int quantity = checked((int)(int.MaxValue / session.GetSellPrice(Item, Faction) + 1));
            session.SeedInventory(Item, quantity);
            int stock = session.GetStock(Item);
            var ledger = new FundsLedger();

            var result = session.SellWithFunds(Item, quantity, Faction, ledger, 1);

            Assert.False(result.Success);
            Assert.Equal(HoldfastTradeFailure.InvalidPrice, result.Failure);
            Assert.Equal(0, ledger.Balance);
            Assert.Empty(ledger.Movements);
            Assert.Equal(stock, session.GetStock(Item));
            Assert.Equal(quantity, session.GetHeld(Item));
        }

        [Fact]
        public void Buy_UnrepresentableQuoteIsRejectedByPreviewAndExecution()
        {
            var session = CreateSession(long.MaxValue);
            int quantity = checked((int)(int.MaxValue / session.GetBuyPrice(Item, Faction) + 1));
            session.SetStock(Item, quantity);

            Assert.False(session.PreviewBuy(Item, quantity, Faction).IsAvailable);
            var result = session.Buy(Item, quantity, Faction);

            Assert.False(result.Success);
            Assert.Equal(HoldfastTradeFailure.InvalidPrice, result.Failure);
            Assert.Equal(long.MaxValue, session.PlayerValue);
            Assert.Equal(quantity, session.GetStock(Item));
            Assert.Equal(0, session.GetHeld(Item));
        }

        [Fact]
        public void Sell_UnrepresentableQuoteIsRejectedByPreviewAndExecution()
        {
            var session = CreateSession();
            int quantity = checked((int)(int.MaxValue / session.GetSellPrice(Item, Faction) + 1));
            session.SeedInventory(Item, quantity);
            int stock = session.GetStock(Item);

            Assert.False(session.PreviewSell(Item, quantity, Faction).IsAvailable);
            var result = session.Sell(Item, quantity, Faction);

            Assert.False(result.Success);
            Assert.Equal(HoldfastTradeFailure.InvalidPrice, result.Failure);
            Assert.Equal(100, session.PlayerValue);
            Assert.Equal(stock, session.GetStock(Item));
            Assert.Equal(quantity, session.GetHeld(Item));
        }

        [Fact]
        public void Sell_WalletOverflowIsRejectedByPreviewAndExecution()
        {
            var session = CreateSession(long.MaxValue);
            session.SeedInventory(Item, 1);
            int stock = session.GetStock(Item);

            Assert.False(session.PreviewSell(Item, 1, Faction).IsAvailable);
            var result = session.Sell(Item, 1, Faction);

            Assert.False(result.Success);
            Assert.Equal(long.MaxValue, session.PlayerValue);
            Assert.Equal(stock, session.GetStock(Item));
            Assert.Equal(1, session.GetHeld(Item));
        }

        [Fact]
        public void Sell_StockOverflowIsRejectedByPreviewAndExecution()
        {
            var session = CreateSession();
            session.SeedInventory(Item, 1);
            session.SetStock(Item, int.MaxValue);

            Assert.False(session.PreviewSell(Item, 1, Faction).IsAvailable);
            var result = session.Sell(Item, 1, Faction);

            Assert.False(result.Success);
            Assert.Equal(100, session.PlayerValue);
            Assert.Equal(int.MaxValue, session.GetStock(Item));
            Assert.Equal(1, session.GetHeld(Item));
        }

        [Fact]
        public void SellWithFunds_StockOverflowPreservesFundsAndItems()
        {
            var session = CreateSession();
            session.SeedInventory(Item, 1);
            session.SetStock(Item, int.MaxValue);
            var ledger = new FundsLedger(100);

            var result = session.SellWithFunds(Item, 1, Faction, ledger, 1);

            Assert.False(result.Success);
            Assert.Equal(100, ledger.Balance);
            Assert.Empty(ledger.Movements);
            Assert.Equal(int.MaxValue, session.GetStock(Item));
            Assert.Equal(1, session.GetHeld(Item));
        }

        [Fact]
        public void PreviewSell_EmbargoMatchesExecutionAndMutatesNothing()
        {
            var session = CreateSession();
            session.SeedInventory(Item, 1);
            session.EmbargoQuery = faction => faction == Faction;
            int stock = session.GetStock(Item);

            var preview = session.PreviewSell(Item, 1, Faction);
            var result = session.Sell(Item, 1, Faction);

            Assert.False(preview.IsAvailable);
            Assert.Equal("trade.embargoed", preview.MessageKey);
            Assert.Equal(HoldfastTradeFailure.Embargoed, result.Failure);
            Assert.Equal(100, session.PlayerValue);
            Assert.Equal(stock, session.GetStock(Item));
            Assert.Equal(1, session.GetHeld(Item));
        }

        [Fact]
        public void ChitConversion_RejectsNonFiniteAndOutOfRangeInputs()
        {
            foreach (float units in new[] { float.NaN, float.PositiveInfinity,
                float.NegativeInfinity, (float)int.MaxValue, float.MaxValue })
            {
                Assert.Throws<ArgumentOutOfRangeException>(() =>
                    HoldfastTradeSession.ChitsFromSettlementUnits(units));
            }
        }
    }
}
