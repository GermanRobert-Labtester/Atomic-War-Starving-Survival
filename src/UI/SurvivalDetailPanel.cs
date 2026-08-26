using System;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Survival Detail panel.
    /// Shows detailed survival needs tracking, health metrics, and survival status.
    /// </summary>
    public partial class SurvivalDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblHealthTitle;
        private VBoxContainer _healthData;
        private Label _lblNeedsTitle;
        private VBoxContainer _needsData;
        private Label _lblRadiationTitle;
        private VBoxContainer _radiationData;
        private Label _lblStatusTitle;
        private VBoxContainer _statusData;

        private readonly string[] _placeholderHealth = {
            "Overall Health: 85/100 (Good)",
            "Physical Health: 90/100",
            "Mental Health: 80/100",
            "Injuries: Minor (Right forearm)",
            "Illness: None",
            "Recovery Status: Recovering"
        };

        private readonly string[] _placeholderNeeds = {
            "Hunger: 80/100 (Adequate)",
            "Thirst: 75/100 (Moderate)",
            "Fatigue: 65/100 (Tired)",
            "Warmth: 85/100 (Adequate)",
            "Morale: 75/100 (Good)",
            "Hygiene: 70/100 (Fair)"
        };

        private readonly string[] _placeholderRadiation = {
            "Current Exposure: 0.8 mSv/hr (Elevated)",
            "Total Exposure: 12.4 mSv (Low risk)",
            "Daily Average: 0.5 mSv/day",
            "Peak Today: 1.2 mSv/hr",
            "Protection Level: 40% (Basic Gas Mask)",
            "Next Checkup: Day 26 (In 1 day)"
        };

        private readonly string[] _placeholderStatus = {
            "Survival Status: Active",
            "Daily Consumption: -12 units",
            "Resource Availability: Adequate",
            "Work Capacity: 85% (Reduced due to injury)",
            "Social Status: Integrated (5/5 survivors)",
            "Long-term Outlook: Stable"
        };

        public void Bind(object survivalDetail)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_healthData == null || _needsData == null || _radiationData == null || _statusData == null) return;

            AshfallUiHelpers.EmptyChildren(_healthData);
            AshfallUiHelpers.EmptyChildren(_needsData);
            AshfallUiHelpers.EmptyChildren(_radiationData);
            AshfallUiHelpers.EmptyChildren(_statusData);

            foreach (string health in _placeholderHealth)
            {
                var label = new Label { Text = health };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _healthData.AddChild(label);
            }

            foreach (string need in _placeholderNeeds)
            {
                var label = new Label { Text = need };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _needsData.AddChild(label);
            }

            foreach (string rad in _placeholderRadiation)
            {
                var label = new Label { Text = rad };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _radiationData.AddChild(label);
            }

            foreach (string status in _placeholderStatus)
            {
                var label = new Label { Text = status };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _statusData.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("SURVIVAL DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblHealthTitle = AshfallUiHelpers.MakeSectionHeader("HEALTH STATUS");
            vbox.AddChild(_lblHealthTitle);

            _healthData = new VBoxContainer();
            _healthData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _healthData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_healthData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblNeedsTitle = AshfallUiHelpers.MakeSectionHeader("SURVIVAL NEEDS");
            vbox.AddChild(_lblNeedsTitle);

            _needsData = new VBoxContainer();
            _needsData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _needsData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_needsData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblRadiationTitle = AshfallUiHelpers.MakeSectionHeader("RADIATION STATUS");
            vbox.AddChild(_lblRadiationTitle);

            _radiationData = new VBoxContainer();
            _radiationData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _radiationData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_radiationData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblStatusTitle = AshfallUiHelpers.MakeSectionHeader("OVERALL STATUS");
            vbox.AddChild(_lblStatusTitle);

            _statusData = new VBoxContainer();
            _statusData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _statusData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_statusData);

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
