// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan182RelationshipDecayIntegrationTests
    {
        [Fact]
        public void PairBond_InteractionUpgradesBond_AndNeglectDecaysBond()
        {
            var system = new RelationshipDecaySystem();

            var pair = system.RegisterOrUpdatePair("surv_alpha", "surv_beta", SurvivorBondType.Friend, initialAffinity: 65f, initialTrust: 55f, currentDay: 1);
            Assert.NotNull(pair);
            Assert.Equal(1, system.TrackedPairCount);

            // Interaction elevates bond to CloseFriend
            system.RecordInteraction("surv_alpha", "surv_beta", "conversation", 15f, 1);
            Assert.Equal(80f, pair.Affinity);
            Assert.Equal(SurvivorBondType.CloseFriend, pair.Bond);

            // Tick days without interaction
            for (int day = 2; day <= 100; day++)
            {
                system.TickDay(day);
            }

            Assert.True(pair.DaysWithoutInteraction > 3);
            Assert.True(pair.Affinity < 50f);
            // Social drift event should have been logged
            Assert.NotEmpty(system.DriftHistory);
        }

        [Fact]
        public void RelationshipDecay_PersistenceRoundtrip_PreservesPairsAndDrift()
        {
            var system = new RelationshipDecaySystem();
            system.RegisterOrUpdatePair("surv_1", "surv_2", SurvivorBondType.Rival, 10f, 10f, 1);

            var state = system.CaptureState();
            string json = JsonSerializer.Serialize(state);
            var restored = JsonSerializer.Deserialize<RelationshipDecayState>(json);
            Assert.NotNull(restored);

            var newSystem = new RelationshipDecaySystem();
            newSystem.RestoreState(restored!);

            Assert.Equal(1, newSystem.TrackedPairCount);
            var p = newSystem.GetPair("surv_1", "surv_2");
            Assert.NotNull(p);
            Assert.Equal(SurvivorBondType.Rival, p.Bond);
        }
    }
}
