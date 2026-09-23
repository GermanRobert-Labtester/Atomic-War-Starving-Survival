// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Cooking;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Cooking
{
    public sealed class Plan136WildlifeCookingIntegrationTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent!;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        [Fact]
        public void CookingCatalog_LoadsAuthoredRecipes_Cleanly()
        {
            string dataDir = GetDataDir();
            var system = CookingSystem.LoadFromDirectory(dataDir, new FileSystemIO());

            Assert.NotNull(system.Recipes);
            Assert.True(system.Recipes.Count >= 15, $"Expected at least 15 recipes, found {system.Recipes.Count}");

            Assert.True(system.TryGetRecipe("recipe_roasted_meat", out var roast));
            Assert.NotNull(roast);
            Assert.Equal("cooked_meat", roast!.outputItemId);
            Assert.True(roast.radiationRemoval >= 0.5f);

            Assert.True(system.TryGetRecipe("recipe_boiled_meat_broth", out var boiled));
            Assert.NotNull(boiled);
            Assert.True(boiled!.radiationRemoval >= 0.8f);

            Assert.True(system.TryGetRecipe("recipe_canned_grain_stew", out var canned));
            Assert.NotNull(canned);
            Assert.Equal("industrial_cooker", canned!.requiredEquipment);
        }

        [Fact]
        public void WildlifeTrapping_TransferCatchToInventory_DepositsMeatAndHides()
        {
            var trappingSystem = new WildlifeTrappingSystem(new SeededRng(42));
            var inventory = new Ashfall.Core.Inventory.Inventory();

            // Set a trap site
            trappingSystem.SetTrap("site_alpha", "bait_scraps", "hunter_marcus", "snare");

            // Mock a catch on site_alpha
            var site = trappingSystem.State.trapSites.First(s => s.siteId == "site_alpha");
            site.hasCatch = true;
            site.catchSpecies = "hare";
            site.carcassYield = 2.0f;
            site.isToxic = true;
            site.contaminationDose = 15f;

            // Transfer catch to inventory
            var res = trappingSystem.TransferCatchToInventory("site_alpha", inventory, "raw_meat");

            Assert.True(res.IsSuccess);
            Assert.True(inventory.CountById("raw_meat") >= 2);
            Assert.True(site.isMeatProcessed);
        }

        [Fact]
        public void CookingLifecycle_ConsumesIngredients_DeliversCookedFood_AndIncreasesSkill()
        {
            string dataDir = GetDataDir();
            var system = CookingSystem.LoadFromDirectory(dataDir, new FileSystemIO(), new SeededRng(42));
            var inventory = new Ashfall.Core.Inventory.Inventory();
            var source = new InventoryCookingSource(inventory);

            // Stock inventory with raw ingredients
            inventory.AddById("raw_meat", 5);

            // Start cooking roasted meat
            var startRes = system.StartCooking("recipe_roasted_meat", "cook_elena", "improvised_stove", source, currentMinute: 0f);
            Assert.True(startRes.IsSuccess);
            Assert.Equal(4, inventory.CountById("raw_meat")); // 1 consumed
            Assert.Single(system.State.activeOperations);

            // Progress cooking to completion
            float requiredMinutes = system.State.activeOperations[0].totalMinutesRequired;
            int completed = system.ProgressCooking(requiredMinutes + 1f, source);

            Assert.Equal(1, completed);
            Assert.Empty(system.State.activeOperations);
            Assert.Single(system.State.completedOperations);

            // Verify food was delivered into inventory
            Assert.True(inventory.CountById("cooked_meat") >= 1);

            // Verify skill progressed
            Assert.True(system.State.cookingSkillLevel > 0f);
            Assert.True(system.State.totalFoodDecontaminated > 0f);
        }

        [Fact]
        public void RadiationDecontamination_RemovesRadiation_BasedOnRecipe()
        {
            string dataDir = GetDataDir();
            var system = CookingSystem.LoadFromDirectory(dataDir, new FileSystemIO(), new SeededRng(100));
            var inventory = new Ashfall.Core.Inventory.Inventory();
            var source = new InventoryCookingSource(inventory);

            // Stock ingredients for broth
            inventory.AddById("raw_meat", 2);
            inventory.AddById("clean_water", 2);

            var startRes = system.StartCooking("recipe_boiled_meat_broth", "cook_master", "basic_boiler", source, currentMinute: 0f);
            Assert.True(startRes.IsSuccess);

            float totalMins = system.State.activeOperations[0].totalMinutesRequired;
            system.ProgressCooking(totalMins + 1f, source);

            var completedOp = system.State.completedOperations.First();
            // Boiling has 85% radiation removal, so remaining fraction should be <= 0.20
            Assert.True(completedOp.radiationRemainingFraction <= 0.25f,
                $"Expected <= 0.25 remaining radiation fraction, but got {completedOp.radiationRemainingFraction}");
        }

        [Fact]
        public void CookingSystem_CaptureRestore_PreservesAllMetricsAndOperations()
        {
            var system = new CookingSystem();
            system.RegisterRecipe(new CookingRecipe
            {
                id = "recipe_test",
                displayName = "Test Soup",
                outputItemId = "test_soup",
                outputQuantity = 2,
                cookTimeMinutes = 15f
            });

            system.StartCooking("recipe_test", "cook_test", "improvised_stove", null, 0f);
            Assert.Single(system.State.activeOperations);

            var state = system.CaptureState();
            Assert.Equal(1, state.schema_version);
            Assert.Single(state.activeOperations);

            var restored = new CookingSystem();
            restored.RegisterRecipe(new CookingRecipe
            {
                id = "recipe_test",
                displayName = "Test Soup",
                outputItemId = "test_soup",
                outputQuantity = 2,
                cookTimeMinutes = 15f
            });
            restored.RestoreState(state);

            Assert.Single(restored.State.activeOperations);
            Assert.Equal("recipe_test", restored.State.activeOperations[0].recipeId);
        }
    }
}
