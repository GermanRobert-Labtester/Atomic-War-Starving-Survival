using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class MilitaryArmoryCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public MilitaryArmoryCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void MilitaryArmoryCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = MilitaryArmoryCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.HoistEntries.Count);
            Assert.Equal(8, catalog.MunitionsEntries.Count);
            Assert.Equal(7, catalog.SonarEntries.Count);
            Assert.Equal(7, catalog.BreachEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void MilitaryArmoryCatalog_Hoists_Integrity()
        {
            var catalog = MilitaryArmoryCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.HoistEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("hoist_jam_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.TurretEmplacementId));
                Assert.False(string.IsNullOrWhiteSpace(item.CaliberDesignation));
                Assert.False(string.IsNullOrWhiteSpace(item.HoistMechanismType));
                Assert.False(string.IsNullOrWhiteSpace(item.JamClassification));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetHoistJam("hoist_jam_pneumatic_rammer_misfeed");
            Assert.NotNull(entry);
            Assert.Equal("TURRET_NORTH_OUTPOST_01", entry.TurretEmplacementId);
        }

        [Fact]
        public void MilitaryArmoryCatalog_Munitions_Integrity()
        {
            var catalog = MilitaryArmoryCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.MunitionsEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("munitions_leaching_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.MagazineVaultId));
                Assert.False(string.IsNullOrWhiteSpace(item.ChemicalAgent));
                Assert.True(item.StorageTemperatureCelsius > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.HazardTier));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetMunitionsLeaching("munitions_leaching_white_phosphorus_casing_rupture");
            Assert.NotNull(entry);
            Assert.Equal("VAULT_EIGHT_ORDNANCE_STORE", entry.MagazineVaultId);
        }

        [Fact]
        public void MilitaryArmoryCatalog_Sonar_Integrity()
        {
            var catalog = MilitaryArmoryCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SonarEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("sonar_fault_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.HydrophoneStationId));
                Assert.False(string.IsNullOrWhiteSpace(item.TransducerElementType));
                Assert.True(item.AttenuationLossDb > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.FailureClassification));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSonarFault("sonar_fault_piezoelectric_ceramic_cracking");
            Assert.NotNull(entry);
            Assert.Equal("PERIMETER_ACOUSTIC_LINE_NORTH", entry.HydrophoneStationId);
        }

        [Fact]
        public void MilitaryArmoryCatalog_Breaches_Integrity()
        {
            var catalog = MilitaryArmoryCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.BreachEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("vault_breach_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.VaultSectorId));
                Assert.False(string.IsNullOrWhiteSpace(item.BarrierMaterial));
                Assert.False(string.IsNullOrWhiteSpace(item.BreachTechnique));
                Assert.True(item.BreachTimeMinutes > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetVaultBreach("vault_breach_thermal_lance_magnesium_slag");
            Assert.NotNull(entry);
            Assert.Equal("VAULT_SEVEN_MAIN_DEPOSIT_DOOR", entry.VaultSectorId);
        }
    }
}
