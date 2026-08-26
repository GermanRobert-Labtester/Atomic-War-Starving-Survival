using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    /// <summary>
    /// Headless map interaction test: discover node → unlock route → select destination.
    /// Proves the full player-facing flow through the Core WastelandMapSystem without
    /// any host or UI dependency.
    /// </summary>
    public class WastelandMapInteractionTests
    {
        private static WastelandMapSystem MakeMap(WastelandMapState? state = null)
        {
            var nodes = new List<MapNode>
            {
                new MapNode { Id = "loc_holdfast", DisplayName = "Holdfast",
                    Danger = MapNodeDanger.None, PositionX = 500, PositionY = 300,
                    StartingUnlocked = true },
                new MapNode { Id = "loc_abandoned_depot", DisplayName = "Abandoned Depot",
                    Danger = MapNodeDanger.Low, PositionX = 700, PositionY = 200,
                    Discoverable = true },
                new MapNode { Id = "loc_merchant_caravanserai", DisplayName = "Merchant Caravanserai",
                    Danger = MapNodeDanger.Medium, PositionX = 400, PositionY = 200,
                    Discoverable = true, StartingUnlocked = true },
                new MapNode { Id = "loc_black_flotilla", DisplayName = "Black Flotilla Outpost",
                    Danger = MapNodeDanger.High, PositionX = 600, PositionY = 500,
                    Discoverable = true },
            };
            var routes = new List<MapRoute>
            {
                new MapRoute { From = "loc_holdfast", To = "loc_abandoned_depot", DistanceKm = 12 },
                new MapRoute { From = "loc_holdfast", To = "loc_merchant_caravanserai", DistanceKm = 8 },
                new MapRoute { From = "loc_merchant_caravanserai", To = "loc_black_flotilla", DistanceKm = 22 },
                new MapRoute { From = "loc_abandoned_depot", To = "loc_black_flotilla", DistanceKm = 30 },
            };
            return new WastelandMapSystem(state ?? new WastelandMapState(), nodes, routes);
        }

        [Fact]
        public void DiscoverNode_UnlocksRoute_ToSelectDestination()
        {
            var map = MakeMap();

            // Starting state: Holdfast and Caravanserai are discovered (StartingUnlocked).
            Assert.True(map.IsDiscovered("loc_holdfast"));
            Assert.True(map.IsDiscovered("loc_merchant_caravanserai"));
            Assert.False(map.IsDiscovered("loc_abandoned_depot"));
            Assert.False(map.IsDiscovered("loc_black_flotilla"));

            // Step 1: Discover the Abandoned Depot (adjacent to Holdfast via route).
            bool discovered = map.Discover("loc_abandoned_depot");
            Assert.True(discovered);
            Assert.True(map.IsDiscovered("loc_abandoned_depot"));

            // Step 2: Route from Holdfast to Abandoned Depot is now traversable.
            var path = map.PlanRoute("loc_holdfast", "loc_abandoned_depot");
            Assert.NotEmpty(path);
            Assert.Equal("loc_holdfast", path[0]);
            Assert.Equal("loc_abandoned_depot", path[^1]);

            // Step 3: Discover Black Flotilla (reachable from Caravanserai).
            map.Discover("loc_black_flotilla");
            Assert.True(map.IsDiscovered("loc_black_flotilla"));

            // Step 4: Plan route to Black Flotilla — shortest path goes through Caravanserai (22km) not Depot (30km).
            var fullPath = map.PlanRoute("loc_holdfast", "loc_black_flotilla");
            Assert.NotEmpty(fullPath);
            Assert.Equal("loc_holdfast", fullPath[0]);
            Assert.Equal("loc_black_flotilla", fullPath[^1]);
            Assert.Contains("loc_merchant_caravanserai", fullPath);
        }

        [Fact]
        public void UndiscoveredNode_CannotBeTraversed()
        {
            var map = MakeMap();

            // Black Flotilla is not discovered — route planning to it returns empty.
            var path = map.PlanRoute("loc_holdfast", "loc_black_flotilla");
            Assert.Empty(path);
        }

        [Fact]
        public void Discover_IsIdempotent()
        {
            var map = MakeMap();

            int eventCount = 0;
            map.OnNodeDiscovered += _ => eventCount++;

            Assert.True(map.Discover("loc_abandoned_depot"));
            Assert.True(map.Discover("loc_abandoned_depot")); // second call is idempotent
            Assert.Equal(1, eventCount); // event fires only once
        }

        [Fact]
        public void SaveReload_PreservesDiscovery()
        {
            var map = MakeMap();
            map.Discover("loc_abandoned_depot");
            map.Discover("loc_black_flotilla");

            var saved = map.CaptureState();

            var restored = MakeMap(saved);
            Assert.True(restored.IsDiscovered("loc_holdfast"));
            Assert.True(restored.IsDiscovered("loc_merchant_caravanserai"));
            Assert.True(restored.IsDiscovered("loc_abandoned_depot"));
            Assert.True(restored.IsDiscovered("loc_black_flotilla"));

            var path = restored.PlanRoute("loc_holdfast", "loc_black_flotilla");
            Assert.NotEmpty(path);
        }

        [Fact]
        public void CatalogLoader_ProducesValidMap()
        {
            var sys = WastelandMapCatalogLoader.CreateSystem("Assets/StreamingAssets/Data");
            Assert.NotEmpty(sys.Nodes);
            Assert.True(sys.IsDiscovered("loc_holdfast"));
        }
    }
}
