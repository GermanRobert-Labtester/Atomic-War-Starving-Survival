// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core.World;

namespace Ashfall.Core.Tests
{
    public sealed class LivingMapRouteProjectionTests
    {
        [Fact]
        public void EmptyRoute_ReturnsInvalidProjection()
        {
            var nodes = new List<MapNode>
            {
                new MapNode { Id = "loc_a", StartingUnlocked = true },
                new MapNode { Id = "loc_b", StartingUnlocked = false }
            };
            var routes = new List<MapRoute>();
            var system = new WastelandMapSystem(new WastelandMapState(), nodes, routes);

            var projection = system.ProjectLivingMapRoute("loc_a", "loc_b");

            Assert.NotNull(projection);
            Assert.False(projection.IsValid);
            Assert.Equal("loc_a", projection.OriginNodeId);
            Assert.Equal("loc_b", projection.DestinationNodeId);
            Assert.Empty(projection.NodeIds);
            Assert.Equal(0, projection.TotalHops);
            Assert.Equal(0f, projection.TotalDistanceKm);
            Assert.False(projection.HasFloodedEdges);
            Assert.False(projection.HasAmphibiousEdges);
            Assert.Empty(projection.AllTags);
        }

        [Fact]
        public void SingleHopFloodedRoute_ProjectsHazardsAndTags()
        {
            var nodes = new List<MapNode>
            {
                new MapNode { Id = "loc_holdfast", StartingUnlocked = true },
                new MapNode { Id = "loc_swamp", StartingUnlocked = true }
            };

            var floodedRoute = new MapRoute
            {
                From = "loc_holdfast",
                To = "loc_swamp",
                DistanceKm = 8.5f,
                Tags = new List<string> { "flooded", "amphibious", "swamp_marsh" }
            };

            var system = new WastelandMapSystem(new WastelandMapState(), nodes, new List<MapRoute> { floodedRoute });

            var projection = system.ProjectLivingMapRoute("loc_holdfast", "loc_swamp");

            Assert.NotNull(projection);
            Assert.True(projection.IsValid);
            Assert.Equal("loc_holdfast", projection.OriginNodeId);
            Assert.Equal("loc_swamp", projection.DestinationNodeId);
            Assert.Equal(2, projection.NodeIds.Count);
            Assert.Equal("loc_holdfast", projection.NodeIds[0]);
            Assert.Equal("loc_swamp", projection.NodeIds[1]);
            Assert.Equal(1, projection.TotalHops);
            Assert.Equal(8.5f, projection.TotalDistanceKm);
            Assert.True(projection.HasFloodedEdges);
            Assert.True(projection.HasAmphibiousEdges);
            Assert.Contains("flooded", projection.AllTags);
            Assert.Contains("amphibious", projection.AllTags);
            Assert.Contains("swamp_marsh", projection.AllTags);
        }

        [Fact]
        public void MultiHopRoute_AggregatesDistanceHopsAndHazards()
        {
            var nodes = new List<MapNode>
            {
                new MapNode { Id = "loc_a", StartingUnlocked = true },
                new MapNode { Id = "loc_b", StartingUnlocked = true },
                new MapNode { Id = "loc_c", StartingUnlocked = true }
            };

            var route1 = new MapRoute
            {
                From = "loc_a",
                To = "loc_b",
                DistanceKm = 10f,
                Tags = new List<string> { "flooded" }
            };

            var route2 = new MapRoute
            {
                From = "loc_b",
                To = "loc_c",
                DistanceKm = 15.5f,
                Tags = new List<string> { "rocky" }
            };

            var system = new WastelandMapSystem(new WastelandMapState(), nodes, new List<MapRoute> { route1, route2 });

            var projection = system.ProjectLivingMapRoute("loc_a", "loc_c");

            Assert.NotNull(projection);
            Assert.True(projection.IsValid);
            Assert.Equal("loc_a", projection.OriginNodeId);
            Assert.Equal("loc_c", projection.DestinationNodeId);
            Assert.Equal(3, projection.NodeIds.Count);
            Assert.Equal(2, projection.TotalHops);
            Assert.Equal(25.5f, projection.TotalDistanceKm);
            Assert.True(projection.HasFloodedEdges);
            Assert.False(projection.HasAmphibiousEdges);
            Assert.Contains("flooded", projection.AllTags);
            Assert.Contains("rocky", projection.AllTags);
        }

        [Fact]
        public void LivingMapRouteProjection_DirectConstructor_HandlesEdgeCases()
        {
            var emptyProj = new LivingMapRouteProjection(null, null, null, -5f, 0, false, false, null, false);
            Assert.Equal(string.Empty, emptyProj.OriginNodeId);
            Assert.Equal(string.Empty, emptyProj.DestinationNodeId);
            Assert.Empty(emptyProj.NodeIds);
            Assert.False(emptyProj.IsValid);
            Assert.Equal(0f, emptyProj.TotalDistanceKm);
            Assert.Equal(0, emptyProj.TotalHops);
            Assert.False(emptyProj.HasFloodedEdges);
            Assert.False(emptyProj.HasAmphibiousEdges);
            Assert.Empty(emptyProj.AllTags);
        }
    }
}
