using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Greenhouse;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class GreenhouseCropExpansionTests
    {
        [Fact]
        public void CropCatalog_ContainsAll13Crops()
        {
            // 12 canonical item_seed_* crops + the F18 mixed-packet route:
            // "seed_packets" (items.json assorted-vegetable envelope) maps to
            // the tuber profile so micro-location seed grants are plantable
            // through the one canonical CropCatalog contract.
            Assert.Equal(13, GreenhouseExpansionCatalog.CropCatalog.All.Length);
        }

        [Fact]
        public void CropCatalog_MixedSeedPacket_IsPlantable_AndCanonical()
        {
            // F18 — the pre-existing generic packet must resolve to a real
            // clean-yield crop; it may not silently remain a dead item.
            var def = GreenhouseExpansionCatalog.CropCatalog.Get(
                GreenhouseExpansionCatalog.Items.SeedPacketsMixed);
            Assert.NotNull(def);
            Assert.Equal("crop_tuber", def!.YieldCleanId);
            Assert.False(def.RequiresUnlock);
        }

        [Theory]
        [InlineData("item_seed_mushroom", "crop_mushroom")]
        [InlineData("item_seed_tuber", "crop_tuber")]
        [InlineData("item_seed_grain", "crop_grain")]
        [InlineData("item_seed_wheat", "crop_wheat")]
        [InlineData("item_seed_hardy_tuber", "crop_hardy_tuber")]
        [InlineData("item_seed_ash_grain", "crop_ash_grain")]
        [InlineData("item_seed_biolum_mushroom", "crop_biolum_mushroom")]
        [InlineData("item_seed_nutrient_algae", "crop_nutrient_algae")]
        [InlineData("item_seed_medicinal_herb", "crop_medicinal_herb")]
        [InlineData("item_seed_leafy_green", "crop_leafy_green")]
        [InlineData("item_seed_oilseed", "crop_oilseed")]
        [InlineData("item_seed_cold_legume", "crop_cold_legume")]
        public void CropCatalog_ResolvesSeedToCleanYield(string seedItemId, string expectedCleanYield)
        {
            var def = GreenhouseExpansionCatalog.CropCatalog.Get(seedItemId);
            Assert.NotNull(def);
            Assert.Equal(expectedCleanYield, def.YieldCleanId);
            Assert.Equal("tainted_food", def.YieldTaintedId);
            Assert.True(def.GrowthHoursToMature > 0);
            Assert.True(def.WaterPerDay > 0);
            Assert.True(def.BaseYield > 0);
            Assert.True(def.BlightResistance > 0);
            Assert.True(def.ContaminationTolerance > 0);
        }

        [Fact]
        public void GreenhouseSystem_SimulatesFrostTuberLifecycle()
        {
            var gh = new GreenhouseSystem(seed: 42);
            gh.EnsurePlots(4);

            // Plant Frost Tuber
            bool planted = gh.Plant(0, GreenhouseExpansionCatalog.Items.SeedHardyTuber, currentDay: 1, out var seedId);
            Assert.True(planted);
            Assert.Equal(GreenhouseExpansionCatalog.Items.SeedHardyTuber, seedId);

            var plot = gh.Plots[0];
            Assert.NotNull(plot);
            Assert.Equal(GreenhouseStage.Sprouting, (GreenhouseStage)plot.stage);
            Assert.Equal(GreenhouseExpansionCatalog.Items.SeedHardyTuber, plot.seedItemId);

            // Water and advance through growth stages
            gh.Water(0, 100f, tainted: false);

            // 120 hours / 24 = 5 days to mature
            for (int day = 2; day <= 8; day++)
            {
                gh.Water(0, 50f, tainted: false);
                gh.TickDay(day, growLightHours: 12f, ashContaminationRate: 0f);
            }

            plot = gh.Plots[0];
            Assert.Equal(GreenhouseStage.Mature, (GreenhouseStage)plot.stage);

            var harvest = gh.Harvest(0);
            Assert.True(harvest.success);
            Assert.Equal("crop_hardy_tuber", harvest.yieldItemId);
            Assert.False(harvest.contaminated);
            Assert.True(harvest.amount >= 3);
        }

        [Fact]
        public void GreenhouseSystem_SimulatesMedicinalHerbAndLeafyGreen()
        {
            var gh = new GreenhouseSystem(seed: 99);
            gh.EnsurePlots(4);

            // Plant Leafy Green in Plot 0 (Fast 60h crop)
            gh.Plant(0, GreenhouseExpansionCatalog.Items.SeedLeafyGreen, currentDay: 1, out _);
            gh.Water(0, 80f, tainted: false);

            // Plant Medicinal Herb in Plot 1 (160h crop)
            gh.Plant(1, GreenhouseExpansionCatalog.Items.SeedMedicinalHerb, currentDay: 1, out _);
            gh.Water(1, 80f, tainted: false);

            // Advance 3 days (72 hours) — Leafy green should mature, herb still growing
            for (int d = 2; d <= 4; d++)
            {
                gh.Water(0, 30f, tainted: false);
                gh.Water(1, 30f, tainted: false);
                gh.TickDay(d, growLightHours: 12f, ashContaminationRate: 0f);
            }

            var plot0 = gh.Plots[0];
            Assert.Equal(GreenhouseStage.Mature, (GreenhouseStage)plot0.stage);

            var plot1 = gh.Plots[1];
            Assert.Equal(GreenhouseStage.Growing, (GreenhouseStage)plot1.stage);

            var harvest0 = gh.Harvest(0);
            Assert.True(harvest0.success);
            Assert.Equal("crop_leafy_green", harvest0.yieldItemId);
        }

        [Fact]
        public void GreenhouseSystem_SaveRestore_PreservesExpandedCropState()
        {
            var gh1 = new GreenhouseSystem(seed: 123);
            gh1.EnsurePlots(4);
            gh1.Plant(0, GreenhouseExpansionCatalog.Items.SeedAshGrain, currentDay: 5, out _);
            gh1.Water(0, 60f, tainted: false);
            gh1.TickDay(6, growLightHours: 10f, ashContaminationRate: 0f);

            var state = gh1.CaptureState();
            Assert.NotNull(state);
            Assert.NotEmpty(state.plots);
            Assert.Equal("item_seed_ash_grain", state.plots[0].seedItemId);

            var gh2 = new GreenhouseSystem(seed: 999);
            gh2.RestoreState(state);

            var restoredPlot = gh2.Plots[0];
            Assert.NotNull(restoredPlot);
            Assert.Equal("item_seed_ash_grain", restoredPlot.seedItemId);
            Assert.Equal(state.plots[0].growth, restoredPlot.growth);
            Assert.Equal(state.plots[0].water, restoredPlot.water);
        }
    }
}
