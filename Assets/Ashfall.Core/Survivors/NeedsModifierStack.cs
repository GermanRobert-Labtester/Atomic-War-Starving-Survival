// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// A persistent, attributable per-hour contribution to one survivor need.
    /// The stack contains presentation/simulation modifiers only; it is not a
    /// second needs authority and is intentionally not part of save state.
    /// </summary>
    public sealed class NeedsModifierContribution
    {
        public string SurvivorId { get; }
        public string SourceId { get; }
        public NeedKind Need { get; }
        public float DeltaPerHour { get; }
        public int Priority { get; }
        public int StartDay { get; }
        public int EndDay { get; }

        public NeedsModifierContribution(string survivorId, string sourceId,
            NeedKind need, float deltaPerHour, int priority = 0,
            int startDay = -1, int endDay = -1)
        {
            SurvivorId = survivorId ?? string.Empty;
            SourceId = sourceId ?? string.Empty;
            Need = need;
            DeltaPerHour = deltaPerHour;
            Priority = priority;
            StartDay = startDay;
            EndDay = endDay;
        }

        public bool IsActive(int day)
        {
            if (day < 0) return true;
            if (StartDay >= 0 && day < StartDay) return false;
            return EndDay < 0 || day <= EndDay;
        }
    }

    /// <summary>
    /// Deterministic external-needs modifier stack. A (survivor, source, need)
    /// tuple has one contribution, so callers can refresh their contribution
    /// without accumulating duplicate rates across day ticks.
    /// </summary>
    public sealed class NeedsModifierStack
    {
        private readonly Dictionary<string, NeedsModifierContribution> _entries
            = new Dictionary<string, NeedsModifierContribution>(StringComparer.Ordinal);
        private readonly Dictionary<string, NeedsModifierContribution> _recent
            = new Dictionary<string, NeedsModifierContribution>(StringComparer.Ordinal);

        public int Count => _entries.Count;

        public void Set(string survivorId, string sourceId, NeedKind need,
            float deltaPerHour, int priority = 0, int startDay = -1, int endDay = -1)
        {
            RequireId(survivorId, nameof(survivorId));
            RequireId(sourceId, nameof(sourceId));
            if (float.IsNaN(deltaPerHour) || float.IsInfinity(deltaPerHour))
                throw new ArgumentOutOfRangeException(nameof(deltaPerHour));
            if (startDay >= 0 && endDay >= 0 && endDay < startDay)
                throw new ArgumentOutOfRangeException(nameof(endDay));

            string key = MakeKey(survivorId, sourceId, need);
            _entries[key] = new NeedsModifierContribution(
                survivorId, sourceId, need, deltaPerHour, priority, startDay, endDay);
        }

        public bool Remove(string survivorId, string sourceId, NeedKind need)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(sourceId))
                return false;
            return _entries.Remove(MakeKey(survivorId, sourceId, need));
        }

        public int ClearSource(string sourceId)
        {
            if (string.IsNullOrEmpty(sourceId)) return 0;
            var keys = new List<string>();
            foreach (var pair in _entries)
                if (string.Equals(pair.Value.SourceId, sourceId, StringComparison.Ordinal))
                    keys.Add(pair.Key);
            for (int i = 0; i < keys.Count; i++) _entries.Remove(keys[i]);
            return keys.Count;
        }

        public int ClearSurvivor(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return 0;
            var keys = new List<string>();
            foreach (var pair in _entries)
                if (string.Equals(pair.Value.SurvivorId, survivorId, StringComparison.Ordinal))
                    keys.Add(pair.Key);
            for (int i = 0; i < keys.Count; i++) _entries.Remove(keys[i]);
            return keys.Count;
        }

        public void Clear() => _entries.Clear();

        public float GetTotal(string survivorId, NeedKind need)
        {
            float total = 0f;
            foreach (var contribution in GetSorted(survivorId, need))
                total += contribution.DeltaPerHour;
            return total;
        }

        public float AggregateFor(string survivorId, NeedKind need, int day)
        {
            float total = 0f;
            foreach (var contribution in GetActiveModifiers(survivorId, need, day))
                total += contribution.DeltaPerHour;
            return total;
        }

        public IReadOnlyList<NeedsModifierContribution> GetActiveModifiers(
            string survivorId, NeedKind need, int day)
        {
            var result = new List<NeedsModifierContribution>();
            if (string.IsNullOrEmpty(survivorId)) return result;
            foreach (var pair in _entries)
            {
                var contribution = pair.Value;
                if (contribution.Need == need
                    && string.Equals(contribution.SurvivorId, survivorId, StringComparison.Ordinal)
                    && contribution.IsActive(day))
                    result.Add(contribution);
            }
            result.Sort(Compare);
            return result;
        }

        /// <summary>Largest active contributors first; ties are deterministic.</summary>
        public IReadOnlyList<NeedsModifierContribution> GetTopContributors(
            string survivorId, NeedKind need, int day, int limit = 3)
        {
            var result = new List<NeedsModifierContribution>(GetActiveModifiers(survivorId, need, day));
            result.Sort((left, right) =>
            {
                int magnitude = Math.Abs(right.DeltaPerHour).CompareTo(Math.Abs(left.DeltaPerHour));
                return magnitude != 0 ? magnitude : Compare(left, right);
            });
            if (limit < 0) limit = 0;
            if (result.Count > limit) result.RemoveRange(limit, result.Count - limit);
            return result;
        }

        public IReadOnlyList<NeedsModifierContribution> GetForSurvivor(string survivorId)
        {
            var result = new List<NeedsModifierContribution>();
            if (string.IsNullOrEmpty(survivorId)) return result;
            foreach (var pair in _entries)
                if (string.Equals(pair.Value.SurvivorId, survivorId, StringComparison.Ordinal))
                    result.Add(pair.Value);
            result.Sort(Compare);
            return result;
        }

        public IReadOnlyList<NeedsModifierContribution> GetRecentForSurvivor(string survivorId)
        {
            var result = new List<NeedsModifierContribution>();
            if (string.IsNullOrEmpty(survivorId)) return result;
            foreach (var pair in _recent)
                if (string.Equals(pair.Value.SurvivorId, survivorId, StringComparison.Ordinal))
                    result.Add(pair.Value);
            result.Sort(Compare);
            return result;
        }

        public void ClearRecentAttributions() => _recent.Clear();

        internal void RecordApplied(NeedsModifierContribution contribution)
        {
            if (contribution == null || string.IsNullOrEmpty(contribution.SurvivorId)
                || string.IsNullOrEmpty(contribution.SourceId)) return;
            _recent[MakeKey(contribution.SurvivorId, contribution.SourceId, contribution.Need)] = contribution;
        }

        internal void ApplyTo(NeedsSystem needs, SurvivorNeedsState survivor, float gameHours, int day)
        {
            if (gameHours <= 0f || survivor == null || !survivor.IsAliveState) return;
            foreach (var contribution in GetActiveForSurvivor(survivor.Id, day))
            {
                if (contribution.DeltaPerHour == 0f) continue;
                needs.Modify(survivor, contribution.Need,
                    contribution.DeltaPerHour * gameHours);
                needs.NotifyAttributedContribution(survivor, contribution);
            }
        }

        private IReadOnlyList<NeedsModifierContribution> GetActiveForSurvivor(string survivorId, int day)
        {
            var result = new List<NeedsModifierContribution>();
            foreach (var contribution in GetForSurvivor(survivorId))
                if (contribution.IsActive(day)) result.Add(contribution);
            return result;
        }

        private IEnumerable<NeedsModifierContribution> GetSorted(string survivorId, NeedKind need)
        {
            var result = new List<NeedsModifierContribution>();
            if (string.IsNullOrEmpty(survivorId)) return result;
            foreach (var pair in _entries)
            {
                var contribution = pair.Value;
                if (string.Equals(contribution.SurvivorId, survivorId, StringComparison.Ordinal)
                    && contribution.Need == need)
                    result.Add(contribution);
            }
            result.Sort(Compare);
            return result;
        }

        private static int Compare(NeedsModifierContribution left,
            NeedsModifierContribution right)
        {
            int priority = left.Priority.CompareTo(right.Priority);
            if (priority != 0) return priority;
            int source = string.CompareOrdinal(left.SourceId, right.SourceId);
            if (source != 0) return source;
            int survivor = string.CompareOrdinal(left.SurvivorId, right.SurvivorId);
            if (survivor != 0) return survivor;
            return ((int)left.Need).CompareTo((int)right.Need);
        }

        private static string MakeKey(string survivorId, string sourceId, NeedKind need)
            => survivorId + "\u001f" + sourceId + "\u001f" + ((int)need).ToString();

        private static void RequireId(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("A non-empty identifier is required.", name);
        }
    }
}
