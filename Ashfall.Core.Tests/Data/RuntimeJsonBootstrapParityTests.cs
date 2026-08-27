using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;
using Ashfall.Core.Tests.Fixtures;
using Xunit;

namespace Ashfall.Core.Tests.Data
{
    public sealed class RuntimeJsonBootstrapParityTests
    {
        private static string FindDataDirectory()
        {
            string current = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(current))
            {
                string candidate = Path.Combine(current, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate))
                    return candidate;

                string parent = Path.GetDirectoryName(current)!;
                if (parent == current) break;
                current = parent;
            }
            throw new DirectoryNotFoundException("Could not find Assets/StreamingAssets/Data directory.");
        }

        [Fact]
        public void ItemCatalogLoader_LoadsAllAuthoritativeItemsFromJson()
        {
            string dataDir = FindDataDirectory();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var catalog = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);

            Assert.True(catalog.Count >= 150, $"Expected >= 150 items loaded, got {catalog.Count}");

            // Canonical survival items
            string[] expectedIds =
            {
                "canned_food", "clean_water", "irradiated_water", "bandage",
                "iodine_pills", "rad_away", "gas_mask", "hazmat_suit",
                "battery", "scrap_mechanical", "scrap_electronic", "scrap_chemical",
                "item_geiger_m3", "item_dosimeter_pen", "item_air_filter_hepa",
                "item_desal_membrane", "filter_pack", "inhaler", "herbal_tea"
            };

            foreach (var id in expectedIds)
            {
                var def = catalog.Get(id);
                Assert.NotNull(def);
                Assert.False(string.IsNullOrEmpty(def!.displayName), $"Item {id} has empty displayName");
                Assert.True(def.weight > 0f || def.tradeValue >= 0f, $"Item {id} has invalid weight/trade stats");
            }

            // Verify specific item attributes
            var food = catalog.Get("canned_food")!;
            Assert.Equal(ItemType.Food, food.type);
            Assert.True(food.hungerRestore > 0f);

            var gasMask = catalog.Get("gas_mask")!;
            Assert.True(gasMask.isEquipable);
            Assert.Equal(EquipSlot.Face, gasMask.equipSlot);
            Assert.True(gasMask.radProtection > 0f);

            var hazmat = catalog.Get("hazmat_suit")!;
            Assert.True(hazmat.isEquipable);
            Assert.Equal(EquipSlot.Body, hazmat.equipSlot);
            Assert.True(hazmat.radProtection > 0f);
        }

        [Fact]
        public void ItemCatalogLoader_StartingSupplies_MatchesAuthoritativeJson()
        {
            string dataDir = FindDataDirectory();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var supplies = ItemCatalogLoader.LoadStartingSupplies(dataDir, fileIO, serializer);
            var catalog = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);

            Assert.NotEmpty(supplies);
            Assert.True(supplies.Count >= 10, $"Expected >= 10 starting supplies, got {supplies.Count}");

            foreach (var (itemId, amount) in supplies)
            {
                Assert.True(amount > 0, $"Starting supply {itemId} has non-positive amount {amount}");
                var item = catalog.Get(itemId);
                Assert.NotNull(item);
            }
        }

        [Fact]
        public void RecipeCatalogLoader_LoadsAllAuthoritativeRecipesFromJson()
        {
            string dataDir = FindDataDirectory();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var catalog = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);
            var recipes = RecipeCatalogLoader.Load(dataDir, fileIO, serializer, catalog);

            Assert.NotEmpty(recipes);
            Assert.True(recipes.Count >= 30, $"Expected >= 30 recipes, got {recipes.Count}");

            foreach (var recipe in recipes)
            {
                Assert.False(string.IsNullOrEmpty(recipe.id), "Recipe has empty id");
                Assert.False(string.IsNullOrEmpty(recipe.recipeName), $"Recipe {recipe.id} has empty name");
                Assert.NotNull(recipe.result);
                Assert.True(recipe.resultAmount > 0, $"Recipe {recipe.id} has non-positive resultAmount");
                Assert.True(recipe.craftingTimeHours > 0f, $"Recipe {recipe.id} has non-positive craftingTimeHours");

                Assert.NotEmpty(recipe.ingredients);
                foreach (var ing in recipe.ingredients)
                {
                    Assert.NotNull(ing.item);
                    Assert.True(ing.amount > 0, $"Recipe {recipe.id} ingredient has non-positive amount");
                }
            }
        }

        [Fact]
        public void SurvivorStartingStateLoader_LoadsStartingSurvivorsFromJson()
        {
            string dataDir = FindDataDirectory();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var starting = SurvivorStartingStateLoader.Load(dataDir, fileIO, serializer);

            Assert.NotEmpty(starting);
            Assert.True(starting.Count >= 3, $"Expected >= 3 starting survivors, got {starting.Count}");

            var mikhail = starting.Find(s => s.id == "survivor_gunner_mikhail");
            Assert.NotNull(mikhail);
            Assert.True(mikhail!.acuteRad, "Gunner Mikhail should start with acute radiation exposure");
            Assert.True(mikhail.lifetimeDose > 0f);

            var sarah = starting.Find(s => s.id == "survivor_dr_sarah_chen");
            Assert.NotNull(sarah);
            Assert.True(sarah!.health > 0f);

            var elena = starting.Find(s => s.id == "elena_vasquez");
            Assert.NotNull(elena);
            Assert.True(elena!.health > 0f);
        }

        [Fact]
        public void ExpeditionCatalogLoader_LoadsExpeditionsFromJson()
        {
            string dataDir = FindDataDirectory();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var expeditions = ExpeditionCatalogLoader.Load(dataDir, fileIO, serializer);

            Assert.NotEmpty(expeditions);
            Assert.True(expeditions.Count >= 2, $"Expected >= 2 expeditions, got {expeditions.Count}");

            var allotments = ExpeditionDefinitionRegistry.Get("loc_the_allotments");
            Assert.NotNull(allotments);
            Assert.True(allotments!.distanceTicks > 0);
            Assert.True(allotments.dangerLevel > 0);
            Assert.NotEmpty(allotments.lootCategories);

            var cut = ExpeditionDefinitionRegistry.Get("loc_denial_cut_substation");
            Assert.NotNull(cut);
            Assert.True(cut!.distanceTicks > 0);
            Assert.NotEmpty(cut.lootCategories);
        }

        private sealed class MockFileIO : IFileIO
        {
            private readonly Dictionary<string, string> _files = new(StringComparer.Ordinal);

            public void SetFile(string path, string content) => _files[path] = content;
            public bool DirectoryExists(string path) => true;
            public bool FileExists(string path) => _files.ContainsKey(path);
            public string ReadAllText(string path) => _files.TryGetValue(path, out var text) ? text : string.Empty;
            public void WriteAllText(string path, string contents) => _files[path] = contents;
            public string Combine(params string[] parts) => string.Join("/", parts);
            public string[] EnumerateFiles(string directory, string searchPattern, SearchOption searchOption) => new string[0];
            public void CreateDirectory(string path) { }
            public void DeleteFile(string path) => _files.Remove(path);
            public void Copy(string source, string destination, bool overwrite) { if (_files.TryGetValue(source, out var c)) _files[destination] = c; }
        }

        [Fact]
        public void RuntimeBootstrap_ChangingJsonDirectlyAltersLiveRuntimeWithoutEditingCSharp()
        {
            var mockIO = new MockFileIO();
            var serializer = new SystemTextJsonSerializer();
            const string mockDataDir = "MockData";

            // Author a custom items.json with modified values
            string customItemsJson = @"
{
  ""schema_version"": 1,
  ""items"": [
    {
      ""id"": ""canned_food"",
      ""displayName"": ""Super Nutrient Ration"",
      ""type"": ""Food"",
      ""stackMax"": 12,
      ""weight"": 0.25,
      ""hungerRestore"": 99.0,
      ""tradeValue"": 100.0
    },
    {
      ""id"": ""custom_plasma_fuel"",
      ""displayName"": ""Plasma Fuel"",
      ""type"": ""Fuel"",
      ""stackMax"": 5,
      ""weight"": 1.0,
      ""tradeValue"": 250.0
    }
  ]
}";
            mockIO.SetFile("MockData/items.json", customItemsJson);

            // Author a custom recipes.json
            string customRecipesJson = @"
{
  ""schema_version"": 1,
  ""recipes"": [
    {
      ""id"": ""recipe_custom_plasma"",
      ""recipeName"": ""Synthesize Plasma"",
      ""ingredients"": [
        { ""itemId"": ""canned_food"", ""amount"": 2 }
      ],
      ""resultItemId"": ""custom_plasma_fuel"",
      ""resultAmount"": 1,
      ""craftingTimeHours"": 1.5,
      ""requiredStationId"": ""workbench""
    }
  ]
}";
            mockIO.SetFile("MockData/recipes.json", customRecipesJson);

            // Author a custom starting_survivors.json
            string customSurvivorsJson = @"
{
  ""schema_version"": 1,
  ""starting_survivors"": [
    {
      ""id"": ""survivor_modded_scout"",
      ""displayName"": ""Modded Wasteland Scout"",
      ""health"": 100.0,
      ""hunger"": 0.0,
      ""thirst"": 0.0,
      ""warmth"": 100.0,
      ""morale"": 90.0,
      ""lifetimeDose"": 0.0,
      ""acuteRad"": false
    }
  ]
}";
            mockIO.SetFile("MockData/starting_survivors.json", customSurvivorsJson);

            // Author a custom expeditions.json
            string customExpeditionsJson = @"
{
  ""schema_version"": 1,
  ""expeditions"": [
    {
      ""id"": ""loc_secret_research_vault"",
      ""displayName"": ""Secret Research Vault"",
      ""distanceTicks"": 12,
      ""dangerLevel"": 9,
      ""encounterChancePerTick"": 0.45,
      ""baseStaminaDrainPerHour"": 4.0,
      ""lootCategories"": [""custom_plasma_fuel""]
    }
  ]
}";
            mockIO.SetFile("MockData/expeditions.json", customExpeditionsJson);

            // Load all 4 catalogs from the mock JSON authority
            var loadedCatalog = ItemCatalogLoader.LoadCatalog(mockDataDir, mockIO, serializer);
            var loadedRecipes = RecipeCatalogLoader.Load(mockDataDir, mockIO, serializer, loadedCatalog);
            var loadedSurvivors = SurvivorStartingStateLoader.Load(mockDataDir, mockIO, serializer);
            var loadedExpeditions = ExpeditionCatalogLoader.Load(mockDataDir, mockIO, serializer);

            // Assert item modifications from JSON
            var food = loadedCatalog.Get("canned_food");
            Assert.NotNull(food);
            Assert.Equal("Super Nutrient Ration", food!.displayName);
            Assert.Equal(99.0f, food.hungerRestore);
            Assert.Equal(12, food.stackMax);

            var plasma = loadedCatalog.Get("custom_plasma_fuel");
            Assert.NotNull(plasma);
            Assert.Equal(250.0f, plasma!.tradeValue);

            // Assert recipe from JSON
            Assert.Single(loadedRecipes);
            Assert.Equal("recipe_custom_plasma", loadedRecipes[0].id);
            Assert.Equal(plasma, loadedRecipes[0].result);
            Assert.Equal(food, loadedRecipes[0].ingredients[0].item);

            // Assert starting survivor from JSON
            Assert.Single(loadedSurvivors);
            Assert.Equal("survivor_modded_scout", loadedSurvivors[0].id);
            Assert.Equal("Modded Wasteland Scout", loadedSurvivors[0].displayName);

            // Assert expedition target from JSON
            var vault = ExpeditionDefinitionRegistry.Get("loc_secret_research_vault");
            Assert.NotNull(vault);
            Assert.Equal(9, vault!.dangerLevel);
            Assert.Equal(12, vault.distanceTicks);
            Assert.Contains("custom_plasma_fuel", vault.lootCategories);
        }

        [Fact]
        public void SaveCompatibility_PersistedItemAndSurvivorIdsResolveAgainstJsonCatalogs()
        {
            string dataDir = FindDataDirectory();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var catalog = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);

            // Simulated inventory save
            var invSave = new InventorySaveState
            {
                slots = new List<SlotSave>
                {
                    new SlotSave { itemId = "canned_food", amount = 10 },
                    new SlotSave { itemId = "clean_water", amount = 8 },
                    new SlotSave { itemId = "bandage", amount = 4 },
                    new SlotSave { itemId = "rad_away", amount = 2 }
                },
                equipped = new List<EquippedSave>
                {
                    new EquippedSave { itemId = "gas_mask", durability = 85f },
                    new EquippedSave { itemId = "hazmat_suit", durability = 90f }
                }
            };

            var inventory = new Ashfall.Core.Inventory.Inventory();
            inventory.RestoreState(invSave, id => catalog.Get(id));

            Assert.Equal(10, inventory.CountById("canned_food"));
            Assert.Equal(8, inventory.CountById("clean_water"));
            Assert.Equal(4, inventory.CountById("bandage"));
            Assert.Equal(2, inventory.CountById("rad_away"));
            Assert.True(inventory.GetEquippedProtection() > 0f);
        }
    }
}
