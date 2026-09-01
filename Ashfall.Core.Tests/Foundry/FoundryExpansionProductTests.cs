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
        public void ProductionCatalog_LoadsAll25Products()
        {
            var catalog = LoadCatalog();
            Assert.Equal(25, catalog.ProductCount);
            Assert.Equal(25, catalog.AllProducts.Count);
        }

        [Theory]
        [InlineData("foundry_prod_plowshare", "item_foundry_plowshare", "agricultural_tool")]
        [InlineData("foundry_prod_t_beam", "item_foundry_t_beam", "structural_beam")]
        [InlineData("foundry_prod_ice_anchor", "item_foundry_ice_anchor", "ice_anchor")]
        [InlineData("foundry_prod_winch_drum", "item_foundry_winch_drum", "winch_drum")]
        [InlineData("foundry_prod_brine_pipe", "item_foundry_brine_pipe", "brine_resistant_pipe")]
        [InlineData("foundry_prod_repair_plate", "item_foundry_repair_plate", "repair_plate")]
        [InlineData("foundry_prod_fastener_bracket", "item_foundry_bracket_fastener", "bracket_fastener")]
        [InlineData("foundry_prod_valve_body", "item_foundry_valve_body", "water_component")]
        [InlineData("foundry_prod_heavy_tool", "item_foundry_heavy_tool", "heavy_tool")]
        [InlineData("foundry_prod_alloy_part", "item_foundry_alloy_part", "heavy_alloy_part")]
        [InlineData("foundry_prod_defense_plate", "item_foundry_defense_plate", "defense_plate")]
        [InlineData("foundry_prod_roof_armor_plate", "item_foundry_roof_armor_plate", "structural_armor")]
        [InlineData("foundry_prod_shoring_bracket", "item_foundry_shoring_bracket", "structural_bracket")]
        [InlineData("foundry_prod_blast_fitting", "item_foundry_blast_fitting", "structural_fitting")]
        [InlineData("foundry_prod_reinforcement_shoe", "item_foundry_reinforcement_shoe", "structural_support")]
        [InlineData("foundry_prod_structural_coupling", "item_foundry_structural_coupling", "structural_coupling")]
        [InlineData("foundry_prod_replacement_die", "item_foundry_replacement_die", "tooling_die")]
        [InlineData("foundry_prod_drill_blanks", "item_foundry_drill_blanks", "drill_blank")]
        [InlineData("foundry_prod_crucible_spare", "item_foundry_crucible_spare", "crucible_spare")]
        [InlineData("foundry_prod_press_fitting", "item_foundry_press_fitting", "press_fitting")]
        [InlineData("foundry_prod_bearing_housing", "item_foundry_bearing_housing", "bearing_housing")]
        [InlineData("foundry_prod_furnace_grate", "item_foundry_furnace_grate", "furnace_grate")]
        [InlineData("foundry_prod_weather_canister", "item_foundry_weather_canister", "abstract_ordnance")]
        [InlineData("foundry_prod_cast_shot", "item_foundry_cast_shot", "abstract_ordnance")]
        [InlineData("foundry_prod_casing_blanks", "item_foundry_casing_blanks", "abstract_ordnance")]
        public void ProductionProduct_HasValidProperties(string productId, string expectedResultItem, string expectedCategory)
        {
            var catalog = LoadCatalog();
            var prod = catalog.GetProduct(productId);
            Assert.NotNull(prod);
            Assert.Equal(expectedResultItem, prod.result_item_id);
            Assert.Equal(expectedCategory, prod.category);
            Assert.True(prod.labor_hours > 0);
            Assert.True(prod.cast_hours > 0);
            Assert.True(prod.fuel_units > 0);
            Assert.True(prod.water_litres > 0);
            Assert.NotEmpty(prod.ingredients);
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
