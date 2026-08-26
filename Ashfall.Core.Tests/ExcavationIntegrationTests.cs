using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ExcavationIntegrationTests
    {
        [Fact]
        public void AddSite_AssignWorkers_AndTick_AdvancesProgress()
        {
            var sys = new ExcavationSystem(new SeededRng(42));
            var add = sys.AddSite("site_delta", "blueprint_room_a", 50f, 0.1f);
            Assert.True(add.IsSuccess);

            var assign = sys.AssignWorkers("site_delta", 3);
            Assert.True(assign.IsSuccess);

            sys.TickDay();
            Assert.True(sys.State.sites[0].progress > 0f);
        }

        [Fact]
        public void ApplyShoring_ReducesRisk()
        {
            var sys = new ExcavationSystem(new SeededRng(42));
            sys.AddSite("site_hazard", "blueprint_vault", 100f, 0.4f);
            var shore = sys.ApplyShoring("site_hazard");

            Assert.True(shore.IsSuccess);
            Assert.True(sys.State.sites[0].shoringApplied);
            Assert.True(sys.State.sites[0].structuralRisk < 0.4f);
        }

        [Fact]
        public void SaveAndRestore_PreservesSitesAndProgress()
        {
            var sys1 = new ExcavationSystem(new SeededRng(42));
            sys1.AddSite("site_1", "blueprint_storage", 80f, 0.2f);
            sys1.AssignWorkers("site_1", 2);
            sys1.TickDay();

            var state = sys1.CaptureState();
            var sys2 = new ExcavationSystem(new SeededRng(42));
            sys2.RestoreState(state);

            Assert.Single(sys2.State.sites);
            Assert.Equal("site_1", sys2.State.sites[0].siteId);
            Assert.Equal(sys1.State.sites[0].progress, sys2.State.sites[0].progress);
        }
    }
}
