// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public class BlackMarketContrabandEngineTests
    {
        [Fact]
        public void CalculateFencingFeePermille_TrustScaling_BoundsBetween200And350()
        {
            // Zero trust -> max fee 350 permille (35%)
            int zeroTrustFee = BlackMarketContrabandEngine.CalculateFencingFeePermille(0);
            Assert.Equal(350, zeroTrustFee);

            // 500 trust -> 350 - 75 = 275 permille (27.5%)
            int midTrustFee = BlackMarketContrabandEngine.CalculateFencingFeePermille(500);
            Assert.Equal(275, midTrustFee);

            // 1000 max trust -> min fee 200 permille (20%)
            int maxTrustFee = BlackMarketContrabandEngine.CalculateFencingFeePermille(1000);
            Assert.Equal(200, maxTrustFee);
        }

        [Fact]
        public void QuoteBuy_ValidItem_GeneratesDebitAndHeat()
        {
            var quote = BlackMarketContrabandEngine.QuoteBuy(
                itemId: "item_military_ammo",
                quantity: 5,
                baseUnitValueChits: 10,
                classification: ContrabandClassification.MilitaryHardware, // 2.0x multiplier
                currentHeatPermille: 100
            );

            Assert.True(quote.IsViable);
            Assert.Equal(BlackMarketActionType.Buy, quote.Action);
            Assert.Equal(20, quote.UnitPriceChits); // 10 * 2.0 = 20
            Assert.Equal(100, quote.GrossValueChits); // 20 * 5 = 100
            Assert.Equal(-100, quote.NetFundsDelta); // Debit 100
            Assert.True(quote.GeneratedHeatPermille > 0);
            Assert.Equal(quote.ProjectedHeatPermille, 100 + quote.GeneratedHeatPermille);
            Assert.Equal(FundsLedger.ReasonBmBuy, quote.ReasonKey);
        }

        [Fact]
        public void QuoteSell_WholesaleMarkdown_GeneratesCredit()
        {
            var quote = BlackMarketContrabandEngine.QuoteSell(
                itemId: "item_narcotics_vial",
                quantity: 4,
                baseUnitValueChits: 20,
                classification: ContrabandClassification.Narcotics, // 1.6x multiplier
                currentHeatPermille: 50
            );

            Assert.True(quote.IsViable);
            Assert.Equal(BlackMarketActionType.Sell, quote.Action);
            // Unit price: 20 * 1.6 * 0.75 = 24
            Assert.Equal(24, quote.UnitPriceChits);
            Assert.Equal(96, quote.GrossValueChits); // 24 * 4 = 96
            Assert.Equal(96, quote.NetFundsDelta); // Credit 96
            Assert.Equal(FundsLedger.ReasonBmSell, quote.ReasonKey);
        }

        [Fact]
        public void QuoteFence_AppliesFeeAndPayout_GeneratesHigherHeat()
        {
            var quote = BlackMarketContrabandEngine.QuoteFence(
                itemId: "item_stolen_weapon",
                quantity: 2,
                baseUnitValueChits: 50,
                classification: ContrabandClassification.BannedWeaponry, // 1.5x multiplier -> unit 75
                currentHeatPermille: 200,
                fenceTrustPermille: 1000 // 200 fee permille (20%)
            );

            Assert.True(quote.IsViable);
            Assert.Equal(BlackMarketActionType.Fence, quote.Action);
            Assert.Equal(75, quote.UnitPriceChits);
            Assert.Equal(150, quote.GrossValueChits); // 75 * 2 = 150
            Assert.Equal(30, quote.FencingFeeChits); // 150 * 20% = 30
            Assert.Equal(120, quote.NetFundsDelta); // 150 - 30 = 120
            Assert.Equal(FundsLedger.ReasonBmFence, quote.ReasonKey);
            Assert.Contains("Fence 2x item_stolen_weapon", quote.Summary);
        }

        [Fact]
        public void QuoteOperations_HeatAboveRaidLockout_ReturnsRejectedQuote()
        {
            var quote = BlackMarketContrabandEngine.QuoteBuy(
                itemId: "item_supplies",
                quantity: 1,
                baseUnitValueChits: 10,
                classification: ContrabandClassification.Unregulated,
                currentHeatPermille: 850 // Above 800 lockout threshold
            );

            Assert.False(quote.IsViable);
            Assert.Contains("locked down", quote.RejectionReason);
        }

        [Fact]
        public void ExecuteTransaction_ValidBuy_DebitsLedgerAndIncrementsHeat()
        {
            var ledger = new FundsLedger(200);
            var quote = BlackMarketContrabandEngine.QuoteBuy(
                itemId: "item_supplies",
                quantity: 2,
                baseUnitValueChits: 10,
                classification: ContrabandClassification.Unregulated,
                currentHeatPermille: 100
            );

            int heat = 100;
            var result = BlackMarketContrabandEngine.ExecuteTransaction(
                ledger,
                quote,
                counterpartyId: "fence_kane",
                day: 15,
                currentHeatPermille: ref heat
            );

            Assert.True(result.Success);
            Assert.Equal(180, ledger.Balance); // 200 - 20 = 180
            Assert.True(heat > 100);
        }

        [Fact]
        public void ExecuteTransaction_InsufficientFunds_FailsWithoutAlteringHeat()
        {
            var ledger = new FundsLedger(10);
            var quote = BlackMarketContrabandEngine.QuoteBuy(
                itemId: "item_weapon",
                quantity: 1,
                baseUnitValueChits: 50,
                classification: ContrabandClassification.Unregulated,
                currentHeatPermille: 100
            );

            int heat = 100;
            var result = BlackMarketContrabandEngine.ExecuteTransaction(
                ledger,
                quote,
                counterpartyId: "fence_kane",
                day: 15,
                currentHeatPermille: ref heat
            );

            Assert.False(result.Success);
            Assert.Equal("InsufficientFunds", result.FailureReason);
            Assert.Equal(10, ledger.Balance);
            Assert.Equal(100, heat); // Unchanged
        }
    }
}
