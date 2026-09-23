// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Campaign;
using Ashfall.Core.Exploration;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Plan163Cartography
{
    public sealed class Plan163CartographyIntegrationTests
    {
        private static string ResolveDataPath(string filename)
        {
            string[] candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates)
            {
                if (File.Exists(c)) return Path.GetFullPath(c);
            }
            return Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename);
        }

        [Fact]
        public void CatalogIntegrity_MapRegionsJson_LoadsAllCanonicalRegions()
        {
            string path = ResolveDataPath("map_regions.json");
            Assert.True(File.Exists(path), $"Catalog missing at: {path}");

            string json = File.ReadAllText(path);
            var system = new CartographySystem();
            system.LoadCatalog(json);

            Assert.True(system.TotalRegionCount >= 8, "Expected at least 8 map regions in catalog.");

            var terrains = system.Regions.Select(r => r.Terrain).Distinct().ToList();
            Assert.Contains(TerrainCategory.Urban, terrains);
            Assert.Contains(TerrainCategory.Rural, terrains);
            Assert.Contains(TerrainCategory.Industrial, terrains);
            Assert.Contains(TerrainCategory.Wasteland, terrains);
            Assert.Contains(TerrainCategory.Water, terrains);

            var metropolis = system.GetRegion("reg_ruined_metropolis");
            Assert.NotNull(metropolis);
            Assert.Equal("Ironspire Metropolis", metropolis.RegionName);
            Assert.Equal(TerrainCategory.Urban, metropolis.Terrain);
            Assert.Contains("loc_metro_station", metropolis.KnownPoiIds);
            Assert.Equal(1.6f, metropolis.ScoutingDifficulty);
        }

        [Fact]
        public void DiscoverRegion_LiftsFogOfWar_AndFiresDiscoverySeam()
        {
            string path = ResolveDataPath("map_regions.json");
            var system = new CartographySystem();
            if (File.Exists(path)) system.LoadCatalog(File.ReadAllText(path));

            MapRegion? discoveredFromSeam = null;
            system.OnRegionDiscoveredSeam = r => discoveredFromSeam = r;

            bool success = system.DiscoverRegion("reg_ash_valley", "scout_kestrel", currentDay: 3);

            Assert.True(success);
            Assert.NotNull(discoveredFromSeam);
            Assert.Equal("reg_ash_valley", discoveredFromSeam.RegionId);
            Assert.True(discoveredFromSeam.IsDiscovered);
            Assert.True(discoveredFromSeam.ExplorationProgress >= 20f);
        }

        [Fact]
        public void SurveyRegion_AppliesEquipmentBonuses_AdvancesProficiencyAndFiresSeams()
        {
            string path = ResolveDataPath("map_regions.json");
            var system = new CartographySystem();
            if (File.Exists(path)) system.LoadCatalog(File.ReadAllText(path));

            MapDiscovery? discoveryFromSeam = null;
            CartographySkill? skillFromSeam = null;
            system.OnPoiMappedSeam = d => discoveryFromSeam = d;
            system.OnCartographySkillAdvancedSeam = s => skillFromSeam = s;

            float toolBonus = CartographySystem.GetEquipmentBonus("survey_tools");
            Assert.Equal(30f, toolBonus);

            var discovery = system.SurveyRegion("reg_iron_ridge", "surveyor_elias", currentDay: 5, equipmentBonus: toolBonus);

            Assert.NotNull(discovery);
            Assert.NotNull(discoveryFromSeam);
            Assert.Equal(discovery, discoveryFromSeam);
            Assert.Equal("reg_iron_ridge", discovery.RegionId);
            Assert.True(discovery.Quality >= 40f);

            Assert.NotNull(skillFromSeam);
            Assert.Equal("surveyor_elias", skillFromSeam.SurvivorId);
            Assert.Equal(17.5f, skillFromSeam.Proficiency); // Started at 15 + 2.5
            Assert.Equal(1, skillFromSeam.MapsCreated);

            var region = system.GetRegion("reg_iron_ridge");
            Assert.NotNull(region);
            Assert.True(region.ExplorationProgress > 0f);
        }

        [Fact]
        public void PurchaseOrAcquireMap_ExpandsRegionalExplorationAndMapCompleteness()
        {
            var system = new CartographySystem();
            system.RegisterRegion("reg_a", "Region A", TerrainCategory.Wasteland);
            system.RegisterRegion("reg_b", "Region B", TerrainCategory.Rural);

            Assert.Equal(0f, system.GetMapCompleteness());

            system.PurchaseOrAcquireMap("reg_a", qualityBoost: 80f);
            Assert.Equal(40.0f, system.GetMapCompleteness());

            system.PurchaseOrAcquireMap("reg_b", qualityBoost: 60f);
            Assert.Equal(70.0f, system.GetMapCompleteness());
        }

        [Fact]
        public void ProjectCanonicalMap_ProjectsWorldKnowledgeWithoutGraphDuplication()
        {
            var nodes = new[]
            {
                new MapNode { Id = "node_bunker", DisplayName = "Primary Vault" },
                new MapNode { Id = "node_relay", DisplayName = "Relay Hill" },
                new MapNode { Id = "node_outpost", DisplayName = "Wasteland Post" }
            };

            var knowledge = new[]
            {
                new MapNodeKnowledgeState { NodeId = "node_bunker", FogState = MapFogState.Visited, LastConfirmedDay = 10 },
                new MapNodeKnowledgeState { NodeId = "node_relay", FogState = MapFogState.Surveyed, LastConfirmedDay = 5 },
                new MapNodeKnowledgeState { NodeId = "node_outpost", FogState = MapFogState.Rumored, LastConfirmedDay = 2 }
            };

            var survey = CartographySystem.ProjectCanonicalMap(nodes, knowledge);

            Assert.Equal(3, survey.Count);

            var bunker = survey.First(s => s.NodeId == "node_bunker");
            Assert.Equal(100f, bunker.SurveyQuality);
            Assert.Equal(MapQualityTier.Detailed, bunker.QualityTier);

            var relay = survey.First(s => s.NodeId == "node_relay");
            Assert.Equal(65f, relay.SurveyQuality);
            Assert.Equal(MapQualityTier.Standard, relay.QualityTier);

            var outpost = survey.First(s => s.NodeId == "node_outpost");
            Assert.Equal(30f, outpost.SurveyQuality);
            Assert.Equal(MapQualityTier.Rough, outpost.QualityTier);
        }

        [Fact]
        public void CaptureAndRestoreState_PreservesFullCartographyEcosystem()
        {
            string path = ResolveDataPath("map_regions.json");
            var system1 = new CartographySystem();
            if (File.Exists(path)) system1.LoadCatalog(File.ReadAllText(path));

            system1.DiscoverRegion("reg_ash_valley", "scout_1", 2);
            system1.SurveyRegion("reg_ash_valley", "surveyor_1", 3, 20f);

            var state = system1.CaptureState();
            Assert.Equal(1, state.SchemaVersion);
            Assert.True(state.Regions.Count >= 8);
            Assert.Single(state.Discoveries);
            Assert.Single(state.Cartographers);

            var system2 = new CartographySystem();
            system2.RestoreState(state);

            Assert.Equal(system1.TotalRegionCount, system2.TotalRegionCount);
            Assert.Equal(1, system2.DiscoveredRegionCount);
            Assert.Single(system2.Discoveries);

            var restoredValley = system2.GetRegion("reg_ash_valley");
            Assert.NotNull(restoredValley);
            Assert.True(restoredValley.IsDiscovered);
            Assert.True(restoredValley.ExplorationProgress > 0f);
        }
    }
}
