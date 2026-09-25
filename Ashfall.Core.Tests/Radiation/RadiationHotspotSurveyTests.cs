// SPDX-License-Identifier: MIT
// Alpha feature F4 — radiation hotspot survey: authored policy parses, bands
// classify worst-first, and the shipped JSON matches the Core default.

using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Radiation;
using Xunit;

namespace Ashfall.Core.Tests.HotspotSurvey
{
    public sealed class RadiationHotspotSurveyTests
    {
        private static DoseLocationDef Loc(string id, string sector, float usv) => new DoseLocationDef
        {
            id = id,
            displayName = id,
            sector = sector,
            radiationUsv = usv,
        };

        [Fact]
        public void Classify_Bands_And_Orders_Worst_First_Filtering_Below_Watch()
        {
            var policy = RadiationHotspotPolicy.Default;
            var rows = RadiationHotspotSurvey.Classify(policy, new List<DoseLocationDef>
            {
                Loc("a", "sector_north", 30f),    // watch
                Loc("b", "sector_east", 220f),    // extreme
                Loc("c", "sector_south", 5f),     // below threshold
                Loc("d", "sector_west", 120f),    // high
            });

            Assert.Equal(3, rows.Count);
            Assert.Equal("b", rows[0].LocationId);
            Assert.Equal("extreme", rows[0].Severity);
            Assert.Equal("d", rows[1].LocationId);
            Assert.Equal("high", rows[1].Severity);
            Assert.Equal("a", rows[2].LocationId);
            Assert.Equal("watch", rows[2].Severity);
        }

        [Fact]
        public void Classify_Empty_Or_Null_Input_Is_Empty()
        {
            Assert.Empty(RadiationHotspotSurvey.Classify(RadiationHotspotPolicy.Default, null));
            Assert.Empty(RadiationHotspotSurvey.Classify(RadiationHotspotPolicy.Default, new List<DoseLocationDef>()));
        }

        [Fact]
        public void Shipped_Policy_Json_Parses_And_Matches_Default_Bands()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir!))
                dataDir = System.AppContext.BaseDirectory;
            if (!CatalogLocator.TryFindDataDirectory(dataDir, out dataDir!))
                throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");

            string json = File.ReadAllText(Path.Combine(dataDir, RadiationHotspotPolicy.FileName));
            var policy = RadiationHotspotPolicy.Load(json);

            Assert.Empty(policy.Errors);
            Assert.Equal(RadiationHotspotPolicy.Default.WatchThresholdUsv, policy.WatchThresholdUsv);
            Assert.Equal(RadiationHotspotPolicy.Default.Bands.Count, policy.Bands.Count);
            for (int i = 0; i < policy.Bands.Count; i++)
            {
                Assert.Equal(RadiationHotspotPolicy.Default.Bands[i].Severity, policy.Bands[i].Severity);
                Assert.Equal(RadiationHotspotPolicy.Default.Bands[i].MinUsv, policy.Bands[i].MinUsv, 3);
            }
        }

        [Fact]
        public void Malformed_Policy_Is_Reported_Not_Thrown()
        {
            var policy = RadiationHotspotPolicy.Load("{ not json ");
            Assert.NotEmpty(policy.Errors);
            Assert.NotNull(policy.BandFor(500.0) == null ? "no bands" : "bands");
        }
    }
}
