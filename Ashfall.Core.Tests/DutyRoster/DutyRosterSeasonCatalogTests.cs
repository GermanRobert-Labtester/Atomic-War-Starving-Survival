using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class DutyRosterSeasonCatalogTests
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

        private static DutyRosterCatalog LoadCatalog()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Could not find StreamingAssets/Data directory");
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(dataDir);
            Assert.NotNull(catalog);
            return catalog;
        }

        [Fact]
        public void Catalog_LoadsExact8SeasonsWithValidSchema()
        {
            var catalog = LoadCatalog();
            Assert.Equal(8, catalog.Seasons.Count);

            foreach (var s in catalog.Seasons)
            {
                Assert.False(string.IsNullOrWhiteSpace(s.id));
                Assert.StartsWith("season_", s.id);
                Assert.True(s.windowMinDays >= 0, $"Season {s.id} windowMinDays must be >= 0");
                Assert.True(s.windowMaxDays >= s.windowMinDays, $"Season {s.id} windowMaxDays must be >= windowMinDays");
                Assert.True(s.encounterWeight >= 0.5f && s.encounterWeight <= 2.5f,
                    $"Season {s.id} encounterWeight {s.encounterWeight} outside [0.5, 2.5]");
                Assert.True(s.steamTripChanceBoost >= 0.0f && s.steamTripChanceBoost <= 0.15f,
                    $"Season {s.id} steamTripChanceBoost {s.steamTripChanceBoost} outside [0.0, 0.15]");
            }
        }

        [Fact]
        public void Catalog_PreservesSecondWinterIdentityAndValues()
        {
            var catalog = LoadCatalog();
            var secondWinter = catalog.GetSeason(DutyRosterIds.SeasonSecondWinter);
            Assert.NotNull(secondWinter);
            Assert.Equal("season_second_winter", secondWinter.id);
            Assert.Equal(8, secondWinter.windowMinDays);
            Assert.Equal(12, secondWinter.windowMaxDays);
            Assert.Equal(1.6f, secondWinter.encounterWeight, 3);
            Assert.Equal(0.08f, secondWinter.steamTripChanceBoost, 3);
        }

        [Fact]
        public void Catalog_ContainsAll7NewSeasonsWithCorrectIds()
        {
            var catalog = LoadCatalog();
            var expectedIds = new[]
            {
                "season_first_ashfall",
                "season_second_winter",
                "season_settling",
                "season_spring_thaw",
                "season_faction_pressure",
                "season_first_siege",
                "season_consolidation",
                "season_long_winter"
            };

            foreach (var id in expectedIds)
            {
                var s = catalog.GetSeason(id);
                Assert.NotNull(s);
                Assert.Equal(id, s.id);
            }
        }

        [Fact]
        public void Catalog_ZeroDuplicateIds()
        {
            var catalog = LoadCatalog();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var s in catalog.Seasons)
            {
                Assert.True(seen.Add(s.id), $"Duplicate season ID: {s.id}");
            }
            Assert.Equal(8, seen.Count);
        }

        [Fact]
        public void Catalog_ContiguousGapFreeAndNoOverlapsAcross365Days()
        {
            var catalog = LoadCatalog();
            Assert.Equal(8, catalog.Seasons.Count);

            // First season must start at Day 0
            Assert.Equal(0, catalog.Seasons[0].windowMinDays);

            // Each subsequent season must start exactly at prior season's max + 1
            for (int i = 1; i < catalog.Seasons.Count; i++)
            {
                var prev = catalog.Seasons[i - 1];
                var curr = catalog.Seasons[i];
                Assert.Equal(prev.windowMaxDays + 1, curr.windowMinDays);
            }

            // Final season must cover at least up to day 365
            Assert.Equal(365, catalog.Seasons[catalog.Seasons.Count - 1].windowMaxDays);
        }

        [Fact]
        public void Selection_EveryDayFrom0To365ResolvesExactlyOneSeason()
        {
            var catalog = LoadCatalog();
            for (int day = 0; day <= 365; day++)
            {
                var season = catalog.GetSeasonForDay(day);
                Assert.NotNull(season);
                Assert.True(day >= season.windowMinDays && day <= season.windowMaxDays,
                    $"Day {day} resolved to {season.id} which does not contain it");

                // Check that exactly one season in catalog matches
                int matchCount = 0;
                for (int i = 0; i < catalog.Seasons.Count; i++)
                {
                    var s = catalog.Seasons[i];
                    if (day >= s.windowMinDays && day <= s.windowMaxDays)
                        matchCount++;
                }
                Assert.Equal(1, matchCount);
            }
        }

        [Theory]
        [InlineData(0, "season_first_ashfall")]
        [InlineData(7, "season_first_ashfall")]
        [InlineData(8, "season_second_winter")]
        [InlineData(12, "season_second_winter")]
        [InlineData(13, "season_settling")]
        [InlineData(30, "season_settling")]
        [InlineData(31, "season_spring_thaw")]
        [InlineData(60, "season_spring_thaw")]
        [InlineData(61, "season_faction_pressure")]
        [InlineData(120, "season_faction_pressure")]
        [InlineData(121, "season_first_siege")]
        [InlineData(180, "season_first_siege")]
        [InlineData(181, "season_consolidation")]
        [InlineData(240, "season_consolidation")]
        [InlineData(241, "season_long_winter")]
        [InlineData(365, "season_long_winter")]
        public void Selection_ExactTransitionBoundaries(int day, string expectedSeasonId)
        {
            var catalog = LoadCatalog();
            var season = catalog.GetSeasonForDay(day);
            Assert.NotNull(season);
            Assert.Equal(expectedSeasonId, season.id);
        }

        [Fact]
        public void Selection_Post365OverflowFallback()
        {
            var catalog = LoadCatalog();
            // Day 366, 400, 500 must return the last season (season_long_winter)
            Assert.Equal("season_long_winter", catalog.GetSeasonForDay(366)?.id);
            Assert.Equal("season_long_winter", catalog.GetSeasonForDay(400)?.id);
            Assert.Equal("season_long_winter", catalog.GetSeasonForDay(500)?.id);
        }

        [Fact]
        public void Selection_NegativeDayReturnsNull()
        {
            var catalog = LoadCatalog();
            Assert.Null(catalog.GetSeasonForDay(-1));
            Assert.Null(catalog.GetSeasonForDay(-50));
        }

        [Fact]
        public void Selection_LargeDayJumpsResolveCorrectly()
        {
            var catalog = LoadCatalog();
            Assert.Equal("season_first_ashfall", catalog.GetSeasonForDay(5)?.id);
            Assert.Equal("season_settling", catalog.GetSeasonForDay(25)?.id);
            Assert.Equal("season_faction_pressure", catalog.GetSeasonForDay(75)?.id);
            Assert.Equal("season_first_siege", catalog.GetSeasonForDay(150)?.id);
            Assert.Equal("season_consolidation", catalog.GetSeasonForDay(210)?.id);
            Assert.Equal("season_long_winter", catalog.GetSeasonForDay(300)?.id);
        }

        [Fact]
        public void SaveRoundTrip_RestoresSimDayAndReResolvesActiveSeasonCleanly()
        {
            var catalog = LoadCatalog();
            var roster = new DutyRosterSystem();
            var marks = new MoraleMarkSystem();
            var encounters = new ShelterEncounterSystem();
            var clock = new SimClock(150); // Day 150 = season_first_siege
            var quests = new DutyRosterQuestRuntime();

            var save = DutyRosterSaveCodec.Capture(roster, marks, encounters, clock, quests);
            var json = new SystemTextJsonSerializer();
            string jsonText = DutyRosterSaveCodec.Encode(save, json);

            var restoredSave = DutyRosterSaveCodec.Decode(jsonText, json);
            Assert.Equal(150, restoredSave.simDay);

            var activeSeason = catalog.GetSeasonForDay(restoredSave.simDay);
            Assert.NotNull(activeSeason);
            Assert.Equal("season_first_siege", activeSeason.id);
            Assert.Equal(1.75f, activeSeason.encounterWeight, 2);
            Assert.Equal(0.03f, activeSeason.steamTripChanceBoost, 2);
        }
    }
}
