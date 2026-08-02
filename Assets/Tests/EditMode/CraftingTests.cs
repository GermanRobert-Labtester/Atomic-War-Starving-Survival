using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// EditMode tests for the crafting pipeline: recipe validation against the
    /// inventory, ingredient consumption + output production over the craft timer,
    /// station gating + wear, pause-awareness, and the recipe catalog lookup.
    /// </summary>
    [TestFixture]
    public class CraftingTests
    {
        private const float Eps = 1e-4f;

        private static ItemDefinition NewItem(string id, ItemType type = ItemType.Material, int stackMax = 20, float weight = 0.5f)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = type;
            item.stackMax = stackMax;
            item.weight = weight;
            return item;
        }

        private static Recipe NewRecipe(string id, ItemDefinition result, int resultAmount, float timeHours, string stationId)
        {
            var recipe = ScriptableObject.CreateInstance<Recipe>();
            recipe.id = id;
            recipe.recipeName = id;
            recipe.result = result;
            recipe.resultAmount = resultAmount;
            recipe.craftingTimeHours = timeHours;
            recipe.requiredStationId = stationId;
            return recipe;
        }

        [Test]
        public void CanCraft_FalseWhenIngredientsMissing_TrueWhenPresent()
        {
            var cloth = NewItem("cloth");
            var bandage = NewItem("bandage", ItemType.Medical);
            var recipe = NewRecipe("craft_bandage", bandage, 1, 0.5f, "");
            recipe.ingredients.Add(new Ingredient { item = cloth, amount = 2 });

            var inventory = new Inventory { Capacity = 20, MaxWeight = 1000f };
            var crafting = new CraftingSystem(inventory);

            Assert.That(crafting.CanCraft(recipe), Is.False); // no cloth yet

            inventory.Add(cloth, 2);
            Assert.That(crafting.CanCraft(recipe), Is.True);

            Assert.That(crafting.CanCraft(null), Is.False);
        }

        [Test]
        public void StartCraft_ConsumesIngredients_AndYieldsOutputAfterTime()
        {
            var cloth = NewItem("cloth");
            var bandage = NewItem("bandage", ItemType.Medical);
            var recipe = NewRecipe("craft_bandage", bandage, 1, 0.5f, "");
            recipe.ingredients.Add(new Ingredient { item = cloth, amount = 2 });

            var inventory = new Inventory { Capacity = 20, MaxWeight = 1000f };
            inventory.Add(cloth, 2);
            var crafting = new CraftingSystem(inventory);

            Assert.That(crafting.StartCraft(recipe), Is.True);
            Assert.That(inventory.Count(cloth), Is.EqualTo(0));    // ingredients consumed up front
            Assert.That(inventory.Count(bandage), Is.EqualTo(0));  // result not produced yet
            Assert.That(crafting.ActiveCraftCount, Is.EqualTo(1));

            crafting.Tick(0.5f);
            Assert.That(inventory.Count(bandage), Is.EqualTo(1));  // result produced on completion
            Assert.That(crafting.ActiveCraftCount, Is.EqualTo(0));
        }

        [Test]
        public void StartCraft_FalseWhenIngredientsMissing()
        {
            var cloth = NewItem("cloth");
            var bandage = NewItem("bandage", ItemType.Medical);
            var recipe = NewRecipe("craft_bandage", bandage, 1, 0.5f, "");
            recipe.ingredients.Add(new Ingredient { item = cloth, amount = 2 });

            var inventory = new Inventory { Capacity = 20, MaxWeight = 1000f };
            var crafting = new CraftingSystem(inventory);

            Assert.That(crafting.StartCraft(recipe), Is.False);
            Assert.That(crafting.ActiveCraftCount, Is.EqualTo(0));
        }

        [Test]
        public void StationGating_RequiresAnOperationalStation()
        {
            var cloth = NewItem("cloth");
            var bandage = NewItem("bandage", ItemType.Medical);
            var recipe = NewRecipe("craft_bandage", bandage, 1, 0.5f, "workbench");
            recipe.ingredients.Add(new Ingredient { item = cloth, amount = 1 });

            var inventory = new Inventory { Capacity = 20, MaxWeight = 1000f };
            inventory.Add(cloth, 1);
            var crafting = new CraftingSystem(inventory);

            Assert.That(crafting.CanCraft(recipe), Is.False); // no station registered

            var station = new CraftingStation { id = "workbench", Condition = 100f };
            crafting.AddStation(station);
            Assert.That(crafting.CanCraft(recipe), Is.True);  // operational station present

            station.Condition = 0f;                            // station broken
            Assert.That(crafting.CanCraft(recipe), Is.False);
        }

        [Test]
        public void Tick_DegradesStationWhenCraftCompletes()
        {
            var cloth = NewItem("cloth");
            var bandage = NewItem("bandage", ItemType.Medical);
            var recipe = NewRecipe("craft_bandage", bandage, 1, 0.5f, "workbench");
            recipe.ingredients.Add(new Ingredient { item = cloth, amount = 1 });

            var inventory = new Inventory { Capacity = 20, MaxWeight = 1000f };
            inventory.Add(cloth, 1);
            var crafting = new CraftingSystem(inventory);
            var station = new CraftingStation { id = "workbench", Condition = 100f };
            crafting.AddStation(station);

            crafting.StartCraft(recipe);
            crafting.Tick(0.5f);

            Assert.That(station.Condition, Is.EqualTo(100f - CraftingSystem.StationWearPerCraft).Within(Eps));
        }

        [Test]
        public void Tick_WhilePaused_DoesNotAdvanceCrafts()
        {
            var cloth = NewItem("cloth");
            var bandage = NewItem("bandage", ItemType.Medical);
            var recipe = NewRecipe("craft_bandage", bandage, 1, 0.5f, "");
            recipe.ingredients.Add(new Ingredient { item = cloth, amount = 1 });

            var inventory = new Inventory { Capacity = 20, MaxWeight = 1000f };
            inventory.Add(cloth, 1);
            var crafting = new CraftingSystem(inventory) { IsPaused = true };

            crafting.StartCraft(recipe);
            crafting.Tick(1f);

            Assert.That(inventory.Count(bandage), Is.EqualTo(0)); // paused: nothing produced
            Assert.That(crafting.ActiveCraftCount, Is.EqualTo(1));
        }

        [Test]
        public void RecipeCatalog_GetById_ReturnsMatchOrNull()
        {
            var bandage = NewItem("bandage", ItemType.Medical);
            var recipe = NewRecipe("craft_bandage", bandage, 1, 0.5f, "");
            var catalog = ScriptableObject.CreateInstance<RecipeCatalogSO>();
            catalog.recipes.Add(recipe);

            Assert.That(catalog.GetById("craft_bandage"), Is.SameAs(recipe));
            Assert.That(catalog.GetById("does_not_exist"), Is.Null);
            Assert.That(catalog.GetById(null), Is.Null);
        }
    }
}
