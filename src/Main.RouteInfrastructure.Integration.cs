// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Medical;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using AtomicWar.GodotApp.UI;
using Godot;
using System;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private RouteInfrastructureSystem? _routeInfrastructure;

        // ─── Setup ───

        private void SetupRouteInfrastructure()
        {
            if (_routeInfrastructure != null) return;
            var state = RouteInfrastructureSaveStore.TryLoad() ?? new RouteInfrastructureState();
            _routeInfrastructure = new RouteInfrastructureSystem(state);
            // Fresh saves have no authored corridor state — seed the flagship
            // minefield and corrugated rail so flail/grinder commands and
            // expedition modifiers have a real segment to act on.
            BootstrapDefaultRouteInfrastructure(_routeInfrastructure, _simDay > 0 ? _simDay : 1);
            WirePlans146ExpeditionRouteModifiers();
        }

        /// <summary>
        /// Registers the canonical demining and rail corridors when the route
        /// section is empty. Never overwrites a restored save that already has
        /// segments.
        /// </summary>
        private static void BootstrapDefaultRouteInfrastructure(RouteInfrastructureSystem routes, int day)
        {
            if (routes == null || routes.GetAllSegments().Count > 0) return;
            routes.RegisterMinefield("expedition_corridor_north", "seg_mine_gap", density01: 0.85f, day: day);
            routes.RegisterMinefield("route_ashfall_pass", "seg_pass_alpha", density01: 0.65f, day: day);
            routes.RegisterRailSegment("rail_trunk_iron_vein", "sector_deep_quarry", initialRoughness: 0.90f, safeSpeedKph: 25.0f, day: day, designSpeedKph: 55f);
            routes.RegisterRailSegment("rail_trunk_iron_reach", "sector_main_line", initialRoughness: 0.82f, safeSpeedKph: 30.0f, day: day, designSpeedKph: 60f);
        }

        /// <summary>
        /// D16 — worst traversal verdict for the wasteland-map route that reaches
        /// <paramref name="locationId"/> from the home holdfast, or
        /// <c>null</c> when no map route exists (legacy multipliers then apply).
        /// </summary>
        private RouteTraversalFeasibility? GetRouteTraversalFeasibility(string locationId)
        {
            var map = _world?.WastelandMap;
            if (map == null || string.IsNullOrEmpty(locationId)) return null;

            RouteTraversalFeasibility? worst = null;
            foreach (var route in map.GetRoutesFrom(MapRouteHomeHoldfast))
            {
                if (route == null) continue;
                if (!string.Equals(route.To, locationId, StringComparison.OrdinalIgnoreCase)) continue;

                var verdict = MapRouteHazardEvaluator.EvaluateTraversal(
                    route,
                    hasAmphibiousCapability: HasAmphibiousTraversalCapability(),
                    isHeavyRainOrFloodSeason: IsFloodSeason(),
                    vehicleGroundClearanceMm: VehicleGroundClearanceMm);

                if (worst == null || verdict.RiskScorePermille > worst.Value.RiskScorePermille) worst = verdict;
            }
            return worst;
        }

        /// <summary>Encounter-risk multiplier contributed by route traversal hazards.</summary>
        private float GetRouteTraversalHazardMultiplier(string locationId)
        {
            var verdict = GetRouteTraversalFeasibility(locationId);
            if (verdict == null) return 1f;
            return 1f + Math.Clamp(verdict.Value.RiskScorePermille / 1000f, 0f, 1f);
        }

        /// <summary>Travel-time multiplier contributed by route traversal delay days.</summary>
        private float GetRouteTraversalDelayMultiplier(string locationId)
        {
            var verdict = GetRouteTraversalFeasibility(locationId);
            if (verdict == null) return 1f;
            return 1f + Math.Clamp(verdict.Value.DelayDays / BaseRouteTravelDays, 0f, 1f);
        }

        /// <summary>Baseline overland travel days used to scale traversal delay.</summary>
        private const float BaseRouteTravelDays = 3f;

        /// <summary>Map node id of the home holdfast (matches the wasteland map authority).</summary>
        private const string MapRouteHomeHoldfast = "loc_holdfast";

        /// <summary>Ground clearance of the rig used for overland traversals.</summary>
        private int VehicleGroundClearanceMm => 200;

        // ─── Save (triad) ───

        private void SaveRouteInfrastructure()
        {
            if (_routeInfrastructure != null)
                CaptureSection("route_infrastructure", RouteInfrastructureSaveStore.TryCapturePersisted(_routeInfrastructure.CaptureState()));
        }

        /// <summary>Flood/weather season read from the canonical weather owner.</summary>
        private bool IsFloodSeason()
        {
            var current = _world?.Weather?.Current ?? Ashfall.Core.WeatherKind.Clear;
            return current == Ashfall.Core.WeatherKind.FalloutStorm
                || current == Ashfall.Core.WeatherKind.BlackRain
                || current == Ashfall.Core.WeatherKind.Blizzard;
        }

        /// <summary>
        /// True when the amphibious draisine authority (the amphibious-traversal
        /// owner) has a crossing rig registered and reports it capable for a
        /// water crossing. No registered rig means no amphibious capability.
        /// </summary>
        private bool HasAmphibiousTraversalCapability()
        {
            if (_amphibiousDraisine?.System == null) return false;

            System.Collections.Generic.Dictionary<string, AmphibiousDraisineState>? save = null;
            try { save = _amphibiousDraisine.CaptureSave(); } catch { return false; }
            if (save == null || save.Count == 0) return false;

            foreach (var vehicleId in save.Keys)
            {
                if (string.IsNullOrEmpty(vehicleId)) continue;
                if (_amphibiousDraisine.IsRouteCapable(vehicleId, "water_crossing", out _)) return true;
            }
            return false;
        }

    }
}
