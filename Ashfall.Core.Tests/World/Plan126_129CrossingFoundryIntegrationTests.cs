// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class Plan126_129CrossingFoundryIntegrationTests
    {
        private static string DataDirectory
        {
            get
            {
                string start = Directory.GetCurrentDirectory();
                if (CatalogLocator.TryFindDataDirectory(start, out string found))
                    return found;
                if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                    return found;
                throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
            }
        }

        private static readonly string[] s_plan126ExpansionItemIds = new[]
        {
            "item_arbitration_token",
            "item_charter_stamp",
            "item_weighbridge_chit",
            "item_smuggled_medicine",
            "item_crossing_bread",
            "item_lamp_oil_crossing",
            "item_filtered_water_crossing",
            "item_quarantine_bands",
            "item_granary_receipt",
            "item_smugglers_ledger",
            "item_rejection_notice",
            "item_crossing_map",
            "item_black_market_pouch",
            "item_charter_draft"
        };

        private static readonly string[] s_plan129ExpansionProductIds = new[]
        {
            "foundry_prod_bronze_datum_plate",
            "foundry_prod_flywheel_rotor_shaft",
            "foundry_prod_flywheel_containment_ring",
            "foundry_prod_culvert_brace",
            "foundry_prod_sealed_lead_pig",
            "foundry_prod_ground_anchor_spikes",
            "foundry_prod_turbine_blade_blank",
            "foundry_prod_rail_grinding_head",
            "foundry_prod_press_tooling_set"
        };

        private sealed class FoundryCatalogJson
        {
            public int schema_version { get; set; }
            public List<FoundryProductJson>? products { get; set; }
        }

        private sealed class FoundryProductJson
        {
            public string? product_id { get; set; }
            public string? display_name { get; set; }
            public string? category { get; set; }
            public string? result_item_id { get; set; }
            public int result_amount { get; set; }
            public List<FoundryIngredientJson>? ingredients { get; set; }
            public float labor_hours { get; set; }
            public float cast_hours { get; set; }
            public int fuel_units { get; set; }
            public int water_litres { get; set; }
            public string? treaty_id { get; set; }
        }

        private sealed class FoundryIngredientJson
        {
            public string? item_id { get; set; }
            public int amount { get; set; }
        }

        [Fact]
        public void Plan126_CrossingItems_LoadsExactTwentyFiveItemsAndValidatesExpansionTier()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new CrossingCatalogLoader(files, json);
            var catalog = loader.Load(DataDirectory);

            Assert.NotNull(catalog);
            Assert.Equal(25, catalog.Items.Count);

            var itemMap = catalog.Items.ToDictionary(i => i.id, StringComparer.Ordinal);

            foreach (string expectedId in s_plan126ExpansionItemIds)
            {
                Assert.True(itemMap.ContainsKey(expectedId), $"Crossing catalog missing Plan 126 item '{expectedId}'");
                var item = itemMap[expectedId];

                Assert.NotNull(item);
                Assert.StartsWith("item_", item.id, StringComparison.Ordinal);
                Assert.False(string.IsNullOrWhiteSpace(item.displayName), $"Item '{expectedId}' has empty displayName");
                Assert.False(string.IsNullOrWhiteSpace(item.description), $"Item '{expectedId}' has empty description");
                Assert.False(string.IsNullOrWhiteSpace(item.type), $"Item '{expectedId}' has empty type");
                Assert.True(item.stackMax > 0, $"Item '{expectedId}' stackMax must be positive");
                Assert.True(item.weight >= 0f, $"Item '{expectedId}' weight must be non-negative");
                Assert.True(item.tradeValue >= 0, $"Item '{expectedId}' tradeValue must be non-negative");
            }
        }

        [Fact]
        public void Plan129_FoundryProduction_LoadsExactThirtyFiveProductsAndValidatesNineExpansionProducts()
        {
            string path = Path.Combine(DataDirectory, "foundry_production.json");
            Assert.True(File.Exists(path), "foundry_production.json must exist");

            var catalog = JsonSerializer.Deserialize<FoundryCatalogJson>(
                File.ReadAllText(path), SystemTextJsonSerializer.Options);

            Assert.NotNull(catalog);
            Assert.Equal(1, catalog!.schema_version);
            Assert.NotNull(catalog.products);
            Assert.Equal(35, catalog.products!.Count);

            var productMap = catalog.products.ToDictionary(p => p.product_id!, StringComparer.Ordinal);

            foreach (string expectedId in s_plan129ExpansionProductIds)
            {
                Assert.True(productMap.ContainsKey(expectedId), $"Foundry catalog missing Plan 129 product '{expectedId}'");
                var prod = productMap[expectedId];

                Assert.NotNull(prod);
                Assert.StartsWith("foundry_prod_", prod.product_id!, StringComparison.Ordinal);
                Assert.False(string.IsNullOrWhiteSpace(prod.display_name), $"Product '{expectedId}' has empty display_name");
                Assert.False(string.IsNullOrWhiteSpace(prod.category), $"Product '{expectedId}' has empty category");
                Assert.False(string.IsNullOrWhiteSpace(prod.result_item_id), $"Product '{expectedId}' has empty result_item_id");
                Assert.StartsWith("item_", prod.result_item_id!, StringComparison.Ordinal);
                Assert.True(prod.result_amount > 0, $"Product '{expectedId}' result_amount must be positive");
                Assert.True(prod.labor_hours > 0f, $"Product '{expectedId}' labor_hours must be positive");
                Assert.True(prod.cast_hours > 0f, $"Product '{expectedId}' cast_hours must be positive");
                Assert.True(prod.fuel_units > 0, $"Product '{expectedId}' fuel_units must be positive");

                Assert.NotNull(prod.ingredients);
                Assert.NotEmpty(prod.ingredients!);
                foreach (var ing in prod.ingredients!)
                {
                    Assert.False(string.IsNullOrWhiteSpace(ing.item_id), $"Ingredient in '{expectedId}' has empty item_id");
                    Assert.True(ing.amount > 0, $"Ingredient '{ing.item_id}' in '{expectedId}' must have positive amount");
                }
            }
        }

        [Fact]
        public void Plan126_129_CrossDomainCoherence_IndustrialSupplyChainAndBorderCommerceLinkages()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var crossing = new CrossingCatalogLoader(files, json).Load(DataDirectory);

            string foundryPath = Path.Combine(DataDirectory, "foundry_production.json");
            var foundry = JsonSerializer.Deserialize<FoundryCatalogJson>(
                File.ReadAllText(foundryPath), SystemTextJsonSerializer.Options);

            // Crossing uses culvert transit and drainage:
            var smugglersCourt = crossing.GetFaction(CrossingIds.FactionSmugglersCourt);
            Assert.NotNull(smugglersCourt);
            Assert.Contains("culvert_transit", smugglersCourt!.offers);

            // Foundry produces heavy culvert braces:
            var culvertBrace = foundry!.products!.FirstOrDefault(p => p.product_id == "foundry_prod_culvert_brace");
            Assert.NotNull(culvertBrace);
            Assert.Equal("item_high_tensile_steel_culvert_brace", culvertBrace!.result_item_id);

            // Crossing economy requires precise calibration weights:
            var calWeight = crossing.Items.FirstOrDefault(i => i.id == CrossingIds.Items.CalibrationWeight);
            Assert.NotNull(calWeight);
            Assert.Equal(80, calWeight!.tradeValue);

            // Foundry produces bronze datum plates for survey and measurement registration:
            var datumPlate = foundry.products.FirstOrDefault(p => p.product_id == "foundry_prod_bronze_datum_plate");
            Assert.NotNull(datumPlate);
            Assert.Equal("item_datum_plate_bronze", datumPlate!.result_item_id);

            // Rule 5: Ensure strict separation of concern
            Assert.All(crossing.Items, i => Assert.StartsWith("item_", i.id));
            Assert.All(foundry.products, p => Assert.StartsWith("foundry_prod_", p.product_id));
        }

        [Fact]
        public void Plan126_129_DeterministicInventoryTradeAndProductionSimulation()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var crossing = new CrossingCatalogLoader(files, json).Load(DataDirectory);

            string foundryPath = Path.Combine(DataDirectory, "foundry_production.json");
            var foundry = JsonSerializer.Deserialize<FoundryCatalogJson>(
                File.ReadAllText(foundryPath), SystemTextJsonSerializer.Options);

            // Seeded deterministic simulation of a border trade & foundry run
            var rng1 = new SeededRng(126129);
            var rng2 = new SeededRng(126129);

            // Select an expansion crossing item to trade
            int crossingItemIdx1 = rng1.Next(0, s_plan126ExpansionItemIds.Length);
            int crossingItemIdx2 = rng2.Next(0, s_plan126ExpansionItemIds.Length);
            Assert.Equal(crossingItemIdx1, crossingItemIdx2);

            string tradedItem = s_plan126ExpansionItemIds[crossingItemIdx1];
            var crossingItem = crossing.Items.First(i => i.id == tradedItem);

            // Select an expansion foundry product to cast
            int foundryProdIdx1 = rng1.Next(0, s_plan129ExpansionProductIds.Length);
            int foundryProdIdx2 = rng2.Next(0, s_plan129ExpansionProductIds.Length);
            Assert.Equal(foundryProdIdx1, foundryProdIdx2);

            string castProduct = s_plan129ExpansionProductIds[foundryProdIdx1];
            var prod = foundry!.products!.First(p => p.product_id == castProduct);

            // Simulate deterministic production cost calculations
            float totalLabor1 = prod.labor_hours * (float)(rng1.NextDouble() * 0.2 + 0.9);
            float totalLabor2 = prod.labor_hours * (float)(rng2.NextDouble() * 0.2 + 0.9);
            Assert.Equal(totalLabor1, totalLabor2);

            Assert.True(crossingItem.tradeValue >= 0);
            Assert.True(prod.result_amount > 0);
        }
    }
}
