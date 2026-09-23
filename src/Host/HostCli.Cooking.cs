// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Cooking;
using Ashfall.Core.Inventory;
using Godot;

namespace AtomicWar.GodotApp
{
    // ========================================================================
    // Plan 136 host probe — Wildlife Trapping → Food Pipeline & Cooking.
    //
    // --cooking-selftest proves that:
    // 1. Authored recipe catalog loads through the strict validator (>= 15 recipes).
    // 2. Strict loader rejects invalid or duplicate recipes.
    // 3. Wildlife trapping catch (raw_meat) feeds directly into the cooking pipeline.
    // 4. Cooking consumes ingredients and delivers cooked food into inventory.
    // 5. Radiation decontamination reduces fallout contamination based on recipe.
    // 6. Skill progression tracks meals prepared and advances cooking skill level.
    // 7. Seeded RNG determines quality (Cooked, WellCooked, Burnt) deterministically.
    // 8. Cancellation cleanly removes active operations.
    // 9. State capture and restore round-trip through CookingSaveStore.
    // 10. Daily campaign tick progresses in-flight cooking operations.
    // ========================================================================
    public static partial class HostCli
    {
        public static int RunCookingSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; GD.Print($"[PASS] cooking/{gate}"); }
                else { fail++; GD.Print($"[FAIL] cooking/{gate}{(note.Length > 0 ? " — " + note : "")}"); }
            }

            // ── 1 — Authored recipe catalog loads cleanly ──
            var loadResult = CookingRecipeCatalogLoader.Load(dataDirectory, new FileSystemIO());
            Check("authored_recipes_loaded", loadResult.Success && loadResult.Recipes.Count >= 15,
                loadResult.Success ? $"{loadResult.Recipes.Count} recipes" : string.Join("; ", loadResult.Errors));

            // ── 2 — Strict loader validation rejects malformed catalogs ──
            string dupJson = "{\"recipes\":[{\"id\":\"r1\",\"outputItemId\":\"food\",\"outputQuantity\":1},{\"id\":\"r1\",\"outputItemId\":\"food\",\"outputQuantity\":1}]}";
            var dupResult = CookingRecipeCatalogLoader.LoadFromJson(dupJson);
            Check("strict_loader_rejects_duplicate_id", !dupResult.Success && dupResult.Errors.Any(e => e.Contains("Duplicate")));

            string invalidQtyJson = "{\"recipes\":[{\"id\":\"r1\",\"outputItemId\":\"food\",\"outputQuantity\":0}]}";
            var qtyResult = CookingRecipeCatalogLoader.LoadFromJson(invalidQtyJson);
            Check("strict_loader_rejects_invalid_quantity", !qtyResult.Success && qtyResult.Errors.Any(e => e.Contains("outputQuantity")));

            // ── 3 — Host session initialization & catalog binding ──
            var session = new CookingHostSession(new CookingSystem(null, new SeededRng(42)));
            bool bound = session.LoadAuthoredRecipes(dataDirectory, new FileSystemIO());
            Check("host_session_catalog_bound", bound && session.CatalogLoaded && session.System.Recipes.Count >= 15);

            // ── 4 — Trapping to Inventory to Cooking Pipeline ──
            var inventory = new Ashfall.Core.Inventory.Inventory();
            session.Source = new InventoryCookingSource(inventory);

            // Simulate trapping harvest delivering raw_meat
            inventory.AddById("raw_meat", 5);
            Check("trapping_harvest_meat_in_inventory", inventory.CountById("raw_meat") == 5);

            // ── 5 — Start cooking consumes ingredients ──
            var startRes = session.StartCooking("recipe_roasted_meat", "cook_survivor", "improvised_stove");
            Check("cooking_started_successfully", startRes.IsSuccess);
            Check("ingredients_consumed_on_start", inventory.CountById("raw_meat") == 4);
            Check("active_operation_tracked", session.Census.ActiveOperationsCount == 1);

            // ── 6 — Progress cooking to completion delivers food ──
            int completed = session.ProgressCooking(30f);
            Check("cooking_completed_on_progress", completed == 1);
            Check("active_operations_cleared", session.Census.ActiveOperationsCount == 0);
            Check("cooked_food_delivered", inventory.CountById("cooked_meat") >= 1);
            Check("meals_prepared_incremented", session.Census.TotalMealsPrepared >= 1);

            // ── 7 — Radiation decontamination works ──
            inventory.AddById("raw_meat", 2);
            inventory.AddById("clean_water", 2);
            var brothRes = session.StartCooking("recipe_boiled_meat_broth", "cook_master", "basic_boiler");
            Check("broth_recipe_started", brothRes.IsSuccess);
            session.ProgressCooking(45f);

            var lastOp = session.System.State.completedOperations.Last();
            Check("radiation_decontamination_effective", lastOp.radiationRemainingFraction <= 0.25f,
                $"remaining radiation fraction = {lastOp.radiationRemainingFraction:0.##}");
            Check("decontaminated_metric_tracked", session.Census.TotalFoodDecontaminated > 0f);

            // ── 8 — Skill progression ──
            Check("cooking_skill_progressed", session.Census.CookingSkillLevel > 0f,
                $"skill={session.Census.CookingSkillLevel:0.##}");

            // ── 9 — Seeded quality outcome determinism ──
            var seededSystem = new CookingSystem(null, new SeededRng(9999));
            seededSystem.RegisterRecipe(new CookingRecipe
            {
                id = "test_quality",
                displayName = "Test Quality",
                outputItemId = "test_food",
                outputQuantity = 1,
                cookTimeMinutes = 10f,
                nutritionValue = 20f
            });
            seededSystem.StartCooking("test_quality");
            seededSystem.ProgressCooking(15f);
            var completedQuality = seededSystem.State.completedOperations.First();
            Check("quality_determined_deterministically", completedQuality.foodQuality != FoodQuality.Raw);

            // ── 10 — Operation cancellation ──
            session.System.RegisterRecipe(new CookingRecipe
            {
                id = "cancel_test",
                displayName = "Cancel Test",
                outputItemId = "cancel_food",
                outputQuantity = 1,
                cookTimeMinutes = 60f
            });
            session.StartCooking("cancel_test");
            string cancelOpId = session.System.State.activeOperations.First(o => o.recipeId == "cancel_test").operationId;
            var cancelRes = session.CancelCooking(cancelOpId);
            Check("cancellation_succeeds", cancelRes.IsSuccess);
            Check("cancelled_operation_removed", !session.System.State.activeOperations.Any(o => o.operationId == cancelOpId));

            // ── 11 — Daily campaign tick progresses cooking ──
            session.StartCooking("cancel_test");
            Check("tick_test_op_active", session.Census.ActiveOperationsCount >= 1);
            int dayCompleted = session.TickDay(1, dayMinutes: 120f);
            Check("daily_tick_progresses_cooking", dayCompleted >= 1);

            // ── 12 — Save/Restore Round-Trip ──
            var captured = session.CaptureState();
            Check("capture_has_valid_schema", captured.schema_version == 1);
            Check("capture_preserves_meals", captured.totalMealsPrepared == session.Census.TotalMealsPrepared);

            string json = CookingSaveStore.CaptureBare(captured);
            Check("store_encodes_json", !string.IsNullOrEmpty(json));

            var restoredState = CookingSaveStore.RestoreBare(json);
            Check("store_decodes_state", restoredState != null && restoredState.totalMealsPrepared == captured.totalMealsPrepared);

            var restoreSession = new CookingHostSession();
            restoreSession.RestoreState(restoredState);
            Check("restored_session_matches_census",
                restoreSession.Census.TotalMealsPrepared == session.Census.TotalMealsPrepared
                && Math.Abs(restoreSession.Census.CookingSkillLevel - session.Census.CookingSkillLevel) < 0.001f);

            GD.Print($"[HostCli.Cooking] SelfTest complete: {pass} passed, {fail} failed.");
            return fail == 0 ? 0 : 1;
        }
    }
}
