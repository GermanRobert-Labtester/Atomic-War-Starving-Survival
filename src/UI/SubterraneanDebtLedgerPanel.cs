// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class SubterraneanDebtLedgerPanel : Control, IBindablePanel
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
                _statusBadgeLabel.Text = "STATUS: BLACK MARKET ARBITRAGE ACTIVE - TOTAL DEBT: 14,850 SCRIP";
            }
        }

        private void BuildInterface()
        {
            var chrome = ThreePanePanelScaffold.BuildChrome(
                this,
                "SHELTER FISCAL AUTHORITY // DEBT LEDGER & SCRIP CLEARINGHOUSE [ECON-04]",
                "STATUS: BLACK MARKET ARBITRAGE ACTIVE - TOTAL DEBT: 14,850 SCRIP",
                AshfallUiHelpers.ToColor(DesignTheme.Warm),
                "[X] CLOSE CONSOLE",
                "[ECON-04] Promissory note #88 registered for 100 rounds 7.62mm.\n[ECON-04] Collateral lien placed on debtor Carver foundry toolset.",
                () => OnClose?.Invoke());
            _headerTitleLabel = chrome.Title;
            _statusBadgeLabel = chrome.Status;
            _closeButton = chrome.Close;
            _logOutputLabel = chrome.Log;
            var bodyHBox = chrome.Body;

            // Left Column (Telemetry)
            var leftPanel = ThreePanePanelScaffold.CreatePanelFrame("FACTION DEBT & COMPOUND INTEREST");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = ThreePanePanelScaffold.CreateColumn(leftPanel, 8);
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("MERIDIAN COMPACT DEBT", "6,400 SCRIP @ 4.5%/WEEK", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RUST GUILD DEBT BALANCE", "4,200 SCRIP @ 8.0%/WEEK", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("HYDRO-BARONS ARREARS", "4,250 SCRIP @ 12.0%/WEEK", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RUST GUILD DEFAULT TIMER", "6 DAYS UNTIL FORECLOSURE", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("CLEAN WATER BARTER SPREAD", "+14.2% PREMIUM OVER BASE", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

            // Center Column (Interactive Controls)
            var centerPanel = ThreePanePanelScaffold.CreatePanelFrame("PROMISSORY NOTES & COLLATERAL SEIZURE");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = ThreePanePanelScaffold.CreateColumn(centerPanel, 12);
            _buttonContainer.AddChild(new Button { Text = "[ISSUE HIGH-INTEREST MERCHANDISE BOND]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[EXECUTE COLLATERAL SEIZURE PROTOCOL]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[ARBITRAGE BLACK MARKET SCRIP FOR RATIONS]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[RESTRUCTURE FACTION ACCORD REPAYMENT]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = ThreePanePanelScaffold.CreatePanelFrame("VAULT B-04 RESERVES & INFLATION");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = ThreePanePanelScaffold.CreateColumn(rightPanel, 8);
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("DIESEL HARD CURRENCY", "1,420 LITERS IN SECURE SUMP", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("LEAD INGOTS COLLATERAL", "840 KG STACKED IN VAULT", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SCRIP CIRCULATION INFLATION", "+1.8% / MONTH (CONTROLLED)", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("DISPUTED CONTRACTS IN ARBITRAGE", "3 ACTIVE LIEN CASES", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

        }
    }
}
