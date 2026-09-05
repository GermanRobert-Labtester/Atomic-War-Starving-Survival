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

        public bool IsBound => _host != null;

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
            _grid.OnRowSelected += idx => RefreshDetailView();
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
            _detailBox.AddThemeConstantOverride("separation", 10);
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
                    r.Cells.Add(new AshfallDataGrid.Cell("breach_active"));
                    r.Cells.Add(new AshfallDataGrid.Cell($"{breach.RouteId} / {breach.SegmentId}"));
                    r.Cells.Add(new AshfallDataGrid.Cell($"{breach.ProgressMeters:F0} / {breach.TargetMeters:F0} m"));
                    r.Cells.Add(new AshfallDataGrid.Cell(breach.Status.ToString(), AshfallDataGrid.CellState.Positive));
                    rows.Add(r);
                }
            }
            else if (_currentTab == "routes")
            {
                var r1 = new AshfallDataGrid.Row();
                r1.Cells.Add(new AshfallDataGrid.Cell("route_ashfall_pass"));
                r1.Cells.Add(new AshfallDataGrid.Cell("Pass Road Alpha"));
                r1.Cells.Add(new AshfallDataGrid.Cell("Active Minefield"));
                r1.Cells.Add(new AshfallDataGrid.Cell("Breach Feasible", AshfallDataGrid.CellState.Caution));
                rows.Add(r1);

                var r2 = new AshfallDataGrid.Row();
                r2.Cells.Add(new AshfallDataGrid.Cell("route_crater_choke"));
                r2.Cells.Add(new AshfallDataGrid.Cell("Crater Rim Defile"));
                r2.Cells.Add(new AshfallDataGrid.Cell("Dense AP/AT Cluster"));
                r2.Cells.Add(new AshfallDataGrid.Cell("Hazardous", AshfallDataGrid.CellState.Warning));
                rows.Add(r2);
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
                _detailTitle.Text = $"Flail Status: {_host.System.State}\n" +
                                    $"Intact Chains: {state.ChainLinksRemaining} / 40\n" +
                                    $"Blast Shield: {state.BlastShieldIntegrity01:P0}\n" +
                                    $"Drum RPM: {state.DrumRpm:F0} RPM\n" +
                                    $"Total Corridor Cleared: {state.TotalClearedDistanceKm:F2} km\n" +
                                    $"Last Event: {_host.LastEvent}";
            }
        }
    }
}
