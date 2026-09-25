// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Presentation-only modal for the typed Plan 143 arc event surface.
    /// It emits event and choice IDs; the host resolves them through the Core
    /// arc system and displays the resulting feedback.
    /// </summary>
    public partial class NarrativeArcModal : Control
    {
        private PanelContainer _panel = null!;
        private Label _titleLabel = null!;
        private Label _subtitleLabel = null!;
        private RichTextLabel _bodyLabel = null!;
        private VBoxContainer _choices = null!;

        public event Action<string, string>? OnChoiceSelected;
        public event Action<string>? OnAcknowledged;

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
                Color = AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.InkPanel)
            };
            backdrop.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(backdrop);

            _panel = new PanelContainer { CustomMinimumSize = new Vector2(820, 560) };
            _panel.SetAnchorsPreset(LayoutPreset.Center);
            AddChild(_panel);

            // UI/UX wave: the arc sheet is a movable window with persisted
            // position (drag the sheet; buttons still receive their clicks).
            UiPanelFlow.AttachDrag(_panel, _panel, "narrative_arc_modal");

            var margin = new MarginContainer();
            margin.AddThemeConstantOverride("margin_left", 26);
            margin.AddThemeConstantOverride("margin_top", 24);
            margin.AddThemeConstantOverride("margin_right", 26);
            margin.AddThemeConstantOverride("margin_bottom", 24);
            _panel.AddChild(margin);

            var root = new VBoxContainer();
            root.AddThemeConstantOverride("separation", 12);
            margin.AddChild(root);

            _titleLabel = new Label { Text = "NARRATIVE ARC" };
            _titleLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeH2);
            _titleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
            root.AddChild(_titleLabel);

            _subtitleLabel = new Label { Text = string.Empty };
            _subtitleLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            _subtitleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
            root.AddChild(_subtitleLabel);
            root.AddChild(new HSeparator());

            _bodyLabel = new RichTextLabel
            {
                BbcodeEnabled = true,
                FitContent = true,
                CustomMinimumSize = new Vector2(0, 170)
            };
            _bodyLabel.AddThemeColorOverride("default_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            root.AddChild(_bodyLabel);

            _choices = new VBoxContainer();
            _choices.AddThemeConstantOverride("separation", 8);
            root.AddChild(_choices);

            var close = new Button
            {
                Text = "Close the account",
                CustomMinimumSize = new Vector2(0, 38)
            };
            close.Pressed += Hide;
            root.AddChild(close);
        }

        public void Display(NarrativeArcEventDefinition definition, int day)
        {
            if (definition == null) return;

            _titleLabel.Text = definition.Title.ToUpperInvariant();
            _subtitleLabel.Text = definition.ArcId.Length == 0
                ? $"Day {day} · independent event"
                : $"Day {day} · {definition.RequiredSurvivorId} · stage {definition.Stage}";
            _bodyLabel.Text = definition.BodyText;
            AshfallUiHelpers.EmptyChildren(_choices);

            if (definition.Choices.Count == 0)
            {
                var acknowledge = new Button
                {
                    Text = "Acknowledge and record",
                    CustomMinimumSize = new Vector2(0, 44)
                };
                acknowledge.Pressed += () => OnAcknowledged?.Invoke(definition.Id);
                _choices.AddChild(acknowledge);
            }
            else
            {
                for (int i = 0; i < definition.Choices.Count; i++)
                {
                    var choice = definition.Choices[i];
                    var captured = choice;
                    var button = new Button
                    {
                        Text = captured.IsExecutable
                            ? captured.Text
                            : $"{captured.Text} [unavailable: {captured.ValidationError}]",
                        Disabled = !captured.IsExecutable,
                        CustomMinimumSize = new Vector2(0, 44),
                        Alignment = HorizontalAlignment.Left
                    };
                    button.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                    button.Pressed += () => OnChoiceSelected?.Invoke(definition.Id, captured.ChoiceId);
                    _choices.AddChild(button);
                }
            }

            Show();
        }

        public void DisplayOutcome(string message)
        {
            AshfallUiHelpers.EmptyChildren(_choices);
            _subtitleLabel.Text = "Recorded in the campaign ledger.";
            _bodyLabel.Text = message;
            Show();
        }
    }
}
