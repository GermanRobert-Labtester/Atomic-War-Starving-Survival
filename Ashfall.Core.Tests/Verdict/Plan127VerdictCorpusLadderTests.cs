#nullable enable
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Verdict;
using Xunit;

namespace Ashfall.Core.Tests.Verdict
{
    public sealed class Plan127VerdictCorpusLadderTests
    {
        private static string ResolveDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void VerdictData_LoadsAll25CorruptionCorpusStrings()
        {
            string dir = ResolveDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var corpus = VerdictCatalogLoader.LoadCorruptionCorpus(dir, io, json);

            Assert.NotNull(corpus);
            Assert.Equal(25, corpus.Count);

            foreach (var line in corpus)
            {
                Assert.False(string.IsNullOrWhiteSpace(line));
            }

            // Verify strings are unique
            var distinct = corpus.Distinct(StringComparer.Ordinal).ToList();
            Assert.Equal(25, distinct.Count);

            // Verify specific atmospheric fragments
            Assert.Contains(corpus, c => c.Contains("CENSUS WINDOW OPEN", StringComparison.OrdinalIgnoreCase));
            Assert.Contains(corpus, c => c.Contains("held pending count", StringComparison.OrdinalIgnoreCase));
            Assert.Contains(corpus, c => c.Contains("counting house online", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void VerdictData_LoadsAll12WorldHistoryLadderEntries()
        {
            string dir = ResolveDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var ladder = VerdictCatalogLoader.LoadWorldHistoryLadder(dir, io, json);

            Assert.NotNull(ladder);
            Assert.Equal(12, ladder.Count);

            for (int i = 0; i < ladder.Count; i++)
            {
                var entry = ladder[i];
                Assert.Equal(i + 1, entry.layer);
                Assert.False(string.IsNullOrWhiteSpace(entry.knowledge_key), $"Layer {entry.layer} missing knowledge_key");
                Assert.False(string.IsNullOrWhiteSpace(entry.title), $"Layer {entry.layer} missing title");
                Assert.False(string.IsNullOrWhiteSpace(entry.discovery_location_id), $"Layer {entry.layer} missing discovery_location_id");
                Assert.False(string.IsNullOrWhiteSpace(entry.body_summary), $"Layer {entry.layer} missing body_summary");
            }

            // Unique knowledge keys
            var distinctKeys = ladder.Select(l => l.knowledge_key).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            Assert.Equal(12, distinctKeys.Count);

            // Verify specific layers
            Assert.Equal("The First Geophone Pit", ladder[0].title);
            Assert.Equal("lore_verdict_geophone_one", ladder[0].knowledge_key);
            Assert.Equal("The Open Count", ladder[11].title);
            Assert.Equal("lore_verdict_open_count", ladder[11].knowledge_key);
        }

        [Fact]
        public void MachineLogSystem_InjectsCorruptionMarkersFromExpandedCorpus()
        {
            string dir = ResolveDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var corpus = VerdictCatalogLoader.LoadCorruptionCorpus(dir, io, json);
            Assert.Equal(25, corpus.Count);

            var system = new MachineLogSystem();
            var rng = new StubRng(7); // Deterministic index

            bool posted = system.InsertCorruptionMarker(day: 42, rng: rng, corpus: corpus);
            Assert.True(posted);

            var entries = system.Entries;
            Assert.Single(entries);
            var entry = entries[0];

            Assert.Equal(42, entry.day);
            Assert.Equal("corruption", entry.facilityId);
            Assert.Equal("anomaly", entry.kind);
            Assert.Contains(entry.bodyShort, corpus);
        }
    }
}
