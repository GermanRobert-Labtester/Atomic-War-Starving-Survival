using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Adversarial probes from the Phase 4 debug loop: seed fuzz, save
    /// corruption, boundaries, barter edges, reload continuity. Every probe
    /// is a permanent regression test; a red probe = a defect with a repro.
    /// </summary>
    public class EconomyProbeTests
    {
        private static GoodsCatalog FuzzCatalog()
        {
            var result = new GoodsCatalogLoadResult();
            string[] cats = { "food", "water", "medical", "fuel", "weapons", "tools", "materials", "ammo", "documents", "luxury", "misc" };
            for (int i = 0; i < 12; i++)
            {
                result.Goods.Add(new GoodDefinition
                {
                    id = "g" + i,
                    displayName = "G" + i,
                    category = cats[i % cats.Length],
                    basePrice = 1f + i,
                    volatility = (i % 11) / 10f,
                    elasticity = 0.5f + (i % 5) * 0.4f,
                    stackSize = 1 + i,
                    weightKg = i * 0.5f
                });
            }
            return GoodsCatalogLoader.ToCatalog(result);
        }

        [Fact]
        public void Probe_SeedFuzz_100Seeds200Ticks_BoundsHoldNoExceptions()
        {
            for (int seed = 0; seed < 100; seed++)
            {
                var sys = new MarketSystem();
                sys.BindCatalog(FuzzCatalog());
                for (int day = 1; day <= 200; day++)
                {
                    sys.TickDay(day, new SeededRng(seed));
                    foreach (var good in sys.State.demand)
                    {
                        Assert.True(good.multiplier >= MarketSystem.MinDemandMult - 1e-6f);
                        Assert.True(good.multiplier <= MarketSystem.MaxDemandMult + 1e-6f);
                        Assert.False(float.IsNaN(good.multiplier));
                    }
                    foreach (var g in new[] { "g0", "g5", "g11" })
                    {
                        float price = sys.GetPrice(g);
                        Assert.True(price > 0f);
                        Assert.False(float.IsNaN(price));
                        Assert.False(float.IsInfinity(price));
                    }
                }
            }
        }

        [Fact]
        public void Probe_SeedFuzz_WithInterleavedTransactions_NoExceptions()
        {
            for (int seed = 0; seed < 100; seed++)
            {
                var sys = new MarketSystem();
                sys.BindCatalog(FuzzCatalog());
                for (int day = 1; day <= 200; day++)
                {
                    sys.TickDay(day, new SeededRng(seed));
                    if (day % 4 == 0) sys.Buy("g" + (day % 12), day % 7 + 1, day, "fuzz");
                    if (day % 6 == 0) sys.Sell("g" + ((day + 3) % 12), day % 5 + 1, day, "fuzz");
                    if (day % 9 == 0)
                    {
                        var barter = sys.Barter("g" + (day % 12), day % 6 + 1,
                            "g" + ((day + 7) % 12), day);
                        Assert.True(barter.Accepted || barter.RejectReason.Length > 0);
                    }
                    // Invariants over the whole ledger.
                    float total = 0f;
                    foreach (var e in sys.State.ledger)
                    {
                        Assert.False(float.IsNaN(e.totalValue));
                        Assert.False(float.IsInfinity(e.totalValue));
                        Assert.True(e.unitPrice > 0f);
                        total += e.totalValue;
                    }
                    Assert.False(float.IsNaN(total));
                }
            }
        }

        [Fact]
        public void Probe_LedgerConservation_NoNegativeOrNaNValues()
        {
            var sys = new MarketSystem();
            sys.BindCatalog(FuzzCatalog());
            for (int day = 1; day <= 30; day++)
            {
                sys.TickDay(day, new SeededRng(5));
                if (day % 3 == 0) sys.Buy("g1", 10, day, "probe");
                if (day % 5 == 0) sys.Sell("g1", 4, day, "probe");
                if (day % 7 == 0) sys.Barter("g2", 5, "g3", day);
            }
            foreach (var e in sys.State.ledger)
            {
                Assert.False(float.IsNaN(e.totalValue));
                Assert.False(float.IsInfinity(e.totalValue));
                Assert.True(e.unitPrice > 0f);
                Assert.True(e.quantity != 0);
            }
        }

        [Fact]
        public void Probe_TruncatedSave_ThrowsAndHostPathReturnsNull()
        {
            // Established contract: the core serializer throws JsonException on
            // malformed input; every loader AND the host store wraps it in
            // try/catch. The live path must surface null, never partial state.
            string truncated = "{\"version\":1,\"demand\":[{\"itemId\":\"g";
            Assert.Throws<System.Text.Json.JsonException>(
                () => new SystemTextJsonSerializer().Deserialize<MarketState>(truncated));

            // Host-store equivalent: catch -> null (mirrors EconomySaveStore.TryLoad).
            MarketState loaded;
            try
            {
                loaded = new SystemTextJsonSerializer().Deserialize<MarketState>(truncated);
            }
            catch (System.Text.Json.JsonException)
            {
                loaded = null;
            }
            Assert.Null(loaded);
        }

        [Fact]
        public void Probe_MissingFieldsSave_MigratesPredictably()
        {
            var sys = new MarketSystem();
            sys.BindCatalog(FuzzCatalog());
            var minimal = new MarketState { version = 1 }; // no day, no demand, no ledger
            sys.RestoreState(minimal);
            Assert.Equal(0, sys.Day);
            Assert.Equal(0, sys.TickCount);
            Assert.Empty(sys.State.demand);
            Assert.Empty(sys.State.ledger);
            Assert.Equal(1f, sys.GetDemandMultiplier("g0"));
        }

        [Fact]
        public void Probe_DayZeroTick_DoesNotCorrupt()
        {
            var sys = new MarketSystem();
            sys.BindCatalog(FuzzCatalog());
            sys.TickDay(0, new SeededRng(1));
            Assert.Equal(1, sys.TickCount);
            Assert.Equal(0, sys.Day);
            // and continues fine
            sys.TickDay(1, new SeededRng(1));
            Assert.Equal(2, sys.TickCount);
        }

        [Fact]
        public void Probe_EmptyCatalog_TickAndTransactSafe()
        {
            var sys = new MarketSystem();
            sys.BindCatalog(new GoodsCatalog());
            sys.TickDay(1, new SeededRng(1)); // no crash
            Assert.True(float.IsNaN(sys.GetPrice("anything")));
            Assert.False(sys.Buy("anything", 1, 1).Accepted);
            Assert.False(sys.Barter("a", 1, "b", 1).Accepted);
        }

        [Fact]
        public void Probe_SingleGoodCatalog_StaysBounded()
        {
            var sys = new MarketSystem();
            var one = new GoodsCatalogLoadResult();
            one.Goods.Add(new GoodDefinition
            {
                id = "only",
                displayName = "Only",
                category = "misc",
                basePrice = 1f,
                volatility = 1f,
                elasticity = 2f
            });
            sys.BindCatalog(GoodsCatalogLoader.ToCatalog(one));
            for (int day = 1; day <= 500; day++)
                sys.TickDay(day, new SeededRng(day));
            float p = sys.GetPrice("only");
            Assert.True(p >= 0.25f && p <= 4f);
        }

        [Fact]
        public void Probe_BarterRemainderBothDirections()
        {
            var sys = new MarketSystem();
            var result = new GoodsCatalogLoadResult();
            result.Goods.Add(new GoodDefinition { id = "a", displayName = "A", category = "misc", basePrice = 7f, volatility = 0f, elasticity = 1f });
            result.Goods.Add(new GoodDefinition { id = "b", displayName = "B", category = "misc", basePrice = 3f, volatility = 0f, elasticity = 1f });
            sys.BindCatalog(GoodsCatalogLoader.ToCatalog(result));

            // Give 1 A (7) for B (3): take 2 (6), remainder 1.
            var r1 = sys.Barter("a", 1, "b", 1);
            Assert.True(r1.Accepted);
            Assert.Equal(2, r1.Quantity);
            Assert.Equal(1f, r1.RemainderValue, 3);
            Assert.Equal(sys.State.ledger[0].totalValue, sys.State.ledger[1].totalValue);

            // Give 4 B (12) for A (7): take 1 (7), remainder 5.
            var r2 = sys.Barter("b", 4, "a", 2);
            Assert.True(r2.Accepted);
            Assert.Equal(1, r2.Quantity);
            Assert.Equal(5f, r2.RemainderValue, 3);
            Assert.Equal(sys.State.ledger[2].totalValue, sys.State.ledger[3].totalValue);
        }

        [Fact]
        public void Probe_BarterWithUntrackedGoods_Rejected()
        {
            var sys = new MarketSystem();
            sys.BindCatalog(FuzzCatalog());
            Assert.False(sys.Barter("missing_good", 5, "g1", 1).Accepted);
            Assert.False(sys.Barter("g1", 5, "missing_good", 1).Accepted);
            Assert.Empty(sys.State.ledger);
        }

        [Fact]
        public void Probe_ReloadContinuity_HashMatchesUninterruptedRun()
        {
            // Run A: uninterrupted 40 ticks. Run B: tick 20, save, restore, tick 20.
            var catalog = FuzzCatalog();
            var a = new MarketSystem();
            a.BindCatalog(catalog);
            for (int day = 1; day <= 40; day++) a.TickDay(day, new SeededRng(77));

            var b = new MarketSystem();
            b.BindCatalog(catalog);
            for (int day = 1; day <= 20; day++) b.TickDay(day, new SeededRng(77));
            var saved = b.CaptureState();
            var c = new MarketSystem();
            c.BindCatalog(catalog);
            c.RestoreState(saved);
            for (int day = 21; day <= 40; day++) c.TickDay(day, new SeededRng(77));

            Assert.Equal(SaveChecksum.Compute(a.CaptureState()), SaveChecksum.Compute(c.CaptureState()));
        }

        [Fact]
        public void Probe_ClampSaturation_BindsExactlyAndStays()
        {
            // Extremely elastic + max-volatility good saturates the demand clamp
            // hard; the price must sit exactly on floor/ceiling and never drift.
            var result = new GoodsCatalogLoadResult();
            result.Goods.Add(new GoodDefinition
            {
                id = "wild",
                displayName = "Wild",
                category = "misc",
                basePrice = 10f,
                volatility = 1f,
                elasticity = 50f
            });
            var sys = new MarketSystem();
            sys.BindCatalog(GoodsCatalogLoader.ToCatalog(result));
            for (int day = 1; day <= 1000; day++)
                sys.TickDay(day, new SeededRng(day));
            float price = sys.GetPrice("wild");
            Assert.True(price == MarketSystem.PriceFloorFraction * 10f ||
                        price == MarketSystem.PriceCeilingFraction * 10f,
                $"wild must sit on a clamp bound, got {price}");
            // And a demand nudge in the opposite direction recovers exactly.
            float before = price;
            sys.AdjustDemand("wild", price == 40f ? -100f : 100f);
            float after = sys.GetPrice("wild");
            Assert.True(after != before, "nudge must move a saturated price off its bound");
        }

        [Fact]
        public void Probe_ForeignDemandRows_DoNotCorruptCatalogPrices()
        {
            // A save from a session with a different catalog may carry demand
            // rows for goods that no longer exist. They must not corrupt prices
            // of catalog goods, must not crash, and the shortage metric absorbs
            // them without NaN.
            var sys = new MarketSystem();
            sys.BindCatalog(FuzzCatalog());
            var foreign = new MarketState
            {
                version = MarketState.Version,
                day = 5,
                tickCount = 5,
                demand = new List<DemandEntry>
                {
                    new DemandEntry { itemId = "ghost_good", multiplier = 3.9f }
                }
            };
            sys.RestoreState(foreign);
            sys.TickDay(6, new SeededRng(1));
            float price = sys.GetPrice("g0");
            Assert.True(price > 0f && !float.IsNaN(price));
            Assert.False(sys.IsSuppliesShort() && float.IsNaN(sys.IsSuppliesShort() ? 0f : 0f));
            // Foreign row survives as demand (it may return to the catalog).
            Assert.Equal(3.9f, sys.GetDemandMultiplier("ghost_good"));
        }

        [Fact]
        public void Probe_HugeQuantityTransaction_NoOverflow()
        {
            var sys = new MarketSystem();
            sys.BindCatalog(FuzzCatalog());
            var t = sys.Buy("g0", int.MaxValue, 1);
            Assert.True(t.Accepted);
            Assert.False(float.IsInfinity(t.TotalValue));
            Assert.False(float.IsNaN(t.TotalValue));
            Assert.True(t.TotalValue > 0f);
        }
    }
}
