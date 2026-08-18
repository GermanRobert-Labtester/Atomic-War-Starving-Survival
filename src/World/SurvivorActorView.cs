using Godot;
using Ashfall.Core.Radiation;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp.World
{
    /// <summary>
    /// A single survivor's visual on the shelter interior. Shows name, a health
    /// indicator (green→red), a radiation indicator (blue→red), and a status
    /// indicator driven by survival needs. Presentation only — all state is read
    /// from the authoritative <see cref="SurvivorNeedsState"/> / <see cref="SurvivorRadState"/>
    /// supplied by the host session.
    /// </summary>
    public partial class SurvivorActorView : Node2D
    {
        public string SurvivorId { get; set; }
        public Label Label { get; private set; }
        public Sprite2D Sprite { get; private set; }
        public ColorRect HealthIndicator { get; private set; }
        public ColorRect RadiationIndicator { get; private set; }
        public ColorRect StatusIndicator { get; private set; }

        public SurvivorNeedsState SurvivorState { get; private set; }

        public SurvivorActorView()
        {
            // Create a margin container to space out elements
            var marginContainer = new MarginContainer();
            marginContainer.AddThemeConstantOverride("margin_left", 4);
            marginContainer.AddThemeConstantOverride("margin_top", 4);
            marginContainer.AddThemeConstantOverride("margin_right", 4);
            marginContainer.AddThemeConstantOverride("margin_bottom", 4);
            AddChild(marginContainer);

            // Center container for the survivor actor
            var centerContainer = new CenterContainer();
            marginContainer.AddChild(centerContainer);

            var actorContainer = new Control();
            centerContainer.AddChild(actorContainer);

            Label = new Label();
            Label.HorizontalAlignment = HorizontalAlignment.Center;
            Label.Position = new Vector2(0, -30);

            Sprite = new Sprite2D();
            Sprite.Texture = GD.Load<Texture2D>("res://assets/sprites/Characters/placeholder_survivor.png");
            Sprite.Scale = new Vector2(0.7f, 0.7f);

            // Health indicator (bottom left of sprite)
            HealthIndicator = CreateStatusIndicator(new Color(0.2f, 0.8f, 0.2f, 0.8f));
            HealthIndicator.Position = new Vector2(-25, 25);

            // Radiation indicator (bottom right of sprite)
            RadiationIndicator = CreateStatusIndicator(new Color(0.8f, 0.8f, 0.2f, 0.8f));
            RadiationIndicator.Position = new Vector2(25, 25);

            // Status indicator (above label)
            StatusIndicator = CreateStatusIndicator(new Color(0.2f, 0.6f, 0.8f, 0.8f));
            StatusIndicator.Position = new Vector2(0, -40);

            // Add all elements to the container
            actorContainer.AddChild(Label);
            actorContainer.AddChild(Sprite);
            actorContainer.AddChild(HealthIndicator);
            actorContainer.AddChild(RadiationIndicator);
            actorContainer.AddChild(StatusIndicator);
        }

        private ColorRect CreateStatusIndicator(Color color)
        {
            var indicator = new ColorRect();
            indicator.Size = new Vector2(20, 5);
            indicator.Color = color;
            return indicator;
        }

        /// <summary>
        /// Refresh this actor from the authoritative survivor state. Needs drive
        /// the status pill; radiation drives the bottom-right indicator.
        /// </summary>
        public void UpdateFromSurvivor(SurvivorNeedsState state, SurvivorRadState? rad = null)
        {
            if (state == null)
            {
                Visible = false;
                return;
            }

            SurvivorState = state;
            Visible = true;
            Label.Text = FormatSurvivorName(state.Id);

            // Health indicator (green = healthy, red = critical)
            float healthRatio = Mathf.Clamp(state.Health / 100f, 0f, 1f);
            HealthIndicator.Color = new Color(
                Mathf.Lerp(0.8f, 0.2f, healthRatio),
                Mathf.Lerp(0.2f, 0.8f, healthRatio),
                0.2f,
                0.8f
            );

            // Radiation indicator (blue = low, red = high) — RadiationDose is 0..100
            float radiationRatio = rad != null ? Mathf.Clamp(rad.RadiationDose / 100f, 0f, 1f) : 0f;
            RadiationIndicator.Color = new Color(
                Mathf.Lerp(0.2f, 0.8f, radiationRatio),
                Mathf.Lerp(0.8f, 0.2f, radiationRatio),
                Mathf.Lerp(0.8f, 0.2f, radiationRatio),
                0.8f
            );

            // Status indicator based on needs
            UpdateStatusIndicator();
        }

        private void UpdateStatusIndicator()
        {
            if (SurvivorState == null)
                return;

            // Simple status based on needs
            if (SurvivorState.Hunger > 70 || SurvivorState.Thirst > 70)
            {
                StatusIndicator.Color = new Color(0.8f, 0.6f, 0.2f, 0.8f); // Yellow - hungry/thirsty
            }
            else if (SurvivorState.Fatigue > 70)
            {
                StatusIndicator.Color = new Color(0.2f, 0.4f, 0.8f, 0.8f); // Blue - tired
            }
            else if (SurvivorState.Warmth < 30)
            {
                StatusIndicator.Color = new Color(0.4f, 0.2f, 0.8f, 0.8f); // Purple - cold
            }
            else
            {
                StatusIndicator.Color = new Color(0.2f, 0.8f, 0.6f, 0.8f); // Green - okay
            }
        }

        private string FormatSurvivorName(string id)
        {
            if (string.IsNullOrEmpty(id)) return "Unknown";
            return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(id.Replace('_', ' '));
        }
    }
}
