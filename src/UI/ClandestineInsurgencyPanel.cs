// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class ClandestineInsurgencyPanel : Control, IBindablePanel
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
                _statusBadgeLabel.Text = "STATUS: ACTIVE SUBVERSION - CELL ALPHA RED DAWN DETECTED";
            }
        }

        private void BuildInterface()
        {
            var chrome = ThreePanePanelScaffold.BuildChrome(
                this,
                "SHELTER COUNTER-INSURGENCY // SEDITIOUS NETWORK MONITORING [POL-02]",
                "STATUS: ACTIVE SUBVERSION - CELL ALPHA RED DAWN DETECTED",
                AshfallUiHelpers.ToColor(DesignTheme.Critical),
                "[X] CLOSE CONSOLE",
                "[POL-02] Wiretap recorded rendezvous at Sump Pipe 4.\n[POL-02] Curfew declared in Sector 03. Insurgency score reduced.",
                () => OnClose?.Invoke());
            _headerTitleLabel = chrome.Title;
            _statusBadgeLabel = chrome.Status;
            _closeButton = chrome.Close;
            _logOutputLabel = chrome.Log;
            var bodyHBox = chrome.Body;

            // Left Column (Telemetry)
            var leftPanel = ThreePanePanelScaffold.CreatePanelFrame("INSURGENT CELLS & PIRATE BROADCASTS");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = ThreePanePanelScaffold.CreateColumn(leftPanel, 8);
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("CELL ALPHA RED DAWN", "4 OPERATIVES (ACTIVE)", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("CELL BETA THE FREE ASH", "3 OPERATIVES (MONITORED)", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("PIRATE RADIO FREQUENCY", "148.250 MHz [INTERCEPTED]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SECTOR 03 LOYALTY INDEX", "34.0% [REBELLION HOTSPOT]", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("POWER BREAKER SABOTAGE RISK", "85.0% [SEVERE VULNERABILITY]", AshfallUiHelpers.ToColor(DesignTheme.Hot)));

            // Center Column (Interactive Controls)
            var centerPanel = ThreePanePanelScaffold.CreatePanelFrame("WIRETAPS & TACTICAL INTERDICTION");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = ThreePanePanelScaffold.CreateColumn(centerPanel, 12);
            _buttonContainer.AddChild(new Button { Text = "[RAID SECTOR 03 HIDEOUT]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[INTERROGATE SUSPECTED REBEL AGENTS]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[IMPOSE EMERGENCY SECTOR 03 CURFEW]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[CONFISCATE COUNTERFEIT RATION STAMPS]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = ThreePanePanelScaffold.CreatePanelFrame("DETENTION BLOCK & CONFISCATED EVIDENCE");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = ThreePanePanelScaffold.CreateColumn(rightPanel, 8);
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("HOLDING CELLS OCCUPANCY", "3 / 6 DETAINEES (VARGA, KOREN)", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("CONFESSION PROBABILITY", "74.5% UNDER INTERROGATION", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("CONFISCATED PAMPHLETS", "142 SEDITIOUS LEAFLETS", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("BLACK-MARKET DETONATORS", "4 CONFISCATED IN FOUNDRY", AshfallUiHelpers.ToColor(DesignTheme.Critical)));

        }
    }
}
