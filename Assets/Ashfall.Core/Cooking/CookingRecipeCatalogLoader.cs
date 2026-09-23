// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Cooking
{
    public sealed class CookingRecipeCatalogLoadResult
    {
        public bool Success => Errors.Count == 0;
        public List<CookingRecipe> Recipes { get; } = new List<CookingRecipe>();
        public List<string> Errors { get; } = new List<string>();
        public int SchemaVersion { get; set; } = 1;
    }

    /// <summary>
    /// Strict loader for recipes_cooking.json (Plan 136 / DEC-306).
    /// Enforces unique recipe IDs, valid output items and quantities, and non-empty inputs.
    /// </summary>
    public static class CookingRecipeCatalogLoader
    {
        public const string DefaultFileName = "recipes_cooking.json";

        public static CookingRecipeCatalogLoadResult Load(string dataDir, IFileIO fileIO)
        {
            var result = new CookingRecipeCatalogLoadResult();
            if (fileIO == null)
            {
                result.Errors.Add("IFileIO is null.");
                return result;
            }

            if (string.IsNullOrWhiteSpace(dataDir))
            {
                result.Errors.Add("Data directory is empty.");
                return result;
            }

            string filePath = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(filePath))
            {
                result.Errors.Add($"Recipe catalog file not found: {filePath}");
                return result;
            }

            string json;
            try
            {
                json = fileIO.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Failed to read {filePath}: {ex.Message}");
                return result;
            }

            return LoadFromJson(json);
        }

        public static CookingRecipeCatalogLoadResult LoadFromJson(string json)
        {
            var result = new CookingRecipeCatalogLoadResult();
            if (string.IsNullOrWhiteSpace(json))
            {
                result.Errors.Add("Recipe catalog JSON is empty.");
                return result;
            }

            try
            {
                var recipes = CatalogLocator.LoadWrappedList<CookingRecipe>(json, SystemTextJsonSerializer.Options);
                if (recipes == null || recipes.Count == 0)
                {
                    result.Errors.Add("No recipes found in JSON catalog.");
                    return result;
                }

                var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var r in recipes)
                {
                    if (string.IsNullOrWhiteSpace(r.id))
                    {
                        result.Errors.Add("Encountered recipe with empty ID.");
                        continue;
                    }

                    if (!seenIds.Add(r.id))
                    {
                        result.Errors.Add($"Duplicate recipe ID: {r.id}");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(r.outputItemId))
                    {
                        result.Errors.Add($"Recipe {r.id} has no outputItemId.");
                        continue;
                    }

                    if (r.outputQuantity <= 0)
                    {
                        result.Errors.Add($"Recipe {r.id} has invalid outputQuantity {r.outputQuantity}.");
                        continue;
                    }

                    if (r.cookTimeMinutes < 0f)
                    {
                        result.Errors.Add($"Recipe {r.id} has negative cookTimeMinutes {r.cookTimeMinutes}.");
                        continue;
                    }

                    result.Recipes.Add(r);
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Exception parsing recipe JSON: {ex.Message}");
            }

            return result;
        }
    }
}
