using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class CrucibleFoundryCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public CrucibleFoundryCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void CrucibleFoundryCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = CrucibleFoundryCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.CrucibleEntries.Count);
            Assert.Equal(8, catalog.CupolaEntries.Count);
            Assert.Equal(7, catalog.PatternEntries.Count);
            Assert.Equal(7, catalog.SandEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void CrucibleFoundryCatalog_Crucible_Integrity()
        {
            var catalog = CrucibleFoundryCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.CrucibleEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("crucible_slag_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.CruciblePotId));
                Assert.False(string.IsNullOrWhiteSpace(item.CrucibleLiningFormula));
                Assert.True(item.MeltTemperatureCelsius > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetCrucible("crucible_slag_plumbago_graphite_wall_wash");
            Assert.NotNull(entry);
            Assert.Equal("STOURBRIDGE_GRAPHITE_POT_01", entry.CruciblePotId);
        }

        [Fact]
        public void CrucibleFoundryCatalog_Cupola_Integrity()
        {
            var catalog = CrucibleFoundryCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.CupolaEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("cupola_melting_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.CupolaFurnaceId));
                Assert.False(string.IsNullOrWhiteSpace(item.CokeToIronChargeRatio));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetCupola("cupola_melting_coke_bed_height_drop_chill");
            Assert.NotNull(entry);
            Assert.Equal("NO_1_FOUNDRY_CUPOLA_STACK", entry.CupolaFurnaceId);
        }

        [Fact]
        public void CrucibleFoundryCatalog_Pattern_Integrity()
        {
            var catalog = CrucibleFoundryCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.PatternEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("pattern_maker_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.PatternShopJobId));
                Assert.False(string.IsNullOrWhiteSpace(item.TimberPatternMaterial));
                Assert.False(string.IsNullOrWhiteSpace(item.ShrinkageAllowanceFraction));
                Assert.True(item.DraftAngleDegrees > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetPattern("pattern_maker_sugar_pine_shrink_rule_scale");
            Assert.NotNull(entry);
            Assert.Equal("PUMP_IMPELLER_HOUSING_PATTERN", entry.PatternShopJobId);
        }

        [Fact]
        public void CrucibleFoundryCatalog_Sand_Integrity()
        {
            var catalog = CrucibleFoundryCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SandEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("green_sand_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.SandMullerBatchId));
                Assert.False(string.IsNullOrWhiteSpace(item.ClayBinderType));
                Assert.True(item.TemperMoisturePct > 0);
                Assert.True(item.GreenCompressiveStrengthKpa > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSand("green_sand_western_bentonite_green_strength");
            Assert.NotNull(entry);
            Assert.Equal("MULLER_STATION_ALPHA_BATCH_01", entry.SandMullerBatchId);
        }
    }
}
