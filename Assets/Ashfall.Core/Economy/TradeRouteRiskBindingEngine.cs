using System;
using Ashfall.Core.World;

namespace Ashfall.Core.Economy
{
    /// <summary>
    /// Immutable trade route transit risk evaluation produced by <see cref="TradeRouteRiskBindingEngine"/>.
    /// </summary>
    public readonly struct TradeRouteRiskEvaluationResult
    {
        public bool IsTransitViable { get; }
        public int RaidProbabilityPermille { get; }
        public int RouteDisruptionRiskPermille { get; }
        public int ExpectedCargoAttritionPermille { get; }
        public int EscortMitigationPermille { get; }
        public bool TriggersDisruptionAlert { get; }
        public string RiskSummary { get; }

        public TradeRouteRiskEvaluationResult(
            bool isTransitViable,
            int raidProbabilityPermille,
            int routeDisruptionRiskPermille,
            int expectedCargoAttritionPermille,
            int escortMitigationPermille,
            bool triggersDisruptionAlert,
            string riskSummary)
        {
            IsTransitViable = isTransitViable;
            RaidProbabilityPermille = raidProbabilityPermille;
            RouteDisruptionRiskPermille = routeDisruptionRiskPermille;
            ExpectedCargoAttritionPermille = expectedCargoAttritionPermille;
            EscortMitigationPermille = escortMitigationPermille;
            TriggersDisruptionAlert = triggersDisruptionAlert;
            RiskSummary = riskSummary ?? string.Empty;
        }
    }

    /// <summary>
    /// Pure domain engine binding trade route contracts to map route hazards (XP-08-F3 / UNBLOCK-02).
    /// Resolves the trade_route_disrupted scanner orphan without duplicate pathfinding.
    /// </summary>
    public static class TradeRouteRiskBindingEngine
    {
        public const int PermilleScale = 1000;

        /// <summary>
        /// Evaluates caravan transit vulnerability, raid risk, and route disruption probability.
        /// </summary>
        /// <param name="contract">Active trade route contract.</param>
        /// <param name="route">Wasteland map travel route segment.</param>
        /// <param name="escortStrengthPermille">Caravan escort security readiness (0..1000).</param>
        /// <param name="regionalHostilityPermille">Hostility and bandit density in transit sector (0..1000).</param>
        /// <returns>Immutable <see cref="TradeRouteRiskEvaluationResult"/>.</returns>
        public static TradeRouteRiskEvaluationResult EvaluateTransitRisk(
            TradeRouteContract contract,
            MapRoute route,
            int escortStrengthPermille,
            int regionalHostilityPermille)
        {
            if (contract == null || route == null)
            {
                return new TradeRouteRiskEvaluationResult(
                    isTransitViable: false,
                    raidProbabilityPermille: 1000,
                    routeDisruptionRiskPermille: 1000,
                    expectedCargoAttritionPermille: 1000,
                    escortMitigationPermille: 0,
                    triggersDisruptionAlert: true,
                    riskSummary: "Invalid trade contract or nonexistent travel route."
                );
            }

            escortStrengthPermille = Math.Max(0, Math.Min(PermilleScale, escortStrengthPermille));
            regionalHostilityPermille = Math.Max(0, Math.Min(PermilleScale, regionalHostilityPermille));

            // Route environmental hazard factor
            int weatherHazardPermille = (int)(route.WeatherHazard * PermilleScale);
            if (route.IsFlooded) weatherHazardPermille += 300;
            weatherHazardPermille = Math.Min(PermilleScale, weatherHazardPermille);

            // Raw raid vulnerability: 60% regional hostility + 20% weather ambush opportunity + 20% contract distance
            int distanceFactor = Math.Min(PermilleScale, ((int)route.DistanceKm * 20));
            int rawRaidRisk = (regionalHostilityPermille * 600 +
                               weatherHazardPermille * 200 +
                               distanceFactor * 200) / PermilleScale;

            // Escort mitigation: armed escorts reduce raid risk up to 75%
            int escortMitigation = (escortStrengthPermille * 750) / PermilleScale;
            int netRaidRisk = Math.Max(10, (rawRaidRisk * (PermilleScale - escortMitigation)) / PermilleScale);

            // Route disruption risk:
            // High raid risk combined with contract cooldown or low reliability tier (< Tier 2)
            int reliabilityBonus = (contract.ReliabilityScore * 150) / PermilleScale;
            int disruptionRisk = netRaidRisk + (weatherHazardPermille / 2) - reliabilityBonus;
            disruptionRisk = Math.Max(0, Math.Min(PermilleScale, disruptionRisk));

            // Expected cargo attrition: fraction of goods lost to banditry or environmental spoilage
            int attrition = (netRaidRisk * 400 + weatherHazardPermille * 200) / PermilleScale;
            attrition = Math.Min(500, attrition); // capped at 50% loss per transit

            bool triggersDisruption = disruptionRisk >= 500 || netRaidRisk >= 600;
            bool viable = disruptionRisk < 750;

            string summary;
            if (triggersDisruption)
            {
                summary = "Critical route danger. High probability of caravan ambush or route disruption.";
            }
            else if (netRaidRisk >= 300)
            {
                summary = "Elevated transit hazard. Escorts actively deterring peripheral skirmishes.";
            }
            else
            {
                summary = "Secure trade corridor. Caravan transit proceeding with minimal friction.";
            }

            return new TradeRouteRiskEvaluationResult(
                isTransitViable: viable,
                raidProbabilityPermille: netRaidRisk,
                routeDisruptionRiskPermille: disruptionRisk,
                expectedCargoAttritionPermille: attrition,
                escortMitigationPermille: escortMitigation,
                triggersDisruptionAlert: triggersDisruption,
                riskSummary: summary
            );
        }
    }
}
