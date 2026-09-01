using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Electromagnetic Isotope Separator & Calutron (ISO-01).
    /// High-vacuum ion beam source, 1.84 Tesla bending arc, and isotope collector slit pockets.
    /// </summary>
    public partial class IsotopeSeparatorPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private Label _headerLabel = null!;
        private Label _statusLabel = null!;
        private Label _voltageLabel = null!;
        private Label _magFieldLabel = null!;
        private Label _vacuumLabel = null!;
        private Label _beamCurrentLabel = null!;
        private Label _yieldLabel = null!;
        private Label _feedbackLabel = null!;
        private Button _strikeBeamButton = null!;
        private Button _rampFieldButton = null!;
        private Button _harvestButton = null!;
        private Button _dumpBeamButton = null!;
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

            _headerLabel = AshfallUiHelpers.MakeLabel("SHELTER NUCLEAR PHYSICS // ELECTROMAGNETIC ISOTOPE SEPARATOR (ISO-01)", 20, true);
            root.AddChild(_headerLabel);

            _statusLabel = AshfallUiHelpers.MakeSectionHeader("[STATUS: ENRICHING - CALUTRON BEAM: 45.2 KV / U-235 PURITY: 19.8% LEU]");
            _statusLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            root.AddChild(_statusLabel);

            root.AddChild(new HSeparator());

            var grid = new GridContainer { Columns = 2 };
            grid.AddThemeConstantOverride("h_separation", 24);
            grid.AddThemeConstantOverride("v_separation", 8);
            root.AddChild(grid);

            _voltageLabel = AshfallUiHelpers.MakeBody("Acceleration Voltage: 45.2 kV DC");
            grid.AddChild(_voltageLabel);

            _magFieldLabel = AshfallUiHelpers.MakeBody("Bending Magnetic Field: 1.84 Tesla");
            grid.AddChild(_magFieldLabel);

            _vacuumLabel = AshfallUiHelpers.MakeBody("Chamber Vacuum: 1.2e-6 Torr High Vacuum");
            grid.AddChild(_vacuumLabel);

            _beamCurrentLabel = AshfallUiHelpers.MakeBody("Total Ion Beam Current: 120.0 mA");
            grid.AddChild(_beamCurrentLabel);

            _yieldLabel = AshfallUiHelpers.MakeBody("U-235 Enrichment Yield: 4.8 g / Day");
            grid.AddChild(_yieldLabel);

            root.AddChild(new HSeparator());

            var consoleBox = new HBoxContainer();
            consoleBox.AddThemeConstantOverride("separation", 12);
            root.AddChild(consoleBox);

            _strikeBeamButton = new Button { Text = "[STRIKE ION BEAM ARC]" };
            _strikeBeamButton.Pressed += () => ShowFeedback("Uranium tetrachloride vaporized. Ion source arc struck at 45.2 kV.");
            consoleBox.AddChild(_strikeBeamButton);

            _rampFieldButton = new Button { Text = "[RAMP 1.8T MAGNETIC FIELD]" };
            _rampFieldButton.Pressed += () => ShowFeedback("Electromagnet coils energized. 180° bending trajectory focused on collectors.");
            consoleBox.AddChild(_rampFieldButton);

            _harvestButton = new Button { Text = "[HARVEST ISOTOPE COLLECTORS]" };
            _harvestButton.Pressed += () => ShowFeedback("14.2g enriched U-235 (19.8% LEU) scraped from graphite pocket.");
            consoleBox.AddChild(_harvestButton);

            _dumpBeamButton = new Button { Text = "[EMERGENCY BEAM DUMP]" };
            _dumpBeamButton.Pressed += () => ShowFeedback("High-voltage crowbar triggered. Beam dumped into water-cooled target.");
            consoleBox.AddChild(_dumpBeamButton);

            _closeButton = new Button { Text = "[CLOSE PANEL]" };
            _closeButton.Pressed += () => { Visible = false; OnClose?.Invoke(); };
            consoleBox.AddChild(_closeButton);

            _feedbackLabel = AshfallUiHelpers.MakeSmall("Radiation shielding active. Lead-lined calutron housing interlocks sealed.");
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
