// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : TradeRouteSaveStore
// Core State : Ashfall.Core.Economy.PlayerTradeRouteSaveState
// Host Caller: Main.TradeRoutes (SetupTradeRoutes / SaveTradeRoutes)
// Purpose    : Plan 192 — Scheduled trade route contracts, reliability tiers,
//              tariffs, exclusive goods, and cancel cooldown.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Economy;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class TradeRouteSaveStore
    {
        public const string FileName = "trade_routes_save.json";
        public const string SectionName = "trade_routes";

        private static readonly SaveStore<PlayerTradeRouteSaveState> s_store =
            SaveStoreHub.Checksummed<PlayerTradeRouteSaveState>(FileName, nameof(TradeRouteSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(PlayerTradeRouteSaveState state) => s_store.CaptureBare(state);
        public static PlayerTradeRouteSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(PlayerTradeRouteSaveState state) => s_store.TrySave(state);
        public static PlayerTradeRouteSaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host session manager for Plan 192 (Trade Route Contracts & Reliability Tiers).
    /// Binds route templates from caravan_trade_routes.json, manages player scheduled
    /// contracts, checks daily run cadence, debits tariffs from FundsLedger, and tracks reliability.
    /// </summary>
    public sealed class TradeRouteHostSession : HostSessionBase
    {
        private readonly PlayerTradeRouteSystem _system;
        private readonly List<CaravanRouteDefinition> _availableRoutes = new();
        private string _lastEvent = string.Empty;

        public PlayerTradeRouteSystem System => _system;
        public string LastEvent => _lastEvent;
        public TradeRouteCensus Census => _system.GetCensus();
        public IReadOnlyList<CaravanRouteDefinition> AvailableRoutes => _availableRoutes;
        public IReadOnlyCollection<TradeRouteContract> Contracts => _system.Contracts;

        public TradeRouteHostSession(string? dataDir = null, PlayerTradeRouteSystem? system = null)
        {
            _system = system ?? new PlayerTradeRouteSystem();

            if (!string.IsNullOrEmpty(dataDir))
            {
                LoadCatalogs(dataDir);
            }
        }

        public static TradeRouteHostSession Create(string dataDir, PlayerTradeRouteSystem? system = null)
        {
            return new TradeRouteHostSession(dataDir, system);
        }

        public void LoadCatalogs(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;

            try
            {
                var routes = CaravanTradeRouteCatalogLoader.Load(dataDir);
                _availableRoutes.Clear();
                if (routes != null)
                {
                    _availableRoutes.AddRange(routes);
                }
                _lastEvent = $"Loaded {_availableRoutes.Count} caravan route definitions.";
                RaiseStateChanged();
            }
            catch (Exception ex)
            {
                _lastEvent = $"Failed to load trade route catalog: {ex.Message}";
            }
        }

        public bool EstablishContract(
            string routeId,
            string counterpartyId,
            int cadenceDays,
            int baseTariffChits,
            int caravanSlotsRequired = 1,
            string? exclusiveGoodId = null,
            int establishedDay = 1,
            IEnumerable<TradeRouteGoodLeg>? goodsOut = null,
            IEnumerable<TradeRouteGoodLeg>? goodsIn = null)
        {
            var contract = new TradeRouteContract(
                routeId,
                counterpartyId,
                cadenceDays,
                baseTariffChits,
                caravanSlotsRequired,
                exclusiveGoodId,
                establishedDay,
                goodsOut,
                goodsIn);

            bool ok = _system.RegisterContract(contract);
            if (ok)
            {
                _lastEvent = $"Contract established for route '{routeId}' with '{counterpartyId}'.";
                RaiseStateChanged();
            }
            return ok;
        }

        public bool RegisterContract(TradeRouteContract contract)
        {
            bool ok = _system.RegisterContract(contract);
            if (ok)
            {
                _lastEvent = $"Registered contract '{contract.RouteId}'.";
                RaiseStateChanged();
            }
            return ok;
        }

        public TradeRouteContract? GetContract(string routeId) => _system.GetContract(routeId);

        public bool CancelContract(string routeId, int currentDay)
        {
            bool ok = _system.CancelContract(routeId, currentDay);
            if (ok)
            {
                _lastEvent = $"Canceled contract '{routeId}' on day {currentDay} (30-day cooldown).";
                RaiseStateChanged();
            }
            return ok;
        }

        public bool SuspendContract(string routeId)
        {
            bool ok = _system.SuspendContract(routeId);
            if (ok)
            {
                _lastEvent = $"Suspended contract '{routeId}'.";
                RaiseStateChanged();
            }
            return ok;
        }

        public bool ResumeContract(string routeId)
        {
            bool ok = _system.ResumeContract(routeId);
            if (ok)
            {
                _lastEvent = $"Resumed contract '{routeId}'.";
                RaiseStateChanged();
            }
            return ok;
        }

        /// <summary>
        /// Daily tick for trade routes.
        /// Evaluates active contracts: if run cadence is met, checks tariff ability
        /// via optional FundsLedger, advances reliability outcome, and updates next run day.
        /// </summary>
        public void TickDay(int currentDay, FundsLedger? funds = null)
        {
            bool anyTicked = false;
            foreach (var contract in _system.Contracts)
            {
                if (contract.CanScheduleRun(currentDay))
                {
                    int tariff = contract.EffectiveTariffChits;
                    bool paid = true;

                    if (funds != null && tariff > 0)
                    {
                        paid = funds.TryDebit(tariff, FundsLedger.ReasonRouteTariff, contract.RouteId, currentDay).Success;
                    }

                    if (paid)
                    {
                        _system.RecordRunOutcome(contract.RouteId, TradeRouteRunOutcome.OnTime, currentDay, tariff);
                        _lastEvent = $"Route '{contract.RouteId}' run completed on time (tariff: {tariff} chits).";
                    }
                    else
                    {
                        _system.RecordRunOutcome(contract.RouteId, TradeRouteRunOutcome.Late, currentDay, 0);
                        _lastEvent = $"Route '{contract.RouteId}' delayed due to insufficient tariff funds.";
                    }

                    anyTicked = true;
                }
            }

            if (anyTicked)
            {
                RaiseStateChanged();
            }
        }

        public PlayerTradeRouteSaveState CaptureState() => _system.CaptureState();

        public void RestoreState(PlayerTradeRouteSaveState? state)
        {
            _system.RestoreState(state);
            RaiseStateChanged();
        }

        public void Reset()
        {
            _system.Reset();
            _availableRoutes.Clear();
            _lastEvent = string.Empty;
            RaiseStateChanged();
        }
    }
}
