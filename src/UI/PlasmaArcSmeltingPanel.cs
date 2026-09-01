using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Plasma Arc Smelting & Slag Vitrification (MET-03).
    /// High-temperature plasma torch melting, copper refractory crucible, and radioactive slag encapsulation.
    /// </summary>
    public partial class PlasmaArcSmeltingPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private Label _headerLabel = null!;
        private Label _statusLabel = null!;
        private Label _torchCurrentLabel = null!;
        private Label _tempLabel = null!;
        private Label _argonLabel = null!;
        private Label _densityLabel = null!;
        private Label _feedbackLabel = null!;
        private Button _strikeTorchButton = null!;
        private Button _pourCrucibleButton = null!;
        private Button _argonGasButton = null!;
        private Button _dumpCrucibleButton = null!;
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

            _headerLabel = AshfallUiHelpers.MakeLabel("SHELTER METALLURGY // PLASMA ARC SMELTING (MET-03)", 20, true);
            root.AddChild(_headerLabel);

            _statusLabel = AshfallUiHelpers.MakeSectionHeader("[STATUS: SMELTING - 3,500°C PLASMA TORCH @ 4,200A / CRUCIBLE: 1,820°C]");
            _statusLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            root.AddChild(_statusLabel);

            root.AddChild(new HSeparator());

            var grid = new GridContainer { Columns = 2 };
            grid.AddThemeConstantOverride("h_separation", 24);
            grid.AddThemeConstantOverride("v_separation", 8);
            root.AddChild(grid);

            _torchCurrentLabel = AshfallUiHelpers.MakeBody("Plasma Torch Current: 4,200 A (Non-Transferred Arc)");
            grid.AddChild(_torchCurrentLabel);

            _tempLabel = AshfallUiHelpers.MakeBody("Refractory Crucible Temp: 1,820.0°C (Molten State)");
            grid.AddChild(_tempLabel);

            _argonLabel = AshfallUiHelpers.MakeBody("Argon Inert Shield Gas: 12.0 L/min");
            grid.AddChild(_argonLabel);

            _densityLabel = AshfallUiHelpers.MakeBody("Vitrified Slag Density: 2.85 g/cm³ (Leach-Proof)");
            grid.AddChild(_densityLabel);

            root.AddChild(new HSeparator());

            var consoleBox = new HBoxContainer();
            consoleBox.AddThemeConstantOverride("separation", 12);
            root.AddChild(consoleBox);

            _strikeTorchButton = new Button { Text = "[STRIKE PLASMA TORCH]" };
            _strikeTorchButton.Pressed += () => ShowFeedback("Plasma arc struck at 3,500°C. Tungsten scrap melting initiated.");
            consoleBox.AddChild(_strikeTorchButton);

            _pourCrucibleButton = new Button { Text = "[TILT CRUCIBLE FOR POUR]" };
            _pourCrucibleButton.Pressed += () => ShowFeedback("Hydraulic tilt engaged. 45kg high-purity alloy cast into ingots.");
            consoleBox.AddChild(_pourCrucibleButton);

            _argonGasButton = new Button { Text = "[INJECT ARGON SHIELD]" };
            _argonGasButton.Pressed += () => ShowFeedback("Argon cover gas flow increased to prevent atmospheric oxidation.");
            consoleBox.AddChild(_argonGasButton);

            _dumpCrucibleButton = new Button { Text = "[EMERGENCY CRUCIBLE DUMP]" };
            _dumpCrucibleButton.Pressed += () => ShowFeedback("Crucible dump valve triggered. Molten charge expelled into refractory sand pit.");
            consoleBox.AddChild(_dumpCrucibleButton);

            _closeButton = new Button { Text = "[CLOSE PANEL]" };
            _closeButton.Pressed += () => { Visible = false; OnClose?.Invoke(); };
            consoleBox.AddChild(_closeButton);

            _feedbackLabel = AshfallUiHelpers.MakeSmall("Water cooling loop active. Crucible jacket temperature: 48°C.");
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
