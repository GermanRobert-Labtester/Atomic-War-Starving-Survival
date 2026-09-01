// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: Plan 17A — Lore data baseline integrity.
// Validates that all Plan 17 JSON data files have correct structure,
// schema_version, unique IDs, and valid field values.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class Plan17ALoreBaselineTests : CatalogTestBase
    {
        private static string FindDataDir()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
            return dataDir ?? string.Empty;
        }

        private static string ReadJsonFile(string dataDir, string relativePath)
        {
            string fullPath = Path.Combine(dataDir, relativePath);
            Assert.True(File.Exists(fullPath), $"Lore JSON not found at: {fullPath}");
            return File.ReadAllText(fullPath);
        }

        // -----------------------------------------------------------------
        // schema_version presence
        // -----------------------------------------------------------------

        [Fact]
        public void WorldHistory_HasSchemaVersion()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            string json = ReadJsonFile(dataDir, "world_history.json");
            var ser = new SystemTextJsonSerializer();
            var root = ser.Deserialize<Dictionary<string, object>>(json);
            Assert.NotNull(root);
            Assert.True(root.ContainsKey("schema_version"), "world_history.json missing schema_version");
        }

        [Fact]
        public void FactionLore_HasSchemaVersion()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            string json = ReadJsonFile(dataDir, "faction_lore.json");
            var ser = new SystemTextJsonSerializer();
            var root = ser.Deserialize<Dictionary<string, object>>(json);
            Assert.NotNull(root);
            Assert.True(root.ContainsKey("schema_version"), "faction_lore.json missing schema_version");
        }

        [Fact]
        public void ArchiveInks_HasSchemaVersion()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            string json = ReadJsonFile(dataDir, "archive_inks.json");
            var ser = new SystemTextJsonSerializer();
            var root = ser.Deserialize<Dictionary<string, object>>(json);
            Assert.NotNull(root);
            Assert.True(root.ContainsKey("schema_version"), "archive_inks.json missing schema_version");
        }

        [Fact]
        public void DeepLoreLocations_HasSchemaVersion()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            string json = ReadJsonFile(dataDir, "deep_lore_locations.json");
            var ser = new SystemTextJsonSerializer();
            var root = ser.Deserialize<Dictionary<string, object>>(json);
            Assert.NotNull(root);
            Assert.True(root.ContainsKey("schema_version"), "deep_lore_locations.json missing schema_version");
        }

        [Fact]
        public void EnvironmentalAtmosphere_HasSchemaVersion()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            string json = ReadJsonFile(dataDir, "environmental_atmosphere_expansion.json");
            var ser = new SystemTextJsonSerializer();
            var root = ser.Deserialize<Dictionary<string, object>>(json);
            Assert.NotNull(root);
            Assert.True(root.ContainsKey("schema_version"), "environmental_atmosphere_expansion.json missing schema_version");
        }

        // -----------------------------------------------------------------
        // world_history.json — valid era values
        // -----------------------------------------------------------------

        [Fact]
        public void WorldHistory_AllErasAreValid()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var validEras = new HashSet<string>(StringComparer.Ordinal)
            {
                "pre_exchange", "hour_zero", "black_sky", "ashfall", "post_exchange"
            };

            string json = ReadJsonFile(dataDir, "world_history.json");
            var ser = new SystemTextJsonSerializer();
            var root = ser.Deserialize<WorldHistoryContainer>(json);
            Assert.NotNull(root);
            Assert.NotNull(root.items);
            Assert.True(root.items.Count > 0, "world_history.json has no items");

            foreach (var item in root.items)
            {
                Assert.True(validEras.Contains(item.era),
                    $"Invalid era '{item.era}' in world_history.json (title: '{item.title}')");
            }
        }

        [Fact]
        public void WorldHistory_AllEntriesHaveRequiredFields()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            string json = ReadJsonFile(dataDir, "world_history.json");
            var ser = new SystemTextJsonSerializer();
            var root = ser.Deserialize<WorldHistoryContainer>(json);
            Assert.NotNull(root?.items);

            foreach (var item in root.items)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.era), $"Missing era on entry '{item.title}'");
                Assert.False(string.IsNullOrWhiteSpace(item.year_month), $"Missing year_month on '{item.title}'");
                Assert.False(string.IsNullOrWhiteSpace(item.title), "Missing title on a world_history entry");
                Assert.False(string.IsNullOrWhiteSpace(item.body), $"Missing body on '{item.title}'");
                Assert.False(string.IsNullOrWhiteSpace(item.knowledge_key), $"Missing knowledge_key on '{item.title}'");
            }
        }

        // -----------------------------------------------------------------
        // faction_lore.json — unique faction_ids
        // -----------------------------------------------------------------

        [Fact]
        public void FactionLore_FactionIdsAreUnique()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            string json = ReadJsonFile(dataDir, "faction_lore.json");
            var ser = new SystemTextJsonSerializer();
            var root = ser.Deserialize<FactionLoreContainer>(json);
            Assert.NotNull(root?.items);

            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var faction in root.items)
            {
                Assert.False(string.IsNullOrWhiteSpace(faction.faction_id), "Missing faction_id");
                Assert.True(seen.Add(faction.faction_id), $"Duplicate faction_id: {faction.faction_id}");
            }
        }

        // -----------------------------------------------------------------
        // deep_lore_locations.json — unique location IDs
        // -----------------------------------------------------------------

        [Fact]
        public void DeepLoreLocations_IdsAreUnique()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            string json = ReadJsonFile(dataDir, "deep_lore_locations.json");
            var ser = new SystemTextJsonSerializer();
            var root = ser.Deserialize<DeepLoreLocationContainer>(json);
            Assert.NotNull(root?.locations);
            Assert.True(root.locations.Count > 0, "deep_lore_locations.json has no locations");

            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var loc in root.locations)
            {
                Assert.False(string.IsNullOrWhiteSpace(loc.id), "Missing id on a deep_lore_location");
                Assert.True(seen.Add(loc.id), $"Duplicate location id: {loc.id}");
            }
        }

        // -----------------------------------------------------------------
        // environmental_atmosphere_expansion.json — valid structure
        // -----------------------------------------------------------------

        [Fact]
        public void EnvironmentalAtmosphere_AllEntriesHaveValidStructure()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            string json = ReadJsonFile(dataDir, "environmental_atmosphere_expansion.json");
            var ser = new SystemTextJsonSerializer();
            var root = ser.Deserialize<AtmosphereContainer>(json);
            Assert.NotNull(root?.environmental_texts);
            Assert.True(root.environmental_texts.Count >= 100,
                $"Expected >= 100 atmosphere entries, got {root.environmental_texts.Count}");

            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var entry in root.environmental_texts)
            {
                Assert.False(string.IsNullOrWhiteSpace(entry.id), "Missing id on atmosphere entry");
                Assert.StartsWith("atm_", entry.id);
                Assert.True(seenIds.Add(entry.id), $"Duplicate atmosphere id: {entry.id}");

                Assert.False(string.IsNullOrWhiteSpace(entry.location), $"Missing location on {entry.id}");
                Assert.False(string.IsNullOrWhiteSpace(entry.text), $"Missing text on {entry.id}");
                Assert.False(string.IsNullOrWhiteSpace(entry.type), $"Missing type on {entry.id}");
            }
        }

        // -----------------------------------------------------------------
        // DTOs for deserialization
        // -----------------------------------------------------------------

        private class WorldHistoryContainer
        {
            public int schema_version;
            public List<WorldHistoryEntry> items;
        }

        private class WorldHistoryEntry
        {
            public string era;
            public string year_month;
            public string title;
            public string body;
            public string discovery_location_id;
            public string discovery_trigger;
            public string knowledge_key;
        }

        private class FactionLoreContainer
        {
            public int schema_version;
            public List<FactionLoreEntry> items;
        }

        private class FactionLoreEntry
        {
            public string faction_id;
            public string display_name;
            public string ideology;
            public string origin_story;
        }

        private class DeepLoreLocationContainer
        {
            public int schema_version;
            public List<DeepLoreLocationEntry> locations;
        }

        private class DeepLoreLocationEntry
        {
            public string id;
            public string displayName;
        }

        private class AtmosphereContainer
        {
            public int schema_version;
            public string collection_id;
            public List<AtmosphereEntry> environmental_texts;
        }

        private class AtmosphereEntry
        {
            public string id;
            public string location;
            public string text;
            public string type;
        }
    }
}
