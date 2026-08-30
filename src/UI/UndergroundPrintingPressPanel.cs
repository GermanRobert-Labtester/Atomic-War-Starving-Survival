using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Underground Printing Press & Propaganda Leaflet (PR-01).
    /// Rotary cylinder press, lead type composition, and rationing decree publishing.
    /// </summary>
    public partial class UndergroundPrintingPressPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private Label _headerLabel = null!;
        private Label _statusLabel = null!;
        private Label _speedLabel = null!;
        private Label _viscosityLabel = null!;
        private Label _paperLabel = null!;
        private Label _moraleLabel = null!;
        private Label _feedbackLabel = null!;
        private Button _composeButton = null!;
        private Button _runPressButton = null!;
        private Button _disperseButton = null!;
        private Button _burnPlatesButton = null!;
        private Button _closeButton = null!;

        public bool IsBound { get; private set; } = true;
        public int SimDay { get; set; } = 1;

        public override void _Ready()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            var bg = new ColorRect { Color = AshfallUiHelpers.ToColor(DesignTheme.Ink) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var margin = AshfallUiHelpers.MakeMargins(16);
            AddChild(margin);

            var root = new VBoxContainer();
            root.AddThemeConstantOverride("separation", 10);
            margin.AddChild(root);

            _headerLabel = AshfallUiHelpers.MakeLabel("SHELTER GOVERNANCE // CLANDESTINE PRINTING PRESS (PR-01)", 20, true);
            root.AddChild(_headerLabel);

            _statusLabel = AshfallUiHelpers.MakeSectionHeader("[STATUS: PRINTING - CYLINDER PRESS: 1,200 IMPRESSIONS/H / EDITION: RATION DECREE #42]");
            _statusLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            root.AddChild(_statusLabel);

            root.AddChild(new HSeparator());

            var grid = new GridContainer { Columns = 2 };
            grid.AddThemeConstantOverride("h_separation", 24);
            grid.AddThemeConstantOverride("v_separation", 8);
            root.AddChild(grid);

            _speedLabel = AshfallUiHelpers.MakeBody("Rotary Press Throughput: 1,200 Impressions / Hour");
            grid.AddChild(_speedLabel);

            _viscosityLabel = AshfallUiHelpers.MakeBody("Petroleum Ink Viscosity: 45 Pa·s (Optimal)");
            grid.AddChild(_viscosityLabel);

            _paperLabel = AshfallUiHelpers.MakeBody("Recycled Paper Stock: 850 Raw Sheets");
            grid.AddChild(_paperLabel);

            _moraleLabel = AshfallUiHelpers.MakeBody("Projected Morale Impact: +12% Hope / Stability");
            grid.AddChild(_moraleLabel);

            root.AddChild(new HSeparator());

            var consoleBox = new HBoxContainer();
            consoleBox.AddThemeConstantOverride("separation", 12);
            root.AddChild(consoleBox);

            _composeButton = new Button { Text = "[COMPOSE LEAD TYPE EDITORIAL]" };
            _composeButton.Pressed += () => ShowFeedback("New editorial composed in lead type tray. Title: 'The Long Winter Thaw'.");
            consoleBox.AddChild(_composeButton);

            _runPressButton = new Button { Text = "[RUN OFFSET CYLINDER PRESS]" };
            _runPressButton.Pressed += () => ShowFeedback("Offset press engaged. 300 ration decrees printed and transferred to drying racks.");
            consoleBox.AddChild(_runPressButton);

            _disperseButton = new Button { Text = "[DISPERSE LEAFLETS TO SECTORS]" };
            _disperseButton.Pressed += () => ShowFeedback("Runners dispatched. Leaflets distributed across Sectors 01 through 04.");
            consoleBox.AddChild(_disperseButton);

            _burnPlatesButton = new Button { Text = "[EMERGENCY BURN PRESS PLATES]" };
            _burnPlatesButton.Pressed += () => ShowFeedback("Thermite charge fired into lead type melting pot. Evidence destroyed.");
            consoleBox.AddChild(_burnPlatesButton);

            _closeButton = new Button { Text = "[CLOSE PANEL]" };
            _closeButton.Pressed += () => { Visible = false; OnClose?.Invoke(); };
            consoleBox.AddChild(_closeButton);

            _feedbackLabel = AshfallUiHelpers.MakeSmall("Press room sound-dampened. Ink rollers aligned to precision tolerances.");
            _feedbackLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
            root.AddChild(_feedbackLabel);
        }

        private void ShowFeedback(string msg)
        {
            if (_feedbackLabel != null)
            {
                _feedbackLabel.Text = msg;
                _feedbackLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
            }
        }

        public void Open()
        {
            Visible = true;
        }

        public void RefreshView() { }
        public void Unbind() { }
    }
}
