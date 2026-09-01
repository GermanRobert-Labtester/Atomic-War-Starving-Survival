using System;
using System.Collections.Generic;

#pragma warning disable CS8618

namespace Ashfall.Core
{
    /// <summary>Serializable fired-consequence ledger for the dispatcher.
    /// Persisted with the expansion-hub save so a consequence side effect
    /// committed before the save can never be committed again after restore.</summary>
    [Serializable]
    public class DebtDispatcherState
    {
        public List<string> firedConsequences = new List<string>();
    }

    /// <summary>
    /// Listens to LedgerDebtSystem events and dispatches consequences
    /// defined in the debt template catalog. Bridges the gap between
    /// the debt runtime (which owns financial truth) and the consequence
    /// catalog (which owns political/economic fallout).
    ///
    /// Idempotency contract: every dispatched side effect carries a stable
    /// identity (debtor + signed day + consequence id) recorded in
    /// <see cref="DebtDispatcherState"/>. Once a side effect is committed,
    /// restoring from any subsequent save must not commit it again — the
    /// fired set must therefore be captured with the save and restored
    /// before the next forfeit can be evaluated.
    /// </summary>
    public class DebtConsequenceDispatcher
    {
        private readonly LedgerDebtSystem _ledger;
        private readonly DebtTemplateCatalog _catalog;
        private readonly HashSet<string> _firedConsequences = new HashSet<string>();
        private Action<DebtConsequence, string, DebtContract>? _standingHandler;
        private Func<int>? _dayProvider;

        public event Action<DebtConsequence, DebtContract> OnConsequenceDispatched;
        public event Action<DebtConsequence, string, DebtContract> OnStandingPenalty;
        public event Action<string, int, DebtContract> OnEmbargoRequested;
        public event Action<string, DebtContract> OnBountyRequested;
        public event Action<string, int, DebtContract> OnCollateralSeizure;
        public event Action<string, int, DebtContract> OnLaborObligation;

        public DebtConsequenceDispatcher(LedgerDebtSystem ledger, DebtTemplateCatalog catalog)
        {
            _ledger = ledger ?? throw new ArgumentNullException(nameof(ledger));
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _ledger.OnForfeitTriggered += HandleForfeit;
            _ledger.OnContractPaid += HandlePaid;
        }

        /// <summary>
        /// Connect to FactionWarSystem so standing penalties from debt defaults
        /// propagate to the authoritative standing system. This is the integration
        /// point between Plan 40 (debt) and Plan 45 (patrols) — standing loss
        /// from debt default makes future patrol encounters more hostile.
        /// The delta applied is the one authored on the consequence that fired
        /// (including escalated consequences), never re-resolved from the base
        /// template.
        /// </summary>
        public void ConnectStandingSystem(Func<string, int, bool> modifyStanding)
        {
            DetachStandingHandler();
            _standingHandler = (consequence, factionId, contract) =>
            {
                if (consequence.standingDelta == 0) return;
                modifyStanding?.Invoke(factionId, consequence.standingDelta);
            };
            OnStandingPenalty += _standingHandler;
        }

        /// <summary>Optional campaign-day source used to stamp forgiven
        /// contracts. Falls back to the contract's signed day when absent.</summary>
        public void SetDayProvider(Func<int> dayProvider) => _dayProvider = dayProvider;

        private void DetachStandingHandler()
        {
            if (_standingHandler != null)
            {
                OnStandingPenalty -= _standingHandler;
                _standingHandler = null;
            }
        }

        /// <summary>Detach from ledger events and standing handler.</summary>
        public void Detach()
        {
            _ledger.OnForfeitTriggered -= HandleForfeit;
            _ledger.OnContractPaid -= HandlePaid;
            DetachStandingHandler();
        }

        /// <summary>Whether a consequence has already fired for this contract instance.</summary>
        public bool HasFired(string identityKey)
        {
            return _firedConsequences.Contains(identityKey);
        }

        /// <summary>Stable identity for one committed consequence side effect:
        /// debtor + contract-instance day + consequence. Deterministic across
        /// save/load (no counters, no RNG).</summary>
        public static string ConsequenceIdentity(DebtContract contract, string consequenceId)
        {
            return contract.debtorId + "@" + contract.signedDay + ":" + consequenceId;
        }

        /// <summary>Reset fired state (for testing).</summary>
        public void ResetFired()
        {
            _firedConsequences.Clear();
        }

        public DebtDispatcherState CaptureState()
        {
            var state = new DebtDispatcherState();
            state.firedConsequences.AddRange(_firedConsequences);
            return state;
        }

        public void RestoreState(DebtDispatcherState saved)
        {
            _firedConsequences.Clear();
            if (saved?.firedConsequences == null) return;
            for (int i = 0; i < saved.firedConsequences.Count; i++)
            {
                var key = saved.firedConsequences[i];
                if (!string.IsNullOrEmpty(key)) _firedConsequences.Add(key);
            }
        }

        private void HandleForfeit(DebtContract contract)
        {
            if (contract == null) return;
            string consequenceId = ResolveConsequenceId(contract);
            if (string.IsNullOrEmpty(consequenceId)) return;

            var consequence = _catalog.GetConsequence(consequenceId);
            if (consequence == null) return;

            string key = ConsequenceIdentity(contract, consequenceId);
            if (_firedConsequences.Contains(key)) return;
            _firedConsequences.Add(key);

            DispatchConsequence(consequence, contract);
        }

        private void HandlePaid(DebtContract contract)
        {
            // Future: could clear embargo or restore standing
        }

        private string ResolveConsequenceId(DebtContract contract)
        {
            // Try template-linked consequence first
            if (!string.IsNullOrEmpty(contract.templateId))
            {
                var template = _catalog.GetTemplate(contract.templateId);
                if (template != null && !string.IsNullOrEmpty(template.consequenceId))
                    return template.consequenceId;
            }
            return null;
        }

        /// <summary>
        /// Collateral to seize: the consequence's authored item, falling back to
        /// the contract template's principal (the pledged good is what the
        /// creditor lends; authored consequences leave the field empty to mean
        /// exactly that).
        /// </summary>
        private DebtTemplate? ResolveTemplate(DebtContract contract)
        {
            return string.IsNullOrEmpty(contract.templateId)
                ? null
                : _catalog.GetTemplate(contract.templateId);
        }

        private void DispatchConsequence(DebtConsequence consequence, DebtContract contract)
        {
            OnConsequenceDispatched?.Invoke(consequence, contract);

            // Resolve target faction: explicit targetFactionId, or fall back to creditorId
            string targetFaction = !string.IsNullOrEmpty(consequence.targetFactionId)
                ? consequence.targetFactionId
                : contract.creditorId ?? string.Empty;

            var template = ResolveTemplate(contract);

            switch (consequence.effectType)
            {
                case "standing_loss":
                    if (!string.IsNullOrEmpty(targetFaction))
                        OnStandingPenalty?.Invoke(consequence, targetFaction, contract);
                    break;

                case "embargo":
                    if (!string.IsNullOrEmpty(consequence.embargoScope))
                        OnEmbargoRequested?.Invoke(consequence.embargoScope, consequence.embargoDurationDays, contract);
                    break;

                case "bounty":
                    if (!string.IsNullOrEmpty(targetFaction))
                        OnBountyRequested?.Invoke(targetFaction, contract);
                    break;

                case "collateral_seizure":
                    TryDispatchSeizure(consequence, template, contract);
                    break;

                case "labor_obligation":
                    if (!string.IsNullOrEmpty(targetFaction) && consequence.laborDays > 0)
                        OnLaborObligation?.Invoke(targetFaction, consequence.laborDays, contract);
                    break;

                case "standing_loss_and_embargo":
                    if (!string.IsNullOrEmpty(targetFaction))
                        OnStandingPenalty?.Invoke(consequence, targetFaction, contract);
                    if (!string.IsNullOrEmpty(consequence.embargoScope))
                        OnEmbargoRequested?.Invoke(consequence.embargoScope, consequence.embargoDurationDays, contract);
                    break;

                case "bounty_and_seizure":
                    if (!string.IsNullOrEmpty(targetFaction))
                        OnBountyRequested?.Invoke(targetFaction, contract);
                    TryDispatchSeizure(consequence, template, contract);
                    break;

                case "raid":
                    if (!string.IsNullOrEmpty(targetFaction))
                        OnBountyRequested?.Invoke(targetFaction, contract);
                    break;

                case "treaty_breach":
                    if (!string.IsNullOrEmpty(targetFaction))
                        OnStandingPenalty?.Invoke(consequence, targetFaction, contract);
                    break;

                case "forgiveness":
                    // Mercy is a ledger mutation, not a presentation event: the
                    // balance is cleared by the canonical transition, no payment
                    // moves. The fired-set key above already guards the "does
                    // not happen twice" rule authored on the consequence.
                    int day = _dayProvider != null ? _dayProvider() : contract.signedDay;
                    _ledger.ForgiveContract(contract.debtorId, day);
                    break;
            }

            // Chain escalation if defined
            if (!string.IsNullOrEmpty(consequence.escalationId))
            {
                var escalation = _catalog.GetConsequence(consequence.escalationId);
                if (escalation != null)
                {
                    string escKey = ConsequenceIdentity(contract, consequence.escalationId);
                    if (!_firedConsequences.Contains(escKey))
                    {
                        _firedConsequences.Add(escKey);
                        DispatchConsequence(escalation, contract);
                    }
                }
            }
        }

        private void TryDispatchSeizure(DebtConsequence consequence, DebtTemplate? template, DebtContract contract)
        {
            string itemId;
            int quantity;
            if (!string.IsNullOrEmpty(consequence.collateralItemId))
            {
                itemId = consequence.collateralItemId;
                quantity = 1;
            }
            else if (template != null && !string.IsNullOrEmpty(template.principalItemId))
            {
                // Authored default: seize the pledged principal, at the lent quantity.
                itemId = template.principalItemId;
                quantity = template.principalQuantity;
            }
            else
            {
                return;
            }

            if (quantity <= 0) return;
            OnCollateralSeizure?.Invoke(itemId, quantity, contract);
        }
    }
}
