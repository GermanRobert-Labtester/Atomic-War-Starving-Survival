using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class HerbalRecipeSnapshot
    {
        public string recipeId;
        public string displayName;
        public int herbsRequired;
        public string remedyEffectText;
        public float craftingTimeHours;
    }

    public class HerbalismSnapshot
    {
        public int herbsInStock;
        public int remediesBrewed;
        public List<HerbalRecipeSnapshot> recipes = new List<HerbalRecipeSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Wasteland Herbalism & Apothecary HUD view-model.
    /// Monitors irradiated flora harvesting, herbal poultices, radiation detox teas,
    /// apothecary brewing recipes, and remedy inventory management.
    /// </summary>
    public class HerbalismHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedRecipeIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnHerbalismChanged;
        public event Action<string> OnBrewHerbalRemedyRequested; // (recipeId)

        private Func<HerbalismSnapshot> _getSnapshot;
        private HerbalismSnapshot _snapshot;

        public void Bind(Func<HerbalismSnapshot> getSnapshot)
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
            ReportOutcome("Selected herbal recipe: " + GetSelectedRecipeName());
            return true;
        }

        public bool SelectPreviousRecipe()
        {
            if (!IsOpen || _snapshot == null || _snapshot.recipes == null || _snapshot.recipes.Count == 0)
                return false;
            SelectedRecipeIndex = (SelectedRecipeIndex - 1 + _snapshot.recipes.Count) % _snapshot.recipes.Count;
            ReportOutcome("Selected herbal recipe: " + GetSelectedRecipeName());
            return true;
        }

        public bool RequestBrewRemedy()
        {
            if (!IsOpen || _snapshot == null || _snapshot.recipes == null || _snapshot.recipes.Count == 0)
            {
                ReportOutcome("No herbal recipe selected for brewing.");
                return false;
            }

            var recipe = GetSelectedRecipe();
            if (recipe == null) return false;

            if (_snapshot != null && _snapshot.herbsInStock < recipe.herbsRequired)
            {
                ReportOutcome("CANNOT BREW: Insufficient harvested herbs in stock!");
                return false;
            }

            if (OnBrewHerbalRemedyRequested == null)
            {
                ReportOutcome("Apothecary brewing bench link offline.");
                return false;
            }

            OnBrewHerbalRemedyRequested.Invoke(recipe.recipeId);
            ReportOutcome("Brewing herbal remedy [" + recipe.displayName + "] (" + recipe.craftingTimeHours.ToString("0.#") + " hrs)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No herbalism action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnHerbalismChanged?.Invoke();
        }

        private HerbalRecipeSnapshot GetSelectedRecipe()
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
            var sb = new StringBuilder("APOTHECARY & WASTELAND HERBALISM  [H] close  ·  [Tab] cycle  ·  [B] brew remedy");

            if (_snapshot == null)
            {
                sb.Append("\nApothecary telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nAPOTHECARY STATS: Harvested Herbs: ").Append(_snapshot.herbsInStock)
              .Append("  ·  Remedies Brewed: ").Append(_snapshot.remediesBrewed);

            sb.Append("\n\nHERBAL APOTHECARY RECIPES:");
            if (_snapshot.recipes == null || _snapshot.recipes.Count == 0)
            {
                sb.Append("\n  No herbal recipes in apothecary book.");
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
                      .Append(" — Cost: ").Append(recipe.herbsRequired).Append(" herbs")
                      .Append(" | Effect: ").Append(recipe.remedyEffectText ?? "Medicinal")
                      .Append(" | Time: ").Append(recipe.craftingTimeHours.ToString("0.#")).Append(" hrs");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nAPOTHECARY LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
