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
        public event Action<string>? OnNodeCompleted;
        public event Action<string, bool>? OnNodeLockChanged;

        public WastelandMapSystem(WastelandMapState state,
            IEnumerable<MapNode> nodes, IEnumerable<MapRoute> routes)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));
            if (routes == null) throw new ArgumentNullException(nameof(routes));
            _nodes = new List<MapNode>();
            var validNodeIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var n in nodes)
            {
                if (n == null || string.IsNullOrEmpty(n.Id)) continue;
                _nodes.Add(n);
                validNodeIds.Add(n.Id);
            }
            if (_nodes.Count == 0)
                throw new InvalidOperationException("WastelandMapSystem: at least one node required.");

            _routes = new List<MapRoute>();
            var seenEdges = new HashSet<string>(StringComparer.Ordinal);
            foreach (var r in routes)
            {
                if (r == null) continue;
                if (string.IsNullOrEmpty(r.From) || string.IsNullOrEmpty(r.To)) continue;
                if (string.Equals(r.From, r.To, StringComparison.Ordinal)) continue; // ignore self-routes
                if (!validNodeIds.Contains(r.From) || !validNodeIds.Contains(r.To)) continue; // ignore dangling endpoints
                if (r.DistanceKm <= 0f || float.IsNaN(r.DistanceKm) || float.IsInfinity(r.DistanceKm)) continue; // ignore negative/zero distances
                string edgeKey = $"{r.From}->{r.To}";
                if (!seenEdges.Add(edgeKey)) continue; // ignore duplicate directed edges

                _routes.Add(r);
            }
            _state.NormalizeAndValidate(_nodes);
        }

        public IReadOnlyList<MapNode> Nodes => _nodes;
        public IReadOnlyList<MapRoute> Routes => _routes;
        public IReadOnlyList<string> DiscoveredNodes => _state.Discovered;
        public IReadOnlyList<string> CompletedNodes => _state.Completed;
        public IReadOnlyList<string> LockedNodes => _state.Locked;

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

        public bool IsCompleted(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return false;
            for (int i = 0; i < _state.Completed.Count; i++)
                if (_state.Completed[i] == nodeId) return true;
            return false;
        }

        public bool Complete(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return false;
            var node = FindNode(nodeId);
            if (node == null) return false;

            Discover(nodeId);

            if (IsCompleted(nodeId)) return true; // idempotent
            _state.Completed.Add(nodeId);
            OnNodeCompleted?.Invoke(nodeId);
            return true;
        }

        public bool IsLocked(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return false;
            if (_state.Unlocked.Contains(nodeId)) return false;
            if (_state.Locked.Contains(nodeId)) return true;
            var node = FindNode(nodeId);
            return node != null && node.Danger == MapNodeDanger.Locked;
        }

        public bool SetLocked(string nodeId, bool locked)
        {
            if (string.IsNullOrEmpty(nodeId)) return false;
            var node = FindNode(nodeId);
            if (node == null) return false;

            bool currentlyLocked = IsLocked(nodeId);
            if (locked == currentlyLocked) return true; // idempotent

            if (locked)
            {
                _state.Unlocked.Remove(nodeId);
                if (!_state.Locked.Contains(nodeId))
                    _state.Locked.Add(nodeId);
            }
            else
            {
                _state.Locked.Remove(nodeId);
                if (!_state.Unlocked.Contains(nodeId))
                    _state.Unlocked.Add(nodeId);
            }

            OnNodeLockChanged?.Invoke(nodeId, locked);
            return true;
        }

        public bool Unlock(string nodeId) => SetLocked(nodeId, false);
        public bool Lock(string nodeId) => SetLocked(nodeId, true);

        public MapNodeStatusKind ResolveNodeStatus(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return MapNodeStatusKind.Unavailable;
            var node = FindNode(nodeId);
            if (node == null) return MapNodeStatusKind.Unavailable;

            if (IsLocked(nodeId))
                return MapNodeStatusKind.Locked;

            if (IsCompleted(nodeId))
                return MapNodeStatusKind.Completed;

            if (IsDiscovered(nodeId))
                return MapNodeStatusKind.Discovered;

            for (int i = 0; i < _routes.Count; i++)
            {
                var r = _routes[i];
                if ((r.To == nodeId && IsDiscovered(r.From)) ||
                    (r.From == nodeId && IsDiscovered(r.To)))
                {
                    return MapNodeStatusKind.Available;
                }
            }

            return MapNodeStatusKind.Unavailable;
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
                    if (IsLocked(r.To) && r.To != toId) continue;
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

    /// <summary>Status of a map node for player interaction and visibility.</summary>
    public enum MapNodeStatusKind
    {
        /// <summary>Node has been discovered on the map.</summary>
        Discovered = 0,

        /// <summary>Node is available and reachable for travel/expeditions.</summary>
        Available = 1,

        /// <summary>Node is locked by story, hazard, or mission requirements.</summary>
        Locked = 2,

        /// <summary>Node content/expedition has been fully resolved or completed.</summary>
        Completed = 3,

        /// <summary>Node is currently unavailable or beyond travel range.</summary>
        Unavailable = 4
    }

    /// <summary>Runtime domain object representing a point of interest or sector node on the wasteland map.</summary>
    [Serializable]
    public sealed class MapNode
    {
        /// <summary>Unique location identifier.</summary>
        public string Id;

        /// <summary>Player-facing display name of the location.</summary>
        public string DisplayName;

        /// <summary>Threat / danger classification of the location.</summary>
        public MapNodeDanger Danger;

        /// <summary>Controlling faction identifier, or empty string if unaligned.</summary>
        public string FactionId;

        /// <summary>Loot table identifier for scavenging and salvage rewards.</summary>
        public string LootTableId;

        /// <summary>X coordinate on the world map canvas.</summary>
        public float PositionX;

        /// <summary>Y coordinate on the world map canvas.</summary>
        public float PositionY;

        /// <summary>Whether this node can be revealed via exploration.</summary>
        public bool Discoverable;

        /// <summary>Whether this node is revealed and unlocked from Day 1.</summary>
        public bool StartingUnlocked;

        /// <summary>Initializes a new empty instance of <see cref="MapNode"/>.</summary>
        public MapNode() { }
    }

    /// <summary>Danger rating classifications for wasteland locations.</summary>
    public enum MapNodeDanger
    {
        /// <summary>No immediate environmental or combat hazard.</summary>
        None = 0,

        /// <summary>Low threat level (light radiation/scavengers).</summary>
        Low = 1,

        /// <summary>Moderate threat level (moderate radiation/hostile patrols).</summary>
        Medium = 2,

        /// <summary>High threat level (severe radiation/warlord presence).</summary>
        High = 3,

        /// <summary>Locked node requiring keycard, quest, or demolition access.</summary>
        Locked = 4
    }

    /// <summary>Runtime domain object representing a travel route edge connecting two map nodes.</summary>
    [Serializable]
    public sealed class MapRoute
    {
        /// <summary>Origin map node identifier.</summary>
        public string From;

        /// <summary>Destination map node identifier.</summary>
        public string To;

        /// <summary>Travel distance in kilometers.</summary>
        public float DistanceKm;

        /// <summary>Weather hazard factor along the route.</summary>
        public float WeatherHazard;

        /// <summary>Initializes a new empty instance of <see cref="MapRoute"/>.</summary>
        public MapRoute() { }
    }

    /// <summary>Serializable state DTO for player wasteland map progression and fog-of-war.</summary>
    [Serializable]
    public sealed class WastelandMapState
    {
        /// <summary>List of discovered location node IDs.</summary>
        public List<string> Discovered = new List<string>();

        /// <summary>List of completed/scavenged location node IDs.</summary>
        public List<string> Completed = new List<string>();

        /// <summary>List of locked location node IDs.</summary>
        public List<string> Locked = new List<string>();

        /// <summary>List of dynamically unlocked location node IDs.</summary>
        public List<string> Unlocked = new List<string>();

        public void NormalizeAndValidate(IReadOnlyList<MapNode> nodes)
        {
            var validIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < nodes.Count; i++) validIds.Add(nodes[i].Id);

            // Always include starting nodes.
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].StartingUnlocked && !Discovered.Contains(nodes[i].Id))
                    Discovered.Add(nodes[i].Id);

                if (nodes[i].Danger == MapNodeDanger.Locked && !Unlocked.Contains(nodes[i].Id) && !Locked.Contains(nodes[i].Id))
                    Locked.Add(nodes[i].Id);
            }

            for (int i = Discovered.Count - 1; i >= 0; i--)
                if (!validIds.Contains(Discovered[i])) Discovered.RemoveAt(i);

            for (int i = Completed.Count - 1; i >= 0; i--)
                if (!validIds.Contains(Completed[i])) Completed.RemoveAt(i);

            for (int i = Locked.Count - 1; i >= 0; i--)
                if (!validIds.Contains(Locked[i])) Locked.RemoveAt(i);

            for (int i = Unlocked.Count - 1; i >= 0; i--)
                if (!validIds.Contains(Unlocked[i])) Unlocked.RemoveAt(i);
        }

        public WastelandMapState Capture() => new WastelandMapState
        {
            Discovered = new List<string>(Discovered),
            Completed = new List<string>(Completed),
            Locked = new List<string>(Locked),
            Unlocked = new List<string>(Unlocked)
        };

        public void RestoreInto(WastelandMapState state, IReadOnlyList<MapNode> nodes)
        {
            Discovered = state.Discovered != null ? new List<string>(state.Discovered) : new List<string>();
            Completed = state.Completed != null ? new List<string>(state.Completed) : new List<string>();
            Locked = state.Locked != null ? new List<string>(state.Locked) : new List<string>();
            Unlocked = state.Unlocked != null ? new List<string>(state.Unlocked) : new List<string>();
            NormalizeAndValidate(nodes);
        }
    }
}
