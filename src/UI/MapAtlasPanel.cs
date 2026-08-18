using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Map Atlas Dashboard (#5 Stitch, Phase 23, Tier-3).
///
/// Phase 23 ships a cartography surface as a Tier-3 HYBRID sub-card sibling
/// of the existing `MapPanel.cs` (Phase 9 modal). The atlas reads the user's
/// own `ExpeditionDefinition` set through `ExpeditionHostSession` (same
/// source as Phase 17's `ExpeditionRadarPanel`) and renders a 5×5 tile
/// grid with sector / danger / radiation / loot summaries per cell.
///
/// Six cards on the status rail and four `AshfallDataGrid` tiles:
///   1. North quadrant (5-row tile grid)
///   2. East quadrant  (5-row tile grid)
///   3. South quadrant (5-row tile grid)
///   4. Action bar     (dispatch / waypoint / detail)
///
/// Plus right-side location detail inspector with live map cell rendering.
/// </summary>
public partial class MapAtlasPanel : Control
{
    public event Action? OnClose;
    public event Action<string>? OnLocationSelected;

    private AshfallDashboardShell _shell = null!;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _northGrid;
    private AshfallDataGrid? _eastGrid;
    private AshfallDataGrid? _southGrid;
    private AshfallDataGrid? _actionBarGrid;
    private VBoxContainer _detailBox = null!;
    private Label _detailTitle = null!;
    private int _selectedIndex = -1;

    private ExpeditionHostSession? _host;
    private List<(string id, string display, string sector, float danger, float rads, string desc)> _locations = new();

    public bool IsBound => _host != null;

    public void Bind(ExpeditionHostSession host)
    {
        _host = host;
        LoadLocationsFromHost();
        RefreshView();
    }

    private void LoadLocationsFromHost()
    {
        _locations.Clear();
        if (_host == null) return;
        var defs = _host.DemoDefinitions;
        for (int i = 0; i < defs.Count; i++)
        {
            var d = defs[i];
            if (d == null) continue;
            _locations.Add((d.id ?? string.Empty,
                d.displayName ?? d.id ?? "Unknown",
                InferSector(d.id),
                d.dangerLevel,
                d.dangerLevel * 2.5f,
                SummariseLoot(d)));
        }
    }

    private static string InferSector(string? id)
    {
        if (string.IsNullOrEmpty(id)) return "Sector ??";
        if (id.Contains("allotments", StringComparison.OrdinalIgnoreCase)) return "Sector 12";
        if (id.Contains("substation", StringComparison.OrdinalIgnoreCase)) return "Sector 04";
        if (id.Contains("cathedral", StringComparison.OrdinalIgnoreCase)) return "Sector 09";
        if (id.Contains("station", StringComparison.OrdinalIgnoreCase)) return "Sector 06";
        if (id.Contains("tunnel", StringComparison.OrdinalIgnoreCase)) return "Sector 02";
        return "Sector ??";
    }

    private static string SummariseLoot(ExpeditionDefinition d)
    {
        if (d.lootCategories == null || d.lootCategories.Count == 0) return "—";
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < d.lootCategories.Count; i++)
        {
            if (sb.Length > 0) sb.Append(", ");
            sb.Append(d.lootCategories[i]);
        }
        return sb.ToString();
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("Map Atlas // Wasteland Cartography Grid", minWidth: 1280, minHeight: 720);
        SetContentRoot(_shell);

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("zones",     "Zones Mapped",  "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);
        _statusRail.AddCard("outposts",  "Outposts",      "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
        _statusRail.AddCard("caravans",  "Caravans",      "—", AshfallMetricCard.Criticality.Caution, minWidth: 100);
        _statusRail.AddCard("dungeons",  "Dungeons",      "—", AshfallMetricCard.Criticality.Warn,   minWidth: 100);
        _statusRail.AddCard("safe",      "Safe Routes",   "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("hazards",   "Hazard Zones",  "—", AshfallMetricCard.Criticality.Critical, minWidth: 120);

        var tileCols = new[]
        {
            new AshfallDataGrid.Column { Header = "Cell",  MinWidth = 60,  Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Tile",  MinWidth = 200, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Sector", MinWidth = 90, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Danger", MinWidth = 80, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Rads/h", MinWidth = 80, Alignment = AshfallDataGrid.ColumnAlign.Right },
        };
        _northGrid = new AshfallDataGrid(tileCols, showHeader: true, minWidth: 380, minHeight: 220);
        _eastGrid = new AshfallDataGrid(tileCols, showHeader: true, minWidth: 380, minHeight: 220);
        _southGrid = new AshfallDataGrid(tileCols, showHeader: true, minWidth: 380, minHeight: 220);
        _eastGrid.OnRowSelected += HandleRowSelected;
        _southGrid.OnRowSelected += HandleRowSelected;
        _northGrid.OnRowSelected += HandleRowSelected;

        var actionCols = new[]
        {
            new AshfallDataGrid.Column { Header = "Action", MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Hint",   MinWidth = 240, Alignment = AshfallDataGrid.ColumnAlign.Left },
        };
        _actionBarGrid = new AshfallDataGrid(actionCols, showHeader: true, minWidth: 380, minHeight: 100);

        var body = new VBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;

        var topRow = new HBoxContainer();
        topRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        topRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        topRow.SizeFlagsVertical = SizeFlags.ExpandFill;

        var northCol = new VBoxContainer();
        northCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        northCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        northCol.AddChild(AshfallUiHelpers.MakeSectionHeader("North Quadrant — Sector 01..05"));
        northCol.AddChild(_northGrid);
        topRow.AddChild(northCol);

        var eastCol = new VBoxContainer();
        eastCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        eastCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        eastCol.AddChild(AshfallUiHelpers.MakeSectionHeader("East Quadrant — Sector 06..10"));
        eastCol.AddChild(_eastGrid);
        topRow.AddChild(eastCol);

        var southCol = new VBoxContainer();
        southCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        southCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        southCol.AddChild(AshfallUiHelpers.MakeSectionHeader("South Quadrant — Sector 11..15"));
        southCol.AddChild(_southGrid);
        topRow.AddChild(southCol);

        body.AddChild(topRow);
        body.AddChild(AshfallUiHelpers.MakeSeparator());

        var actionRow = new HBoxContainer();
        actionRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        actionRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        actionRow.SizeFlagsVertical = SizeFlags.ExpandFill;

        var actionCol = new VBoxContainer();
        actionCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        actionCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        actionCol.AddChild(AshfallUiHelpers.MakeSectionHeader("Action Bar"));
        actionCol.AddChild(_actionBarGrid);
        actionRow.AddChild(actionCol);

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.CustomMinimumSize = new Vector2(280, 220);
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("LOCATION DETAIL"));
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
            "Bind an ExpeditionHostSession to see live map cells. Select a tile to view location detail."));
        actionRow.AddChild(_detailBox);

        body.AddChild(actionRow);
        _shell.SetContent(body);
        _detailTitle = AshfallUiHelpers.MakeSectionHeader("LOCATION DETAIL");
        RefreshView();
    }

    private void SetContentRoot(Control root)
    {
        AddChild(root);
        root.SetAnchorsPreset(LayoutPreset.FullRect);
        root.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        root.SizeFlagsVertical = SizeFlags.ExpandFill;
    }

    private void HandleRowSelected(int idx)
    {
        _selectedIndex = idx;
        var locationId = ResolveVisibleRow(idx);
        if (!string.IsNullOrEmpty(locationId))
            OnLocationSelected?.Invoke(locationId);
        RefreshDetail();
    }

    public void RefreshView()
    {
        RefreshStatusRail();
        BuildTileRows();
        BuildActionRows();
        RefreshDetail();
    }

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        if (_host == null || _locations.Count == 0)
        {
            _statusRail.Set("zones",    "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("outposts", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("caravans", "—", AshfallMetricCard.Criticality.Caution);
            _statusRail.Set("dungeons", "—", AshfallMetricCard.Criticality.Warn);
            _statusRail.Set("safe",     "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("hazards",  "—", AshfallMetricCard.Criticality.Critical);
            return;
        }

        int outposts = 0, caravans = 0, dungeons = 0, safe = 0, hazards = 0;
        for (int i = 0; i < _locations.Count; i++)
        {
            var l = _locations[i];
            if (l.danger >= 4f) dungeons++;
            else if (l.danger >= 2f) hazards++;
            else outposts++;
            if (l.danger == 0f) safe++;
        }
        if (_host?.Engine != null && _host.Engine.ActiveCount > 0) caravans = _host.Engine.ActiveCount;

        _statusRail.Set("zones",    $"{_locations.Count}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("outposts", $"{outposts}", outposts > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Caution);
        _statusRail.Set("caravans", $"{caravans}", caravans > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("dungeons", $"{dungeons}", dungeons > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("safe",     $"{safe}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("hazards",  $"{hazards}", hazards > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
    }

    private void BuildTileRows()
    {
        if (_northGrid == null || _eastGrid == null || _southGrid == null) return;

        if (_host == null || _locations.Count == 0)
        {
            _northGrid.SetRows(BuildFixtureRows(0));
            _eastGrid.SetRows(BuildFixtureRows(1));
            _southGrid.SetRows(BuildFixtureRows(2));
            return;
        }

        _northGrid.SetRows(TileRowsFor(0));
        _eastGrid.SetRows(TileRowsFor(1));
        _southGrid.SetRows(TileRowsFor(2));
    }

    private List<AshfallDataGrid.Row> TileRowsFor(int quadrant)
    {
        var rows = new List<AshfallDataGrid.Row>();
        for (int i = 0; i < _locations.Count; i++)
        {
            var l = _locations[i];
            if (l.sector == null) continue;
            int sectorNum = ExtractSectorNum(l.sector);
            if (QuadrantForSector(sectorNum) != quadrant) continue;
            int cell = (i % 5) + 1;
            var cells = new List<AshfallDataGrid.Cell>
            {
                new($"Q{quadrant+1}.{cell}", AshfallDataGrid.CellState.Muted),
                new(l.display, AshfallDataGrid.CellState.Normal),
                new(l.sector, AshfallDataGrid.CellState.Muted),
                new($"LVL {(int)l.danger}", l.danger >= 4f ? AshfallDataGrid.CellState.Critical :
                                        l.danger >= 2f ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Normal),
                new($"{l.rads:0.0}", l.rads > 10f ? AshfallDataGrid.CellState.Critical : AshfallDataGrid.CellState.Normal),
            };
            rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
        }
        if (rows.Count == 0)
        {
            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("— quadrant empty —", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                }
            });
        }
        return rows;
    }

    private static int ExtractSectorNum(string sector)
    {
        int idx = sector.LastIndexOf(' ');
        if (idx < 0 || idx + 1 >= sector.Length) return 0;
        if (int.TryParse(sector.Substring(idx + 1), out int n)) return n;
        return 0;
    }

    private static int QuadrantForSector(int sectorNum)
    {
        if (sectorNum <= 0) return 0;
        if (sectorNum <= 5) return 0;   // North 01..05
        if (sectorNum <= 10) return 1;  // East 06..10
        if (sectorNum <= 15) return 2;  // South 11..15
        return 0;
    }

    private string ResolveVisibleRow(int visibleIndex)
    {
        if (_host == null || _locations.Count == 0) return string.Empty;
        int seen = -1;
        for (int i = 0; i < _locations.Count; i++)
        {
            int sectorNum = ExtractSectorNum(_locations[i].sector);
            // Match any quadrant: visible rows are those the grids emit.
            seen++;
            if (seen == visibleIndex) return _locations[i].id;
        }
        return string.Empty;
    }

    private void BuildActionRows()
    {
        if (_actionBarGrid == null) return;
        _actionBarGrid.SetRows(BuildActionFixtureRows());
    }

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        while (_detailBox.GetChildCount() > 0)
        {
            var c = _detailBox.GetChild(0);
            _detailBox.RemoveChild(c);
            c.QueueFree();
        }
        _detailTitle = AshfallUiHelpers.MakeSectionHeader("LOCATION DETAIL");
        _detailBox.AddChild(_detailTitle);
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        if (_host == null || _locations.Count == 0)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Map atlas offline. Bind an ExpeditionHostSession to see live map cells."));
            return;
        }
        if (_selectedIndex < 0)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Select a tile row to view location detail."));
            return;
        }
        var loc = FindLocation(_selectedIndex);
        if (loc == null)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Selected row is out of scope."));
            return;
        }
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Sector", loc.Value.sector,
            AshfallUiHelpers.ToColor(DesignTheme.Warm)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Danger", $"LVL {(int)loc.Value.danger}",
            loc.Value.danger >= 4f ? AshfallUiHelpers.ToColor(DesignTheme.Critical) :
            loc.Value.danger >= 2f ? AshfallUiHelpers.ToColor(DesignTheme.Entropy) :
                                       AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Rads/h", $"{loc.Value.rads:0.0}",
            loc.Value.rads > 10f ? AshfallUiHelpers.ToColor(DesignTheme.Critical) : AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeSubsectionHeader("Loot Categories"));
        _detailBox.AddChild(AshfallUiHelpers.MakeSmall(loc.Value.desc, autowrap: true));
    }

    private (string id, string display, string sector, float danger, float rads, string desc)? FindLocation(int visibleIndex)
    {
        if (_host == null || _locations.Count == 0) return null;
        int seen = -1;
        for (int i = 0; i < _locations.Count; i++)
        {
            seen++;
            if (seen == visibleIndex) return _locations[i];
        }
        return null;
    }

    /// <summary>Hard-coded fixture rows for the bound=false case. Per-quadrant tiles drawn from canonical location ids.</summary>
    internal static List<AshfallDataGrid.Row> BuildFixtureRows(int quadrant)
    {
        var rows = new List<AshfallDataGrid.Row>();
        if (quadrant == 0)
        {
            rows.Add(new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Q1.1", AshfallDataGrid.CellState.Muted),
                new("The Denial Cut Substation", AshfallDataGrid.CellState.Normal),
                new("Sector 04", AshfallDataGrid.CellState.Muted),
                new("LVL 4", AshfallDataGrid.CellState.Critical),
                new("10.0", AshfallDataGrid.CellState.Critical),
            }, Selectable = true });
        }
        else if (quadrant == 1)
        {
            rows.Add(new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Q2.1", AshfallDataGrid.CellState.Muted),
                new("Holdfast Bunker [home]", AshfallDataGrid.CellState.Normal),
                new("Sector 07", AshfallDataGrid.CellState.Muted),
                new("LVL 0", AshfallDataGrid.CellState.Normal),
                new("0.5", AshfallDataGrid.CellState.Normal),
            }, Selectable = true });
        }
        else
        {
            rows.Add(new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Q3.1", AshfallDataGrid.CellState.Muted),
                new("The Works Allotment Commune", AshfallDataGrid.CellState.Normal),
                new("Sector 12", AshfallDataGrid.CellState.Muted),
                new("LVL 2", AshfallDataGrid.CellState.Warning),
                new("5.0", AshfallDataGrid.CellState.Normal),
            }, Selectable = true });
        }
        return rows;
    }

    internal static List<AshfallDataGrid.Row> BuildActionFixtureRows()
    {
        return new List<AshfallDataGrid.Row>
        {
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Dispatch Sortie", AshfallDataGrid.CellState.Normal),
                new("Select a tile · pick a survivor · pick a stance", AshfallDataGrid.CellState.Muted),
            }, Selectable = false },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Plot Waypoint",  AshfallDataGrid.CellState.Normal),
                new("Mark a safe route for the caravan",              AshfallDataGrid.CellState.Muted),
            }, Selectable = false },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Inspect Detail", AshfallDataGrid.CellState.Normal),
                new("Click a tile to view loot categories and rads",   AshfallDataGrid.CellState.Muted),
            }, Selectable = false },
        };
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
}
