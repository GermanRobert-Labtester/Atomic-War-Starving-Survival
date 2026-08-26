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
    /// ASHFALL — Sublevel Sump & Flood Control Panel.
    /// Manages bunker drainage nodes, sump pump installation, power distribution,
    /// sandbag/float valve mitigations, and emergency drain cycles.
    /// </summary>
    public partial class SumpFloodingPanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _nodeList = null!;
        private VBoxContainer _controlsContainer = null!;
        private VBoxContainer _incidentLogContainer = null!;
        private Label _eventLogLabel = null!;

        private SumpFloodingHostSession? _host;
        private string? _selectedNodeId;

        public bool IsBound => _host != null;

        public void Bind(SumpFloodingHostSession session)
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

            _shell = new AshfallDashboardShell("SYS: SUBLEVEL SUMP & FLOOD CONTROL // MATRIX", minWidth: 1040, minHeight: 680);
            center.AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("groundwater", "GROUNDWATER", "0 cm", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("nodes", "DRAIN NODES", "0", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("pumps", "ACTIVE PUMPS", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("flooded", "FLOOD STATUS", "CLEAR", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("status", "POWER GRID", "ONLINE", AshfallMetricCard.Criticality.Normal, minWidth: 120);

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

            // Column 1: Nodes Matrix
            var leftPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            leftPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftPanel.SizeFlagsStretchRatio = 0.95f;
            var leftMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            leftPanel.AddChild(leftMargin);
            var leftVbox = new VBoxContainer();
            leftVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            leftMargin.AddChild(leftVbox);
            leftVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("SUBLEVEL SUMP NODES"));
            var leftScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _nodeList = new VBoxContainer();
            _nodeList.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _nodeList.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftScroll.AddChild(_nodeList);
            leftVbox.AddChild(leftScroll);
            gridRow.AddChild(leftPanel);

            // Column 2: Selected Node Inspector & Pump Controls
            var centerPanel = AshfallUiHelpers.MakePanel(minWidth: 380);
            centerPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerPanel.SizeFlagsStretchRatio = 1.2f;
            var centerMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            centerPanel.AddChild(centerMargin);
            var centerVbox = new VBoxContainer();
            centerVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            centerMargin.AddChild(centerVbox);
            centerVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("NODE TELEMETRY & PUMP CONTROLS"));
            var centerScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _controlsContainer = new VBoxContainer();
            _controlsContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _controlsContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerScroll.AddChild(_controlsContainer);
            centerVbox.AddChild(centerScroll);
            gridRow.AddChild(centerPanel);

            // Column 3: Incident Log & Hydrology Stats
            var rightPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            rightPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightPanel.SizeFlagsStretchRatio = 0.95f;
            var rightMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            rightPanel.AddChild(rightMargin);
            var rightVbox = new VBoxContainer();
            rightVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            rightMargin.AddChild(rightVbox);
            rightVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("FLOOD INCIDENT LOG"));
            var rightScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _incidentLogContainer = new VBoxContainer();
            _incidentLogContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _incidentLogContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightScroll.AddChild(_incidentLogContainer);
            rightVbox.AddChild(rightScroll);

            rightVbox.AddChild(AshfallUiHelpers.MakeSeparator());
            _eventLogLabel = AshfallUiHelpers.MakeMetadata("No recent pump events.");
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

            AshfallUiHelpers.EmptyChildren(_nodeList);
            AshfallUiHelpers.EmptyChildren(_controlsContainer);
            AshfallUiHelpers.EmptyChildren(_incidentLogContainer);

            var s = _host.System.State;
            int totalNodes = s.nodes.Count;
            int activePumps = s.nodes.Count(n => n.hasSumpPump && n.pumpPowered);
            int floodedNodes = s.nodes.Count(n => n.isFlooded);

            _statusRail.Set("groundwater", $"{s.globalGroundwaterLevel:F0} cm", s.globalGroundwaterLevel > 150f ? AshfallMetricCard.Criticality.Critical : s.globalGroundwaterLevel > 80f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("nodes", totalNodes.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("pumps", activePumps.ToString(), activePumps > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Warn);
            _statusRail.Set("flooded", floodedNodes > 0 ? $"{floodedNodes} FLOODED" : "NOMINAL", floodedNodes > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("status", activePumps > 0 ? "DRAINING" : "STANDBY", AshfallMetricCard.Criticality.Normal);

            if (!string.IsNullOrEmpty(_host.LastEvent))
            {
                _eventLogLabel.Text = _host.LastEvent;
            }

            if (s.nodes.Count == 0)
            {
                _nodeList.AddChild(AshfallUiHelpers.MakeMetadata("No sump nodes configured. Click below to register lower sump."));
                var btnInit = AshfallUiHelpers.MakeButton("INITIALIZE LOWER SUMP", () =>
                {
                    _host.AddNode("sump_sublevel_01", "Sublevel 01 Bilge", 180f);
                    _host.AddNode("sump_sublevel_02", "Reactor Basement Sump", 250f);
                    RefreshView();
                });
                _nodeList.AddChild(btnInit);
            }
            else
            {
                if (_selectedNodeId == null || !s.nodes.Exists(n => n.nodeId == _selectedNodeId))
                {
                    _selectedNodeId = s.nodes[0].nodeId;
                }

                foreach (var node in s.nodes)
                {
                    var nodeCard = AshfallUiHelpers.MakePanel();
                    var cardMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingXs);
                    nodeCard.AddChild(cardMargin);
                    var cardVbox = new VBoxContainer();
                    cardVbox.AddThemeConstantOverride("separation", 3);
                    cardMargin.AddChild(cardVbox);

                    var headerRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                    headerRow.AddChild(AshfallUiHelpers.MakeBadgeIcon(node.isFlooded ? "badge_corneal_burn" : "badge_exhaustion", 18));
                    var nameLbl = AshfallUiHelpers.MakeBody(node.displayName);
                    nameLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    headerRow.AddChild(nameLbl);
                    cardVbox.AddChild(headerRow);

                    float ratio = node.maxWaterLevelCm > 0f ? node.waterLevelCm / node.maxWaterLevelCm : 0f;
                    var waterRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                    var waterLbl = AshfallUiHelpers.MakeMono($"WATER: {node.waterLevelCm:F0}/{node.maxWaterLevelCm:F0}cm {BuildGauge(ratio)}");
                    waterLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    waterLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(node.isFlooded ? DesignTheme.Critical : ratio > 0.5f ? DesignTheme.Warm : DesignTheme.Pale));
                    waterRow.AddChild(waterLbl);
                    cardVbox.AddChild(waterRow);

                    var pumpStatusLbl = AshfallUiHelpers.MakeSmall(node.hasSumpPump ? (node.pumpPowered ? "PUMP: RUNNING" : "PUMP: POWER OFF") : "NO PUMP INSTALLED");
                    pumpStatusLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(node.hasSumpPump && node.pumpPowered ? DesignTheme.Lethe : DesignTheme.Dim));
                    cardVbox.AddChild(pumpStatusLbl);

                    var selectBtn = AshfallUiHelpers.MakeButton($"SELECT // {node.nodeId}", () =>
                    {
                        _selectedNodeId = node.nodeId;
                        RefreshView();
                    });
                    selectBtn.CustomMinimumSize = new Vector2(0, 24);
                    cardVbox.AddChild(selectBtn);

                    _nodeList.AddChild(nodeCard);
                }
            }

            // Selected Node Controls
            var activeNode = s.nodes.FirstOrDefault(n => n.nodeId == _selectedNodeId);
            if (activeNode != null)
            {
                _controlsContainer.AddChild(AshfallUiHelpers.MakeSectionHeader($"NODE: {activeNode.displayName.ToUpperInvariant()}"));
                _controlsContainer.AddChild(AshfallUiHelpers.MakeDataRow("Node ID", activeNode.nodeId, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
                _controlsContainer.AddChild(AshfallUiHelpers.MakeDataRow("Water Depth", $"{activeNode.waterLevelCm:F1} cm / {activeNode.maxWaterLevelCm:F0} cm", AshfallUiHelpers.ToColor(activeNode.isFlooded ? DesignTheme.Critical : DesignTheme.Warm)));
                _controlsContainer.AddChild(AshfallUiHelpers.MakeDataRow("Flooded State", activeNode.isFlooded ? "CRITICAL FLOOD - EQUIPMENT DISABLED" : "DRY / CONTROLLED", AshfallUiHelpers.ToColor(activeNode.isFlooded ? DesignTheme.Critical : DesignTheme.Lethe)));
                _controlsContainer.AddChild(AshfallUiHelpers.MakeDataRow("Sump Pump", activeNode.hasSumpPump ? $"Installed (Condition {activeNode.pumpCondition:F0}%)" : "None", AshfallUiHelpers.ToColor(activeNode.hasSumpPump ? DesignTheme.Pale : DesignTheme.Dim)));
                _controlsContainer.AddChild(AshfallUiHelpers.MakeDataRow("Float Valve", activeNode.hasFloatValve ? "Active" : "Not Installed", AshfallUiHelpers.ToColor(activeNode.hasFloatValve ? DesignTheme.Lethe : DesignTheme.Dim)));
                _controlsContainer.AddChild(AshfallUiHelpers.MakeDataRow("Sandbag Barriers", activeNode.hasSandbagMitigation ? "Fortified" : "None", AshfallUiHelpers.ToColor(activeNode.hasSandbagMitigation ? DesignTheme.Warm : DesignTheme.Dim)));

                _controlsContainer.AddChild(AshfallUiHelpers.MakeSeparator());
                _controlsContainer.AddChild(AshfallUiHelpers.MakeSubsectionHeader("OPERATIONAL CONTROLS"));

                if (!activeNode.hasSumpPump)
                {
                    var btnInstall = AshfallUiHelpers.MakeButton("INSTALL INDUSTRIAL SUMP PUMP", () =>
                    {
                        _host.InstallPump(activeNode.nodeId);
                        RefreshView();
                    });
                    _controlsContainer.AddChild(btnInstall);
                }
                else
                {
                    var btnPower = AshfallUiHelpers.MakeButton(activeNode.pumpPowered ? "DISENGAGE PUMP POWER" : "ENGAGE PUMP POWER", () =>
                    {
                        _host.SetNodePower(activeNode.nodeId, !activeNode.pumpPowered);
                        RefreshView();
                    });
                    _controlsContainer.AddChild(btnPower);
                }

                if (!activeNode.hasSandbagMitigation)
                {
                    var btnSandbags = AshfallUiHelpers.MakeButton("DEPLOY SANDBAG BARRIER", () =>
                    {
                        _host.AddMitigation(activeNode.nodeId, "sandbag");
                        RefreshView();
                    });
                    _controlsContainer.AddChild(btnSandbags);
                }

                if (!activeNode.hasFloatValve)
                {
                    var btnValve = AshfallUiHelpers.MakeButton("INSTALL AUTOMATIC FLOAT VALVE", () =>
                    {
                        _host.AddMitigation(activeNode.nodeId, "float_valve");
                        RefreshView();
                    });
                    _controlsContainer.AddChild(btnValve);
                }

                var btnDrain = AshfallUiHelpers.MakeButton("MANUAL BILGE EMERGENCY DRAIN", () =>
                {
                    _host.DrainNode(activeNode.nodeId);
                    RefreshView();
                });
                _controlsContainer.AddChild(btnDrain);
            }
            else
            {
                _controlsContainer.AddChild(AshfallUiHelpers.MakeMetadata("Select a node from the matrix to view telemetry."));
            }

            // Populate Incident Log
            if (s.incidentLog.Count == 0)
            {
                _incidentLogContainer.AddChild(AshfallUiHelpers.MakeMetadata("No flood incidents recorded."));
            }
            else
            {
                foreach (var inc in s.incidentLog.TakeLast(8))
                {
                    _incidentLogContainer.AddChild(AshfallUiHelpers.MakeMono($"Day {inc.day} [{inc.kind}]: {inc.description}"));
                }
            }
        }

        private static string BuildGauge(float ratio)
        {
            int totalBars = 8;
            int filled = Math.Clamp((int)Math.Round(ratio * totalBars), 0, totalBars);
            return "[" + new string('|', filled) + new string('-', totalBars - filled) + "]";
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
