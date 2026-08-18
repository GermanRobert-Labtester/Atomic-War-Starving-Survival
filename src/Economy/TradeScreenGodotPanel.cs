using System;
using System.Collections.Generic;
using Godot;
using static AtomicWar.GodotApp.UI.AshfallUiHelpers;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Radio;
namespace AtomicWar.GodotApp.Economy
{
    
    /// <summary>
    /// Full Godot host implementation of the Ashfall Trade Screen &amp; Economy HUD.
    /// Hits all fields:
    /// - Header: Faction emblem, name, leader name, succession gen, stance badge, trust meter, aggression meter, repel count, parley readiness
    /// - Market &amp; Shocks: World phase, day, active price shock badges with icons, scarcity tier badges
    /// - Two-column Barter: Player offers (items, qualitative worth, biological offerings: blood, marrow, plasma, organ) and Faction asks
    /// - Arbitrator: Deal fairness badge, value comparison, confirm button, parley button
    /// - Radio Ticker: Intercept chatter and parley resolutions
    /// 
    /// Follows strict token styling via Ashfall.Core.UI.Theme.
    /// </summary>
    public partial class TradeScreenGodotPanel : PanelContainer
    {
        // ── Data / Bindings ──────────────────────────────────────────
        private EconomyHostSession _session;
        private IFactionStanceProvider _stanceProvider;
        private IPriceShockProvider _priceShockProvider;
        private ITradeScreenViewModel _viewModel;
        private ITradeIntentSink _intentSink;

        private string _activeFactionId = "scavenger_camp";
        private readonly Dictionary<string, int> _playerOfferCounts = new();
        private readonly Dictionary<string, int> _factionAskCounts = new();
        private readonly Dictionary<BiologicalTradeItem, int> _bioOfferCounts = new();
        private readonly Dictionary<BiologicalTradeItem, Label> _bioCountLabels = new();
        private readonly List<Button> _bioStepperButtons = new();

        // ── UI Controls ──────────────────────────────────────────────
        // Header
        private TextureRect _textureFactionEmblem;
        private Label _lblFactionName;
        private Label _lblLeader;
        private Label _badgeStance;
        private Label _lblTellPlate;
        private Label _lblTrust;
        private Label _lblAggression;
        private Label _lblRepels;
        private Label _lblParleyStatus;

        // News from Outside strip (world phase/day + shocks + scarcity)
        private Label _lblPhaseDay;
        private HBoxContainer _newsStrip;
        private HBoxContainer _shocksContainer;
        private Label _lblScarcitySummary;

        // Columns
        private VBoxContainer _playerOfferList;
        private Label _lblPlayerWorth;
        private VBoxContainer _bioTradeContainer;
        private readonly List<HBoxContainer> _bioTradeRows = new();

        private VBoxContainer _factionStockList;
        private Label _lblFactionAskWorth;

        // Grim drawer (biological trading, collapsed by default)
        private Button _grimDrawerToggle;
        private VBoxContainer _grimDrawerBody;

        // Arbitrator
        private Label _lblFairness;
        private ColorRect _scalePlayerFill;
        private ColorRect _scaleFactionFill;
        private Button _btnConfirmTrade;
        private Button _btnDemandParley;

        // Radio Ticker
        private Label _lblRadioTicker;

        // ── Probing / Verification Surface ───────────────────────────
        public bool HasFactionEmblem => _textureFactionEmblem?.Texture != null;
        public bool HasLeaderLabel => !string.IsNullOrEmpty(_lblLeader?.Text);
        public bool HasStanceBadge => !string.IsNullOrEmpty(_badgeStance?.Text);
        public bool HasTrustMeter => !string.IsNullOrEmpty(_lblTrust?.Text);
        public bool HasAggressionMeter => !string.IsNullOrEmpty(_lblAggression?.Text);
        public bool HasRepelCounter => !string.IsNullOrEmpty(_lblRepels?.Text);
        public bool HasPriceShockBanner => _shocksContainer != null;
        public bool HasBioTradeRows => _bioTradeRows.Count >= 4;
        public bool HasFairnessIndicator => !string.IsNullOrEmpty(_lblFairness?.Text);
        public bool HasParleyButton => _btnDemandParley != null;
        public bool HasRadioTicker => _lblRadioTicker != null;
        public bool HasTellPlate => !string.IsNullOrEmpty(_lblTellPlate?.Text);
        public bool HasNewsStrip => _newsStrip != null && _newsStrip.GetChildCount() > 0;
        public bool HasArbitratorScale => _scalePlayerFill != null && _scaleFactionFill != null;
        public bool IsGrimDrawerCollapsed => _grimDrawerBody == null || !_grimDrawerBody.Visible;
        public bool IsViewModelBound => _viewModel != null;
        public int ActiveOfferCount => _playerOfferCounts.Count;
        public int ActiveAskCount => _factionAskCounts.Count;
        public int ActiveBioCount => _bioOfferCounts.Count;

        /// <summary>Raised when the overlay requests dismissal (host wires Escape/close).</summary>
        public event Action? OnClose;

        public void Open()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
        }

        public void Close()
        {
            Visible = false;
            OnClose?.Invoke();
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            CustomMinimumSize = new Vector2(global::Ashfall.Core.UI.Theme.TradePanelMinWidth, global::Ashfall.Core.UI.Theme.TradePanelMaxHeight);

            // Apply 9-slice panel background via shared helper (frame_9slice first)
            AddThemeStyleboxOverride("panel", MakePanelFrameStyleBox());

            BuildLayout();
        }

        private void BuildLayout()
        {
            var mainVbox = new VBoxContainer();
            mainVbox.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingSm);
            AddChild(mainVbox);

            // 1. Header Bar (9-slice via shared helper; falls back gracefully)
            var headerContainer = new PanelContainer();
            headerContainer.AddThemeStyleboxOverride("panel", MakeHeaderFrameStyleBox());

            var headerHbox = new HBoxContainer();
            headerHbox.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingMd);
            headerContainer.AddChild(headerHbox);

            // Faction Emblem
            _textureFactionEmblem = new TextureRect
            {
                CustomMinimumSize = new Vector2(40, 40),
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered
            };
            headerHbox.AddChild(_textureFactionEmblem);

            // Faction & Leader Info
            var titleVbox = new VBoxContainer();
            _lblFactionName = new Label
            {
                Text = "FACTION: SCAVENGER CAMP",
                HorizontalAlignment = HorizontalAlignment.Left
            };
            _lblFactionName.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeH3);
            _lblFactionName.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Warm));
            titleVbox.AddChild(_lblFactionName);

            _lblLeader = new Label
            {
                Text = "Leader: Varek (gen 1)",
                HorizontalAlignment = HorizontalAlignment.Left
            };
            _lblLeader.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
            _lblLeader.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Muted));
            titleVbox.AddChild(_lblLeader);
            headerHbox.AddChild(titleVbox);

            headerHbox.AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Stance & Trust Meters
            var statusVbox = new VBoxContainer();
            statusVbox.Alignment = BoxContainer.AlignmentMode.End;

            var statusHbox = new HBoxContainer();
            statusHbox.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingSm);

            _badgeStance = new Label { Text = "[ STANCE: TRADE ]" };
            _badgeStance.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeBody);
            _badgeStance.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Hot));
            statusHbox.AddChild(_badgeStance);

            _lblTrust = new Label { Text = "Trust: +10" };
            _lblTrust.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
            _lblTrust.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Pale));
            statusHbox.AddChild(_lblTrust);
            statusVbox.AddChild(statusHbox);

            var metaHbox = new HBoxContainer();
            metaHbox.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingSm);

            _lblAggression = new Label { Text = "Aggression: 0.40" };
            _lblAggression.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
            _lblAggression.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Muted));
            metaHbox.AddChild(_lblAggression);

            _lblRepels = new Label { Text = "Holds: x0" };
            _lblRepels.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
            _lblRepels.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Muted));
            metaHbox.AddChild(_lblRepels);

            _lblParleyStatus = new Label { Text = "" };
            _lblParleyStatus.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
            _lblParleyStatus.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Warm));
            metaHbox.AddChild(_lblParleyStatus);

            statusVbox.AddChild(metaHbox);
            headerHbox.AddChild(statusVbox);
            mainVbox.AddChild(headerContainer);

            // 1b. Tell Plate — the trader's readable posture (data-defined line)
            _lblTellPlate = new Label
            {
                Text = "",
                HorizontalAlignment = HorizontalAlignment.Left,
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _lblTellPlate.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
            _lblTellPlate.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Muted));
            mainVbox.AddChild(_lblTellPlate);

            // 2. News from Outside strip — the world intruding on the deal
            _newsStrip = new HBoxContainer();
            _newsStrip.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingMd);

            var lblNewsTitle = new Label { Text = "NEWS FROM OUTSIDE ·" };
            lblNewsTitle.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
            lblNewsTitle.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Dim));
            _newsStrip.AddChild(lblNewsTitle);

            _lblPhaseDay = new Label { Text = "Phase: CivilWar · Day 1" };
            _lblPhaseDay.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
            _lblPhaseDay.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Pale));
            _newsStrip.AddChild(_lblPhaseDay);

            _shocksContainer = new HBoxContainer();
            _shocksContainer.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingSm);
            _newsStrip.AddChild(_shocksContainer);

            _lblScarcitySummary = new Label { Text = "Scarcity: Normal" };
            _lblScarcitySummary.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
            _lblScarcitySummary.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Muted));
            _newsStrip.AddChild(_lblScarcitySummary);

            mainVbox.AddChild(_newsStrip);

            // 3. Two-Column Barter Body
            var columnsHbox = new HBoxContainer();
            columnsHbox.SizeFlagsVertical = SizeFlags.ExpandFill;
            columnsHbox.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingMd);

            // Left Column: Player Offers
            var playerCol = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            var lblOfferTitle = new Label { Text = "YOUR OFFER" };
            lblOfferTitle.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeH3);
            lblOfferTitle.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Pale));
            playerCol.AddChild(lblOfferTitle);

            var playerScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill, CustomMinimumSize = new Vector2(0, 140) };
            _playerOfferList = new VBoxContainer();
            playerScroll.AddChild(_playerOfferList);
            playerCol.AddChild(playerScroll);

            _lblPlayerWorth = new Label { Text = "Offer Worth: None" };
            _lblPlayerWorth.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
            _lblPlayerWorth.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Warm));
            playerCol.AddChild(_lblPlayerWorth);

            // Biological Trade — the grim drawer beneath the table.
            // Collapsed by default; deliberately unsettling, never required.
            _grimDrawerToggle = new Button
            {
                Text = "▸ BIOLOGICAL — THE DRAWER",
                ToggleMode = true,
                ButtonPressed = false
            };
            _grimDrawerToggle.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
            _grimDrawerToggle.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Critical));
            _grimDrawerToggle.Toggled += on =>
            {
                _grimDrawerBody.Visible = on;
                _grimDrawerToggle.Text = on ? "▾ BIOLOGICAL — THE DRAWER" : "▸ BIOLOGICAL — THE DRAWER";
            };
            playerCol.AddChild(_grimDrawerToggle);

            _grimDrawerBody = new VBoxContainer { Visible = false };
            playerCol.AddChild(_grimDrawerBody);

            _bioTradeContainer = new VBoxContainer();
            BuildBioTradeRows();
            _grimDrawerBody.AddChild(_bioTradeContainer);

            columnsHbox.AddChild(playerCol);

            // Right Column: Faction Stock / Demands
            var factionCol = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            var lblFactionTitle = new Label { Text = "THEIR STOCK & ASKS" };
            lblFactionTitle.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeH3);
            lblFactionTitle.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Pale));
            factionCol.AddChild(lblFactionTitle);

            var factionScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill, CustomMinimumSize = new Vector2(0, 140) };
            _factionStockList = new VBoxContainer();
            factionScroll.AddChild(_factionStockList);
            factionCol.AddChild(factionScroll);

            _lblFactionAskWorth = new Label { Text = "Demand Worth: None" };
            _lblFactionAskWorth.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
            _lblFactionAskWorth.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Warm));
            factionCol.AddChild(_lblFactionAskWorth);

            columnsHbox.AddChild(factionCol);
            mainVbox.AddChild(columnsHbox);

            // 4. Center Arbitrator & Buttons
            var arbitratorHbox = new HBoxContainer();
            arbitratorHbox.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingMd);

            // The arbitrator's scale: balance bar etched at table center.
            // The explicit FAIR/SHORT label is retained (gates check the label).
            _scalePlayerFill = new ColorRect
            {
                Color = ToGodotColor(global::Ashfall.Core.UI.Theme.Hot),
                CustomMinimumSize = new Vector2(70, 8)
            };
            _scaleFactionFill = new ColorRect
            {
                Color = ToGodotColor(global::Ashfall.Core.UI.Theme.Muted),
                CustomMinimumSize = new Vector2(70, 8)
            };
            arbitratorHbox.AddChild(_scalePlayerFill);
            arbitratorHbox.AddChild(_scaleFactionFill);

            _lblFairness = new Label { Text = "DEAL IS FAIR" };
            _lblFairness.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeH3);
            _lblFairness.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Hot));
            arbitratorHbox.AddChild(_lblFairness);

            arbitratorHbox.AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });

            _btnDemandParley = new Button { Text = "DEMAND PARLEY [P]", Visible = false };
            _btnDemandParley.Pressed += () => DemandParley();
            arbitratorHbox.AddChild(_btnDemandParley);

            _btnConfirmTrade = new Button { Text = "CONFIRM BARTER" };
            _btnConfirmTrade.Pressed += () => ExecuteTrade();
            arbitratorHbox.AddChild(_btnConfirmTrade);

            mainVbox.AddChild(arbitratorHbox);

            // 5. Bottom Radio Ticker
            _lblRadioTicker = new Label
            {
                Text = "RADIO: Monitoring frequency 104.7 MHz...",
                HorizontalAlignment = HorizontalAlignment.Left
            };
            _lblRadioTicker.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
            _lblRadioTicker.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Dim));
            mainVbox.AddChild(_lblRadioTicker);
        }

        private void BuildBioTradeRows()
        {
            _bioTradeRows.Clear();
            _bioCountLabels.Clear();
            _bioStepperButtons.Clear();
            var bioItems = new[]
            {
                (BiologicalTradeItem.PintOfBlood, "Pint of Blood", "icon_bio_blood.png"),
                (BiologicalTradeItem.BoneMarrow, "Bone Marrow", "icon_bio_marrow.png"),
                (BiologicalTradeItem.Plasma, "Plasma", "icon_bio_plasma.png"),
                (BiologicalTradeItem.Organ, "Organ", "icon_bio_organ.png")
            };

            foreach (var (bioKind, name, iconFile) in bioItems)
            {
                var row = new HBoxContainer();
                row.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingXs);

                var iconRect = new TextureRect
                {
                    CustomMinimumSize = new Vector2(20, 20),
                    StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                    Texture = LoadTexture($"res://Assets/UI/Icons/{iconFile}")
                };
                row.AddChild(iconRect);

                var lbl = new Label { Text = name, CustomMinimumSize = new Vector2(100, 0) };
                lbl.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
                lbl.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Pale));
                row.AddChild(lbl);

                var btnMinus = new Button { Text = "-" };
                var btnPlus = new Button { Text = "+" };
                var countLbl = new Label { Text = "0", CustomMinimumSize = new Vector2(20, 0), HorizontalAlignment = HorizontalAlignment.Center };
                countLbl.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);

                btnMinus.Pressed += () =>
                {
                    int cur = _bioOfferCounts.GetValueOrDefault(bioKind, 0);
                    if (cur > 0)
                    {
                        _bioOfferCounts[bioKind] = cur - 1;
                        countLbl.Text = (cur - 1).ToString();
                        RefreshCalculations();
                    }
                };

                btnPlus.Pressed += () =>
                {
                    int cur = _bioOfferCounts.GetValueOrDefault(bioKind, 0);
                    _bioOfferCounts[bioKind] = cur + 1;
                    countLbl.Text = (cur + 1).ToString();
                    RefreshCalculations();
                };

                row.AddChild(btnMinus);
                row.AddChild(countLbl);
                row.AddChild(btnPlus);

                _bioTradeContainer.AddChild(row);
                _bioTradeRows.Add(row);
                _bioCountLabels[bioKind] = countLbl;
                _bioStepperButtons.Add(btnMinus);
                _bioStepperButtons.Add(btnPlus);
            }
        }

        // ── Binding & Lifecycle ──────────────────────────────────────

        private IFactionRadioProvider _radioProvider;
        private ISeededRng _rng;

        /// <summary>
        /// Skin-track / convergence binding: paint the staged table from the
        /// Act 0 seam. When bound, this takes priority over the session binding
        /// and the panel never touches simulation types.
        /// </summary>
        public void BindViewModel(ITradeScreenViewModel viewModel, ITradeIntentSink intentSink)
        {
            if (_viewModel != null)
            {
                _viewModel.Changed -= OnViewModelChanged;
            }
            _viewModel = viewModel;
            _intentSink = intentSink;
            if (_viewModel != null)
            {
                _viewModel.Changed += OnViewModelChanged;
            }
            OnViewModelChanged();
        }

        private void OnViewModelChanged()
        {
            RefreshView();
        }

        public void BindSession(
            EconomyHostSession session,
            IFactionStanceProvider stanceProvider = null,
            IPriceShockProvider priceShockProvider = null,
            IFactionRadioProvider radioProvider = null,
            ISeededRng rng = null)
        {
            _session = session;
            _stanceProvider = stanceProvider;
            _priceShockProvider = priceShockProvider;
            _radioProvider = radioProvider;
            _rng = rng ?? new SeededRng(2026);

            if (_viewModel != null)
            {
                _viewModel.Changed -= OnViewModelChanged;
                _viewModel = null;
                _intentSink = null;
            }

            if (_session != null)
            {
                _session.StateChanged += RefreshView;
            }

            RefreshView();
        }

        public void SetActiveFaction(string factionId)
        {
            _activeFactionId = factionId;
            RefreshView();
        }

        public void AddPlayerOffer(string itemId, int count)
        {
            if (count <= 0) _playerOfferCounts.Remove(itemId);
            else _playerOfferCounts[itemId] = count;
            RefreshCalculations();
        }

        public void AddFactionAsk(string itemId, int count)
        {
            if (count <= 0) _factionAskCounts.Remove(itemId);
            else _factionAskCounts[itemId] = count;
            RefreshCalculations();
        }

        public void RefreshView()
        {
            if (_viewModel != null)
            {
                RefreshFromViewModel();
                return;
            }
            if (_session == null) return;

            // The tell plate is seam-driven; clear it in session mode.
            if (_lblTellPlate != null) _lblTellPlate.Text = "";

            // 1. Update Header Fields
            _lblFactionName.Text = $"FACTION: {_activeFactionId.ToUpper().Replace('_', ' ')}";
            _textureFactionEmblem.Texture = AtomicWar.GodotApp.FactionIconLoader.LoadFor(_activeFactionId);

            float trust = _stanceProvider?.GetEffectiveTrust(_activeFactionId) ?? 0f;
            var stance = _stanceProvider?.GetStance(_activeFactionId) ?? TradeStance.Trade;
            float aggression = _stanceProvider?.GetRaidAggression(_activeFactionId) ?? 0.5f;

            _lblLeader.Text = $"Leader: {_activeFactionId} Commander";
            _badgeStance.Text = $"[ STANCE: {stance.ToString().ToUpper()} ]";
            _badgeStance.AddThemeColorOverride("font_color", GetStanceColor(stance));

            _lblTrust.Text = $"Trust: {trust:+0;-0;0}";
            _lblAggression.Text = $"Aggression: {aggression:0.00}";
            _lblRepels.Text = "Holds: x0";

            // 2. Update Market & Shocks Banner
            int day = _session.Market?.Day ?? 1;
            _lblPhaseDay.Text = $"Phase: CivilWar · Day {day}";

            foreach (Node child in _shocksContainer.GetChildren())
                child.QueueFree();

            if (_priceShockProvider != null)
            {
                var kinds = new[] { PriceShockKind.PlumePassing, PriceShockKind.ConvoyAmbush, PriceShockKind.FactionWar, PriceShockKind.WinterDeepens };
                var activeBadges = new List<ShockBadgeData>();
                foreach (var k in kinds)
                {
                    if (_priceShockProvider.TryGetPriceShock(k, day, out var rule))
                    {
                        activeBadges.Add(new ShockBadgeData(rule.Kind, rule.Multiplier, rule.Trigger));
                    }
                }
                RebuildShockBadges(activeBadges);
            }

            // 3. Populate Stock Lists
            PopulateGoodsLists();

            // 4. Update Arbitrator
            RefreshCalculations();

            // 5. Update Radio Ticker
            if (_radioProvider != null && _lblRadioTicker != null)
            {
                var intercept = _radioProvider.GetFactionEvent(_activeFactionId, RadioEventKind.InterceptChatter, day, _rng);
                _lblRadioTicker.Text = $"RADIO: [{intercept.Callsign}] {intercept.Message}";
            }
        }

        /// <summary>
        /// Paints the staged table purely from the Act 0 seam. Reads only;
        /// zero simulation types touched.
        /// </summary>
        private void RefreshFromViewModel()
        {
            var vm = _viewModel;

            // 1. Header — the trader's presence
            _lblFactionName.Text = vm.FactionName.Length > 0
                ? $"FACTION: {vm.FactionName.ToUpperInvariant()}"
                : "FACTION: —";
            _textureFactionEmblem.Texture = AtomicWar.GodotApp.FactionIconLoader.LoadFor(vm.FactionId);
            _lblLeader.Text = $"Leader: {vm.LeaderName} (gen {vm.SuccessionGeneration})";
            _badgeStance.Text = vm.StanceBadgeText;
            _badgeStance.AddThemeColorOverride("font_color", GetStanceColor(vm.Stance));

            // 2. Tell plate — posture readable from the plate alone
            _lblTellPlate.Text = vm.StanceTellLine;

            // 3. Ledger-edge meters
            _lblTrust.Text = $"Trust: {vm.Trust:+0;-0;0}";
            _lblAggression.Text = $"Aggression: {vm.Aggression:0.00}";
            _lblRepels.Text = $"Holds: x{vm.ConsecutiveRepels}";
            _lblParleyStatus.Text = vm.HasSurrendered
                ? "[S] SURRENDERED"
                : (vm.CanDemandParley ? "[P] PARLEY READY" : "");

            // 4. News from Outside strip
            _lblPhaseDay.Text = $"Phase: {vm.WorldPhaseLabel} · Day {vm.WorldDay}";
            RebuildShockBadges(vm.ShockBadges);
            if (vm.ScarcityMultipliers.Count > 0)
            {
                var parts = new List<string>();
                foreach (var band in vm.ScarcityMultipliers)
                {
                    parts.Add($"{band.DisplayName} x{band.Multiplier:0.0}");
                }
                _lblScarcitySummary.Text = $"Scarcity: {string.Join(", ", parts)}";
            }
            else
            {
                _lblScarcitySummary.Text = "Scarcity: Normal";
            }

            // 5. Table edges — what you push across vs what they guard
            ClearList(_playerOfferList);
            ClearList(_factionStockList);
            foreach (var line in vm.PlayerOffers)
            {
                _playerOfferList.AddChild(MakeTableLineLabel($"{line.Quantity}x {line.DisplayName} — {line.WorthLabel}"));
            }
            foreach (var line in vm.FactionDemands)
            {
                _factionStockList.AddChild(MakeTableLineLabel($"{line.Quantity}x {line.DisplayName} — {line.WorthLabel}"));
            }
            if (vm.PlayerOffers.Count == 0 && vm.BiologicalOffers.Count == 0)
            {
                _playerOfferList.AddChild(MakeTableLineLabel("— your edge of the table is bare —", dimmed: true));
            }
            if (vm.FactionDemands.Count == 0)
            {
                _factionStockList.AddChild(MakeTableLineLabel("— their edge of the table is bare —", dimmed: true));
            }

            int bioCount = 0;
            foreach (var pair in vm.BiologicalOffers) bioCount += pair.Value;
            _lblPlayerWorth.Text = $"Offer Worth: {Ashfall.Core.Economy.TradeWorthLabels.Format(vm.PlayerOfferValue)} ({vm.PlayerOffers.Count} items, {bioCount} bio)";
            _lblFactionAskWorth.Text = $"Demand Worth: {Ashfall.Core.Economy.TradeWorthLabels.Format(vm.FactionAskValue)} ({vm.FactionDemands.Count} items)";

            // 6. Grim drawer — counts shown, steppers are session-mode controls
            foreach (var pair in _bioCountLabels)
            {
                pair.Value.Text = vm.BiologicalOffers.TryGetValue(pair.Key, out int count) ? count.ToString() : "0";
            }
            bool steppersVisible = _session != null;
            foreach (var stepper in _bioStepperButtons)
            {
                stepper.Visible = steppersVisible;
            }

            // 7. Arbitrator — diegetic scale + explicit label (gates check the label)
            _lblFairness.Text = vm.FairnessLabel;
            _lblFairness.AddThemeColorOverride("font_color", GetFairnessColor(vm.Fairness));
            _btnConfirmTrade.Disabled = !vm.CanConfirm;
            _btnDemandParley.Visible = vm.CanDemandParley;
            UpdateArbitratorScale(vm.PlayerOfferValue, vm.FactionAskValue);

            // 8. Radio — the room's radio, unchanged position
            _lblRadioTicker.Text = vm.RadioTickerLine;
        }

        private void RebuildShockBadges(IReadOnlyList<ShockBadgeData> badges)
        {
            foreach (Node child in _shocksContainer.GetChildren())
                child.QueueFree();

            if (badges == null) return;
            foreach (var badge in badges)
            {
                var shockBadge = new HBoxContainer();
                var icon = new TextureRect
                {
                    CustomMinimumSize = new Vector2(16, 16),
                    StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                    Texture = LoadTexture(GetShockIconPath(badge.Kind))
                };
                shockBadge.AddChild(icon);
                var lbl = new Label { Text = $"{badge.Kind} (x{badge.Multiplier:0.0})" };
                lbl.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeLabel);
                lbl.AddThemeColorOverride("font_color", ToGodotColor(global::Ashfall.Core.UI.Theme.Warm));
                shockBadge.AddChild(lbl);
                _shocksContainer.AddChild(shockBadge);
            }
        }

        private static void ClearList(VBoxContainer list)
        {
            foreach (Node child in list.GetChildren())
                child.QueueFree();
        }

        private Label MakeTableLineLabel(string text, bool dimmed = false)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
            lbl.AddThemeColorOverride("font_color", ToGodotColor(dimmed ? global::Ashfall.Core.UI.Theme.Dim : global::Ashfall.Core.UI.Theme.Pale));
            return lbl;
        }

        private void UpdateArbitratorScale(float playerVal, float factionVal)
        {
            float total = playerVal + factionVal;
            float ratio = total > 0f ? playerVal / total : 0.5f;
            const float width = 140f;
            _scalePlayerFill.CustomMinimumSize = new Vector2(Mathf.Max(6f, width * ratio), 8);
            _scaleFactionFill.CustomMinimumSize = new Vector2(Mathf.Max(6f, width * (1f - ratio)), 8);
        }

        private static Color GetFairnessColor(TradeFairness fairness)
        {
            switch (fairness)
            {
                case TradeFairness.Fair:
                    return ToGodotColor(global::Ashfall.Core.UI.Theme.Hot);
                case TradeFairness.Short:
                    return ToGodotColor(global::Ashfall.Core.UI.Theme.Critical);
                default:
                    return ToGodotColor(global::Ashfall.Core.UI.Theme.Dim);
            }
        }

        private void PopulateGoodsLists()
        {
            if (_session?.Catalog == null) return;

            foreach (Node child in _playerOfferList.GetChildren())
                child.QueueFree();
            foreach (Node child in _factionStockList.GetChildren())
                child.QueueFree();

            foreach (var good in _session.Catalog.All())
            {
                // Offer row
                var offerRow = new HBoxContainer();
                offerRow.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingSm);

                var icon1 = new TextureRect
                {
                    CustomMinimumSize = new Vector2(24, 24),
                    StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                    Texture = AssetRegistry.GetItem(good.id).Texture
                };
                offerRow.AddChild(icon1);

                var lblGood1 = new Label { Text = good.displayName, CustomMinimumSize = new Vector2(120, 0) };
                lblGood1.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
                offerRow.AddChild(lblGood1);

                var btnAddOffer = new Button { Text = "+ Offer" };
                string gId = good.id;
                btnAddOffer.Pressed += () =>
                {
                    int cur = _playerOfferCounts.GetValueOrDefault(gId, 0);
                    AddPlayerOffer(gId, cur + 1);
                };
                offerRow.AddChild(btnAddOffer);
                _playerOfferList.AddChild(offerRow);

                // Ask row
                var askRow = new HBoxContainer();
                askRow.AddThemeConstantOverride("separation", global::Ashfall.Core.UI.Theme.SpacingSm);

                var icon2 = new TextureRect
                {
                    CustomMinimumSize = new Vector2(24, 24),
                    StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                    Texture = AssetRegistry.GetItem(good.id).Texture
                };
                askRow.AddChild(icon2);

                float price = _session.Market.GetPrice(good.id);
                var lblGood2 = new Label { Text = $"{good.displayName} ({price:0.00})", CustomMinimumSize = new Vector2(140, 0) };
                lblGood2.AddThemeFontSizeOverride("font_size", global::Ashfall.Core.UI.Theme.FontSizeSmall);
                askRow.AddChild(lblGood2);

                var btnAddAsk = new Button { Text = "+ Ask" };
                btnAddAsk.Pressed += () =>
                {
                    int cur = _factionAskCounts.GetValueOrDefault(gId, 0);
                    AddFactionAsk(gId, cur + 1);
                };
                askRow.AddChild(btnAddAsk);
                _factionStockList.AddChild(askRow);
            }
        }

        private void RefreshCalculations()
        {
            float playerVal = 0f;
            foreach (var (itemId, count) in _playerOfferCounts)
            {
                float baseP = _session?.Market?.GetPrice(itemId) ?? 10f;
                playerVal += baseP * count;
            }
            foreach (var (bio, count) in _bioOfferCounts)
            {
                playerVal += global::Ashfall.Core.Economy.TradePricing.BioUnitValue(bio) * count;
            }

            float factionVal = 0f;
            foreach (var (itemId, count) in _factionAskCounts)
            {
                float baseP = _session?.Market?.GetPrice(itemId) ?? 10f;
                factionVal += baseP * count;
            }

            if (_lblPlayerWorth != null)
            {
                _lblPlayerWorth.Text = $"Offer Worth: {FormatWorthLabel(playerVal)} ({_playerOfferCounts.Count} items, {_bioOfferCounts.Count} bio)";
            }
            if (_lblFactionAskWorth != null)
            {
                _lblFactionAskWorth.Text = $"Demand Worth: {FormatWorthLabel(factionVal)} ({_factionAskCounts.Count} items)";
            }

            foreach (var pair in _bioCountLabels)
            {
                pair.Value.Text = _bioOfferCounts.GetValueOrDefault(pair.Key, 0).ToString();
            }

            bool isFair = playerVal >= factionVal;
            if (_lblFairness != null)
            {
                _lblFairness.Text = isFair ? "DEAL IS FAIR" : "OFFER SHORT";
                _lblFairness.AddThemeColorOverride("font_color", isFair ? ToGodotColor(global::Ashfall.Core.UI.Theme.Hot) : ToGodotColor(global::Ashfall.Core.UI.Theme.Critical));
            }

            UpdateArbitratorScale(playerVal, factionVal);

            var stance = _stanceProvider?.GetStance(_activeFactionId) ?? TradeStance.Trade;
            bool willTrade = stance == TradeStance.Trade || stance == TradeStance.ShareIntel;
            if (_btnConfirmTrade != null)
            {
                _btnConfirmTrade.Disabled = !isFair || !willTrade;
            }
        }

        private void ExecuteTrade()
        {
            if (_intentSink != null)
            {
                _intentSink.TryConfirmTrade();
                RefreshView();
                return;
            }

            if (_lblRadioTicker != null)
            {
                if (_radioProvider != null)
                {
                    var intercept = _radioProvider.GetFactionEvent(_activeFactionId, RadioEventKind.TradeReaction, _session?.Market?.Day ?? 1, _rng);
                    _lblRadioTicker.Text = $"RADIO: [{intercept.Callsign}] {intercept.Message}";
                }
                else
                {
                    _lblRadioTicker.Text = "RADIO: Barter confirmed. Goods exchanged at checkpoint.";
                }
            }
            _playerOfferCounts.Clear();
            _factionAskCounts.Clear();
            _bioOfferCounts.Clear();
            RefreshCalculations();
        }

        private void DemandParley()
        {
            if (_intentSink != null)
            {
                _intentSink.TryDemandParley();
                return;
            }

            if (_lblRadioTicker != null)
            {
                if (_radioProvider != null)
                {
                    var intercept = _radioProvider.GetFactionEvent(_activeFactionId, RadioEventKind.ParleyResolution, _session?.Market?.Day ?? 1, _rng);
                    _lblRadioTicker.Text = $"RADIO: [{intercept.Callsign}] {intercept.Message}";
                }
                else
                {
                    _lblRadioTicker.Text = "RADIO: Parley demand transmitted. Awaiting emissary.";
                }
            }
        }

        // ── Presentation Helpers ─────────────────────────────────────

        private static Color ToGodotColor((float r, float g, float b, float a) token)
        {
            return ToColor(token);
        }

        private static Color GetStanceColor(TradeStance stance)
        {
            switch (stance)
            {
                case TradeStance.ShareIntel:
                case TradeStance.Trade:
                    return ToGodotColor(global::Ashfall.Core.UI.Theme.Hot);
                case TradeStance.Refuse:
                    return ToGodotColor(global::Ashfall.Core.UI.Theme.Muted);
                case TradeStance.Rob:
                    return ToGodotColor(global::Ashfall.Core.UI.Theme.Entropy);
                case TradeStance.HostileRaid:
                default:
                    return ToGodotColor(global::Ashfall.Core.UI.Theme.Critical);
            }
        }

        private static string GetShockIconPath(PriceShockKind kind)
        {
            switch (kind)
            {
                case PriceShockKind.PlumePassing: return "res://Assets/UI/Icons/icon_shock_plume.png";
                case PriceShockKind.ConvoyAmbush: return "res://Assets/UI/Icons/icon_shock_convoy.png";
                case PriceShockKind.FactionWar: return "res://Assets/UI/Icons/icon_shock_war.png";
                case PriceShockKind.WinterDeepens: return "res://Assets/UI/Icons/icon_shock_winter.png";
                default: return "res://Assets/UI/Icons/icon_shock_plume.png";
            }
        }

        private static string FormatWorthLabel(float value)
        {
            // One source of truth: the core owns the qualitative thresholds.
            return global::Ashfall.Core.Economy.TradeWorthLabels.Format(value);
        }

        private static Texture2D LoadTexture(string path)
        {
            return TryLoadTexture(path);
        }
    }
}
