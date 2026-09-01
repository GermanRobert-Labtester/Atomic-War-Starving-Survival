using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Onboarding;

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

            _titleLabel = AshfallUiHelpers.MakeTitle("DAY 1 OBJECTIVE", Ashfall.Core.UI.Theme.FontSizeH2);
            _titleLabel.HorizontalAlignment = HorizontalAlignment.Left;
            _titleLabel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            _titleLabel.TooltipText = "The first-hour onboarding tracker. Survives save/load.";
            header.AddChild(_titleLabel);

            _closeBtn = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => { Visible = false; });
            _closeBtn.TooltipText = "Close the onboarding hint panel. Your progress is preserved.";
            _closeBtn.CustomMinimumSize = new Vector2(110, 32);
            header.AddChild(_closeBtn);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _assistanceLabel = AshfallUiHelpers.MakeMono("ASSISTANCE: STANDARD");
            _assistanceLabel.TooltipText = "How much the journey guides each beat. Toggle via the assistance button below.";
            vbox.AddChild(_assistanceLabel);

            _objectiveLabel = AshfallUiHelpers.MakeBody("Booting renderer — a real save/load resume is wired in Core.");
            _objectiveLabel.TooltipText = "Exactly what is required to complete this onboarding step.";
            _objectiveLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            vbox.AddChild(_objectiveLabel);

            _hintLabel = AshfallUiHelpers.MakeMono("HINT: —");
            _hintLabel.TooltipText = "A contextual nudge that appears when you pause or hit a wall.";
            _hintLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            vbox.AddChild(_hintLabel);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Action row ──
            var actionRow = new HBoxContainer();
            actionRow.AddThemeConstantOverride("separation", 8);
            vbox.AddChild(actionRow);

            _showBtn = AshfallUiHelpers.MakeButton("SHOW ME WHERE", OnShowClicked, false);
            _showBtn.TooltipText = "Open the real system for this step so you can act.";
            _showBtn.CustomMinimumSize = new Vector2(160, 36);
            actionRow.AddChild(_showBtn);

            _skipBtn = AshfallUiHelpers.MakeButton("SKIP STEP", OnSkipClicked, false);
            _skipBtn.TooltipText = "Mark this step known and move on. Real actions are still yours to take when ready.";
            _skipBtn.CustomMinimumSize = new Vector2(120, 36);
            actionRow.AddChild(_skipBtn);

            _dismissBtn = AshfallUiHelpers.MakeButton("DISMISS HINT", OnDismissClicked, false);
            _dismissBtn.TooltipText = "Hide the contextual hint for this step until you advance.";
            _dismissBtn.CustomMinimumSize = new Vector2(140, 36);
            actionRow.AddChild(_dismissBtn);

            _cycleAssistBtn = AshfallUiHelpers.MakeButton("ASSISTANCE: STANDARD", OnCycleAssistanceClicked, false);
            _cycleAssistBtn.TooltipText = "Cycle between MINIMAL (objective only), STANDARD (default), GUIDED (extra help).";
            _cycleAssistBtn.CustomMinimumSize = new Vector2(200, 36);
            actionRow.AddChild(_cycleAssistBtn);

            _replayBtn = AshfallUiHelpers.MakeButton("REPLAY", OnReplayClicked, false);
            _replayBtn.TooltipText = "Restart the onboarding hints from Day 1 (does not undo gameplay).";
            _replayBtn.CustomMinimumSize = new Vector2(100, 36);
            actionRow.AddChild(_replayBtn);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            vbox.AddChild(AshfallUiHelpers.MakeSectionHeader("JOURNEY CHECKLIST"));

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
                _titleLabel.Text = "ONBOARDING OFFLINE";
                _objectiveLabel.Text = "Booting the renderer…";
                _hintLabel.Text = "HINT: —";
                _assistanceLabel.Text = "ASSISTANCE: STANDARD";
                EmptyChecklist();
                SetActionEnabledStates(false);
                UpdateAssistanceButtonLabel(OnboardingAssistance.Standard);
                return;
            }

            var def = j.CurrentStageDef;
            bool complete = j.JourneyComplete;

            _titleLabel.Text = complete
                ? "JOURNEY COMPLETE"
                : $"CURRENT: {def.Title.ToUpperInvariant()}";
            _objectiveLabel.Text = complete
                ? "You reached Day 2. Returning the ledger to your command."
                : def.Objective;
            _hintLabel.Text = BuildHintLine(j, def);
            _assistanceLabel.Text = $"ASSISTANCE: {j.Assistance.ToString().ToUpperInvariant()}";

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
            _checklist.AddChild(AshfallUiHelpers.MakeMetadata("Onboarding is not active for this run."));
        }

        private void RebuildChecklist(OnboardingJourney j)
        {
            if (_checklist == null) return;
            AshfallUiHelpers.EmptyChildren(_checklist);

            for (int i = 0; i < OnboardingCatalog.Order.Length; i++)
            {
                var def = OnboardingCatalog.Order[i];
                bool isCurrent = (int)def.Id == (int)j.CurrentStage && !j.JourneyComplete;
                bool done = j.IsStageComplete(def.Id);
                string glyph = done ? "[OK]" : (isCurrent ? "[..]" : "[ ]");
                var lbl = new Label
                {
                    Text = $"  {glyph}  {def.Title}",
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
            string contextual = def.Id switch
            {
                OnboardingStage.Protocol =>
                    "Pick ration, maintenance, and radio. The bunker stores adjust around your choices.",
                OnboardingStage.Inspect =>
                    "Inspect any three rooms — every confirming note is a fallback if the next storm cuts light.",
                OnboardingStage.Rationing =>
                    "Open the stores. Read the food and water you are rationing against.",
                OnboardingStage.Assignment =>
                    "Pull one survivor onto a duty from the Duty Roster. Their shifts move the bunker forward.",
                OnboardingStage.Weather =>
                    "Read the forecast before you end the day — fallout storms change outdoor rad.",
                OnboardingStage.InventoryUse =>
                    "Equip something real. The geiger or gas mask only protects the hands that wear them.",
                OnboardingStage.DayAdvance =>
                    "Confirm the advance. The morning briefing returns once Day 2 lands.",
                _ => "HINT: —",
            };

            string hintKey = MakeHintKey(def.Id);
            bool dismissed = j.IsHintDismissed(hintKey);
            if (dismissed) return "HINT: (dismissed)";

            if (j.Assistance == OnboardingAssistance.Minimal)
                return string.Empty;
            return $"HINT: {contextual}";
        }

        public static string MakeHintKey(OnboardingStage id) => $"stage.{(int)id}.hint";
    }
}
