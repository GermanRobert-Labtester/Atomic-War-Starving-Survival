// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class AquiferTreatyConcessionPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private Label? _headerTitleLabel;
        private Label? _statusBadgeLabel;
        private Button? _closeButton;
        private VBoxContainer? _telemetryContainer;
        private VBoxContainer? _buttonContainer;
        private VBoxContainer? _dataContainer;
        private Label? _logOutputLabel;

        public bool IsBound { get; private set; } = true;

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
                _statusBadgeLabel.Text = "STATUS: COMPACT RATIFIED - PURIFIED FLOW: 420 L/HR / TRIBUTE: 4 DAYS";
            }
        }

        private void BuildInterface()
        {
            var chrome = ThreePanePanelScaffold.BuildChrome(
                this,
                "DIPLOMATIC HYDRAULICS // HYDRO-BARONS AQUIFER TREATIES [AQUI-01]",
                "STATUS: COMPACT RATIFIED - PURIFIED FLOW: 420 L/HR / TRIBUTE: 4 DAYS",
                AshfallUiHelpers.ToColor(DesignTheme.Warm),
                "[X] CLOSE CONSOLE",
                "[AQUI-01] Siphon flow rate locked at 420.5 L/Hour.\n[AQUI-01] Tribute consignment verified by Hydro-Baron courier.",
                () => OnClose?.Invoke());
            _headerTitleLabel = chrome.Title;
            _statusBadgeLabel = chrome.Status;
            _closeButton = chrome.Close;
            _logOutputLabel = chrome.Log;
            var bodyHBox = chrome.Body;

            // Left Column (Telemetry)
            var leftPanel = ThreePanePanelScaffold.CreatePanelFrame("BASAL AQUIFER STRATA & SIPHON SENSORS");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = ThreePanePanelScaffold.CreateColumn(leftPanel, 8);
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("BASAL AQUIFER SIPHON DEPTH", "-620 METERS (STRATA 4)", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("WATER SALINITY PURITY", "120 ppm (POTABLE GRADE)", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("TRITIUM HEAVY ISOTOPE", "4.2 Bq/L [SAFE COMMERCE]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("HYDRO-BARON DIPLOMATIC FAVOR", "+45 [CONCESSIONARY ALLIANCE]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("WAR TENSION INDEX", "12% [CONTROLLED]", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

            // Center Column (Interactive Controls)
            var centerPanel = ThreePanePanelScaffold.CreatePanelFrame("MANIFOLD SLUICE VALVES & BYPASS");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = ThreePanePanelScaffold.CreateColumn(centerPanel, 12);
            _buttonContainer.AddChild(new Button { Text = "[RATIFY EMERGENCY WATER-FOR-AMMO ACCORD]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[THROTTLE TRIBUTARY BYPASS TO 50%]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[FLUSH CONTAMINATED SEDIMENT TRAP]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[DISPATCH NEGOTIATOR TO HYDRO-BARON CONCLAVE]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = ThreePanePanelScaffold.CreatePanelFrame("TRIBUTE VAULT & ARREARS LEDGER");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = ThreePanePanelScaffold.CreateColumn(rightPanel, 8);
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("7.62x39mm AMMO TRIBUTE", "450 ROUNDS CRATED", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("PURIFIED LEAD INGOTS", "120 KG IN ESCROW", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SEALED ANTIBIOTICS VIALS", "18 UNITS STORED", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("DEFAULT LOCK-OUT TIMER", "4 DAYS UNTIL SIPHON CLOSURE", AshfallUiHelpers.ToColor(DesignTheme.Hot)));

        }
    }
}
