// SPDX-License-Identifier: MIT
using Ashfall.Core.Expeditions;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    /// <summary>
    /// Plans 146–149 MED: estimate/preview travel stretch must match the
    /// live Start projection formula (ceil, min 1, no registry mutation).
    /// </summary>
    public sealed class ExpeditionTravelStretchTests
    {
        [Fact]
        public void ProjectDistanceTicks_NeutralMultiplier_LeavesBaseUnchanged()
        {
            Assert.Equal(8, ExpeditionTravelStretch.ProjectDistanceTicks(8, 1f));
            Assert.Equal(8, ExpeditionTravelStretch.ProjectDistanceTicks(8, 1.0005f));
            Assert.Equal(8, ExpeditionTravelStretch.ProjectDistanceTicks(8, 0f));
        }

        [Fact]
        public void ProjectDistanceTicks_SlowRail_StretchesWithCeil()
        {
            // 8 ticks * 2.0 → 16; 8 * 1.25 → 10; 1 * 1.1 → ceil 2
            Assert.Equal(16, ExpeditionTravelStretch.ProjectDistanceTicks(8, 2f));
            Assert.Equal(10, ExpeditionTravelStretch.ProjectDistanceTicks(8, 1.25f));
            Assert.Equal(2, ExpeditionTravelStretch.ProjectDistanceTicks(1, 1.1f));
        }

        [Fact]
        public void ProjectDefinition_DoesNotMutateSource_AndMatchesRouteModifier()
        {
            var routes = new RouteInfrastructureSystem();
            routes.RegisterRailSegment("rail_slow", "s1", 0.9f, 25f, 1);
            float mult = routes.GetTravelModifier("rail_slow");
            Assert.True(mult > 1f);

            var source = new ExpeditionDefinition
            {
                id = "rail_slow",
                displayName = "Slow Rail",
                distanceTicks = 8,
                dangerLevel = 2,
                lootCategories = new System.Collections.Generic.List<string> { "scrap" }
            };

            var projected = ExpeditionTravelStretch.ProjectDefinition(source, mult);
            Assert.Equal(8, source.distanceTicks);
            Assert.True(projected.distanceTicks > source.distanceTicks);
            Assert.Equal(
                ExpeditionTravelStretch.ProjectDistanceTicks(8, mult),
                projected.distanceTicks);
            Assert.Equal(source.id, projected.id);
            Assert.NotSame(source, projected);
        }

        [Fact]
        public void ProjectDefinition_Neutral_ReturnsSameInstance()
        {
            var source = new ExpeditionDefinition { id = "x", distanceTicks = 5 };
            var projected = ExpeditionTravelStretch.ProjectDefinition(source, 1f);
            Assert.Same(source, projected);
        }
    }
}
