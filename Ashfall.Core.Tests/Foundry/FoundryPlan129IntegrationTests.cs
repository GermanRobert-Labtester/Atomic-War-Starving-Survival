// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Foundry;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.Foundry
{
    /// <summary>
    /// Plan 129 — Foundry Production Expansion (11 -> 20+ Products) Integration Suite.
    /// Validates catalog cardinality, foreign-key resolution, numeric ranges,
    /// downstream role coverage, save/load stability, determinism, and the three
    /// canonical strategic choice scenarios (Winter Fuel Conflict, Treaty vs Internal,
    /// and Expedition Preparation).
    /// </summary>
    public sealed class FoundryPlan129IntegrationTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static (SilentFoundryCatalog catalog, FoundryProductionFile production, HashSet<string> itemIds, HashSet<string> treatyIds) LoadFullData()
        {
            string dataDir = FindDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var production = SilentFoundryCatalogLoader.LoadProduction(dataDir, files, json);
            var faction = SilentFoundryCatalogLoader.LoadFaction(dataDir, files, json);
            var catalog = new SilentFoundryCatalog();
            catalog.Load(production, faction);

            // Item IDs from foundry_items.json and items.json
            var itemIds = new HashSet<string>(StringComparer.Ordinal);
            string foundryItemsPath = Path.Combine(dataDir, "foundry_items.json");
            if (File.Exists(foundryItemsPath))
            {
                var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(foundryItemsPath));
                foreach (var elem in doc.RootElement.GetProperty("items").EnumerateArray())
                {
                    if (elem.TryGetProperty("id", out var idProp))
                        itemIds.Add(idProp.GetString()!);
                }
            }

            string itemsPath = Path.Combine(dataDir, "items.json");
            if (File.Exists(itemsPath))
            {
                var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(itemsPath));
                foreach (var elem in doc.RootElement.GetProperty("items").EnumerateArray())
                {
                    if (elem.TryGetProperty("id", out var idProp))
                        itemIds.Add(idProp.GetString()!);
                }
            }

            // Treaty IDs from foundry_accords.json
            var treatyIds = new HashSet<string>(StringComparer.Ordinal);
            string accordsPath = Path.Combine(dataDir, "foundry_accords.json");
            if (File.Exists(accordsPath))
            {
                var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(accordsPath));
                if (doc.RootElement.TryGetProperty("treaties", out var treatiesProp))
                {
                    foreach (var elem in treatiesProp.EnumerateArray())
                    {
                        if (elem.TryGetProperty("treaty_id", out var tidProp))
                            treatyIds.Add(tidProp.GetString()!);
                    }
                }
            }

            return (catalog, production, itemIds, treatyIds);
        }

        private static SilentFoundrySystem CreateWiredSystem(
            SilentFoundryCatalog catalog,
            Dictionary<string, int> inventory,
            int seed = 1009)
        {
            var sys = new SilentFoundrySystem(rng: new SeededRng(seed));
            sys.BindCatalog(catalog, 4);
            sys.BindInventory(
                id => inventory.TryGetValue(id, out int v) ? v : 0,
                (_, _) => true,
                (id, amt) => inventory[id] = (inventory.TryGetValue(id, out int v) ? v : 0) + amt,
                (id, amt) => inventory[id] = Math.Max(0, (inventory.TryGetValue(id, out int v) ? v : 0) - amt));
            sys.Unlock(1);
            return sys;
        }

        // =================================================================
        // 1. Cardinality & Catalog Integrity
        // =================================================================

        [Fact]
        public void CatalogCardinality_MeetsAndExceedsTwentyProductFloor()
        {
            var (catalog, production, _, _) = LoadFullData();
            // Plan 129 target is 20 products; live repository has 26 verified products.
            Assert.True(production.products.Count >= 20, $"Expected >= 20 products, got {production.products.Count}");
            Assert.Equal(production.products.Count, catalog.ProductCount);
            Assert.Equal(production.products.Count, catalog.AllProducts.Count);
        }

        [Fact]
        public void ProductIds_AreUniqueAndFollowPrefixConvention()
        {
            var (_, production, _, _) = LoadFullData();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var p in production.products)
            {
                Assert.StartsWith("foundry_prod_", p.product_id);
                Assert.True(seen.Add(p.product_id), $"Duplicate product_id: {p.product_id}");
                Assert.False(string.IsNullOrWhiteSpace(p.display_name));
            }
        }

        [Fact]
        public void ForeignKeys_EveryResultItemAndIngredientResolves()
        {
            var (_, production, itemIds, treatyIds) = LoadFullData();
            Assert.NotEmpty(itemIds);

            foreach (var p in production.products)
            {
                Assert.True(itemIds.Contains(p.result_item_id),
                    $"Product '{p.product_id}' result_item_id '{p.result_item_id}' not found in item catalogs.");

                Assert.NotEmpty(p.ingredients);
                foreach (var ing in p.ingredients)
                {
                    Assert.True(itemIds.Contains(ing.item_id),
                        $"Product '{p.product_id}' ingredient '{ing.item_id}' not found in item catalogs.");
                    Assert.True(ing.amount > 0, $"Product '{p.product_id}' ingredient '{ing.item_id}' amount must be > 0.");
                }

                if (!string.IsNullOrEmpty(p.treaty_id))
                {
                    Assert.True(treatyIds.Contains(p.treaty_id),
                        $"Product '{p.product_id}' treaty_id '{p.treaty_id}' not found in foundry_accords.json.");
                    Assert.True(p.quota_amount > 0,
                        $"Product '{p.product_id}' with treaty '{p.treaty_id}' must have quota_amount > 0.");
                }
            }
        }

        [Fact]
        public void NumericBounds_AllCostsAndTargetsAreValid()
        {
            var (_, production, _, _) = LoadFullData();
            foreach (var p in production.products)
            {
                Assert.True(p.result_amount > 0, $"{p.product_id}: result_amount must be > 0");
                Assert.True(p.labor_hours > 0, $"{p.product_id}: labor_hours must be > 0");
                Assert.True(p.cast_hours > 0, $"{p.product_id}: cast_hours must be > 0");
                Assert.True(p.fuel_units > 0, $"{p.product_id}: fuel_units must be > 0");
                Assert.True(p.water_litres > 0, $"{p.product_id}: water_litres must be > 0");
                Assert.InRange(p.skill_target, 0f, 1f);
                Assert.InRange(p.quality_target, 0f, 100f);
            }
        }

        // =================================================================
        // 2. Downstream Roles and Reachability
        // =================================================================

        [Fact]
        public void RoleAndSinkCoverage_SpansAllStrategicDomains()
        {
            var (_, production, _, _) = LoadFullData();
            var sinks = production.products.Select(p => p.sink).Distinct().ToHashSet();

            // Sinks must cover survival, shelter, infrastructure, trade, defense, workshop, mining
            Assert.Contains("shelter", sinks);
            Assert.Contains("infrastructure", sinks);
            Assert.Contains("water", sinks);
            Assert.Contains("defense", sinks);
            Assert.Contains("workshop", sinks);
            Assert.Contains("mining", sinks);
            Assert.Contains("trade", sinks);
            Assert.Contains("road", sinks);
            Assert.Contains("saltworks", sinks);
            Assert.Contains("agriculture", sinks);
        }

        // =================================================================
        // 3. Strategic Scenarios (Section 3.12)
        // =================================================================

        [Fact]
        public void StrategicChoice_ScenarioA_WinterFuelConflict()
        {
            // Scenario A: Manufacturing heavy roof armor plate (9 fuel units) competes
            // directly with fuel reserved for shelter heating.
            var (catalog, _, _, _) = LoadFullData();
            var roofArmor = catalog.GetProduct("foundry_prod_roof_armor_plate");
            Assert.NotNull(roofArmor);
            Assert.Equal(9, roofArmor!.fuel_units);
            Assert.Equal(120, roofArmor.water_litres);

            // Starting fuel reserve = 10 units of coal
            var inventory = new Dictionary<string, int>
            {
                ["scrap_metal"] = 50,
                ["item_foundry_flux"] = 10,
                [SilentFoundryIds.ItemCoal] = 10,
                [SilentFoundryIds.ItemCharcoal] = 0,
                [SilentFoundryIds.ItemCleanWater] = 200
            };

            var sys = CreateWiredSystem(catalog, inventory);

            int day = 10;
            // 1. Smelt heavy roof armor
            string result = sys.StartProduction("foundry_prod_roof_armor_plate", workers: 4, workerSkill: 0.7f, day: day);
            Assert.Contains("Heat started", result);

            // 2. Fuel consumed: 9 coal units deducted immediately
            Assert.Equal(1, inventory[SilentFoundryIds.ItemCoal]);
            Assert.Equal(80, inventory[SilentFoundryIds.ItemCleanWater]);

            // 3. Subsequent heating or smelting demands are blocked by the fuel deficit
            string secondResult = sys.StartProduction("foundry_prod_roof_armor_plate", workers: 4, workerSkill: 0.7f, day: day);
            // Blocked because a heat is in progress
            Assert.Contains("already in progress", secondResult);

            // Complete the heat
            CompleteHeat(sys, ref day);
            Assert.Equal(FoundryHeatStage.Complete, sys.HeatStage);
            Assert.Equal(1, inventory["item_foundry_roof_armor_plate"]);

            // Attempting another heat requiring fuel fails due to fuel starvation
            string thirdResult = sys.StartProduction("foundry_prod_t_beam", workers: 4, workerSkill: 0.7f, day: day);
            Assert.Contains("Not enough fuel", thirdResult);
        }

        [Fact]
        public void StrategicChoice_ScenarioB_TreatyVsInternalRepair()
        {
            // Scenario B: Producing Brine-Resistant Pipe advances the treaty quota for
            // The Office's iodine exchange, rather than hoarding pipes for local water treatment.
            var (catalog, _, _, _) = LoadFullData();
            var pipe = catalog.GetProduct("foundry_prod_brine_pipe");
            Assert.NotNull(pipe);
            Assert.Equal("treaty_brine_pipe_and_iodine_exchange", pipe!.treaty_id);
            Assert.Equal(4, pipe.quota_amount);

            var inventory = new Dictionary<string, int>
            {
                ["scrap_metal"] = 100,
                ["item_foundry_alloy_additive"] = 20,
                ["item_foundry_flux"] = 20,
                [SilentFoundryIds.ItemCoal] = 50,
                [SilentFoundryIds.ItemCleanWater] = 500
            };

            var sys = CreateWiredSystem(catalog, inventory);
            sys.BindTreaties(new Dictionary<string, int>
            {
                ["treaty_brine_pipe_and_iodine_exchange"] = 280
            });

            // Cast 4 pipes to meet quota
            int currentDay = 280;
            for (int i = 0; i < 4; i++)
            {
                sys.PerformMaintenance(currentDay);
                sys.StartProduction("foundry_prod_brine_pipe", workers: 4, workerSkill: 0.75f, day: currentDay);
                CompleteHeat(sys, ref currentDay);
            }

            Assert.Equal(4, inventory["item_foundry_brine_pipe"]);

            // Evaluate treaty compliance on cycle assessment day
            bool quotaMetFired = false;
            sys.OnTreatyQuotaMet += c =>
            {
                if (c.treatyId == "treaty_brine_pipe_and_iodine_exchange")
                    quotaMetFired = true;
            };

            sys.AssessTreatyCompliance(310);
            Assert.True(quotaMetFired, "Expected treaty_brine_pipe_and_iodine_exchange quota to be fulfilled");
        }

        [Fact]
        public void StrategicChoice_ScenarioC_ExpeditionPreparation()
        {
            // Scenario C: Casting portable tools and ice anchors for expeditions
            var (catalog, _, _, _) = LoadFullData();
            var tool = catalog.GetProduct("foundry_prod_heavy_tool");
            Assert.NotNull(tool);
            Assert.Equal("item_foundry_heavy_tool", tool!.result_item_id);

            var inventory = new Dictionary<string, int>
            {
                ["scrap_metal"] = 50,
                ["item_foundry_flux"] = 10,
                [SilentFoundryIds.ItemCoal] = 30,
                [SilentFoundryIds.ItemCleanWater] = 200
            };

            var sys = CreateWiredSystem(catalog, inventory);

            int day = 5;
            sys.StartProduction("foundry_prod_heavy_tool", workers: 2, workerSkill: 0.75f, day: day);
            CompleteHeat(sys, ref day);

            Assert.Equal(1, inventory["item_foundry_heavy_tool"]);

            // Also cast ice anchors (30 per bundle)
            sys.StartProduction("foundry_prod_ice_anchor", workers: 2, workerSkill: 0.75f, day: day);
            CompleteHeat(sys, ref day);

            Assert.Equal(30, inventory["item_foundry_ice_anchor"]);
        }

        // =================================================================
        // 4. Save / Restore & Determinism
        // =================================================================

        [Fact]
        public void ActiveHeat_SurvivesSaveAndRestoreRoundtrip()
        {
            var (catalog, _, _, _) = LoadFullData();
            var inventory = new Dictionary<string, int>
            {
                ["scrap_metal"] = 50,
                ["item_foundry_flux"] = 10,
                [SilentFoundryIds.ItemCoal] = 30,
                [SilentFoundryIds.ItemCleanWater] = 200
            };

            int day = 15;
            var sys1 = CreateWiredSystem(catalog, inventory);
            sys1.StartProduction("foundry_prod_furnace_grate", workers: 3, workerSkill: 0.75f, day: day);
            Assert.Equal(FoundryHeatStage.ChargeLoaded, sys1.HeatStage);

            // Advance sys1 to AtHeat
            while (sys1.HeatStage != FoundryHeatStage.AtHeat && day < 100)
            {
                day++;
                sys1.TickDaily(day);
            }
            Assert.Equal(FoundryHeatStage.AtHeat, sys1.HeatStage);

            // Capture state
            var state = sys1.CaptureState();

            // Restore in fresh system
            var sys2 = CreateWiredSystem(catalog, inventory);
            sys2.RestoreState(state);

            Assert.Equal(FoundryHeatStage.AtHeat, sys2.HeatStage);
            Assert.Equal("foundry_prod_furnace_grate", sys2.State.activeProductId);
            Assert.Equal(3, sys2.State.assignedWorkers);

            // Finish cast in restored system
            string tapMsg = sys2.TapAndCast(day);
            Assert.StartsWith("Tap successful", tapMsg);
            while (sys2.HeatStage != FoundryHeatStage.Complete && day < 100)
            {
                day++;
                sys2.TickDaily(day);
            }
            Assert.Equal(FoundryHeatStage.Complete, sys2.HeatStage);
            Assert.Equal(1, inventory["item_foundry_furnace_grate"]);
        }

        [Fact]
        public void SimulationOutcomes_AreIdenticalWithSameSeed()
        {
            var (catalog, _, _, _) = LoadFullData();

            var invA = new Dictionary<string, int>
            {
                ["scrap_metal"] = 50,
                ["item_foundry_flux"] = 10,
                [SilentFoundryIds.ItemCoal] = 30,
                [SilentFoundryIds.ItemCleanWater] = 200
            };
            var sysA = CreateWiredSystem(catalog, invA, seed: 4242);

            var invB = new Dictionary<string, int>
            {
                ["scrap_metal"] = 50,
                ["item_foundry_flux"] = 10,
                [SilentFoundryIds.ItemCoal] = 30,
                [SilentFoundryIds.ItemCleanWater] = 200
            };
            var sysB = CreateWiredSystem(catalog, invB, seed: 4242);

            int dayA = 1;
            sysA.StartProduction("foundry_prod_repair_plate", workers: 2, workerSkill: 0.75f, day: dayA);
            CompleteHeat(sysA, ref dayA);

            int dayB = 1;
            sysB.StartProduction("foundry_prod_repair_plate", workers: 2, workerSkill: 0.75f, day: dayB);
            CompleteHeat(sysB, ref dayB);

            Assert.Equal(sysA.State.contamination, sysB.State.contamination);
            Assert.Equal(sysA.State.sandQuality, sysB.State.sandQuality);
            Assert.Equal(sysA.HeatStage, sysB.HeatStage);
            Assert.Equal(invA["item_foundry_repair_plate"], invB["item_foundry_repair_plate"]);
        }

        private static void CompleteHeat(SilentFoundrySystem sys, ref int day)
        {
            while (sys.HeatStage != FoundryHeatStage.AtHeat && day < 500)
            {
                day++;
                sys.TickDaily(day);
            }
            Assert.Equal(FoundryHeatStage.AtHeat, sys.HeatStage);

            string tap = sys.TapAndCast(day); // AtHeat -> Tapped
            Assert.StartsWith("Tap successful", tap);
            while (sys.HeatStage != FoundryHeatStage.Complete && day < 500)
            {
                day++;
                sys.TickDaily(day);
            }
            Assert.Equal(FoundryHeatStage.Complete, sys.HeatStage);
        }
    }
}
