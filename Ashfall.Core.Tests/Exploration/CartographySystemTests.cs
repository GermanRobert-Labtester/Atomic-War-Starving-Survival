// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Exploration;
using Ashfall.Core.World;
using Ashfall.Core.Campaign;
using Xunit;

namespace Ashfall.Core.Tests.Exploration
{
    public sealed class CartographySystemTests
    {
        [Fact]
        public void RegisterRegion_InitializesUnexploredRegion()
        {
            var system = new CartographySystem();
            var region = system.RegisterRegion("reg_valley", "Ash Valley", TerrainCategory.Wasteland, new[] { "poi_bunker_1" }, new[] { "rad_zone" });

            Assert.NotNull(region);
            Assert.Equal("reg_valley", region.RegionId);
            Assert.Equal("Ash Valley", region.RegionName);
            Assert.False(region.IsDiscovered);
            Assert.Equal(0f, region.ExplorationProgress);
            Assert.Equal(1, system.TotalRegionCount);
            Assert.Equal(0, system.DiscoveredRegionCount);
        }

        [Fact]
        public void DiscoverRegion_LiftsFogOfWarAndSetsInitialProgress()
        {
            var system = new CartographySystem();
            system.RegisterRegion("reg_coastal", "Dead Coast", TerrainCategory.Water);

            bool discovered = system.DiscoverRegion("reg_coastal", "scout_1", currentDay: 2);

            Assert.True(discovered);
            Assert.Equal(1, system.DiscoveredRegionCount);
            var region = system.GetRegion("reg_coastal");
            Assert.NotNull(region);
            Assert.True(region.IsDiscovered);
            Assert.True(region.ExplorationProgress >= 20f);
        }

        [Fact]
        public void SurveyRegion_IncreasesExplorationAndAdvancesCartographerSkill()
        {
            var system = new CartographySystem();
            system.RegisterRegion("reg_city", "Ruined Metropolis", TerrainCategory.Urban, new[] { "poi_metro" });

            var discovery = system.SurveyRegion("reg_city", "cartographer_dan", currentDay: 3, equipmentBonus: 10f);

            Assert.NotNull(discovery);
            Assert.Equal("reg_city", discovery.RegionId);
            Assert.Equal("poi_metro", discovery.LocationId);
            Assert.True(discovery.Quality > 0f);

            var region = system.GetRegion("reg_city");
            Assert.NotNull(region);
            Assert.True(region.ExplorationProgress > 0f);
        }

        [Fact]
        public void PurchaseOrAcquireMap_RevealsRegionAndBoostsProgress()
        {
            var system = new CartographySystem();
            system.RegisterRegion("reg_silo", "Missile Silo Field", TerrainCategory.Industrial);

            bool acquired = system.PurchaseOrAcquireMap("reg_silo", qualityBoost: 60f);

            Assert.True(acquired);
            var region = system.GetRegion("reg_silo");
            Assert.NotNull(region);
            Assert.True(region.IsDiscovered);
            Assert.Equal(60f, region.ExplorationProgress);
        }

        [Fact]
        public void GetMapCompleteness_ComputesAverageExplorationProgress()
        {
            var system = new CartographySystem();
            system.RegisterRegion("reg_1", "Area 1", TerrainCategory.Rural);
            system.RegisterRegion("reg_2", "Area 2", TerrainCategory.Rural);

            system.PurchaseOrAcquireMap("reg_1", 100f);
            system.PurchaseOrAcquireMap("reg_2", 50f);

            Assert.Equal(75.0f, system.GetMapCompleteness());
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new CartographySystem();
            system1.RegisterRegion("reg_canyon", "Red Canyon", TerrainCategory.Wasteland, new[] { "cave_1" });
            system1.SurveyRegion("reg_canyon", "surveyor_ann", 4, 15f);

            var state = system1.CaptureState();
            Assert.Single(state.Regions);
            Assert.Single(state.Discoveries);
            Assert.Single(state.Cartographers);

            var system2 = new CartographySystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.TotalRegionCount);
            Assert.Equal(1, system2.DiscoveredRegionCount);
            var restored = system2.GetRegion("reg_canyon");
            Assert.NotNull(restored);
            Assert.Equal("Red Canyon", restored.RegionName);
            Assert.True(restored.ExplorationProgress > 0f);
        }

        [Fact]
        public void ProjectCanonicalMap_UsesWorldKnowledgeForQualityAndProvenance()
        {
            var nodes = new List<MapNode>
            {
                new MapNode { Id = "loc_b", DisplayName = "B Sector" },
                new MapNode { Id = "loc_a", DisplayName = "A Sector" }
            };
            var knowledge = new List<MapNodeKnowledgeState>
            {
                new MapNodeKnowledgeState
                {
                    NodeId = "loc_b",
                    FogState = MapFogState.Surveyed,
                    LastConfirmedDay = 9,
                    Provenance = new CampaignProvenanceRecord(
                        KnowledgeSourceKind.ExpeditionSurvey,
                        "survey_team_1",
                        "survey_engine",
                        9,
                        InformationConfidence.High),
                    Traits = new List<string> { "rad_zone" }
                }
            };

            var projection = CartographySystem.ProjectCanonicalMap(nodes, knowledge);

            Assert.Equal(new[] { "loc_a", "loc_b" }, projection.Select(p => p.NodeId));
            Assert.Equal(MapFogState.Unknown, projection[0].FogState);
            Assert.Equal(0f, projection[0].SurveyQuality);
            Assert.Equal(MapQualityTier.Standard, projection[1].QualityTier);
            Assert.Equal("survey_team_1", projection[1].ProvenanceSourceId);
            Assert.Equal(KnowledgeSourceKind.ExpeditionSurvey, projection[1].ProvenanceKind);
            Assert.Equal(new[] { "rad_zone" }, projection[1].Traits);
        }

        [Fact]
        public void ProjectCanonicalMap_DoesNotMutateKnowledgeRecords()
        {
            var nodes = new List<MapNode> { new MapNode { Id = "loc_a", DisplayName = "A" } };
            var knowledge = new List<MapNodeKnowledgeState>
            {
                new MapNodeKnowledgeState { NodeId = "loc_a", FogState = MapFogState.Visited, Traits = new List<string> { "safe" } }
            };

            var projection = CartographySystem.ProjectCanonicalMap(nodes, knowledge);
            ((List<string>)projection[0].Traits)[0] = "changed";

            Assert.Equal("safe", knowledge[0].Traits[0]);
        }
    }
}
