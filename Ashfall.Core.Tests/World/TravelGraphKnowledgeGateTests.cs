using System.Collections.Generic;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class TravelGraphKnowledgeGateTests
    {
        [Fact]
        public void EvaluateAccess_UnknownOrRumoredRoute_RefusesDispatch()
        {
            var route = new MapRoute
            {
                From = "loc_shelter",
                To = "loc_pass",
                DistanceKm = 12
            };

            var unknownResult = TravelGraphKnowledgeGate.EvaluateAccess(route, RouteKnowledgeTier.Unknown);
            var rumoredResult = TravelGraphKnowledgeGate.EvaluateAccess(route, RouteKnowledgeTier.Rumored);

            Assert.False(unknownResult.IsAccessible);
            Assert.Contains("unexplored territory", unknownResult.RefusalReason);

            Assert.False(rumoredResult.IsAccessible);
            Assert.Contains("only rumored", rumoredResult.RefusalReason);
        }

        [Fact]
        public void EvaluateAccess_ScoutedRoute_AllowsFootDispatch_RefusesHeavyConvoy()
        {
            var route = new MapRoute
            {
                From = "loc_shelter",
                To = "loc_pass",
                DistanceKm = 12,
                WeatherHazard = 0.2f
            };

            var footResult = TravelGraphKnowledgeGate.EvaluateAccess(route, RouteKnowledgeTier.Scouted, isHeavyVehicleConvoy: false);
            var convoyResult = TravelGraphKnowledgeGate.EvaluateAccess(route, RouteKnowledgeTier.Scouted, isHeavyVehicleConvoy: true);

            Assert.True(footResult.IsAccessible);
            Assert.False(convoyResult.IsAccessible);
            Assert.Contains("requires fully surveyed and mapped corridor", convoyResult.RefusalReason);
        }

        [Fact]
        public void EvaluateAccess_AerialSurvey_PromotesKnowledgeTier()
        {
            var route = new MapRoute
            {
                From = "loc_shelter",
                To = "loc_pass",
                DistanceKm = 12
            };

            // Aerial survey promotes Rumored to Scouted, making it accessible on foot
            var result = TravelGraphKnowledgeGate.EvaluateAccess(
                route,
                knowledge: RouteKnowledgeTier.Rumored,
                isHeavyVehicleConvoy: false,
                hasAerialSurvey: true
            );

            Assert.True(result.IsAccessible);
            Assert.Equal(RouteKnowledgeTier.Scouted, result.CurrentKnowledge);
        }

        [Fact]
        public void EvaluateAccess_MappedRoute_AllowsAllDispatch()
        {
            var route = new MapRoute
            {
                From = "loc_shelter",
                To = "loc_outpost",
                DistanceKm = 25,
                WeatherHazard = 0.1f,
                Tags = new List<string> { "flooded" }
            };

            var convoyResult = TravelGraphKnowledgeGate.EvaluateAccess(route, RouteKnowledgeTier.Mapped, isHeavyVehicleConvoy: true);

            Assert.True(convoyResult.IsAccessible);
            Assert.Equal(RouteKnowledgeTier.Mapped, convoyResult.CurrentKnowledge);
            Assert.True(convoyResult.EffectiveHazardScorePermille >= 300); // 50 base + 250 flooded
        }
    }
}
