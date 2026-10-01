// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Radiation;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — the single place a measured value becomes a UI colour token.
    ///
    /// <para>Loop-1 finding: the three-band rule (Critical / Warm / Pale) was
    /// inlined in <c>GameHudOverlay.ApplyNeedChip</c> while StatusPanel and
    /// SurvivorDetailPanel each re-derived a weaker two-band approximation, so
    /// the same measurement could read amber in one panel and red in another.
    /// Every caller now resolves through this helper, which reads its
    /// thresholds from the owning authorities and never re-types a number.</para>
    ///
    /// <para>Presentation only — no simulation logic, no mutable state, no
    /// engine serialization. Tokens come from <see cref="Ashfall.Core.UI.Theme"/>.</para>
    /// </summary>
    public static class AshfallUiBands
    {
        /// <summary>Nominal (no band crossed) — need surfaces.</summary>
        public static (float r, float g, float b, float a) Nominal => Ashfall.Core.UI.Theme.Pale;

        /// <summary>
        /// Nominal for a <em>dose</em> surface. Deliberately the Lethe
        /// cyan-grey, not <see cref="Nominal"/>: both radiation readings already
        /// shipped in Lethe, and Lethe is the calm-reading token across the
        /// dosimetry surfaces. Loop-3 caught that flattening this to Pale
        /// silently invalidated the game_hud_default golden, so the difference
        /// is now named and gated instead of accidental.
        /// </summary>
        public static (float r, float g, float b, float a) Calm => Ashfall.Core.UI.Theme.Lethe;

        /// <summary>Warn band crossed.</summary>
        public static (float r, float g, float b, float a) Warn => Ashfall.Core.UI.Theme.Warm;

        /// <summary>Critical band crossed.</summary>
        public static (float r, float g, float b, float a) Critical => Ashfall.Core.UI.Theme.Critical;

        /// <summary>
        /// Hardening: a NaN or infinite reading is never nominal. A corrupt
        /// baseline or a divide-by-zero upstream must surface as an alarm, not
        /// as a healthy-looking row.
        /// </summary>
        private static bool IsUnreadable(float value)
            => float.IsNaN(value) || float.IsInfinity(value);

        /// <summary>
        /// Three-band need colouring for a <c>highIsBad</c> need (hunger, thirst,
        /// fatigue). Reads the same rule the HUD need chips use.
        /// </summary>
        /// <param name="warnAt">The warn threshold. Comes before
        /// <paramref name="criticalAt"/> in the signature on purpose: the call
        /// site reads more naturally that way, and loop-1 caught exactly one
        /// call site that had them the other way round and silently killed its
        /// own warn band.</param>
        public static (float r, float g, float b, float a) ForNeed(
            float value, bool highIsBad, float warnAt, float criticalAt)
        {
            if (highIsBad) return ForHigh(value, warnAt, criticalAt);
            return ForLow(value, warnAt, criticalAt);
        }

        private static (float r, float g, float b, float a) ForHigh(
            float value, float warnAt, float criticalAt)
        {
            if (IsUnreadable(value)) return Critical;
            return value >= criticalAt ? Critical
                : value >= warnAt ? Warn
                : Nominal;
        }

        /// <summary>
        /// Three-band colouring for a low-is-bad reading (morale, warmth), where
        /// a small value is the alarming one. The single implementation of the
        /// low-is-bad rule; <see cref="ForNeed"/> delegates here.
        /// </summary>
        public static (float r, float g, float b, float a) ForLow(
            float value, float warnAt, float criticalAt)
        {
            if (IsUnreadable(value)) return Critical;
            return value <= criticalAt ? Critical
                : value <= warnAt ? Warn
                : Nominal;
        }

        /// <summary>
        /// Radiation dose banding, straight off the radiation authority. Shares
        /// <see cref="RadiationSystem.WarnThreshold"/> with the host's
        /// <c>radiation_high</c> toast, so the row and the toast can never
        /// disagree about the band the survivor just crossed.
        /// </summary>
        public static (float r, float g, float b, float a) ForDose(float msv)
        {
            if (IsUnreadable(msv)) return Critical;
            if (msv >= RadiationSystem.AcuteThreshold) return Critical;
            if (msv >= RadiationSystem.WarnThreshold) return Warn;
            return Calm;
        }

        /// <summary>
        /// Loop-3 — the UI tree carries a SECOND band vocabulary: status-rail
        /// metric cards are driven by <see cref="AshfallMetricCard.Criticality"/>
        /// rather than by a colour tuple, and 123 UI files still band those
        /// cards against re-typed literals (fervor 70, catalyst 40, condition
        /// 50, dose rate 10 ...). Folding all of those in is its own package;
        /// this adapter is the sanctioned path so the next wave can adopt the
        /// owning thresholds here instead of re-deriving them per panel.
        ///
        /// <para>Nominal covers both the Pale and Lethe calm tokens: a card has
        /// no way to express the distinction, and inventing a "Caution"-less
        /// mapping would be a new inconsistency.</para>
        /// </summary>
        public static AshfallMetricCard.Criticality ToCriticality(
            (float r, float g, float b, float a) band)
            => band.Equals(Critical) ? AshfallMetricCard.Criticality.Critical
                : band.Equals(Warn) ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Normal;

        /// <summary>Band-first entry point for metric-card call sites.</summary>
        public static AshfallMetricCard.Criticality CriticalityForNeed(
            float value, bool highIsBad, float warnAt, float criticalAt)
            => ToCriticality(ForNeed(value, highIsBad, warnAt, criticalAt));

        /// <summary>Band-first entry point for dose metric cards.</summary>
        public static AshfallMetricCard.Criticality CriticalityForDose(float msv)
            => ToCriticality(ForDose(msv));
    }
}
