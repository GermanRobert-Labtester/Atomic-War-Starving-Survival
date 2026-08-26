using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class WaterTreatmentPotableCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public WaterTreatmentPotableCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void WaterTreatmentPotableCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = WaterTreatmentPotableCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.SandEntries.Count);
            Assert.Equal(8, catalog.OzoneEntries.Count);
            Assert.Equal(7, catalog.ChlorineEntries.Count);
            Assert.Equal(7, catalog.CarbonEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void WaterTreatmentPotableCatalog_Sand_Integrity()
        {
            var catalog = WaterTreatmentPotableCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SandEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("slow_sand_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.FilterBasinId));
                Assert.True(item.FiltrationRateMetersPerHour > 0);
                Assert.True(item.InfluentTurbidityNtu > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSand("slow_sand_biofilm_schmutzdecke_maturation");
            Assert.NotNull(entry);
            Assert.Equal("PRIMARY_SLOW_SAND_BASIN_01", entry.FilterBasinId);
        }

        [Fact]
        public void WaterTreatmentPotableCatalog_Ozone_Integrity()
        {
            var catalog = WaterTreatmentPotableCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.OzoneEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("ozone_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.OzonatorUnitId));
                Assert.True(item.ContactTimeMinutes >= 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetOzone("ozone_corona_dielectric_glass_tube_puncture");
            Assert.NotNull(entry);
            Assert.Equal("MAIN_CORONA_GENERATOR_01", entry.OzonatorUnitId);
        }

        [Fact]
        public void WaterTreatmentPotableCatalog_Chlorine_Integrity()
        {
            var catalog = WaterTreatmentPotableCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.ChlorineEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("chlorine_titration_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.DosingStationId));
                Assert.False(string.IsNullOrWhiteSpace(item.HypochloriteReagentGrade));
                Assert.True(item.FreeChlorineResidualMgL > 0);
                Assert.True(item.WaterPhAtSampling > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetChlorine("chlorine_titration_dpd_free_residual_endpoint");
            Assert.NotNull(entry);
            Assert.Equal("DISTRIBUTION_HEADER_CHLORINATOR", entry.DosingStationId);
        }

        [Fact]
        public void WaterTreatmentPotableCatalog_Carbon_Integrity()
        {
            var catalog = WaterTreatmentPotableCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.CarbonEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("carbon_adsorption_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.CarbonFilterVesselId));
                Assert.False(string.IsNullOrWhiteSpace(item.CarbonBaseFeedstock));
                Assert.True(item.IodineNumberMgG > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetCarbon("carbon_adsorption_coconut_shell_iodine_number");
            Assert.NotNull(entry);
            Assert.Equal("PRIMARY_GAC_ADSORPTION_COLUMN_01", entry.CarbonFilterVesselId);
        }
    }
}
