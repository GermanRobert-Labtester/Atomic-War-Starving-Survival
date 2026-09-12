// SPDX-License-Identifier: MIT
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

        [Fact]
        public void BunkerGraffiti_IdempotentLoading_DoesNotDuplicateOnRepeat()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "bunker_graffiti_postings.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerGraffitiCatalog();

            // First load: exactly 36
            catalog.Load(json, serializer);
            Assert.Equal(36, catalog.AllPostings.Count);
            Assert.Equal(36, catalog.Count);

            // Second load with identical payload: must remain exactly 36
            catalog.Load(json, serializer);
            Assert.Equal(36, catalog.AllPostings.Count);
            Assert.Equal(36, catalog.Count);

            // Clear: resets to 0
            catalog.Clear();
            Assert.Equal(0, catalog.Count);
            Assert.Empty(catalog.AllPostings);

            // Reload after clear: restores to 36
            catalog.Load(json, serializer);
            Assert.Equal(36, catalog.Count);
        }

        [Fact]
        public void BunkerGraffiti_LoadFromDirectory_LoadsAll76Postings()
        {
            string dataDir = FindDataDir();
            var serializer = new SystemTextJsonSerializer();
            var fileIo = new FileSystemIO();

            var catalog = BunkerGraffitiCatalog.LoadFromDirectory(dataDir, fileIo, serializer);

            // 36 canonical base + 40 expansion = 76 unique postings
            Assert.Equal(76, catalog.Count);
            Assert.Equal(76, catalog.AllPostings.Count);

            // Check sample from base
            var baseEntry = catalog.GetById("graf_01_the_first_stoker_rule");
            Assert.NotNull(baseEntry);
            Assert.Equal(3, baseEntry.recorded_day);

            // Check sample from expansion
            var expEntry = catalog.GetById("graf_dir_01_pump");
            Assert.NotNull(expEntry);
            Assert.Equal(5, expEntry.recorded_day);
            Assert.Equal("South corridor junction", expEntry.location);

            // Duplicate reload check: calling Load again with expansion json does not increase count
            string expPath = Path.Combine(dataDir, "narrative", "graffiti_expansion.json");
            catalog.Load(File.ReadAllText(expPath), serializer);
            Assert.Equal(76, catalog.Count);
        }

        [Fact]
        public void BunkerGraffiti_DuplicateAndCaseInsensitiveRejection()
        {
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerGraffitiCatalog();

            string testJson = @"
            {
                ""schema_version"": 1,
                ""collection_id"": ""test"",
                ""postings"": [
                    {
                        ""posting_id"": ""graf_test_01"",
                        ""recorded_day"": 1,
                        ""location"": ""Corridor"",
                        ""medium"": ""Chalk"",
                        ""author_signature"": ""Someone"",
                        ""category"": ""Test"",
                        ""content"": ""First"",
                        ""morale_effect"": ""none"",
                        ""tags"": [""tag1""]
                    },
                    {
                        ""posting_id"": ""graf_test_01"",
                        ""recorded_day"": 2,
                        ""location"": ""Corridor"",
                        ""medium"": ""Chalk"",
                        ""author_signature"": ""Someone"",
                        ""category"": ""Test"",
                        ""content"": ""Duplicate exact ID"",
                        ""morale_effect"": ""none"",
                        ""tags"": [""tag1""]
                    },
                    {
                        ""posting_id"": ""GRAF_TEST_01"",
                        ""recorded_day"": 3,
                        ""location"": ""Corridor"",
                        ""medium"": ""Chalk"",
                        ""author_signature"": ""Someone"",
                        ""category"": ""Test"",
                        ""content"": ""Duplicate uppercase ID"",
                        ""morale_effect"": ""none"",
                        ""tags"": [""tag1""]
                    },
                    {
                        ""posting_id"": ""graf_test_02"",
                        ""recorded_day"": 4,
                        ""location"": ""Kitchen"",
                        ""medium"": ""Paint"",
                        ""author_signature"": ""Chef"",
                        ""category"": ""Test"",
                        ""content"": ""Second unique"",
                        ""morale_effect"": ""none"",
                        ""tags"": [""tag2""]
                    }
                ]
            }";

            catalog.Load(testJson, serializer);
            Assert.Equal(2, catalog.Count);
            Assert.Equal("First", catalog.GetById("graf_test_01")?.content);
            Assert.Equal("Second unique", catalog.GetById("graf_test_02")?.content);
        }

        [Fact]
        public void BunkerGraffiti_MalformedEntries_AreSafelyIgnored()
        {
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerGraffitiCatalog();

            string testJson = @"
            {
                ""schema_version"": 1,
                ""collection_id"": ""test"",
                ""postings"": [
                    null,
                    {
                        ""posting_id"": """",
                        ""recorded_day"": 1,
                        ""location"": ""Corridor"",
                        ""content"": ""No ID""
                    },
                    {
                        ""posting_id"": ""graf_bad_content"",
                        ""recorded_day"": 1,
                        ""location"": ""Corridor"",
                        ""content"": ""   ""
                    },
                    {
                        ""posting_id"": ""graf_neg_day"",
                        ""recorded_day"": -5,
                        ""location"": ""Corridor"",
                        ""content"": ""Negative Day""
                    },
                    {
                        ""posting_id"": ""graf_valid"",
                        ""recorded_day"": 10,
                        ""location"": ""Corridor"",
                        ""content"": ""Valid content""
                    }
                ]
            }";

            catalog.Load(testJson, serializer);
            Assert.Equal(1, catalog.Count);
            Assert.NotNull(catalog.GetById("graf_valid"));
        }

        [Fact]
        public void BunkerGraffiti_DayFiltering_CorrectBoundaries()
        {
            string dataDir = FindDataDir();
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerGraffitiCatalog();
            string json = File.ReadAllText(Path.Combine(dataDir, "narrative", "bunker_graffiti_postings.json"));
            catalog.Load(json, serializer);

            // graf_01 is recorded_day: 3
            var day2 = catalog.GetUnlockedByDay(2);
            Assert.DoesNotContain(day2, p => p.posting_id == "graf_01_the_first_stoker_rule");

            var day3 = catalog.GetUnlockedByDay(3);
            Assert.Contains(day3, p => p.posting_id == "graf_01_the_first_stoker_rule");

            var day4 = catalog.GetUnlockedByDay(4);
            Assert.Contains(day4, p => p.posting_id == "graf_01_the_first_stoker_rule");

            // Negative day returns empty
            Assert.Empty(catalog.GetUnlockedByDay(-1));

            // All unlocked on Day 3650
            Assert.Equal(36, catalog.GetUnlockedByDay(3650).Count);
        }

        [Fact]
        public void BunkerGraffiti_DeterministicSortOrder()
        {
            string dataDir = FindDataDir();
            var serializer = new SystemTextJsonSerializer();
            var fileIo = new FileSystemIO();
            var catalog = BunkerGraffitiCatalog.LoadFromDirectory(dataDir, fileIo, serializer);

            Assert.Equal(76, catalog.Count);
            for (int i = 1; i < catalog.AllPostings.Count; i++)
            {
                var prev = catalog.AllPostings[i - 1];
                var curr = catalog.AllPostings[i];

                Assert.True(curr.recorded_day >= prev.recorded_day,
                    $"Order violation: Day {curr.recorded_day} preceded by Day {prev.recorded_day}");

                if (curr.recorded_day == prev.recorded_day)
                {
                    Assert.True(string.Compare(curr.posting_id, prev.posting_id, StringComparison.Ordinal) >= 0,
                        $"ID tie-breaker violation: {curr.posting_id} after {prev.posting_id}");
                }
            }
        }

        [Fact]
        public void BunkerGraffiti_LocationProjection_ResolvesExpectedTargets()
        {
            string dataDir = FindDataDir();
            var serializer = new SystemTextJsonSerializer();
            var fileIo = new FileSystemIO();
            var catalog = BunkerGraffitiCatalog.LoadFromDirectory(dataDir, fileIo, serializer);

            // All 76 postings must map to a recognized canonical target
            foreach (var p in catalog.AllPostings)
            {
                string target = BunkerGraffitiProjection.ResolveCanonicalTarget(p);
                Assert.False(string.IsNullOrWhiteSpace(target), $"Posting {p.posting_id} resolved to empty target");
                Assert.True(target.StartsWith("room_") || target.StartsWith("loc") || target.StartsWith("rural_") || target.StartsWith("suburban_") || target.StartsWith("government_") || target.StartsWith("stranger_"),
                    $"Posting {p.posting_id} resolved to unexpected target '{target}'");
            }

            // Verify room counts
            var kitchenPostings = catalog.GetPostingsForRoom("room_kitchen");
            Assert.Equal(8, kitchenPostings.Count);

            var waterPumpPostings = catalog.GetPostingsForRoom("room_water_pump");
            Assert.Equal(5, waterPumpPostings.Count);

            var clinicPostings = catalog.GetPostingsForRoom("room_clinic");
            Assert.Equal(6, clinicPostings.Count);

            // Isolation test: pump posting must NEVER appear in the clinic
            Assert.DoesNotContain(clinicPostings, p => p.posting_id == "graf_dir_01_pump");
            Assert.Contains(waterPumpPostings, p => p.posting_id == "graf_dir_01_pump");

            // World location query test
            var dataCenterPostings = catalog.GetPostingsForLocation("location_submerged_data_center");
            Assert.Single(dataCenterPostings);
            Assert.Equal("graf_warn_14_gas", dataCenterPostings[0].posting_id);

            // Day gating on room query
            var earlyKitchen = catalog.GetPostingsForRoom("room_kitchen", currentDay: 15);
            Assert.True(earlyKitchen.Count > 0 && earlyKitchen.Count < 8);
        }

        [Fact]
        public void BunkerGraffiti_MoraleEffect_IsDescriptiveStringOnly()
        {
            string dataDir = FindDataDir();
            var serializer = new SystemTextJsonSerializer();
            var fileIo = new FileSystemIO();
            var catalog = BunkerGraffitiCatalog.LoadFromDirectory(dataDir, fileIo, serializer);

            foreach (var p in catalog.AllPostings)
            {
                Assert.NotNull(p.morale_effect);
                Assert.IsType<string>(p.morale_effect);
                Assert.NotEmpty(p.morale_effect.Trim());
            }
        }
    }
}
