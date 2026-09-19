// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.InformationFlow;
using Xunit;

namespace Ashfall.Core.Tests.InformationFlow
{
    public sealed class RumorSystemTests
    {
        [Fact]
        public void RegisterHub_StoresHubMetadata()
        {
            var system = new RumorSystem();
            var hub = system.RegisterHub("hub_crossroads", "Crossroads Trading Post", "loc_crossroads", 0.9f, "mercantile");

            Assert.NotNull(hub);
            Assert.Equal("hub_crossroads", hub.HubId);
            Assert.Equal(0.9f, hub.Credibility);
            Assert.Equal(1, system.HubCount);
        }

        [Fact]
        public void GenerateRumor_CreatesRumorAndEmitsEvent()
        {
            var system = new RumorSystem();
            system.RegisterHub("hub_1", "Outpost Hub", "loc_outpost");

            WastelandRumor? emitted = null;
            system.OnRumorGenerated += r => emitted = r;

            var rumor = system.GenerateRumor(
                originLocationId: "loc_outpost",
                subjectType: RumorSubjectType.Faction,
                subjectId: "faction_militia",
                headline: "Militia Patrol Ambushed",
                description: "Heavy fighting reported north of the valley.",
                truthfulness: 0.9f,
                currentDay: 4
            );

            Assert.NotNull(rumor);
            Assert.Equal("faction_militia", rumor.SubjectId);
            Assert.Equal(0.9f, rumor.Truthfulness);
            Assert.Contains("hub_1", rumor.ReachedHubIds);
            Assert.Equal(emitted, rumor);
            Assert.Equal(1, system.TotalRumorCount);
        }

        [Fact]
        public void PropagateRumorToHub_TransfersRumorAndMutatesTruthfulness()
        {
            var system = new RumorSystem();
            var h1 = system.RegisterHub("h1", "Hub 1", "loc_1", 0.8f);
            var h2 = system.RegisterHub("h2", "Hub 2", "loc_2", 0.7f);

            var rumor = system.GenerateRumor("loc_1", RumorSubjectType.Economy, "grain_supply", "Grain Shortage Looming", "Stocks running thin", 0.9f, 1);
            float initialTruth = rumor.Truthfulness;

            bool propagated = system.PropagateRumorToHub(rumor.RumorId, "h2");

            Assert.True(propagated);
            Assert.Contains("h2", rumor.ReachedHubIds);
            Assert.True(rumor.Truthfulness < initialTruth); // Truthfulness slightly mutated as it spreads
        }

        [Fact]
        public void InterceptRumor_MarksIntercepted()
        {
            var system = new RumorSystem();
            var rumor = system.GenerateRumor("loc_radio", RumorSubjectType.Event, "storm_warning", "Radio Alert", "Radio frequency intercept", 1.0f, 2);

            Assert.False(rumor.IsIntercepted);
            bool intercepted = system.InterceptRumor(rumor.RumorId);

            Assert.True(intercepted);
            Assert.True(rumor.IsIntercepted);
            Assert.Equal(1, system.InterceptedRumorCount);
        }

        [Fact]
        public void TickDay_DecaysTruthfulnessAndRemovesExpiredRumors()
        {
            var system = new RumorSystem();
            var rumor = system.GenerateRumor("loc_old", RumorSubjectType.Location, "vault_ruin", "Old Tale", "Ancient rumor", truthfulness: 0.04f, currentDay: 1);

            // Tick day 2: truthfulness is 0.04 - 0.05 <= 0, so it expires
            system.TickDay(currentDay: 2);

            Assert.Equal(0, system.TotalRumorCount);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new RumorSystem();
            system1.RegisterHub("hub_a", "Trading Hub", "loc_market", 0.85f);
            var r = system1.GenerateRumor("loc_market", RumorSubjectType.Economy, "ammo_price", "Ammo Surge", "High ammo prices", 0.75f, 3);
            system1.InterceptRumor(r.RumorId);

            var state = system1.CaptureState();
            Assert.Single(state.Hubs);
            Assert.Single(state.Rumors);

            var system2 = new RumorSystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.HubCount);
            Assert.Equal(1, system2.InterceptedRumorCount);
            var restoredRumor = system2.GetInterceptedRumors().First();
            Assert.Equal("Ammo Surge", restoredRumor.Headline);
            Assert.Equal(0.75f, restoredRumor.Truthfulness);
        }
    }
}
