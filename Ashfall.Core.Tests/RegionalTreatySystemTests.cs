using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class RegionalTreatySystemTests
    {
        [Fact] public void Propose_CreatesTreaty()
        {
            var rt = Create();
            rt.LoadCatalog(MakeTreaties());
            var r = rt.Propose("road_iron_charter");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(rt.State.treaties);
        }

        [Fact] public void Propose_Duplicate_Blocks()
        {
            var rt = Create();
            rt.LoadCatalog(MakeTreaties());
            rt.Propose("road_iron_charter");
            var r = rt.Propose("road_iron_charter");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void Ratify_ChangesStatus()
        {
            var rt = Create();
            rt.LoadCatalog(MakeTreaties());
            rt.Propose("road_iron_charter");
            var r = rt.Ratify("road_iron_charter", 50);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.True(rt.IsActive("road_iron_charter"));
        }

        [Fact] public void Ratify_WithoutProposal_Fails()
        {
            var rt = Create();
            var r = rt.Ratify("road_iron_charter", 50);
            Assert.Equal(ActionResult.StatusKind.Failed, r.Status);
        }

        [Fact] public void IsActive_AfterRatify_True()
        {
            var rt = Create();
            rt.LoadCatalog(MakeTreaties());
            rt.Propose("road_iron_charter");
            rt.Ratify("road_iron_charter", 50);
            Assert.True(rt.IsActive("road_iron_charter"));
        }

        [Fact] public void IsActive_Unratified_False()
        {
            var rt = Create();
            Assert.False(rt.IsActive("road_iron_charter"));
        }

        [Fact] public void GetActiveEffects_ReturnsEffects()
        {
            var rt = Create();
            rt.LoadCatalog(MakeTreaties());
            rt.Propose("road_iron_charter");
            rt.Ratify("road_iron_charter", 50);
            var effects = rt.GetActiveEffects();
            Assert.NotEmpty(effects);
        }

        [Fact] public void CaptureRestoreState_PreservesTreaties()
        {
            var rt = Create();
            rt.LoadCatalog(MakeTreaties());
            rt.Propose("road_iron_charter");
            var state = rt.CaptureState();
            Assert.Single(state.treaties);

            var rt2 = Create();
            rt2.RestoreState(state);
            Assert.Single(rt2.State.treaties);
        }

        private static RegionalTreatySystem Create() => new RegionalTreatySystem();

        private static System.Collections.Generic.List<TreatyDefinition> MakeTreaties()
        {
            return new System.Collections.Generic.List<TreatyDefinition>
            {
                new TreatyDefinition
                {
                    treaty_id = "road_iron_charter", display_name = "Road Iron Charter",
                    faction_id = "hydro_barons", ratification_cost_scrap = 30,
                    effects = new System.Collections.Generic.List<TreatyEffect>
                    {
                        new TreatyEffect { effect_type = "economy_discount", value = 0.1f },
                        new TreatyEffect { effect_type = "route_access", target_id = "coastal_route" }
                    }
                }
            };
        }
    }
}
