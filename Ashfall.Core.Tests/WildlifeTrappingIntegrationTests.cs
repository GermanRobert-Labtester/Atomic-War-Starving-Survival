using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WildlifeTrappingIntegrationTests
    {
        [Fact]
        public void SetTrap_AndCheck_ResolvesCatch()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var set = sys.SetTrap("site_valley", "bait_grain", "dweller_hunter");
            Assert.True(set.IsSuccess);
            Assert.Single(sys.State.trapSites);

            var check = sys.CheckTraps();
            Assert.True(check.IsSuccess);
        }

        [Fact]
        public void SaveAndRestore_PreservesTrapSites()
        {
            var sys1 = new WildlifeTrappingSystem(new SeededRng(42));
            sys1.SetTrap("site_woods", "bait_scrap", "hunter_1");

            var state = sys1.CaptureState();
            var sys2 = new WildlifeTrappingSystem(new SeededRng(42));
            sys2.RestoreState(state);

            Assert.Single(sys2.State.trapSites);
            Assert.Equal("site_woods", sys2.State.trapSites[0].siteId);
            Assert.Equal("bait_scrap", sys2.State.trapSites[0].baitType);
        }
    }
}
