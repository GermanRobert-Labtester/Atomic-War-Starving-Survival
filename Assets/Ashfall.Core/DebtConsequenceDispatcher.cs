using System;
using System.Collections.Generic;

#pragma warning disable CS8618

namespace Ashfall.Core
{
    /// <summary>
    /// Listens to LedgerDebtSystem events and dispatches consequences
    /// defined in the debt template catalog. Bridges the gap between
    /// the debt runtime (which owns financial truth) and the consequence
    /// catalog (which owns political/economic fallout).
    /// </summary>
    public class DebtConsequenceDispatcher
    {
        private readonly LedgerDebtSystem _ledger;
        private readonly DebtTemplateCatalog _catalog;
        private readonly HashSet<string> _firedConsequences = new HashSet<string>();

        public event Action<DebtConsequence, DebtContract> OnConsequenceDispatched;
        public event Action<string, DebtContract> OnStandingPenalty;
        public event Action<string, int, DebtContract> OnEmbargoRequested;
        public event Action<string, DebtContract> OnBountyRequested;
        public event Action<string, DebtContract> OnCollateralSeizure;
        public event Action<string, int, DebtContract> OnLaborObligation;

        public DebtConsequenceDispatcher(LedgerDebtSystem ledger, DebtTemplateCatalog catalog)
        {
            _ledger = ledger ?? throw new ArgumentNullException(nameof(ledger));
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _ledger.OnForfeitTriggered += HandleForfeit;
            _ledger.OnContractPaid += HandlePaid;
        }

        /// <summary>Detach from ledger events.</summary>
        public void Detach()
        {
            _ledger.OnForfeitTriggered -= HandleForfeit;
            _ledger.OnContractPaid -= HandlePaid;
        }

        /// <summary>Whether a consequence has already fired for this contract.</summary>
        public bool HasFired(string contractKey)
        {
            return _firedConsequences.Contains(contractKey);
        }

        /// <summary>Reset fired state (for testing).</summary>
        public void ResetFired()
        {
            _firedConsequences.Clear();
        }

        private void HandleForfeit(DebtContract contract)
        {
            if (contract == null) return;
            string consequenceId = ResolveConsequenceId(contract);
            if (string.IsNullOrEmpty(consequenceId)) return;

            var consequence = _catalog.GetConsequence(consequenceId);
            if (consequence == null) return;

            string key = contract.debtorId + ":" + consequenceId;
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

        private void DispatchConsequence(DebtConsequence consequence, DebtContract contract)
        {
            OnConsequenceDispatched?.Invoke(consequence, contract);

            switch (consequence.effectType)
            {
                case "standing_loss":
                    if (!string.IsNullOrEmpty(consequence.targetFactionId))
                        OnStandingPenalty?.Invoke(consequence.targetFactionId, contract);
                    break;

                case "embargo":
                    if (!string.IsNullOrEmpty(consequence.embargoScope))
                        OnEmbargoRequested?.Invoke(consequence.embargoScope, consequence.embargoDurationDays, contract);
                    break;

                case "bounty":
                    if (!string.IsNullOrEmpty(consequence.targetFactionId))
                        OnBountyRequested?.Invoke(consequence.targetFactionId, contract);
                    break;

                case "collateral_seizure":
                    if (!string.IsNullOrEmpty(consequence.collateralItemId))
                        OnCollateralSeizure?.Invoke(consequence.collateralItemId, contract);
                    break;

                case "labor_obligation":
                    if (!string.IsNullOrEmpty(consequence.targetFactionId) && consequence.laborDays > 0)
                        OnLaborObligation?.Invoke(consequence.targetFactionId, consequence.laborDays, contract);
                    break;

                case "standing_loss_and_embargo":
                    if (!string.IsNullOrEmpty(consequence.targetFactionId))
                        OnStandingPenalty?.Invoke(consequence.targetFactionId, contract);
                    if (!string.IsNullOrEmpty(consequence.embargoScope))
                        OnEmbargoRequested?.Invoke(consequence.embargoScope, consequence.embargoDurationDays, contract);
                    break;

                case "bounty_and_seizure":
                    if (!string.IsNullOrEmpty(consequence.targetFactionId))
                        OnBountyRequested?.Invoke(consequence.targetFactionId, contract);
                    if (!string.IsNullOrEmpty(consequence.collateralItemId))
                        OnCollateralSeizure?.Invoke(consequence.collateralItemId, contract);
                    break;

                case "raid":
                    if (!string.IsNullOrEmpty(consequence.targetFactionId))
                        OnBountyRequested?.Invoke(consequence.targetFactionId, contract);
                    break;

                case "treaty_breach":
                    if (!string.IsNullOrEmpty(consequence.targetFactionId))
                        OnStandingPenalty?.Invoke(consequence.targetFactionId, contract);
                    break;

                case "forgiveness":
                    // Forgiveness clears the debt — handled by caller
                    break;
            }

            // Chain escalation if defined
            if (!string.IsNullOrEmpty(consequence.escalationId))
            {
                var escalation = _catalog.GetConsequence(consequence.escalationId);
                if (escalation != null)
                {
                    string escKey = contract.debtorId + ":" + consequence.escalationId;
                    if (!_firedConsequences.Contains(escKey))
                    {
                        _firedConsequences.Add(escKey);
                        DispatchConsequence(escalation, contract);
                    }
                }
            }
        }
    }
}
