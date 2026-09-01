using System;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Stress-driven relapse rule table for
    /// <see cref="ChemicalDependencySystem.ReportStress(string, string, float)"/>.
    /// Engine-agnostic: pure function based on existing constants in
    /// <see cref="ChemicalDependencySystem"/>. No randomness — the host
    /// passes <c>magnitude</c> and <paramref name="kind"/>; the same call
    /// returns the same delta.
    ///
    /// Magnitude scale (suggested host-side contract):
    ///   magnitude ∈ [0, 1]. Above 1 is clamped. Below 0 returns 0.
    ///   0.20 — small stressor (cave-in argument with a bunkmate);
    ///   0.50 — significant stressor (ration cut announced);
    ///   0.80 — major stressor (lost a friend, raid repelled);
    ///   1.00 — extreme stressor (held at gunpoint on scavenging run).
    ///
    /// Per-kind severity mirrors <see cref="ChemicalDependencySystem.KindBaseSeverity"/>:
    ///   Opioid     0.9 — highest relapse pull (physical pain);
    ///   Alcohol    0.7 — strong social pull;
    ///   Stimulant  0.6 — energy / insomnia pull;
    ///   Sedative   0.5 — anxiety pull.
    ///
    /// The delta is intentionally asymmetric: stress INCREASES dependency,
    /// never decreases it (decay is the natural path on TickHours). This
    /// matches Unity-era behaviour: stress is an accelerant, not a remedy.
    /// </summary>
    public static class StressRelapseRules
    {
        /// <summary>
        /// Maximum dependency increase per single
        /// <see cref="ChemicalDependencySystem.ReportStress(string, string, float)"/>
        /// call. Stays well below MaxDependencyLevel so a stress burst alone
        /// can never bump a clean baseline straight to rock-bottom in one call.
        /// </summary>
        public const float MaxDeltaPerCall = 0.15f;

        /// <summary>
        /// Minimum input magnitude that can produce any non-zero delta. Calls
        /// below this threshold return 0 — prevents inflation when hosts
        /// spam-fire cheap signals (e.g. per-tick ambient mood).
        /// </summary>
        public const float MinMeaningfulMagnitude = 0.05f;

        /// <summary>
        /// Compute the deterministic dependency-level delta for a stress call.
        /// Negative or zero magnitudes are clamped to "no effect". Magnitudes
        /// above 1 are clamped to 1. The kind scales the delta through the
        /// kind's base severity.
        /// </summary>
        public static float ComputeDelta(float magnitude, ChemicalDependencyKind kind)
        {
            if (magnitude < MinMeaningfulMagnitude) return 0f;
            if (float.IsNaN(magnitude) || float.IsInfinity(magnitude)) return 0f;
            magnitude = Math.Min(1f, magnitude);

            float severity = ChemicalDependencySystem.KindBaseSeverity.TryGetValue(
                kind, out float s) ? s : 0.5f;

            float delta = magnitude * severity * MaxDeltaPerCall;
            // Round-trip stable across hosts (SaveChecksum is byte-exact).
            return (float)Math.Round(delta, 4, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Cheap predicate for hosts that want to pre-filter calls (e.g. the
        /// moral-branching system only fires on "major" stressors). Returns
        /// true if the magnitude would produce any non-zero delta for the
        /// most sensitive kind (Opioid); conservative callers may pre-filter
        /// before invoking <see cref="ChemicalDependencySystem.ReportStress"/>.
        /// </summary>
        public static bool IsReportable(float magnitude)
        {
            return magnitude >= MinMeaningfulMagnitude
                && !float.IsNaN(magnitude)
                && !float.IsInfinity(magnitude);
        }
    }
}
