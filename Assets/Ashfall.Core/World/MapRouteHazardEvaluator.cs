// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.World
{
    public readonly struct RouteTraversalFeasibility
    {
        public bool IsTraversable { get; }
        public int DelayDays { get; }
        public int RiskScorePermille { get; }
        public string RefusalReason { get; }

        public RouteTraversalFeasibility(bool isTraversable, int delayDays, int riskScorePermille, string refusalReason)
        {
            IsTraversable = isTraversable;
            DelayDays = Math.Max(0, delayDays);
            RiskScorePermille = Math.Max(0, Math.Min(1000, riskScorePermille));
            RefusalReason = refusalReason ?? string.Empty;
        }
    }

    /// <summary>
    /// D16: Wasteland Map Route Dynamic Edge Hazard & Flooding Evaluator.
    /// Pure domain evaluator for vehicle and expedition traversal feasibility across
    /// flooded, amphibious, and mud-hazard route segments without unseeded RNG or graph mutation.
    /// </summary>
    public static class MapRouteHazardEvaluator
    {
        public static RouteTraversalFeasibility EvaluateTraversal(
            MapRoute route,
            bool hasAmphibiousCapability,
            bool isHeavyRainOrFloodSeason = false,
            int vehicleGroundClearanceMm = 200)
        {
            if (route == null)
            {
                return new RouteTraversalFeasibility(false, 0, 1000, "Route does not exist.");
            }

            int delay = 0;
            int risk = 50; // baseline 50 permille

            // Amphibious waterways require amphibious vehicles strictly
            if (route.IsAmphibious && !hasAmphibiousCapability)
            {
                return new RouteTraversalFeasibility(
                    false,
                    0,
                    1000,
                    "Route traverses deep water; amphibious capability required.");
            }

            // Flooded road segments
            if (route.IsFlooded)
            {
                if (!hasAmphibiousCapability)
                {
                    if (vehicleGroundClearanceMm < 300)
                    {
                        return new RouteTraversalFeasibility(
                            false,
                            0,
                            800,
                            "Route flooded; insufficient vehicle ground clearance (<300mm).");
                    }

                    // High clearance vehicle can ford with delay and risk
                    delay += 2;
                    risk += 350;
                }
                else
                {
                    // Amphibious vehicle handles flood with small delay
                    delay += 1;
                    risk += 100;
                }
            }

            // Seasonal mud hazards
            if (isHeavyRainOrFloodSeason && (route.HasTag("mud_hazard") || route.HasTag("lowland")))
            {
                delay += 1;
                risk += 200;
            }

            // Radiation hotspot segments
            if (route.HasTag("rad_hotspot"))
            {
                risk += 250;
            }

            return new RouteTraversalFeasibility(true, delay, risk, string.Empty);
        }
    }
}
