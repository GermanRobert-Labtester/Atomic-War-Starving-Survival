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

        [Fact]
        public void AuthoredData_RumorHubsJson_LoadsSuccessfully()
        {
            var dataDir = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
                    "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            if (!System.IO.File.Exists(System.IO.Path.Combine(dataDir, "rumor_hubs.json")))
                dataDir = System.IO.Path.GetFullPath("Assets/StreamingAssets/Data");

            string filePath = System.IO.Path.Combine(dataDir, "rumor_hubs.json");
            Assert.True(System.IO.File.Exists(filePath), $"File not found: {filePath}");

            string json = System.IO.File.ReadAllText(filePath);
            var catalog = JsonSerializer.Deserialize<RumorCatalogData>(json);
            Assert.NotNull(catalog);
            Assert.True(catalog!.hubs.Count >= 6);

            foreach (var hub in catalog.hubs)
            {
                Assert.False(string.IsNullOrWhiteSpace(hub.hub_id));
                Assert.False(string.IsNullOrWhiteSpace(hub.hub_name));
                Assert.False(string.IsNullOrWhiteSpace(hub.location_id));
                Assert.True(hub.credibility > 0f && hub.credibility <= 1.0f);
            }

            var system = new RumorSystem();
            system.LoadCatalog(catalog);
            Assert.True(system.HubCount >= 6);
        }

        [Fact]
        public void IntelligenceBriefingReport_DerivesActionableBriefing_FromLocationRumors()
        {
            var dataDir = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
                    "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            if (!System.IO.File.Exists(System.IO.Path.Combine(dataDir, "rumor_hubs.json")))
                dataDir = System.IO.Path.GetFullPath("Assets/StreamingAssets/Data");

            string filePath = System.IO.Path.Combine(dataDir, "rumor_hubs.json");
            string json = System.IO.File.ReadAllText(filePath);
            var catalog = JsonSerializer.Deserialize<RumorCatalogData>(json);

            var system = new RumorSystem();
            system.LoadCatalog(catalog);

            // Empty location report
            var emptyReport = system.CreateBriefingReport("loc_crossroads");
            Assert.Equal(0, emptyReport.TotalItems);
            Assert.Equal("Crossroads Caravanserai", emptyReport.HubName);

            // Generate threat rumor (Faction)
            system.GenerateRumor(
                originLocationId: "loc_crossroads",
                subjectType: RumorSubjectType.Faction,
                subjectId: "faction_iron_raiders",
                headline: "Raider Ambush Spotted",
                description: "Raider scouts positioned on northern ridge.",
                truthfulness: 0.90f,
                currentDay: 1
            );

            // Generate opportunity rumor (Economy)
            system.GenerateRumor(
                originLocationId: "loc_crossroads",
                subjectType: RumorSubjectType.Economy,
                subjectId: "med_cache",
                headline: "Medical Crate Uncovered",
                description: "Hospital basement cache undisturbed.",
                truthfulness: 0.80f,
                currentDay: 1
            );

            var briefing = system.CreateBriefingReport("loc_crossroads");
            Assert.Equal(2, briefing.TotalItems);
            Assert.Equal(1, briefing.ThreatCount);
            Assert.Equal(1, briefing.OpportunityCount);
            Assert.Equal(0.85f, briefing.AverageTruthfulness, precision: 2);
            Assert.All(briefing.Items, item => Assert.True(item.IsVerified));
        }
    }
}
