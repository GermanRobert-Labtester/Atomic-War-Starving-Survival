using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class BlackProjectsCatalogTests
    {
        private readonly string _narrativeDir;

        public BlackProjectsCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void BlackProjectsCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = BlackProjectsCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.OrbitalEntries.Count);
            Assert.Equal(8, catalog.DroneEntries.Count);
            Assert.Equal(7, catalog.CobaltEntries.Count);
            Assert.Equal(7, catalog.VaultEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void BlackProjectsCatalog_OrbitalTelemetry_Integrity()
        {
            var catalog = BlackProjectsCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.OrbitalEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("telemetry_olympus_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.Callsign));
                Assert.False(string.IsNullOrWhiteSpace(item.EntryType));
                Assert.True(item.OrbitalAltitudeKm > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetOrbital("telemetry_olympus_harpoon_07_discharge");
            Assert.NotNull(entry);
            Assert.Equal("OLYMPUS-PLATFORM-02", entry.Callsign);
        }

        [Fact]
        public void BlackProjectsCatalog_DroneCarrierBlackboxes_Integrity()
        {
            var catalog = BlackProjectsCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.DroneEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("blackbox_valkyrie_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.CarrierId));
                Assert.False(string.IsNullOrWhiteSpace(item.RecordType));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetDrone("blackbox_valkyrie_takeoff_sortie_01");
            Assert.NotNull(entry);
            Assert.Equal("UAV-CARRIER-VALKYRIE-09", entry.CarrierId);
        }

        [Fact]
        public void BlackProjectsCatalog_CobaltDirectives_Integrity()
        {
            var catalog = BlackProjectsCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.CobaltEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("directive_cobalt_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.DirectiveCode));
                Assert.False(string.IsNullOrWhiteSpace(item.Classification));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetCobalt("directive_cobalt_dual_key_authorization");
            Assert.NotNull(entry);
            Assert.Equal("EO-99-Z-COBALT-RELEASE", entry.DirectiveCode);
        }

        [Fact]
        public void BlackProjectsCatalog_ArchitectVaultAudits_Integrity()
        {
            var catalog = BlackProjectsCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.VaultEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("audit_architect_vault_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.VaultId));
                Assert.False(string.IsNullOrWhiteSpace(item.AuditType));
                Assert.False(string.IsNullOrWhiteSpace(item.ComplianceStatus));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetVault("audit_architect_vault_last_survivor_cremation");
            Assert.NotNull(entry);
            Assert.Equal("BUNKER-00-ARCHITECT-PRIME", entry.VaultId);
        }
    }
}
