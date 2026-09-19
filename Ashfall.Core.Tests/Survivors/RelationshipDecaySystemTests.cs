// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class RelationshipDecaySystemTests
    {
        [Fact]
        public void RegisterOrUpdatePair_NormalizesOrderAndStoresInitialValues()
        {
            var system = new RelationshipDecaySystem();
            var pair = system.RegisterOrUpdatePair("bob", "alice", SurvivorBondType.Friend, 50f, 40f, 1);

            Assert.NotNull(pair);
            Assert.Equal("alice", pair.SurvivorA);
            Assert.Equal("bob", pair.SurvivorB);
            Assert.Equal(SurvivorBondType.Friend, pair.Bond);
            Assert.Equal(50f, pair.Affinity);
            Assert.Equal(1, system.TrackedPairCount);

            // Fetching in reverse order retrieves the same pair
            var fetched = system.GetPair("bob", "alice");
            Assert.Equal(pair, fetched);
        }

        [Fact]
        public void RecordInteraction_BoostsAffinityAndTrust_AndUpgradesBond()
        {
            var system = new RelationshipDecaySystem();
            system.RegisterOrUpdatePair("dan", "claire", SurvivorBondType.Acquaintance, initialAffinity: 35f, initialTrust: 25f, currentDay: 1);

            system.RecordInteraction("dan", "claire", "shared_meal", affinityBonus: 15f, currentDay: 2);

            var pair = system.GetPair("claire", "dan");
            Assert.NotNull(pair);
            Assert.Equal(50f, pair.Affinity);
            Assert.Equal(SurvivorBondType.Friend, pair.Bond); // upgraded from acquaintance
            Assert.Equal(0, pair.DaysWithoutInteraction);
        }

        [Fact]
        public void TickDay_DecaysAffinityAfterNeglectDays()
        {
            var system = new RelationshipDecaySystem();
            var pair = system.RegisterOrUpdatePair("ellen", "frank", SurvivorBondType.Friend, initialAffinity: 30f, currentDay: 1);

            // Tick 4 days without interaction
            for (int day = 1; day <= 4; day++)
            {
                system.TickDay(day);
            }

            Assert.Equal(4, pair.DaysWithoutInteraction);
            Assert.True(pair.Affinity < 30f); // decayed on day 4
        }

        [Fact]
        public void TickDay_TriggersDriftEventWhenAffinityDropsPastThreshold()
        {
            var system = new RelationshipDecaySystem();
            var pair = system.RegisterOrUpdatePair("gina", "hank", SurvivorBondType.Friend, initialAffinity: 0.5f, currentDay: 1);
            pair.DaysWithoutInteraction = 3;

            SocialDriftEvent? driftEvent = null;
            system.OnBondDrifted += ev => driftEvent = ev;

            system.TickDay(currentDay: 5);

            Assert.NotNull(driftEvent);
            Assert.Equal(SocialDriftType.GrewApart, driftEvent.DriftType);
            Assert.Equal(SurvivorBondType.Acquaintance, pair.Bond);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new RelationshipDecaySystem();
            system1.RegisterOrUpdatePair("ian", "jenny", SurvivorBondType.Mentor, 65f, 55f, 3);
            system1.RecordInteraction("ian", "jenny", "training", 10f, 4);

            var state = system1.CaptureState();
            Assert.Single(state.Pairs);

            var system2 = new RelationshipDecaySystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.TrackedPairCount);
            var restored = system2.GetPair("ian", "jenny");
            Assert.NotNull(restored);
            Assert.Equal(SurvivorBondType.Mentor, restored.Bond);
            Assert.Equal(75f, restored.Affinity);
        }
    }
}
