// SPDX-License-Identifier: MIT
using Godot;

#pragma warning disable CS8618
namespace AtomicWar.GodotApp.World
{
    public partial class RoomHotspotView : Node2D
    {
        [Signal]
        public delegate void ClickedEventHandler(string roomId);

        public string RoomId
        {
            get => _roomId;
            set
            {
                if (_roomId != value)
                {
                    _roomId = value;
                    UpdateIcon();
                }
            }
        }
        private string _roomId = string.Empty;
        public Label Label { get; private set; }
        public Area2D HotspotArea { get; private set; }
        public ColorRect Background { get; private set; }
        public Sprite2D Icon { get; private set; }

        private string _displayName = string.Empty;
        private string _statusText = string.Empty;
        private int _occupantCount = 0;

        public RoomHotspotView()
        {
            // Placeholder room prop pictogram (absent art simply leaves the badge).
            Icon = new Sprite2D
            {
                Position = new Vector2(0, -60),
                Scale = new Vector2(0.5f, 0.5f)
            };
            AddChild(Icon);

            // Semi-transparent background badge
            Background = new ColorRect
            {
                Size = new Vector2(130, 44),
                Position = new Vector2(-65, -22),
                Color = new Color(0.08f, 0.1f, 0.12f, 0.75f)
            };
            AddChild(Background);

            Label = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Size = new Vector2(130, 44),
                Position = new Vector2(-65, -22)
            };
            Label.AddThemeFontSizeOverride("font_size", 12);
            AddChild(Label);

            HotspotArea = new Area2D();
            var collisionShape = new CollisionShape2D();
            collisionShape.Shape = new RectangleShape2D { Size = new Vector2(130, 44) };
            HotspotArea.AddChild(collisionShape);
            HotspotArea.InputEvent += OnInputEvent;
            HotspotArea.MouseEntered += OnMouseEntered;
            HotspotArea.MouseExited += OnMouseExited;
            AddChild(HotspotArea);
        }

        public void SetRoomInfo(string displayName, string statusText, int occupantCount)
        {
            _displayName = displayName;
            _statusText = statusText;
            _occupantCount = occupantCount;
            UpdateDisplay();
        }

        /// <summary>Resolves the placeholder room pictogram, null-safe when art is absent.</summary>
        private void UpdateIcon()
        {
            if (Icon == null) return;
            Icon.Texture = string.IsNullOrEmpty(_roomId)
                ? null
                : HoldfastInteriorView.TryLoadShelterTexture(HoldfastInteriorView.ShelterArtDir + _roomId + ".png");
        }

        private void UpdateDisplay()
        {
            string countStr = _occupantCount > 0 ? $" ({_occupantCount})" : "";
            Label.Text = $"{_displayName}{countStr}";
            Label.TooltipText = string.IsNullOrEmpty(_statusText)
                ? $"{_displayName} — {_occupantCount} dweller(s)"
                : $"{_displayName} — {_occupantCount} dweller(s)\n{_statusText}";
        }

        private void OnMouseEntered()
        {
            Background.Color = new Color(0.18f, 0.25f, 0.32f, 0.9f);
            Label.AddThemeColorOverride("font_color", new Color(0.95f, 0.85f, 0.4f));
        }

        private void OnMouseExited()
        {
            Background.Color = new Color(0.08f, 0.1f, 0.12f, 0.75f);
            Label.RemoveThemeColorOverride("font_color");
        }

        private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
        {
            if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
            {
                EmitSignal(SignalName.Clicked, RoomId);
            }
        }
    }
}
