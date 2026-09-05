// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Shelter;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — EB-PVD Thermal Barrier Coating Panel (Plan 146).
    /// Programmatic UI implementation for EbPvdCoatingHostSession.
    /// </summary>
    public partial class EbPvdCoatingPanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action<string, string>? OnActionRequested;

        private AshfallDashboardShell _shell = null!;
        private AshfallSidebar? _sidebar;
        private AshfallStatusRail? _statusRail;
        private AshfallDataGrid? _grid;
        private VBoxContainer _detailBox = null!;
        private Label _detailTitle = null!;

        private string _currentTab = "overview";
        private EbPvdCoatingHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(EbPvdCoatingHostSession session)
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

            _shell = new AshfallDashboardShell("EB-PVD COATER // THERMAL BARRIER DEPOSITION", minWidth: 1100, minHeight: 720);
            AddChild(_shell);

            var sidebarItems = new[]
            {
                new AshfallSidebar.Item { Id = "overview", Label = "Chamber Overview", Hint = "beam & process telemetry", IconPath = "" },
                new AshfallSidebar.Item { Id = "coatings", Label = "Coating Recipes", Hint = "zirconia, alumina, pyrochlore", IconPath = "" },
                new AshfallSidebar.Item { Id = "completed", Label = "Coated Inventory", Hint = "finished turbine & engine parts", IconPath = "" },
                new AshfallSidebar.Item { Id = "maintenance", Label = "Chamber Service", Hint = "filament & vacuum pump seals", IconPath = "" }
            };
            _sidebar = _shell.SetSidebar(sidebarItems, "Section", "overview");
            _sidebar.OnSelected += tab =>
            {
                _currentTab = tab;
                RefreshView();
            };

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("machine_status", "CHAMBER STATUS", "Operational", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("vacuum_level", "CHAMBER VACUUM", "1.0e-5 mbar", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("beam_power", "BEAM POWER", "10.0 kW", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("completed_count", "PARTS COATED", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            var contentSplit = new HSplitContainer();
            contentSplit.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            contentSplit.SizeFlagsVertical = SizeFlags.ExpandFill;

            var cols = new[]
            {
                new AshfallDataGrid.Column { Header = "IDENTIFIER", MinWidth = 240 },
                new AshfallDataGrid.Column { Header = "CLASSIFICATION", MinWidth = 220 },
                new AshfallDataGrid.Column { Header = "THICKNESS", MinWidth = 140 },
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

            var detailHeader = new Label { Text = "PROCESS TELEMETRY & CONTROL" };
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
                _statusRail?.Set("machine_status", "Unbound", AshfallMetricCard.Criticality.Caution);
                return;
            }

            var state = _host.System.StateDto;
            string statusText = _host.System.IsOperational ? state.ActiveJob?.Status.ToString() ?? "Idle" : "Maintenance Required";
            var crit = _host.System.IsOperational ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Warn;
            _statusRail?.Set("machine_status", statusText, crit);

            float powerKw = state.ActiveJob?.BeamPowerKw ?? 0f;
            _statusRail?.Set("beam_power", $"{powerKw:F1} kW", AshfallMetricCard.Criticality.Normal);
            _statusRail?.Set("completed_count", state.CompletedRecords.Count.ToString(), AshfallMetricCard.Criticality.Normal);

            var rows = new List<AshfallDataGrid.Row>();
            if (_currentTab == "overview" || _currentTab == "coatings")
            {
                var job = state.ActiveJob;
                if (job != null)
                {
                    var r = new AshfallDataGrid.Row();
                    r.Cells.Add(new AshfallDataGrid.Cell(job.JobId));
                    r.Cells.Add(new AshfallDataGrid.Cell(job.CoatingId));
                    r.Cells.Add(new AshfallDataGrid.Cell($"{job.CoatingThicknessUm:F1} µm"));
                    r.Cells.Add(new AshfallDataGrid.Cell(job.Status.ToString(), AshfallDataGrid.CellState.Positive));
                    rows.Add(r);
                }
                else
                {
                    var r1 = new AshfallDataGrid.Row();
                    r1.Cells.Add(new AshfallDataGrid.Cell("ebpvd_tbc_yttria_stabilized_zirconia"));
                    r1.Cells.Add(new AshfallDataGrid.Cell("7YSZ Thermal Barrier"));
                    r1.Cells.Add(new AshfallDataGrid.Cell("125 µm"));
                    r1.Cells.Add(new AshfallDataGrid.Cell("Recipe Available", AshfallDataGrid.CellState.Normal));
                    rows.Add(r1);

                    var r2 = new AshfallDataGrid.Row();
                    r2.Cells.Add(new AshfallDataGrid.Cell("ebpvd_tbc_gadolinium_zirconate"));
                    r2.Cells.Add(new AshfallDataGrid.Cell("Gd2Zr2O7 Pyrochlore"));
                    r2.Cells.Add(new AshfallDataGrid.Cell("150 µm"));
                    r2.Cells.Add(new AshfallDataGrid.Cell("Recipe Available", AshfallDataGrid.CellState.Normal));
                    rows.Add(r2);

                    var r3 = new AshfallDataGrid.Row();
                    r3.Cells.Add(new AshfallDataGrid.Cell("ebpvd_tbc_alumina_barrier"));
                    r3.Cells.Add(new AshfallDataGrid.Cell("Alumina Barrier"));
                    r3.Cells.Add(new AshfallDataGrid.Cell("50 µm"));
                    r3.Cells.Add(new AshfallDataGrid.Cell("Recipe Available", AshfallDataGrid.CellState.Normal));
                    rows.Add(r3);
                }
            }
            else if (_currentTab == "completed")
            {
                foreach (var rec in state.CompletedRecords)
                {
                    var r = new AshfallDataGrid.Row();
                    r.Cells.Add(new AshfallDataGrid.Cell(rec.InstanceId));
                    r.Cells.Add(new AshfallDataGrid.Cell(rec.SubstrateTag));
                    r.Cells.Add(new AshfallDataGrid.Cell($"{rec.ThicknessUm:F0} µm"));
                    r.Cells.Add(new AshfallDataGrid.Cell($"Ready (+{rec.MaxTempBonusC:F0}°C)", AshfallDataGrid.CellState.Positive));
                    rows.Add(r);
                }
            }
            else if (_currentTab == "maintenance")
            {
                var r1 = new AshfallDataGrid.Row();
                r1.Cells.Add(new AshfallDataGrid.Cell("maint_filament"));
                r1.Cells.Add(new AshfallDataGrid.Cell("Electron Gun Filament"));
                r1.Cells.Add(new AshfallDataGrid.Cell($"{state.FilamentHours:F1} hrs"));
                r1.Cells.Add(new AshfallDataGrid.Cell(state.FilamentHours > 80f ? "Replace Due" : "Nominal", state.FilamentHours > 80f ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Normal));
                rows.Add(r1);

                var r2 = new AshfallDataGrid.Row();
                r2.Cells.Add(new AshfallDataGrid.Cell("maint_pump"));
                r2.Cells.Add(new AshfallDataGrid.Cell("Diffusion Vacuum Pump"));
                r2.Cells.Add(new AshfallDataGrid.Cell($"{state.VacuumPumpHours:F1} hrs"));
                r2.Cells.Add(new AshfallDataGrid.Cell(state.VacuumPumpHours > 100f ? "Service Due" : "Nominal", state.VacuumPumpHours > 100f ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Normal));
                rows.Add(r2);
            }

            _grid?.SetRows(rows);
            RefreshDetailView();
        }

        private void RefreshDetailView()
        {
            if (_detailTitle == null || _host == null) return;
            var state = _host.System.StateDto;

            if (_currentTab == "maintenance")
            {
                _detailTitle.Text = $"Chamber Condition: {state.MachineCondition01:P0}\n" +
                                    $"Filament Run: {state.FilamentHours:F1} / 100 hrs\n" +
                                    $"Vacuum Pump Run: {state.VacuumPumpHours:F1} / 150 hrs\n" +
                                    $"Shielding: {state.ChamberShieldingCondition01:P0}\n" +
                                    $"Last Event: {_host.LastEvent}";
            }
            else if (state.ActiveJob != null)
            {
                var job = state.ActiveJob;
                _detailTitle.Text = $"Active Job: {job.JobId}\n" +
                                    $"Coating: {job.CoatingId}\n" +
                                    $"Substrate: {job.SubstrateTag}\n" +
                                    $"Deposition Progress: {job.ProgressHours:F1} / {job.RequiredHours:F1} hrs\n" +
                                    $"Thickness: {job.CoatingThicknessUm:F1} µm\n" +
                                    $"Beam Power: {job.BeamPowerKw:F1} kW ({job.BeamVoltageKv} kV x {job.BeamCurrentA} A)\n" +
                                    $"Status: {job.Status}\n" +
                                    $"Last Event: {_host.LastEvent}";
            }
            else
            {
                _detailTitle.Text = $"Chamber Idle — Ready for deposition.\n" +
                                    $"Select a recipe to commence high-vacuum electron-beam coating.\n" +
                                    $"Substrates accepted: Superalloy Blade, Combustor Tile, Diesel Injector.\n" +
                                    $"Last Event: {_host.LastEvent}";
            }
        }
    }
}
