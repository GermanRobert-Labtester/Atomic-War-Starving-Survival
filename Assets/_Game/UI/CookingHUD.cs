using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class CookingRecipeSnapshot
    {
        public string recipeId;
        public string displayName;
        public int rawFoodCost;
        public float waterCost;
        public float fuelCost;
        public float foodValueYield;
        public float spoilageRiskPercent;
    }

    public class CookingSnapshot
    {
        public int totalCookedMeals;
        public float fuelAvailable;
        public List<CookingRecipeSnapshot> recipes = new List<CookingRecipeSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Bunker Kitchen & Cooking HUD view-model.
    /// Manages ration meal preparation, raw food/meat recipes, water/fuel cost telemetry,
    /// food poisoning spoilage risk, and survivor meal distribution.
    /// </summary>
    public class CookingHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedRecipeIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnCookingChanged;
        public event Action<string> OnCookMealRequested; // (recipeId)

        private Func<CookingSnapshot> _getSnapshot;
        private CookingSnapshot _snapshot;

        public void Bind(Func<CookingSnapshot> getSnapshot)
        {
            _getSnapshot = getSnapshot;
            Refresh();
        }

        public void Open()
        {
            if (IsOpen) return;
            IsOpen = true;
            Refresh();
        }

        public void Close()
        {
            if (!IsOpen) return;
            IsOpen = false;
            Refresh();
        }

        public void Toggle()
        {
            if (IsOpen) Close();
            else Open();
        }

        public bool SelectNextRecipe()
        {
            if (!IsOpen || _snapshot == null || _snapshot.recipes == null || _snapshot.recipes.Count == 0)
                return false;
            SelectedRecipeIndex = (SelectedRecipeIndex + 1) % _snapshot.recipes.Count;
            ReportOutcome("Selected recipe: " + GetSelectedRecipeName());
            return true;
        }

        public bool SelectPreviousRecipe()
        {
            if (!IsOpen || _snapshot == null || _snapshot.recipes == null || _snapshot.recipes.Count == 0)
                return false;
            SelectedRecipeIndex = (SelectedRecipeIndex - 1 + _snapshot.recipes.Count) % _snapshot.recipes.Count;
            ReportOutcome("Selected recipe: " + GetSelectedRecipeName());
            return true;
        }

        public bool RequestCookMeal()
        {
            if (!IsOpen || _snapshot == null || _snapshot.recipes == null || _snapshot.recipes.Count == 0)
            {
                ReportOutcome("No recipe selected for cooking.");
                return false;
            }

            var recipe = GetSelectedRecipe();
            if (recipe == null) return false;

            if (_snapshot != null && _snapshot.fuelAvailable < recipe.fuelCost)
            {
                ReportOutcome("CANNOT COOK: Insufficient stove fuel!");
                return false;
            }

            if (OnCookMealRequested == null)
            {
                ReportOutcome("Kitchen stove link offline.");
                return false;
            }

            OnCookMealRequested.Invoke(recipe.recipeId);
            ReportOutcome("Cooking meal [" + recipe.displayName + "] (Fuel Cost: " + recipe.fuelCost.ToString("0.#") + ")...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No cooking action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnCookingChanged?.Invoke();
        }

        private CookingRecipeSnapshot GetSelectedRecipe()
        {
            if (_snapshot != null && _snapshot.recipes != null && SelectedRecipeIndex >= 0 && SelectedRecipeIndex < _snapshot.recipes.Count)
            {
                return _snapshot.recipes[SelectedRecipeIndex];
            }
            return null;
        }

        private string GetSelectedRecipeName()
        {
            var r = GetSelectedRecipe();
            return r != null ? r.displayName : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("BUNKER KITCHEN & RATION COOKING  [K] close  ·  [Tab] cycle  ·  [C] cook meal");

            if (_snapshot == null)
            {
                sb.Append("\nKitchen stove telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nSTOVE STATS: Total Meals Cooked: ").Append(_snapshot.totalCookedMeals)
              .Append("  ·  Fuel Available: ").Append(_snapshot.fuelAvailable.ToString("0.#")).Append(" units");

            sb.Append("\n\nAVAILABLE RATION RECIPES:");
            if (_snapshot.recipes == null || _snapshot.recipes.Count == 0)
            {
                sb.Append("\n  No cooking recipes available.");
            }
            else
            {
                for (int i = 0; i < _snapshot.recipes.Count; i++)
                {
                    var recipe = _snapshot.recipes[i];
                    if (recipe == null) continue;

                    bool selected = (i == SelectedRecipeIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(recipe.displayName ?? recipe.recipeId)
                      .Append(" — Raw Food: ").Append(recipe.rawFoodCost)
                      .Append(" | Water: ").Append(recipe.waterCost.ToString("0.#")).Append(" L")
                      .Append(" | Fuel: ").Append(recipe.fuelCost.ToString("0.#"))
                      .Append(" | Yield: +").Append(recipe.foodValueYield.ToString("0.#")).Append(" Cal");

                    if (recipe.spoilageRiskPercent > 0f)
                        sb.Append(" [Spoilage Risk: ").Append(recipe.spoilageRiskPercent.ToString("0")).Append("%]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nKITCHEN LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
