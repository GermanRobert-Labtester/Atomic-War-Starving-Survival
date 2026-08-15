using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class ApicultureBeeCatalogTests
    {
        private readonly string _narrativeDir;

        public ApicultureBeeCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void ApicultureBeeCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = ApicultureBeeCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.FoundationEntries.Count);
            Assert.Equal(8, catalog.RedLightEntries.Count);
            Assert.Equal(7, catalog.ExtractorEntries.Count);
            Assert.Equal(7, catalog.WaxEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void ApicultureBeeCatalog_Foundation_Integrity()
        {
            var catalog = ApicultureBeeCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.FoundationEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("langstroth_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.HiveAssemblyId));
                Assert.False(string.IsNullOrWhiteSpace(item.CombFoundationWaxGrade));
                Assert.True(item.CellBaseDiameterMm > 0);
                Assert.True(item.FrameCount > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetFoundation("langstroth_wax_foundation_embossed_roller_mill");
            Assert.NotNull(entry);
            Assert.Equal("PRIMARY_APIARY_LANGSTROTH_01", entry.HiveAssemblyId);
        }

        [Fact]
        public void ApicultureBeeCatalog_RedLight_Integrity()
        {
            var catalog = ApicultureBeeCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.RedLightEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("apiculture_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.ChamberZoneId));
                Assert.True(item.IlluminationWavelengthNm > 0);
                Assert.True(item.ColonyPopulationCount > 0);
                Assert.True(item.BroodChamberTemperatureCelsius > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetRedLight("apiculture_red_light_wavelength_blindness");
            Assert.NotNull(entry);
            Assert.Equal("SUBTERRANEAN_APIARY_FLIGHT_BAY_01", entry.ChamberZoneId);
        }

        [Fact]
        public void ApicultureBeeCatalog_Extractor_Integrity()
        {
            var catalog = ApicultureBeeCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.ExtractorEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("honey_extractor_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.ExtractorUnitId));
                Assert.True(item.HoneyMoisturePct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetExtractor("honey_extractor_radial_basket_frame_balance");
            Assert.NotNull(entry);
            Assert.Equal("COMMISSARY_RADIAL_EXTRACTOR_01", entry.ExtractorUnitId);
        }

        [Fact]
        public void ApicultureBeeCatalog_Wax_Integrity()
        {
            var catalog = ApicultureBeeCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.WaxEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("beeswax_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.RenderingVatId));
                Assert.True(item.BeeswaxMeltingPointCelsius > 0);
                Assert.True(item.UnadulteratedPurityPct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetWax("beeswax_solar_wax_extractor_slumgum_press");
            Assert.NotNull(entry);
            Assert.Equal("SOLAR_MELTER_RETORT_01", entry.RenderingVatId);
        }
    }
}
