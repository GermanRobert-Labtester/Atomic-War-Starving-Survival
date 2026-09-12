// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.IO;

namespace Ashfall.Core.Foundry
{
    // ---------------------------------------------------------------------
    // Plan B66 — Subterranean Heavy Manufacturing & Metallurgical Smelting.
    // Expansion of the Silent Foundry authority (Options A gate honored:
    // no competing MetallurgySystem; this catalog feeds the existing
    // SilentFoundrySystem heat machine through a catalog merge).
    // ---------------------------------------------------------------------

    [Serializable]
    public sealed class MetallurgyIngredientEntry
    {
        public string item_id = string.Empty;
        public int amount = 1;
    }

    [Serializable]
    public sealed class MetallurgyRecipeEntry
    {
        public string id = string.Empty;
        public string display_name = string.Empty;

        /// <summary>Charge materials consumed at batch start (atomic).</summary>
        public List<MetallurgyIngredientEntry> input_items = new List<MetallurgyIngredientEntry>();

        public int fuel_units = 0;
        public int water_litres = 0;

        /// <summary>Authored gameplay tier 1..3 (no real furnace temperatures).</summary>
        public int process_heat_tier = 1;

        public string flux_item_id = string.Empty;
        public int flux_amount = 0;

        /// <summary>Slag units (0..100 normalized scale) deposited per batch.</summary>
        public float slag_yield = 0f;

        public int labor_days = 1;
        public float skill_target = 0.5f;
        public float quality_target = 70f;

        public string result_item_id = string.Empty;
        public int result_amount = 1;

        public string[] tags = Array.Empty<string>();

        /// <summary>
        /// Project onto the existing foundry product entry so the heavy roster
        /// rides the standard heat stage machine, quality roll and output
        /// transaction without a parallel production path.
        /// </summary>
        public FoundryProductEntry ToProductEntry()
        {
            var entry = new FoundryProductEntry
            {
                product_id = id,
                display_name = display_name,
                result_item_id = result_item_id,
                result_amount = result_amount,
                labor_hours = labor_days * 24f,
                cast_hours = labor_days * 12f,
                fuel_units = fuel_units,
                water_litres = water_litres,
                skill_target = skill_target,
                quality_target = quality_target,
                category = "heavy_metallurgy",
                sink = "metallurgy_b66",
                notes = "Plan B66 heavy metallurgy roster",
                tags = tags
            };
            for (int i = 0; i < input_items.Count; i++)
            {
                if (input_items[i] == null || string.IsNullOrEmpty(input_items[i].item_id)) continue;
                entry.ingredients.Add(new FoundryIngredientEntry
                {
                    item_id = input_items[i].item_id,
                    amount = input_items[i].amount
                });
            }
            return entry;
        }
    }

    [Serializable]
    public sealed class MetallurgyRecipesFile
    {
        public int schema_version = 1;
        public string collection_id = string.Empty;
        public List<MetallurgyRecipeEntry> recipes = new List<MetallurgyRecipeEntry>();
    }

    /// <summary>
    /// Runtime catalog of heavy metallurgy recipes. Loaded from the data
    /// authority (metallurgy_recipes.json); never mutated by gameplay.
    /// </summary>
    public sealed class MetallurgyHeavyCatalog
    {
        private readonly Dictionary<string, MetallurgyRecipeEntry> _byId =
            new Dictionary<string, MetallurgyRecipeEntry>(StringComparer.Ordinal);

        public List<string> Errors { get; } = new List<string>();
        public IReadOnlyCollection<MetallurgyRecipeEntry> Recipes => _byId.Values;

        public void Load(MetallurgyRecipesFile file)
        {
            if (file?.recipes == null) return;
            for (int i = 0; i < file.recipes.Count; i++)
            {
                var r = file.recipes[i];
                if (r == null || string.IsNullOrEmpty(r.id))
                {
                    Errors.Add("recipes[" + i + "]: missing id");
                    continue;
                }
                if (string.IsNullOrEmpty(r.result_item_id))
                    Errors.Add(r.id + ": missing result_item_id");
                if (r.input_items == null || r.input_items.Count == 0)
                    Errors.Add(r.id + ": no input_items");
                if (r.process_heat_tier < 1 || r.process_heat_tier > 3)
                    Errors.Add(r.id + ": process_heat_tier out of range 1..3");
                if (r.labor_days < 1)
                    Errors.Add(r.id + ": labor_days must be >= 1");
                if (r.result_amount < 1)
                    Errors.Add(r.id + ": result_amount must be >= 1");
                if (_byId.ContainsKey(r.id))
                {
                    Errors.Add(r.id + ": duplicate recipe id");
                    continue;
                }
                _byId[r.id] = r;
            }
        }

        public MetallurgyRecipeEntry? GetRecipe(string recipeId)
        {
            if (string.IsNullOrEmpty(recipeId)) return null;
            return _byId.TryGetValue(recipeId, out var entry) ? entry : null;
        }
    }

    /// <summary>Engine-agnostic loader for metallurgy_recipes.json.</summary>
    public static class MetallurgyCatalogLoader
    {
        public const string FileName = "metallurgy_recipes.json";

        public static MetallurgyHeavyCatalog Load(
            string dataDirectory,
            IFileIO? files = null,
            IJsonSerializer? serializer = null)
        {
            files = files ?? new FileSystemIO();
            serializer = serializer ?? new SystemTextJsonSerializer();
            var catalog = new MetallurgyHeavyCatalog();
            string path = Path.Combine(dataDirectory, FileName);
            if (!files.FileExists(path)) return catalog;
            string text = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(text)) return catalog;
            try
            {
                var file = serializer.Deserialize<MetallurgyRecipesFile>(text);
                catalog.Load(file);
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(path, "MetallurgyRecipesFile", ex_CATDIAG);
            }
            return catalog;
        }
    }
}
