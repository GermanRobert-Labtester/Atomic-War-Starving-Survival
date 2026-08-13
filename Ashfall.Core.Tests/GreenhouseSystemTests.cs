using System.Collections.Generic;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    public class GreenhouseSystemTests
    {
        private const string Mushroom = GreenhouseExpansionCatalog.Items.SeedMushroom;
        private const string Wheat = GreenhouseExpansionCatalog.Items.SeedWheat;

        private static GreenhouseSystem FreshSystem(int seed = 1)
        {
            var sys = new GreenhouseSystem(seed);
            sys.EnsurePlots(2);
            return sys;
        }

        [Fact]
        public void Plant_Requires_FallowPlot_AndKnownSeed()
        {
            var sys = FreshSystem();
            Assert.True(sys.Plant(0, Mushroom, 1, out var consumed));
            Assert.Equal(Mushroom, consumed);

            // Unknown seed rejected
            Assert.False(sys.Plant(1, "not_a_real_seed", 1, out _));
            // Occupied plot rejected
            Assert.False(sys.Plant(0, Mushroom, 1, out _));
            // Out of range rejected
            Assert.False(sys.Plant(99, Mushroom, 1, out _));
        }

        [Fact]
        public void WellTendedCrop_Matures_AndHarvests_Clean()
        {
            var sys = FreshSystem();
            var matured = new List<string>();
            sys.OnCropMatured += (plot, seed) => matured.Add(seed);

            Assert.True(sys.Plant(0, Mushroom, 1, out _));
            sys.Water(0, 60f, tainted: false);

            for (int i = 0; i < 5; i++)
                sys.TickDay(2 + i, growLightHours: 4f, ashContaminationRate: 0f);

            Assert.Contains(Mushroom, matured);
            var harvest = sys.Harvest(0);
            Assert.True(harvest.success, "Mature plot should harvest");
            Assert.Equal(GreenhouseExpansionCatalog.Items.CropMushroom, harvest.yieldItemId);
            Assert.Equal(2, harvest.amount);
            Assert.False(harvest.contaminated, "Clean soil should yield a clean crop");
            Assert.Equal(1, sys.TotalHarvests);
            Assert.True(GreenhouseSystem.IsFallow(sys.Plots[0]));
        }

        [Fact]
        public void TaintedIrrigation_ContaminatesHarvest()
        {
            var sys = FreshSystem();
            sys.Plant(0, Mushroom, 1, out _);
            sys.Water(0, 60f, tainted: true);
            Assert.True(sys.Plots[0].soilContamination >= 60f);

            for (int i = 0; i < 5; i++)
                sys.TickDay(2 + i, growLightHours: 4f, ashContaminationRate: 0f);

            var harvest = sys.Harvest(0);
            Assert.True(harvest.success);
            Assert.True(harvest.contaminated, "Contaminated soil should taint the harvest");
            Assert.Equal(GreenhouseExpansionCatalog.Items.TaintedFood, harvest.yieldItemId);
        }

        [Fact]
        public void Drought_StallsGrowth_AndFiresDriedOut()
        {
            var sys = FreshSystem();
            var dried = new List<int>();
            sys.OnPlotDriedOut += dried.Add;

            sys.Plant(0, Mushroom, 1, out _);
            sys.Water(0, 8f, tainted: false);

            sys.TickDay(2, growLightHours: 4f, ashContaminationRate: 0f);
            // Day 2 uses the 8 water; day 3 starts dry
            sys.TickDay(3, growLightHours: 4f, ashContaminationRate: 0f);

            Assert.Contains(0, dried);
            Assert.Equal(0f, sys.Plots[0].water);
        }

        [Fact]
        public void PreWarWheat_RequiresUnlock()
        {
            var sys = FreshSystem();
            Assert.False(sys.IsPreWarWheatUnlocked);
            Assert.False(sys.Plant(0, Wheat, 1, out _), "Wheat locked initially");

            sys.UnlockPreWarWheat();
            Assert.True(sys.IsPreWarWheatUnlocked);
            Assert.True(sys.Plant(0, Wheat, 1, out var consumed));
            Assert.Equal(Wheat, consumed);
        }

        [Fact]
        public void HeadlessDemo_PassesAllChecks()
        {
            var report = GreenhouseHeadlessDemo.Run();
            Assert.True(report.Passed);
            Assert.Equal(0, report.FailedCount);
            Assert.True(report.PassedCount >= 15);
        }
    }
}
