// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class Plan22KitchenNutritionTests
    {
        private static KitchenNutritionSystem CreateSystem(out Inventory.Inventory inv, out NeedsSystem needs, int seed = 42)
        {
            inv = new Inventory.Inventory();
            needs = new NeedsSystem();
            return new KitchenNutritionSystem(new SeededRng(seed), inv, needs);
        }

        [Fact]
        public void ServeMeal_ReducesHungerAndAppliesMorale()
        {
            var k = CreateSystem(out var inv, out var needs);
            inv.AddById("meat", 10);
            k.StartPrepJob("recipe_fungal_stew", "cook_1", new Dictionary<string, int> { { "meat", 2 } });
            k.TickDay(1);

            needs.Register(new SurvivorNeedsState { Id = "survivor_sasha", Hunger = 60f, Morale = 50f });
            float initialHunger = needs.Get("survivor_sasha")!.Hunger;
            float initialMorale = needs.Get("survivor_sasha")!.Morale;

            var res = k.ServeMeal("survivor_sasha", "recipe_fungal_stew");
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);

            float postHunger = needs.Get("survivor_sasha")!.Hunger;
            float postMorale = needs.Get("survivor_sasha")!.Morale;

            // Hunger must be reduced significantly (at least 30, for safe meal 8 * 6 = 48)
            Assert.True(postHunger < initialHunger);
            Assert.True(initialHunger - postHunger >= 30f);

            // Morale must be changed
            Assert.NotEqual(initialMorale, postMorale);

            // Serving log must be recorded
            Assert.Single(k.State.servingLog);
            Assert.Equal("survivor_sasha", k.State.servingLog[0].survivorId);
            Assert.Equal("recipe_fungal_stew", k.State.servingLog[0].recipeId);
        }

        [Fact]
        public void ServeAllMeals_SufficientPortions_ServesAllLivingSurvivors()
        {
            var k = CreateSystem(out var inv, out var needs);
            inv.AddById("meat", 10);
            // Produce 3 portions
            k.StartPrepJob("recipe_stew", "cook_1", new Dictionary<string, int> { { "meat", 2 } });
            k.TickDay(1);

            var crew = new List<string> { "survivor_a", "survivor_b", "survivor_c" };
            foreach (var s in crew)
            {
                needs.Register(new SurvivorNeedsState { Id = s, Hunger = 70f });
            }

            var res = k.ServeAllMeals(crew, "recipe_stew");
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.Equal(0, k.GetAvailablePortions("recipe_stew"));
            Assert.Equal(3, k.State.servingLog.Count);

            foreach (var s in crew)
            {
                Assert.True(needs.Get(s)!.Hunger < 70f);
            }
        }

        [Fact]
        public void ServeAllMeals_InsufficientPortions_BlocksAtomicallyWithoutPartialConsumption()
        {
            var k = CreateSystem(out var inv, out var needs);
            inv.AddById("meat", 10);
            // Produce 3 portions
            k.StartPrepJob("recipe_stew", "cook_1", new Dictionary<string, int> { { "meat", 2 } });
            k.TickDay(1);

            var crew = new List<string> { "survivor_1", "survivor_2", "survivor_3", "survivor_4" }; // 4 crew, only 3 portions
            foreach (var s in crew)
            {
                needs.Register(new SurvivorNeedsState { Id = s, Hunger = 80f });
            }

            var res = k.ServeAllMeals(crew, "recipe_stew");
            Assert.Equal(ActionResult.StatusKind.Blocked, res.Status);
            Assert.Equal("insufficient_portions", res.FailureCode);

            // Atomicity check: all 3 portions remain, 0 meals logged, no survivor hunger modified
            Assert.Equal(3, k.GetAvailablePortions("recipe_stew"));
            Assert.Empty(k.State.servingLog);
            foreach (var s in crew)
            {
                Assert.Equal(80f, needs.Get(s)!.Hunger);
            }
        }

        [Fact]
        public void UpdateSpoilage_ExpiresPortionsAndEmitsEvent()
        {
            var k = CreateSystem(out var inv, out _);
            inv.AddById("meat", 10);
            k.StartPrepJob("recipe_stew", "cook_1", new Dictionary<string, int> { { "meat", 2 } });
            k.TickDay(1); // Produced, ambient shelf life = 2 days

            string? spoiledRecipe = null;
            int spoiledCount = 0;
            k.OnPortionsSpoiled += (recipeId, count) =>
            {
                spoiledRecipe = recipeId;
                spoiledCount = count;
            };

            // Day 2 tick (spoilageTimer decreases from 2 to 1)
            k.TickDay(2);
            Assert.Equal(3, k.GetAvailablePortions("recipe_stew"));

            // Day 3 tick (spoilageTimer decreases from 1 to 0 -> spoils)
            k.TickDay(3);
            Assert.Equal(0, k.GetAvailablePortions("recipe_stew"));
            Assert.Equal("recipe_stew", spoiledRecipe);
            Assert.Equal(3, spoiledCount);

            // Cannot serve spoiled meals
            var serveRes = k.ServeMeal("survivor_x", "recipe_stew");
            Assert.Equal(ActionResult.StatusKind.Blocked, serveRes.Status);
        }

        [Fact]
        public void SpoilagePreservation_CellarAndRefrigeration_SetsAuthoritativeDays()
        {
            // Ambient = 2 days
            var k1 = CreateSystem(out var inv1, out _);
            inv1.AddById("meat", 5);
            k1.StartPrepJob("stew_ambient", "cook", new Dictionary<string, int> { { "meat", 1 } });
            k1.TickDay(1);
            Assert.Equal(2f, k1.State.pantry[0].spoilageTimer);
            Assert.Equal(PreservationMethod.None, k1.State.pantry[0].preservation);

            // Cellar = 5 days
            var k2 = CreateSystem(out var inv2, out _);
            k2.SetCellar(true, 8f);
            inv2.AddById("meat", 5);
            k2.StartPrepJob("stew_cellar", "cook", new Dictionary<string, int> { { "meat", 1 } });
            k2.TickDay(1);
            Assert.Equal(5f, k2.State.pantry[0].spoilageTimer);
            Assert.Equal(PreservationMethod.RootCellar, k2.State.pantry[0].preservation);

            // Refrigeration = 14 days
            var k3 = CreateSystem(out var inv3, out _);
            k3.SetRefrigeration(true);
            inv3.AddById("meat", 5);
            k3.StartPrepJob("stew_fridge", "cook", new Dictionary<string, int> { { "meat", 1 } });
            k3.TickDay(1);
            Assert.Equal(14f, k3.State.pantry[0].spoilageTimer);
            Assert.Equal(PreservationMethod.Refrigeration, k3.State.pantry[0].preservation);
        }

        [Fact]
        public void KitchenSaveRoundtrip_PreservesPantryAndServingLogs()
        {
            var k = CreateSystem(out var inv, out var needs);
            inv.AddById("meat", 10);
            k.SetCellar(true, 7f);
            k.StartPrepJob("stew_save", "cook", new Dictionary<string, int> { { "meat", 2 } });
            k.TickDay(1);
            k.ServeMeal("survivor_test", "stew_save");

            var state = k.CaptureState();
            Assert.True(state.hasCellar);
            Assert.Equal(7f, state.cellarTempC);
            Assert.Single(state.pantry);
            Assert.Equal(2, state.pantry[0].portionCount);
            Assert.Single(state.servingLog);

            var k2 = CreateSystem(out _, out _);
            k2.RestoreState(state);
            Assert.True(k2.State.hasCellar);
            Assert.Equal(7f, k2.State.cellarTempC);
            Assert.Equal(2, k2.GetAvailablePortions("stew_save"));
            Assert.Single(k2.State.servingLog);
            Assert.Equal("survivor_test", k2.State.servingLog[0].survivorId);
        }
    }
}
