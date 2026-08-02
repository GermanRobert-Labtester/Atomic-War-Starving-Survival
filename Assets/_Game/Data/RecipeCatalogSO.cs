using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Crafting;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// ScriptableObject catalog of crafting recipes; imported from
    /// StreamingAssets/Data/recipes.json.
    /// </summary>
    [CreateAssetMenu(fileName = "NewRecipeCatalog", menuName = "ASHFALL/Data/Recipe Catalog")]
    public class RecipeCatalogSO : ScriptableObject
    {
        public List<Recipe> recipes = new List<Recipe>();

        /// <summary>Look up a recipe by its snake_case id.</summary>
        public Recipe GetById(string id) => throw new System.NotImplementedException();
    }
}
