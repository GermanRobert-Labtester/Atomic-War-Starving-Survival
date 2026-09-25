// SPDX-License-Identifier: MIT
// ============================================================================
// CORE-MECH W1 — Foodborne disease bridge (pure math).
//
// Plan: docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
//       (W1-FOODBORNE-DISEASE-BRIDGE, appendices AA.1 / AW.1 / BD.1).
//
// FoodPreservationSystem owns spoilage; DiseaseSystem owns infection. This
// file is the ONLY place the two are translated, and it is pure: no state, no
// rng, no Godot reference. The caller passes the authored base probability
// from the disease catalog's exposure_sources row and the spoiled share of the
// food actually eaten; the result is the probability modifier handed to
// DiseaseExposureContext.ProbabilityModifier (DiseaseSystem.TryExpose applies
// base_probability * ProbabilityModifier * difficultyMultiplier, then rolls
// with the disease authority's own seeded rng).
// ============================================================================

using System;

namespace Ashfall.Core.Disease
{
    /// <summary>
    /// Pure translation between preserved-food spoilage and foodborne exposure.
    /// Deterministic: identical inputs always produce identical outputs.
    /// </summary>
    public static class FoodborneExposureMath
    {
        /// <summary>Exposure source id authored for spoiled preserved stock.</summary>
        public const string SpoiledPreservedStockSourceId = "spoiled_preserved_stock";

        /// <summary>
        /// Fraction of a tracked stock that has spoiled (0..1). Returns 0 when the
        /// item is untracked, empty, or not spoiled — an untracked item is treated
        /// as having no preservation record (fail-closed, no exposure).
        /// </summary>
        public static float SpoiledShare(int totalTracked, int spoiledTracked)
        {
            if (totalTracked <= 0) return 0f;
            if (spoiledTracked <= 0) return 0f;
            if (spoiledTracked >= totalTracked) return 1f;
            return (float)spoiledTracked / totalTracked;
        }

        /// <summary>
        /// Effective exposure probability for one eating event, before the
        /// disease authority applies its difficulty multiplier and rolls.
        /// Clamped to [0, 1]; NaN/inverted inputs are neutralized.
        /// </summary>
        public static float EffectiveProbability(float baseProbability, float spoiledShare)
        {
            if (float.IsNaN(baseProbability) || baseProbability <= 0f) return 0f;
            if (float.IsNaN(spoiledShare) || spoiledShare <= 0f) return 0f;
            float value = baseProbability * Math.Min(1f, spoiledShare);
            if (value > 1f) value = 1f;
            return value;
        }

        /// <summary>
        /// The probability modifier to place on <see cref="DiseaseExposureContext.ProbabilityModifier"/>
        /// for a spoiled-share meal: the spoiled share itself, so the catalog's
        /// base_probability remains the authority for the disease risk curve.
        /// Returns 0 when there is nothing spoiled (no exposure attempted).
        /// </summary>
        public static float ExposureModifierForShare(float spoiledShare)
        {
            if (float.IsNaN(spoiledShare) || spoiledShare <= 0f) return 0f;
            return Math.Min(1f, spoiledShare);
        }
    }
}
