// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Factions;

namespace Ashfall.Core.Economy
{
    public enum LoanEscalationStage
    {
        Current = 0,
        Grace = 1,
        Delinquent = 2,
        Defaulted = 3,
        Settled = 4,
        Forgiven = 5
    }

    [Serializable]
    public sealed class LoanDebtRecordSaveState
    {
        public string DebtId { get; set; } = string.Empty;
        public string CreditorFactionId { get; set; } = string.Empty;
        public string DebtorId { get; set; } = string.Empty;
        public int PrincipalChits { get; set; }
        public int CurrentBalanceChits { get; set; }
        public int DailyInterestPermille { get; set; }
        public int IssuedDay { get; set; }
        public int DueDay { get; set; }
        public int GracePeriodDays { get; set; }
        public LoanEscalationStage Stage { get; set; }
        public int LastInterestAccrualDay { get; set; }
        public int TotalInterestAccrued { get; set; }
        public int TotalRepaid { get; set; }
        public bool BountyPlaced { get; set; }
        public string? AssociatedBountyId { get; set; }
        public int EnforcerRaidRiskPermille { get; set; }
        public int LastEscalationDay { get; set; }
    }

    [Serializable]
    public sealed class LoanSharkEnforcerEngineSaveState
    {
        public int schema_version { get; set; } = 1;
        public List<LoanDebtRecordSaveState> Debts { get; set; } = new();
    }

    /// <summary>
    /// Represents an active or closed loan record managed by the loan shark enforcer engine.
    /// </summary>
    public sealed class LoanDebtRecord
    {
        public string DebtId { get; }
        public string CreditorFactionId { get; }
        public string DebtorId { get; }
        public int PrincipalChits { get; }
        public int CurrentBalanceChits { get; set; }
        public int DailyInterestPermille { get; }
        public int IssuedDay { get; }
        public int DueDay { get; }
        public int GracePeriodDays { get; }
        public LoanEscalationStage Stage { get; set; }
        public int LastInterestAccrualDay { get; set; }
        public int TotalInterestAccrued { get; set; }
        public int TotalRepaid { get; set; }
        public bool BountyPlaced { get; set; }
        public string? AssociatedBountyId { get; set; }
        public int EnforcerRaidRiskPermille { get; set; }
        public int LastEscalationDay { get; set; }

        public LoanDebtRecord(
            string debtId,
            string creditorFactionId,
            string debtorId,
            int principalChits,
            int termDays,
            int dailyInterestPermille,
            int gracePeriodDays,
            int issuedDay)
        {
            DebtId = debtId ?? throw new ArgumentNullException(nameof(debtId));
            CreditorFactionId = creditorFactionId ?? throw new ArgumentNullException(nameof(creditorFactionId));
            DebtorId = debtorId ?? throw new ArgumentNullException(nameof(debtorId));
            PrincipalChits = principalChits;
            CurrentBalanceChits = principalChits;
            DailyInterestPermille = dailyInterestPermille;
            IssuedDay = issuedDay;
            DueDay = issuedDay + Math.Max(1, termDays);
            GracePeriodDays = Math.Max(0, gracePeriodDays);
            Stage = LoanEscalationStage.Current;
            LastInterestAccrualDay = issuedDay;
            TotalInterestAccrued = 0;
            TotalRepaid = 0;
            BountyPlaced = false;
            AssociatedBountyId = null;
            EnforcerRaidRiskPermille = 0;
            LastEscalationDay = issuedDay;
        }

        internal LoanDebtRecord(LoanDebtRecordSaveState state)
        {
            DebtId = state.DebtId;
            CreditorFactionId = state.CreditorFactionId;
            DebtorId = state.DebtorId;
            PrincipalChits = state.PrincipalChits;
            CurrentBalanceChits = state.CurrentBalanceChits;
            DailyInterestPermille = state.DailyInterestPermille;
            IssuedDay = state.IssuedDay;
            DueDay = state.DueDay;
            GracePeriodDays = state.GracePeriodDays;
            Stage = state.Stage;
            LastInterestAccrualDay = state.LastInterestAccrualDay;
            TotalInterestAccrued = state.TotalInterestAccrued;
            TotalRepaid = state.TotalRepaid;
            BountyPlaced = state.BountyPlaced;
            AssociatedBountyId = state.AssociatedBountyId;
            EnforcerRaidRiskPermille = state.EnforcerRaidRiskPermille;
            LastEscalationDay = state.LastEscalationDay;
        }

        public LoanDebtRecordSaveState CaptureState()
        {
            return new LoanDebtRecordSaveState
            {
                DebtId = DebtId,
                CreditorFactionId = CreditorFactionId,
                DebtorId = DebtorId,
                PrincipalChits = PrincipalChits,
                CurrentBalanceChits = CurrentBalanceChits,
                DailyInterestPermille = DailyInterestPermille,
                IssuedDay = IssuedDay,
                DueDay = DueDay,
                GracePeriodDays = GracePeriodDays,
                Stage = Stage,
                LastInterestAccrualDay = LastInterestAccrualDay,
                TotalInterestAccrued = TotalInterestAccrued,
                TotalRepaid = TotalRepaid,
                BountyPlaced = BountyPlaced,
                AssociatedBountyId = AssociatedBountyId,
                EnforcerRaidRiskPermille = EnforcerRaidRiskPermille,
                LastEscalationDay = LastEscalationDay
            };
        }
    }

    /// <summary>
    /// XP-04-F6 / UNBLOCK-02 §5.11: Wasteland Loan Shark Debt & Enforcer Escalation Engine.
    /// Pure domain engine governing high-risk wasteland credit, daily compounding interest,
    /// commercial trade sanctions, bounty placement via FactionBountySystem, and enforcer raid risks.
    /// Uses FundsLedger for settlement without parallel debt stores.
    /// </summary>
    public sealed class LoanSharkEnforcerEngine
    {
        public const int DefaultDailyInterestPermille = 20; // 2% per day
        public const int DefaultGracePeriodDays = 3;
        public const int DelinquencyThresholdDays = 5;
        public const int MaxEnforcerRaidRiskPermille = 800; // 80%

        private readonly FundsLedger? _fundsLedger;
        private readonly FactionBountySystem? _bountySystem;
        private readonly LedgerDebtSystem? _debtSystem;
        private readonly List<LoanDebtRecord> _debts = new();

        public IReadOnlyList<LoanDebtRecord> Debts => _debts;
        public IReadOnlyList<LoanDebtRecord> ActiveDebts =>
            _debts.Where(d => d.Stage != LoanEscalationStage.Settled && d.Stage != LoanEscalationStage.Forgiven).ToList();

        public event Action<LoanDebtRecord>? OnLoanIssued;
        public event Action<LoanDebtRecord>? OnLoanGracePeriodEntered;
        public event Action<LoanDebtRecord>? OnLoanDelinquent;
        public event Action<LoanDebtRecord>? OnLoanDefaulted;
        public event Action<LoanDebtRecord, int>? OnBountyPlaced;
        public event Action<LoanDebtRecord>? OnLoanSettled;
        public event Action<LoanDebtRecord>? OnLoanForgiven;
        public event Action<LoanDebtRecord, int>? OnInterestAccrued;

        public LoanSharkEnforcerEngine(
            FundsLedger? fundsLedger = null,
            FactionBountySystem? bountySystem = null,
            LedgerDebtSystem? debtSystem = null)
        {
            _fundsLedger = fundsLedger;
            _bountySystem = bountySystem;
            _debtSystem = debtSystem;
        }

        public LoanDebtRecord IssueLoan(
            string creditorFactionId,
            string debtorId,
            int principalChits,
            int termDays,
            int dailyInterestPermille = DefaultDailyInterestPermille,
            int gracePeriodDays = DefaultGracePeriodDays,
            int currentDay = 1)
        {
            if (string.IsNullOrWhiteSpace(creditorFactionId))
                throw new ArgumentException("CreditorFactionId cannot be empty.", nameof(creditorFactionId));
            if (string.IsNullOrWhiteSpace(debtorId))
                throw new ArgumentException("DebtorId cannot be empty.", nameof(debtorId));
            if (principalChits <= 0)
                throw new ArgumentOutOfRangeException(nameof(principalChits), "Principal must be greater than zero.");
            if (termDays <= 0)
                throw new ArgumentOutOfRangeException(nameof(termDays), "Term days must be greater than zero.");
            if (dailyInterestPermille < 0)
                throw new ArgumentOutOfRangeException(nameof(dailyInterestPermille), "Interest permille cannot be negative.");
            if (gracePeriodDays < 0)
                throw new ArgumentOutOfRangeException(nameof(gracePeriodDays), "Grace period days cannot be negative.");

            string debtId = $"loan_{creditorFactionId}_{debtorId}_{currentDay}_{_debts.Count + 1}";
            var record = new LoanDebtRecord(
                debtId,
                creditorFactionId,
                debtorId,
                principalChits,
                termDays,
                dailyInterestPermille,
                gracePeriodDays,
                currentDay);

            _debts.Add(record);

            // Disburse principal funds if ledger is attached
            if (_fundsLedger != null)
            {
                _fundsLedger.TryCredit(principalChits, "loan_disbursement", debtId, currentDay);
            }

            OnLoanIssued?.Invoke(record);
            return record;
        }

        public LoanDebtRecord? GetDebt(string debtId)
        {
            if (string.IsNullOrWhiteSpace(debtId)) return null;
            return _debts.FirstOrDefault(d => string.Equals(d.DebtId, debtId, StringComparison.OrdinalIgnoreCase));
        }

        public void ProcessDailyTick(int currentDay)
        {
            var active = ActiveDebts;
            for (int i = 0; i < active.Count; i++)
            {
                var debt = active[i];
                if (currentDay <= debt.LastInterestAccrualDay)
                    continue;

                int daysElapsed = currentDay - debt.LastInterestAccrualDay;

                // Accrue daily compounding interest for each elapsed day
                for (int day = 0; day < daysElapsed; day++)
                {
                    if (debt.CurrentBalanceChits <= 0)
                        break;

                    int interest = (int)(((long)debt.CurrentBalanceChits * debt.DailyInterestPermille) / 1000L);
                    if (interest == 0 && debt.DailyInterestPermille > 0 && debt.CurrentBalanceChits > 0)
                    {
                        interest = 1; // 1 chit minimal floor for positive balance and interest rate
                    }

                    // Guard against overflow
                    if ((long)debt.CurrentBalanceChits + interest > int.MaxValue)
                    {
                        debt.CurrentBalanceChits = int.MaxValue;
                    }
                    else
                    {
                        debt.CurrentBalanceChits += interest;
                    }

                    debt.TotalInterestAccrued += interest;
                    OnInterestAccrued?.Invoke(debt, interest);
                }

                debt.LastInterestAccrualDay = currentDay;

                // Evaluate escalation stages
                if (currentDay <= debt.DueDay)
                {
                    debt.Stage = LoanEscalationStage.Current;
                }
                else if (currentDay <= debt.DueDay + debt.GracePeriodDays)
                {
                    if (debt.Stage != LoanEscalationStage.Grace)
                    {
                        debt.Stage = LoanEscalationStage.Grace;
                        debt.LastEscalationDay = currentDay;
                        OnLoanGracePeriodEntered?.Invoke(debt);
                    }
                }
                else if (currentDay <= debt.DueDay + debt.GracePeriodDays + DelinquencyThresholdDays)
                {
                    if (debt.Stage != LoanEscalationStage.Delinquent)
                    {
                        debt.Stage = LoanEscalationStage.Delinquent;
                        debt.LastEscalationDay = currentDay;
                        OnLoanDelinquent?.Invoke(debt);
                    }
                }
                else
                {
                    if (debt.Stage != LoanEscalationStage.Defaulted)
                    {
                        debt.Stage = LoanEscalationStage.Defaulted;
                        debt.LastEscalationDay = currentDay;
                        OnLoanDefaulted?.Invoke(debt);
                    }

                    // Place bounty if not already placed
                    if (!debt.BountyPlaced && _bountySystem != null)
                    {
                        int severityDelta = debt.CurrentBalanceChits > 1000 ? -20 :
                                           debt.CurrentBalanceChits > 500 ? -15 : -10;

                        var bounty = _bountySystem.IssuePatrolBounty(
                            debt.CreditorFactionId,
                            encounterId: "underworld_debt",
                            choiceId: $"loan_shark_default_{debt.DebtId}",
                            authoredStandingDelta: severityDelta,
                            day: currentDay);

                        if (bounty != null)
                        {
                            debt.BountyPlaced = true;
                            debt.AssociatedBountyId = bounty.BountyId;
                            OnBountyPlaced?.Invoke(debt, severityDelta);
                        }
                    }

                    // Calculate enforcer raid risk permille (150 base + 50 per day past default)
                    int daysPastDefault = currentDay - (debt.DueDay + debt.GracePeriodDays + DelinquencyThresholdDays);
                    debt.EnforcerRaidRiskPermille = Math.Min(MaxEnforcerRaidRiskPermille, 150 + (daysPastDefault * 50));
                }
            }
        }

        public FundsResult RepayDebt(string debtId, int amountChits, int currentDay)
        {
            var debt = GetDebt(debtId);
            if (debt == null)
            {
                return FundsResult.Failed(0, "DebtNotFound");
            }

            if (debt.Stage == LoanEscalationStage.Settled || debt.Stage == LoanEscalationStage.Forgiven)
            {
                return FundsResult.Failed(debt.CurrentBalanceChits, "DebtAlreadyClosed");
            }

            if (amountChits <= 0)
            {
                return FundsResult.Failed(debt.CurrentBalanceChits, "AmountMustBePositive");
            }

            int amountToDebit = Math.Min(amountChits, debt.CurrentBalanceChits);

            if (_fundsLedger != null)
            {
                var result = _fundsLedger.TryDebit(amountToDebit, FundsLedger.ReasonDebtSettlement, debtId, currentDay);
                if (!result.Success)
                {
                    return result;
                }
            }

            debt.CurrentBalanceChits -= amountToDebit;
            debt.TotalRepaid += amountToDebit;

            if (debt.CurrentBalanceChits == 0)
            {
                debt.Stage = LoanEscalationStage.Settled;
                debt.EnforcerRaidRiskPermille = 0;
                OnLoanSettled?.Invoke(debt);
            }

            return FundsResult.Succeeded(debt.CurrentBalanceChits + amountToDebit, debt.CurrentBalanceChits, -amountToDebit);
        }

        public bool ForgiveDebt(string debtId, int currentDay, string reason = "creditor_forgiven")
        {
            var debt = GetDebt(debtId);
            if (debt == null) return false;
            if (debt.Stage == LoanEscalationStage.Settled || debt.Stage == LoanEscalationStage.Forgiven)
                return false;

            debt.Stage = LoanEscalationStage.Forgiven;
            debt.EnforcerRaidRiskPermille = 0;
            debt.LastEscalationDay = currentDay;
            OnLoanForgiven?.Invoke(debt);
            return true;
        }

        public bool IsTradeSanctioned(string creditorFactionId)
        {
            if (string.IsNullOrWhiteSpace(creditorFactionId)) return false;
            return _debts.Any(d =>
                string.Equals(d.CreditorFactionId, creditorFactionId, StringComparison.OrdinalIgnoreCase) &&
                (d.Stage == LoanEscalationStage.Delinquent || d.Stage == LoanEscalationStage.Defaulted));
        }

        public bool CheckEnforcerRaidTrigger(string debtId, int rollPermille)
        {
            var debt = GetDebt(debtId);
            if (debt == null || debt.Stage != LoanEscalationStage.Defaulted)
                return false;

            return rollPermille >= 0 && rollPermille < debt.EnforcerRaidRiskPermille;
        }

        public LoanSharkEnforcerEngineSaveState CaptureState()
        {
            var state = new LoanSharkEnforcerEngineSaveState
            {
                schema_version = 1,
                Debts = new List<LoanDebtRecordSaveState>(_debts.Count)
            };

            for (int i = 0; i < _debts.Count; i++)
            {
                state.Debts.Add(_debts[i].CaptureState());
            }

            return state;
        }

        public void RestoreState(LoanSharkEnforcerEngineSaveState? state)
        {
            _debts.Clear();
            if (state?.Debts == null) return;

            for (int i = 0; i < state.Debts.Count; i++)
            {
                var dState = state.Debts[i];
                if (dState != null)
                {
                    _debts.Add(new LoanDebtRecord(dState));
                }
            }
        }
    }
}
