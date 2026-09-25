// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using Ashfall.Core.Shelter;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Deep Geothermal Boreholes & Clean Aquifer Pumping.
    /// Bound to GeothermalAquiferHostSession. Actions route through Main.HandleGeothermalAction.
    /// </summary>
    public partial class GeothermalAquiferPanel : Control, IBindablePanel
    {
        public event Action<string, string>? OnActionRequested;
        public event Action? OnClose;

        private GeothermalAquiferHostSession? _host;
        private Label _headerLabel = null!;
        private Label _statusLabel = null!;
        private Label _depthLabel = null!;
        private Label _strataLabel = null!;
        private Label _turbineLabel = null!;
        private Label _pressureLabel = null!;
        private Label _scaleLabel = null!;
        private Label _feedbackLabel = null!;
        private Button _drillButton = null!;
        private Button _casingButton = null!;
        private Button _turbineButton = null!;
        private Button _descaleButton = null!;
        private Button _ventButton = null!;
        private Button _tapButton = null!;
        private Button _closeButton = null!;

        public bool IsBound => _host != null;
        public int SimDay { get; set; } = 1;

        public void Bind(GeothermalAquiferHostSession session)
        {
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
            }
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
                _host = null;
            }
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            var bg = new ColorRect { Color = AshfallUiHelpers.ToColor(DesignTheme.Ink) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var margin = AshfallUiHelpers.MakeMargins(16);
            AddChild(margin);

            var root = new VBoxContainer();
            root.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            margin.AddChild(root);

            _headerLabel = AshfallUiHelpers.MakeLabel("INFRASTRUCTURE // DEEP GEOTHERMAL BOREHOLES", 20, true);
            root.AddChild(_headerLabel);

            _statusLabel = AshfallUiHelpers.MakeSectionHeader("[STATUS: IDLE]");
            root.AddChild(_statusLabel);

            root.AddChild(new HSeparator());

            var grid = new GridContainer { Columns = 2 };
            grid.AddThemeConstantOverride("h_separation", 24);
            grid.AddThemeConstantOverride("v_separation", 8);
            root.AddChild(grid);

            _depthLabel = AshfallUiHelpers.MakeBody("Borehole Depth: 0 m");
            grid.AddChild(_depthLabel);

            _strataLabel = AshfallUiHelpers.MakeBody("Strata: —");
            grid.AddChild(_strataLabel);

            _turbineLabel = AshfallUiHelpers.MakeBody("Turbine: Not commissioned");
            grid.AddChild(_turbineLabel);

            _pressureLabel = AshfallUiHelpers.MakeBody("Steam Pressure: 0 PSI");
            grid.AddChild(_pressureLabel);

            _scaleLabel = AshfallUiHelpers.MakeBody("Mineral Scaling: 0%");
            grid.AddChild(_scaleLabel);

            root.AddChild(new HSeparator());

            var consoleBox = new HBoxContainer();
            consoleBox.AddThemeConstantOverride("separation", 12);
            root.AddChild(consoleBox);

            _drillButton = AshfallUiHelpers.MakeButton("START DRILLING", () => OnActionRequested?.Invoke("start_drilling", ""));
            consoleBox.AddChild(_drillButton);

            _casingButton = AshfallUiHelpers.MakeButton("INSTALL CASING (100m)", () => OnActionRequested?.Invoke("install_casing", "100"));
            consoleBox.AddChild(_casingButton);

            _turbineButton = AshfallUiHelpers.MakeButton("COMMISSION TURBINE", () => OnActionRequested?.Invoke("commission_turbine", ""));
            consoleBox.AddChild(_turbineButton);

            _descaleButton = AshfallUiHelpers.MakeButton("DESCALE SYSTEM", () => OnActionRequested?.Invoke("descale", ""));
            consoleBox.AddChild(_descaleButton);

            _ventButton = AshfallUiHelpers.MakeButton("VENT PRESSURE", () => OnActionRequested?.Invoke("vent_pressure", ""));
            consoleBox.AddChild(_ventButton);

            _tapButton = AshfallUiHelpers.MakeButton("TAP AQUIFER", () => OnActionRequested?.Invoke("tap_aquifer", ""));
            consoleBox.AddChild(_tapButton);

            _closeButton = new Button { Text = "[CLOSE PANEL]" };
            _closeButton.Pressed += () => { Visible = false; OnClose?.Invoke(); };
            consoleBox.AddChild(_closeButton);

            _feedbackLabel = AshfallUiHelpers.MakeSmall("Borehole idle. Begin drilling to reach productive strata.");
            _feedbackLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
            root.AddChild(_feedbackLabel);
        }

        public void RefreshView()
        {
            if (_host == null) return;
            var state = _host.State;
            var currentStrata = _host.System.GetCurrentStrata();

            _depthLabel.Text = $"Borehole Depth: {state.currentDepthMeters:F0} m";
            _strataLabel.Text = $"Strata: {currentStrata?.DisplayName ?? "—"}";
            _turbineLabel.Text = $"Turbine: {(state.turbineCommissioned ? "Commissioned" : "Not commissioned")}";
            _pressureLabel.Text = $"Steam Pressure: {state.steamPressurePsi:F0} PSI";
            _scaleLabel.Text = $"Mineral Scaling: {state.mineralScaling:F0}%";

            string status = state.projectActive ? "DRILLING" :
                            state.turbineCommissioned ? "GENERATING" : "IDLE";
            _statusLabel.Text = $"[STATUS: {status}]";

            // Button states
            _drillButton.Disabled = state.projectActive || state.drillBitCondition <= 0f;
            _casingButton.Disabled = !state.projectActive;
            _turbineButton.Disabled = state.turbineCommissioned;
            _descaleButton.Disabled = state.mineralScaling <= 0f;
            _ventButton.Disabled = state.steamPressurePsi <= 0f;
            _tapButton.Disabled = state.aquiferTapped;
        }

        public void Open()
        {
            Visible = true;
        }

        public void ShowFeedback(string msg)
        {
            if (_feedbackLabel != null)
            {
                _feedbackLabel.Text = msg;
                _feedbackLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
            }
        }
    }
}
