using Godot;

namespace AtomicWar.GodotApp.World
{
    public partial class SurvivorActorView : Node2D
    {
        public string SurvivorId { get; set; }
        public Label Label { get; private set; }
        public Sprite2D Sprite { get; private set; }

        public SurvivorActorView()
        {
            Label = new Label();
            AddChild(Label);

            Sprite = new Sprite2D();
            Sprite.Texture = GD.Load<Texture2D>("res://assets/sprites/Characters/placeholder_survivor.png");
            AddChild(Sprite);
        }
    }
}