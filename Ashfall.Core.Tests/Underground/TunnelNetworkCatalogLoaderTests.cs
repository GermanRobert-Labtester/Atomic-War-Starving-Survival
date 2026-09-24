// SPDX-License-Identifier: MIT
// Plan 167 — strict tunnel catalog loader, census, and schema-gate tests.

using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Underground;
using Xunit;

namespace Ashfall.Core.Tests.Underground
{
    public sealed class TunnelNetworkCatalogLoaderTests
    {
        private static string RepoRoot()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "Ashfall.csproj")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found");
        }

        private static string AuthoredJson() =>
            File.ReadAllText(Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data", "underground_tunnels.json"));

        [Fact]
        public void Authored_Catalog_Loads_And_Registers()
        {
            var catalog = TunnelNetworkCatalogLoader.LoadFromJson(AuthoredJson());
            Assert.True(catalog.junctions.Count >= 3);
            Assert.True(catalog.segments.Count >= 3);

            var system = new TunnelNetworkSystem();
            system.LoadCatalog(catalog);
            Assert.Equal(catalog.segments.Count, system.TotalSegmentCount);
            Assert.Equal(catalog.junctions.Count, system.JunctionCount);
        }

        [Theory]
        [InlineData("{\"schema_version\":1,\"junctions\":[{\"id\":\"j1\",\"name\":\"J1\"}],\"segments\":[{\"id\":\"s1\",\"name\":\"S1\",\"from\":\"a\",\"to\":\"b\"},{\"id\":\"s1\",\"name\":\"S1b\",\"from\":\"a\",\"to\":\"c\"}]}")]
        [InlineData("{\"schema_version\":1,\"junctions\":[{\"id\":\"j1\",\"name\":\"J1\"}],\"segments\":[{\"id\":\"s1\",\"name\":\"S1\",\"from\":\"a\",\"to\":\"b\",\"hazards\":[\"Dragonfire\"]}]}")]
        [InlineData("{\"schema_version\":1,\"junctions\":[{\"id\":\"j1\",\"name\":\"J1\",\"connected_segments\":[\"ghost\"]}],\"segments\":[{\"id\":\"s1\",\"name\":\"S1\",\"from\":\"a\",\"to\":\"b\"}]}")]
        [InlineData("{\"schema_version\":1,\"junctions\":[{\"id\":\"j1\",\"name\":\"J1\"}],\"segments\":[{\"id\":\"s1\",\"name\":\"S1\",\"from\":\"a\",\"to\":\"b\",\"length_hours\":0.1}]}")]
        [InlineData("{\"schema_version\":1,\"junctions\":[{\"id\":\"j1\",\"name\":\"J1\"}],\"segments\":[{\"id\":\"s1\",\"name\":\"S1\",\"from\":\"a\",\"to\":\"b\",\"difficulty\":9}]}")]
        [InlineData("{\"schema_version\":1,\"junctions\":[{\"id\":\"j1\",\"name\":\"J1\"}],\"segments\":[{\"id\":\"s1\",\"name\":\"S1\",\"from\":\"a\",\"to\":\"b\",\"integrity\":150}]}")]
        [InlineData("{\"schema_version\":2,\"junctions\":[{\"id\":\"j1\",\"name\":\"J1\"}],\"segments\":[{\"id\":\"s1\",\"name\":\"S1\",\"from\":\"a\",\"to\":\"b\"}]}")]
        public void Malformed_Catalog_Is_Rejected(string json)
        {
            Assert.Throws<InvalidOperationException>(() => TunnelNetworkCatalogLoader.LoadFromJson(json));
        }

        [Fact]
        public void Census_Reports_Truthful_Counts()
        {
            var catalog = TunnelNetworkCatalogLoader.LoadFromJson(AuthoredJson());
            var system = new TunnelNetworkSystem();
            system.LoadCatalog(catalog);

            var first = system.CaptureState().Segments.First();
            system.DiscoverSegment(first.SegmentId);
            var hazardous = system.CaptureState().Segments.First(s => s.Hazards.Count > 0);

            var census = system.GetCensus();
            Assert.Equal(system.TotalSegmentCount, census.TotalSegments);
            Assert.Equal(system.JunctionCount, census.TotalJunctions);
            Assert.Equal(1, census.DiscoveredSegments);
            Assert.True(census.HazardousSegments >= 1);
            Assert.True(census.AverageIntegrity > 0f);
        }

        [Fact]
        public void RestoreState_Rejects_Newer_Schema_And_Accepts_Legacy()
        {
            var system = new TunnelNetworkSystem();
            var state = system.CaptureState();

            var newer = system.CaptureState();
            newer.SchemaVersion = 2;
            Assert.Throws<InvalidOperationException>(() => system.RestoreState(newer));

            var legacy = system.CaptureState();
            legacy.SchemaVersion = 0;
            system.RestoreState(legacy); // legacy payload is accepted and normalized
            Assert.Equal(1, system.CaptureState().SchemaVersion);
        }

        [Fact]
        public void DailyTick_Degrades_And_Collapses()
        {
            var catalog = TunnelNetworkCatalogLoader.LoadFromJson(AuthoredJson());
            var system = new TunnelNetworkSystem();
            system.LoadCatalog(catalog);

            var segment = system.FindSegment(catalog.segments[0].id)!;
            segment.StructuralIntegrity = 0.5f;
            system.TickDay(3);
            Assert.Equal(TunnelStatus.Collapsed, segment.Status);
        }
    }
}
