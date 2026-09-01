// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Crafting;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.PlayerCommand;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CraftingCommandTests
    {
        [Fact]
        public void PreviewCraft_UnknownRecipe_ReturnsUnavailable()
        {
            var inventory = new Inventory.Inventory();
            var system = new CraftingSystem(inventory);
            var preview = system.PreviewCraft(null!, stateVersion: 1L);

            Assert.False(preview.IsAvailable);
            Assert.Equal("missing_recipe", preview.FailureCode);
            Assert.Equal(PlayerCommandCode.CraftStart, preview.CommandCode);
            Assert.Equal(1L, preview.StateVersion);
        }

        [Fact]
        public void PreviewCraft_Available_ShowsProjectedDeltas()
        {
            var inventory = new Inventory.Inventory();
            var system = new CraftingSystem(inventory);
            system.SetDayProvider(() => 1);

            var recipe = new Recipe
            {
                id = "recipe_test",
                result = new ItemDefinition { id = "item_test", type = ItemType.Material, stackMax = 99, weight = 1f },
                resultAmount = 1,
                craftingTimeHours = 2f,
                ingredients = new List<Ingredient>
                {
                    new Ingredient { item = new ItemDefinition { id = "scrap_mechanical", type = ItemType.Material, stackMax = 99, weight = 1f }, amount = 2 }
                }
            };

            // Pre-seed ingredients so preview can validate availability
            inventory.Add(recipe.ingredients[0].item, recipe.ingredients[0].amount);

            var preview = system.PreviewCraft(recipe, stateVersion: 5L);

            Assert.True(preview.IsAvailable);
            Assert.Equal(2f, preview.EstimatedDurationHours);
            Assert.Equal(-2, preview.ProjectedDeltas["scrap_mechanical"]);
            Assert.Equal(1, preview.ProjectedDeltas["item_test"]);
            Assert.Equal(5L, preview.StateVersion);
        }

        [Fact]
        public void ExecuteCraft_Success_AdvancesStateVersion()
        {
            var inventory = new Inventory.Inventory();
            var system = new CraftingSystem(inventory);
            system.SetDayProvider(() => 1);

            var recipe = new Recipe
            {
                id = "recipe_test",
                result = new ItemDefinition { id = "item_test", type = ItemType.Material },
                resultAmount = 1,
                craftingTimeHours = 1f,
                ingredients = new List<Ingredient>
                {
                    new Ingredient { item = new ItemDefinition { id = "scrap_mechanical" }, amount = 1 }
                }
            };

            inventory.Add(new ItemDefinition { id = "scrap_mechanical", type = ItemType.Material, stackMax = 99, weight = 1f }, 1);

            var result = system.ExecuteCraft(recipe, expectedStateVersion: 10L, currentStateVersion: 10L);

            Assert.True(result.IsSuccess);
            Assert.Equal(10L, result.ExpectedStateVersion);
            Assert.Equal(11L, result.ActualStateVersion);
            Assert.Equal("craft.started", result.MessageKey);
            Assert.Equal(-1, result.Deltas["scrap_mechanical"]);
            Assert.Equal(1, result.Deltas["item_test"]);
            Assert.Equal(1, system.ActiveCraftCount);
        }

        [Fact]
        public void ExecuteCraft_StalePreview_RejectsWithoutMutation()
        {
            var inventory = new Inventory.Inventory();
            var system = new CraftingSystem(inventory);
            system.SetDayProvider(() => 1);

            var recipe = new Recipe
            {
                id = "recipe_test",
                result = new ItemDefinition { id = "item_test", type = ItemType.Material },
                resultAmount = 1,
                craftingTimeHours = 1f,
                ingredients = new List<Ingredient>
                {
                    new Ingredient { item = new ItemDefinition { id = "scrap_mechanical" }, amount = 1 }
                }
            };

            inventory.Add(new ItemDefinition { id = "scrap_mechanical", type = ItemType.Material, stackMax = 99, weight = 1f }, 1);

            var result = system.ExecuteCraft(recipe, expectedStateVersion: 99L, currentStateVersion: 100L);

            Assert.False(result.IsSuccess);
            Assert.Equal("stale_preview", result.FailureCode);
            Assert.Equal(0, system.ActiveCraftCount);
        }

        [Fact]
        public void ExecuteCraft_Failure_NoPartialMutation()
        {
            var inventory = new Inventory.Inventory();
            var system = new CraftingSystem(inventory);
            system.SetDayProvider(() => 1);

            var recipe = new Recipe
            {
                id = "recipe_test",
                result = new ItemDefinition { id = "item_test", type = ItemType.Material, stackMax = 1, weight = 1f },
                resultAmount = 1,
                craftingTimeHours = 1f,
                ingredients = new List<Ingredient>
                {
                    new Ingredient { item = new ItemDefinition { id = "scrap_mechanical" }, amount = 1 }
                }
            };

            var result = system.ExecuteCraft(recipe, expectedStateVersion: 1L, currentStateVersion: 1L);

            Assert.False(result.IsSuccess);
            Assert.Equal(0, system.ActiveCraftCount);
            Assert.Equal(0, inventory.CountById("scrap_mechanical"));
        }
    }

    public class ExpeditionCommandTests
    {
        [Fact]
        public void PreviewStart_InvalidParams_ReturnsUnavailable()
        {
            var system = new ExpeditionSystem();
            var preview = system.PreviewStart(null!, "survivor_a", 1, stateVersion: 1L);

            Assert.False(preview.IsAvailable);
            Assert.Equal("invalid_params", preview.FailureCode);
            Assert.Equal(PlayerCommandCode.ExpeditionDispatch, preview.CommandCode);
        }

        [Fact]
        public void PreviewStart_AlreadyActive_ReturnsUnavailable()
        {
            var system = new ExpeditionSystem();
            var def = new ExpeditionDefinition { id = "loc_test", displayName = "Test", distanceTicks = 5, dangerLevel = 1 };
            system.Start(def, "survivor_a", 1);
            var preview = system.PreviewStart(def, "survivor_a", 1, stateVersion: 1L);

            Assert.False(preview.IsAvailable);
            Assert.Equal("already_active", preview.FailureCode);
        }

        [Fact]
        public void PreviewStart_Available_ShowsProjectedDeltas()
        {
            var system = new ExpeditionSystem();
            var def = new ExpeditionDefinition { id = "loc_test", displayName = "Test", distanceTicks = 5, dangerLevel = 1, baseStaminaDrainPerHour = 2f };
            var preview = system.PreviewStart(def, "survivor_a", 1, stateVersion: 1L);

            Assert.True(preview.IsAvailable);
            Assert.Equal(PlayerCommandCode.ExpeditionDispatch, preview.CommandCode);
            Assert.True(preview.IsIrreversible);
            Assert.NotEmpty(preview.RiskCodes);
            Assert.Contains("encounter_risk", preview.RiskCodes);
            Assert.Contains("stamina_drain", preview.RiskCodes);
            Assert.True(preview.ProjectedDeltas.ContainsKey("travel_ticks"));
        }

        [Fact]
        public void ExecuteStart_Success_AdvancesStateVersion()
        {
            var system = new ExpeditionSystem();
            var def = new ExpeditionDefinition { id = "loc_test", displayName = "Test", distanceTicks = 5, dangerLevel = 1 };
            var result = system.ExecuteStart(def, "survivor_a", 1, expectedStateVersion: 10L, currentStateVersion: 10L);

            Assert.True(result.IsSuccess);
            Assert.Equal(10L, result.ExpectedStateVersion);
            Assert.Equal(11L, result.ActualStateVersion);
            Assert.Equal(1, system.ActiveCount);
        }

        [Fact]
        public void ExecuteStart_StalePreview_RejectsWithoutMutation()
        {
            var system = new ExpeditionSystem();
            var def = new ExpeditionDefinition { id = "loc_test", displayName = "Test", distanceTicks = 5, dangerLevel = 1 };
            var result = system.ExecuteStart(def, "survivor_a", 1, expectedStateVersion: 99L, currentStateVersion: 100L);

            Assert.False(result.IsSuccess);
            Assert.Equal("stale_preview", result.FailureCode);
            Assert.Equal(0, system.ActiveCount);
        }

        [Fact]
        public void PreviewPushLuck_WrongPhase_ReturnsUnavailable()
        {
            var system = new ExpeditionSystem();
            var def = new ExpeditionDefinition { id = "loc_test", displayName = "Test", distanceTicks = 5, dangerLevel = 1 };
            system.Start(def, "survivor_a", 1);
            var preview = system.PreviewPushLuck("survivor_a", stateVersion: 1L);

            Assert.False(preview.IsAvailable);
            Assert.Equal("wrong_phase", preview.FailureCode);
        }
    }
}
