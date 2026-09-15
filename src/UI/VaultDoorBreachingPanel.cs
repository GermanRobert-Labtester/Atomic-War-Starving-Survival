// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class VaultDoorBreachingPanel : Control, IBindablePanel
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
                _statusBadgeLabel.Text = "STATUS: BREACH ACTIVE - DOOR 04-A / DEADBOLT: 42.6%";
            }
        }

        private void BuildInterface()
        {
            var chrome = ThreePanePanelScaffold.BuildChrome(
                this,
                "TACTICAL BREACHING // ARMORED VAULT DOOR RIG [SEC-03]",
                "STATUS: BREACH ACTIVE - DOOR 04-A / DEADBOLT: 42.6%",
                AshfallUiHelpers.ToColor(DesignTheme.Hot),
                "[X] CLOSE CONSOLE",
                "[SEC-03] Thermal lance ignited at 2,840C. Lead layer melting.\n[SEC-03] Hydraulic rams pressurized to 480 Bar.",
                () => OnClose?.Invoke());
            _headerTitleLabel = chrome.Title;
            _statusBadgeLabel = chrome.Status;
            _closeButton = chrome.Close;
            _logOutputLabel = chrome.Log;
            var bodyHBox = chrome.Body;

            // Left Column (Telemetry)
            var leftPanel = ThreePanePanelScaffold.CreatePanelFrame("LOCK TUMBLER ACOUSTIC STETHOSCOPE");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = ThreePanePanelScaffold.CreateColumn(leftPanel, 8);
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("6-PIN TUMBLER ALIGNMENT", "PINS 1,2,4 ALIGNED / 3,5,6 BINDING", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SHEAR FREQUENCY SCANNER", "1,420 Hz RESONANT NOTCH", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("TENSION WRENCH TORQUE", "14.5 Nm APPLIED", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("THERMAL LANCE CORE TEMP", "2,840C @ 42 BAR O2", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("HYDRAULIC RAM DISPLACEMENT", "142mm / 480 BAR PRESSURE", AshfallUiHelpers.ToColor(DesignTheme.Warm)));

            // Center Column (Interactive Controls)
            var centerPanel = ThreePanePanelScaffold.CreatePanelFrame("HYDRAULIC RAMS & THERMAL LANCE");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = ThreePanePanelScaffold.CreateColumn(centerPanel, 12);
            _buttonContainer.AddChild(new Button { Text = "[IGNITE MAGNESIUM THERMAL LANCE]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[PRESSURIZE 500 BAR HYDRAULIC RAMS]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[PULSE ACOUSTIC SHEAR RESONATOR]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[DEPLOY EXPLOSIVE LINEAR SHAPED CHARGE]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = ThreePanePanelScaffold.CreatePanelFrame("DOOR INTEGRITY & CONSUMABLES");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = ThreePanePanelScaffold.CreateColumn(rightPanel, 8);
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("OUTER ARMOR PENETRATION", "100% BREACHED", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("LEAD CORE BURN DEPTH", "68.5% CUT THROUGH", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("INNER TUNGSTEN PLATE", "22.0% INTEGRITY LEFT", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("MAGNESIUM THERMAL RODS", "3 REMAINING IN RIG", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

        }
    }
}
