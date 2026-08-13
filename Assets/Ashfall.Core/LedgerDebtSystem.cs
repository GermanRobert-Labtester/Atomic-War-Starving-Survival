using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — §5.3 LedgerDebtSystem.
    /// Debt as a document. Read twice. Forfeit named. No hidden clause.
    /// Spec: docs/expansions/expansion_04_nobodys_charter_plan.md §5.3.
    /// Differentiation from the Tally: jurisdiction (local — collection at the
    /// Lockup or hall, never a hatch visit), forfeit grade (a named good, not
    /// death), mobility (the Underwrite will not walk to Allocation 12).
    /// Deliberately simple: no amortisation, no credit score, no compounding
    /// beyond the single rate field.
    /// </summary>
    [Serializable]
    public class DebtContract
    {
        public string debtorId;
        public float principal;
        public int termDays;
        public float rate;
        /// <summary>Named good or service-days — never abstract.</summary>
        public string forfeit;
        /// <summary>Dessa reads it twice. After the second time there is only the ink.</summary>
        public int readCount;
        public bool signed;
        public int signedDay = -1;
        public int daysRemaining;
        public bool paid;
        public bool forfeited;
    }

    [Serializable]
    public class LedgerDebtSystemState
    {
        public string systemId = LedgerDebtSystem.SystemId;
        public List<DebtContract> contracts = new List<DebtContract>();
        public bool ledgerTampered;
    }

    public class LedgerDebtSystem
    {
        public const string SystemId = "ledger_debt_system";
        /// <summary>The contract must be read aloud twice before ink.</summary>
        public const int ReadsRequired = 2;

        private LedgerDebtSystemState _state = new LedgerDebtSystemState();

        public event Action<DebtContract> OnContractSigned;
        public event Action<DebtContract> OnContractPaid;
        public event Action<DebtContract> OnContractRenegotiated;
        public event Action<DebtContract> OnForfeitTriggered;
        public event Action OnLedgerTampered;
        public event Action<LedgerDebtSystemState> OnStateChanged;

        public LedgerDebtSystemState State => _state;
        public IReadOnlyList<DebtContract> Contracts => _state.contracts;
        public bool LedgerTampered => _state.ledgerTampered;

        public DebtContract GetContract(string debtorId)
        {
            if (string.IsNullOrEmpty(debtorId)) return null;
            for (int i = 0; i < _state.contracts.Count; i++)
            {
                var c = _state.contracts[i];
                if (c != null && c.debtorId == debtorId) return c;
            }
            return null;
        }

        /// <summary>
        /// One reading of the contract. Creates the draft on the first reading,
        /// increments on the second. Returns false if the terms are invalid or
        /// the debtor already has ink (signed, unpaid) or an unresolved forfeit.
        /// </summary>
        public bool PresentContract(string debtorId, float principal, int termDays, float rate, string forfeit)
        {
            if (string.IsNullOrEmpty(debtorId)) return false;
            if (principal <= 0f || termDays <= 0) return false;
            if (string.IsNullOrEmpty(forfeit)) return false;

            var contract = GetContract(debtorId);
            if (contract != null && contract.signed && !contract.paid)
                return false; // ink is ink — no new draft over an open debt

            if (contract == null)
            {
                contract = new DebtContract { debtorId = debtorId };
                _state.contracts.Add(contract);
            }
            contract.principal = principal;
            contract.termDays = termDays;
            contract.rate = rate;
            contract.forfeit = forfeit;
            contract.readCount++;
            RaiseChanged();
            return true;
        }

        /// <summary>Sign. Requires two readings. The terms freeze at the moment of ink.</summary>
        public bool SignContract(string debtorId, int day)
        {
            var contract = GetContract(debtorId);
            if (contract == null) return false;
            if (contract.signed) return false;
            if (contract.readCount < ReadsRequired) return false;

            contract.signed = true;
            contract.signedDay = day;
            contract.daysRemaining = contract.termDays;
            OnContractSigned?.Invoke(contract);
            RaiseChanged();
            return true;
        }

        /// <summary>
        /// Daily tick. Signed, unpaid contracts run their term down. At zero the
        /// forfeit is due — named up front, now collectable at the Lockup.
        /// </summary>
        public void TickDaily(int day)
        {
            for (int i = 0; i < _state.contracts.Count; i++)
            {
                var c = _state.contracts[i];
                if (c == null || !c.signed || c.paid || c.forfeited) continue;
                c.daysRemaining--;
                if (c.daysRemaining <= 0)
                {
                    c.forfeited = true;
                    OnForfeitTriggered?.Invoke(c);
                }
            }
            RaiseChanged();
        }

        /// <summary>
        /// Pay in full. Allowed while signed — and still allowed after the
        /// forfeit came due (paying the named good back is the honoured path).
        /// </summary>
        public bool PayContract(string debtorId, int day)
        {
            var contract = GetContract(debtorId);
            if (contract == null || !contract.signed) return false;
            if (contract.paid) return false;

            contract.paid = true;
            contract.forfeited = false;
            OnContractPaid?.Invoke(contract);
            RaiseChanged();
            return true;
        }

        /// <summary>
        /// Tear the draft up and write it again. The new terms must be read
        /// twice before they can be signed — the old ink is not amended, it is
        /// replaced, and only while nothing is owed and no forfeit is pending.
        /// </summary>
        public bool RenegotiateContract(string debtorId, float newPrincipal, int newTermDays, float newRate, string newForfeit)
        {
            if (string.IsNullOrEmpty(debtorId)) return false;
            if (newPrincipal <= 0f || newTermDays <= 0) return false;
            if (string.IsNullOrEmpty(newForfeit)) return false;

            var contract = GetContract(debtorId);
            if (contract == null) return false;
            if (contract.signed && (contract.paid || contract.forfeited)) return false;
            if (contract.signed && contract.daysRemaining > 0) return false; // no silent amendment of live ink

            contract.principal = newPrincipal;
            contract.termDays = newTermDays;
            contract.rate = newRate;
            contract.forfeit = newForfeit;
            contract.readCount = 0;
            contract.signed = false;
            contract.signedDay = -1;
            contract.daysRemaining = 0;
            OnContractRenegotiated?.Invoke(contract);
            RaiseChanged();
            return true;
        }

        /// <summary>Cross something out. One strike on the ledger per playthrough.</summary>
        public bool TamperLedger()
        {
            if (_state.ledgerTampered) return false;
            _state.ledgerTampered = true;
            OnLedgerTampered?.Invoke();
            RaiseChanged();
            return true;
        }

        /// <summary>Flat rate, never compounded: principal × (1 + rate).</summary>
        public float TotalOwed(string debtorId)
        {
            var c = GetContract(debtorId);
            if (c == null || !c.signed) return 0f;
            return c.principal * (1f + c.rate);
        }

        public LedgerDebtSystemState CaptureState()
        {
            var copy = new LedgerDebtSystemState
            {
                systemId = _state.systemId,
                ledgerTampered = _state.ledgerTampered,
                contracts = new List<DebtContract>()
            };
            for (int i = 0; i < _state.contracts.Count; i++)
            {
                var c = _state.contracts[i];
                if (c == null) continue;
                copy.contracts.Add(new DebtContract
                {
                    debtorId = c.debtorId,
                    principal = c.principal,
                    termDays = c.termDays,
                    rate = c.rate,
                    forfeit = c.forfeit,
                    readCount = c.readCount,
                    signed = c.signed,
                    signedDay = c.signedDay,
                    daysRemaining = c.daysRemaining,
                    paid = c.paid,
                    forfeited = c.forfeited
                });
            }
            return copy;
        }

        public void RestoreState(LedgerDebtSystemState saved)
        {
            _state = saved ?? new LedgerDebtSystemState();
            if (string.IsNullOrEmpty(_state.systemId)) _state.systemId = SystemId;
            if (_state.contracts == null) _state.contracts = new List<DebtContract>();
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
