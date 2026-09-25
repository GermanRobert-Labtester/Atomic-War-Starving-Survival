// SPDX-License-Identifier: MIT
// Alpha feature G4 — pre-departure route dose check: worst band, safest
// alternative, hotspot count, and summed baseline reading.

using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Radiation;
using Xunit;

namespace Ashfall.Core.Tests.RouteDose
{
    public sealed class RouteDoseCheckTests
    {
        private static DoseLocationDef Loc(string id, string sector, float usv) => new DoseLocationDef
        {
            id = id, displayName = id, sector = sector, radiationUsv = usv,
        };

        [Fact]
        public void Evaluate_Names_Worst_Band_And_Safest_Alternative()
        {
            var check = RouteDoseCheckService.Evaluate(RadiationHotspotPolicy.Default, new List<DoseLocationDef>
            {
                Loc("depot", "surface", 12f),
                Loc("crater", "surface", 260f),
                Loc("gully", "surface", 55f),
            });

            Assert.False(check.Clear);
            Assert.Equal("crater", check.Worst!.Value.LocationId);
            Assert.Equal("extreme", check.Worst.Value.Severity);
            Assert.Equal("depot", check.Safest!.Value.LocationId);
            Assert.Equal(2, check.HotspotCount);
            Assert.Equal(327.0, check.TotalUsv, 3);
        }

        [Fact]
        public void Evaluate_Clean_Route_Reports_Clear_With_Safest()
        {
            var check = RouteDoseCheckService.Evaluate(RadiationHotspotPolicy.Default, new List<DoseLocationDef>
            {
                Loc("bunker_a", "bunker", 3f),
                Loc("bunker_b", "bunker", 8f),
            });

            Assert.True(check.Clear);
            Assert.Equal(0, check.HotspotCount);
            Assert.Equal("bunker_a", check.Safest!.Value.LocationId);
        }

        [Fact]
        public void Evaluate_Empty_Or_Null_Input_Is_Clear_And_Empty()
        {
            Assert.True(RouteDoseCheckService.Evaluate(RadiationHotspotPolicy.Default, null).Clear);
            Assert.True(RouteDoseCheckService.Evaluate(RadiationHotspotPolicy.Default, new List<DoseLocationDef>()).Clear);
            Assert.Equal(0, RouteDoseCheckService.Evaluate(RadiationHotspotPolicy.Default, new List<DoseLocationDef>()).TotalUsv);
        }
    }
}
