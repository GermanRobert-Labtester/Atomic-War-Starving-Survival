using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class PaperMakingCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public PaperMakingCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void PaperMakingCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = PaperMakingCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.BeaterEntries.Count);
            Assert.Equal(8, catalog.MouldEntries.Count);
            Assert.Equal(7, catalog.PressEntries.Count);
            Assert.Equal(7, catalog.SizingEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void PaperMakingCatalog_Beater_Integrity()
        {
            var catalog = PaperMakingCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.BeaterEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("hollander_beater_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.BeaterTubId));
                Assert.False(string.IsNullOrWhiteSpace(item.RagFeedstockType));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetBeater("hollander_beater_flax_linen_rag_maceration");
            Assert.NotNull(entry);
            Assert.Equal("PRIMARY_HOLLANDER_PULP_ENGINE_01", entry.BeaterTubId);
        }

        [Fact]
        public void PaperMakingCatalog_Mould_Integrity()
        {
            var catalog = PaperMakingCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.MouldEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("deckle_mould_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.MouldFrameId));
                Assert.True(item.WireMeshCountPerInch > 0);
                Assert.True(item.SheetWidthMm > 0);
                Assert.True(item.SheetLengthMm > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetMould("deckle_mould_laid_wire_chain_line_spacing");
            Assert.NotNull(entry);
            Assert.Equal("IMPERIAL_LAID_MOULD_PAIR_01", entry.MouldFrameId);
        }

        [Fact]
        public void PaperMakingCatalog_Press_Integrity()
        {
            var catalog = PaperMakingCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.PressEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("screw_press_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.PressStationId));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetPress("screw_press_couching_wool_felt_interleaving");
            Assert.NotNull(entry);
            Assert.Equal("COMMISSARY_COUCHING_BENCH_01", entry.PressStationId);
        }

        [Fact]
        public void PaperMakingCatalog_Sizing_Integrity()
        {
            var catalog = PaperMakingCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SizingEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("tub_sizing_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.SizingVatId));
                Assert.True(item.CobbWaterAbsorptionGPerM2 > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSizing("tub_sizing_hide_glue_gelatin_immersion");
            Assert.NotNull(entry);
            Assert.Equal("PRIMARY_GELATIN_TUB_01", entry.SizingVatId);
        }
    }
}
