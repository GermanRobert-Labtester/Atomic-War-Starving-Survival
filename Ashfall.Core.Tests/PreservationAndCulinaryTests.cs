// SPDX-License-Identifier: MIT
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

        [Fact]
        public void PreservationRecipes_CatalogTable_IsAuthoredAndValid()
        {
            string dataDir = FindDataDir();
            string recipesPath = Path.Combine(dataDir, "recipes.json");
            string json = File.ReadAllText(recipesPath);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var file = JsonSerializer.Deserialize<RecipeFileDto>(json, options);
            Assert.NotNull(file);
            var failures = new List<string>();

            foreach (var testCase in new[]
            {
                (RecipeId: "craft_pickled_tubers", ExpectedResultItem: "item_pickled_tubers", ExpectedAmount: 4, ExpectedStation: "stove"),
                (RecipeId: "craft_dried_mushrooms", ExpectedResultItem: "item_dried_mushrooms", ExpectedAmount: 6, ExpectedStation: "stove"),
                (RecipeId: "craft_smoked_meat_rations", ExpectedResultItem: "item_smoked_meat", ExpectedAmount: 4, ExpectedStation: "stove"),
                (RecipeId: "craft_canned_grain_stew", ExpectedResultItem: "item_canned_grain_stew", ExpectedAmount: 3, ExpectedStation: "stove"),
                (RecipeId: "craft_salted_fish_meat", ExpectedResultItem: "item_salted_meat", ExpectedAmount: 5, ExpectedStation: "workbench"),
                (RecipeId: "craft_rendered_fat_confit", ExpectedResultItem: "item_fat_confit", ExpectedAmount: 3, ExpectedStation: "stove"),
                (RecipeId: "craft_fermented_sauerkraut", ExpectedResultItem: "item_fermented_sauerkraut", ExpectedAmount: 5, ExpectedStation: "workbench"),
                (RecipeId: "craft_honey_preserved_pulp", ExpectedResultItem: "item_honey_preserved_pulp", ExpectedAmount: 4, ExpectedStation: "stove"),
                (RecipeId: "craft_dried_herb_packets", ExpectedResultItem: "item_dried_herb_packets", ExpectedAmount: 4, ExpectedStation: "workbench"),
                (RecipeId: "craft_brined_legume_mash", ExpectedResultItem: "item_brined_legume_mash", ExpectedAmount: 4, ExpectedStation: "stove"),
            })
            {
                var recipe = file!.recipes.Find(r => r.id == testCase.RecipeId);
                if (recipe == null)
                {
                    failures.Add($"recipe '{testCase.RecipeId}' is missing");
                    continue;
                }

                if (recipe.resultItemId != testCase.ExpectedResultItem)
                {
                    failures.Add(
                        $"recipe '{testCase.RecipeId}' expected result '{testCase.ExpectedResultItem}', got '{recipe.resultItemId}'");
                }

                if (recipe.resultAmount != testCase.ExpectedAmount)
                {
                    failures.Add(
                        $"recipe '{testCase.RecipeId}' expected amount {testCase.ExpectedAmount}, got {recipe.resultAmount}");
                }

                if (recipe.requiredStationId != testCase.ExpectedStation)
                {
                    failures.Add(
                        $"recipe '{testCase.RecipeId}' expected station '{testCase.ExpectedStation}', got '{recipe.requiredStationId}'");
                }

                if (recipe.craftingTimeHours <= 0)
                {
                    failures.Add($"recipe '{testCase.RecipeId}' must have positive crafting time");
                }

                if (recipe.ingredients.Count == 0)
                {
                    failures.Add($"recipe '{testCase.RecipeId}' must have ingredients");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
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
