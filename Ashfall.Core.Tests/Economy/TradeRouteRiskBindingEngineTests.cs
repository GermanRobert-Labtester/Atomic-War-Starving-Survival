using Ashfall.Core.Economy;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public class TradeRouteRiskBindingEngineTests
    {
        [Fact]
        public void EvaluateTransitRisk_SecureRouteWithEscorts_MinimalRisk()
        {
            var contract = new TradeRouteContract("route_01", "loc_haven", cadenceDays: 5, baseTariffChits: 10);
            var route = new MapRoute
            {
                From = "loc_shelter",
                To = "loc_haven",
                DistanceKm = 10,
                WeatherHazard = 0.05f
            };

            var result = TradeRouteRiskBindingEngine.EvaluateTransitRisk(
                contract,
                route,
                escortStrengthPermille: 800,
                regionalHostilityPermille: 100
            );

            Assert.True(result.IsTransitViable);
            Assert.False(result.TriggersDisruptionAlert);
            Assert.True(result.RaidProbabilityPermille < 100);
            Assert.True(result.ExpectedCargoAttritionPermille <= 50);
            Assert.Contains("Secure trade corridor", result.RiskSummary);
        }

        [Fact]
        public void EvaluateTransitRisk_HostileAndFlooded_TriggersDisruptionAlert()
        {
            var contract = new TradeRouteContract("route_02", "loc_swamp", cadenceDays: 5, baseTariffChits: 10);
            var route = new MapRoute
            {
                From = "loc_shelter",
                To = "loc_swamp",
                DistanceKm = 25,
                WeatherHazard = 0.4f,
                Tags = new System.Collections.Generic.List<string> { "flooded" }
            };

            var result = TradeRouteRiskBindingEngine.EvaluateTransitRisk(
                contract,
                route,
                escortStrengthPermille: 100, // Minimal escort
                regionalHostilityPermille: 850 // High bandit density
            );

            Assert.True(result.TriggersDisruptionAlert);
            Assert.True(result.RaidProbabilityPermille >= 500);
            Assert.True(result.RouteDisruptionRiskPermille >= 500);
            Assert.Contains("Critical route danger", result.RiskSummary);
        }

        [Fact]
        public void EvaluateTransitRisk_EscortsDeterRaids_CalculatesSignificantMitigation()
        {
            var contract = new TradeRouteContract("route_03", "loc_b", cadenceDays: 5, baseTariffChits: 10);
            var route = new MapRoute
            {
                From = "loc_a",
                To = "loc_b",
                DistanceKm = 15,
                WeatherHazard = 0.1f
            };

            var unescorted = TradeRouteRiskBindingEngine.EvaluateTransitRisk(
                contract,
                route,
                escortStrengthPermille: 0,
                regionalHostilityPermille: 500
            );

            var escorted = TradeRouteRiskBindingEngine.EvaluateTransitRisk(
                contract,
                route,
                escortStrengthPermille: 600,
                regionalHostilityPermille: 500
            );

            Assert.True(escorted.RaidProbabilityPermille < unescorted.RaidProbabilityPermille);
            Assert.Equal(450, escorted.EscortMitigationPermille); // 600 * 750 / 1000 = 450
        }

        [Fact]
        public void EvaluateTransitRisk_NullInputs_ReturnsSafeFailureResult()
        {
            var result = TradeRouteRiskBindingEngine.EvaluateTransitRisk(null, null, 0, 0);

            Assert.False(result.IsTransitViable);
            Assert.True(result.TriggersDisruptionAlert);
            Assert.Equal(1000, result.RaidProbabilityPermille);
        }
    }
}
