using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Subterranean War Dog Kennel & Radiation Bio-Monitoring (FAUN-01).
    /// Guard canine behavior conditioning, dosimeter collar telemetry, and decontamination wash sump.
    /// </summary>
    public partial class WarDogKennelPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private Label _headerLabel = null!;
        private Label _statusLabel = null!;
        private Label _caninesLabel = null!;
        private Label _heartRateLabel = null!;
        private Label _radDoseLabel = null!;
        private Label _moraleLabel = null!;
        private Label _scentLabel = null!;
        private Label _feedbackLabel = null!;
        private Button _deployButton = null!;
        private Button _radBathButton = null!;
        private Button _rationsButton = null!;
        private Button _drillButton = null!;
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

            _headerLabel = AshfallUiHelpers.MakeLabel("SHELTER SECURITY & FAUNA // CANINE KENNEL & BIO-MONITOR (FAUN-01)", 20, true);
            root.AddChild(_headerLabel);

            _statusLabel = AshfallUiHelpers.MakeSectionHeader("[STATUS: ACTIVE - KENNEL OCCUPANCY: 6/6 CANINES / 2 ON EXPEDITION PATROL]");
            _statusLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            root.AddChild(_statusLabel);

            root.AddChild(new HSeparator());

            var grid = new GridContainer { Columns = 2 };
            grid.AddThemeConstantOverride("h_separation", 24);
            grid.AddThemeConstantOverride("v_separation", 8);
            root.AddChild(grid);

            _caninesLabel = AshfallUiHelpers.MakeBody("Active War Dogs: 6 (K-01 Cerberus, K-02 Echo, K-03 Rust)");
            grid.AddChild(_caninesLabel);

            _heartRateLabel = AshfallUiHelpers.MakeBody("Pack Heart Rate Average: 84 BPM (Resting Vitals)");
            grid.AddChild(_heartRateLabel);

            _radDoseLabel = AshfallUiHelpers.MakeBody("Collar Dosimeter Average: 0.12 mSv/h (Within Margins)");
            grid.AddChild(_radDoseLabel);

            _moraleLabel = AshfallUiHelpers.MakeBody("Canine Pack Loyalty & Morale: 94%");
            grid.AddChild(_moraleLabel);

            _scentLabel = AshfallUiHelpers.MakeBody("Tracking & Scent Readiness: 98%");
            grid.AddChild(_scentLabel);

            root.AddChild(new HSeparator());

            var consoleBox = new HBoxContainer();
            consoleBox.AddThemeConstantOverride("separation", 12);
            root.AddChild(consoleBox);

            _deployButton = new Button { Text = "[DEPLOY SCOUT DOG]" };
            _deployButton.Pressed += () => ShowFeedback("K-02 'Echo' deployed on surface perimeter scout patrol.");
            consoleBox.AddChild(_deployButton);

            _radBathButton = new Button { Text = "[RAD-CLEANSE VET BATH]" };
            _radBathButton.Pressed += () => ShowFeedback("Chelating de-dusting wash cycle completed for active kennel cohort.");
            consoleBox.AddChild(_radBathButton);

            _rationsButton = new Button { Text = "[DISPENSE FORTIFIED MEAT]" };
            _rationsButton.Pressed += () => ShowFeedback("6 high-protein fortified meat rations dispensed to kennel pens.");
            consoleBox.AddChild(_rationsButton);

            _drillButton = new Button { Text = "[INITIATE AGGRESSION DRILL]" };
            _drillButton.Pressed += () => ShowFeedback("Bite sleeve aggression drill completed. Combat readiness +15%.");
            consoleBox.AddChild(_drillButton);

            _closeButton = new Button { Text = "[CLOSE PANEL]" };
            _closeButton.Pressed += () => { Visible = false; OnClose?.Invoke(); };
            consoleBox.AddChild(_closeButton);

            _feedbackLabel = AshfallUiHelpers.MakeSmall("Armored kennel enclosures climate-controlled. Bio-telemetry synchronized.");
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
