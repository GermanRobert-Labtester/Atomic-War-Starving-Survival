// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Economy
{
    public sealed class TradeRouteMonopolyValidationResult
    {
        public bool IsValid { get; }
        public string FailureReason { get; }

        private TradeRouteMonopolyValidationResult(bool isValid, string failureReason)
        {
            IsValid = isValid;
            FailureReason = failureReason ?? string.Empty;
        }

        public static TradeRouteMonopolyValidationResult Valid() =>
            new TradeRouteMonopolyValidationResult(true, string.Empty);

        public static TradeRouteMonopolyValidationResult Invalid(string reason) =>
            new TradeRouteMonopolyValidationResult(false, reason);
    }

    [Serializable]
    public sealed class RouteMarketSaturationState
    {
        public string RouteId { get; set; } = string.Empty;
        public string ExclusiveGoodId { get; set; } = string.Empty;
        public int SaturationPermille { get; set; }
        public int TotalUnitsDelivered { get; set; }
        public int LastDeliveryDay { get; set; }
        public int LastRecoveryDay { get; set; }
    }

    [Serializable]
    public sealed class TradeRouteMonopolySaveState
    {
        public int schema_version { get; set; } = 1;
        public List<RouteMarketSaturationState> RouteStates { get; set; } = new();
    }

    /// <summary>
    /// F13-G / UNBLOCK-02 §4.7 / §5.6 / §17.2:
    /// Tier-4 Exclusive Goods & Trade Monopoly Discipline Engine.
    /// Governs Tier-4 exclusive goods eligibility, strictly-superior item validation,
    /// market saturation price decay curves, daily demand recovery, and cargo quota bounds.
    /// </summary>
    public sealed class TradeRouteMonopolyEngine
    {
        public const int BaseMonopolyPremiumPermille = 250; // +25% base premium at 0 saturation
        public const int SaturationPerUnitDeliveredPermille = 25; // 2.5% saturation per unit
        public const int DailySaturationRecoveryPermille = 50; // 5% recovery per day
        public const int MaxExclusiveUnitsPerRun = 5; // Quota ceiling per caravan run
        public const int UnitsPerCaravanSlot = 3;

        private readonly Dictionary<string, RouteMarketSaturationState> _routeStates = new(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyDictionary<string, RouteMarketSaturationState> RouteStates => _routeStates;

        public event Action<string, string, int>? OnExclusiveRunDelivered;
        public event Action<string, int>? OnMarketSaturationUpdated;

        public TradeRouteMonopolyValidationResult ValidateExclusiveGood(
            TradeRouteContract contract,
            IReadOnlyCollection<string>? forbiddenSuperiorGoods = null)
        {
            if (contract == null)
                return TradeRouteMonopolyValidationResult.Invalid("NullContract");

            if (string.IsNullOrWhiteSpace(contract.ExclusiveGoodId))
                return TradeRouteMonopolyValidationResult.Invalid("NoExclusiveGoodConfigured");

            // Strictly superior check (UNBLOCK-02 §4.7 named-item balance rule)
            if (forbiddenSuperiorGoods != null && forbiddenSuperiorGoods.Contains(contract.ExclusiveGoodId))
                return TradeRouteMonopolyValidationResult.Invalid("ForbiddenStrictlySuperiorGood");

            // Tier 4 gate check
            if (contract.Tier != TradeRouteReliabilityTier.Tier4)
                return TradeRouteMonopolyValidationResult.Invalid("Tier4ReliabilityRequired");

            return TradeRouteMonopolyValidationResult.Valid();
        }

        public int GetCurrentMonopolyPremiumPermille(string routeId)
        {
            if (string.IsNullOrWhiteSpace(routeId)) return 0;
            if (!_routeStates.TryGetValue(routeId, out var state)) return BaseMonopolyPremiumPermille;

            int remainingMarginPermille = Math.Max(0, 1000 - state.SaturationPermille);
            return (int)(((long)BaseMonopolyPremiumPermille * remainingMarginPermille) / 1000L);
        }

        public int CalculateExclusiveGoodUnitValue(int baseUnitValueChits, string routeId)
        {
            if (baseUnitValueChits <= 0) return 0;
            int premiumPermille = GetCurrentMonopolyPremiumPermille(routeId);
            int premiumChits = (int)(((long)baseUnitValueChits * premiumPermille) / 1000L);
            return baseUnitValueChits + premiumChits;
        }

        public int CalculateCargoQuota(TradeRouteContract contract)
        {
            if (contract == null || contract.CaravanSlotsRequired <= 0) return 0;
            return Math.Min(MaxExclusiveUnitsPerRun, contract.CaravanSlotsRequired * UnitsPerCaravanSlot);
        }

        public void RecordDelivery(string routeId, string exclusiveGoodId, int units, int currentDay)
        {
            if (string.IsNullOrWhiteSpace(routeId) || units <= 0) return;

            if (!_routeStates.TryGetValue(routeId, out var state))
            {
                state = new RouteMarketSaturationState
                {
                    RouteId = routeId,
                    ExclusiveGoodId = exclusiveGoodId ?? string.Empty,
                    SaturationPermille = 0,
                    TotalUnitsDelivered = 0,
                    LastDeliveryDay = currentDay,
                    LastRecoveryDay = currentDay
                };
                _routeStates[routeId] = state;
            }

            state.TotalUnitsDelivered += units;
            int saturationIncrease = units * SaturationPerUnitDeliveredPermille;
            state.SaturationPermille = Math.Min(1000, state.SaturationPermille + saturationIncrease);
            state.LastDeliveryDay = currentDay;
            state.LastRecoveryDay = currentDay;

            OnExclusiveRunDelivered?.Invoke(routeId, exclusiveGoodId, units);
            OnMarketSaturationUpdated?.Invoke(routeId, state.SaturationPermille);
        }

        public void ProcessDailyRecovery(int currentDay)
        {
            foreach (var kvp in _routeStates)
            {
                var state = kvp.Value;
                if (currentDay <= state.LastRecoveryDay) continue;

                int elapsedDays = currentDay - state.LastRecoveryDay;
                int recovery = elapsedDays * DailySaturationRecoveryPermille;
                int previousSaturation = state.SaturationPermille;
                state.SaturationPermille = Math.Max(0, state.SaturationPermille - recovery);
                state.LastRecoveryDay = currentDay;

                if (previousSaturation != state.SaturationPermille)
                {
                    OnMarketSaturationUpdated?.Invoke(state.RouteId, state.SaturationPermille);
                }
            }
        }

        public TradeRouteMonopolySaveState CaptureState()
        {
            var save = new TradeRouteMonopolySaveState
            {
                schema_version = 1,
                RouteStates = new List<RouteMarketSaturationState>(_routeStates.Count)
            };

            foreach (var kvp in _routeStates)
            {
                var s = kvp.Value;
                save.RouteStates.Add(new RouteMarketSaturationState
                {
                    RouteId = s.RouteId,
                    ExclusiveGoodId = s.ExclusiveGoodId,
                    SaturationPermille = s.SaturationPermille,
                    TotalUnitsDelivered = s.TotalUnitsDelivered,
                    LastDeliveryDay = s.LastDeliveryDay,
                    LastRecoveryDay = s.LastRecoveryDay
                });
            }

            return save;
        }

        public void RestoreState(TradeRouteMonopolySaveState? state)
        {
            _routeStates.Clear();
            if (state?.RouteStates == null) return;

            for (int i = 0; i < state.RouteStates.Count; i++)
            {
                var s = state.RouteStates[i];
                if (s != null && !string.IsNullOrWhiteSpace(s.RouteId))
                {
                    _routeStates[s.RouteId] = new RouteMarketSaturationState
                    {
                        RouteId = s.RouteId,
                        ExclusiveGoodId = s.ExclusiveGoodId,
                        SaturationPermille = Math.Clamp(s.SaturationPermille, 0, 1000),
                        TotalUnitsDelivered = s.TotalUnitsDelivered,
                        LastDeliveryDay = s.LastDeliveryDay,
                        LastRecoveryDay = s.LastRecoveryDay
                    };
                }
            }
        }
    }
}
