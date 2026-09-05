// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests
{
    public class CraftAttributionTradeSpecialtyTests
    {
        private const string CrafterElena = "survivor_elena_vasquez";
        private const string CrafterMikhail = "survivor_mikhail";

        [Fact]
        public void CraftContext_DirectConsumption_AdvancesTradeSpecialtyForMatchingCrafter()
        {
            var specialty = new TradeSpecialtySystem();
            int milestoneCount = 0;
            specialty.OnSpecialtyMilestone += (sv, prof, tier) =>
            {
                if (sv == CrafterElena) milestoneCount++;
            };

            var context1 = new CraftContext
            {
                StationId = "workbench",
                CrafterSurvivorId = CrafterElena,
                ProfessionId = "machinist",
                RecipeId = "recipe_wrench",
                ResultItemId = "wrench_standard",
                ResultAmount = 1,
                CompletedDay = 5
            };

            specialty.OnCraftCompleted(context1);

            Assert.Equal(1, specialty.GetMasteryTier(CrafterElena));
            Assert.Equal(1, milestoneCount);
            Assert.False(specialty.HasMasteredTrade(CrafterElena));

            var context2 = new CraftContext
            {
                StationId = "workbench",
                CrafterSurvivorId = CrafterElena,
                ProfessionId = "machinist",
                RecipeId = "recipe_gear",
                ResultItemId = "gear_standard",
                ResultAmount = 1,
                CompletedDay = 6
            };

            specialty.OnCraftCompleted(context2);

            Assert.Equal(2, specialty.GetMasteryTier(CrafterElena));
            Assert.Equal(2, milestoneCount);
        }

        [Fact]
        public void CraftContext_WrongItemOrProfession_DoesNotAdvanceMilestone()
        {
            var specialty = new TradeSpecialtySystem();

            // Wrong item category for machinist (bandage belongs to nurse)
            var mismatchContext = new CraftContext
            {
                StationId = "workbench",
                CrafterSurvivorId = CrafterElena,
                ProfessionId = "machinist",
                RecipeId = "recipe_bandage",
                ResultItemId = "bandage_clean",
                ResultAmount = 1,
                CompletedDay = 1
            };

            specialty.OnCraftCompleted(mismatchContext);
            Assert.Equal(0, specialty.GetMasteryTier(CrafterElena));

            // Unknown profession
            var unknownProfContext = new CraftContext
            {
                StationId = "workbench",
                CrafterSurvivorId = CrafterMikhail,
                ProfessionId = "unknown_drifter",
                RecipeId = "recipe_wrench",
                ResultItemId = "wrench_standard",
                ResultAmount = 1,
                CompletedDay = 1
            };

            specialty.OnCraftCompleted(unknownProfContext);
            Assert.Equal(0, specialty.GetMasteryTier(CrafterMikhail));
        }

        [Fact]
        public void CraftContext_UnassignedCrafter_ExplicitNoOpPolicy()
        {
            var specialty = new TradeSpecialtySystem();
            bool eventFired = false;
            specialty.OnSpecialtyMilestone += (_, _, _) => eventFired = true;

            var unassigned = new CraftContext
            {
                StationId = "automated_distillery",
                CrafterSurvivorId = string.Empty, // No assigned dweller
                ProfessionId = string.Empty,
                RecipeId = "recipe_filter",
                ResultItemId = "water_filter",
                ResultAmount = 1,
                CompletedDay = 2
            };

            Assert.False(unassigned.HasAssignedCrafter);

            // Must not throw or advance any progression
            specialty.OnCraftCompleted(unassigned);
            Assert.False(eventFired);

            // Null context safety
            specialty.OnCraftCompleted(null);
            Assert.False(eventFired);
        }

        [Fact]
        public void CraftingSystem_OnCraftContextCompleted_EmitsAttributionToTradeSpecialty()
        {
            var inv = new InventoryContainer();
            var scrapItem = new ItemDefinition { id = "scrap_metal", stackMax = 50 };
            var wrenchItem = new ItemDefinition { id = "wrench_standard", stackMax = 10 };
            inv.Add(scrapItem, 10);

            var station = new CraftingStation { id = "workbench", condition = 100f };
            var crafting = new CraftingSystem(inv);
            crafting.AddStation(station);

            var recipe = new Recipe
            {
                id = "recipe_wrench",
                recipeName = "Standard Wrench",
                requiredStationId = "workbench",
                craftingTimeHours = 2f,
                result = wrenchItem,
                resultAmount = 1,
                ingredients = new List<Ingredient>
                {
                    new Ingredient { item = scrapItem, amount = 2 }
                }
            };

            var specialty = new TradeSpecialtySystem();
            specialty.BindToCrafting(crafting);

            crafting.SetCrafterProfessionLookup(id => id == CrafterElena ? "machinist" : null);

            bool started = crafting.StartCraft(recipe, crafterId: CrafterElena);
            Assert.True(started);

            // Fast forward time to complete craft
            crafting.Tick(3f);

            Assert.Equal(0, crafting.ActiveCraftCount);
            Assert.Equal(1, specialty.GetMasteryTier(CrafterElena));
        }

        [Fact]
        public void CraftingSystem_SaveRestore_PreservesActiveCraftAttribution()
        {
            var inv1 = new InventoryContainer();
            var scrapItem = new ItemDefinition { id = "scrap_metal", stackMax = 50 };
            var wrenchItem = new ItemDefinition { id = "wrench_standard", stackMax = 10 };
            inv1.Add(scrapItem, 10);

            var station = new CraftingStation { id = "workbench", condition = 100f };
            var crafting1 = new CraftingSystem(inv1);
            crafting1.AddStation(station);

            var recipe = new Recipe
            {
                id = "recipe_wrench",
                recipeName = "Standard Wrench",
                requiredStationId = "workbench",
                craftingTimeHours = 10f,
                result = wrenchItem,
                resultAmount = 1,
                ingredients = new List<Ingredient>
                {
                    new Ingredient { item = scrapItem, amount = 2 }
                }
            };

            crafting1.SetCrafterProfessionLookup(id => id == CrafterElena ? "machinist" : null);
            crafting1.StartCraft(recipe, crafterId: CrafterElena, stationId: "workbench", professionId: "machinist");

            // Capture state midway
            var save = crafting1.CaptureState();
            Assert.NotNull(save.ActiveCrafts);
            Assert.Single(save.ActiveCrafts);
            Assert.Equal(CrafterElena, save.ActiveCrafts[0].CrafterId);
            Assert.Equal("workbench", save.ActiveCrafts[0].StationId);
            Assert.Equal("machinist", save.ActiveCrafts[0].CrafterProfessionId);

            // Restore into fresh system
            var inv2 = new InventoryContainer();
            var crafting2 = new CraftingSystem(inv2);
            crafting2.AddStation(station);
            crafting2.SetRecipeLookup(id => id == "recipe_wrench" ? recipe : null);
            crafting2.RestoreState(save);

            var specialty = new TradeSpecialtySystem();
            specialty.BindToCrafting(crafting2);

            // Complete the remaining craft
            crafting2.Tick(12f);

            Assert.Equal(0, crafting2.ActiveCraftCount);
            Assert.Equal(1, specialty.GetMasteryTier(CrafterElena));
        }

        [Fact]
        public void TradeSpecialty_SaveRestore_PreservesAttributedProgress()
        {
            var sys = new TradeSpecialtySystem();
            sys.OnItemCrafted(CrafterElena, "machinist", "wrench_standard");
            sys.OnItemCrafted(CrafterElena, "machinist", "gear_standard");
            Assert.Equal(2, sys.GetMasteryTier(CrafterElena));

            var save = sys.CaptureState();
            var restored = new TradeSpecialtySystem();
            restored.RestoreState(save);

            Assert.Equal(2, restored.GetMasteryTier(CrafterElena));

            // Complete 3rd craft on restored state
            restored.OnItemCrafted(CrafterElena, "machinist", "spring_standard");
            Assert.Equal(3, restored.GetMasteryTier(CrafterElena));
            Assert.True(restored.HasMasteredTrade(CrafterElena));
        }

        [Theory]
        [InlineData("mechanic", "machinist")]
        [InlineData("Mechanical Engineer", "machinist")]
        [InlineData("Doctor", "nurse")]
        [InlineData("surgeon", "nurse")]
        [InlineData("Paramedic", "nurse")]
        [InlineData("Electrician", "electrician")]
        [InlineData("Teacher", "teacher")]
        public void NormalizeProfession_CanonicalizesKnownSynonyms(string raw, string expected)
        {
            string canonical = TradeSpecialtySystem.NormalizeProfession(raw);
            Assert.Equal(expected, canonical);
        }
    }
}
