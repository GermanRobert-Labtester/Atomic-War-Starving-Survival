using System;
using System.Collections.Generic;

namespace Ashfall.Core.Foundry
{
    /// <summary>
    /// Plan B66 — Subterranean Heavy Manufacturing &amp; Metallurgical Smelting.
    /// Expansion partial of the Silent Foundry authority: heavy-recipe batches,
    /// crucible slag accumulation/service, flux preflight, extra refractory
    /// wear from high heat tiers, and a ventilation emission handoff.
    ///
    /// Architecture rules honored:
    /// - No competing production authority — heavy batches ride the existing
    ///   heat stage machine, quality roll and output transaction.
    /// - Ventilation owns air state — this partial only registers emission
    ///   sources via <see cref="VentilationSystem"/>.
    /// - Deterministic: no wall clock, no unseeded randomness.
    /// - Legacy saves: missing metallurgy fields normalize to an idle, clean
    ///   crucible — never an active batch or slagged lining.
    /// </summary>
    public sealed partial class SilentFoundrySystem
    {
        private const string HeavySourceId = "vent_src_silent_foundry_heavy";

        private MetallurgyHeavyCatalog? _metallurgyCatalog;
        private VentilationSystem? _ventilation;

        // -----------------------------------------------------------------
        // Binding
        // -----------------------------------------------------------------

        /// <summary>
        /// Bind the heavy metallurgy catalog and merge its projected products
        /// into the bound production catalog so the standard heat machine can
        /// resolve them. Call after <see cref="BindCatalog"/>.
        /// </summary>
        public void BindMetallurgyCatalog(MetallurgyHeavyCatalog catalog)
        {
            if (catalog == null) return;
            if (_catalog == null) return;
            _metallurgyCatalog = catalog;
            if (catalog.Errors.Count > 0)
            {
                for (int i = 0; i < catalog.Errors.Count; i++)
                    _log.Warn("[SilentFoundry] metallurgy catalog: " + catalog.Errors[i]);
            }

            var projected = new List<FoundryProductEntry>();
            foreach (var recipe in catalog.Recipes)
                projected.Add(recipe.ToProductEntry());
            _catalog.MergeHeavyRecipes(projected);
        }

        /// <summary>
        /// Bind the shelter ventilation authority. While a heavy batch is in
        /// the furnace, an emission source is registered; it is deactivated
        /// when the batch completes, fails or is dumped.
        /// </summary>
        public void BindVentilation(VentilationSystem ventilation)
        {
            if (ventilation == null) return;
            _ventilation = ventilation;

            // Deactivate any stale heavy source when a heat ends, whatever
            // path ended it (completion, failure, incident, burnout).
            OnProductionCompleted += _ => DeactivateHeavySource();
            OnCastFailed += _ => DeactivateHeavySource();
        }

        // -----------------------------------------------------------------
        // Queries
        // -----------------------------------------------------------------

        /// <summary>Normalized slag level (0..100) in the crucible.</summary>
        public float SlagLevel => MathfCompat.Clamp(_state.metallurgySlag, 0f, 100f);

        /// <summary>True while a heavy metallurgy recipe occupies the furnace.</summary>
        public bool IsHeavyBatchActive => !string.IsNullOrEmpty(_state.activeMetallurgyRecipeId);

        public MetallurgyRecipeEntry? ActiveHeavyRecipe
            => _metallurgyCatalog?.GetRecipe(_state.activeMetallurgyRecipeId);

        // -----------------------------------------------------------------
        // Actions
        // -----------------------------------------------------------------

        /// <summary>
        /// Start a heavy metallurgy batch. Preflight is atomic: flux, charge,
        /// fuel and water are all validated before anything is consumed; a
        /// failed start consumes nothing.
        /// </summary>
        public string StartHeavyBatch(string recipeId, int workers, float workerSkill, int day)
        {
            if (!_state.unlocked) return "The Silent Foundry is not unlocked.";
            if (_state.laborDispute == FoundryLaborDispute.StrikeActive)
                return "The strike has shut the charging floor; no heavy batch can start.";
            if (HeatStage != FoundryHeatStage.Idle && HeatStage != FoundryHeatStage.Complete)
                return "A heat is already in progress (" + HeatStage + ").";
            if (IsHeavyBatchActive)
                return "The crucible still holds a heavy batch (" + _state.activeMetallurgyRecipeId + ").";
            if (_metallurgyCatalog == null)
                return "No metallurgy catalog bound.";

            var recipe = _metallurgyCatalog.GetRecipe(recipeId);
            if (recipe == null) return "Unknown heavy recipe: " + recipeId;

            workers = MathfCompat.Clamp(workers, 1, MaxWorkers);

            // Flux preflight (metallurgy-specific; consumed here, once).
            int fluxHeld = 0;
            if (!string.IsNullOrEmpty(recipe.flux_item_id) && recipe.flux_amount > 0)
            {
                fluxHeld = _getCount(recipe.flux_item_id);
                if (fluxHeld < recipe.flux_amount)
                    return "Missing flux " + recipe.flux_item_id + " (need " + recipe.flux_amount
                        + ", have " + fluxHeld + ").";
            }

            // Delegate the charge + fuel + water preflight/consumption to the
            // standard production path (projected product entry).
            string result = StartProduction(recipeId, workers, workerSkill, day);
            if (HeatStage != FoundryHeatStage.ChargeLoaded)
            {
                // StartProduction refused; nothing was consumed. Flux has not
                // been consumed yet either — the start stays atomic.
                return result;
            }

            // Flux commits only after the standard charge succeeded — atomic.
            if (!string.IsNullOrEmpty(recipe.flux_item_id) && recipe.flux_amount > 0)
                _consume(recipe.flux_item_id, recipe.flux_amount);

            _state.activeMetallurgyRecipeId = recipeId;
            RegisterHeavySource(recipe);
            Raise("silent_foundry_heavy_batch_started",
                recipe.display_name + " heavy batch loaded (heat tier " + recipe.process_heat_tier + ")");
            RaiseStateChanged();
            return result;
        }

        /// <summary>
        /// Skim/service the crucible: removes accumulated slag. A maintenance
        /// action — allowed any time the foundry is unlocked.
        /// </summary>
        public string SkimSlag(int day)
        {
            if (!_state.unlocked) return "The Silent Foundry is not unlocked.";
            if (_state.metallurgySlag <= 0f)
                return "The crucible is clean; nothing to skim.";

            float before = _state.metallurgySlag;
            _state.metallurgySlag = Math.Max(0f, _state.metallurgySlag - 40f);
            Raise("silent_foundry_slag_serviced",
                "slag skimmed day " + day + " (" + before.ToString("F0") + " → " + _state.metallurgySlag.ToString("F0") + ")");
            RaiseStateChanged();
            return "Crucible skimmed. Slag " + before.ToString("F0") + " → " + _state.metallurgySlag.ToString("F0") + ".";
        }

        // -----------------------------------------------------------------
        // Daily integration (called from TickDaily after AdvanceHeatStage)
        // -----------------------------------------------------------------

        /// <summary>Accumulate slag while a heavy batch is in the furnace.</summary>
        private void AdvanceMetallurgy(int day)
        {
            if (!IsHeavyBatchActive) return;
            if (HeatStage == FoundryHeatStage.Idle || HeatStage == FoundryHeatStage.Complete) return;

            var recipe = ActiveHeavyRecipe;
            if (recipe == null) return;

            float perDay = recipe.slag_yield / Math.Max(1, recipe.labor_days);
            _state.metallurgySlag = MathfCompat.Clamp(_state.metallurgySlag + perDay, 0f, 100f);
        }

        /// <summary>
        /// Clear the heavy-batch marker after the standard machine resolves the
        /// cast, apply heavy-heat refractory wear, and count the batch. Called
        /// from the production completion path.
        /// </summary>
        private void OnHeavyCastResolved(string productId)
        {
            if (!string.Equals(productId, _state.activeMetallurgyRecipeId, StringComparison.Ordinal))
                return;

            var recipe = ActiveHeavyRecipe;
            _state.activeMetallurgyRecipeId = string.Empty;
            DeactivateHeavySource();
            if (recipe == null) return;

            // High heat tiers are harder on the lining. Deliberate extra wear
            // on top of the standard per-day wear — never a separate health pool.
            float liningWear = 1.5f * recipe.process_heat_tier;
            _state.refractoryLining = Math.Max(0f, _state.refractoryLining - liningWear);
            _state.metallurgyBatchesCompleted++;
        }

        private void RegisterHeavySource(MetallurgyRecipeEntry recipe)
        {
            if (_ventilation == null) return;
            float smoke = 4f + 2f * recipe.process_heat_tier;
            float co = 20f + 10f * recipe.process_heat_tier;
            _ventilation.RegisterSource(new VentilationSource
            {
                sourceId = HeavySourceId,
                roomId = SilentFoundryIds.BlueprintRoomId,
                smokeOutputPerDay = smoke,
                coOutputPerDay = co,
                requiresExhaust = true,
                isActive = true
            });
        }

        private void DeactivateHeavySource()
        {
            if (_ventilation == null) return;
            _ventilation.SetSourceActive(HeavySourceId, false);
        }
    }
}
