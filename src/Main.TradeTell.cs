// SPDX-License-Identifier: MIT
// PLAN-TRADE-TELL-TRUTH-248 — market tells host wiring. Derived read model:
// no save section; the corpus reloads from the data authority on setup.

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private TradeTellHostSession? _tradeTells;

        public TradeTellHostSession? TradeTells => _tradeTells;

        public void SetupTradeTells()
        {
            if (_tradeTells != null) return;
            _tradeTells = TradeTellHostSession.Create(_dataDir);
        }

        public void ResetTradeTells()
        {
            _tradeTells = null;
        }

        public Ashfall.Core.Economy.TradeTell? ReadTradeTell(Ashfall.Core.Economy.TradeStance stance, float trust)
        {
            if (_tradeTells == null) return null;
            var rng = _campaignDay != null
                ? _campaignDay.Rng.Fork("trade_tell", _campaignDay.LastAdvancedDay)
                : new Ashfall.Core.SeededRng(248);
            return _tradeTells.SelectTell(stance, trust, rng);
        }
    }
}
