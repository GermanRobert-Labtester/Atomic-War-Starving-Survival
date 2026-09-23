// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Difficulty
{
    /// <summary>
    /// EN-01 / UNBLOCK-05: Difficulty-Consequence Weave Read Model.
    /// Pure projection over DifficultyScalarsProvider mapping canonical difficulty scalars
    /// into consequence modifiers for the four world consumer sites:
    /// 1. War stage severity multiplier (scales faction friction / escalation).
    /// 2. Crisis deadline offset days (adjusts deadline urgency).
    /// 3. Shock and rumor weight scaling (amplifies informational disruption).
    /// 4. Monotonicity validation across presets.
    /// Engine-free, headless testable.
    /// </summary>
    public sealed class DifficultyConsequenceWeave
    {
        public DifficultyScalarsProvider Scalars { get; }

        public DifficultyConsequenceWeave(DifficultyScalarsProvider scalars)
        {
            Scalars = scalars ?? throw new ArgumentNullException(nameof(scalars));
        }

        /// <summary>
        /// War stage severity multiplier: scales war chain friction and stage acceleration
        /// based on hostile encounter rate and crisis deadline pressure.
        /// </summary>
        public float WarStageSeverityMultiplier
        {
            get
            {
                // Hostile encounter rate scales base severity; deadline pressure accelerates stage shifts
                float deadlineFactor = Scalars.CrisisDeadlineMult <= 0f ? 1f : 1f / Scalars.CrisisDeadlineMult;
                return Scalars.HostileEncounterMult * deadlineFactor;
            }
        }

        /// <summary>
        /// Computes effective crisis deadline days from base authored days.
        /// </summary>
        public int ComputeCrisisDeadlineDays(int baseDays)
        {
            if (baseDays <= 0) return 1;
            int scaled = (int)Math.Round(baseDays * Scalars.CrisisDeadlineMult);
            return Math.Max(1, scaled);
        }

        /// <summary>
        /// Shock and rumor weight multiplier: scales the chance/frequency of negative
        /// information flow events under higher difficulty.
        /// </summary>
        public float ShockWeightMultiplier
        {
            get
            {
                // Blends market volatility and hostile pressure
                return (Scalars.MarketPriceMult + Scalars.HostileEncounterMult) * 0.5f;
            }
        }

        /// <summary>
        /// Asserts whether preset A is monotonically equal or harsher than preset B across all scalars.
        /// Note that for deadline multiplier, a smaller multiplier means shorter/harsher deadlines.
        /// </summary>
        public static bool IsMonotonicallyHarsherOrEqual(DifficultyScalarsProvider harsher, DifficultyScalarsProvider gentler)
        {
            if (harsher == null || gentler == null) return false;

            return harsher.HungerMult >= gentler.HungerMult
                && harsher.ThirstMult >= gentler.ThirstMult
                && harsher.RadiationMult >= gentler.RadiationMult
                && harsher.DiseaseMult >= gentler.DiseaseMult
                && harsher.HostileEncounterMult >= gentler.HostileEncounterMult
                && harsher.MarketPriceMult >= gentler.MarketPriceMult
                && harsher.EquipmentDecayMult >= gentler.EquipmentDecayMult
                && harsher.CrisisDeadlineMult <= gentler.CrisisDeadlineMult;
        }
    }
}
