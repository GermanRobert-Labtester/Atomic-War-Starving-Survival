// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class Plan211BlackMarketSettlementTests
    {
        private const string Syndicate = "faction_wasteland_outlaws";
        private const string Entry = "black_market_field_medicine";
        private const string Item = "medical_kit";

        private sealed class Context
        {
            public BlackMarketSystem Market { get; init; } = null!;
            public HoldfastTradeSession Wallet { get; init; } = null!;
            public Ashfall.Core.Inventory.Inventory Inventory { get; init; } = null!;
            public ItemCatalog Items { get; init; } = null!;
            public BlackMarketSettlementService Settlement { get; init; } = null!;
        }

        private static string DataDir()
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

        private static Context Create(long startingValue = 10_000, int stock = 5)
        {
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var marketLoad = GoodsCatalogLoader.Load(DataDir(), fileIO, serializer);
            Assert.False(marketLoad.HasErrors, string.Join("; ", marketLoad.Errors));
            var prices = new MarketSystem();
            prices.BindCatalog(GoodsCatalogLoader.ToCatalog(marketLoad));

            var blackLoad = BlackMarketInventoryCatalogLoader.Load(DataDir(), fileIO, serializer);
            Assert.False(blackLoad.HasErrors, string.Join("; ", blackLoad.Errors));
            var blackMarket = new BlackMarketSystem();
            blackMarket.BindCatalog(BlackMarketInventoryCatalogLoader.ToCatalog(blackLoad));
            blackMarket.BindMarket(prices);
            Assert.True(blackMarket.DiscoverContact(Syndicate, 1));
            blackMarket.State.stock.Add(new BlackMarketStockLine
            {
                entryId = Entry,
                quantity = stock,
                generatedDay = 2
            });
            blackMarket.State.stockOwners.Add(Syndicate);

            var items = ItemCatalogLoader.LoadCatalog(DataDir(), fileIO, serializer);
            var inventory = new Ashfall.Core.Inventory.Inventory();
            var wallet = new HoldfastTradeSession(new HoldfastCatalog(), startingValue, inventory);
            return new Context
            {
                Market = blackMarket,
                Wallet = wallet,
                Inventory = inventory,
                Items = items,
                Settlement = new BlackMarketSettlementService(blackMarket, wallet, inventory, items)
            };
        }

        [Fact]
        public void RealBlackMarketItems_AllResolveToCanonicalInventoryDefinitions()
        {
            var ctx = Create();
            foreach (var entry in ctx.Market.Catalog.EntriesById.Values)
                Assert.NotNull(ctx.Items.Get(entry.item_id));
            Assert.NotNull(ctx.Items.Get("diamond"));
        }

        [Fact]
        public void Buy_SettlesWalletInventoryAndStock_UsingCeiling()
        {
            var ctx = Create();
            var preview = ctx.Settlement.PreviewBuy(Syndicate, Entry, 2, day: 2);
            Assert.True(preview.IsAvailable, preview.Message);
            long walletBefore = ctx.Wallet.Value;
            int stockBefore = ctx.Market.GetStockLine(Syndicate, Entry)!.quantity;

            var result = ctx.Settlement.Buy(Syndicate, Entry, 2, day: 2);

            Assert.True(result.Success, result.Message);
            Assert.Equal((long)Math.Ceiling(ctx.Market.GetBuyPrice(
                Syndicate, ctx.Market.Catalog.FindEntry(Entry)!) * 2f), result.SettlementUnits);
            Assert.Equal(walletBefore - result.SettlementUnits, ctx.Wallet.Value);
            Assert.Equal(2, ctx.Inventory.CountById(Item));
            Assert.Equal(stockBefore - 2, ctx.Market.GetStockLine(Syndicate, Entry)!.quantity);
        }

        [Fact]
        public void Buy_InsufficientFunds_LeavesEveryOwnerUnchanged()
        {
            var ctx = Create(startingValue: 1);
            int stockBefore = ctx.Market.GetStockLine(Syndicate, Entry)!.quantity;

            var result = ctx.Settlement.Buy(Syndicate, Entry, 1, day: 2);

            Assert.False(result.Success);
            Assert.Equal("insufficient_funds", result.ReasonId);
            Assert.Equal(1, ctx.Wallet.Value);
            Assert.Equal(0, ctx.Inventory.CountById(Item));
            Assert.Equal(stockBefore, ctx.Market.GetStockLine(Syndicate, Entry)!.quantity);
        }

        [Fact]
        public void Buy_NoStock_LeavesEveryOwnerUnchanged()
        {
            var ctx = Create(stock: 0);
            var result = ctx.Settlement.Buy(Syndicate, Entry, 1, day: 2);

            Assert.False(result.Success);
            Assert.Equal("insufficient_stock", result.ReasonId);
            Assert.Equal(10_000, ctx.Wallet.Value);
            Assert.Equal(0, ctx.Inventory.CountById(Item));
            Assert.Equal(0, ctx.Market.GetStockLine(Syndicate, Entry)!.quantity);
        }

        [Fact]
        public void Buy_AccessTierFailure_IsOwnedByBlackMarketPreflight()
        {
            var ctx = Create();
            const string restricted = "black_market_pre_war_armament";
            ctx.Market.State.stock.Add(new BlackMarketStockLine
            {
                entryId = restricted,
                quantity = 1,
                generatedDay = 2
            });
            ctx.Market.State.stockOwners.Add(Syndicate);

            var result = ctx.Settlement.Buy(Syndicate, restricted, 1, day: 2);

            Assert.False(result.Success);
            Assert.Equal("access_tier_too_low", result.ReasonId);
            Assert.Equal(10_000, ctx.Wallet.Value);
            Assert.Equal(0, ctx.Inventory.CountById("weapon_sidearm"));
            Assert.Equal(1, ctx.Market.GetStockLine(Syndicate, restricted)!.quantity);
        }

        [Fact]
        public void Buy_InventoryCapacityFailure_LeavesEveryOwnerUnchanged()
        {
            var ctx = Create();
            ctx.Inventory.Capacity = 1;
            Assert.True(ctx.Inventory.Add(new ItemDefinition
            {
                id = "occupied_slot",
                displayName = "Occupied",
                stackMax = 1,
                weight = 0f
            }, 1));
            int stockBefore = ctx.Market.GetStockLine(Syndicate, Entry)!.quantity;

            var result = ctx.Settlement.Buy(Syndicate, Entry, 1, day: 2);

            Assert.False(result.Success);
            Assert.Equal("inventory_capacity", result.ReasonId);
            Assert.Equal(10_000, ctx.Wallet.Value);
            Assert.Equal(0, ctx.Inventory.CountById(Item));
            Assert.Equal(stockBefore, ctx.Market.GetStockLine(Syndicate, Entry)!.quantity);
        }

        [Fact]
        public void Buy_RejectingSecondLeg_RollsBackStagedStockAndWallet()
        {
            var ctx = Create();
            int stockBefore = ctx.Market.GetStockLine(Syndicate, Entry)!.quantity;
            long walletBefore = ctx.Wallet.Value;

            var quote = ctx.Market.Buy(Syndicate, Entry, 1, day: 2,
                _ => ctx.Wallet.TryDebitValue(10, () => false));

            Assert.False(quote.Valid);
            Assert.Equal("settlement_failed", quote.RejectReason);
            Assert.Equal(stockBefore, ctx.Market.GetStockLine(Syndicate, Entry)!.quantity);
            Assert.Equal(walletBefore, ctx.Wallet.Value);
        }

        [Fact]
        public void Sell_SettlesInventoryWalletAndStock_UsingFloor()
        {
            var ctx = Create();
            Assert.True(ctx.Inventory.Add(ctx.Items.Get(Item)!, 2));
            long walletBefore = ctx.Wallet.Value;
            int stockBefore = ctx.Market.GetStockLine(Syndicate, Entry)!.quantity;

            var result = ctx.Settlement.Sell(Syndicate, Entry, 2, day: 2);

            Assert.True(result.Success, result.Message);
            Assert.Equal((long)Math.Floor(ctx.Market.GetSellPrice(
                Syndicate, ctx.Market.Catalog.FindEntry(Entry)!) * 2f), result.SettlementUnits);
            Assert.Equal(walletBefore + result.SettlementUnits, ctx.Wallet.Value);
            Assert.Equal(0, ctx.Inventory.CountById(Item));
            Assert.Equal(stockBefore + 2, ctx.Market.GetStockLine(Syndicate, Entry)!.quantity);
        }

        [Fact]
        public void Sell_MissingGoods_LeavesEveryOwnerUnchanged()
        {
            var ctx = Create();
            int stockBefore = ctx.Market.GetStockLine(Syndicate, Entry)!.quantity;

            var result = ctx.Settlement.Sell(Syndicate, Entry, 1, day: 2);

            Assert.False(result.Success);
            Assert.Equal("insufficient_inventory", result.ReasonId);
            Assert.Equal(10_000, ctx.Wallet.Value);
            Assert.Equal(stockBefore, ctx.Market.GetStockLine(Syndicate, Entry)!.quantity);
        }

        [Fact]
        public void Sell_RejectingSecondLeg_RollsBackStagedStock()
        {
            var ctx = Create();
            int stockBefore = ctx.Market.GetStockLine(Syndicate, Entry)!.quantity;
            var quote = ctx.Market.Sell(Syndicate, Entry, 1, day: 2, _ => false);

            Assert.False(quote.Valid);
            Assert.Equal("settlement_failed", quote.RejectReason);
            Assert.Equal(stockBefore, ctx.Market.GetStockLine(Syndicate, Entry)!.quantity);
        }

        [Fact]
        public void Loan_CreditsCanonicalWallet_AndKeepsExistingTrustPolicy()
        {
            var ctx = Create(startingValue: 100);
            float trustBefore = ctx.Market.FindLedger(Syndicate)!.trust;

            var result = ctx.Settlement.TakeLoan(Syndicate, 200, day: 3, durationDays: 7);

            Assert.True(result.Success, result.Message);
            Assert.Equal(300, ctx.Wallet.Value);
            var debt = Assert.Single(ctx.Market.State.debts);
            Assert.Equal(200f, debt.principalUnits);
            Assert.Equal(10, debt.dueDay);
            Assert.Equal(trustBefore + 10f, ctx.Market.FindLedger(Syndicate)!.trust);
        }

        [Fact]
        public void Loan_SecondAttemptRejected_WithoutWalletCredit()
        {
            var ctx = Create(startingValue: 100);
            Assert.True(ctx.Settlement.TakeLoan(Syndicate, 100, day: 3, durationDays: 7).Success);
            long walletAfterFirst = ctx.Wallet.Value;

            var second = ctx.Settlement.TakeLoan(Syndicate, 100, day: 3, durationDays: 7);

            Assert.False(second.Success);
            Assert.Equal("active_loan_exists", second.ReasonId);
            Assert.Equal(walletAfterFirst, ctx.Wallet.Value);
            Assert.Single(ctx.Market.State.debts);
        }

        [Fact]
        public void Loan_RejectingSettlement_RollsBackDebtAndTrust()
        {
            var ctx = Create();
            float trustBefore = ctx.Market.FindLedger(Syndicate)!.trust;

            var debt = ctx.Market.TakeLoan(Syndicate, 100, day: 3, durationDays: 7, _ => false);

            Assert.Null(debt);
            Assert.Empty(ctx.Market.State.debts);
            Assert.Equal(trustBefore, ctx.Market.FindLedger(Syndicate)!.trust);
        }

        [Fact]
        public void Repay_DebitsCanonicalWallet_AndCompletesDebt()
        {
            var ctx = Create(startingValue: 100);
            var loan = ctx.Settlement.TakeLoan(Syndicate, 200, day: 3, durationDays: 7);
            Assert.True(loan.Success);
            long walletBefore = ctx.Wallet.Value;

            var result = ctx.Settlement.Repay(loan.DebtId, 200, day: 4);

            Assert.True(result.Success, result.Message);
            Assert.Equal(walletBefore - 200, ctx.Wallet.Value);
            Assert.Equal(UnderworldDebtRecord.StatusRepaid, ctx.Market.State.debts.Single().status);
        }

        [Fact]
        public void Repay_InsufficientFunds_LeavesDebtAndWalletUnchanged()
        {
            var ctx = Create(startingValue: 0);
            var debt = ctx.Market.TakeLoan(Syndicate, 100, day: 3, durationDays: 7);
            Assert.NotNull(debt);
            Assert.True(ctx.Wallet.TryDebitValue(ctx.Wallet.Value));
            float repaidBefore = debt!.repaidUnits;

            var result = ctx.Settlement.Repay(debt.debtId, 100, day: 4);

            Assert.False(result.Success);
            Assert.Equal("insufficient_funds", result.ReasonId);
            Assert.Equal(0, ctx.Wallet.Value);
            Assert.Equal(repaidBefore, debt.repaidUnits);
            Assert.Equal(UnderworldDebtRecord.StatusActive, debt.status);
        }

        [Fact]
        public void Repay_RejectingSettlement_RollsBackDebtAndTrust()
        {
            var ctx = Create();
            var debt = ctx.Market.TakeLoan(Syndicate, 100, day: 3, durationDays: 7);
            Assert.NotNull(debt);
            float trustBefore = ctx.Market.FindLedger(Syndicate)!.trust;

            bool repaid = ctx.Market.RepayDebt(debt!.debtId, 100, day: 4, _ => false);

            Assert.False(repaid);
            Assert.Equal(0f, debt.repaidUnits);
            Assert.Equal(UnderworldDebtRecord.StatusActive, debt.status);
            Assert.Equal(trustBefore, ctx.Market.FindLedger(Syndicate)!.trust);
        }

        [Fact]
        public void SettledBuy_RoundTripsThroughExistingThreeSaveOwners()
        {
            var ctx = Create();
            var bought = ctx.Settlement.Buy(Syndicate, Entry, 2, day: 2);
            Assert.True(bought.Success);
            var marketSave = ctx.Market.CaptureState();
            var walletSave = ctx.Wallet.CaptureState();
            var inventorySave = ctx.Inventory.CaptureState();

            var restored = Create();
            restored.Inventory.RestoreState(inventorySave, id => restored.Items.Get(id));
            Assert.True(restored.Wallet.TryRestoreState(walletSave, out string walletError), walletError);
            restored.Market.RestoreState(marketSave);

            Assert.Equal(ctx.Wallet.Value, restored.Wallet.Value);
            Assert.Equal(ctx.Inventory.CountById(Item), restored.Inventory.CountById(Item));
            Assert.Equal(ctx.Market.GetStockLine(Syndicate, Entry)!.quantity,
                restored.Market.GetStockLine(Syndicate, Entry)!.quantity);
        }

        [Fact]
        public void LoanOverdue_RemainsExactlyOnceAcrossSaveRestore()
        {
            var ctx = Create(startingValue: 100);
            var loan = ctx.Settlement.TakeLoan(Syndicate, 100, day: 3, durationDays: 2);
            Assert.True(loan.Success);

            var restored = Create(startingValue: 0);
            restored.Market.RestoreState(ctx.Market.CaptureState());
            Assert.True(restored.Wallet.TryRestoreState(ctx.Wallet.CaptureState(), out string error), error);
            int overdueEvents = 0;
            restored.Market.OnDebtOverdue += _ => overdueEvents++;
            restored.Market.TickDaily(5);
            Assert.Equal(1, overdueEvents);

            var restoredAgain = Create(startingValue: 0);
            restoredAgain.Market.RestoreState(restored.Market.CaptureState());
            int replayedEvents = 0;
            restoredAgain.Market.OnDebtOverdue += _ => replayedEvents++;
            restoredAgain.Market.TickDaily(5);
            restoredAgain.Market.TickDaily(6);

            Assert.Equal(0, replayedEvents);
            Assert.Equal(UnderworldDebtRecord.StatusDefaulted,
                restoredAgain.Market.State.debts.Single().status);
        }
    }
}
