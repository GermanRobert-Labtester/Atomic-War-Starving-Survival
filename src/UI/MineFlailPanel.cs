// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Expeditions;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Mine-Clearing Flail Vehicle Panel (Plan 147).
    /// Programmatic UI implementation for MineClearingFlailHostSession.
    /// </summary>
    public partial class MineFlailPanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action<string, string>? OnActionRequested;

        private AshfallDashboardShell _shell = null!;
        private AshfallSidebar? _sidebar;
        private AshfallStatusRail? _statusRail;
        private AshfallDataGrid? _grid;
        private VBoxContainer _detailBox = null!;
        private Label _detailTitle = null!;

        private string _currentTab = "breach";
        private MineClearingFlailHostSession? _host;
        private string _selectedRouteId = string.Empty;
        private string _selectedSegmentId = string.Empty;
        private VBoxContainer? _actionsBox;
        private string _feedbackText = string.Empty;
        private bool _feedbackIsFailure;

        public bool IsBound => _host != null;
        public string LastFeedback { get; private set; } = string.Empty;

        public void ShowFeedback(string message, bool isFailure)
        {
            _feedbackText = message ?? string.Empty;
            _feedbackIsFailure = isFailure;
            LastFeedback = _feedbackText;
            RefreshView();
        }

        public void Bind(MineClearingFlailHostSession session)
        {
            if (_host != null)
                _host.StateChanged -= RefreshView;
            _host = session;
            if (_host != null)
                _host.StateChanged += RefreshView;
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null)
                _host.StateChanged -= RefreshView;
            _host = null;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("DEMINING FLAIL MODULE // VEHICLE BREACHING CONTROL", minWidth: 1100, minHeight: 720);
            AddChild(_shell);

            var sidebarItems = new[]
            {
                new AshfallSidebar.Item { Id = "breach", Label = "Breach Operations", Hint = "active mechanical mine clearance", IconPath = "" },
                new AshfallSidebar.Item { Id = "telemetry", Label = "Rotor Telemetry", Hint = "drum RPM & hydraulic pressure", IconPath = "" },
                new AshfallSidebar.Item { Id = "routes", Label = "Minefields Identified", Hint = "contaminated expedition corridors", IconPath = "" },
                new AshfallSidebar.Item { Id = "maintenance", Label = "Flail Maintenance", Hint = "strike chains & blast deflection", IconPath = "" }
            };
            _sidebar = _shell.SetSidebar(sidebarItems, "Section", "breach");
            _sidebar.OnSelected += tab =>
            {
                _currentTab = tab;
                RefreshView();
            };

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("flail_status", "ROTOR STATUS", "Ready", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("chain_links", "STRIKE CHAINS", "40 / 40", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("shield_integrity", "BLAST DEFLECTOR", "100%", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("cleared_distance", "TOTAL CLEARED", "0.0 km", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            var contentSplit = new HSplitContainer();
            contentSplit.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            contentSplit.SizeFlagsVertical = SizeFlags.ExpandFill;

            var cols = new[]
            {
                new AshfallDataGrid.Column { Header = "IDENTIFIER", MinWidth = 240 },
                new AshfallDataGrid.Column { Header = "ROUTE / SECTOR", MinWidth = 220 },
                new AshfallDataGrid.Column { Header = "CLEARANCE", MinWidth = 140 },
                new AshfallDataGrid.Column { Header = "STATUS", MinWidth = 180 }
            };
            _grid = new AshfallDataGrid(cols, showHeader: true, minWidth: 620, minHeight: 380);
            _grid.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _grid.SizeFlagsVertical = SizeFlags.ExpandFill;
            _grid.OnRowSelected += idx =>
            {
                if (idx >= 0 && idx < _grid.Rows.Count && _grid.Rows[idx].Cells.Count > 0)
                {
                    string key = _grid.Rows[idx].Cells[0].Text ?? string.Empty;
                    int sep = key.IndexOf(':');
                    if (sep > 0 && sep < key.Length - 1)
                    {
                        _selectedRouteId = key.Substring(0, sep);
                        _selectedSegmentId = key.Substring(sep + 1);
                    }
                    else
                    {
                        _selectedRouteId = key;
                        _selectedSegmentId = string.Empty;
                    }
                }
                RefreshDetailView();
            };
            contentSplit.AddChild(_grid);

            var detailPanel = new PanelContainer();
            detailPanel.CustomMinimumSize = new Vector2(380, 380);
            detailPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            detailPanel.SizeFlagsVertical = SizeFlags.ExpandFill;
            detailPanel.AddThemeStyleboxOverride("panel", AshfallUiHelpers.MakePanelFrameStyleBox());

            var detailMargin = new MarginContainer();
            detailMargin.AddThemeConstantOverride("margin_left", DesignTheme.SpacingSm);
            detailMargin.AddThemeConstantOverride("margin_top", DesignTheme.SpacingSm);
            detailMargin.AddThemeConstantOverride("margin_right", DesignTheme.SpacingSm);
            detailMargin.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingSm);
            detailPanel.AddChild(detailMargin);

            _detailBox = new VBoxContainer();
            _detailBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
            _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            detailMargin.AddChild(_detailBox);

            var detailHeader = new Label { Text = "BREACH TELEMETRY & STATUS" };
            detailHeader.AddThemeFontOverride("font", AshfallUiHelpers.FontBarlowSemiBold);
            detailHeader.AddThemeFontSizeOverride("font_size", 16);
            detailHeader.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _detailBox.AddChild(detailHeader);

            _detailTitle = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _detailTitle.AddThemeFontOverride("font", AshfallUiHelpers.FontBarlowRegular);
            _detailTitle.AddThemeFontSizeOverride("font_size", 14);
            _detailTitle.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            _detailBox.AddChild(_detailTitle);

            _actionsBox = new VBoxContainer();
            _actionsBox.AddThemeConstantOverride("separation", 8);
            _detailBox.AddChild(_actionsBox);

            contentSplit.AddChild(detailPanel);
            _shell.SetContent(contentSplit);

            _shell.AttachHeaderCloseButton("[X] CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        public void RefreshView()
        {
            if (_host == null)
            {
                _statusRail?.Set("flail_status", "Unbound", AshfallMetricCard.Criticality.Caution);
                return;
            }

            var state = _host.System.StateDto;
            string statusText = _host.System.IsOperational ? state.ActiveBreach?.Status.ToString() ?? "Ready" : "Damaged / Maintenance";
            var crit = _host.System.IsOperational ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Warn;
            _statusRail?.Set("flail_status", statusText, crit);

            _statusRail?.Set("chain_links", $"{state.ChainLinksRemaining} / 40", state.ChainLinksRemaining < 20 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail?.Set("shield_integrity", $"{state.BlastShieldIntegrity01:P0}", state.BlastShieldIntegrity01 < 0.4f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail?.Set("cleared_distance", $"{state.TotalClearedDistanceKm:F1} km", AshfallMetricCard.Criticality.Normal);

            var rows = new List<AshfallDataGrid.Row>();
            if (_currentTab == "breach" || _currentTab == "telemetry")
            {
                var breach = state.ActiveBreach;
                if (breach != null)
                {
                    var r = new AshfallDataGrid.Row();
                    r.Cells.Add(new AshfallDataGrid.Cell($"{breach.RouteId}:{breach.SegmentId}"));
                    r.Cells.Add(new AshfallDataGrid.Cell($"{breach.RouteId} / {breach.SegmentId}"));
                    r.Cells.Add(new AshfallDataGrid.Cell($"{breach.ProgressMeters:F0} / {breach.TargetMeters:F0} m"));
                    r.Cells.Add(new AshfallDataGrid.Cell(breach.Status.ToString(), AshfallDataGrid.CellState.Positive));
                    rows.Add(r);
                }
            }
            else if (_currentTab == "routes")
            {
                bool any = false;
                if (_host.Routes != null)
                {
                    foreach (var seg in _host.Routes.GetAllSegments())
                    {
                        if (seg == null || seg.MinefieldState == null || seg.MinefieldState.Density01 <= 0f)
                            continue;
                        any = true;
                        var r = new AshfallDataGrid.Row();
                        r.Cells.Add(new AshfallDataGrid.Cell($"{seg.RouteId}:{seg.SegmentId}"));
                        r.Cells.Add(new AshfallDataGrid.Cell($"{seg.RouteId} / {seg.SegmentId}"));
                        r.Cells.Add(new AshfallDataGrid.Cell($"Density {seg.MinefieldState.Density01:P0}"));
                        var cellState = seg.ClearanceState == "cleared"
                            ? AshfallDataGrid.CellState.Positive
                            : AshfallDataGrid.CellState.Warning;
                        r.Cells.Add(new AshfallDataGrid.Cell(seg.ClearanceState, cellState));
                        rows.Add(r);
                    }
                }
                if (!any)
                {
                    var r = new AshfallDataGrid.Row();
                    r.Cells.Add(new AshfallDataGrid.Cell("expedition_corridor_north:seg_mine_gap"));
                    r.Cells.Add(new AshfallDataGrid.Cell("expedition_corridor_north / seg_mine_gap"));
                    r.Cells.Add(new AshfallDataGrid.Cell("No live minefield yet"));
                    r.Cells.Add(new AshfallDataGrid.Cell("Awaiting bootstrap", AshfallDataGrid.CellState.Caution));
                    rows.Add(r);
                }
            }
            else if (_currentTab == "maintenance")
            {
                var r1 = new AshfallDataGrid.Row();
                r1.Cells.Add(new AshfallDataGrid.Cell("maint_chains"));
                r1.Cells.Add(new AshfallDataGrid.Cell("Strike Flail Chain Assemblies"));
                r1.Cells.Add(new AshfallDataGrid.Cell($"{state.ChainLinksRemaining} / 40 Intact"));
                r1.Cells.Add(new AshfallDataGrid.Cell(state.ChainLinksRemaining < 30 ? "Replace Links" : "Nominal", state.ChainLinksRemaining < 30 ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Normal));
                rows.Add(r1);

                var r2 = new AshfallDataGrid.Row();
                r2.Cells.Add(new AshfallDataGrid.Cell("maint_shield"));
                r2.Cells.Add(new AshfallDataGrid.Cell("Hardox Armored Deflector"));
                r2.Cells.Add(new AshfallDataGrid.Cell($"{state.BlastShieldIntegrity01:P0} Integrity"));
                r2.Cells.Add(new AshfallDataGrid.Cell(state.BlastShieldIntegrity01 < 0.5f ? "Weld Reinforcement" : "Nominal", state.BlastShieldIntegrity01 < 0.5f ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Normal));
                rows.Add(r2);
            }

            _grid?.SetRows(rows);
            RefreshDetailView();
        }

        private void RefreshDetailView()
        {
            if (_detailTitle == null || _host == null) return;
            var state = _host.System.StateDto;
            ClearActions();

            if (state.ActiveBreach != null)
            {
                var b = state.ActiveBreach;
                _detailTitle.Text = $"Active Breach: {b.RouteId} / {b.SegmentId}\n" +
                                    $"Distance Cleared: {b.ProgressMeters:F0} / {b.TargetMeters:F0} m\n" +
                                    $"Drum RPM: {state.DrumRpm:F0} RPM\n" +
                                    $"Hydraulic Pressure: {state.HydraulicPressureBar:F0} Bar\n" +
                                    $"Status: {b.Status}\n" +
                                    $"Last Event: {_host.LastEvent}";
            }
            else
            {
                string routeId = string.IsNullOrEmpty(_selectedRouteId) ? "expedition_corridor_north" : _selectedRouteId;
                string segmentId = string.IsNullOrEmpty(_selectedSegmentId) ? "seg_mine_gap" : _selectedSegmentId;
                _detailTitle.Text = $"Flail Status: {_host.System.State}\n" +
                                    $"Intact Chains: {state.ChainLinksRemaining} / 40\n" +
                                    $"Blast Shield: {state.BlastShieldIntegrity01:P0}\n" +
                                    $"Selected corridor: {routeId}:{segmentId}\n" +
                                    $"Last Event: {_host.LastEvent}";
                if (_currentTab == "routes" || _currentTab == "breach")
                    AddActionButton("START BREACH", "start_breach", $"{routeId}|{segmentId}");
            }

            if (_currentTab == "maintenance")
            {
                AddActionButton("REPLACE CHAINS", "maintain", "replace_chains");
                AddActionButton("SERVICE HYDRAULICS", "maintain", "service_hydraulics");
                AddActionButton("REPAIR BLAST SHIELD", "maintain", "repair_blast_shield");
            }

            if (!string.IsNullOrEmpty(_feedbackText) && _actionsBox != null)
            {
                _actionsBox.AddChild(_feedbackIsFailure
                    ? AshfallUiHelpers.MakeWarning(_feedbackText)
                    : AshfallUiHelpers.MakeSuccess(_feedbackText));
            }
        }

        private void ClearActions()
        {
            if (_actionsBox == null) return;
            while (_actionsBox.GetChildCount() > 0)
            {
                var child = _actionsBox.GetChild(0);
                _actionsBox.RemoveChild(child);
                child.QueueFree();
            }
        }

        private void AddActionButton(string label, string action, string param)
        {
            if (_actionsBox == null) return;
            var btn = AshfallUiHelpers.MakeButton(label, () => OnActionRequested?.Invoke(action, param));
            _actionsBox.AddChild(btn);
        }
    }
}
