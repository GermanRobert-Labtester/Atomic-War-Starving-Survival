using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class RefrigerationFermentationCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public RefrigerationFermentationCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void RefrigerationFermentationCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = RefrigerationFermentationCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.ChillerEntries.Count);
            Assert.Equal(8, catalog.PicklingEntries.Count);
            Assert.Equal(7, catalog.CellarEntries.Count);
            Assert.Equal(7, catalog.SmokeEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void RefrigerationFermentationCatalog_Chillers_Integrity()
        {
            var catalog = RefrigerationFermentationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.ChillerEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("ammonia_chiller_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.ChillerUnitId));
                Assert.True(item.RefrigerantChargeKg > 0);
                Assert.True(item.LeakRatePpmAmbient >= 0);
                Assert.False(string.IsNullOrWhiteSpace(item.SystemFailureMode));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetChiller("ammonia_chiller_generator_coil_stress_corrosion");
            Assert.NotNull(entry);
            Assert.Equal("COLD_STORAGE_ABSORPTION_PLANT_01", entry.ChillerUnitId);
        }

        [Fact]
        public void RefrigerationFermentationCatalog_Pickling_Integrity()
        {
            var catalog = RefrigerationFermentationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.PicklingEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("pickling_spoil_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.BarrelBatchCode));
                Assert.False(string.IsNullOrWhiteSpace(item.FoodSubstrateType));
                Assert.True(item.SalinityPercentage >= 0);
                Assert.False(string.IsNullOrWhiteSpace(item.SpoilageOrganism));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetPickling("pickling_spoil_kahm_yeast_surface_pellicle");
            Assert.NotNull(entry);
            Assert.Equal("SAUERKRAUT_BARREL_OAK_104", entry.BarrelBatchCode);
        }

        [Fact]
        public void RefrigerationFermentationCatalog_Cellars_Integrity()
        {
            var catalog = RefrigerationFermentationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.CellarEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("cellar_rot_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.RootCellarBayId));
                Assert.False(string.IsNullOrWhiteSpace(item.StoredCropSpecies));
                Assert.True(item.AmbientHumidityPct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.FungalPathogenName));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetCellar("cellar_rot_phytophthora_potato_late_blight");
            Assert.NotNull(entry);
            Assert.Equal("CELLAR_BAY_POTATO_VAULT_01", entry.RootCellarBayId);
        }

        [Fact]
        public void RefrigerationFermentationCatalog_Smokehouse_Integrity()
        {
            var catalog = RefrigerationFermentationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SmokeEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("smokehouse_assay_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.SmokehouseFacilityId));
                Assert.False(string.IsNullOrWhiteSpace(item.FuelWoodSpecies));
                Assert.True(item.CreosoteDepositMgKg > 0);
                Assert.True(item.CuringTemperatureCelsius > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSmoke("smokehouse_assay_green_pine_tar_creosote_crust");
            Assert.NotNull(entry);
            Assert.Equal("COMMUNAL_SMOKEHOUSE_KILN_01", entry.SmokehouseFacilityId);
        }
    }
}
