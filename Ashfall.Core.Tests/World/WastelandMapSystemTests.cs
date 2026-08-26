using System;
using System.Collections.Generic;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class WastelandMapSystemTests
    {
        private static WastelandMapSystem MakeMap()
        {
            var nodes = new List<MapNode>
            {
                new MapNode { Id = "a", DisplayName = "A", Danger = MapNodeDanger.None,
                    PositionX = 0, PositionY = 0, StartingUnlocked = true },
                new MapNode { Id = "b", DisplayName = "B", Danger = MapNodeDanger.Low,
                    PositionX = 10, PositionY = 0, Discoverable = true },
                new MapNode { Id = "c", DisplayName = "C", Danger = MapNodeDanger.High,
                    PositionX = 20, PositionY = 0, Discoverable = true },
                new MapNode { Id = "d", DisplayName = "D", Danger = MapNodeDanger.Medium,
                    PositionX = 5, PositionY = 5, Discoverable = true },
                new MapNode { Id = "isolated", DisplayName = "Isolated",
                    Danger = MapNodeDanger.Locked, PositionX = 100, PositionY = 100,
                    Discoverable = true }
            };
            var routes = new List<MapRoute>
            {
                new MapRoute { From = "a", To = "b", DistanceKm = 10 },
                new MapRoute { From = "b", To = "c", DistanceKm = 12 },
                new MapRoute { From = "a", To = "d", DistanceKm = 5 },
                new MapRoute { From = "d", To = "c", DistanceKm = 8 }
            };
            return new WastelandMapSystem(new WastelandMapState(), nodes, routes);
        }

        [Fact]
        public void StartingNodes_DiscoveredByDefault()
        {
            var m = MakeMap();
            Assert.True(m.IsDiscovered("a"));
            Assert.False(m.IsDiscovered("b"));
        }

        [Fact]
        public void Discover_AddsNodeAndIsIdempotent()
        {
            var m = MakeMap();
            Assert.True(m.Discover("b"));
            Assert.True(m.IsDiscovered("b"));
            Assert.True(m.Discover("b")); // idempotent
            var count = 0;
            for (int i = 0; i < m.Nodes.Count; i++)
                if (m.IsDiscovered(m.Nodes[i].Id)) count++;
            Assert.Equal(2, count);
        }

        [Fact]
        public void Discover_UnknownFails()
        {
            var m = MakeMap();
            Assert.False(m.Discover("ghost"));
        }

        [Fact]
        public void Events_FireOnDiscover()
        {
            var m = MakeMap();
            string? captured = null;
            m.OnNodeDiscovered += id => captured = id;
            m.Discover("b");
            Assert.Equal("b", captured);
        }

        [Fact]
        public void PlanRoute_ShortestPath()
        {
            var m = MakeMap();
            m.Discover("b");
            m.Discover("c");
            m.Discover("d");
            // a → d (5km) → c (8km) = 13km, vs a → b → c = 22km
            var path = m.PlanRoute("a", "c");
            Assert.Equal(3, path.Count);
            Assert.Equal("a", path[0]);
            Assert.Equal("c", path[2]);
        }

        [Fact]
        public void PlanRoute_NoPath_ReturnsEmpty()
        {
            var m = MakeMap();
            // 'isolated' has no routes
            var path = m.PlanRoute("a", "isolated");
            Assert.Empty(path);
        }

        [Fact]
        public void PlanRoute_RequiresBothEndsDiscovered()
        {
            var m = MakeMap();
            var path = m.PlanRoute("a", "b"); // b not discovered
            Assert.Empty(path);
        }

        [Fact]
        public void PlanRoute_DeterministicForSameState()
        {
            var m1 = MakeMap();
            var m2 = MakeMap();
            foreach (var m in new[] { m1, m2 })
            {
                m.Discover("b");
                m.Discover("c");
                m.Discover("d");
            }
            var p1 = m1.PlanRoute("a", "c");
            var p2 = m2.PlanRoute("a", "c");
            Assert.Equal(p1, p2);
        }

        [Fact]
        public void GetRoutesFrom_ReturnsOutboundEdges()
        {
            var m = MakeMap();
            var routes = m.GetRoutesFrom("a");
            Assert.Equal(2, routes.Count); // a→b, a→d
        }

        [Fact]
        public void CaptureRestore_RoundTrip()
        {
            var m = MakeMap();
            m.Discover("b");
            var save = m.CaptureState();
            var fresh = MakeMap();
            fresh.RestoreState(save);
            Assert.True(fresh.IsDiscovered("b"));
        }

        [Fact]
        public void UndiscoveredNodes_AreNotInRoutes()
        {
            var m = MakeMap();
            m.Discover("b");
            // 'c' is not discovered, so a→b→c cannot complete.
            var path = m.PlanRoute("a", "c");
            Assert.Empty(path);
        }
    }
}
