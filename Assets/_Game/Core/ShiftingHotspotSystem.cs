using System;
using System.Collections.Generic;
using AtomicWar._Game.Environment;
using UnityEngine;
using Random = System.Random;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Result of a successful windstorm hotspot shift (Prompt #14).
    /// </summary>
    public sealed class HotspotShiftResult
    {
        public string FromNodeId;
        public string ToNodeId;
        public float MovedRad;
        public float FromResidualRad;
        public int Day;
    }

    /// <summary>
    /// Prompt #14 — Shifting Hotspots: lethal death-zone rad pockets migrate
    /// two path-hops after Day 30 windstorms. Knowledge map is invalidated so
    /// old surveys lie until re-measured.
    /// </summary>
    public class ShiftingHotspotSystem
    {
        /// <summary>Matches <see cref="MapGenerator.DeathZoneRadThreshold"/>.</summary>
        public const float DeathZoneRadThreshold = MapGenerator.DeathZoneRadThreshold;

        /// <summary>Rad left behind when a pocket moves (ambient residual).</summary>
        public const float ResidualRadAfterShift = 28f;

        /// <summary>Path hops the lethal pocket travels (spec: two hops).</summary>
        public const int ShiftHopDistance = 2;

        /// <summary>Earliest campaign day a windstorm shift may fire.</summary>
        public const int MinDayForShift = 30;

        /// <summary>Daily chance of a windstorm shift once eligible.</summary>
        public const float DailyShiftChance = 0.18f;

        /// <summary>Minimum days between successful shifts.</summary>
        public const int MinDaysBetweenShifts = 5;

        private readonly Random _rng;
        private GeneratedMap _map;
        private RadiationKnowledgeMap _knowledge;
        private int _lastShiftDay = -999;
        private int _shiftCount;
        private readonly List<HotspotShiftRecord> _history = new List<HotspotShiftRecord>();
        /// <summary>Cached undirected adjacency; rebuilt when map is rebound.</summary>
        private Dictionary<string, List<string>> _adjacencyCache;

        public int LastShiftDay => _lastShiftDay;
        public int ShiftCount => _shiftCount;
        public IReadOnlyList<HotspotShiftRecord> History => _history;

        public event Action<HotspotShiftResult> OnHotspotShifted;
        public event Action OnStateChanged;

        public ShiftingHotspotSystem(Random rng = null)
        {
            _rng = rng ?? new Random(14);
        }

        public void Bind(GeneratedMap map, RadiationKnowledgeMap knowledge = null)
        {
            _map = map;
            _knowledge = knowledge;
            // Map topology is fixed after bind — rebuild adjacency once.
            _adjacencyCache = null;
        }

        /// <summary>Count current death-zone nodes on the bound map.</summary>
        public int CountDeathZones()
        {
            if (_map?.Nodes == null) return 0;
            int n = 0;
            for (int i = 0; i < _map.Nodes.Count; i++)
            {
                if (_map.Nodes[i] != null && _map.Nodes[i].IsDeathZone)
                    n++;
            }
            return n;
        }

        /// <summary>
        /// Daily roll: post-Day-30 rare windstorm may move one death zone.
        /// Returns true when a shift applied.
        /// </summary>
        public bool TickDay(int day)
        {
            if (day < MinDayForShift) return false;
            if (_map == null) return false;
            if (day - _lastShiftDay < MinDaysBetweenShifts) return false;
            if (_rng.NextDouble() >= DailyShiftChance) return false;
            return TryShift(day) != null;
        }

        /// <summary>
        /// Force a shift for tests / scripted events. Picks a death zone and a
        /// two-hop non-shelter target (prefer not already a death zone).
        /// </summary>
        public HotspotShiftResult TryShift(int day, string preferFromId = null)
        {
            if (_map?.Nodes == null) return null;

            MapNode from = null;
            if (!string.IsNullOrEmpty(preferFromId))
            {
                from = _map.GetNode(preferFromId);
                if (from != null && (!from.IsDeathZone || from.IsShelter))
                    from = null;
            }
            if (from == null)
                from = PickDeathZoneSource();
            if (from == null) return null;

            var targets = CollectNodesAtHopDistance(from.NodeId, ShiftHopDistance);
            if (targets.Count == 0) return null;

            // Prefer non-death-zone, non-shelter destinations
            MapNode to = PickTarget(targets);
            if (to == null) return null;

            return ApplyShift(from, to, day);
        }

        /// <summary>
        /// Nodes exactly <paramref name="hops"/> undirected path-hops from origin.
        /// Shelter is excluded from the result list.
        /// </summary>
        public List<MapNode> CollectNodesAtHopDistance(string originId, int hops)
        {
            var result = new List<MapNode>();
            if (_map == null || string.IsNullOrEmpty(originId) || hops < 1) return result;

            var adj = BuildAdjacency();
            if (!adj.ContainsKey(originId)) return result;

            var dist = new Dictionary<string, int> { [originId] = 0 };
            var q = new Queue<string>();
            q.Enqueue(originId);
            while (q.Count > 0)
            {
                string cur = q.Dequeue();
                int d = dist[cur];
                if (d >= hops) continue;
                if (!adj.TryGetValue(cur, out var neighbors)) continue;
                for (int i = 0; i < neighbors.Count; i++)
                {
                    string nb = neighbors[i];
                    if (dist.ContainsKey(nb)) continue;
                    dist[nb] = d + 1;
                    q.Enqueue(nb);
                }
            }

            foreach (var kv in dist)
            {
                if (kv.Value != hops) continue;
                var node = _map.GetNode(kv.Key);
                if (node == null || node.IsShelter) continue;
                result.Add(node);
            }

            // Stable order for determinism when picking with RNG
            result.Sort((a, b) => string.CompareOrdinal(a.NodeId, b.NodeId));
            return result;
        }

        private MapNode PickDeathZoneSource()
        {
            var zones = new List<MapNode>();
            for (int i = 0; i < _map.Nodes.Count; i++)
            {
                var n = _map.Nodes[i];
                if (n != null && n.IsDeathZone && !n.IsShelter)
                    zones.Add(n);
            }
            if (zones.Count == 0) return null;
            zones.Sort((a, b) => string.CompareOrdinal(a.NodeId, b.NodeId));
            return zones[_rng.Next(zones.Count)];
        }

        private MapNode PickTarget(List<MapNode> candidates)
        {
            if (candidates == null || candidates.Count == 0) return null;
            var preferred = new List<MapNode>();
            for (int i = 0; i < candidates.Count; i++)
            {
                if (candidates[i] != null && !candidates[i].IsDeathZone)
                    preferred.Add(candidates[i]);
            }
            var pool = preferred.Count > 0 ? preferred : candidates;
            return pool[_rng.Next(pool.Count)];
        }

        private HotspotShiftResult ApplyShift(MapNode from, MapNode to, int day)
        {
            float moved = from.TrueRad;
            float residual = ResidualRadAfterShift;

            // Old pocket cools toward ambient residual
            from.TrueRad = residual;
            from.RumoredRad = residual * 0.5f;
            from.IsDeathZone = false;

            // New pocket inherits lethal rad
            to.TrueRad = moved;
            to.RumoredRad = moved * 0.4f;
            to.IsDeathZone = moved >= DeathZoneRadThreshold;

            SyncKnowledge(from);
            SyncKnowledge(to);

            var record = new HotspotShiftRecord
            {
                FromNodeId = from.NodeId,
                ToNodeId = to.NodeId,
                MovedRad = moved,
                FromResidualRad = residual,
                Day = day
            };
            _history.Add(record);
            _lastShiftDay = day;
            _shiftCount++;

            _map.NotifyMapChanged();

            var result = new HotspotShiftResult
            {
                FromNodeId = from.NodeId,
                ToNodeId = to.NodeId,
                MovedRad = moved,
                FromResidualRad = residual,
                Day = day
            };
            OnHotspotShifted?.Invoke(result);
            OnStateChanged?.Invoke();
            return result;
        }

        private void SyncKnowledge(MapNode node)
        {
            if (_knowledge == null || node == null) return;
            if (_knowledge.GetTile(node.NodeId) == null)
                _knowledge.SeedTile(node.NodeId, node.TrueRad, node.RumoredRad, 1f);
            else
            {
                _knowledge.SetTrueRad(node.NodeId, node.TrueRad);
                _knowledge.InvalidateKnowledge(node.NodeId, node.RumoredRad);
            }
        }

        private Dictionary<string, List<string>> BuildAdjacency()
        {
            if (_adjacencyCache != null) return _adjacencyCache;
            if (_map == null)
            {
                _adjacencyCache = new Dictionary<string, List<string>>();
                return _adjacencyCache;
            }

            var map = new Dictionary<string, List<string>>();
            void Ensure(string id)
            {
                if (!map.ContainsKey(id))
                    map[id] = new List<string>();
            }

            if (_map.Nodes != null)
            {
                for (int i = 0; i < _map.Nodes.Count; i++)
                    if (_map.Nodes[i] != null) Ensure(_map.Nodes[i].NodeId);
            }
            if (_map.Paths != null)
            {
                for (int i = 0; i < _map.Paths.Count; i++)
                {
                    var p = _map.Paths[i];
                    if (p == null) continue;
                    Ensure(p.FromNodeId);
                    Ensure(p.ToNodeId);
                    map[p.FromNodeId].Add(p.ToNodeId);
                    map[p.ToNodeId].Add(p.FromNodeId);
                }
            }
            _adjacencyCache = map;
            return _adjacencyCache;
        }

        /// <summary>
        /// Replay saved history onto a freshly seed-generated map (save/load).
        /// </summary>
        public void ReplayHistoryOntoMap(GeneratedMap map, RadiationKnowledgeMap knowledge = null)
        {
            if (map == null || _history.Count == 0) return;
            var prevMap = _map;
            var prevKnowledge = _knowledge;
            Bind(map, knowledge);
            for (int i = 0; i < _history.Count; i++)
            {
                var rec = _history[i];
                if (rec == null) continue;
                var from = map.GetNode(rec.FromNodeId);
                var to = map.GetNode(rec.ToNodeId);
                if (from == null || to == null) continue;

                from.TrueRad = rec.FromResidualRad;
                from.RumoredRad = rec.FromResidualRad * 0.5f;
                from.IsDeathZone = false;

                to.TrueRad = rec.MovedRad;
                to.RumoredRad = rec.MovedRad * 0.4f;
                to.IsDeathZone = rec.MovedRad >= DeathZoneRadThreshold;

                SyncKnowledge(from);
                SyncKnowledge(to);
            }
            map.NotifyMapChanged();
            // Keep binding on the restored map
            if (prevMap != null && prevMap != map)
            {
                // already bound to map
            }
            _ = prevKnowledge;
        }

        public ShiftingHotspotSave CaptureState()
        {
            var save = new ShiftingHotspotSave
            {
                LastShiftDay = _lastShiftDay,
                ShiftCount = _shiftCount
            };
            for (int i = 0; i < _history.Count; i++)
            {
                var h = _history[i];
                if (h == null) continue;
                save.History.Add(new HotspotShiftRecord
                {
                    FromNodeId = h.FromNodeId,
                    ToNodeId = h.ToNodeId,
                    MovedRad = h.MovedRad,
                    FromResidualRad = h.FromResidualRad,
                    Day = h.Day
                });
            }
            return save;
        }

        public void RestoreState(ShiftingHotspotSave save)
        {
            _history.Clear();
            if (save == null)
            {
                _lastShiftDay = -999;
                _shiftCount = 0;
                if (_map != null)
                    ResetMapToSeedBaseline();
                return;
            }
            _lastShiftDay = save.LastShiftDay;
            _shiftCount = Math.Max(0, save.ShiftCount);
            if (save.History != null)
            {
                for (int i = 0; i < save.History.Count; i++)
                {
                    var h = save.History[i];
                    if (h == null || string.IsNullOrEmpty(h.FromNodeId)) continue;
                    _history.Add(new HotspotShiftRecord
                    {
                        FromNodeId = h.FromNodeId,
                        ToNodeId = h.ToNodeId,
                        MovedRad = h.MovedRad,
                        FromResidualRad = h.FromResidualRad,
                        Day = h.Day
                    });
                }
            }
            // Seed baseline then re-apply windstorm history (idempotent across loads).
            if (_map != null)
            {
                ResetMapToSeedBaseline();
                if (_history.Count > 0)
                    ReplayHistoryOntoMap(_map, _knowledge);
            }
        }

        /// <summary>
        /// Copy TrueRad / RumoredRad / IsDeathZone from a fresh seed generate so
        /// history replay never double-applies mid-session loads.
        /// </summary>
        private void ResetMapToSeedBaseline()
        {
            if (_map == null) return;
            var clean = MapGenerator.Generate(_map.Seed);
            if (clean?.Nodes == null) return;
            for (int i = 0; i < _map.Nodes.Count; i++)
            {
                var n = _map.Nodes[i];
                if (n == null) continue;
                var c = clean.GetNode(n.NodeId);
                if (c == null) continue;
                n.TrueRad = c.TrueRad;
                n.RumoredRad = c.RumoredRad;
                n.IsDeathZone = c.IsDeathZone;
                if (_knowledge != null)
                {
                    if (_knowledge.GetTile(n.NodeId) == null)
                        _knowledge.SeedTile(n.NodeId, n.TrueRad, n.RumoredRad, 1f);
                    else
                        _knowledge.SetTrueRad(n.NodeId, n.TrueRad);
                }
            }
        }
    }

    [Serializable]
    public class HotspotShiftRecord
    {
        public string FromNodeId;
        public string ToNodeId;
        public float MovedRad;
        public float FromResidualRad;
        public int Day;
    }

    [Serializable]
    public class ShiftingHotspotSave
    {
        public int LastShiftDay = -999;
        public int ShiftCount;
        public List<HotspotShiftRecord> History = new List<HotspotShiftRecord>();
    }
}
