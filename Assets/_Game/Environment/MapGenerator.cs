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
                encounters: new[] { "enc_deserters", "enc_feral_dogs", "enc_collapsed_rubble", "enc_sleeping_ghoul" });
            for (int i = 0; i < outskirts.Count; i++)
            {
                // Prompt #208 — City Outskirts count as City map nodes (Urban Pathfinder).
                if (outskirts[i].Tags == null) outskirts[i].Tags = new List<string>();
                if (!outskirts[i].HasTag("city")) outskirts[i].Tags.Add("city");
                if (!outskirts[i].HasTag("urban")) outskirts[i].Tags.Add("urban");
                if (!outskirts[i].HasTag("ruin")) outskirts[i].Tags.Add("ruin");
                map.Nodes.Add(outskirts[i]);
            }

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

            // Prompt #12 — UXO flags via derived RNG so layout path/rad rolls stay stable.
            AssignUxoFlags(map, seed);

            // Prompt #14 — death-zone tags from high TrueRad (deterministic from seed layout).
            AssignDeathZones(map);

            // Prompt #15 — one narrative Deserter's Stand site (derived RNG, layout-stable).
            AssignDeserterStandFlags(map, seed);

            // REPROMOTE-Encounter-001 — tag highway / toll corridor nodes "roadblock"
            // so ExpeditionSystem class roadblock dispatch fires without SO id match.
            AssignRoadblockFlags(map, seed);

            return map;
        }

        /// <summary>
        /// Tag highway-style corridor nodes with <c>roadblock</c> (and <c>highway</c>)
        /// so expedition class-roadblock dispatch can resolve from map tags alone.
        /// Pure function of seed + layout; does not change path graph.
        /// </summary>
        public static void AssignRoadblockFlags(GeneratedMap map, int seed)
        {
            if (map?.Nodes == null) return;

            // 1) Name-based: tolls, overpasses, depots, bridges, roads, highways.
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                var n = map.Nodes[i];
                if (n == null || n.IsShelter) continue;
                if (!IsHighwayStyleName(n.DisplayName) && !IsHighwayStyleName(n.NodeId))
                    continue;
                EnsureTag(n, "highway");
                EnsureTag(n, "roadblock");
            }

            // 2) Guarantee at least one roadblock on an outskirts lateral corridor
            //    (main travel arteries between rings) via derived RNG.
            bool any = false;
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                if (map.Nodes[i] != null && map.Nodes[i].HasTag("roadblock"))
                {
                    any = true;
                    break;
                }
            }
            if (any) return;

            var candidates = new List<MapNode>();
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                var n = map.Nodes[i];
                if (n == null || n.IsShelter) continue;
                if (n.Ring == DangerRing.CityOutskirts || n.Ring == DangerRing.Suburbs)
                    candidates.Add(n);
            }
            if (candidates.Count == 0) return;
            candidates.Sort((a, b) => string.CompareOrdinal(a.NodeId, b.NodeId));
            var rng = new Random(unchecked(seed * 1664525 + 1013904223));
            int pick = rng.Next(candidates.Count);
            EnsureTag(candidates[pick], "highway");
            EnsureTag(candidates[pick], "roadblock");
        }

        private static bool IsHighwayStyleName(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            // Case-insensitive keyword scan for corridor / roadblock fiction.
            string n = name;
            return ContainsIgnoreCase(n, "toll")
                || ContainsIgnoreCase(n, "overpass")
                || ContainsIgnoreCase(n, "highway")
                || ContainsIgnoreCase(n, "road")
                || ContainsIgnoreCase(n, "bridge")
                || ContainsIgnoreCase(n, "depot")
                || ContainsIgnoreCase(n, "rail");
        }

        private static bool ContainsIgnoreCase(string hay, string needle)
        {
            return hay.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void EnsureTag(MapNode n, string tag)
        {
            if (n == null || string.IsNullOrEmpty(tag)) return;
            if (n.Tags == null) n.Tags = new List<string>();
            if (!n.HasTag(tag)) n.Tags.Add(tag);
        }

        /// <summary>
        /// Place exactly one Deserter's Stand narrative site on an outskirts or
        /// ground-zero node (Prompt #15). Derived RNG keeps layout/UXO rolls stable.
        /// </summary>
        public static void AssignDeserterStandFlags(GeneratedMap map, int seed)
        {
            if (map?.Nodes == null) return;
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                if (map.Nodes[i] != null)
                    map.Nodes[i].HasDeserterStand = false;
            }

            var candidates = new List<MapNode>();
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                var n = map.Nodes[i];
                if (n == null || n.IsShelter) continue;
                if (n.Ring == DangerRing.CityOutskirts || n.Ring == DangerRing.GroundZero)
                    candidates.Add(n);
            }
            if (candidates.Count == 0) return;

            candidates.Sort((a, b) => string.CompareOrdinal(a.NodeId, b.NodeId));
            var rng = new Random(unchecked(seed * 1664525 + 1013904223));
            int pick = rng.Next(candidates.Count);
            candidates[pick].HasDeserterStand = true;
        }

        /// <summary>
        /// Tag non-shelter nodes with lethal ambient rad as death zones (Prompt #14).
        /// Pure function of TrueRad — same seed always yields the same tags.
        /// </summary>
        public static void AssignDeathZones(GeneratedMap map)
        {
            if (map?.Nodes == null) return;
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                var n = map.Nodes[i];
                if (n == null) continue;
                if (n.IsShelter)
                {
                    n.IsDeathZone = false;
                    continue;
                }
                n.IsDeathZone = n.TrueRad >= DeathZoneRadThreshold;
            }
        }

        /// <summary>TrueRad at/above this marks a node as a death zone at gen (Prompt #14).</summary>
        public const float DeathZoneRadThreshold = 200f;

        /// <summary>
        /// Seed ~20% of non-shelter nodes with hidden civil-war UXO (Prompt #12).
        /// Uses a hash-derived stream so existing layout draws are unchanged.
        /// </summary>
        public static void AssignUxoFlags(GeneratedMap map, int seed)
        {
            if (map?.Nodes == null) return;
            // Separate stream — does not advance the layout RNG used above.
            var rng = new Random(unchecked(seed * 1103515245 + 12345));
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                var n = map.Nodes[i];
                if (n == null || n.IsShelter)
                {
                    if (n != null) n.HasUxo = false;
                    continue;
                }
                n.HasUxo = rng.NextDouble() < UxoMapChance;
            }
        }

        /// <summary>Exposed for tests; matches <see cref="AtomicWar._Game.Core.UxoHazardSystem.MapUxoChance"/>.</summary>
        public const float UxoMapChance = 0.20f;

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
