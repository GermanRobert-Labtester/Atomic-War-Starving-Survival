// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.InformationFlow;
using Xunit;

namespace Ashfall.Core.Tests.InformationFlow
{
    public sealed class Plan203RumorNetworkIntegrationTests
    {
        [Fact]
        public void HubRegistration_AndRumorGeneration_AutoConnectsOriginHub()
        {
            var system = new RumorSystem();
            system.RegisterHub("hub_crossroads", "Crossroads", "loc_crossroads", 0.85f);

            var rumor = system.GenerateRumor(
                originLocationId: "loc_crossroads",
                subjectType: RumorSubjectType.Faction,
                subjectId: "faction_settlers",
                headline: "Trade Agreement Ratified",
                description: "New trade terms established.",
                truthfulness: 0.9f,
                currentDay: 1
            );

            Assert.NotNull(rumor);
            Assert.Contains("hub_crossroads", rumor.ReachedHubIds);
            Assert.Equal(1, system.TotalRumorCount);
        }

        [Fact]
        public void Propagation_MutatesTruthfulness_BasedOnHubCredibility()
        {
            var system = new RumorSystem();
            system.RegisterHub("hub_1", "Hub 1", "loc_1", 0.9f);
            system.RegisterHub("hub_2", "Low Cred Hub", "loc_2", 0.5f);

            var rumor = system.GenerateRumor("loc_1", RumorSubjectType.Economy, "fuel_depot", "Fuel Cache Found", "Abandoned bunker discovered", 0.95f, 1);
            float truthBefore = rumor.Truthfulness;

            bool propagated = system.PropagateRumorToHub(rumor.RumorId, "hub_2");
            Assert.True(propagated);
            Assert.Contains("hub_2", rumor.ReachedHubIds);
            Assert.True(rumor.Truthfulness < truthBefore);
        }

        [Fact]
        public void Interception_UpdatesFlag_AndEmitsEvent()
        {
            var system = new RumorSystem();
            var rumor = system.GenerateRumor("loc_1", RumorSubjectType.Event, "storm_warning", "Radiation Storm Brewing", "High particulate count", 0.8f, 1);

            WastelandRumor? interceptedEvent = null;
            system.OnRumorIntercepted += r => interceptedEvent = r;

            bool ok = system.InterceptRumor(rumor.RumorId);
            Assert.True(ok);
            Assert.True(rumor.IsIntercepted);
            Assert.Equal(rumor, interceptedEvent);
            Assert.Equal(1, system.InterceptedRumorCount);
        }

        [Fact]
        public void DailyDecay_ReducesTruth_AndExpiresOldRumors()
        {
            var system = new RumorSystem();
            var rumor = system.GenerateRumor("loc_1", RumorSubjectType.Location, "loc_ruins", "Ruins Cleared", "Salvage crew reported clear", 0.5f, 1);

            system.TickDay(2);
            Assert.True(rumor.Truthfulness < 0.5f);

            // Tick past 30 days
            system.TickDay(35);
            Assert.Equal(0, system.TotalRumorCount);
        }

        [Fact]
        public void StateSerialization_RoundTrips_HubsRumorsAndIntercepts()
        {
            var original = new RumorSystem();
            original.RegisterHub("hub_a", "Alpha Hub", "loc_a", 0.88f, "military");
            var r = original.GenerateRumor("loc_a", RumorSubjectType.Faction, "faction_militia", "Militia Reorganizing", "New commander assigned", 0.8f, 2);
            original.InterceptRumor(r.RumorId);

            var captured = original.CaptureState();
            string json = JsonSerializer.Serialize(captured);
            var restoredState = JsonSerializer.Deserialize<RumorNetworkState>(json);

            Assert.NotNull(restoredState);
            var restored = new RumorSystem(restoredState);

            Assert.Equal(original.HubCount, restored.HubCount);
            Assert.Equal(original.TotalRumorCount, restored.TotalRumorCount);
            Assert.Equal(original.InterceptedRumorCount, restored.InterceptedRumorCount);

            var restoredRumor = restored.Rumors.First();
            Assert.Equal(r.RumorId, restoredRumor.RumorId);
            Assert.Equal(r.Headline, restoredRumor.Headline);
            Assert.True(restoredRumor.IsIntercepted);
        }
    }
}
