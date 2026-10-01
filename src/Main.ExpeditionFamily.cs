// SPDX-License-Identifier: MIT
// PLAN-EXPEDITION-FAMILY-TRUTH-269 — expedition family host wiring (pure
// evaluators; no save section, no mutable campaign state).

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ExpeditionFamilyHostSession? _expeditionFamily;
        public ExpeditionFamilyHostSession? ExpeditionFamily => _expeditionFamily;

        public void SetupExpeditionFamily()
        {
            if (_expeditionFamily != null) return;
            _expeditionFamily = new ExpeditionFamilyHostSession();
        }

        public void ResetExpeditionFamily()
        {
            _expeditionFamily = null;
            // Task 10 — a campaign reset must not leave the previous run's
            // return ceremony on the expedition panel.
            _expeditionPanel?.ClearReturnSummary();
        }
    }
}
