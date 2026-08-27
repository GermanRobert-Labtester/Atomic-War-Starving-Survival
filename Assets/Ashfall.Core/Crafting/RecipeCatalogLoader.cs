using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.IO;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Crafting
{
    [Serializable]
    internal sealed class RecipeJsonDto
    {
        public string id { get; set; } = string.Empty;
        public string recipeName { get; set; } = string.Empty;
        public List<RecipeIngredientJsonDto>? ingredients { get; set; }
        public string resultItemId { get; set; } = string.Empty;
        public int resultAmount { get; set; } = 1;
        public float craftingTimeHours { get; set; } = 1f;
        public string requiredStationId { get; set; } = string.Empty;
    }

    [Serializable]
    internal sealed class RecipeIngredientJsonDto
    {
        public string itemId { get; set; } = string.Empty;
        public int amount { get; set; } = 1;
    }

    /// <summary>
    /// Shared Core loader for crafting recipes from recipes.json (authority).
    /// Resolves ingredient and result ItemDefinitions from the supplied ItemCatalog.
    /// Zero engine dependencies; adheres to Invariant 1 and Invariant 6.
    /// </summary>
    public static class RecipeCatalogLoader
    {
        public const string FileName = "recipes.json";

        public static List<Recipe> Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer, ItemCatalog catalog)
        {
            var loadResult = LoadWithResult(dataDir, fileIO, serializer, catalog);
            return loadResult.Entries.ToList();
        }

        public static CatalogLoadResult<Recipe> LoadWithResult(
            string dataDir, IFileIO fileIO, IJsonSerializer serializer, ItemCatalog catalog)
        {
            string path = fileIO.Combine(dataDir, FileName);
            var result = new CatalogLoadResult<Recipe>(
                path,
                "Recipe list",
                CatalogClassification.Required);

            if (fileIO == null || serializer == null || catalog == null || string.IsNullOrEmpty(dataDir))
            {
                if (catalog == null || fileIO == null || serializer == null || string.IsNullOrEmpty(dataDir))
                    result.AddFatal("Required dependencies are null");
                return result;
            }

            if (!fileIO.FileExists(path))
            {
                result.AddFatal("Required catalog file not found: " + path);
                return result;
            }

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
            {
                result.AddError("Catalog file is empty: " + path);
                return result;
            }

            try
            {
                var dtos = CatalogLocator.LoadWrappedList<RecipeJsonDto>(raw, SystemTextJsonSerializer.Options);
                for (int i = 0; i < dtos.Count; i++)
                {
                    var dto = dtos[i];
                    if (dto == null || string.IsNullOrEmpty(dto.id)) continue;

                    var recipe = new Recipe
                    {
                        id = dto.id,
                        recipeName = !string.IsNullOrEmpty(dto.recipeName) ? dto.recipeName : dto.id,
                        result = !string.IsNullOrEmpty(dto.resultItemId) ? catalog.Get(dto.resultItemId)! : null!,
                        resultAmount = dto.resultAmount > 0 ? dto.resultAmount : 1,
                        craftingTimeHours = dto.craftingTimeHours > 0f ? dto.craftingTimeHours : 1f,
                        requiredStationId = dto.requiredStationId ?? string.Empty
                    };

                    if (dto.ingredients != null)
                    {
                        for (int j = 0; j < dto.ingredients.Count; j++)
                        {
                            var ing = dto.ingredients[j];
                            if (ing == null || string.IsNullOrEmpty(ing.itemId)) continue;
                            var def = catalog.Get(ing.itemId);
                            if (def != null)
                            {
                                recipe.ingredients.Add(new Ingredient
                                {
                                    item = def,
                                    amount = ing.amount > 0 ? ing.amount : 1
                                });
                            }
                        }
                    }

                    result.AddEntry(recipe);
                }
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn("RecipeCatalogLoader", FileName, ex);
                result.AddFatal("Failed to load recipes: " + ex.Message, ex);
            }

            return result;
        }
    }
}
