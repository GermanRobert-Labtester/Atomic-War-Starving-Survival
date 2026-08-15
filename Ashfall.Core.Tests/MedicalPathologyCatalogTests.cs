using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class MedicalPathologyCatalogTests
    {
        private readonly string _narrativeDir;

        public MedicalPathologyCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void MedicalPathologyCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = MedicalPathologyCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.AutopsyEntries.Count);
            Assert.Equal(8, catalog.PharmaEntries.Count);
            Assert.Equal(7, catalog.SurgeryEntries.Count);
            Assert.Equal(7, catalog.SensoryEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void MedicalPathologyCatalog_Autopsy_Integrity()
        {
            var catalog = MedicalPathologyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.AutopsyEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("pathology_autopsy_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.CaseNumber));
                Assert.False(string.IsNullOrWhiteSpace(item.AnatomicalRegion));
                Assert.True(item.EstimatedDoseRads > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.CauseOfDeath));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetAutopsy("pathology_autopsy_bone_marrow_aplasia");
            Assert.NotNull(entry);
            Assert.Equal("POST-MORTEM-CASE-014", entry.CaseNumber);
        }

        [Fact]
        public void MedicalPathologyCatalog_Pharma_Integrity()
        {
            var catalog = MedicalPathologyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.PharmaEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("pharma_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.CompoundName));
                Assert.False(string.IsNullOrWhiteSpace(item.ActiveAgent));
                Assert.False(string.IsNullOrWhiteSpace(item.PreparationMethod));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetPharma("pharma_powdered_willow_bark_salicylate");
            Assert.NotNull(entry);
            Assert.Equal("CRUDE_SALICYLATE_PULVER", entry.CompoundName);
        }

        [Fact]
        public void MedicalPathologyCatalog_Surgery_Integrity()
        {
            var catalog = MedicalPathologyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SurgeryEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("surgery_log_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.OperationCode));
                Assert.False(string.IsNullOrWhiteSpace(item.LeadSurgeon));
                Assert.False(string.IsNullOrWhiteSpace(item.SurvivalOutcome));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSurgery("surgery_log_triage_amputation_crushed_femur");
            Assert.NotNull(entry);
            Assert.Equal("DR_ELENA_ROSTOVA", entry.LeadSurgeon);
        }

        [Fact]
        public void MedicalPathologyCatalog_Sensory_Integrity()
        {
            var catalog = MedicalPathologyCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SensoryEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("sensory_loss_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.CaseFile));
                Assert.False(string.IsNullOrWhiteSpace(item.SensoryModality));
                Assert.False(string.IsNullOrWhiteSpace(item.PathologicalCause));
                Assert.True(item.FunctionalImpairmentPct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSensory("sensory_loss_posterior_subcapsular_cataracts");
            Assert.NotNull(entry);
            Assert.Equal("CLINICAL-SENSORY-CASE-008", entry.CaseFile);
        }
    }
}
