using System;
using System.Collections.Generic;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Authoritative contextual information for a route corridor (Section 12).
    /// </summary>
    public sealed class RouteGateContext
    {
        public string RouteId { get; init; } = string.Empty;
        public string OriginHubId { get; init; } = string.Empty;
        public string DestinationHubId { get; init; } = string.Empty;
        public string ControllerFactionId { get; init; } = string.Empty;
        public IReadOnlyList<string> TraversedLocationIds { get; init; } = Array.Empty<string>();
        public IReadOnlyList<string> CreditorFactionIdsReachable { get; init; } = Array.Empty<string>();
        public string EncounterRegionTag { get; init; } = string.Empty;
    }

    /// <summary>
    /// Interface for authoritative route-context resolution.
    /// </summary>
    public interface IRouteContextResolver
    {
        RouteGateContext Resolve(string routeOrTargetId);
    }

    /// <summary>
    /// Default canonical route context resolver. Maps route IDs and targets
    /// to their hubs, controller factions, and reachable creditors.
    /// </summary>
    public sealed class RouteGateContextResolver : IRouteContextResolver
    {
        private static readonly Dictionary<string, RouteGateContext> s_canonicalRoutes =
            new Dictionary<string, RouteGateContext>(StringComparer.OrdinalIgnoreCase);

        static RouteGateContextResolver()
        {
            RegisterRoute(new RouteGateContext
            {
                RouteId = "route_12_the_cloud_eyrie_meteorological_ascent",
                OriginHubId = "settlement_12_the_glass_desert_observatory",
                DestinationHubId = "settlement_04_church_of_the_broken_bell",
                ControllerFactionId = "warlords_sector_4",
                TraversedLocationIds = new[] { "settlement_12", "pass_mount_karkov", "settlement_04" },
                CreditorFactionIdsReachable = Array.Empty<string>(),
                EncounterRegionTag = "warlord_territory"
            }, "route_12");

            RegisterRoute(new RouteGateContext
            {
                RouteId = "route_08_the_high_voltage_grid_battery_relay",
                OriginHubId = "settlement_08_hydro_dam_nine_overlook",
                DestinationHubId = "settlement_20_the_valley_sunken_atrium_republic",
                ControllerFactionId = "faction_central_garrison",
                TraversedLocationIds = new[] { "settlement_08", "pylon_88_relay", "settlement_20" },
                CreditorFactionIdsReachable = new[] { "faction_railway_guild", "faction_hydro_barons" },
                EncounterRegionTag = "hydro_grid"
            }, "route_08");

            RegisterRoute(new RouteGateContext
            {
                RouteId = "route_01_the_iron_line_garrison_express",
                OriginHubId = "settlement_01_fort_karkov_rail_garrison",
                DestinationHubId = "settlement_10_the_blind_substation_market",
                ControllerFactionId = "faction_central_garrison",
                TraversedLocationIds = new[] { "settlement_01", "milepost_14", "settlement_10" },
                CreditorFactionIdsReachable = new[] { "faction_railway_guild", "faction_supply_corps" },
                EncounterRegionTag = "railway_corridor"
            }, "route_01");

            RegisterRoute(new RouteGateContext
            {
                RouteId = "route_02_the_canal_barge_waterway_run",
                OriginHubId = "settlement_02_lock_seven_canal_tollgate",
                DestinationHubId = "settlement_13_the_flooded_subway_terminal",
                ControllerFactionId = "faction_hydro_barons",
                TraversedLocationIds = new[] { "settlement_02", "reedbed_lock", "settlement_13" },
                CreditorFactionIdsReachable = new[] { "faction_hydro_barons" },
                EncounterRegionTag = "canal_waterway"
            }, "route_02");

            RegisterRoute(new RouteGateContext
            {
                RouteId = "route_05_the_spoil_heap_coke_and_steel_route",
                OriginHubId = "settlement_05_the_black_cinder_slag_camp",
                DestinationHubId = "settlement_01_fort_karkov_rail_garrison",
                ControllerFactionId = "faction_central_garrison",
                TraversedLocationIds = new[] { "settlement_05", "slag_plateau", "settlement_01" },
                CreditorFactionIdsReachable = new[] { "faction_supply_corps" },
                EncounterRegionTag = "highland_plateau"
            }, "route_05");

            RegisterRoute(new RouteGateContext
            {
                RouteId = "route_07_the_aluminium_whale_salvage_run",
                OriginHubId = "settlement_07_the_crashed_antheus_cargo_hull",
                DestinationHubId = "settlement_05_the_black_cinder_slag_camp",
                ControllerFactionId = "warlords_sector_4",
                TraversedLocationIds = new[] { "settlement_07", "marsh_sinkholes", "settlement_05" },
                CreditorFactionIdsReachable = Array.Empty<string>(),
                EncounterRegionTag = "lowland_marsh"
            }, "route_07");
        }

        private static void RegisterRoute(RouteGateContext context, string alias)
        {
            s_canonicalRoutes[context.RouteId] = context;
            if (!string.IsNullOrEmpty(alias))
                s_canonicalRoutes[alias] = context;
        }

        public RouteGateContext Resolve(string routeOrTargetId)
        {
            if (string.IsNullOrWhiteSpace(routeOrTargetId))
            {
                return new RouteGateContext
                {
                    RouteId = string.Empty,
                    ControllerFactionId = string.Empty
                };
            }

            if (s_canonicalRoutes.TryGetValue(routeOrTargetId, out var direct))
                return direct;

            // Prefix/substring search for friendly IDs like "route_12"
            foreach (var kvp in s_canonicalRoutes)
            {
                if (kvp.Key.StartsWith(routeOrTargetId, StringComparison.OrdinalIgnoreCase) ||
                    routeOrTargetId.StartsWith(kvp.Key, StringComparison.OrdinalIgnoreCase))
                {
                    return kvp.Value;
                }
            }

            // Fallback: minimal context
            return new RouteGateContext
            {
                RouteId = routeOrTargetId,
                ControllerFactionId = string.Empty
            };
        }
    }
}
