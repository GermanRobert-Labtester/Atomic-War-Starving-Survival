using System;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Achievements panel.
    /// Shows player achievements, statistics, and progression milestones.
    /// </summary>
    public partial class AchievementsPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblStatsTitle;
        private VBoxContainer _statsList;
        private Label _lblAchievementsTitle;
        private VBoxContainer _achievementsList;
        private Label _lblMilestonesTitle;
        private VBoxContainer _milestonesList;

        // Placeholder stats data
        private readonly string[] _placeholderStats = {
            "Days Survived: 25",
            "Survivors Rescued: 5",
            "Expeditions Completed: 3",
            "Items Crafted: 42",
            "Quests Completed: 8",
            "Factions Met: 4"
        };

        // Placeholder achievements data
        private readonly string[] _placeholderAchievements = {
            "First Steps — Survived first day",
            "Scavenger — Found first supplies",
            "Medic — Used first medical item",
            "Explorer — Discovered first location",
            "Trader — Completed first trade",
            "Leader — Made first leadership decision",
            "Diplomat — Established first faction contact",
            "Survivor — Reached day 25"
        };

        // Placeholder milestones data
        private readonly string[] _placeholderMilestones = {
            "Day 10: Bunker established as permanent base",
            "Day 15: First successful expedition completed",
            "Day 20: Radio communication established",
            "Day 25: Community expanded to 5 survivors",
            "Day 30: First trade agreement signed"
        };

        // Real data from host session
        // private AchievementsHostSession? _achievementsHost;

        public void Bind(object achievements) // placeholder for AchievementsHostSession
        {
            // _achievementsHost = (AchievementsHostSession)achievements;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_statsList == null || _achievementsList == null || _milestonesList == null) return;

            // Clear existing lists
            AshfallUiHelpers.EmptyChildren(_statsList);
            AshfallUiHelpers.EmptyChildren(_achievementsList);
            AshfallUiHelpers.EmptyChildren(_milestonesList);

            // Display placeholder stats
            foreach (string stats in _placeholderStats)
            {
                var label = new Label { Text = stats };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _statsList.AddChild(label);
            }

            // Display placeholder achievements
            foreach (string achievement in _placeholderAchievements)
            {
                var label = new Label { Text = achievement };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _achievementsList.AddChild(label);
            }

            // Display placeholder milestones
            foreach (string milestone in _placeholderMilestones)
            {
                var label = new Label { Text = milestone };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _milestonesList.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("ACHIEVEMENTS & STATS", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Stats section
            _lblStatsTitle = AshfallUiHelpers.MakeSectionHeader("GAME STATISTICS");
            vbox.AddChild(_lblStatsTitle);

            _statsList = new VBoxContainer();
            _statsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _statsList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_statsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Achievements section
            _lblAchievementsTitle = AshfallUiHelpers.MakeSectionHeader("ACHIEVEMENTS");
            vbox.AddChild(_lblAchievementsTitle);

            _achievementsList = new VBoxContainer();
            _achievementsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _achievementsList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_achievementsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Milestones section
            _lblMilestonesTitle = AshfallUiHelpers.MakeSectionHeader("MILESTONES");
            vbox.AddChild(_lblMilestonesTitle);

            _milestonesList = new VBoxContainer();
            _milestonesList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _milestonesList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_milestonesList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);

            var hint = AshfallUiHelpers.MakeSmall("[Esc] to close");
            hint.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeLabel);
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            vbox.AddChild(hint);
        }

        public void Open()
        {
            Visible = true;
            QueueRedraw();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;

            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
