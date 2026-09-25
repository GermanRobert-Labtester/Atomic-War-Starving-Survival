// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using Ashfall.Core.Expeditions;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Long-Range Reconnaissance Drones &amp; High-Altitude Mapping.
    /// Bound to ReconTelemetryHostSession. Actions route through Main.HandleReconTelemetryAction.
    /// </summary>
    public partial class ReconTelemetryPanel : Control, IBindablePanel
    {
        public event Action<string, string>? OnActionRequested;
        public event Action? OnClose;

        private ReconTelemetryHostSession? _host;
        private Label _headerLabel = null!;
        private Label _statusLabel = null!;
        private Label _missionsLabel = null!;
        private Label _sectorsLabel = null!;
        private Label _platformsLabel = null!;
        private Label _forecastsLabel = null!;
        private Label _feedbackLabel = null!;
        private Button _launchBtn = null!;
        private Button _surveyBtn = null!;
        private Button _recoverBtn = null!;
        private Button _forecastBtn = null!;
        private Button _scoutBtn = null!;
        private Button _closeButton = null!;

        public bool IsBound => _host != null;
        public int SimDay { get; set; } = 1;

        public void Bind(ReconTelemetryHostSession session)
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

            _headerLabel = AshfallUiHelpers.MakeLabel("EXPEDITIONS // RECON TELEMETRY", 20, true);
            root.AddChild(_headerLabel);

            _statusLabel = AshfallUiHelpers.MakeSectionHeader("[STATUS: IDLE]");
            root.AddChild(_statusLabel);

            root.AddChild(new HSeparator());

            var grid = new GridContainer { Columns = 2 };
            grid.AddThemeConstantOverride("h_separation", 24);
            grid.AddThemeConstantOverride("v_separation", 8);
            root.AddChild(grid);

            _missionsLabel = AshfallUiHelpers.MakeBody("Active Missions: 0");
            grid.AddChild(_missionsLabel);

            _sectorsLabel = AshfallUiHelpers.MakeBody("Surveyed Sectors: 0");
            grid.AddChild(_sectorsLabel);

            _platformsLabel = AshfallUiHelpers.MakeBody("Launched Platforms: 0");
            grid.AddChild(_platformsLabel);

            _forecastsLabel = AshfallUiHelpers.MakeBody("Active Forecasts: 0");
            grid.AddChild(_forecastsLabel);

            root.AddChild(new HSeparator());

            var consoleBox = new HBoxContainer();
            consoleBox.AddThemeConstantOverride("separation", 12);
            root.AddChild(consoleBox);

            _launchBtn = AshfallUiHelpers.MakeButton("LAUNCH PROBE", () => OnActionRequested?.Invoke("launch", "probe_short_range_quad_uav"));
            consoleBox.AddChild(_launchBtn);

            _surveyBtn = AshfallUiHelpers.MakeButton("SURVEY SECTORS", () => OnActionRequested?.Invoke("survey", "loc_holdfast"));
            consoleBox.AddChild(_surveyBtn);

            _recoverBtn = AshfallUiHelpers.MakeButton("RECOVER PLATFORM", () => OnActionRequested?.Invoke("recover", ""));
            consoleBox.AddChild(_recoverBtn);

            _forecastBtn = AshfallUiHelpers.MakeButton("GENERATE FORECAST", () => OnActionRequested?.Invoke("forecast", ""));
            consoleBox.AddChild(_forecastBtn);

            _scoutBtn = AshfallUiHelpers.MakeButton("SCOUT ROUTE", () => OnActionRequested?.Invoke("scout", "route_ice_road"));
            consoleBox.AddChild(_scoutBtn);

            _closeButton = new Button { Text = "[CLOSE PANEL]" };
            _closeButton.Pressed += () => { Visible = false; OnClose?.Invoke(); };
            consoleBox.AddChild(_closeButton);

            _feedbackLabel = AshfallUiHelpers.MakeSmall("Recon systems standby. Launch a probe to begin.");
            _feedbackLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
            root.AddChild(_feedbackLabel);
        }

        public void RefreshView()
        {
            if (_host == null) return;
            var state = _host.State;

            _missionsLabel.Text = $"Active Missions: {state.activeMissions.Count}";
            _sectorsLabel.Text = $"Surveyed Sectors: {state.surveyedSectorIds.Count}";
            _platformsLabel.Text = $"Launched Platforms: {state.launchedPlatformIds.Count}";
            _forecastsLabel.Text = $"Active Forecasts: {state.forecasts.Count}";

            string status = state.activeMissions.Count > 0 ? "ACTIVE" : "IDLE";
            _statusLabel.Text = $"[STATUS: {status}]";

            _launchBtn.Disabled = state.activeMissions.Count > 0;
            _surveyBtn.Disabled = state.activeMissions.Count == 0;
            _recoverBtn.Disabled = state.activeMissions.Count == 0;
            _forecastBtn.Disabled = state.launchedPlatformIds.Count == 0;
            _scoutBtn.Disabled = state.activeMissions.Count == 0;
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
