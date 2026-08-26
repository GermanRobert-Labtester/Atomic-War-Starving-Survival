using System;
using System.Collections.Generic;
using Godot;
using AtomicWar.GodotApp.UI;
using Ashfall.Core.UI;
using Ashfall.Core.YearOfAsh;

namespace AtomicWar.GodotApp.YearOfAsh
{
    /// <summary>
    /// Godot host surface for Year of Ash questlines (Days 180–360).
    ///
    /// Two modes on one panel: an offer list of questlines that can actually be started,
    /// and a stage view with the current narrative prompt and its choices. Thin
    /// presentation node — every decision goes back to <see cref="QuestlineSystem"/> via
    /// events; this file holds no questline rules.
    ///
    /// Only <see cref="QuestlineSystem.GetPlayableQuestlines"/> is ever offered. The JSON
    /// catalog carries objectives with no choices, so those questlines would strand the
    /// player in a permanently Active record; the withheld count is shown instead of
    /// quietly dropping them.
    /// </summary>
    public partial class QuestlineModal : Control
    {
        private PanelContainer _panel = null!;
        private Label _titleLabel = null!;
        private Label _subtitleLabel = null!;
        private RichTextLabel _bodyText = null!;
        private VBoxContainer _choicesContainer = null!;
        private Label _withheldLabel = null!;
        private Button _closeButton = null!;

        /// <summary>Player asked to begin this questline.</summary>
        public event Action<QuestlineDefinition>? OnQuestlineChosen;

        /// <summary>Player took <c>choiceId</c> in the active questline.</summary>
        public event Action<string, string>? OnChoiceTaken;

        public event Action? OnModalClosed;

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
                Color = AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.InkPanel)
            };
            backdrop.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(backdrop);

            _panel = new PanelContainer
            {
                CustomMinimumSize = new Vector2(760, 580)
            };
            _panel.SetAnchorsPreset(LayoutPreset.Center);
            AddChild(_panel);

            var margin = new MarginContainer();
            margin.AddThemeConstantOverride("margin_left", 24);
            margin.AddThemeConstantOverride("margin_top", 24);
            margin.AddThemeConstantOverride("margin_right", 24);
            margin.AddThemeConstantOverride("margin_bottom", 24);
            _panel.AddChild(margin);

            var rootBox = new VBoxContainer();
            rootBox.AddThemeConstantOverride("separation", 14);
            margin.AddChild(rootBox);

            _titleLabel = new Label { Text = "QUESTLINES" };
            _titleLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeH2);
            _titleLabel.AddThemeColorOverride("font_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
            rootBox.AddChild(_titleLabel);

            _subtitleLabel = new Label { Text = string.Empty };
            _subtitleLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            _subtitleLabel.AddThemeColorOverride("font_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
            rootBox.AddChild(_subtitleLabel);

            rootBox.AddChild(new HSeparator());

            _bodyText = new RichTextLabel
            {
                BbcodeEnabled = true,
                FitContent = true,
                CustomMinimumSize = new Vector2(0, 120)
            };
            _bodyText.AddThemeColorOverride("default_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            rootBox.AddChild(_bodyText);

            _choicesContainer = new VBoxContainer();
            _choicesContainer.AddThemeConstantOverride("separation", 8);
            rootBox.AddChild(_choicesContainer);

            _withheldLabel = new Label
            {
                Text = string.Empty,
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _withheldLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
            _withheldLabel.AddThemeColorOverride("font_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
            rootBox.AddChild(_withheldLabel);

            _closeButton = new Button
            {
                Text = "Close the ledger",
                CustomMinimumSize = new Vector2(0, 38)
            };
            _closeButton.Pressed += () =>
            {
                Hide();
                OnModalClosed?.Invoke();
            };
            rootBox.AddChild(_closeButton);
        }

        private void ClearChoices()
        {
            AshfallUiHelpers.EmptyChildren(_choicesContainer);
        }

        /// <summary>
        /// Shows the questlines that can be started on <paramref name="day"/>.
        /// <paramref name="withheld"/> is reported rather than hidden.
        /// </summary>
        public void DisplayOffers(IReadOnlyList<QuestlineDefinition> offers, int day, int withheld)
        {
            _titleLabel.Text = "AVAILABLE QUESTLINES";
            _subtitleLabel.Text = $"Day {day} · {offers.Count} open";
            ClearChoices();

            if (offers.Count == 0)
            {
                _bodyText.Text = "Nothing is open to you today. The year turns anyway.";
            }
            else
            {
                _bodyText.Text = "Work that will take a decision from you before it ends.";
                foreach (var def in offers)
                {
                    var captured = def;
                    var btn = new Button
                    {
                        Text = $"[ {captured.title} ]  {captured.stages.Count} stages · from day {captured.minDay}",
                        CustomMinimumSize = new Vector2(0, 42),
                        Alignment = HorizontalAlignment.Left
                    };
                    btn.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                    btn.Pressed += () => OnQuestlineChosen?.Invoke(captured);
                    _choicesContainer.AddChild(btn);
                }
            }

            _withheldLabel.Text = withheld > 0
                ? $"{withheld} questlines withheld: no authored choices in " +
                  "year_of_ash_quests.json, so they could be started but never finished."
                : string.Empty;

            Show();
        }

        /// <summary>Shows the current stage of an active questline and its choices.</summary>
        public void DisplayStage(QuestlineDefinition def, QuestStage stage, int day)
        {
            if (def == null || stage == null) return;

            _titleLabel.Text = def.title.ToUpperInvariant();
            _subtitleLabel.Text = $"Day {day} · {stage.title}" +
                (string.IsNullOrEmpty(def.factionTag) ? "" : $" · {def.factionTag}");
            _bodyText.Text = string.IsNullOrEmpty(stage.narrativePrompt)
                ? def.synopsis
                : stage.narrativePrompt;

            ClearChoices();
            foreach (var choice in stage.choices)
            {
                var captured = choice;
                var btn = new Button
                {
                    Text = captured.text,
                    CustomMinimumSize = new Vector2(0, 42),
                    Alignment = HorizontalAlignment.Left
                };
                btn.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                btn.Pressed += () => OnChoiceTaken?.Invoke(def.questlineId, captured.choiceId);
                _choicesContainer.AddChild(btn);
            }

            _withheldLabel.Text = string.Empty;
            Show();
        }

        /// <summary>Shows the outcome of a choice, and whether the questline ended.</summary>
        public void DisplayResolution(QuestChoiceResult result, bool questEnded)
        {
            if (result == null) return;

            ClearChoices();

            var narrative = new Label
            {
                Text = string.IsNullOrEmpty(result.outcomeNarrative)
                    ? "It is done."
                    : result.outcomeNarrative,
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            narrative.AddThemeColorOverride("font_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            _choicesContainer.AddChild(narrative);

            var ledger = new Label
            {
                Text = $"Morale {result.moraleDelta:+#;-#;0} · Guilt {result.guiltDelta:+#;-#;0}" +
                       (string.IsNullOrEmpty(result.factionId)
                           ? ""
                           : $" · {result.factionId} {result.factionDelta:+#;-#;0}") +
                       (string.IsNullOrEmpty(result.grantItemId)
                           ? ""
                           : $" · {result.grantItemId} ×{result.grantItemQty}"),
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            ledger.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
            ledger.AddThemeColorOverride("font_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot));
            _choicesContainer.AddChild(ledger);

            _subtitleLabel.Text = questEnded
                ? $"Questline {result.newQuestStatus}."
                : "The questline continues.";
            Show();
        }
    }
}
