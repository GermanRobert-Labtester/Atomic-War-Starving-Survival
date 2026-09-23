// SPDX-License-Identifier: MIT
// Plan 83 × Plan 74 cross-system integration test
// Plan 83 — Weather Season Windows Expansion (3 → 10 season windows)
// Plan 74 — Narrative Progression Chapters Expansion (5 → 15 campaign chapters)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public sealed class Plan83_74WeatherNarrativeIntegrationTests
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

        // ── Weather Seasons (Plan 83) ──────────────────────────────────────────

        [Fact]
        public void WeatherSeasons_HasTenWindowsWithStrictlyIncreasingStartDays()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var profile = WeatherProfileLoader.Load(dataDir, io, json);

            Assert.NotNull(profile);
            Assert.Equal(10, profile.seasons.Count);

            var ids = new HashSet<string>();
            int lastStartDay = -1;

            foreach (var season in profile.seasons)
            {
                Assert.False(string.IsNullOrEmpty(season.id), "Season has empty id");
                Assert.True(ids.Add(season.id), $"Duplicate season id: {season.id}");
                Assert.True(season.startDay > lastStartDay,
                    $"Season '{season.id}' startDay {season.startDay} not strictly greater than previous {lastStartDay}");
                lastStartDay = season.startDay;

                // Weights within reasonable bounds (0.0 .. 5.0)
                Assert.True(season.clearWeight >= 0f && season.clearWeight <= 5f);
                Assert.True(season.rainWeight >= 0f && season.rainWeight <= 5f);
                Assert.True(season.overcastWeight >= 0f && season.overcastWeight <= 5f);
                Assert.True(season.ashfallWeight >= 0f && season.ashfallWeight <= 5f);
                Assert.True(season.falloutStormWeight >= 0f && season.falloutStormWeight <= 5f);
                Assert.True(season.blizzardWeight >= 0f && season.blizzardWeight <= 5f);
                Assert.True(season.blackRainWeight >= 0f && season.blackRainWeight <= 5f);
            }
        }

        // ── Narrative Progression (Plan 74) ─────────────────────────────────────

        [Fact]
        public void NarrativeProgression_HasFifteenChaptersWithContiguousOrder()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var chapters = NarrativeProgressionCatalogLoader.Load(dataDir, io, json);

            Assert.Equal(15, chapters.Count);

            var orders = chapters.Select(c => c.order).OrderBy(o => o).ToList();
            for (int i = 0; i < 15; i++)
            {
                Assert.Equal(i + 1, orders[i]);
                Assert.False(string.IsNullOrWhiteSpace(chapters[i].description),
                    $"Chapter at index {i} has empty description");
            }
        }

        // ── Cross-System Coherence ──────────────────────────────────────────────

        [Fact]
        public void CrossSystem_BothCatalogsLoadIndependentlyAndProvideFullCampaignCoverage()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var weatherProfile = WeatherProfileLoader.Load(dataDir, io, json);
            var chapters = NarrativeProgressionCatalogLoader.Load(dataDir, io, json);

            Assert.NotNull(weatherProfile);
            Assert.True(weatherProfile.seasons.Count >= 10,
                $"Weather catalog must contain >= 10 season windows; got {weatherProfile.seasons.Count}");

            Assert.True(chapters.Count >= 15,
                $"Narrative progression must contain >= 15 chapters; got {chapters.Count}");

            // Weather seasons start at day 0 and extend to day 280+
            Assert.Equal(0, weatherProfile.seasons.First().startDay);
            Assert.True(weatherProfile.seasons.Last().startDay >= 240,
                "Final weather season window should start at or after day 240");

            // Narrative progression starts at Chapter 1 and concludes at Chapter 15
            Assert.Equal(1, chapters.Min(c => c.order));
            Assert.Equal(15, chapters.Max(c => c.order));
        }

        [Fact]
        public void CrossSystem_WeatherAndNarrativeReflectCampaignEvolutionTones()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var weatherProfile = WeatherProfileLoader.Load(dataDir, io, json);
            var chapters = NarrativeProgressionCatalogLoader.Load(dataDir, io, json);

            // Early game: First Thaw weather exists alongside Chapter 1 (The Exchange) / Chapter 2 (Ashfall)
            var firstThaw = weatherProfile.seasons.FirstOrDefault(s => s.id == "window_first_thaw");
            Assert.NotNull(firstThaw);

            var chapter1 = chapters.FirstOrDefault(c => c.order == 1);
            Assert.NotNull(chapter1);
            Assert.Contains("The Exchange", chapter1.description);

            // Late game: Black Rain Season weather exists alongside Chapter 14 (The Muster) / Chapter 15 (The Inheritance)
            var blackRainSeason = weatherProfile.seasons.FirstOrDefault(s => s.id == "window_black_rain_season");
            Assert.NotNull(blackRainSeason);
            Assert.True(blackRainSeason.blackRainWeight >= 2.0f);

            var chapter15 = chapters.FirstOrDefault(c => c.order == 15);
            Assert.NotNull(chapter15);
            Assert.Contains("The Inheritance", chapter15.description);
        }

    }
}
