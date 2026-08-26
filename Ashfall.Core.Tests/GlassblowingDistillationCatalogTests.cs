using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class GlassblowingDistillationCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public GlassblowingDistillationCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void GlassblowingDistillationCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = GlassblowingDistillationCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.MeltEntries.Count);
            Assert.Equal(8, catalog.CondenserEntries.Count);
            Assert.Equal(7, catalog.GreaseEntries.Count);
            Assert.Equal(7, catalog.AnnealEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void GlassblowingDistillationCatalog_Melts_Integrity()
        {
            var catalog = GlassblowingDistillationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.MeltEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("glass_melt_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.FurnacePotIdentifier));
                Assert.False(string.IsNullOrWhiteSpace(item.BatchFeedstockFormula));
                Assert.True(item.MeltTemperatureCelsius > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.OpticalClarityGrade));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetMelt("glass_melt_quartz_sand_potash_batch_fining");
            Assert.NotNull(entry);
            Assert.Equal("POT_FURNACE_CRUCIBLE_01", entry.FurnacePotIdentifier);
        }

        [Fact]
        public void GlassblowingDistillationCatalog_Condensers_Integrity()
        {
            var catalog = GlassblowingDistillationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.CondenserEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("condenser_fracture_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.DistillationRigId));
                Assert.False(string.IsNullOrWhiteSpace(item.GlasswareComponent));
                Assert.False(string.IsNullOrWhiteSpace(item.FailurePhenomenon));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetCondenser("condenser_fracture_cold_water_inlet_thermal_shock");
            Assert.NotNull(entry);
            Assert.Equal("NITRIC_ACID_RETORT_APPARATUS_01", entry.DistillationRigId);
        }

        [Fact]
        public void GlassblowingDistillationCatalog_Grease_Integrity()
        {
            var catalog = GlassblowingDistillationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.GreaseEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("joint_grease_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.ApparatusStationId));
                Assert.False(string.IsNullOrWhiteSpace(item.LubricantCompoundUsed));
                Assert.True(item.SystemVacuumTorr > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.FailureOutcome));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetGrease("joint_grease_hydrocarbon_vaseline_leaching");
            Assert.NotNull(entry);
            Assert.Equal("ETHER_EXTRACTION_LAB_BENCH_01", entry.ApparatusStationId);
        }

        [Fact]
        public void GlassblowingDistillationCatalog_Anneal_Integrity()
        {
            var catalog = GlassblowingDistillationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.AnnealEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("annealing_lehr_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.LehrFurnaceId));
                Assert.False(string.IsNullOrWhiteSpace(item.AnnealedGlassArticle));
                Assert.True(item.SoakTemperatureCelsius > 0);
                Assert.True(item.ResidualStressOpticalRetardanceNmCm > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetAnneal("annealing_lehr_polarimeter_isochromatic_fringes");
            Assert.NotNull(entry);
            Assert.Equal("CONTINUOUS_MESH_BELT_LEHR_01", entry.LehrFurnaceId);
        }
    }
}
