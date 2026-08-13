using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — §5.3 LedgerDebtSystem.
    /// Debt as a document. Read twice. Forfeit named. No hidden clause.
    ///
    /// Differentiation from the Tally: jurisdiction (local), forfeit grade
    /// (goods, not death), mobility (will not walk to Allocation 12).
    /// Collection happens at the Lockup or hall — never at the hatch.
    ///
    /// Plain C#, event-driven, save/load safe.
    /// </summary>

    // ── Data types ─────────────────────────────────────────────────────

    [Serializable]
    public class DebtContract
    {
        public string contractId;
        public string debtorId;     // who owes
        public string creditorId;   // who is owed (typically npc_dessa_vane / faction_the_underwrite)
        public string principal;    // the goods or service described in plain language
        public int termDays;        // days until forfeit triggers
        public float rate;          // interest or service rate (simple, one-field)
        public string forfeit;      // named good or service-days — never abstract
        public int daySigned;
        public bool isPaid;
        public bool forfeitTriggered;
        public bool renegotiated;
    }

    [Serializable]
    public class LedgerDebtState
    {
        public string systemId = LedgerDebtSystem.SystemId;
        public List<DebtContract> contracts = new List<DebtContract>();
        public int contractsSigned;
        public int contractsPaid;
        public int forfeitsTriggered;
        public int ledgerTamperAttempts;
    }

    // ── System ─────────────────────────────────────────────────────────

    public class LedgerDebtSystem
    {
        public const string SystemId = "ledger_debt_system";

        private LedgerDebtState _state = new LedgerDebtState();

        public event Action<DebtContract> OnContractSigned;
        public event Action<DebtContract> OnContractPaid;
        public event Action<DebtContract> OnContractRenegotiated;
        public event Action<DebtContract> OnForfeitTriggered;
        public event Action<string> OnLedgerTampered; // contractId

        public LedgerDebtState State => _state;
        public IReadOnlyList<DebtContract> Contracts => _state.contracts;

        // ── Queries ────────────────────────────────────────────────────

        public DebtContract GetContract(string contractId)
        {
            if (string.IsNullOrEmpty(contractId)) return null;
            for (int i = 0; i < _state.contracts.Count; i++)
            {
                var c = _state.contracts[i];
                if (c != null && c.contractId == contractId) return c;
            }
            return null;
        }

        public List<DebtContract> GetContractsForDebtor(string debtorId)
        {
            var result = new List<DebtContract>();
            if (string.IsNullOrEmpty(debtorId)) return result;
            for (int i = 0; i < _state.contracts.Count; i++)
            {
                var c = _state.contracts[i];
                if (c != null && c.debtorId == debtorId) result.Add(c);
            }
            return result;
        }

        public List<DebtContract> GetOutstandingContracts()
        {
            var result = new List<DebtContract>();
            for (int i = 0; i < _state.contracts.Count; i++)
            {
                var c = _state.contracts[i];
                if (c != null && !c.isPaid && !c.forfeitTriggered) result.Add(c);
            }
            return result;
        }

        // ── Actions ────────────────────────────────────────────────────

        /// <summary>
        /// Sign a new debt contract. The contract must be read twice before
        /// signing (enforced at the host/UI layer, not here — this system
        /// trusts that the host already read it twice).
        /// </summary>
        public bool SignContract(DebtContract contract, int currentDay)
        {
            if (contract == null) return false;
            if (string.IsNullOrEmpty(contract.contractId)) return false;
            if (GetContract(contract.contractId) != null) return false; // no duplicates

            contract.daySigned = currentDay;
            contract.isPaid = false;
            contract.forfeitTriggered = false;
            contract.renegotiated = false;
            _state.contracts.Add(contract);
            _state.contractsSigned++;
            OnContractSigned?.Invoke(contract);
            return true;
        }

        /// <summary>
        /// Pay off a contract in full. Returns false if already paid or forfeited.
        /// </summary>
        public bool PayContract(string contractId)
        {
            var contract = GetContract(contractId);
            if (contract == null) return false;
            if (contract.isPaid || contract.forfeitTriggered) return false;

            contract.isPaid = true;
            _state.contractsPaid++;
            OnContractPaid?.Invoke(contract);
            return true;
        }

        /// <summary>
        /// Renegotiate contract terms (extend term, reduce principal).
        /// Both parties must agree (host layer enforces this).
        /// </summary>
        public bool RenegotiateContract(string contractId, int newTermDays, string newPrincipal)
        {
            var contract = GetContract(contractId);
            if (contract == null) return false;
            if (contract.isPaid || contract.forfeitTriggered) return false;

            if (newTermDays > 0) contract.termDays = newTermDays;
            if (!string.IsNullOrEmpty(newPrincipal)) contract.principal = newPrincipal;
            contract.renegotiated = true;
            OnContractRenegotiated?.Invoke(contract);
            return true;
        }

        /// <summary>
        /// Daily tick: check for expired contracts and trigger forfeits.
        /// Called by the host game loop, not a background thread.
        /// </summary>
        public void Tick(int currentDay)
        {
            for (int i = 0; i < _state.contracts.Count; i++)
            {
                var c = _state.contracts[i];
                if (c == null || c.isPaid || c.forfeitTriggered) continue;
                if (currentDay - c.daySigned >= c.termDays)
                {
                    c.forfeitTriggered = true;
                    _state.forfeitsTriggered++;
                    OnForfeitTriggered?.Invoke(c);
                }
            }
        }

        /// <summary>
        /// Attempt to tamper with the ledger (e.g., erase a contract).
        /// Always fails — Ivo's records do not lie. Raises event for
        /// narrative consequences.
        /// </summary>
        public bool AttemptTamper(string contractId)
        {
            _state.ledgerTamperAttempts++;
            OnLedgerTampered?.Invoke(contractId);
            return false; // always fails
        }

        // ── Save / Load ────────────────────────────────────────────────

        public LedgerDebtState CaptureState() => _state;

        public void RestoreState(LedgerDebtState saved)
        {
            if (saved == null) return;
            _state = saved;
        }
    }
}
