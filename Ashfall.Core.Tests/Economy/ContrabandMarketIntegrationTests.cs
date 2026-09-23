// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class ContrabandMarketIntegrationTests
    {
        private static (ShelterBarterSystem barter, InventoryContainer inv) CreateTestSetup()
        {
            var inv = new InventoryContainer();
            var barter = new ShelterBarterSystem(new SeededRng(777), inv);

            // Add standard inventory stock for player using TryProduce
            inv.TryProduce("item_blowtorch", 1);
            inv.TryProduce("item_contraband_narcotic", 5);
            inv.TryProduce("item_scrap_metal", 20);

            return (barter, inv);
        }

        [Fact]
        public void Standard_merchant_refuses_contraband_goods()
        {
            var (barter, inv) = CreateTestSetup();

            barter.ContrabandCheck = id => id.StartsWith("item_contraband_", StringComparison.OrdinalIgnoreCase);

            // Force arrival of caravan_scrap_salvagers (honest caravan)
            var caravan = barter.Catalog["caravan_scrap_salvagers"];
            var cState = barter.State.caravans[caravan.caravan_id];
            cState.isAtAirlock = true;
            cState.remainingStock["item_fuel"] = 10;

            var offers = new Dictionary<string, int> { { "item_contraband_narcotic", 2 } };
            var requests = new Dictionary<string, int> { { "item_fuel", 2 } };

            var result = barter.ExecuteTrade("caravan_scrap_salvagers", offers, requests);
            Assert.False(result.IsSuccess);
            Assert.Equal("contraband_refused", result.FailureCode);
        }

        [Fact]
        public void Black_market_smuggler_accepts_contraband_and_dispatches_event()
        {
            var (barter, inv) = CreateTestSetup();

            barter.ContrabandCheck = id => id.StartsWith("item_contraband_", StringComparison.OrdinalIgnoreCase);

            // Force airlock arrival for caravan_black_market_munitions
            var caravan = barter.Catalog["caravan_black_market_munitions"];
            var cState = barter.State.caravans[caravan.caravan_id];
            cState.isAtAirlock = true;
            cState.remainingStock["item_ammo_9mm"] = 100;

            string? tradedCaravan = null;
            string? tradedItem = null;
            int tradedQty = 0;
            barter.OnContrabandTraded += (cId, iId, qty) =>
            {
                tradedCaravan = cId;
                tradedItem = iId;
                tradedQty = qty;
            };

            // Player offers blowtorch (legal 45 value) + contraband
            var offers = new Dictionary<string, int>
            {
                { "item_blowtorch", 1 },
                { "item_contraband_narcotic", 1 }
            };
            var requests = new Dictionary<string, int> { { "item_ammo_9mm", 5 } };

            var result = barter.ExecuteTrade("caravan_black_market_munitions", offers, requests);
            Assert.True(result.IsSuccess);
            Assert.Equal("caravan_black_market_munitions", tradedCaravan);
            Assert.Equal("item_contraband_narcotic", tradedItem);
            Assert.Equal(1, tradedQty);
        }

        [Fact]
        public void BlackMarketContrabandEngine_CalculatesFencingFees_BasedOnTrust()
        {
            // Zero trust -> 35% fee (350 permille)
            int zeroTrustFee = BlackMarketContrabandEngine.CalculateFencingFeePermille(0);
            Assert.Equal(350, zeroTrustFee);

            // Max trust (1000 permille) -> 20% fee (200 permille)
            int maxTrustFee = BlackMarketContrabandEngine.CalculateFencingFeePermille(1000);
            Assert.Equal(200, maxTrustFee);

            // Mid trust (500 permille) -> 275 permille
            int midTrustFee = BlackMarketContrabandEngine.CalculateFencingFeePermille(500);
            Assert.Equal(275, midTrustFee);
        }

        [Fact]
        public void BlackMarketContrabandEngine_RejectsTrade_WhenRaidLockoutHeatActive()
        {
            // Heat at 850 >= RaidLockoutHeatPermille (800) -> buy rejected
            var quote = BlackMarketContrabandEngine.QuoteBuy(
                "item_contraband_military_chip",
                quantity: 1,
                baseUnitValueChits: 100,
                classification: ContrabandClassification.MilitaryHardware,
                currentHeatPermille: 850);

            Assert.False(quote.IsViable);
            Assert.Contains("locked down", quote.RejectionReason, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void BlackMarketContrabandEngine_GeneratesHeat_OnViableTrade()
        {
            var quote = BlackMarketContrabandEngine.QuoteBuy(
                "item_contraband_military_chip",
                quantity: 2,
                baseUnitValueChits: 50,
                classification: ContrabandClassification.MilitaryHardware,
                currentHeatPermille: 100);

            Assert.True(quote.IsViable);
            Assert.True(quote.GeneratedHeatPermille > 0);
            Assert.Equal(100 + quote.GeneratedHeatPermille, quote.ProjectedHeatPermille);
        }
    }
}
