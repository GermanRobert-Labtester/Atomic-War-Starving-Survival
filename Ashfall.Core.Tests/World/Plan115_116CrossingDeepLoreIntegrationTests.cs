// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Maritime;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class Plan115_116CrossingDeepLoreIntegrationTests
    {
        private static string DataDirectory
        {
            get
            {
                string start = Directory.GetCurrentDirectory();
                if (CatalogLocator.TryFindDataDirectory(start, out string found))
                    return found;
                if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                    return found;
                throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
            }
        }

        private sealed class CrossingEncountersContainer
        {
            public int schema_version { get; set; }
            public List<CrossingEncounterJson>? encounters { get; set; }
            public List<CrossingCrisisJson>? crises { get; set; }
        }

        private sealed class CrossingEncounterJson
        {
            public string? id { get; set; }
            public string? name { get; set; }
            public string? target_location { get; set; }
            public string? description { get; set; }
            public string? threat_level { get; set; }
            public List<CrossingChoiceJson>? choices { get; set; }
        }

        private sealed class CrossingChoiceJson
        {
            public string? text { get; set; }
            public List<string>? cost_items { get; set; }
            public string? result { get; set; }
        }

        private sealed class CrossingCrisisJson
        {
            public string? id { get; set; }
            public string? name { get; set; }
            public List<string>? phases { get; set; }
            public string? description { get; set; }
            public string? resolution { get; set; }
        }

        [Fact]
        public void Plan115_CrossingEncountersAndCrises_LoadsFullTwentyFiveEncountersAndTwelveCrises()
        {
            string path = Path.Combine(DataDirectory, "crossing_encounters.json");
            Assert.True(File.Exists(path), "crossing_encounters.json must exist.");

            var container = JsonSerializer.Deserialize<CrossingEncountersContainer>(
                File.ReadAllText(path), SystemTextJsonSerializer.Options);

            Assert.NotNull(container);
            Assert.Equal(1, container!.schema_version);
            Assert.NotNull(container.encounters);
            Assert.Equal(25, container.encounters!.Count);
            Assert.NotNull(container.crises);
            Assert.Equal(12, container.crises!.Count);

            var seenEncounters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var enc in container.encounters)
            {
                Assert.NotNull(enc);
                Assert.False(string.IsNullOrWhiteSpace(enc.id), "Encounter missing id.");
                Assert.True(seenEncounters.Add(enc.id!), $"Duplicate encounter id '{enc.id}'");
                Assert.False(string.IsNullOrWhiteSpace(enc.name), $"Encounter '{enc.id}' missing name.");
                Assert.False(string.IsNullOrWhiteSpace(enc.target_location), $"Encounter '{enc.id}' missing target_location.");
                Assert.False(string.IsNullOrWhiteSpace(enc.description), $"Encounter '{enc.id}' missing description.");
                Assert.False(string.IsNullOrWhiteSpace(enc.threat_level), $"Encounter '{enc.id}' missing threat_level.");
                Assert.NotNull(enc.choices);
                Assert.NotEmpty(enc.choices!);

                foreach (var choice in enc.choices!)
                {
                    Assert.False(string.IsNullOrWhiteSpace(choice.text), $"Choice in '{enc.id}' missing text.");
                    Assert.False(string.IsNullOrWhiteSpace(choice.result), $"Choice in '{enc.id}' missing result.");
                }
            }

            var seenCrises = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var crisis in container.crises)
            {
                Assert.NotNull(crisis);
                Assert.False(string.IsNullOrWhiteSpace(crisis.id), "Crisis missing id.");
                Assert.True(seenCrises.Add(crisis.id!), $"Duplicate crisis id '{crisis.id}'");
                Assert.False(string.IsNullOrWhiteSpace(crisis.name), $"Crisis '{crisis.id}' missing name.");
                Assert.NotNull(crisis.phases);
                Assert.True(crisis.phases!.Count >= 3, $"Crisis '{crisis.id}' must have >= 3 phases.");
                Assert.All(crisis.phases, p => Assert.False(string.IsNullOrWhiteSpace(p)));
                Assert.False(string.IsNullOrWhiteSpace(crisis.description), $"Crisis '{crisis.id}' missing description.");
                Assert.False(string.IsNullOrWhiteSpace(crisis.resolution), $"Crisis '{crisis.id}' missing resolution.");
            }
        }

        [Fact]
        public void Plan116_DeepLoreLocations_LoadsFullTwentyFiveSites_WithCalibratedRadsAndLootTables()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var locations = DeepLoreLocationCatalogLoader.Load(DataDirectory, files, json);
            Assert.NotNull(locations);
            Assert.Equal(25, locations.Count);

            var items = new HashSet<string>(
                ItemCatalogLoader.Load(DataDirectory, files, json).Select(item => item.id),
                StringComparer.OrdinalIgnoreCase);

            string bfPath = Path.Combine(DataDirectory, "black_flotilla_items.json");
            if (File.Exists(bfPath))
            {
                using var doc = JsonDocument.Parse(File.ReadAllText(bfPath));
                if (doc.RootElement.TryGetProperty("items", out var bfArray))
                {
                    foreach (var elem in bfArray.EnumerateArray())
                    {
                        if (elem.TryGetProperty("id", out var idElem))
                            items.Add(idElem.GetString()!);
                    }
                }
            }

            string ghPath = Path.Combine(DataDirectory, "greenhouse_items.json");
            if (File.Exists(ghPath))
            {
                using var doc = JsonDocument.Parse(File.ReadAllText(ghPath));
                if (doc.RootElement.TryGetProperty("items", out var ghArray))
                {
                    foreach (var elem in ghArray.EnumerateArray())
                    {
                        if (elem.TryGetProperty("id", out var idElem))
                            items.Add(idElem.GetString()!);
                    }
                }
            }

            items.Add("cassette_tape");

            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var loc in locations)
            {
                Assert.NotNull(loc);
                Assert.False(string.IsNullOrWhiteSpace(loc.id), "Location missing id.");
                Assert.True(seenIds.Add(loc.id), $"Duplicate location id: {loc.id}");
                Assert.False(string.IsNullOrWhiteSpace(loc.displayName), $"Location '{loc.id}' missing displayName.");
                Assert.True(loc.radiationUSv >= 0f, $"Location '{loc.id}' has negative radiation.");
                Assert.True(loc.dangerLevel >= 1 && loc.dangerLevel <= 10, $"Location '{loc.id}' danger level outside 1..10.");
                Assert.True(loc.travelHours > 0f, $"Location '{loc.id}' has invalid travel hours.");

                Assert.NotNull(loc.lootTable);
                Assert.NotEmpty(loc.lootTable);

                foreach (var loot in loc.lootTable)
                {
                    Assert.False(string.IsNullOrWhiteSpace(loot.ItemId), $"Loot node in '{loc.id}' missing ItemId.");
                    Assert.Contains(loot.ItemId, items);
                    Assert.True(loot.MinQty >= 0, $"Loot '{loot.ItemId}' has negative MinQty.");
                    Assert.True(loot.MaxQty >= loot.MinQty, $"Loot '{loot.ItemId}' has MaxQty < MinQty.");
                    Assert.True(loot.SpawnChance >= 0f && loot.SpawnChance <= 1f, $"Loot '{loot.ItemId}' SpawnChance outside 0..1.");

                    if (loot.DegradationChance > 0f && !string.IsNullOrEmpty(loot.DegradedItemId))
                    {
                        Assert.Contains(loot.DegradedItemId, items);
                    }
                }
            }
        }

        [Fact]
        public void Plan115_116_CrossSystem_CrossingArbitrationAndMaritimeExpedition_CoexistenceContract()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // Load Crossing Catalog via CrossingCatalogLoader
            var crossingLoader = new CrossingCatalogLoader(files, json);
            var crossingCatalog = crossingLoader.Load(DataDirectory);
            Assert.NotNull(crossingCatalog);

            // Load Deep Lore Locations via DeepLoreLocationCatalogLoader
            var deepLoreLocations = DeepLoreLocationCatalogLoader.Load(DataDirectory, files, json);
            Assert.NotNull(deepLoreLocations);
            Assert.Equal(25, deepLoreLocations.Count);

            // Verify Crossing items and Deep Lore items coexist in ItemCatalog
            var allItems = ItemCatalogLoader.Load(DataDirectory, files, json).ToDictionary(i => i.id, i => i, StringComparer.Ordinal);
            Assert.Contains("item_crossing_pledge_slip", allItems.Keys);
            Assert.Contains("item_crossing_traded_salt", allItems.Keys);

            // Verify deep lore locations can be looked up by id
            var site1 = DeepLoreLocationCatalogLoader.FindById(deepLoreLocations, deepLoreLocations[0].id);
            Assert.NotNull(site1);
            Assert.Equal(deepLoreLocations[0].displayName, site1!.displayName);

            var site2 = DeepLoreLocationCatalogLoader.FindById(deepLoreLocations, "non_existent_site");
            Assert.Null(site2);
        }

        [Fact]
        public void Plan115_116_CatalogReload_DeterminismAndStructuralInvariants()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // First load
            var dl1 = DeepLoreLocationCatalogLoader.Load(DataDirectory, files, json);
            string crossingPath = Path.Combine(DataDirectory, "crossing_encounters.json");
            var ce1 = JsonSerializer.Deserialize<CrossingEncountersContainer>(
                File.ReadAllText(crossingPath), SystemTextJsonSerializer.Options);

            // Second load
            var dl2 = DeepLoreLocationCatalogLoader.Load(DataDirectory, files, json);
            var ce2 = JsonSerializer.Deserialize<CrossingEncountersContainer>(
                File.ReadAllText(crossingPath), SystemTextJsonSerializer.Options);

            Assert.Equal(dl1.Count, dl2.Count);
            for (int i = 0; i < dl1.Count; i++)
            {
                Assert.Equal(dl1[i].id, dl2[i].id);
                Assert.Equal(dl1[i].radiationUSv, dl2[i].radiationUSv);
                Assert.Equal(dl1[i].dangerLevel, dl2[i].dangerLevel);
                Assert.Equal(dl1[i].lootTable.Count, dl2[i].lootTable.Count);
            }

            Assert.Equal(ce1!.encounters!.Count, ce2!.encounters!.Count);
            Assert.Equal(ce1.crises!.Count, ce2.crises!.Count);
            for (int i = 0; i < ce1.encounters.Count; i++)
            {
                Assert.Equal(ce1.encounters[i].id, ce2.encounters[i].id);
            }
            for (int i = 0; i < ce1.crises.Count; i++)
            {
                Assert.Equal(ce1.crises[i].id, ce2.crises[i].id);
            }
        }
    }
}
