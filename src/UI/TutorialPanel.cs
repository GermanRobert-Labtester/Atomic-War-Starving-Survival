using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Tutorial panel.
    /// Shows game controls, basic instructions, and onboarding help.
    /// </summary>
    public partial class TutorialPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblControlsTitle;
        private VBoxContainer _controlsList;
        private Label _lblBasicsTitle;
        private VBoxContainer _basicsList;
        private Label _lblTipsTitle;
        private VBoxContainer _tipsList;

        // Placeholder tutorial data
        private readonly string[] _placeholderControls = {
            "[WASD/Arrows] — Move camera around the bunker",
            "[Click] — Select items, survivors, or UI elements",
            "[Right-Click] — Open context menu for selected item",
            "[E] — Interact with objects and survivors",
            "[Tab] — Open quick inventory",
            "[Esc] — Close current panel or menu",
            "[F1] — Open this help panel",
            "[F5] — Quick save",
            "[F9] — Quick load"
        };

        private readonly string[] _placeholderBasics = {
            "Survival Needs — Monitor hunger, thirst, fatigue, warmth, and morale",
            "Radiation — Keep radiation levels below 100 mSv to avoid illness",
            "Survivors — Assign work shifts to keep everyone productive",
            "Inventory — Manage supplies, craft items, and equip gear",
            "Expeditions — Send survivors to scavenge for rare resources",
            "Factions — Build relationships for trade and alliances",
            "Weather — Prepare for nuclear winter and fallout storms"
        };

        private readonly string[] _placeholderTips = {
            "Always keep iodine pills stocked for radiation emergencies",
            "Water filters are essential — clean water is more valuable than food",
            "Gas masks reduce radiation exposure by 40% when worn",
            "Train survivors in multiple skills for flexibility",
            "Trade with factions when you have surplus resources",
            "Document everything in your journal — it matters later",
            "Don't neglect morale — unhappy survivors work less efficiently",
            "Stockpile medical supplies — they're irreplaceable once used"
        };

        // Real data from host session
        // private TutorialHostSession? _tutorialHost;

        public void Bind(object tutorial) // placeholder for TutorialHostSession
        {
            // _tutorialHost = (TutorialHostSession)tutorial;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_controlsList == null || _basicsList == null || _tipsList == null) return;

            // Clear existing lists
            AshfallUiHelpers.EmptyChildren(_controlsList);
            AshfallUiHelpers.EmptyChildren(_basicsList);
            AshfallUiHelpers.EmptyChildren(_tipsList);

            // Display placeholder controls
            foreach (string control in _placeholderControls)
            {
                var label = new Label { Text = control };
                label.CustomMinimumSize = new Vector2(400, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _controlsList.AddChild(label);
            }

            // Display placeholder basics
            foreach (string basic in _placeholderBasics)
            {
                var label = new Label { Text = basic };
                label.CustomMinimumSize = new Vector2(400, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _basicsList.AddChild(label);
            }

            // Display placeholder tips
            foreach (string tip in _placeholderTips)
            {
                var label = new Label { Text = tip };
                label.CustomMinimumSize = new Vector2(400, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _tipsList.AddChild(label);
            }
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

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc/F1]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);
        }

        public void Open()
        {
            Visible = true;
            QueueRedraw();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;

            if (@event is InputEventKey key && key.Pressed && (key.Keycode == Key.Escape || key.Keycode == Key.F1))
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
