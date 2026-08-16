using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Quest Detail panel.
    /// Shows detailed quest information, objectives, progress, and quest rewards.
    /// </summary>
    public partial class QuestDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblQuestInfoTitle;
        private VBoxContainer _questInfo;
        private Label _lblObjectivesTitle;
        private VBoxContainer _objectivesList;
        private Label _lblProgressTitle;
        private VBoxContainer _progressList;
        private Label _lblRewardsTitle;
        private VBoxContainer _rewardsList;

        // Placeholder quest data
        private readonly string[] _placeholderQuestInfo = {
            "Quest: Find Clean Water Source",
            "Type: Main Story Quest",
            "Difficulty: Medium",
            "Status: Active",
            "Day Started: 12",
            "Estimated Completion: Day 15"
        };

        private readonly string[] _placeholderObjectives = {
            "Explore 3 sectors within 5km radius",
            "Locate potential clean water source",
            "Test water quality (requires water filter)",
            "Return to bunker with clean water sample",
            "Report findings to Elena (Leader)"
        };

        private readonly string[] _placeholderProgress = {
            "Sectors explored: 2/3 (67%)",
            "Water source located: Yes (Sector 8)",
            "Water tested: No (pending water filter)",
            "Sample collected: No (pending water test)",
            "Report submitted: No (pending sample)"
        };

        private readonly string[] _placeholderRewards = {
            "+20 Rations — Food supply bonus",
            "+10 Morale — Community morale boost",
            "+5 Knowledge — Water purification research",
            "Unlock: Advanced Water Filter recipe",
            "Unlock: Hydroponic irrigation system"
        };

        // Real data from host session
        // private QuestHostSession? _questHost;
        // private string _selectedQuestId;

        public void Bind(object quest, string questId) // placeholder for QuestHostSession
        {
            // _questHost = (QuestHostSession)quest;
            // _selectedQuestId = questId;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_questInfo == null || _objectivesList == null || _progressList == null || _rewardsList == null) return;

            // Clear existing lists
            while (_questInfo.GetChildCount() > 0)
                _questInfo.RemoveChild(_questInfo.GetChild(0));
            while (_objectivesList.GetChildCount() > 0)
                _objectivesList.RemoveChild(_objectivesList.GetChild(0));
            while (_progressList.GetChildCount() > 0)
                _progressList.RemoveChild(_progressList.GetChild(0));
            while (_rewardsList.GetChildCount() > 0)
                _rewardsList.RemoveChild(_rewardsList.GetChild(0));

            // Display placeholder quest info
            foreach (string info in _placeholderQuestInfo)
            {
                var label = new Label { Text = info };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _questInfo.AddChild(label);
            }

            // Display placeholder objectives
            foreach (string objective in _placeholderObjectives)
            {
                var label = new Label { Text = objective };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _objectivesList.AddChild(label);
            }

            // Display placeholder progress
            foreach (string progress in _placeholderProgress)
            {
                var label = new Label { Text = progress };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _progressList.AddChild(label);
            }

            // Display placeholder rewards
            foreach (string reward in _placeholderRewards)
            {
                var label = new Label { Text = reward };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot));
                _rewardsList.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("QUEST DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Quest info section
            _lblQuestInfoTitle = AshfallUiHelpers.MakeSectionHeader("QUEST INFORMATION");
            vbox.AddChild(_lblQuestInfoTitle);

            _questInfo = new VBoxContainer();
            _questInfo.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _questInfo.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_questInfo);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Objectives section
            _lblObjectivesTitle = AshfallUiHelpers.MakeSectionHeader("OBJECTIVES");
            vbox.AddChild(_lblObjectivesTitle);

            _objectivesList = new VBoxContainer();
            _objectivesList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _objectivesList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_objectivesList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Progress section
            _lblProgressTitle = AshfallUiHelpers.MakeSectionHeader("PROGRESS");
            vbox.AddChild(_lblProgressTitle);

            _progressList = new VBoxContainer();
            _progressList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _progressList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_progressList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Rewards section
            _lblRewardsTitle = AshfallUiHelpers.MakeSectionHeader("REWARDS");
            vbox.AddChild(_lblRewardsTitle);

            _rewardsList = new VBoxContainer();
            _rewardsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _rewardsList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_rewardsList);

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
