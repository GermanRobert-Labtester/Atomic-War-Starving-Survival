// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Expansion 32 — The Wild: wildlife harvest quota host wiring.
// The signed pure WildlifeHarvestQuotaEngine (DEC-86) is the quota/predator/taming
// authority. Populations and extinction flags stay with WildlifeMigrationSystem
// and WildlifeEcosystemSystem; trap sites stay with WildlifeTrappingSystem. This
// host owns only the seasonal harvest ledger.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private WildlifeHarvestHostSession? _wildlifeHarvest;
        private bool _wildlifeHarvestDirty;

        public WildlifeHarvestHostSession? WildlifeHarvest => _wildlifeHarvest;

        public void SetupWildlifeHarvest()
        {
            if (_wildlifeHarvest != null) return;

            var saved = WildlifeHarvestSaveStore.TryLoad();
            _wildlifeHarvest = WildlifeHarvestHostSession.Create(saved);
            _wildlifeHarvest.StateChanged += () => _wildlifeHarvestDirty = true;
        }

        public HarvestQuotaResult EvaluateWildlifeQuota(string speciesId, int populationPermille, int reproductionPermille, int requestedUnits)
        {
            SetupWildlifeHarvest();
            return _wildlifeHarvest?.EvaluateQuota(speciesId, populationPermille, reproductionPermille, requestedUnits)
                ?? WildlifeHarvestQuotaEngine.EvaluateHarvestQuota(populationPermille, reproductionPermille, requestedUnits);
        }

        /// <summary>
        /// Applies a harvest against the seasonal ledger. Population facts are
        /// supplied by the canonical owners; this records only what was taken.
        /// </summary>
        public HarvestQuotaResult HarvestWildlife(string speciesId, int populationPermille, int reproductionPermille, int requestedUnits)
        {
            SetupWildlifeHarvest();
            return _wildlifeHarvest?.ApplyHarvest(speciesId, populationPermille, reproductionPermille, requestedUnits)
                ?? WildlifeHarvestQuotaEngine.EvaluateHarvestQuota(populationPermille, reproductionPermille, requestedUnits);
        }

        public void BeginWildlifeHarvestSeason(int season)
        {
            SetupWildlifeHarvest();
            _wildlifeHarvest?.BeginSeason(season);
        }

        public PredatorConflictPosture EvaluateWildlifePredatorConflict(int predatorPopulationPermille, int proximityMetres, int shelterNoisePermille)
        {
            SetupWildlifeHarvest();
            return _wildlifeHarvest?.EvaluatePredatorConflict(predatorPopulationPermille, proximityMetres, shelterNoisePermille)
                ?? WildlifeHarvestQuotaEngine.EvaluatePredatorConflict(predatorPopulationPermille, proximityMetres, shelterNoisePermille);
        }

        public TamingReadinessResult EvaluateWildlifeTamingReadiness(int animalHungerPermille, int trustExposurePermille, int speciesTamabilityPermille, int tamingSeed)
        {
            SetupWildlifeHarvest();
            return _wildlifeHarvest?.EvaluateTamingReadiness(animalHungerPermille, trustExposurePermille, speciesTamabilityPermille, tamingSeed)
                ?? WildlifeHarvestQuotaEngine.EvaluateTamingReadiness(animalHungerPermille, trustExposurePermille, speciesTamabilityPermille, tamingSeed);
        }

        public WildlifeHarvestCensus GetWildlifeHarvestCensus() => _wildlifeHarvest?.Census ?? default;

        public void SaveWildlifeHarvest()
        {
            if (_wildlifeHarvest == null) return;
            var state = _wildlifeHarvest.CaptureState();
            WildlifeHarvestSaveStore.TrySave(state);
            if (CaptureSection(WildlifeHarvestSaveStore.SectionName, WildlifeHarvestSaveStore.TryCapturePersisted(state)))
                _wildlifeHarvestDirty = false;
        }

        public void FlushWildlifeHarvestIfDirty()
        {
            if (_wildlifeHarvestDirty)
                SaveWildlifeHarvest();
        }

        public void ResetWildlifeHarvest()
        {
            _wildlifeHarvest = null;
            _wildlifeHarvestDirty = false;
        }
    }
}
