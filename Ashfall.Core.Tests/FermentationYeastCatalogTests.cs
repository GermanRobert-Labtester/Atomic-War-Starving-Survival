using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class FermentationYeastCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public FermentationYeastCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void FermentationYeastCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = FermentationYeastCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.SourdoughEntries.Count);
            Assert.Equal(8, catalog.BrewingEntries.Count);
            Assert.Equal(7, catalog.SilageEntries.Count);
            Assert.Equal(7, catalog.CrockEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void FermentationYeastCatalog_Sourdough_Integrity()
        {
            var catalog = FermentationYeastCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SourdoughEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("sourdough_mother_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.StarterCrockId));
                Assert.False(string.IsNullOrWhiteSpace(item.MicrobialConsortiumType));
                Assert.True(item.CulturePhLevel > 0);
                Assert.True(item.LacticToAceticAcidRatio > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSourdough("sourdough_mother_ph_titration_lactic_acetic_ratio");
            Assert.NotNull(entry);
            Assert.Equal("COMMISSARY_BAKERY_MOTHER_01", entry.StarterCrockId);
        }

        [Fact]
        public void FermentationYeastCatalog_Brewing_Integrity()
        {
            var catalog = FermentationYeastCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.BrewingEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("brewers_yeast_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.FermentationTunId));
                Assert.False(string.IsNullOrWhiteSpace(item.YeastStrainDesignation));
                Assert.True(item.ApparentAttenuationPct > 0);
                Assert.True(item.FermentationTemperatureCelsius > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetBrewing("brewers_yeast_krausen_head_top_cropping_skimming");
            Assert.NotNull(entry);
            Assert.Equal("COMMISSARY_BREWHOUSE_TUN_01", entry.FermentationTunId);
        }

        [Fact]
        public void FermentationYeastCatalog_Silage_Integrity()
        {
            var catalog = FermentationYeastCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SilageEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("silage_pit_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.SilageTrenchId));
                Assert.False(string.IsNullOrWhiteSpace(item.ForageSubstrateCrop));
                Assert.True(item.PitFermentationPh > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSilage("silage_pit_clostridium_butyric_spoilage_foul_odor");
            Assert.NotNull(entry);
            Assert.Equal("DEEP_BUNKER_FORAGE_TRENCH_01", entry.SilageTrenchId);
        }

        [Fact]
        public void FermentationYeastCatalog_Crock_Integrity()
        {
            var catalog = FermentationYeastCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.CrockEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("fermentation_crock_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.StonewareCrockId));
                Assert.True(item.CrockVolumeLiters > 0);
                Assert.True(item.BrineSalinityPct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetCrock("fermentation_crock_water_channel_gutter_evaporation");
            Assert.NotNull(entry);
            Assert.Equal("PRESERVATION_CELLAR_CROCK_01", entry.StonewareCrockId);
        }
    }
}
