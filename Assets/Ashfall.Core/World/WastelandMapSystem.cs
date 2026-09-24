// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Campaign;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Underground;
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
        private readonly Dictionary<string, TrapSiteMapLocation> _trapSiteLocations =
            new Dictionary<string, TrapSiteMapLocation>(StringComparer.Ordinal);

        /// <summary>Plan 167: Subterranean tunnel network authority.</summary>
        public TunnelNetworkSystem Tunnels { get; }

        public event Action<string>? OnNodeDiscovered;
        public event Action<string, MapFogState>? OnNodeKnowledgeChanged;
        public event Action<string>? OnNodeCompleted;
        public event Action<string, bool>? OnNodeLockChanged;
        public event Action? OnMarkersChanged;

        public WastelandMapSystem(WastelandMapState state,
            IEnumerable<MapNode> nodes, IEnumerable<MapRoute> routes,
            IEnumerable<TrapSiteMapLocation>? trapSiteLocations = null,
            TunnelNetworkCatalogData? tunnelCatalog = null)
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
            _state.Tunnels ??= new TunnelNetworkState();
            Tunnels = new TunnelNetworkSystem(_state.Tunnels);
            // Plan 167 — seed from the authored catalog when the host supplies
            // it; otherwise fall back to the built-in canonical network. A
            // restored campaign replaces this state via RestoreState() before
            // any tick, so the player's discovered/repaired network survives.
            if (tunnelCatalog != null)
            {
                Tunnels.Clear();
                Tunnels.LoadCatalog(tunnelCatalog);
            }
            EnsureCanonicalTunnels();
            if (trapSiteLocations != null)
            {
                foreach (var location in trapSiteLocations)
                    RegisterTrapSiteLocation(location);
            }
        }

        public WastelandMapState State => _state;
        public IReadOnlyList<MapNode> Nodes => _nodes;
        public IReadOnlyList<MapRoute> Routes => _routes;
        public IReadOnlyList<string> DiscoveredNodes => _state.Discovered;
        public IReadOnlyList<string> CompletedNodes => _state.Completed;
        public IReadOnlyList<string> LockedNodes => _state.Locked;
        public IReadOnlyList<MapNodeKnowledgeState> Knowledge => _state.Knowledge;
        public IReadOnlyList<MapMarkerState> Markers => _state.Markers;

        public void RegisterTrapSiteLocation(TrapSiteMapLocation location)
        {
            if (location == null || string.IsNullOrEmpty(location.SiteId)) return;
            _trapSiteLocations[location.SiteId] = location;
        }

        public bool TryResolveTrapSitePosition(string siteId, out float positionX, out float positionY)
        {
            positionX = 0f;
            positionY = 0f;
            if (string.IsNullOrEmpty(siteId)) return false;

            var directNode = FindNode(siteId);
            if (directNode != null)
            {
                positionX = directNode.PositionX;
                positionY = directNode.PositionY;
                return true;
            }

            if (!_trapSiteLocations.TryGetValue(siteId, out var location)) return false;
            var anchor = FindNode(location.AnchorNodeId);
            if (anchor == null) return false;
            positionX = anchor.PositionX + location.OffsetX;
            positionY = anchor.PositionY + location.OffsetY;
            return true;
        }

        /// <summary>Creates or updates one known-information trap marker. The
        /// marker contains no catch, bycatch, disease, contamination, or yield.</summary>
        public bool UpsertTrapMarker(string siteId, string trapId, string trapType,
            float positionX, float positionY, bool broken)
        {
            if (string.IsNullOrEmpty(siteId)) return false;
            string markerId = TrapMarkerId(siteId);
            var marker = _state.Markers.FirstOrDefault(m => m != null && m.MarkerId == markerId);
            if (marker == null)
            {
                marker = new MapMarkerState { MarkerId = markerId };
                _state.Markers.Add(marker);
            }

            marker.Category = "trapping";
            marker.SourceId = siteId;
            marker.DefinitionId = trapId ?? string.Empty;
            marker.TrapType = trapType ?? string.Empty;
            marker.LabelKey = Ashfall.Core.Localization.WildlifeTrappingLocalization.TrapNameKey(trapId ?? string.Empty);
            marker.IconKey = "map.trap";
            marker.Condition = broken ? "broken" : "healthy";
            marker.PositionX = positionX;
            marker.PositionY = positionY;
            OnMarkersChanged?.Invoke();
            return true;
        }

        public bool EnsureTrapMarker(string siteId, string trapId, string trapType, bool broken)
        {
            if (!TryResolveTrapSitePosition(siteId, out float x, out float y)) return false;
            return UpsertTrapMarker(siteId, trapId, trapType, x, y, broken);
        }

        public bool RemoveTrapMarker(string siteId)
        {
            string markerId = TrapMarkerId(siteId);
            for (int i = _state.Markers.Count - 1; i >= 0; i--)
            {
                var marker = _state.Markers[i];
                if (marker != null && marker.MarkerId == markerId)
                {
                    _state.Markers.RemoveAt(i);
                    OnMarkersChanged?.Invoke();
                    return true;
                }
            }
            return false;
        }

        public void ReconcileTrapMarkers(IEnumerable<TrapMapMarkerSource> activeSources)
        {
            var expected = new HashSet<string>(StringComparer.Ordinal);
            if (activeSources != null)
            {
                foreach (var source in activeSources)
                {
                    if (source == null || string.IsNullOrEmpty(source.SiteId)) continue;
                    expected.Add(TrapMarkerId(source.SiteId));
                    UpsertTrapMarker(source.SiteId, source.TrapId, source.TrapType,
                        source.PositionX, source.PositionY, source.IsBroken);
                }
            }

            for (int i = _state.Markers.Count - 1; i >= 0; i--)
            {
                var marker = _state.Markers[i];
                if (marker == null) { _state.Markers.RemoveAt(i); continue; }
                if (marker.Category == "trapping" && !expected.Contains(marker.MarkerId))
                    _state.Markers.RemoveAt(i);
            }
            OnMarkersChanged?.Invoke();
        }

        public static string TrapMarkerId(string siteId) => $"trap:{siteId ?? string.Empty}";

        public bool IsDiscovered(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return false;
            for (int i = 0; i < _state.Discovered.Count; i++)
                if (_state.Discovered[i] == nodeId) return true;
            return false;
        }

        public MapFogState GetFogState(string nodeId)
        {
            var k = GetNodeKnowledge(nodeId);
            return k?.FogState ?? MapFogState.Unknown;
        }

        public MapNodeKnowledgeState? GetNodeKnowledge(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return null;
            for (int i = 0; i < _state.Knowledge.Count; i++)
            {
                if (string.Equals(_state.Knowledge[i].NodeId, nodeId, StringComparison.Ordinal))
                    return _state.Knowledge[i];
            }
            return null;
        }

        public bool Discover(string nodeId) => DiscoverVisited(nodeId, "legacy", 1);

        public bool DiscoverRumor(string nodeId, string sourceId, int day, InformationConfidence confidence = InformationConfidence.Medium)
        {
            if (string.IsNullOrEmpty(nodeId)) return false;
            var node = FindNode(nodeId);
            if (node == null) return false;

            var existing = GetNodeKnowledge(nodeId);
            if (existing != null)
            {
                existing.LastConfirmedDay = Math.Max(existing.LastConfirmedDay, day);
                if (existing.FogState > MapFogState.Rumored)
                {
                    return true;
                }
                if (existing.Provenance != null && confidence > existing.Provenance.Confidence)
                {
                    existing.Provenance.Confidence = confidence;
                }
                return true;
            }

            var record = new MapNodeKnowledgeState
            {
                NodeId = nodeId,
                FogState = MapFogState.Rumored,
                LastConfirmedDay = Math.Max(1, day),
                Provenance = new CampaignProvenanceRecord(
                    KnowledgeSourceKind.RadioIntercept,
                    sourceId,
                    "radio_system",
                    day,
                    confidence,
                    nodeId)
            };

            _state.Knowledge.Add(record);
            if (!_state.Discovered.Contains(nodeId))
                _state.Discovered.Add(nodeId);

            OnNodeDiscovered?.Invoke(nodeId);
            OnNodeKnowledgeChanged?.Invoke(nodeId, MapFogState.Rumored);
            return true;
        }

        public bool DiscoverSurvey(string nodeId, string surveySourceId, int day, IEnumerable<string>? traits = null)
        {
            if (string.IsNullOrEmpty(nodeId)) return false;
            var node = FindNode(nodeId);
            if (node == null) return false;

            var existing = GetNodeKnowledge(nodeId);
            if (existing != null)
            {
                existing.LastConfirmedDay = Math.Max(existing.LastConfirmedDay, day);
                if (traits != null)
                {
                    foreach (var t in traits)
                    {
                        if (!string.IsNullOrEmpty(t) && !existing.Traits.Contains(t))
                            existing.Traits.Add(t);
                    }
                }

                if (existing.FogState < MapFogState.Surveyed)
                {
                    existing.FogState = MapFogState.Surveyed;
                    existing.Provenance = new CampaignProvenanceRecord(
                        KnowledgeSourceKind.ExpeditionSurvey,
                        surveySourceId,
                        "survey_engine",
                        day,
                        InformationConfidence.High,
                        nodeId);
                    OnNodeKnowledgeChanged?.Invoke(nodeId, MapFogState.Surveyed);
                }
                return true;
            }

            var record = new MapNodeKnowledgeState
            {
                NodeId = nodeId,
                FogState = MapFogState.Surveyed,
                LastConfirmedDay = Math.Max(1, day),
                Provenance = new CampaignProvenanceRecord(
                    KnowledgeSourceKind.ExpeditionSurvey,
                    surveySourceId,
                    "survey_engine",
                    day,
                    InformationConfidence.High,
                    nodeId)
            };

            if (traits != null)
            {
                foreach (var t in traits)
                {
                    if (!string.IsNullOrEmpty(t) && !record.Traits.Contains(t))
                        record.Traits.Add(t);
                }
            }

            _state.Knowledge.Add(record);
            if (!_state.Discovered.Contains(nodeId))
                _state.Discovered.Add(nodeId);

            OnNodeDiscovered?.Invoke(nodeId);
            OnNodeKnowledgeChanged?.Invoke(nodeId, MapFogState.Surveyed);
            CheckAutoTunnelDiscovery(nodeId);
            return true;
        }

        public bool DiscoverVisited(string nodeId, string survivorId, int day)
        {
            if (string.IsNullOrEmpty(nodeId)) return false;
            var node = FindNode(nodeId);
            if (node == null) return false;

            bool isNewlyDiscovered = !IsDiscovered(nodeId);

            var existing = GetNodeKnowledge(nodeId);
            if (existing != null)
            {
                existing.LastConfirmedDay = Math.Max(existing.LastConfirmedDay, day);
                if (existing.FogState != MapFogState.Visited)
                {
                    existing.FogState = MapFogState.Visited;
                    existing.Provenance = new CampaignProvenanceRecord(
                        KnowledgeSourceKind.ExpeditionVisit,
                        survivorId,
                        "expedition_system",
                        day,
                        InformationConfidence.Confirmed,
                        nodeId);
                    OnNodeKnowledgeChanged?.Invoke(nodeId, MapFogState.Visited);
                }
            }
            else
            {
                var record = new MapNodeKnowledgeState
                {
                    NodeId = nodeId,
                    FogState = MapFogState.Visited,
                    LastConfirmedDay = Math.Max(1, day),
                    Provenance = new CampaignProvenanceRecord(
                        KnowledgeSourceKind.ExpeditionVisit,
                        survivorId,
                        "expedition_system",
                        day,
                        InformationConfidence.Confirmed,
                        nodeId)
                };
                _state.Knowledge.Add(record);
                OnNodeKnowledgeChanged?.Invoke(nodeId, MapFogState.Visited);
            }

            if (!_state.Discovered.Contains(nodeId))
                _state.Discovered.Add(nodeId);

            if (isNewlyDiscovered)
                OnNodeDiscovered?.Invoke(nodeId);

            CheckAutoTunnelDiscovery(nodeId);
            return true;
        }

        public MapNodeIntelView? GetNodeIntel(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return null;
            var node = FindNode(nodeId);
            if (node == null) return null;

            var knowledge = GetNodeKnowledge(nodeId);
            var fogState = knowledge?.FogState ?? MapFogState.Unknown;

            if (fogState == MapFogState.Unknown)
            {
                return new MapNodeIntelView
                {
                    NodeId = nodeId,
                    DisplayName = "Unknown Sector",
                    FogState = MapFogState.Unknown,
                    PositionX = 0f,
                    PositionY = 0f,
                    Danger = MapNodeDanger.None,
                    DangerBand = "Unknown",
                    FactionId = string.Empty,
                    LootDescription = "Unknown",
                    Traits = Array.Empty<string>(),
                    Provenance = null,
                    LastConfirmedDay = 0,
                    Routable = false
                };
            }

            if (fogState == MapFogState.Rumored)
            {
                int hash = ComputeStableHash(nodeId);
                float offsetX = ((hash % 50) + 50) * (((hash & 1) == 0) ? 1f : -1f);
                float offsetY = (((hash / 50) % 50) + 50) * (((hash & 2) == 0) ? 1f : -1f);

                string dangerBand = node.Danger switch
                {
                    MapNodeDanger.Low => "Low",
                    MapNodeDanger.Medium => "Medium",
                    MapNodeDanger.High => "High",
                    MapNodeDanger.Locked => "High",
                    _ => "Low"
                };

                return new MapNodeIntelView
                {
                    NodeId = nodeId,
                    DisplayName = node.DisplayName,
                    FogState = MapFogState.Rumored,
                    PositionX = node.PositionX + offsetX,
                    PositionY = node.PositionY + offsetY,
                    Danger = MapNodeDanger.None,
                    DangerBand = dangerBand,
                    FactionId = string.Empty,
                    LootDescription = "Unconfirmed scrap / rumor",
                    Traits = Array.Empty<string>(),
                    Provenance = knowledge?.Provenance,
                    LastConfirmedDay = knowledge?.LastConfirmedDay ?? 0,
                    Routable = false
                };
            }

            return new MapNodeIntelView
            {
                NodeId = nodeId,
                DisplayName = node.DisplayName,
                FogState = fogState,
                PositionX = node.PositionX,
                PositionY = node.PositionY,
                Danger = node.Danger,
                DangerBand = node.Danger.ToString(),
                FactionId = node.FactionId ?? string.Empty,
                LootDescription = fogState == MapFogState.Visited ? node.LootTableId : $"Category: {node.LootTableId}",
                Traits = knowledge?.Traits != null ? knowledge.Traits.ToArray() : Array.Empty<string>(),
                Provenance = knowledge?.Provenance,
                LastConfirmedDay = knowledge?.LastConfirmedDay ?? 0,
                Routable = true
            };
        }

        private static int ComputeStableHash(string str)
        {
            unchecked
            {
                uint hash = 2166136261;
                for (int i = 0; i < str.Length; i++)
                {
                    hash ^= str[i];
                    hash *= 16777619;
                }
                return (int)(hash & 0x7FFFFFFF);
            }
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

        public MapRoute? GetRoute(string fromId, string toId)
        {
            if (string.IsNullOrEmpty(fromId) || string.IsNullOrEmpty(toId)) return null;
            for (int i = 0; i < _routes.Count; i++)
            {
                if (_routes[i].From == fromId && _routes[i].To == toId)
                    return _routes[i];
            }
            return null;
        }

        /// <summary>Checks whether the route between two nodes has the "flooded" tag (D16).</summary>
        public bool IsRouteFlooded(string fromId, string toId)
        {
            var route = GetRoute(fromId, toId);
            return route != null && route.IsFlooded;
        }

        /// <summary>Checks whether the route between two nodes has the specified tag (D16).</summary>
        public bool HasRouteTag(string fromId, string toId, string tag)
        {
            var route = GetRoute(fromId, toId);
            return route != null && route.HasTag(tag);
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
                    if (r.To != toId && GetFogState(r.To) == MapFogState.Rumored) continue;
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

        /// <summary>
        /// EN-02 / UNBLOCK-05: Projects a living map route with edge conditions,
        /// distance, hops, and terrain tags without rebuilding GraphTravelPlanner.
        /// </summary>
        public LivingMapRouteProjection ProjectLivingMapRoute(string fromId, string toId)
        {
            if (string.IsNullOrEmpty(fromId) || string.IsNullOrEmpty(toId))
                return LivingMapRouteProjection.Empty(fromId, toId);

            var path = PlanRoute(fromId, toId);
            if (path == null || path.Count == 0)
                return LivingMapRouteProjection.Empty(fromId, toId);

            if (fromId == toId)
                return new LivingMapRouteProjection(fromId, toId, path, 0f, 0, false, false, Array.Empty<string>(), true);

            float totalDistKm = 0f;
            bool hasFlooded = false;
            bool hasAmphibious = false;
            var tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < path.Count - 1; i++)
            {
                string u = path[i];
                string v = path[i + 1];
                var edge = GetRoute(u, v);
                if (edge != null)
                {
                    totalDistKm += edge.DistanceKm;
                    if (edge.IsFlooded) hasFlooded = true;
                    if (edge.IsAmphibious) hasAmphibious = true;
                    if (edge.Tags != null)
                    {
                        for (int t = 0; t < edge.Tags.Count; t++)
                            tags.Add(edge.Tags[t]);
                    }
                }
            }

            int hops = path.Count - 1;
            return new LivingMapRouteProjection(
                fromId,
                toId,
                path,
                totalDistKm,
                hops,
                hasFlooded,
                hasAmphibious,
                tags,
                true);
        }

        public ExpeditionEstimate EstimateRoute(
            ExpeditionDefinition def,
            ExpeditionStance stance = ExpeditionStance.Stealth,
            bool isNightScavenge = false,
            ExpeditionVehicleProfile? vehicle = null,
            float weaponReadiness = 1f,
            float weaponJamRisk = 0f)
        {
            return ExpeditionSystem.Estimate(def, stance, isNightScavenge, vehicle, weaponReadiness, weaponJamRisk);
        }

        public ExpeditionEstimate? EstimateRoute(
            string fromId,
            string toId,
            ExpeditionStance stance = ExpeditionStance.Stealth,
            bool isNightScavenge = false,
            ExpeditionVehicleProfile? vehicle = null,
            float weaponReadiness = 1f,
            float weaponJamRisk = 0f)
        {
            var path = PlanRoute(fromId, toId);
            if (path.Count == 0 && fromId != toId) return null;

            float totalDistKm = 0f;
            for (int i = 0; i < path.Count - 1; i++)
            {
                string u = path[i];
                string v = path[i + 1];
                for (int rIdx = 0; rIdx < _routes.Count; rIdx++)
                {
                    var r = _routes[rIdx];
                    if (r.From == u && r.To == v)
                    {
                        totalDistKm += r.DistanceKm;
                        break;
                    }
                }
            }

            var targetNode = GetNode(toId);
            if (targetNode == null) return null;

            int distanceTicks = Math.Max(1, (int)Math.Ceiling(totalDistKm / 5.0f));
            int dangerLevel = targetNode.Danger switch
            {
                MapNodeDanger.Low => 1,
                MapNodeDanger.Medium => 2,
                MapNodeDanger.High => 3,
                _ => 1
            };

            var def = new ExpeditionDefinition
            {
                id = toId,
                displayName = targetNode.DisplayName,
                distanceTicks = distanceTicks,
                dangerLevel = dangerLevel,
                scavenging_table_id = targetNode.LootTableId
            };

            return ExpeditionSystem.Estimate(def, stance, isNightScavenge, vehicle, weaponReadiness, weaponJamRisk);
        }

        public WastelandMapState CaptureState()
        {
            _state.Tunnels = Tunnels.CaptureState();
            return _state.Capture();
        }

        public void RestoreState(WastelandMapState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            _state.RestoreInto(state, _nodes);
            Tunnels.RestoreState(_state.Tunnels ?? new TunnelNetworkState());
            EnsureCanonicalTunnels();
            OnMarkersChanged?.Invoke();
        }

        // -----------------------------------------------------------------
        // Plan 167 — Underground Tunnel Network
        // -----------------------------------------------------------------

        public void EnsureCanonicalTunnels()
        {
            if (Tunnels.TotalSegmentCount > 0) return;

            Tunnels.RegisterJunction("loc_holdfast", "Holdfast Undercroft");
            Tunnels.RegisterJunction("loc_cut_abandoned_depot", "Depot Sub-Basement", hasResource: true, resourceType: "scrap");
            Tunnels.RegisterJunction("loc_cut_radiation_zone_alpha", "Vault Conduit Alpha");

            Tunnels.RegisterSegment(
                segmentId: "tun_holdfast_depot",
                name: "Depot Maintenance Tunnel",
                connectsFrom: "loc_holdfast",
                connectsTo: "loc_cut_abandoned_depot",
                lengthHours: 1.5f,
                difficulty: 2,
                integrity: 100f);

            Tunnels.RegisterSegment(
                segmentId: "tun_depot_alpha",
                name: "Deep Service Conduit",
                connectsFrom: "loc_cut_abandoned_depot",
                connectsTo: "loc_cut_radiation_zone_alpha",
                lengthHours: 2.5f,
                difficulty: 3,
                integrity: 75f);
        }

        public (bool CanTraverse, float TravelTimeHours, string Reason) CanTraverseTunnel(string fromNodeId, string toNodeId)
        {
            return Tunnels.CanTraverse(fromNodeId, toNodeId);
        }

        public bool DiscoverTunnel(string segmentId)
        {
            bool discovered = Tunnels.DiscoverSegment(segmentId);
            if (discovered)
            {
                OnMarkersChanged?.Invoke();
            }
            return discovered;
        }

        public bool RepairTunnel(string segmentId, float repairAmount = 50f)
        {
            bool repaired = Tunnels.RepairSegment(segmentId, repairAmount);
            if (repaired)
            {
                OnMarkersChanged?.Invoke();
            }
            return repaired;
        }

        public IReadOnlyList<TunnelSegment> GetDiscoveredTunnels() => Tunnels.GetDiscoveredSegments();

        private void CheckAutoTunnelDiscovery(string nodeId)
        {
            if (Tunnels == null) return;
            foreach (var segment in Tunnels.CaptureState().Segments)
            {
                if (!segment.IsDiscovered &&
                    (string.Equals(segment.ConnectsFrom, nodeId, StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(segment.ConnectsTo, nodeId, StringComparison.OrdinalIgnoreCase)))
                {
                    string otherNode = string.Equals(segment.ConnectsFrom, nodeId, StringComparison.OrdinalIgnoreCase)
                        ? segment.ConnectsTo
                        : segment.ConnectsFrom;
                    if (IsDiscovered(otherNode))
                    {
                        DiscoverTunnel(segment.SegmentId);
                    }
                }
            }
        }

        private MapNode? FindNode(string id)
        {
            for (int i = 0; i < _nodes.Count; i++)
                if (_nodes[i].Id == id) return _nodes[i];
            return null;
        }
    }

    /// <summary>Strategic wasteland cartography fog-of-war states.</summary>
    public enum MapFogState
    {
        Unknown = 0,
        Rumored = 1,
        Surveyed = 2,
        Visited = 3
    }

    /// <summary>
    /// Persistent knowledge state of a location node, tracking fog state, provenance, and survey traits.
    /// </summary>
    [Serializable]
    public sealed class MapNodeKnowledgeState
    {
        public string NodeId = string.Empty;
        public MapFogState FogState = MapFogState.Unknown;
        public CampaignProvenanceRecord? Provenance;
        public int LastConfirmedDay;
        public List<string> Traits = new List<string>();

        public MapNodeKnowledgeState() { }

        public MapNodeKnowledgeState Clone() => new MapNodeKnowledgeState
        {
            NodeId = NodeId,
            FogState = FogState,
            Provenance = Provenance?.Clone(),
            LastConfirmedDay = LastConfirmedDay,
            Traits = Traits != null ? new List<string>(Traits) : new List<string>()
        };
    }

    /// <summary>
    /// Player-facing derived intelligence view for a map node, applying fog-of-war obfuscation.
    /// </summary>
    public sealed class MapNodeIntelView
    {
        public string NodeId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public MapFogState FogState { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public MapNodeDanger Danger { get; set; }
        public string DangerBand { get; set; } = string.Empty;
        public string FactionId { get; set; } = string.Empty;
        public string LootDescription { get; set; } = string.Empty;
        public IReadOnlyList<string> Traits { get; set; } = Array.Empty<string>();
        public CampaignProvenanceRecord? Provenance { get; set; }
        public int LastConfirmedDay { get; set; }
        public bool Routable { get; set; }
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

        /// <summary>Travel domain: "land" or "water" (river, canal, lake).</summary>
        public string TravelDomain = "land";

        /// <summary>Water current strength (-1.0 upstream penalty to +1.0 downstream bonus).</summary>
        public float CurrentStrength = 0f;

        /// <summary>Toxic chemical/fallout contamination in water (0.0 clean to 1.0 deadly).</summary>
        public float ToxicContamination = 0f;

        /// <summary>Optional route condition and terrain tags (e.g. "flooded", "amphibious", "hazard_high") (D16).</summary>
        public List<string> Tags = new List<string>();

        /// <summary>Returns true if the route has the specified tag (case-insensitive).</summary>
        public bool HasTag(string tag)
        {
            if (Tags == null || string.IsNullOrWhiteSpace(tag)) return false;
            for (int i = 0; i < Tags.Count; i++)
            {
                if (string.Equals(Tags[i], tag, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>Whether this route is marked with the "flooded" tag (D16).</summary>
        public bool IsFlooded => HasTag("flooded");

        /// <summary>Whether this route is marked with the "amphibious" tag (D16).</summary>
        public bool IsAmphibious => HasTag("amphibious");

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

        /// <summary>List of registered damaged-map fragment IDs.</summary>
        public List<string> RegisteredMapFragments = new List<string>();

        /// <summary>Strategic knowledge and fog-of-war states per location.</summary>
        public List<MapNodeKnowledgeState> Knowledge = new List<MapNodeKnowledgeState>();

        /// <summary>Player-known map markers, including active trapping sites.</summary>
        public List<MapMarkerState> Markers = new List<MapMarkerState>();

        /// <summary>Plan 167: Subterranean tunnel network topology, hazards, and integrity.</summary>
        public TunnelNetworkState Tunnels = new TunnelNetworkState();

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

            if (Knowledge == null) Knowledge = new List<MapNodeKnowledgeState>();
            for (int i = Knowledge.Count - 1; i >= 0; i--)
            {
                if (Knowledge[i] == null || string.IsNullOrEmpty(Knowledge[i].NodeId) || !validIds.Contains(Knowledge[i].NodeId))
                    Knowledge.RemoveAt(i);
            }

            if (Markers == null) Markers = new List<MapMarkerState>();
            var markerIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = Markers.Count - 1; i >= 0; i--)
            {
                var marker = Markers[i];
                if (marker == null || string.IsNullOrEmpty(marker.MarkerId) || !markerIds.Add(marker.MarkerId))
                    Markers.RemoveAt(i);
            }

            var knowledgeMap = new Dictionary<string, MapNodeKnowledgeState>(StringComparer.Ordinal);
            for (int i = 0; i < Knowledge.Count; i++)
            {
                if (!knowledgeMap.ContainsKey(Knowledge[i].NodeId))
                    knowledgeMap[Knowledge[i].NodeId] = Knowledge[i];
            }

            // Legacy save migration: any discovered node without knowledge defaults to Visited
            for (int i = 0; i < Discovered.Count; i++)
            {
                string discId = Discovered[i];
                if (!knowledgeMap.ContainsKey(discId))
                {
                    var legacyRec = new MapNodeKnowledgeState
                    {
                        NodeId = discId,
                        FogState = MapFogState.Visited,
                        LastConfirmedDay = 1,
                        Provenance = new CampaignProvenanceRecord(
                            KnowledgeSourceKind.ExpeditionVisit,
                            "legacy_save",
                            "expedition_system",
                            1,
                            InformationConfidence.Confirmed,
                            discId)
                    };
                    Knowledge.Add(legacyRec);
                    knowledgeMap[discId] = legacyRec;
                }
            }

            // Starting unlocked nodes default to Visited
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].StartingUnlocked)
                {
                    if (!knowledgeMap.ContainsKey(nodes[i].Id))
                    {
                        var startRec = new MapNodeKnowledgeState
                        {
                            NodeId = nodes[i].Id,
                            FogState = MapFogState.Visited,
                            LastConfirmedDay = 1,
                            Provenance = new CampaignProvenanceRecord(
                                KnowledgeSourceKind.ExpeditionVisit,
                                "starting_unlocked",
                                "wasteland_map",
                                1,
                                InformationConfidence.Confirmed,
                                nodes[i].Id)
                        };
                        Knowledge.Add(startRec);
                        knowledgeMap[nodes[i].Id] = startRec;
                    }
                }
            }

            // Synchronize Discovered with Knowledge: any Rumored, Surveyed, or Visited node is Discovered
            for (int i = 0; i < Knowledge.Count; i++)
            {
                var k = Knowledge[i];
                if (k.FogState >= MapFogState.Rumored && !Discovered.Contains(k.NodeId))
                {
                    Discovered.Add(k.NodeId);
                }
            }
        }

        public WastelandMapState Capture() => new WastelandMapState
        {
            Discovered = new List<string>(Discovered),
            Completed = new List<string>(Completed),
            Locked = new List<string>(Locked),
            Unlocked = new List<string>(Unlocked),
            RegisteredMapFragments = new List<string>(RegisteredMapFragments),
            Knowledge = Knowledge != null ? Knowledge.Select(k => k.Clone()).ToList() : new List<MapNodeKnowledgeState>(),
            Markers = Markers != null ? Markers.Select(m => m.Clone()).ToList() : new List<MapMarkerState>(),
            Tunnels = Tunnels != null ? new TunnelNetworkSystem(Tunnels).CaptureState() : new TunnelNetworkState()
        };

        public void RestoreInto(WastelandMapState state, IReadOnlyList<MapNode> nodes)
        {
            Discovered = state.Discovered != null ? new List<string>(state.Discovered) : new List<string>();
            Completed = state.Completed != null ? new List<string>(state.Completed) : new List<string>();
            Locked = state.Locked != null ? new List<string>(state.Locked) : new List<string>();
            Unlocked = state.Unlocked != null ? new List<string>(state.Unlocked) : new List<string>();
            RegisteredMapFragments = state.RegisteredMapFragments != null ? new List<string>(state.RegisteredMapFragments) : new List<string>();
            Knowledge = state.Knowledge != null ? state.Knowledge.Select(k => k.Clone()).ToList() : new List<MapNodeKnowledgeState>();
            Markers = state.Markers != null ? state.Markers.Select(m => m.Clone()).ToList() : new List<MapMarkerState>();
            Tunnels = state.Tunnels != null ? new TunnelNetworkSystem(state.Tunnels).CaptureState() : new TunnelNetworkState();
            NormalizeAndValidate(nodes);
        }
    }

    [Serializable]
    public sealed class MapMarkerState
    {
        public string MarkerId = string.Empty;
        public string Category = string.Empty;
        public string SourceId = string.Empty;
        public string DefinitionId = string.Empty;
        public string TrapType = string.Empty;
        public string LabelKey = string.Empty;
        public string IconKey = string.Empty;
        public string Condition = "healthy";
        public float PositionX;
        public float PositionY;

        public MapMarkerState Clone() => new MapMarkerState
        {
            MarkerId = MarkerId,
            Category = Category,
            SourceId = SourceId,
            DefinitionId = DefinitionId,
            TrapType = TrapType,
            LabelKey = LabelKey,
            IconKey = IconKey,
            Condition = Condition,
            PositionX = PositionX,
            PositionY = PositionY
        };
    }

    [Serializable]
    public sealed class TrapMapMarkerSource
    {
        public string SiteId = string.Empty;
        public string TrapId = string.Empty;
        public string TrapType = string.Empty;
        public float PositionX;
        public float PositionY;
        public bool IsBroken;
    }

    [Serializable]
    public sealed class TrapSiteMapLocation
    {
        public string SiteId = string.Empty;
        public string AnchorNodeId = string.Empty;
        public float OffsetX;
        public float OffsetY;
    }
}
