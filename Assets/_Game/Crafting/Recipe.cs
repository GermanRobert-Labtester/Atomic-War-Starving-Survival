using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Crafting
{
    /// <summary>
    /// ScriptableObject crafting recipe: inputs, output, craft time, and the
    /// station required. Authored as a .asset or imported from
    /// StreamingAssets/Data/recipes.json.
    /// </summary>
    [CreateAssetMenu(fileName = "NewRecipe", menuName = "ASHFALL/Recipe")]
    public class Recipe : ScriptableObject
    {
        public string id;
        public string recipeName;
        public List<Ingredient> ingredients = new List<Ingredient>();
        public ItemDefinition result;
        public int resultAmount = 1;
        public float craftingTimeHours = 1f;
        public string requiredStationId;
    }

    /// <summary>A quantity of an item required (or produced) by a recipe.</summary>
    [System.Serializable]
    public class Ingredient
    {
        public ItemDefinition item;
        public int amount = 1;
    }
}
