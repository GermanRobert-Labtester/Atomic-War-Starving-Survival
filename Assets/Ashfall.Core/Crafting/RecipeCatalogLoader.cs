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

        /// <summary>
        /// Recipe ids in recipes.json that have <c>resultAmount = 0</c>
        /// because their real effect is a non-inventory action (heater
        /// refuel, thermal-pipe thaw, deep rebar injection, advanced water
        /// purifier rebuild, desalination still rebuild, improvised heater
        /// rebuild — each preserves its named output as a documentary
        /// status token; the inventory is not consumed and not produced)
        /// wired through gameplay systems external to this loader. They are
        /// explicitly allowlisted so a strict-mode load does not break
        /// them; new recipes with zero resultAmount MUST NOT be added to
        /// this set without first giving them an authoritative non-inventory
        /// command surface. Plan 10 remediation removed the sink
        /// "lubricate_weapon" — it was NOT in the allowlist and is now
        /// expected to fail closed under any loader run.
        /// </summary>
        public static readonly System.Collections.Generic.HashSet<string> LegacyZeroResultAllowlist =
            new System.Collections.Generic.HashSet<string>(System.StringComparer.Ordinal)
            {
                "refuel_heater",
                "thaw_frozen_pipe",
                "inject_concrete_pillar",
                "craft_advanced_water_purifier",
                "craft_desalination_still",
                "craft_improvised_heater"
            };

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

                    // Strict-mode validation (Plan 10 remediation): a recipe
                    // that declares a result item but produces zero of it is
                    // a sink — it consumes player inventory for no defined
                    // outcome. The legacy allowlist below is the one
                    // permitted exception for pre-existing non-inventory
                    // effects. Any new entry must not be added to it without
                    // an authoritative non-inventory command surface.
                    if (dto.resultAmount <= 0
                        && !string.IsNullOrEmpty(dto.resultItemId)
                        && !LegacyZeroResultAllowlist.Contains(dto.id))
                    {
                        throw new System.IO.InvalidDataException(
                            "recipes.json recipe '" + dto.id
                            + "' declares resultItemId='" + dto.resultItemId
                            + "' but resultAmount=" + dto.resultAmount
                            + " — a normal inventory recipe cannot silently consume inputs with a zero result."
                            + " Either set resultAmount > 0 or remove the result item; this loader forbids the sink pattern.");
                    }

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
            catch (System.IO.InvalidDataException)
            {
                // Re-raise hard validation errors (sink-pattern rejection and
                // future schema-version-mismatch errors). These MUST bubble out
                // — a malformed recipes.json must never silently abort and
                // continue with a partial catalog.
                throw;
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
