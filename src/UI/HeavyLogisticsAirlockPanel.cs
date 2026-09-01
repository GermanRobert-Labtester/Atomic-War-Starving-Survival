using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Subterranean Heavy Logistics Airlock & Cargo Sump (LOG-03).
    /// Dual blast doors interlock, pneumatic de-dusting blowdown, and 25-ton gantry hoist.
    /// </summary>
    public partial class HeavyLogisticsAirlockPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private Label _headerLabel = null!;
        private Label _statusLabel = null!;
        private Label _doorStatusLabel = null!;
        private Label _pressureLabel = null!;
        private Label _deconLabel = null!;
        private Label _craneLabel = null!;
        private Label _feedbackLabel = null!;
        private Button _equalizeButton = null!;
        private Button _deconButton = null!;
        private Button _openInnerButton = null!;
        private Button _lockdownButton = null!;
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

            _headerLabel = AshfallUiHelpers.MakeLabel("SHELTER LOGISTICS // HEAVY CARGO AIRLOCK & SUMP (LOG-03)", 20, true);
            root.AddChild(_headerLabel);

            _statusLabel = AshfallUiHelpers.MakeSectionHeader("[STATUS: READY - INNER DOOR: LOCKED / OUTER DOOR: SEALED / PRESSURE: 1.02 BAR]");
            _statusLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            root.AddChild(_statusLabel);

            root.AddChild(new HSeparator());

            var grid = new GridContainer { Columns = 2 };
            grid.AddThemeConstantOverride("h_separation", 24);
            grid.AddThemeConstantOverride("v_separation", 8);
            root.AddChild(grid);

            _doorStatusLabel = AshfallUiHelpers.MakeBody("Blast Door Interlock: Inner (Locked) | Outer (Sealed)");
            grid.AddChild(_doorStatusLabel);

            _pressureLabel = AshfallUiHelpers.MakeBody("Airlock Barometric Pressure: 1.02 Bar (Equalized)");
            grid.AddChild(_pressureLabel);

            _deconLabel = AshfallUiHelpers.MakeBody("Pneumatic De-Dusting Blowdown: 100% Decontaminated");
            grid.AddChild(_deconLabel);

            _craneLabel = AshfallUiHelpers.MakeBody("25T Overhead Gantry Hoist Load: 8.4 Tons (APC Chassis)");
            grid.AddChild(_craneLabel);

            root.AddChild(new HSeparator());

            var consoleBox = new HBoxContainer();
            consoleBox.AddThemeConstantOverride("separation", 12);
            root.AddChild(consoleBox);

            _equalizeButton = new Button { Text = "[BAROMETRIC EQUALIZATION]" };
            _equalizeButton.Pressed += () => ShowFeedback("Equalization valves cycled. Pressure balanced with subterranean sector.");
            consoleBox.AddChild(_equalizeButton);

            _deconButton = new Button { Text = "[CYCLE DECON BLOWDOWN]" };
            _deconButton.Pressed += () => ShowFeedback("High-pressure pneumatic blowdown active. Fallout particulate trapped in sump.");
            consoleBox.AddChild(_deconButton);

            _openInnerButton = new Button { Text = "[OPEN INNER BLAST DOOR]" };
            _openInnerButton.Pressed += () => ShowFeedback("Mechanical locking cams disengaged. Inner 25-ton blast door opening.");
            consoleBox.AddChild(_openInnerButton);

            _lockdownButton = new Button { Text = "[EMERGENCY AIRLOCK PURGE]" };
            _lockdownButton.Pressed += () => ShowFeedback("Emergency blast seal triggered. Dual doors mechanically bolted.");
            consoleBox.AddChild(_lockdownButton);

            _closeButton = new Button { Text = "[CLOSE PANEL]" };
            _closeButton.Pressed += () => { Visible = false; OnClose?.Invoke(); };
            consoleBox.AddChild(_closeButton);

            _feedbackLabel = AshfallUiHelpers.MakeSmall("Heavy freight track clear. Sump drainage pumps standing by.");
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
