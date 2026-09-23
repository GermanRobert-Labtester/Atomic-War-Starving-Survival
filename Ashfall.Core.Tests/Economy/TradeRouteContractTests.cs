// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class TradeRouteContractTests
    {
        [Fact]
        public void InitialState_DefaultsToTier1_AndExpectedScheduling()
        {
            var contract = new TradeRouteContract(
                routeId: "route_shelter_to_krasthold",
                counterpartyId: "holdfast_krasthold",
                cadenceDays: 5,
                baseTariffChits: 100,
                caravanSlotsRequired: 2,
                exclusiveGoodId: "item_krasthold_vodka",
                establishedDay: 10,
                goodsOut: new[] { new TradeRouteGoodLeg { ItemId = "item_iodine", UnitsPerRun = 6 } },
                goodsIn: new[] { new TradeRouteGoodLeg { ItemId = "item_grain_sack", UnitsPerRun = 10 } });

            Assert.Equal("route_shelter_to_krasthold", contract.RouteId);
            Assert.Equal("holdfast_krasthold", contract.CounterpartyId);
            Assert.Equal(5, contract.CadenceDays);
            Assert.Equal(100, contract.BaseTariffChits);
            Assert.Equal(100, contract.EffectiveTariffChits);
            Assert.Equal(2, contract.CaravanSlotsRequired);
            Assert.Equal("item_krasthold_vodka", contract.ExclusiveGoodId);
            Assert.Equal(10, contract.EstablishedDay);
            Assert.Equal(15, contract.NextRunDay);
            Assert.Equal(0, contract.ReliabilityScore);
            Assert.Equal(0, contract.RunsCompleted);
            Assert.Equal(0, contract.RunsFailed);
            Assert.Equal(TradeRouteReliabilityTier.Tier1, contract.Tier);
            Assert.False(contract.IsExclusiveGoodUnlocked);
            Assert.False(contract.IsSuspended);
            Assert.False(contract.IsOnCooldown(10));
            Assert.True(contract.CanScheduleRun(15));
            Assert.False(contract.CanScheduleRun(14));
            Assert.Single(contract.GoodsOut);
            Assert.Single(contract.GoodsIn);
        }

        [Fact]
        public void ReliabilityOutcomes_AndTierProgression_WorkCorrectly()
        {
            var contract = new TradeRouteContract("r1", "cp1", 3, 100, establishedDay: 1);

            // Tier 1 initially
            Assert.Equal(TradeRouteReliabilityTier.Tier1, contract.Tier);

            // 5 OnTime runs -> score 5 -> Tier 2
            for (int i = 1; i <= 5; i++)
            {
                contract.RecordRunOutcome(TradeRouteRunOutcome.OnTime, currentDay: i * 3);
            }
            Assert.Equal(5, contract.ReliabilityScore);
            Assert.Equal(5, contract.RunsCompleted);
            Assert.Equal(TradeRouteReliabilityTier.Tier2, contract.Tier);
            Assert.Equal(100, contract.EffectiveTariffChits);

            // 7 more OnTime runs -> score 12 -> Tier 3
            for (int i = 6; i <= 12; i++)
            {
                contract.RecordRunOutcome(TradeRouteRunOutcome.OnTime, currentDay: i * 3);
            }
            Assert.Equal(12, contract.ReliabilityScore);
            Assert.Equal(12, contract.RunsCompleted);
            Assert.Equal(TradeRouteReliabilityTier.Tier3, contract.Tier);
            // 25% discount on 100 chits -> 75 chits
            Assert.Equal(75, contract.EffectiveTariffChits);

            // 8 more OnTime runs -> score 20 -> Tier 4
            for (int i = 13; i <= 20; i++)
            {
                contract.RecordRunOutcome(TradeRouteRunOutcome.OnTime, currentDay: i * 3);
            }
            Assert.Equal(20, contract.ReliabilityScore);
            Assert.Equal(20, contract.RunsCompleted);
            Assert.Equal(TradeRouteReliabilityTier.Tier4, contract.Tier);
            Assert.Equal(75, contract.EffectiveTariffChits);

            // Late run -> -1 penalty
            contract.RecordRunOutcome(TradeRouteRunOutcome.Late, currentDay: 65);
            Assert.Equal(19, contract.ReliabilityScore);
            Assert.Equal(TradeRouteReliabilityTier.Tier3, contract.Tier);

            // Failed run -> -2 penalty
            contract.RecordRunOutcome(TradeRouteRunOutcome.Failed, currentDay: 70);
            Assert.Equal(17, contract.ReliabilityScore);
            Assert.Equal(1, contract.RunsFailed);
            Assert.Equal(TradeRouteReliabilityTier.Tier3, contract.Tier);
        }

        [Fact]
        public void ReliabilityScore_FloorsAtZero()
        {
            var contract = new TradeRouteContract("r1", "cp1", 5, 50);
            Assert.Equal(0, contract.ReliabilityScore);

            contract.RecordRunOutcome(TradeRouteRunOutcome.Failed, currentDay: 10);
            Assert.Equal(0, contract.ReliabilityScore);

            contract.RecordRunOutcome(TradeRouteRunOutcome.Late, currentDay: 15);
            Assert.Equal(0, contract.ReliabilityScore);
        }

        [Fact]
        public void ExclusiveGood_UnlocksOnlyAtTier4()
        {
            var contract = new TradeRouteContract("r1", "cp1", 5, 50, exclusiveGoodId: "special_ammo");
            Assert.False(contract.IsExclusiveGoodUnlocked);

            // Bring to Tier 3 (12 score)
            for (int i = 0; i < 12; i++)
            {
                contract.RecordRunOutcome(TradeRouteRunOutcome.OnTime, i * 5);
            }
            Assert.Equal(TradeRouteReliabilityTier.Tier3, contract.Tier);
            Assert.False(contract.IsExclusiveGoodUnlocked);

            // Bring to Tier 4 (20 score)
            for (int i = 12; i < 20; i++)
            {
                contract.RecordRunOutcome(TradeRouteRunOutcome.OnTime, i * 5);
            }
            Assert.Equal(TradeRouteReliabilityTier.Tier4, contract.Tier);
            Assert.True(contract.IsExclusiveGoodUnlocked);
        }

        [Fact]
        public void Cancellation_Triggers30DayCooldown_AndResetsReliability()
        {
            var contract = new TradeRouteContract("r1", "cp1", 5, 50);
            for (int i = 0; i < 10; i++)
            {
                contract.RecordRunOutcome(TradeRouteRunOutcome.OnTime, i * 5);
            }
            Assert.Equal(10, contract.ReliabilityScore);

            contract.Cancel(currentDay: 50);
            Assert.Equal(0, contract.ReliabilityScore);
            Assert.True(contract.IsSuspended);
            Assert.True(contract.IsOnCooldown(50));
            Assert.True(contract.IsOnCooldown(79));
            Assert.False(contract.IsOnCooldown(80)); // 30 days elapsed
            Assert.False(contract.CanScheduleRun(80)); // Suspended
        }

        [Fact]
        public void SaveAndRestore_PreservesAllState()
        {
            var original = new TradeRouteContract(
                routeId: "route_test",
                counterpartyId: "faction_test",
                cadenceDays: 7,
                baseTariffChits: 80,
                caravanSlotsRequired: 3,
                exclusiveGoodId: "item_rare_metal",
                establishedDay: 5,
                goodsOut: new[] { new TradeRouteGoodLeg { ItemId = "item_food", UnitsPerRun = 20 } },
                goodsIn: new[] { new TradeRouteGoodLeg { ItemId = "item_fuel", UnitsPerRun = 15 } });

            for (int i = 0; i < 15; i++)
            {
                original.RecordRunOutcome(TradeRouteRunOutcome.OnTime, 5 + (i * 7));
            }
            original.RecordRunOutcome(TradeRouteRunOutcome.Failed, 120);

            var state = original.CaptureState();
            var restored = TradeRouteContract.RestoreState(state);

            Assert.Equal(original.RouteId, restored.RouteId);
            Assert.Equal(original.CounterpartyId, restored.CounterpartyId);
            Assert.Equal(original.CadenceDays, restored.CadenceDays);
            Assert.Equal(original.BaseTariffChits, restored.BaseTariffChits);
            Assert.Equal(original.EffectiveTariffChits, restored.EffectiveTariffChits);
            Assert.Equal(original.CaravanSlotsRequired, restored.CaravanSlotsRequired);
            Assert.Equal(original.ExclusiveGoodId, restored.ExclusiveGoodId);
            Assert.Equal(original.EstablishedDay, restored.EstablishedDay);
            Assert.Equal(original.ReliabilityScore, restored.ReliabilityScore);
            Assert.Equal(original.RunsCompleted, restored.RunsCompleted);
            Assert.Equal(original.RunsFailed, restored.RunsFailed);
            Assert.Equal(original.NextRunDay, restored.NextRunDay);
            Assert.Equal(original.Tier, restored.Tier);
            Assert.Equal(original.IsSuspended, restored.IsSuspended);
            Assert.Equal(original.LastCanceledDay, restored.LastCanceledDay);
            Assert.Equal(original.GoodsOut.Count, restored.GoodsOut.Count);
            Assert.Equal(original.GoodsIn.Count, restored.GoodsIn.Count);
            Assert.Equal(original.GoodsOut[0].ItemId, restored.GoodsOut[0].ItemId);
            Assert.Equal(original.GoodsOut[0].UnitsPerRun, restored.GoodsOut[0].UnitsPerRun);
        }
    }
}
