using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WaystationIntegrationTests
    {
        [Fact]
        public void Unlock_AndAssignWatch_UpdatesOutpost()
        {
            var sys = new WaystationSystem();
            Assert.False(sys.Unlocked);

            sys.Unlock();
            Assert.True(sys.Unlocked);
            Assert.True(sys.StoveLit);

            bool watch = sys.AssignWatch(new[] { "dweller_scout_1", "dweller_scout_2" });
            Assert.True(watch);
            Assert.Equal(2, sys.State.watchSurvivorIds.Length);
        }

        [Fact]
        public void DailyTick_DegradesFilter_AndTracksResupply()
        {
            var sys = new WaystationSystem();
            sys.Unlock();
            sys.TickDaily(iceRoadOpen: true);

            Assert.True(sys.State.filterHealth < 100f);
            Assert.Equal(1, sys.State.daysSinceResupply);
        }

        [Fact]
        public void SaveAndRestore_PreservesWaystationState()
        {
            var sys1 = new WaystationSystem();
            sys1.Unlock();
            sys1.AssignWatch(new[] { "scout_a" });
            sys1.TickDaily(iceRoadOpen: true);

            var state = sys1.CaptureState();
            var sys2 = new WaystationSystem();
            sys2.RestoreState(state);

            Assert.True(sys2.Unlocked);
            Assert.Single(sys2.State.watchSurvivorIds);
            Assert.Equal(sys1.State.filterHealth, sys2.State.filterHealth);
        }
    }
}
