// SPDX-License-Identifier: MIT
// PLAN-KNOCK-WHITELIST-TRUTH-155 — orphan door-arrival gate host wiring.
// Authored whitelist is loaded once at bootstrap; no mutable state, no save.

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private KnockWhitelistHostSession? _knockWhitelist;
        public KnockWhitelistHostSession? KnockWhitelist => _knockWhitelist;

        public void SetupKnockWhitelist()
        {
            if (_knockWhitelist != null) return;
            _knockWhitelist = new KnockWhitelistHostSession();
            _knockWhitelist.Load(_dataDir);
        }

        public void ResetKnockWhitelist() { _knockWhitelist = null; }
    }
}
