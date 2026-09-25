// SPDX-License-Identifier: MIT
// DEC-358 / PFGL-RT-W1: engine-free encounter arena geometry for realtime combat.
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.IO;

namespace Ashfall.Core.Combat
{
    [Serializable]
    public class CombatArenaLaneSpine
    {
        public int lane;
        public float x;
    }

    [Serializable]
    public class CombatArenaCoverNode
    {
        public string id = string.Empty;
        public float x;
        public float y;
        public float radius = 1f;
        public float cover_rating = 0.3f;
    }

    [Serializable]
    public class CombatArenaClimbSegment
    {
        public string id = string.Empty;
        public float x0;
        public float y0;
        public float x1;
        public float y1;
    }

    [Serializable]
    public class CombatArenaVolume
    {
        public float x;
        public float y;
        public float w = 2f;
        public float h = 8f;

        public bool Contains(float px, float py)
        {
            return px >= x && px <= x + w && py >= y && py <= y + h;
        }
    }

    [Serializable]
    public class CombatArenaSpawn
    {
        public int lane = 1;
        public float x;
        public float y = 1f;
    }

    [Serializable]
    public class CombatArenaDefinition
    {
        public string id = string.Empty;
        public float width = 24f;
        public float height = 10f;
        public float walk_speed = 2.4f;
        public float run_speed = 4.8f;
        public float climb_speed = 1.6f;
        public float flee_speed = 5.2f;
        public List<CombatArenaLaneSpine> lane_spines = new List<CombatArenaLaneSpine>();
        public List<CombatArenaCoverNode> cover_nodes = new List<CombatArenaCoverNode>();
        public List<CombatArenaClimbSegment> climb_segments = new List<CombatArenaClimbSegment>();
        public CombatArenaVolume extract_volume = new CombatArenaVolume();
        public List<CombatArenaSpawn> player_spawns = new List<CombatArenaSpawn>();
        public List<CombatArenaSpawn> enemy_spawns = new List<CombatArenaSpawn>();
    }

    [Serializable]
    public class CombatArenaCatalogData
    {
        public int schema_version = 1;
        public string collection_id = "combat_arenas";
        public List<CombatArenaDefinition> arenas = new List<CombatArenaDefinition>();
    }

    /// <summary>
    /// Authored encounter arenas. Falls back to a procedural 3-lane spine when JSON is absent.
    /// </summary>
    public static class CombatArenaCatalog
    {
        public const string FileName = "combat_arenas.json";
        public const string DefaultArenaId = "arena_lane_spine_default";
        public const int CurrentSchemaVersion = 1;

        private static readonly Dictionary<string, CombatArenaDefinition> ById =
            new Dictionary<string, CombatArenaDefinition>(StringComparer.Ordinal);

        private static bool _seeded;

        public static CombatArenaDefinition CreateDefaultArena()
        {
            return new CombatArenaDefinition
            {
                id = DefaultArenaId,
                width = 24f,
                height = 10f,
                walk_speed = 2.4f,
                run_speed = 4.8f,
                climb_speed = 1.6f,
                flee_speed = 5.2f,
                lane_spines = new List<CombatArenaLaneSpine>
                {
                    new CombatArenaLaneSpine { lane = 0, x = 4f },
                    new CombatArenaLaneSpine { lane = 1, x = 12f },
                    new CombatArenaLaneSpine { lane = 2, x = 20f }
                },
                cover_nodes = new List<CombatArenaCoverNode>
                {
                    new CombatArenaCoverNode { id = "cover_p_left", x = 3f, y = 2f, radius = 1.2f, cover_rating = 0.35f },
                    new CombatArenaCoverNode { id = "cover_e_right", x = 21f, y = 2f, radius = 1.2f, cover_rating = 0.35f }
                },
                climb_segments = new List<CombatArenaClimbSegment>
                {
                    new CombatArenaClimbSegment { id = "climb_barrier_a", x0 = 10f, y0 = 0f, x1 = 10f, y1 = 3f }
                },
                extract_volume = new CombatArenaVolume { x = 0.5f, y = 1f, w = 2f, h = 8f },
                player_spawns = new List<CombatArenaSpawn>
                {
                    new CombatArenaSpawn { lane = 1, x = 6f, y = 1f },
                    new CombatArenaSpawn { lane = 0, x = 5f, y = 2.5f },
                    new CombatArenaSpawn { lane = 2, x = 5f, y = 3.5f }
                },
                enemy_spawns = new List<CombatArenaSpawn>
                {
                    new CombatArenaSpawn { lane = 1, x = 18f, y = 1f },
                    new CombatArenaSpawn { lane = 0, x = 19f, y = 2.5f },
                    new CombatArenaSpawn { lane = 2, x = 19f, y = 3.5f }
                }
            };
        }

        public static void SeedDefaults()
        {
            ById.Clear();
            var d = CreateDefaultArena();
            ById[d.id] = d;
            _seeded = true;
        }

        public static bool Load(string dataDirectory, IFileIO files, IJsonSerializer json)
        {
            if (files == null || json == null || string.IsNullOrEmpty(dataDirectory))
            {
                SeedDefaults();
                return false;
            }

            string path = files.Combine(dataDirectory, FileName);
            if (!files.FileExists(path))
            {
                SeedDefaults();
                return false;
            }

            var data = json.Deserialize<CombatArenaCatalogData>(files.ReadAllText(path));
            ById.Clear();
            if (data?.arenas != null)
            {
                if (data.schema_version > CurrentSchemaVersion)
                    throw new System.IO.InvalidDataException(
                        FileName + " schema " + data.schema_version + " is newer than supported " + CurrentSchemaVersion + ".");

                for (int i = 0; i < data.arenas.Count; i++)
                {
                    var a = data.arenas[i];
                    if (a == null || string.IsNullOrWhiteSpace(a.id)) continue;
                    Normalize(a);
                    ById[a.id] = a;
                }
            }

            if (ById.Count == 0)
            {
                SeedDefaults();
                return false;
            }

            _seeded = true;
            return true;
        }

        public static CombatArenaDefinition GetOrDefault(string? arenaId)
        {
            if (!_seeded) SeedDefaults();
            if (!string.IsNullOrEmpty(arenaId) && ById.TryGetValue(arenaId, out var found) && found != null)
                return found;
            if (ById.TryGetValue(DefaultArenaId, out var def) && def != null)
                return def;
            var created = CreateDefaultArena();
            ById[created.id] = created;
            return created;
        }

        public static IReadOnlyCollection<string> AllIds
        {
            get
            {
                if (!_seeded) SeedDefaults();
                return ById.Keys;
            }
        }

        public static bool NearClimbSegment(CombatArenaDefinition arena, float x, float y, float radius, out CombatArenaClimbSegment? segment)
        {
            segment = null;
            if (arena?.climb_segments == null) return false;
            float best = float.MaxValue;
            for (int i = 0; i < arena.climb_segments.Count; i++)
            {
                var s = arena.climb_segments[i];
                if (s == null) continue;
                float d = DistancePointToSegment(x, y, s.x0, s.y0, s.x1, s.y1);
                if (d <= radius && d < best)
                {
                    best = d;
                    segment = s;
                }
            }
            return segment != null;
        }

        public static int NearestLane(CombatArenaDefinition arena, float x)
        {
            if (arena?.lane_spines == null || arena.lane_spines.Count == 0) return 1;
            int bestLane = arena.lane_spines[0].lane;
            float best = Math.Abs(arena.lane_spines[0].x - x);
            for (int i = 1; i < arena.lane_spines.Count; i++)
            {
                float d = Math.Abs(arena.lane_spines[i].x - x);
                if (d < best)
                {
                    best = d;
                    bestLane = arena.lane_spines[i].lane;
                }
            }
            return MathfCompat.Clamp(bestLane, 0, 2);
        }

        public static bool Contains(CombatArenaVolume? volume, float x, float y)
        {
            if (volume == null || volume.w <= 0f || volume.h <= 0f) return false;
            return x >= volume.x && y >= volume.y
                && x <= volume.x + volume.w && y <= volume.y + volume.h;
        }

        public static void VolumeCenter(CombatArenaVolume? volume, out float cx, out float cy)
        {
            if (volume == null || volume.w <= 0f || volume.h <= 0f)
            {
                cx = 1f;
                cy = 2f;
                return;
            }
            cx = volume.x + volume.w * 0.5f;
            cy = volume.y + volume.h * 0.5f;
        }

        private static void Normalize(CombatArenaDefinition a)
        {
            a.id = a.id?.Trim() ?? string.Empty;
            if (a.width <= 0f) a.width = 24f;
            if (a.height <= 0f) a.height = 10f;
            if (a.walk_speed <= 0f) a.walk_speed = 2.4f;
            if (a.run_speed <= 0f) a.run_speed = 4.8f;
            if (a.climb_speed <= 0f) a.climb_speed = 1.6f;
            if (a.flee_speed <= 0f) a.flee_speed = 5.2f;
            a.lane_spines ??= new List<CombatArenaLaneSpine>();
            a.cover_nodes ??= new List<CombatArenaCoverNode>();
            a.climb_segments ??= new List<CombatArenaClimbSegment>();
            a.player_spawns ??= new List<CombatArenaSpawn>();
            a.enemy_spawns ??= new List<CombatArenaSpawn>();
            a.extract_volume ??= new CombatArenaVolume();
        }

        private static float DistancePointToSegment(float px, float py, float x0, float y0, float x1, float y1)
        {
            float dx = x1 - x0;
            float dy = y1 - y0;
            float lenSq = dx * dx + dy * dy;
            if (lenSq <= 0.0001f)
            {
                float ex = px - x0;
                float ey = py - y0;
                return (float)Math.Sqrt(ex * ex + ey * ey);
            }

            float t = ((px - x0) * dx + (py - y0) * dy) / lenSq;
            if (t < 0f) t = 0f;
            else if (t > 1f) t = 1f;
            float cx = x0 + t * dx;
            float cy = y0 + t * dy;
            float ox = px - cx;
            float oy = py - cy;
            return (float)Math.Sqrt(ox * ox + oy * oy);
        }
    }
}
