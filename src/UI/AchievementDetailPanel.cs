using System;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Achievement Detail panel.
    /// Shows detailed achievement information, unlock conditions, and achievement history.
    /// </summary>
    public partial class AchievementDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblAchievementInfoTitle;
        private VBoxContainer _achievementInfo;
        private Label _lblUnlockedTitle;
        private VBoxContainer _unlockedAchievements;
        private Label _lblLockedTitle;
        private VBoxContainer _lockedAchievements;

        private readonly string[] _placeholderAchievementInfo = {
            "Achievement: First Contact",
            "Category: Social",
            "Description: Make first contact with another survivor group",
            "Reward: +50 Morale, +10 Reputation",
            "Unlocked: Day 15",
            "Difficulty: Medium"
        };

        private readonly string[] _placeholderUnlocked = {
            "First Steps — Survived first day ✓",
            "Scavenger — Found first supplies ✓",
            "Medic — Used first medical item ✓",
            "Explorer — Discovered first location ✓",
            "Trader — Completed first trade ✓",
            "Leader — Made first leadership decision ✓",
            "Diplomat — Established first faction contact ✓",
            "Survivor — Reached day 25 ✓"
        };

        private readonly string[] _placeholderLocked = {
            "Master Scavenger — Collect 100 supplies (Locked)",
            "Medicine Man — Treat 50 patients (Locked)",
            "Explorer — Discover all locations (Locked)",
            "Diplomat — Establish 5 faction relationships (Locked)",
            "Survivor — Reach day 100 (Locked)",
            "Master Builder — Complete all shelter upgrades (Locked)"
        };

        public void Bind(object achievementDetail)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_achievementInfo == null || _unlockedAchievements == null || _lockedAchievements == null) return;

            AshfallUiHelpers.EmptyChildren(_achievementInfo);
            AshfallUiHelpers.EmptyChildren(_unlockedAchievements);
            AshfallUiHelpers.EmptyChildren(_lockedAchievements);

            foreach (string info in _placeholderAchievementInfo)
            {
                var label = new Label { Text = info };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _achievementInfo.AddChild(label);
            }

            foreach (string unlocked in _placeholderUnlocked)
            {
                var label = new Label { Text = unlocked };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _unlockedAchievements.AddChild(label);
            }

            foreach (string locked in _placeholderLocked)
            {
                var label = new Label { Text = locked };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
                _lockedAchievements.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("ACHIEVEMENT DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblAchievementInfoTitle = AshfallUiHelpers.MakeSectionHeader("ACHIEVEMENT INFORMATION");
            vbox.AddChild(_lblAchievementInfoTitle);

            _achievementInfo = new VBoxContainer();
            _achievementInfo.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _achievementInfo.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_achievementInfo);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblUnlockedTitle = AshfallUiHelpers.MakeSectionHeader("UNLOCKED ACHIEVEMENTS");
            vbox.AddChild(_lblUnlockedTitle);

            _unlockedAchievements = new VBoxContainer();
            _unlockedAchievements.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _unlockedAchievements.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_unlockedAchievements);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblLockedTitle = AshfallUiHelpers.MakeSectionHeader("LOCKED ACHIEVEMENTS");
            vbox.AddChild(_lblLockedTitle);

            _lockedAchievements = new VBoxContainer();
            _lockedAchievements.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _lockedAchievements.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_lockedAchievements);

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
