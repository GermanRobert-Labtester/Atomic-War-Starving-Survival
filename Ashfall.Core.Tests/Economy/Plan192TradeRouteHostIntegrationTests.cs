// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class Plan192TradeRouteHostIntegrationTests
    {
        [Fact]
        public void Census_ReflectsContractCountsAndOutcomes()
        {
            var system = new PlayerTradeRouteSystem();
            var census0 = system.GetCensus();
            Assert.Equal(0, census0.ActiveContracts);
            Assert.Equal(0, census0.TotalRunsCompleted);
            Assert.Equal(0, census0.TotalRunsFailed);
            Assert.Equal(0, census0.TotalTariffChitsPaid);

            var c1 = new TradeRouteContract("r1", "cp1", 3, 50);
            var c2 = new TradeRouteContract("r2", "cp2", 5, 80);
            system.RegisterContract(c1);
            system.RegisterContract(c2);

            system.RecordRunOutcome("r1", TradeRouteRunOutcome.OnTime, 4, tariffPaid: 50);
            system.RecordRunOutcome("r2", TradeRouteRunOutcome.Failed, 6, tariffPaid: 0);

            var census1 = system.GetCensus();
            Assert.Equal(2, census1.ActiveContracts);
            Assert.Equal(1, census1.TotalRunsCompleted);
            Assert.Equal(1, census1.TotalRunsFailed);
            Assert.Equal(50, census1.TotalTariffChitsPaid);
        }

        [Fact]
        public void RegisterContract_StoresAndRetrieves()
        {
            var system = new PlayerTradeRouteSystem();
            var contract = new TradeRouteContract("r_main", "faction_compact", 7, 120, caravanSlotsRequired: 2);
            bool registered = system.RegisterContract(contract);

            Assert.True(registered);
            Assert.NotNull(system.GetContract("r_main"));
            Assert.Equal("faction_compact", system.GetContract("r_main")!.CounterpartyId);
            Assert.Equal(2, system.GetContract("r_main")!.CaravanSlotsRequired);
        }

        [Fact]
        public void CancelContract_SetsSuspendedAnd30DayCooldown()
        {
            var system = new PlayerTradeRouteSystem();
            var contract = new TradeRouteContract("r_cancel", "faction_scale", 4, 60);
            system.RegisterContract(contract);

            for (int i = 0; i < 5; i++)
            {
                contract.RecordRunOutcome(TradeRouteRunOutcome.OnTime, i * 4);
            }
            Assert.Equal(5, contract.ReliabilityScore);

            bool canceled = system.CancelContract("r_cancel", currentDay: 50);
            Assert.True(canceled);
            Assert.True(contract.IsSuspended);
            Assert.Equal(0, contract.ReliabilityScore);
            Assert.True(contract.IsOnCooldown(60));
            Assert.False(contract.IsOnCooldown(85));
        }

        [Fact]
        public void SuspendAndResume_ModifiesState()
        {
            var system = new PlayerTradeRouteSystem();
            var contract = new TradeRouteContract("r_susp", "faction_scale", 5, 50, establishedDay: 1);
            system.RegisterContract(contract);

            Assert.True(system.SuspendContract("r_susp"));
            Assert.True(contract.IsSuspended);
            Assert.False(contract.CanScheduleRun(6));

            Assert.True(system.ResumeContract("r_susp"));
            Assert.False(contract.IsSuspended);
            Assert.True(contract.CanScheduleRun(6));
        }

        [Fact]
        public void TierProgression_UnlocksDiscountsAndExclusiveGoods()
        {
            var contract = new TradeRouteContract("r_tier", "cp", 2, 100, exclusiveGoodId: "item_secret_alloy");

            // Tier 1
            Assert.Equal(TradeRouteReliabilityTier.Tier1, contract.Tier);
            Assert.Equal(100, contract.EffectiveTariffChits);
            Assert.False(contract.IsExclusiveGoodUnlocked);

            // Tier 2 (threshold: 5)
            for (int i = 1; i <= 5; i++) contract.RecordRunOutcome(TradeRouteRunOutcome.OnTime, i * 2);
            Assert.Equal(TradeRouteReliabilityTier.Tier2, contract.Tier);
            Assert.Equal(100, contract.EffectiveTariffChits);

            // Tier 3 (threshold: 12) -> 25% discount
            for (int i = 6; i <= 12; i++) contract.RecordRunOutcome(TradeRouteRunOutcome.OnTime, i * 2);
            Assert.Equal(TradeRouteReliabilityTier.Tier3, contract.Tier);
            Assert.Equal(75, contract.EffectiveTariffChits);
            Assert.False(contract.IsExclusiveGoodUnlocked);

            // Tier 4 (threshold: 20) -> exclusive good unlocked
            for (int i = 13; i <= 20; i++) contract.RecordRunOutcome(TradeRouteRunOutcome.OnTime, i * 2);
            Assert.Equal(TradeRouteReliabilityTier.Tier4, contract.Tier);
            Assert.Equal(75, contract.EffectiveTariffChits);
            Assert.True(contract.IsExclusiveGoodUnlocked);
        }

        [Fact]
        public void SaveAndRestore_PreservesAllActiveContractsAndTariffs()
        {
            var system = new PlayerTradeRouteSystem();
            var c1 = new TradeRouteContract("route_one", "cp1", 5, 80, establishedDay: 2);
            var c2 = new TradeRouteContract("route_two", "cp2", 10, 150, exclusiveGoodId: "item_relic_core", establishedDay: 1);

            system.RegisterContract(c1);
            system.RegisterContract(c2);

            system.RecordRunOutcome("route_one", TradeRouteRunOutcome.OnTime, 7, tariffPaid: 80);
            system.RecordRunOutcome("route_two", TradeRouteRunOutcome.Failed, 11, tariffPaid: 0);

            var state = system.CaptureState();
            Assert.Equal(80, state.TotalTariffChitsPaid);
            Assert.Equal(2, state.Contracts.Count);

            var restored = new PlayerTradeRouteSystem();
            restored.RestoreState(state);

            Assert.Equal(80, restored.TotalTariffChitsPaid);
            var r1 = restored.GetContract("route_one");
            var r2 = restored.GetContract("route_two");
            Assert.NotNull(r1);
            Assert.NotNull(r2);
            Assert.Equal(1, r1!.RunsCompleted);
            Assert.Equal(1, r2!.RunsFailed);
            Assert.Equal("item_relic_core", r2.ExclusiveGoodId);
        }

        [Fact]
        public void CorruptSaveState_HandledGracefully()
        {
            var system = new PlayerTradeRouteSystem();
            system.RestoreState(null);
            Assert.Empty(system.Contracts);
            Assert.Equal(0, system.TotalTariffChitsPaid);

            var emptyState = new PlayerTradeRouteSaveState();
            system.RestoreState(emptyState);
            Assert.Empty(system.Contracts);
            Assert.Equal(0, system.TotalTariffChitsPaid);
        }
    }
}
