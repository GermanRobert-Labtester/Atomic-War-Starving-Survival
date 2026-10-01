// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// P010 — transient day-over-day needs trend read model.
    ///
    /// <para>Captures one baseline snapshot of every registered survivor's needs
    /// at the start of a day, then answers "how much did this need move since
    /// the baseline?" for the presentation surfaces. It is deliberately
    /// <b>not persisted</b>: after a load there is no prior day to compare
    /// against, so the tracker reports no baseline and panels omit the trend
    /// rather than invent one. It owns no simulation rule and never mutates a
    /// <see cref="SurvivorNeedsState"/>.</para>
    /// </summary>
    public sealed class NeedsDayDeltaTracker
    {
        private static readonly NeedKind[] Tracked =
        {
            NeedKind.Hunger,
            NeedKind.Thirst,
            NeedKind.Fatigue,
            NeedKind.Morale,
            NeedKind.Warmth
        };

        private readonly Dictionary<string, float[]> _baseline = new Dictionary<string, float[]>(StringComparer.Ordinal);

        /// <summary>Day the current baseline was captured for, or -1 when none.</summary>
        public int BaselineDay { get; private set; } = -1;

        /// <summary>True when at least one survivor has a captured baseline.</summary>
        public bool HasBaseline => BaselineDay >= 0 && _baseline.Count > 0;

        /// <summary>
        /// Snapshot the roster's current needs as the start-of-day baseline.
        /// Safe to call repeatedly; a retry re-captures the (restored) pre-day
        /// values, so the tracker never carries a failed attempt's history.
        /// </summary>
        public void Capture(int day, IReadOnlyList<SurvivorNeedsState> roster)
        {
            BaselineDay = day;
            _baseline.Clear();
            if (roster == null) return;
            for (int i = 0; i < roster.Count; i++)
            {
                var survivor = roster[i];
                if (survivor == null || string.IsNullOrEmpty(survivor.Id)) continue;
                _baseline[survivor.Id] = new[]
                {
                    survivor.Hunger,
                    survivor.Thirst,
                    survivor.Fatigue,
                    survivor.Morale,
                    survivor.Warmth
                };
            }
        }

        /// <summary>Drops the baseline (restore / fresh session).</summary>
        public void Clear()
        {
            _baseline.Clear();
            BaselineDay = -1;
        }

        /// <summary>
        /// Delta for one tracked need, or false when no baseline exists for the
        /// survivor or the kind is not tracked. Positive means the need rose.
        /// </summary>
        public bool TryGetDelta(SurvivorNeedsState survivor, NeedKind kind, out float delta)
        {
            delta = 0f;
            if (survivor == null || string.IsNullOrEmpty(survivor.Id)) return false;
            if (!_baseline.TryGetValue(survivor.Id, out var baseline)) return false;

            int index = IndexOf(kind);
            if (index < 0 || index >= baseline.Length) return false;

            float current;
            switch (kind)
            {
                case NeedKind.Hunger: current = survivor.Hunger; break;
                case NeedKind.Thirst: current = survivor.Thirst; break;
                case NeedKind.Fatigue: current = survivor.Fatigue; break;
                case NeedKind.Morale: current = survivor.Morale; break;
                case NeedKind.Warmth: current = survivor.Warmth; break;
                default: return false;
            }

            delta = current - baseline[index];
            // Clamp to the authored 0..100 need range so a corrupt baseline or an
            // unusually long gap can never render an implausible delta value.
            if (delta > 100f) delta = 100f;
            else if (delta < -100f) delta = -100f;
            return true;
        }

        /// <summary>
        /// Days the current baseline spans up to <paramref name="currentDay"/>,
        /// or 0 when no baseline exists. 1 is the normal next-day delta; greater
        /// than 1 means a skipped/offline gap the caller may annotate.
        /// </summary>
        public int DaySpan(int currentDay)
            => BaselineDay < 0 ? 0 : Math.Max(1, currentDay - BaselineDay);

        private static int IndexOf(NeedKind kind)
        {
            for (int i = 0; i < Tracked.Length; i++)
                if (Tracked[i] == kind) return i;
            return -1;
        }
    }
}
