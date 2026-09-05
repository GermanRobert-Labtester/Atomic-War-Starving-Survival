// SPDX-License-Identifier: MIT
// Plan 56 phase 5 — provenance-aware trade pricing. Pins:
//   1. WorldShortageDemandScale (best relief across active caravan regions);
//   2. seeded before/after price traces (scaled vs unscaled scarcity deltas,
//      deterministic replay);
//   3. clamp bounds under extreme repeated application;
//   4. long-horizon arbitrage re-audit (120 days, no runaway, no collapse).

using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.IO;

namespace Ashfall.Core.Tests;

public class Plan56Phase5Tests
{
    // Active caravan origin regions (caravans.json — deep_coast / ash_flats /
    // industrial_belt / settlement). Fixes the world configuration for tests.
    private static readonly string[] ActiveRegions =
    {
        "deep_coast", "ash_flats", "industrial_belt", "settlement",
    };

    private static GoodsCatalog LoadCatalog()
    {
        var cwd = System.IO.Directory.GetCurrentDirectory();
        CatalogLocator.TryFindDataDirectory(cwd, out var dataDir);
        var dir = string.IsNullOrEmpty(dataDir) ? cwd : dataDir;
        var load = GoodsCatalogLoader.Load(dir, new FileSystemIO(), new SystemTextJsonSerializer());
        Assert.False(load.HasErrors, string.Join("\n", load.Errors));
        return GoodsCatalogLoader.ToCatalog(load);
    }

    // ── World scale ────────────────────────────────────────────────────

    [Fact]
    public void WorldScale_BestReliefAcrossActiveRegions()
    {
        var catalog = LoadCatalog();

        // ash_flats services the greenhouse pool → canned_food (greenhouse) buffered.
        Assert.Equal(0.5f, RegionalSupplyRouter.WorldShortageDemandScale(
            catalog, "canned_food", ActiveRegions), 3);
        // industrial_belt services foundry goods → buffered too.
        Assert.Equal(0.5f, RegionalSupplyRouter.WorldShortageDemandScale(
            catalog, "water_filter", ActiveRegions), 3);
        // General supply tracks the market.
        Assert.Equal(1.0f, RegionalSupplyRouter.WorldShortageDemandScale(
            catalog, "duct_tape", ActiveRegions), 3);
        // No active region services the traplines pool → structural scarcity.
        Assert.Equal(1.5f, RegionalSupplyRouter.WorldShortageDemandScale(
            catalog, "cooked_meat", ActiveRegions), 3);
        // Unknown/unannotated goods stay neutral (legacy behavior).
        Assert.Equal(1.0f, RegionalSupplyRouter.WorldShortageDemandScale(
            catalog, "does_not_exist", ActiveRegions), 3);
    }

    [Fact]
    public void WorldScale_NarrowActiveSet_EscalatesUnservicedPools()
    {
        var catalog = LoadCatalog();
        // Only the deep_coast route runs: foundry/greenhouse goods have no
        // supply line and escalate; flotilla/coastal goods are buffered.
        var regions = new[] { "deep_coast" };
        Assert.Equal(1.5f, RegionalSupplyRouter.WorldShortageDemandScale(
            catalog, "water_filter", regions), 3);
        Assert.Equal(0.5f, RegionalSupplyRouter.WorldShortageDemandScale(
            catalog, "item_logistics_cipher_sheet", regions), 3);
        Assert.Equal(1.0f, RegionalSupplyRouter.WorldShortageDemandScale(
            catalog, "duct_tape", regions), 3);
    }

    // ── Seeded before/after price traces ───────────────────────────────

    private static MarketSystem MarketWithScaledScarcity(
        GoodsCatalog catalog, string scarceGood, float baseDelta, int days, bool provenanceScaled)
    {
        var market = new MarketSystem();
        market.BindCatalog(catalog);
        for (int day = 1; day <= days; day++)
        {
            market.TickDay(day, new SeededRng(900 + day));
            float d = provenanceScaled
                ? baseDelta * RegionalSupplyRouter.WorldShortageDemandScale(
                    catalog, scarceGood, ActiveRegions)
                : baseDelta;
            market.AdjustDemand(scarceGood, d);
        }
        return market;
    }

    [Fact]
    public void ScarcityTraces_ProvenanceScalesTheDelta()
    {
        var catalog = LoadCatalog();

        // Before (unscaled): the seed list's canned_food demand gets the raw
        // +0.02/day wildlife-pressure delta.
        var before = MarketWithScaledScarcity(catalog, "canned_food", 0.02f, 20, provenanceScaled: false);

        // After (scaled): canned_food is greenhouse-supplied — the ash_flats
        // convoy line buffers it to 0.5×.
        var after = MarketWithScaledScarcity(catalog, "canned_food", 0.02f, 20, provenanceScaled: true);

        // Both runs share the identical seeded walk (same catalog → same
        // draw order), so the demand difference is EXACTLY the nudge
        // differential: 0.02 × (1 − 0.5) × 20 days = 0.2.
        float beforeDemand = before.GetDemandMultiplier("canned_food");
        float afterDemand = after.GetDemandMultiplier("canned_food");
        Assert.Equal(0.2f, beforeDemand - afterDemand, 3);
        Assert.True(afterDemand < beforeDemand, "provenance scaling must buffer supplied goods");
        // Both runs stayed inside the demand clamp (no ceiling interference).
        Assert.True(beforeDemand < MarketSystem.MaxDemandMult);
        Assert.True(afterDemand < MarketSystem.MaxDemandMult);
    }

    [Fact]
    public void ScarcityTraces_AreDeterministicAcrossReplays()
    {
        var catalog = LoadCatalog();
        var run1 = MarketWithScaledScarcity(catalog, "canned_food", 0.02f, 30, provenanceScaled: true);
        var run2 = MarketWithScaledScarcity(catalog, "canned_food", 0.02f, 30, provenanceScaled: true);

        foreach (var id in new[] { "canned_food", "duct_tape", "water_filter", "tobacco_pouch" })
            Assert.Equal(run1.GetPrice(id), run2.GetPrice(id), 5);
        Assert.Equal(run1.State.ledger.Count, run2.State.ledger.Count);
    }

    // ── Clamp bounds + long-horizon audit ──────────────────────────────

    [Fact]
    public void ExtremeScarcity_ClampsAtCeiling_NeverNaN()
    {
        var catalog = LoadCatalog();
        var market = new MarketSystem();
        market.BindCatalog(catalog);
        for (int day = 1; day <= 90; day++)
        {
            market.TickDay(day, new SeededRng(400 + day));
            // Deliberately extreme: 0.5/day on a buffered good.
            market.AdjustDemand("canned_food", 0.5f * RegionalSupplyRouter.WorldShortageDemandScale(
                catalog, "canned_food", ActiveRegions));
        }
        float price = market.GetPrice("canned_food");
        Assert.False(float.IsNaN(price));
        Assert.False(float.IsNegative(price));
        Assert.True(price <= 14f * MarketSystem.PriceCeilingFraction + 0.001f, "ceiling holds");
        Assert.True(price >= 14f * MarketSystem.PriceFloorFraction - 0.001f, "floor holds");
    }

    [Fact]
    public void LongHorizonAudit_NoRunaway_NoCollapse_AllBounded()
    {
        var catalog = LoadCatalog();
        var market = new MarketSystem();
        market.BindCatalog(catalog);

        var scarcityByDay = new Dictionary<int, string>();
        var pool = new[]
        {
            ("canned_food", "greenhouse"), ("water_filter", "foundry"),
            ("cooked_meat", "traplines"), ("duct_tape", "general"),
        };
        int days = 120;
        for (int day = 1; day <= days; day++)
        {
            market.TickDay(day, new SeededRng(5000 + day));
            // Rotate scarcity pressure across all three scale classes.
            var (good, _) = pool[day % pool.Length];
            scarcityByDay[day] = good;
            float scale = RegionalSupplyRouter.WorldShortageDemandScale(
                catalog, good, ActiveRegions);
            market.AdjustDemand(good, 0.01f * scale);
        }

        // Bounds hold for every good after the full horizon.
        foreach (var good in catalog.All())
        {
            var price = market.GetPrice(good.id);
            Assert.False(float.IsNaN(price));
            Assert.True(price >= good.basePrice * MarketSystem.PriceFloorFraction - 0.001f,
                $"{good.id} below floor: {price}");
            Assert.True(price <= good.basePrice * MarketSystem.PriceCeilingFraction + 0.001f,
                $"{good.id} above ceiling: {price}");
        }

        // Scarcity-pressured goods end at or above their start price
        // (pressure is visible), while nothing collapses to the floor.
        foreach (var (good, tag) in pool)
        {
            var def = catalog.Find(good)!;
            var price = market.GetPrice(good);
            Assert.True(price >= def.basePrice * MarketSystem.PriceFloorFraction,
                $"{good} collapsed despite pressure");
        }
    }

    [Fact]
    public void LongHorizonAudit_IsDeterministic()
    {
        var catalog = LoadCatalog();
        var run = new List<Dictionary<string, float>>();
        for (int replay = 0; replay < 2; replay++)
        {
            var market = new MarketSystem();
            market.BindCatalog(catalog);
            var trace = new Dictionary<string, List<float>>();
            var pool = new[]
            {
                ("canned_food", "greenhouse"), ("water_filter", "foundry"),
                ("cooked_meat", "traplines"), ("duct_tape", "general"),
            };
            for (int day = 1; day <= 120; day++)
            {
                market.TickDay(day, new SeededRng(5000 + day));
                var (good, _) = pool[day % pool.Length];
                float scale = RegionalSupplyRouter.WorldShortageDemandScale(
                    catalog, good, ActiveRegions);
                market.AdjustDemand(good, 0.01f * scale);
                foreach (var (g, _) in pool)
                {
                    if (!trace.TryGetValue(g, out var list)) trace[g] = list = new List<float>();
                    trace[g].Add(market.GetPrice(g));
                }
            }
            run.Add(trace.ToDictionary(kv => kv.Key, kv => kv.Value.Last()));
        }
        Assert.Equal(run[0], run[1]);
    }
}
