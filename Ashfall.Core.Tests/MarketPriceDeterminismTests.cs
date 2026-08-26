using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Economy;

namespace Ashfall.Core.Tests
{
    public class MarketPriceDeterminismTests
    {
        private static MarketSystem CreateSystemWithGoods()
        {
            var sys = new MarketSystem();
            var result = new GoodsCatalogLoadResult();
            result.Goods.Add(new GoodDefinition { id = "canned_food", displayName = "Canned Food", category = "food", basePrice = 10f, volatility = 0.5f, elasticity = 1f });
            result.Goods.Add(new GoodDefinition { id = "clean_water", displayName = "Clean Water", category = "water", basePrice = 8f, volatility = 0.5f, elasticity = 1f });
            result.Goods.Add(new GoodDefinition { id = "fuel_canister", displayName = "Fuel", category = "fuel", basePrice = 15f, volatility = 0.5f, elasticity = 1f });
            sys.BindCatalog(GoodsCatalogLoader.ToCatalog(result));
            return sys;
        }

        [Fact]
        public void SameSeed_Determinism_PriceTrajectoryIdentical()
        {
            var seed = 42;
            var a = CreateSystemWithGoods();
            var b = CreateSystemWithGoods();
            var rngA = new SeededRng(seed);
            var rngB = new SeededRng(seed);
            var pricesA = new List<float>();
            var pricesB = new List<float>();
            for (int day = 1; day <= 14; day++)
            {
                a.TickDay(day, rngA);
                b.TickDay(day, rngB);
                pricesA.Add(a.GetPrice("canned_food"));
                pricesB.Add(b.GetPrice("canned_food"));
            }
            Assert.Equal(pricesA.Count, pricesB.Count);
            for (int i = 0; i < pricesA.Count; i++)
                Assert.Equal(pricesA[i], pricesB[i]);
        }

        [Fact]
        public void DifferentSeed_Divergence_Allowed_ButWithinBounds()
        {
            var a = CreateSystemWithGoods();
            var b = CreateSystemWithGoods();
            var rngA = new SeededRng(42);
            var rngB = new SeededRng(999);
            for (int day = 1; day <= 30; day++)
            {
                a.TickDay(day, rngA);
                b.TickDay(day, rngB);
            }
            float priceA = a.GetPrice("canned_food");
            float priceB = b.GetPrice("canned_food");
            // Both must be valid and within [0.25*base, 4*base] = [2.5, 40] for base 10
            Assert.True(priceA >= 2.5f && priceA <= 40f, $"priceA {priceA} out of bounds");
            Assert.True(priceB >= 2.5f && priceB <= 40f, $"priceB {priceB} out of bounds");
            // Divergence is allowed, not required to be different, but both valid
            Assert.False(float.IsNaN(priceA));
            Assert.False(float.IsInfinity(priceA));
        }

        [Theory]
        [InlineData(42, 7)]
        [InlineData(123, 14)]
        [InlineData(999, 30)]
        public void NumericalSafety_NoNaNOrInfinityOrNegative(int seed, int days)
        {
            var sys = CreateSystemWithGoods();
            var rng = new SeededRng(seed);
            for (int day = 1; day <= days; day++)
            {
                sys.TickDay(day, rng);
                foreach (var id in new[] { "canned_food", "clean_water", "fuel_canister" })
                {
                    float price = sys.GetPrice(id);
                    Assert.False(float.IsNaN(price), $"NaN for {id} day {day} seed {seed}");
                    Assert.False(float.IsInfinity(price), $"Infinity for {id} day {day} seed {seed}");
                    Assert.True(price >= 0f, $"Negative price {price} for {id}");
                    // Price floor/ceiling: base 10 => [2.5,40], base 8 => [2,32], base 15 => [3.75,60]
                    // Check against system constants
                    float basePrice = id == "canned_food" ? 10f : id == "clean_water" ? 8f : 15f;
                    Assert.True(price >= basePrice * MarketSystem.PriceFloorFraction - 1e-5f, $"Price {price} below floor for {id}");
                    Assert.True(price <= basePrice * MarketSystem.PriceCeilingFraction + 1e-5f, $"Price {price} above ceiling 4x for {id}");
                }
            }
        }

        [Fact]
        public void PriceExplosion_BoundedByCeiling()
        {
            var sys = CreateSystemWithGoods();
            var rng = new SeededRng(1);
            // Force many ticks with high demand
            for (int day = 1; day <= 100; day++)
            {
                sys.TickDay(day, rng);
                // Artificially push demand high
                sys.AdjustDemand("canned_food", 1f);
            }
            float price = sys.GetPrice("canned_food");
            Assert.True(price <= 10f * MarketSystem.PriceCeilingFraction + 1e-5f, $"Price explosion beyond 4x ceiling: {price}");
            Assert.True(price >= 10f * MarketSystem.PriceFloorFraction - 1e-5f);
        }

        [Fact]
        public void AllGoods_Determinism_And_Bounds()
        {
            var sysA = CreateSystemWithGoods();
            var sysB = CreateSystemWithGoods();
            var rngA = new SeededRng(123);
            var rngB = new SeededRng(123);
            for (int day = 1; day <= 30; day++)
            {
                sysA.TickDay(day, rngA);
                sysB.TickDay(day, rngB);
                foreach (var id in new[] { "canned_food", "clean_water", "fuel_canister" })
                {
                    Assert.Equal(sysA.GetPrice(id), sysB.GetPrice(id));
                    float price = sysA.GetPrice(id);
                    Assert.False(float.IsNaN(price));
                    Assert.False(float.IsInfinity(price));
                    Assert.True(price > 0);
                }
            }
        }

        [Fact]
        public void FuzzCatalog_AllGoods_Determinism_And_Bounds()
        {
            GoodsCatalog CreateFuzz()
            {
                var result = new GoodsCatalogLoadResult();
                string[] cats = { "food", "water", "fuel", "medical" };
                for (int i = 0; i < 12; i++)
                    result.Goods.Add(new GoodDefinition { id = "g" + i, displayName = "G" + i, category = cats[i % cats.Length], basePrice = 5f + i, volatility = 0.5f, elasticity = 1f });
                return GoodsCatalogLoader.ToCatalog(result);
            }
            var sysA = new MarketSystem(); sysA.BindCatalog(CreateFuzz());
            var sysB = new MarketSystem(); sysB.BindCatalog(CreateFuzz());
            var rngA = new SeededRng(99);
            var rngB = new SeededRng(99);
            for (int day = 1; day <= 30; day++)
            {
                sysA.TickDay(day, rngA);
                sysB.TickDay(day, rngB);
                for (int i = 0; i < 12; i++)
                {
                    string id = "g" + i;
                    float pA = sysA.GetPrice(id);
                    float pB = sysB.GetPrice(id);
                    Assert.Equal(pA, pB);
                    Assert.False(float.IsNaN(pA));
                    Assert.False(float.IsInfinity(pA));
                    Assert.True(pA > 0);
                    // Bounds 0.25*base to 4*base
                    float basePrice = 5f + i;
                    Assert.True(pA >= basePrice * MarketSystem.PriceFloorFraction - 1e-5f);
                    Assert.True(pA <= basePrice * MarketSystem.PriceCeilingFraction + 1e-5f);
                }
            }
        }
    }
}
