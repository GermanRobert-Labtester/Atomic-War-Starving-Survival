// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Medical;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Microfluidic Diagnostics Panel (Plan 148).
    /// Programmatic UI implementation for MicrofluidicDiagnosticHostSession.
    /// </summary>
    public partial class MicrofluidicDiagnosticPanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action<string, string>? OnActionRequested;

        private AshfallDashboardShell _shell = null!;
        private AshfallSidebar? _sidebar;
        private AshfallStatusRail? _statusRail;
        private AshfallDataGrid? _grid;
        private VBoxContainer _detailBox = null!;
        private Label _detailTitle = null!;

        private string _currentTab = "active";
        private MicrofluidicDiagnosticHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(MicrofluidicDiagnosticHostSession session)
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

            _shell = new AshfallDashboardShell("MICROFLUIDIC DIAGNOSTICS // RAPID IMMUNOCHIP ANALYZER", minWidth: 1100, minHeight: 720);
            AddChild(_shell);

            var sidebarItems = new[]
            {
                new AshfallSidebar.Item { Id = "active", Label = "Active Runs", Hint = "multichannel optical reader", IconPath = "" },
                new AshfallSidebar.Item { Id = "catalog", Label = "Assay Panels", Hint = "pathogen targets & immunochips", IconPath = "" },
                new AshfallSidebar.Item { Id = "results", Label = "Clinical Results", Hint = "confirmed patient detections", IconPath = "" },
                new AshfallSidebar.Item { Id = "fab", Label = "Cartridge Fab", Hint = "PDMS casting & reagent loading", IconPath = "" }
            };
            _sidebar = _shell.SetSidebar(sidebarItems, "Section", "active");
            _sidebar.OnSelected += tab =>
            {
                _currentTab = tab;
                RefreshView();
            };

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("reader_status", "READER STATUS", "Nominal", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("active_channels", "CHANNELS BUSY", "0 / 4", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("cartridges_fabbed", "CHIPS FABRICATED", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("completed_tests", "TOTAL TESTS", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            var contentSplit = new HSplitContainer();
            contentSplit.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            contentSplit.SizeFlagsVertical = SizeFlags.ExpandFill;

            var cols = new[]
            {
                new AshfallDataGrid.Column { Header = "IDENTIFIER", MinWidth = 240 },
                new AshfallDataGrid.Column { Header = "TARGET / PATIENT", MinWidth = 220 },
                new AshfallDataGrid.Column { Header = "PROGRESS", MinWidth = 140 },
                new AshfallDataGrid.Column { Header = "RESULT / STATUS", MinWidth = 180 }
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

            var detailHeader = new Label { Text = "ASSAY PROTOCOL & DIAGNOSTICS" };
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
                _statusRail?.Set("reader_status", "Unbound", AshfallMetricCard.Criticality.Caution);
                return;
            }

            var state = _host.System.StateDto;
            string statusText = _host.System.IsOperational ? $"{state.ActiveRuns.Count} Running" : "Maintenance Required";
            var crit = _host.System.IsOperational ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Warn;
            _statusRail?.Set("reader_status", statusText, crit);

            _statusRail?.Set("active_channels", $"{state.ActiveRuns.Count} / 4", AshfallMetricCard.Criticality.Normal);
            _statusRail?.Set("cartridges_fabbed", state.CartridgesManufactured.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail?.Set("completed_tests", state.CompletedResults.Count.ToString(), AshfallMetricCard.Criticality.Normal);

            var rows = new List<AshfallDataGrid.Row>();
            if (_currentTab == "active")
            {
                foreach (var run in state.ActiveRuns)
                {
                    var r = new AshfallDataGrid.Row();
                    r.Cells.Add(new AshfallDataGrid.Cell(run.RunId));
                    r.Cells.Add(new AshfallDataGrid.Cell($"{run.AssayId} / {run.PatientId}"));
                    r.Cells.Add(new AshfallDataGrid.Cell($"{run.ProgressMinutes:F0} / {run.RequiredMinutes:F0} min"));
                    r.Cells.Add(new AshfallDataGrid.Cell(run.Status.ToString(), AshfallDataGrid.CellState.Positive));
                    rows.Add(r);
                }
            }
            else if (_currentTab == "catalog")
            {
                foreach (var assay in _host.System.Assays)
                {
                    var r = new AshfallDataGrid.Row();
                    r.Cells.Add(new AshfallDataGrid.Cell(assay.Id));
                    r.Cells.Add(new AshfallDataGrid.Cell(assay.DisplayName));
                    r.Cells.Add(new AshfallDataGrid.Cell($"{assay.BaseDurationMinutes:F0} min"));
                    r.Cells.Add(new AshfallDataGrid.Cell($"Sens {assay.Sensitivity:P0} / Spec {assay.Specificity:P0}", AshfallDataGrid.CellState.Normal));
                    rows.Add(r);
                }
            }
            else if (_currentTab == "results")
            {
                foreach (var result in state.CompletedResults)
                {
                    var r = new AshfallDataGrid.Row();
                    r.Cells.Add(new AshfallDataGrid.Cell(result.RunId));
                    r.Cells.Add(new AshfallDataGrid.Cell($"{result.AssayId} / {result.PatientId}"));
                    r.Cells.Add(new AshfallDataGrid.Cell($"Day {result.CompletedDay}"));
                    var stateKind = result.ResultKind == DiagnosticResultKind.Positive
                        ? AshfallDataGrid.CellState.Warning
                        : AshfallDataGrid.CellState.Positive;
                    r.Cells.Add(new AshfallDataGrid.Cell($"{result.ResultKind} ({result.Confidence01:P0})", stateKind));
                    rows.Add(r);
                }
            }
            else if (_currentTab == "fab")
            {
                if (state.ActiveManufacturingJob != null)
                {
                    var job = state.ActiveManufacturingJob;
                    var r = new AshfallDataGrid.Row();
                    r.Cells.Add(new AshfallDataGrid.Cell(job.JobId));
                    r.Cells.Add(new AshfallDataGrid.Cell(job.AssayId));
                    r.Cells.Add(new AshfallDataGrid.Cell($"{job.ProgressMinutes:F0} / {job.RequiredMinutes:F0} min"));
                    r.Cells.Add(new AshfallDataGrid.Cell(job.Status.ToString(), AshfallDataGrid.CellState.Positive));
                    rows.Add(r);
                }
                var rSummary = new AshfallDataGrid.Row();
                rSummary.Cells.Add(new AshfallDataGrid.Cell("cartridges_total"));
                rSummary.Cells.Add(new AshfallDataGrid.Cell("Fabricated Chips"));
                rSummary.Cells.Add(new AshfallDataGrid.Cell($"{state.CartridgesManufactured} units"));
                rSummary.Cells.Add(new AshfallDataGrid.Cell("Inventory Ready", AshfallDataGrid.CellState.Positive));
                rows.Add(rSummary);
            }

            _grid?.SetRows(rows);
            RefreshDetailView();
        }

        private void RefreshDetailView()
        {
            if (_detailTitle == null || _host == null) return;
            var state = _host.System.StateDto;

            if (_currentTab == "active" && state.ActiveRuns.Count > 0)
            {
                var run = state.ActiveRuns[0];
                _detailTitle.Text = $"Active Run: {run.RunId}\n" +
                                    $"Assay: {run.AssayId}\n" +
                                    $"Patient: {run.PatientId}\n" +
                                    $"Operator: {run.OperatorId}\n" +
                                    $"Incubation Progress: {run.ProgressMinutes:F0} / {run.RequiredMinutes:F0} min\n" +
                                    $"Status: {run.Status}\n" +
                                    $"Last Event: {_host.LastEvent}";
            }
            else if (_currentTab == "fab" && state.ActiveManufacturingJob != null)
            {
                var job = state.ActiveManufacturingJob;
                _detailTitle.Text = $"Casting Job: {job.JobId}\n" +
                                    $"Target Assay: {job.AssayId}\n" +
                                    $"Fab Progress: {job.ProgressMinutes:F0} / {job.RequiredMinutes:F0} min\n" +
                                    $"Mold Integrity: {state.MasterMoldCondition01:P0}\n" +
                                    $"Last Event: {_host.LastEvent}";
            }
            else
            {
                _detailTitle.Text = $"System Status: {_host.System.State}\n" +
                                    $"Master Mold Condition: {state.MasterMoldCondition01:P0}\n" +
                                    $"Machine Condition: {state.MachineCondition01:P0}\n" +
                                    $"Total Fabricated: {state.CartridgesManufactured}\n" +
                                    $"Total Clinical Records: {state.CompletedResults.Count}\n" +
                                    $"Last Event: {_host.LastEvent}";
            }
        }
    }
}
