// SPDX-License-Identifier: MIT
// ============================================================================
// CORE-MECH W3 — Seasonal pressure (pure, deterministic provider).
//
// Plan: docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
//       (W3-WINTER-PRESSURE-POWER-WATER, appendices AA.3 / AW.3 / BD.3).
//
// Companion to W2's FalloutWindowProvider, deliberately NOT the same class and
// deliberately NOT the same column: W2 answers "how much radiation is in the
// air?" (exposureMultiplier), W3 answers "how hard is the season working on
// utilities?" (pressureMultiplier). Both are read-only views over the one
// Year-of-Ash calendar, so there is still exactly one season authority and one
// authored vocabulary — only the semantic differs. Reusing the exposure view for
// utility draw would silently equate a radiological event with a fuel crisis;
// keeping them apart names that difference instead of hiding it (BK.1).
//
// Pure: no state beyond the authored rows, no rng, no engine reference.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.YearOfAsh
{
    /// <summary>One day of authored seasonal pressure, with provenance.</summary>
    public sealed class SeasonalPressureDay
    {
        public int Day { get; }
        public string EventId { get; }
        public string HazardType { get; }
        public float Multiplier { get; }

        public SeasonalPressureDay(int day, string eventId, string hazardType, float multiplier)
        {
            Day = day;
            EventId = eventId ?? string.Empty;
            HazardType = hazardType ?? string.Empty;
            Multiplier = multiplier;
        }
    }

    /// <summary>
    /// Read-only day → seasonal-pressure view for utility consumers (water
    /// treatment first, power per the named decision). Absent provider, absent
    /// row, or an unannotated event all yield the neutral multiplier 1.0 —
    /// winter is pressure, never a gift, and never a silent discount.
    /// </summary>
    public sealed class SeasonalPressureProvider
    {
        public const float NeutralMultiplier = 1.0f;
        public const int DefaultRadiusDays = 1;

        private readonly Dictionary<int, float> _byDay;
        private readonly Dictionary<int, string> _eventIdByDay;
        private readonly Dictionary<int, string> _hazardByDay;
        private readonly int _radius;

        public SeasonalPressureProvider(
            IEnumerable<YearOfAshEventEntry>? events,
            int radiusDays = DefaultRadiusDays)
        {
            _radius = Math.Max(0, radiusDays);
            _byDay = new Dictionary<int, float>();
            _eventIdByDay = new Dictionary<int, string>();
            _hazardByDay = new Dictionary<int, string>();
            if (events == null) return;

            foreach (var e in events)
            {
                if (e == null) continue;
                float mult = e.pressureMultiplier;
                if (float.IsNaN(mult) || float.IsInfinity(mult) || mult <= NeutralMultiplier) continue;
                if (mult > 4f) mult = 4f; // authored sanity clamp for utility load
                for (int d = e.day - _radius; d <= e.day + _radius; d++)
                {
                    if (_byDay.TryGetValue(d, out float existing) && existing >= mult) continue;
                    _byDay[d] = mult;
                    _eventIdByDay[d] = e.id ?? string.Empty;
                    _hazardByDay[d] = e.hazardType ?? string.Empty;
                }
            }
        }

        /// <summary>Neutral provider (no calendar loaded).</summary>
        public static SeasonalPressureProvider Empty { get; } = new SeasonalPressureProvider(null);

        /// <summary>The utility-load multiplier for a day; 1.0 when clear.</summary>
        public float MultiplierFor(int day)
        {
            return _byDay.TryGetValue(day, out float m) ? m : NeutralMultiplier;
        }

        /// <summary>True when the day sits inside an authored pressure window.</summary>
        public bool IsUnderPressure(int day) => _byDay.ContainsKey(day);

        /// <summary>Provenance for the readout/journal; null when clear.</summary>
        public SeasonalPressureDay? PressureFor(int day)
        {
            if (!_byDay.TryGetValue(day, out float m)) return null;
            _eventIdByDay.TryGetValue(day, out string ev);
            _hazardByDay.TryGetValue(day, out string hz);
            return new SeasonalPressureDay(day, ev ?? string.Empty, hz ?? string.Empty, m);
        }

        /// <summary>Authored pressure days, ascending.</summary>
        public IReadOnlyList<int> PressureDays()
        {
            var days = new List<int>(_byDay.Keys);
            days.Sort();
            return days;
        }
    }
}
