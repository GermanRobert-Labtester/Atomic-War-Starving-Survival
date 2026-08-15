using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using Ashfall.Core;
// HoldfastLocationEntry exists in BOTH Ashfall.Core and AtomicWar._Game.Data.
// Pinned to the Data type to preserve this seeder's existing behaviour; the two
// should be unified next (see ASHFALL_DEEP_CODE_AUDIT_2026-08-14.md).
using HoldfastLocationEntry = Ashfall.Core.HoldfastLocationEntry;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Appends District 8 nodes onto the proc-gen map after MapGenerator.Generate.
    /// A spine south→north plus two Cut spurs. Tags: region_holdfast.
    /// </summary>
    public static class HoldfastMapSeeder
    {
        public const string RegionTag = "region_holdfast";
        public const string CutTag = "the_cut";
        public const string SaltTag = "the_saltworks";
        public const string ClusterTag = "the_cluster";
        public const string ShelfTag = "the_shelf";

        public static int Attach(GeneratedMap map, IReadOnlyList<HoldfastLocationEntry> locations)
        {
            if (map == null || map.Nodes == null) return 0;
            if (locations == null || locations.Count == 0) return 0;

            int added = 0;
            float angle = 1.15f;
            for (int i = 0; i < locations.Count; i++)
            {
                var loc = locations[i];
                if (loc == null || string.IsNullOrEmpty(loc.id)) continue;
                if (loc.overlay_on_unlock) continue;
                if (map.GetNode(loc.id) != null) continue;

                var node = new MapNode
                {
                    NodeId = loc.id,
                    DisplayName = loc.displayName,
                    Ring = RingFor(loc.region),
                    DistanceFromShelter = loc.travelHours,
                    AngleRadians = angle,
                    LayoutRadius = 0.72f + (i * 0.012f),
                    TrueRad = loc.baseRadsPerHour,
                    RumoredRad = loc.baseRadsPerHour * 0.55f,
                    DangerLevel = loc.dangerLevel,
                    LootTableId = "loot_outskirts_gang",
                    RadZoneProfileId = "rad_outskirts_med",
                    Tags = new List<string> { RegionTag, loc.region ?? CutTag }
                };
                map.Nodes.Add(node);
                added++;
            }

            SeedCutSpine(map);
            SeedSaltAndCluster(map);
            SeedShelf(map);
            map.NotifyMapChanged();
            return added;
        }

        private static DangerRing RingFor(string region)
        {
            if (region == SaltTag || region == ClusterTag) return DangerRing.CityOutskirts;
            if (region == ShelfTag) return DangerRing.GroundZero;
            return DangerRing.CityOutskirts;
        }

        private static void SeedCutSpine(GeneratedMap map)
        {
            Link(map, GeneratedMap.ShelterNodeId, IceRoadSystem.LocIceRoadGate, 6.0f);
            Link(map, IceRoadSystem.LocIceRoadGate, IceRoadSystem.LocKilometre19, 0.8f);
            Link(map, IceRoadSystem.LocKilometre19, IceRoadSystem.LocWeighHut, 0.6f);
            Link(map, IceRoadSystem.LocWeighHut, IceRoadSystem.LocWaystationA, 1.0f);
            Link(map, IceRoadSystem.LocWaystationA, IceRoadSystem.LocSouthBeacon, 0.4f);
            Link(map, IceRoadSystem.LocWeighHut, IceRoadSystem.LocDredgerHulk, 0.7f);
            Link(map, IceRoadSystem.LocWaystationA, IceRoadSystem.LocBrinePool, 0.6f);
            Link(map, IceRoadSystem.LocSouthBeacon, IceRoadSystem.LocAccident12, 0.5f);
        }

        private static void SeedSaltAndCluster(GeneratedMap map)
        {
            Link(map, IceRoadSystem.LocWaystationA, "location_abandoned_desalination", 1.2f);
            Link(map, "location_abandoned_desalination", "loc_salt_membrane_hall", 0.4f);
            Link(map, "location_abandoned_desalination", "loc_salt_grade_hut", 0.3f);
            Link(map, "loc_salt_grade_hut", "loc_salt_iodine_store", 0.3f);
            Link(map, "location_abandoned_desalination", "loc_salt_outfall", 0.5f);
            Link(map, "loc_salt_outfall", IceRoadSystem.LocBrinePool, 0.4f);
            Link(map, "location_abandoned_desalination", "loc_salt_cooling_canal", 0.4f);
            Link(map, "loc_salt_cooling_canal", "loc_cluster_steam_substation", 0.6f);
            Link(map, "loc_cluster_steam_substation", "loc_cluster_gatehouse", 0.4f);
            Link(map, "loc_cluster_gatehouse", "loc_cluster_quad", 0.2f);
            Link(map, "loc_cluster_quad", "loc_cluster_block_c", 0.2f);
            Link(map, "loc_cluster_quad", "loc_cluster_clinic", 0.2f);
            Link(map, "loc_cluster_quad", "loc_cluster_school", 0.2f);
            Link(map, "loc_cluster_quad", "loc_cluster_office", 0.2f);
            Link(map, "location_abandoned_desalination", "loc_salt_intake_caisson", 0.5f);
            Link(map, "location_abandoned_desalination", "loc_salt_scrap_membranes", 0.3f);
        }

        private static void SeedShelf(GeneratedMap map)
        {
            Link(map, IceRoadSystem.LocSouthBeacon, "location_frozen_river_barge", 2.0f);
            Link(map, "location_frozen_river_barge", "loc_shelf_pressure_ridge", 1.2f);
            Link(map, "loc_shelf_pressure_ridge", "loc_shelf_hearth4", 1.0f);
            Link(map, "location_frozen_river_barge", "location_crashed_icebreaker_convoy", 1.4f);
            Link(map, "loc_shelf_hearth4", "loc_shelf_roadstead_crane", 0.6f);
            Link(map, "loc_shelf_hearth4", "loc_shelf_foghorn", 0.8f);
        }

        private static void Link(GeneratedMap map, string a, string b, float hours)
        {
            if (map.GetNode(a) == null || map.GetNode(b) == null) return;
            if (map.GetPath(a, b) != null) return;
            if (map.Paths == null) map.Paths = new List<MapPath>();
            map.Paths.Add(new MapPath(a, b, Mathf.Max(0.1f, hours)));
        }
    }
}
