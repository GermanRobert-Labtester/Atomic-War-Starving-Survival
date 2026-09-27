// SPDX-License-Identifier: MIT
// PLAN-JOURNEY-CONTEXT-TRUTH-156 — host journey diagnostics context. Pure
// derived diagnostics for CI triage; no save section.

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private JourneyDiagnosticsHostSession? _journeyDiagnostics;
        public JourneyDiagnosticsHostSession? JourneyDiagnostics => _journeyDiagnostics;

        public void SetupJourneyDiagnostics()
        {
            if (_journeyDiagnostics != null) return;
            _journeyDiagnostics = new JourneyDiagnosticsHostSession();
        }

        public void ResetJourneyDiagnostics() { _journeyDiagnostics = null; }
    }
}
