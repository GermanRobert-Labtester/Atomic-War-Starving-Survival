using System;
using System.Collections.Generic;
#pragma warning disable CS8618

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
        /// <summary>Settled contracts (paid in full). The record is never overwritten.</summary>
        public List<DebtContract> closedContracts = new List<DebtContract>();
        public bool ledgerTampered;
    }

    public class LedgerDebtSystem
    {
        public const string SystemId = "ledger_debt_system";
        /// <summary>The contract must be read aloud twice before ink.</summary>
        public const int ReadsRequired = 2;
        /// <summary>
        /// §5.3 "requires a FRESH Standing if contested": a Standing ruling only
        /// authorises a contested renegotiation while it is younger than this
        /// many days. Composed with CrossingArbitrationSystem at the host layer.
        /// </summary>
        public const int StandingFreshDays = 3;

        private LedgerDebtSystemState _state = new LedgerDebtSystemState();

        public event Action<DebtContract> OnContractSigned;
        public event Action<DebtContract> OnContractPaid;
        public event Action<DebtContract> OnContractRenegotiated;
        public event Action<DebtContract> OnForfeitTriggered;
        public event Action OnLedgerTampered;
        public event Action<LedgerDebtSystemState> OnStateChanged;

        public LedgerDebtSystemState State => _state;
        public IReadOnlyList<DebtContract> Contracts => _state.contracts;
        /// <summary>Settled (paid-in-full) contracts. The ink is history; it is never rewritten.</summary>
        public IReadOnlyList<DebtContract> ClosedContracts => _state.closedContracts;
        public bool LedgerTampered => _state.ledgerTampered;

        public DebtContract? GetContract(string debtorId)
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
            if (contract != null && contract.forfeited && !contract.paid)
                return false; // unresolved forfeit — the named good is still owed

            if (contract != null && contract.paid)
            {
                // Settled ink is archived, never overwritten — a second season's
                // debt starts from a fresh draft (read twice, same as everyone).
                _state.contracts.Remove(contract);
                _state.closedContracts.Add(contract);
                contract = null;
            }

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
        /// Renegotiate. On an unsigned draft this tears it up and rewrites it
        /// (the new terms must be read twice again). On signed ink it is only
        /// allowed at term end — the terms extend, the rate adjusts, the forfeit
        /// stays named up front, and the ink stays ink (bible §5.3: "on term
        /// end: … renegotiated"). A CONTESTED renegotiation of signed ink
        /// additionally requires a fresh Standing: the caller supplies a
        /// <paramref name="freshStanding"/> callback (composed with
        /// CrossingArbitrationSystem at the host layer) and the amendment is
        /// refused unless it returns true. The gate lives HERE, in the core —
        /// not in an optional host wrapper, so no host can bypass it.
        /// </summary>
        public bool RenegotiateContract(string debtorId, float newPrincipal, int newTermDays, float newRate, string newForfeit,
            bool contested = false, Func<bool> freshStanding = null!)
        {
            if (string.IsNullOrEmpty(debtorId)) return false;
            if (newPrincipal <= 0f || newTermDays <= 0) return false;
            if (string.IsNullOrEmpty(newForfeit)) return false;

            var contract = GetContract(debtorId);
            if (contract == null) return false;
            if (contract.paid || contract.forfeited) return false;

            if (contract.signed)
            {
                // Term-end renegotiation only — no silent amendment of live ink
                // mid-term. The forfeit is still named up front.
                if (contract.daysRemaining > 1) return false;
                // Contested ink is amended only under a fresh Standing (§5.3).
                if (contested && (freshStanding == null || !freshStanding())) return false;
                contract.principal = newPrincipal;
                contract.termDays = newTermDays;
                contract.rate = newRate;
                contract.forfeit = newForfeit;
                contract.daysRemaining = newTermDays;
            }
            else
            {
                // Draft: tear up and write it again. The new terms must be read
                // twice before they can be signed.
                contract.principal = newPrincipal;
                contract.termDays = newTermDays;
                contract.rate = newRate;
                contract.forfeit = newForfeit;
                contract.readCount = 0;
                contract.signed = false;
                contract.signedDay = -1;
                contract.daysRemaining = 0;
            }

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

        /// <summary>Flat rate, never compounded: principal × (1 + rate). Settled debt owes nothing.</summary>
        public float TotalOwed(string debtorId)
        {
            var c = GetContract(debtorId);
            if (c == null || !c.signed || c.paid) return 0f;
            return c.principal * (1f + c.rate);
        }

        public LedgerDebtSystemState CaptureState() => CopyState(_state);

        public void RestoreState(LedgerDebtSystemState saved)
        {
            // Defensive copy: the restored state must not alias the object the
            // save system handed us (it may be reused or mutated elsewhere).
            _state = CopyState(saved ?? new LedgerDebtSystemState());
            if (string.IsNullOrEmpty(_state.systemId)) _state.systemId = SystemId;
            if (_state.contracts == null) _state.contracts = new List<DebtContract>();
            if (_state.closedContracts == null) _state.closedContracts = new List<DebtContract>();
            RaiseChanged();
        }

        private static DebtContract CopyContract(DebtContract c) => new DebtContract
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
        };

        private static LedgerDebtSystemState CopyState(LedgerDebtSystemState source)
        {
            var copy = new LedgerDebtSystemState
            {
                systemId = source.systemId,
                ledgerTampered = source.ledgerTampered,
                contracts = new List<DebtContract>(),
                closedContracts = new List<DebtContract>()
            };
            for (int i = 0; i < source.contracts.Count; i++)
            {
                var c = source.contracts[i];
                if (c != null) copy.contracts.Add(CopyContract(c));
            }
            for (int i = 0; i < source.closedContracts.Count; i++)
            {
                var c = source.closedContracts[i];
                if (c != null) copy.closedContracts.Add(CopyContract(c));
            }
            return copy;
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
