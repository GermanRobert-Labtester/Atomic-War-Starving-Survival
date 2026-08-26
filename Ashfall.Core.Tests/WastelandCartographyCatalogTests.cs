using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class WastelandCartographyCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public WastelandCartographyCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void WastelandCartographyCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = WastelandCartographyCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.TopoEntries.Count);
            Assert.Equal(8, catalog.RouteEntries.Count);
            Assert.Equal(7, catalog.MudflowEntries.Count);
            Assert.Equal(7, catalog.LimnologyEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void WastelandCartographyCatalog_TopoSheets_Integrity()
        {
            var catalog = WastelandCartographyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.TopoEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("topo_sheet_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.QuadrangleName));
                Assert.False(string.IsNullOrWhiteSpace(item.GridScaleRatio));
                Assert.True(item.PeakGammaFieldRHr >= 0);
                Assert.False(string.IsNullOrWhiteSpace(item.DominantTerrainFeature));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetTopoSheet("topo_sheet_ground_zero_trinitite_basin");
            Assert.NotNull(entry);
            Assert.Equal("GROUND_ZERO_CRATER_BASIN_QUAD", entry.QuadrangleName);
        }

        [Fact]
        public void WastelandCartographyCatalog_Routes_Integrity()
        {
            var catalog = WastelandCartographyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.RouteEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("scav_route_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.RouteIdentifier));
                Assert.False(string.IsNullOrWhiteSpace(item.LeadScoutName));
                Assert.False(string.IsNullOrWhiteSpace(item.HazardLevel));
                Assert.True(item.DistanceKilometers > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetRouteNote("scav_route_bridge_toll_ambush_point");
            Assert.NotNull(entry);
            Assert.Equal("EXPEDITION_ROUTE_HIGHWAY_12", entry.RouteIdentifier);
        }

        [Fact]
        public void WastelandCartographyCatalog_Mudflows_Integrity()
        {
            var catalog = WastelandCartographyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.MudflowEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("mudflow_report_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.CanyonLocationId));
                Assert.True(item.EstimatedSlurryVolumeM3 > 0);
                Assert.True(item.FlowVelocityKmh > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.StructuralImpactSeverity));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetMudflow("mudflow_report_ash_slurry_debris_torrent");
            Assert.NotNull(entry);
            Assert.Equal("BLIND_GULCH_UPPER_CHUTE", entry.CanyonLocationId);
        }

        [Fact]
        public void WastelandCartographyCatalog_Limnology_Integrity()
        {
            var catalog = WastelandCartographyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.LimnologyEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("crater_lake_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.CraterLakeName));
                Assert.True(item.MaximumDepthMeters > 0);
                Assert.True(item.BottomLayerDissolvedH2sPpm > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.StratificationType));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetLimnology("crater_lake_meromictic_anoxic_bottom_stratum");
            Assert.NotNull(entry);
            Assert.Equal("GROUND_ZERO_MEROMICTIC_LAKE", entry.CraterLakeName);
        }
    }
}
