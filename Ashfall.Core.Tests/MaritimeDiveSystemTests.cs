using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class MaritimeDiveSystemTests
    {
        [Fact] public void RegisterSite_CreatesSite()
        {
            var md = Create();
            var r = md.RegisterSite("site_reef", "Sunken Reef", 30f, 0.2f);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(md.State.sites);
        }

        [Fact] public void RegisterSite_Duplicate_Blocks()
        {
            var md = Create();
            md.RegisterSite("site_reef", "Sunken Reef", 30f, 0.2f);
            var r = md.RegisterSite("site_reef", "Sunken Reef", 30f, 0.2f);
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void ConductDive_ReturnsOutcome()
        {
            var md = Create();
            md.RegisterSite("site_reef", "Sunken Reef", 30f, 0.1f);
            var r = md.ConductDive("site_reef", "diver_1", 0.9f);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(md.State.outcomes);
        }

        [Fact] public void ConductDive_UnknownSite_Fails()
        {
            var md = Create();
            var r = md.ConductDive("nonexistent", "diver_1", 0.9f);
            Assert.Equal(ActionResult.StatusKind.Failed, r.Status);
        }

        [Fact] public void ConductDive_MarksSiteExplored()
        {
            var md = Create();
            md.RegisterSite("site_reef", "Sunken Reef", 30f, 0.1f);
            md.ConductDive("site_reef", "diver_1", 0.9f);
            Assert.True(md.State.sites[0].isExplored);
        }

        [Fact] public void ConductDive_TracksRadiationDose()
        {
            var md = Create();
            md.RegisterSite("site_hot", "Hot Zone Wreck", 50f, 0.8f);
            md.ConductDive("site_hot", "diver_1", 0.5f);
            var outcome = md.State.outcomes[0];
            Assert.True(outcome.radiationDose > 0);
        }

        [Fact] public void CaptureRestoreState_PreservesSites()
        {
            var md = Create();
            md.RegisterSite("site_reef", "Sunken Reef", 30f, 0.2f);
            md.ConductDive("site_reef", "diver_1", 0.9f);
            var state = md.CaptureState();
            Assert.Single(state.sites);
            Assert.Single(state.outcomes);

            var md2 = Create();
            md2.RestoreState(state);
            Assert.Single(md2.State.sites);
            Assert.Single(md2.State.outcomes);
        }

        private static MaritimeDiveSystem Create() => new MaritimeDiveSystem(new SeededRng(42));
    }
}
