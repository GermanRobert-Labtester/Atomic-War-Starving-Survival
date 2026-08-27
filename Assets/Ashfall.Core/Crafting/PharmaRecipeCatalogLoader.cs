using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.IO;

namespace Ashfall.Core.Crafting
{
    [Serializable]
    internal sealed class PharmaRecipeJsonDto
    {
        public string recipe_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public List<string>? input_ids { get; set; }
        public List<int>? input_amounts { get; set; }
        public string output_item_id { get; set; } = string.Empty;
        public int output_amount { get; set; } = 1;
        public float base_hours { get; set; } = 2f;
        public float required_temperature { get; set; } = 80f;
        public float purity_target { get; set; } = 0.9f;
        public float dependency_risk { get; set; } = 0f;
        public string required_station { get; set; } = "pharma_bench";
        public string category { get; set; } = "pharmaceutical";
    }

    /// <summary>
    /// Core loader for pharmaceutical compounding & distillation recipes from pharma_recipes.json.
    /// Engine-agnostic, zero host dependencies.
    /// </summary>
    public static class PharmaRecipeCatalogLoader
    {
        public const string FileName = "pharma_recipes.json";

        public static PharmaRecipeCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            var result = LoadWithResult(dataDir, fileIO, serializer);
            var catalog = new PharmaRecipeCatalog();
            catalog.recipes = result.Entries.ToList();
            return catalog;
        }

        public static CatalogLoadResult<PharmaRecipe> LoadWithResult(
            string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            string path = fileIO.Combine(dataDir, FileName);
            var result = new CatalogLoadResult<PharmaRecipe>(
                path,
                "Pharmaceutical recipe catalog",
                CatalogClassification.Required);

            if (fileIO == null || serializer == null || string.IsNullOrEmpty(dataDir))
            {
                result.AddFatal("Required dependencies are null");
                return result;
            }

            if (!fileIO.FileExists(path))
            {
                result.AddFatal("Pharma recipes catalog file not found: " + path);
                return result;
            }

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
            {
                result.AddError("Pharma recipes catalog file is empty: " + path);
                return result;
            }

            try
            {
                var dtos = CatalogLocator.LoadWrappedList<PharmaRecipeJsonDto>(raw, SystemTextJsonSerializer.Options);
                for (int i = 0; i < dtos.Count; i++)
                {
                    var dto = dtos[i];
                    if (dto == null || string.IsNullOrEmpty(dto.recipe_id)) continue;

                    var recipe = new PharmaRecipe
                    {
                        recipe_id = dto.recipe_id,
                        display_name = !string.IsNullOrEmpty(dto.display_name) ? dto.display_name : dto.recipe_id,
                        input_ids = dto.input_ids ?? new List<string>(),
                        input_amounts = dto.input_amounts ?? new List<int>(),
                        output_item_id = dto.output_item_id ?? string.Empty,
                        output_amount = dto.output_amount > 0 ? dto.output_amount : 1,
                        base_hours = dto.base_hours > 0f ? dto.base_hours : 2f,
                        required_temperature = dto.required_temperature,
                        purity_target = dto.purity_target > 0f ? dto.purity_target : 0.85f,
                        dependency_risk = dto.dependency_risk,
                        required_station = !string.IsNullOrEmpty(dto.required_station) ? dto.required_station : "pharma_bench",
                        category = !string.IsNullOrEmpty(dto.category) ? dto.category : "pharmaceutical"
                    };

                    result.AddEntry(recipe);
                }
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn("PharmaRecipeCatalogLoader", FileName, ex);
                result.AddFatal("Failed to load pharma recipes: " + ex.Message, ex);
            }

            return result;
        }
    }
}
