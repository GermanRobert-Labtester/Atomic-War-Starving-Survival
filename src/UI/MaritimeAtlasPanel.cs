using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Maritime Atlas Dashboard (#48 Stitch, Phase 24, Tier-3).
///
/// Phase 24 ships the maritime / dive-site coordinate panel as a Tier-3
/// HYBRID sub-card sibling of the existing `MaritimePanel.cs` and
/// `DeepCoastPanel.cs` (Phase 9 modals). The atlas reads the user's own
/// `DiveSiteDefinition` catalog through the Maritime host session (the
/// same wire as `DiveInstanceRunner`).
///
/// Four tiles:
///   1. Deckhouse tile     — first-leg surface access
///   2. Companionway tile  — second-leg hull access
///   3. Hold Approach tile  — third-leg keeper trace
///   4. The Hold tile       — fourth-leg recovery deep-end
///
/// Plus six status rail cards and a right-side dive detail inspector.
/// </summary>
public partial class MaritimeAtlasPanel : Control
{
    public event Action? OnClose;
    // OnSiteSelected was previously declared here but had zero subscribers
    // anywhere in the codebase (audit §10). Removed in the cleanup pass;
    // re-introduce with a host subscriber if a downstream consumer is added.

    private AshfallDashboardShell _shell = null!;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _deckhouseGrid;
    private AshfallDataGrid? _companionwayGrid;
    private AshfallDataGrid? _holdApproachGrid;
    private AshfallDataGrid? _holdGrid;
    private AshfallDataGrid? _actionBarGrid;
    private VBoxContainer _detailBox = null!;
    private Label _detailTitle = null!;
    private int _selectedIndex = -1;

    private MaritimeHostSession? _host;
    private List<(string siteId, string name, int oxygen, float noiseFloor, string keeper, int rooms, string tideWindow)> _sites = new();
    private int _campaignDay;

    /// <summary>
    /// Authoritative campaign day for tide-window presentation (optional —
    /// without a day provider the tide column shows the authored rule only).
    /// </summary>
    public Func<int>? CampaignDayProvider { get; set; }

    public bool IsBound => _host != null;

    public void Bind(MaritimeHostSession host)
    {
        _host = host;
        LoadSitesFromHost();
        RefreshView();
    }

    private void LoadSitesFromHost()
    {
        _sites.Clear();
        if (_host == null) return;

        if (_host.Dive.Catalog?.dive_sites != null && _host.Dive.Catalog.dive_sites.Count > 0)
        {
            foreach (var site in _host.Dive.Catalog.dive_sites)
            {
                if (site == null) continue;
                _sites.Add((
                    site.site_id,
                    site.name,
                    site.oxygen_budget_ticks,
                    site.base_noise_floor,
                    site.keeper_thread_id ?? "—",
                    site.rooms?.Count ?? 4,
                    site.tide_window ?? "any"
                ));
            }
        }
        else if (_host.Dive.Sites != null && _host.Dive.Sites.Count > 0)
        {
            foreach (var site in _host.Dive.Sites)
            {
                _sites.Add((
                    site.siteId,
                    site.displayName,
                    (int)(site.depthMeters * 2.5f),
                    site.hazardLevel,
                    site.isHazardous ? "q_keeper_of_logs" : "—",
                    4,
                    "any"
                ));
            }
        }
        else
        {
            _sites.Add(("site_exp09_ss_sovereign", "S.S. Sovereign", 120, 0.85f, "q_keeper_of_logs", 4, "slack"));
            _sites.Add(("site_exp09_ferry_terminal", "The Drowned Ferry Terminal", 90, 0.60f, "—", 4, "any"));
            _sites.Add(("site_exp09_barge_flotilla", "The Barge Flotilla", 100, 0.40f, "—", 4, "any"));
            _sites.Add(("site_exp09_naval_patrol", "The Patrol Craft", 80, 0.70f, "—", 4, "any"));
        }
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("Maritime Atlas // Deep Coast Dive Coordinates", minWidth: 1280, minHeight: 720);
        SetContentRoot(_shell);

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("stage",      "Stage",        "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
        _statusRail.AddCard("oxygen",     "Oxygen",       "—", AshfallMetricCard.Criticality.Normal, minWidth: 90);
        _statusRail.AddCard("decision",   "Decision",     "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);
        _statusRail.AddCard("risk",       "Detection",    "—", AshfallMetricCard.Criticality.Warn, minWidth: 110);
        _statusRail.AddCard("chloroform", "Chloroform",   "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("assigned",   "Crew",         "—", AshfallMetricCard.Criticality.Normal, minWidth: 90);

        // Room-tile cols.
        var roomCols = new[]
        {
            new AshfallDataGrid.Column { Header = "Site",        MinWidth = 200, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Coy. thread", MinWidth = 140, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Oxygen",      MinWidth = 80,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Noise",       MinWidth = 80,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Rooms",       MinWidth = 70,  Alignment = AshfallDataGrid.ColumnAlign.Right },
        };
        _deckhouseGrid = new AshfallDataGrid(roomCols, showHeader: true, minWidth: 380, minHeight: 100);
        _companionwayGrid = new AshfallDataGrid(roomCols, showHeader: true, minWidth: 380, minHeight: 100);
        _holdApproachGrid = new AshfallDataGrid(roomCols, showHeader: true, minWidth: 380, minHeight: 100);
        _holdGrid = new AshfallDataGrid(roomCols, showHeader: true, minWidth: 380, minHeight: 100);

        var actionCols = new[]
        {
            new AshfallDataGrid.Column { Header = "Action", MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Hint",   MinWidth = 260, Alignment = AshfallDataGrid.ColumnAlign.Left },
        };
        _actionBarGrid = new AshfallDataGrid(actionCols, showHeader: true, minWidth: 380, minHeight: 100);

        var body = new VBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;

        // Compose 2x2 tile grid.
        var topRow = new HBoxContainer();
        topRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        topRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        topRow.SizeFlagsVertical = SizeFlags.ExpandFill;

        var northWest = new VBoxContainer();
        northWest.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        northWest.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        northWest.AddChild(AshfallUiHelpers.MakeSectionHeader("Room 0 — Deckhouse"));
        northWest.AddChild(_deckhouseGrid);
        topRow.AddChild(northWest);

        var northEast = new VBoxContainer();
        northEast.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        northEast.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        northEast.AddChild(AshfallUiHelpers.MakeSectionHeader("Room 1 — Companionway"));
        northEast.AddChild(_companionwayGrid);
        topRow.AddChild(northEast);

        body.AddChild(topRow);

        var botRow = new HBoxContainer();
        botRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        botRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        botRow.SizeFlagsVertical = SizeFlags.ExpandFill;

        var southWest = new VBoxContainer();
        southWest.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        southWest.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        southWest.AddChild(AshfallUiHelpers.MakeSectionHeader("Room 2 — Hold Approach (Keeper Trace)"));
        southWest.AddChild(_holdApproachGrid);
        botRow.AddChild(southWest);

        var southEast = new VBoxContainer();
        southEast.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        southEast.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        southEast.AddChild(AshfallUiHelpers.MakeSectionHeader("Room 3 — The Hold"));
        southEast.AddChild(_holdGrid);
        botRow.AddChild(southEast);

        body.AddChild(botRow);
        body.AddChild(AshfallUiHelpers.MakeSeparator());

        var actionRow = new HBoxContainer();
        actionRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        actionRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        actionRow.SizeFlagsVertical = SizeFlags.ExpandFill;

        var actionCol = new VBoxContainer();
        actionCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        actionCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        actionCol.AddChild(AshfallUiHelpers.MakeSectionHeader("Dive Action Bar"));
        actionCol.AddChild(_actionBarGrid);
        actionRow.AddChild(actionCol);

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.CustomMinimumSize = new Vector2(280, 220);
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("DIVE DETAIL"));
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
            "Bind a MaritimeHostSession to see live dive-site state."));
        actionRow.AddChild(_detailBox);

        body.AddChild(actionRow);
        _shell.SetContent(body);
        _detailTitle = AshfallUiHelpers.MakeSectionHeader("DIVE DETAIL");
        RefreshView();
    }

    private void SetContentRoot(Control root)
    {
        AddChild(root);
        root.SetAnchorsPreset(LayoutPreset.FullRect);
        root.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        root.SizeFlagsVertical = SizeFlags.ExpandFill;
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
        if (_host == null || _sites.Count == 0)
        {
            _statusRail.Set("stage",     "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("oxygen",    "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("decision",  "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("risk",      "—", AshfallMetricCard.Criticality.Warn);
            _statusRail.Set("chloroform","—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("assigned",  "—", AshfallMetricCard.Criticality.Normal);
            return;
        }
        int totalOxygen = 0;
        for (int i = 0; i < _sites.Count; i++) totalOxygen += _sites[i].oxygen;
        _statusRail.Set("stage", "Sealed", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("oxygen", $"{totalOxygen}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("decision", "None", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("risk", "—", AshfallMetricCard.Criticality.Caution);
        _statusRail.Set("chloroform", "—", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("assigned", $"{_sites.Count}", AshfallMetricCard.Criticality.Normal);
    }

    private void BuildTileRows()
    {
        if (_deckhouseGrid == null || _companionwayGrid == null
            || _holdApproachGrid == null || _holdGrid == null) return;

        if (_host == null || _sites.Count == 0)
        {
            _deckhouseGrid.SetRows(BuildRoomFixtureRows(0));
            _companionwayGrid.SetRows(BuildRoomFixtureRows(1));
            _holdApproachGrid.SetRows(BuildRoomFixtureRows(2));
            _holdGrid.SetRows(BuildRoomFixtureRows(3));
            return;
        }
        _deckhouseGrid.SetRows(RoomRowsFor(0));
        _companionwayGrid.SetRows(RoomRowsFor(1));
        _holdApproachGrid.SetRows(RoomRowsFor(2));
        _holdGrid.SetRows(RoomRowsFor(3));
    }

    private List<AshfallDataGrid.Row> RoomRowsFor(int room)
    {
        var rows = new List<AshfallDataGrid.Row>();
        for (int i = 0; i < _sites.Count; i++)
        {
            var s = _sites[i];
            // Distribute sites across rooms deterministically.
            if ((i % 4) != room) continue;
            var cells = new List<AshfallDataGrid.Cell>
            {
                new(s.name, AshfallDataGrid.CellState.Normal),
                new(s.keeper, AshfallDataGrid.CellState.Muted),
                new($"{s.oxygen} t", AshfallDataGrid.CellState.Normal),
                new($"{s.noiseFloor:0.00}", AshfallDataGrid.CellState.Normal),
                new($"{s.rooms}", AshfallDataGrid.CellState.Muted),
            };
            rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = false });
        }
        if (rows.Count == 0)
        {
            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("— empty —", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                }
            });
        }
        return rows;
    }

    private void BuildActionRows()
    {
        if (_actionBarGrid == null) return;
        _actionBarGrid.SetRows(BuildActionFixtureRows());
    }

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        AshfallUiHelpers.EmptyChildren(_detailBox);
        _detailTitle = AshfallUiHelpers.MakeSectionHeader("DIVE DETAIL");
        _detailBox.AddChild(_detailTitle);
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        if (_host == null || _sites.Count == 0)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Maritime atlas offline. Bind a MaritimeHostSession to see live dive-site state."));
            return;
        }
        if (_selectedIndex < 0)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Dive site state surfaces here when the dive begins."));
            return;
        }
        var (siteId, name, oxygen, noiseFloor, keeper, rooms, tideWindow) = _sites[_selectedIndex];
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Site", name,
            AshfallUiHelpers.ToColor(DesignTheme.Warm)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Site Id", siteId,
            AshfallUiHelpers.ToColor(DesignTheme.Pale)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Oxygen", $"{oxygen} ticks",
            AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Noise floor", $"{noiseFloor:0.00}",
            AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Keeper thread", keeper,
            AshfallUiHelpers.ToColor(DesignTheme.Muted)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Rooms", $"{rooms}",
            AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Tide window", TideText(tideWindow),
            AshfallUiHelpers.ToColor(DesignTheme.Muted)));
    }

    private string TideText(string rawWindow)
    {
        var parsed = Ashfall.Core.Maritime.DiveSiteTideWindows.Parse(rawWindow);
        if (parsed == Ashfall.Core.Maritime.TideWindow.Any) return "Any tide";

        string rule = until_text(rawWindow, 0);
        int day = CampaignDayProvider?.Invoke() ?? -1;
        if (day < 0) return rule; // no day authority: show the authored rule

        int until = Ashfall.Core.Maritime.TideCalendar.DaysUntilOpen(parsed, day);
        if (until == 0) return rule + " — open now";
        return rule + " · now: " + Ashfall.Core.Maritime.TideCalendar.PhaseName(Ashfall.Core.Maritime.TideCalendar.PhaseFor(day)).ToLowerInvariant() + ", opens in " + until + " d";
    }

    private static string until_text(string window, int days)
    {
        return window switch
        {
            "slack" => "Open at slack water only",
            "low" => "Open only at low tide",
            "high" => "Open only at high water",
            "falling" => "Opens on the falling tide",
            "unsafe_at_peak" => "Unsafe during peak flow",
            _ => "Any tide"
        };
    }

    /// <summary>Hard-coded fixture rows for the bound=false case. 1 row per room tile.</summary>
    internal static List<AshfallDataGrid.Row> BuildRoomFixtureRows(int room)
    {
        var rows = new List<AshfallDataGrid.Row>();
        string roomName = room switch
        {
            0 => "S.S. Sovereign",
            1 => "Submarine Yard (East)",
            2 => "S.S. Sovereign",
            _ => "Harbor Wreck Pt.3",
        };
        int oxygen = room switch
        {
            0 => 120,
            1 => 96,
            2 => 120,
            _ => 84,
        };
        string keeper = room switch
        {
            0 => "q_keeper_of_logs",
            1 => "q_books_of_chrome",
            2 => "q_keeper_of_logs",
            _ => "q_keeper_of_logs",
        };
        rows.Add(new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
        {
            new(roomName, AshfallDataGrid.CellState.Normal),
            new(keeper,   AshfallDataGrid.CellState.Muted),
            new($"{oxygen} t", AshfallDataGrid.CellState.Normal),
            new("0.85",     AshfallDataGrid.CellState.Normal),
            new("4",        AshfallDataGrid.CellState.Muted),
        }, Selectable = false });
        return rows;
    }

    internal static List<AshfallDataGrid.Row> BuildActionFixtureRows()
    {
        return new List<AshfallDataGrid.Row>
        {
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Surface Approach", AshfallDataGrid.CellState.Normal),
                new("Acid squall covers deckhouse entry", AshfallDataGrid.CellState.Muted),
            }, Selectable = false },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Hull Infiltration", AshfallDataGrid.CellState.Normal),
                new("Companionway quiets the rain; lower mask",   AshfallDataGrid.CellState.Muted),
            }, Selectable = false },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Hold Approach",    AshfallDataGrid.CellState.Normal),
                new("432 Hz pipes harmonize with breath · fear raises resonance",  AshfallDataGrid.CellState.Muted),
            }, Selectable = false },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Recover the Dweller", AshfallDataGrid.CellState.Normal),
                new("Final leg · chloroform the keeper · extract",             AshfallDataGrid.CellState.Muted),
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
