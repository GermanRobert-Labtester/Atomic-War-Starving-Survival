using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Deep Borehole Seismograph & Fault Line Stress (GEO-02).
    /// Triaxial borehole accelerometers, Gutenberg-Richter analysis, and tectonic tremor triangulation.
    /// </summary>
    public partial class BoreholeSeismographPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private Label _headerLabel = null!;
        private Label _statusLabel = null!;
        private Label _depthLabel = null!;
        private Label _velocityLabel = null!;
        private Label _stressLabel = null!;
        private Label _tremorLabel = null!;
        private Label _feedbackLabel = null!;
        private Button _calibrateButton = null!;
        private Button _triangulateButton = null!;
        private Button _reinforceButton = null!;
        private Button _alarmButton = null!;
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

            _headerLabel = AshfallUiHelpers.MakeLabel("SHELTER GEOLOGY // DEEP BOREHOLE SEISMOGRAPH (GEO-02)", 20, true);
            root.AddChild(_headerLabel);

            _statusLabel = AshfallUiHelpers.MakeSectionHeader("[STATUS: MONITORING - 3-AXIS PROBE @ -800M / AMBIENT NOISE: 1.2 RICHTER]");
            _statusLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            root.AddChild(_statusLabel);

            root.AddChild(new HSeparator());

            var grid = new GridContainer { Columns = 2 };
            grid.AddThemeConstantOverride("h_separation", 24);
            grid.AddThemeConstantOverride("v_separation", 8);
            root.AddChild(grid);

            _depthLabel = AshfallUiHelpers.MakeBody("Borehole Sensor Depth: -800.0 m (Bedrock Anchor)");
            grid.AddChild(_depthLabel);

            _velocityLabel = AshfallUiHelpers.MakeBody("P-Wave Velocity: 5.8 km/s (Basalt Layer)");
            grid.AddChild(_velocityLabel);

            _stressLabel = AshfallUiHelpers.MakeBody("Tectonic Shear Stress: 42.5 MPa (Accumulating)");
            grid.AddChild(_stressLabel);

            _tremorLabel = AshfallUiHelpers.MakeBody("Background Micro-Seismicity: 1.2 Richter M_L");
            grid.AddChild(_tremorLabel);

            root.AddChild(new HSeparator());

            var consoleBox = new HBoxContainer();
            consoleBox.AddThemeConstantOverride("separation", 12);
            root.AddChild(consoleBox);

            _calibrateButton = new Button { Text = "[CALIBRATE ACCELEROMETERS]" };
            _calibrateButton.Pressed += () => ShowFeedback("Triaxial piezo sensors zeroed and calibrated against reference pendulum.");
            consoleBox.AddChild(_calibrateButton);

            _triangulateButton = new Button { Text = "[TRIANGULATE EPICENTER]" };
            _triangulateButton.Pressed += () => ShowFeedback("Epicenter localized: 4.2 km North-East at depth -1.2 km.");
            consoleBox.AddChild(_triangulateButton);

            _reinforceButton = new Button { Text = "[REINFORCE SHEAR DAMPERS]" };
            _reinforceButton.Pressed += () => ShowFeedback("Hydraulic foundation dampers pre-stressed for seismic isolation.");
            consoleBox.AddChild(_reinforceButton);

            _alarmButton = new Button { Text = "[TRIGGER TECTONIC ALARM]" };
            _alarmButton.Pressed += () => ShowFeedback("Subterranean tremor alert broadcast to all shelter living sectors.");
            consoleBox.AddChild(_alarmButton);

            _closeButton = new Button { Text = "[CLOSE PANEL]" };
            _closeButton.Pressed += () => { Visible = false; OnClose?.Invoke(); };
            consoleBox.AddChild(_closeButton);

            _feedbackLabel = AshfallUiHelpers.MakeSmall("Seismic telemetry recording to drum memory. Fault slip sensors active.");
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
