using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class PaperPrintingCatalogTests
    {
        private readonly string _narrativeDir;

        public PaperPrintingCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void PaperPrintingCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = PaperPrintingCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.PulpEntries.Count);
            Assert.Equal(8, catalog.InkEntries.Count);
            Assert.Equal(7, catalog.TypeEntries.Count);
            Assert.Equal(7, catalog.StencilEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void PaperPrintingCatalog_Pulp_Integrity()
        {
            var catalog = PaperPrintingCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.PulpEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("rag_pulp_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.BeaterStationId));
                Assert.False(string.IsNullOrWhiteSpace(item.RawFiberSource));
                Assert.True(item.FreenessCanadianMl > 0);
                Assert.True(item.PulpHydrationHours > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetPulp("rag_pulp_hollander_beater_linen_fibrillation");
            Assert.NotNull(entry);
            Assert.Equal("HOLLANDER_BEATER_MILL_01", entry.BeaterStationId);
        }

        [Fact]
        public void PaperPrintingCatalog_Ink_Integrity()
        {
            var catalog = PaperPrintingCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.InkEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("ink_assay_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.InkFormulationCode));
                Assert.False(string.IsNullOrWhiteSpace(item.TanninSource));
                Assert.True(item.MeasuredPhLevel > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.PigmentComplex));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetInk("ink_assay_oak_gall_tannic_acid_maceration");
            Assert.NotNull(entry);
            Assert.Equal("FORMULA_OAK_GALL_TANNIN_01", entry.InkFormulationCode);
        }

        [Fact]
        public void PaperPrintingCatalog_Type_Integrity()
        {
            var catalog = PaperPrintingCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.TypeEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("type_wear_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.FontCaseIdentifier));
                Assert.False(string.IsNullOrWhiteSpace(item.TypeMetalComposition));
                Assert.True(item.ImpressionCountCycles > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.WearPhenomenon));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetType("type_wear_antimony_eutectic_matrix_fatigue");
            Assert.NotNull(entry);
            Assert.Equal("CASLON_OLD_STYLE_12PT_ROMAN", entry.FontCaseIdentifier);
        }

        [Fact]
        public void PaperPrintingCatalog_Stencil_Integrity()
        {
            var catalog = PaperPrintingCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.StencilEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("stencil_smear_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.StencilPrintId));
                Assert.False(string.IsNullOrWhiteSpace(item.MatrixMaterialType));
                Assert.False(string.IsNullOrWhiteSpace(item.InkPigmentBase));
                Assert.False(string.IsNullOrWhiteSpace(item.SmearArtifactDescription));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetStencil("stencil_smear_linoleum_carving_gouge_slip");
            Assert.NotNull(entry);
            Assert.Equal("PROPAGANDA_WOODCUT_POSTER_01", entry.StencilPrintId);
        }
    }
}
