// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Farming;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Farming
{
    public class FungiCultivationSystemTests
    {
        private UndergroundFloraCatalog CreateTestCatalog()
        {
            return new UndergroundFloraCatalog
            {
                strains = new List<FungusStrainDef>
                {
                    new FungusStrainDef
                    {
                        strain_id = "strain_phosphor_bracket",
                        growth_days = 2,
                        moisture_min = 0.3f,
                        moisture_max = 0.9f,
                        darkness_required = true,
                        light_output = 15f,
                        yield_item_id = "fungus_spores_bioluminescent",
                        yield_count = 3
                    },
                    new FungusStrainDef
                    {
                        strain_id = "strain_grey_mycelium",
                        growth_days = 2,
                        moisture_min = 0.4f,
                        moisture_max = 0.9f,
                        darkness_required = true,
                        light_output = 0f,
                        yield_item_id = "harvested_mushrooms_subterranean",
                        yield_count = 5
                    }
                },
                substrates = new List<SubstrateDef>
                {
                    new SubstrateDef
                    {
                        substrate_id = "substrate_organic_compost",
                        nutrition_multiplier = 1.0f,
                        moisture_retention = 0.8f,
                        contamination_risk = 0.05f
                    }
                }
            };
        }

        [Fact]
        public void CultivateSpores_ConsumesSpores_AndPlantsPlot()
        {
            var inv = new Inventory.Inventory();
            inv.AddById("harvested_mushrooms_subterranean", 1);
            inv.AddById("fungus_spores_common", 2);

            var sys = new FungiCultivationSystem(new SeededRng(42), inv);
            sys.RegisterCatalog(CreateTestCatalog());

            sys.EnsurePlot("plot_1", "room_cellar");

            var plantRes = sys.CultivateSpores("plot_1", "strain_grey_mycelium", "substrate_organic_compost", 1);
            Assert.True(plantRes.IsSuccess);

            var plot = sys.State.plots.Find(p => p.plotId == "plot_1");
            Assert.NotNull(plot);
            Assert.Equal("strain_grey_mycelium", plot!.strainId);
            Assert.False(plot.isHarvestReady);
        }

        [Fact]
        public void FungiGrowth_And_Harvest_Lifecycle()
        {
            var inv = new Inventory.Inventory();
            inv.AddById("fungus_spores_common", 5);

            var sys = new FungiCultivationSystem(new SeededRng(42), inv);
            sys.RegisterCatalog(CreateTestCatalog());

            sys.EnsurePlot("plot_bed", "room_bunker_deep");
            sys.CultivateSpores("plot_bed", "strain_grey_mycelium", "substrate_organic_compost", 1);

            // Day 1
            sys.TickDay(1, roomIsDark: true);
            var plot = sys.State.plots.Find(p => p.plotId == "plot_bed");
            Assert.True(plot!.growthStage > 0f);
            Assert.False(plot.isHarvestReady);

            // Day 2
            sys.TickDay(2, roomIsDark: true);
            Assert.True(plot.isHarvestReady);

            // Harvest
            var harvestRes = sys.HarvestPlot("plot_bed");
            Assert.True(harvestRes.IsSuccess);
            Assert.Equal(5, inv.CountById("harvested_mushrooms_subterranean"));
            Assert.Null(plot.strainId);
            Assert.False(plot.isHarvestReady);
        }

        [Fact]
        public void BioluminescentFungi_GeneratesRoomLight()
        {
            var inv = new Inventory.Inventory();
            inv.AddById("fungus_spores_common", 5);

            var sys = new FungiCultivationSystem(new SeededRng(42), inv);
            sys.RegisterCatalog(CreateTestCatalog());

            sys.EnsurePlot("plot_lum", "room_hydroponics");
            sys.CultivateSpores("plot_lum", "strain_phosphor_bracket", "substrate_organic_compost", 1);

            float initialLight = sys.GetBioluminescentLightOutput("room_hydroponics");
            Assert.Equal(0f, initialLight);

            sys.TickDay(1, roomIsDark: true);
            float lightAfterDay1 = sys.GetBioluminescentLightOutput("room_hydroponics");
            Assert.True(lightAfterDay1 > 0f);
        }

        [Fact]
        public void ToxicBloom_CanBePurged_WithCleanWater()
        {
            var inv = new Inventory.Inventory();
            inv.AddById("clean_water", 5);

            var sys = new FungiCultivationSystem(new SeededRng(42), inv);
            sys.RegisterCatalog(CreateTestCatalog());

            var plot = sys.EnsurePlot("plot_mold", "room_cellar");
            plot.hasToxicBloom = true;

            var purgeRes = sys.PurgeToxicBloom("plot_mold");
            Assert.True(purgeRes.IsSuccess);
            Assert.False(plot.hasToxicBloom);
            Assert.Equal(3, inv.CountById("clean_water")); // 2 consumed
        }

        [Fact]
        public void State_RoundTrip_PreservesFungiPlots()
        {
            var sys = new FungiCultivationSystem(new SeededRng(42));
            sys.RegisterCatalog(CreateTestCatalog());
            var p = sys.EnsurePlot("plot_save", "room_crypt");
            p.growthStage = 0.75f;
            p.strainId = "strain_grey_mycelium";

            var state = sys.State;
            var json = System.Text.Json.JsonSerializer.Serialize(state);

            var deserialized = System.Text.Json.JsonSerializer.Deserialize<FungiCultivationState>(json);
            var sys2 = new FungiCultivationSystem(new SeededRng(42));
            sys2.RestoreState(deserialized!);

            var restored = sys2.State.plots.Find(pl => pl.plotId == "plot_save");
            Assert.NotNull(restored);
            Assert.Equal(0.75f, restored!.growthStage, 2);
            Assert.Equal("strain_grey_mycelium", restored.strainId);
        }
    }
}
