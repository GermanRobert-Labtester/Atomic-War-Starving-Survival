using System.Collections.Generic;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class RegionalTreatyIntegrationTests
    {
        [Fact]
        public void ProposeAndRatify_TreatyAdvancesStatus()
        {
            var sys = new RegionalTreatySystem();
            sys.LoadCatalog(new List<TreatyDefinition>
            {
                new TreatyDefinition
                {
                    treaty_id = "treaty_meridian_accord",
                    display_name = "Meridian Peace Accord",
                    faction_id = "faction_meridian",
                    ratification_cost_scrap = 50f,
                    compliance_check_interval_days = 30f
                }
            });

            var prop = sys.Propose("treaty_meridian_accord");
            Assert.True(prop.IsSuccess);
            Assert.Equal(TreatyStatus.Proposed, sys.State.treaties[0].status);

            var rat = sys.Ratify("treaty_meridian_accord", 50);
            Assert.True(rat.IsSuccess);
            Assert.Equal(TreatyStatus.Ratified, sys.State.treaties[0].status);
        }

        [Fact]
        public void SaveAndRestore_PreservesTreaties()
        {
            var sys1 = new RegionalTreatySystem();
            sys1.LoadCatalog(new List<TreatyDefinition>
            {
                new TreatyDefinition { treaty_id = "treaty_non_aggression", ratification_cost_scrap = 10f }
            });
            sys1.Propose("treaty_non_aggression");
            sys1.Ratify("treaty_non_aggression", 10);

            var state = sys1.CaptureState();
            var sys2 = new RegionalTreatySystem();
            sys2.RestoreState(state);

            Assert.Single(sys2.State.treaties);
            Assert.Equal("treaty_non_aggression", sys2.State.treaties[0].treatyId);
            Assert.Equal(TreatyStatus.Ratified, sys2.State.treaties[0].status);
        }
    }
}
