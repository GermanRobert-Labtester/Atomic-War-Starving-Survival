// SPDX-License-Identifier: MIT
using System;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.Localization;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.Host;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.Localization;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Tutorial / Help panel.
    /// Shows the real control scheme and honest survival basics. Controls are
    /// curated to match the actual key handlers in Main.Application.cs and the
    /// sidebar-driven panel navigation — no fabricated bindings (e.g. no WASD
    /// camera, no F5 quick-save; those do not exist in ASHFALL).
    /// </summary>
    public partial class TutorialPanel : Control
    {
        public event Action? OnClose;
        public event Action<string>? OnContextualAcknowledged;

        private VBoxContainer _contentVBox = null!;
        private Label _lblControlsTitle;
        private VBoxContainer _controlsList;
        private Label _lblBasicsTitle;
        private VBoxContainer _basicsList;
        private Label _lblTipsTitle;
        private VBoxContainer _tipsList;
        private AcceptDialog _contextualDialog = null!;
        private string _activeContextualId = string.Empty;
        public bool IsContextualLessonVisible => _contextualDialog != null && _contextualDialog.Visible;

        private int _simDay = 1;

        /// <summary>True once Bind has been called with a real day.</summary>
        public bool IsBound { get; private set; }

        /// <summary>Number of control rows rendered (probe for ui-tests).</summary>
        public int RenderedControlsCount { get; private set; }

        // Real controls — curated to match Main.Application.cs key handlers +
        // the sidebar-driven navigation. Canonical InputMap actions are defined
        // in AshfallInputActions.cs and reconciled at runtime.
        private static (string key, string action)[] GetCurrentControls() => new[]
        {
            ("[Click Sidebar]", "Open shelter systems — Survivors, Inventory, Medical, Map, etc."),
            (AshfallInputActions.GetActionPrompt(AshfallInputActions.Close), "Close current panel or return to main menu"),
            (AshfallInputActions.GetActionPrompt(AshfallInputActions.Journal), "Open the Journal / Field Manual / Codex"),
            (AshfallInputActions.GetActionPrompt(AshfallInputActions.Events), "Open the Events log"),
            (AshfallInputActions.GetActionPrompt(AshfallInputActions.Forecast), "Open the Weather forecast"),
            (AshfallInputActions.GetActionPrompt(AshfallInputActions.WeatherHistory), "Open the Weather history"),
            (AshfallInputActions.GetActionPrompt(AshfallInputActions.Expeditions), "Open the Expeditions management panel"),
            (AshfallInputActions.GetActionPrompt(AshfallInputActions.Holdfast), "Open the Holdfast trade terminal"),
            ("[1-5]", "Switch Journal tabs (while Journal is open)"),
            (AshfallInputActions.GetActionPrompt(AshfallInputActions.Confirm), "Confirm daily briefing / advance day"),
            (AshfallInputActions.GetActionPrompt(AshfallInputActions.Help), "Toggle this Tutorial & Help overlay")
        };

        // Honest survival basics — aligned with the systems that actually exist.
        private static readonly string[] RealBasics =
        {
            "Needs — Hunger, Thirst, Fatigue, Warmth, and Morale keep rising. At 90+, Health starts to fail and does not recover on its own.",
            "Radiation — Outdoor fallout and storms add dose. Above 50 mSv, Acute Sickness takes −5 HP/hr. Give Rad-Away or iodine in Medical.",
            "Water — Three people drink ~3.6 clean units a day. Check stores before you end the day.",
            "Power — Air and water filters need watts. A brownout stops filtration and indoor radiation climbs.",
            "Duty Roster — Assign people to Kitchen, Water, Maintenance, or Guard. Unassigned hands do not keep the shelter running.",
            "Expeditions — Send a team for salvage. Check masks, fuel, and dose before they leave.",
            "Weather — Fallout storms and black rain spike outdoor dose. Keep people inside during hazard alerts."
        };

        // Honest tips — no fabricated item behaviour.
        private static readonly string[] RealTips =
        {
            "Mikhail starts Day 1 with Acute Radiation. Give Rad-Away in Medical or he may be dead within 16 hours.",
            "Keep iodine on the shelf. It buys hours of resistance when a storm hits.",
            "Clean water runs out before food. Keep the filter and desalination membranes working.",
            "A gas mask cuts outdoor dose. A hazmat suit cuts it further. Both wear out.",
            "Watch the Dose Ledger. Cumulative exposure becomes chronic illness, not just a spike.",
            "Low morale slows work. Rest, records, and caregiving can bring it back.",
            "End the day on purpose. Advance Day ticks every system once.",
            "Save before a risky expedition. Continue loads the last day-advance save."
        };

        public void Bind(int simDay = 1)
        {
            _simDay = simDay;
            IsBound = true;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_controlsList == null || _basicsList == null || _tipsList == null) return;

            AshfallUiHelpers.EmptyChildren(_controlsList);
            AshfallUiHelpers.EmptyChildren(_basicsList);
            AshfallUiHelpers.EmptyChildren(_tipsList);

            RenderedControlsCount = 0;
            foreach (var (key, action) in GetCurrentControls())
            {
                var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                var k = AshfallUiHelpers.MakeMono(key);
                k.CustomMinimumSize = new Vector2(150, 0);
                k.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                row.AddChild(k);

                var desc = AshfallUiHelpers.MakeSmall(action, true);
                desc.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                row.AddChild(desc);
                _controlsList.AddChild(row);
                RenderedControlsCount++;
            }

            foreach (string basic in RealBasics)
            {
                var label = new Label { Text = basic };
                label.CustomMinimumSize = new Vector2(400, 0);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _basicsList.AddChild(label);
            }

            foreach (string tip in RealTips)
            {
                var label = new Label { Text = tip };
                label.CustomMinimumSize = new Vector2(400, 0);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _tipsList.AddChild(label);
            }
        }

        /// <summary>Shows one event-driven contextual lesson. The lesson ID
        /// comes from Core onboarding state; text is resolved at presentation
        /// time so locale changes never touch campaign state.</summary>
        public void ShowContextual(string tutorialId)
        {
            if (_contextualDialog == null || string.IsNullOrWhiteSpace(tutorialId)) return;
            if (_contextualDialog.Visible) return;
            string title = AshfallLocalization.Tr(
                WildlifeTrappingLocalization.TutorialTitleKey(tutorialId), tutorialId);
            string fallbackBody = tutorialId switch
            {
                WildlifeTrappingLocalization.FirstSnareTutorialId =>
                    "Make or find a trap, set it on a valid site, and bait it if you can spare the food. Check it later, then butcher a catch. Wild prey can carry sickness or contamination.",
                WildlifeTrappingLocalization.WearOutTutorialId =>
                    "Traps wear as they work. A broken trap stops catching but stays on the map. Repair it or replace it.",
                WildlifeTrappingLocalization.BycatchTutorialId =>
                    "A trap can catch something unintended. Inspect catches before processing: bycatch may help, hurt, or carry disease or contamination.",
                Ashfall.Core.Localization.OnboardingLessonLocalization.ProtectionBeforeDispatchId =>
                    "Radiation does not announce itself. Check the party's protection and dose \u2014 gear, anti-rad, or a shorter route.",
                Ashfall.Core.Localization.OnboardingLessonLocalization.SevereWeatherPrepId =>
                    "Weather carries the dose with it. A severe day raises exposure and cuts travel; read the forecast before you commit people or supplies.",
                _ => "A new lesson is ready. Check the related shelter panel before you continue."
            };
            _contextualDialog.Title = title;
            _contextualDialog.DialogText = AshfallLocalization.Tr(
                WildlifeTrappingLocalization.TutorialBodyKey(tutorialId), fallbackBody);
            _activeContextualId = tutorialId;
            _contextualDialog.PopupCentered(new Vector2I(720, 300));
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(550, 0);
            container.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("TUTORIAL & HELP", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Controls section
            _lblControlsTitle = AshfallUiHelpers.MakeSectionHeader("CONTROLS");
            vbox.AddChild(_lblControlsTitle);

            _controlsList = new VBoxContainer();
            _controlsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _controlsList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_controlsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Basics section
            _lblBasicsTitle = AshfallUiHelpers.MakeSectionHeader("SURVIVAL BASICS");
            vbox.AddChild(_lblBasicsTitle);

            _basicsList = new VBoxContainer();
            _basicsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _basicsList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_basicsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Tips section
            _lblTipsTitle = AshfallUiHelpers.MakeSectionHeader("SURVIVAL TIPS");
            vbox.AddChild(_lblTipsTitle);

            _tipsList = new VBoxContainer();
            _tipsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _tipsList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_tipsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton(
                $"CLOSE [{AshfallInputActions.GetActionPrompt(AshfallInputActions.Close)}/{AshfallInputActions.GetActionPrompt(AshfallInputActions.Help)}]",
                () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);

            _contextualDialog = new AcceptDialog
            {
                Title = "SURVIVAL LESSON",
                DialogText = string.Empty
            };
            _contextualDialog.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            _contextualDialog.Confirmed += () =>
            {
                string completedId = _activeContextualId;
                _activeContextualId = string.Empty;
                if (!string.IsNullOrEmpty(completedId))
                    OnContextualAcknowledged?.Invoke(completedId);
            };
            AddChild(_contextualDialog);
        }

        public void Open()
        {
            Visible = true;
            QueueRedraw();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;

            if (AshfallInputActions.IsCloseOrCancel(@event) || AshfallInputActions.IsHelp(@event))
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
