using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Cross-tool review of the WaystationSystem extraction (Holdfast §5.4).
    /// One forward camp: watch cap, filter burn, stove notches, resupply,
    /// wintering, save roundtrip.
    /// </summary>
    public class WaystationSystemTests
    {
        [Fact]
        public void LockedUntilUnlock()
        {
            var way = new WaystationSystem();
            Assert.False(way.Unlocked);
            Assert.False(way.AssignWatch(new[] { "sv_one" }));
            float before = way.State.filterHealth;
            way.TickDaily(iceRoadOpen: true);
            Assert.Equal(before, way.State.filterHealth, 2);
            Assert.Equal(0, way.State.daysSinceResupply);
        }

        [Fact]
        public void UnlockLightsStoveAndRaisesEvent()
        {
            var way = new WaystationSystem();
            bool unlocked = false;
            way.OnUnlocked += () => unlocked = true;
            way.Unlock();
            way.Unlock(); // idempotent
            Assert.True(way.Unlocked);
            Assert.True(way.StoveLit);
            Assert.True(unlocked);
        }

        [Fact]
        public void AssignWatchCapsAtTwoAndTracksBunks()
        {
            var way = new WaystationSystem();
            way.Unlock();
            Assert.True(way.AssignWatch(new[] { "sv_one", "sv_two", "sv_three", "sv_four" }));
            Assert.Equal(2, way.State.watchSurvivorIds.Length);
            Assert.Equal(2, way.State.bunksOccupied);
            Assert.True(way.AssignWatch(new[] { "sv_solo" }));
            Assert.Single(way.State.watchSurvivorIds);
            Assert.Equal(1, way.State.bunksOccupied);
        }

        [Fact]
        public void TickBurnsFilterFasterWhenRoadClosed()
        {
            var open = new WaystationSystem();
            open.Unlock();
            open.TickDaily(iceRoadOpen: true);
            Assert.Equal(100f - 4f * 1.4f, open.State.filterHealth, 2);

            var closed = new WaystationSystem();
            closed.Unlock();
            closed.TickDaily(iceRoadOpen: false);
            Assert.Equal(100f - 4f * 1.4f * 1.1f, closed.State.filterHealth, 2);
        }

        [Fact]
        public void StoveDiesAfterElevenNotches()
        {
            var way = new WaystationSystem();
            way.Unlock();
            bool died = false;
            way.OnStoveDied += () => died = true;

            for (int d = 0; d < WaystationSystem.FilterWindowNotches; d++)
                way.TickDaily(iceRoadOpen: true);
            Assert.True(way.StoveLit, "stove survives exactly the notch window");

            way.TickDaily(iceRoadOpen: true);
            Assert.False(way.StoveLit, "dies past the notch window");
            Assert.True(died);
        }

        [Fact]
        public void ResupplyResetsAndRelights()
        {
            var way = new WaystationSystem();
            way.Unlock();
            for (int d = 0; d < 20; d++)
                way.TickDaily(iceRoadOpen: true);
            Assert.False(way.StoveLit);
            float worn = way.State.filterHealth;

            way.Resupply();
            Assert.True(way.StoveLit);
            Assert.Equal(0, way.State.daysSinceResupply);
            Assert.Equal(worn + 40f, way.State.filterHealth, 2);
            Assert.True(way.State.filterHealth <= 100f);
        }

        [Fact]
        public void WinteringRelightsStove()
        {
            var way = new WaystationSystem();
            way.Unlock();
            for (int d = 0; d < 20; d++)
                way.TickDaily(iceRoadOpen: true);
            Assert.False(way.StoveLit);

            way.SetWintering(true);
            Assert.True(way.StoveLit);
            Assert.True(way.State.winteringClosedWindow);

            way.SetWintering(false);
            Assert.False(way.State.winteringClosedWindow);
        }

        [Fact]
        public void SaveRoundTripPreservesWatchAndFilter()
        {
            var way = new WaystationSystem();
            way.Unlock();
            way.AssignWatch(new[] { "sv_one", "sv_two" });
            for (int d = 0; d < 5; d++)
                way.TickDaily(iceRoadOpen: false);
            way.SetWintering(true);

            var json = new SystemTextJsonSerializer();
            var restored = new WaystationSystem();
            restored.RestoreState(json.Deserialize<WaystationSystemState>(json.Serialize(way.CaptureState())));

            Assert.True(restored.Unlocked);
            Assert.Equal(2, restored.State.watchSurvivorIds.Length);
            Assert.Equal(way.State.filterHealth, restored.State.filterHealth, 2);
            Assert.Equal(way.State.daysSinceResupply, restored.State.daysSinceResupply);
            Assert.True(restored.State.winteringClosedWindow);
        }

        [Fact]
        public void StateChangedFiresOnMutations()
        {
            var way = new WaystationSystem();
            int changed = 0;
            way.OnStateChanged += _ => changed++;
            way.Unlock();
            way.AssignWatch(new[] { "sv_one" });
            way.TickDaily(iceRoadOpen: true);
            way.Resupply();
            Assert.True(changed >= 4);
        }
    }
}
