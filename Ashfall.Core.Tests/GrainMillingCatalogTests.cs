using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class GrainMillingCatalogTests
    {
        private readonly string _narrativeDir;

        public GrainMillingCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void GrainMillingCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = GrainMillingCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.MillstoneEntries.Count);
            Assert.Equal(8, catalog.SilkEntries.Count);
            Assert.Equal(7, catalog.SiloEntries.Count);
            Assert.Equal(7, catalog.TemperEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void GrainMillingCatalog_Millstone_Integrity()
        {
            var catalog = GrainMillingCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.MillstoneEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("burr_millstone_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.MillstonePairId));
                Assert.False(string.IsNullOrWhiteSpace(item.StoneMaterialType));
                Assert.True(item.CracksPerInchCount > 0);
                Assert.True(item.RunnerRotationalRpm > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetMillstone("burr_millstone_french_chert_chisel_cracking");
            Assert.NotNull(entry);
            Assert.Equal("PRIMARY_BURR_RUNNER_PAIR_01", entry.MillstonePairId);
        }

        [Fact]
        public void GrainMillingCatalog_Silk_Integrity()
        {
            var catalog = GrainMillingCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SilkEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("bolting_silk_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.SifterReelId));
                Assert.False(string.IsNullOrWhiteSpace(item.SilkGauzeGrade));
                Assert.True(item.MeshApertureMicrons > 0);
                Assert.True(item.FlourExtractionYieldPct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSilk("bolting_silk_gauze_number_mesh_selection");
            Assert.NotNull(entry);
            Assert.Equal("PRIMARY_PATENT_FLOUR_BOLTER_01", entry.SifterReelId);
        }

        [Fact]
        public void GrainMillingCatalog_Silo_Integrity()
        {
            var catalog = GrainMillingCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SiloEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("grain_silo_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.GrainSiloBinId));
                Assert.False(string.IsNullOrWhiteSpace(item.GrainCropSpecies));
                Assert.True(item.GrainMoistureContentPct > 0);
                Assert.True(item.GrainTemperatureCelsius > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSilo("grain_silo_granary_weevil_larva_hollow_berry");
            Assert.NotNull(entry);
            Assert.Equal("DEEP_STORAGE_SILO_ALPHA_01", entry.GrainSiloBinId);
        }

        [Fact]
        public void GrainMillingCatalog_Temper_Integrity()
        {
            var catalog = GrainMillingCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.TemperEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("mill_tempering_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.ConditioningBinId));
                Assert.True(item.TargetMillingMoisturePct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetTemper("mill_tempering_hard_wheat_bran_toughening");
            Assert.NotNull(entry);
            Assert.Equal("PRIMARY_TEMPERING_SILO_01", entry.ConditioningBinId);
        }
    }
}
