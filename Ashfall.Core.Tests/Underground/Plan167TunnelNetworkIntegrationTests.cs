// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Underground;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Underground
{
    public sealed class Plan167TunnelNetworkIntegrationTests
    {
        private static WastelandMapSystem CreateTestMap()
        {
            var nodes = new List<MapNode>
            {
                new MapNode
                {
                    Id = "loc_holdfast",
                    DisplayName = "Holdfast Shelter",
                    Danger = MapNodeDanger.None,
                    StartingUnlocked = true,
                    PositionX = 100,
                    PositionY = 100
                },
                new MapNode
                {
                    Id = "loc_cut_abandoned_depot",
                    DisplayName = "Abandoned Depot",
                    Danger = MapNodeDanger.Low,
                    StartingUnlocked = false,
                    PositionX = 200,
                    PositionY = 200
                },
                new MapNode
                {
                    Id = "loc_cut_radiation_zone_alpha",
                    DisplayName = "Radiation Zone Alpha",
                    Danger = MapNodeDanger.High,
                    StartingUnlocked = false,
                    PositionX = 300,
                    PositionY = 300
                }
            };

            var routes = new List<MapRoute>
            {
                new MapRoute { From = "loc_holdfast", To = "loc_cut_abandoned_depot", DistanceKm = 5f },
                new MapRoute { From = "loc_cut_abandoned_depot", To = "loc_cut_radiation_zone_alpha", DistanceKm = 8f }
            };

            return new WastelandMapSystem(new WastelandMapState(), nodes, routes);
        }

        [Fact]
        public void WastelandMapSystem_InitializesTunnels_WithCanonicalDefaults()
        {
            var map = CreateTestMap();

            Assert.NotNull(map.Tunnels);
            Assert.True(map.Tunnels.TotalSegmentCount >= 2);
            Assert.True(map.Tunnels.JunctionCount >= 3);
        }

        [Fact]
        public void CanTraverseTunnel_RespectsDiscoveryAndStructuralIntegrity()
        {
            var map = CreateTestMap();

            // Undiscovered initially
            var beforeDiscovery = map.CanTraverseTunnel("loc_holdfast", "loc_cut_abandoned_depot");
            Assert.False(beforeDiscovery.CanTraverse);

            // Discover tunnel segment
            bool discovered = map.DiscoverTunnel("tun_holdfast_depot");
            Assert.True(discovered);

            var afterDiscovery = map.CanTraverseTunnel("loc_holdfast", "loc_cut_abandoned_depot");
            Assert.True(afterDiscovery.CanTraverse);
            Assert.Equal(1.5f, afterDiscovery.TravelTimeHours);

            // Traversal is bidirectional
            var reverse = map.CanTraverseTunnel("loc_cut_abandoned_depot", "loc_holdfast");
            Assert.True(reverse.CanTraverse);
        }

        [Fact]
        public void RepairTunnel_RestoresCollapsedTunnel()
        {
            var map = CreateTestMap();
            map.DiscoverTunnel("tun_holdfast_depot");

            var segment = map.Tunnels.GetDiscoveredSegments().First(s => s.SegmentId == "tun_holdfast_depot");
            segment.StructuralIntegrity = 0f;
            segment.Status = TunnelStatus.Collapsed;

            var blocked = map.CanTraverseTunnel("loc_holdfast", "loc_cut_abandoned_depot");
            Assert.False(blocked.CanTraverse);

            bool repaired = map.RepairTunnel("tun_holdfast_depot", repairAmount: 60f);
            Assert.True(repaired);
            Assert.Equal(60f, segment.StructuralIntegrity);
            Assert.Equal(TunnelStatus.Clear, segment.Status);

            var clear = map.CanTraverseTunnel("loc_holdfast", "loc_cut_abandoned_depot");
            Assert.True(clear.CanTraverse);
        }

        [Fact]
        public void AutoDiscovery_WhenEndpointsSurveyed()
        {
            var map = CreateTestMap();

            // loc_holdfast is starting unlocked (discovered). Surveying depot connects both endpoints.
            var before = map.CanTraverseTunnel("loc_holdfast", "loc_cut_abandoned_depot");
            Assert.False(before.CanTraverse);

            map.DiscoverSurvey("loc_cut_abandoned_depot", "surveyor_1", day: 3);

            var after = map.CanTraverseTunnel("loc_holdfast", "loc_cut_abandoned_depot");
            Assert.True(after.CanTraverse);
        }

        [Fact]
        public void DailyTick_DegradesTunnelIntegrity()
        {
            var map = CreateTestMap();
            map.DiscoverTunnel("tun_holdfast_depot");

            var segment = map.Tunnels.GetDiscoveredSegments().First(s => s.SegmentId == "tun_holdfast_depot");
            segment.StructuralIntegrity = 20f;

            map.Tunnels.TickDay(currentDay: 5);

            Assert.True(segment.StructuralIntegrity < 20f);
            Assert.Contains(TunnelHazardType.CollapseRisk, segment.Hazards);
        }

        [Fact]
        public void SaveRestore_RoundTrips_TunnelState_ThroughWastelandMapSave()
        {
            var map1 = CreateTestMap();
            map1.DiscoverTunnel("tun_holdfast_depot");
            var seg1 = map1.Tunnels.GetDiscoveredSegments().First(s => s.SegmentId == "tun_holdfast_depot");
            seg1.StructuralIntegrity = 85f;

            var saved = map1.CaptureState();
            Assert.NotNull(saved.Tunnels);
            Assert.Single(saved.Tunnels.Segments, s => s.IsDiscovered);

            var map2 = CreateTestMap();
            map2.RestoreState(saved);

            Assert.Equal(1, map2.GetDiscoveredTunnels().Count);
            var res = map2.CanTraverseTunnel("loc_holdfast", "loc_cut_abandoned_depot");
            Assert.True(res.CanTraverse);

            var restoredSeg = map2.Tunnels.GetDiscoveredSegments().First(s => s.SegmentId == "tun_holdfast_depot");
            Assert.Equal(85f, restoredSeg.StructuralIntegrity);
        }

        [Fact]
        public void AuthoredData_UndergroundTunnelsJson_LoadsAndCanBeReinforced()
        {
            var dataDir = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
                    "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            if (!System.IO.File.Exists(System.IO.Path.Combine(dataDir, "underground_tunnels.json")))
                dataDir = System.IO.Path.GetFullPath("Assets/StreamingAssets/Data");

            string filePath = System.IO.Path.Combine(dataDir, "underground_tunnels.json");
            Assert.True(System.IO.File.Exists(filePath), $"File not found: {filePath}");

            string json = System.IO.File.ReadAllText(filePath);
            var catalog = System.Text.Json.JsonSerializer.Deserialize<TunnelNetworkCatalogData>(json);
            Assert.NotNull(catalog);
            Assert.True(catalog!.junctions.Count >= 3);
            Assert.True(catalog.segments.Count >= 3);

            var system = new TunnelNetworkSystem();
            system.LoadCatalog(catalog);
            Assert.Equal(3, system.TotalSegmentCount);
            Assert.Equal(3, system.JunctionCount);

            // Reinforce conduit A
            var conduit = system.FindSegment("seg_shelter_to_depot");
            Assert.NotNull(conduit);
            float intBefore = conduit!.StructuralIntegrity;
            bool reinforced = system.ReinforceSegment("seg_shelter_to_depot", 10f);
            Assert.True(reinforced);
            Assert.Equal(Math.Min(100f, intBefore + 10f), conduit.StructuralIntegrity);

            // Clear hazard on Mine Rail B
            var rail = system.FindSegment("seg_depot_to_radzone");
            Assert.NotNull(rail);
            Assert.Contains(TunnelHazardType.ToxicGas, rail!.Hazards);
            bool cleared = system.ClearHazard("seg_depot_to_radzone", TunnelHazardType.ToxicGas);
            Assert.True(cleared);
            Assert.DoesNotContain(TunnelHazardType.ToxicGas, rail.Hazards);

            // Discover and evaluate surface bypass
            system.DiscoverSegment("seg_shelter_to_depot");
            var bypass = system.EvaluateSurfaceBypass("loc_holdfast", "loc_cut_abandoned_depot");
            Assert.True(bypass.Found);
            Assert.True(bypass.SavedHours > 0f);
        }
    }
}
