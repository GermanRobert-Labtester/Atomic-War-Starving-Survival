using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

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

        // ─────────────────────────────────────────────────────────────
        // Save / load — CrafterId is persisted, Crafter is [NonSerialized]
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// CaptureState writes CrafterId, so RestoreState must rebind Crafter from it.
        /// Otherwise a craft saved mid-run completes with a null crafter and silently
        /// drops that survivor's completion-time yield perks (Pharmacologist high-yield,
        /// Alchemist double-yield), which both require crafter != null.
        /// </summary>
        [Test]
        public void RestoreState_RebindsCrafter_FromPersistedCrafterId()
        {
            var cloth = NewItem("cloth");
            var bandage = NewItem("bandage", ItemType.Medical);
            var recipe = NewRecipe("craft_bandage", bandage, 1, 4f, "");
            recipe.ingredients.Add(new Ingredient { item = cloth, amount = 1 });

            var crafter = new Survivor { Id = "sv_crafter", DisplayName = "Tinker" };

            var seededInv = new Inventory { Capacity = 20, MaxWeight = 1000f };
            seededInv.Add(cloth, 1);
            var seeded = new CraftingSystem(seededInv);
            Assert.That(seeded.StartCraft(recipe, crafter), Is.True);

            var save = seeded.CaptureState();
            Assert.That(save.ActiveCrafts[0].CrafterId, Is.EqualTo("sv_crafter"),
                "CrafterId must be captured for the rebind to be possible");

            var loaded = new CraftingSystem(new Inventory { Capacity = 20, MaxWeight = 1000f });
            loaded.SetRecipeLookup(id => id == "craft_bandage" ? recipe : null);
            loaded.SetSurvivorLookup(id => id == "sv_crafter" ? crafter : null);
            loaded.RestoreState(save);

            Assert.That(loaded.ActiveCraftCount, Is.EqualTo(1));
            Assert.That(loaded.ActiveCrafts[0].Crafter, Is.SameAs(crafter),
                "Crafter must be rebound from CrafterId on restore");
        }

        /// <summary>An unknown or absent crafter id must restore the craft, not drop it.</summary>
        [Test]
        public void RestoreState_KeepsCraft_WhenCrafterCannotBeResolved()
        {
            var cloth = NewItem("cloth");
            var bandage = NewItem("bandage", ItemType.Medical);
            var recipe = NewRecipe("craft_bandage", bandage, 1, 4f, "");
            recipe.ingredients.Add(new Ingredient { item = cloth, amount = 1 });

            var seededInv = new Inventory { Capacity = 20, MaxWeight = 1000f };
            seededInv.Add(cloth, 1);
            var seeded = new CraftingSystem(seededInv);
            // No crafter: an anonymous player-initiated craft.
            Assert.That(seeded.StartCraft(recipe), Is.True);

            var loaded = new CraftingSystem(new Inventory { Capacity = 20, MaxWeight = 1000f });
            loaded.SetRecipeLookup(id => id == "craft_bandage" ? recipe : null);
            // Deliberately no survivor lookup wired.
            loaded.RestoreState(seeded.CaptureState());

            Assert.That(loaded.ActiveCraftCount, Is.EqualTo(1),
                "A craft with no resolvable crafter must still be restored");
            Assert.That(loaded.ActiveCrafts[0].Crafter, Is.Null);
        }
    }
}
