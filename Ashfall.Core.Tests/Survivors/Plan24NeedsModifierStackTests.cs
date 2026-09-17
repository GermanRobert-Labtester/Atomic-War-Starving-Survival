// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan24NeedsModifierStackTests
    {
        [Fact]
        public void ContributionsReplaceBySourceAndReturnDeterministicOrder()
        {
            var stack = new NeedsModifierStack();
            stack.Set("survivor_a", "z_cold", NeedKind.Fatigue, 2f, priority: 20);
            stack.Set("survivor_a", "a_sleep", NeedKind.Fatigue, -3f, priority: 10);
            stack.Set("survivor_a", "m_work", NeedKind.Fatigue, 1f, priority: 20);
            stack.Set("survivor_a", "z_cold", NeedKind.Fatigue, 4f, priority: 20);

            var contributions = stack.GetForSurvivor("survivor_a");

            Assert.Equal(3, stack.Count);
            Assert.Equal(new[] { "a_sleep", "m_work", "z_cold" },
                new List<string>(contributions.Count)
                {
                    contributions[0].SourceId,
                    contributions[1].SourceId,
                    contributions[2].SourceId
                });
            Assert.Equal(2f, stack.GetTotal("survivor_a", NeedKind.Fatigue));
        }

        [Fact]
        public void StackAppliesRatesThroughNeedsOwnerAndEmitsAttribution()
        {
            var needs = new NeedsSystem(new NeedsProfile
            {
                fatiguePerHour = 1f,
                hungerPerHour = 0f,
                thirstPerHour = 0f,
                warmthLossPerHourInCold = 0f
            });
            var survivor = new SurvivorNeedsState { Id = "survivor_a", Fatigue = 10f };
            needs.Register(survivor);
            var sources = new List<string>();
            needs.OnAttributedContribution += (_, contribution) => sources.Add(contribution.SourceId);
            needs.SetExternalModifier("survivor_a", "sleep_schedule", NeedKind.Fatigue, -2f);

            needs.Tick(2f);

            Assert.Equal(8f, survivor.Fatigue);
            Assert.Equal(new[] { "sleep_schedule" }, sources);
        }

        [Fact]
        public void RemoveAndClearDoNotLeaveRatesBehind()
        {
            var stack = new NeedsModifierStack();
            stack.Set("survivor_a", "sleep", NeedKind.Fatigue, -1f);
            stack.Set("survivor_a", "meal", NeedKind.Hunger, -2f);
            stack.Set("survivor_b", "sleep", NeedKind.Fatigue, -1f);

            Assert.True(stack.Remove("survivor_a", "sleep", NeedKind.Fatigue));
            Assert.False(stack.Remove("survivor_a", "sleep", NeedKind.Fatigue));
            Assert.Equal(1, stack.ClearSource("sleep"));
            Assert.Equal(1, stack.Count);
            Assert.Equal(-2f, stack.GetTotal("survivor_a", NeedKind.Hunger));
        }

        [Fact]
        public void DeadSurvivorDoesNotReceiveExternalContribution()
        {
            var needs = new NeedsSystem(new NeedsProfile
            {
                fatiguePerHour = 0f,
                hungerPerHour = 0f,
                thirstPerHour = 0f,
                warmthLossPerHourInCold = 0f
            });
            var survivor = new SurvivorNeedsState
            {
                Id = "survivor_a", Fatigue = 10f, IsAlive = false, IsDead = true
            };
            needs.Register(survivor);
            needs.SetExternalModifier("survivor_a", "sleep", NeedKind.Fatigue, -5f);

            needs.Tick(24f);

            Assert.Equal(10f, survivor.Fatigue);
        }

        [Fact]
        public void TimedContributionExpiresAndTopContributorsAreDeterministic()
        {
            var stack = new NeedsModifierStack();
            stack.Set("s", "z_source", NeedKind.Fatigue, 1f, startDay: 3, endDay: 4);
            stack.Set("s", "a_source", NeedKind.Fatigue, 2f, startDay: 3, endDay: 8);
            stack.Set("s", "b_source", NeedKind.Fatigue, -2f, startDay: 3, endDay: 8);

            Assert.Equal(0f, stack.AggregateFor("s", NeedKind.Fatigue, day: 2));
            Assert.Equal(1f, stack.AggregateFor("s", NeedKind.Fatigue, day: 3));
            Assert.Equal(0f, stack.AggregateFor("s", NeedKind.Fatigue, day: 5));
            var top = stack.GetTopContributors("s", NeedKind.Fatigue, day: 3, limit: 2);
            Assert.Equal("a_source", top[0].SourceId);
            Assert.Equal("b_source", top[1].SourceId);
        }

        [Fact]
        public void NeedsOwnerAppliesOnlyCurrentDayAndKeepsRecentAttributionBounded()
        {
            var needs = new NeedsSystem(new NeedsProfile
            {
                fatiguePerHour = 0f,
                hungerPerHour = 0f,
                thirstPerHour = 0f,
                warmthLossPerHourInCold = 0f
            });
            var survivor = new SurvivorNeedsState { Id = "survivor_a", Fatigue = 10f };
            needs.Register(survivor);
            needs.SetExternalModifier("survivor_a", "withdrawal", NeedKind.Fatigue,
                5f, startDay: 3, endDay: 3);

            needs.CurrentDay = 2;
            needs.Tick(1f);
            Assert.Equal(10f, survivor.Fatigue);
            Assert.Equal(0, needs.ModifierStack.GetRecentForSurvivor("survivor_a").Count);

            needs.CurrentDay = 3;
            needs.Tick(1f);
            Assert.Equal(15f, survivor.Fatigue);
            Assert.Equal("withdrawal",
                needs.ModifierStack.GetRecentForSurvivor("survivor_a")[0].SourceId);

            needs.CurrentDay = 4;
            needs.Tick(1f);
            Assert.Equal(15f, survivor.Fatigue);
            Assert.Equal(0, needs.ModifierStack.GetRecentForSurvivor("survivor_a").Count);
        }
    }
}
