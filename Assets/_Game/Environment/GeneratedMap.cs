using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Seeded wasteland graph: shelter hub + three danger rings, connected paths,
    /// weather-scaled travel, and fog-of-war reveal state.
    /// </summary>
    [Serializable]
    public class GeneratedMap
    {
        public const string ShelterNodeId = "shelter";

        public int Seed;
        public List<MapNode> Nodes = new List<MapNode>();
        public List<MapPath> Paths = new List<MapPath>();

        public event Action OnMapChanged;

        public MapNode GetNode(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId) || Nodes == null) return null;
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i] != null && Nodes[i].NodeId == nodeId)
                    return Nodes[i];
            }
            return null;
        }

        public MapNode ShelterNode => GetNode(ShelterNodeId);

        public MapPath GetPath(string a, string b)
        {
            if (Paths == null) return null;
            for (int i = 0; i < Paths.Count; i++)
            {
                if (Paths[i] != null && Paths[i].Connects(a, b))
                    return Paths[i];
            }
            return null;
        }

        /// <summary>
        /// Weather travel multiplier. Blizzards double path time (spec).
        /// </summary>
        public static float WeatherTravelMultiplier(WeatherKind weather)
        {
            switch (weather)
            {
                case WeatherKind.Blizzard: return 2f;
                case WeatherKind.FalloutStorm: return 1.75f;
                case WeatherKind.Ashfall: return 1.35f;
                case WeatherKind.Rain: return 1.15f;
                case WeatherKind.Overcast: return 1.05f;
                default: return 1f;
            }
        }

        /// <summary>Base path hours × weather multiplier.</summary>
        public float GetPathTravelHours(string fromId, string toId, WeatherKind weather)
        {
            var path = GetPath(fromId, toId);
            if (path == null) return 0f;
            return path.BaseTravelHours * WeatherTravelMultiplier(weather);
        }

        /// <summary>
        /// Shortest-path travel hours from shelter to node (sum of edge bases),
        /// then apply weather. Returns 0 for shelter / missing.
        /// </summary>
        public float GetTravelHoursFromShelter(string nodeId, WeatherKind weather)
        {
            if (string.IsNullOrEmpty(nodeId) || nodeId == ShelterNodeId) return 0f;
            var route = FindPath(ShelterNodeId, nodeId);
            if (route == null || route.Count < 2) return 0f;

            float hours = 0f;
            for (int i = 0; i < route.Count - 1; i++)
            {
                var path = GetPath(route[i], route[i + 1]);
                if (path != null) hours += path.BaseTravelHours;
            }
            return hours * WeatherTravelMultiplier(weather);
        }

        /// <summary>BFS shortest path by hop count; ties broken by lower base hours.</summary>
        public List<string> FindPath(string fromId, string toId)
        {
            var path = new List<string>();
            return TryFindPath(fromId, toId, path) ? path : null;
        }

        /// <summary>
        /// Buffer overload: fills <paramref name="buffer"/> (cleared first) instead of
        /// allocating a fresh list per call. Returns false when no path exists.
        /// </summary>
        public bool TryFindPath(string fromId, string toId, List<string> buffer)
        {
            if (buffer != null) buffer.Clear();
            if (string.IsNullOrEmpty(fromId) || string.IsNullOrEmpty(toId) || buffer == null) return false;
            if (fromId == toId)
            {
                buffer.Add(fromId);
                return true;
            }

            var neighbors = BuildAdjacency();
            if (!neighbors.ContainsKey(fromId) || !neighbors.ContainsKey(toId))
                return false;

            var prev = new Dictionary<string, string>();
            var dist = new Dictionary<string, float>();
            var queue = new Queue<string>();
            queue.Enqueue(fromId);
            dist[fromId] = 0f;

            while (queue.Count > 0)
            {
                string cur = queue.Dequeue();
                if (cur == toId) break;
                if (!neighbors.TryGetValue(cur, out var edges)) continue;
                for (int i = 0; i < edges.Count; i++)
                {
                    string next = edges[i].nodeId;
                    float cost = dist[cur] + edges[i].hours;
                    if (dist.TryGetValue(next, out float existing) && existing <= cost + 0.0001f)
                        continue;
                    dist[next] = cost;
                    prev[next] = cur;
                    queue.Enqueue(next);
                }
            }

            if (!prev.ContainsKey(toId)) return false;

            string walk = toId;
            buffer.Add(walk);
            while (prev.TryGetValue(walk, out string p))
            {
                buffer.Add(p);
                walk = p;
            }
            buffer.Reverse();
            return true;
        }

        /// <summary>Reveal a node (radio intel or first visit). Keeps rumored rad.</summary>
        public void RevealNode(string nodeId)
        {
            var n = GetNode(nodeId);
            if (n == null || n.IsShelter) return;
            if (!n.IsRevealed)
            {
                n.IsRevealed = true;
                OnMapChanged?.Invoke();
            }
        }

        /// <summary>Mark visited + revealed (survivor reached the site).</summary>
        public void MarkVisited(string nodeId)
        {
            var n = GetNode(nodeId);
            if (n == null || n.IsShelter) return;
            bool changed = false;
            if (!n.IsVisited) { n.IsVisited = true; changed = true; }
            if (!n.IsRevealed) { n.IsRevealed = true; changed = true; }
            if (changed) OnMapChanged?.Invoke();
        }

        /// <summary>Inject or update rumored rad (radio / events) without full reveal.</summary>
        public void SetRumoredRad(string nodeId, float rumoredRad)
        {
            var n = GetNode(nodeId);
            if (n == null) return;
            n.RumoredRad = Mathf.Max(0f, rumoredRad);
            OnMapChanged?.Invoke();
        }

        /// <summary>
        /// Player-facing view. Unsurveyed/unrevealed nodes are silhouettes with rumoredRad only.
        /// </summary>
        public MapNodePlayerView GetPlayerView(string nodeId)
        {
            var n = GetNode(nodeId);
            var view = new MapNodePlayerView { NodeId = nodeId, DisplayedRad = float.NaN };
            if (n == null) return view;

            view.Ring = n.Ring;
            view.DistanceFromShelter = n.DistanceFromShelter;
            view.RumoredRad = n.RumoredRad;
            view.IsRevealed = n.IsRevealed || n.IsVisited;
            view.IsVisited = n.IsVisited;
            view.IsShelter = n.IsShelter;
            view.LootTableId = view.IsRevealed ? n.LootTableId : string.Empty;
            view.DangerLevel = view.IsRevealed ? n.DangerLevel : 0f;
            view.LayoutX = Mathf.Cos(n.AngleRadians) * n.LayoutRadius;
            view.LayoutY = Mathf.Sin(n.AngleRadians) * n.LayoutRadius;

            if (n.IsShelter)
            {
                view.Label = n.DisplayName;
                view.IsSilhouette = false;
                view.DisplayedRad = 0f;
                return view;
            }

            if (view.IsRevealed)
            {
                view.Label = n.DisplayName;
                view.IsSilhouette = false;
                // Once revealed, still show rumor-blended estimate until surveyed in KnowledgeMap;
                // map itself exposes true only after visit for planning honesty on visited sites.
                view.DisplayedRad = n.IsVisited ? n.TrueRad : n.RumoredRad;
            }
            else
            {
                view.Label = n.GetDisplayLabel();
                view.IsSilhouette = true;
                view.DisplayedRad = n.RumoredRad;
            }
            return view;
        }

        public List<MapNodePlayerView> GetAllPlayerViews()
        {
            var list = new List<MapNodePlayerView>();
            FillPlayerViews(list);
            return list;
        }

        /// <summary>
        /// Buffer overload: clears <paramref name="buffer"/> and fills it in place,
        /// so steady-state map refreshes allocate nothing (pool-friendly hot path).
        /// </summary>
        public void GetAllPlayerViews(List<MapNodePlayerView> buffer)
        {
            if (buffer == null) return;
            buffer.Clear();
            FillPlayerViews(buffer);
        }

        private void FillPlayerViews(List<MapNodePlayerView> buffer)
        {
            if (Nodes == null) return;
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i] != null)
                    buffer.Add(GetPlayerView(Nodes[i].NodeId));
            }
        }

        /// <summary>Stable fingerprint for determinism tests (ids, loot, distances, edges).</summary>
        public string ComputeLayoutFingerprint()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("seed=").Append(Seed).Append(';');
            if (Nodes != null)
            {
                // Sort by id for stable order
                var ordered = new List<MapNode>(Nodes);
                ordered.Sort((a, b) => string.CompareOrdinal(a?.NodeId, b?.NodeId));
                for (int i = 0; i < ordered.Count; i++)
                {
                    var n = ordered[i];
                    if (n == null) continue;
                    sb.Append(n.NodeId).Append('|')
                        .Append(n.DisplayName).Append('|')
                        .Append((int)n.Ring).Append('|')
                        .Append(n.DistanceFromShelter.ToString("R")).Append('|')
                        .Append(n.TrueRad.ToString("R")).Append('|')
                        .Append(n.RumoredRad.ToString("R")).Append('|')
                        .Append(n.LootTableId).Append('|')
                        .Append(n.RadZoneProfileId).Append('|')
                        .Append(n.DangerLevel.ToString("R")).Append('|')
                        .Append(n.AngleRadians.ToString("R")).Append('|')
                        .Append(n.LayoutRadius.ToString("R")).Append('|');
                    if (n.EncounterDeckIds != null)
                    {
                        for (int e = 0; e < n.EncounterDeckIds.Count; e++)
                            sb.Append(n.EncounterDeckIds[e]).Append(',');
                    }
                    sb.Append(';');
                }
            }
            if (Paths != null)
            {
                var edges = new List<string>();
                for (int i = 0; i < Paths.Count; i++)
                {
                    var p = Paths[i];
                    if (p == null) continue;
                    string a = p.FromNodeId;
                    string b = p.ToNodeId;
                    if (string.CompareOrdinal(a, b) > 0) { var t = a; a = b; b = t; }
                    edges.Add(a + "-" + b + ":" + p.BaseTravelHours.ToString("R"));
                }
                edges.Sort(StringComparer.Ordinal);
                for (int i = 0; i < edges.Count; i++)
                    sb.Append(edges[i]).Append(';');
            }
            return sb.ToString();
        }

        public GeneratedMapSave CaptureState()
        {
            var save = new GeneratedMapSave { Seed = Seed };
            if (Nodes != null)
            {
                save.Nodes = new MapNodeRevealSave[Nodes.Count];
                for (int i = 0; i < Nodes.Count; i++)
                {
                    var n = Nodes[i];
                    if (n == null) continue;
                    save.Nodes[i] = new MapNodeRevealSave
                    {
                        NodeId = n.NodeId,
                        IsRevealed = n.IsRevealed,
                        IsVisited = n.IsVisited,
                        RumoredRad = n.RumoredRad
                    };
                }
            }
            return save;
        }

        /// <summary>Re-apply reveal/visit/rumor flags after regenerating from seed.</summary>
        public void RestoreRevealState(GeneratedMapSave save)
        {
            if (save?.Nodes == null) return;
            for (int i = 0; i < save.Nodes.Length; i++)
            {
                var row = save.Nodes[i];
                if (row == null) continue;
                var n = GetNode(row.NodeId);
                if (n == null) continue;
                n.IsRevealed = row.IsRevealed;
                n.IsVisited = row.IsVisited;
                n.RumoredRad = row.RumoredRad;
            }
            OnMapChanged?.Invoke();
        }

        private Dictionary<string, List<(string nodeId, float hours)>> BuildAdjacency()
        {
            var map = new Dictionary<string, List<(string, float)>>();
            void Ensure(string id)
            {
                if (!map.ContainsKey(id))
                    map[id] = new List<(string, float)>();
            }

            if (Nodes != null)
            {
                for (int i = 0; i < Nodes.Count; i++)
                    if (Nodes[i] != null) Ensure(Nodes[i].NodeId);
            }
            if (Paths != null)
            {
                for (int i = 0; i < Paths.Count; i++)
                {
                    var p = Paths[i];
                    if (p == null) continue;
                    Ensure(p.FromNodeId);
                    Ensure(p.ToNodeId);
                    map[p.FromNodeId].Add((p.ToNodeId, p.BaseTravelHours));
                    map[p.ToNodeId].Add((p.FromNodeId, p.BaseTravelHours));
                }
            }
            return map;
        }
    }

    [Serializable]
    public class GeneratedMapSave
    {
        public int Seed;
        public MapNodeRevealSave[] Nodes;
    }

    [Serializable]
    public class MapNodeRevealSave
    {
        public string NodeId;
        public bool IsRevealed;
        public bool IsVisited;
        public float RumoredRad;
    }
}
