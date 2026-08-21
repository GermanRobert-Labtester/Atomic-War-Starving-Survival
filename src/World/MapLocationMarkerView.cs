using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.World
{
    /// <summary>
    /// ASHFALL — Clickable map node marker.
    /// Displays a location on the wasteland map with danger-based visual styling.
    /// Emits NodeSelected signal when clicked.
    /// </summary>
    public partial class MapLocationMarkerView : Node2D
    {
        [Signal]
        public delegate void NodeSelectedEventHandler(string nodeId);

        [Export]
        public string NodeId { get; set; } = string.Empty;

        [Export]
        public string DisplayName { get; set; } = string.Empty;

        [Export]
        public string DangerLevel { get; set; } = "none"; // none, low, high, locked

        [Export]
        public Vector2 PositionOffset { get; set; } = Vector2.Zero;

        private Sprite2D _markerSprite = null!;
        private Label _nameLabel = null!;
        private Area2D _hotspotArea = null!;

        public override void _Ready()
        {
            _markerSprite = GetNode<Sprite2D>("MarkerSprite");
            _nameLabel = GetNode<Label>("NameLabel");
            _hotspotArea = GetNode<Area2D>("HotspotArea");

            // Set up visuals based on danger level
            UpdateVisuals();

            // Connect signals
            _hotspotArea.InputEvent += OnInputEvent;
            _nameLabel.Text = DisplayName;
        }

        private void UpdateVisuals()
        {
            if (_markerSprite == null) return;

            // Set marker color based on danger level
            Color markerColor = DangerLevel switch
            {
                "none" => AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe),      // Green - safe
                "low" => AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm),       // Yellow - low risk
                "high" => AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Entropy),    // Orange - high risk
                "locked" => AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical), // Red - locked/contested
                _ => AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe)
            };

            _markerSprite.Modulate = markerColor;
        }

        private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
        {
            if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
            {
                EmitSignal(SignalName.NodeSelected, NodeId);
            }
        }

        public new void SetPosition(Vector2 worldPosition)
        {
            Position = worldPosition + PositionOffset;
        }
    }
}