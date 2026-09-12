// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class BunkerCourtCatalogTests : CatalogTestBase
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void BunkerCourt_LoadsAll24CanonicalTrials()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "bunker_court_verdicts_codex.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerCourtCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(24, catalog.AllCases.Count);

            // Test first case (The Air Duct Moonshine Still)
            var c1 = catalog.GetById("case_01_the_air_duct_moonshine_still");
            Assert.NotNull(c1);
            Assert.Equal("TRIB-084-MOONSHINE", c1.docket_number);
            Assert.Equal("Ilya Morozov (Instrument Technician)", c1.defendant_name);
            Assert.Contains("solder joints were immaculate", c1.clerk_margin_notes);

            // Test search by defendant
            var ilyaCases = catalog.GetByDefendant("Ilya");
            Assert.True(ilyaCases.Count >= 2); // Moonshine, Intercom Prank, Battery Acid

            // Test final case (Constitution Ratification)
            var c24 = catalog.GetById("case_24_the_ratification_of_the_century_constitution");
            Assert.NotNull(c24);
            Assert.Equal("TRIB-3650-CONSTITUTION", c24.docket_number);
            Assert.Contains("struck the brass anvil three times", c24.clerk_margin_notes);

            // Test tag search
            var humor = catalog.GetByTag("humor");
            Assert.True(humor.Count >= 6);
        }

        [Fact]
        public void BunkerCourt_AllEntriesHaveValidFieldsAndUniqueDockets()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "bunker_court_verdicts_codex.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerCourtCatalog();
            catalog.Load(json, serializer);

            var seenDockets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var c in catalog.AllCases)
            {
                Assert.False(string.IsNullOrWhiteSpace(c.case_id), "Missing case_id");
                Assert.True(seenIds.Add(c.case_id), $"Duplicate case_id: {c.case_id}");

                Assert.False(string.IsNullOrWhiteSpace(c.docket_number), $"Missing docket_number on {c.case_id}");
                Assert.True(seenDockets.Add(c.docket_number), $"Duplicate docket: {c.docket_number}");

                Assert.False(string.IsNullOrWhiteSpace(c.defendant_name), $"Missing defendant on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.presiding_magistrate), $"Missing magistrate on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.charge_summary), $"Missing charges on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.evidence_presented), $"Missing evidence on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.verdict_outcome), $"Missing verdict on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.disciplinary_penalty), $"Missing penalty on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.clerk_margin_notes), $"Missing clerk notes on {c.case_id}");
                Assert.True(c.clerk_margin_notes.Length > 25, $"Clerk notes too brief on {c.case_id}");
                Assert.NotNull(c.tags);
                Assert.True(c.tags.Length > 0, $"Tags empty on {c.case_id}");
            }
        }

        [Fact]
        public void BunkerCourt_LoadIsIdempotent()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "bunker_court_verdicts_codex.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerCourtCatalog();

            // First load
            catalog.Load(json, serializer);
            Assert.Equal(24, catalog.Count);
            Assert.Equal(24, catalog.AllCases.Count);

            // Second load with identical JSON should not duplicate entries
            catalog.Load(json, serializer);
            Assert.Equal(24, catalog.Count);
            Assert.Equal(24, catalog.AllCases.Count);

            // Third load
            catalog.Load(json, serializer);
            Assert.Equal(24, catalog.Count);
            Assert.Equal(24, catalog.AllCases.Count);
        }

        [Fact]
        public void BunkerCourt_ClearResetsState()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "bunker_court_verdicts_codex.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerCourtCatalog();

            catalog.Load(json, serializer);
            Assert.Equal(24, catalog.Count);

            catalog.Clear();
            Assert.Equal(0, catalog.Count);
            Assert.Empty(catalog.AllCases);
            Assert.Null(catalog.GetById("case_01_the_air_duct_moonshine_still"));
            Assert.Null(catalog.GetByDocket("TRIB-084-MOONSHINE"));
        }

        [Fact]
        public void BunkerCourt_DeterministicSortOrder()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "bunker_court_verdicts_codex.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerCourtCatalog();
            catalog.Load(json, serializer);

            int previousDay = 0;
            foreach (var c in catalog.AllCases)
            {
                int currentDay = c.GetDocketDay();
                Assert.True(currentDay >= previousDay,
                    $"Cases not ordered by day: {c.case_id} day {currentDay} < previous {previousDay}");
                previousDay = currentDay;
            }

            // Verify first is Day 84 and last is Day 3650
            Assert.Equal(84, catalog.AllCases[0].GetDocketDay());
            Assert.Equal(3650, catalog.AllCases[catalog.AllCases.Count - 1].GetDocketDay());
        }

        [Fact]
        public void BunkerCourt_DocketNumberParsing()
        {
            var entry = new BunkerCourtCaseEntry { docket_number = "TRIB-084-MOONSHINE" };
            Assert.Equal(84, entry.GetDocketDay());

            entry.docket_number = "TRIB-1020-BATTERY";
            Assert.Equal(1020, entry.GetDocketDay());

            entry.docket_number = "TRIB-3650-CONSTITUTION";
            Assert.Equal(3650, entry.GetDocketDay());

            entry.docket_number = "INVALID-FORMAT";
            Assert.Equal(0, entry.GetDocketDay());

            entry.docket_number = "";
            Assert.Equal(0, entry.GetDocketDay());

            entry.docket_number = null!;
            Assert.Equal(0, entry.GetDocketDay());
        }

        [Fact]
        public void BunkerCourt_QueryHelpers()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "bunker_court_verdicts_codex.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerCourtCatalog();
            catalog.Load(json, serializer);

            // GetByDocket
            var byDocket = catalog.GetByDocket("trib-084-moonshine");
            Assert.NotNull(byDocket);
            Assert.Equal("case_01_the_air_duct_moonshine_still", byDocket.case_id);

            // GetByMagistrate
            var byMagistrate = catalog.GetByMagistrate("Dmitri");
            Assert.True(byMagistrate.Count >= 5);

            // GetByVerdict
            var guilty = catalog.GetByVerdict("Guilty");
            Assert.True(guilty.Count >= 10);

            // GetUnlockedByDay
            var unlockedDay200 = catalog.GetUnlockedByDay(200);
            Assert.Equal(4, unlockedDay200.Count); // Cases 1-4 (days 84, 112, 145, 188)

            var unlockedDay100 = catalog.GetUnlockedByDay(100);
            Assert.Single(unlockedDay100); // Case 1 (day 84)

            // GetBySearch
            var searchMoonshine = catalog.GetBySearch("moonshine");
            Assert.True(searchMoonshine.Count >= 1);

            var searchAnvil = catalog.GetBySearch("brass anvil");
            Assert.Single(searchAnvil);
            Assert.Equal("case_24_the_ratification_of_the_century_constitution", searchAnvil[0].case_id);

            var searchNonexistent = catalog.GetBySearch("zyxwvutsrqp");
            Assert.Empty(searchNonexistent);
        }

        [Fact]
        public void BunkerCourt_RejectsMalformedEntries()
        {
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerCourtCatalog();

            string malformedJson = @"{
                ""schema_version"": 1,
                ""collection_id"": ""test"",
                ""cases"": [
                    null,
                    { ""case_id"": """", ""docket_number"": ""TRIB-001-TEST"" },
                    { ""case_id"": ""valid_1"", ""docket_number"": """" },
                    { ""case_id"": ""valid_2"", ""docket_number"": ""TRIB-002-TEST"", ""defendant_name"": ""Tester"" }
                ]
            }";

            catalog.Load(malformedJson, serializer);
            Assert.Single(catalog.AllCases);
            Assert.Equal("valid_2", catalog.AllCases[0].case_id);
        }

        [Fact]
        public void BunkerCourt_LoadFromDirectory()
        {
            string dataDir = FindDataDir();
            var io = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var catalog = BunkerCourtCatalog.LoadFromDirectory(dataDir, io, serializer);
            Assert.Equal(24, catalog.Count);
            Assert.NotNull(catalog.GetById("case_01_the_air_duct_moonshine_still"));
            Assert.NotNull(catalog.GetById("case_24_the_ratification_of_the_century_constitution"));
        }

        [Fact]
        public void BunkerCourt_ManifestIntegration()
        {
            string dataDir = FindDataDir();
            string courtFilePath = Path.Combine(dataDir, "narrative", "bunker_court_verdicts_codex.json");
            string manifestFilePath = Path.Combine(dataDir, "narrative_discovery_manifest.json");

            string courtJson = File.ReadAllText(courtFilePath);
            string manifestJson = File.ReadAllText(manifestFilePath);

            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerCourtCatalog();
            catalog.Load(courtJson, serializer);

            using var doc = JsonDocument.Parse(manifestJson);
            var entries = doc.RootElement.GetProperty("entries");

            var manifestCourtCases = new Dictionary<string, JsonElement>(StringComparer.Ordinal);
            foreach (var entry in entries.EnumerateArray())
            {
                if (entry.TryGetProperty("source_catalog", out var sc) &&
                    sc.GetString() == "narrative/bunker_court_verdicts_codex.json")
                {
                    string recId = entry.GetProperty("source_record_id").GetString()!;
                    manifestCourtCases[recId] = entry;
                }
            }

            // Every single one of the 24 cases must be registered in the manifest
            Assert.Equal(24, catalog.AllCases.Count);
            foreach (var c in catalog.AllCases)
            {
                Assert.True(manifestCourtCases.ContainsKey(c.case_id),
                    $"Court case {c.case_id} ({c.docket_number}) missing from narrative_discovery_manifest.json");

                var manifestEntry = manifestCourtCases[c.case_id];
                string discId = manifestEntry.GetProperty("discovery_id").GetString()!;
                Assert.StartsWith("disc_court_", discId);
                Assert.Equal("library_terminal", manifestEntry.GetProperty("channel").GetString());
                Assert.Equal("government_bunker", manifestEntry.GetProperty("producer_id").GetString());
                Assert.True(manifestEntry.GetProperty("min_day").GetInt32() > 0);
                Assert.True(manifestEntry.GetProperty("one_time").GetBoolean());
            }
        }

        [Fact]
        public void BunkerCourt_ZeroGameplayMutationContract()
        {
            string dataDir = FindDataDir();
            var io = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var catalog = BunkerCourtCatalog.LoadFromDirectory(dataDir, io, serializer);

            // Record baseline
            int countBefore = catalog.Count;
            Assert.Equal(24, countBefore);

            // Execute exhaustive queries
            for (int day = 1; day <= 4000; day += 500)
            {
                var unlocked = catalog.GetUnlockedByDay(day);
                Assert.NotNull(unlocked);
            }

            var searchRes = catalog.GetBySearch("penalty");
            Assert.NotNull(searchRes);

            // Assert catalog remains unchanged and pure read-only
            Assert.Equal(countBefore, catalog.Count);
            Assert.Equal(24, catalog.AllCases.Count);
        }
    }
}
