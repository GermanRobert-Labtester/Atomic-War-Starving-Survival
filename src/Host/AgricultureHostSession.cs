// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : AgricultureHostSession (Plan 162)
// Core Systems : AgricultureSystem + NutritionDiversitySystem
// Purpose      : Thin Godot adapter — LastEvent feedback, dirty-flag save
//      flush, and inventory-gated commands. All gameplay math lives in Core;
//      the host only resolves items through the canonical Inventory.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Farming;

namespace AtomicWar.GodotApp
{
    public sealed class AgricultureHostSession : HostSessionBase
    {
        public AgricultureSystem System { get; }
        public NutritionDiversitySystem Nutrition { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public AgricultureHostSession(AgricultureSystem system, NutritionDiversitySystem nutrition)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            Nutrition = nutrition ?? throw new ArgumentNullException(nameof(nutrition));

            System.OnStrainPlanted += (plot, strain, day) =>
            {
                LastEvent = $"Planted strain {strain} on plot {plot + 1}.";
                RaiseStateChanged();
            };
            System.OnPlotInfested += (plot, pest) =>
            {
                LastEvent = $"Plot {plot + 1} infested: {pest}.";
                RaiseStateChanged();
            };
            System.OnPestTreated += (plot, item) =>
            {
                LastEvent = $"Plot {plot + 1} treated with {item}.";
                RaiseStateChanged();
            };
            System.OnMutationDetermined += (plot, outcome, variant) =>
            {
                LastEvent = outcome == AgriMutationOutcome.None
                    ? $"Plot {plot + 1} matured clean."
                    : $"Plot {plot + 1} matured with a mutation ({outcome}).";
                RaiseStateChanged();
            };
            System.OnHarvest += h =>
            {
                LastEvent = $"Harvested {h.finalAmount}x {h.yieldItemId} from plot {h.plotIndex + 1} ({h.qualityTier}).";
                RaiseStateChanged();
            };
            System.OnCompostStarted += (recipe, day) =>
            {
                LastEvent = $"Compost batch started ({recipe}).";
                RaiseStateChanged();
            };
            System.OnCompostCollected += (recipe, count) =>
            {
                LastEvent = $"Compost collected: {count}x ({recipe}).";
                RaiseStateChanged();
            };
            System.OnFirstHarvest += _ =>
            {
                LastEvent = "First shelter-grown harvest. The bench holds something you grew.";
                RaiseStateChanged();
            };
            System.OnBlightNarrative += plot =>
            {
                LastEvent = $"Blight spreading on plot {plot + 1}.";
                RaiseStateChanged();
            };
        }

        public void MarkDirty(string reason)
        {
            LastEvent = reason;
            RaiseStateChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            AgricultureSaveStore.TrySave(new AgricultureCampaignState
            {
                agriculture = System.CaptureState(),
                nutrition = Nutrition.CaptureState()
            });
            base.Save();
        }
    }
}
