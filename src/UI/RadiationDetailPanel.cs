using System;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Radiation Detail panel.
    /// Shows detailed radiation data, dosimeter readings, protection levels, and radiation events.
    /// </summary>
    public partial class RadiationDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblCurrentTitle;
        private VBoxContainer _currentData;
        private Label _lblDosimeterTitle;
        private VBoxContainer _dosimeterData;
        private Label _lblProtectionTitle;
        private VBoxContainer _protectionData;
        private Label _lblEventsTitle;
        private VBoxContainer _eventsList;

        // Placeholder radiation data
        private readonly string[] _placeholderCurrent = {
            "Current Level: 0.8 mSv/hr (Elevated)",
            "Total Exposure: 12.4 mSv (Low)",
            "Peak Today: 1.2 mSv/hr (Hour 14)",
            "Baseline: 0.1 mSv/hr (Normal)",
            "Danger Threshold: 100 mSv/hr",
            "Chronic Risk: Low (below 10 mSv/yr)"
        };

        private readonly string[] _placeholderDosimeter = {
            "Device: Digital Dosimeter (Brand X)",
            "Battery: 85% (Good)",
            "Calibration: Last calibrated Day 10",
            "Accuracy: ±5% (Standard)",
            "Reading Method: Real-time + cumulative",
            "Alarm Threshold: 50 mSv/hr (Warning)",
            "Emergency Threshold: 100 mSv/hr (Danger)"
        };

        private readonly string[] _placeholderProtection = {
            "Current Protection: 40% (Basic Gas Mask)",
            "Max Protection: 75% (Full Hazmat Suit)",
            "Shelter Reduction: 65% (Bunker Shielding)",
            "Time in Shelter: 18 hours (Today)",
            "Time Outdoors: 6 hours (Today)",
            "Effective Exposure: 4.2 mSv (Adjusted)"
        };

        private readonly string[] _placeholderEvents = {
            "[Day 5] Fallout storm passed — +2.5 mSv cumulative",
            "[Day 12] Radiation spike detected — Sector 4 elevated",
            "[Day 18] Dosimeter recalibrated — accuracy improved",
            "[Day 22] Fallout warning issued — Seek shelter immediately",
            "[Day 25] Radiation levels stabilizing — Normal operations"
        };

        // Real data from host session
        // private RadiationHostSession? _radiationHost;

        public void Bind(object radiation) // placeholder for RadiationHostSession
        {
            // _radiationHost = (RadiationHostSession)radiation;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_currentData == null || _dosimeterData == null || _protectionData == null || _eventsList == null) return;

            // Clear existing lists
            AshfallUiHelpers.EmptyChildren(_currentData);
            AshfallUiHelpers.EmptyChildren(_dosimeterData);
            AshfallUiHelpers.EmptyChildren(_protectionData);
            AshfallUiHelpers.EmptyChildren(_eventsList);

            // Display placeholder current radiation
            foreach (string data in _placeholderCurrent)
            {
                var label = new Label { Text = data };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _currentData.AddChild(label);
            }

            // Display placeholder dosimeter data
            foreach (string data in _placeholderDosimeter)
            {
                var label = new Label { Text = data };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _dosimeterData.AddChild(label);
            }

            // Display placeholder protection data
            foreach (string data in _placeholderProtection)
            {
                var label = new Label { Text = data };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _protectionData.AddChild(label);
            }

            // Display placeholder radiation events
            foreach (string radiationEvent in _placeholderEvents)
            {
                var label = new Label { Text = radiationEvent };
                label.CustomMinimumSize = new Vector2(350, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
                _eventsList.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("RADIATION DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Current radiation section
            _lblCurrentTitle = AshfallUiHelpers.MakeSectionHeader("CURRENT RADIATION");
            vbox.AddChild(_lblCurrentTitle);

            _currentData = new VBoxContainer();
            _currentData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _currentData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_currentData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Dosimeter section
            _lblDosimeterTitle = AshfallUiHelpers.MakeSectionHeader("DOSIMETER STATUS");
            vbox.AddChild(_lblDosimeterTitle);

            _dosimeterData = new VBoxContainer();
            _dosimeterData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _dosimeterData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_dosimeterData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Protection section
            _lblProtectionTitle = AshfallUiHelpers.MakeSectionHeader("PROTECTION LEVELS");
            vbox.AddChild(_lblProtectionTitle);

            _protectionData = new VBoxContainer();
            _protectionData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _protectionData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_protectionData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Events section
            _lblEventsTitle = AshfallUiHelpers.MakeSectionHeader("RADIATION EVENTS");
            vbox.AddChild(_lblEventsTitle);

            _eventsList = new VBoxContainer();
            _eventsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _eventsList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_eventsList);

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
