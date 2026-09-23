// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class MapRouteHazardEvaluatorTests
    {
        [Fact]
        public void EvaluateTraversal_AmphibiousRouteWithoutAmphibiousVehicle_Refused()
        {
            var route = new MapRoute
            {
                From = "loc_shelter",
                To = "loc_marsh",
                DistanceKm = 15,
                Tags = new List<string> { "amphibious" }
            };

            var result = MapRouteHazardEvaluator.EvaluateTraversal(
                route,
                hasAmphibiousCapability: false);

            Assert.False(result.IsTraversable);
            Assert.Contains("amphibious capability required", result.RefusalReason);
        }

        [Fact]
        public void EvaluateTraversal_FloodedRoute_LowClearance_Refused()
        {
            var route = new MapRoute
            {
                From = "loc_shelter",
                To = "loc_valley",
                DistanceKm = 10,
                Tags = new List<string> { "flooded" }
            };

            var result = MapRouteHazardEvaluator.EvaluateTraversal(
                route,
                hasAmphibiousCapability: false,
                vehicleGroundClearanceMm: 200);

            Assert.False(result.IsTraversable);
            Assert.Contains("insufficient vehicle ground clearance", result.RefusalReason);
        }

        [Fact]
        public void EvaluateTraversal_FloodedRoute_HighClearance_AllowsWithDelay()
        {
            var route = new MapRoute
            {
                From = "loc_shelter",
                To = "loc_valley",
                DistanceKm = 10,
                Tags = new List<string> { "flooded" }
            };

            var result = MapRouteHazardEvaluator.EvaluateTraversal(
                route,
                hasAmphibiousCapability: false,
                vehicleGroundClearanceMm: 350);

            Assert.True(result.IsTraversable);
            Assert.Equal(2, result.DelayDays);
            Assert.Equal(400, result.RiskScorePermille); // 50 base + 350 flood
        }

        [Fact]
        public void EvaluateTraversal_MudHazardInRainSeason_AddsDelayAndRisk()
        {
            var route = new MapRoute
            {
                From = "loc_a",
                To = "loc_b",
                DistanceKm = 8,
                Tags = new List<string> { "mud_hazard" }
            };

            var dryResult = MapRouteHazardEvaluator.EvaluateTraversal(
                route,
                hasAmphibiousCapability: false,
                isHeavyRainOrFloodSeason: false);

            var rainyResult = MapRouteHazardEvaluator.EvaluateTraversal(
                route,
                hasAmphibiousCapability: false,
                isHeavyRainOrFloodSeason: true);

            Assert.True(dryResult.IsTraversable);
            Assert.Equal(0, dryResult.DelayDays);

            Assert.True(rainyResult.IsTraversable);
            Assert.Equal(1, rainyResult.DelayDays);
            Assert.Equal(250, rainyResult.RiskScorePermille); // 50 base + 200 rain
        }
    }
}
