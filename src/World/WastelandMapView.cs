using AtomicWar.GodotApp.UI;
using Godot;
using System;
using System.Collections.Generic;
using Ashfall.Core.World;
using AtomicWar.GodotApp.Localization;

namespace AtomicWar.GodotApp.World
{
    /// <summary>
    /// ASHFALL — Wasteland Map View Controller.
    /// Manages the wasteland map scene, renders node markers with live discovered/available/locked/completed/unavailable
    /// status from authoritative <see cref="WastelandMapSystem"/> read model, and handles interactions.
    /// </summary>
    public partial class WastelandMapView : Node2D
    {
        [Signal]
        public delegate void NodeSelectedEventHandler(string nodeId);

        private PackedScene _markerScene = null!;
        private Node2D _mapNodesContainer = null!;
        private WorldHostSession? _worldHost;
        private WastelandMapSystem? _mapSystem;

        public override void _Ready()
        {
            _mapNodesContainer = GetNode<Node2D>("MapNodes");
            _markerScene = GD.Load<PackedScene>("res://src/World/MapLocationMarkerView.tscn");

            Initialize();
        }

        public void Bind(WorldHostSession? worldHost)
        {
            _worldHost = worldHost;
            Bind(worldHost?.WastelandMap);
        }

        public void Bind(WastelandMapSystem? mapSystem)
        {
            if (_mapSystem != null)
            {
                _mapSystem.OnNodeDiscovered -= HandleMapNodeChanged;
                _mapSystem.OnNodeKnowledgeChanged -= HandleNodeKnowledgeChanged;
                _mapSystem.OnMarkersChanged -= HandleMarkersChanged;
            }
            _mapSystem = mapSystem;
            if (_mapSystem != null)
            {
                _mapSystem.OnNodeDiscovered += HandleMapNodeChanged;
                _mapSystem.OnNodeKnowledgeChanged += HandleNodeKnowledgeChanged;
                _mapSystem.OnMarkersChanged += HandleMarkersChanged;
            }
            if (_mapNodesContainer != null)
            {
                Initialize();
            }
        }

        private void HandleMapNodeChanged(string nodeId)
        {
            Initialize();
        }

        private void HandleNodeKnowledgeChanged(string nodeId, MapFogState fogState)
        {
            Initialize();
        }

        private void HandleMarkersChanged() => Initialize();

        public void Initialize()
        {
            GD.Print("[Ashfall Godot][World] Initializing map nodes...");

            try
            {
                ClearMarkers();

                var nodes = GetMapNodes();
                if (nodes != null && nodes.Count > 0)
                {
                    GD.Print($"[Ashfall Godot][World] Loaded {nodes.Count} map nodes");
                    foreach (var node in nodes)
                    {
                        CreateLocationMarker(node);
                    }
                    CreateTrapMarkers();
                }
                else
                {
                    GD.PrintErr("[Ashfall Godot][World] Failed to load map data or no nodes found");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Ashfall Godot][World] Error initializing: {ex.Message}");
            }
        }

        public MapLocationMarkerStatus ResolveNodeStatus(MapNode node)
        {
            if (node == null) return MapLocationMarkerStatus.Unavailable;

            if (_mapSystem != null)
            {
                var status = _mapSystem.ResolveNodeStatus(node.Id);
                return status switch
                {
                    MapNodeStatusKind.Locked => MapLocationMarkerStatus.Locked,
                    MapNodeStatusKind.Completed => MapLocationMarkerStatus.Completed,
                    MapNodeStatusKind.Discovered => MapLocationMarkerStatus.Discovered,
                    MapNodeStatusKind.Available => MapLocationMarkerStatus.Available,
                    _ => MapLocationMarkerStatus.Unavailable
                };
            }

            // Fallback when no active system: starting unlocked are discovered, discoverable are available
            if (node.Danger == MapNodeDanger.Locked)
                return MapLocationMarkerStatus.Locked;
            if (node.StartingUnlocked)
                return MapLocationMarkerStatus.Discovered;
            if (node.Discoverable)
                return MapLocationMarkerStatus.Available;

            return MapLocationMarkerStatus.Unavailable;
        }

        private IReadOnlyList<MapNode> GetMapNodes()
        {
            if (_mapSystem != null)
                return _mapSystem.Nodes;

            if (_worldHost?.WastelandMap != null)
                return _worldHost.WastelandMap.Nodes;

            return Array.Empty<MapNode>();
        }

        private void ClearMarkers()
        {
            if (_mapNodesContainer == null) return;
            foreach (Node child in _mapNodesContainer.GetChildren())
            {
                if (child is MapLocationMarkerView marker)
                {
                    marker.NodeSelected -= OnNodeSelected;
                }
            }
            AshfallUiHelpers.EmptyChildren(_mapNodesContainer);
        }

        private void CreateLocationMarker(MapNode node)
        {
            try
            {
                var marker = _markerScene.Instantiate<MapLocationMarkerView>();
                marker.NodeId = node.Id;
                marker.DisplayName = node.DisplayName;

                var intel = _mapSystem?.GetNodeIntel(node.Id);
                if (intel != null && intel.FogState == MapFogState.Rumored)
                {
                    marker.PositionOffset = new Vector2(0, -30);
                    marker.SetPosition(new Vector2(intel.PositionX, intel.PositionY));
                    marker.StatusBadge = "RUMOR";
                    marker.DangerLevel = !string.IsNullOrEmpty(intel.DangerBand) ? intel.DangerBand.ToLowerInvariant() : "none";
                    marker.Status = MapLocationMarkerStatus.Discovered;
                }
                else if (intel != null && intel.FogState == MapFogState.Surveyed)
                {
                    marker.PositionOffset = new Vector2(0, -30);
                    marker.SetPosition(new Vector2(intel.PositionX, intel.PositionY));
                    marker.StatusBadge = "SURVEYED";
                    marker.DangerLevel = WorldEscalatedDanger(node, DangerToString(node.Danger));
                    marker.Status = ResolveNodeStatus(node);
                }
                else
                {
                    marker.PositionOffset = new Vector2(0, -30);
                    marker.SetPosition(new Vector2(node.PositionX, node.PositionY));
                    marker.StatusBadge = string.Empty;
                    marker.DangerLevel = WorldEscalatedDanger(node, DangerToString(node.Danger));
                    marker.Status = ResolveNodeStatus(node);
                }

                marker.NodeSelected += OnNodeSelected;
                _mapNodesContainer.AddChild(marker);

                GD.Print($"[Ashfall Godot][World] Created marker for {node.DisplayName} (status={marker.Status}) at ({marker.Position.X}, {marker.Position.Y})");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Ashfall Godot][World] Failed to create marker for {node.Id}: {ex.Message}");
            }
        }

        private void CreateTrapMarkers()
        {
            if (_mapSystem == null) return;
            foreach (var markerState in _mapSystem.Markers)
            {
                if (markerState == null || markerState.Category != "trapping") continue;
                try
                {
                    var marker = _markerScene.Instantiate<MapLocationMarkerView>();
                    marker.NodeId = markerState.MarkerId;
                    marker.DisplayName = AshfallLocalization.Tr(
                        markerState.LabelKey,
                        string.IsNullOrEmpty(markerState.DefinitionId)
                            ? "Known Trap"
                            : markerState.DefinitionId.Replace('_', ' '));
                    marker.PositionOffset = new Vector2(0, -30);
                    marker.SetPosition(new Vector2(markerState.PositionX, markerState.PositionY));
                    marker.DangerLevel = markerState.Condition == "broken" ? "locked" : "none";
                    marker.StatusBadge = markerState.Condition == "broken"
                        ? "TRAP · BROKEN"
                        : "TRAP · HEALTHY";
                    marker.Status = markerState.Condition == "broken"
                        ? MapLocationMarkerStatus.Locked
                        : MapLocationMarkerStatus.Discovered;
                    marker.NodeSelected += OnNodeSelected;
                    _mapNodesContainer.AddChild(marker);
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"[Ashfall Godot][World] Failed to create trap marker {markerState.MarkerId}: {ex.Message}");
                }
            }
        }

        private static string DangerToString(MapNodeDanger danger) => danger switch
        {
            MapNodeDanger.Low => "low",
            MapNodeDanger.Medium => "medium",
            MapNodeDanger.High => "high",
            MapNodeDanger.Locked => "locked",
            _ => "none"
        };

        /// <summary>
        /// Task 122: authored map danger, escalated one step when the live
        /// evolving-world ledger shows threats or ruin at the location. Never
        /// demotes, never crosses into gate semantics ("locked").
        /// </summary>
        private string WorldEscalatedDanger(MapNode node, string authored)
        {
            var rec = _worldHost?.LocationEvolution?.TryGetRecord(node.Id);
            if (rec == null) return authored;
            int steps = (rec.activeThreats.Count > 0 ? 1 : 0) + (rec.isRuined ? 1 : 0);
            if (steps == 0 || authored == "locked") return authored;

            var ladder = new[] { "none", "low", "medium", "high" };
            int at = System.Array.IndexOf(ladder, authored);
            if (at < 0) return authored;
            return ladder[Math.Min(ladder.Length - 1, at + steps)];
        }

        private void OnNodeSelected(string nodeId)
        {
            GD.Print($"[Ashfall Godot][World] Node selected: {nodeId}");
            EmitSignal(SignalName.NodeSelected, nodeId);
        }

        public override void _ExitTree()
        {
            if (_mapSystem != null)
            {
                _mapSystem.OnNodeDiscovered -= HandleMapNodeChanged;
                _mapSystem.OnNodeKnowledgeChanged -= HandleNodeKnowledgeChanged;
                _mapSystem.OnMarkersChanged -= HandleMarkersChanged;
            }
            ClearMarkers();
            base._ExitTree();
        }
    }
}
