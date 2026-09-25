// SPDX-License-Identifier: MIT
// ============================================================================
// CORE-MECH W2 — Dose storm window (pure, deterministic provider).
//
// Plan: docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
//       (W2-DOSE-STORM-WINDOW, appendices AA.2 / AW.2 / BD.2).
//
// The Year-of-Ash calendar owns the season; the dose ledger owns the reading.
// This provider is the ONLY translation between them: day → exposure
// multiplier. It holds no state beyond the authored rows it is built from,
// draws no rng, and never writes anything. Callers condition the *nominal*
// mSv before handing it to DoseLedgerSystem.BookReading, whose own seeded roll,
// anti-rad timing, and band logic stay untouched (AA.2 receiver contract).
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.YearOfAsh
{
    /// <summary>One day of conditioned exposure, with provenance.</summary>
    public sealed class FalloutWindowDay
    {
        public int Day { get; }
        public string EventId { get; }
        public string HazardType { get; }
        public float Multiplier { get; }

        public FalloutWindowDay(int day, string eventId, string hazardType, float multiplier)
        {
            Day = day;
            EventId = eventId ?? string.Empty;
            HazardType = hazardType ?? string.Empty;
            Multiplier = multiplier;
        }
    }

    /// <summary>
    /// Read-only day → fallout-exposure view over the Year-of-Ash event catalog.
    /// A day is "in window" only when an authored event carries a multiplier
    /// above 1.0. Absent provider, absent row, or an unannotated event all yield
    /// the neutral multiplier 1.0 (fail-closed: the calendar never *reduces*
    /// exposure on its own).
    /// </summary>
    public sealed class FalloutWindowProvider
    {
        public const float NeutralMultiplier = 1.0f;
        public const int DefaultRadiusDays = 1;

        private readonly Dictionary<int, float> _byDay;
        private readonly Dictionary<int, string> _eventIdByDay;
        private readonly Dictionary<int, string> _hazardByDay;
        private readonly int _radius;

        public FalloutWindowProvider(
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
                float mult = e.exposureMultiplier;
                if (float.IsNaN(mult) || float.IsInfinity(mult) || mult <= NeutralMultiplier) continue;
                if (mult > 8f) mult = 8f; // authored sanity clamp; never runaway pressure
                for (int d = e.day - _radius; d <= e.day + _radius; d++)
                {
                    // A stronger authored day wins if windows overlap.
                    if (_byDay.TryGetValue(d, out float existing) && existing >= mult) continue;
                    _byDay[d] = mult;
                    _eventIdByDay[d] = e.id ?? string.Empty;
                    _hazardByDay[d] = e.hazardType ?? string.Empty;
                }
            }
        }

        /// <summary>Neutral provider (no calendar loaded).</summary>
        public static FalloutWindowProvider Empty { get; } = new FalloutWindowProvider(null);

        /// <summary>The exposure multiplier for a day; 1.0 when clear.</summary>
        public float MultiplierFor(int day)
        {
            return _byDay.TryGetValue(day, out float m) ? m : NeutralMultiplier;
        }

        /// <summary>True when the day sits inside an authored exposure window.</summary>
        public bool IsInWindow(int day) => _byDay.ContainsKey(day);

        /// <summary>Provenance for the readout/journal: null when clear.</summary>
        public FalloutWindowDay? WindowFor(int day)
        {
            if (!_byDay.TryGetValue(day, out float m)) return null;
            _eventIdByDay.TryGetValue(day, out string ev);
            _hazardByDay.TryGetValue(day, out string hz);
            return new FalloutWindowDay(day, ev ?? string.Empty, hz ?? string.Empty, m);
        }

        /// <summary>Authored window days, ascending (for surfaces and tests).</summary>
        public IReadOnlyList<int> WindowDays()
        {
            var days = new List<int>(_byDay.Keys);
            days.Sort();
            return days;
        }
    }
}
