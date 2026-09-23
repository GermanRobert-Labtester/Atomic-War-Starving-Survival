// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core.World;

namespace Ashfall.Core.Tests
{
    public sealed class WastelandMapRouteTagTests
    {
        [Fact]
        public void MapRouteDef_Tags_DefaultsEmpty_AndQueriesCaseInsensitive()
        {
            var def = new MapRouteDef
            {
                from = "loc_a",
                to = "loc_b",
                distanceKm = 10f
            };

            Assert.NotNull(def.tags);
            Assert.Empty(def.tags);
            Assert.False(def.HasTag("flooded"));
            Assert.False(def.IsFlooded);
            Assert.False(def.IsAmphibious);

            def.tags.Add("Flooded");
            def.tags.Add("Amphibious");
            def.tags.Add("Rough_Terrain");

            Assert.True(def.HasTag("flooded"));
            Assert.True(def.HasTag("FLOODED"));
            Assert.True(def.IsFlooded);
            Assert.True(def.IsAmphibious);
            Assert.True(def.HasTag("rough_terrain"));
            Assert.False(def.HasTag("nonexistent"));
        }

        [Fact]
        public void MapRoute_Tags_DefaultsEmpty_AndQueriesCaseInsensitive()
        {
            var route = new MapRoute
            {
                From = "loc_a",
                To = "loc_b",
                DistanceKm = 15f
            };

            Assert.NotNull(route.Tags);
            Assert.Empty(route.Tags);
            Assert.False(route.HasTag("flooded"));
            Assert.False(route.IsFlooded);
            Assert.False(route.IsAmphibious);

            route.Tags.Add("flooded");
            route.Tags.Add("amphibious");

            Assert.True(route.HasTag("flooded"));
            Assert.True(route.IsFlooded);
            Assert.True(route.HasTag("amphibious"));
            Assert.True(route.IsAmphibious);
            Assert.False(route.HasTag("mountain_pass"));
        }

        [Fact]
        public void WastelandMapSystem_RouteTagQueries_WorkCorrectly()
        {
            var nodes = new List<MapNode>
            {
                new MapNode { Id = "loc_holdfast", StartingUnlocked = true },
                new MapNode { Id = "loc_swamp", StartingUnlocked = true },
                new MapNode { Id = "loc_mountain", StartingUnlocked = true }
            };

            var floodedRoute = new MapRoute
            {
                From = "loc_holdfast",
                To = "loc_swamp",
                DistanceKm = 5f,
                Tags = new List<string> { "flooded", "amphibious" }
            };

            var normalRoute = new MapRoute
            {
                From = "loc_holdfast",
                To = "loc_mountain",
                DistanceKm = 12f
            };

            var system = new WastelandMapSystem(new WastelandMapState(), nodes, new List<MapRoute> { floodedRoute, normalRoute });

            // GetRoute
            var foundFlooded = system.GetRoute("loc_holdfast", "loc_swamp");
            Assert.NotNull(foundFlooded);
            Assert.Equal("loc_holdfast", foundFlooded!.From);
            Assert.Equal("loc_swamp", foundFlooded.To);

            var foundNormal = system.GetRoute("loc_holdfast", "loc_mountain");
            Assert.NotNull(foundNormal);

            var missing = system.GetRoute("loc_swamp", "loc_mountain");
            Assert.Null(missing);

            // IsRouteFlooded & HasRouteTag
            Assert.True(system.IsRouteFlooded("loc_holdfast", "loc_swamp"));
            Assert.True(system.HasRouteTag("loc_holdfast", "loc_swamp", "amphibious"));
            Assert.False(system.HasRouteTag("loc_holdfast", "loc_swamp", "mountain"));

            Assert.False(system.IsRouteFlooded("loc_holdfast", "loc_mountain"));
            Assert.False(system.HasRouteTag("loc_holdfast", "loc_mountain", "flooded"));

            // Nonexistent route queries return false
            Assert.False(system.IsRouteFlooded("loc_unknown", "loc_swamp"));
            Assert.False(system.HasRouteTag("loc_unknown", "loc_swamp", "flooded"));
        }
    }
}
