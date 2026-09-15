// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class UltrasonicDecontaminationAirlockPanel : Control, IBindablePanel
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
                _statusBadgeLabel.Text = "STATUS: AIRLOCK CYCLE ACTIVE - HOT ZONE PURGE / DOSE: 18.2 mSv";
            }
        }

        private void BuildInterface()
        {
            var chrome = ThreePanePanelScaffold.BuildChrome(
                this,
                "RADIOLOGICAL DETOX // ULTRASONIC DELUGE AIRLOCK [RAD-05]",
                "STATUS: AIRLOCK CYCLE ACTIVE - HOT ZONE PURGE / DOSE: 18.2 mSv",
                AshfallUiHelpers.ToColor(DesignTheme.Critical),
                "[X] CLOSE CONSOLE",
                "[RAD-05] 32 kHz ultrasonic transducers energized for deluge cycle.\n[RAD-05] Survivor skin radiation reduced from 140 mSv to 1.2 mSv.",
                () => OnClose?.Invoke());
            _headerTitleLabel = chrome.Title;
            _statusBadgeLabel = chrome.Status;
            _closeButton = chrome.Close;
            _logOutputLabel = chrome.Log;
            var bodyHBox = chrome.Body;

            // Left Column (Telemetry)
            var leftPanel = ThreePanePanelScaffold.CreatePanelFrame("SURVIVOR ANATOMICAL DOSIMETRY");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = ThreePanePanelScaffold.CreateColumn(leftPanel, 8);
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("BOOTS CONTAMINATION HOTSPOT", "140 mSv/hr [SEVERE EXPOSURE]", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RESPIRATOR CANISTER DOSE", "42 mSv/hr [SATURATED]", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("HANDS AND FOREARMS", "28 mSv/hr [MODERATE]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("INTERNAL CHELATION DRIP", "PRUSSIAN BLUE / EDTA ACTIVE", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SURVIVOR IN AIRLOCK", "SCOUT VANE (EXPEDITION RETURN)", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

            // Center Column (Interactive Controls)
            var centerPanel = ThreePanePanelScaffold.CreatePanelFrame("DELUGE SPRAY NOZZLES & CAVITATION");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = ThreePanePanelScaffold.CreateColumn(centerPanel, 12);
            _buttonContainer.AddChild(new Button { Text = "[INITIATE HIGH-PRESSURE CHELATING WASH]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[PULSE 32 KHZ ULTRASONIC CAVITATION]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[INJECT PRESSURIZED FOAM SEALANT]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[PNEUMATIC BLOWER SUCTION PURGE]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = ThreePanePanelScaffold.CreatePanelFrame("EFFLUENT SUMP & RESIN FILTERS");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = ThreePanePanelScaffold.CreateColumn(rightPanel, 8);
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RADIOACTIVE SLUDGE SUMP", "1,240 L / 2,000 L (4,800 Bq/L)", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("FILTER BANK A (HEPA/RESIN)", "82% SATURATED (BACKWASH REQ)", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("FILTER BANK B (ACTIVATED)", "34% SATURATED [NOMINAL]", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("CLEAN HAZMAT SUIT RACK", "6 SUITS READY / 2 IN WASH", AshfallUiHelpers.ToColor(DesignTheme.Warm)));

        }
    }
}
