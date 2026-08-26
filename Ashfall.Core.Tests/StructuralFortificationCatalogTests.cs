using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class StructuralFortificationCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public StructuralFortificationCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void StructuralFortificationCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = StructuralFortificationCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.GateEntries.Count);
            Assert.Equal(8, catalog.SiltEntries.Count);
            Assert.Equal(7, catalog.LeadWallEntries.Count);
            Assert.Equal(7, catalog.FilterEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void StructuralFortificationCatalog_Gates_Integrity()
        {
            var catalog = StructuralFortificationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.GateEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("audit_gate_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.GateDesignation));
                Assert.False(string.IsNullOrWhiteSpace(item.MechanicalSubsystem));
                Assert.False(string.IsNullOrWhiteSpace(item.FailureMode));
                Assert.False(string.IsNullOrWhiteSpace(item.StructuralSeverity));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetGateAudit("audit_gate_hydraulic_ram_seal_blowout");
            Assert.NotNull(entry);
            Assert.Equal("PRIMARY_BLAST_GATE_01", entry.GateDesignation);
        }

        [Fact]
        public void StructuralFortificationCatalog_Silt_Integrity()
        {
            var catalog = StructuralFortificationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SiltEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("silt_report_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.SumpBasinId));
                Assert.False(string.IsNullOrWhiteSpace(item.PumpModel));
                Assert.True(item.SiltDensityGramsPerLiter > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.OperationalStatus));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSiltReport("silt_report_cast_iron_impeller_cavitation");
            Assert.NotNull(entry);
            Assert.Equal("MAIN_DRAINAGE_SUMP_SECTOR_4", entry.SumpBasinId);
        }

        [Fact]
        public void StructuralFortificationCatalog_LeadWall_Integrity()
        {
            var catalog = StructuralFortificationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.LeadWallEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("lead_wall_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.WallSectorId));
                Assert.True(item.LeadThicknessMm > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.StructuralDegradationMode));
                Assert.True(item.ShieldingIntegrityPct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetLeadWall("lead_wall_gravitational_creep_sagging");
            Assert.NotNull(entry);
            Assert.Equal("SECTOR_1_PRIMARY_SHADOW_WALL", entry.WallSectorId);
        }

        [Fact]
        public void StructuralFortificationCatalog_Filters_Integrity()
        {
            var catalog = StructuralFortificationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.FilterEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("filter_clog_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.FilterBankId));
                Assert.False(string.IsNullOrWhiteSpace(item.FilterStage));
                Assert.True(item.DifferentialPressurePascals > 0);
                Assert.True(item.AirflowLossPct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetFilterClog("filter_clog_volcanic_ash_cake_compaction");
            Assert.NotNull(entry);
            Assert.Equal("MAIN_SURFACE_INTAKE_BANK_ALPHA", entry.FilterBankId);
        }
    }
}
