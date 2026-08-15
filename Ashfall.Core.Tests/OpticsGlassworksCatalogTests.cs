using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class OpticsGlassworksCatalogTests
    {
        private readonly string _narrativeDir;

        public OpticsGlassworksCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void OpticsGlassworksCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = OpticsGlassworksCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.PrismEntries.Count);
            Assert.Equal(8, catalog.SightGlassEntries.Count);
            Assert.Equal(7, catalog.RadBrowningEntries.Count);
            Assert.Equal(7, catalog.ScintillatorEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void OpticsGlassworksCatalog_Prisms_Integrity()
        {
            var catalog = OpticsGlassworksCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.PrismEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("prism_delam_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.PeriscopeAssemblyId));
                Assert.False(string.IsNullOrWhiteSpace(item.OpticalGlassType));
                Assert.False(string.IsNullOrWhiteSpace(item.OpticalCementType));
                Assert.True(item.TransmissionLossPct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetPrism("prism_delam_canada_balsam_cement_yellowing");
            Assert.NotNull(entry);
            Assert.Equal("PERISCOPE_MAST_NORTH_SENTRY_01", entry.PeriscopeAssemblyId);
        }

        [Fact]
        public void OpticsGlassworksCatalog_SightGlasses_Integrity()
        {
            var catalog = OpticsGlassworksCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SightGlassEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("sight_glass_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.BoilerSystemId));
                Assert.False(string.IsNullOrWhiteSpace(item.GlassComposition));
                Assert.True(item.OperatingPressureBar > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.FailureClassification));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSightGlass("sight_glass_steam_caustic_alkali_grooving");
            Assert.NotNull(entry);
            Assert.Equal("MAIN_STEAM_GENERATOR_BOILER_01", entry.BoilerSystemId);
        }

        [Fact]
        public void OpticsGlassworksCatalog_RadBrowning_Integrity()
        {
            var catalog = OpticsGlassworksCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.RadBrowningEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("rad_brown_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.OpticSystemId));
                Assert.False(string.IsNullOrWhiteSpace(item.SubstrateMaterial));
                Assert.True(item.AccumulatedGammaDoseRad > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.SolarizationSpectralBand));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetRadBrowning("rad_brown_lanthanum_dense_crown_color_centers");
            Assert.NotNull(entry);
            Assert.Equal("SNIPER_NIGHT_TELESCOPE_8X", entry.OpticSystemId);
        }

        [Fact]
        public void OpticsGlassworksCatalog_Scintillators_Integrity()
        {
            var catalog = OpticsGlassworksCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.ScintillatorEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("scint_crystal_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.DetectorUnitId));
                Assert.False(string.IsNullOrWhiteSpace(item.CrystalComposition));
                Assert.True(item.QuantumYieldPhotonsKev > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.DegradationMode));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetScintillator("scint_crystal_thallium_iodide_hygroscopic_fogging");
            Assert.NotNull(entry);
            Assert.Equal("SCINTILLATION_PROBE_MODEL_44_9", entry.DetectorUnitId);
        }
    }
}
