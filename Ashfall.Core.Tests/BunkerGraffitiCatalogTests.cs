using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class BunkerGraffitiCatalogTests
    : CatalogTestBase{
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void BunkerGraffiti_LoadsAll36CanonicalPostings()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "bunker_graffiti_postings.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerGraffitiCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(36, catalog.AllPostings.Count);

            // Test first posting (Stoker Rule Day 3)
            var first = catalog.GetById("graf_01_the_first_stoker_rule");
            Assert.NotNull(first);
            Assert.Equal("Fyodor the Stoker", first.author_signature);
            Assert.Equal(3, first.recorded_day);
            Assert.Contains("LIGNITE", first.content);

            // Test final posting (Sonya Day 3650)
            var final = catalog.GetById("graf_36_the_final_slate_greeting");
            Assert.NotNull(final);
            Assert.Equal("Sonya, Council President", final.author_signature);
            Assert.Equal(3650, final.recorded_day);
            Assert.Contains("Love one another in the daylight", final.content);

            // Test day filtering
            var yearOneGraffiti = catalog.GetUnlockedByDay(360);
            Assert.Equal(15, yearOneGraffiti.Count);

            // Test category search
            var culinary = catalog.GetByCategory("Culinary");
            Assert.True(culinary.Count >= 2);
        }

        [Fact]
        public void BunkerGraffiti_AllEntriesHaveValidContentAndMorale()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "bunker_graffiti_postings.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerGraffitiCatalog();
            catalog.Load(json, serializer);

            foreach (var p in catalog.AllPostings)
            {
                Assert.False(string.IsNullOrWhiteSpace(p.posting_id), "Missing posting_id");
                Assert.True(p.recorded_day > 0, $"Invalid recorded_day on {p.posting_id}");
                Assert.False(string.IsNullOrWhiteSpace(p.location), $"Missing location on {p.posting_id}");
                Assert.False(string.IsNullOrWhiteSpace(p.medium), $"Missing medium on {p.posting_id}");
                Assert.False(string.IsNullOrWhiteSpace(p.author_signature), $"Missing author on {p.posting_id}");
                Assert.False(string.IsNullOrWhiteSpace(p.category), $"Missing category on {p.posting_id}");
                Assert.False(string.IsNullOrWhiteSpace(p.content), $"Missing content on {p.posting_id}");
                Assert.False(string.IsNullOrWhiteSpace(p.morale_effect), $"Missing morale effect on {p.posting_id}");
                Assert.NotNull(p.tags);
                Assert.True(p.tags.Length > 0, $"Tags empty on {p.posting_id}");
            }
        }
    }
}
