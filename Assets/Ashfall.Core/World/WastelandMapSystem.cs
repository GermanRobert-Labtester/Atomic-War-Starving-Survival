using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.World
{
    /// <summary>
    /// ASHFALL Travel Map authority (item 4).
    ///
    /// Core query + state for wasteland travel. Reads
    /// <c>Assets/StreamingAssets/Data/wasteland_map_v1.json</c> for
    /// canonical nodes + route edges. Tracks per-node discovery state,
    /// runs deterministic route planning between two nodes, and exposes
    /// the data the host (WastelandMapView, MapAtlasPanel, expedition
    /// launchers) needs to render fog-of-war, hazards, and progress.
    /// </summary>
    public sealed class WastelandMapSystem
    {
        private readonly WastelandMapState _state;
        private readonly List<MapNode> _nodes;
        private readonly List<MapRoute> _routes;

        public event Action<string>? OnNodeDiscovered;

        public WastelandMapSystem(WastelandMapState state,
            IEnumerable<MapNode> nodes, IEnumerable<MapRoute> routes)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));
            if (routes == null) throw new ArgumentNullException(nameof(routes));
            _nodes = new List<MapNode>();
            foreach (var n in nodes)
            {
                if (n == null || string.IsNullOrEmpty(n.Id)) continue;
                _nodes.Add(n);
            }
            if (_nodes.Count == 0)
                throw new InvalidOperationException("WastelandMapSystem: at least one node required.");
            _routes = new List<MapRoute>(routes);
            _state.NormalizeAndValidate(_nodes);
        }

        public IReadOnlyList<MapNode> Nodes => _nodes;
        public IReadOnlyList<MapRoute> Routes => _routes;

        public bool IsDiscovered(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return false;
            for (int i = 0; i < _state.Discovered.Count; i++)
                if (_state.Discovered[i] == nodeId) return true;
            return false;
        }

        public bool Discover(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return false;
            var node = FindNode(nodeId);
            if (node == null) return false;
            if (IsDiscovered(nodeId)) return true; // idempotent
            _state.Discovered.Add(nodeId);
            OnNodeDiscovered?.Invoke(nodeId);
            return true;
        }

        public MapNode? GetNode(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return null;
            for (int i = 0; i < _nodes.Count; i++)
                if (_nodes[i].Id == nodeId) return _nodes[i];
            return null;
        }

        public IReadOnlyList<MapRoute> GetRoutesFrom(string nodeId)
        {
            var list = new List<MapRoute>();
            if (string.IsNullOrEmpty(nodeId)) return list;
            for (int i = 0; i < _routes.Count; i++)
                if (_routes[i].From == nodeId) list.Add(_routes[i]);
            return list;
        }

        /// <summary>
        /// Deterministic BFS shortest path (by distance) between two
        /// discovered nodes. Returns an empty list when no path exists.
        /// Undiscovered intermediate nodes are not traversed.
        /// </summary>
        public List<string> PlanRoute(string fromId, string toId)
        {
            var path = new List<string>();
            if (string.IsNullOrEmpty(fromId) || string.IsNullOrEmpty(toId)) return path;
            if (fromId == toId) { path.Add(fromId); return path; }
            if (!IsDiscovered(fromId) || !IsDiscovered(toId)) return path;
            var dist = new Dictionary<string, float>(StringComparer.Ordinal);
            var prev = new Dictionary<string, string?>(StringComparer.Ordinal);
            var queue = new Queue<string>();
            foreach (var n in _nodes) dist[n.Id] = float.PositiveInfinity;
            dist[fromId] = 0f;
            queue.Enqueue(fromId);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (current == toId) break;
                foreach (var r in _routes)
                {
                    if (r.From != current) continue;
                    if (!IsDiscovered(r.To)) continue;
                    float nd = dist[current] + r.DistanceKm;
                    if (nd < dist[r.To])
                    {
                        dist[r.To] = nd;
                        prev[r.To] = current;
                        queue.Enqueue(r.To);
                    }
                }
            }
            if (float.IsPositiveInfinity(dist[toId])) return path;
            var rev = new List<string>();
            string? at = toId;
            while (at != null)
            {
                rev.Add(at);
                if (!prev.TryGetValue(at, out var p)) break;
                at = p;
            }
            rev.Reverse();
            return rev;
        }

        public WastelandMapState CaptureState() => _state.Capture();

        public void RestoreState(WastelandMapState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            _state.RestoreInto(state, _nodes);
        }

        private MapNode? FindNode(string id)
        {
            for (int i = 0; i < _nodes.Count; i++)
                if (_nodes[i].Id == id) return _nodes[i];
            return null;
        }
    }

    [Serializable]
    public sealed class MapNode
    {
        public string Id;
        public string DisplayName;
        public MapNodeDanger Danger;
        public string FactionId;
        public string LootTableId;
        public float PositionX;
        public float PositionY;
        public bool Discoverable;
        public bool StartingUnlocked;

        public MapNode() { }
    }

    public enum MapNodeDanger
    {
        None = 0,
        Low = 1,
        Medium = 2,
        High = 3,
        Locked = 4
    }

    [Serializable]
    public sealed class MapRoute
    {
        public string From;
        public string To;
        public float DistanceKm;
        public float WeatherHazard;

        public MapRoute() { }
    }

    [Serializable]
    public sealed class WastelandMapState
    {
        public List<string> Discovered = new List<string>();

        public void NormalizeAndValidate(IReadOnlyList<MapNode> nodes)
        {
            var validIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < nodes.Count; i++) validIds.Add(nodes[i].Id);

            // Always include starting nodes.
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].StartingUnlocked && !Discovered.Contains(nodes[i].Id))
                    Discovered.Add(nodes[i].Id);
            }

            for (int i = Discovered.Count - 1; i >= 0; i--)
                if (!validIds.Contains(Discovered[i])) Discovered.RemoveAt(i);
        }

        public WastelandMapState Capture() => new WastelandMapState
        {
            Discovered = new List<string>(Discovered)
        };

        public void RestoreInto(WastelandMapState state, IReadOnlyList<MapNode> nodes)
        {
            Discovered = state.Discovered != null ? new List<string>(state.Discovered) : new List<string>();
            NormalizeAndValidate(nodes);
        }
    }
}
