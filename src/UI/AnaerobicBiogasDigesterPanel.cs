using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Anaerobic Biogas Digester & Methane Scrubbing (PWR-06).
    /// Continuous Stirred Anaerobic Tank Reactor (CSTR) with iron sponge H2S desulfurization.
    /// </summary>
    public partial class AnaerobicBiogasDigesterPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private Label _headerLabel = null!;
        private Label _statusLabel = null!;
        private Label _tempLabel = null!;
        private Label _phLabel = null!;
        private Label _yieldLabel = null!;
        private Label _purityLabel = null!;
        private Label _h2sLabel = null!;
        private Label _feedbackLabel = null!;
        private Button _feedButton = null!;
        private Button _scrubberButton = null!;
        private Button _compressButton = null!;
        private Button _ventButton = null!;
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

            _headerLabel = AshfallUiHelpers.MakeLabel("SHELTER BIO-ENERGY // ANAEROBIC BIOGAS DIGESTER (PWR-06)", 20, true);
            root.AddChild(_headerLabel);

            _statusLabel = AshfallUiHelpers.MakeSectionHeader("[STATUS: FERMENTING - MESOPHILIC DIGESTER: 38.2°C / CH4 PURITY: 96.5%]");
            _statusLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            root.AddChild(_statusLabel);

            root.AddChild(new HSeparator());

            var grid = new GridContainer { Columns = 2 };
            grid.AddThemeConstantOverride("h_separation", 24);
            grid.AddThemeConstantOverride("v_separation", 8);
            root.AddChild(grid);

            _tempLabel = AshfallUiHelpers.MakeBody("Slurry Temperature: 38.2°C (Optimal Mesophilic Range)");
            grid.AddChild(_tempLabel);

            _phLabel = AshfallUiHelpers.MakeBody("Digester Slurry pH: 7.25 (Methanogen Active)");
            grid.AddChild(_phLabel);

            _yieldLabel = AshfallUiHelpers.MakeBody("Biogas Generation Yield: 68.0 m³/day");
            grid.AddChild(_yieldLabel);

            _purityLabel = AshfallUiHelpers.MakeBody("Methane (CH4) Concentration: 96.5%");
            grid.AddChild(_purityLabel);

            _h2sLabel = AshfallUiHelpers.MakeBody("Iron Sponge H2S Saturation: 12 ppm (Safe)");
            grid.AddChild(_h2sLabel);

            root.AddChild(new HSeparator());

            var consoleBox = new HBoxContainer();
            consoleBox.AddThemeConstantOverride("separation", 12);
            root.AddChild(consoleBox);

            _feedButton = new Button { Text = "[FEED ORGANIC WASTE SLURRY]" };
            _feedButton.Pressed += () => ShowFeedback("Fed 50L organic biomass slurry to digester. Fermentation rate increased.");
            consoleBox.AddChild(_feedButton);

            _scrubberButton = new Button { Text = "[REGENERATE H2S SCRUBBER]" };
            _scrubberButton.Pressed += () => ShowFeedback("Iron sponge desulfurization media backwashed and regenerated.");
            consoleBox.AddChild(_scrubberButton);

            _compressButton = new Button { Text = "[COMPRESS METHANE TO CYLINDERS]" };
            _compressButton.Pressed += () => ShowFeedback("Compressed 20 m³ purified biomethane to 200-Bar buffer storage.");
            consoleBox.AddChild(_compressButton);

            _ventButton = new Button { Text = "[VENT OVERPRESSURE]" };
            _ventButton.Pressed += () => ShowFeedback("Emergency flare vent opened. Digester head pressure normalized.");
            consoleBox.AddChild(_ventButton);

            _closeButton = new Button { Text = "[CLOSE PANEL]" };
            _closeButton.Pressed += () => { Visible = false; OnClose?.Invoke(); };
            consoleBox.AddChild(_closeButton);

            _feedbackLabel = AshfallUiHelpers.MakeSmall("Ready. Digester operates continuously under anaerobic conditions.");
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
