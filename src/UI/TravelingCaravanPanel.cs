// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Traveling Caravans & Regional Trade Route Panel.
    /// Manages itinerant merchants, wasteland route nodes, daily movements,
    /// and ration-based outpost barter.
    /// </summary>
    public partial class TravelingCaravanPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _caravanList = null!;
        private VBoxContainer _caravanInspector = null!;
        private VBoxContainer _routeLogContainer = null!;
        private Label _eventLogLabel = null!;

        private TravelingCaravanHostSession? _host;
        private TradeVoiceResolver? _voiceResolver;
        private Func<WeatherKind>? _weatherProvider;
        private string? _selectedCaravanId;
        private string _voiceEvent = string.Empty;
        /// <summary>Plan 14A (B1) — last embargo transition reason, set on the
        /// OnCaravanEmbargoed edge and kept only while a route stays blocked.</summary>
        private string _embargoReason = string.Empty;

        public bool IsBound => _host != null;
        public string CurrentTraderProfileId { get; private set; } = string.Empty;

        public void Bind(
            TravelingCaravanHostSession session,
            TradeVoiceResolver? voiceResolver = null,
            Func<WeatherKind>? weatherProvider = null)
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
                _host.Engine.OnCaravanEmbargoed -= OnCaravanEmbargoed;
                _host.Engine.OnCaravanResumed -= OnCaravanResumed;
            }
            _host = session;
            _voiceResolver = voiceResolver;
            _weatherProvider = weatherProvider;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
                // Plan 14A (B1) clearing edge: stale BLOCKED state must clear on
                // the resume event, never wait for a poll or a reopen.
                _host.Engine.OnCaravanEmbargoed += OnCaravanEmbargoed;
                _host.Engine.OnCaravanResumed += OnCaravanResumed;
            }
            RefreshView();
        }

        private void OnCaravanEmbargoed(CaravanEntry caravan, string weatherReason)
        {
            _embargoReason = weatherReason ?? string.Empty;
            RefreshView();
        }

        private void OnCaravanResumed(CaravanEntry caravan)
        {
            if (_host != null && !_host.Engine.State.activeCaravans.Exists(c => c.embargoBlocked))
                _embargoReason = string.Empty; // no blocked routes remain
            RefreshView();
        }

        /// <summary>Plan 14A (B1) — the card/route state string. The blocked
        /// flag is the Core authority's durable transition state; the weather
        /// reason rides the embargo transition event (held only while a route
        /// remains blocked).</summary>
        private string ResolveRouteStateText(CaravanEntry caravan)
        {
            if (caravan.embargoBlocked)
            {
                string reason = string.IsNullOrEmpty(_embargoReason) ? "weather" : _embargoReason;
                return $"ROUTE: BLOCKED — {reason}";
            }
            float progress = ResolveRouteProgress(caravan);
            if (progress < 1f)
                return $"ROUTE: SLOWED (x{progress:0.00})";
            return "ROUTE: NOMINAL";
        }

        /// <summary>Plan 14B (B1) — the current weather's route multiplier for
        /// this caravan's origin region, from the embargo authority. The panel
        /// never computes weather gating.</summary>
        private float ResolveRouteProgress(CaravanEntry caravan)
        {
            var embargoes = _host?.Engine.Embargoes;
            if (embargoes == null || string.IsNullOrEmpty(caravan.originRegion))
                return 1f;
            if (_weatherProvider == null) return 1f; // no weather bound — neutral projection
            return embargoes.GetRouteProgressMultiplier(caravan.originRegion, _weatherProvider());
        }

        /// <summary>Plan 14B (B1) — specialty cargo: the caravan's authored
        /// regional stock (its inventory), named by the best available label.
        /// Pure read of existing state.</summary>
        private static string ResolveSpecialtyCargo(CaravanEntry caravan)
        {
            if (caravan.inventory == null || caravan.inventory.Count == 0) return string.Empty;
            var top = caravan.inventory
                .Where(i => i != null && !string.IsNullOrEmpty(i.itemId))
                .OrderByDescending(i => i.quantity)
                .ThenBy(i => i.itemId, StringComparer.Ordinal)
                .Take(2)
                .ToList();
            if (top.Count == 0) return string.Empty;
            return string.Join(", ", top.Select(i => $"{i.itemId} x{i.quantity}"));
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.04f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            _shell = new AshfallDashboardShell("SYS: TRAVELING CARAVANS & REGIONAL TRADE // ROUTE RADAR", minWidth: 1040, minHeight: 680);
            center.AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("caravans", "ACTIVE CARAVANS", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("docked", "DOCKED AT HOLDFAST", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("trades", "TRADES LOGGED", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("routes", "TRADE LANES", "SECURE", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("status", "ROUTE RADAR", "SCANNING", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _shell.AttachHeaderCloseButton("CLOSE [Esc]", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            // 3-Column Layout
            var gridRow = new HBoxContainer();
            gridRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            gridRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            gridRow.SizeFlagsVertical = SizeFlags.ExpandFill;

            // Column 1: Active Caravans List
            var leftPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            leftPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftPanel.SizeFlagsStretchRatio = 0.95f;
            var leftMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            leftPanel.AddChild(leftMargin);
            var leftVbox = new VBoxContainer();
            leftVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            leftMargin.AddChild(leftVbox);
            leftVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("ACTIVE TRADE CARAVANS"));
            var leftScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _caravanList = new VBoxContainer();
            _caravanList.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _caravanList.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftScroll.AddChild(_caravanList);
            leftVbox.AddChild(leftScroll);
            gridRow.AddChild(leftPanel);

            // Column 2: Selected Caravan Inspector & Barter Actions
            var centerPanel = AshfallUiHelpers.MakePanel(minWidth: 380);
            centerPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerPanel.SizeFlagsStretchRatio = 1.2f;
            var centerMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            centerPanel.AddChild(centerMargin);
            var centerVbox = new VBoxContainer();
            centerVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            centerMargin.AddChild(centerVbox);
            centerVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("CARAVAN CARGO & BARTER"));
            var centerScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _caravanInspector = new VBoxContainer();
            _caravanInspector.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _caravanInspector.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerScroll.AddChild(_caravanInspector);
            centerVbox.AddChild(centerScroll);
            gridRow.AddChild(centerPanel);

            // Column 3: Telemetry & Trade History
            var rightPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            rightPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightPanel.SizeFlagsStretchRatio = 0.95f;
            var rightMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            rightPanel.AddChild(rightMargin);
            var rightVbox = new VBoxContainer();
            rightVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            rightMargin.AddChild(rightVbox);
            rightVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("ROUTE LOGS & DISPATCH"));
            var rightScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _routeLogContainer = new VBoxContainer();
            _routeLogContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _routeLogContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightScroll.AddChild(_routeLogContainer);
            rightVbox.AddChild(rightScroll);

            rightVbox.AddChild(AshfallUiHelpers.MakeSeparator());
            _eventLogLabel = AshfallUiHelpers.MakeMetadata("No recent caravan movements.");
            _eventLogLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            rightVbox.AddChild(_eventLogLabel);

            gridRow.AddChild(rightPanel);

            _shell.SetContent(gridRow);
            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
        }

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;

            AshfallUiHelpers.EmptyChildren(_caravanList);
            AshfallUiHelpers.EmptyChildren(_caravanInspector);
            AshfallUiHelpers.EmptyChildren(_routeLogContainer);

            var caravans = _host.Engine.State.activeCaravans;
            int totalCaravans = caravans.Count;
            int dockedAtHoldfast = caravans.Count(c => c.currentNodeId == "loc_holdfast_gate");

            _statusRail.Set("caravans", totalCaravans.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("docked", dockedAtHoldfast > 0 ? $"{dockedAtHoldfast} AT GATE" : "0 AT GATE", dockedAtHoldfast > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("trades", _host.Engine.State.completedTradesCount.ToString(), AshfallMetricCard.Criticality.Normal);
            // Plan 14A (B1): route state from the embargo authority's durable
            // blocked flags — never a panel-side weather recomputation.
            int blockedRoutes = caravans.Count(c => c.embargoBlocked);
            _statusRail.Set("routes", blockedRoutes > 0 ? $"BLOCKED x{blockedRoutes}" : "SECURE",
                blockedRoutes > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("status", totalCaravans > 0 ? "TRACKING" : "IDLE", AshfallMetricCard.Criticality.Normal);

            if (!string.IsNullOrEmpty(_voiceEvent))
            {
                _eventLogLabel.Text = _voiceEvent;
            }
            else if (!string.IsNullOrEmpty(_host.LastEvent))
            {
                _eventLogLabel.Text = _host.LastEvent;
            }

            // Populate Caravan List
            if (caravans.Count == 0)
            {
                _caravanList.AddChild(AshfallUiHelpers.MakeMetadata("No trade caravans currently en route in this sector."));
            }
            else
            {
                if (_selectedCaravanId == null || !caravans.Exists(c => c.caravanId == _selectedCaravanId))
                {
                    _selectedCaravanId = caravans[0].caravanId;
                }

                foreach (var c in caravans)
                {
                    var card = AshfallUiHelpers.MakePanel();
                    var cardMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingXs);
                    card.AddChild(cardMargin);
                    var cardVbox = new VBoxContainer();
                    cardVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
                    cardMargin.AddChild(cardVbox);

                    var headerRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                    headerRow.AddChild(AshfallUiHelpers.MakeBadgeIcon("badge_corneal_burn", 18));
                    var nameLbl = AshfallUiHelpers.MakeBody(c.caravanName);
                    nameLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    headerRow.AddChild(nameLbl);
                    cardVbox.AddChild(headerRow);

                    var routeLbl = AshfallUiHelpers.MakeMono($"LOCATION: [{c.currentNodeId}]");
                    routeLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(c.currentNodeId == "loc_holdfast_gate" ? DesignTheme.Lethe : DesignTheme.Warm));
                    cardVbox.AddChild(routeLbl);

                    // Plan 14A (B1): BLOCKED/SLOWED state as text on every card.
                    string routeState = ResolveRouteStateText(c);
                    var stateLbl = AshfallUiHelpers.MakeSmall(routeState);
                    stateLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(
                        c.embargoBlocked ? DesignTheme.Critical : DesignTheme.Dim));
                    cardVbox.AddChild(stateLbl);

                    var factionLbl = AshfallUiHelpers.MakeSmall($"FACTION: {c.factionId}");
                    factionLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
                    cardVbox.AddChild(factionLbl);

                    var selectBtn = AshfallUiHelpers.MakeButton($"INSPECT // {c.caravanId}", () =>
                    {
                        _selectedCaravanId = c.caravanId;
                        RefreshView();
                    });
                    selectBtn.CustomMinimumSize = new Vector2(0, 24);
                    cardVbox.AddChild(selectBtn);

                    _caravanList.AddChild(card);
                }
            }

            // Caravan Inspector & Barter Controls
            var curCaravan = caravans.FirstOrDefault(c => c.caravanId == _selectedCaravanId);
            if (curCaravan != null)
            {
                _caravanInspector.AddChild(AshfallUiHelpers.MakeSectionHeader($"CARAVAN: {curCaravan.caravanName.ToUpperInvariant()}"));
                _caravanInspector.AddChild(AshfallUiHelpers.MakeDataRow("Caravan ID", curCaravan.caravanId, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
                _caravanInspector.AddChild(AshfallUiHelpers.MakeDataRow("Affiliated Faction", curCaravan.factionId, AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
                _caravanInspector.AddChild(AshfallUiHelpers.MakeDataRow("Current Waypoint", curCaravan.currentNodeId, AshfallUiHelpers.ToColor(DesignTheme.Warm)));
                _caravanInspector.AddChild(AshfallUiHelpers.MakeDataRow("Scheduled Route", string.Join(" -> ", curCaravan.routeNodeIds), AshfallUiHelpers.ToColor(DesignTheme.Pale)));
                // Plan 14A/14B (B1): origin + specialty + route progress from
                // the existing read models (embargo flag, route multiplier,
                // inventory); the panel formats values, never derives them.
                _caravanInspector.AddChild(AshfallUiHelpers.MakeDataRow("Origin Region",
                    string.IsNullOrEmpty(curCaravan.originRegion) ? "—" : curCaravan.originRegion,
                    AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
                string specialty = ResolveSpecialtyCargo(curCaravan);
                if (!string.IsNullOrEmpty(specialty))
                    _caravanInspector.AddChild(AshfallUiHelpers.MakeDataRow("Specialty Cargo", specialty,
                        AshfallUiHelpers.ToColor(DesignTheme.Warm)));
                var routeStateRow = AshfallUiHelpers.MakeDataRow("Route State", ResolveRouteStateText(curCaravan),
                    AshfallUiHelpers.ToColor(curCaravan.embargoBlocked ? DesignTheme.Critical : DesignTheme.Lethe));
                _caravanInspector.AddChild(routeStateRow);
                float progress = ResolveRouteProgress(curCaravan);
                _caravanInspector.AddChild(AshfallUiHelpers.MakeDataRow("Route Progress Multiplier",
                    $"x{progress:0.00}" + (progress <= 0f ? " (blocked)" : string.Empty),
                    AshfallUiHelpers.ToColor(progress <= 0f ? DesignTheme.Critical : DesignTheme.Dim)));

                string voiceLine = ResolveCaravanVoice(curCaravan);
                if (!string.IsNullOrWhiteSpace(voiceLine))
                {
                    _caravanInspector.AddChild(AshfallUiHelpers.MakeSeparator());
                    _caravanInspector.AddChild(AshfallUiHelpers.MakeSubsectionHeader("CARAVAN VOICE"));
                    var voiceLabel = AshfallUiHelpers.MakeMetadata(voiceLine);
                    voiceLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                    _caravanInspector.AddChild(voiceLabel);
                }

                _caravanInspector.AddChild(AshfallUiHelpers.MakeSeparator());
                _caravanInspector.AddChild(AshfallUiHelpers.MakeSubsectionHeader("BARTER TRANSACTIONS"));

                var btnBuyMed = AshfallUiHelpers.MakeButton("BARTER FOR ANTIBIOTICS (5 RATIONS)", () =>
                    TryCaravanPurchase(curCaravan, "item_antibiotics", 1));
                _caravanInspector.AddChild(btnBuyMed);

                var btnBuyWater = AshfallUiHelpers.MakeButton("BARTER FOR CLEAN WATER (1 RATION)", () =>
                    TryCaravanPurchase(curCaravan, "item_clean_water", 2));
                _caravanInspector.AddChild(btnBuyWater);

                var btnBuyFood = AshfallUiHelpers.MakeButton("BARTER FOR CANNED FOOD (2 RATIONS)", () =>
                    TryCaravanPurchase(curCaravan, "item_canned_food", 1));
                _caravanInspector.AddChild(btnBuyFood);
            }
            else
            {
                _caravanInspector.AddChild(AshfallUiHelpers.MakeMetadata("Select a caravan from the route radar to trade."));
            }

            // Route Actions & Logs
            _routeLogContainer.AddChild(AshfallUiHelpers.MakeSectionHeader("ROUTE STATUS"));
            _routeLogContainer.AddChild(AshfallUiHelpers.MakeMetadata("Caravan trade routes advance automatically with the daily simulation clock."));

            _routeLogContainer.AddChild(AshfallUiHelpers.MakeSeparator());
            _routeLogContainer.AddChild(AshfallUiHelpers.MakeSubsectionHeader("CARAVAN RADIO CHATTER"));
            if (curCaravan != null)
            {
                string voiceLine = ResolveCaravanVoice(curCaravan);
                _routeLogContainer.AddChild(AshfallUiHelpers.MakeMono(
                    string.IsNullOrWhiteSpace(voiceLine)
                        ? "No caravan voice signal."
                        : voiceLine));
            }
            else
            {
                _routeLogContainer.AddChild(AshfallUiHelpers.MakeMono("No caravan selected."));
            }
        }

        private string ResolveCaravanVoice(CaravanEntry caravan)
        {
            CurrentTraderProfileId = string.Empty;
            if (_voiceResolver == null) return string.Empty;

            var result = _voiceResolver.ResolveGreeting(CreateCaravanVoiceContext(caravan));
            CurrentTraderProfileId = result.ProfileId;
            string displayName = VoiceDisplayName(result.ProfileId);
            return $"{displayName}: {result.Text}";
        }

        private void TryCaravanPurchase(CaravanEntry caravan, string itemId, int amount)
        {
            if (_host == null) return;

            _voiceEvent = string.Empty;
            int rations = 10;
            bool success = _host.Engine.TryBuyItem(caravan.caravanId, itemId, amount, ref rations);
            if (_voiceResolver != null)
            {
                var result = _voiceResolver.ResolveLine(
                    CreateCaravanVoiceContext(caravan),
                    success ? TradeVoiceLineFamily.Acceptance : TradeVoiceLineFamily.Rejection,
                    success ? "pleased" : "polite");
                _voiceEvent = $"{VoiceDisplayName(result.ProfileId)}: {result.Text}";
            }
            RefreshView();
        }

        private TradeVoiceContext CreateCaravanVoiceContext(CaravanEntry caravan) =>
            new TradeVoiceContext
            {
                CaravanId = caravan.caravanId,
                CaravanOriginRegion = caravan.originRegion,
                FactionId = caravan.factionId,
                StableContextKey = caravan.caravanId,
                Trust = 0f
            };

        private string VoiceDisplayName(string profileId) =>
            _voiceResolver != null &&
            _voiceResolver.Catalog.TryGetTrader(profileId, out var trader)
                ? trader.display_name
                : "The Merchant";

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                Visible = false;
                GetViewport().SetInputAsHandled();
            }
        }


    public void Unbind()
    {
        if (_host != null)
            {
                _host.StateChanged -= RefreshView;
            }
    }

    public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
