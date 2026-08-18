using Godot;
using System;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.UI;
using System.Text.Json;
using System.Collections.Generic;

namespace AtomicWar.GodotApp.World
{
    /// <summary>
    /// ASHFALL — Wasteland Map View Controller.
    /// Manages the wasteland map scene, loads node data from JSON, and handles interactions.
    /// </summary>
    public partial class WastelandMapView : Node2D
    {
        [Signal]
        public delegate void NodeSelectedEventHandler(string nodeId);

        private PackedScene _markerScene = null!;
        private Node2D _mapNodesContainer = null!;
        private WorldHostSession? _worldHost;

        public override void _Ready()
        {
            _mapNodesContainer = GetNode<Node2D>("MapNodes");
            
            // Load the marker scene
            _markerScene = GD.Load<PackedScene>("res://src/World/MapLocationMarkerView.tscn");
            
            Initialize();
        }

        public void Bind(WorldHostSession? worldHost)
        {
            _worldHost = worldHost;
        }

        public void Initialize()
        {
            GD.Print("WastelandMapView: Initializing map nodes...");
            
            try
            {
                // Load the wasteland map data
                var mapData = LoadWastelandMapData();
                
                if (mapData != null && mapData.Nodes != null)
                {
                    GD.Print($"WastelandMapView: Loaded {mapData.Nodes.Count} nodes");
                    
                    // Create markers for each node
                    foreach (var node in mapData.Nodes)
                    {
                        CreateLocationMarker(node);
                    }
                }
                else
                {
                    GD.PrintErr("WastelandMapView: Failed to load map data or no nodes found");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"WastelandMapView: Error initializing: {ex.Message}");
            }
        }

        private WastelandMapData? LoadWastelandMapData()
        {
            // Try to load from StreamingAssets first
            string jsonPath = "res://Assets/StreamingAssets/Data/wasteland_map_v1.json";
            
            try
            {
                var file = FileAccess.Open(jsonPath, FileAccess.ModeFlags.Read);
                string json = file.GetAsText();
                file.Close();
                
                return JsonSerializer.Deserialize<WastelandMapData>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                GD.PrintErr($"WastelandMapView: Failed to load map data from {jsonPath}: {ex.Message}");
                return null;
            }
        }

        private void CreateLocationMarker(WastelandMapNode node)
        {
            try
            {
                // Instantiate the marker scene
                var marker = _markerScene.Instantiate<MapLocationMarkerView>();
                marker.NodeId = node.Id;
                marker.DisplayName = node.DisplayName;
                marker.DangerLevel = node.Danger;
                marker.PositionOffset = new Vector2(0, -30); // Position label above marker
                marker.SetPosition(new Vector2(node.PositionX, node.PositionY));
                
                // Connect the signal
                marker.NodeSelected += OnNodeSelected;
                
                // Add to container
                _mapNodesContainer.AddChild(marker);
                
                GD.Print($"WastelandMapView: Created marker for {node.DisplayName} at ({node.PositionX}, {node.PositionY})");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"WastelandMapView: Failed to create marker for {node.Id}: {ex.Message}");
            }
        }

        private void OnNodeSelected(string nodeId)
        {
            GD.Print($"WastelandMapView: Node selected: {nodeId}");
            EmitSignal(SignalName.NodeSelected, nodeId);
        }
    }

    /// <summary>
    /// Data structure for wasteland map nodes
    /// </summary>
    public class WastelandMapNode
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Danger { get; set; } = "none"; // none, low, high, locked
        public string? Faction { get; set; }
        public string? LootTable { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }

    /// <summary>
    /// Root data structure for wasteland map
    /// </summary>
    public class WastelandMapData
    {
        public int SchemaVersion { get; set; }
        public List<WastelandMapNode> Nodes { get; set; } = new List<WastelandMapNode>();
    }
}