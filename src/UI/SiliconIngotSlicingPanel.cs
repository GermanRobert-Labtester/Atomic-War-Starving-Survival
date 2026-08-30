using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Monocrystalline Silicon Ingot Slicing & Diamond Wire Saw (ELEC-03).
    /// Precision semiconductor wafer fabrication and silicon carbide slurry cooling.
    /// </summary>
    public partial class SiliconIngotSlicingPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private Label _headerLabel = null!;
        private Label _statusLabel = null!;
        private Label _wireSpeedLabel = null!;
        private Label _slurryLabel = null!;
        private Label _kerfLabel = null!;
        private Label _purityLabel = null!;
        private Label _feedbackLabel = null!;
        private Button _sliceButton = null!;
        private Button _slurryButton = null!;
        private Button _collectButton = null!;
        private Button _brakeButton = null!;
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

            _headerLabel = AshfallUiHelpers.MakeLabel("SHELTER SEMICONDUCTOR // SILICON INGOT SLICING (ELEC-03)", 20, true);
            root.AddChild(_headerLabel);

            _statusLabel = AshfallUiHelpers.MakeSectionHeader("[STATUS: SLICING - DIAMOND WIRE: 24 M/S / WAFER THICKNESS: 180 µM]");
            _statusLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            root.AddChild(_statusLabel);

            root.AddChild(new HSeparator());

            var grid = new GridContainer { Columns = 2 };
            grid.AddThemeConstantOverride("h_separation", 24);
            grid.AddThemeConstantOverride("v_separation", 8);
            root.AddChild(grid);

            _wireSpeedLabel = AshfallUiHelpers.MakeBody("Diamond Wire Saw Speed: 24.0 m/s");
            grid.AddChild(_wireSpeedLabel);

            _slurryLabel = AshfallUiHelpers.MakeBody("SiC Coolant Slurry Flow: 14.0 L/min");
            grid.AddChild(_slurryLabel);

            _kerfLabel = AshfallUiHelpers.MakeBody("Wafer Kerf Loss: 70 µm / Target: 180 µm");
            grid.AddChild(_kerfLabel);

            _purityLabel = AshfallUiHelpers.MakeBody("Monocrystalline Purity: 99.9999% (6N Electronic Grade)");
            grid.AddChild(_purityLabel);

            root.AddChild(new HSeparator());

            var consoleBox = new HBoxContainer();
            consoleBox.AddThemeConstantOverride("separation", 12);
            root.AddChild(consoleBox);

            _sliceButton = new Button { Text = "[ENGAGE DIAMOND MULTI-WIRE SLICE]" };
            _sliceButton.Pressed += () => ShowFeedback("Diamond wire web tensioned. Czochralski boule feed started.");
            consoleBox.AddChild(_sliceButton);

            _slurryButton = new Button { Text = "[FEED SILICON CARBIDE SLURRY]" };
            _slurryButton.Pressed += () => ShowFeedback("Abrasive coolant nozzles purged and pressurized to 4.2 Bar.");
            consoleBox.AddChild(_slurryButton);

            _collectButton = new Button { Text = "[COLLECT WAFERS TO CASSETTE]" };
            _collectButton.Pressed += () => ShowFeedback("32 pristine 200mm silicon wafers transferred to cleanroom cassette.");
            consoleBox.AddChild(_collectButton);

            _brakeButton = new Button { Text = "[EMERGENCY WIRE BRAKE]" };
            _brakeButton.Pressed += () => ShowFeedback("Emergency tension brake engaged. Wire saw halted within 120ms.");
            consoleBox.AddChild(_brakeButton);

            _closeButton = new Button { Text = "[CLOSE PANEL]" };
            _closeButton.Pressed += () => { Visible = false; OnClose?.Invoke(); };
            consoleBox.AddChild(_closeButton);

            _feedbackLabel = AshfallUiHelpers.MakeSmall("Cleanroom laminar flow hood active. Particle count: Class 100.");
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
