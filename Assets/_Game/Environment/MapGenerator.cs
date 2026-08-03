using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Seeded wasteland layout: three danger rings (Suburbs → City Outskirts →
    /// Ground Zero) as a connected web of <see cref="MapNode"/>s. Same seed
    /// always yields the same nodes, loot table ids, rads, and path distances.
    /// </summary>
    public static class MapGenerator
    {
        public const int SuburbsNodeCount = 4;
        public const int OutskirtsNodeCount = 4;
        public const int GroundZeroNodeCount = 3;

        // Loot table ids (snake_case — catalogs may bind later)
        public const string LootSuburbs = "loot_suburbs_looted";
        public const string LootOutskirts = "loot_outskirts_gang";
        public const string LootGroundZero = "loot_ground_zero_military";

        // Rad zone profile ids
        public const string RadSuburbs = "rad_suburbs_low";
        public const string RadOutskirts = "rad_outskirts_med";
        public const string RadGroundZero = "rad_ground_zero_extreme";

        /// <summary>
        /// Generate a full map graph from <paramref name="seed"/>.
        /// Pure function of seed — no global state.
        /// </summary>
        public static GeneratedMap Generate(int seed)
        {
            var rng = new Random(seed);
            var map = new GeneratedMap { Seed = seed };

            // Shelter hub
            var shelter = new MapNode
            {
                NodeId = GeneratedMap.ShelterNodeId,
                DisplayName = "Bunker",
                Ring = DangerRing.Shelter,
                DistanceFromShelter = 0f,
                AngleRadians = 0f,
                LayoutRadius = 0f,
                TrueRad = 0f,
                RumoredRad = 0f,
                RadZoneProfileId = "rad_shelter_safe",
                LootTableId = string.Empty,
                IsRevealed = true,
                IsVisited = true,
                DangerLevel = 0f
            };
            map.Nodes.Add(shelter);

            // --- Ring 0: Suburbs (close, looted, low rads) ---
            var suburbs = CreateRing(
                rng, DangerRing.Suburbs, SuburbsNodeCount,
                layoutRadius: 0.35f,
                namePool: SuburbNames,
                idPrefix: "node_suburb_",
                radMin: 5f, radMax: 18f,
                rumorBias: 0.55f,
                lootId: LootSuburbs,
                radZoneId: RadSuburbs,
                danger: 1.2f,
                encounters: new[] { "enc_feral_dogs", "enc_collapsed_rubble" });
            for (int i = 0; i < suburbs.Count; i++)
                map.Nodes.Add(suburbs[i]);

            // --- Ring 1: City Outskirts (mid, gang controlled, med rads) ---
            var outskirts = CreateRing(
                rng, DangerRing.CityOutskirts, OutskirtsNodeCount,
                layoutRadius: 0.65f,
                namePool: OutskirtsNames,
                idPrefix: "node_outskirts_",
                radMin: 35f, radMax: 90f,
                rumorBias: 0.45f,
                lootId: LootOutskirts,
                radZoneId: RadOutskirts,
                danger: 2.5f,
                encounters: new[] { "enc_deserters", "enc_feral_dogs", "enc_collapsed_rubble" });
            for (int i = 0; i < outskirts.Count; i++)
                map.Nodes.Add(outskirts[i]);

            // --- Ring 2: Ground Zero (far, military loot, extreme rads) ---
            var groundZero = CreateRing(
                rng, DangerRing.GroundZero, GroundZeroNodeCount,
                layoutRadius: 0.95f,
                namePool: GroundZeroNames,
                idPrefix: "node_ground_zero_",
                radMin: 160f, radMax: 420f,
                rumorBias: 0.35f,
                lootId: LootGroundZero,
                radZoneId: RadGroundZero,
                danger: 4.5f,
                encounters: new[] { "enc_deserters", "enc_feral_dogs" });
            for (int i = 0; i < groundZero.Count; i++)
                map.Nodes.Add(groundZero[i]);

            // Paths: shelter → each suburb; ring spokes; lateral neighbors
            ConnectShelterToRing(map, suburbs, baseHoursMin: 1.5f, baseHoursMax: 3.5f, rng);
            ConnectRings(map, suburbs, outskirts, baseHoursMin: 2.5f, baseHoursMax: 5f, rng);
            ConnectRings(map, outskirts, groundZero, baseHoursMin: 3.5f, baseHoursMax: 7f, rng);
            ConnectLateral(map, suburbs, baseHoursMin: 1f, baseHoursMax: 2.5f, rng);
            ConnectLateral(map, outskirts, baseHoursMin: 1.5f, baseHoursMax: 3.5f, rng);
            ConnectLateral(map, groundZero, baseHoursMin: 2f, baseHoursMax: 4f, rng);

            // DistanceFromShelter = shortest-path base hours from bunker
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                var n = map.Nodes[i];
                if (n == null || n.IsShelter) continue;
                n.DistanceFromShelter = ComputeBaseDistance(map, n.NodeId);
            }

            return map;
        }

        // -----------------------------------------------------------------
        // Internals
        // -----------------------------------------------------------------

        private static List<MapNode> CreateRing(
            Random rng,
            DangerRing ring,
            int count,
            float layoutRadius,
            string[] namePool,
            string idPrefix,
            float radMin,
            float radMax,
            float rumorBias,
            string lootId,
            string radZoneId,
            float danger,
            string[] encounters)
        {
            var list = new List<MapNode>(count);
            // Deterministic name pick without replacement when possible
            var nameOrder = ShuffledIndices(rng, namePool.Length);

            float angleJitter = (float)(rng.NextDouble() * Math.PI * 2.0);
            for (int i = 0; i < count; i++)
            {
                float angle = angleJitter + (i * (Mathf.PI * 2f / count));
                // Small deterministic radius jitter
                float rJitter = layoutRadius + (float)((rng.NextDouble() - 0.5) * 0.06);
                float trueRad = Lerp(radMin, radMax, (float)rng.NextDouble());
                // Rumors under/over-shoot truth
                float rumor = trueRad * rumorBias + (float)(rng.NextDouble() * trueRad * (1f - rumorBias));
                rumor = Mathf.Max(0f, rumor);

                int nameIdx = nameOrder[i % nameOrder.Length];
                string name = namePool[nameIdx];
                // Disambiguate if pool smaller than count
                if (i >= namePool.Length)
                    name = name + " " + (i + 1);

                var node = new MapNode
                {
                    NodeId = idPrefix + (i + 1).ToString("00"),
                    DisplayName = name,
                    Ring = ring,
                    AngleRadians = angle,
                    LayoutRadius = rJitter,
                    TrueRad = trueRad,
                    RumoredRad = rumor,
                    RadZoneProfileId = radZoneId,
                    LootTableId = lootId,
                    DangerLevel = danger,
                    IsRevealed = false,
                    IsVisited = false,
                    EncounterDeckIds = new List<string>(encounters)
                };
                list.Add(node);
            }
            return list;
        }

        private static void ConnectShelterToRing(
            GeneratedMap map, List<MapNode> ring, float baseHoursMin, float baseHoursMax, Random rng)
        {
            for (int i = 0; i < ring.Count; i++)
            {
                float hours = Lerp(baseHoursMin, baseHoursMax, (float)rng.NextDouble());
                map.Paths.Add(new MapPath(GeneratedMap.ShelterNodeId, ring[i].NodeId, RoundHours(hours)));
            }
        }

        private static void ConnectRings(
            GeneratedMap map,
            List<MapNode> inner,
            List<MapNode> outer,
            float baseHoursMin,
            float baseHoursMax,
            Random rng)
        {
            // Each outer node connects to nearest-by-angle inner node + one secondary
            for (int o = 0; o < outer.Count; o++)
            {
                int best = 0;
                float bestDiff = float.MaxValue;
                for (int i = 0; i < inner.Count; i++)
                {
                    float d = AngleDiff(outer[o].AngleRadians, inner[i].AngleRadians);
                    if (d < bestDiff) { bestDiff = d; best = i; }
                }
                float hours = Lerp(baseHoursMin, baseHoursMax, (float)rng.NextDouble());
                AddPathUnique(map, inner[best].NodeId, outer[o].NodeId, RoundHours(hours));

                // Secondary spoke for web connectivity
                int second = (best + 1) % inner.Count;
                float hours2 = Lerp(baseHoursMin, baseHoursMax, (float)rng.NextDouble());
                AddPathUnique(map, inner[second].NodeId, outer[o].NodeId, RoundHours(hours2));
            }
        }

        private static void ConnectLateral(
            GeneratedMap map, List<MapNode> ring, float baseHoursMin, float baseHoursMax, Random rng)
        {
            if (ring == null || ring.Count < 2) return;
            for (int i = 0; i < ring.Count; i++)
            {
                int j = (i + 1) % ring.Count;
                float hours = Lerp(baseHoursMin, baseHoursMax, (float)rng.NextDouble());
                AddPathUnique(map, ring[i].NodeId, ring[j].NodeId, RoundHours(hours));
            }
        }

        private static void AddPathUnique(GeneratedMap map, string a, string b, float hours)
        {
            if (map.GetPath(a, b) != null) return;
            map.Paths.Add(new MapPath(a, b, hours));
        }

        private static float ComputeBaseDistance(GeneratedMap map, string nodeId)
        {
            var route = map.FindPath(GeneratedMap.ShelterNodeId, nodeId);
            if (route == null || route.Count < 2) return 0f;
            float hours = 0f;
            for (int i = 0; i < route.Count - 1; i++)
            {
                var p = map.GetPath(route[i], route[i + 1]);
                if (p != null) hours += p.BaseTravelHours;
            }
            return hours;
        }

        private static int[] ShuffledIndices(Random rng, int n)
        {
            var idx = new int[n];
            for (int i = 0; i < n; i++) idx[i] = i;
            for (int i = n - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                int t = idx[i];
                idx[i] = idx[j];
                idx[j] = t;
            }
            return idx;
        }

        private static float Lerp(float a, float b, float t) => a + (b - a) * Mathf.Clamp01(t);

        private static float AngleDiff(float a, float b)
        {
            float d = Mathf.Abs(a - b) % (Mathf.PI * 2f);
            if (d > Mathf.PI) d = Mathf.PI * 2f - d;
            return d;
        }

        private static float RoundHours(float h) => Mathf.Round(h * 10f) / 10f;

        // Name pools (no real places / people)
        private static readonly string[] SuburbNames =
        {
            "Ash Lot Blocks",
            "Rail Siding",
            "Strip Mall Shell",
            "School Yard Fence",
            "Water Tower Slope",
            "Dead Orchard"
        };

        private static readonly string[] OutskirtsNames =
        {
            "Toll Bridge Camp",
            "Warehouse Row",
            "Overpass Nest",
            "Canal Works",
            "Bus Depot Yard",
            "Concrete Spillway"
        };

        private static readonly string[] GroundZeroNames =
        {
            "Blast Crown",
            "Hardened Hangar",
            "Command Bunker Rim",
            "Crater Stack",
            "Missile Silo Gate"
        };
    }
}
