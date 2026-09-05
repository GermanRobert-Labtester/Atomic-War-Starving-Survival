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
    /// ASHFALL — Rail Grinding & Corridor Rehabilitation Panel (Plan 149).
    /// Programmatic UI implementation for RailGrindingHostSession.
    /// </summary>
    public partial class RailGrindingPanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action<string, string>? OnActionRequested;

        private AshfallDashboardShell _shell = null!;
        private AshfallSidebar? _sidebar;
        private AshfallStatusRail? _statusRail;
        private AshfallDataGrid? _grid;
        private VBoxContainer _detailBox = null!;
        private Label _detailTitle = null!;

        private string _currentTab = "operations";
        private RailGrindingHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(RailGrindingHostSession session)
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

            _shell = new AshfallDashboardShell("RAIL GRINDING & REPROFILING // STRATEGIC RAIL CORRIDOR CONTROL", minWidth: 1100, minHeight: 720);
            AddChild(_shell);

            var sidebarItems = new[]
            {
                new AshfallSidebar.Item { Id = "operations", Label = "Grinding Operations", Hint = "active track reprofiling passes", IconPath = "" },
                new AshfallSidebar.Item { Id = "corridors", Label = "Rail Corridors", Hint = "roughness & maximum safe speeds", IconPath = "" },
                new AshfallSidebar.Item { Id = "stones", Label = "Abrasive Disc Wear", Hint = "corundum stones & water misting", IconPath = "" },
                new AshfallSidebar.Item { Id = "maintenance", Label = "Draisine Service", Hint = "hydraulic downforce & alignment", IconPath = "" }
            };
            _sidebar = _shell.SetSidebar(sidebarItems, "Section", "operations");
            _sidebar.OnSelected += tab =>
            {
                _currentTab = tab;
                RefreshView();
            };

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("grinder_status", "GRINDER STATUS", "Ready", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("stone_diameter", "ABRASIVE WHEEL", "250 mm", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("downforce", "DOWNFORCE", "6.0 Bar", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("speed_cap", "SPEED LIMIT", "55 km/h", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            var contentSplit = new HSplitContainer();
            contentSplit.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            contentSplit.SizeFlagsVertical = SizeFlags.ExpandFill;

            var cols = new[]
            {
                new AshfallDataGrid.Column { Header = "IDENTIFIER", MinWidth = 240 },
                new AshfallDataGrid.Column { Header = "TRACK SECTOR / ROUTE", MinWidth = 220 },
                new AshfallDataGrid.Column { Header = "ROUGHNESS / SPEED", MinWidth = 140 },
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

            var detailHeader = new Label { Text = "GRINDING TELEMETRY & PROFILE CONTROL" };
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
                _statusRail?.Set("grinder_status", "Unbound", AshfallMetricCard.Criticality.Caution);
                return;
            }

            var state = _host.System.StateDto;
            string statusText = _host.System.IsOperational ? state.ActiveJob?.Status.ToString() ?? "Ready" : "Damaged / Maintenance";
            var crit = _host.System.IsOperational ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Warn;
            _statusRail?.Set("grinder_status", statusText, crit);

            _statusRail?.Set("stone_diameter", $"{state.StoneDiameterMm:F0} mm", state.StoneDiameterMm < 130f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail?.Set("downforce", $"{state.DownforceBar:F1} Bar", AshfallMetricCard.Criticality.Normal);
            _statusRail?.Set("speed_cap", "55 km/h", AshfallMetricCard.Criticality.Normal);

            var rows = new List<AshfallDataGrid.Row>();
            if (_currentTab == "operations")
            {
                var job = state.ActiveJob;
                if (job != null)
                {
                    var r = new AshfallDataGrid.Row();
                    r.Cells.Add(new AshfallDataGrid.Cell("job_active"));
                    r.Cells.Add(new AshfallDataGrid.Cell($"{job.RouteId} / {job.SegmentId}"));
                    r.Cells.Add(new AshfallDataGrid.Cell($"{job.ProgressKm:F1} / {job.TargetKm:F1} km"));
                    r.Cells.Add(new AshfallDataGrid.Cell(job.Status.ToString(), AshfallDataGrid.CellState.Positive));
                    rows.Add(r);
                }
            }
            else if (_currentTab == "corridors")
            {
                var r1 = new AshfallDataGrid.Row();
                r1.Cells.Add(new AshfallDataGrid.Cell("rail_trunk_iron_reach"));
                r1.Cells.Add(new AshfallDataGrid.Cell("Iron Reach Main Line"));
                r1.Cells.Add(new AshfallDataGrid.Cell("Roughness: 0.82"));
                r1.Cells.Add(new AshfallDataGrid.Cell("Needs Reprofiling", AshfallDataGrid.CellState.Warning));
                rows.Add(r1);

                var r2 = new AshfallDataGrid.Row();
                r2.Cells.Add(new AshfallDataGrid.Cell("rail_spur_quarry_junction"));
                r2.Cells.Add(new AshfallDataGrid.Cell("Quarry Spur 4"));
                r2.Cells.Add(new AshfallDataGrid.Cell("Roughness: 0.28"));
                r2.Cells.Add(new AshfallDataGrid.Cell("Clear & Reprofiled", AshfallDataGrid.CellState.Positive));
                rows.Add(r2);
            }
            else if (_currentTab == "stones")
            {
                var r = new AshfallDataGrid.Row();
                r.Cells.Add(new AshfallDataGrid.Cell("corundum_grinding_stone_set"));
                r.Cells.Add(new AshfallDataGrid.Cell("Vitrified Corundum Disc Set"));
                r.Cells.Add(new AshfallDataGrid.Cell($"{state.StoneDiameterMm:F1} / 250 mm"));
                r.Cells.Add(new AshfallDataGrid.Cell(state.StoneDiameterMm < 130f ? "Replace Due" : "Nominal", state.StoneDiameterMm < 130f ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Normal));
                rows.Add(r);
            }
            else if (_currentTab == "maintenance")
            {
                var r1 = new AshfallDataGrid.Row();
                r1.Cells.Add(new AshfallDataGrid.Cell("maint_alignment"));
                r1.Cells.Add(new AshfallDataGrid.Cell("Wheelhead Angular Alignment"));
                r1.Cells.Add(new AshfallDataGrid.Cell("±0.5° Tolerance"));
                r1.Cells.Add(new AshfallDataGrid.Cell("Calibrated", AshfallDataGrid.CellState.Positive));
                rows.Add(r1);

                var r2 = new AshfallDataGrid.Row();
                r2.Cells.Add(new AshfallDataGrid.Cell("maint_water_misting"));
                r2.Cells.Add(new AshfallDataGrid.Cell("Spark Suppression Misting Nozzles"));
                r2.Cells.Add(new AshfallDataGrid.Cell("Active Water Feed"));
                r2.Cells.Add(new AshfallDataGrid.Cell("Nominal", AshfallDataGrid.CellState.Positive));
                rows.Add(r2);
            }

            _grid?.SetRows(rows);
            RefreshDetailView();
        }

        private void RefreshDetailView()
        {
            if (_detailTitle == null || _host == null) return;
            var state = _host.System.StateDto;

            if (state.ActiveJob != null)
            {
                var job = state.ActiveJob;
                _detailTitle.Text = $"Active Grinding Job: {job.RouteId} / {job.SegmentId}\n" +
                                    $"Pass Progress: {job.ProgressKm:F1} / {job.TargetKm:F1} km\n" +
                                    $"Passes Done: {job.PassesCompleted}\n" +
                                    $"Stone Diameter: {state.StoneDiameterMm:F1} mm\n" +
                                    $"Status: {job.Status}\n" +
                                    $"Last Event: {_host.LastEvent}";
            }
            else
            {
                _detailTitle.Text = $"Rail Grinder Status: {_host.System.State}\n" +
                                    $"Stone Diameter: {state.StoneDiameterMm:F1} mm\n" +
                                    $"Downforce: {state.DownforceBar:F1} Bar\n" +
                                    $"Motor RPM: {state.MotorRpm:F0} RPM\n" +
                                    $"Last Event: {_host.LastEvent}";
            }
        }
    }
}
