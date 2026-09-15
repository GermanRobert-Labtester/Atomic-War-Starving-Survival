// SPDX-License-Identifier: MIT
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class RegionalSupplyPriceIntegrationTests
    {
        [Fact]
        public void RegionalSupplyChangesQuoteWhenAtlasHasNoOverride()
        {
            var load = new GoodsCatalogLoadResult();
            load.Goods.Add(new GoodDefinition
            {
                id = "foundry_scrap",
                category = "materials",
                basePrice = 100f,
                regionalSupply = "foundry"
            });
            var market = new MarketSystem();
            market.BindCatalog(GoodsCatalogLoader.ToCatalog(load));
            market.BindRegionalPriceAtlas(
                new RegionalPriceAtlas(new RegionalPriceCatalog()),
                "foundry");

            var explanation = market.ExplainPrice("foundry_scrap");

            Assert.Equal(50f, explanation.finalPrice, 5);
            var factor = Assert.Single(
                explanation.factors,
                record => record.kind == PriceFactorKind.RegionalSupply);
            Assert.Equal(0.5f, factor.multiplier, 5);
            Assert.Equal("regional_supply:foundry", factor.sourceId);
        }

        [Fact]
        public void ExplicitAtlasOverrideRetainsPrecedenceOverRegionalSupply()
        {
            var load = new GoodsCatalogLoadResult();
            load.Goods.Add(new GoodDefinition
            {
                id = "foundry_scrap",
                category = "materials",
                basePrice = 100f,
                regionalSupply = "foundry"
            });
            var regionalCatalog = new RegionalPriceCatalog();
            regionalCatalog.Add(new RegionalPriceEntry(
                "foundry", "foundry_scrap", string.Empty, 1200, "balanced"));
            var market = new MarketSystem();
            market.BindCatalog(GoodsCatalogLoader.ToCatalog(load));
            market.BindRegionalPriceAtlas(new RegionalPriceAtlas(regionalCatalog), "foundry");

            var explanation = market.ExplainPrice("foundry_scrap");

            Assert.Equal(120f, explanation.finalPrice, 4);
            Assert.Contains(explanation.factors, record => record.kind == PriceFactorKind.Regional);
            Assert.DoesNotContain(explanation.factors, record => record.kind == PriceFactorKind.RegionalSupply);
        }
    }
}
