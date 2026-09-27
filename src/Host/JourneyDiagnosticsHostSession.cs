// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : JourneyDiagnosticsHostSession
// Purpose      : PLAN-JOURNEY-CONTEXT-TRUTH-156 — the host-side travel context.
//                Wraps JourneyExecutionContext so route/day/action reads and
//                failure diagnostics are one contract, not per-consumer views.
//                Diagnostics only; no save section.
// ============================================================================
using Ashfall.Core.Journeys;

namespace AtomicWar.GodotApp
{
    public sealed class JourneyDiagnosticsHostSession
    {
        private JourneyExecutionContext? _context;
        public bool Active => _context != null;
        public string JourneyName => _context?.JourneyName ?? string.Empty;
        public int Day => _context?.Day ?? 0;
        public int StepIndex => _context?.StepIndex ?? 0;
        public string CurrentRoute => _context?.CurrentRoute ?? string.Empty;

        public void Begin(string journeyName, ulong seed)
        {
            _context = new JourneyExecutionContext(journeyName, seed);
        }

        public void Navigate(string route, string actionDescription)
        {
            _context?.Navigate(route, actionDescription);
        }

        public void AdvanceDay(int day)
        {
            _context?.AdvanceDay(day);
        }

        public string DescribeFailure(string message) =>
            _context != null ? _context.FormatFailureDiagnostic(message) : "no active journey";

        public string FailureJson(string message) =>
            _context != null ? _context.FormatFailureJson(message) : "{}";

        public void End() { _context = null; }
    }
}
