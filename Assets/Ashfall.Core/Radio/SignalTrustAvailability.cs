// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Tasks 9–12 Wave 2 — deterministic trust-to-availability weighting
    /// (plan §10D/§10E).
    ///
    /// <para>Trust affects the RELATIVE WEIGHT of future signal candidates
    /// only — never a selected signal's authored authenticity, never an
    /// already-active signal, and never absolutely (a modifier can zero out
    /// no category: both bounds keep ≥ half weight). High trust raises the
    /// relative weight of genuine signals; low trust raises the relative
    /// weight of trap/false-flag signals.</para>
    ///
    /// <para>All math is integer permille — no floating-point drift. The
    /// intended narrative frame is receiver/network credibility (the shelter's
    /// growing radio competence and standing among legitimate callers), not a
    /// global social reputation.</para>
    ///
    /// <para>Current architecture note (Wave 0/2 evidence): the distress
    /// catalog is statically tunable — there is no dynamic candidate-selection
    /// pool in the runtime yet. This API is the documented, tested policy the
    /// future selection consumer (Task 11+ / availability wave) must call; it
    /// is deliberately shipped without inventing a scan mechanic.</para>
    /// </summary>
    public static class SignalTrustAvailability
    {
        public const int MinPermille = 500;   // 0.5× — bounded, never zero
        public const int MaxPermille = 1500;  // 1.5× — bounded, never dominant

        /// <summary>
        /// Weight modifier (permille) for GENUINE signals at the given trust
        /// score. Neutral 50 → 1000‰; 100 → 1500‰; 0 → 500‰. Monotonic.
        /// </summary>
        public static int GenuineModifierPermille(int score)
        {
            int clamped = SignalTrustPolicy.Clamp(score);
            return Math.Clamp(SignalTrustPolicy.NeutralScore * 10 + clamped * 10, MinPermille, MaxPermille);
        }

        /// <summary>
        /// Weight modifier (permille) for TRAP/FALSE-FLAG signals at the given
        /// trust score. Mirror of <see cref="GenuineModifierPermille"/>:
        /// neutral 50 → 1000‰; low trust raises trap weight; high trust lowers
        /// it — but never below half weight, so traps never become guaranteed
        /// or impossible.
        /// </summary>
        public static int TrapModifierPermille(int score)
        {
            int clamped = SignalTrustPolicy.Clamp(score);
            return Math.Clamp(SignalTrustPolicy.NeutralScore * 30 - clamped * 10, MinPermille, MaxPermille);
        }

        /// <summary>Applies the category modifier to a base weight (integer, floored at 0).</summary>
        public static int ApplyModifier(int baseWeight, int modifierPermille)
        {
            if (baseWeight <= 0) return 0;
            long weighted = (long)baseWeight * modifierPermille / 1000;
            return (int)Math.Clamp(weighted, 0, int.MaxValue);
        }

        /// <summary>
        /// One weighted candidate row. <see cref="IsGenuine"/> classifies the
        /// modifier category; authenticity itself is authored catalog truth
        /// and is never changed by trust.
        /// </summary>
        public readonly struct WeightedCandidate
        {
            public string SignalId { get; }
            public int BaseWeight { get; }
            public bool IsGenuine { get; }
            public int FinalWeight { get; }

            public WeightedCandidate(string signalId, int baseWeight, bool isGenuine, int finalWeight)
            {
                SignalId = signalId;
                BaseWeight = baseWeight;
                IsGenuine = isGenuine;
                FinalWeight = finalWeight;
            }
        }

        /// <summary>
        /// Applies trust modifiers to an ALREADY ordered candidate list
        /// (callers sort by stable signal ID first — never hash order) and
        /// returns the weighted rows in the same order. Deterministic,
        /// side-effect free, allocation-light.
        /// </summary>
        public static List<WeightedCandidate> ModifyCandidates(
            IReadOnlyList<(string SignalId, int BaseWeight, bool IsGenuine)> orderedCandidates,
            int trustScore)
        {
            var result = new List<WeightedCandidate>(orderedCandidates?.Count ?? 0);
            if (orderedCandidates == null) return result;
            foreach (var (signalId, baseWeight, isGenuine) in orderedCandidates)
            {
                int modifier = isGenuine ? GenuineModifierPermille(trustScore) : TrapModifierPermille(trustScore);
                result.Add(new WeightedCandidate(signalId, baseWeight, isGenuine, ApplyModifier(baseWeight, modifier)));
            }
            return result;
        }
    }
}
