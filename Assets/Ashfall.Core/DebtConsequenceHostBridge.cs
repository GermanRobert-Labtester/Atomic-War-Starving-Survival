using System;
using System.Collections.Generic;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;

#pragma warning disable CS8618

namespace Ashfall.Core
{
    /// <summary>One bounded bonded-labor obligation created by a debt default.
    /// Bounded by <see cref="endDay"/> — never a permanent assignment.</summary>
    [Serializable]
    public class DebtLaborObligationRecord
    {
        /// <summary>Stable source identity (debt:{contract}:{consequence}); one
        /// obligation per consequence, defended here as well as in the dispatcher.</summary>
        public string sourceId = string.Empty;
        public string creditorFactionId = string.Empty;
        public string survivorId = string.Empty;
        public int laborDays;
        public int startDay;
        public int endDay;
        public bool released;
    }

    [Serializable]
    public class DebtConsequenceBridgeState
    {
        public List<DebtLaborObligationRecord> laborObligations = new List<DebtLaborObligationRecord>();
    }

    /// <summary>
    /// Host-side consequence routing: turns DebtConsequenceDispatcher's typed
    /// requests into mutations on the canonical live authorities.
    ///
    ///   standing   → FactionWarSystem.ModifyStanding (canonical clamping)
    ///   embargo    → FactionEmbargoLedger.TryAddEmbargo (day-derived expiry)
    ///   bounty/raid→ IronRaidersSystem.ProvokeRaid (deterministic, once per
    ///                consequence via the dispatcher's persisted fired-set)
    ///   collateral → inventory authority via injected delegates (all-or-nothing)
    ///   labor      → bounded obligation ledger owned here (endDay-bounded)
    ///
    /// The bridge coordinates; it never substitutes for the authorities. The
    /// dispatcher's persisted fired-set is the primary idempotency store; the
    /// embargo and labor ledgers additionally dedupe by source identity.
    /// </summary>
    public class DebtConsequenceHostBridge
    {
        private readonly DebtConsequenceDispatcher _dispatcher;
        private readonly FactionWarSystem _factionWar;
        private readonly FactionEmbargoLedger _embargoes;
        private readonly IronRaidersSystem? _ironRaiders;
        private readonly Func<string, int, bool>? _tryRemoveItems;
        private readonly Func<string, int>? _countItem;
        private readonly Func<string>? _selectLaborSurvivor;
        private readonly Func<int> _currentDay;
        private readonly ILog _log;

        private DebtConsequenceBridgeState _state = new DebtConsequenceBridgeState();

        public event Action<DebtLaborObligationRecord> OnLaborObligationCreated;
        public event Action<DebtLaborObligationRecord> OnLaborObligationReleased;
        public event Action OnStateChanged;

        public IReadOnlyList<DebtLaborObligationRecord> LaborObligations => _state.laborObligations;

        /// <summary>Counters for diagnostics (consequences dispatched/seen).</summary>
        public int DispatchedCount { get; private set; }
        public int StandingApplications { get; private set; }
        public int EmbargoApplications { get; private set; }
        public int BountyApplications { get; private set; }
        public int SeizureApplications { get; private set; }

        public DebtConsequenceHostBridge(
            DebtConsequenceDispatcher dispatcher,
            FactionWarSystem factionWar,
            FactionEmbargoLedger embargoes,
            Func<int> currentDay,
            ILog? log = null,
            IronRaidersSystem? ironRaiders = null,
            Func<string, int, bool>? tryRemoveItems = null,
            Func<string, int>? countItem = null,
            Func<string>? selectLaborSurvivor = null)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            _factionWar = factionWar ?? throw new ArgumentNullException(nameof(factionWar));
            _embargoes = embargoes ?? throw new ArgumentNullException(nameof(embargoes));
            _currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
            _log = log ?? NullLog.Instance;
            _ironRaiders = ironRaiders;
            _tryRemoveItems = tryRemoveItems;
            _countItem = countItem;
            _selectLaborSurvivor = selectLaborSurvivor;

            Attach();
        }

        /// <summary>Subscribe to the dispatcher's typed requests. Safe to call
        /// again only after <see cref="Detach"/> — a live bridge is subscribed
        /// exactly once (enforced by the guard below).</summary>
        private bool _attached;
        private void Attach()
        {
            if (_attached) return;
            _attached = true;
            _dispatcher.OnStandingPenalty += HandleStandingPenalty;
            _dispatcher.OnEmbargoRequested += HandleEmbargoRequested;
            _dispatcher.OnBountyRequested += HandleBountyRequested;
            _dispatcher.OnCollateralSeizure += HandleCollateralSeizure;
            _dispatcher.OnLaborObligation += HandleLaborObligation;
            _dispatcher.OnConsequenceDispatched += HandleConsequenceDispatched;
        }

        /// <summary>Detach at session teardown so recomposition cannot leave
        /// dangling subscriptions into old systems.</summary>
        public void Detach()
        {
            if (!_attached) return;
            _attached = false;
            _dispatcher.OnStandingPenalty -= HandleStandingPenalty;
            _dispatcher.OnEmbargoRequested -= HandleEmbargoRequested;
            _dispatcher.OnBountyRequested -= HandleBountyRequested;
            _dispatcher.OnCollateralSeizure -= HandleCollateralSeizure;
            _dispatcher.OnLaborObligation -= HandleLaborObligation;
            _dispatcher.OnConsequenceDispatched -= HandleConsequenceDispatched;
        }

        private void HandleConsequenceDispatched(DebtConsequence consequence, DebtContract contract)
        {
            DispatchedCount++;
            // Structured diagnostic evidence that default handling completed.
            // Logging is telemetry only — never the idempotency store.
            _log.Info("DebtConsequenceDispatched { campaignDay=" + _currentDay()
                + " debtor=" + contract.debtorId
                + " template=" + contract.templateId
                + " creditor=" + contract.creditorId
                + " consequence=" + consequence.id
                + " effectType=" + consequence.effectType
                + " dispatchId=" + DebtConsequenceDispatcher.ConsequenceIdentity(contract, consequence.id)
                + " }");
        }

        private void HandleStandingPenalty(DebtConsequence consequence, string factionId, DebtContract contract)
        {
            // Canonical authority validates the faction, applies and clamps.
            _factionWar.ModifyStanding(factionId, consequence.standingDelta);
            StandingApplications++;
            _log.Info("DebtStandingPenalty { faction=" + factionId
                + " delta=" + consequence.standingDelta
                + " debtor=" + contract.debtorId + " }");
        }

        private void HandleEmbargoRequested(string scope, int durationDays, DebtContract contract)
        {
            // Scope "creditor_faction" (and the empty default) suspends trade
            // with the creditor; the ledger owns start/end and dedupe.
            string faction = contract.creditorId ?? string.Empty;
            if (string.IsNullOrEmpty(faction) || durationDays <= 0) return;
            string sourceId = DebtSourceId(contract, "embargo");
            bool added = _embargoes.TryAddEmbargo(faction, scope, _currentDay(), durationDays, sourceId);
            if (added)
            {
                EmbargoApplications++;
                _log.Info("DebtEmbargo { faction=" + faction + " scope=" + scope
                    + " days=" + durationDays + " source=" + sourceId + " }");
            }
        }

        private void HandleBountyRequested(string factionId, DebtContract contract)
        {
            // Deterministic handoff: the dispatcher's persisted fired-set
            // guarantees this request is unique per consequence, so provoking
            // the raiders here cannot double-schedule after a reload.
            if (_ironRaiders == null)
            {
                _log.Warn("DebtBounty dropped: raid authority unavailable (host invariant failure).");
                return;
            }
            if (!_ironRaiders.State.isActive) _ironRaiders.Activate();
            _ironRaiders.ProvokeRaid();
            BountyApplications++;
            _log.Info("DebtBounty { faction=" + factionId + " debtor=" + contract.debtorId
                + " raids=" + _ironRaiders.RaidsThisSeason + " }");
        }

        private void HandleCollateralSeizure(string itemId, int quantity, DebtContract contract)
        {
            // All-or-nothing: if the pledged good is not fully present, nothing
            // is removed and the shortfall is logged (the creditor's collectors
            // leave empty-handed — the debt forfeit stays named).
            if (_tryRemoveItems == null)
            {
                _log.Warn("DebtCollateralSeizure dropped: inventory authority unavailable (host invariant failure).");
                return;
            }
            int available = _countItem != null ? _countItem(itemId) : 0;
            if (available < quantity)
            {
                _log.Warn("DebtCollateralSeizure shortfall { item=" + itemId
                    + " wanted=" + quantity + " held=" + available + " } — nothing seized.");
                return;
            }
            if (_tryRemoveItems(itemId, quantity))
            {
                SeizureApplications++;
                _log.Info("DebtCollateralSeizure { item=" + itemId + " qty=" + quantity
                    + " debtor=" + contract.debtorId + " }");
            }
        }

        private void HandleLaborObligation(string creditorFactionId, int laborDays, DebtContract contract)
        {
            string sourceId = DebtSourceId(contract, "labor");
            for (int i = 0; i < _state.laborObligations.Count; i++)
            {
                if (_state.laborObligations[i].sourceId == sourceId) return; // one per consequence
            }

            var record = new DebtLaborObligationRecord
            {
                sourceId = sourceId,
                creditorFactionId = creditorFactionId,
                survivorId = _selectLaborSurvivor?.Invoke() ?? string.Empty,
                laborDays = laborDays,
                startDay = _currentDay(),
                endDay = _currentDay() + laborDays
            };
            _state.laborObligations.Add(record);
            OnLaborObligationCreated?.Invoke(record);
            OnStateChanged?.Invoke();
            _log.Info("DebtLaborObligation { faction=" + creditorFactionId
                + " survivor=" + record.survivorId + " days=" + laborDays
                + " window=" + record.startDay + ".." + record.endDay + " }");
        }

        /// <summary>Daily tick: releases obligations whose bounded window has
        /// closed. Day-derived, idempotent.</summary>
        public void TickDaily(int day)
        {
            for (int i = _state.laborObligations.Count - 1; i >= 0; i--)
            {
                var r = _state.laborObligations[i];
                if (r.released || day < r.endDay) continue;
                r.released = true;
                OnLaborObligationReleased?.Invoke(r);
                _log.Info("DebtLaborReleased { survivor=" + r.survivorId + " source=" + r.sourceId + " }");
            }
        }

        /// <summary>Whether a survivor is currently bound by an unreleased
        /// labor obligation (projections read this; they never recompute it).</summary>
        public bool IsBoundToLabor(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            for (int i = 0; i < _state.laborObligations.Count; i++)
            {
                var r = _state.laborObligations[i];
                if (!r.released && r.survivorId == survivorId) return true;
            }
            return false;
        }

        /// <summary>Defense-in-depth source identity shared by the embargo and
        /// labor ledgers: debt:{debtor}@{day}:{consequence}/{kind}.</summary>
        public static string DebtSourceId(DebtContract contract, string kind)
        {
            // The template's consequence ids are resolved by the dispatcher;
            // the bridge sees requests per consequence chain, so the contract
            // instance identity plus the request kind is the stable key.
            return "debt:" + contract.debtorId + "@" + contract.signedDay + ":" + kind;
        }

        public DebtConsequenceBridgeState CaptureState()
        {
            var copy = new DebtConsequenceBridgeState();
            for (int i = 0; i < _state.laborObligations.Count; i++)
            {
                var r = _state.laborObligations[i];
                if (r == null) continue;
                copy.laborObligations.Add(new DebtLaborObligationRecord
                {
                    sourceId = r.sourceId ?? string.Empty,
                    creditorFactionId = r.creditorFactionId ?? string.Empty,
                    survivorId = r.survivorId ?? string.Empty,
                    laborDays = r.laborDays,
                    startDay = r.startDay,
                    endDay = r.endDay,
                    released = r.released
                });
            }
            return copy;
        }

        public void RestoreState(DebtConsequenceBridgeState saved)
        {
            _state = new DebtConsequenceBridgeState();
            if (saved?.laborObligations == null)
            {
                OnStateChanged?.Invoke();
                return;
            }
            for (int i = 0; i < saved.laborObligations.Count; i++)
            {
                var r = saved.laborObligations[i];
                if (r == null || string.IsNullOrEmpty(r.sourceId)) continue;
                _state.laborObligations.Add(new DebtLaborObligationRecord
                {
                    sourceId = r.sourceId,
                    creditorFactionId = r.creditorFactionId ?? string.Empty,
                    survivorId = r.survivorId ?? string.Empty,
                    laborDays = r.laborDays,
                    startDay = r.startDay,
                    endDay = r.endDay,
                    released = r.released
                });
            }
            OnStateChanged?.Invoke();
        }
    }
}
