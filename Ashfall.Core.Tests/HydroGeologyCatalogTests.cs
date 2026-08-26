using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class HydroGeologyCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public HydroGeologyCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void HydroGeologyCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = HydroGeologyCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.WellEntries.Count);
            Assert.Equal(8, catalog.BiotaEntries.Count);
            Assert.Equal(7, catalog.SteamEntries.Count);
            Assert.Equal(7, catalog.MineralEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void HydroGeologyCatalog_Wells_Integrity()
        {
            var catalog = HydroGeologyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.WellEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("well_contam_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.WellIdentifier));
                Assert.False(string.IsNullOrWhiteSpace(item.AquiferStratum));
                Assert.False(string.IsNullOrWhiteSpace(item.ContaminantAgent));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetWellContamination("well_contam_tritium_percolation_spike");
            Assert.NotNull(entry);
            Assert.Equal("DEEP_ARTESIAN_WELL_03", entry.WellIdentifier);
        }

        [Fact]
        public void HydroGeologyCatalog_Biota_Integrity()
        {
            var catalog = HydroGeologyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.BiotaEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("biota_cave_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.SpeciesDesignation));
                Assert.False(string.IsNullOrWhiteSpace(item.CavernLocation));
                Assert.False(string.IsNullOrWhiteSpace(item.EcologicalNiche));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetCaveBiota("biota_cave_eyeless_albino_trout");
            Assert.NotNull(entry);
            Assert.Equal("SALVELINUS_SUBTERRANEUS_ALBA", entry.SpeciesDesignation);
        }

        [Fact]
        public void HydroGeologyCatalog_Steam_Integrity()
        {
            var catalog = HydroGeologyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SteamEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("steam_vent_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.VentManifoldId));
                Assert.True(item.SteamTemperatureCelsius > 0);
                Assert.True(item.LinePressureBar > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.FailureDiagnostic));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSteamVent("steam_vent_superheated_nozzle_erosion");
            Assert.NotNull(entry);
            Assert.Equal("PRIMARY_GEOTHERMAL_HEADER_01", entry.VentManifoldId);
        }

        [Fact]
        public void HydroGeologyCatalog_Minerals_Integrity()
        {
            var catalog = HydroGeologyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.MineralEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("stalactite_assay_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.SampleSpecimenId));
                Assert.False(string.IsNullOrWhiteSpace(item.MineralSpecies));
                Assert.True(item.PrimaryMetalAssayPct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetMineralAssay("stalactite_assay_uranophane_canary_crust");
            Assert.NotNull(entry);
            Assert.Equal("SPELEO-MINERAL-ASSAY-009", entry.SampleSpecimenId);
        }
    }
}
