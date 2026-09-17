// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Memorial
{
    /// <summary>
    /// Plan 60 / D7 — the production <see cref="IGriefSink"/>: routes a
    /// memorialized death's grief into the single existing relationship
    /// authority (<see cref="SurvivorRelationsSystem"/>), scaled by the
    /// authored <see cref="DeathQuality"/> multiplier.
    ///
    /// This adapter exists because both halves of the grief chain were bound to
    /// nothing: <see cref="MemorialSystem.GriefSink"/> was never assigned in the
    /// host, so <c>GriefSink?.ApplyDispersion(...)</c> silently no-oped, and
    /// <c>SurvivorRelationsSystem.ApplyGrief</c> had no caller outside tests.
    /// Neither a second grief model nor a new morale channel is introduced here —
    /// this is the bridge between two authorities that already exist.
    ///
    /// Determinism: surviving ids are de-duplicated and sorted ordinally before
    /// any mutation, so dictionary or roster iteration order cannot change the
    /// result; the per-survivor amount is a pure function of the inputs.
    /// </summary>
    public sealed class RelationsGriefSink : IGriefSink
    {
        private readonly SurvivorRelationsSystem _relations;
        private readonly Func<string, bool> _isAlive;

        /// <summary>
        /// Ceiling on grief applied per survivor per memorialization, expressed
        /// against the relationship ledger's own 0..100 grief scale. Bounded by
        /// design: a peaceful death must not farm morale and a cascade of deaths
        /// must not instantly max every relationship.
        /// </summary>
        public const float MaxGriefPerSurvivorPerEvent = 20f;

        /// <summary>
        /// Records how many dispersion applications the sink has performed, so a
        /// host test can assert "grief fired exactly once" across a save/reload —
        /// the idempotence property Memorialize already guarantees for the entry.
        /// </summary>
        public int AppliedEventCount { get; private set; }

        /// <summary>Total survivor applications performed (not survivors ×
        /// events): the number a test compares against expected reach.</summary>
        public int AppliedSurvivorCount { get; private set; }

        /// <summary>Plan 24C (A3) — the bond-grief needs window: a mourner's
        /// fatigue/morale rates decay linearly to zero over this many days
        /// from the last loss. Data-authored window length (the plan's
        /// "duration/intensity from existing grief data where available" — the
        /// intensity itself derives from the persisted relationship grief).</summary>
        public const int BondGriefDurationDays = 10;
        /// <summary>Morale points per day at full grief intensity (negative).</summary>
        public const float BondGriefMoralePerDay = -2f;
        /// <summary>Fatigue points per day at full grief intensity.</summary>
        public const float BondGriefFatiguePerDay = 1.5f;

        /// <summary>Plan 24C (A3) — the pure grief-rate function shared by the
        /// host's daily projection and the tests: linear decay to zero across
        /// the window, intensity from the persisted relationship grief (which
        /// absorbed the death-quality scale), expressed per hour. Zero outside
        /// the window — the expiry is derivable from canonical facts alone.</summary>
        public static float BondMoralePerHour(float relationshipGrief, int daysSinceOnset)
        {
            if (daysSinceOnset < 0 || daysSinceOnset >= BondGriefDurationDays) return 0f;
            float intensity = Math.Clamp(relationshipGrief / 100f, 0f, 1f)
                * (1f - (float)daysSinceOnset / BondGriefDurationDays);
            return BondGriefMoralePerDay * intensity / 24f;
        }

        public static float BondFatiguePerHour(float relationshipGrief, int daysSinceOnset)
        {
            if (daysSinceOnset < 0 || daysSinceOnset >= BondGriefDurationDays) return 0f;
            float intensity = Math.Clamp(relationshipGrief / 100f, 0f, 1f)
                * (1f - (float)daysSinceOnset / BondGriefDurationDays);
            return BondGriefFatiguePerDay * intensity / 24f;
        }

        /// <param name="relations">The relationship authority. Null makes the
        /// sink an intentional no-op so a host can run without relationships
        /// without failing the memorial pipeline.</param>
        /// <param name="isAlive">Optional liveness filter; the deceased is always
        /// skipped regardless.</param>
        public RelationsGriefSink(
            SurvivorRelationsSystem relations,
            Func<string, bool> isAlive = null)
        {
            _relations = relations;
            _isAlive = isAlive;
        }

        /// <inheritdoc/>
        public void ApplyDispersion(
            string deceasedId,
            IReadOnlyList<string> survivingRelationshipIds,
            float baseGriefAmount,
            DeathQuality quality,
            int day)
        {
            if (_relations == null) return;
            AppliedEventCount++;

            if (survivingRelationshipIds == null || survivingRelationshipIds.Count == 0)
                return;

            // Memorialize passes the deceased's morale delta as the base amount;
            // grief magnitude is a positive quantity regardless of the sign the
            // morale channel used.
            float baseAmount = Math.Abs(baseGriefAmount);
            if (baseAmount <= 0f) return;

            float amount = baseAmount * CapturingGriefSink.QualityScale(quality);
            if (amount > MaxGriefPerSurvivorPerEvent)
                amount = MaxGriefPerSurvivorPerEvent;

            // Stable order + de-dupe: the ledger must reach the same state for the
            // same inputs on either host and across a reload.
            var ids = new List<string>(survivingRelationshipIds.Count);
            for (int i = 0; i < survivingRelationshipIds.Count; i++)
            {
                string id = survivingRelationshipIds[i];
                if (string.IsNullOrEmpty(id)) continue;
                if (!string.IsNullOrEmpty(deceasedId)
                    && string.Equals(id, deceasedId, StringComparison.Ordinal)) continue;
                if (!ids.Contains(id)) ids.Add(id);
            }
            ids.Sort(StringComparer.Ordinal);

            for (int i = 0; i < ids.Count; i++)
            {
                if (_isAlive != null && !_isAlive(ids[i])) continue;
                _relations.ApplyGrief(ids[i], amount);
                // Plan 24C (A3): stamp the grief onset on the affected pair so
                // the derived grief-to-needs projection (host daily refresh)
                // can decay the mourner's fatigue/morale rates from persisted
                // canonical facts alone — nothing about the needs effect is
                // saved outside the relationship ledger. The latest loss
                // re-anchors the window.
                if (_relations.TryGetRelationship(deceasedId, ids[i], out var rel)
                    && rel != null)
                    rel.grief_since_day = day;
                AppliedSurvivorCount++;
            }
        }
    }
}
