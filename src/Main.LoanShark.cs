// SPDX-License-Identifier: MIT
// PLAN-ECONOMY-LEDGER-TRUTH-96 — loan-shark enforcer host wiring.

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private LoanSharkHostSession? _loanShark;
        private bool _loanSharkDirty;

        public LoanSharkHostSession? LoanShark => _loanShark;

        public void SetupLoanShark()
        {
            if (_loanShark != null) return;
            _loanShark = new LoanSharkHostSession();
            var saved = LoanSharkSaveStore.TryLoad();
            if (saved != null) _loanShark.RestoreState(saved);
            _loanShark.StateChanged += () => _loanSharkDirty = true;
        }

        public void SaveLoanShark()
        {
            if (_loanShark == null) return;
            var state = _loanShark.CaptureState();
            if (CaptureSection("loan_shark", LoanSharkSaveStore.TryCapturePersisted(state)))
            {
                _loanSharkDirty = false;
            }
        }

        public void ResetLoanShark()
        {
            _loanShark = null;
            _loanSharkDirty = false;
        }

        public void TickLoanShark(int day)
        {
            _loanShark?.TickDay(day);
        }
    }
}
