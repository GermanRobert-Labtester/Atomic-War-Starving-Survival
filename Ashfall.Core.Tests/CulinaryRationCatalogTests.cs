using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class CulinaryRationCatalogTests
    : CatalogTestBase{
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void CulinaryRation_LoadsAll30CanonicalRecipes()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "culinary_ration_codex.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new CulinaryRationCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(30, catalog.AllRecipes.Count);

            // Test first recipe (Lard-Fried Oyster Mushrooms on Sourdough)
            var r1 = catalog.GetById("dish_01_fried_oyster_mushrooms_on_sourdough");
            Assert.NotNull(r1);
            Assert.Equal(480.0f, r1.calories_per_portion);
            Assert.Equal(6, r1.daily_morale_modifier);
            Assert.Contains("screaming hot pork lard", r1.preparation_instructions);
            Assert.Contains("Master Oleg trades half his tobacco ration", r1.canteen_gossip_review);

            // Test high morale dishes (>= 7 morale boost)
            var highMorale = catalog.GetHighMoraleDishes(7);
            Assert.True(highMorale.Count >= 10);

            // Test category search
            var soups = catalog.GetByCategory("Soups");
            Assert.True(soups.Count >= 4);

            // Test finale dish (The Century Apple Tart)
            var r30 = catalog.GetById("dish_30_the_century_seed_harvest_apple_tart");
            Assert.NotNull(r30);
            Assert.Equal(10, r30.daily_morale_modifier);
            Assert.Equal(650.0f, r30.calories_per_portion);
            Assert.Contains("Day 3650 Ratification Banquet", r30.canteen_gossip_review);

            // Test tag search
            var dessert = catalog.GetByTag("dessert");
            Assert.True(dessert.Count >= 4);
        }

        [Fact]
        public void CulinaryRation_AllEntriesHaveValidFieldsAndIngredients()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "culinary_ration_codex.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new CulinaryRationCatalog();
            catalog.Load(json, serializer);

            foreach (var r in catalog.AllRecipes)
            {
                Assert.False(string.IsNullOrWhiteSpace(r.recipe_id), "Missing recipe_id");
                Assert.False(string.IsNullOrWhiteSpace(r.dish_name), $"Missing dish_name on {r.recipe_id}");
                Assert.False(string.IsNullOrWhiteSpace(r.meal_category), $"Missing meal_category on {r.recipe_id}");
                Assert.True(r.calories_per_portion > 0f, $"Invalid calories on {r.recipe_id}");
                Assert.True(r.daily_morale_modifier > 0, $"Invalid morale modifier on {r.recipe_id}");
                Assert.NotNull(r.required_ingredients);
                Assert.True(r.required_ingredients.Length > 0, $"Ingredients empty on {r.recipe_id}");
                Assert.False(string.IsNullOrWhiteSpace(r.preparation_instructions), $"Missing prep steps on {r.recipe_id}");
                Assert.True(r.preparation_instructions.Length > 30, $"Prep steps too brief on {r.recipe_id}");
                Assert.False(string.IsNullOrWhiteSpace(r.canteen_gossip_review), $"Missing gossip review on {r.recipe_id}");
                Assert.NotNull(r.tags);
                Assert.True(r.tags.Length > 0, $"Tags empty on {r.recipe_id}");
            }
        }
    }
}
