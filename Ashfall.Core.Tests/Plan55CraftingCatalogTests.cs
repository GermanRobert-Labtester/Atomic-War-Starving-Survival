using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 55 — crafting recipe expansion contract tests.
    ///
    /// Pins the expanded recipes.json authority (73 legacy + 8 Plan-55 recipes
    /// = 81) against the merged item catalog: reference resolution, positive
    /// amounts, station identity, reachability of every Plan-55 ingredient,
    /// and ammo material conservation (no free casings/primers/powder).
    /// </summary>
    public sealed class Plan55CraftingCatalogTests
    {
        private static string? FindDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir)) return dir;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir)) return dir;
            return null;
        }

        private static (List<Recipe> recipes, ItemCatalog catalog) Load()
        {
            string? dataDir = FindDataDir();
            Assert.False(dataDir == null, "StreamingAssets/Data directory not found");
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var catalog = ItemCatalogLoader.LoadCatalog(dataDir!, fileIO, serializer);
            var recipes = RecipeCatalogLoader.Load(dataDir!, fileIO, serializer, catalog);
            return (recipes, catalog);
        }

        [Fact]
        public void Catalog_reaches_the_80_recipe_breadth_target()
        {
            var (recipes, _) = Load();
            Assert.True(recipes.Count >= 80,
                $"Expected at least 80 recipes after Plan 55, found {recipes.Count}");
        }

        [Fact]
        public void All_plan55_recipe_ids_are_present_and_unique()
        {
            var (recipes, _) = Load();
            var ids = recipes.Select(r => r.id).ToList();
            var plan55 = new[]
            {
                "craft_flatbread", "craft_boiled_roots", "craft_vegetable_soup",
                "craft_pemmican", "craft_travel_ration", "craft_splint",
                "reload_556", "reload_762"
            };
            foreach (var id in plan55)
                Assert.True(ids.Contains(id), $"Plan 55 recipe missing: {id}");
            Assert.Equal(ids.Count, ids.Distinct().Count());
        }

        [Fact]
        public void Every_recipe_ingredient_and_result_resolves_and_is_positive()
        {
            var (recipes, catalog) = Load();
            foreach (var recipe in recipes)
            {
                Assert.False(string.IsNullOrEmpty(recipe.id), "recipe with empty id");
                Assert.True(recipe.resultAmount > 0, $"{recipe.id}: resultAmount must be positive");
                Assert.True(recipe.craftingTimeHours > 0f, $"{recipe.id}: craftingTimeHours must be positive");
                Assert.True(recipe.result != null,
                    $"{recipe.id}: resultItemId '{recipe.result?.id}' does not resolve in the merged item catalog");
                foreach (var ing in recipe.ingredients)
                {
                    Assert.True(ing.item != null,
                        $"{recipe.id}: ingredient does not resolve in the merged item catalog");
                    Assert.True(ing.amount > 0, $"{recipe.id}: ingredient amount must be positive");
                }
            }
        }

        /// <summary>
        /// Pre-existing legacy recipes that reference the "distiller" station.
        /// No shelter infrastructure owns a distiller, so these four remain
        /// unreachable — a documented pre-Plan-55 gap, not a Plan-55 recipe.
        /// </summary>
        public static readonly HashSet<string> LegacyDistillerRecipes =
            new HashSet<string>(StringComparer.Ordinal)
            { "brew_lethe_substitute", "craft_distilled_spirits", "craft_fuel_gel", "craft_antiseptic_solution" };

        [Fact]
        public void Plan55_recipes_only_use_registered_station_ids()
        {
            // Runtime registration authority: CraftingHostSession seeds/syncs
            // "workbench"; Main.World.SyncCraftingStationsFromShelter bridges
            // "stove" (room_kitchen), "heater" (room_generator) and
            // "water_purifier" (room_filtration). New Plan-55 recipes must
            // not use the unregistered "distiller" station.
            var (recipes, _) = Load();
            var allowed = new HashSet<string>(StringComparer.Ordinal)
                { "workbench", "stove", "heater", "water_purifier", "" };
            var plan55 = new HashSet<string>(StringComparer.Ordinal)
                { "craft_flatbread", "craft_boiled_roots", "craft_vegetable_soup",
                  "craft_pemmican", "craft_travel_ration", "craft_splint",
                  "reload_556", "reload_762" };
            foreach (var recipe in recipes)
            {
                if (plan55.Contains(recipe.id))
                    Assert.True(allowed.Contains(recipe.requiredStationId),
                        $"Plan 55 recipe {recipe.id}: station '{recipe.requiredStationId}' has no runtime registration");
            }
        }

        [Fact]
        public void Legacy_distiller_gap_is_bounded_to_known_recipes()
        {
            var (recipes, _) = Load();
            var distiller = recipes.Where(r => r.requiredStationId == "distiller").Select(r => r.id).ToHashSet(StringComparer.Ordinal);
            Assert.True(distiller.SetEquals(LegacyDistillerRecipes),
                $"Unexpected distiller-station recipe set: {string.Join(", ", distiller)}");
        }

        [Fact]
        public void Plan55_food_outputs_resolve_as_food_items()
        {
            var (_, catalog) = Load();
            var plan55Food = new[] { "item_flatbread", "item_boiled_roots", "item_vegetable_soup", "item_pemmican", "item_travel_ration" };
            foreach (var id in plan55Food)
            {
                var def = catalog.Get(id);
                Assert.True(def != null, $"Plan 55 food item missing from catalog: {id}");
                Assert.Equal(ItemType.Food, def!.type);
                Assert.True(def.hungerRestore > 0f, $"{id}: food output must restore hunger");
            }
        }

        [Fact]
        public void Plan55_ammo_reload_recipes_consume_casings_one_to_one()
        {
            // Material conservation: a reload batch may never produce more
            // rounds than the brass consumed — no free casings, primers or
            // powder may be created by crafting.
            var (recipes, _) = Load();
            foreach (var id in new[] { "reload_556", "reload_762" })
            {
                var recipe = recipes.First(r => r.id == id);
                int brass = recipe.ingredients.Where(i => i.item != null && i.item.id == "empty_brass_shell")
                                             .Sum(i => i.amount);
                int primers = recipe.ingredients.Where(i => i.item != null && i.item.id == "reloading_primer")
                                                .Sum(i => i.amount);
                int powder = recipe.ingredients.Where(i => i.item != null && i.item.id == "smokeless_powder")
                                               .Sum(i => i.amount);
                Assert.True(brass > 0, $"{id}: must consume empty_brass_shell");
                Assert.True(primers > 0, $"{id}: must consume reloading_primer");
                Assert.True(powder > 0, $"{id}: must consume smokeless_powder");
                Assert.True(recipe.resultAmount <= brass,
                    $"{id}: output {recipe.resultAmount} exceeds brass consumed {brass} — free casings are forbidden");
            }
        }

        [Fact]
        public void Reloading_components_have_live_acquisition_paths()
        {
            // Plan 55 provenance: primers and powder must be obtainable
            // (military depot / police station scavenging tables), otherwise
            // every reload recipe — legacy and new — is unreachable.
            string? dataDir = FindDataDir();
            Assert.False(dataDir == null, "StreamingAssets/Data directory not found");
            string path = Path.Combine(dataDir!, "scavenging_tables.json");
            var raw = new FileSystemIO().ReadAllText(path);
            Assert.Contains("reloading_primer", raw, StringComparison.Ordinal);
            Assert.Contains("smokeless_powder", raw, StringComparison.Ordinal);
        }

        [Fact]
        public void Splint_recipe_output_matches_medical_treatment_requirement()
        {
            // medical_texts.json 'medical_fracture' requires the exact item
            // id "splint"; the crafted output must be that canonical id.
            var (recipes, catalog) = Load();
            var splint = recipes.First(r => r.id == "craft_splint");
            Assert.Equal("splint", splint.result?.id);
            Assert.True(catalog.Get("splint") != null, "splint item must resolve");
        }

        [Fact]
        public void Plan55_food_output_values_do_not_exceed_ingredient_values()
        {
            // Buy→craft→sell guard: output trade value must not exceed the
            // summed ingredient trade value, so no Plan-55 food recipe is an
            // unlimited money printer.
            var (recipes, catalog) = Load();
            foreach (var id in new[] { "craft_flatbread", "craft_boiled_roots", "craft_vegetable_soup", "craft_pemmican", "craft_travel_ration" })
            {
                var recipe = recipes.First(r => r.id == id);
                float input = recipe.ingredients.Sum(i => (i.item?.tradeValue ?? 0f) * i.amount);
                float output = (recipe.result?.tradeValue ?? 0f) * recipe.resultAmount;
                Assert.True(output <= input,
                    $"{id}: output value {output} exceeds ingredient value {input} — arbitrage loop");
            }
        }
    }
}
