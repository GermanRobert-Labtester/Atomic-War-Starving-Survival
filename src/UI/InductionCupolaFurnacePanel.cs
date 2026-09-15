// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class InductionCupolaFurnacePanel : Control, IBindablePanel
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
                _statusBadgeLabel.Text = "STATUS: SMELTING ACTIVE - MOLTEN HIGH-TUNGSTEN ALLOY / 1,640C";
            }
        }

        private void BuildInterface()
        {
            var chrome = ThreePanePanelScaffold.BuildChrome(
                this,
                "SUBTERRANEAN METALLURGY // INDUCTION CUPOLA FURNACE [MET-03]",
                "STATUS: SMELTING ACTIVE - MOLTEN HIGH-TUNGSTEN ALLOY / 1,640C",
                AshfallUiHelpers.ToColor(DesignTheme.Critical),
                "[X] CLOSE CONSOLE",
                "[MET-03] Induction coils drew 850 kW. Core melt reached 1,640C.\n[MET-03] 24 armor-grade steel billets poured into casting molds.",
                () => OnClose?.Invoke());
            _headerTitleLabel = chrome.Title;
            _statusBadgeLabel = chrome.Status;
            _closeButton = chrome.Close;
            _logOutputLabel = chrome.Log;
            var bodyHBox = chrome.Body;

            // Left Column (Telemetry)
            var leftPanel = ThreePanePanelScaffold.CreatePanelFrame("CRUCIBLE INDUCTION COILS & FLUX");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = ThreePanePanelScaffold.CreateColumn(leftPanel, 8);
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("CORE MELT TEMPERATURE", "1,640C @ 850 kW INDUCTION", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("REFRACTORY LINING WEAR", "18.5% (ZIRCONIA BRICK OK)", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("MAGNESIUM INJECTION FLUX", "14.2 KG/CHARGE [DESULFURIZING]", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SLAG VISCOSITY INDEX", "2.1 Poise [FLUID DRAIN]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("MAGNETIC STIRRING FREQ", "60 Hz AC [HOMOGENEOUS]", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

            // Center Column (Interactive Controls)
            var centerPanel = ThreePanePanelScaffold.CreatePanelFrame("TILTING HYDRAULICS & INGOT POURING");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = ThreePanePanelScaffold.CreateColumn(centerPanel, 12);
            _buttonContainer.AddChild(new Button { Text = "[ACTIVATE 850 KW INDUCTION COILS]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[TILT CRUCIBLE FOR MOLTEN POUR]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[INJECT MAGNESIUM DESULFURIZER]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[SKIM ACCUMULATED SURFACE SLAG]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = ThreePanePanelScaffold.CreatePanelFrame("CAST INGOT INVENTORY & ALLOY STOCK");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = ThreePanePanelScaffold.CreateColumn(rightPanel, 8);
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("ARMOR-GRADE STEEL BILLETS", "24 INGOTS (1,200 KG)", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("LEAD-TUNGSTEN RADIATION SHIELDS", "8 PLATES CAST", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RAW SCRAP CHARGE HOPPER", "3,400 KG REBAR / RAILS", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SLAG DISPOSAL SUMP", "420 KG DIVERTED TO ROAD BED", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

        }
    }
}
