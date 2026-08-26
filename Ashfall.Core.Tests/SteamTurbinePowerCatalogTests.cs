using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class SteamTurbinePowerCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public SteamTurbinePowerCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void SteamTurbinePowerCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = SteamTurbinePowerCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.WellEntries.Count);
            Assert.Equal(8, catalog.TurbineEntries.Count);
            Assert.Equal(7, catalog.BoilerEntries.Count);
            Assert.Equal(7, catalog.TrapEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void SteamTurbinePowerCatalog_Well_Integrity()
        {
            var catalog = SteamTurbinePowerCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.WellEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("steam_well_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.WellheadDesignation));
                Assert.True(item.ReservoirEnthalpyKjKg > 0);
                Assert.True(item.WellheadPressureBar > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetWell("steam_well_silica_scaling_choke_orifice");
            Assert.NotNull(entry);
            Assert.Equal("GEOTHERMAL_BOREHOLE_DEEP_01", entry.WellheadDesignation);
        }

        [Fact]
        public void SteamTurbinePowerCatalog_Turbine_Integrity()
        {
            var catalog = SteamTurbinePowerCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.TurbineEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("turbine_blade_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.TurbineUnitIdentifier));
                Assert.True(item.RotorSpeedRpm >= 0);
                Assert.True(item.SteamWetnessFractionPct >= 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetTurbine("turbine_blade_last_stage_water_droplet_erosion");
            Assert.NotNull(entry);
            Assert.Equal("TURBINE_GENERATOR_SET_01", entry.TurbineUnitIdentifier);
        }

        [Fact]
        public void SteamTurbinePowerCatalog_Boiler_Integrity()
        {
            var catalog = SteamTurbinePowerCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.BoilerEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("boiler_deaerator_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.BoilerPlantId));
                Assert.True(item.DissolvedOxygenPpb > 0);
                Assert.True(item.FeedwaterPh > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetBoiler("boiler_deaerator_dissolved_oxygen_pitting");
            Assert.NotNull(entry);
            Assert.Equal("MAIN_WATER_TUBE_BOILER_01", entry.BoilerPlantId);
        }

        [Fact]
        public void SteamTurbinePowerCatalog_Trap_Integrity()
        {
            var catalog = SteamTurbinePowerCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.TrapEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("steam_trap_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.SteamDistributionBayId));
                Assert.False(string.IsNullOrWhiteSpace(item.TrapMechanismType));
                Assert.True(item.SystemLinePressureBar > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetTrap("steam_trap_inverted_bucket_prime_loss");
            Assert.NotNull(entry);
            Assert.Equal("MAIN_DISTRIBUTION_GALLERY_EAST", entry.SteamDistributionBayId);
        }
    }
}
