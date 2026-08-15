using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class TanningLeatherCatalogTests
    {
        private readonly string _narrativeDir;

        public TanningLeatherCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void TanningLeatherCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = TanningLeatherCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.BarkEntries.Count);
            Assert.Equal(8, catalog.MineralEntries.Count);
            Assert.Equal(7, catalog.BatingEntries.Count);
            Assert.Equal(7, catalog.CurryingEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void TanningLeatherCatalog_Bark_Integrity()
        {
            var catalog = TanningLeatherCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.BarkEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("oak_bark_tan_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.TanneryVatId));
                Assert.False(string.IsNullOrWhiteSpace(item.BarkSourceBotanical));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetBark("oak_bark_tan_chestnut_liquor_density");
            Assert.NotNull(entry);
            Assert.Equal("LAY_AWAY_PIT_ROW_ALPHA_01", entry.TanneryVatId);
        }

        [Fact]
        public void TanningLeatherCatalog_Mineral_Integrity()
        {
            var catalog = TanningLeatherCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.MineralEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("mineral_tan_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.MineralTanLiquorId));
                Assert.False(string.IsNullOrWhiteSpace(item.MineralTanningAgent));
                Assert.True(item.LiquorPhLevel > 0);
                Assert.True(item.HydrothermalShrinkTempCelsius > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetMineral("mineral_tan_potassium_alum_white_tawing");
            Assert.NotNull(entry);
            Assert.Equal("WHITE_TAWING_DRUM_UNIT_01", entry.MineralTanLiquorId);
        }

        [Fact]
        public void TanningLeatherCatalog_Bating_Integrity()
        {
            var catalog = TanningLeatherCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.BatingEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("rawhide_bate_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.BeamhousePitId));
                Assert.False(string.IsNullOrWhiteSpace(item.DelimingChemicalAgent));
                Assert.False(string.IsNullOrWhiteSpace(item.PhenolphthaleinTestStatus));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetBating("rawhide_bate_ammonium_sulfate_deliming_stall");
            Assert.NotNull(entry);
            Assert.Equal("DELIMING_WASHER_PADDLE_01", entry.BeamhousePitId);
        }

        [Fact]
        public void TanningLeatherCatalog_Currying_Integrity()
        {
            var catalog = TanningLeatherCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.CurryingEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("leather_harness_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.CurryingWorkshopId));
                Assert.False(string.IsNullOrWhiteSpace(item.FatliquorCompoundFormula));
                Assert.True(item.OilContentPercentage > 0);
                Assert.True(item.TensileStrengthPsi > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetCurrying("leather_harness_neatsfoot_oil_cold_stuffing");
            Assert.NotNull(entry);
            Assert.Equal("HEAVY_HARNESS_CURRYING_BENCH_01", entry.CurryingWorkshopId);
        }
    }
}
