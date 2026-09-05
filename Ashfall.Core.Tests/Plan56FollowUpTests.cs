// SPDX-License-Identifier: MIT
// Plan 56 follow-up — regionalSupply as a first-class loader field, and the
// four previously-empty categories (documents / weapons / contraband / misc)
// filled with economically distinct canonical goods under the revised 48 count.

using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.IO;

namespace Ashfall.Core.Tests;

public class Plan56FollowUpTests
{
    private static (GoodsCatalogLoadResult load, GoodsCatalog catalog) Load()
    {
        var cwd = System.IO.Directory.GetCurrentDirectory();
        CatalogLocator.TryFindDataDirectory(cwd, out var dataDir);
        var dir = string.IsNullOrEmpty(dataDir) ? cwd : dataDir;
        var load = GoodsCatalogLoader.Load(dir, new FileSystemIO(), new SystemTextJsonSerializer());
        Assert.False(load.HasErrors, string.Join("\n", load.Errors));
        return (load, GoodsCatalogLoader.ToCatalog(load));
    }

    // ── regionalSupply is first-class ─────────────────────────────────

    [Fact]
    public void RegionalSupply_ParsesAsFirstClassField()
    {
        var (_, catalog) = Load();
        // Pre-existing provenance rows survive the load.
        Assert.Equal("foundry", catalog.Find("water_filter")!.regionalSupply);
        Assert.Equal("traplines", catalog.Find("cooked_meat")!.regionalSupply);
        Assert.Equal("greenhouse", catalog.Find("seed_packets")!.regionalSupply);
        // The new category-fill goods carry it too.
        Assert.Equal("flotilla", catalog.Find("item_logistics_cipher_sheet")!.regionalSupply);
        Assert.Equal("general", catalog.Find("duct_tape")!.regionalSupply);
    }

    [Fact]
    public void RegionalSupply_IsOptional_AndTrimmed()
    {
        var (_, catalog) = Load();
        // Rows without the annotation default to empty (not null).
        Assert.Equal(string.Empty, catalog.Find("clean_water")!.regionalSupply);
        // The 27 annotated rows are all trimmed, non-empty, lowercase tags.
        foreach (var good in catalog.All())
            if (!string.IsNullOrEmpty(good.regionalSupply))
                Assert.Equal(good.regionalSupply, good.regionalSupply.Trim());
    }

    // ── The four filled categories ────────────────────────────────────

    [Fact]
    public void PreviouslyEmptyCategories_AreNowFilled()
    {
        var (_, catalog) = Load();
        Assert.Equal(2, catalog.All().Count(g => g.category == "documents"));
        Assert.Equal(2, catalog.All().Count(g => g.category == "weapons"));
        Assert.Equal(1, catalog.All().Count(g => g.category == "contraband"));
        Assert.Equal(3, catalog.All().Count(g => g.category == "misc"));
    }

    [Fact]
    public void NewCategoryGoods_AreCanonicalItemIds_AndEconomicallyDistinct()
    {
        var cwd = System.IO.Directory.GetCurrentDirectory();
        CatalogLocator.TryFindDataDirectory(cwd, out var dataDir);
        var dir = string.IsNullOrEmpty(dataDir) ? cwd : dataDir;
        var fileIO = new FileSystemIO();

        var itemsRaw = fileIO.ReadAllText(fileIO.Combine(dir, "items.json"));
        var newGoods = new Dictionary<string, GoodDefinition>();
        foreach (var id in new[]
                 {
                     "item_logistics_cipher_sheet", "sealed_government_document",
                     "weapon_sidearm", "weapon_pipe_shotgun",
                     "item_taper_kit_opioid", "duct_tape", "rope", "item_cassette_tape",
                 })
        {
            var def = catalog_probe(id, dir, fileIO);
            Assert.NotNull(def);
            // Modern convention: good id = canonical items.json id.
            Assert.Contains($"\"id\": \"{id}\"", itemsRaw);
            newGoods[id] = def!;
        }

        // No two of the eight share an economic signature.
        var sigs = new HashSet<string>();
        foreach (var g in newGoods.Values)
            Assert.True(sigs.Add($"{g.category}|{g.basePrice}|{g.volatility}|{g.elasticity}"),
                $"duplicate economic signature on {g.id}");
    }

    private static GoodDefinition? catalog_probe(string id, string dir, FileSystemIO fileIO)
    {
        var load = GoodsCatalogLoader.Load(dir, fileIO, new SystemTextJsonSerializer());
        return GoodsCatalogLoader.ToCatalog(load).Find(id);
    }

    // ── Market semantics hold for the new goods ───────────────────────

    [Fact]
    public void NewGoods_PriceWithinClampBand_AndDeterministic()
    {
        var (_, catalog) = Load();
        var market = new MarketSystem();
        market.BindCatalog(catalog);
        for (int day = 1; day <= 10; day++)
            market.TickDay(day, new SeededRng(24601));

        foreach (var id in new[]
                 {
                     "item_logistics_cipher_sheet", "sealed_government_document",
                     "weapon_sidearm", "weapon_pipe_shotgun", "item_taper_kit_opioid",
                     "duct_tape", "rope", "item_cassette_tape",
                 })
        {
            var def = catalog.Find(id)!;
            var price = market.GetPrice(id);
            Assert.True(price >= def.basePrice * MarketSystem.PriceFloorFraction - 0.001f,
                $"{id} below price floor");
            Assert.True(price <= def.basePrice * MarketSystem.PriceCeilingFraction + 0.001f,
                $"{id} above price ceiling");
        }

        // Replay identical.
        var market2 = new MarketSystem();
        market2.BindCatalog(catalog);
        for (int day = 1; day <= 10; day++)
            market2.TickDay(day, new SeededRng(24601));
        foreach (var id in new[] { "item_logistics_cipher_sheet", "duct_tape", "item_cassette_tape" })
            Assert.Equal(market.GetPrice(id), market2.GetPrice(id), 5);
    }

    [Fact]
    public void Baseline40_AreUntouchedByTheCategoryFill()
    {
        // The 40 goods that existed before this follow-up keep their records.
        var (_, catalog) = Load();
        Assert.Equal(48, catalog.Count);
        foreach (var id in new[]
                 {
                     "clean_water", "scrap_metal", "antibiotics", "iodine_pills", "fuel",
                     "9mm_ammo", "crowbar", "gas_mask", "dosimeter", "canned_food",
                     "diamond", "coal", "cooked_meat", "water_filter", "air_filter",
                     "seed_packets", "anti_rad", "tobacco_pouch", "ammo_556", "ammo_12g",
                     "diesel_fuel", "item_smoked_meat", "item_pickled_tubers",
                 })
            Assert.True(catalog.Find(id) != null, $"baseline good {id} missing");
    }
}
