#nullable enable
// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 136 — Wildlife Trapping → Food Pipeline & Cooking in the running game.
//
// Authority boundary: CookingSystem is the single authority for food
// preparation recipes, active cooking operations, food safety/decontamination,
// and cooking skill metrics. It interacts directly with InventorySystem via
// InventoryCookingSource without shadow stores.
// ============================================================================
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Cooking;
using Ashfall.Core.Random;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private CookingHostSession? _cooking;
        private bool _cookingDirty;

        public CookingHostSession? Cooking => _cooking;

        /// <summary>
        /// Loads authored cooking recipes and restores any persisted state.
        /// Binds inventory directly as the ingredient and meal transfer source.
        /// </summary>
        private CookingHostSession? EnsureCooking()
        {
            if (_cooking != null) return _cooking;
            try
            {
                var rng = _campaignDay?.Rng?.GetStream(CampaignStreamIds.Shelter)?.Rng;
                var system = new CookingSystem(null, rng);
                var session = new CookingHostSession(system);

                session.BindInventory(_inventory);

                if (!session.LoadAuthoredRecipes(_dataDir, new FileSystemIO()))
                {
                    GD.PrintErr($"[Cooking] Failed to load recipes: {string.Join("; ", session.LoadErrors)}");
                }

                session.RestoreState(CookingSaveStore.TryLoad());

                session.System.OnCookingCompletedSeam += op =>
                {
                    _journal?.TryAddRawEntry(
                        "cooking_meal_prepared",
                        $"Prepared {op.outputQuantity}x {op.outputItemId} ({op.foodQuality}). Decontaminated {(1f - op.radiationRemainingFraction) * 100f:0}% radiation.",
                        null!, Math.Max(1, _simDay));
                    _consequenceLedger?.Increment(
                        $"meals_cooked::{op.recipeId}", op.outputQuantity, "cooking", "meal_prepared", Math.Max(1, _simDay));
                    _cookingDirty = true;
                };

                session.System.OnSkillLevelUpSeam += lvl =>
                {
                    _journal?.TryAddRawEntry(
                        "cooking_skill_levelup",
                        $"Shelter cooking skill advanced to level {lvl:0.##}.",
                        null!, Math.Max(1, _simDay));
                    _cookingDirty = true;
                };

                session.System.OnRecipeDiscoveredSeam += recipeId =>
                {
                    _journal?.TryAddRawEntry(
                        "cooking_recipe_discovered",
                        $"Discovered culinary recipe: {recipeId}.",
                        null!, Math.Max(1, _simDay));
                    _cookingDirty = true;
                };

                _cooking = session;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Cooking] cooking system unavailable: {ex.Message}");
                _cooking = null;
            }
            return _cooking;
        }

        private void SetupCooking()
        {
            var session = EnsureCooking();
            if (session == null) return;

            if (_saveLoadHost != null
                && _saveLoadHost.TryGetSectionPayload(CookingSaveStore.SectionName, out string payload))
            {
                RestoreCooking(payload);
            }

            var census = session.Census;
            GD.Print($"[Cooking] {census.DiscoveredRecipesCount} recipes loaded. Total meals: {census.TotalMealsPrepared}, Skill: {census.CookingSkillLevel:0.#}.");
        }

        private void SaveCooking()
        {
            var session = _cooking;
            if (session == null) return;
            try
            {
                string payload = CookingSaveStore.CapturePersisted(session.CaptureState());
                CaptureSection(CookingSaveStore.SectionName, payload);
                _cookingDirty = false;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Cooking] Failed to save cooking section: {ex.Message}");
            }
        }

        private void RestoreCooking(string payload)
        {
            var session = EnsureCooking();
            if (session == null || string.IsNullOrWhiteSpace(payload)) return;
            try
            {
                session.RestoreFromPayload(payload);
                _cookingDirty = false;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Cooking] Failed to restore cooking from payload: {ex.Message}");
            }
        }

        private void FlushCookingIfDirty()
        {
            if (_cookingDirty)
            {
                SaveCooking();
            }
        }

        private void ResetCooking()
        {
            _cooking = null;
            _cookingDirty = false;
        }

        private void TickCooking(int currentDay)
        {
            var session = EnsureCooking();
            if (session == null) return;

            // Daily 120 minutes of cooking duty
            int completed = session.TickDay(currentDay, dayMinutes: 120f);
            if (completed > 0)
            {
                _cookingDirty = true;
            }
        }
    }
}
