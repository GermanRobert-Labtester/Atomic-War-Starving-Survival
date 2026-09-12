// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class JournalCorpusTests
    {
        private sealed class TestAuthor : ISurvivorAuthor
        {
            public TestAuthor(string id, string name, RiskBiasTrait bias = RiskBiasTrait.Realist)
            {
                Id = id;
                DisplayName = name;
                RiskBias = bias;
            }

            public string Id { get; }
            public string DisplayName { get; }
            public RiskBiasTrait RiskBias { get; }
        }

        [Fact]
        public void Loader_NormalizesAllPlan142Sources_WithoutDuplicateKeys()
        {
            string dataDir = FindDataDirectory();
            var loader = new JournalCorpusCatalogLoader(
                new FileSystemIO(),
                new SystemTextJsonSerializer());

            JournalCorpusCatalog catalog = loader.Load(dataDir);

            Assert.Equal(189, catalog.Count);
            Assert.Contains(catalog.Records,
                r => r.Id == "journal_day_32_rationing_decision"
                    && r.KnowledgeKey == r.Id
                    && r.Hour < 0f
                    && r.Timestamp == "Day 32"
                    && r.Title == "Day 32: The Rationing Debate");
            Assert.Contains(catalog.Records,
                r => r.Id == "journal_raw_01_geo_thermal_haze"
                    && r.KnowledgeKey == "location_geo_thermal_plant_ruins"
                    && r.Hour == 14f
                    && r.Timestamp == "Day 47, 14h");
        }

        [Fact]
        public void Adapter_UsesAuthoredBodyIdentityAndTime_AndDoesNotFloodOnBind()
        {
            string dataDir = FindDataDirectory();
            var catalog = new JournalCorpusCatalogLoader(
                new FileSystemIO(),
                new SystemTextJsonSerializer()).Load(dataDir);
            var elena = new TestAuthor("elena_vasquez", "Elena Vasquez", RiskBiasTrait.Cautious);
            var journal = new JournalSystem();

            journal.BindAuthoredCorpus(new JournalCorpusAdapter(catalog, new[] { elena }));

            Assert.Equal(0, journal.EntryCount);

            var authored = journal.TryAddRawEntry(
                "journal_day_32_rationing_decision",
                "Producer fallback text must not replace authored prose.",
                null!,
                day: 999,
                hour: 23f);

            Assert.NotNull(authored);
            Assert.Equal("journal_day_32_rationing_decision", authored!.Id);
            Assert.Equal("Day 32: The Rationing Debate", catalog.Records[0].Title);
            Assert.Contains("bunker is running low on food", authored.Text);
            Assert.Equal("elena_vasquez", authored.AuthorId);
            Assert.Equal("Elena Vasquez", authored.AuthorName);
            Assert.Equal(32, authored.Day);
            Assert.Equal(-1f, authored.Hour);
            Assert.Equal("Day 32", authored.Timestamp);
        }

        [Fact]
        public void Adapter_UnresolvedAmbientAuthorRemainsDisplayOnly()
        {
            string dataDir = FindDataDirectory();
            var catalog = new JournalCorpusCatalogLoader(
                new FileSystemIO(),
                new SystemTextJsonSerializer()).Load(dataDir);
            var journal = new JournalSystem();
            journal.BindAuthoredCorpus(new JournalCorpusAdapter(catalog));

            var entry = journal.TryDiscoverKnowledge(
                "journal_qm_01_first_count",
                new TestAuthor("fallback", "Fallback"),
                day: 400,
                hour: 4f);

            Assert.NotNull(entry);
            Assert.Equal("quartermaster_yelena", entry!.AuthorName);
            Assert.Equal(string.Empty, entry.AuthorId);
            Assert.Equal(9, entry.Day);
            Assert.Equal(-1f, entry.Hour);
            Assert.Equal("Day 9", entry.Timestamp);
            Assert.Equal(1, journal.CodexUnlockCount);
        }

        [Fact]
        public void Adapter_RetryAfterSaveRestoreIsIdempotentAndEventSilent()
        {
            string dataDir = FindDataDirectory();
            var catalog = new JournalCorpusCatalogLoader(
                new FileSystemIO(),
                new SystemTextJsonSerializer()).Load(dataDir);
            var adapter = new JournalCorpusAdapter(catalog);
            var source = new JournalSystem();
            source.BindAuthoredCorpus(adapter);
            Assert.NotNull(source.TryAddRawEntry(
                "journal_raw_01_geo_thermal_haze",
                "ignored",
                null!,
                1));

            var restored = new JournalSystem();
            restored.BindAuthoredCorpus(adapter);
            int entriesAdded = 0;
            int pings = 0;
            restored.OnEntryAdded += _ => entriesAdded++;
            restored.OnNotificationPing += _ => pings++;
            restored.RestoreState(source.CaptureState());

            Assert.Equal(0, entriesAdded);
            Assert.Equal(0, pings);
            Assert.Null(restored.TryAddRawEntry(
                "location_geo_thermal_plant_ruins",
                "retry",
                null!,
                2));
            Assert.Single(restored.Entries);
            Assert.Equal(0, entriesAdded);
            Assert.Equal(0, pings);
        }

        [Fact]
        public void AuthoredActivation_IsProducerBoundAndIgnoresUnknownKeys()
        {
            string dataDir = FindDataDirectory();
            var catalog = new JournalCorpusCatalogLoader(
                new FileSystemIO(),
                new SystemTextJsonSerializer()).Load(dataDir);
            var journal = new JournalSystem();
            journal.BindAuthoredCorpus(new JournalCorpusAdapter(catalog));

            Assert.Null(journal.TryAddAuthoredEntry("generated_key"));
            var entry = journal.TryAddAuthoredEntry("location_geo_thermal_plant_ruins");

            Assert.NotNull(entry);
            Assert.Equal("journal_raw_01_geo_thermal_haze", entry!.Id);
            Assert.Equal("location_geo_thermal_plant_ruins", entry.KnowledgeKey);
            Assert.Equal(47, entry.Day);
            Assert.Equal(14f, entry.Hour);
        }

        [Fact]
        public void Catalog_RejectsDuplicateIdsAndKnowledgeKeys()
        {
            var catalog = new JournalCorpusCatalog();
            catalog.Add(new JournalCorpusRecord
            {
                Id = "authored_a",
                KnowledgeKey = "knowledge_a",
                Text = "A",
                Day = 1
            });

            Assert.Throws<JournalCorpusFormatException>(() => catalog.Add(
                new JournalCorpusRecord
                {
                    Id = "authored_a",
                    KnowledgeKey = "knowledge_b",
                    Text = "B",
                    Day = 2
                }));
            Assert.Throws<JournalCorpusFormatException>(() => catalog.Add(
                new JournalCorpusRecord
                {
                    Id = "authored_b",
                    KnowledgeKey = "knowledge_a",
                    Text = "B",
                    Day = 2
                }));
        }

        [Fact]
        public void Loader_RejectsInvalidTimestampFixture()
        {
            string root = Path.Combine(
                Path.GetTempPath(),
                "ashfall-plan142-invalid-journal-" + Environment.ProcessId);
            string narrative = Path.Combine(root, "narrative");
            Directory.CreateDirectory(narrative);
            try
            {
                File.WriteAllText(
                    Path.Combine(narrative, "journal_entries_batch_1.json"),
                    "{\"entries\":[{\"id\":\"bad_entry\",\"text\":\"body\",\"timestamp\":\"Day 2, 09h\",\"author_name\":\"A\",\"author_id\":\"a\",\"knowledge_key\":\"bad_key\",\"day\":2,\"hour\":8}]}");

                var loader = new JournalCorpusCatalogLoader(
                    new FileSystemIO(),
                    new SystemTextJsonSerializer());
                Assert.Throws<JournalCorpusFormatException>(() => loader.Load(root));
            }
            finally
            {
                if (Directory.Exists(root))
                    Directory.Delete(root, recursive: true);
            }
        }

        private static string FindDataDirectory()
        {
            if (CatalogLocator.TryFindDataDirectory(
                    Directory.GetCurrentDirectory(),
                    out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(
                    AppContext.BaseDirectory,
                    out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found.");
        }
    }
}
