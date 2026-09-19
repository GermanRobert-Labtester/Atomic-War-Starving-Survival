// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    public sealed class DiscoveryConsequenceSystemTests
    {
        [Fact]
        public void RegisterDiscovery_AddsRecord_AndAutoTriggersThreatClearedSafetyBonus()
        {
            var system = new DiscoveryConsequenceSystem();
            DiscoveryRecord? observedDisc = null;
            ConsequenceOutcome? observedOutcome = null;

            system.OnDiscoveryRegistered += d => observedDisc = d;
            system.OnConsequenceTriggered += c => observedOutcome = c;

            var rec = system.RegisterDiscovery("loc_north_bridge", DiscoveryType.ThreatCleared, "Bandit Nest Cleared", "Raiders eliminated", day: 4);

            Assert.NotNull(rec);
            Assert.Equal(observedDisc, rec);
            Assert.Equal(1, system.DiscoveryCount);
            Assert.Equal(1, system.ConsequenceCount);
            Assert.NotNull(observedOutcome);
            Assert.Equal(0.15f, observedOutcome.CaravanSafetyBonus);
        }

        [Fact]
        public void ConcealDiscovery_SetsStatusToConcealed()
        {
            var system = new DiscoveryConsequenceSystem();
            var rec = system.RegisterDiscovery("loc_old_depot", DiscoveryType.ResourceDeposit, "Hidden Cache", "Sealed crates", day: 2);

            bool ok = system.ConcealDiscovery(rec.DiscoveryId);

            Assert.True(ok);
            Assert.Equal(ConsequenceStatus.Concealed, rec.Status);
        }

        [Fact]
        public void ExploitDiscovery_TriggersOutcomeWithFactionStanding()
        {
            var system = new DiscoveryConsequenceSystem();
            var rec = system.RegisterDiscovery(
                "loc_iron_mine",
                DiscoveryType.ResourceDeposit,
                "Iron Vein",
                "High grade ore",
                day: 3,
                associatedFactionId: "faction_miners",
                resourceYield: "iron_ore");

            var outcome = system.ExploitDiscovery(rec.DiscoveryId, day: 5);

            Assert.NotNull(outcome);
            Assert.Equal(ConsequenceStatus.Exploited, rec.Status);
            Assert.Equal(5f, outcome.FactionStandingDelta);
            Assert.Equal("faction_miners", outcome.FactionId);
            Assert.Contains("iron_ore", outcome.Description);
        }

        [Fact]
        public void EscalateDiscovery_RecordsEscalationOutcome()
        {
            var system = new DiscoveryConsequenceSystem();
            var rec = system.RegisterDiscovery("loc_forgotten_vault", DiscoveryType.RuinsUncovered, "Ancient Bunker", "Blast doors ajar", day: 6);

            var outcome = system.EscalateDiscovery(rec.DiscoveryId, day: 12);

            Assert.NotNull(outcome);
            Assert.Equal(ConsequenceStatus.Escalated, rec.Status);
            Assert.Contains("opportunists", outcome.Description);
        }

        [Fact]
        public void GetTotalCaravanSafetyBonus_CapsAtFiftyPercent()
        {
            var system = new DiscoveryConsequenceSystem();

            // 4 threat clearings = 4 * 0.15 = 0.60, should cap at 0.50
            system.RegisterDiscovery("loc_p1", DiscoveryType.ThreatCleared, "C1", "D1", day: 1);
            system.RegisterDiscovery("loc_p2", DiscoveryType.ThreatCleared, "C2", "D2", day: 2);
            system.RegisterDiscovery("loc_p3", DiscoveryType.ThreatCleared, "C3", "D3", day: 3);
            system.RegisterDiscovery("loc_p4", DiscoveryType.ThreatCleared, "C4", "D4", day: 4);

            float bonus = system.GetTotalCaravanSafetyBonus();
            Assert.Equal(0.50f, bonus);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new DiscoveryConsequenceSystem();
            system1.RegisterDiscovery("loc_test", DiscoveryType.StrategicLocation, "High Ground", "Overlooks valley", day: 1);

            var state = system1.CaptureState();
            Assert.Single(state.Discoveries);

            var system2 = new DiscoveryConsequenceSystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.DiscoveryCount);
            var items = system2.GetDiscoveriesAt("loc_test");
            Assert.Single(items);
            Assert.Equal("High Ground", items[0].Title);
        }

        [Fact]
        public void RegisterExpeditionDiscovery_IsIdempotent_AndPersistsStableConsequence()
        {
            var system = new DiscoveryConsequenceSystem();

            var first = system.RegisterExpeditionDiscovery("loc_radio_depot", day: 4);
            var duplicate = system.RegisterExpeditionDiscovery("loc_radio_depot", day: 9);

            Assert.NotNull(first);
            Assert.Same(first, duplicate);
            Assert.Equal(1, system.DiscoveryCount);
            Assert.Equal(1, system.ConsequenceCount);
            Assert.Equal("expedition_discovery_loc_radio_depot", first!.DiscoveryId);

            var restored = new DiscoveryConsequenceSystem();
            restored.RestoreState(system.CaptureState());
            var replay = restored.RegisterExpeditionDiscovery("loc_radio_depot", day: 12);

            Assert.NotNull(replay);
            Assert.Equal(1, restored.DiscoveryCount);
            Assert.Equal(1, restored.ConsequenceCount);
        }
    }
}
