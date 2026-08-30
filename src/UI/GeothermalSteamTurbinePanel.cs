using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Deep Geothermal Flash Steam Turbine & Condenser (PWR-07).
    /// Superheated brine cyclone separation, impulse turbine rotor, and closed-loop condenser.
    /// </summary>
    public partial class GeothermalSteamTurbinePanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private Label _headerLabel = null!;
        private Label _statusLabel = null!;
        private Label _pressureLabel = null!;
        private Label _tempLabel = null!;
        private Label _rpmLabel = null!;
        private Label _loadLabel = null!;
        private Label _vacuumLabel = null!;
        private Label _feedbackLabel = null!;
        private Button _steamValButton = null!;
        private Button _antiScaleButton = null!;
        private Button _drainBrineButton = null!;
        private Button _tripButton = null!;
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

            _headerLabel = AshfallUiHelpers.MakeLabel("SHELTER ENERGY // GEOTHERMAL STEAM TURBINE (PWR-07)", 20, true);
            root.AddChild(_headerLabel);

            _statusLabel = AshfallUiHelpers.MakeSectionHeader("[STATUS: GENERATING - TURBINE #01 @ 3,000 RPM / POWER OUTPUT: 4.2 MW]");
            _statusLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            root.AddChild(_statusLabel);

            root.AddChild(new HSeparator());

            var grid = new GridContainer { Columns = 2 };
            grid.AddThemeConstantOverride("h_separation", 24);
            grid.AddThemeConstantOverride("v_separation", 8);
            root.AddChild(grid);

            _pressureLabel = AshfallUiHelpers.MakeBody("Flash Separator Pressure: 24.5 Bar");
            grid.AddChild(_pressureLabel);

            _tempLabel = AshfallUiHelpers.MakeBody("Superheated Steam Temperature: 224.0°C");
            grid.AddChild(_tempLabel);

            _rpmLabel = AshfallUiHelpers.MakeBody("Turbine Rotor Speed: 3,000 RPM (Grid Synchronous)");
            grid.AddChild(_rpmLabel);

            _loadLabel = AshfallUiHelpers.MakeBody("Generator Electrical Load: 4.2 MW Continuous");
            grid.AddChild(_loadLabel);

            _vacuumLabel = AshfallUiHelpers.MakeBody("Surface Condenser Vacuum: 0.08 Bar Abs");
            grid.AddChild(_vacuumLabel);

            root.AddChild(new HSeparator());

            var consoleBox = new HBoxContainer();
            consoleBox.AddThemeConstantOverride("separation", 12);
            root.AddChild(consoleBox);

            _steamValButton = new Button { Text = "[ENGAGE STEAM INLET VALVE]" };
            _steamValButton.Pressed += () => ShowFeedback("Governor throttle valve opened. Steam flow increased to 85 kg/s.");
            consoleBox.AddChild(_steamValButton);

            _antiScaleButton = new Button { Text = "[INJECT ANTI-SCALING INHIBITOR]" };
            _antiScaleButton.Pressed += () => ShowFeedback("Phosphonate inhibitor dosed into geothermal brine loop.");
            consoleBox.AddChild(_antiScaleButton);

            _drainBrineButton = new Button { Text = "[DRAIN SEPARATOR BRINE]" };
            _drainBrineButton.Pressed += () => ShowFeedback("Spent mineralized brine reinjected into deep geological sump.");
            consoleBox.AddChild(_drainBrineButton);

            _tripButton = new Button { Text = "[EMERGENCY TURBINE TRIP]" };
            _tripButton.Pressed += () => ShowFeedback("Emergency trip solenoid fired. Main steam stop valve slammed shut.");
            consoleBox.AddChild(_tripButton);

            _closeButton = new Button { Text = "[CLOSE PANEL]" };
            _closeButton.Pressed += () => { Visible = false; OnClose?.Invoke(); };
            consoleBox.AddChild(_closeButton);

            _feedbackLabel = AshfallUiHelpers.MakeSmall("Geothermal wellhead pressure nominal. Subterranean heat exchanger stable.");
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
