// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Disease;
using Ashfall.Core.Research;
using Ashfall.Core.Shelter;
using System;
using System.Collections.Generic;
using System.IO;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private FoodPreservationSystem? _foodPreservation64;
        private bool _foodPreservation64Dirty;
        public FoodPreservationSystem? FoodPreservationSystem => _foodPreservation64;

        private void SaveFoodPreservation()
        {
            if (_foodPreservation64 != null)
            {
                CaptureSection(FoodPreservationSaveStore.SectionName,
                    FoodPreservationSaveStore.TryCapturePersisted(_foodPreservation64.CaptureState()));
                _foodPreservation64Dirty = false;
            }
        }

        private void FlushFoodPreservationIfDirty()
        {
            if (_foodPreservation64Dirty)
                SaveFoodPreservation();
        }

        // ── CORE-MECH W1 · foodborne disease bridge ──────────────────────────
        // One translation point from preserved-food spoilage (FoodPreservationSystem
        // owns the spoilage truth) into the disease authority's exposure pipeline
        // (DiseaseSystem owns infection). The probability curve lives in the catalog
        // row `spoiled_preserved_stock`; this method never invents one. Fail-closed:
        // no preservation owner, no tracking, or no catalog row means no exposure
        // attempt and no invented safety claim beyond the journal fact.
        private void OnFoodConsumedForSpoilage(string itemId, int amount, string survivorId)
        {
            var preservation = _foodPreservation64;
            if (preservation == null) return;

            int total = preservation.GetTotalFood(itemId);
            if (total <= 0) return; // item is not preserved stock; no preservation record

            int spoiled = preservation.GetSpoiledFood(itemId);
            float share = FoodborneExposureMath.SpoiledShare(total, spoiled);
            if (share <= 0f) return;

            if (_disease == null) SetupDisease();
            if (_disease == null) return;

            var source = _disease.Catalog.GetExposureSource(FoodborneExposureMath.SpoiledPreservedStockSourceId);
            if (source == null) return; // catalog row missing: fail-closed, no invented row

            float modifier = FoodborneExposureMath.ExposureModifierForShare(share);
            var context = new DiseaseExposureContext
            {
                SurvivorId = survivorId,
                DiseaseId = source.disease_id,
                SourceId = source.source_id,
                ProbabilityModifier = modifier,
                BypassImmunity = false,
                Day = _simDay
            };

            var result = _disease.Engine.TryExpose(context);

            SetupJournal();
            _journal?.TryAddRawEntry(
                $"food_spoilage_exposure_{survivorId}_{_simDay}_{itemId}",
                $"{survivorId} ate {itemId} with {share:P0} spoiled share — " +
                (result.Infected
                    ? $"contracted {result.DiseaseId}."
                    : $"exposure attempt passed ({result.Reason})."),
                null!, _simDay);
        }

    }
}
