using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class PolymerTextileCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public PolymerTextileCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void PolymerTextileCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = PolymerTextileCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.GasketEntries.Count);
            Assert.Equal(8, catalog.AramidEntries.Count);
            Assert.Equal(7, catalog.TireEntries.Count);
            Assert.Equal(7, catalog.FilmEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void PolymerTextileCatalog_Gaskets_Integrity()
        {
            var catalog = PolymerTextileCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.GasketEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("gasket_degrade_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.MaskModelDesignation));
                Assert.False(string.IsNullOrWhiteSpace(item.ElastomerPolymerType));
                Assert.True(item.OzoneExposurePpm > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.DegradationSeverity));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetGasket("gasket_degrade_ozone_corona_cracking");
            Assert.NotNull(entry);
            Assert.Equal("M17_SURVIVAL_RESPIRATOR_FACEPIECE", entry.MaskModelDesignation);
        }

        [Fact]
        public void PolymerTextileCatalog_Aramids_Integrity()
        {
            var catalog = PolymerTextileCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.AramidEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("aramid_rot_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.ArmorItemId));
                Assert.False(string.IsNullOrWhiteSpace(item.AramidYarnType));
                Assert.True(item.ResidualTensileStrengthPct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.FailurePhenomenon));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetAramid("aramid_rot_hydrolytic_chain_cleavage");
            Assert.NotNull(entry);
            Assert.Equal("PASGT_BALLISTIC_VEST_MK2", entry.ArmorItemId);
        }

        [Fact]
        public void PolymerTextileCatalog_Tires_Integrity()
        {
            var catalog = PolymerTextileCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.TireEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("tire_retread_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.TireCasingId));
                Assert.False(string.IsNullOrWhiteSpace(item.RubberCompoundFormula));
                Assert.True(item.VulcanizationTempCelsius > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.RoadWearRating));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetTire("tire_retread_carbon_black_reinforcement_mix");
            Assert.NotNull(entry);
            Assert.Equal("MILITARY_TRUCK_TIRE_11R20", entry.TireCasingId);
        }

        [Fact]
        public void PolymerTextileCatalog_Films_Integrity()
        {
            var catalog = PolymerTextileCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.FilmEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("celluloid_decay_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.FilmArchiveReelId));
                Assert.False(string.IsNullOrWhiteSpace(item.PolymerBaseChemistry));
                Assert.False(string.IsNullOrWhiteSpace(item.DecompositionStage));
                Assert.True(item.CombustionTemperatureCelsius > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetFilm("celluloid_decay_nitric_acid_sweat_vinegar_syndrome");
            Assert.NotNull(entry);
            Assert.Equal("CIVIL_DEFENSE_REEL_35MM_104", entry.FilmArchiveReelId);
        }
    }
}
