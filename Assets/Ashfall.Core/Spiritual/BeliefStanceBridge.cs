// SPDX-License-Identifier: MIT
// ============================================================================
// CORE-MECH W9 — Belief → faction stance bridge (pure, bounded, deterministic).
//
// Plan: docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
//       (W9-BELIEF-STANCE-BRIDGE, appendices AA.9 / AV.9 / BD.9).
//
// The gap: survivors' beliefs move through the Zealotry owner, and faction
// standing lives in FactionStanceEngine — and the two had never been connected,
// so interiority was cosmetic. This file is that connection and nothing else:
// a pure translation from an authored belief affinity into a bounded trust
// delta. It NEVER writes standing. The caller hands the result to
// FactionStanceEngine.ModifyTrust, which remains the single write path.
//
// Runaway guard: a conversion wave can fire dozens of events in one day, so the
// bridge enforces a per-(belief, faction, day) budget. Without it, a mass
// conversion would swing trust by hundreds of points in a single tick.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Spiritual;

namespace Ashfall.Core.Spiritual
{
    /// <summary>One standing movement this bridge proposes (not yet applied).</summary>
    public sealed class BeliefStanceShift
    {
        public string BeliefId { get; }
        public string FactionId { get; }
        public float Delta { get; }
        public string Reason { get; }

        public BeliefStanceShift(string beliefId, string factionId, float delta, string reason)
        {
            BeliefId = beliefId ?? string.Empty;
            FactionId = factionId ?? string.Empty;
            Delta = delta;
            Reason = reason ?? string.Empty;
        }
    }

    /// <summary>
    /// Translates belief events into bounded faction-trust proposals.
    /// Construction requires the authored spiritual catalog, so a belief with no
    /// authored affinity can never move standing.
    /// </summary>
    public sealed class BeliefStanceBridge
    {
        /// <summary>Maximum absolute trust a single day of one belief may move.</summary>
        public const float DailyBudgetPerPair = 4f;

        /// <summary>Authored leanings are clamped to this ceiling before weighting.</summary>
        public const float MaxAuthoredLeaning = 6f;

        /// <summary>Faith crisis costs standing, but never more than this per event.</summary>
        public const float CrisisPenalty = 3f;

        /// <summary>Recovered faith (outreach/rites) returns a fraction of the lost ground.</summary>
        public const float RecoveryFraction = 0.5f;

        private readonly Dictionary<string, BeliefMovementDefinition> _beliefs;
        private readonly Dictionary<string, float> _spentToday = new Dictionary<string, float>(StringComparer.Ordinal);
        private int _day = int.MinValue;

        public BeliefStanceBridge(SpiritualCatalog catalog)
        {
            _beliefs = new Dictionary<string, BeliefMovementDefinition>(StringComparer.Ordinal);
            if (catalog?.Movements == null) return;
            foreach (var b in catalog.Movements)
            {
                if (b == null || string.IsNullOrWhiteSpace(b.Id)) continue;
                _beliefs[b.Id] = b; // Ordinal: last wins is fine, ids are unique
            }
        }

        public int AuthoredBeliefCount => _beliefs.Count;

        /// <summary>Roll the per-day budget. Call once at the start of each day.</summary>
        public void BeginDay(int day)
        {
            if (_day == day) return;
            _day = day;
            _spentToday.Clear();
        }

        /// <summary>How much of today's budget this pair has already spent (surface + tests).</summary>
        public float SpentToday(string beliefId, string factionId)
            => _spentToday.TryGetValue(PairKey(beliefId, factionId), out var v) ? v : 0f;

        /// <summary>
        /// A survivor converted to (or deepened) a belief. Conviction (0..100)
        /// scales the authored leaning; a frail conviction barely registers.
        /// Returns proposals in deterministic (factionId ordinal) order.
        /// </summary>
        public IReadOnlyList<BeliefStanceShift> OnBeliefAdhered(
            string beliefId, int conviction, int day, float intensity = 1f)
        {
            BeginDay(day);
            var shifts = new List<BeliefStanceShift>();
            if (!_beliefs.TryGetValue(beliefId ?? string.Empty, out var belief)) return shifts;
            if (belief.FactionLeaning == null || belief.FactionLeaning.Count == 0) return shifts;

            float conviction01 = Clamp01(conviction / 100f);
            float scale = Clamp01(intensity) * (0.35f + 0.65f * conviction01);

            var factions = new List<string>(belief.FactionLeaning.Keys);
            factions.Sort(StringComparer.Ordinal);
            foreach (var factionId in factions)
            {
                float authored = belief.FactionLeaning[factionId];
                float proposed = Clamp(authored, -MaxAuthoredLeaning, MaxAuthoredLeaning) * scale;
                var applied = TakeFromBudget(beliefId, factionId, proposed, day, "adherence");
                if (!NearlyZero(applied)) shifts.Add(new BeliefStanceShift(beliefId, factionId, applied, "adherence"));
            }
            return shifts;
        }

        /// <summary>A shattered faith reads badly to everyone who was counting on it.</summary>
        public IReadOnlyList<BeliefStanceShift> OnFaithCrisis(string beliefId, int conviction, int day)
        {
            BeginDay(day);
            var shifts = new List<BeliefStanceShift>();
            if (!_beliefs.TryGetValue(beliefId ?? string.Empty, out var belief)) return shifts;
            if (belief.FactionLeaning == null || belief.FactionLeaning.Count == 0) return shifts;

            // Factions that leaned positive lose the most; detractors gain slightly.
            float conviction01 = Clamp01(conviction / 100f);
            var factions = new List<string>(belief.FactionLeaning.Keys);
            factions.Sort(StringComparer.Ordinal);
            foreach (var factionId in factions)
            {
                float authored = Clamp(belief.FactionLeaning[factionId], -MaxAuthoredLeaning, MaxAuthoredLeaning);
                float magnitude = Math.Abs(authored) * conviction01;
                float proposed = -Math.Sign(authored) * magnitude; // friends lose, detractors gain
                var applied = TakeFromBudget(beliefId, factionId, proposed, day, "crisis");
                if (!NearlyZero(applied)) shifts.Add(new BeliefStanceShift(beliefId, factionId, applied, "crisis"));
            }
            return shifts;
        }

        /// <summary>
        /// Counterplay: outreach, rites, and policy pull standing back toward
        /// neutral. Returns the recovery proposal for a pair, budget-bounded.
        /// </summary>
        public BeliefStanceShift? OnOutreach(int day, string beliefId, string factionId, float recovery01 = 1f)
        {
            BeginDay(day);
            if (!_beliefs.TryGetValue(beliefId ?? string.Empty, out var belief)) return null;
            if (belief.FactionLeaning == null || !belief.FactionLeaning.ContainsKey(factionId ?? string.Empty)) return null;

            float authored = Clamp(belief.FactionLeaning[factionId ?? string.Empty], -MaxAuthoredLeaning, MaxAuthoredLeaning);
            float proposed = -authored * Clamp01(recovery01) * RecoveryFraction;
            var applied = TakeFromBudget(beliefId, factionId, proposed, day, "outreach");
            return NearlyZero(applied) ? null : new BeliefStanceShift(beliefId, factionId, applied, "outreach");
        }

        /// <summary>Legibility: which factions this belief moves, and how hard.</summary>
        public IReadOnlyList<BeliefStanceShift> DescribeStandingInfluence(string beliefId)
        {
            var list = new List<BeliefStanceShift>();
            if (!_beliefs.TryGetValue(beliefId ?? string.Empty, out var belief) || belief.FactionLeaning == null) return list;
            var factions = new List<string>(belief.FactionLeaning.Keys);
            factions.Sort(StringComparer.Ordinal);
            foreach (var f in factions)
            {
                list.Add(new BeliefStanceShift(beliefId, f, belief.FactionLeaning[f], "authored"));
            }
            return list;
        }

        private float TakeFromBudget(string beliefId, string factionId, float proposed, int day, string reason)
        {
            if (NearlyZero(proposed)) return 0f;
            string key = PairKey(beliefId, factionId);
            float spent = SpentToday(beliefId, factionId);
            float signedBudget = proposed > 0 ? DailyBudgetPerPair : -DailyBudgetPerPair;
            float remaining = signedBudget - spent;
            float applied;
            if (proposed > 0) applied = Math.Max(0f, Math.Min(proposed, remaining));
            else applied = Math.Min(0f, Math.Max(proposed, remaining));
            if (NearlyZero(applied)) return 0f;
            _spentToday[key] = spent + applied;
            return applied;
        }

        private static string PairKey(string beliefId, string factionId) => beliefId + "|" + factionId;
        private static bool NearlyZero(float v) => Math.Abs(v) < 0.0001f;
        private static float Clamp01(float v) => float.IsNaN(v) ? 0f : Math.Clamp(v, 0f, 1f);
        private static float Clamp(float v, float lo, float hi) => float.IsNaN(v) ? 0f : Math.Clamp(v, lo, hi);
    }
}
