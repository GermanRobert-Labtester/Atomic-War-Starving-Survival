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
                AppliedSurvivorCount++;
            }
        }
    }
}
