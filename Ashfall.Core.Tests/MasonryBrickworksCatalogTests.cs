using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class MasonryBrickworksCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public MasonryBrickworksCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void MasonryBrickworksCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = MasonryBrickworksCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.KilnEntries.Count);
            Assert.Equal(8, catalog.MortarEntries.Count);
            Assert.Equal(7, catalog.RefractoryEntries.Count);
            Assert.Equal(7, catalog.AdobeEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void MasonryBrickworksCatalog_Kilns_Integrity()
        {
            var catalog = MasonryBrickworksCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.KilnEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("lime_kiln_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.KilnStructureId));
                Assert.False(string.IsNullOrWhiteSpace(item.FeedstockStoneType));
                Assert.True(item.CalcinationTempCelsius > 0);
                Assert.True(item.QuicklimeYieldTons > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetKiln("lime_kiln_limestone_calcination_temperature");
            Assert.NotNull(entry);
            Assert.Equal("VERTICAL_SHAFT_KILN_SECTOR_7", entry.KilnStructureId);
        }

        [Fact]
        public void MasonryBrickworksCatalog_Mortar_Integrity()
        {
            var catalog = MasonryBrickworksCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.MortarEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("pozzolan_mortar_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.MortarRecipeCode));
                Assert.False(string.IsNullOrWhiteSpace(item.PozzolanicSource));
                Assert.True(item.CompressiveStrengthMpa > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.CuringEnvironment));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetMortar("pozzolan_mortar_volcanic_ash_lime_reaction");
            Assert.NotNull(entry);
            Assert.Equal("ROMAN_POZZOLAN_HYDRAULIC_01", entry.MortarRecipeCode);
        }

        [Fact]
        public void MasonryBrickworksCatalog_Refractory_Integrity()
        {
            var catalog = MasonryBrickworksCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.RefractoryEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("firebrick_spall_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.FurnaceZoneId));
                Assert.False(string.IsNullOrWhiteSpace(item.RefractoryBrickGrade));
                Assert.True(item.OperatingTemperatureCelsius > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.FailureMechanism));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetRefractory("firebrick_spall_silica_inversion_volume_jump");
            Assert.NotNull(entry);
            Assert.Equal("REVERBERATORY_HEARTH_ROOF_ARCH", entry.FurnaceZoneId);
        }

        [Fact]
        public void MasonryBrickworksCatalog_Adobe_Integrity()
        {
            var catalog = MasonryBrickworksCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.AdobeEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("mudbrick_assay_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.AdobeBatchIdentifier));
                Assert.False(string.IsNullOrWhiteSpace(item.ReinforcementFiberType));
                Assert.False(string.IsNullOrWhiteSpace(item.ClayToSandRatio));
                Assert.True(item.WetCompressiveStrengthMpa > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetAdobe("mudbrick_assay_wheat_straw_tensile_reinforcement");
            Assert.NotNull(entry);
            Assert.Equal("SUN_DRIED_ADOBE_BLOCK_SET_01", entry.AdobeBatchIdentifier);
        }
    }
}
