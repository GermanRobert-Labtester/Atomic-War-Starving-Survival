// SPDX-License-Identifier: MIT
// ASHFALL Plan 192 — Trade Route Contracts & Reliability Tiers Host Wiring.

using System;
using Godot;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private TradeRouteHostSession? _tradeRoutes;
        private bool _tradeRoutesDirty;

        public TradeRouteHostSession? TradeRoutes => _tradeRoutes;

        public void SetupTradeRoutes()
        {
            if (_tradeRoutes != null) return;

            _tradeRoutes = TradeRouteHostSession.Create(_dataDir);

            var saved = TradeRouteSaveStore.TryLoad();
            if (saved != null)
            {
                _tradeRoutes.RestoreState(saved);
            }

            _tradeRoutes.StateChanged += () => _tradeRoutesDirty = true;
        }

        public void SaveTradeRoutes()
        {
            if (_tradeRoutes == null) return;
            var state = _tradeRoutes.CaptureState();
            TradeRouteSaveStore.TrySave(state);
            if (CaptureSection("trade_routes", TradeRouteSaveStore.TryCapturePersisted(state)))
            {
                _tradeRoutesDirty = false;
            }
        }

        public void TickTradeRoutes(int day)
        {
            if (_tradeRoutes == null) SetupTradeRoutes();
            _tradeRoutes?.TickDay(day);
        }

        public void FlushTradeRoutesIfDirty()
        {
            if (_tradeRoutesDirty)
            {
                SaveTradeRoutes();
            }
        }

        public void ResetTradeRoutes()
        {
            _tradeRoutes = null;
            _tradeRoutesDirty = false;
        }
    }
}
