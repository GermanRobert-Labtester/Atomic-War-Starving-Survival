using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ExcavationSystemTests
    {
        [Fact] public void AddSite_CreatesSite()
        {
            var ex = Create();
            var r = ex.AddSite("site_a", "room_cellar", 100f, 0.3f);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(ex.State.sites);
        }

        [Fact] public void AddSite_Duplicate_Blocks()
        {
            var ex = Create();
            ex.AddSite("site_a", "room_cellar", 100f, 0.3f);
            var r = ex.AddSite("site_a", "room_cellar", 100f, 0.3f);
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void AssignWorkers_EnablesProgress()
        {
            var ex = Create();
            ex.AddSite("site_a", "room_cellar", 100f, 0.3f);
            ex.AssignWorkers("site_a", 2);
            ex.TickDay();
            var site = ex.State.sites[0];
            Assert.True(site.progress > 0);
        }

        [Fact] public void ApplyShoring_ReducesRisk()
        {
            var ex = Create();
            ex.AddSite("site_a", "room_cellar", 100f, 0.5f);
            ex.ApplyShoring("site_a");
            Assert.True(ex.State.sites[0].structuralRisk < 0.5f);
        }

        [Fact] public void Site_Completes_WhenProgressMet()
        {
            var ex = Create();
            ex.AddSite("site_a", "room_cellar", 10f, 0f); // low requirement, no risk
            ex.AssignWorkers("site_a", 3);
            ex.TickDay();
            Assert.True(ex.State.sites[0].isComplete);
        }

        [Fact] public void CaptureRestoreState_PreservesSites()
        {
            var ex = Create();
            ex.AddSite("site_a", "room_cellar", 100f, 0.3f);
            ex.AssignWorkers("site_a", 2);
            var state = ex.CaptureState();
            Assert.Single(state.sites);

            var ex2 = Create();
            ex2.RestoreState(state);
            Assert.Single(ex2.State.sites);
            Assert.Equal(2, ex2.State.sites[0].assignedWorkerCount);
        }

        private static ExcavationSystem Create() => new ExcavationSystem(new SeededRng(42));
    }
}
