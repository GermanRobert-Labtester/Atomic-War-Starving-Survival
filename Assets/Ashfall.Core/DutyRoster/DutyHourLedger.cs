// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Survivors;
#pragma warning disable CS8618

namespace Ashfall.Core.DutyRoster
{
    /// <summary>
    /// One survivor's committed vs. recommended duty hours for the current day.
    /// </summary>
    public sealed class DutyHourSnapshot
    {
        public string SurvivorId { get; }
        public string RoleId { get; }
        /// <summary>The full shift load the survivor signed up for (the held
        /// role's standard maximum hours). This is the load they are actually
        /// working, NOT re-capped by their current fitness.</summary>
        public float CommittedHours { get; }
        /// <summary>The current fitness verdict's recommended maximum for this
        /// survivor on this role (caps by impairment).</summary>
        public float RecommendedHours { get; }
        /// <summary>Committed minus recommended; positive means overwork.</summary>
        public float ExcessHours => CommittedHours - RecommendedHours;
        public bool IsOverworked => ExcessHours > 0.001f;

        public DutyHourSnapshot(string survivorId, string roleId,
            float committedHours, float recommendedHours)
        {
            SurvivorId = survivorId ?? string.Empty;
            RoleId = roleId ?? string.Empty;
            CommittedHours = Math.Max(0f, committedHours);
            RecommendedHours = Math.Max(0f, recommendedHours);
        }
    }

    /// <summary>
    /// Plan 24A/24B (Task A2) — the duty-hour accumulator. A DERIVED,
    /// never-persisted projection owned by the roster/duty authority: it reads
    /// the canonical assignment state (one full-day role per survivor), the
    /// role's data-authored shift load, and the current fitness verdict to
    /// answer "is this survivor carrying more hours than their fitness
    /// recommends?".
    ///
    /// <para>Overwork arises from stale assignments: a survivor assigned while
    /// fit (12h shift) whose fitness receded (impaired verdict recommends 8h)
    /// keeps the assignment — the morning invalidation only vacates
    /// hard-blocked roles — so the committed load exceeds the recommendation.
    /// This ledger makes that gap measurable; it never mutates assignments,
    /// needs, or producer output by itself.</para>
    ///
    /// <para>Determinism: a pure function of the bound inputs — no RNG, no
    /// wall clock, stable survivor ordering (ordinal id sort).</para>
    /// </summary>
    public sealed class DutyHourLedger
    {
        /// <summary>Stable reason id for the overwork state (24A vocabulary pattern).</summary>
        public const string OverworkReasonId = "duty_hours_overwork";
        /// <summary>Fallback shift load when a role has no authored hours.</summary>
        public const float DefaultRoleHours = 12f;

        private readonly Func<string, string> _getRoleOf;
        private readonly Func<string, float> _getRoleCommittedHours;
        private readonly Func<string, string, RoleFitnessVerdict?> _previewRoleFitness;
        private readonly Func<IReadOnlyList<string>> _getAssignedSurvivorIds;

        /// <param name="getRoleOf">survivorId → held role id (or empty).</param>
        /// <param name="getRoleCommittedHours">roleId → the role's standard
        /// maximum-hour shift load (from the data-authored duty-role catalog).</param>
        /// <param name="previewRoleFitness">(survivorId, roleId) → current role
        /// fitness verdict carrying RecommendedMaxHours.</param>
        /// <param name="getAssignedSurvivorIds">ordered roster ids holding a role.</param>
        public DutyHourLedger(
            Func<string, string> getRoleOf,
            Func<string, float> getRoleCommittedHours,
            Func<string, string, RoleFitnessVerdict?> previewRoleFitness,
            Func<IReadOnlyList<string>> getAssignedSurvivorIds)
        {
            _getRoleOf = getRoleOf ?? (_ => string.Empty);
            _getRoleCommittedHours = getRoleCommittedHours ?? (_ => DefaultRoleHours);
            _previewRoleFitness = previewRoleFitness ?? ((_, _) => null);
            _getAssignedSurvivorIds = getAssignedSurvivorIds ?? (() => Array.Empty<string>());
        }

        /// <summary>Hours snapshot for one survivor. Unassigned survivors have
        /// zero committed and zero recommended hours (never overworked).</summary>
        public DutyHourSnapshot For(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId))
                return new DutyHourSnapshot(survivorId, string.Empty, 0f, 0f);

            string role = _getRoleOf(survivorId) ?? string.Empty;
            if (string.IsNullOrEmpty(role))
                return new DutyHourSnapshot(survivorId, string.Empty, 0f, 0f);

            float committed = _getRoleCommittedHours(role);
            if (committed <= 0f) committed = DefaultRoleHours;

            var verdict = _previewRoleFitness(survivorId, role);
            float recommended = verdict?.RecommendedMaxHours ?? committed;
            if (recommended < 0f) recommended = 0f;

            return new DutyHourSnapshot(survivorId, role, committed, recommended);
        }

        /// <summary>All overworked survivors in stable (ordinal id) order —
        /// the projection the daily boundary consumes for needs/yield effects.</summary>
        public IReadOnlyList<DutyHourSnapshot> Overworked()
        {
            var result = new List<DutyHourSnapshot>();
            var ids = _getAssignedSurvivorIds() ?? Array.Empty<string>();
            for (int i = 0; i < ids.Count; i++)
            {
                var snapshot = For(ids[i]);
                if (snapshot.IsOverworked) result.Add(snapshot);
            }
            result.Sort((left, right) =>
                string.CompareOrdinal(left.SurvivorId, right.SurvivorId));
            return result;
        }

        /// <summary>True when the survivor is overworked (single-survivor convenience).</summary>
        public bool IsOverworked(string survivorId) => For(survivorId).IsOverworked;
    }
}
