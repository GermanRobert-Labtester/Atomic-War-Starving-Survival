// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.Audio;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Shelter Airlock Barter Terminal (Plan 147 Follow-up).
    /// Programmatic UI implementation for the shelter barter route bound to ShelterBarterSystem.
    /// Follows the state -> blocker -> cost -> consequence panel standard and UI-07/UI-09 acceptance rules.
    /// Reconciled with Google Stitch Screen 8510d12322714acf990baaead9dabddc.
    /// </summary>
    public partial class ShelterBarterPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;

        // Container references
        private VBoxContainer _caravanListContainer = null!;
        private VBoxContainer _merchantStockContainer = null!;
        private VBoxContainer _playerStoresContainer = null!;
        private Label _merchantSubtotalLabel = null!;
        private Label _playerSubtotalLabel = null!;

        // Arbitrator Balance Scale & Diagnostics
        private Label _arbitratorStatusBadge = null!;
        private Label _balanceMetricsLabel = null!;
        private ProgressBar _balanceProgressBar = null!;
        private Label _balanceCoverageLabel = null!;

        // Pre-condition Gate Diagnostics
        private Label _gateAirlockLabel = null!;
        private Label _gatePresenceLabel = null!;
        private Label _gateRequestsLabel = null!;
        private Label _gateToleranceLabel = null!;
        private Label _gateCounterfeitLabel = null!;

        // Cost & Consequence Summary
        private Label _costSummaryLabel = null!;
        private Label _gainSummaryLabel = null!;
        private Label _consequenceLabel = null!;

        // Action controls
        private Button _clearButton = null!;
        private Button _executeButton = null!;
        private Label _feedbackLabel = null!;

        // Binding state
        private ShelterBarterSystem? _barterSystem;
        private Ashfall.Core.Inventory.Inventory? _inventory;
        private JournalSystem? _journal;
        private Func<string, ItemDefinition?>? _itemLookup;

        // Selection and allocations
        private string? _selectedCaravanId;
        private readonly Dictionary<string, int> _playerOffers = new(StringComparer.Ordinal);
        private readonly Dictionary<string, int> _playerRequests = new(StringComparer.Ordinal);
        private int _playerAppraisalSkillLevel = 0;
        private string _feedbackMessage = "Ready for barter negotiations at shelter airlock.";
        private bool _feedbackIsError = false;

        public bool IsBound => _barterSystem != null;
        public string? SelectedCaravanId => _selectedCaravanId;
        public IReadOnlyDictionary<string, int> PlayerOffers => _playerOffers;
        public IReadOnlyDictionary<string, int> PlayerRequests => _playerRequests;

        public void Bind(
            ShelterBarterSystem barterSystem,
            Ashfall.Core.Inventory.Inventory inventory,
            JournalSystem? journal = null,
            Func<string, ItemDefinition?>? itemLookup = null)
        {
            Unbind();

            _barterSystem = barterSystem ?? throw new ArgumentNullException(nameof(barterSystem));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _journal = journal;
            _itemLookup = itemLookup;

            _barterSystem.OnBarterStateChanged += HandleBarterStateChanged;
            _barterSystem.OnCaravanArrived += HandleCaravanArrived;
            _barterSystem.OnCaravanDeparted += HandleCaravanDeparted;

            // Auto-select first at-airlock caravan or first catalog entry
            SelectDefaultCaravan();

            RefreshView();
        }

        public void Unbind()
        {
            if (_barterSystem != null)
            {
                _barterSystem.OnBarterStateChanged -= HandleBarterStateChanged;
                _barterSystem.OnCaravanArrived -= HandleCaravanArrived;
                _barterSystem.OnCaravanDeparted -= HandleCaravanDeparted;
                _barterSystem = null;
            }
            _inventory = null;
            _journal = null;
            _itemLookup = null;
            _playerOffers.Clear();
            _playerRequests.Clear();
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            // Dark semi-transparent background overlay
            var bg = new ColorRect { Color = new Color(0.04f, 0.04f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            _shell = new AshfallDashboardShell("SHELTER AIRLOCK BARTER // CARAVAN TRADING ROUTE", minWidth: 1100, minHeight: 700);
            center.AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("caravan", "ACTIVE CARAVAN", "—", AshfallMetricCard.Criticality.Normal, minWidth: 160);
            _statusRail.AddCard("airlock", "AIRLOCK PRESENCE", "—", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("tolerance", "BARTER TOLERANCE", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("counterfeit", "COUNTERFEIT RISK", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("trades", "LIFETIME TRADES", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _shell.AttachHeaderCloseButton("CLOSE [Esc]", () =>
            {
                Close();
            });

            // Build layout structure
            var mainContent = new VBoxContainer();
            mainContent.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            mainContent.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            mainContent.SizeFlagsVertical = SizeFlags.ExpandFill;

            // Upper area: Left Caravan Selector (320px) + Two-Column Barter Tables
            var upperSplit = new HBoxContainer();
            upperSplit.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            upperSplit.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            upperSplit.SizeFlagsVertical = SizeFlags.ExpandFill;
            upperSplit.SizeFlagsStretchRatio = 1.4f;

            // ── Left Column: Caravan Registry & Schedule (320px) ──
            var leftPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            leftPanel.SizeFlagsHorizontal = SizeFlags.Fill;
            leftPanel.CustomMinimumSize = new Vector2(310, 0);

            var leftMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            leftPanel.AddChild(leftMargin);

            var leftVbox = new VBoxContainer();
            leftVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            leftMargin.AddChild(leftVbox);

            leftVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("CARAVAN REGISTRY & SCHEDULE"));
            leftVbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var leftScroll = new ScrollContainer
            {
                SizeFlagsVertical = SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
            };
            _caravanListContainer = new VBoxContainer();
            _caravanListContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _caravanListContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftScroll.AddChild(_caravanListContainer);
            leftVbox.AddChild(leftScroll);

            upperSplit.AddChild(leftPanel);

            // ── Right Area: Two-Column Barter Tables (Merchant Stock vs Shelter Stores) ──
            var tablesRow = new HBoxContainer();
            tablesRow.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            tablesRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            tablesRow.SizeFlagsVertical = SizeFlags.ExpandFill;

            // Table 1: Merchant Stock (Offered by Caravan)
            var merchantPanel = AshfallUiHelpers.MakePanel();
            merchantPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            var merchantMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            merchantPanel.AddChild(merchantMargin);

            var merchantVbox = new VBoxContainer();
            merchantVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            merchantMargin.AddChild(merchantVbox);

            var merchantHeaderRow = new HBoxContainer();
            merchantHeaderRow.AddChild(AshfallUiHelpers.MakeSectionHeader("MERCHANT STOCK (OFFERED)"));
            var brokerBadge = AshfallUiHelpers.MakeMetadata("[BROKER INVENTORY]");
            brokerBadge.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            merchantHeaderRow.AddChild(brokerBadge);
            merchantVbox.AddChild(merchantHeaderRow);

            var merchantColsHeader = new HBoxContainer();
            merchantColsHeader.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            var mH1 = AshfallUiHelpers.MakeMetadata("ITEM IDENTIFIER");
            mH1.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            merchantColsHeader.AddChild(mH1);
            var mH2 = AshfallUiHelpers.MakeMetadata("STOCK");
            mH2.CustomMinimumSize = new Vector2(50, 0);
            merchantColsHeader.AddChild(mH2);
            var mH3 = AshfallUiHelpers.MakeMetadata("VALUE");
            mH3.CustomMinimumSize = new Vector2(65, 0);
            merchantColsHeader.AddChild(mH3);
            var mH4 = AshfallUiHelpers.MakeMetadata("TAKE (QTY)");
            mH4.CustomMinimumSize = new Vector2(85, 0);
            merchantColsHeader.AddChild(mH4);
            var mH5 = AshfallUiHelpers.MakeMetadata("SUBTOTAL");
            mH5.CustomMinimumSize = new Vector2(65, 0);
            merchantColsHeader.AddChild(mH5);
            merchantVbox.AddChild(merchantColsHeader);
            merchantVbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var merchantScroll = new ScrollContainer
            {
                SizeFlagsVertical = SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
            };
            _merchantStockContainer = new VBoxContainer();
            _merchantStockContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _merchantStockContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            merchantScroll.AddChild(_merchantStockContainer);
            merchantVbox.AddChild(merchantScroll);

            merchantVbox.AddChild(AshfallUiHelpers.MakeSeparator());
            _merchantSubtotalLabel = AshfallUiHelpers.MakeBody("REQUESTED: 0.0 VALUE UNITS");
            _merchantSubtotalLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            merchantVbox.AddChild(_merchantSubtotalLabel);

            tablesRow.AddChild(merchantPanel);

            // Table 2: Shelter Stores (Offered by Player)
            var playerPanel = AshfallUiHelpers.MakePanel();
            playerPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            var playerMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            playerPanel.AddChild(playerMargin);

            var playerVbox = new VBoxContainer();
            playerVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            playerMargin.AddChild(playerVbox);

            var playerHeaderRow = new HBoxContainer();
            playerHeaderRow.AddChild(AshfallUiHelpers.MakeSectionHeader("SHELTER STORES (OFFERED)"));
            var vaultBadge = AshfallUiHelpers.MakeMetadata("[VAULT STORAGE]");
            vaultBadge.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Success));
            playerHeaderRow.AddChild(vaultBadge);
            playerVbox.AddChild(playerHeaderRow);

            var playerColsHeader = new HBoxContainer();
            playerColsHeader.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            var pH1 = AshfallUiHelpers.MakeMetadata("ITEM IDENTIFIER");
            pH1.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            playerColsHeader.AddChild(pH1);
            var pH2 = AshfallUiHelpers.MakeMetadata("AVAIL");
            pH2.CustomMinimumSize = new Vector2(50, 0);
            playerColsHeader.AddChild(pH2);
            var pH3 = AshfallUiHelpers.MakeMetadata("VALUE");
            pH3.CustomMinimumSize = new Vector2(65, 0);
            playerColsHeader.AddChild(pH3);
            var pH4 = AshfallUiHelpers.MakeMetadata("OFFER (QTY)");
            pH4.CustomMinimumSize = new Vector2(85, 0);
            playerColsHeader.AddChild(pH4);
            var pH5 = AshfallUiHelpers.MakeMetadata("SUBTOTAL");
            pH5.CustomMinimumSize = new Vector2(65, 0);
            playerColsHeader.AddChild(pH5);
            playerVbox.AddChild(playerColsHeader);
            playerVbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var playerScroll = new ScrollContainer
            {
                SizeFlagsVertical = SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
            };
            _playerStoresContainer = new VBoxContainer();
            _playerStoresContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _playerStoresContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            playerScroll.AddChild(_playerStoresContainer);
            playerVbox.AddChild(playerScroll);

            playerVbox.AddChild(AshfallUiHelpers.MakeSeparator());
            _playerSubtotalLabel = AshfallUiHelpers.MakeBody("OFFERED: 0.0 VALUE UNITS");
            _playerSubtotalLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Success));
            playerVbox.AddChild(_playerSubtotalLabel);

            tablesRow.AddChild(playerPanel);

            upperSplit.AddChild(tablesRow);
            mainContent.AddChild(upperSplit);

            // ── Lower Area: Arbitrator Balance Scale & Pre-Condition Diagnostics ──
            var lowerPanel = AshfallUiHelpers.MakePanel();
            lowerPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            var lowerMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            lowerPanel.AddChild(lowerMargin);

            var lowerVbox = new VBoxContainer();
            lowerVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            lowerMargin.AddChild(lowerVbox);

            var diagnosticRow = new HBoxContainer();
            diagnosticRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            diagnosticRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            // Left: Arbitrator Balance Scale
            var scaleBox = new VBoxContainer();
            scaleBox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            scaleBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scaleBox.SizeFlagsStretchRatio = 1.3f;

            var scaleHeaderRow = new HBoxContainer();
            scaleHeaderRow.AddChild(AshfallUiHelpers.MakeSectionHeader("ARBITRATOR BALANCE SCALE"));
            _arbitratorStatusBadge = AshfallUiHelpers.MakeBody("[SELECT ITEMS]");
            scaleHeaderRow.AddChild(_arbitratorStatusBadge);
            scaleBox.AddChild(scaleHeaderRow);

            _balanceMetricsLabel = AshfallUiHelpers.MakeMetadata("OFFERED: 0.0 VU | TOLERANCE THRESHOLD: 0.0 VU | REQUIRED: 0.0 VU");
            scaleBox.AddChild(_balanceMetricsLabel);

            _balanceProgressBar = new ProgressBar
            {
                CustomMinimumSize = new Vector2(0, 16),
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                MinValue = 0,
                MaxValue = 100,
                Value = 0,
                ShowPercentage = false
            };
            scaleBox.AddChild(_balanceProgressBar);

            _balanceCoverageLabel = AshfallUiHelpers.MakeMetadata("Coverage: 0% — Fair deal requires offer meeting caravan tolerance margin.");
            scaleBox.AddChild(_balanceCoverageLabel);

            diagnosticRow.AddChild(scaleBox);

            // Right: Pre-Condition Gate Diagnostics
            var gateBox = new VBoxContainer();
            gateBox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            gateBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            gateBox.SizeFlagsStretchRatio = 1.0f;

            gateBox.AddChild(AshfallUiHelpers.MakeSectionHeader("PRE-CONDITION GATE DIAGNOSTICS"));
            _gateAirlockLabel = AshfallUiHelpers.MakeMetadata("AIRLOCK ACCESSIBLE: [ ... ]");
            gateBox.AddChild(_gateAirlockLabel);
            _gatePresenceLabel = AshfallUiHelpers.MakeMetadata("CARAVAN AT AIRLOCK: [ ... ]");
            gateBox.AddChild(_gatePresenceLabel);
            _gateRequestsLabel = AshfallUiHelpers.MakeMetadata("GOODS REQUESTED: [ ... ]");
            gateBox.AddChild(_gateRequestsLabel);
            _gateToleranceLabel = AshfallUiHelpers.MakeMetadata("VALUATION THRESHOLD: [ ... ]");
            gateBox.AddChild(_gateToleranceLabel);
            _gateCounterfeitLabel = AshfallUiHelpers.MakeMetadata("APPRAISAL VERIFICATION: [ ... ]");
            gateBox.AddChild(_gateCounterfeitLabel);

            diagnosticRow.AddChild(gateBox);
            lowerVbox.AddChild(diagnosticRow);

            lowerVbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Cost & Consequence Summary Strip (state -> blocker -> cost -> consequence)
            var costConsequenceRow = new HBoxContainer();
            costConsequenceRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            costConsequenceRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            var summaryVbox = new VBoxContainer();
            summaryVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            summaryVbox.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            _costSummaryLabel = AshfallUiHelpers.MakeBody("COST: None allocated.");
            _costSummaryLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Critical));
            summaryVbox.AddChild(_costSummaryLabel);

            _gainSummaryLabel = AshfallUiHelpers.MakeBody("GAIN: None requested.");
            _gainSummaryLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Success));
            summaryVbox.AddChild(_gainSummaryLabel);

            _consequenceLabel = AshfallUiHelpers.MakeMetadata("CONSEQUENCE: Deducts stock permanently from caravan; increments trade ledger; logs transaction to Journal.");
            summaryVbox.AddChild(_consequenceLabel);

            costConsequenceRow.AddChild(summaryVbox);

            // Action Buttons
            var buttonsVbox = new HBoxContainer();
            buttonsVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);

            _clearButton = AshfallUiHelpers.MakeButton("CLEAR ALLOCATIONS", HandleClearAllocations);
            _clearButton.CustomMinimumSize = new Vector2(150, 36);
            buttonsVbox.AddChild(_clearButton);

            _executeButton = AshfallUiHelpers.MakeButton("EXECUTE BARTER TRANSACTION", HandleExecuteTrade);
            _executeButton.CustomMinimumSize = new Vector2(240, 36);
            buttonsVbox.AddChild(_executeButton);

            costConsequenceRow.AddChild(buttonsVbox);
            lowerVbox.AddChild(costConsequenceRow);

            mainContent.AddChild(lowerPanel);

            // Single Feedback Strip at bottom
            var feedbackPanel = AshfallUiHelpers.MakePanel();
            feedbackPanel.CustomMinimumSize = new Vector2(0, 28);
            var fbMargin = AshfallUiHelpers.MakeMargins(4, 2, 4, 2);
            feedbackPanel.AddChild(fbMargin);
            _feedbackLabel = AshfallUiHelpers.MakeMetadata(_feedbackMessage);
            _feedbackLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            fbMargin.AddChild(_feedbackLabel);
            mainContent.AddChild(feedbackPanel);

            _shell.SetContent(mainContent);

            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
        }

        public void Close() {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
                Visible = false;
            OnClose?.Invoke();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                Close();
                GetViewport().SetInputAsHandled();
            }
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }

        private void HandleBarterStateChanged()
        {
            RefreshView();
        }

        private void HandleCaravanArrived(MerchantCaravanDef caravan)
        {
            if (_selectedCaravanId == null || _selectedCaravanId == caravan.caravan_id)
            {
                _selectedCaravanId = caravan.caravan_id;
            }
            SetFeedback($"Caravan arrived: {caravan.name} is now docked at the airlock.", isError: false);
            RefreshView();
        }

        private void HandleCaravanDeparted(MerchantCaravanDef caravan)
        {
            if (_selectedCaravanId == caravan.caravan_id)
            {
                _playerOffers.Clear();
                _playerRequests.Clear();
            }
            SetFeedback($"Caravan departed: {caravan.name} moved on.", isError: false);
            RefreshView();
        }

        private void SelectDefaultCaravan()
        {
            if (_barterSystem == null) return;
            // Prefer an active at-airlock caravan
            foreach (var kvp in _barterSystem.Catalog)
            {
                if (_barterSystem.State.caravans.TryGetValue(kvp.Key, out var cState) && cState.isAtAirlock)
                {
                    _selectedCaravanId = kvp.Key;
                    return;
                }
            }
            // Otherwise first registered caravan
            _selectedCaravanId = _barterSystem.Catalog.Keys.FirstOrDefault();
        }

        public void RefreshView()
        {
            if (_barterSystem == null || _shell == null || _statusRail == null)
            {
                return;
            }

            // Verify or fix selected caravan
            if (_selectedCaravanId == null || !_barterSystem.Catalog.ContainsKey(_selectedCaravanId))
            {
                SelectDefaultCaravan();
            }

            MerchantCaravanDef? selectedCaravan = null;
            CaravanRuntimeState? caravanState = null;
            if (_selectedCaravanId != null && _barterSystem.Catalog.TryGetValue(_selectedCaravanId, out selectedCaravan))
            {
                _barterSystem.State.caravans.TryGetValue(_selectedCaravanId, out caravanState);
            }

            // 1. Status Rail
            RefreshStatusRail(selectedCaravan, caravanState);

            // 2. Caravan Registry List
            RefreshCaravanList();

            // 3. Two-Column Barter Tables
            RefreshMerchantStockTable(selectedCaravan, caravanState);
            RefreshPlayerStoresTable(selectedCaravan);

            // 4. Arbitrator Balance Scale & Pre-Condition Diagnostics
            RefreshBalanceAndDiagnostics(selectedCaravan, caravanState);

            // 5. Feedback Label
            _feedbackLabel.Text = _feedbackMessage;
            _feedbackLabel.AddThemeColorOverride("font_color", _feedbackIsError
                ? AshfallUiHelpers.ToColor(DesignTheme.Critical)
                : AshfallUiHelpers.ToColor(DesignTheme.Pale));
        }

        private void RefreshStatusRail(MerchantCaravanDef? caravan, CaravanRuntimeState? cState)
        {
            if (_statusRail == null || _barterSystem == null) return;

            string name = caravan != null ? caravan.name : "None Selected";
            _statusRail.Set("caravan", name, AshfallMetricCard.Criticality.Normal);

            if (cState != null && cState.isAtAirlock && caravan != null)
            {
                int remainingDays = Math.Max(1, caravan.stay_duration_days - cState.daysPresent + 1);
                _statusRail.Set("airlock", $"PRESENT ({remainingDays}d left)", AshfallMetricCard.Criticality.Normal);
            }
            else if (caravan != null)
            {
                int currentDay = _barterSystem.State.currentDay;
                int daysUntil = (caravan.schedule_period_days - (currentDay % caravan.schedule_period_days)) % caravan.schedule_period_days;
                if (daysUntil == 0) daysUntil = caravan.schedule_period_days;
                _statusRail.Set("airlock", $"EN ROUTE ({daysUntil}d)", AshfallMetricCard.Criticality.Caution);
            }
            else
            {
                _statusRail.Set("airlock", "OFFLINE", AshfallMetricCard.Criticality.Normal);
            }

            if (caravan != null)
            {
                float tolPct = caravan.barter_tolerance_bp / 100f;
                float margin = Math.Abs(100f - tolPct);
                _statusRail.Set("tolerance", $"{tolPct:F1}% (±{margin:F1}%)", AshfallMetricCard.Criticality.Normal);

                float riskPct = caravan.counterfeit_risk_bp / 100f;
                _statusRail.Set("counterfeit", $"{riskPct:F1}%", riskPct > 10f ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            }
            else
            {
                _statusRail.Set("tolerance", "—", AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("counterfeit", "—", AshfallMetricCard.Criticality.Normal);
            }

            _statusRail.Set("trades", $"{_barterSystem.State.completedTradesCount} COMPLETED", AshfallMetricCard.Criticality.Normal);
        }

        private void RefreshCaravanList()
        {
            if (_caravanListContainer == null || _barterSystem == null) return;
            AshfallUiHelpers.EmptyChildren(_caravanListContainer);

            int currentDay = _barterSystem.State.currentDay;

            foreach (var kvp in _barterSystem.Catalog)
            {
                var def = kvp.Value;
                _barterSystem.State.caravans.TryGetValue(def.caravan_id, out var cState);
                bool isAtAirlock = cState != null && cState.isAtAirlock;
                bool isSelected = def.caravan_id == _selectedCaravanId;

                var cardPanel = AshfallUiHelpers.MakePanel();
                cardPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;

                var margin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingXs);
                cardPanel.AddChild(margin);

                var vbox = new VBoxContainer();
                vbox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
                margin.AddChild(vbox);

                // Caravan title
                var nameBtn = AshfallUiHelpers.MakeButton(def.name.ToUpperInvariant(), () =>
                {
                    if (_selectedCaravanId != def.caravan_id)
                    {
                        _selectedCaravanId = def.caravan_id;
                        _playerOffers.Clear();
                        _playerRequests.Clear();
                        RefreshView();
                    }
                });
                nameBtn.Alignment = HorizontalAlignment.Left;
                vbox.AddChild(nameBtn);

                // Status Pill
                if (isAtAirlock)
                {
                    int daysLeft = Math.Max(1, def.stay_duration_days - (cState?.daysPresent ?? 1) + 1);
                    var pill = AshfallUiHelpers.MakeMetadata($"[AT AIRLOCK - DEPARTS IN {daysLeft}D]");
                    pill.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                    vbox.AddChild(pill);
                }
                else
                {
                    int daysUntil = (def.schedule_period_days - (currentDay % def.schedule_period_days)) % def.schedule_period_days;
                    if (daysUntil == 0) daysUntil = def.schedule_period_days;
                    var pill = AshfallUiHelpers.MakeMetadata($"[EN ROUTE - ARRIVES IN {daysUntil}D]");
                    pill.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
                    vbox.AddChild(pill);
                }

                // Description
                var desc = AshfallUiHelpers.MakeMetadata(def.description);
                desc.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                vbox.AddChild(desc);

                // Demands & Supplies
                string demandsStr = def.demanded_item_tags.Count > 0
                    ? string.Join(", ", def.demanded_item_tags.Select(t => FormatTagName(t)))
                    : "None";
                var demLbl = AshfallUiHelpers.MakeMetadata($"Demands: {demandsStr}");
                demLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
                vbox.AddChild(demLbl);

                if (isSelected)
                {
                    var selIndicator = AshfallUiHelpers.MakeMetadata("▶ CURRENTLY INSPECTING");
                    selIndicator.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                    vbox.AddChild(selIndicator);
                }

                _caravanListContainer.AddChild(cardPanel);
            }
        }

        private void RefreshMerchantStockTable(MerchantCaravanDef? caravan, CaravanRuntimeState? cState)
        {
            if (_merchantStockContainer == null) return;
            AshfallUiHelpers.EmptyChildren(_merchantStockContainer);

            if (caravan == null || cState == null)
            {
                _merchantStockContainer.AddChild(AshfallUiHelpers.MakeMetadata("Select a caravan from the registry."));
                _merchantSubtotalLabel.Text = "REQUESTED: 0.0 VALUE UNITS";
                return;
            }

            bool isAtAirlock = cState.isAtAirlock;
            float totalCost = 0f;

            if (caravan.stock.Count == 0)
            {
                _merchantStockContainer.AddChild(AshfallUiHelpers.MakeMetadata("Caravan has no stock configured."));
            }

            var stockItems = _barterSystem != null ? _barterSystem.GetPrioritizedStock(caravan) : (IReadOnlyList<CaravanStockItem>)caravan.stock;
            foreach (var stockItem in stockItems)
            {
                string itemId = stockItem.item_id;
                int available = cState.remainingStock.TryGetValue(itemId, out int rem) ? rem : 0;
                int currentReq = _playerRequests.TryGetValue(itemId, out int rQty) ? rQty : 0;

                // Clamp request if available stock dropped
                if (currentReq > available)
                {
                    currentReq = available;
                    if (currentReq > 0) _playerRequests[itemId] = currentReq;
                    else _playerRequests.Remove(itemId);
                }

                // Unit valuation
                float baseVal = _barterSystem?.GetBaseItemValue(itemId) ?? 5f;
                float unitPrice = baseVal * (stockItem.price_multiplier_bp / (float)ShelterBarterSystem.BasisPointsScale);
                float rowSubtotal = unitPrice * currentReq;
                totalCost += rowSubtotal;

                var row = new HBoxContainer();
                row.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
                row.SizeFlagsHorizontal = SizeFlags.ExpandFill;

                // Name & Type
                var nameLbl = AshfallUiHelpers.MakeMetadata(FormatItemName(itemId, _itemLookup));
                nameLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                row.AddChild(nameLbl);

                // Stock
                var stockLbl = AshfallUiHelpers.MakeMetadata($"{available}/{stockItem.quantity}");
                stockLbl.CustomMinimumSize = new Vector2(50, 0);
                row.AddChild(stockLbl);

                // Unit Value
                var valLbl = AshfallUiHelpers.MakeMetadata($"{unitPrice:F1} VU");
                valLbl.CustomMinimumSize = new Vector2(65, 0);
                valLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                row.AddChild(valLbl);

                // Counter: [-] [qty] [+]
                var counterBox = new HBoxContainer();
                counterBox.CustomMinimumSize = new Vector2(85, 0);
                counterBox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);

                var minusBtn = AshfallUiHelpers.MakeButton("-", () =>
                {
                    if (currentReq > 0)
                    {
                        currentReq--;
                        if (currentReq > 0) _playerRequests[itemId] = currentReq;
                        else _playerRequests.Remove(itemId);
                        RefreshView();
                    }
                }, disabled: currentReq <= 0);
                minusBtn.CustomMinimumSize = new Vector2(24, 22);
                counterBox.AddChild(minusBtn);

                var qtyLbl = AshfallUiHelpers.MakeMetadata(currentReq.ToString());
                qtyLbl.CustomMinimumSize = new Vector2(28, 22);
                qtyLbl.HorizontalAlignment = HorizontalAlignment.Center;
                qtyLbl.VerticalAlignment = VerticalAlignment.Center;
                if (currentReq > 0)
                    qtyLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                counterBox.AddChild(qtyLbl);

                var plusBtn = AshfallUiHelpers.MakeButton("+", () =>
                {
                    if (isAtAirlock && currentReq < available)
                    {
                        currentReq++;
                        _playerRequests[itemId] = currentReq;
                        RefreshView();
                    }
                }, disabled: !isAtAirlock || currentReq >= available);
                plusBtn.CustomMinimumSize = new Vector2(24, 22);
                counterBox.AddChild(plusBtn);

                row.AddChild(counterBox);

                // Subtotal
                var subLbl = AshfallUiHelpers.MakeMetadata($"{rowSubtotal:F1} VU");
                subLbl.CustomMinimumSize = new Vector2(65, 0);
                if (currentReq > 0)
                    subLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                row.AddChild(subLbl);

                _merchantStockContainer.AddChild(row);
            }

            _merchantSubtotalLabel.Text = $"REQUESTED: {totalCost:F1} VALUE UNITS";
        }

        private void RefreshPlayerStoresTable(MerchantCaravanDef? caravan)
        {
            if (_playerStoresContainer == null) return;
            AshfallUiHelpers.EmptyChildren(_playerStoresContainer);

            if (_inventory == null || _barterSystem == null)
            {
                _playerStoresContainer.AddChild(AshfallUiHelpers.MakeMetadata("Storage system unavailable."));
                _playerSubtotalLabel.Text = "OFFERED: 0.0 VALUE UNITS";
                return;
            }

            // Aggregate items from inventory
            var invItems = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var slot in _inventory.Slots)
            {
                if (slot.Item != null && slot.Amount > 0)
                {
                    string id = slot.Item.id;
                    invItems[id] = invItems.GetValueOrDefault(id, 0) + slot.Amount;
                }
            }

            // Ensure any allocated items are retained in table
            foreach (var kvp in _playerOffers)
            {
                if (!invItems.ContainsKey(kvp.Key))
                    invItems[kvp.Key] = 0;
            }

            if (invItems.Count == 0)
            {
                _playerStoresContainer.AddChild(AshfallUiHelpers.MakeMetadata("No tradeable goods in shelter storage."));
                _playerSubtotalLabel.Text = "OFFERED: 0.0 VALUE UNITS";
                return;
            }

            float totalOfferVal = 0f;

            foreach (var kvp in invItems.OrderBy(k => k.Key))
            {
                string itemId = kvp.Key;
                int available = kvp.Value;
                int currentOffer = _playerOffers.TryGetValue(itemId, out int offQty) ? offQty : 0;

                // Clamp offer if storage count dropped
                if (currentOffer > available)
                {
                    currentOffer = available;
                    if (currentOffer > 0) _playerOffers[itemId] = currentOffer;
                    else _playerOffers.Remove(itemId);
                }

                // Valuation
                float baseVal = _barterSystem.GetBaseItemValue(itemId);
                bool isDemanded = caravan != null && caravan.demanded_item_tags.Any(t => itemId.Contains(t, StringComparison.OrdinalIgnoreCase));
                float mult = isDemanded && caravan != null ? (caravan.demanded_price_mult_bp / (float)ShelterBarterSystem.BasisPointsScale) : 1f;

                if (_barterSystem.IsSevereWinterWeather && (itemId.Contains("fuel", StringComparison.OrdinalIgnoreCase) || itemId.Contains("food", StringComparison.OrdinalIgnoreCase)))
                {
                    mult *= 1.30f;
                }

                float effectiveUnitPrice = baseVal * mult;
                float rowSubtotal = effectiveUnitPrice * currentOffer;
                totalOfferVal += rowSubtotal;

                var row = new HBoxContainer();
                row.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
                row.SizeFlagsHorizontal = SizeFlags.ExpandFill;

                // Name & Demand tag
                var nameVbox = new VBoxContainer();
                nameVbox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                nameVbox.AddThemeConstantOverride("separation", 0);

                var nameLbl = AshfallUiHelpers.MakeMetadata(FormatItemName(itemId, _itemLookup));
                nameVbox.AddChild(nameLbl);

                if (isDemanded)
                {
                    var bonusLbl = AshfallUiHelpers.MakeMetadata("+DEMAND BONUS");
                    bonusLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Success));
                    nameVbox.AddChild(bonusLbl);
                }
                row.AddChild(nameVbox);

                // Available
                var availLbl = AshfallUiHelpers.MakeMetadata(available.ToString());
                availLbl.CustomMinimumSize = new Vector2(50, 0);
                row.AddChild(availLbl);

                // Unit Value
                var valLbl = AshfallUiHelpers.MakeMetadata($"{effectiveUnitPrice:F1} VU");
                valLbl.CustomMinimumSize = new Vector2(65, 0);
                valLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Success));
                row.AddChild(valLbl);

                // Counter: [-] [qty] [+]
                var counterBox = new HBoxContainer();
                counterBox.CustomMinimumSize = new Vector2(85, 0);
                counterBox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);

                var minusBtn = AshfallUiHelpers.MakeButton("-", () =>
                {
                    if (currentOffer > 0)
                    {
                        currentOffer--;
                        if (currentOffer > 0) _playerOffers[itemId] = currentOffer;
                        else _playerOffers.Remove(itemId);
                        RefreshView();
                    }
                }, disabled: currentOffer <= 0);
                minusBtn.CustomMinimumSize = new Vector2(24, 22);
                counterBox.AddChild(minusBtn);

                var qtyLbl = AshfallUiHelpers.MakeMetadata(currentOffer.ToString());
                qtyLbl.CustomMinimumSize = new Vector2(28, 22);
                qtyLbl.HorizontalAlignment = HorizontalAlignment.Center;
                qtyLbl.VerticalAlignment = VerticalAlignment.Center;
                if (currentOffer > 0)
                    qtyLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Success));
                counterBox.AddChild(qtyLbl);

                var plusBtn = AshfallUiHelpers.MakeButton("+", () =>
                {
                    if (currentOffer < available)
                    {
                        currentOffer++;
                        _playerOffers[itemId] = currentOffer;
                        RefreshView();
                    }
                }, disabled: currentOffer >= available);
                plusBtn.CustomMinimumSize = new Vector2(24, 22);
                counterBox.AddChild(plusBtn);

                row.AddChild(counterBox);

                // Subtotal
                var subLbl = AshfallUiHelpers.MakeMetadata($"{rowSubtotal:F1} VU");
                subLbl.CustomMinimumSize = new Vector2(65, 0);
                if (currentOffer > 0)
                    subLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Success));
                row.AddChild(subLbl);

                _playerStoresContainer.AddChild(row);
            }

            _playerSubtotalLabel.Text = $"OFFERED: {totalOfferVal:F1} VALUE UNITS";
        }

        private void RefreshBalanceAndDiagnostics(MerchantCaravanDef? caravan, CaravanRuntimeState? cState)
        {
            if (_barterSystem == null) return;

            bool isAirlockAccessible = _barterSystem.IsAirlockAccessible();
            bool isCaravanAtAirlock = cState != null && cState.isAtAirlock;
            bool hasRequests = _playerRequests.Count > 0;

            float playerOfferVal = caravan != null ? _barterSystem.CalculatePlayerOfferValue(caravan, _playerOffers) : 0f;
            float caravanRequiredVal = caravan != null ? _barterSystem.CalculateCaravanStockCost(caravan, _playerRequests) : 0f;

            long scaledOffer = (long)(playerOfferVal * ShelterBarterSystem.BasisPointsScale);
            long toleranceBp = caravan?.barter_tolerance_bp ?? ShelterBarterSystem.BasisPointsScale;
            long scaledRequiredWithTolerance = (long)(caravanRequiredVal * toleranceBp);

            bool isValueSufficient = scaledOffer >= scaledRequiredWithTolerance && hasRequests;
            float toleranceThresholdVal = (caravanRequiredVal * toleranceBp) / (float)ShelterBarterSystem.BasisPointsScale;

            // 1. Balance scale visuals
            _balanceMetricsLabel.Text = $"OFFERED: {playerOfferVal:F1} VU | TOLERANCE THRESHOLD: ≥ {toleranceThresholdVal:F1} VU | REQUIRED: {caravanRequiredVal:F1} VU";

            float progressPct = 0f;
            if (toleranceThresholdVal > 0.001f)
            {
                progressPct = Math.Clamp((playerOfferVal / toleranceThresholdVal) * 100f, 0f, 100f);
            }
            else if (hasRequests && playerOfferVal > 0f)
            {
                progressPct = 100f;
            }
            _balanceProgressBar.Value = progressPct;

            if (!hasRequests)
            {
                _arbitratorStatusBadge.Text = "[SELECT GOODS TO PURCHASE]";
                _arbitratorStatusBadge.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
                _balanceCoverageLabel.Text = "Coverage: 0% — Allocate caravan stock on the left, then offer shelter stores on the right.";
            }
            else if (!isCaravanAtAirlock)
            {
                _arbitratorStatusBadge.Text = "[CARAVAN NOT AT AIRLOCK]";
                _arbitratorStatusBadge.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Critical));
                _balanceCoverageLabel.Text = "Barter cannot be executed: Caravan is en route.";
            }
            else if (!isAirlockAccessible)
            {
                _arbitratorStatusBadge.Text = "[AIRLOCK FROZEN / INACCESSIBLE]";
                _arbitratorStatusBadge.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Critical));
                _balanceCoverageLabel.Text = "Barter blocked: The shelter airlock is frozen.";
            }
            else if (isValueSufficient)
            {
                float surplus = playerOfferVal - toleranceThresholdVal;
                _arbitratorStatusBadge.Text = "[FAIR DEAL — MERCHANT ACCEPTS]";
                _arbitratorStatusBadge.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Success));
                _balanceCoverageLabel.Text = $"Coverage: {progressPct:F0}% — Trade viable. Surplus of +{surplus:F1} VU converts to merchant trust.";
            }
            else
            {
                float deficit = toleranceThresholdVal - playerOfferVal;
                _arbitratorStatusBadge.Text = $"[INSUFFICIENT OFFER — NEED +{deficit:F1} VU]";
                _arbitratorStatusBadge.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                _balanceCoverageLabel.Text = $"Coverage: {progressPct:F0}% — Offer is below merchant's tolerance threshold.";
            }

            // 2. Pre-condition Gate Diagnostics
            FormatGateLabel(_gateAirlockLabel, "AIRLOCK ACCESSIBLE", isAirlockAccessible, isAirlockAccessible ? "PASS" : "BLOCKED (FROZEN)");
            FormatGateLabel(_gatePresenceLabel, "CARAVAN AT AIRLOCK", isCaravanAtAirlock, isCaravanAtAirlock ? "PASS" : "BLOCKED (EN ROUTE)");
            FormatGateLabel(_gateRequestsLabel, "GOODS REQUESTED", hasRequests, hasRequests ? $"{_playerRequests.Values.Sum()} ITEMS" : "NONE ALLOCATED");
            FormatGateLabel(_gateToleranceLabel, "VALUATION THRESHOLD", isValueSufficient, isValueSufficient ? "PASS (MEETS TOLERANCE)" : "INSUFFICIENT OFFER");

            float riskPct = (caravan?.counterfeit_risk_bp ?? 0) / 100f;
            string appText = _playerAppraisalSkillLevel >= 2 ? $"PASS (APPRAISAL LVL {_playerAppraisalSkillLevel})" : $"RISK {riskPct:F1}% (UNAPPRAISED)";
            FormatGateLabel(_gateCounterfeitLabel, "APPRAISAL VERIFICATION", true, appText);

            // 3. Cost & Consequence Summary
            if (_playerOffers.Count > 0)
            {
                var costItems = _playerOffers.Select(kv => $"{kv.Value}x {FormatItemName(kv.Key, _itemLookup)}");
                _costSummaryLabel.Text = $"COST: {string.Join(", ", costItems)} (-{playerOfferVal:F1} VU)";
            }
            else
            {
                _costSummaryLabel.Text = "COST: None allocated.";
            }

            if (_playerRequests.Count > 0)
            {
                var gainItems = _playerRequests.Select(kv => $"{kv.Value}x {FormatItemName(kv.Key, _itemLookup)}");
                _gainSummaryLabel.Text = $"GAIN: {string.Join(", ", gainItems)} (+{caravanRequiredVal:F1} VU)";
            }
            else
            {
                _gainSummaryLabel.Text = "GAIN: None requested.";
            }

            _consequenceLabel.Text = "CONSEQUENCE: Deducts stock permanently from caravan; increments trade ledger; logs transaction to Journal.";

            // 4. Action Button State
            bool canExecute = isAirlockAccessible && isCaravanAtAirlock && hasRequests && isValueSufficient;
            _executeButton.Disabled = !canExecute;
            _clearButton.Disabled = _playerOffers.Count == 0 && _playerRequests.Count == 0;
        }

        private static void FormatGateLabel(Label lbl, string header, bool pass, string text)
        {
            lbl.Text = $"{header}: [ {text} ]";
            lbl.AddThemeColorOverride("font_color", pass
                ? AshfallUiHelpers.ToColor(DesignTheme.Success)
                : AshfallUiHelpers.ToColor(DesignTheme.Critical));
        }

        private void HandleClearAllocations()
        {
            _playerOffers.Clear();
            _playerRequests.Clear();
            SetFeedback("Allocations cleared.", isError: false);
            RefreshView();
        }

        private void HandleExecuteTrade()
        {
            if (_barterSystem == null || _selectedCaravanId == null)
            {
                SetFeedback("Cannot execute trade: Barter system not initialized.", isError: true);
                return;
            }

            var result = _barterSystem.ExecuteTrade(
                _selectedCaravanId,
                _playerOffers,
                _playerRequests,
                _playerAppraisalSkillLevel);

            if (result.IsSuccess)
            {
                var caravan = _barterSystem.Catalog[_selectedCaravanId];
                int tradeCount = _barterSystem.State.completedTradesCount;

                string gainSummary = string.Join(", ", _playerRequests.Select(kv => $"{kv.Value}x {FormatItemName(kv.Key, _itemLookup)}"));
                string costSummary = string.Join(", ", _playerOffers.Select(kv => $"{kv.Value}x {FormatItemName(kv.Key, _itemLookup)}"));

                string msg = $"Trade executed with {caravan.name}! Acquired {gainSummary} for {costSummary}. (Trade #{tradeCount})";
                SetFeedback(msg, isError: false);

                // Journal integration
                _journal?.TryAddRawEntry(
                    $"shelter_barter_trade_{tradeCount}",
                    $"Completed barter trade with {caravan.name} at airlock: received {gainSummary} in exchange for {costSummary}.",
                    null!,
                    _barterSystem.State.currentDay);

                _playerOffers.Clear();
                _playerRequests.Clear();
                RefreshView();
            }
            else
            {
                string readableError = ResolveReadableError(result.FailureCode);
                SetFeedback($"Trade blocked: {readableError}", isError: true);
                RefreshView();
            }
        }

        private static string ResolveReadableError(string? reason)
        {
            return reason switch
            {
                "unknown_caravan" => "Selected merchant caravan is not recognized by the manifest.",
                "caravan_not_at_airlock" => "The caravan is en route and has not arrived at the airlock yet.",
                "airlock_inaccessible" => "The shelter airlock is frozen or blocked. Restore heat before operating the airlock.",
                "no_items_requested" => "No merchant goods have been selected for trade.",
                "insufficient_caravan_stock" => "The caravan does not have enough remaining stock to fulfill this request.",
                "insufficient_value" => "Offered goods do not meet the merchant's required valuation and tolerance margin.",
                "counterfeit_detected" => "Counterfeit or defective goods detected during trade appraisal. Negotiation aborted.",
                "storage_capacity_exceeded" => "Shelter storage capacity or weight limit exceeded. Make room in storage first.",
                "transaction_commit_failed" => "Failed to commit inventory transfer.",
                _ => string.IsNullOrWhiteSpace(reason) ? "Unknown barter constraint failed." : reason.Replace('_', ' ')
            };
        }

        private void SetFeedback(string text, bool isError)
        {
            _feedbackMessage = text;
            _feedbackIsError = isError;
        }

        public static string FormatItemName(string itemId, Func<string, ItemDefinition?>? lookup = null)
        {
            if (string.IsNullOrWhiteSpace(itemId)) return "Unknown Item";
            if (lookup != null)
            {
                var def = lookup(itemId);
                if (def != null && !string.IsNullOrWhiteSpace(def.displayName))
                    return def.displayName;
            }
            string canon = ItemAliases.ToCanonical(itemId);
            if (lookup != null)
            {
                var def = lookup(canon);
                if (def != null && !string.IsNullOrWhiteSpace(def.displayName))
                    return def.displayName;
            }
            string clean = itemId;
            if (clean.StartsWith("item_", StringComparison.OrdinalIgnoreCase))
                clean = clean.Substring(5);
            clean = clean.Replace('_', ' ');
            return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(clean);
        }

        private static string FormatTagName(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) return string.Empty;
            return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(tag.Replace('_', ' '));
        }
    }
}
