// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class CryogenicPermafrostCorePanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private Label? _headerTitleLabel;
        private Label? _statusBadgeLabel;
        private Button? _closeButton;
        private VBoxContainer? _telemetryContainer;
        private VBoxContainer? _buttonContainer;
        private VBoxContainer? _dataContainer;
        private Label? _logOutputLabel;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildInterface();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public bool IsBound { get; private set; } = true;

        public void Bind(object? session)
        {
            IsBound = true;
            RefreshView();
        }

        public void Unbind()
        {
            IsBound = false;
        }

        public void RefreshView()
        {
            if (_statusBadgeLabel != null)
            {
                _statusBadgeLabel.Text = "STATUS: DEEP FREEZE ALERT - PERMAFROST: -450M / CORE TEMP: -62.4C";
            }
        }

        private void BuildInterface()
        {
            var chrome = ThreePanePanelScaffold.BuildChrome(
                this,
                "SHELTER THERMAL DYNAMICS // CRYOGENIC PERMAFROST CORE [CRYO-02]",
                "STATUS: DEEP FREEZE ALERT - PERMAFROST: -450M / CORE TEMP: -62.4C",
                AshfallUiHelpers.ToColor(DesignTheme.Critical),
                "[X] CLOSE CONSOLE",
                "[CRYO-02] Permafrost creep penetrating sub-level 4 perimeter bulkhead.\n[CRYO-02] Resistive coils engaged in residential sector.",
                () => OnClose?.Invoke());
            _headerTitleLabel = chrome.Title;
            _statusBadgeLabel = chrome.Status;
            _closeButton = chrome.Close;
            _logOutputLabel = chrome.Log;
            var bodyHBox = chrome.Body;

            // Left Column (Telemetry)
            var leftPanel = ThreePanePanelScaffold.CreatePanelFrame("PERMAFROST STRATA & SECONDARY CRYO LOOPS");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = ThreePanePanelScaffold.CreateColumn(leftPanel, 8);
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("PERMAFROST ADVANCE DEPTH", "-450.2 METERS", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("FROST LINE CREEP VELOCITY", "+0.42 M/DAY", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("LOOP A (LIQUID N2 / GLYCOL)", "14.2 BAR @ -62.4C", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("LOOP B (GEOTHERMAL SINK)", "18.0 BAR @ -74.1C", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("GEOTHERMAL SINK FLUX", "142 kW / DEPLETION: 68%", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

            // Center Column (Interactive Controls)
            var centerPanel = ThreePanePanelScaffold.CreatePanelFrame("CRYOGENIC MANIFOLD & THERMAL INJECTION");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = ThreePanePanelScaffold.CreateColumn(centerPanel, 12);
            _buttonContainer.AddChild(new Button { Text = "[PULSE GEOTHERMAL THERMAL INJECTION]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[THROTTLE GLYCOL CHILLER PUMPS]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[ENGAGE EMERGENCY RESISTIVE HEATING COILS]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[VENT CRYOGENIC NITROGEN OVERPRESSURE]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = ThreePanePanelScaffold.CreatePanelFrame("THERMAL BALLAST & ATTRITION RISKS");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = ThreePanePanelScaffold.CreateColumn(rightPanel, 8);
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("DIESEL BALLAST RESERVES", "1,240 LITERS (120 L/DAY)", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("COAL SLURRY STOCKPILE", "4,200 KG (450 KG/DAY)", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SECTOR 04 HYPOTHERMIA RISK", "42.5% [CRITICAL EXPOSURE]", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RESISTIVE HEATER DRAW", "34.5 kW [ACTIVE]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));

        }
    }
}
