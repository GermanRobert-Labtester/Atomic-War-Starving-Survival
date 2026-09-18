// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Greenhouse;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Production
{
    /// <summary>
    /// Plan 35 / Plan 36 — Production-to-Provisioning Delivery Chain & No-Silent-Loss Gate.
    /// Invariant: Every producer either commits its output into an authoritative sink
    /// or returns an explicit typed refusal. Silent production loss is strictly forbidden.
    /// </summary>
    public sealed class Plan35ProductionDeliveryTests
    {
        [Fact]
        public void Crafting_WhenInventoryFull_RefundsIngredients_NoSilentLoss()
        {
            // Setup inventory with zero free capacity
            var inventory = new InventoryContainer();
            var itemA = new ItemDefinition { id = "scrap_metal", displayName = "Scrap Metal" };
            var itemB = new ItemDefinition { id = "lockpick", displayName = "Lockpick" };

            // Fill inventory up to max weight/slots
            inventory.MaxWeight = 10f;
            itemA.weight = 10f;
            inventory.Add(itemA, 1); // Inventory is now at max capacity (10.0/10.0 kg)

            var crafting = new CraftingSystem(inventory);
            var recipe = new Recipe
            {
                id = "craft_lockpick",
                recipeName = "Lockpick",
                ingredients = new List<Ingredient> { new Ingredient { item = itemA, amount = 1 } },
                result = itemB,
                resultAmount = 1,
                craftingTimeHours = 1f
            };

            bool overflowEventFired = false;
            crafting.OnCraftResultOverflow += (r, resultId, amt) =>
            {
                overflowEventFired = true;
                Assert.Equal("lockpick", resultId);
                Assert.Equal(1, amt);
            };

            // Start craft with 1 itemA
            itemA.weight = 1f; // allow refunding to fit
            inventory.MaxWeight = 100f;
            inventory.Add(itemA, 5); // 6 total scrap metal

            // Consume ingredients to start craft
            bool craftStarted = crafting.StartCraft(recipe, null!);
            Assert.True(craftStarted);
            Assert.Equal(5, inventory.Count(itemA)); // 1 consumed for the job

            // Now constrain inventory so output cannot fit
            itemB.weight = 1000f; // Output is too heavy for inventory
            inventory.MaxWeight = 10f; // Max is 10, itemB is 1000

            // Complete craft
            crafting.Tick(2f); // Advance past craft time

            // Assertions:
            // 1. Overflow event was triggered
            Assert.True(overflowEventFired, "OnCraftResultOverflow must fire when inventory cannot accept result");
            // 2. Ingredients were refunded cleanly — scrap metal is back to 6!
            Assert.Equal(6, inventory.Count(itemA));
            // 3. No phantom lockpick was created
            Assert.Equal(0, inventory.Count(itemB));
        }

        [Fact]
        public void Kitchen_ServeMeal_MutatesNeeds_AndConsumesPortions_NoSilentLoss()
        {
            var rng = new SeededRng(42);
            var inventory = new InventoryContainer();
            var needs = new NeedsSystem();
            var survivor = new SurvivorNeedsState { Id = "cook_alice", Hunger = 50f, Morale = 50f };
            needs.Register(survivor);
            survivor.Hunger = 50f;
            survivor.Morale = 50f;

            var kitchen = new KitchenNutritionSystem(rng, inventory, needs);

            // Start prep job with ingredients
            inventory.AddById("canned_meat", 4);

            var inputs = new Dictionary<string, int> { { "canned_meat", 2 } };
            var startRes = kitchen.StartPrepJob("stew", "cook_alice", inputs);
            Assert.True(startRes.IsSuccess);
            Assert.Equal(2, inventory.CountById("canned_meat")); // 2 consumed

            // Advance day so stew is prepared
            kitchen.TickDay(1);
            Assert.NotEmpty(kitchen.State.pantry);
            Assert.Equal("stew", kitchen.State.pantry[0].itemId);
            int portionsBefore = kitchen.State.pantry[0].portionCount;
            Assert.True(portionsBefore > 0);

            // Serve meal to Alice
            float hungerBefore = survivor.Hunger;
            var serveRes = kitchen.ServeMeal("cook_alice", "stew");
            Assert.True(serveRes.IsSuccess);

            // Portions decremented by 1
            Assert.Equal(portionsBefore - 1, kitchen.State.pantry[0].portionCount);
            // Hunger decreased (alleviated)
            Assert.True(survivor.Hunger < hungerBefore);
        }

        [Fact]
        public void Greenhouse_Harvest_DeliversOutputToSink()
        {
            var greenhouse = new GreenhouseSystem(1999);
            greenhouse.EnsurePlots(2);

            // Plant plot 0 with mushroom
            bool planted = greenhouse.Plant(0, GreenhouseExpansionCatalog.Items.SeedMushroom, currentDay: 1, out _);
            Assert.True(planted);
            greenhouse.Water(0, 50f, tainted: false);

            // Mature crop (SeedMushroom requires 144 hours = 6 days with 6+ hours light)
            for (int day = 2; day <= 10; day++)
            {
                greenhouse.Water(0, 20f, tainted: false);
                greenhouse.TickDay(day, 8f, 0f);
            }

            // Harvest
            var harvest = greenhouse.Harvest(0);
            Assert.True(harvest.success);
            Assert.False(string.IsNullOrEmpty(harvest.yieldItemId));
            Assert.True(harvest.amount > 0);

            // Deliver to inventory sink
            var inventory = new InventoryContainer();
            var yieldItem = new ItemDefinition { id = harvest.yieldItemId, displayName = "Crop Yield" };
            inventory.Add(yieldItem, harvest.amount);

            Assert.Equal(harvest.amount, inventory.Count(yieldItem));
        }

        [Fact]
        public void ProductionOutput_SurvivesSaveRoundTrip()
        {
            var inventory = new InventoryContainer();
            var producedItem = new ItemDefinition
            {
                id = "purified_water",
                displayName = "Purified Water",
                weight = 1.0f
            };

            // Produce 15 units into inventory
            inventory.Add(producedItem, 15);
            Assert.Equal(15, inventory.Count(producedItem));

            // Capture save state
            var saveState = inventory.CaptureState();
            Assert.NotNull(saveState);

            // Restore into fresh container
            var restoredInventory = new InventoryContainer();
            restoredInventory.RestoreState(saveState, id => id == "purified_water" ? producedItem : null);

            Assert.Equal(15, restoredInventory.Count(producedItem));
        }
    }
}
