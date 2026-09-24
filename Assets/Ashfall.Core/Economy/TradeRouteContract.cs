// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Economy
{
    public enum TradeRouteReliabilityTier
    {
        Tier1 = 1,
        Tier2 = 2,
        Tier3 = 3,
        Tier4 = 4
    }

    public enum TradeRouteRunOutcome
    {
        OnTime = 1,
        Late = 2,
        Failed = 3
    }

    public struct TradeRouteCensus
    {
        public int ActiveContracts;
        public int TotalRunsCompleted;
        public int TotalRunsFailed;
        public int TotalTariffChitsPaid;
    }

    [Serializable]
    public sealed class TradeRouteGoodLeg
    {
        public string ItemId { get; set; } = string.Empty;
        public int UnitsPerRun { get; set; }
    }

    [Serializable]
    public sealed class TradeRouteContractSaveState
    {
        public int schema_version { get; set; } = 1;
        public string RouteId { get; set; } = string.Empty;
        public string CounterpartyId { get; set; } = string.Empty;
        public int CadenceDays { get; set; }
        public int BaseTariffChits { get; set; }
        public int CaravanSlotsRequired { get; set; } = 1;
        public string? ExclusiveGoodId { get; set; }
        public int EstablishedDay { get; set; }
        public int ReliabilityScore { get; set; }
        public int RunsCompleted { get; set; }
        public int RunsFailed { get; set; }
        public int NextRunDay { get; set; }
        public bool IsSuspended { get; set; }
        public int LastCanceledDay { get; set; } = -1;
        public List<TradeRouteGoodLeg> GoodsOut { get; set; } = new();
        public List<TradeRouteGoodLeg> GoodsIn { get; set; } = new();
    }

    /// <summary>
    /// Plan 192 / XP-08 Route Contract and Reliability Tier Engine.
    /// Manages player-side scheduled trade route contracts, tariffs in chits,
    /// reliability progression, tier thresholds, cooldown, and persistence.
    /// </summary>
    public sealed class TradeRouteContract
    {
        public const int CooldownDaysAfterCancel = 30;
        public const int Tier2Threshold = 5;
        public const int Tier3Threshold = 12;
        public const int Tier4Threshold = 20;

        public string RouteId { get; }
        public string CounterpartyId { get; }
        public int CadenceDays { get; }
        public int BaseTariffChits { get; }
        public int CaravanSlotsRequired { get; }
        public string? ExclusiveGoodId { get; }
        public int EstablishedDay { get; }

        public int ReliabilityScore { get; private set; }
        public int RunsCompleted { get; private set; }
        public int RunsFailed { get; private set; }
        public int NextRunDay { get; private set; }
        public bool IsSuspended { get; private set; }
        public int LastCanceledDay { get; private set; } = -1;

        public IReadOnlyList<TradeRouteGoodLeg> GoodsOut => _goodsOut;
        public IReadOnlyList<TradeRouteGoodLeg> GoodsIn => _goodsIn;

        private readonly List<TradeRouteGoodLeg> _goodsOut = new();
        private readonly List<TradeRouteGoodLeg> _goodsIn = new();

        public TradeRouteContract(
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
            if (string.IsNullOrWhiteSpace(routeId))
                throw new ArgumentException("RouteId cannot be empty.", nameof(routeId));
            if (string.IsNullOrWhiteSpace(counterpartyId))
                throw new ArgumentException("CounterpartyId cannot be empty.", nameof(counterpartyId));
            if (cadenceDays <= 0)
                throw new ArgumentOutOfRangeException(nameof(cadenceDays), "CadenceDays must be positive.");
            if (baseTariffChits < 0)
                throw new ArgumentOutOfRangeException(nameof(baseTariffChits), "BaseTariffChits cannot be negative.");
            if (caravanSlotsRequired <= 0)
                throw new ArgumentOutOfRangeException(nameof(caravanSlotsRequired), "CaravanSlotsRequired must be positive.");

            RouteId = routeId;
            CounterpartyId = counterpartyId;
            CadenceDays = cadenceDays;
            BaseTariffChits = baseTariffChits;
            CaravanSlotsRequired = caravanSlotsRequired;
            ExclusiveGoodId = exclusiveGoodId;
            EstablishedDay = establishedDay;
            NextRunDay = establishedDay + cadenceDays;

            if (goodsOut != null) _goodsOut.AddRange(goodsOut);
            if (goodsIn != null) _goodsIn.AddRange(goodsIn);
        }

        public TradeRouteReliabilityTier Tier
        {
            get
            {
                if (ReliabilityScore >= Tier4Threshold) return TradeRouteReliabilityTier.Tier4;
                if (ReliabilityScore >= Tier3Threshold) return TradeRouteReliabilityTier.Tier3;
                if (ReliabilityScore >= Tier2Threshold) return TradeRouteReliabilityTier.Tier2;
                return TradeRouteReliabilityTier.Tier1;
            }
        }

        public int EffectiveTariffChits
        {
            get
            {
                if (Tier >= TradeRouteReliabilityTier.Tier3)
                {
                    // 25% discount, rounded to nearest integer, floored at 0
                    return Math.Max(0, (int)Math.Round(BaseTariffChits * 0.75f));
                }
                return BaseTariffChits;
            }
        }

        public bool IsExclusiveGoodUnlocked => Tier == TradeRouteReliabilityTier.Tier4 && !string.IsNullOrEmpty(ExclusiveGoodId);

        public bool IsOnCooldown(int currentDay)
        {
            if (LastCanceledDay < 0) return false;
            return currentDay - LastCanceledDay < CooldownDaysAfterCancel;
        }

        public bool CanScheduleRun(int currentDay)
        {
            if (IsSuspended) return false;
            if (IsOnCooldown(currentDay)) return false;
            return currentDay >= NextRunDay;
        }

        public void RecordRunOutcome(TradeRouteRunOutcome outcome, int currentDay)
        {
            switch (outcome)
            {
                case TradeRouteRunOutcome.OnTime:
                    ReliabilityScore += 1;
                    RunsCompleted += 1;
                    break;
                case TradeRouteRunOutcome.Late:
                    ReliabilityScore = Math.Max(0, ReliabilityScore - 1);
                    RunsCompleted += 1;
                    break;
                case TradeRouteRunOutcome.Failed:
                    ReliabilityScore = Math.Max(0, ReliabilityScore - 2);
                    RunsFailed += 1;
                    break;
            }

            NextRunDay = currentDay + CadenceDays;
        }

        public void Suspend()
        {
            IsSuspended = true;
        }

        public void Resume()
        {
            IsSuspended = false;
        }

        public void Cancel(int currentDay)
        {
            LastCanceledDay = currentDay;
            ReliabilityScore = 0;
            IsSuspended = true;
        }

        public TradeRouteContractSaveState CaptureState()
        {
            return new TradeRouteContractSaveState
            {
                schema_version = 1,
                RouteId = RouteId,
                CounterpartyId = CounterpartyId,
                CadenceDays = CadenceDays,
                BaseTariffChits = BaseTariffChits,
                CaravanSlotsRequired = CaravanSlotsRequired,
                ExclusiveGoodId = ExclusiveGoodId,
                EstablishedDay = EstablishedDay,
                ReliabilityScore = ReliabilityScore,
                RunsCompleted = RunsCompleted,
                RunsFailed = RunsFailed,
                NextRunDay = NextRunDay,
                IsSuspended = IsSuspended,
                LastCanceledDay = LastCanceledDay,
                GoodsOut = new List<TradeRouteGoodLeg>(_goodsOut),
                GoodsIn = new List<TradeRouteGoodLeg>(_goodsIn)
            };
        }

        public static TradeRouteContract RestoreState(TradeRouteContractSaveState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            var contract = new TradeRouteContract(
                state.RouteId,
                state.CounterpartyId,
                state.CadenceDays,
                state.BaseTariffChits,
                state.CaravanSlotsRequired > 0 ? state.CaravanSlotsRequired : 1,
                state.ExclusiveGoodId,
                state.EstablishedDay,
                state.GoodsOut,
                state.GoodsIn)
            {
                ReliabilityScore = Math.Max(0, state.ReliabilityScore),
                RunsCompleted = state.RunsCompleted,
                RunsFailed = state.RunsFailed,
                NextRunDay = state.NextRunDay,
                IsSuspended = state.IsSuspended,
                LastCanceledDay = state.LastCanceledDay
            };

            return contract;
        }
    }
}
