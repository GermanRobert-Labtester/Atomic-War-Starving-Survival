using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Quests panel.
    /// Shows active quests, objectives, and story progression.
    /// </summary>
    public partial class QuestsPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblActiveTitle;
        private VBoxContainer _activeQuests;
        private Label _lblCompletedTitle;
        private VBoxContainer _completedQuests;

        // Placeholder quest data
        private readonly string[] _placeholderActiveQuests = {
            "Quest: Find Clean Water Source",
            "Status: Active - Day 12/15",
            "Objective: Locate clean water source within 5km",
            "Reward: +20 Rations, +10 Morale",
            "Progress: Explored 3 sectors, found 1 potential source"
        };

        private readonly string[] _placeholderCompletedQuests = {
            "[Day 8] Completed: Establish Radio Contact",
            "[Day 5] Completed: Secure Water Supply",
            "[Day 3] Completed: First Contact with Survivors"
        };

        // Real data from host session
        // private QuestHostSession? _questHost;

        public void Bind(object quests) // placeholder for QuestHostSession
        {
            // _questHost = (QuestHostSession)quests;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_activeQuests == null || _completedQuests == null) return;

            // Clear existing lists
            while (_activeQuests.GetChildCount() > 0)
                _activeQuests.RemoveChild(_activeQuests.GetChild(0));
            while (_completedQuests.GetChildCount() > 0)
                _completedQuests.RemoveChild(_completedQuests.GetChild(0));

            // Display placeholder active quests
            foreach (string quest in _placeholderActiveQuests)
            {
                var label = new Label { Text = quest };
                label.CustomMinimumSize = new Vector2(400, 40);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _activeQuests.AddChild(label);
            }

            // Display placeholder completed quests
            foreach (string quest in _placeholderCompletedQuests)
            {
                var label = new Label { Text = quest };
                label.CustomMinimumSize = new Vector2(400, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
                _completedQuests.AddChild(label);
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
            vbox.CustomMinimumSize = new Vector2(500, 0);
            container.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("QUESTS & STORY", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Active quests section
            _lblActiveTitle = AshfallUiHelpers.MakeSectionHeader("ACTIVE QUESTS");
            vbox.AddChild(_lblActiveTitle);

            _activeQuests = new VBoxContainer();
            _activeQuests.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _activeQuests.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_activeQuests);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Completed quests section
            _lblCompletedTitle = AshfallUiHelpers.MakeSectionHeader("COMPLETED QUESTS");
            vbox.AddChild(_lblCompletedTitle);

            _completedQuests = new VBoxContainer();
            _completedQuests.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _completedQuests.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_completedQuests);

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
