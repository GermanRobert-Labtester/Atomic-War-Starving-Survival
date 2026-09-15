// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Onboarding;
using AtomicWar.GodotApp.Localization;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Persisted, accessible hint surface for the first-hour onboarding journey.
    /// Renders the current objective, a checklist of all stages, and the
    /// affordances the spec requires: <em>skip</em>, <em>replay</em>,
    /// <em>show me where</em>, <em>dismiss</em>, and an assistance selector.
    ///
    /// <para>
    /// The panel is always-available reference (no special key required to
    /// close and re-open) and respects the user's reduced-motion preference
    /// by avoiding any visual animation. Every interactive control is
    /// keyboard-focusable with a non-empty label and tooltip, satisfying the
    /// <c>UiAccessibilitySelfTest</c>'s focus / label / close / no-trap
    /// invariants.
    /// </para>
    /// </summary>
    public partial class OnboardingHintPanel : Control
    {
        public event Action<string>? OnShowMeWhereRequested;
        public event Action? OnCurrentStepSkipped;
        public event Action? OnJourneyReplayed;
        public event Action? OnHintDismissed;
        public event Action<OnboardingAssistance>? OnAssistanceChanged;

        private Label _titleLabel = null!;
        private Label _objectiveLabel = null!;
        private Label _hintLabel = null!;
        private Label _assistanceLabel = null!;
        private ScrollContainer _checklistScroll = null!;
        private VBoxContainer _checklist = null!;
        private Button _showBtn = null!;
        private Button _dismissBtn = null!;
        private Button _skipBtn = null!;
        private Button _replayBtn = null!;
        private Button _cycleAssistBtn = null!;
        private Button _closeBtn = null!;

        private OnboardingJourney? _journey = null;

        public OnboardingJourney? Journey => _journey;
        public bool IsOpen => Visible;

        /// <summary>Bind the panel to a journey; the panel does not mutate the
        /// journey, it only renders state and emits requests back to the host.</summary>
        public void Bind(OnboardingJourney journey)
        {
            _journey = journey;
            RefreshView();
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.06f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var panel = new PanelContainer { CustomMinimumSize = new Vector2(620, 0) };
            panel.AddThemeStyleboxOverride("panel",
                new StyleBoxFlat
                {
                    BgColor = AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.SurfaceCard),
                    BorderColor = AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Line),
                    BorderWidthLeft = 1,
                    BorderWidthTop = 1,
                    BorderWidthRight = 1,
                    BorderWidthBottom = 1,
                });
            center.AddChild(panel);

            var margins = new MarginContainer();
            margins.AddThemeConstantOverride("margin_left", 24);
            margins.AddThemeConstantOverride("margin_top", 20);
            margins.AddThemeConstantOverride("margin_right", 24);
            margins.AddThemeConstantOverride("margin_bottom", 20);
            panel.AddChild(margins);

            var vbox = new VBoxContainer();
            vbox.AddThemeConstantOverride("separation", 10);
            margins.AddChild(vbox);

            // ── Header ──
            var header = new HBoxContainer();
            header.AddThemeConstantOverride("separation", 8);
            vbox.AddChild(header);

            _titleLabel = AshfallUiHelpers.MakeTitle(T("onboarding.title", "FIRST-HOUR OBJECTIVE"),
                Ashfall.Core.UI.Theme.FontSizeH2);
            _titleLabel.HorizontalAlignment = HorizontalAlignment.Left;
            _titleLabel.TooltipText = T("onboarding.tooltip.tracker",
                "First-hour checklist. Progress is kept with the save.");
            header.AddChild(_titleLabel);
            _closeBtn = AshfallUiHelpers.MakeButton(T("ui.common.close_short", "CLOSE [Esc]"),
                () => { Visible = false; });
            _closeBtn.TooltipText = T("onboarding.tooltip.close",
                "Close the onboarding hint panel. Your progress is preserved.");
            _closeBtn.CustomMinimumSize = new Vector2(110, 32);
            header.AddChild(_closeBtn);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _assistanceLabel = AshfallUiHelpers.MakeMono(T("onboarding.assistance.standard",
                "ASSISTANCE: STANDARD"));
            _assistanceLabel.TooltipText = T("onboarding.tooltip.assistance",
                "How much guidance each step shows. Toggle with the button below.");
            vbox.AddChild(_assistanceLabel);

            _objectiveLabel = AshfallUiHelpers.MakeBody(T("onboarding.status.booting",
                "Reading the day's objective."));
            _objectiveLabel.TooltipText = T("onboarding.tooltip.objective",
                "Exactly what is required to complete this onboarding step.");
            _objectiveLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            vbox.AddChild(_objectiveLabel);

            _hintLabel = AshfallUiHelpers.MakeMono(T("onboarding.hint.empty", "HINT: —"));
            _hintLabel.TooltipText = T("onboarding.tooltip.hint",
                "A short hint if you stall on this step.");
            _hintLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            vbox.AddChild(_hintLabel);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Action row ──
            var actionRow = new HBoxContainer();
            actionRow.AddThemeConstantOverride("separation", 8);
            vbox.AddChild(actionRow);

            _showBtn = AshfallUiHelpers.MakeButton(T("onboarding.action.show_where", "SHOW ME WHERE"),
                OnShowClicked, false);
            _showBtn.TooltipText = T("onboarding.tooltip.show_where",
                "Open the panel this step needs.");
            _showBtn.CustomMinimumSize = new Vector2(160, 36);
            actionRow.AddChild(_showBtn);

            _skipBtn = AshfallUiHelpers.MakeButton(T("onboarding.action.skip", "SKIP STEP"),
                OnSkipClicked, false);
            _skipBtn.TooltipText = T("onboarding.tooltip.skip",
                "Mark this step known and move on. Real actions are still yours to take when ready.");
            _skipBtn.CustomMinimumSize = new Vector2(120, 36);
            actionRow.AddChild(_skipBtn);

            _dismissBtn = AshfallUiHelpers.MakeButton(T("onboarding.action.dismiss", "DISMISS HINT"),
                OnDismissClicked, false);
            _dismissBtn.TooltipText = T("onboarding.tooltip.dismiss",
                "Hide the contextual hint for this step until you advance.");
            _dismissBtn.CustomMinimumSize = new Vector2(140, 36);
            actionRow.AddChild(_dismissBtn);

            _cycleAssistBtn = AshfallUiHelpers.MakeButton(T("onboarding.assistance.standard",
                "ASSISTANCE: STANDARD"), OnCycleAssistanceClicked, false);
            _cycleAssistBtn.TooltipText = T("onboarding.tooltip.assistance_cycle",
                "Cycle between MINIMAL (objective only), STANDARD (default), GUIDED (extra help).");
            _cycleAssistBtn.CustomMinimumSize = new Vector2(200, 36);
            actionRow.AddChild(_cycleAssistBtn);

            _replayBtn = AshfallUiHelpers.MakeButton(T("onboarding.action.replay", "REPLAY"),
                OnReplayClicked, false);
            _replayBtn.TooltipText = T("onboarding.tooltip.replay",
                "Restart the onboarding hints from Day 1 (does not undo gameplay).");
            _replayBtn.CustomMinimumSize = new Vector2(100, 36);
            actionRow.AddChild(_replayBtn);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            vbox.AddChild(AshfallUiHelpers.MakeSectionHeader(T("onboarding.checklist.title",
                "JOURNEY CHECKLIST")));

            _checklistScroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(560, 220),
                SizeFlagsVertical = Control.SizeFlags.ExpandFill,
            };
            vbox.AddChild(_checklistScroll);

            _checklist = new VBoxContainer();
            _checklist.AddThemeConstantOverride("separation", 6);
            _checklistScroll.AddChild(_checklist);

            RefreshView();
        }

        public void Show()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
        }

        public void RefreshView()
        {
            if (_titleLabel == null) return;

            var j = _journey;
            if (j == null)
            {
                _titleLabel.Text = T("onboarding.status.offline", "ONBOARDING OFFLINE");
                _objectiveLabel.Text = T("onboarding.status.booting", "Booting the renderer…");
                _hintLabel.Text = T("onboarding.hint.empty", "HINT: —");
                _assistanceLabel.Text = T("onboarding.assistance.standard", "ASSISTANCE: STANDARD");
                EmptyChecklist();
                SetActionEnabledStates(false);
                UpdateAssistanceButtonLabel(OnboardingAssistance.Standard);
                return;
            }

            var def = j.CurrentStageDef;
            bool complete = j.JourneyComplete;

            _titleLabel.Text = complete
                ? T("onboarding.status.complete", "JOURNEY COMPLETE")
                : string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    T("onboarding.status.current", "CURRENT: {0}"),
                    LocalizedStageTitle(def));
            _objectiveLabel.Text = complete
                ? T(j.Profile == OnboardingProfile.FirstHour
                    ? "onboarding.status.first_hour_complete"
                    : "onboarding.status.day_two",
                    j.Profile == OnboardingProfile.FirstHour
                        ? "The first-hour sequence is complete. The shelter is yours to manage."
                        : "You reached Day 2. Returning the ledger to your command.")
                : LocalizedStageObjective(def);
            _hintLabel.Text = BuildHintLine(j, def);
            _assistanceLabel.Text = AssistanceLabel(j.Assistance);

            RebuildChecklist(j);
            SetActionEnabledStates(!complete);
            UpdateAssistanceButtonLabel(j.Assistance);
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && !key.IsEcho()
                && key.Keycode == Key.Escape)
            {
                Visible = false;
                GetViewport().SetInputAsHandled();
            }
        }

        public void MarkHintDismissed()
        {
            var j = _journey;
            if (j == null) return;
            j.DismissHint(MakeHintKey(j.CurrentStage));
            OnHintDismissed?.Invoke();
            RefreshView();
        }

        public void CycleAssistance(OnboardingAssistance level)
        {
            UpdateAssistanceButtonLabel(level);
            OnAssistanceChanged?.Invoke(level);
            RefreshView();
        }

        // ── Private ──

        private void OnShowClicked()
        {
            var j = _journey;
            if (j == null) return;
            var def = j.CurrentStageDef;
            var route = def.ShowMeWhereRoute;
            if (string.IsNullOrWhiteSpace(route)) return;
            j.RecordShowMeWhere(j.CurrentStage);
            OnShowMeWhereRequested?.Invoke(route);
        }

        private void OnSkipClicked() => OnCurrentStepSkipped?.Invoke();

        private void OnReplayClicked() => OnJourneyReplayed?.Invoke();

        private void OnDismissClicked() => MarkHintDismissed();

        private void OnCycleAssistanceClicked()
        {
            var j = _journey;
            if (j == null) return;
            OnboardingAssistance next = j.Assistance switch
            {
                OnboardingAssistance.Minimal => OnboardingAssistance.Standard,
                OnboardingAssistance.Standard => OnboardingAssistance.Guided,
                _ => OnboardingAssistance.Minimal,
            };
            UpdateAssistanceButtonLabel(next);
            OnAssistanceChanged?.Invoke(next);
        }

        private void UpdateAssistanceButtonLabel(OnboardingAssistance value)
        {
            if (_cycleAssistBtn == null) return;
            _cycleAssistBtn.Text = $"ASSISTANCE: {value.ToString().ToUpperInvariant()}";
        }

        private void SetActionEnabledStates(bool journeyOpen)
        {
            _showBtn.Disabled = !journeyOpen;
            _skipBtn.Disabled = !journeyOpen;
            _dismissBtn.Disabled = !journeyOpen;
            _replayBtn.Disabled = !journeyOpen;
            _cycleAssistBtn.Disabled = !journeyOpen;
        }

        private void EmptyChecklist()
        {
            if (_checklist == null) return;
            AshfallUiHelpers.EmptyChildren(_checklist);
            _checklist.AddChild(AshfallUiHelpers.MakeMetadata(T("onboarding.status.inactive",
                "Onboarding is not active for this run.")));
        }

        private void RebuildChecklist(OnboardingJourney j)
        {
            if (_checklist == null) return;
            AshfallUiHelpers.EmptyChildren(_checklist);

            var order = OnboardingCatalog.OrderFor(j.Profile);
            for (int i = 0; i < order.Count; i++)
            {
                var def = OnboardingCatalog.DefFor(j.Profile, order[i]);
                bool isCurrent = (int)def.Id == (int)j.CurrentStage && !j.JourneyComplete;
                bool done = j.IsStageComplete(def.Id);
                string glyph = done ? "[OK]" : (isCurrent ? "[..]" : "[ ]");
                var lbl = new Label
                {
                    Text = $"  {glyph}  {LocalizedStageTitle(def)}",
                };
                lbl.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                var colorToken = done
                    ? Ashfall.Core.UI.Theme.Muted
                    : (isCurrent ? Ashfall.Core.UI.Theme.Hot : Ashfall.Core.UI.Theme.Pale);
                lbl.AddThemeColorOverride("font_color",
                    AshfallUiHelpers.ToColor(colorToken));
                _checklist.AddChild(lbl);
            }
        }

        private static string BuildHintLine(OnboardingJourney j, OnboardingStageDef def)
        {
            string contextualKey = def.Id switch
            {
                OnboardingStage.Protocol => "onboarding.hint.protocol",
                OnboardingStage.Inspect => "onboarding.hint.inspect",
                OnboardingStage.Rationing => "onboarding.hint.rationing",
                OnboardingStage.Assignment => "onboarding.hint.assignment",
                OnboardingStage.Weather => "onboarding.hint.weather",
                OnboardingStage.InventoryUse => "onboarding.hint.inventory",
                OnboardingStage.DayAdvance => "onboarding.hint.day_advance",
                OnboardingStage.Water => "onboarding.hint.water",
                OnboardingStage.Power => "onboarding.hint.power",
                OnboardingStage.Food => "onboarding.hint.food",
                OnboardingStage.Research => "onboarding.hint.research",
                OnboardingStage.Expedition => "onboarding.hint.expedition",
                _ => "onboarding.hint.empty",
            };
            string fallback = def.Id switch
            {
                OnboardingStage.Protocol => "Pick ration, maintenance, and radio. The bunker stores adjust around your choices.",
                OnboardingStage.Inspect => "Inspect any three rooms — every confirming note is a fallback if the next storm cuts light.",
                OnboardingStage.Rationing => "Open the stores. Read the food and water you are rationing against.",
                OnboardingStage.Assignment => "Pull one survivor onto a duty from the Duty Roster. Their shifts move the bunker forward.",
                OnboardingStage.Weather => "Read the forecast before you end the day — fallout storms change outdoor rad.",
                OnboardingStage.InventoryUse => "Equip something real. The geiger or gas mask only protects the hands that wear them.",
                OnboardingStage.DayAdvance => "Confirm the advance. The morning briefing returns once Day 2 lands.",
                OnboardingStage.Water => "Start a treatment batch at the water plant. The stores only change if a batch actually runs.",
                OnboardingStage.Power => "Open the grid and throw one breaker. Watch the rooms change.",
                OnboardingStage.Food => "Eat one ration from stores. The count should drop.",
                OnboardingStage.Research => "Start an available research node. It appears in the queue.",
                OnboardingStage.Expedition => "Send a team with the expedition command. They leave the shelter.",
                _ => "HINT: —",
            };

            string hintKey = MakeHintKey(def.Id);
            bool dismissed = j.IsHintDismissed(hintKey);
            if (dismissed) return T("onboarding.hint.dismissed", "HINT: (dismissed)");

            if (j.Assistance == OnboardingAssistance.Minimal)
                return string.Empty;
            return $"{T("onboarding.hint.prefix", "HINT:")} {T(contextualKey, fallback)}";
        }

        private static string T(string key, string fallback) =>
            AshfallLocalization.Tr(key, fallback);

        private static string LocalizedStageTitle(OnboardingStageDef def) =>
            T(StageKey(def.Id, "title"), def.Title).ToUpperInvariant();

        private static string LocalizedStageObjective(OnboardingStageDef def) =>
            T(StageKey(def.Id, "objective"), def.Objective);

        private static string StageKey(OnboardingStage id, string part) =>
            $"onboarding.{id switch
            {
                OnboardingStage.InventoryUse => "inventory_use",
                OnboardingStage.DayAdvance => "day_advance",
                _ => id.ToString().ToLowerInvariant()
            }}.{part}";

        private static string AssistanceLabel(OnboardingAssistance assistance) =>
            assistance switch
            {
                OnboardingAssistance.Minimal => T("onboarding.assistance.minimal", "ASSISTANCE: MINIMAL"),
                OnboardingAssistance.Guided => T("onboarding.assistance.guided", "ASSISTANCE: GUIDED"),
                _ => T("onboarding.assistance.standard", "ASSISTANCE: STANDARD"),
            };

        public static string MakeHintKey(OnboardingStage id) => $"stage.{(int)id}.hint";
    }
}
