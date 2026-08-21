using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class CulinaryRecipeEntry
    {
        public string recipe_id;
        public string dish_name;
        public string meal_category;
        public float calories_per_portion;
        public int daily_morale_modifier;
        public string[] required_ingredients;
        public string preparation_instructions;
        public string canteen_gossip_review;
        public string[] tags;
    }

    [Serializable]
    public sealed class CulinaryRationFile
    {
        public int schema_version;
        public string collection_id;
        public List<CulinaryRecipeEntry> recipes = new List<CulinaryRecipeEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 30 Survival Culinary Recipes & Canteen Ration Protocols.
    /// </summary>
    public sealed class CulinaryRationCatalog
    {
        private readonly Dictionary<string, CulinaryRecipeEntry> _byId =
            new Dictionary<string, CulinaryRecipeEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<CulinaryRecipeEntry> _allRecipes = new List<CulinaryRecipeEntry>();

        public IReadOnlyList<CulinaryRecipeEntry> AllRecipes => _allRecipes;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<CulinaryRationFile>(json);
            if (file?.recipes == null) return;

            foreach (var r in file.recipes)
            {
                if (r == null || string.IsNullOrEmpty(r.recipe_id)) continue;
                _byId[r.recipe_id] = r;
                _allRecipes.Add(r);
            }
        }

        public CulinaryRecipeEntry? GetById(string recipeId)
        {
            if (string.IsNullOrEmpty(recipeId)) return null;
            _byId.TryGetValue(recipeId, out var entry);
            return entry;
        }

        public List<CulinaryRecipeEntry> GetByCategory(string categorySnippet)
        {
            var results = new List<CulinaryRecipeEntry>();
            if (string.IsNullOrEmpty(categorySnippet)) return results;

            for (int i = 0; i < _allRecipes.Count; i++)
            {
                var r = _allRecipes[i];
                if (r.meal_category != null &&
                    r.meal_category.IndexOf(categorySnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(r);
                }
            }
            return results;
        }

        public List<CulinaryRecipeEntry> GetHighMoraleDishes(int minMoraleModifier = 7)
        {
            var results = new List<CulinaryRecipeEntry>();
            for (int i = 0; i < _allRecipes.Count; i++)
            {
                var r = _allRecipes[i];
                if (r.daily_morale_modifier >= minMoraleModifier)
                {
                    results.Add(r);
                }
            }
            return results;
        }

        public List<CulinaryRecipeEntry> GetByTag(string tag)
        {
            var results = new List<CulinaryRecipeEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allRecipes.Count; i++)
            {
                var r = _allRecipes[i];
                if (r.tags == null) continue;
                for (int j = 0; j < r.tags.Length; j++)
                {
                    if (string.Equals(r.tags[j], tag, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(r);
                        break;
                    }
                }
            }
            return results;
        }
    }
}
