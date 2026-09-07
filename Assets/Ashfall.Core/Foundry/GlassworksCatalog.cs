// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.IO;

namespace Ashfall.Core.Foundry
{
    /// <summary>One authored B100 glassworks charge ingredient.</summary>
    [Serializable]
    public sealed class GlassworksIngredientEntry
    {
        public string item_id = string.Empty;
        public int amount = 1;
    }

    /// <summary>
    /// One glassworks product projected into SilentFoundrySystem's ordinary
    /// heat/casting lifecycle. Values are gameplay abstractions, not process
    /// instructions.
    /// </summary>
    [Serializable]
    public sealed class GlassworksRecipeEntry
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public List<GlassworksIngredientEntry> input_items = new List<GlassworksIngredientEntry>();
        public int fuel_units;
        public int water_litres;
        public int labor_days = 1;
        public float skill_target = 0.5f;
        public float quality_target = 70f;
        public string result_item_id = string.Empty;
        public int result_amount = 1;
        public string[] tags = Array.Empty<string>();

        public FoundryProductEntry ToProductEntry()
        {
            var entry = new FoundryProductEntry
            {
                product_id = id,
                display_name = display_name,
                category = "glass_works",
                result_item_id = result_item_id,
                result_amount = result_amount,
                labor_hours = labor_days * 24f,
                cast_hours = labor_days * 12f,
                fuel_units = fuel_units,
                water_litres = water_litres,
                skill_target = skill_target,
                quality_target = quality_target,
                sink = "glassworks_b100",
                notes = "Plan B100 scientific glassworks roster",
                tags = tags
            };

            foreach (var input in input_items)
            {
                if (input == null || string.IsNullOrWhiteSpace(input.item_id)) continue;
                entry.ingredients.Add(new FoundryIngredientEntry
                {
                    item_id = input.item_id,
                    amount = input.amount
                });
            }
            return entry;
        }
    }

    [Serializable]
    public sealed class GlassworksRecipesFile
    {
        public int schema_version = 1;
        public string collection_id = string.Empty;
        public List<GlassworksRecipeEntry> recipes = new List<GlassworksRecipeEntry>();
    }

    /// <summary>Validated in-memory view of the B100 authored roster.</summary>
    public sealed class GlassworksCatalog
    {
        private readonly Dictionary<string, GlassworksRecipeEntry> _byId =
            new Dictionary<string, GlassworksRecipeEntry>(StringComparer.Ordinal);

        public List<string> Errors { get; } = new List<string>();
        public IReadOnlyCollection<GlassworksRecipeEntry> Recipes => _byId.Values;

        public void Load(GlassworksRecipesFile? file)
        {
            if (file == null) return;
            if (file.schema_version != 1)
                Errors.Add("unsupported schema_version " + file.schema_version);

            var recipes = file.recipes ?? new List<GlassworksRecipeEntry>();
            for (int i = 0; i < recipes.Count; i++)
            {
                var recipe = recipes[i];
                if (recipe == null || string.IsNullOrWhiteSpace(recipe.id))
                {
                    Errors.Add("recipes[" + i + "]: missing id");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(recipe.result_item_id))
                    Errors.Add(recipe.id + ": missing result_item_id");
                if (recipe.input_items == null || recipe.input_items.Count == 0)
                    Errors.Add(recipe.id + ": no input_items");
                if (recipe.input_items != null)
                {
                    foreach (var input in recipe.input_items)
                    {
                        if (input == null || string.IsNullOrWhiteSpace(input.item_id))
                            Errors.Add(recipe.id + ": input item missing id");
                        else if (input.amount < 1)
                            Errors.Add(recipe.id + ": input item amount must be >= 1");
                    }
                }
                if (recipe.fuel_units < 0 || recipe.water_litres < 0)
                    Errors.Add(recipe.id + ": charge costs cannot be negative");
                if (recipe.labor_days < 1)
                    Errors.Add(recipe.id + ": labor_days must be >= 1");
                if (recipe.result_amount < 1)
                    Errors.Add(recipe.id + ": result_amount must be >= 1");
                if (_byId.ContainsKey(recipe.id))
                {
                    Errors.Add(recipe.id + ": duplicate recipe id");
                    continue;
                }
                _byId[recipe.id] = recipe;
            }
        }

        public GlassworksRecipeEntry? GetRecipe(string recipeId)
            => !string.IsNullOrEmpty(recipeId) && _byId.TryGetValue(recipeId, out var recipe)
                ? recipe
                : null;
    }

    public static class GlassworksCatalogLoader
    {
        public const string FileName = "glassworks_recipes.json";

        public static GlassworksCatalog Load(
            string dataDirectory,
            IFileIO? files = null,
            IJsonSerializer? serializer = null)
        {
            files ??= new FileSystemIO();
            serializer ??= new SystemTextJsonSerializer();
            var catalog = new GlassworksCatalog();
            string path = Path.Combine(dataDirectory, FileName);
            if (!files.FileExists(path)) return catalog;
            string text = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(text)) return catalog;
            try
            {
                catalog.Load(serializer.Deserialize<GlassworksRecipesFile>(text));
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(path, "GlassworksRecipesFile", ex);
                catalog.Errors.Add("load failed: " + ex.Message);
            }
            return catalog;
        }
    }
}
