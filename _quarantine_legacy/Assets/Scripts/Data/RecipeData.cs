using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar.Data
{
    [Serializable]
    public struct ItemIngredient
    {
        public ItemData Item;
        public int Amount;
    }

    [CreateAssetMenu(fileName = "NewRecipe", menuName = "AtomicWar/Data/RecipeData")]
    public class RecipeData : ScriptableObject
    {
        public string Id;
        public string RecipeName;
        public Sprite Icon;

        public List<ItemIngredient> Ingredients = new List<ItemIngredient>();
        public ItemData ResultItem;
        public int ResultAmount = 1;

        public float CraftingTimeInHours = 1f;
        public int RequiredWorkstationTier = 1;
    }
}
