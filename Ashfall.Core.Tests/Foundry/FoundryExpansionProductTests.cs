// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Foundry;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.Foundry
{
    public sealed class FoundryExpansionProductTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static SilentFoundryCatalog LoadCatalog()
        {
            string dataDir = FindDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var production = SilentFoundryCatalogLoader.LoadProduction(dataDir, files, json);
            var faction = SilentFoundryCatalogLoader.LoadFaction(dataDir, files, json);
            var catalog = new SilentFoundryCatalog();
            catalog.Load(production, faction);
            return catalog;
        }

        [Fact]
        public void ProductionCatalog_LoadsCompleteCatalog_IncludesSumpRecoveryMelt()
        {
            var catalog = LoadCatalog();
            // Plan 70: the catalog grows concurrently (recovery melts etc.) —
            // pin a floor rather than an exact count, and pin the sump-cake
            // recovery melt explicitly (item_sludge_cake → scrap_metal).
            Assert.Equal(35, catalog.ProductCount);
            Assert.Equal(catalog.ProductCount, catalog.AllProducts.Count);
            var melt = catalog.GetProduct("foundry_prod_sludge_cake_recovery_melt");
            Assert.NotNull(melt);
            Assert.Equal("scrap_metal", melt!.result_item_id);
            Assert.Contains(melt.ingredients, i => i.item_id == "item_sludge_cake");
        }

        [Fact]
        public void ProductionProduct_HasValidProperties()
        {
            var cases = new[]
            {
                (ProductId: "foundry_prod_plowshare", ExpectedResultItem: "item_foundry_plowshare", ExpectedCategory: "agricultural_tool"),
                (ProductId: "foundry_prod_t_beam", ExpectedResultItem: "item_foundry_t_beam", ExpectedCategory: "structural_beam"),
                (ProductId: "foundry_prod_ice_anchor", ExpectedResultItem: "item_foundry_ice_anchor", ExpectedCategory: "ice_anchor"),
                (ProductId: "foundry_prod_winch_drum", ExpectedResultItem: "item_foundry_winch_drum", ExpectedCategory: "winch_drum"),
                (ProductId: "foundry_prod_brine_pipe", ExpectedResultItem: "item_foundry_brine_pipe", ExpectedCategory: "brine_resistant_pipe"),
                (ProductId: "foundry_prod_repair_plate", ExpectedResultItem: "item_foundry_repair_plate", ExpectedCategory: "repair_plate"),
                (ProductId: "foundry_prod_fastener_bracket", ExpectedResultItem: "item_foundry_bracket_fastener", ExpectedCategory: "bracket_fastener"),
                (ProductId: "foundry_prod_valve_body", ExpectedResultItem: "item_foundry_valve_body", ExpectedCategory: "water_component"),
                (ProductId: "foundry_prod_heavy_tool", ExpectedResultItem: "item_foundry_heavy_tool", ExpectedCategory: "heavy_tool"),
                (ProductId: "foundry_prod_alloy_part", ExpectedResultItem: "item_foundry_alloy_part", ExpectedCategory: "heavy_alloy_part"),
                (ProductId: "foundry_prod_defense_plate", ExpectedResultItem: "item_foundry_defense_plate", ExpectedCategory: "defense_plate"),
                (ProductId: "foundry_prod_roof_armor_plate", ExpectedResultItem: "item_foundry_roof_armor_plate", ExpectedCategory: "structural_armor"),
                (ProductId: "foundry_prod_shoring_bracket", ExpectedResultItem: "item_foundry_shoring_bracket", ExpectedCategory: "structural_bracket"),
                (ProductId: "foundry_prod_blast_fitting", ExpectedResultItem: "item_foundry_blast_fitting", ExpectedCategory: "structural_fitting"),
                (ProductId: "foundry_prod_reinforcement_shoe", ExpectedResultItem: "item_foundry_reinforcement_shoe", ExpectedCategory: "structural_support"),
                (ProductId: "foundry_prod_structural_coupling", ExpectedResultItem: "item_foundry_structural_coupling", ExpectedCategory: "structural_coupling"),
                (ProductId: "foundry_prod_replacement_die", ExpectedResultItem: "item_foundry_replacement_die", ExpectedCategory: "tooling_die"),
                (ProductId: "foundry_prod_drill_blanks", ExpectedResultItem: "item_foundry_drill_blanks", ExpectedCategory: "drill_blank"),
                (ProductId: "foundry_prod_crucible_spare", ExpectedResultItem: "item_foundry_crucible_spare", ExpectedCategory: "crucible_spare"),
                (ProductId: "foundry_prod_press_fitting", ExpectedResultItem: "item_foundry_press_fitting", ExpectedCategory: "press_fitting"),
                (ProductId: "foundry_prod_bearing_housing", ExpectedResultItem: "item_foundry_bearing_housing", ExpectedCategory: "bearing_housing"),
                (ProductId: "foundry_prod_furnace_grate", ExpectedResultItem: "item_foundry_furnace_grate", ExpectedCategory: "furnace_grate"),
                (ProductId: "foundry_prod_weather_canister", ExpectedResultItem: "item_foundry_weather_canister", ExpectedCategory: "abstract_ordnance"),
                (ProductId: "foundry_prod_cast_shot", ExpectedResultItem: "item_foundry_cast_shot", ExpectedCategory: "abstract_ordnance"),
                (ProductId: "foundry_prod_casing_blanks", ExpectedResultItem: "item_foundry_casing_blanks", ExpectedCategory: "abstract_ordnance"),
                (ProductId: "foundry_prod_bronze_datum_plate", ExpectedResultItem: "item_datum_plate_bronze", ExpectedCategory: "survey_datum"),
                (ProductId: "foundry_prod_flywheel_rotor_shaft", ExpectedResultItem: "item_forged_rotor_shaft", ExpectedCategory: "rotor_shaft"),
                (ProductId: "foundry_prod_flywheel_containment_ring", ExpectedResultItem: "item_containment_ring_steel", ExpectedCategory: "containment_ring"),
                (ProductId: "foundry_prod_culvert_brace", ExpectedResultItem: "item_high_tensile_steel_culvert_brace", ExpectedCategory: "structural_brace"),
                (ProductId: "foundry_prod_sealed_lead_pig", ExpectedResultItem: "item_sealed_lead_pig", ExpectedCategory: "radiation_container"),
                (ProductId: "foundry_prod_ground_anchor_spikes", ExpectedResultItem: "item_hardened_ground_anchor_spikes", ExpectedCategory: "defense_anchor"),
                (ProductId: "foundry_prod_turbine_blade_blank", ExpectedResultItem: "item_superalloy_turbine_blade_blank", ExpectedCategory: "turbine_blank"),
                (ProductId: "foundry_prod_rail_grinding_head", ExpectedResultItem: "item_rail_grinding_head", ExpectedCategory: "rail_tooling"),
                (ProductId: "foundry_prod_press_tooling_set", ExpectedResultItem: "item_press_tooling_set", ExpectedCategory: "press_tooling")
            };
            var catalog = LoadCatalog();
            var failures = new List<string>();

            foreach (var test in cases)
            {
                var product = catalog.GetProduct(test.ProductId);
                if (product == null)
                {
                    failures.Add($"{test.ProductId}: product is missing");
                    continue;
                }

                if (!string.Equals(test.ExpectedResultItem, product.result_item_id, StringComparison.Ordinal))
                    failures.Add($"{test.ProductId}: expected result {test.ExpectedResultItem}, got {product.result_item_id}");
                if (!string.Equals(test.ExpectedCategory, product.category, StringComparison.Ordinal))
                    failures.Add($"{test.ProductId}: expected category {test.ExpectedCategory}, got {product.category}");
                if (product.labor_hours <= 0)
                    failures.Add($"{test.ProductId}: labor_hours must be positive");
                if (product.cast_hours <= 0)
                    failures.Add($"{test.ProductId}: cast_hours must be positive");
                if (product.fuel_units <= 0)
                    failures.Add($"{test.ProductId}: fuel_units must be positive");
                if (product.water_litres <= 0)
                    failures.Add($"{test.ProductId}: water_litres must be positive");
                if (product.ingredients == null || product.ingredients.Count == 0)
                    failures.Add($"{test.ProductId}: ingredients must not be empty");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void TreatyProducts_CarryExplicitAuthoredQuotas()
        {
            var catalog = LoadCatalog();

            var anchors = catalog.GetProduct("foundry_prod_ice_anchor");
            Assert.NotNull(anchors);
            Assert.Equal("treaty_road_iron_charter", anchors.treaty_id);
            Assert.Equal(60, anchors.quota_amount);

            var winches = catalog.GetProduct("foundry_prod_winch_drum");
            Assert.NotNull(winches);
            Assert.Equal("treaty_road_iron_charter", winches.treaty_id);
            Assert.Equal(3, winches.quota_amount);

            var pipes = catalog.GetProduct("foundry_prod_brine_pipe");
            Assert.NotNull(pipes);
            Assert.Equal("treaty_brine_pipe_and_iodine_exchange", pipes.treaty_id);
            Assert.Equal(4, pipes.quota_amount);
        }

        [Fact]
        public void HeatBands_FollowPhysicalHierarchy()
        {
            var catalog = LoadCatalog();

            // Band 1: Light castings <= 3 fuel units
            var shot = catalog.GetProduct("foundry_prod_cast_shot");
            var bracket = catalog.GetProduct("foundry_prod_fastener_bracket");
            Assert.NotNull(shot);
            Assert.NotNull(bracket);
            Assert.True(shot.fuel_units <= 3);
            Assert.True(bracket.fuel_units <= 3);

            // Band 4: Extreme heavy alloy >= 6 fuel units and >= 12 labor hours
            var roofArmor = catalog.GetProduct("foundry_prod_roof_armor_plate");
            var bearingHousing = catalog.GetProduct("foundry_prod_bearing_housing");
            Assert.NotNull(roofArmor);
            Assert.NotNull(bearingHousing);
            Assert.True(roofArmor.fuel_units >= 6);
            Assert.True(roofArmor.labor_hours >= 12);
            Assert.True(bearingHousing.fuel_units >= 6);
            Assert.True(bearingHousing.labor_hours >= 12);
        }

        [Fact]
        public void SmeltingWorkflow_CanCastNewToolingDie()
        {
            var catalog = LoadCatalog();
            var sys = new SilentFoundrySystem(rng: new SeededRng(1009));
            sys.BindCatalog(catalog, 4);

            var inventory = new Dictionary<string, int>
            {
                ["scrap_metal"] = 100,
                ["item_foundry_alloy_additive"] = 10,
                ["item_foundry_flux"] = 20,
                [SilentFoundryIds.ItemCoal] = 50,
                [SilentFoundryIds.ItemCharcoal] = 50,
                [SilentFoundryIds.ItemCleanWater] = 200,
                ["item_foundry_firebrick"] = 10
            };

            sys.BindInventory(
                id => inventory.TryGetValue(id, out int v) ? v : 0,
                (_, _) => true,
                (id, amt) => inventory[id] = (inventory.TryGetValue(id, out int v) ? v : 0) + amt,
                (id, amt) => inventory[id] = Math.Max(0, (inventory.TryGetValue(id, out int v) ? v : 0) - amt));

            sys.Unlock(1);

            string startMsg = sys.StartProduction("foundry_prod_replacement_die", workers: 4, workerSkill: 0.7f, day: 2);
            Assert.Contains("Heat started", startMsg);
            Assert.Equal(FoundryHeatStage.ChargeLoaded, sys.HeatStage);

            string tapMsg = sys.TapAndCast(3);
            Assert.False(string.IsNullOrEmpty(tapMsg));
        }
    }
}
