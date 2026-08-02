using AtomicWar.Core.Services;
using AtomicWar.Data;
using AtomicWar.Runtime.Crafting;
using AtomicWar.Runtime.Inventory;
using AtomicWar.Runtime.Scavenging;
using AtomicWar.Runtime.Survivors;
using UnityEngine;

namespace AtomicWar.Core
{
    /// <summary>
    /// Bootstraps initial gameplay data for the Shelter Scene prototype:
    /// - 2 Survivors (Mark & Katya)
    /// - Initial inventory items (Food & Cloth)
    /// - 1 Scavenging Location (Abandoned Pharmacy)
    /// - 1 Crafting Recipe (Bandage)
    /// </summary>
    public class GameBootstrap : MonoBehaviour
    {
        [Header("Runtime References")]
        public ItemData FoodItem;
        public ItemData ClothItem;
        public ItemData BandageItem;
        public RecipeData BandageRecipe;
        public ScavengeLocationData PharmacyLocation;

        private void Start()
        {
            InitializePrototypeData();
        }

        private void InitializePrototypeData()
        {
            // 1. Create fallback ScriptableObjects if not assigned in Inspector
            if (FoodItem == null)
            {
                FoodItem = ScriptableObject.CreateInstance<ItemData>();
                FoodItem.Id = "item_food";
                FoodItem.DisplayName = "Canned Food";
                FoodItem.Description = "Nourishing canned food. Restores 40 Hunger.";
                FoodItem.Category = ItemCategory.Food;
                FoodItem.HungerRestored = 40f;
                FoodItem.Weight = 0.5f;
            }

            if (ClothItem == null)
            {
                ClothItem = ScriptableObject.CreateInstance<ItemData>();
                ClothItem.Id = "item_cloth";
                ClothItem.DisplayName = "Rags & Cloth";
                ClothItem.Description = "Basic crafting component for medical items and filters.";
                ClothItem.Category = ItemCategory.Material;
                ClothItem.Weight = 0.2f;
            }

            if (BandageItem == null)
            {
                BandageItem = ScriptableObject.CreateInstance<ItemData>();
                BandageItem.Id = "item_bandage";
                BandageItem.DisplayName = "Sterile Bandage";
                BandageItem.Description = "Treats wounds and restores 30 Health.";
                BandageItem.Category = ItemCategory.Medical;
                BandageItem.HealthRestored = 30f;
                BandageItem.Weight = 0.1f;
            }

            if (BandageRecipe == null)
            {
                BandageRecipe = ScriptableObject.CreateInstance<RecipeData>();
                BandageRecipe.Id = "recipe_bandage";
                BandageRecipe.RecipeName = "Craft Bandage";
                BandageRecipe.Ingredients.Add(new ItemIngredient { Item = ClothItem, Amount = 2 });
                BandageRecipe.ResultItem = BandageItem;
                BandageRecipe.ResultAmount = 1;
                BandageRecipe.CraftingTimeInHours = 1f;
            }

            if (PharmacyLocation == null)
            {
                PharmacyLocation = ScriptableObject.CreateInstance<ScavengeLocationData>();
                PharmacyLocation.Id = "loc_pharmacy";
                PharmacyLocation.LocationName = "Abandoned Pharmacy";
                PharmacyLocation.Description = "A looted pharmacy containing medical rags and rare food tins.";
                PharmacyLocation.Danger = DangerLevel.Cautious;
                PharmacyLocation.PossibleLootPool.Add(new ItemIngredient { Item = FoodItem, Amount = 2 });
                PharmacyLocation.PossibleLootPool.Add(new ItemIngredient { Item = ClothItem, Amount = 4 });
                PharmacyLocation.PossibleLootPool.Add(new ItemIngredient { Item = BandageItem, Amount = 1 });
            }

            // 2. Register starting survivors
            var survivorSystem = ServiceLocator.Get<SurvivorSystem>();
            if (survivorSystem != null && survivorSystem.ActiveSurvivors.Count == 0)
            {
                var markData = ScriptableObject.CreateInstance<SurvivorData>();
                markData.Id = "survivor_mark";
                markData.CharacterName = "Mark (Handyman)";
                markData.Profession = "Handyman";
                markData.MaxHealth = 100f;
                markData.BaseHungerDecayRate = 2.0f;
                markData.BaseFatigueDecayRate = 1.5f;
                markData.CombatEfficiency = 1.2f;

                var katyaData = ScriptableObject.CreateInstance<SurvivorData>();
                katyaData.Id = "survivor_katya";
                katyaData.CharacterName = "Katya (Nurse)";
                katyaData.Profession = "Nurse";
                katyaData.MaxHealth = 100f;
                katyaData.BaseHungerDecayRate = 1.8f;
                katyaData.BaseFatigueDecayRate = 1.2f;
                katyaData.CraftingSpeedMultiplier = 1.3f;

                survivorSystem.AddSurvivor(markData);
                survivorSystem.AddSurvivor(katyaData);
            }

            // 3. Add initial inventory stock
            var inventorySystem = ServiceLocator.Get<InventorySystem>();
            if (inventorySystem != null && inventorySystem.Slots.Count == 0)
            {
                inventorySystem.AddItem(FoodItem, 3);
                inventorySystem.AddItem(ClothItem, 4);
            }

            Debug.Log("[GameBootstrap] Initialized 2 survivors (Mark & Katya), initial inventory stock (3x Food, 4x Cloth), and Abandoned Pharmacy location.");
        }
    }
}
