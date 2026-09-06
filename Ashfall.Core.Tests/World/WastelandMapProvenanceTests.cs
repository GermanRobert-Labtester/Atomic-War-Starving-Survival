using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Expeditions;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class WastelandMapProvenanceTests
    {
        private static (WastelandMapSystem sys, WastelandMapState state) MakeMap()
        {
            var nodes = new List<MapNode>
            {
                new MapNode
                {
                    Id = "node_base",
                    DisplayName = "Holdfast Base",
                    Danger = MapNodeDanger.None,
                    PositionX = 100f,
                    PositionY = 200f,
                    StartingUnlocked = true,
                    LootTableId = "loot_starting_base"
                },
                new MapNode
                {
                    Id = "node_radio_tower",
                    DisplayName = "Radio Tower Relay",
                    Danger = MapNodeDanger.Low,
                    PositionX = 300f,
                    PositionY = 400f,
                    Discoverable = true,
                    LootTableId = "loot_electronics_basic"
                },
                new MapNode
                {
                    Id = "node_flooded_quarry",
                    DisplayName = "Flooded Quarry",
                    Danger = MapNodeDanger.Medium,
                    PositionX = 500f,
                    PositionY = 600f,
                    Discoverable = true,
                    LootTableId = "loot_industrial_scrap"
                },
                new MapNode
                {
                    Id = "node_missile_silo",
                    DisplayName = "Abandoned Silo Complex",
                    Danger = MapNodeDanger.High,
                    PositionX = 700f,
                    PositionY = 800f,
                    Discoverable = true,
                    LootTableId = "loot_military_munitions"
                }
            };

            var routes = new List<MapRoute>
            {
                new MapRoute { From = "node_base", To = "node_radio_tower", DistanceKm = 25f },
                new MapRoute { From = "node_radio_tower", To = "node_flooded_quarry", DistanceKm = 30f },
                new MapRoute { From = "node_flooded_quarry", To = "node_missile_silo", DistanceKm = 40f }
            };

            var state = new WastelandMapState();
            var sys = new WastelandMapSystem(state, nodes, routes);
            return (sys, state);
        }

        [Fact]
        public void MapFogState_StartsUnknown_TransitionsToRumored_Surveyed_Visited()
        {
            var (sys, _) = MakeMap();

            // node_radio_tower is undiscovered at start
            Assert.Equal(MapFogState.Unknown, sys.GetFogState("node_radio_tower"));
            Assert.False(sys.IsDiscovered("node_radio_tower"));

            // 1. Discover as Rumor
            bool rumorResult = sys.DiscoverRumor("node_radio_tower", "intercept_ch3", 5, InformationConfidence.Medium);
            Assert.True(rumorResult);
            Assert.Equal(MapFogState.Rumored, sys.GetFogState("node_radio_tower"));
            Assert.True(sys.IsDiscovered("node_radio_tower"));

            var kRumor = sys.GetNodeKnowledge("node_radio_tower");
            Assert.NotNull(kRumor);
            Assert.Equal(MapFogState.Rumored, kRumor.FogState);
            Assert.Equal(5, kRumor.LastConfirmedDay);
            Assert.NotNull(kRumor.Provenance);
            Assert.Equal(KnowledgeSourceKind.RadioIntercept, kRumor.Provenance.SourceKind);
            Assert.Equal("intercept_ch3", kRumor.Provenance.SourceId);
            Assert.Equal(InformationConfidence.Medium, kRumor.Provenance.Confidence);

            // 2. Discover as Survey
            var traits = new[] { "trait_aquifer_potable", "trait_rad_hotspot" };
            bool surveyResult = sys.DiscoverSurvey("node_radio_tower", "survey_radar_01", 10, traits);
            Assert.True(surveyResult);
            Assert.Equal(MapFogState.Surveyed, sys.GetFogState("node_radio_tower"));

            var kSurvey = sys.GetNodeKnowledge("node_radio_tower");
            Assert.NotNull(kSurvey);
            Assert.Equal(MapFogState.Surveyed, kSurvey.FogState);
            Assert.Equal(10, kSurvey.LastConfirmedDay);
            Assert.Equal(KnowledgeSourceKind.ExpeditionSurvey, kSurvey.Provenance!.SourceKind);
            Assert.Equal("survey_radar_01", kSurvey.Provenance.SourceId);
            Assert.Contains("trait_aquifer_potable", kSurvey.Traits);
            Assert.Contains("trait_rad_hotspot", kSurvey.Traits);

            // 3. Discover as Visited
            bool visitResult = sys.DiscoverVisited("node_radio_tower", "survivor_elena", 15);
            Assert.True(visitResult);
            Assert.Equal(MapFogState.Visited, sys.GetFogState("node_radio_tower"));

            var kVisit = sys.GetNodeKnowledge("node_radio_tower");
            Assert.NotNull(kVisit);
            Assert.Equal(MapFogState.Visited, kVisit.FogState);
            Assert.Equal(15, kVisit.LastConfirmedDay);
            Assert.Equal(KnowledgeSourceKind.ExpeditionVisit, kVisit.Provenance!.SourceKind);
            Assert.Equal("survivor_elena", kVisit.Provenance.SourceId);
            // Traits remain preserved from survey
            Assert.Contains("trait_aquifer_potable", kVisit.Traits);
        }

        [Fact]
        public void FogState_NeverRegresses_WhenLowerStateDiscovered()
        {
            var (sys, _) = MakeMap();

            // Visit the silo directly
            sys.DiscoverVisited("node_missile_silo", "scout_01", 3);
            Assert.Equal(MapFogState.Visited, sys.GetFogState("node_missile_silo"));

            // Attempt to discover rumor for the same silo on Day 12
            bool rumorResult = sys.DiscoverRumor("node_missile_silo", "rumor_trader", 12, InformationConfidence.Low);
            Assert.True(rumorResult);

            // Must stay Visited, but LastConfirmedDay updates
            Assert.Equal(MapFogState.Visited, sys.GetFogState("node_missile_silo"));
            var k = sys.GetNodeKnowledge("node_missile_silo");
            Assert.NotNull(k);
            Assert.Equal(12, k.LastConfirmedDay);
            Assert.Equal(KnowledgeSourceKind.ExpeditionVisit, k.Provenance!.SourceKind);
        }

        [Fact]
        public void RumorObfuscation_FuzzesCoordinatesAndHidesDetailsDeterministically()
        {
            var (sys, _) = MakeMap();
            sys.DiscoverRumor("node_flooded_quarry", "scanner_sweep", 2);

            var intel1 = sys.GetNodeIntel("node_flooded_quarry");
            Assert.NotNull(intel1);
            Assert.Equal(MapFogState.Rumored, intel1.FogState);
            // Coords must be offset (fuzzed) by 50..100
            Assert.NotEqual(500f, intel1.PositionX);
            Assert.NotEqual(600f, intel1.PositionY);
            Assert.False(intel1.Routable);
            Assert.Equal(MapNodeDanger.None, intel1.Danger); // exact danger rating hidden
            Assert.Equal("Medium", intel1.DangerBand); // coarse band preserved
            Assert.Equal("Unconfirmed scrap / rumor", intel1.LootDescription);
            Assert.Empty(intel1.Traits);

            // Repeat inquiry must yield deterministic identical fuzzed coords
            var intel2 = sys.GetNodeIntel("node_flooded_quarry");
            Assert.NotNull(intel2);
            Assert.Equal(intel1.PositionX, intel2.PositionX);
            Assert.Equal(intel1.PositionY, intel2.PositionY);
        }

        [Fact]
        public void SurveyState_RevealsExactCoords_ConfirmedDanger_AndTraits()
        {
            var (sys, _) = MakeMap();
            sys.DiscoverSurvey("node_flooded_quarry", "seismic_drone", 7, new[] { "trait_heavy_brine" });

            var intel = sys.GetNodeIntel("node_flooded_quarry");
            Assert.NotNull(intel);
            Assert.Equal(MapFogState.Surveyed, intel.FogState);
            Assert.Equal(500f, intel.PositionX);
            Assert.Equal(600f, intel.PositionY);
            Assert.True(intel.Routable);
            Assert.Equal(MapNodeDanger.Medium, intel.Danger);
            Assert.Equal("Medium", intel.DangerBand);
            Assert.Contains("trait_heavy_brine", intel.Traits);
        }

        [Fact]
        public void PlanRoute_DoesNotTraverseThroughRumoredIntermediateNodes()
        {
            var (sys, _) = MakeMap();

            // Starting base is visited
            Assert.Equal(MapFogState.Visited, sys.GetFogState("node_base"));

            // Radio tower (intermediate) is only Rumored
            sys.DiscoverRumor("node_radio_tower", "rumor_src", 1);

            // Flooded quarry is Surveyed
            sys.DiscoverSurvey("node_flooded_quarry", "survey_src", 2);

            // Path from base to flooded quarry goes through radio tower.
            // Since radio tower is only Rumored, intermediate traversal is blocked!
            var path = sys.PlanRoute("node_base", "node_flooded_quarry");
            Assert.Empty(path);

            // Upgrading radio tower to Surveyed unlocks the route
            sys.DiscoverSurvey("node_radio_tower", "survey_src", 3);
            var pathUnlocked = sys.PlanRoute("node_base", "node_flooded_quarry");
            Assert.Equal(3, pathUnlocked.Count);
            Assert.Equal("node_base", pathUnlocked[0]);
            Assert.Equal("node_radio_tower", pathUnlocked[1]);
            Assert.Equal("node_flooded_quarry", pathUnlocked[2]);
        }

        [Fact]
        public void RouteEstimation_MatchesExpeditionSystemEstimateDirectly()
        {
            var (sys, _) = MakeMap();
            sys.DiscoverSurvey("node_radio_tower", "survey", 1);
            sys.DiscoverSurvey("node_flooded_quarry", "survey", 1);

            var def = new ExpeditionDefinition
            {
                id = "node_flooded_quarry",
                displayName = "Flooded Quarry",
                distanceTicks = 11,
                dangerLevel = 2,
                scavenging_table_id = "loot_industrial_scrap"
            };

            // Direct comparison between WastelandMapSystem.EstimateRoute and ExpeditionSystem.Estimate
            var mapEstimate = sys.EstimateRoute(def, ExpeditionStance.Speed, isNightScavenge: true);
            var directEstimate = ExpeditionSystem.Estimate(def, ExpeditionStance.Speed, isNightScavenge: true);

            Assert.Equal(directEstimate.distanceTicks, mapEstimate.distanceTicks);
            Assert.Equal(directEstimate.outboundTicks, mapEstimate.outboundTicks);
            Assert.Equal(directEstimate.inboundTicks, mapEstimate.inboundTicks);
            Assert.Equal(directEstimate.totalTicks, mapEstimate.totalTicks);
            Assert.Equal(directEstimate.fuelRequired, mapEstimate.fuelRequired);
            Assert.Equal(directEstimate.breakdownRiskTotal, mapEstimate.breakdownRiskTotal);
            Assert.Equal(directEstimate.stance, mapEstimate.stance);
        }

        [Fact]
        public void LegacySaveMigration_AutomaticallyPopulatesKnowledgeAsVisited()
        {
            // Simulate an older save that only contains string lists in Discovered
            var legacyState = new WastelandMapState();
            legacyState.Discovered.Add("node_flooded_quarry");
            legacyState.Discovered.Add("node_missile_silo");
            Assert.Empty(legacyState.Knowledge);

            var nodes = new List<MapNode>
            {
                new MapNode { Id = "node_base", StartingUnlocked = true },
                new MapNode { Id = "node_flooded_quarry" },
                new MapNode { Id = "node_missile_silo" },
                new MapNode { Id = "node_radio_tower" }
            };

            legacyState.NormalizeAndValidate(nodes);

            // Starting base + 2 legacy nodes must now have Knowledge entries as Visited
            Assert.Equal(3, legacyState.Knowledge.Count);

            var kBase = legacyState.Knowledge.Find(k => k.NodeId == "node_base");
            Assert.NotNull(kBase);
            Assert.Equal(MapFogState.Visited, kBase.FogState);

            var kQuarry = legacyState.Knowledge.Find(k => k.NodeId == "node_flooded_quarry");
            Assert.NotNull(kQuarry);
            Assert.Equal(MapFogState.Visited, kQuarry.FogState);
            Assert.Equal(InformationConfidence.Confirmed, kQuarry.Provenance!.Confidence);
            Assert.Equal(KnowledgeSourceKind.ExpeditionVisit, kQuarry.Provenance.SourceKind);

            var kSilo = legacyState.Knowledge.Find(k => k.NodeId == "node_missile_silo");
            Assert.NotNull(kSilo);
            Assert.Equal(MapFogState.Visited, kSilo.FogState);
        }

        [Fact]
        public void Knowledge_SaveLoadRoundTrip_PreservesAllProperties()
        {
            var (sys1, _) = MakeMap();
            sys1.DiscoverRumor("node_radio_tower", "radio_alpha", 4, InformationConfidence.Medium);
            sys1.DiscoverSurvey("node_flooded_quarry", "survey_beta", 8, new[] { "trait_rad_hotspot", "trait_aquifer_potable" });
            sys1.DiscoverVisited("node_missile_silo", "scout_gamma", 12);

            var captured = sys1.CaptureState();

            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(captured);
            var deserialized = serializer.Deserialize<WastelandMapState>(json);
            Assert.NotNull(deserialized);

            var (sys2, _) = MakeMap();
            sys2.RestoreState(deserialized);

            // Verify radio tower (Rumored)
            Assert.Equal(MapFogState.Rumored, sys2.GetFogState("node_radio_tower"));
            var kRadio = sys2.GetNodeKnowledge("node_radio_tower");
            Assert.NotNull(kRadio);
            Assert.Equal("radio_alpha", kRadio.Provenance!.SourceId);
            Assert.Equal(InformationConfidence.Medium, kRadio.Provenance.Confidence);

            // Verify flooded quarry (Surveyed)
            Assert.Equal(MapFogState.Surveyed, sys2.GetFogState("node_flooded_quarry"));
            var kQuarry = sys2.GetNodeKnowledge("node_flooded_quarry");
            Assert.NotNull(kQuarry);
            Assert.Equal("survey_beta", kQuarry.Provenance!.SourceId);
            Assert.Contains("trait_rad_hotspot", kQuarry.Traits);
            Assert.Contains("trait_aquifer_potable", kQuarry.Traits);

            // Verify missile silo (Visited)
            Assert.Equal(MapFogState.Visited, sys2.GetFogState("node_missile_silo"));
            var kSilo = sys2.GetNodeKnowledge("node_missile_silo");
            Assert.NotNull(kSilo);
            Assert.Equal("scout_gamma", kSilo.Provenance!.SourceId);
        }
    }
}
