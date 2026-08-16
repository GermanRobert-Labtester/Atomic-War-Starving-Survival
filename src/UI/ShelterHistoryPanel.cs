using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Shelter History panel.
    /// Shows detailed shelter history, maintenance records, and shelter upgrades timeline.
    /// </summary>
    public partial class ShelterHistoryPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblHistoryTitle;
        private VBoxContainer _shelterHistory;
        private Label _lblMaintenanceTitle;
        private VBoxContainer _maintenanceRecords;
        private Label _lblUpgradesTitle;
        private VBoxContainer _upgradeTimeline;

        private readonly string[] _placeholderHistory = {
            "[Day 1] Bunker established — Initial assessment",
            "[Day 5] First structural repairs — East wing damage",
            "[Day 10] Radiation shielding installed — 40% reduction",
            "[Day 15] Air filtration upgraded — 65% efficiency",
            "[Day 20] Blast doors repaired — Full operational"
        };

        private readonly string[] _placeholderMaintenance = {
            "Day 1-5: Structural assessment and initial repairs",
            "Day 5-10: Radiation shielding installation",
            "Day 10-15: Air filtration system upgrade",
            "Day 15-20: Blast door repairs and testing",
            "Day 20-25: Regular maintenance and inspections",
            "Total Maintenance Events: 5 major events"
        };

        private readonly string[] _placeholderUpgrades = {
            "Day 10: Radiation shielding +40% (Complete)",
            "Day 15: Air filtration +65% efficiency (Complete)",
            "Day 20: Blast doors +100% operational (Complete)",
            "Day 25: Solar power integration +15% (In progress)",
            "Day 30: Water collection system +80% (In progress)",
            "Next Upgrade: Shielding completion (Day 35)"
        };

        public void Bind(object shelterHistory)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_shelterHistory == null || _maintenanceRecords == null || _upgradeTimeline == null) return;

            while (_shelterHistory.GetChildCount() > 0) _shelterHistory.RemoveChild(_shelterHistory.GetChild(0));
            while (_maintenanceRecords.GetChildCount() > 0) _maintenanceRecords.RemoveChild(_maintenanceRecords.GetChild(0));
            while (_upgradeTimeline.GetChildCount() > 0) _upgradeTimeline.RemoveChild(_upgradeTimeline.GetChild(0));

            foreach (string history in _placeholderHistory)
            {
                var label = new Label { Text = history };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _shelterHistory.AddChild(label);
            }

            foreach (string maintenance in _placeholderMaintenance)
            {
                var label = new Label { Text = maintenance };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _maintenanceRecords.AddChild(label);
            }

            foreach (string upgrade in _placeholderUpgrades)
            {
                var label = new Label { Text = upgrade };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _upgradeTimeline.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("SHELTER HISTORY", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblHistoryTitle = AshfallUiHelpers.MakeSectionHeader("SHELTER HISTORY");
            vbox.AddChild(_lblHistoryTitle);

            _shelterHistory = new VBoxContainer();
            _shelterHistory.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _shelterHistory.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_shelterHistory);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblMaintenanceTitle = AshfallUiHelpers.MakeSectionHeader("MAINTENANCE RECORDS");
            vbox.AddChild(_lblMaintenanceTitle);

            _maintenanceRecords = new VBoxContainer();
            _maintenanceRecords.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _maintenanceRecords.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_maintenanceRecords);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblUpgradesTitle = AshfallUiHelpers.MakeSectionHeader("UPGRADE TIMELINE");
            vbox.AddChild(_lblUpgradesTitle);

            _upgradeTimeline = new VBoxContainer();
            _upgradeTimeline.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _upgradeTimeline.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_upgradeTimeline);

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
