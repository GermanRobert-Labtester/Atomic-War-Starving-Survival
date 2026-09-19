// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class WildlifeMapOverlayTests
    {
        [Fact]
        public void MergeSectorAdjacency_AddsNeighborsWithoutReplacingSeedGraph()
        {
            var wildlife = new WildlifeMigrationSystem();
            wildlife.SetSectorAdjacency(new[]
            {
                ("sector_home", new List<string> { "sector_east" })
            });

            wildlife.MergeSectorAdjacency(new[]
            {
                ("sector_home", new List<string> { "sector_east", "sector_north" }),
                ("sector_north", new List<string> { "sector_home" })
            });

            Assert.True(wildlife.TryGetNeighbors("sector_home", out var home));
            Assert.Contains("sector_east", home);
            Assert.Contains("sector_north", home);
            Assert.True(wildlife.TryGetNeighbors("sector_north", out var north));
            Assert.Contains("sector_home", north);
        }
    }
}
