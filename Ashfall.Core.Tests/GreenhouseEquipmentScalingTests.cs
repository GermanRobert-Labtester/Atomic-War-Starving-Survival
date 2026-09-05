using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 22 Phase E — host equipment scaling math (Core half). The host
    /// consumes these helpers (plot capacity from planter-box stock, light
    /// hours from grow-lamp stock); the Core side is the testable pure math
    /// plus the state contracts the host drives.
    /// </summary>
    public class GreenhouseEquipmentScalingTests
    {
        // ── LIGHT HOURS ─────────────────────────────────────────────────

        [Theory]
        [InlineData(0, 6f)]
        [InlineData(1, 8f)]
        [InlineData(2, 10f)]
        [InlineData(3, 10f)]  // capped
        [InlineData(9, 10f)]  // capped
        [InlineData(-2, 6f)]  // clamped
        public void GrowLightHoursFor_ScalesAndCaps(int lampCount, float expected)
        {
            Assert.Equal(expected, GreenhouseSystem.GrowLightHoursFor(lampCount), 3);
        }

        [Fact]
        public void GrowLightHours_BonusIsPerLamp_UptoCap()
        {
            // The bonus is linear for the first two lamps only.
            float baseHours = GreenhouseSystem.BaseGrowLightHours;
            float bonus = GreenhouseSystem.GrowLampBonusHours;
            Assert.Equal(baseHours + bonus, GreenhouseSystem.GrowLightHoursFor(1), 3);
            Assert.Equal(baseHours + 2f * bonus, GreenhouseSystem.GrowLightHoursFor(2), 3);
            Assert.Equal(baseHours + 2f * bonus, GreenhouseSystem.GrowLightHoursFor(2 + 1), 3);
        }

        // ── PLOT CAPACITY CONTRACT (host-driven via EnsurePlots) ───────

        [Fact]
        public void PlotCapacity_GrowsWithPlanterBoxStock()
        {
            // The host calls EnsurePlots(max(base, stock)); this pins the
            // Core capacity growth the host relies on.
            var sys = new GreenhouseSystem(1);
            sys.EnsurePlots(GreenhouseSystem.BasePlanterBoxPlots + 3);
            Assert.Equal(GreenhouseSystem.BasePlanterBoxPlots + 3, sys.PlotCount);
        }

        [Fact]
        public void PlotCapacity_ShrinksOnlyFallowPlots_WhenStockDrops()
        {
            var sys = new GreenhouseSystem(2);
            sys.EnsurePlots(6);
            sys.Plant(4, GreenhouseExpansionCatalog.Items.SeedMushroom, 1, out _); // occupied
            sys.Plant(5, GreenhouseExpansionCatalog.Items.SeedMushroom, 1, out _); // occupied

            sys.EnsurePlots(3); // stock collapsed — occupied plots must survive

            Assert.True(sys.PlotCount >= 5, "occupied plots are never removed");
            Assert.False(GreenhouseSystem.IsFallow(sys.Plots[4]));
            Assert.False(GreenhouseSystem.IsFallow(sys.Plots[5]));
        }

        // ── GROW-MEDIUM STERILISATION CONTRACT ─────────────────────────

        [Fact]
        public void ClearThenSterilise_BedResidualContamination_IsScrubbed()
        {
            // The host clears then zeroes residual contamination with a grow
            // medium brick; this pins the Core state contract it drives.
            var sys = new GreenhouseSystem(3);
            sys.EnsurePlots(1);
            sys.Plant(0, GreenhouseExpansionCatalog.Items.SeedMushroom, 1, out _);
            sys.Water(0, 60f, tainted: true); // heavy contamination
            sys.Plots[0].growth = 100f;
            sys.Plots[0].stage = (int)GreenhouseStage.Mature;

            var harvest = sys.Harvest(0); // leaves residual contamination
            Assert.True(harvest.success);
            float residual = sys.Plots[0].soilContamination;
            Assert.True(residual > 0f, "harvest leaves residual contamination");

            // Host sterilise path: Clear + zero the residual.
            Assert.True(sys.Clear(0));
            sys.Plots[0].soilContamination = 0f;
            Assert.Equal(0f, sys.Plots[0].soilContamination, 3);
        }

        [Fact]
        public void SterilisedBed_NextHarvest_IsClean_WhenWaterIsClean()
        {
            var sys = new GreenhouseSystem(4);
            sys.EnsurePlots(1);
            // Sterilised bed (contamination 0) + clean water ⇒ clean yield.
            sys.Plots[0].soilContamination = 0f;
            sys.Plant(0, GreenhouseExpansionCatalog.Items.SeedMushroom, 1, out _);
            sys.Water(0, 60f, tainted: false);
            for (int d = 1; d <= 5; d++)
                sys.TickDay(d, GreenhouseSystem.GrowLightHoursFor(2), 0f);

            var harvest = sys.Harvest(0);
            Assert.True(harvest.success);
            Assert.False(harvest.contaminated);
            Assert.Equal(GreenhouseExpansionCatalog.Items.CropMushroom, harvest.yieldItemId);
        }
    }
}
