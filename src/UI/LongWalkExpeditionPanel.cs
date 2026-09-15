// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class LongWalkExpeditionPanel : Control, IBindablePanel
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
                _statusBadgeLabel.Text = "STATUS: PILGRIMAGE ACTIVE - 1,240 KM / ATTRITION: HIGH";
            }
        }

        private void BuildInterface()
        {
            var chrome = ThreePanePanelScaffold.BuildChrome(
                this,
                "EXPEDITIONARY COMMAND // THE LONG WALK - FALLOUT CORRIDOR [EXP-06]",
                "STATUS: PILGRIMAGE ACTIVE - 1,240 KM / ATTRITION: HIGH",
                AshfallUiHelpers.ToColor(DesignTheme.Hot),
                "[X] CLOSE CONSOLE",
                "[EXP-06] Party crossed frozen irradiated riverbed.\n[EXP-06] Waypoint Beta bivouac established. Fire kindled.",
                () => OnClose?.Invoke());
            _headerTitleLabel = chrome.Title;
            _statusBadgeLabel = chrome.Status;
            _closeButton = chrome.Close;
            _logOutputLabel = chrome.Log;
            var bodyHBox = chrome.Body;

            // Left Column (Telemetry)
            var leftPanel = ThreePanePanelScaffold.CreatePanelFrame("CORRIDOR CONTOURS & RADIO RELAYS");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = ThreePanePanelScaffold.CreateColumn(leftPanel, 8);
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("TOTAL CORRIDOR DISTANCE", "1,240 KM TO OLD MERIDIAN", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("CURRENT MARCH DISTANCE", "412 KM AT GLASS BASIN", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RELAY GAMMA SIGNAL STRENGTH", "42% CARRIER LOCK", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RADIOACTIVE DUST FRONT", "+34 KM/H APPROACHING", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("FALLOUT EXPOSURE LEVEL", "12.4 R/HR ON ROUTE", AshfallUiHelpers.ToColor(DesignTheme.Critical)));

            // Center Column (Interactive Controls)
            var centerPanel = ThreePanePanelScaffold.CreatePanelFrame("PARTY METABOLICS & WAYPOINT DISPATCH");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = ThreePanePanelScaffold.CreateColumn(centerPanel, 12);
            _buttonContainer.AddChild(new Button { Text = "[DISPATCH AIRDROP RESUPPLY CRATE]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[AUTHORIZE NIGHT MARCH IN BLIZZARD]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[ORDER EMERGENCY RAD-X PURGE PROTOCOL]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[ESTABLISH REINFORCED WAYPOINT BIVOUAC]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = ThreePanePanelScaffold.CreatePanelFrame("SUPPLY CACHES & RADIO WIRETAP");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = ThreePanePanelScaffold.CreateColumn(rightPanel, 8);
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("WATER STOCKS (EXPEDITION)", "3.2 DAYS REMAINING", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RATION KITS (EXPEDITION)", "5.5 DAYS REMAINING", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RAD-X AMPULES IN FIELD", "8 DOSES REMAINING", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("CACHE 104 (IRON SIPHON)", "SEALED AND RECOVERED", AshfallUiHelpers.ToColor(DesignTheme.Warm)));

        }
    }
}
