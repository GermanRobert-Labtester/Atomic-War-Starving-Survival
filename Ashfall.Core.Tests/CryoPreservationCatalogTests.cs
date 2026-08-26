using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class CryoPreservationCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public CryoPreservationCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void CryoPreservationCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = CryoPreservationCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.GermplasmEntries.Count);
            Assert.Equal(8, catalog.CompressorEntries.Count);
            Assert.Equal(7, catalog.PermafrostEntries.Count);
            Assert.Equal(7, catalog.GenomeEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void CryoPreservationCatalog_Germplasm_Integrity()
        {
            var catalog = CryoPreservationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.GermplasmEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("germplasm_audit_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.AccessionNumber));
                Assert.False(string.IsNullOrWhiteSpace(item.CropSpecies));
                Assert.True(item.StorageTemperatureKelvin > 0);
                Assert.True(item.GerminationViabilityPct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetGermplasm("germplasm_audit_hard_red_winter_wheat");
            Assert.NotNull(entry);
            Assert.Equal("GERM-CRYO-RUS-0941", entry.AccessionNumber);
        }

        [Fact]
        public void CryoPreservationCatalog_Compressors_Integrity()
        {
            var catalog = CryoPreservationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.CompressorEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("compressor_fail_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.CryocoolerUnitId));
                Assert.False(string.IsNullOrWhiteSpace(item.WorkingFluid));
                Assert.True(item.OperatingPressureBar > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.FailureMode));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetCompressor("compressor_fail_stirling_displacer_piston_blowby");
            Assert.NotNull(entry);
            Assert.Equal("STIRLING_COLD_HEAD_ALPHA_01", entry.CryocoolerUnitId);
        }

        [Fact]
        public void CryoPreservationCatalog_Permafrost_Integrity()
        {
            var catalog = CryoPreservationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.PermafrostEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("methane_eruption_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.VaultGeologicalSector));
                Assert.True(item.EstimatedMethaneVolumeM3 > 0);
                Assert.True(item.StructuralDisplacementCm > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.EruptionTrigger));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetPermafrost("methane_eruption_clathrate_hydrate_explosive_outgassing");
            Assert.NotNull(entry);
            Assert.Equal("PERMAFROST_VAULT_DECK_4_SUB", entry.VaultGeologicalSector);
        }

        [Fact]
        public void CryoPreservationCatalog_Genomes_Integrity()
        {
            var catalog = CryoPreservationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.GenomeEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("crop_genome_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.CultivarId));
                Assert.True(item.GenerationCycle > 0);
                Assert.True(item.MutationRatePerMegabase > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.PhenotypicDefect));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetGenome("crop_genome_spontaneous_point_mutation_load");
            Assert.NotNull(entry);
            Assert.Equal("HEIRLOOM_BARLEY_GOLDEN_PROMISE", entry.CultivarId);
        }
    }
}
