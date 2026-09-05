// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Moral Choice Decision Modal ("The Weight of Survival").
    /// Surfaces authored moral dilemmas, narrative encounters, and irrevocable tactical options
    /// to the player without exposing underlying numeric moral/empathy metrics.
    /// Implements <see cref="IModalPanel"/> for focus management and keyboard handling.
    /// </summary>
    public partial class MoralChoiceModal : Control, IModalPanel
    {
        public event Action<string, int>? OnChoiceSelected;
        public event Action? OnClose;
        public event Action? OnModalClosed;

        public bool IsModalOpen => Visible;
        public Control? InitialFocusControl => _firstInteractiveButton ?? _closeButton;

        private Label _titleLabel = null!;
        private Label _subtitleLabel = null!;
        private VBoxContainer _encounterContainer = null!;
        private VBoxContainer _choicesContainer = null!;
        private VBoxContainer _feedbackContainer = null!;
        private Button _closeButton = null!;
        private Control? _firstInteractiveButton;

        private MoralChoiceQuestDefinition? _currentQuest;
        private MoralChoiceSystem? _moralChoiceSystem;
        private Action<string, int>? _onChoiceCallback;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildLayout();
            Visible = false;
        }

        private void BuildLayout()
        {
            AshfallUiHelpers.EmptyChildren(this);

            // Dark semi-transparent scrim backdrop
            var scrim = new ColorRect
            {
                Color = new Color(0.02f, 0.02f, 0.04f, 0.88f)
            };
            scrim.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(scrim);

            // Center dialog container (max width 1100, centered)
            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var panelCard = AshfallUiHelpers.MakeCardFrame("THE WEIGHT OF SURVIVAL", "ETHICAL DIRECTIVE & TACTICAL CHOICE");
            panelCard.CustomMinimumSize = new Vector2(1040, 680);
            center.AddChild(panelCard);

            var margin = panelCard.GetChild<MarginContainer>(0);
            var mainVBox = margin.GetChild<VBoxContainer>(0);

            // Title & category header
            _titleLabel = AshfallUiHelpers.MakeTitle("MORAL DILEMMA // UNRESOLVED");
            _titleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
            mainVBox.AddChild(_titleLabel);

            _subtitleLabel = AshfallUiHelpers.MakeSmall("CATEGORY: UNKNOWN · LOCATION: GENERAL SECTOR");
            _subtitleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            mainVBox.AddChild(_subtitleLabel);

            mainVBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Scrollable central content
            var scroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(980, 440),
                SizeFlagsVertical = SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
            };
            mainVBox.AddChild(scroll);

            var scrollContent = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            scrollContent.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(scrollContent);

            _encounterContainer = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
            scrollContent.AddChild(_encounterContainer);

            _choicesContainer = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
            scrollContent.AddChild(_choicesContainer);

            _feedbackContainer = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
            scrollContent.AddChild(_feedbackContainer);

            mainVBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Bottom bar with close/return
            var bottomBar = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingMd);
            bottomBar.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            _closeButton = AshfallUiHelpers.MakeButton("RETURN TO OVERVIEW // [ESC]", () => CloseModal());
            _closeButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            bottomBar.AddChild(_closeButton);

            mainVBox.AddChild(bottomBar);
        }

        public void Bind(
            MoralChoiceQuestDefinition quest,
            MoralChoiceSystem? moralChoiceSystem = null,
            Action<string, int>? onChoiceCallback = null)
        {
            _currentQuest = quest ?? throw new ArgumentNullException(nameof(quest));
            _moralChoiceSystem = moralChoiceSystem;
            _onChoiceCallback = onChoiceCallback;
            _firstInteractiveButton = null;

            RefreshContent();
        }

        public void RefreshContent()
        {
            if (_currentQuest == null) return;

            bool isResolved = _moralChoiceSystem?.IsResolved(_currentQuest.Id) ?? false;
            MoralChoiceResolution? resolution = null;
            _moralChoiceSystem?.TryGetResolution(_currentQuest.Id, out resolution);

            // Header titles
            string statusTag = isResolved ? "RESOLVED & RECORDED" : "TACTICAL ACTION REQUIRED";
            _titleLabel.Text = $"ETHICAL PROTOCOL // {_currentQuest.DisplayName.ToUpperInvariant()}";
            _subtitleLabel.Text = $"CATEGORY: {_currentQuest.Category.ToUpperInvariant()} · STATUS: {statusTag} · LOCATION: {(string.IsNullOrEmpty(_currentQuest.LocationId) ? "SECTOR PERIMETER" : _currentQuest.LocationId)}";

            // 1. Encounter / Narrative briefing
            AshfallUiHelpers.EmptyChildren(_encounterContainer);
            var encounterCard = AshfallUiHelpers.MakeCardFrame("FIELD ENCOUNTER DOSSIER", _currentQuest.Category.ToUpperInvariant());
            var encBox = encounterCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            if (!string.IsNullOrWhiteSpace(_currentQuest.Trigger))
            {
                var trigLabel = AshfallUiHelpers.MakeBody($"► SITUATION: {_currentQuest.Trigger}");
                trigLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                encBox.AddChild(trigLabel);
                encBox.AddChild(AshfallUiHelpers.MakeSeparator());
            }

            string encounterText = !string.IsNullOrWhiteSpace(_currentQuest.Discovery)
                ? _currentQuest.Discovery
                : "A critical dilemma confronts the shelter cohort. Survival calculations require immediate leadership action.";

            var bodyLbl = AshfallUiHelpers.MakeBody(encounterText);
            encBox.AddChild(bodyLbl);
            _encounterContainer.AddChild(encounterCard);

            // 2. Choices section
            AshfallUiHelpers.EmptyChildren(_choicesContainer);
            var choicesCard = AshfallUiHelpers.MakeCardFrame("AUTHORITATIVE DECISION GATES", isResolved ? "RESOLUTION RECORDED" : "SELECT ACTION");
            var chBox = choicesCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            if (!isResolved)
            {
                var warnNotice = AshfallUiHelpers.MakeSmall("ATTENTION: Ethical choices permanently alter survivor morale, camp chatter, and regional branch viability. Once committed, a choice cannot be rescinded.");
                warnNotice.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warning));
                chBox.AddChild(warnNotice);
                chBox.AddChild(AshfallUiHelpers.MakeSeparator());
            }

            for (int i = 0; i < _currentQuest.Choices.Count; i++)
            {
                int choiceIndex = i;
                var opt = _currentQuest.Choices[i];
                bool wasChosen = isResolved && resolution != null && resolution.choiceIndex == i;

                var optBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);

                if (isResolved)
                {
                    if (wasChosen)
                    {
                        var row = AshfallUiHelpers.MakeDataRow($"[COMMITTED RESOLUTION] Option {i + 1}", opt.Label, AshfallUiHelpers.ToColor(DesignTheme.Hot));
                        optBox.AddChild(row);

                        if (!string.IsNullOrEmpty(opt.OutcomeText))
                        {
                            var outLbl = AshfallUiHelpers.MakeSmall($"Consequence: {opt.OutcomeText}");
                            outLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                            optBox.AddChild(outLbl);
                        }

                        if (!string.IsNullOrEmpty(opt.Epitaph))
                        {
                            var epiLbl = AshfallUiHelpers.MakeSmall($"Camp Chronicle: \"{opt.Epitaph}\"");
                            epiLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
                            optBox.AddChild(epiLbl);
                        }
                    }
                    else
                    {
                        var row = AshfallUiHelpers.MakeDataRow($"[UNSELECTED] Option {i + 1}", opt.Label, AshfallUiHelpers.ToColor(DesignTheme.Dim));
                        optBox.AddChild(row);
                    }
                }
                else
                {
                    // Active unresolved option: interactive button without exposing numeric scores
                    var btn = AshfallUiHelpers.MakeButton($"[{i + 1}] COMMIT PATH // {opt.Label.ToUpperInvariant()}", () =>
                    {
                        ExecuteChoice(choiceIndex);
                    });
                    btn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    optBox.AddChild(btn);

                    if (_firstInteractiveButton == null)
                        _firstInteractiveButton = btn;
                }

                chBox.AddChild(optBox);
                if (i < _currentQuest.Choices.Count - 1)
                    chBox.AddChild(AshfallUiHelpers.MakeSeparator());
            }

            _choicesContainer.AddChild(choicesCard);

            // 3. Feedback / consequence strip
            AshfallUiHelpers.EmptyChildren(_feedbackContainer);
            if (isResolved && resolution != null)
            {
                var fbCard = AshfallUiHelpers.MakeCardFrame("RESOLUTION ARCHIVE & CONSEQUENCE RECORD", $"RESOLVED DAY {resolution.resolvedDay}");
                var fbBox = fbCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

                string arrow = resolution.impactMark == "up" ? "🔺 Positive Social Trajectory"
                    : resolution.impactMark == "down" ? "🔻 Hardened Survival Stance" : "⚪ Neutral Pragmatic Shift";

                fbBox.AddChild(AshfallUiHelpers.MakeDataRow("Moral Resonance", arrow, AshfallUiHelpers.ToColor(DesignTheme.Warm)));
                fbBox.AddChild(AshfallUiHelpers.MakeDataRow("Camp Record", resolution.epitaph, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
                fbBox.AddChild(AshfallUiHelpers.MakeDataRow("Journal Status", "Archived to permanent Holdfast survival chronicle.", AshfallUiHelpers.ToColor(DesignTheme.Pale)));

                _feedbackContainer.AddChild(fbCard);
            }
        }

        private void ExecuteChoice(int choiceIndex)
        {
            if (_currentQuest == null) return;
            string questId = _currentQuest.Id;

            // Prefer Bind callback when present so host paths cannot double-resolve
            // via both OnChoiceSelected and the Bind delegate.
            if (_onChoiceCallback != null)
                _onChoiceCallback.Invoke(questId, choiceIndex);
            else
                OnChoiceSelected?.Invoke(questId, choiceIndex);

            // Re-render in place
            RefreshContent();
        }

        public void Open()
        {
            Visible = true;
            RefreshContent();
            _firstInteractiveButton?.GrabFocus();
        }

        public void SelectChoiceForTest(int choiceIndex) => ExecuteChoice(choiceIndex);

        public void CloseModal()
        {
            Visible = false;
            OnModalClosed?.Invoke();
            OnClose?.Invoke();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;

            if (@event is InputEventKey key && key.Pressed)
            {
                if (key.Keycode == Key.Escape)
                {
                    CloseModal();
                    GetViewport().SetInputAsHandled();
                    return;
                }

                // Keyboard quick-selection for options 1-9 if unresolved
                if (_currentQuest != null && (_moralChoiceSystem == null || !_moralChoiceSystem.IsResolved(_currentQuest.Id)))
                {
                    int number = -1;
                    if (key.Keycode >= Key.Key1 && key.Keycode <= Key.Key9)
                        number = (int)(key.Keycode - Key.Key1);
                    else if (key.Keycode >= Key.Kp1 && key.Keycode <= Key.Kp9)
                        number = (int)(key.Keycode - Key.Kp1);

                    if (number >= 0 && number < _currentQuest.Choices.Count)
                    {
                        ExecuteChoice(number);
                        GetViewport().SetInputAsHandled();
                    }
                }
            }
        }
    }
}
