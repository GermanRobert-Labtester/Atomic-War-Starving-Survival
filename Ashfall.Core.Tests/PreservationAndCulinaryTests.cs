using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class PreservationAndCulinaryTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private sealed class RecipeFileDto
        {
            public int schema_version { get; set; }
            public List<RecipeEntryDto> recipes { get; set; } = new List<RecipeEntryDto>();
        }

        private sealed class RecipeEntryDto
        {
            public string id { get; set; } = string.Empty;
            public string recipeName { get; set; } = string.Empty;
            public List<RecipeIngredientDto> ingredients { get; set; } = new List<RecipeIngredientDto>();
            public string resultItemId { get; set; } = string.Empty;
            public int resultAmount { get; set; }
            public float craftingTimeHours { get; set; }
            public string requiredStationId { get; set; } = string.Empty;
        }

        private sealed class RecipeIngredientDto
        {
            public string itemId { get; set; } = string.Empty;
            public int amount { get; set; }
        }

        [Theory]
        [InlineData("craft_pickled_tubers", "item_pickled_tubers", 4, "stove")]
        [InlineData("craft_dried_mushrooms", "item_dried_mushrooms", 6, "stove")]
        [InlineData("craft_smoked_meat_rations", "item_smoked_meat", 4, "stove")]
        [InlineData("craft_canned_grain_stew", "item_canned_grain_stew", 3, "stove")]
        [InlineData("craft_salted_fish_meat", "item_salted_meat", 5, "workbench")]
        [InlineData("craft_rendered_fat_confit", "item_fat_confit", 3, "stove")]
        [InlineData("craft_fermented_sauerkraut", "item_fermented_sauerkraut", 5, "workbench")]
        [InlineData("craft_honey_preserved_pulp", "item_honey_preserved_pulp", 4, "stove")]
        [InlineData("craft_dried_herb_packets", "item_dried_herb_packets", 4, "workbench")]
        [InlineData("craft_brined_legume_mash", "item_brined_legume_mash", 4, "stove")]
        public void PreservationRecipes_AreAuthoredAndValid(string recipeId, string expectedResultItem, int expectedAmount, string expectedStation)
        {
            string dataDir = FindDataDir();
            string recipesPath = Path.Combine(dataDir, "recipes.json");
            string json = File.ReadAllText(recipesPath);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var file = JsonSerializer.Deserialize<RecipeFileDto>(json, options);
            Assert.NotNull(file);

            var recipe = file.recipes.Find(r => r.id == recipeId);
            Assert.NotNull(recipe);
            Assert.Equal(expectedResultItem, recipe.resultItemId);
            Assert.Equal(expectedAmount, recipe.resultAmount);
            Assert.Equal(expectedStation, recipe.requiredStationId);
            Assert.True(recipe.craftingTimeHours > 0);
            Assert.NotEmpty(recipe.ingredients);
        }

        [Fact]
        public void KitchenNutritionSystem_PantrySupportsPreservationMethods()
        {
            var pantryItem = new PantryItem
            {
                itemId = "food_pickled_tubers",
                displayName = "Pickled Greenhouse Tubers",
                spoilageTimer = 45f,
                maxSpoilageDays = 45f,
                preservation = PreservationMethod.Fermentation,
                portionCount = 10,
                isSpoiled = false
            };

            Assert.Equal("food_pickled_tubers", pantryItem.itemId);
            Assert.Equal(PreservationMethod.Fermentation, pantryItem.preservation);
            Assert.Equal(45f, pantryItem.maxSpoilageDays);
            Assert.False(pantryItem.isSpoiled);
        }
    }
}
