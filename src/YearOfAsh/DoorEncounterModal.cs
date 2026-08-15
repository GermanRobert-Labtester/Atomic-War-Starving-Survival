using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.YearOfAsh;

namespace AtomicWar.GodotApp.YearOfAsh
{
    /// <summary>
    /// Godot 4.7+ UI Modal for Bunker Hatch / Door Encounters (Days 180 to 360).
    /// Displays visitor information, dynamic choices with resource/trait requirements,
    /// and previews/displays real-time psychological reactions from living bunker survivors.
    /// Thin presentation node; zero simulation logic.
    /// </summary>
    public partial class DoorEncounterModal : Control
    {
        private PanelContainer _panel = null!;
        private Label _titleLabel = null!;
        private Label _factionLabel = null!;
        private RichTextLabel _descriptionText = null!;
        private VBoxContainer _choicesContainer = null!;
        private VBoxContainer _reactionsContainer = null!;
        private Button _closeButton = null!;

        public event Action<DoorEncounterEntry, EncounterChoice> OnChoiceClicked;
        public event Action OnModalClosed;

        public bool IsOpen => Visible;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildInterface();
            Hide();
        }

        private void BuildInterface()
        {
            var backdrop = new ColorRect
            {
                Color = new Color(0.02f, 0.03f, 0.04f, 0.85f)
            };
            backdrop.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(backdrop);

            _panel = new PanelContainer
            {
                CustomMinimumSize = new Vector2(720, 560)
            };
            _panel.SetAnchorsPreset(LayoutPreset.Center);
            AddChild(_panel);

            var rootMargin = new MarginContainer();
            rootMargin.AddThemeConstantOverride("margin_left", 24);
            rootMargin.AddThemeConstantOverride("margin_top", 24);
            rootMargin.AddThemeConstantOverride("margin_right", 24);
            rootMargin.AddThemeConstantOverride("margin_bottom", 24);
            _panel.AddChild(rootMargin);

            var rootBox = new VBoxContainer();
            rootBox.AddThemeConstantOverride("separation", 14);
            rootMargin.AddChild(rootBox);

            _titleLabel = new Label
            {
                Text = "SHELTER DOOR ENCOUNTER",
                HorizontalAlignment = HorizontalAlignment.Left
            };
            _titleLabel.AddThemeFontSizeOverride("font_size", 22);
            _titleLabel.AddThemeColorOverride("font_color", new Color(0.95f, 0.65f, 0.25f));
            rootBox.AddChild(_titleLabel);

            _factionLabel = new Label
            {
                Text = "Visitor Affiliation: Unknown",
                HorizontalAlignment = HorizontalAlignment.Left
            };
            _factionLabel.AddThemeFontSizeOverride("font_size", 14);
            _factionLabel.AddThemeColorOverride("font_color", new Color(0.6f, 0.75f, 0.7f));
            rootBox.AddChild(_factionLabel);

            rootBox.AddChild(new HSeparator());

            _descriptionText = new RichTextLabel
            {
                BbcodeEnabled = true,
                FitContent = true,
                CustomMinimumSize = new Vector2(0, 100)
            };
            _descriptionText.AddThemeColorOverride("default_color", new Color(0.88f, 0.9f, 0.88f));
            rootBox.AddChild(_descriptionText);

            var choicesHeader = new Label
            {
                Text = "AVAILABLE ACTIONS & THRESHOLD PROTOCOLS"
            };
            choicesHeader.AddThemeFontSizeOverride("font_size", 14);
            choicesHeader.AddThemeColorOverride("font_color", new Color(0.4f, 0.8f, 0.9f));
            rootBox.AddChild(choicesHeader);

            _choicesContainer = new VBoxContainer();
            _choicesContainer.AddThemeConstantOverride("separation", 8);
            rootBox.AddChild(_choicesContainer);

            _reactionsContainer = new VBoxContainer();
            _reactionsContainer.AddThemeConstantOverride("separation", 6);
            rootBox.AddChild(_reactionsContainer);

            _closeButton = new Button
            {
                Text = "Seal Outer Hatch & Return to Bunker",
                CustomMinimumSize = new Vector2(0, 38)
            };
            _closeButton.Pressed += () =>
            {
                Hide();
                OnModalClosed?.Invoke();
            };
            rootBox.AddChild(_closeButton);
        }

        public void DisplayEncounter(
            DoorEncounterEntry encounter,
            IReadOnlyList<SurvivorOccupantSnapshot> roster)
        {
            if (encounter == null) return;

            _titleLabel.Text = $"HATCH ENCOUNTER: {encounter.visitorName.ToUpperInvariant()}";
            _factionLabel.Text = $"Faction Alignment: {encounter.visitorFaction} | Threat Level: {encounter.threatLevel}";
            _descriptionText.Text = encounter.description;

            foreach (Node child in _choicesContainer.GetChildren())
                child.QueueFree();
            foreach (Node child in _reactionsContainer.GetChildren())
                child.QueueFree();

            foreach (var choice in encounter.choices)
            {
                var btn = new Button
                {
                    Text = $"[ {choice.choiceId} ] {choice.text}",
                    CustomMinimumSize = new Vector2(0, 42),
                    Alignment = HorizontalAlignment.Left
                };
                btn.AddThemeFontSizeOverride("font_size", 13);
                btn.Pressed += () => OnChoiceClicked?.Invoke(encounter, choice);
                _choicesContainer.AddChild(btn);
            }

            Show();
        }

        public void DisplayResolution(EncounterResolutionResult result)
        {
            if (result == null) return;

            foreach (Node child in _choicesContainer.GetChildren())
                child.QueueFree();

            var summaryLabel = new Label
            {
                Text = $"RESOLUTION: {result.outcomeText}\nNet Morale Delta: {result.netMoraleDelta:+#;-#;0} | Net Guilt Delta: {result.netGuiltDelta:+#;-#;0}",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            summaryLabel.AddThemeColorOverride("font_color", new Color(0.95f, 0.85f, 0.4f));
            _choicesContainer.AddChild(summaryLabel);

            foreach (var rx in result.survivorReactions)
            {
                var rxLabel = new Label
                {
                    Text = $" • {rx.survivorName}: \"{rx.dialogueReaction}\" (Morale: {rx.moraleDelta:+#;-#;0}, Guilt: {rx.guiltDelta:+#;-#;0})",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                rxLabel.AddThemeFontSizeOverride("font_size", 12);
                rxLabel.AddThemeColorOverride("font_color", new Color(0.7f, 0.8f, 0.75f));
                _reactionsContainer.AddChild(rxLabel);
            }
        }
    }
}
