// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class TroposphericRadioRelayPanel : Control, IBindablePanel
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
                _statusBadgeLabel.Text = "STATUS: TROPOSCATTER LOCKED - SECTOR 7 DISTANT BEACON (88.4%)";
            }
        }

        private void BuildInterface()
        {
            var chrome = ThreePanePanelScaffold.BuildChrome(
                this,
                "RADIO TELEMETRY // TROPOSPHERIC SCATTER RELAY [COMM-03]",
                "STATUS: TROPOSCATTER LOCKED - SECTOR 7 DISTANT BEACON (88.4%)",
                AshfallUiHelpers.ToColor(DesignTheme.Warm),
                "[X] CLOSE CONSOLE",
                "[COMM-03] 10kW Klystron tube energized. High-power troposcatter active.\n[COMM-03] Decrypted Morse teletype stream from Meridian Relay.",
                () => OnClose?.Invoke());
            _headerTitleLabel = chrome.Title;
            _statusBadgeLabel = chrome.Status;
            _closeButton = chrome.Close;
            _logOutputLabel = chrome.Log;
            var bodyHBox = chrome.Body;

            // Left Column (Telemetry)
            var leftPanel = ThreePanePanelScaffold.CreatePanelFrame("ATMOSPHERIC IONIZATION & FREQUENCIES");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = ThreePanePanelScaffold.CreateColumn(leftPanel, 8);
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("TROPOSPHERIC DUCTING INDEX", "1.42 [OPTIMAL PROPAGATION]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("IONOSPHERIC ALTITUDE", "85 KM REFLECTION LAYER", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("BAND 1 (MERIDIAN SIPHON)", "42.500 MHz [CARRIER LOCKED]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("BAND 2 (RUST GUILD BEACON)", "104.200 MHz [STRONG CARRIER]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SIGNAL-TO-NOISE RATIO", "18.5 dB [CLEAR DECODE]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));

            // Center Column (Interactive Controls)
            var centerPanel = ThreePanePanelScaffold.CreatePanelFrame("PARABOLIC DISH & KLYSTRON AMP");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = ThreePanePanelScaffold.CreateColumn(centerPanel, 12);
            _buttonContainer.AddChild(new Button { Text = "[ALIGN PARABOLIC DISH AZIMUTH]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[ENGAGE 10KW KLYSTRON TRANSMITTER]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[FILTER ATMOSPHERIC NOISE SPIKE]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[RECORD ENCRYPTED DIEGETIC BROADCAST]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = ThreePanePanelScaffold.CreatePanelFrame("DECRYPTED TELETYPE & ACTIVE NODES");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = ThreePanePanelScaffold.CreateColumn(rightPanel, 8);
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("TRANSMITTER ECHO-01", "OLD RADIO MAST (ONLINE)", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("STATION K-9", "FOUNDRY LOOP (INTERMITTENT)", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("LOADED CIPHER KEY", "KEY #04 IRON-7 [AUTHENTIC]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("DECRYPTED BUFFER", "WATER RATION REVISED -20%", AshfallUiHelpers.ToColor(DesignTheme.Hot)));

        }
    }
}
