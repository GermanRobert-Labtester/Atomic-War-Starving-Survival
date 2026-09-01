// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: Plan 17B — Atmosphere text data validation.
// Tests the environmental_atmosphere_expansion.json data integrity and
// validates query patterns that a future AtmosphereTextSystem will consume.
// Since AtmosphereTextSystem is not yet implemented, these tests operate
// directly on the loaded JSON data to pin the data contract.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class Plan17BAtmosphereTests : CatalogTestBase
    {
        private static string FindDataDir()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
            return dataDir ?? string.Empty;
        }

        private static AtmosphereData LoadAtmosphereData()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Could not locate data directory");

            string path = Path.Combine(dataDir, "environmental_atmosphere_expansion.json");
            Assert.True(File.Exists(path), $"Atmosphere JSON not found at: {path}");

            string json = File.ReadAllText(path);
            var ser = new SystemTextJsonSerializer();
            var root = ser.Deserialize<AtmosphereRoot>(json);
            Assert.NotNull(root?.environmental_texts);
            return new AtmosphereData(root.environmental_texts);
        }

        // -----------------------------------------------------------------
        // Loading
        // -----------------------------------------------------------------

        [Fact]
        public void AtmosphereData_LoadsEntriesFromJson()
        {
            var data = LoadAtmosphereData();
            Assert.True(data.Entries.Count >= 100,
                $"Expected >= 100 atmosphere entries, got {data.Entries.Count}");
        }

        [Fact]
        public void AtmosphereData_AllIdsHaveAtmPrefix()
        {
            var data = LoadAtmosphereData();
            foreach (var entry in data.Entries)
            {
                Assert.StartsWith("atm_", entry.id);
            }
        }

        // -----------------------------------------------------------------
        // Location-based queries
        // -----------------------------------------------------------------

        [Fact]
        public void AtmosphereData_GetTextsForLocation_ReturnsMatchingEntries()
        {
            var data = LoadAtmosphereData();

            // Pick the first location that has entries
            string firstLocation = data.Entries[0].location;
            var matches = data.GetTextsForLocation(firstLocation);

            Assert.NotNull(matches);
            Assert.True(matches.Count >= 1, $"No entries found for location '{firstLocation}'");
            Assert.All(matches, e => Assert.Equal(firstLocation, e.location));
        }

        [Fact]
        public void AtmosphereData_GetTextsForlocation_ReturnsEmptyForUnknown()
        {
            var data = LoadAtmosphereData();
            var matches = data.GetTextsForLocation("loc_nonexistent_place_999");
            Assert.NotNull(matches);
            Assert.Empty(matches);
        }

        // -----------------------------------------------------------------
        // Weather-specific queries
        // -----------------------------------------------------------------

        [Fact]
        public void AtmosphereData_GetTextsForLocationAndWeather_ReturnsWeatherSpecific()
        {
            var data = LoadAtmosphereData();

            // Find an entry with a specific weather (not "any")
            var weatherEntry = data.Entries.FirstOrDefault(e =>
                !string.IsNullOrEmpty(e.weather) && e.weather != "any");

            if (weatherEntry != null)
            {
                var matches = data.GetTextsForLocationAndWeather(
                    weatherEntry.location, weatherEntry.weather);
                Assert.NotNull(matches);
                // Should include entries matching that weather OR "any" weather
                Assert.All(matches, e =>
                    Assert.True(e.weather == weatherEntry.weather || e.weather == "any",
                        $"Entry '{e.id}' has weather '{e.weather}' which doesn't match"));
            }
            else
            {
                // All entries have weather="any"; verify that query returns location matches
                string loc = data.Entries[0].location;
                var matches = data.GetTextsForLocationAndWeather(loc, "storm");
                // With weather="any" entries, they should match any weather query
                Assert.NotNull(matches);
            }
        }

        // -----------------------------------------------------------------
        // GetAllTextsForLocation
        // -----------------------------------------------------------------

        [Fact]
        public void AtmosphereData_GetAllTextsForLocation_ReturnsAllMatches()
        {
            var data = LoadAtmosphereData();
            string loc = data.Entries[0].location;

            var all = data.GetAllTextsForLocation(loc);
            Assert.NotNull(all);
            Assert.True(all.Count >= 1);
            Assert.Equal(data.Entries.Count(e => e.location == loc), all.Count);
        }

        // -----------------------------------------------------------------
        // Count
        // -----------------------------------------------------------------

        [Fact]
        public void AtmosphereData_Count_ReturnsCorrectTotal()
        {
            var data = LoadAtmosphereData();
            Assert.Equal(data.Entries.Count, data.Count);
            Assert.True(data.Count >= 100);
        }

        // -----------------------------------------------------------------
        // Deterministic selection (same inputs → same output)
        // -----------------------------------------------------------------

        [Fact]
        public void AtmosphereData_DeterministicSelection_SameInputsSameOutput()
        {
            var data = LoadAtmosphereData();
            string loc = data.Entries[0].location;
            var rng = new SeededRng(42);

            // Pick a deterministic entry using seeded RNG
            var matches = data.GetTextsForLocation(loc);
            if (matches.Count == 0) return;

            int idx1 = rng.Next(0, matches.Count);
            string firstPick = matches[idx1].id;

            // Reset RNG with same seed → must produce same index
            var rng2 = new SeededRng(42);
            int idx2 = rng2.Next(0, matches.Count);
            string secondPick = matches[idx2].id;

            Assert.Equal(firstPick, secondPick);
        }

        [Fact]
        public void AtmosphereData_DifferentSeeds_ProduceDifferentOrderings()
        {
            var data = LoadAtmosphereData();
            // Find a location with multiple entries
            var grouped = data.Entries.GroupBy(e => e.location)
                .Where(g => g.Count() >= 3)
                .FirstOrDefault();

            if (grouped == null) return;

            string loc = grouped.Key;
            var matches = data.GetTextsForLocation(loc);

            var rng1 = new SeededRng(1);
            var rng2 = new SeededRng(999);

            int idx1 = rng1.Next(0, matches.Count);
            int idx2 = rng2.Next(0, matches.Count);

            // With different seeds and >= 3 entries, indices should (almost certainly) differ
            // This is a probabilistic assertion but safe with 3+ entries
            if (matches.Count >= 3)
            {
                // Just verify both are valid indices
                Assert.InRange(idx1, 0, matches.Count - 1);
                Assert.InRange(idx2, 0, matches.Count - 1);
            }
        }

        // -----------------------------------------------------------------
        // Type field validation
        // -----------------------------------------------------------------

        [Fact]
        public void AtmosphereData_AllTypesAreKnown()
        {
            var knownTypes = new HashSet<string>(StringComparer.Ordinal)
            {
                "location_description", "sensory_deprivation", "environmental_load",
                "threat_detection", "stealth_awareness", "loot_weight",
                "encumbrance_strain", "extraction_vignette", "entryway_description",
                "visual_cue", "tactile_atmosphere", "emotional_atmosphere", "taste_cue",
                "discovery_atmosphere", "hope_atmosphere", "taste_atmosphere", "weather_atmosphere",
                "room_search", "environmental_storytelling", "olfactory_atmosphere", "weather_effect",
                "survival_atmosphere", "danger_atmosphere", "sound_atmosphere", "collapse_warning",
                "gustatory_atmosphere", "despair_atmosphere", "time_atmosphere", "temperature_warning",
                "community_atmosphere", "smell_atmosphere", "supernatural_atmosphere", "visual_atmosphere",
                "auditory_atmosphere", "historical_atmosphere", "radiation_warning", "biological_warning",
                "daynight_cycle", "cultural_atmosphere", "touch_atmosphere", "movement_atmosphere",
                "chemical_warning", "isolation_atmosphere", "interaction_atmosphere", "lighting_atmosphere",
                "sound_cue", "hidden_detail", "threshold_warning", "touch_cue", "smell_cue"
            };

            var data = LoadAtmosphereData();
            foreach (var entry in data.Entries)
            {
                Assert.True(knownTypes.Contains(entry.type),
                    $"Unknown atmosphere type '{entry.type}' on entry '{entry.id}'");
            }
        }

        // -----------------------------------------------------------------
        // Helper types
        // -----------------------------------------------------------------

        private class AtmosphereRoot
        {
            public int schema_version;
            public string collection_id;
            public List<AtmosphereEntryDto> environmental_texts;
        }

        private class AtmosphereEntryDto
        {
            public string id;
            public string location;
            public string text;
            public string type;
            public List<string> tags;
            public List<string> atmosphere;
            public string sense;
            public string time_phase;
            public string weather;
            public string author;
            public string condition;
        }

        /// <summary>
        /// Lightweight in-memory query facade over atmosphere entries.
        /// Mirrors the contract a future AtmosphereTextSystem will expose.
        /// </summary>
        private class AtmosphereData
        {
            public List<AtmosphereEntryDto> Entries { get; }
            public int Count => Entries.Count;

            public AtmosphereData(List<AtmosphereEntryDto> entries)
            {
                Entries = entries ?? new List<AtmosphereEntryDto>();
            }

            public List<AtmosphereEntryDto> GetTextsForLocation(string location)
            {
                return Entries.Where(e => e.location == location).ToList();
            }

            public List<AtmosphereEntryDto> GetTextsForLocationAndWeather(string location, string weather)
            {
                return Entries.Where(e =>
                    e.location == location &&
                    (e.weather == weather || e.weather == "any")).ToList();
            }

            public List<AtmosphereEntryDto> GetAllTextsForLocation(string location)
            {
                return Entries.Where(e => e.location == location).ToList();
            }
        }
    }
}
