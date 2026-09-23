// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    /// <summary>
    /// Plan 74 — Narrative Progression Chapters Expansion: 5 → 15 Campaign Chapters.
    /// Verifies that narrative_progression.json loads 15 chapters with unique, contiguous ordering (1..15).
    /// </summary>
    public sealed class Plan74NarrativeProgressionExpansionTests
    {
        private static string FindDataDir()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return string.Empty;
        }

        private static List<NarrativeProgressionEntry> LoadCatalog()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Could not locate StreamingAssets/Data directory");
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            return NarrativeProgressionCatalogLoader.Load(dataDir, io, json);
        }

        [Fact]
        public void Catalogue_LoadsExactlyFifteenChapters()
        {
            var chapters = LoadCatalog();
            Assert.Equal(15, chapters.Count);
        }

        [Fact]
        public void Catalogue_OrdersAreStrictlyContiguousFromOneToFifteen()
        {
            var chapters = LoadCatalog();
            var orders = chapters.Select(c => c.order).OrderBy(o => o).ToList();

            for (int expected = 1; expected <= 15; expected++)
            {
                Assert.Equal(expected, orders[expected - 1]);
            }
        }

        [Fact]
        public void Catalogue_AllDescriptionsAreNonEmptyAndSubstantive()
        {
            var chapters = LoadCatalog();
            foreach (var chapter in chapters)
            {
                Assert.False(string.IsNullOrWhiteSpace(chapter.description),
                    $"Chapter order {chapter.order} has empty description");
                Assert.True(chapter.description.Length >= 20,
                    $"Chapter order {chapter.order} description is too short ({chapter.description.Length} chars)");
            }
        }

        [Fact]
        public void Catalogue_PreservesOriginalFiveChaptersAndTenExpansions()
        {
            var chapters = LoadCatalog();
            var map = chapters.ToDictionary(c => c.order);

            // Baseline chapters 1-5 preserved
            Assert.Contains("The Exchange", map[1].description);
            Assert.Contains("Ashfall", map[2].description);
            Assert.Contains("The Bunker", map[3].description);
            Assert.Contains("First Contact", map[4].description);
            Assert.Contains("The Long Winter", map[5].description);

            // Expanded chapters 6-15 present
            Assert.Contains("The Consolidation", map[6].description);
            Assert.Contains("The Long Dark", map[7].description);
            Assert.Contains("The Thaw", map[8].description);
            Assert.Contains("The Schism", map[9].description);
            Assert.Contains("The Black Market", map[10].description);
            Assert.Contains("The Reckoning", map[11].description);
            Assert.Contains("The Rebuilding", map[12].description);
            Assert.Contains("The Second Winter", map[13].description);
            Assert.Contains("The Muster", map[14].description);
            Assert.Contains("The Inheritance", map[15].description);
        }
    }
}
