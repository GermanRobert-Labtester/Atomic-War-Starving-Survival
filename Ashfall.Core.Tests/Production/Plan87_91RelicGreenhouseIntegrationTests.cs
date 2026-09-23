// SPDX-License-Identifier: MIT
// Plan 87 × Plan 91 cross-system integration test
// Plan 87 — Relic Crafting Recipes & Restoration Provenance
// Plan 91 — Greenhouse Botanical Items Expansion (14 → 30+ items)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.Crafting;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.Production
{
    public sealed class Plan87_91RelicGreenhouseIntegrationTests
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

        // ── Relic Recipes (Plan 87) ───────────────────────────────────────────

        [Fact]
        public void RelicRecipes_LoadsAllThirtyNineRecipesWithValidContracts()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = RelicCatalogLoader.Load(dataDir, io, json);

            Assert.NotNull(catalog);
            Assert.Equal(39, catalog.relics.Count);

            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var relic in catalog.relics)
            {
                Assert.False(string.IsNullOrWhiteSpace(relic.relic_id), "Relic has empty relic_id");
                Assert.True(seenIds.Add(relic.relic_id), $"Duplicate relic_id: {relic.relic_id}");
                Assert.False(string.IsNullOrWhiteSpace(relic.display_name), $"Relic {relic.relic_id} has empty display_name");
                Assert.True(relic.repair_time_hours > 0f, $"Relic {relic.relic_id} has invalid repair_time_hours");
                Assert.True(relic.morale_bonus >= 0, $"Relic {relic.relic_id} has negative morale_bonus");
                Assert.True(relic.required_components != null && relic.required_components.Count > 0,
                    $"Relic {relic.relic_id} has no required components");
                Assert.False(string.IsNullOrWhiteSpace(relic.description),
                    $"Relic {relic.relic_id} has empty description");
            }
        }

        // ── Greenhouse Botanical Items (Plan 91) ──────────────────────────────

        [Fact]
        public void GreenhouseItems_LoadsAtLeastThirtyItemsWithValidCategories()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            string filePath = Path.Combine(dataDir, "greenhouse_items.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            using var doc = JsonDocument.Parse(File.ReadAllText(filePath));
            var root = doc.RootElement;
            Assert.True(root.TryGetProperty("items", out var itemsElement));

            var items = itemsElement.EnumerateArray().ToList();
            Assert.True(items.Count >= 30, $"Expected >= 30 greenhouse items; got {items.Count}");

            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in items)
            {
                string id = item.GetProperty("id").GetString()!;
                Assert.False(string.IsNullOrWhiteSpace(id), "Greenhouse item missing id");
                Assert.True(seenIds.Add(id), $"Duplicate greenhouse item id: {id}");

                string displayName = item.GetProperty("displayName").GetString()!;
                Assert.False(string.IsNullOrWhiteSpace(displayName), $"Item {id} missing displayName");

                int stackMax = item.GetProperty("stackMax").GetInt32();
                Assert.True(stackMax >= 1, $"Item {id} has invalid stackMax: {stackMax}");

                double weight = item.GetProperty("weight").GetDouble();
                Assert.True(weight >= 0.0, $"Item {id} has negative weight: {weight}");

                int tradeValue = item.GetProperty("tradeValue").GetInt32();
                Assert.True(tradeValue >= 1, $"Item {id} has non-positive tradeValue: {tradeValue}");
            }

            // Verify core functional categories present
            Assert.True(seenIds.Contains("item_seed_mushroom"), "Missing baseline seed");
            Assert.True(seenIds.Contains("item_greenhouse_trowel"), "Missing greenhouse tool");
            Assert.True(seenIds.Contains("item_greenhouse_compost"), "Missing greenhouse fertilizer");
            Assert.True(seenIds.Contains("item_greenhouse_drip_kit"), "Missing greenhouse water management");
            Assert.True(seenIds.Contains("item_greenhouse_insecticidal_soap"), "Missing greenhouse pest control");
            Assert.True(seenIds.Contains("item_greenhouse_glass_pane"), "Missing greenhouse structural supply");
        }

        // ── Cross-System Coherence ──────────────────────────────────────────────

        [Fact]
        public void CrossSystem_BothCatalogsLoadIndependentlyWithoutIdCollision()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var relicCatalog = RelicCatalogLoader.Load(dataDir, io, json);
            Assert.NotNull(relicCatalog);
            Assert.True(relicCatalog.relics.Count >= 39);

            string greenhousePath = Path.Combine(dataDir, "greenhouse_items.json");
            using var doc = JsonDocument.Parse(File.ReadAllText(greenhousePath));
            var greenhouseIds = doc.RootElement.GetProperty("items")
                .EnumerateArray()
                .Select(e => e.GetProperty("id").GetString()!)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            Assert.True(greenhouseIds.Count >= 30);

            // Relic IDs and Greenhouse IDs must be strictly disjoint
            var relicIds = relicCatalog.relics.Select(r => r.relic_id).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var overlap = relicIds.Intersect(greenhouseIds).ToList();
            Assert.Empty(overlap);
        }

        [Fact]
        public void CrossSystem_RelicsAndGreenhouseReflectCivilizationRebuildingInfrastructure()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var relicCatalog = RelicCatalogLoader.Load(dataDir, io, json);
            Assert.NotNull(relicCatalog);

            // High-morale cultural relics exist for shelter hope
            var gramophone = relicCatalog.relics.FirstOrDefault(r => r.relic_id == "gramophone");
            Assert.NotNull(gramophone);
            Assert.True(gramophone.morale_bonus >= 5);

            // Greenhouse food staples exist for caloric survival
            string greenhousePath = Path.Combine(dataDir, "greenhouse_items.json");
            using var doc = JsonDocument.Parse(File.ReadAllText(greenhousePath));
            var items = doc.RootElement.GetProperty("items").EnumerateArray().ToList();

            var crops = items.Where(e => e.GetProperty("id").GetString()!.StartsWith("crop_")).ToList();
            Assert.True(crops.Count >= 4, $"Expected >= 4 crop food outputs; got {crops.Count}");
        }
    }
}
