using System;
using System.Collections.Generic;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — appends the Crossing onto the proc-gen map
    /// after MapGenerator.Generate. Spine: bunker → approach waypoints (§2.4,
    /// existing Sector 4 nodes) → the Viaduct Gate → the watch → the
    /// Scalehouse hub. Spokes: Weighbridge / Stallrow / Underwrite / Records.
    /// Tags: region_crossing. The gate itself is enforced by ExpeditionSystem
    /// via SetVouchAccessSystem — the seeder only lays the geography.
    /// Mirrors HoldfastMapSeeder (engine-agnostic: no UnityEngine).
    /// </summary>
    public static class CrossingMapSeeder
    {
        public const string RegionTag = "region_crossing";

        // Existing Sector 4 nodes that gain meaning as approach waypoints
        // (bible §2.4: "the road to the Crossing runs past here").
        private const string WaypointConvoyYard = "location_abandoned_convoy_yard";
        private const string WaypointDieselTankFarm = "loc_diesel_tank_farm";
        private const string WaypointRecoveryYard = "loc_recovery_yard";

        public static int Attach(GeneratedMap map, IReadOnlyList<CrossingLocationEntry> locations)
        {
            if (map == null || map.Nodes == null) return 0;
            if (locations == null || locations.Count == 0) return 0;

            int added = 0;
            float angle = 2.35f;
            for (int i = 0; i < locations.Count; i++)
            {
                var loc = locations[i];
                if (loc == null || string.IsNullOrEmpty(loc.id)) continue;
                if (loc.overlay_on_unlock || loc.recast_always) continue;
                if (map.GetNode(loc.id) != null) continue;

                var node = new MapNode
                {
                    NodeId = loc.id,
                    DisplayName = loc.displayName,
                    Ring = DangerRing.CityOutskirts,
                    DistanceFromShelter = loc.travelHours,
                    AngleRadians = angle,
                    LayoutRadius = 0.66f + (i * 0.012f),
                    TrueRad = loc.baseRadsPerHour,
                    RumoredRad = loc.baseRadsPerHour * 0.55f,
                    DangerLevel = loc.dangerLevel,
                    LootTableId = "loot_outskirts_gang",
                    RadZoneProfileId = "rad_outskirts_med",
                    Tags = new List<string> { RegionTag, loc.region ?? RegionTag }
                };
                map.Nodes.Add(node);
                added++;
            }

            SeedSpine(map);
            SeedSpokes(map);
            map.NotifyMapChanged();
            return added;
        }

        /// <summary>Bunker → approach waypoints → the Viaduct Gate → the watch → the Scalehouse hub.</summary>
        private static void SeedSpine(GeneratedMap map)
        {
            Link(map, GeneratedMap.ShelterNodeId, WaypointConvoyYard, 6.0f);
            Link(map, WaypointConvoyYard, WaypointDieselTankFarm, 1.2f);
            Link(map, WaypointDieselTankFarm, WaypointRecoveryYard, 0.8f);
            Link(map, WaypointRecoveryYard, CrossingIds.Locations.ViaductGate, 1.0f);
            Link(map, CrossingIds.Locations.ViaductGate, CrossingIds.Locations.Watchtower, 0.5f);
            Link(map, CrossingIds.Locations.Watchtower, CrossingIds.Locations.Scalehouse, 0.5f);
        }

        /// <summary>Scalehouse Row spokes.</summary>
        private static void SeedSpokes(GeneratedMap map)
        {
            Link(map, CrossingIds.Locations.ViaductGate, CrossingIds.Locations.Scalehouse, 1.0f);
            Link(map, CrossingIds.Locations.Scalehouse, CrossingIds.Locations.Weighbridge, 0.5f);
            Link(map, CrossingIds.Locations.Scalehouse, CrossingIds.Locations.Stallrow, 0.5f);
            Link(map, CrossingIds.Locations.Scalehouse, CrossingIds.Locations.Underwrite, 0.5f);
            Link(map, CrossingIds.Locations.Stallrow, CrossingIds.Locations.RecordsRoom, 0.3f);
        }

        private static void Link(GeneratedMap map, string a, string b, float hours)
        {
            if (map.GetNode(a) == null || map.GetNode(b) == null) return;
            if (map.GetPath(a, b) != null) return;
            if (map.Paths == null) map.Paths = new List<MapPath>();
            map.Paths.Add(new MapPath(a, b, Math.Max(0.1f, hours)));
        }
    }
}
