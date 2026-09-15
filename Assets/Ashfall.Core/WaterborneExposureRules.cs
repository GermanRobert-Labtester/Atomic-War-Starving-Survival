// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Disease;

namespace Ashfall.Core
{
    /// <summary>
    /// B5–B8 expansion (§9.12): unsafe-water exposure rules. Core owns the
    /// policy; the host adapter sweeps survivors through the canonical
    /// <see cref="DiseaseSystem.TryExpose"/> contract — infection ownership
    /// stays with the disease authority (no direct stat writes anywhere).
    ///
    /// The treatment authority's <c>OnPathogenExposure</c> event (a dose
    /// emitted when contaminated water was actually processed/consumed)
    /// maps to the authored waterborne catalog entries: typhoid for ordinary
    /// contaminated draw, dysentery stacking on severe exposure. Deterministic
    /// pure functions; no RNG, no wall clock.
    /// </summary>
    public static class WaterborneExposureRules
    {
        /// <summary>Exposure source id recorded with each context (diagnostic
        /// provenance in the disease engine's history).</summary>
        public const string SourceId = "water_treatment_output";

        /// <summary>Pathogen dose below this CFU-scale value exposes no one
        /// (bounded noise floor — clean output never rolls the dice).</summary>
        public const float DoseFloor = 0.01f;

        /// <summary>Dose at/above which the severe waterborne track
        /// (dysentery) joins typhoid — a contaminated-treatment emergency.</summary>
        public const float SevereDoseThreshold = 2.0f;

        /// <summary>Probability modifier bounds (same shape as the sanitation
        /// authority's pathogen modifier): dose grows exposure probability but
        /// never guarantees infection — DiseaseSystem owns the roll.</summary>
        public const float ProbabilityModifierMin = 0.5f;
        public const float ProbabilityModifierMax = 2.0f;

        public static bool ShouldRunExposureSweep(float pathogenDose) =>
            pathogenDose > DoseFloor;

        /// <summary>Deterministic dose→probability mapping. A zero dose maps
        /// to the floor modifier; the ceiling caps an emergency.</summary>
        public static float ProbabilityModifierFor(float pathogenDose) =>
            Math.Clamp(0.5f + pathogenDose * 0.75f, ProbabilityModifierMin, ProbabilityModifierMax);

        /// <summary>
        /// The authored waterborne diseases this exposure can carry. Typhoid
        /// always accompanies a live pathogen dose; dysentery joins when the
        /// dose is severe (a contaminated-treatment emergency).
        /// </summary>
        public static IReadOnlyList<string> DiseasesFor(float pathogenDose)
        {
            if (!ShouldRunExposureSweep(pathogenDose))
                return Array.Empty<string>();
            return pathogenDose >= SevereDoseThreshold
                ? new[] { DiseaseIds.TyphoidWaterborne, DiseaseIds.Dysentery }
                : new[] { DiseaseIds.TyphoidWaterborne };
        }
    }
}
