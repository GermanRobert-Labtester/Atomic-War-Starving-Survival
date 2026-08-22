using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.Expeditions;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class ContractorRosterState
    {
        public string systemId = ContractorRosterSystem.SystemId;
        public List<Contractor> contractors = new List<Contractor>();
        public List<ContractOffer> activeOffers = new List<ContractOffer>();
    }

    [Serializable]
    public sealed class ContractOffer
    {
        public string offerId = string.Empty;
        public string candidateId = string.Empty;
        public string role = string.Empty;
        public int initialFee;
        public int dailyHazardPay;
        public string paymentCurrency = "scrap_metal";
        public int termDays;
        public float loyalty = 100f;
        public ContractStatus status;
        public int proposedDay = -1;
        public int startDay = -1;
        public int expiryDay = -1;
        public List<string> requiredSkills = new List<string>();
        public List<string> equipmentIds = new List<string>();
    }

    [Serializable]
    public sealed class Contractor
    {
        public string contractorId = string.Empty;
        public string displayName = string.Empty;
        public string role = string.Empty;
        public float loyalty = 100f;
        public float trust = 50f;
        public ContractStatus status;
        public int startDay = -1;
        public int expiryDay = -1;
        public int missedPayments;
        public bool isInjured;
        public bool isDeceased;
        public List<string> skillIds = new List<string>();
        public List<string> equipmentIds = new List<string>();
    }

    public enum ContractStatus { Available, Active, Expired, Dismissed, Deceased }

    public sealed class ContractorRosterSystem
    {
        public const string SystemId = "contractor_roster";
        private ContractorRosterState _state = new ContractorRosterState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly Inventory.Inventory _inventory;
        private readonly DutyRosterSystem _roster;
        private readonly ExpeditionSystem _expedition;
        private int _currentDay;

        public ContractorRosterState State => _state;
        public event Action<Contractor> OnContractorStatusChanged;
        public event Action<ContractOffer> OnOfferStatusChanged;
        public event Action OnRosterChanged;

        public ContractorRosterSystem(
            ISeededRng rng,
            Inventory.Inventory inventory,
            DutyRosterSystem roster,
            ExpeditionSystem expedition,
            ILog log = null!)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _roster = roster ?? throw new ArgumentNullException(nameof(roster));
            _expedition = expedition ?? throw new ArgumentNullException(nameof(expedition));
            _log = log ?? NullLog.Instance;
        }

        public ActionResult GenerateOffer(string candidateId, string role, List<string> requiredSkills, int initialFee, int dailyPay, int termDays)
        {
            if (_state.activeOffers.Exists(o => o.candidateId == candidateId && o.status == ContractStatus.Available))
                return ActionResult.Blocked("offer_exists", "contractor.offer_exists");

            var offer = new ContractOffer
            {
                offerId = $"offer_{_currentDay}_{candidateId}",
                candidateId = candidateId, role = role,
                requiredSkills = requiredSkills ?? new List<string>(),
                initialFee = initialFee, dailyHazardPay = dailyPay,
                paymentCurrency = "scrap_metal",
                termDays = termDays,
                proposedDay = _currentDay,
                status = ContractStatus.Available
            };
            _state.activeOffers.Add(offer);
            OnRosterChanged?.Invoke();
            return ActionResult.Success("contractor.offer_generated");
        }

        public ActionResult AcceptOffer(string offerId)
        {
            var offer = _state.activeOffers.Find(o => o.offerId == offerId);
            if (offer == null) return ActionResult.Failed("unknown_offer", "contractor.unknown_offer");
            if (offer.status != ContractStatus.Available) return ActionResult.Blocked("not_available", "contractor.not_available");

            string currency = string.IsNullOrEmpty(offer.paymentCurrency) ? "scrap_metal" : offer.paymentCurrency;
            // Atomic payment
            if (_inventory.CountById(currency) < offer.initialFee)
                return ActionResult.Blocked("insufficient_funds", "contractor.insufficient_funds");

            _inventory.RemoveById(currency, offer.initialFee);

            var contractor = new Contractor
            {
                contractorId = offer.candidateId,
                displayName = offer.candidateId,
                role = offer.role,
                skillIds = new List<string>(offer.requiredSkills),
                equipmentIds = new List<string>(offer.equipmentIds),
                status = ContractStatus.Active,
                startDay = _currentDay,
                expiryDay = _currentDay + offer.termDays,
                loyalty = 100f
            };
            _state.contractors.Add(contractor);

            offer.status = ContractStatus.Active;
            offer.startDay = _currentDay;

            _log.Info($"[Contractor] hired {offer.candidateId} as {offer.role}");
            OnContractorStatusChanged?.Invoke(contractor);
            OnOfferStatusChanged?.Invoke(offer);
            OnRosterChanged?.Invoke();
            return ActionResult.Success("contractor.hired");
        }

        public ActionResult Dismiss(string contractorId)
        {
            var contractor = _state.contractors.Find(c => c.contractorId == contractorId);
            if (contractor == null) return ActionResult.Failed("unknown_contractor", "contractor.unknown");
            if (contractor.status != ContractStatus.Active) return ActionResult.Blocked("not_active", "contractor.not_active");

            contractor.status = ContractStatus.Dismissed;
            _log.Info($"[Contractor] dismissed {contractorId}");
            OnContractorStatusChanged?.Invoke(contractor);
            OnRosterChanged?.Invoke();
            return ActionResult.Success("contractor.dismissed");
        }

        public void TickDay(int day)
        {
            _currentDay = day;

            // Process active contractors
            foreach (var c in _state.contractors)
            {
                if (c.status != ContractStatus.Active) continue;

                // Expiry check FIRST — a contractor whose contract is ending today
                // must not be charged for a missed daily wage on their last day.
                if (day >= c.expiryDay && c.status == ContractStatus.Active)
                {
                    c.status = ContractStatus.Expired;
                    _log.Info($"[Contractor] {c.contractorId} contract expired");
                    OnContractorStatusChanged?.Invoke(c);
                    continue;
                }

                // Daily hazard pay
                var activeOffer = _state.activeOffers.Find(o => o.candidateId == c.contractorId && o.status == ContractStatus.Active);
                if (activeOffer != null)
                {
                    string currency = string.IsNullOrEmpty(activeOffer.paymentCurrency) ? "scrap_metal" : activeOffer.paymentCurrency;
                    if (_inventory.CountById(currency) >= activeOffer.dailyHazardPay)
                    {
                        _inventory.RemoveById(currency, activeOffer.dailyHazardPay);
                    }
                    else
                    {
                        c.missedPayments++;
                        c.loyalty = Math.Max(0, c.loyalty - 10f);
                        if (c.missedPayments >= 3)
                        {
                            c.status = ContractStatus.Expired;
                            _log.Warn($"[Contractor] {c.contractorId} left due to unpaid wages");
                            OnContractorStatusChanged?.Invoke(c);
                        }
                    }
                }
            }

            OnRosterChanged?.Invoke();
        }

        public bool IsAvailableForExpedition(string contractorId)
        {
            var c = _state.contractors.Find(c => c.contractorId == contractorId);
            if (c == null || c.status != ContractStatus.Active || c.isInjured || c.isDeceased)
                return false;

            // Chain 5 (audit): a contractor already on an active expedition is
            // not available for another. The _expedition port was injected but
            // never consulted, so a single contractor could be sent out twice
            // simultaneously. ExpeditionSystem.Start enforces one expedition
            // per survivor; this guard surfaces that constraint here.
            if (_expedition != null && _expedition.Active.ContainsKey(contractorId))
                return false;

            return true;
        }

        public ContractorRosterState CaptureState() => _state;
        public void RestoreState(ContractorRosterState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnRosterChanged?.Invoke();
        }
    }
}
