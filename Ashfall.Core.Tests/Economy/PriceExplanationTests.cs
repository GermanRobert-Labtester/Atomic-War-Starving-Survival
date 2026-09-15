// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class PriceExplanationTests
    {
        private static MarketSystem CreateMarket()
        {
            var load = new GoodsCatalogLoadResult();
            load.Goods.Add(new GoodDefinition
            {
                id = "canned_food",
                displayName = "Canned Food",
                category = "food",
                basePrice = 10f,
                volatility = 0.2f,
                elasticity = 1f
            });
            var market = new MarketSystem();
            market.BindCatalog(GoodsCatalogLoader.ToCatalog(load));
            return market;
        }

        [Fact]
        public void Explanation_ReconcilesToAuthoritativeQuote_AndIsSideEffectFree()
        {
            var market = CreateMarket();
            market.AdjustDemand("canned_food", 0.5f);
            var before = market.CaptureState();

            var explanation = market.ExplainPrice("canned_food", MarketTransactionSide.Sell);

            Assert.Equal(market.GetPrice("canned_food"), explanation.finalPrice);
            Assert.Equal(15f, explanation.finalPrice, 5);
            Assert.Equal(MarketTransactionSide.Sell, explanation.side);
            Assert.Single(explanation.factors, f => f.kind == PriceFactorKind.Demand);
            Assert.Equal(5f, explanation.TotalFactorDelta, 5);
            Assert.Equal(before.demand.Count, market.State.demand.Count);
            Assert.Equal(before.demand[0].multiplier, market.State.demand[0].multiplier);
            Assert.Empty(market.State.ledger);
        }

        [Fact]
        public void Explanation_FactorOrder_IsStableAcrossRepeatedReads()
        {
            var market = CreateMarket();
            market.AdjustDemand("canned_food", -0.4f);

            var first = market.ExplainPrice("canned_food");
            var second = market.ExplainPrice("canned_food");

            Assert.Equal(first.finalPrice, second.finalPrice);
            Assert.Equal(first.factors.Count, second.factors.Count);
            for (int i = 0; i < first.factors.Count; i++)
            {
                Assert.Equal(first.factors[i].kind, second.factors[i].kind);
                Assert.Equal(first.factors[i].sourceId, second.factors[i].sourceId);
                Assert.Equal(first.factors[i].afterPrice, second.factors[i].afterPrice);
            }
        }

        [Fact]
        public void Explanation_UnknownGood_IsExplicitlyInvalid()
        {
            var explanation = CreateMarket().ExplainPrice("unknown_good");

            Assert.Equal("unknown_good", explanation.itemId);
            Assert.True(float.IsNaN(explanation.finalPrice));
            Assert.Empty(explanation.factors);
        }
    }
}
