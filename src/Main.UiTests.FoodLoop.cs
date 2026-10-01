// SPDX-License-Identifier: MIT
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        /// <summary>
        /// Food-loop verification (user task 5) — the real composed game, the
        /// real panels' host seams, not standalone fixtures.
        ///
        /// Gates:
        ///   1. A fresh New Game knows every starter cooking recipe: the
        ///      authored catalog loads AND the discovery list survives boot
        ///      (regression gate for the RestoreState(null) wipe that logged
        ///      "[Cooking] 0 recipes loaded" on every healthy boot).
        ///   2. The kitchen panel is bound to the live kitchen session and
        ///      opens.
        ///   3. Cook: the START PREP button's seam (StartPrepJob) starts a
        ///      job and consumes the ingredient bill.
        ///   4. A real day advance (TickSimDay) completes the job into pantry
        ///      portions through the kitchen day owner.
        ///   5. Eat: the SERVE ALL button's seam (ServeAllMeals) reduces
        ///      every living survivor's hunger.
        ///   6. Eat/drink: the holdfast terminal's seam (ConsumeResult)
        ///      reduces hunger (canned_food) and thirst (clean_water).
        ///   7. The Plan 136 cooking authority (recipes_cooking.json) is also
        ///      live in the composed game: StartCooking + ProgressCooking
        ///      deliver cooked_meal into the bound inventory.
        ///   8. The kitchen panel's cooking strip (PFGL W5 P2) drives that same
        ///      authority: START COOK consumes the authored bill, ADVANCE
        ///      completes the batch, CANCEL removes it — no direct-core call.
        /// </summary>
        private void RunFoodLoopSelfTestAndQuit()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "ashfall_food_loop_" + DateTime.UtcNow.Ticks); // DETERMINISM_ALLOWLIST: Test harness temporary directory
            bool pass = true;
            void Check(bool cond, string name)
            {
                if (cond) GD.Print($"  [PASS] {name}");
                else { GD.PrintErr($"  [FAIL] {name}"); pass = false; }
            }

            try
            {
                Directory.CreateDirectory(tempDir);

                GD.Print("── FOOD LOOP SELF-TEST (fresh game, panel seams) ──");

                // Boot: the same save/load host wiring _Ready() performs,
                // pointed at an isolated temp directory.
                BuildUserInterface();
                _saveLoadHost = new SaveLoadHostSession();
                _saveLoadHost.Initialize(tempDir);
                AddChild(_saveLoadHost);

                // New Game: the real production entry point.
                StartNewGame();

                // ── Gate 1: starter cooking recipes are known on a fresh game ──
                SetupCooking();
                var cooking = _cooking;
                Check(cooking != null && cooking.CatalogLoaded,
                    "cooking catalog loaded on a fresh game");
                if (cooking != null)
                {
                    var census = cooking.Census;
                    int total = cooking.System.Recipes.Count;
                    GD.Print($"  [Cooking] recipes registered: {total}, discovered: {census.DiscoveredRecipesCount}");
                    Check(total > 0, $"the authored cooking catalog bound at least one starter recipe (got {total})");
                    Check(census.DiscoveredRecipesCount == total,
                        $"fresh game knows every starter cooking recipe (discovered {census.DiscoveredRecipesCount} of {total})");
                    Check(cooking.System.TryGetRecipe("recipe_roasted_meat", out _),
                        "starter recipe 'recipe_roasted_meat' resolves through the cooking authority");
                }

                // ── Gate 2: the kitchen panel is bound and opens ──
                SetupKitchenNutrition();
                Check(_kitchenNutrition != null, "kitchen nutrition session composed");
                Check(_kitchenNutritionPanel != null && _kitchenNutritionPanel.IsBound,
                    "kitchen panel is bound to the live kitchen session");
                if (_kitchenNutritionPanel != null)
                {
                    _kitchenNutritionPanel.Open();
                    Check(_kitchenNutritionPanel.Visible, "kitchen panel opens for the player");
                    _kitchenNutritionPanel.Visible = false;
                }

                // The panel's own resolver — the exact cook selection the
                // START PREP button performs (assigned mess cook, else the
                // player survivor).
                string cookId = _kitchenNutritionPanel?.DefaultSurvivorResolver?.Invoke()
                    ?? _survivors?.RosterState.FirstOrDefault(s => s != null && s.IsAliveState)?.Id
                    ?? "cook_shelter";
                var living = _survivors?.RosterState
                    .Where(s => s != null && s.IsAliveState)
                    .Select(s => s!.Id)
                    .ToList() ?? new List<string>();
                Check(living.Count > 0, "fresh campaign has living crew to cook for");

                // ── Gate 3: cook — the START PREP button's seam ──
                // recipe_fungal_stew is the kitchen panel's first catalog
                // recipe (1x Phosphor Cap Fungi, 1x Clean Water).
                GD.Print($"  [diag] inventory.Survivors bound: {_inventory.Survivors != null}, ApplyNeedOverride bound: {_inventory.ApplyNeedOverride != null}, catalog count: {_inventory.Catalog.Count}");
                GD.Print($"  [diag] storage slots used: {_inventory.Inventory.Slots.Count}/{_inventory.Inventory.Capacity}, weight: {_inventory.Inventory.GetCurrentWeight():0.#}/{_inventory.Inventory.MaxWeight:0.#}");
                // FINDING (reported, not gated): a fresh standard game starts
                // with the 20-slot Holdfast storage already full, so any NEW
                // item type is rejected (weight/capacity). A player cannot
                // bring foraged ingredients home on day 1 without first
                // emptying slots. The harness frees three single-unit starter
                // stacks the way a player would make room.
                _inventory.Remove("item_desal_membrane", 1);
                _inventory.Remove("rad_away", 1);
                _inventory.Remove("item_dosimeter_pen", 1);
                string addMushrooms = _inventory!.Add("crop_biolum_mushroom", 2);
                string addWater = _inventory.Add("clean_water", 2);
                GD.Print($"  [diag] Add(crop_biolum_mushroom,2) -> \"{addMushrooms}\"; Add(clean_water,2) -> \"{addWater}\"");
                int mushroomsBefore = _inventory.Inventory.CountById("crop_biolum_mushroom");
                Check(mushroomsBefore >= 1,
                    $"test ingredients reached the real inventory (crop_biolum_mushroom: {mushroomsBefore})");
                var stewInputs = new Dictionary<string, int>
                {
                    ["crop_biolum_mushroom"] = 1,
                    ["clean_water"] = 1
                };
                var prep = _kitchenNutrition!.StartPrepJob("recipe_fungal_stew", cookId, stewInputs);
                Check(prep.IsSuccess, $"START PREP seam starts the fungal stew job (cook: {cookId}, result: {prep.FailureCode ?? "ok"})");
                Check(_inventory.Inventory.CountById("crop_biolum_mushroom") == mushroomsBefore - 1,
                    "prep job consumed the ingredient bill from the real inventory");

                // ── Gate 4: a real day advance completes the cook ──
                int nextDay = _campaignDay!.Calendar.CurrentDay + 1;
                TickSimDay(nextDay);
                Check(_campaignDay.Calendar.CurrentDay == nextDay,
                    $"real day advance through the coordinator reached day {nextDay}");
                int portions = _kitchenNutrition.System.GetAvailablePortions("recipe_fungal_stew");
                Check(portions >= living.Count,
                    $"day advance completed the prep job into pantry portions ({portions} available for {living.Count} crew)");

                // ── Gate 5: eat/drink — the holdfast terminal's seam ──
                // Runs before SERVE ALL so the eater still has nonzero
                // hunger for canned_food to reduce (needs floor at 0).
                string eater = living[0];
                float hungerA = _survivors!.Needs.Get(eater)?.Hunger ?? 0f;
                float thirstA = _survivors.Needs.Get(eater)?.Thirst ?? 0f;
                int cannedA = _inventory.Inventory.CountById("canned_food");
                var eat = _inventory.ConsumeResult("canned_food", eater);
                float hungerB = _survivors.Needs.Get(eater)?.Hunger ?? 0f;
                Check(eat.IsSuccess && hungerB < hungerA && _inventory.Inventory.CountById("canned_food") == cannedA - 1,
                    $"eating canned_food through the consume seam reduced {eater}'s hunger ({hungerA:0.#} -> {hungerB:0.#}, result: {eat.MessageKey})");
                int waterA = _inventory.Inventory.CountById("clean_water");
                var drink = _inventory.ConsumeResult("clean_water", eater);
                float thirstB = _survivors.Needs.Get(eater)?.Thirst ?? 0f;
                Check(drink.IsSuccess && thirstB < thirstA && _inventory.Inventory.CountById("clean_water") == waterA - 1,
                    $"drinking clean_water through the consume seam reduced {eater}'s thirst ({thirstA:0.#} -> {thirstB:0.#}, result: {drink.MessageKey})");

                // ── Gate 6: eat — the SERVE ALL button's seam ──
                var hungerBefore = new Dictionary<string, float>();
                foreach (var id in living)
                    hungerBefore[id] = _survivors!.Needs.Get(id)?.Hunger ?? 0f;
                var serveAll = _kitchenNutrition.ServeAllMeals(living, "recipe_fungal_stew");
                Check(serveAll.IsSuccess,
                    $"SERVE ALL seam served every living crew member ({living.Count} meals)");
                bool allFed = living.Count > 0;
                foreach (var id in living)
                {
                    float after = _survivors!.Needs.Get(id)?.Hunger ?? 0f;
                    // A survivor just fed by the consume seam may already sit
                    // at the 0 floor; the gate requires no increase and at
                    // least one survivor strictly fed.
                    if (after > hungerBefore[id]) allFed = false;
                }
                Check(allFed && living.Any(id => (_survivors!.Needs.Get(id)?.Hunger ?? 0f) < hungerBefore[id]),
                    "every served survivor's hunger did not increase and at least one was strictly fed");

                // ── Gate 7: the Plan 136 cooking authority is live too ──
                string addMeat = _inventory.Add("raw_meat", 1);
                GD.Print($"  [diag] Add(raw_meat,1) -> \"{addMeat}\"");
                var startCook = cooking!.StartCooking("recipe_roasted_meat", cookId, "improvised_stove");
                Check(startCook.IsSuccess, $"cooking authority starts a roasted-meat operation from the bound inventory (result: {startCook.FailureCode ?? "ok"})");
                int completed = cooking.ProgressCooking(30f);
                int cookedMeat = _inventory.Inventory.CountById("cooked_meat");
                Check(completed == 1 && cookedMeat >= 1,
                    $"cooking operation completed and delivered cooked_meat to the inventory ({cookedMeat} on hand)");

                // ── Gate 8: the kitchen panel cooking strip drives CookingSystem ──
                // PFGL W5 P2 — the panel's strip is the player-operable link
                // for the Plan 136 cooking authority; every verb goes through
                // the same CookingHostSession seam a button press uses.
                Check(_kitchenNutritionPanel != null && _kitchenNutritionPanel.IsCookingBound,
                    "kitchen panel cooking strip is bound to the live cooking authority");
                if (_kitchenNutritionPanel != null)
                {
                    _inventory.Add("raw_meat", 1);
                    _kitchenNutritionPanel.SelectedCookingRecipeId = "recipe_roasted_meat";
                    int rawBeforePanel = _inventory.Inventory.CountById("raw_meat");
                    var panelStart = _kitchenNutritionPanel.StartSelectedCooking(cookId);
                    Check(panelStart.IsSuccess && _inventory.Inventory.CountById("raw_meat") == rawBeforePanel - 1,
                        $"kitchen cooking strip START COOK consumed the authored bill (result: {panelStart.FailureCode ?? "ok"}, feedback: {_kitchenNutritionPanel.LastCookingFeedback})");
                    int panelCompleted = _kitchenNutritionPanel.AdvanceCooking(30f);
                    Check(panelCompleted >= 1,
                        $"kitchen cooking strip ADVANCE completed the batch ({panelCompleted}) — {_kitchenNutritionPanel.LastCookingFeedback}");

                    _inventory.Add("raw_meat", 1);
                    var cancelStart = _kitchenNutritionPanel.StartSelectedCooking(cookId);
                    string? opId = cooking!.System.State.activeOperations
                        .FirstOrDefault(o => o.recipeId == "recipe_roasted_meat")?.operationId;
                    Check(cancelStart.IsSuccess && opId != null,
                        "kitchen cooking strip started a cancellable batch");
                    if (opId != null)
                    {
                        var panelCancel = _kitchenNutritionPanel.CancelCooking(opId);
                        Check(panelCancel.IsSuccess && _kitchenNutritionPanel.ActiveCookingOperationCount == 0,
                            $"kitchen cooking strip CANCEL removed the operation — {_kitchenNutritionPanel.LastCookingFeedback}");
                    }

                    // Refusal path: with the cancelled batch's bill consumed and
                    // no raw meat left, the same start must surface the host's
                    // refusal LastEvent — never the previous success sentence.
                    int leftoverMeat = _inventory.Inventory.CountById("raw_meat");
                    if (leftoverMeat > 0) _inventory.Remove("raw_meat", leftoverMeat);
                    _kitchenNutritionPanel.SelectedCookingRecipeId = "recipe_roasted_meat";
                    var panelRefused = _kitchenNutritionPanel.StartSelectedCooking(cookId);
                    Check(!panelRefused.IsSuccess
                        && _kitchenNutritionPanel.LastCookingFeedback == cooking!.LastEvent
                        && _kitchenNutritionPanel.LastCookingFeedback.Contains("refused", StringComparison.OrdinalIgnoreCase),
                        $"kitchen cooking strip surfaces the host refusal (result: {panelRefused.FailureCode}, feedback: {_kitchenNutritionPanel.LastCookingFeedback})");
                }

                HostCli.EmitSummary("food_loop_selftest", pass, pass ? 0 : 1);
                QuitUiTestAfterFrame(pass ? 0 : 1);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] FoodLoopSelfTest exception: {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}");
                HostCli.EmitSummary("food_loop_selftest", false, 1);
                QuitUiTestAfterFrame(1);
            }
            finally
            {
                try
                {
                    if (Directory.Exists(tempDir))
                        Directory.Delete(tempDir, recursive: true);
                }
                catch
                {
                    // Temp cleanup is best-effort; never let it mask the test result.
                }
            }
        }
    }
}
