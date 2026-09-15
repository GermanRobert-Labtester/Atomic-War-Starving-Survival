// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ashfall.Core.Shelter
{
    public sealed class PreservationTierDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int shelf_life_days { get; set; } = 5;
        public float shelf_life_multiplier { get; set; } = 1.0f;
        public int power_draw_watts { get; set; } = 0;
        public int flavor_morale_bonus { get; set; } = 0;
        public float spoilage_toxin_risk { get; set; } = 0.1f;
        public List<string> allowed_food_types { get; set; } = new List<string>();
    }

    public sealed class CuringRecipeDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string target_tier { get; set; } = string.Empty;
        public string input_item_id { get; set; } = string.Empty;
        public int input_quantity { get; set; } = 1;
        public string preservative_item_id { get; set; } = string.Empty;
        public int preservative_quantity { get; set; } = 1;
        public string output_item_id { get; set; } = string.Empty;
        public int output_quantity { get; set; } = 1;
        public int work_ticks_required { get; set; } = 4;
    }

    public sealed class FoodPreservationCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<PreservationTierDef> preservation_tiers { get; set; } = new List<PreservationTierDef>();
        public List<CuringRecipeDef> curing_recipes { get; set; } = new List<CuringRecipeDef>();
        /// <summary>Item id → food type string matching tier <c>allowed_food_types</c>.</summary>
        public Dictionary<string, string> food_type_by_item_id { get; set; } = new Dictionary<string, string>(StringComparer.Ordinal);

        private readonly Dictionary<string, PreservationTierDef> _tiersById = new Dictionary<string, PreservationTierDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, CuringRecipeDef> _recipesById = new Dictionary<string, CuringRecipeDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, string> _foodTypeByItemId = new Dictionary<string, string>(StringComparer.Ordinal);

        public void Index()
        {
            _tiersById.Clear();
            _recipesById.Clear();
            _foodTypeByItemId.Clear();

            foreach (var tier in preservation_tiers)
            {
                if (!string.IsNullOrEmpty(tier.id))
                    _tiersById[tier.id] = tier;
            }

            foreach (var recipe in curing_recipes)
            {
                if (!string.IsNullOrEmpty(recipe.id))
                    _recipesById[recipe.id] = recipe;
            }

            if (food_type_by_item_id != null)
            {
                foreach (var pair in food_type_by_item_id)
                {
                    if (!string.IsNullOrEmpty(pair.Key) && !string.IsNullOrEmpty(pair.Value))
                        _foodTypeByItemId[pair.Key] = pair.Value;
                }
            }
        }

        public PreservationTierDef? GetTier(string tierId)
        {
            if (string.IsNullOrEmpty(tierId)) return null;
            _tiersById.TryGetValue(tierId, out var tier);
            return tier;
        }

        public CuringRecipeDef? GetRecipe(string recipeId)
        {
            if (string.IsNullOrEmpty(recipeId)) return null;
            _recipesById.TryGetValue(recipeId, out var recipe);
            return recipe;
        }

        /// <summary>
        /// Resolves a catalog item id to the food-type token used by tier allow-lists.
        /// Missing map entries fall back to the item id itself (identity).
        /// </summary>
        public string ResolveFoodType(string foodItemId)
        {
            if (string.IsNullOrEmpty(foodItemId)) return string.Empty;
            if (_foodTypeByItemId.TryGetValue(foodItemId, out var mapped) && !string.IsNullOrEmpty(mapped))
                return mapped;
            return foodItemId;
        }

        /// <summary>
        /// Empty or null <c>allowed_food_types</c> means unrestricted (backward compatible).
        /// </summary>
        public bool IsFoodTypeAllowed(PreservationTierDef? tier, string foodType)
        {
            if (tier?.allowed_food_types == null || tier.allowed_food_types.Count == 0)
                return true;
            if (string.IsNullOrEmpty(foodType)) return false;
            for (int i = 0; i < tier.allowed_food_types.Count; i++)
            {
                if (string.Equals(tier.allowed_food_types[i], foodType, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }
    }

    public static class FoodPreservationCatalogLoader
    {
        public static FoodPreservationCatalog Load(string dataDir, IFileIO fileIo)
        {
            string path = Path.Combine(dataDir, "food_preservation.json");
            if (!fileIo.FileExists(path))
            {
                return new FoodPreservationCatalog();
            }

            string json = fileIo.ReadAllText(path);
            var catalog = JsonSerializer.Deserialize<FoodPreservationCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new FoodPreservationCatalog();

            catalog.Index();
            return catalog;
        }
    }
}
