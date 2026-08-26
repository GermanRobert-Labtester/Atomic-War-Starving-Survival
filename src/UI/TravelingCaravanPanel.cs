using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
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
    public partial class TravelingCaravanPanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _caravanList = null!;
        private VBoxContainer _caravanInspector = null!;
        private VBoxContainer _routeLogContainer = null!;
        private Label _eventLogLabel = null!;

        private TravelingCaravanHostSession? _host;
        private string? _selectedCaravanId;

        public bool IsBound => _host != null;

        public void Bind(TravelingCaravanHostSession session)
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
            }
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
            }
            RefreshView();
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
            _statusRail.Set("routes", "SECURE", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("status", totalCaravans > 0 ? "TRACKING" : "IDLE", AshfallMetricCard.Criticality.Normal);

            if (!string.IsNullOrEmpty(_host.LastEvent))
            {
                _eventLogLabel.Text = _host.LastEvent;
            }

            // Populate Caravan List
            if (caravans.Count == 0)
            {
                _caravanList.AddChild(AshfallUiHelpers.MakeMetadata("No trade caravans currently en route."));
                var btnSpawn = AshfallUiHelpers.MakeButton("SUMMON WANDERING MENDERS CARAVAN", () =>
                {
                    _host.SpawnDemoCaravan("loc_holdfast_gate");
                    RefreshView();
                });
                _caravanList.AddChild(btnSpawn);
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
                    cardVbox.AddThemeConstantOverride("separation", 3);
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

                _caravanInspector.AddChild(AshfallUiHelpers.MakeSeparator());
                _caravanInspector.AddChild(AshfallUiHelpers.MakeSubsectionHeader("BARTER TRANSACTIONS"));

                var btnBuyMed = AshfallUiHelpers.MakeButton("BARTER FOR ANTIBIOTICS (5 RATIONS)", () =>
                {
                    int rations = 10;
                    _host.Engine.TryBuyItem(curCaravan.caravanId, "item_antibiotics", 1, ref rations);
                    RefreshView();
                });
                _caravanInspector.AddChild(btnBuyMed);

                var btnBuyWater = AshfallUiHelpers.MakeButton("BARTER FOR CLEAN WATER (1 RATION)", () =>
                {
                    int rations = 10;
                    _host.Engine.TryBuyItem(curCaravan.caravanId, "item_clean_water", 2, ref rations);
                    RefreshView();
                });
                _caravanInspector.AddChild(btnBuyWater);

                var btnBuyFood = AshfallUiHelpers.MakeButton("BARTER FOR CANNED FOOD (2 RATIONS)", () =>
                {
                    int rations = 10;
                    _host.Engine.TryBuyItem(curCaravan.caravanId, "item_canned_food", 1, ref rations);
                    RefreshView();
                });
                _caravanInspector.AddChild(btnBuyFood);
            }
            else
            {
                _caravanInspector.AddChild(AshfallUiHelpers.MakeMetadata("Select a caravan from the route radar to trade."));
            }

            // Route Actions & Logs
            _routeLogContainer.AddChild(AshfallUiHelpers.MakeSectionHeader("ROUTE MANAGEMENT"));
            var btnTickRoute = AshfallUiHelpers.MakeButton("STEP ROUTE (DAILY TICK)", () =>
            {
                _host.TickDemo();
                RefreshView();
            });
            _routeLogContainer.AddChild(btnTickRoute);

            _routeLogContainer.AddChild(AshfallUiHelpers.MakeSeparator());
            _routeLogContainer.AddChild(AshfallUiHelpers.MakeSubsectionHeader("CARAVAN RADIO CHATTER"));
            _routeLogContainer.AddChild(AshfallUiHelpers.MakeMono("Menders Radio: 'Approaching Holdfast perimeter with fresh salvage and medical supplies.'"));
            _routeLogContainer.AddChild(AshfallUiHelpers.MakeMono("Salt Merchant: 'Transit cleared through Sector 4. No raider sightings.'"));
        }

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

        public override void _ExitTree()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
            }
            base._ExitTree();
        }
    }
}
