using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class DailySurvivalCatalogTests
    {
        private readonly string _narrativeDir;

        public DailySurvivalCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void DailySurvivalCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = DailySurvivalCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.JournalEntries.Count);
            Assert.Equal(8, catalog.BotanicalEntries.Count);
            Assert.Equal(7, catalog.FolkloreEntries.Count);
            Assert.Equal(7, catalog.FraudEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void DailySurvivalCatalog_Journals_Integrity()
        {
            var catalog = DailySurvivalCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.JournalEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("journal_psych_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.AuthorDesignation));
                Assert.False(string.IsNullOrWhiteSpace(item.QuietHourTime));
                Assert.False(string.IsNullOrWhiteSpace(item.PsychologicalMarker));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetJournal("journal_psych_insomnia_vent_hum");
            Assert.NotNull(entry);
            Assert.Equal("WATCHMAN_ELIAS_SECTOR_3", entry.AuthorDesignation);
        }

        [Fact]
        public void DailySurvivalCatalog_Botanical_Integrity()
        {
            var catalog = DailySurvivalCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.BotanicalEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("botany_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.BotanicalName));
                Assert.False(string.IsNullOrWhiteSpace(item.CultivationTray));
                Assert.False(string.IsNullOrWhiteSpace(item.EdibilityStatus));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetBotanical("botany_sonya_tree_graft_sapling");
            Assert.NotNull(entry);
            Assert.Equal("MALUS_DOMESTICA_HEIRLOOM_SONYA", entry.BotanicalName);
        }

        [Fact]
        public void DailySurvivalCatalog_Folklore_Integrity()
        {
            var catalog = DailySurvivalCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.FolkloreEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("folklore_children_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.TraditionType));
                Assert.False(string.IsNullOrWhiteSpace(item.OriginSector));
                Assert.False(string.IsNullOrWhiteSpace(item.FolkTheme));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetFolklore("folklore_children_the_clicking_beetle_rhyme");
            Assert.NotNull(entry);
            Assert.Equal("JUMP_ROPE_RHYME", entry.TraditionType);
        }

        [Fact]
        public void DailySurvivalCatalog_RationFraud_Integrity()
        {
            var catalog = DailySurvivalCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.FraudEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("fraud_ration_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.CaseId));
                Assert.False(string.IsNullOrWhiteSpace(item.InfractionType));
                Assert.False(string.IsNullOrWhiteSpace(item.AccusedCulprit));
                Assert.False(string.IsNullOrWhiteSpace(item.VerdictPenalty));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetFraud("fraud_ration_sawdust_flour_dilution");
            Assert.NotNull(entry);
            Assert.Equal("BAKER_HANS_MULLER", entry.AccusedCulprit);
        }
    }
}
