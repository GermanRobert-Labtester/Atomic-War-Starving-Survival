// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Culture;
using Xunit;

namespace Ashfall.Core.Tests.Culture
{
    public class ShelterFestivalEngineTests
    {
        [Fact]
        public void ScheduleFestival_InitializesPlanWithCommodities()
        {
            var engine = new ShelterFestivalEngine();
            var plan = engine.ScheduleFestival(FestivalType.HarvestCommunion, "Autumn Harvest Feast", plannedDay: 15, durationDays: 2);

            Assert.NotNull(plan);
            Assert.Equal(FestivalType.HarvestCommunion, plan.Type);
            Assert.Equal("Autumn Harvest Feast", plan.Title);
            Assert.Equal(15, plan.PlannedDay);
            Assert.Equal(2, plan.DurationDays);
            Assert.Equal(150, plan.MoraleBoostPermille);
            Assert.Equal(200, plan.DespairReductionPermille);
            Assert.NotEmpty(plan.RequiredCommodities);
            Assert.Single(engine.Festivals);
        }

        [Fact]
        public void TryCommenceFestival_ConsumesCommodities_RejectsCurrency()
        {
            var engine = new ShelterFestivalEngine();
            var plan = engine.ScheduleFestival(FestivalType.RemembranceVigil, "Vigil of the Lost", plannedDay: 10);

            var consumedItems = new Dictionary<string, int>();
            bool TryConsume(string item, int amount)
            {
                consumedItems[item] = amount;
                return true;
            }

            FestivalPlan? commencedEventPlan = null;
            engine.OnFestivalCommenced += p => commencedEventPlan = p;

            bool success = engine.TryCommenceFestival(plan.FestivalId, currentDay: 10, TryConsume);
            Assert.True(success);
            Assert.True(plan.IsActive);
            Assert.NotNull(commencedEventPlan);
            Assert.True(consumedItems.ContainsKey("item_fuel"));

            // Reject festival that includes currency
            var illegalCustom = new List<FestivalCommodityRequirement>
            {
                new FestivalCommodityRequirement("item_chits", 50)
            };
            var illegalPlan = engine.ScheduleFestival(FestivalType.FoundingJubilee, "Corrupt Jubilee", 20, 1, illegalCustom);
            bool illegalSuccess = engine.TryCommenceFestival(illegalPlan.FestivalId, currentDay: 20, TryConsume);
            Assert.False(illegalSuccess);
            Assert.False(illegalPlan.IsActive);
        }

        [Fact]
        public void ProcessDailyTick_AdvancesDaysAndCompletes()
        {
            var engine = new ShelterFestivalEngine();
            var plan = engine.ScheduleFestival(FestivalType.MidwinterSolstice, "Yule Hearth", 50, durationDays: 2);

            engine.TryCommenceFestival(plan.FestivalId, 50, (item, amt) => true);
            Assert.True(plan.IsActive);
            Assert.Equal(1, plan.DaysActive);

            FestivalPlan? completedPlan = null;
            engine.OnFestivalCompleted += p => completedPlan = p;

            // Day 51: Second active day
            engine.ProcessDailyTick(51);
            Assert.True(plan.IsActive);
            Assert.Equal(2, plan.DaysActive);
            Assert.Null(completedPlan);

            // Day 52: Third day -> Duration (2) exceeded, completes!
            engine.ProcessDailyTick(52);
            Assert.False(plan.IsActive);
            Assert.True(plan.IsCompleted);
            Assert.NotNull(completedPlan);
            Assert.Equal(plan.FestivalId, completedPlan.FestivalId);
        }

        [Fact]
        public void CancelFestival_RemovesPlan()
        {
            var engine = new ShelterFestivalEngine();
            var plan = engine.ScheduleFestival(FestivalType.FoundingJubilee, "Cancelled Jubilee", 100);

            Assert.Single(engine.Festivals);
            bool cancelled = engine.CancelFestival(plan.FestivalId);
            Assert.True(cancelled);
            Assert.Empty(engine.Festivals);
        }

        [Fact]
        public void SaveRestoreRoundTrip_PreservesAllFestivalStates()
        {
            var engine1 = new ShelterFestivalEngine();
            var p1 = engine1.ScheduleFestival(FestivalType.HarvestCommunion, "Fest 1", 10, durationDays: 2);
            engine1.TryCommenceFestival(p1.FestivalId, 10, (item, amt) => true);

            var p2 = engine1.ScheduleFestival(FestivalType.RemembranceVigil, "Fest 2", 30);

            var saveState = engine1.CaptureState();
            Assert.NotNull(saveState);
            Assert.Equal(2, saveState.Festivals.Count);

            var engine2 = new ShelterFestivalEngine();
            engine2.RestoreState(saveState);

            Assert.Equal(2, engine2.Festivals.Count);
            var restoredP1 = engine2.Festivals[0];
            Assert.Equal(p1.FestivalId, restoredP1.FestivalId);
            Assert.True(restoredP1.IsActive);
            Assert.Equal(1, restoredP1.DaysActive);

            var restoredP2 = engine2.Festivals[1];
            Assert.Equal(p2.FestivalId, restoredP2.FestivalId);
            Assert.False(restoredP2.IsActive);
        }
    }
}
