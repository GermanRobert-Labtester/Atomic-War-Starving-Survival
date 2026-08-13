using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Cross-tool review of the BrineWaterSystem extraction (Holdfast §5.2).
    /// Pins the B6 gate (no auto-trip from boot), the degradation curve, the
    /// steam-trip / cluster-cooling pipeline and the membrane resolutions.
    /// </summary>
    public class BrineWaterSystemTests
    {
        [Fact]
        public void NoDegradationBeforeUnlock()
        {
            var brine = new BrineWaterSystem();
            for (int d = 1; d <= 30; d++)
                brine.TickDaily(d, WeatherKind.Blizzard, -20f, outfallShifted: false);
            Assert.False(brine.Unlocked);
            Assert.Equal(72f, brine.MembraneIntegrity);
            Assert.False(brine.SteamTripped, "must not auto-trip around day 18 from boot (B6)");
        }

        [Fact]
        public void UnlockStartsDailyLoad()
        {
            var brine = new BrineWaterSystem();
            brine.Unlock();
            brine.TickDaily(1, WeatherKind.Clear, -5f, outfallShifted: false);
            Assert.Equal(72f - 3.2f, brine.MembraneIntegrity, 2);
        }

        [Fact]
        public void OutfallShiftedReducesLoad()
        {
            var brine = new BrineWaterSystem();
            brine.Unlock();
            brine.TickDaily(1, WeatherKind.Clear, -5f, outfallShifted: true);
            Assert.Equal(72f - 3.2f * 0.55f, brine.MembraneIntegrity, 2);
        }

        [Fact]
        public void FalseSpringAndIceStormIncreaseLoad()
        {
            var falseSpring = new BrineWaterSystem();
            falseSpring.Unlock();
            falseSpring.TickDaily(1, WeatherKind.FalseSpring, 3f, outfallShifted: false);
            Assert.Equal(72f - 3.2f * 1.15f, falseSpring.MembraneIntegrity, 2);

            var iceStorm = new BrineWaterSystem();
            iceStorm.Unlock();
            iceStorm.TickDaily(1, WeatherKind.IceStorm, -25f, outfallShifted: false);
            Assert.Equal(72f - 3.2f * 1.15f, iceStorm.MembraneIntegrity, 2);
        }

        [Fact]
        public void IntegrityClampsAtZero()
        {
            var brine = new BrineWaterSystem();
            brine.Unlock();
            for (int d = 1; d <= 60; d++)
                brine.TickDaily(d, WeatherKind.Blizzard, -20f, outfallShifted: false);
            Assert.Equal(0f, brine.MembraneIntegrity);
        }

        [Fact]
        public void SteamTripFiresBelowThreshold()
        {
            var brine = new BrineWaterSystem();
            brine.Unlock();
            bool tripped = false;
            brine.OnSteamTrip += () => tripped = true;

            int day = 1;
            while (day < 40 && !brine.SteamTripped)
                brine.TickDaily(day++, WeatherKind.Blizzard, -20f, outfallShifted: false);

            Assert.True(brine.SteamTripped);
            Assert.True(tripped);
            Assert.True(brine.MembraneIntegrity < BrineWaterSystem.SteamTripIntegrity);
            Assert.True(brine.State.steamTripDay > 0);
        }

        [Fact]
        public void ClusterCoolsTowardOutdoorFloorAfterTrip()
        {
            var brine = new BrineWaterSystem();
            brine.Unlock();
            int day = 1;
            while (day < 40 && !brine.SteamTripped)
                brine.TickDaily(day++, WeatherKind.Blizzard, -20f, outfallShifted: false);
            Assert.True(brine.SteamTripped);

            float before = brine.ClusterIndoorC;
            brine.TickDaily(day, WeatherKind.Blizzard, -20f, outfallShifted: false);
            // One tick past trip: hoursSinceTrip = 24 → t = 0.5 → 16 + (-20 - 16) * 0.5 = -2
            Assert.True(brine.ClusterIndoorC < before, "cluster keeps cooling after trip");
            Assert.Equal(16f + (-20f - 16f) * 0.5f, brine.ClusterIndoorC, 2);

            // Warm outdoor air still falls back to the -18 floor, never heats.
            brine.TickDaily(day + 1, WeatherKind.Rain, 4f, outfallShifted: false);
            Assert.True(brine.ClusterIndoorC <= 16f);
        }

        [Fact]
        public void RepairWithResinSavesMembraneAboveForty()
        {
            var brine = new BrineWaterSystem();
            brine.Unlock();
            int day = 1;
            while (day < 40 && !brine.SteamTripped)
                brine.TickDaily(day++, WeatherKind.Blizzard, -20f, outfallShifted: false);

            Assert.True(brine.RepairWithResin(3));
            Assert.False(brine.SteamTripped, "three drums push integrity past 40 → membrane saved");
            Assert.True(brine.State.membraneSaved);
            Assert.Equal(14f, brine.ClusterIndoorC);
        }

        [Fact]
        public void RepairWithResinRejectsZeroOrNegative()
        {
            var brine = new BrineWaterSystem();
            brine.Unlock();
            Assert.False(brine.RepairWithResin(0));
            Assert.False(brine.RepairWithResin(-1));
        }

        [Fact]
        public void ResolveMembraneStripSector4()
        {
            var brine = new BrineWaterSystem();
            brine.Unlock();
            int day = 1;
            while (day < 40 && !brine.SteamTripped)
                brine.TickDaily(day++, WeatherKind.Blizzard, -20f, outfallShifted: false);

            brine.ResolveMembraneStripSector4();
            Assert.True(brine.State.membraneSector4Strip);
            Assert.False(brine.State.membraneLetDrop);
            Assert.False(brine.SteamTripped, "sector-4 strip = four resin drums");
        }

        [Fact]
        public void ResolveMembraneLetDrop()
        {
            var brine = new BrineWaterSystem();
            brine.Unlock();
            brine.ResolveMembraneLetDrop();
            Assert.True(brine.State.membraneLetDrop);
            Assert.False(brine.State.membraneSaved);
        }

        [Fact]
        public void UnlockSaltTradeAlsoUnlocks()
        {
            var brine = new BrineWaterSystem();
            brine.UnlockSaltTrade();
            Assert.True(brine.Unlocked);
            Assert.True(brine.State.saltTradeUnlocked);
        }

        [Fact]
        public void HaulCleanWaterSouthLosesQuarter()
        {
            var brine = new BrineWaterSystem();
            Assert.Equal(7.5f, brine.HaulCleanWaterSouth(10f), 2);
            Assert.Equal(0f, brine.HaulCleanWaterSouth(0f));
            Assert.Equal(0f, brine.HaulCleanWaterSouth(-5f));
        }

        [Fact]
        public void SaveRoundTripPreservesTrippedPipeline()
        {
            var brine = new BrineWaterSystem();
            brine.Unlock();
            int day = 1;
            while (day < 40 && !brine.SteamTripped)
                brine.TickDaily(day++, WeatherKind.Blizzard, -20f, outfallShifted: false);

            var json = new SystemTextJsonSerializer();
            var restored = new BrineWaterSystem();
            restored.RestoreState(json.Deserialize<BrineWaterSystemState>(json.Serialize(brine.CaptureState())));
            Assert.True(restored.Unlocked);
            Assert.True(restored.SteamTripped);
            Assert.Equal(brine.MembraneIntegrity, restored.MembraneIntegrity, 2);
            Assert.Equal(brine.State.steamTripDay, restored.State.steamTripDay);
            Assert.Equal(brine.ClusterIndoorC, restored.ClusterIndoorC, 2);
        }
    }
}
