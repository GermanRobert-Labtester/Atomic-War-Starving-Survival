using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class SeedBankPreservationCatalogTests
    {
        private readonly string _narrativeDir;

        public SeedBankPreservationCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void SeedBankPreservationCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = SeedBankPreservationCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.CryoEntries.Count);
            Assert.Equal(8, catalog.RagdollEntries.Count);
            Assert.Equal(7, catalog.SilicaEntries.Count);
            Assert.Equal(7, catalog.HeirloomEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void SeedBankPreservationCatalog_Cryo_Integrity()
        {
            var catalog = SeedBankPreservationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.CryoEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("cryo_seed_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.DewarCanisterId));
                Assert.False(string.IsNullOrWhiteSpace(item.CropBotanicalSpecies));
                Assert.True(item.StorageTemperatureCelsius < 0);
                Assert.True(item.SeedMoistureContentPct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetCryo("cryo_seed_liquid_nitrogen_embryo_vitrification");
            Assert.NotNull(entry);
            Assert.Equal("CRYO_DEWAR_ALPHA_CAN_01", entry.DewarCanisterId);
        }

        [Fact]
        public void SeedBankPreservationCatalog_Ragdoll_Integrity()
        {
            var catalog = SeedBankPreservationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.RagdollEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("ragdoll_germination_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.GerminationTrayId));
                Assert.False(string.IsNullOrWhiteSpace(item.CropCultivarName));
                Assert.True(item.SeedsTestedCount > 0);
                Assert.True(item.GerminationViabilityPct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetRagdoll("ragdoll_germination_blotter_paper_radicle_emergence");
            Assert.NotNull(entry);
            Assert.Equal("INCUBATOR_BENCH_RAGDOLL_01", entry.GerminationTrayId);
        }

        [Fact]
        public void SeedBankPreservationCatalog_Silica_Integrity()
        {
            var catalog = SeedBankPreservationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SilicaEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("silica_seed_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.StorageVesselId));
                Assert.False(string.IsNullOrWhiteSpace(item.DesiccantCompoundType));
                Assert.True(item.SeedBatchMoisturePct > 0);
                Assert.True(item.EquilibriumRhPct > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSilica("silica_seed_cobalt_chloride_indicator_bead_color");
            Assert.NotNull(entry);
            Assert.Equal("HERMETIC_MASON_JAR_BANK_01", entry.StorageVesselId);
        }

        [Fact]
        public void SeedBankPreservationCatalog_Heirloom_Integrity()
        {
            var catalog = SeedBankPreservationCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.HeirloomEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("heirloom_seed_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.LandraceVarietyId));
                Assert.True(item.GenerationCycleNumber > 0);
                Assert.True(item.ParentPopulationSize > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetHeirloom("heirloom_seed_inbreeding_depression_isolation_distance");
            Assert.NotNull(entry);
            Assert.Equal("CHEROKEE_PURPLE_HERITAGE_TOMATO", entry.LandraceVarietyId);
        }
    }
}
