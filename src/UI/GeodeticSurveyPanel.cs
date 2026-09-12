// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Geodetic Survey Panel (Plan 79).
/// Presents the triangulation network, baselines, and hidden route discovery.
/// Programmatic UI mimicking GreenhousePanel.
/// </summary>
public partial class GeodeticSurveyPanel : Control
{
    public event Action? OnClose;
    public event Action<string, string>? OnActionRequested;

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _grid;
    private VBoxContainer _detailBox = null!;
    private Label _detailTitle = null!;
    private int _selectedIndex = -1;
    private string _filter = "points"; // points | monuments | triangles | routes

    private GeodeticSurveyHostSession? _host;

    public bool IsBound => _host != null;

    public void Bind(GeodeticSurveyHostSession session)
    {
        if (_host != null)
            _host.StateChanged -= RefreshView;
        _host = session;
        if (_host != null)
            _host.StateChanged += RefreshView;
        RefreshView();
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("Geodetic Survey // Triangulation Network", minWidth: 1100, minHeight: 720);
        SetContentRoot(_shell);

        var sidebarItems = new[]
        {
            new AshfallSidebar.Item { Id = "points",    Label = "Sighted Points",  Hint = "roster of sighted points, angles, uncertainties", IconPath = "" },
            new AshfallSidebar.Item { Id = "monuments", Label = "Monuments",       Hint = "established datum monuments", IconPath = "" },
            new AshfallSidebar.Item { Id = "triangles", Label = "Network",         Hint = "resolved triangles and baselines", IconPath = "" },
            new AshfallSidebar.Item { Id = "routes",    Label = "Hidden Routes",   Hint = "discovered survey corridors", IconPath = "" },
        };
        _sidebar = _shell.SetSidebar(sidebarItems, "View", "points");
        _sidebar.OnSelected += HandleSidebar;

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("network", "Network Accuracy", "—", AshfallMetricCard.Criticality.Normal, minWidth: 140);
        _statusRail.AddCard("monuments", "Monuments", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
        _statusRail.AddCard("triangles", "Triangles", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
        _statusRail.AddCard("routes", "Routes Found", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
        _statusRail.AddCard("signal", "Signal Loss", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);

        var cols = new[]
        {
            new AshfallDataGrid.Column { Header = "ID",        MinWidth = 120, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Type/Info", MinWidth = 140, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Metric 1",  MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Metric 2",  MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Status",    MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
        };
        _grid = new AshfallDataGrid(cols, showHeader: true, minWidth: 720, minHeight: 320);
        _grid.OnRowSelected += HandleRowSelected;

        var body = new HBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;

        body.AddChild(_grid);

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.CustomMinimumSize = new Vector2(300, 320);
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_detailBox);

        _detailTitle = AshfallUiHelpers.MakeSectionHeader("DETAIL");
        _detailTitle.HorizontalAlignment = HorizontalAlignment.Left;
        _detailBox.AddChild(_detailTitle);
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Select a row for details."));

        _shell.SetContent(body);

        RefreshView();
    }

    private void SetContentRoot(Control root)
    {
        AddChild(root);
        root.SetAnchorsPreset(LayoutPreset.FullRect);
        root.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        root.SizeFlagsVertical = SizeFlags.ExpandFill;
    }

    private void HandleSidebar(string id)
    {
        _filter = id;
        _selectedIndex = -1;
        RefreshView();
    }

    private void HandleRowSelected(int idx)
    {
        _selectedIndex = idx;
        RefreshDetail();
    }

    public void RefreshView()
    {
        RefreshStatusRail();
        BuildGridRows();
        RefreshDetail();
    }

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        if (_host == null)
        {
            _statusRail.Set("network", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("monuments", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("triangles", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("routes", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("signal", "—", AshfallMetricCard.Criticality.Normal);
            return;
        }

        var state = _host.System.State;
        _statusRail.Set("network", $"{state.networkAccuracy:F2}", state.networkAccuracy < 0.5f ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("monuments", $"{state.monuments.Count}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("triangles", $"{state.resolvedTriangles.Count}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("routes", $"{state.surveyedCorridorIds.Count} / {state.unlockedShortcutIds.Count}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("signal", "Nominal", AshfallMetricCard.Criticality.Normal);
    }

    private void BuildGridRows()
    {
        if (_grid == null) return;
        var rows = new List<AshfallDataGrid.Row>();

        if (_host == null)
        {
            rows.Add(new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell> {
                new("— no host —", AshfallDataGrid.CellState.Muted),
                new("—", AshfallDataGrid.CellState.Muted),
                new("—", AshfallDataGrid.CellState.Muted),
                new("—", AshfallDataGrid.CellState.Muted),
                new("—", AshfallDataGrid.CellState.Muted),
            }});
            _grid.SetRows(rows);
            return;
        }

        var state = _host.System.State;
        var cat = _host.System.Catalog;

        if (_filter == "points")
        {
            for (int i = 0; i < cat.survey_points.Count; i++)
            {
                var pt = cat.survey_points[i];
                var cells = new List<AshfallDataGrid.Cell>
                {
                    new(pt.survey_point_id, AshfallDataGrid.CellState.Normal),
                    new(pt.display_name, AshfallDataGrid.CellState.Normal),
                    new($"Elev: {pt.elevation_m}m", AshfallDataGrid.CellState.Muted),
                    new($"Vis: {pt.visibility_class}", AshfallDataGrid.CellState.Muted),
                    new(pt.point_type, AshfallDataGrid.CellState.Muted)
                };
                rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
            }
        }
        else if (_filter == "monuments")
        {
            for (int i = 0; i < state.monuments.Count; i++)
            {
                var m = state.monuments[i];
                var cells = new List<AshfallDataGrid.Cell>
                {
                    new(m.monumentId, AshfallDataGrid.CellState.Normal),
                    new(m.surveyPointId, AshfallDataGrid.CellState.Normal),
                    new($"Integrity: {m.integrity * 100f:0}%", m.integrity < 0.5f ? AshfallDataGrid.CellState.Caution : AshfallDataGrid.CellState.Normal),
                    new($"Day: {m.establishedDay}", AshfallDataGrid.CellState.Muted),
                    new(m.isActive ? "ACTIVE" : "OFFLINE", m.isActive ? AshfallDataGrid.CellState.Positive : AshfallDataGrid.CellState.Critical)
                };
                rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
            }
        }
        else if (_filter == "triangles")
        {
            for (int i = 0; i < state.resolvedTriangles.Count; i++)
            {
                var t = state.resolvedTriangles[i];
                var cells = new List<AshfallDataGrid.Cell>
                {
                    new(t.triangleId, AshfallDataGrid.CellState.Normal),
                    new($"{t.pointAId} - {t.pointBId} - {t.pointCId}", AshfallDataGrid.CellState.Normal),
                    new($"Acc: {t.accuracy:F2}", AshfallDataGrid.CellState.Muted),
                    new($"Day: {t.resolvedDay}", AshfallDataGrid.CellState.Muted),
                    new("RESOLVED", AshfallDataGrid.CellState.Positive)
                };
                rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
            }
        }
        else if (_filter == "routes")
        {
            for (int i = 0; i < state.unlockedShortcutIds.Count; i++)
            {
                var r = state.unlockedShortcutIds[i];
                bool surveyed = state.surveyedCorridorIds.Contains(r);
                var cells = new List<AshfallDataGrid.Cell>
                {
                    new(r, AshfallDataGrid.CellState.Normal),
                    new("Hidden Route", AshfallDataGrid.CellState.Normal),
                    new($"Bonus: +{_host.System.GetSpeedBonus(r):P0}", AshfallDataGrid.CellState.Muted),
                    new($"Drift Red: -{_host.System.GetDriftReduction(r):P0}", AshfallDataGrid.CellState.Muted),
                    new(surveyed ? "SURVEYED" : "KNOWN", surveyed ? AshfallDataGrid.CellState.Positive : AshfallDataGrid.CellState.Normal)
                };
                rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
            }
        }

        if (rows.Count == 0)
        {
            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new("— empty —", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                }
            });
        }
        _grid.SetRows(rows);
    }

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        AshfallUiHelpers.EmptyChildren(_detailBox);

        _detailTitle = AshfallUiHelpers.MakeSectionHeader("DETAIL");
        _detailTitle.HorizontalAlignment = HorizontalAlignment.Left;
        _detailBox.AddChild(_detailTitle);

        var separator = AshfallUiHelpers.MakeSeparator();
        separator.CustomMinimumSize = new Vector2(0, 2);
        _detailBox.AddChild(separator);

        if (_host == null)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Geodetic survey offline."));
            return;
        }

        var state = _host.System.State;
        var cat = _host.System.Catalog;
        string? selectedId = null;

        if (_selectedIndex >= 0)
        {
            if (_filter == "points" && _selectedIndex < cat.survey_points.Count)
            {
                var pt = cat.survey_points[_selectedIndex];
                selectedId = pt.survey_point_id;
                _detailTitle.Text = $"POINT DETAIL: {pt.display_name}";
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("ID", pt.survey_point_id, AshfallUiHelpers.ToColor(DesignTheme.Dim)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Type", pt.point_type, AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Elevation", $"{pt.elevation_m} m", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Visibility", pt.visibility_class, AshfallUiHelpers.ToColor(DesignTheme.Dim)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Quality", $"{pt.baseline_quality:F2}", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            }
            else if (_filter == "monuments" && _selectedIndex < state.monuments.Count)
            {
                var m = state.monuments[_selectedIndex];
                selectedId = m.monumentId;
                _detailTitle.Text = $"MONUMENT: {m.surveyPointId}";
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("ID", m.monumentId, AshfallUiHelpers.ToColor(DesignTheme.Dim)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Integrity", $"{m.integrity:P0}", AshfallUiHelpers.ToColor(m.integrity < 0.5f ? DesignTheme.LetheAmber : DesignTheme.Dim)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Status", m.isActive ? "ACTIVE" : "OFFLINE", AshfallUiHelpers.ToColor(m.isActive ? DesignTheme.Lethe : DesignTheme.Critical)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Established", $"Day {m.establishedDay}", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            }
            else if (_filter == "triangles" && _selectedIndex < state.resolvedTriangles.Count)
            {
                var t = state.resolvedTriangles[_selectedIndex];
                selectedId = t.triangleId;
                _detailTitle.Text = "RESOLVED TRIANGLE";
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("ID", t.triangleId, AshfallUiHelpers.ToColor(DesignTheme.Dim)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Points", $"{t.pointAId}, {t.pointBId}, {t.pointCId}", AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Accuracy", $"{t.accuracy:F2}", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            }
            else if (_filter == "routes" && _selectedIndex < state.unlockedShortcutIds.Count)
            {
                var r = state.unlockedShortcutIds[_selectedIndex];
                selectedId = r;
                _detailTitle.Text = "HIDDEN ROUTE";
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Route ID", r, AshfallUiHelpers.ToColor(DesignTheme.Dim)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Surveyed", state.surveyedCorridorIds.Contains(r) ? "YES" : "NO", AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
            }
        }
        else
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Select a row to view details."));
        }

        // Diagnostics
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("DIAGNOSTICS"));
        _detailBox.AddChild(AshfallUiHelpers.MakeSmall("Visual wireframe network (Alpha/Beta/Gamma): ONLINE"));
        _detailBox.AddChild(AshfallUiHelpers.MakeSmall("Baseline length metrics tracking: ACTIVE"));
        _detailBox.AddChild(AshfallUiHelpers.MakeSmall("Real-time power/caloric draw: BALANCED"));
        _detailBox.AddChild(AshfallUiHelpers.MakeSmall("'Hidden Route' discovery threshold: MONITORED"));

        // LastEvent strip
        if (!string.IsNullOrEmpty(_host.LastEvent))
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSmall(_host.LastEvent));
        }

        // Actions
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("ACTIONS"));
        var actionRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);

        var establishBtn = AshfallUiHelpers.MakeButton("ESTABLISH MONUMENT",
            () => { if (selectedId != null) OnActionRequested?.Invoke("establish", selectedId); });
        establishBtn.CustomMinimumSize = new Vector2(140, 30);
        actionRow.AddChild(establishBtn);

        var observeBtn = AshfallUiHelpers.MakeButton("OBSERVE",
            () => { if (selectedId != null) OnActionRequested?.Invoke("observe", selectedId); });
        observeBtn.CustomMinimumSize = new Vector2(90, 30);
        actionRow.AddChild(observeBtn);

        var resolveBtn = AshfallUiHelpers.MakeButton("RESOLVE TRIANGLE",
            () => { if (selectedId != null) OnActionRequested?.Invoke("resolve", selectedId); });
        resolveBtn.CustomMinimumSize = new Vector2(140, 30);
        actionRow.AddChild(resolveBtn);

        _detailBox.AddChild(actionRow);
    }

    public void Open()
    {
        Visible = true;
        RefreshView();
        QueueRedraw();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!Visible) return;
        if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
        {
            OnClose?.Invoke();
            GetViewport().SetInputAsHandled();
        }
    }

    public void Unbind()
    {
        if (_host != null)
        {
            _host.StateChanged -= RefreshView;
            _host = null;
        }
        RefreshView();
    }

    public override void _ExitTree()
    {
        Unbind();
        base._ExitTree();
    }
}
