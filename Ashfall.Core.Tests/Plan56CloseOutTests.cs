// SPDX-License-Identifier: MIT
// Plan 56 close-out — independent verification pins the parallel expansion
// relied on but never wrote:
//   1. old-save compatibility (a pre-expansion MarketState restores cleanly;
//      goods absent from the save read demand 1.0 — id-keyed, migration-free);
//   2. settlement/caravan goods-reference integrity (every referenced id
//      resolves in the goods catalog, or in items.json for item-space needs).

using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.IO;

namespace Ashfall.Core.Tests;

public class Plan56CloseOutTests
{
    private static string DataDir()
    {
        var cwd = System.IO.Directory.GetCurrentDirectory();
        CatalogLocator.TryFindDataDirectory(cwd, out var dataDir);
        return string.IsNullOrEmpty(dataDir) ? cwd : dataDir;
    }

    private static GoodsCatalog LoadCatalog()
    {
        var load = GoodsCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
        Assert.False(load.HasErrors, string.Join("\n", load.Errors));
        return GoodsCatalogLoader.ToCatalog(load);
    }

    // ── Old-save compatibility (Plan 56 §14) ──────────────────────────

    [Fact]
    public void OldSave_NewGoodsDefaultToNeutralDemand_AndContinueDeterministically()
    {
        var catalog = LoadCatalog();

        // A pre-expansion market save: demand rows exist only for goods that
        // existed before the expansion, keyed by id (no positional arrays).
        var oldSave = new MarketState
        {
            version = MarketState.Version,
            day = 30,
            tickCount = 30,
            demand =
            {
                new DemandEntry { itemId = "clean_water", multiplier = 1.4f },
                new DemandEntry { itemId = "scrap_metal", multiplier = 0.8f },
            },
        };

        var market = new MarketSystem();
        market.RestoreState(oldSave);
        market.BindCatalog(catalog);

        // Every good absent from the old save reads neutral demand 1.0.
        foreach (var good in catalog.All())
        {
            float expected = good.id == "clean_water" ? 1.4f
                : good.id == "scrap_metal" ? 0.8f
                : 1f;
            Assert.Equal(expected, market.GetDemandMultiplier(good.id), 3);
        }

        // Prices for the old goods are preserved exactly (id-keyed restore).
        Assert.Equal(8f * 1.4f, market.GetPrice("clean_water"), 3);
        Assert.Equal(3f * 0.8f, market.GetPrice("scrap_metal"), 3);

        // Deterministic continuation on the expanded catalog.
        market.TickDay(31, new SeededRng(560));
        float afterTick = market.GetPrice("tobacco_pouch");
        var market2 = new MarketSystem();
        market2.RestoreState(oldSave);
        market2.BindCatalog(catalog);
        market2.TickDay(31, new SeededRng(560));
        Assert.Equal(afterTick, market2.GetPrice("tobacco_pouch"), 5);
    }

    [Fact]
    public void NewGoodMarketState_RoundTrips_AndContinuesIdentically()
    {
        var catalog = LoadCatalog();
        var market = new MarketSystem();
        market.BindCatalog(catalog);
        for (int day = 1; day <= 5; day++)
            market.TickDay(day, new SeededRng(770));

        var saved = market.CaptureState();
        var restored = new MarketSystem();
        restored.RestoreState(saved);
        restored.BindCatalog(catalog);

        // Post-reload prices identical for a representative new good.
        Assert.Equal(market.GetPrice("tobacco_pouch"), restored.GetPrice("tobacco_pouch"), 5);
        Assert.Equal(market.GetPrice("ammo_556"), restored.GetPrice("ammo_556"), 5);

        // Seeded continuation identical after reload.
        var a = new List<float>();
        var b = new List<float>();
        for (int day = 6; day <= 10; day++)
        {
            market.TickDay(day, new SeededRng(770 + day));
            restored.TickDay(day, new SeededRng(770 + day));
            a.Add(market.GetPrice("diesel_fuel"));
            b.Add(restored.GetPrice("diesel_fuel"));
        }
        Assert.Equal(a, b);
    }

    // ── Reference integrity: settlements + caravans (Plan 56 §49) ─────

    private sealed class StringListProbe
    {
        public List<string> trade_goods = new();
        public List<string> trade_needs = new();
    }

    [Fact]
    public void SettlementAndCaravanGoodsRefs_AllResolve()
    {
        var catalog = LoadCatalog();
        var dir = DataDir();
        var fileIO = new FileSystemIO();

        // items.json ids are legal for trade_needs (item-space demands).
        var itemsRaw = fileIO.ReadAllText(fileIO.Combine(dir, "items.json"));
        var itemIds = new HashSet<string>(System.StringComparer.Ordinal);
        foreach (var m in System.Text.RegularExpressions.Regex.Matches(itemsRaw, "\"id\"\\s*:\\s*\"([a-z0-9_]+)\"").Cast<System.Text.RegularExpressions.Match>())
            itemIds.Add(m.Groups[1].Value);

        int goodsRefs = 0;
        int itemSpaceRefs = 0;

        var settlements = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, System.Text.Json.JsonElement>>(
            fileIO.ReadAllText(fileIO.Combine(dir, "settlements.json")));
        Assert.True(settlements != null && settlements.ContainsKey("settlements"));
        foreach (var s in settlements["settlements"].EnumerateArray())
        {
            foreach (var probe in new[] { "trade_goods", "trade_needs" })
            {
                if (!s.TryGetProperty(probe, out var arr)) continue;
                foreach (var r in arr.EnumerateArray())
                {
                    var id = r.GetString();
                    if (string.IsNullOrEmpty(id)) continue;
                    if (catalog.Find(id) != null) { goodsRefs++; continue; }
                    if (itemIds.Contains(id)) { itemSpaceRefs++; continue; }
                    Assert.True(false, $"settlements.json {probe}: reference '{id}' resolves to neither the goods catalog nor items.json");
                }
            }
        }

        var caravans = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, System.Text.Json.JsonElement>>(
            fileIO.ReadAllText(fileIO.Combine(dir, "caravans.json")));
        Assert.True(caravans != null && caravans.ContainsKey("caravans"));
        foreach (var c in caravans["caravans"].EnumerateArray())
        {
            if (!c.TryGetProperty("specialty_goods", out var arr)) continue;
            foreach (var g in arr.EnumerateArray())
            {
                if (!g.TryGetProperty("item_id", out var idEl)) continue;
                var id = idEl.GetString();
                if (string.IsNullOrEmpty(id)) continue;
                if (catalog.Find(id) != null) { goodsRefs++; continue; }
                if (itemIds.Contains(id)) { itemSpaceRefs++; continue; }
                Assert.True(false, $"caravans.json specialty_goods: reference '{id}' resolves to neither catalog");
            }
        }

        // The wiring is real: the batch must exercise both reference spaces.
        Assert.True(goodsRefs > 0, "no goods-catalog references found in settlement/caravan profiles");
        Assert.True(itemSpaceRefs > 0, "no item-space references found (trade needs are item-space by design)");
    }

    [Fact]
    public void AllSixPlan56Goods_AreReachableThroughSettlementOrCaravanProfiles()
    {
        var dir = DataDir();
        var fileIO = new FileSystemIO();
        var catalog = LoadCatalog();
        var plan56Goods = new[]
        {
            "ammo_556", "ammo_12g", "diesel_fuel",
            "item_smoked_meat", "item_pickled_tubers", "tobacco_pouch",
        };

        var settlementsRaw = fileIO.ReadAllText(fileIO.Combine(dir, "settlements.json"));
        var caravansRaw = fileIO.ReadAllText(fileIO.Combine(dir, "caravans.json"));

        foreach (var id in plan56Goods)
        {
            bool inSettlement = settlementsRaw.Contains("\"" + id + "\"");
            bool inCaravan = caravansRaw.Contains("\"" + id + "\"");
            Assert.True(inSettlement || inCaravan,
                $"plan 56 good '{id}' is not wired into any settlement or caravan profile");
            Assert.True(catalog.Find(id) != null, $"'{id}' must exist in the goods catalog");
        }
    }
}
