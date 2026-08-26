using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class SoapSaponificationCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public SoapSaponificationCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void SoapSaponificationCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = SoapSaponificationCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.LyeEntries.Count);
            Assert.Equal(8, catalog.TallowEntries.Count);
            Assert.Equal(7, catalog.CuringEntries.Count);
            Assert.Equal(7, catalog.GlycerinEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void SoapSaponificationCatalog_Lye_Integrity()
        {
            var catalog = SoapSaponificationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.LyeEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("wood_ash_lye_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.LeachingVatId));
                Assert.False(string.IsNullOrWhiteSpace(item.FeedstockAshSource));
                Assert.True(item.LyeSpecificGravityBaume > 0);
                Assert.True(item.PotassiumHydroxideConcentrationPct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetLye("wood_ash_lye_leaching_barrel_straw_filter");
            Assert.NotNull(entry);
            Assert.Equal("PRIMARY_ASH_LEACHING_VAT_01", entry.LeachingVatId);
        }

        [Fact]
        public void SoapSaponificationCatalog_Tallow_Integrity()
        {
            var catalog = SoapSaponificationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.TallowEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("tallow_saponification_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.BoilingKettleId));
                Assert.True(item.FatChargeTallowKg > 0);
                Assert.True(item.SaponificationTemperatureCelsius > 0);
                Assert.True(item.GrainCurdYieldKg > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetTallow("tallow_saponification_boiling_kettle_emulsion");
            Assert.NotNull(entry);
            Assert.Equal("COMMISSARY_SOAP_KETTLE_01", entry.BoilingKettleId);
        }

        [Fact]
        public void SoapSaponificationCatalog_Curing_Integrity()
        {
            var catalog = SoapSaponificationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.CuringEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("cold_process_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.MoldingRackId));
                Assert.True(item.CureTimeDays > 0);
                Assert.True(item.BarMoistureContentPct > 0);
                Assert.True(item.ShoreHardnessDurometer > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetCuring("cold_process_trace_gel_phase_insulation");
            Assert.NotNull(entry);
            Assert.Equal("WOODEN_FRAME_MOLD_BAY_01", entry.MoldingRackId);
        }

        [Fact]
        public void SoapSaponificationCatalog_Glycerin_Integrity()
        {
            var catalog = SoapSaponificationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.GlycerinEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("sweet_water_glycerin_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.GlycerinStillId));
                Assert.True(item.CrudeGlycerinConcentrationPct > 0);
                Assert.True(item.GlycerolPurityPct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetGlycerin("sweet_water_glycerin_spent_lye_acid_precipitation");
            Assert.NotNull(entry);
            Assert.Equal("SWEET_WATER_ACID_TREATING_TANK", entry.GlycerinStillId);
        }
    }
}
