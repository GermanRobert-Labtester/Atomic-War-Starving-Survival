// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class BasalRadonMigrationPanel : Control, IBindablePanel
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
                _statusBadgeLabel.Text = "STATUS: RADON INVERSION ACTIVE - SUMP: 3,420 Bq/m3 (CRITICAL)";
            }
        }

        private void BuildInterface()
        {
            var chrome = ThreePanePanelScaffold.BuildChrome(
                this,
                "SHELTER RADIOLOGY // BASAL RADON INVERSION & STRATA MIGRATION [RAD-04]",
                "STATUS: RADON INVERSION ACTIVE - SUMP: 3,420 Bq/m3 (CRITICAL)",
                AshfallUiHelpers.ToColor(DesignTheme.Critical),
                "[X] CLOSE CONSOLE",
                "[RAD-04] Tectonic fissure degassing detected beneath sub-level 4.\n[RAD-04] Exhaust blower velocity set to maximum negative draft.",
                () => OnClose?.Invoke());
            _headerTitleLabel = chrome.Title;
            _statusBadgeLabel = chrome.Status;
            _closeButton = chrome.Close;
            _logOutputLabel = chrome.Log;
            var bodyHBox = chrome.Body;

            // Left Column (Telemetry)
            var leftPanel = ThreePanePanelScaffold.CreatePanelFrame("GEOLOGICAL RADON DIFFUSION & SCINTILLATION");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = ThreePanePanelScaffold.CreateColumn(leftPanel, 8);
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SUMP SCINTILLATION CHAMBER", "3,420 Bq/m3 [CRITICAL]", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SUB-LEVEL 3 AIR DUCTWAYS", "1,240 Bq/m3 [HIGH]", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RESIDENTIAL BARRACKS", "380 Bq/m3 [MODERATE]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("222Rn ISOTOPE HALF-LIFE", "3.82 DAYS (DECAY ACTIVE)", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("BASAL BAROMETRIC INVERSION", "-18.4 mbar [NEGATIVE]", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

            // Center Column (Interactive Controls)
            var centerPanel = ThreePanePanelScaffold.CreatePanelFrame("VENTILATION CURTAINS & SUMP EXTRACTION");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = ThreePanePanelScaffold.CreateColumn(centerPanel, 12);
            _buttonContainer.AddChild(new Button { Text = "[MAXIMIZE SUMP EXHAUST BLOWERS]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[PULSE POSITIVE PRESSURE AIR CURTAIN]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[BACKWASH ACTIVATED CHARCOAL FILTER BED]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[ENGAGE EMERGENCY SECTOR 08 SEAL]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = ThreePanePanelScaffold.CreatePanelFrame("BIOLOGICAL LUNG DOSE & FILTER INVENTORY");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = ThreePanePanelScaffold.CreateColumn(rightPanel, 8);
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("ALPHA DECAY LUNG BURDEN", "142 mSv/SURVIVOR [HIGH]", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("POLONIUM-218 SATURATION", "84.2% IN EXHAUST PLUME", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("INTACT RESPIRATORY MASKS", "14 / 18 SURVIVORS", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("REPLACEMENT CHARCOAL CANISTERS", "28 REMAINING", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

        }
    }
}
