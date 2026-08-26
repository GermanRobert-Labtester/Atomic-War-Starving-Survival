using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class CharcoalPyrolysisCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public CharcoalPyrolysisCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void CharcoalPyrolysisCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = CharcoalPyrolysisCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.MoundEntries.Count);
            Assert.Equal(8, catalog.RetortEntries.Count);
            Assert.Equal(7, catalog.BiocharEntries.Count);
            Assert.Equal(7, catalog.ForgeEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void CharcoalPyrolysisCatalog_Mound_Integrity()
        {
            var catalog = CharcoalPyrolysisCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.MoundEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("charcoal_mound_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.CoalingMoundId));
                Assert.False(string.IsNullOrWhiteSpace(item.FeedstockTimberSpecies));
                Assert.True(item.PyrolysisPeakTemperatureCelsius > 0);
                Assert.True(item.CharcoalGravimetricYieldPct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetMound("charcoal_mound_turf_capping_air_throttling");
            Assert.NotNull(entry);
            Assert.Equal("SUBTERRANEAN_KILN_MOUND_01", entry.CoalingMoundId);
        }

        [Fact]
        public void CharcoalPyrolysisCatalog_Retort_Integrity()
        {
            var catalog = CharcoalPyrolysisCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.RetortEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("retort_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.RetortVesselId));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetRetort("retort_pyroligneous_acid_wood_vinegar_collection");
            Assert.NotNull(entry);
            Assert.Equal("RECOVERY_RETORT_UNIT_ALPHA_01", entry.RetortVesselId);
        }

        [Fact]
        public void CharcoalPyrolysisCatalog_Biochar_Integrity()
        {
            var catalog = CharcoalPyrolysisCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.BiocharEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("biochar_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.SoilAmendmentLotId));
                Assert.True(item.BiocharBetSurfaceAreaM2PerG > 0);
                Assert.True(item.CationExchangeCapacityMeq > 0);
                Assert.True(item.SoilPhBufferedLevel > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetBiochar("biochar_compost_charging_nitrogen_quench");
            Assert.NotNull(entry);
            Assert.Equal("GREENHOUSE_SOIL_BED_ALPHA_01", entry.SoilAmendmentLotId);
        }

        [Fact]
        public void CharcoalPyrolysisCatalog_Forge_Integrity()
        {
            var catalog = CharcoalPyrolysisCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.ForgeEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("forge_charcoal_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.CarbonAssayBatchId));
                Assert.True(item.FixedCarbonPct > 0);
                Assert.True(item.AshContentPct > 0);
                Assert.True(item.ForgeHearthPeakTempCelsius > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetForge("forge_charcoal_fixed_carbon_proximate_analysis");
            Assert.NotNull(entry);
            Assert.Equal("HEAVY_FORGE_LUMP_BATCH_01", entry.CarbonAssayBatchId);
        }
    }
}
