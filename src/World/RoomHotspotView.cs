using Godot;

#pragma warning disable CS8618
namespace AtomicWar.GodotApp.World
{
    public partial class RoomHotspotView : Node2D
    {
        [Signal]
        public delegate void ClickedEventHandler(string roomId);

        public string RoomId { get; set; }
        public Label Label { get; private set; }
        public Area2D HotspotArea { get; private set; }

        public RoomHotspotView()
        {
            Label = new Label();
            AddChild(Label);

            HotspotArea = new Area2D();
            var collisionShape = new CollisionShape2D();
            collisionShape.Shape = new RectangleShape2D { Size = new Vector2(100, 50) };
            HotspotArea.AddChild(collisionShape);
            HotspotArea.InputEvent += OnInputEvent;
            AddChild(HotspotArea);
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