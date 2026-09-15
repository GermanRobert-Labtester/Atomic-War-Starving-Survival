// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class IronCenotaphMemorialPanel : Control, IBindablePanel
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
                _statusBadgeLabel.Text = "STATUS: MEMORIAL FLAME ACTIVE - RECORDED: 38 SOULS (-18% GRIEF)";
            }
        }

        private void BuildInterface()
        {
            var chrome = ThreePanePanelScaffold.BuildChrome(
                this,
                "SHELTER COMMEMORATION // THE IRON CENOTAPH [RIT-01]",
                "STATUS: MEMORIAL FLAME ACTIVE - RECORDED: 38 SOULS (-18% GRIEF)",
                AshfallUiHelpers.ToColor(DesignTheme.Warm),
                "[X] CLOSE CONSOLE",
                "[RIT-01] Bronze plaque carved for Dr. Aris Thorne.\n[RIT-01] Memorial vigil conducted. Living survivor grief mitigated.",
                () => OnClose?.Invoke());
            _headerTitleLabel = chrome.Title;
            _statusBadgeLabel = chrome.Status;
            _closeButton = chrome.Close;
            _logOutputLabel = chrome.Log;
            var bodyHBox = chrome.Body;

            // Left Column (Telemetry)
            var leftPanel = ThreePanePanelScaffold.CreatePanelFrame("CASUALTY ROLL & MORTALITY CAUSES");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = ThreePanePanelScaffold.CreateColumn(leftPanel, 8);
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RECORDED CASUALTIES", "38 SOULS ON MEMORIAL WALL", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RADIATION POISONING", "42.0% OF ALL DEATHS", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("STARVATION & DEHYDRATION", "24.0% OF ALL DEATHS", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("COMBAT & TRAUMA", "34.0% OF ALL DEATHS", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("WALL PLAQUE CAPACITY", "38 / 80 SLOTS OCCUPIED", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

            // Center Column (Interactive Controls)
            var centerPanel = ThreePanePanelScaffold.CreatePanelFrame("BRONZE EPITAPH ENGRAVER & ETERNAL FLAME");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = ThreePanePanelScaffold.CreateColumn(centerPanel, 12);
            _buttonContainer.AddChild(new Button { Text = "[ENGRAVE BRONZE MEMORIAL PLAQUE]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[REFUEL ETERNAL MEMORIAL FLAME]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[HOLD ALL-SHELTER VIGIL & MOMENT OF SILENCE]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[RECITE DIEGETIC COMMEMORATION EULOGY]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = ThreePanePanelScaffold.CreatePanelFrame("MEMORIAL RELICS & VIGIL ATTENDANCE");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = ThreePanePanelScaffold.CreateColumn(rightPanel, 8);
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("ETERNAL OIL FLAME RESERVOIR", "42.5 LITERS (0.2 L/DAY)", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("MEMORIAL RELICS PRESERVED", "24 DOG TAGS / 6 WATCHES", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SHELTER GRIEF MITIGATION", "-18.5% DESPAIR INDEX", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("LAST VIGIL ATTENDANCE", "94% OF LIVING POPULATION", AshfallUiHelpers.ToColor(DesignTheme.Warm)));

        }
    }
}
