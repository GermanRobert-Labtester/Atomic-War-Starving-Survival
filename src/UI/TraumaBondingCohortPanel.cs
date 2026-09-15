// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class TraumaBondingCohortPanel : Control, IBindablePanel
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
                _statusBadgeLabel.Text = "STATUS: COHORT STRESS CRITICAL - TRAUMA RESONANCE: 78.4%";
            }
        }

        private void BuildInterface()
        {
            var chrome = ThreePanePanelScaffold.BuildChrome(
                this,
                "SHELTER SOCIAL PSYCHOLOGY // TRAUMA BONDING & COHORT MATRIX [SOC-03]",
                "STATUS: COHORT STRESS CRITICAL - TRAUMA RESONANCE: 78.4%",
                AshfallUiHelpers.ToColor(DesignTheme.Hot),
                "[X] CLOSE CONSOLE",
                "[SOC-03] Rostova and Vane formed mutual grief pact.\n[SOC-03] Morale de-escalation protocol lowered conflict tension.",
                () => OnClose?.Invoke());
            _headerTitleLabel = chrome.Title;
            _statusBadgeLabel = chrome.Status;
            _closeButton = chrome.Close;
            _logOutputLabel = chrome.Log;
            var bodyHBox = chrome.Body;

            // Left Column (Telemetry)
            var leftPanel = ThreePanePanelScaffold.CreatePanelFrame("SURVIVOR PSYCHOMETRICS & CRISIS VECTORS");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = ThreePanePanelScaffold.CreateColumn(leftPanel, 8);
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SHARED TRAUMA RESONANCE", "78.4% [SEVERE STRAIN]", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("VANE - ROSTOVA BOND INDEX", "+0.65 [MUTUAL LOSS]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("MILLER - CARVER ANTAGONISM", "-0.82 [FOUNDRY ACCIDENT]", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("PANIC CONTAGION RISK", "44.0% [ISOLATION NEEDED]", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("COLLECTIVE SHELTER GRIEF", "88.2% [DESPAIR SPIKE]", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

            // Center Column (Interactive Controls)
            var centerPanel = ThreePanePanelScaffold.CreatePanelFrame("COHORT COHESION & SOCIAL INTERVENTIONS");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = ThreePanePanelScaffold.CreateColumn(centerPanel, 12);
            _buttonContainer.AddChild(new Button { Text = "[ADMINISTER EMOTIONAL DE-ESCALATION TALK]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[DISTRIBUTE EXTRA PHARMA SEDATIVES]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[SEPARATE SEDITIOUS CONFLICT PAIRS]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[ORDER MANDATORY RECREATION & VINYL LISTENING]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = ThreePanePanelScaffold.CreatePanelFrame("GRIEVANCE LEDGER & PHARMA STOCKS");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = ThreePanePanelScaffold.CreateColumn(rightPanel, 8);
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("COHORT ALPHA (VETERANS)", "88% COHESION / STABLE", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("COHORT BETA (FOUNDRY)", "42% COHESION / FRACTURED", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("COHORT GAMMA (BEREAVED)", "94% COHESION / UNITED", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("DIAZEPAM / HALOPERIDOL VIALS", "14 DOSES REMAINING", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

        }
    }
}
