// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Economy
{
    [Serializable]
    public sealed class PlayerTradeRouteSaveState
    {
        public int schema_version { get; set; } = 1;
        public List<TradeRouteContractSaveState> Contracts { get; set; } = new();
        public int TotalTariffChitsPaid { get; set; }
    }

    /// <summary>
    /// Plan 192 / XP-08 Player Trade Route System.
    /// Manages active player-established scheduled trade route contracts,
    /// progression, reliability tracking, tariff accounting, and save persistence.
    /// </summary>
    public sealed class PlayerTradeRouteSystem
    {
        private readonly Dictionary<string, TradeRouteContract> _contracts = new(StringComparer.OrdinalIgnoreCase);

        public int TotalTariffChitsPaid { get; private set; }

        public IReadOnlyCollection<TradeRouteContract> Contracts => _contracts.Values;

        public TradeRouteCensus GetCensus()
        {
            int completed = 0;
            int failed = 0;
            foreach (var contract in _contracts.Values)
            {
                completed += contract.RunsCompleted;
                failed += contract.RunsFailed;
            }

            return new TradeRouteCensus
            {
                ActiveContracts = _contracts.Count,
                TotalRunsCompleted = completed,
                TotalRunsFailed = failed,
                TotalTariffChitsPaid = TotalTariffChitsPaid
            };
        }

        public bool RegisterContract(TradeRouteContract contract)
        {
            if (contract == null || string.IsNullOrWhiteSpace(contract.RouteId))
                return false;

            _contracts[contract.RouteId] = contract;
            return true;
        }

        public TradeRouteContract? GetContract(string routeId)
        {
            if (string.IsNullOrWhiteSpace(routeId))
                return null;

            return _contracts.TryGetValue(routeId, out var contract) ? contract : null;
        }

        public bool RemoveContract(string routeId)
        {
            if (string.IsNullOrWhiteSpace(routeId))
                return false;

            return _contracts.Remove(routeId);
        }

        public bool CancelContract(string routeId, int currentDay)
        {
            var contract = GetContract(routeId);
            if (contract == null) return false;

            contract.Cancel(currentDay);
            return true;
        }

        public bool SuspendContract(string routeId)
        {
            var contract = GetContract(routeId);
            if (contract == null) return false;

            contract.Suspend();
            return true;
        }

        public bool ResumeContract(string routeId)
        {
            var contract = GetContract(routeId);
            if (contract == null) return false;

            contract.Resume();
            return true;
        }

        public void RecordRunOutcome(string routeId, TradeRouteRunOutcome outcome, int currentDay, int tariffPaid = 0)
        {
            var contract = GetContract(routeId);
            if (contract != null)
            {
                contract.RecordRunOutcome(outcome, currentDay);
                if (tariffPaid > 0)
                {
                    TotalTariffChitsPaid += tariffPaid;
                }
            }
        }

        public PlayerTradeRouteSaveState CaptureState()
        {
            var state = new PlayerTradeRouteSaveState
            {
                schema_version = 1,
                TotalTariffChitsPaid = TotalTariffChitsPaid
            };

            foreach (var contract in _contracts.Values)
            {
                state.Contracts.Add(contract.CaptureState());
            }

            return state;
        }

        public void RestoreState(PlayerTradeRouteSaveState? state)
        {
            if (state == null) return;

            _contracts.Clear();
            TotalTariffChitsPaid = state.TotalTariffChitsPaid;

            if (state.Contracts != null)
            {
                foreach (var contractState in state.Contracts)
                {
                    if (contractState != null && !string.IsNullOrWhiteSpace(contractState.RouteId))
                    {
                        var contract = TradeRouteContract.RestoreState(contractState);
                        _contracts[contract.RouteId] = contract;
                    }
                }
            }
        }

        public void Reset()
        {
            _contracts.Clear();
            TotalTariffChitsPaid = 0;
        }
    }
}
