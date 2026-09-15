// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    /// <summary>
    /// Plan 212 Phase 2 — MarketSystem extension behavior: backward-compat v1
    /// path, category indices, bounded smoothing, shocks (apply/refresh/
    /// expiry), trade pressure (buy/sell/decay), arbitrage bounds, v2 save
    /// round-trip, v1 legacy migration, and paired determinism.
    /// </summary>
    public sealed class Plan212DynamicEconomyTests
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
                volatility = 0f,        // zero volatility isolates category/shock math
                elasticity = 1f
            });
            load.Goods.Add(new GoodDefinition
            {
                id = "bandages",
                displayName = "Bandages",
                category = "medical",
                basePrice = 20f,
                volatility = 0f,
                elasticity = 1f
            });
            var market = new MarketSystem();
            market.BindCatalog(GoodsCatalogLoader.ToCatalog(load));
            return market;
        }

        private static CommodityBaselineCatalog CreateCommodityCatalog()
        {
            var load = new CommodityBaselineLoadResult();
            load.Categories.Add(new CommodityBaselineDefinition
            {
                category_id = "food",
                base_multiplier_permille = 1000,
                elasticity_class = "medium",
                scarcity_floor_permille = 700,
                scarcity_ceiling_permille = 2000
            });
            load.Categories.Add(new CommodityBaselineDefinition
            {
                category_id = "medical",
                base_multiplier_permille = 1100,
                elasticity_class = "high",
                scarcity_floor_permille = 800,
                scarcity_ceiling_permille = 2600
            });
            return CommodityBaselineCatalogLoader.ToCatalog(load);
        }

        // ── Backward compatibility ────────────────────────────────────

        [Fact]
        public void NoCommodityCatalog_GetPrice_IsIdenticalToLegacyPath()
        {
            var market = CreateMarket();
            market.AdjustDemand("canned_food", 0.5f);
            // base 10 × demand 1.5 = 15; no category factor can exist.
            Assert.Equal(15f, market.GetPrice("canned_food"), 5);
            var explanation = market.ExplainPrice("canned_food");
            Assert.Single(explanation.factors, f => f.kind == PriceFactorKind.Demand);
            Assert.Equal(1f, market.GetEffectiveCategoryMultiplierForItem("canned_food"), 5);
        }

        [Fact]
        public void NoCommodityCatalog_IndexAtNeutral_NoFactorRowsEmitted()
        {
            var market = CreateMarket();
            market.BindCommodityCatalog(CreateCommodityCatalog());
            // Untracked category index = 1.0 → identical price, no factor rows.
            Assert.Equal(1f, market.GetCategoryMultiplier("food"), 5);
            Assert.Equal(10f, market.GetPrice("canned_food"), 5);
            var explanation = market.ExplainPrice("canned_food");
            Assert.Single(explanation.factors, f => f.kind == PriceFactorKind.Demand);
        }

        // ── Category indices ─────────────────────────────────────────

        [Fact]
        public void CategoryIndex_TrackedCategory_ScalesPrice()
        {
            var market = CreateMarket();
            market.BindCommodityCatalog(CreateCommodityCatalog());
            market.GetStateMutable().categoryIndices.Add(new CategoryIndexEntry { categoryId = "food", multiplier = 1.2f });
            Assert.Equal(12f, market.GetPrice("canned_food"), 4);
            var explanation = market.ExplainPrice("canned_food");
            Assert.Single(explanation.factors, f => f.kind == PriceFactorKind.Category);
            Assert.Equal(1.2f, explanation.factors.First(f => f.kind == PriceFactorKind.Category).multiplier, 5);
        }

        [Fact]
        public void TickDay_IndexPullsTowardBaselineTarget_WithBoundedSmoothing()
        {
            var market = CreateMarket();
            market.BindCommodityCatalog(CreateCommodityCatalog());
            var state = market.GetStateMutable();
            // Force a 2.0 index against a 1.0 target.
            state.categoryIndices.Add(new CategoryIndexEntry { categoryId = "food", multiplier = 2f });

            market.TickDay(2, new SeededRng(7));

            float afterOne = market.GetCategoryMultiplier("food");
            // Bounded lerp: moved 25% of the gap (2.0 → 1.75), not a teleport.
            Assert.Equal(1.75f, afterOne, 4);
            market.TickDay(3, new SeededRng(7));
            Assert.Equal(1.5625f, market.GetCategoryMultiplier("food"), 4);
        }

        [Fact]
        public void TickDay_IndexStaysWithinAuthoredBounds_AndConvergesToBaselineTarget()
        {
            var market = CreateMarket();
            market.BindCommodityCatalog(CreateCommodityCatalog());
            var state = market.GetStateMutable();
            // Force far above the authored ceiling (2.0): every step must be
            // clamped into [0.7, 2.0] while the smoothing pulls toward the
            // baseline target (1.0).
            state.categoryIndices.Add(new CategoryIndexEntry { categoryId = "food", multiplier = 9f });
            // Read path clamps to the authored baseline bounds [0.7, 2.0].
            float last = market.GetCategoryMultiplier("food");
            Assert.Equal(2.0f, last, 4);
            for (int day = 2; day < 20; day++)
            {
                market.TickDay(day, new SeededRng(7));
                last = market.GetCategoryMultiplier("food");
                Assert.InRange(last, 0.7f, 2.0f);
            }
            // Converged to (within lerp residual of) the authored baseline.
            Assert.True(Math.Abs(last - 1f) < 0.01f, $"index must converge to baseline (got {last})");
        }

        // ── Shocks ───────────────────────────────────────────────────

        [Fact]
        public void ApplyShock_Shortage_RaisesPrice_Crash_LowersPrice()
        {
            var market = CreateMarket();
            market.BindCommodityCatalog(CreateCommodityCatalog());
            var state = market.GetStateMutable();
            state.categoryIndices.Add(new CategoryIndexEntry { categoryId = "medical", multiplier = 1.1f });

            Assert.NotNull(market.ApplyShock("medical", isShortage: true, severityBp: 2000f, startDay: 5, durationDays: 4, sourceId: "weather_winter"));
            // Demand 1.0 × category 1.1 × shortage (1.2) = 26.4 on base 20.
            Assert.Equal(26.4f, market.GetPrice("bandages"), 3);

            market.GetStateMutable().activeShocks.Clear();
            Assert.NotNull(market.ApplyShock("medical", isShortage: false, severityBp: 2000f, startDay: 5, durationDays: 4, sourceId: "convoy_arrival"));
            // 1.1 × 0.8 = 0.88 → 17.6.
            Assert.Equal(17.6f, market.GetPrice("bandages"), 3);
        }

        [Fact]
        public void ApplyShock_SeverityClamps_AndUnknownCategoryRejected()
        {
            var market = CreateMarket();
            market.BindCommodityCatalog(CreateCommodityCatalog());
            var shock = market.ApplyShock("medical", true, 9000f, 5, 4, "test");
            Assert.NotNull(shock);
            Assert.Equal(3000f, shock!.severityBp, 5);
            Assert.Null(market.ApplyShock("not_a_category", true, 1000f, 5, 4, "test"));
        }

        [Fact]
        public void ApplyShock_IsIdempotentPerCategoryKindSource()
        {
            var market = CreateMarket();
            market.BindCommodityCatalog(CreateCommodityCatalog());
            var first = market.ApplyShock("food", true, 1000f, 5, 4, "weather_winter");
            var refreshed = market.ApplyShock("food", true, 2500f, 6, 8, "weather_winter");
            Assert.NotNull(first);
            Assert.NotNull(refreshed);
            Assert.Equal(first!.shockId, refreshed!.shockId);
            Assert.Equal(2500f, refreshed.severityBp, 5);
            Assert.Equal(14, refreshed.expiryDay);
            Assert.Single(market.ActiveShocks);
        }

        [Fact]
        public void Shock_ExpiresDeterministically_Once()
        {
            var market = CreateMarket();
            market.BindCommodityCatalog(CreateCommodityCatalog());
            market.GetStateMutable().categoryIndices.Add(new CategoryIndexEntry { categoryId = "medical", multiplier = 1.1f });
            var expired = new List<MarketShockState>();
            market.OnShockExpired += s => expired.Add(s);

            var shock = market.ApplyShock("medical", true, 2000f, 5, 3, "weather_winter");
            Assert.NotNull(shock);
            Assert.Equal(8, shock!.expiryDay); // startDay 5 + duration 3
            Assert.Equal(26.4f, market.GetPrice("bandages"), 3);

            market.TickDay(7, new SeededRng(3));
            Assert.Empty(expired); // 8 > 7: still active through day 7

            market.TickDay(8, new SeededRng(7));
            Assert.Single(expired); // expiryDay 8 <= 8: fires exactly on the day-8 tick
            market.TickDay(9, new SeededRng(7));
            Assert.Single(expired); // never re-fires
            Assert.Empty(market.ActiveShocks);
            // Price reconciles to the live category index (asymptotic smoothing:
            // the index drifted toward the shocked target while active, and is
            // now pulled back toward the 1.1 baseline).
            Assert.Equal(20f * market.GetCategoryMultiplier("medical"), market.GetPrice("bandages"), 4);
            Assert.True(Math.Abs(market.GetPrice("bandages") - 22f) < 0.7f);
        }

        // ── Trade pressure ───────────────────────────────────────────

        [Fact]
        public void Buy_Sell_RecordOpposingPressure_BarterSplitsLegs()
        {
            var market = CreateMarket();
            market.BindCommodityCatalog(CreateCommodityCatalog());
            market.Buy("canned_food", 10, 1, "settlement");
            market.Sell("canned_food", 4, 1, "settlement");
            var pressure = market.TradePressure.Single(p => p.categoryId == "food");
            Assert.Equal(10f, pressure.buyUnits, 4);
            Assert.Equal(4f, pressure.sellUnits, 4);

            market.Barter("bandages", 2, "canned_food", 1);
            var medPressure = market.TradePressure.Single(p => p.categoryId == "medical");
            Assert.Equal(2f, medPressure.sellUnits, 4);
        }

        [Fact]
        public void BuyPressure_RaisesTarget_SellPressure_LowersTarget()
        {
            var market = CreateMarket();
            market.BindCommodityCatalog(CreateCommodityCatalog());
            // Seed the index so the pressure term is observable immediately.
            market.GetStateMutable().categoryIndices.Add(new CategoryIndexEntry { categoryId = "food", multiplier = 1f });

            market.Buy("canned_food", 2000, 1, "settlement");
            market.TickDay(2, new SeededRng(7));
            float raised = market.GetCategoryMultiplier("food");
            Assert.True(raised > 1f, $"buy pressure must raise the index (got {raised})");

            var market2 = CreateMarket();
            market2.BindCommodityCatalog(CreateCommodityCatalog());
            market2.GetStateMutable().categoryIndices.Add(new CategoryIndexEntry { categoryId = "food", multiplier = 1f });
            market2.Sell("canned_food", 2000, 1, "settlement");
            market2.TickDay(2, new SeededRng(7));
            float lowered = market2.GetCategoryMultiplier("food");
            Assert.True(lowered < 1f, $"sell pressure must lower the index (got {lowered})");
        }

        [Fact]
        public void Pressure_DecaysDaily_TowardZero()
        {
            var market = CreateMarket();
            market.Buy("canned_food", 100, 1, "settlement");
            var pressure = market.TradePressure.Single(p => p.categoryId == "food");
            Assert.Equal(100f, pressure.buyUnits, 4);
            market.TickDay(2, new SeededRng(7));
            Assert.Equal(75f, pressure.buyUnits, 4);
            market.TickDay(3, new SeededRng(7));
            Assert.Equal(56.25f, pressure.buyUnits, 4);
        }

        // ── Arbitrage controls ───────────────────────────────────────

        [Fact]
        public void Arbitrage_RepeatedBuySellLoops_NeverCompound()
        {
            var market = CreateMarket();
            market.BindCommodityCatalog(CreateCommodityCatalog());
            market.GetStateMutable().categoryIndices.Add(new CategoryIndexEntry { categoryId = "food", multiplier = 1f });

            int day = 1;
            float spent = 0f, earned = 0f;
            for (int cycle = 0; cycle < 30; cycle++)
            {
                spent += market.GetPrice("canned_food") * 100f;
                market.Buy("canned_food", 100, day, "settlement");
                day++;
                market.TickDay(day, new SeededRng(day));
                earned += market.GetPrice("canned_food") * 100f;
                market.Sell("canned_food", 100, day, "settlement");
                day++;
                market.TickDay(day, new SeededRng(day));
            }

            // The loop is self-limiting: sells push the index down each cycle,
            // so per-cycle gain cannot compound. Cumulative sell value must
            // stay within a bounded envelope of spend (the documented
            // oscillation amplitude), never an exponential multiple.
            Assert.True(spent > 0);
            double ratio = earned / spent;
            Assert.True(ratio < 1.6, $"round-trip ratio must stay bounded (got {ratio:F3} — compounding suspicion)");
            // After the player stops, the index decays back to its baseline.
            for (int i = 0; i < 20; i++) market.TickDay(day++, new SeededRng(day));
            // Decayed back to (within rounding of) the authored baseline.
            Assert.True(Math.Abs(market.GetCategoryMultiplier("food") - 1f) < 0.01f);
        }

        // ── Save / restore ───────────────────────────────────────────

        [Fact]
        public void CaptureRestore_CurrentVersion_RoundTripsExactly()
        {
            var market = CreateMarket();
            market.BindCommodityCatalog(CreateCommodityCatalog());
            market.GetStateMutable().categoryIndices.Add(new CategoryIndexEntry { categoryId = "food", multiplier = 1.3f });
            market.GetStateMutable().categoryIndices.Add(new CategoryIndexEntry { categoryId = "medical", multiplier = 1.1f });
            market.ApplyShock("medical", true, 1500f, 3, 6, "weather_winter");
            market.Buy("canned_food", 50, 3, "settlement");
            market.TickDay(4, new SeededRng(11));

            var saved = market.CaptureState();
            // Plan 14A bumped the market save additively v2→v3 (nested embargo
            // state); this pin tracks the current version.
            Assert.Equal(3, saved.version);

            var restored = new MarketSystem();
            restored.BindCatalog(GoodsCatalogLoader.ToCatalog(new GoodsCatalogLoadResult
            {
                Goods =
                {
                    new GoodDefinition { id = "canned_food", displayName = "Canned Food", category = "food", basePrice = 10f, volatility = 0f, elasticity = 1f },
                    new GoodDefinition { id = "bandages", displayName = "Bandages", category = "medical", basePrice = 20f, volatility = 0f, elasticity = 1f }
                }
            }));
            restored.BindCommodityCatalog(CreateCommodityCatalog());
            restored.RestoreState(saved);

            Assert.Equal(market.GetPrice("bandages"), restored.GetPrice("bandages"), 5);
            Assert.Equal(market.GetCategoryMultiplier("food"), restored.GetCategoryMultiplier("food"), 5);
            Assert.Single(restored.ActiveShocks);
            Assert.Equal(saved.activeShocks[0].shockId, restored.ActiveShocks[0].shockId);
            Assert.Equal(saved.tradePressure.Count, restored.TradePressure.Count);
        }

        [Fact]
        public void Restore_V1LegacySave_MigratesNeutral_NeutralNotRunaway()
        {
            var market = new MarketSystem();
            market.BindCatalog(GoodsCatalogLoader.ToCatalog(new GoodsCatalogLoadResult
            {
                Goods =
                {
                    new GoodDefinition { id = "canned_food", displayName = "Canned Food", category = "food", basePrice = 10f, volatility = 0f, elasticity = 1f },
                    new GoodDefinition { id = "bandages", displayName = "Bandages", category = "medical", basePrice = 20f, volatility = 0f, elasticity = 1f }
                }
            }));
            market.BindCommodityCatalog(CreateCommodityCatalog());            var legacy = new MarketState
            {
                version = 1,
                day = 30,
                demand = new List<DemandEntry> { new DemandEntry { itemId = "canned_food", multiplier = 1.4f } }
            };
            market.RestoreState(legacy);

            // Legacy save: neutral Plan 212 state, legacy demand preserved.
            Assert.Empty(market.ActiveShocks);
            Assert.Empty(market.TradePressure);
            Assert.Equal(1f, market.GetCategoryMultiplier("food"), 5);
            Assert.Equal(1.4f, market.GetDemandMultiplier("canned_food"), 5);
            // Price is the legacy path only: 10 × 1.4 = 14.
            Assert.Equal(14f, market.GetPrice("canned_food"), 5);
        }

        [Fact]
        public void Restore_NewerVersion_ThrowsLoudly()
        {
            var market = new MarketSystem();
            // Always one past the CURRENT supported version (Plan 14A moved it
            // 2→3; the contract is "newer than supported throws", not a pinned
            // literal that silently stops throwing on every version bump).
            var future = new MarketState { version = MarketState.Version + 1 };
            Assert.Throws<InvalidOperationException>(() => market.RestoreState(future));
        }

        // ── Determinism ──────────────────────────────────────────────

        [Fact]
        public void PairedRun_SameSeedSameInputs_IdenticalPrices()
        {
            var a = CreateMarket();
            var b = CreateMarket();
            a.BindCommodityCatalog(CreateCommodityCatalog());
            b.BindCommodityCatalog(CreateCommodityCatalog());

            int day = 1;
            for (int i = 0; i < 30; i++)
            {
                a.Buy("canned_food", 10, day, "settlement");
                b.Buy("canned_food", 10, day, "settlement");
                if (i == 15)
                {
                    a.ApplyShock("medical", true, 1800f, day, 5, "weather_winter");
                    b.ApplyShock("medical", true, 1800f, day, 5, "weather_winter");
                }
                day++;
                a.TickDay(day, new SeededRng(day));
                b.TickDay(day, new SeededRng(day));
                Assert.Equal(a.GetPrice("canned_food"), b.GetPrice("canned_food"), 5);
                Assert.Equal(a.GetPrice("bandages"), b.GetPrice("bandages"), 5);
            }
            Assert.Equal(a.CaptureState().day, b.CaptureState().day);
        }

        [Fact]
        public void TickDay_TicksExactlyOnce_PerCall_NoDoubleAdvance()
        {
            var market = CreateMarket();
            var before = market.State.tickCount;
            market.TickDay(2, new SeededRng(7));
            Assert.Equal(before + 1, market.State.tickCount);
            Assert.Equal(2, market.State.day);
        }
    }

    /// <summary>Test seam: exposes the live mutable state for index seeding.</summary>
    internal static class Plan212TestExtensions
    {
        public static MarketState GetStateMutable(this MarketSystem market) => market.State;
    }
}
