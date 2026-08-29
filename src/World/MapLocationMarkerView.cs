using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using CoreTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.World
{
    /// <summary>
    /// Lifecycle state of a map location marker on the wasteland surface.
    /// </summary>
    public enum MapLocationMarkerStatus
    {
        /// <summary>Discovered and available for immediate player travel and inspection.</summary>
        Discovered,

        /// <summary>Reachable through an adjacent discovered route; scoutable or selectable.</summary>
        Available,

        /// <summary>Locked due to severe radiation, extreme hazard, or faction blockade.</summary>
        Locked,

        /// <summary>Location has been fully scavenged, cleared, or narrative arc completed.</summary>
        Completed,

        /// <summary>Undiscovered behind fog of war; obscured and not yet reachable.</summary>
        Unavailable
    }

    /// <summary>
    /// ASHFALL — Clickable map node marker.
    /// Displays a location on the wasteland map with distinct visual styling for
    /// discovered, available, locked, completed, and unavailable states.
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
        public string DangerLevel { get; set; } = "none"; // none, low, medium, high, locked

        [Export]
        public MapLocationMarkerStatus Status { get; set; } = MapLocationMarkerStatus.Discovered;

        [Export]
        public string StatusBadge { get; set; } = string.Empty;

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

            // Set up visuals based on status and danger level
            UpdateVisuals();

            // Connect signals
            _hotspotArea.InputEvent += OnInputEvent;
        }

        /// <summary>
        /// Visibly configures marker color, opacity, badge text, and label styling
        /// according to the active <see cref="Status"/> and <see cref="DangerLevel"/>.
        /// </summary>
        public void UpdateVisuals()
        {
            if (_markerSprite == null || _nameLabel == null) return;

            Color baseDangerColor = DangerLevel switch
            {
                "none" => UI.AshfallUiHelpers.ToColor(CoreTheme.Lethe),      // Green - safe
                "low" => UI.AshfallUiHelpers.ToColor(CoreTheme.Warm),       // Yellow - low risk
                "medium" => UI.AshfallUiHelpers.ToColor(CoreTheme.Warm),    // Yellow/Amber - medium risk
                "high" => UI.AshfallUiHelpers.ToColor(CoreTheme.Entropy),   // Orange - high risk
                "locked" => UI.AshfallUiHelpers.ToColor(CoreTheme.Critical),// Red - locked/contested
                _ => UI.AshfallUiHelpers.ToColor(CoreTheme.Lethe)
            };

            switch (Status)
            {
                case MapLocationMarkerStatus.Discovered:
                    _markerSprite.Modulate = baseDangerColor;
                    _markerSprite.SelfModulate = new Color(1f, 1f, 1f, 1.0f);
                    _nameLabel.Text = string.IsNullOrEmpty(StatusBadge) ? DisplayName : $"{DisplayName} [{StatusBadge}]";
                    _nameLabel.Modulate = new Color(1f, 1f, 1f, 1.0f);
                    break;

                case MapLocationMarkerStatus.Available:
                    // Ozone bright highlight indicating reachable open route
                    _markerSprite.Modulate = UI.AshfallUiHelpers.ToColor(CoreTheme.Ozone);
                    _markerSprite.SelfModulate = new Color(1f, 1f, 1f, 0.95f);
                    _nameLabel.Text = $"{DisplayName} [AVAILABLE]";
                    _nameLabel.Modulate = UI.AshfallUiHelpers.ToColor(CoreTheme.Ozone);
                    break;

                case MapLocationMarkerStatus.Locked:
                    // Red critical warning
                    _markerSprite.Modulate = UI.AshfallUiHelpers.ToColor(CoreTheme.Critical);
                    _markerSprite.SelfModulate = new Color(1f, 1f, 1f, 0.9f);
                    _nameLabel.Text = $"{DisplayName} [LOCKED]";
                    _nameLabel.Modulate = UI.AshfallUiHelpers.ToColor(CoreTheme.Critical);
                    break;

                case MapLocationMarkerStatus.Completed:
                    // Muted silver/gray with checkmark
                    _markerSprite.Modulate = UI.AshfallUiHelpers.ToColor(CoreTheme.Muted);
                    _markerSprite.SelfModulate = new Color(1f, 1f, 1f, 0.6f);
                    _nameLabel.Text = $"{DisplayName} [CLEARED ✓]";
                    _nameLabel.Modulate = UI.AshfallUiHelpers.ToColor(CoreTheme.Muted);
                    break;

                case MapLocationMarkerStatus.Unavailable:
                default:
                    // Heavy fog-of-war dimming with obscured title
                    _markerSprite.Modulate = new Color(0.25f, 0.25f, 0.28f, 0.35f);
                    _markerSprite.SelfModulate = new Color(1f, 1f, 1f, 0.35f);
                    _nameLabel.Text = "??? [UNEXPLORED]";
                    _nameLabel.Modulate = new Color(0.5f, 0.5f, 0.5f, 0.4f);
                    break;
            }
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
