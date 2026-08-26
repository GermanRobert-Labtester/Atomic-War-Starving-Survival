using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class CordageCableCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public CordageCableCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void CordageCableCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = CordageCableCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.HempEntries.Count);
            Assert.Equal(8, catalog.WireEntries.Count);
            Assert.Equal(7, catalog.HawserEntries.Count);
            Assert.Equal(7, catalog.SpliceEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void CordageCableCatalog_Hemp_Integrity()
        {
            var catalog = CordageCableCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.HempEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("hemp_fiber_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.RettingFloorId));
                Assert.False(string.IsNullOrWhiteSpace(item.RawStalkCropOrigin));
                Assert.True(item.FiberTensileTenacityCnTex > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetHemp("hemp_fiber_dew_retting_pectin_breakdown");
            Assert.NotNull(entry);
            Assert.Equal("MUSHROOM_CAVERN_RETTING_BED_01", entry.RettingFloorId);
        }

        [Fact]
        public void CordageCableCatalog_Wire_Integrity()
        {
            var catalog = CordageCableCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.WireEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("wire_rope_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.CableSpoolIdentifier));
                Assert.False(string.IsNullOrWhiteSpace(item.WireRopeConstruction));
                Assert.True(item.NominalDiameterMm > 0);
                Assert.True(item.BreakingStrengthMetricTons > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetWire("wire_rope_6x19_seale_construction_fatigue");
            Assert.NotNull(entry);
            Assert.Equal("HOIST_SHAFT_MAIN_CABLE_01", entry.CableSpoolIdentifier);
        }

        [Fact]
        public void CordageCableCatalog_Hawser_Integrity()
        {
            var catalog = CordageCableCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.HawserEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("manila_hawser_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.HawserCoilId));
                Assert.False(string.IsNullOrWhiteSpace(item.FiberBotanicalOrigin));
                Assert.True(item.RopeDiameterInches > 0);
                Assert.True(item.TensileBreakLoadKn > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetHawser("manila_hawser_abaca_fiber_saltwater_steep");
            Assert.NotNull(entry);
            Assert.Equal("TUNNEL_FERRY_TOW_HAWSER_01", entry.HawserCoilId);
        }

        [Fact]
        public void CordageCableCatalog_Splice_Integrity()
        {
            var catalog = CordageCableCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SpliceEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("rope_transmission_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.DriveLineShaftId));
                Assert.False(string.IsNullOrWhiteSpace(item.RopeDriveSystem));
                Assert.True(item.TransmittedPowerKilowatts > 0);
                Assert.True(item.SpliceLengthDiameters > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSplice("rope_transmission_long_splice_tuck_protrusion");
            Assert.NotNull(entry);
            Assert.Equal("CENTRAL_MACHINE_SHOP_MAIN_SHAFT", entry.DriveLineShaftId);
        }
    }
}
