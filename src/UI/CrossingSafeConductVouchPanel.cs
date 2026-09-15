// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class CrossingSafeConductVouchPanel : Control, IBindablePanel
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
                _statusBadgeLabel.Text = "STATUS: BRIDGEHEAD ARMED - TRANSIT PERMITS: 14 / SURCHARGE: +25%";
            }
        }

        private void BuildInterface()
        {
            var chrome = ThreePanePanelScaffold.BuildChrome(
                this,
                "FRONTIER CHECKPOINT // CROSSING SAFE-CONDUCT VOUCH [VOUCH-01]",
                "STATUS: BRIDGEHEAD ARMED - TRANSIT PERMITS: 14 / SURCHARGE: +25%",
                AshfallUiHelpers.ToColor(DesignTheme.Warm),
                "[X] CLOSE CONSOLE",
                "[VOUCH-01] Caravan Salt Walker cleared checkpoint. Toll paid 150 Scrip.\n[VOUCH-01] Contraband scan negative on sector gate 2.",
                () => OnClose?.Invoke());
            _headerTitleLabel = chrome.Title;
            _statusBadgeLabel = chrome.Status;
            _closeButton = chrome.Close;
            _logOutputLabel = chrome.Log;
            var bodyHBox = chrome.Body;

            // Left Column (Telemetry)
            var leftPanel = ThreePanePanelScaffold.CreatePanelFrame("RIVER GORGE SECTOR GRID & QUEUE");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = ThreePanePanelScaffold.CreateColumn(leftPanel, 8);
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SENTRY ALERTNESS INDEX", "92% [LETHAL FORCE AUTHORIZED]", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("INCOMING CARAVAN", "SALT WALKER (6 PACK BEASTS)", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SUSPICIOUS DESERTER CELL", "GRAY RATS (3 ARMED MEN)", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("CONTRABAND SCANNER", "NO ACTIVE ISOTOPES DETECTED", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("GORGE WIND VELOCITY", "42 KM/H CROSSWIND", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

            // Center Column (Interactive Controls)
            var centerPanel = ThreePanePanelScaffold.CreatePanelFrame("PARCHMENT STAMP & TARIFF CALCULATOR");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = ThreePanePanelScaffold.CreateColumn(centerPanel, 12);
            _buttonContainer.AddChild(new Button { Text = "[STAMP AUTHORIZED SAFE-CONDUCT PASS]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[CONFISCATE CONTRABAND & DETAIN]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[RAISE HEAVY SPIKE BARRIER]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[FIRE WARNING SHOT ACROSS GORGE]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = ThreePanePanelScaffold.CreatePanelFrame("TOLL VAULT & REFUGEE QUOTA");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = ThreePanePanelScaffold.CreateColumn(rightPanel, 8);
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("COLLECTED TOLL SCRIP", "1,840 SCRIP IN VAULT", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("CONFISCATED WEAPONS", "6 RIFLES / 1 DYNAMITE CRATE", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("DAILY REFUGEE QUOTA", "14 / 20 PERMITS ISSUED", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RUST GUILD STANDING", "+60 [PREFERENTIAL RATE]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));

        }
    }
}
