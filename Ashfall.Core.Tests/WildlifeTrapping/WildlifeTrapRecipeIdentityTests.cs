// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Workstream D — trap crafting recipe output verification (flagship
    /// trapping tranche). Proves the full identity chain
    ///
    ///   Recipe (recipes.json) → Crafted Item (items.json)
    ///     → Trap Definition (wildlife_trapping_catalog.json) → deployment
    ///
    /// against the real data authority through the production loaders, plus
    /// a craft-to-inventory transaction through the real CraftingSystem.
    ///
    /// LOCKED AUTHORING CONTRACT:
    ///
    ///   craft_trap_improvised_wire → trap_improvised_wire
    ///       ingredients: copper_wire_10m_of_10m ×1
    ///   craft_trap_box             → trap_box
    ///       ingredients: scrap_wood ×2, scrap_metal ×1, box_of_nails_10 ×1
    ///   craft_trap_fish            → trap_fish
    ///       ingredients: scrap_wood ×2, rope ×1
    /// </summary>
    public sealed class WildlifeTrapRecipeIdentityTests
    {
        private static string FindDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir)) return dir;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir)) return dir;
            return "Assets/StreamingAssets/Data";
        }

        private static (List<Recipe> recipes, ItemCatalog items, WildlifeTrappingCatalog traps) Load()
        {
            string dataDir = FindDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var items = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, json);
            var recipes = RecipeCatalogLoader.Load(dataDir, fileIO, json, items);
            var traps = WildlifeTrappingCatalogLoader.Load(dataDir, fileIO, json);
            Assert.NotNull(traps);
            return (recipes, items, traps!);
        }

        private static Recipe GetTrapRecipe(List<Recipe> recipes, string recipeId)
        {
            var recipe = recipes.SingleOrDefault(r => r.id == recipeId);
            Assert.True(recipe != null, $"trap recipe '{recipeId}' missing from recipes.json");
            return recipe!;
        }

        private static readonly (string recipeId, string resultId)[] TrapRecipes =
        {
            ("craft_trap_improvised_wire", "trap_improvised_wire"),
            ("craft_trap_box", "trap_box"),
            ("craft_trap_fish", "trap_fish")
        };

        // ── D1/D2: recipes load, outputs locked ─────────────────────────────

        [Fact]
        public void TrapRecipes_LoadWithUniqueIds()
        {
            var (recipes, _, _) = Load();
            foreach (var (recipeId, _) in TrapRecipes)
                _ = GetTrapRecipe(recipes, recipeId); // throws with a clear message when missing
        }

        [Fact]
        public void TrapRecipes_ResultIdentity_MatchesTrapDefinitionIds()
        {
            var (recipes, items, traps) = Load();
            foreach (var (recipeId, resultId) in TrapRecipes)
            {
                var recipe = GetTrapRecipe(recipes, recipeId);
                Assert.True(recipe.result != null, $"{recipeId}: result item '{resultId}' must resolve in items.json");
                Assert.Equal(resultId, recipe.result.id);
                Assert.Equal(1, recipe.resultAmount);

                // The invariant: recipe.resultItemId == trapDefinition.trap_id
                Assert.True(traps.Traps.ContainsKey(resultId),
                    $"{recipeId} result '{resultId}' has no matching trap definition — crafted trap would be undeployable");
            }
        }

        // ── D3: ingredients locked exactly (identity + quantity) ────────────

        [Fact]
        public void TrapRecipes_Ingredients_MatchAuthoredDesign()
        {
            var (recipes, _, _) = Load();

            var expected = new Dictionary<string, (string item, int amount)[]>
            {
                ["craft_trap_improvised_wire"] = new[] { ("copper_wire_10m_of_10m", 1) },
                ["craft_trap_box"] = new[] { ("scrap_wood", 2), ("scrap_metal", 1), ("box_of_nails_10", 1) },
                ["craft_trap_fish"] = new[] { ("scrap_wood", 2), ("rope", 1) }
            };

            foreach (var (recipeId, expectedIngredients) in expected)
            {
                var recipe = GetTrapRecipe(recipes, recipeId);
                var actual = recipe.ingredients
                    .GroupBy(i => (i.item?.id ?? string.Empty), StringComparer.Ordinal)
                    .ToDictionary(g => g.Key, g => g.Sum(i => i.amount), StringComparer.Ordinal);

                Assert.True(actual.Count == expectedIngredients.Length,
                    $"{recipeId}: expected {expectedIngredients.Length} distinct ingredients, found {actual.Count} " +
                    $"[{string.Join(", ", actual.Keys)}]");

                foreach (var (item, amount) in expectedIngredients)
                {
                    Assert.True(actual.TryGetValue(item, out int found) && found == amount,
                        $"{recipeId}: ingredient mismatch — expected '{item}' ×{amount}, " +
                        $"found {(actual.TryGetValue(item, out int f) ? $"{item} ×{f}" : "missing")}");
                }
            }
        }

        // ── D4: item-catalog reference resolution ────────────────────────────

        [Fact]
        public void TrapRecipes_AllIngredientsAndResults_ResolveInItemCatalog()
        {
            var (recipes, items, _) = Load();
            foreach (var (recipeId, _) in TrapRecipes)
            {
                var recipe = GetTrapRecipe(recipes, recipeId);
                Assert.True(items.Get(recipe.result.id) != null,
                    $"{recipeId}: result '{recipe.result.id}' does not resolve in items.json");
                foreach (var ing in recipe.ingredients)
                {
                    Assert.True(ing.item != null && items.Get(ing.item.id) != null,
                        $"{recipeId}: ingredient '{ing.item?.id ?? "(null)"}' does not resolve in items.json");
                    Assert.True(ing.amount > 0, $"{recipeId}: ingredient '{ing.item.id}' has non-positive amount");
                }
            }
        }

        // ── D5: trap definition parameter sanity ─────────────────────────────

        [Fact]
        public void TrapDefinitions_ForRecipeResults_HaveDeployableParameters()
        {
            var (_, _, traps) = Load();
            foreach (var (_, resultId) in TrapRecipes)
            {
                var def = traps.Traps[resultId];
                Assert.True(def.checkIntervalDays > 0, $"{resultId}: checkIntervalDays must be positive");
                Assert.True(def.durabilityChecks > 0, $"{resultId}: durabilityChecks must be positive");
                Assert.False(string.IsNullOrEmpty(def.trapType), $"{resultId}: trapType must be authored");
            }
        }

        // ── D8/D9: cross-catalog integrity rule ──────────────────────────────

        [Fact]
        public void TrapRecipeIntegrity_AllRealRecipes_PassClean()
        {
            var (recipes, _, traps) = Load();
            var errors = TrapRecipeIntegrity.Validate(recipes, traps.Traps);
            Assert.True(errors.Count == 0,
                "trap recipe integrity violations:\n" + string.Join("\n", errors));
        }

        [Fact]
        public void TrapRecipeIntegrity_BrokenResultMapping_IsCaughtWithClearMessage()
        {
            // Negative fixture: the result item EXISTS in the item catalog
            // (so ordinary reference validation passes), but no trap
            // definition matches — the subtle case where deployment would
            // fail at runtime despite a clean items.json check.
            var (_, items, traps) = Load();
            var ordinaryItem = items.Get("cloth");
            Assert.True(ordinaryItem != null, "fixture requires 'cloth' in items.json");

            var broken = new Recipe
            {
                id = "craft_trap_broken_fixture",
                recipeName = "Deliberately Broken Trap Fixture",
                result = ordinaryItem!, // valid item, NOT a trap definition
                resultAmount = 1
            };

            var errors = TrapRecipeIntegrity.Validate(new[] { broken }, traps.Traps);
            Assert.Single(errors);
            Assert.Contains("no matching trap definition", errors[0]);
            Assert.Contains("craft_trap_broken_fixture", errors[0]);
        }

        // ── D6: craft-to-inventory through the real CraftingSystem ──────────

        [Theory]
        [InlineData("craft_trap_improvised_wire")]
        [InlineData("craft_trap_box")]
        [InlineData("craft_trap_fish")]
        public void CraftToInventory_RealRecipe_ProducesExactlyTheTrapItem(string recipeId)
        {
            var (recipes, items, _) = Load();
            var recipe = GetTrapRecipe(recipes, recipeId);

            var inv = new InventoryContainer();
            foreach (var ing in recipe.ingredients)
                Assert.True(inv.Add(ing.item, ing.amount), $"fixture: could not seed '{ing.item.id}'");

            var sys = new CraftingSystem(inv);
            sys.AddStation(new CraftingStation { id = "workbench" });

            Assert.True(sys.CanCraft(recipe), $"{recipeId}: CanCraft must pass with exact ingredients");
            Assert.True(sys.StartCraft(recipe), $"{recipeId}: StartCraft must succeed with exact ingredients");

            // Ingredients consumed at start
            foreach (var ing in recipe.ingredients)
                Assert.Equal(0, inv.Count(ing.item));

            sys.Tick(recipe.craftingTimeHours + 0.1f);

            // Exactly one correct trap item, no adjacent trap item
            Assert.Equal(recipe.resultAmount, inv.Count(recipe.result));
            foreach (var (_, otherResultId) in TrapRecipes)
            {
                if (otherResultId == recipe.result.id) continue;
                var otherDef = items.Get(otherResultId);
                if (otherDef != null)
                    Assert.Equal(0, inv.Count(otherDef));
            }
        }

        // ── D7: crafted item deploys with catalog parameters ─────────────────

        [Fact]
        public void CraftedTrap_DeploysThroughCoreSetTrap_WithCatalogParameters()
        {
            var (recipes, _, traps) = Load();
            var recipe = GetTrapRecipe(recipes, "craft_trap_improvised_wire");
            var def = traps.Traps["trap_improvised_wire"];

            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.RegisterTrapDefinition(traps.Traps["trap_improvised_wire"]);

            const int deployDay = 7;
            sys.TickDay(deployDay, 0f);
            var res = sys.SetTrap("site_crafted", "bait_scrap_meat", "hunter_1",
                def.trapType, def.trap_id, def.checkIntervalDays, def.durabilityChecks);

            Assert.True(res.IsSuccess);
            var site = sys.State.trapSites.Single(s => s.siteId == "site_crafted");
            Assert.Equal(recipe.result.id, site.trapId); // crafted item identity == deployed trap identity
            Assert.Equal(def.durabilityChecks, site.remainingDurability);
            Assert.Equal(deployDay + def.checkIntervalDays, site.checkDay);
        }
    }
}
