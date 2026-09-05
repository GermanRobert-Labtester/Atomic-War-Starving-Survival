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
        bool IsRegisteredRoute(string routeOrTargetId);
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

            RegisterRoute(new RouteGateContext
            {
                RouteId = "route_03_the_black_loess_grain_haul",
                OriginHubId = "settlement_03_the_sunken_grain_silo_collective",
                DestinationHubId = "settlement_20_the_valley_sunken_atrium_republic",
                ControllerFactionId = "commune_republic",
                TraversedLocationIds = new[] { "settlement_03", "settlement_20" },
                CreditorFactionIdsReachable = Array.Empty<string>(),
                EncounterRegionTag = "river_basin"
            }, "route_03");

            RegisterRoute(new RouteGateContext
            {
                RouteId = "route_04_the_holy_spring_mercy_trek",
                OriginHubId = "settlement_04_church_of_the_broken_bell",
                DestinationHubId = "settlement_06_the_drowned_sanatorium_baths",
                ControllerFactionId = "sisterhood_of_mercy",
                TraversedLocationIds = new[] { "settlement_04", "settlement_06" },
                CreditorFactionIdsReachable = Array.Empty<string>(),
                EncounterRegionTag = "holy_spring"
            }, "route_04");

            RegisterRoute(new RouteGateContext
            {
                RouteId = "route_06_the_thermal_brine_salt_pass",
                OriginHubId = "settlement_06_the_drowned_sanatorium_baths",
                DestinationHubId = "settlement_02_lock_seven_canal_tollgate",
                ControllerFactionId = "faction_hydro_barons",
                TraversedLocationIds = new[] { "settlement_06", "settlement_02" },
                CreditorFactionIdsReachable = Array.Empty<string>(),
                EncounterRegionTag = "frozen_lake"
            }, "route_06");

            RegisterRoute(new RouteGateContext
            {
                RouteId = "route_09_the_nomad_ridge_drift",
                OriginHubId = "settlement_09_the_volga_steppe_nomad_encampment",
                DestinationHubId = "settlement_08_hydro_dam_nine_overlook",
                ControllerFactionId = "steppe_nomads",
                TraversedLocationIds = new[] { "settlement_09", "settlement_08" },
                CreditorFactionIdsReachable = Array.Empty<string>(),
                EncounterRegionTag = "exposed_ridge"
            }, "route_09");

            RegisterRoute(new RouteGateContext
            {
                RouteId = "route_10_the_blind_substation_courier_run",
                OriginHubId = "settlement_10_the_blind_substation_market",
                DestinationHubId = "settlement_11_the_quarry_crusher_works",
                ControllerFactionId = "neutral_merchants",
                TraversedLocationIds = new[] { "settlement_10", "settlement_11" },
                CreditorFactionIdsReachable = Array.Empty<string>(),
                EncounterRegionTag = "substation_grid"
            }, "route_10");

            RegisterRoute(new RouteGateContext
            {
                RouteId = "route_11_the_quarry_pit_granite_cartage",
                OriginHubId = "settlement_11_the_quarry_crusher_works",
                DestinationHubId = "settlement_05_the_black_cinder_slag_camp",
                ControllerFactionId = "quarry_syndicate",
                TraversedLocationIds = new[] { "settlement_11", "settlement_05" },
                CreditorFactionIdsReachable = Array.Empty<string>(),
                EncounterRegionTag = "quarry_pit"
            }, "route_11");

            RegisterRoute(new RouteGateContext
            {
                RouteId = "route_13_the_subway_tile_vault_underpass",
                OriginHubId = "settlement_13_the_flooded_subway_terminal",
                DestinationHubId = "settlement_14_the_dry_dock_barge_citadel",
                ControllerFactionId = "subway_scavengers",
                TraversedLocationIds = new[] { "settlement_13", "settlement_14" },
                CreditorFactionIdsReachable = Array.Empty<string>(),
                EncounterRegionTag = "metro_underpass"
            }, "route_13");

            RegisterRoute(new RouteGateContext
            {
                RouteId = "route_14_the_flotilla_coastal_shallows_run",
                OriginHubId = "settlement_14_the_dry_dock_barge_citadel",
                DestinationHubId = "settlement_07_the_crashed_antheus_cargo_hull",
                ControllerFactionId = "black_flotilla",
                TraversedLocationIds = new[] { "settlement_14", "settlement_07" },
                CreditorFactionIdsReachable = Array.Empty<string>(),
                EncounterRegionTag = "coastal_shallows"
            }, "route_14");

            RegisterRoute(new RouteGateContext
            {
                RouteId = "route_15_the_tar_cauldron_pitch_express",
                OriginHubId = "settlement_15_the_tar_cauldron_refinery",
                DestinationHubId = "settlement_16_the_radioactive_graveyard_siding",
                ControllerFactionId = "bitumen_refiners",
                TraversedLocationIds = new[] { "settlement_15", "settlement_16" },
                CreditorFactionIdsReachable = Array.Empty<string>(),
                EncounterRegionTag = "tar_cauldron"
            }, "route_15");

            RegisterRoute(new RouteGateContext
            {
                RouteId = "route_16_the_radioactive_graveyard_salvage_crawl",
                OriginHubId = "settlement_16_the_radioactive_graveyard_siding",
                DestinationHubId = "settlement_17_the_telegraph_wire_runner_post",
                ControllerFactionId = "graveyard_salvagers",
                TraversedLocationIds = new[] { "settlement_16", "settlement_17" },
                CreditorFactionIdsReachable = Array.Empty<string>(),
                EncounterRegionTag = "radioactive_graveyard"
            }, "route_16");

            RegisterRoute(new RouteGateContext
            {
                RouteId = "route_17_the_telegraph_wire_runner_post",
                OriginHubId = "settlement_17_the_telegraph_wire_runner_post",
                DestinationHubId = "settlement_18_the_ash_waste_hermitage",
                ControllerFactionId = "wire_runners",
                TraversedLocationIds = new[] { "settlement_17", "settlement_18" },
                CreditorFactionIdsReachable = Array.Empty<string>(),
                EncounterRegionTag = "telegraph_post"
            }, "route_17");

            RegisterRoute(new RouteGateContext
            {
                RouteId = "route_18_the_ash_waste_pilgrim_way",
                OriginHubId = "settlement_18_the_ash_waste_hermitage",
                DestinationHubId = "settlement_04_church_of_the_broken_bell",
                ControllerFactionId = "ash_pilgrims",
                TraversedLocationIds = new[] { "settlement_18", "settlement_04" },
                CreditorFactionIdsReachable = Array.Empty<string>(),
                EncounterRegionTag = "ash_waste"
            }, "route_18");
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

        public bool IsRegisteredRoute(string routeOrTargetId)
        {
            if (string.IsNullOrWhiteSpace(routeOrTargetId))
                return false;

            if (s_canonicalRoutes.ContainsKey(routeOrTargetId))
                return true;

            var ctx = Resolve(routeOrTargetId);
            return !string.IsNullOrEmpty(ctx.ControllerFactionId);
        }
    }
}
