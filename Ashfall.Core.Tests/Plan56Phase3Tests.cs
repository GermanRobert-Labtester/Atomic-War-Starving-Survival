// SPDX-License-Identifier: MIT
// Plan 56 phase 3 — regionalSupply provenance drives market behavior:
//   1. RegionalSupplyRouter tag normalization (both origin vocabularies);
//   2. provenance-driven caravan specialty cargo (replaces the hand-coded
//      regional tables when a catalog is bound);
//   3. settlement shortage logic (ShortageDemandScale + waystation resupply
//      filter: local goods ride out a shortage, pure imports lapse);
//   4. determinism and demo compatibility (unbound systems keep legacy stock).

using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.IO;
using Ashfall.Core.Waystation;

namespace Ashfall.Core.Tests;

public class Plan56Phase3Tests
{
    private static GoodsCatalog LoadCatalog()
    {
        var cwd = System.IO.Directory.GetCurrentDirectory();
        CatalogLocator.TryFindDataDirectory(cwd, out var dataDir);
        var dir = string.IsNullOrEmpty(dataDir) ? cwd : dataDir;
        var load = GoodsCatalogLoader.Load(dir, new FileSystemIO(), new SystemTextJsonSerializer());
        Assert.False(load.HasErrors, string.Join("\n", load.Errors));
        return GoodsCatalogLoader.ToCatalog(load);
    }

    // ── Tag normalization ──────────────────────────────────────────────

    [Fact]
    public void TagsForOrigin_HandlesBothVocabularies()
    {
        // Caravan route regions (caravans.json origin_region).
        Assert.Contains("coastal", RegionalSupplyRouter.TagsForOrigin("deep_coast"));
        Assert.Contains("flotilla", RegionalSupplyRouter.TagsForOrigin("deep_coast"));
        Assert.Contains("foundry", RegionalSupplyRouter.TagsForOrigin("industrial_belt"));
        Assert.Contains("greenhouse", RegionalSupplyRouter.TagsForOrigin("ash_flats"));
        // Legacy regional-specialty regions (the old hand-coded switch).
        Assert.Contains("flotilla", RegionalSupplyRouter.TagsForOrigin("flotilla"));
        Assert.Contains("foundry", RegionalSupplyRouter.TagsForOrigin("foundry"));
        Assert.Contains("greenhouse", RegionalSupplyRouter.TagsForOrigin("greenhouse"));
        Assert.Contains("traplines", RegionalSupplyRouter.TagsForOrigin("traplines"));
        // Everything trades general supply.
        Assert.Contains("general", RegionalSupplyRouter.TagsForOrigin("settlement"));
        Assert.Contains("general", RegionalSupplyRouter.TagsForOrigin("deep_coast"));
        Assert.Contains("general", RegionalSupplyRouter.TagsForOrigin(""));
    }

    // ── Provenance-driven cargo ────────────────────────────────────────

    [Fact]
    public void SpecialtyCargo_FlotillaOrigin_BindsFlotillaAnnotatedGoods()
    {
        var catalog = LoadCatalog();
        var lots = RegionalSupplyRouter.SpecialtyCargoForOrigin(catalog, "flotilla", maxLots: 4);
        Assert.NotEmpty(lots);
        Assert.True(lots.Count <= 4);
        foreach (var lot in lots)
        {
            var def = catalog.Find(lot.GoodId)!;
            // Every lot is annotated for the flotilla/coastal/general pool.
            Assert.Contains(def.regionalSupply, new[] { "flotilla", "coastal", "general" });
            // Quantity derives from trade stack size; priceRations from the ration anchor.
            Assert.InRange(lot.Quantity, 1, 8);
            Assert.True(lot.PriceRations >= 1);
            Assert.Equal(lot.PriceRations,
                System.Math.Max(1, (int)System.Math.Round(def.basePrice / 8f, System.MidpointRounding.AwayFromZero)));
        }
        // Base staples never appear as specialty lots.
        Assert.DoesNotContain(lots, l => l.GoodId == "clean_water");
        Assert.DoesNotContain(lots, l => l.GoodId == "canned_food");
        Assert.DoesNotContain(lots, l => l.GoodId == "antibiotics");
    }

    [Fact]
    public void SpecialtyCargo_IsDeterministic()
    {
        var catalog = LoadCatalog();
        var a = RegionalSupplyRouter.SpecialtyCargoForOrigin(catalog, "industrial_belt", 4);
        var b = RegionalSupplyRouter.SpecialtyCargoForOrigin(catalog, "industrial_belt", 4);
        Assert.Equal(a.Select(x => x.GoodId), b.Select(x => x.GoodId));

        var c = RegionalSupplyRouter.SpecialtyCargoForOrigin(catalog, "industrial_belt", 4, new SeededRng(64));
        var d = RegionalSupplyRouter.SpecialtyCargoForOrigin(catalog, "industrial_belt", 4, new SeededRng(64));
        Assert.Equal(c.Select(x => x.GoodId), d.Select(x => x.GoodId));
    }

    [Fact]
    public void ProducesGood_MatchesProvenance()
    {
        var catalog = LoadCatalog();
        // The foundry produces water_filter (regionalSupply: foundry).
        Assert.True(RegionalSupplyRouter.ProducesGood(catalog, "foundry", "water_filter"));
        Assert.True(RegionalSupplyRouter.ProducesGood(catalog, "industrial_belt", "water_filter"));
        // The flotilla does not produce it.
        Assert.False(RegionalSupplyRouter.ProducesGood(catalog, "flotilla", "water_filter"));
        // General supply is produced everywhere.
        Assert.True(RegionalSupplyRouter.ProducesGood(catalog, "greenhouse", "duct_tape"));
    }

    // ── Settlement shortage logic ──────────────────────────────────────

    [Fact]
    public void ShortageDemandScale_ProducedGeneralImported()
    {
        // Local production buffers the shortage.
        Assert.Equal(0.5f, RegionalSupplyRouter.ShortageDemandScale("foundry", "industrial_belt"), 3);
        // General supply tracks the market.
        Assert.Equal(1.0f, RegionalSupplyRouter.ShortageDemandScale("general", "industrial_belt"), 3);
        // Pure imports escalate.
        Assert.Equal(1.5f, RegionalSupplyRouter.ShortageDemandScale("greenhouse", "industrial_belt"), 3);
        Assert.Equal(1.5f, RegionalSupplyRouter.ShortageDemandScale("traplines", "ash_flats"), 3);
    }

    [Fact]
    public void FilterStockForShortage_KeepsLocalAndGeneral_DropsPureImports()
    {
        var catalog = LoadCatalog();
        var stock = new List<string>
        {
            "water_filter",                  // foundry — local to industrial_belt
            "mechanical_parts",              // foundry — local
            "tobacco_pouch",                 // settlement — general supply? no: settlement tag, imported here
            "duct_tape",                     // general — rides through
            "bandage",                       // item-space (not a good) — outside the market model, kept
            "seed_packets",                  // greenhouse — pure import here, lapses
        };
        var kept = RegionalSupplyRouter.FilterStockForShortage(stock, catalog, "industrial_belt");

        Assert.Contains("water_filter", kept);
        Assert.Contains("mechanical_parts", kept);
        Assert.Contains("duct_tape", kept);
        Assert.Contains("bandage", kept);
        Assert.DoesNotContain("seed_packets", kept);
        Assert.DoesNotContain("tobacco_pouch", kept);
    }

    // ── Caravan integration (bound vs unbound) ─────────────────────────

    [Fact]
    public void Caravan_BoundCatalog_SpawnsProvenanceCargo()
    {
        var catalog = LoadCatalog();
        var system = new TravelingCaravanSystem { Catalog = catalog };
        var route = new List<string> { "n1", "n2", "n3" };
        system.SpawnCaravan("caravan_test", "Test Train", "faction_the_scale", route, "flotilla");

        var caravan = system.GetCaravanAtNode("n1");
        Assert.NotNull(caravan);

        // Base staples are always present (legacy default trio).
        Assert.Contains(caravan!.inventory, i => i.itemId == "item_clean_water");
        // Specialty lots are provenance-annotated (no greenhouse-only leakage).
        var specialties = caravan.inventory
            .Where(i => i.itemId != "item_clean_water" && i.itemId != "item_canned_food" && i.itemId != "item_antibiotics")
            .ToList();
        Assert.NotEmpty(specialties);
        foreach (var item in specialties)
        {
            var def = catalog.Find(item.itemId);
            Assert.NotNull(def);
            Assert.Contains(def!.regionalSupply, new[] { "flotilla", "coastal", "general" });
            Assert.True(item.priceRations >= 1);
        }
    }

    [Fact]
    public void Caravan_UnboundSystem_KeepsLegacyRegionalTables()
    {
        // Demo/compat path: without a bound catalog the legacy hand-coded
        // tables apply (herbal_tea is a greenhouse-exclusive legacy row).
        var system = new TravelingCaravanSystem();
        system.SpawnCaravan("caravan_legacy", "Legacy Train", "faction_the_scale",
            new List<string> { "n1", "n2" }, "greenhouse");
        var caravan = system.GetCaravanAtNode("n1");
        Assert.NotNull(caravan);
        Assert.Contains(caravan!.inventory, i => i.itemId == "herbal_tea");
    }

    // ── Waystation shortage resupply ───────────────────────────────────

    private static WaystationNetworkSystem WaystationUnderTest(bool marketShort)
    {
        var catalog = LoadCatalog();
        var network = new WaystationNetworkSystem();
        network.BindShortagePolicy(catalog, () => marketShort);
        // Force a resupply on the next tick.
        var state = network.State;
        for (int i = 0; i < state.stations.Count; i++)
            state.stations[i].daysSinceResupply = 7;
        return network;
    }

    [Fact]
    public void Waystation_ShortageResupply_KeepsLocalDropsImports()
    {
        var network = WaystationUnderTest(marketShort: true);
        network.TickDay();
        // Every station's stock now excludes pure imports for its region.
        foreach (var station in network.State.stations)
        {
            var def = network.GetDefinition(station.stationId);
            var region = def!.region;
            foreach (var id in station.availableStockItemIds)
            {
                var good = catalog_probe(id);
                if (good == null) continue; // item-space stock passes through
                var scale = RegionalSupplyRouter.ShortageDemandScale(good.regionalSupply, region);
                Assert.True(scale < 1.5f,
                    $"{station.stationId}: imported good {id} lapsed but stayed in stock");
            }
        }
    }

    [Fact]
    public void Waystation_NormalMarket_ResuppliesFullStock()
    {
        var network = WaystationUnderTest(marketShort: false);
        var before = network.State.stations.Select(s => (s.stationId, s.availableStockItemIds.Count)).ToList();
        network.TickDay();
        foreach (var (stationId, expected) in before)
        {
            var def = network.GetDefinition(stationId)!;
            Assert.Equal(def.stock_item_ids.Count, network.GetStation(stationId)!.availableStockItemIds.Count);
        }
    }

    private static GoodDefinition? catalog_probe(string id)
    {
        var cwd = System.IO.Directory.GetCurrentDirectory();
        CatalogLocator.TryFindDataDirectory(cwd, out var dataDir);
        var dir = string.IsNullOrEmpty(dataDir) ? cwd : dataDir;
        var load = GoodsCatalogLoader.Load(dir, new FileSystemIO(), new SystemTextJsonSerializer());
        return GoodsCatalogLoader.ToCatalog(load).Find(id);
    }
}
