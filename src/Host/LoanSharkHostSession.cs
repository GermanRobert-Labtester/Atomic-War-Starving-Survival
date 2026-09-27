// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : LoanSharkHostSession
// Purpose      : PLAN-ECONOMY-LEDGER-TRUTH-96 — host the loan-shark enforcer
//                authority. Owns debt records, escalation, and trade sanctions.
//                Persists under `loan_shark`.
// ============================================================================
using Ashfall.Core.Economy;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class LoanSharkSaveStore
    {
        public const string FileName = "loan_shark_save.json";
        public const string SectionName = "loan_shark";
        private static readonly SaveStore<LoanSharkEnforcerEngineSaveState> s_store =
            SaveStoreHub.Checksummed<LoanSharkEnforcerEngineSaveState>(FileName, nameof(LoanSharkSaveStore));
        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static string TryCapturePersisted(LoanSharkEnforcerEngineSaveState state) => s_store.CaptureBare(state);
        public static LoanSharkEnforcerEngineSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(LoanSharkEnforcerEngineSaveState state) => s_store.TrySave(state);
        public static LoanSharkEnforcerEngineSaveState? TryLoad() => s_store.TryLoad();
    }

    public sealed class LoanSharkHostSession : HostSessionBase
    {
        public LoanSharkEnforcerEngine Engine { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public LoanSharkHostSession()
        {
            Engine = new LoanSharkEnforcerEngine();
            Engine.OnLoanIssued += d => { LastEvent = $"Loan issued: {d.DebtId}."; RaiseStateChanged(); };
            Engine.OnLoanDefaulted += d => { LastEvent = $"Loan defaulted: {d.DebtId}."; RaiseStateChanged(); };
            Engine.OnLoanSettled += d => { LastEvent = $"Loan settled: {d.DebtId}."; RaiseStateChanged(); };
            Engine.OnLoanForgiven += d => { LastEvent = $"Loan forgiven: {d.DebtId}."; RaiseStateChanged(); };
        }

        public int ActiveDebtCount => Engine.ActiveDebts.Count;

        public LoanDebtRecord IssueLoan(string creditorFactionId, string debtorId, int principalChits, int termDays, int currentDay)
        {
            var debt = Engine.IssueLoan(creditorFactionId, debtorId, principalChits, termDays, currentDay: currentDay);
            LastEvent = $"Loan issued: {debt.DebtId}.";
            RaiseStateChanged();
            return debt;
        }

        public FundsResult RepayDebt(string debtId, int amountChits, int currentDay)
        {
            var result = Engine.RepayDebt(debtId, amountChits, currentDay);
            LastEvent = $"Repayment on {debtId}: {result}";
            RaiseStateChanged();
            return result;
        }

        public bool ForgiveDebt(string debtId, int currentDay, string reason = "creditor_forgiven")
        {
            bool ok = Engine.ForgiveDebt(debtId, currentDay, reason);
            if (ok) { LastEvent = $"Loan forgiven: {debtId}."; RaiseStateChanged(); }
            return ok;
        }

        public bool IsTradeSanctioned(string creditorFactionId) => Engine.IsTradeSanctioned(creditorFactionId);
        public void TickDay(int day) { Engine.ProcessDailyTick(day); RaiseStateChanged(); }
        public LoanSharkEnforcerEngineSaveState CaptureState() => Engine.CaptureState();
        public void RestoreState(LoanSharkEnforcerEngineSaveState? state) => Engine.RestoreState(state);
    }
}
