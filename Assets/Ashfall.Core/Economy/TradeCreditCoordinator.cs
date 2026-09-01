using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using Ashfall.Core.YearOfAsh;

#pragma warning disable CS8618

namespace Ashfall.Core.Economy
{
    /// <summary>
    /// Presentation/transaction projection of one valid debt template — never
    /// contract state. The signed ledger contract remains authoritative after
    /// acceptance. Every material term is a catalog field, not a UI string.
    /// </summary>
    public sealed class CreditOffer
    {
        public string TemplateId { get; }
        public string CreditorId { get; }
        public string PrincipalItemId { get; }
        public int PrincipalQuantity { get; }
        public int TermDays { get; }
        public float Rate { get; }
        public string ForfeitDescription { get; }
        /// <summary>Authored consequence summary (catalog description) shown on
        /// the offer so the player knows what default costs before signing.</summary>
        public string ConsequenceSummary { get; }
        public string CreditorDisplayName { get; }

        public CreditOffer(DebtTemplate template, DebtConsequence consequence)
        {
            TemplateId = template.id;
            CreditorId = template.creditorId;
            PrincipalItemId = template.principalItemId;
            PrincipalQuantity = template.principalQuantity;
            TermDays = template.termDays;
            Rate = template.rate;
            ForfeitDescription = template.forfeitDescription;
            ConsequenceSummary = consequence?.description ?? string.Empty;
            CreditorDisplayName = template.displayName ?? template.id;
        }
    }

    public sealed class CreditOfferResult
    {
        public bool Eligible { get; private set; }
        public CreditOffer? Offer { get; private set; }
        /// <summary>Failure taxonomy: credit_no_matching_template,
        /// credit_hostile_standing, credit_existing_debt, credit_embargoed,
        /// credit_template_inactive. Empty when eligible.</summary>
        public string Reason { get; private set; } = string.Empty;

        public static CreditOfferResult Eligible_(CreditOffer offer) =>
            new CreditOfferResult { Eligible = true, Offer = offer };
        public static CreditOfferResult Reject(string reason) =>
            new CreditOfferResult { Eligible = false, Reason = reason ?? "credit_unknown" };
    }

    public sealed class CreditAcceptResult
    {
        public bool Success { get; private set; }
        public string DebtorId { get; private set; } = string.Empty;
        public string Reason { get; private set; } = string.Empty;

        public static CreditAcceptResult Ok(string debtorId) =>
            new CreditAcceptResult { Success = true, DebtorId = debtorId };
        public static CreditAcceptResult Fail(string reason) =>
            new CreditAcceptResult { Success = false, Reason = reason ?? "credit_unknown" };
    }

    /// <summary>
    /// Turns an insufficient-funds trade rejection into an optional, explicit,
    /// catalog-driven credit offer when the creditor and campaign state permit.
    ///
    /// Rules baked in:
    ///   - a failed trade may OFFER debt; it can never sign one implicitly;
    ///   - the offer is a projection of the template — terms are never copied
    ///     from UI strings;
    ///   - acceptance revalidates every gate before ink (stale offers die here);
    ///   - the contract ceremony is the ledger's own two-reading rite;
    ///   - principal disbursement and signing are compensating: if either half
    ///     fails, the other half is rolled back — the player never owes debt
    ///     without goods, and never holds goods without debt.
    /// </summary>
    public sealed class TradeCreditCoordinator
    {
        private readonly LedgerDebtSystem _ledger;
        private readonly DebtTemplateCatalog _catalog;
        private readonly FactionEmbargoLedger _embargoes;
        private readonly FactionWarSystem? _factionWar;
        private readonly Func<int> _currentDay;
        private readonly Func<string, int, bool> _tryGrantItems;
        private readonly Action<string, int>? _revokeItems;
        private readonly string _debtorId;
        private readonly ILog _log;

        public TradeCreditCoordinator(
            LedgerDebtSystem ledger,
            DebtTemplateCatalog catalog,
            FactionEmbargoLedger embargoes,
            Func<int> currentDay,
            Func<string, int, bool> tryGrantItems,
            string debtorId,
            FactionWarSystem? factionWar = null,
            Action<string, int>? revokeItems = null,
            ILog? log = null)
        {
            _ledger = ledger ?? throw new ArgumentNullException(nameof(ledger));
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _embargoes = embargoes ?? throw new ArgumentNullException(nameof(embargoes));
            _currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
            _tryGrantItems = tryGrantItems ?? throw new ArgumentNullException(nameof(tryGrantItems));
            _debtorId = debtorId;
            _factionWar = factionWar;
            _revokeItems = revokeItems;
            _log = log ?? NullLog.Instance;
        }

        /// <summary>The ledger status that counts as unresolved exposure for
        /// the same-creditor gate: signed and not settled (live, overdue or
        /// defaulted-forfeit pending). Paid and forgiven ink never blocks.</summary>
        public bool HasUnpaidDebtFromCreditor(string creditorId)
        {
            if (string.IsNullOrEmpty(creditorId)) return false;
            for (int i = 0; i < _ledger.Contracts.Count; i++)
            {
                var c = _ledger.Contracts[i];
                if (c == null || c.creditorId != creditorId) continue;
                if (c.signed && !c.paid && !c.forgiven) return true;
            }
            return false;
        }

        /// <summary>
        /// Resolve the credit template for a failed trade context. Deterministic
        /// order: catalog order (authored file order), first eligible wins.
        /// </summary>
        public CreditOfferResult TryBuildCreditOffer(string creditorId, string requestedItemId)
        {
            string canonicalItem = ItemAliases.ToCanonical(requestedItemId);
            DebtTemplate? match = null;
            for (int i = 0; i < _catalog.Templates.Count; i++)
            {
                var t = _catalog.Templates[i];
                if (t == null) continue;
                if (t.creditorId != creditorId) continue;
                if (ItemAliases.ToCanonical(t.principalItemId) != canonicalItem) continue;
                string reason = EvaluateGates(t, canonicalItem);
                if (string.IsNullOrEmpty(reason))
                {
                    match = t;
                    break;
                }
            }

            if (match == null)
            {
                // Distinguish "nothing matches this trade" from "a template
                // matched but a gate refused" for diagnostics and tests.
                bool anyTemplate = false;
                for (int i = 0; i < _catalog.Templates.Count; i++)
                {
                    var t = _catalog.Templates[i];
                    if (t == null || t.creditorId != creditorId) continue;
                    if (ItemAliases.ToCanonical(t.principalItemId) == canonicalItem) { anyTemplate = true; break; }
                }
                return CreditOfferResult.Reject(anyTemplate ? FirstGateRejection(creditorId, canonicalItem) : "credit_no_matching_template");
            }

            var consequence = !string.IsNullOrEmpty(match.consequenceId) ? _catalog.GetConsequence(match.consequenceId) : null;
            _log.Info("CreditOfferShown { campaignDay=" + _currentDay() + " creditor=" + creditorId
                + " template=" + match.id + " item=" + match.principalItemId
                + " qty=" + match.principalQuantity + " }");
            return CreditOfferResult.Eligible_(new CreditOffer(match, consequence));
        }

        private string FirstGateRejection(string creditorId, string canonicalItem)
        {
            for (int i = 0; i < _catalog.Templates.Count; i++)
            {
                var t = _catalog.Templates[i];
                if (t == null || t.creditorId != creditorId) continue;
                if (ItemAliases.ToCanonical(t.principalItemId) != canonicalItem) continue;
                string reason = EvaluateGates(t, canonicalItem);
                if (!string.IsNullOrEmpty(reason)) return reason;
            }
            return "credit_no_matching_template";
        }

        /// <summary>All gates must pass for an offer to exist. Returns the
        /// first failing reason, or empty when eligible.</summary>
        private string EvaluateGates(DebtTemplate template, string canonicalRequestedItem)
        {
            // Standing gate: creditors do not lend into hostility. The
            // threshold is the faction authority's canonical constant.
            if (_factionWar != null)
            {
                int standing = _factionWar.GetStanding(template.creditorId);
                if (standing <= FactionWarSystem.HostileStandingThreshold)
                    return "credit_hostile_standing";
            }

            // Existing-debt gate: one unresolved exposure per creditor.
            if (HasUnpaidDebtFromCreditor(template.creditorId))
                return "credit_existing_debt";

            // Embargo gate: credit cannot bypass a suspension.
            if (_embargoes.IsEmbargoed(template.creditorId, _currentDay()))
                return "credit_embargoed";

            // Principal-relevance gate: the failed trade must involve the
            // template's principal — no medical debt offered over a rifle.
            if (ItemAliases.ToCanonical(template.principalItemId) != canonicalRequestedItem)
                return "credit_no_matching_template";

            // Template-active gate: every authored template is currently
            // unconditioned (no day/unlock fields in the schema); the hook
            // stays here so a future schema field lands in one place.
            return string.Empty;
        }

        /// <summary>
        /// Explicit acceptance. Revalidates every gate (the offer may have gone
        /// stale while on screen), runs the ledger's two-reading ceremony,
        /// signs, then disburses the principal. Order is grant → sign: if
        /// signing fails the grant is revoked, so the transaction can never
        /// end half-committed. Re-entering acceptance for the same creditor
        /// after a save/reload hits the same-creditor gate before any grant.
        /// </summary>
        public CreditAcceptResult TryAcceptCredit(string templateId, string creditorId)
        {
            var template = _catalog.GetTemplate(templateId);
            if (template == null || template.creditorId != creditorId)
                return CreditAcceptResult.Fail("credit_no_matching_template");
            if (!string.IsNullOrEmpty(EvaluateGates(template, ItemAliases.ToCanonical(template.principalItemId))))
                return CreditAcceptResult.Fail("credit_stale_offer");

            // The ink: two readings, then signature — the same rite everywhere,
            // never bypassed by writing ledger state directly.
            string forfeit = template.forfeitDescription;
            if (!_ledger.PresentContract(_debtorId, template.principalQuantity, template.termDays, template.rate, forfeit, creditorId, template.id))
                return CreditAcceptResult.Fail("credit_sign_failed");
            if (!_ledger.PresentContract(_debtorId, template.principalQuantity, template.termDays, template.rate, forfeit, creditorId, template.id))
                return CreditAcceptResult.Fail("credit_sign_failed");
            int day = _currentDay();

            // Disburse first, then sign; compensate the grant if ink fails.
            if (!_tryGrantItems(template.principalItemId, template.principalQuantity))
            {
                _log.Warn("CreditPrincipalTransferFailed { item=" + template.principalItemId
                    + " qty=" + template.principalQuantity + " } — contract not signed, grant rolled back.");
                return CreditAcceptResult.Fail("credit_principal_transfer_failed");
            }
            if (!_ledger.SignContract(_debtorId, day))
            {
                _revokeItems?.Invoke(template.principalItemId, template.principalQuantity);
                _log.Warn("CreditSignFailed { template=" + template.id + " } — principal disbursement revoked.");
                return CreditAcceptResult.Fail("credit_sign_failed");
            }

            _log.Info("CreditContractSigned { campaignDay=" + day + " debtor=" + _debtorId
                + " creditor=" + creditorId + " template=" + template.id
                + " item=" + template.principalItemId + " qty=" + template.principalQuantity
                + " term=" + template.termDays + " rate=" + template.rate + " }");
            return CreditAcceptResult.Ok(_debtorId);
        }
    }
}
