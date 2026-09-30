// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 213 — Survivor barter & informal economy host composition. The Core
// SurvivorBarterSystem remains the sole offer/trade/reputation/favor authority;
// item custody stays with Inventory. This partial composes the authored rule
// catalog, save section, and the canonical campaign day tick.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private SurvivorBarterHostSession? _barter;
        private bool _barterDirty;

        public SurvivorBarterHostSession? SurvivorBarterSession => _barter;

        public void SetupSurvivorBarter()
        {
            if (_barter != null) return;
            var saved = SurvivorBarterSaveStore.TryLoad();
            _barter = SurvivorBarterHostSession.Create(saved);
            _barter.StateChanged += () => _barterDirty = true;
            _barter.LoadCatalog(CatalogPath.ResolveDataDir());
        }

        public string ActivateBarterRule(string ruleId)
        {
            SetupSurvivorBarter();
            return _barter!.SetActiveRule(ruleId);
        }

        public BarterOffer? CreateSurvivorBarterOffer(
            string offererId, string targetId,
            IEnumerable<string>? offeredItems = null, IEnumerable<string>? requestedItems = null)
        {
            SetupSurvivorBarter();
            int day = _campaignDay?.Calendar.CurrentDay ?? _simDay;
            var offer = _barter!.CreateOffer(offererId, targetId, day, offeredItems, requestedItems);
            if (offer != null) _barterDirty = true;
            return offer;
        }

        public CompletedBarterTrade? AcceptSurvivorBarterOffer(string offerId)
        {
            SetupSurvivorBarter();
            int day = _campaignDay?.Calendar.CurrentDay ?? _simDay;
            var trade = _barter!.AcceptOffer(offerId, day);
            if (trade != null) _barterDirty = true;
            return trade;
        }

        public bool RejectSurvivorBarterOffer(string offerId)
        {
            SetupSurvivorBarter();
            bool ok = _barter!.RejectOffer(offerId);
            if (ok) _barterDirty = true;
            return ok;
        }

        public bool RaiseBarterDispute(string tradeId, string complainantId)
        {
            SetupSurvivorBarter();
            bool ok = _barter!.RaiseDispute(tradeId, complainantId);
            if (ok) _barterDirty = true;
            return ok;
        }

        public TradeReputation GetBarterReputation(string survivorA, string survivorB)
        {
            SetupSurvivorBarter();
            return _barter!.GetReputation(survivorA, survivorB);
        }

        public void TickSurvivorBarter(int day)
        {
            SetupSurvivorBarter();
            if (_barter == null) return;
            _barter.TickDay(day);
            _barterDirty = true;
        }

        public (int Offers, int Trades) GetBarterReadout()
        {
            SetupSurvivorBarter();
            if (_barter == null) return (0, 0);
            return (_barter.ActiveOfferCount, _barter.CompletedTradeCount);
        }

        public void SaveSurvivorBarter()
        {
            if (_barter == null) return;
            var state = _barter.CaptureState();
            if (CaptureSection(SurvivorBarterSaveStore.SectionName, SurvivorBarterSaveStore.TryCapturePersisted(state)))
                _barterDirty = false;
        }

        public void FlushSurvivorBarterIfDirty()
        {
            if (_barterDirty) SaveSurvivorBarter();
        }

        public void ResetSurvivorBarter()
        {
            _barter = null;
            _barterDirty = false;
        }
    }
}
