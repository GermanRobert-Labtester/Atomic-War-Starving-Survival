using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WildlifeTrappingSystemTests
    {
        [Fact] public void SetTrap_CreatesSite()
        {
            var wt = Create();
            var r = wt.SetTrap("perimeter_north", "meat_scraps", "hunter_1");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(wt.State.trapSites);
        }

        [Fact] public void CheckTraps_WhenSet_MayCatch()
        {
            var wt = Create();
            wt.SetTrap("perimeter_north", "meat_scraps", "hunter_1");
            var r = wt.CheckTraps();
            Assert.True(r.Status == ActionResult.StatusKind.Success);
        }

        [Fact] public void Butcher_AfterCatch_Processes()
        {
            var wt = Create();
            wt.SetTrap("perimeter_north", "meat_scraps", "hunter_1");
            wt.TickDay(5); // advances time and checks traps

            var site = wt.State.trapSites[0];
            if (site.hasCatch)
            {
                var r = wt.Butcher("perimeter_north");
                Assert.Equal(ActionResult.StatusKind.Success, r.Status);
                Assert.True(site.isMeatProcessed);
            }
        }

        [Fact] public void Butcher_WithoutCatch_Blocks()
        {
            var wt = Create();
            var r = wt.Butcher("perimeter_north");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void RemoveToxin_OnlyWhenToxic()
        {
            var wt = Create();
            wt.SetTrap("perimeter_north", "meat_scraps", "hunter_1");
            // Force a catch to test
            var site = wt.State.trapSites[0];
            site.hasCatch = true;
            site.isToxic = true;

            var r = wt.RemoveToxin("perimeter_north");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.True(site.toxinRemoved);
        }

        [Fact] public void CaptureRestoreState_PreservesTraps()
        {
            var wt = Create();
            wt.SetTrap("perimeter_north", "meat_scraps", "hunter_1");
            var state = wt.CaptureState();
            Assert.Single(state.trapSites);

            var wt2 = Create();
            wt2.RestoreState(state);
            Assert.Single(wt2.State.trapSites);
        }

        private static WildlifeTrappingSystem Create() => new WildlifeTrappingSystem(new SeededRng(42));
    }
}
