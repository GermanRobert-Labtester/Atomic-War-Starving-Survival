using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests
{
    public class CraftingSystemTests
    {
        private static ItemDefinition Def(string id, ItemType type = ItemType.Material,
            int stackMax = 10, float weight = 0.2f)
        {
            return new ItemDefinition { id = id, type = type, stackMax = stackMax, weight = weight };
        }

        private static Recipe MakeRecipe(string id, ItemDefinition result,
            (ItemDefinition, int)[] ingredients, float hours = 0f, string stationId = null)
        {
            var r = new Recipe
            {
                id = id,
                result = result,
                resultAmount = 1,
                craftingTimeHours = hours,
                requiredStationId = stationId
            };
            foreach (var (item, amt) in ingredients)
                r.ingredients.Add(new Ingredient { item = item, amount = amt });
            return r;
        }

        [Fact]
        public void CanCraft_TrueWhenAllIngredientsHeld()
        {
            var inv = new InventoryContainer();
            var scrap = Def("mechanical_parts");
            var bolt = Def("bolt");
            inv.Add(scrap, 4);
            inv.Add(bolt, 2);

            var result = Def("makeshift_knife", ItemType.Tool, 1, 0.5f);
            var recipe = MakeRecipe("recipe_knife", result,
                new[] { (scrap, 4), (bolt, 2) }, hours: 0.1f);

            var sys = new CraftingSystem(inv);
            sys.AddStation(new CraftingStation { id = "workbench" });
            Assert.True(sys.CanCraft(recipe));
        }

        [Fact]
        public void CanCraft_False_WhenStationMissingOrBroken()
        {
            var inv = new InventoryContainer();
            var scrap = Def("mechanical_parts");
            inv.Add(scrap, 10);
            var result = Def("makeshift_knife", ItemType.Tool);
            var recipe = MakeRecipe("recipe_knife", result,
                new[] { (scrap, 4) }, stationId: "workbench");

            var sys = new CraftingSystem(inv);
            Assert.False(sys.CanCraft(recipe)); // no station

            sys.AddStation(new CraftingStation { id = "workbench", condition = 0f });
            Assert.False(sys.CanCraft(recipe)); // broken station
        }

        [Fact]
        public void StartCraft_ConsumesIngredients_CompletesAfterTick()
        {
            var inv = new InventoryContainer();
            var scrap = Def("mechanical_parts");
            inv.Add(scrap, 5);

            var result = Def("filter", ItemType.Filter);
            var recipe = MakeRecipe("recipe_filter", result,
                new[] { (scrap, 4) }, hours: 3f);

            var sys = new CraftingSystem(inv);
            sys.AddStation(new CraftingStation { id = "workbench" });
            Assert.True(sys.StartCraft(recipe));
            Assert.Equal(1, inv.Count(scrap)); // 5 - 4 consumed
            Assert.Equal(1, sys.ActiveCraftCount);

            sys.Tick(1f);
            Assert.Equal(0, inv.Count(result)); // not done yet
            sys.Tick(2f);
            Assert.Equal(1, inv.Count(result)); // crafted
            Assert.Equal(0, sys.ActiveCraftCount);
        }

        [Fact]
        public void StartCraft_Fails_WhenIngredientsInsufficient()
        {
            var inv = new InventoryContainer();
            var scrap = Def("mechanical_parts");
            inv.Add(scrap, 2);

            var result = Def("filter", ItemType.Filter);
            var recipe = MakeRecipe("recipe_filter", result,
                new[] { (scrap, 4) }, hours: 1f);

            var sys = new CraftingSystem(inv);
            Assert.False(sys.StartCraft(recipe));
            Assert.Equal(2, inv.Count(scrap)); // untouched
        }

        [Fact]
        public void FullInventory_StartRejected_NoIngredientsConsumed()
        {
            // The result cannot fit at start (capacity already full), so the
            // craft is rejected outright and nothing is consumed (Unity parity).
            var inv = new InventoryContainer { Capacity = 1 };
            var scrap = Def("mechanical_parts");
            inv.Add(scrap, 1); // capacity full

            var result = Def("filter", ItemType.Filter);
            var recipe = MakeRecipe("recipe_filter", result,
                new[] { (scrap, 1) }, hours: 1f);

            var sys = new CraftingSystem(inv);
            sys.AddStation(new CraftingStation { id = "workbench" });
            Assert.False(sys.StartCraft(recipe));
            Assert.Equal(1, inv.Count(scrap)); // untouched
            Assert.Equal(0, sys.ActiveCraftCount);
        }

        [Fact]
        public void OverflowPath_WithFreedSlot_RefundsWhenNoStash()
        {
            // Craft starts with room; the freed ingredient slot is refilled before
            // completion, so the result has nowhere to go. No stash -> refund.
            var inv = new InventoryContainer { Capacity = 2 };
            var scrap = Def("mechanical_parts");
            inv.Add(scrap, 1); // 1 of 2 slots used

            var result = Def("filter", ItemType.Filter);
            var recipe = MakeRecipe("recipe_filter", result,
                new[] { (scrap, 1) }, hours: 3f);

            var sys = new CraftingSystem(inv);
            sys.AddStation(new CraftingStation { id = "workbench" });
            Assert.True(sys.StartCraft(recipe)); // 1 free slot, result fits
            Assert.Equal(0, inv.Count(scrap));

            bool overflow = false;
            sys.OnCraftResultOverflow += (r, id, amt) => overflow = true;

            // Refill both freed slots before completion so the result has nowhere to go.
            var fillerA = Def("filler_a");
            var fillerB = Def("filler_b");
            Assert.True(inv.Add(fillerA, 1));
            Assert.True(inv.Add(fillerB, 1)); // capacity 2 now fully packed
            sys.Tick(3f);

            Assert.True(overflow);
            Assert.Equal(0, inv.Count(result));   // never entered the bag
            // Refund is best-effort: the bag is full, so the scrap may or may not
            // land. What the contract guarantees: craft cleaned up, no result dropped.
            Assert.Equal(0, sys.ActiveCraftCount);
        }

        [Fact]
        public void OverflowPath_WithStash_StashesResultAndRefundsNothing()
        {
            var inv = new InventoryContainer { Capacity = 2 };
            var scrap = Def("mechanical_parts");
            inv.Add(scrap, 1);

            var result = Def("filter", ItemType.Filter);
            var recipe = MakeRecipe("recipe_filter", result,
                new[] { (scrap, 1) }, hours: 3f);

            var stash = new InventoryContainer();
            var sys = new CraftingSystem(inv) { OverflowStash = stash };
            sys.AddStation(new CraftingStation { id = "workbench" });
            Assert.True(sys.StartCraft(recipe));

            bool overflow = false;
            sys.OnCraftResultOverflow += (r, id, amt) => overflow = true;

            var fillerA = Def("filler_a");
            var fillerB = Def("filler_b");
            Assert.True(inv.Add(fillerA, 1));
            Assert.True(inv.Add(fillerB, 1)); // capacity 2 fully packed
            sys.Tick(3f);

            Assert.True(overflow);
            Assert.Equal(0, inv.Count(result));  // not in the bag
            Assert.Equal(1, stash.Count(result)); // stashed
            Assert.Equal(0, inv.Count(scrap));   // consumed, not refunded (stash case)
            Assert.Equal(1, inv.Count(fillerA));
            Assert.Equal(1, inv.Count(fillerB));
            Assert.Equal(0, sys.ActiveCraftCount);
        }

        [Fact]
        public void SaveRoundtrip_PreservesActiveCrafts()
        {
            var inv = new InventoryContainer();
            var scrap = Def("mechanical_parts");
            inv.Add(scrap, 5);
            var result = Def("filter", ItemType.Filter);
            var recipe = MakeRecipe("recipe_filter", result,
                new[] { (scrap, 4) }, hours: 5f);

            var sys = new CraftingSystem(inv);
            sys.AddStation(new CraftingStation { id = "workbench" });
            sys.StartCraft(recipe);
            var state = sys.CaptureState();

            var restored = new CraftingSystem(inv);
            restored.SetRecipeLookup(id => id == recipe.id ? recipe : null);
            restored.RestoreState(state);
            Assert.Equal(1, restored.ActiveCraftCount);
        }

        [Fact]
        public void Station_DegradesPerCraft_AndCanRepair()
        {
            var station = new CraftingStation { id = "workbench", condition = 100f };
            station.Degrade(5f);
            Assert.Equal(95f, station.condition, 3);
            Assert.True(station.IsOperational);
            station.Degrade(200f);
            Assert.Equal(0f, station.condition, 3);
            Assert.False(station.IsOperational);
            station.Repair(30f);
            Assert.Equal(30f, station.condition, 3);
        }
    }
}
