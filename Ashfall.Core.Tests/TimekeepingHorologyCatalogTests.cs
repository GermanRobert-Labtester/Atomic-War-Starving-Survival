using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class TimekeepingHorologyCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public TimekeepingHorologyCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void TimekeepingHorologyCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = TimekeepingHorologyCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.EscapementEntries.Count);
            Assert.Equal(8, catalog.PendulumEntries.Count);
            Assert.Equal(7, catalog.SpringEntries.Count);
            Assert.Equal(7, catalog.WaterEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void TimekeepingHorologyCatalog_Escapement_Integrity()
        {
            var catalog = TimekeepingHorologyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.EscapementEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("escapement_wear_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.ClockMechanismId));
                Assert.False(string.IsNullOrWhiteSpace(item.EscapementType));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetEscapement("escapement_wear_synthetic_ruby_pallet_chipping");
            Assert.NotNull(entry);
            Assert.Equal("MASTER_REGULATOR_CENTRAL_TOWER", entry.ClockMechanismId);
        }

        [Fact]
        public void TimekeepingHorologyCatalog_Pendulum_Integrity()
        {
            var catalog = TimekeepingHorologyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.PendulumEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("pendulum_thermal_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.PendulumAssemblyId));
                Assert.False(string.IsNullOrWhiteSpace(item.RodMaterialAlloy));
                Assert.True(item.ThermalExpansionCoefficientPpmK > 0);
                Assert.True(item.BobMassKg > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetPendulum("pendulum_thermal_invar_alloy_aging_drift");
            Assert.NotNull(entry);
            Assert.Equal("MASTER_INVAR_PENDULUM_01", entry.PendulumAssemblyId);
        }

        [Fact]
        public void TimekeepingHorologyCatalog_Spring_Integrity()
        {
            var catalog = TimekeepingHorologyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SpringEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("mainspring_fatigue_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.SpringBarrelId));
                Assert.False(string.IsNullOrWhiteSpace(item.SpringAlloyType));
                Assert.True(item.FullWindupTorqueNm > 0);
                Assert.True(item.FailureCycleCount > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSpring("mainspring_fatigue_micro_crack_arbor_eye_snap");
            Assert.NotNull(entry);
            Assert.Equal("MARINE_CHRONOMETER_BARREL_01", entry.SpringBarrelId);
        }

        [Fact]
        public void TimekeepingHorologyCatalog_Water_Integrity()
        {
            var catalog = TimekeepingHorologyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.WaterEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("clepsydra_silt_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.ClepsydraStationId));
                Assert.False(string.IsNullOrWhiteSpace(item.OrificeMaterialType));
                Assert.True(item.NominalFlowMlMin > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetWater("clepsydra_silt_jeweled_orifice_calcite_crust");
            Assert.NotNull(entry);
            Assert.Equal("RESERVOIR_CLEPSYDRA_CLOCK_01", entry.ClepsydraStationId);
        }
    }
}
