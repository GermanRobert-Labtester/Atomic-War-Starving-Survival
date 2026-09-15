// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Plan 210 Wave 6 — cross-plan consequence rules for the sanitation
    /// authority. Core owns the policy; the host day owner is a thin adapter:
    /// <list type="bullet">
    /// <item>disease: a bounded daily cholera sweep via the AUTHORED
    /// <c>foul_water_draw</c> exposure source — the disease system keeps
    /// infection ownership (§170.8);</item>
    /// <item>morale: reversible marks on Hazardous hygiene, cleared on
    /// recovery (temporary mess must not become permanent damage);</item>
    /// <item>economy: a small bounded medical shortage shock on crisis —
    /// the market owns prices (never rewritten directly).</item>
    /// </list>
    /// Deterministic pure functions; no RNG, no wall clock.
    /// </summary>
    public static class SanitationConsequenceRules
    {
        /// <summary>Authored cholera exposure source in disease_catalog.json.</summary>
        public const string CholeraSourceId = "foul_water_draw";

        public static bool ShouldRunDailyExposureSweep(HygieneBand shelterBand, bool hasActiveSpill) =>
            shelterBand >= HygieneBand.Poor || hasActiveSpill;

        /// <summary>
        /// Morale mark for the band. Only Hazardous earns a permanent mark —
        /// and it is cleared on recovery to Acceptable (reversible), so a
        /// temporary mess never becomes permanent morale damage.
        /// </summary>
        public const string HazardousMarkId = "mark_sanitation_hazardous";

        public static bool ShouldSetHazardousMark(HygieneBand band) => band == HygieneBand.Hazardous;
        public static bool ShouldClearHazardousMark(HygieneBand band) => band <= HygieneBand.Acceptable;

        /// <summary>
        /// Crisis demand shock: Squalid/Hazardous shelter or an active spill
        /// raises medical demand for two days. Null when hygiene is
        /// acceptable (no shock, no spam).
        /// </summary>
        public const string CrisisShockSourceId = "sanitation_crisis";
        public const float CrisisShockSeverityBp = 1000f;
        public const int CrisisShockDurationDays = 2;

        public static bool ShouldApplyCrisisDemandShock(HygieneBand band, bool hasActiveSpill) =>
            band >= HygieneBand.Squalid || hasActiveSpill;
    }
}
