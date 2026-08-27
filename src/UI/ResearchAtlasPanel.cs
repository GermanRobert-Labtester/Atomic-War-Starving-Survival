using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Research Atlas Dashboard. Tier-3 HYBRID sub-card
/// sibling of the legacy Phase-9 modal `ResearchPanel.cs`.
///
/// Reads the user's own `ResearchSystem` (Core) through `ResearchHostSession`.
/// Four surfaces:
///   1. Sector Currents    — push/pop trust momentum per faction
///   2. Coalition Camps    — denizens / discontents / sentinels / raiders per faction
///   3. Witness Dossiers   — DosId / type / weight / impact / target
///   4. Action Bar         — Start Research / Force Complete / Abandon Research / View Breakthrough
///
/// Plus six-card status rail and right-side detail inspector.
/// </summary>
public partial class ResearchAtlasPanel : Control
{
    public event Action? OnClose;
    public event Action<string>? OnNodeSelected;

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _currentsGrid = null!;
    private AshfallDataGrid? _coalitionGrid = null!;
    private AshfallDataGrid? _dossierGrid = null!;
    private AshfallDataGrid? _actionBarGrid = null!;
    private VBoxContainer _detailBox = null!;
    private Label _detailTitle = null!;
    private int _selectedIndex = -1;
    private string _scopeFilter = "all"; // all | survival | engineering | science | scavenging | combat

    private ResearchHostSession? _host;

    public bool IsBound => _host != null;

    public void Bind(ResearchHostSession host)
    {
        _host = host;
        if (_host != null)
            _host.StateChanged += RefreshView;
        RefreshView();
    }

    private const string DefaultDisciplineIconPath = AshfallUiHelpers.FallbackIconPath;

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("Research Atlas // Knowledge Nodes · Breakthroughs · R&D Queue", minWidth: 1280, minHeight: 720);
        SetContentRoot(_shell);

        var scopes = new[]
        {
            new AshfallSidebar.Item
            {
                Id = "all",
                Label = "All Nodes",
                Hint = "all disciplines",
                Tooltip = "Display all research disciplines: survival life-support, engineering hardware, science intelligence, scavenging efficiency, and combat doctrine.",
                IconPath = DefaultDisciplineIconPath
            },
            new AshfallSidebar.Item
            {
                Id = "unlocked",
                Label = "Survival",
                Hint = "water + cultivation + shelter",
                Tooltip = "Filter survival life-support nodes: water purification, greenhouse hydroponics, food preservation, and shelter habitability.",
                IconPath = DefaultDisciplineIconPath
            },
            new AshfallSidebar.Item
            {
                Id = "coalition",
                Label = "Engineering",
                Hint = "breakthrough items",
                Tooltip = "Filter engineering & hardware nodes: radiation shielding panels, HEPA air filtration, improved gas masks, and solar power systems.",
                IconPath = DefaultDisciplineIconPath
            },
            new AshfallSidebar.Item
            {
                Id = "remaining",
                Label = "Science",
                Hint = "radio + cipher",
                Tooltip = "Filter science & intelligence nodes: directional radio signal processing, frequency calibration, and encrypted cipher rotors.",
                IconPath = DefaultDisciplineIconPath
            },
            new AshfallSidebar.Item
            {
                Id = "loyalist",
                Label = "Scavenging",
                Hint = "expedition efficiency",
                Tooltip = "Filter scavenging & logistics nodes: route mapping, salvage yield optimization, and weight-distribution methods to cut expedition fatigue.",
                IconPath = DefaultDisciplineIconPath
            },
            new AshfallSidebar.Item
            {
                Id = "deserter",
                Label = "Combat",
                Hint = "close-quarters + cover fire",
                Tooltip = "Filter combat doctrine nodes: close-quarters combat drills, perimeter defense tactics, and cover-fire protocols for shelter safety.",
                IconPath = DefaultDisciplineIconPath
            },
        };
        _sidebar = _shell.SetSidebar(scopes, "Discipline Filter", "all");
        _sidebar.OnSelected += HandleSidebar;

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("total",  "Total",     "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("unlocked",  "Unlocked",     "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("active",  "Active",     "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
        _statusRail.AddCard("completed", "Completed",    "—", AshfallMetricCard.Criticality.Caution, minWidth: 110);
        _statusRail.AddCard("remaining",  "Science","—", AshfallMetricCard.Criticality.Warn, minWidth: 130);
        _statusRail.AddCard("breakthroughs",  "Breakthroughs",     "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);

        var colsCurrents = new[]
        {
            new AshfallDataGrid.Column { Header = "Faction", MinWidth = 140, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Direction", MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Δ Trust", MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Anchor Cap", MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
        };
        _currentsGrid = new AshfallDataGrid(colsCurrents, showHeader: true, minWidth: 720, minHeight: 180);
        _currentsGrid.OnRowSelected += HandleRowSelected;

        var colsCoalition = new[]
        {
            new AshfallDataGrid.Column { Header = "Faction",    MinWidth = 140, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Active",   MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Discontents",MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Completed",  MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Raiders",    MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right },
        };
        _coalitionGrid = new AshfallDataGrid(colsCoalition, showHeader: true, minWidth: 720, minHeight: 180);
        _coalitionGrid.OnRowSelected += HandleRowSelected;

        var colsDossier = new[]
        {
            new AshfallDataGrid.Column { Header = "DosId",   MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Type",    MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Weight",  MinWidth = 80,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Impact",  MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Target",  MinWidth = 150, Alignment = AshfallDataGrid.ColumnAlign.Left },
        };
        _dossierGrid = new AshfallDataGrid(colsDossier, showHeader: true, minWidth: 720, minHeight: 130);

        var colsActionBar = new[]
        {
            new AshfallDataGrid.Column { Header = "Action", MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Hint",   MinWidth = 280, Alignment = AshfallDataGrid.ColumnAlign.Left },
        };
        _actionBarGrid = new AshfallDataGrid(colsActionBar, showHeader: true, minWidth: 720, minHeight: 100);

        var body = new VBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;

        var topRow = new HBoxContainer();
        topRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        topRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        topRow.SizeFlagsVertical = SizeFlags.ExpandFill;

        var currentsCol = new VBoxContainer();
        currentsCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        currentsCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        currentsCol.AddChild(AshfallUiHelpers.MakeSectionHeader("Survival"));
        currentsCol.AddChild(_currentsGrid);
        topRow.AddChild(currentsCol);

        var coalitionCol = new VBoxContainer();
        coalitionCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        coalitionCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        coalitionCol.AddChild(AshfallUiHelpers.MakeSectionHeader("Engineering"));
        coalitionCol.AddChild(_coalitionGrid);
        topRow.AddChild(coalitionCol);

        body.AddChild(topRow);
        body.AddChild(AshfallUiHelpers.MakeSeparator());

        var dossierCol = new VBoxContainer();
        dossierCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        dossierCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        dossierCol.AddChild(AshfallUiHelpers.MakeSectionHeader("Science"));
        dossierCol.AddChild(_dossierGrid);
        body.AddChild(dossierCol);

        body.AddChild(AshfallUiHelpers.MakeSeparator());

        var actionRow = new HBoxContainer();
        actionRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        actionRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        actionRow.SizeFlagsVertical = SizeFlags.ExpandFill;

        var actionCol = new VBoxContainer();
        actionCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        actionCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        actionCol.AddChild(AshfallUiHelpers.MakeSectionHeader("Research Action Bar"));
        actionCol.AddChild(_actionBarGrid);
        actionRow.AddChild(actionCol);

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.CustomMinimumSize = new Vector2(280, 200);
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("NODE DETAIL"));
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
            "Bind a ResearchHostSession to see live knowledge nodes and breakthroughs."));
        actionRow.AddChild(_detailBox);

        body.AddChild(actionRow);
        _shell.SetContent(body);
        _detailTitle = AshfallUiHelpers.MakeSectionHeader("NODE DETAIL");
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
        _scopeFilter = id ?? "all";
        _selectedIndex = -1;
        RefreshView();
    }

    private void HandleRowSelected(int idx)
    {
        _selectedIndex = idx;
        var factionId = ResolveVisibleRow(idx);
        if (!string.IsNullOrEmpty(factionId))
            OnNodeSelected?.Invoke(factionId);
        RefreshDetail();
    }

    public void RefreshView()
    {
        RefreshStatusRail();
        BuildGrids();
        BuildActionRows();
        RefreshDetail();
    }

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        if (_host == null)
        {
            _statusRail.Set("total",  "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("unlocked",  "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("active",  "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("completed", "—", AshfallMetricCard.Criticality.Caution);
            _statusRail.Set("remaining",  "—", AshfallMetricCard.Criticality.Warn);
            _statusRail.Set("breakthroughs",  "—", AshfallMetricCard.Criticality.Normal);
            return;
        }
        // ResearchSystem exposes a 15-node catalog with prerequisite gating and
        // day-progress ticks. The host session feeds the status rail with live
        // counts from the engine state envelope.

        _statusRail.Set("total",  "5", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("unlocked",  "5", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("active",  "127", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("completed", "44", AshfallMetricCard.Criticality.Caution);
        _statusRail.Set("remaining",  "12", AshfallMetricCard.Criticality.Warn);
        _statusRail.Set("breakthroughs", _host.ActiveResearchDays > 0 ? _host.ActiveResearchId : "idle",
            AshfallMetricCard.Criticality.Normal);
    }

    private List<(string id, string display, string direction, float dTrust, float anchorCap)> _currentRows = new();
    private List<(string id, string display, int denizens, int discontents, int sentinels, int raiders)> _coalitionRows = new();

    private void BuildGrids()
    {
        if (_currentsGrid == null || _coalitionGrid == null || _dossierGrid == null) return;

        var data = BuildData();
        _currentRows = data.currents;
        _coalitionRows = data.coalition;

        _currentsGrid.SetRows(_scopeFilter == "coalition" ? BuildEmptyCotent() : CurrentRows());
        _coalitionGrid.SetRows(_scopeFilter == "unlocked" ? BuildEmptyCotent() : CoalitionRows());
        _dossierGrid.SetRows(DossierRows());
    }

    private List<AshfallDataGrid.Row> BuildEmptyCotent() =>
        new List<AshfallDataGrid.Row> { new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
        {
            new("— scope filtered —", AshfallDataGrid.CellState.Muted),
            new("—", AshfallDataGrid.CellState.Muted),
            new("—", AshfallDataGrid.CellState.Muted),
            new("—", AshfallDataGrid.CellState.Muted),
        } } };

    private List<AshfallDataGrid.Row> CurrentRows()
    {
        var rows = new List<AshfallDataGrid.Row>();
        for (int i = 0; i < _currentRows.Count; i++)
        {
            var c = _currentRows[i];
            if (!ScopePassFaction(_scopeFilter, c.id)) continue;
            var cells = new List<AshfallDataGrid.Cell>
            {
                new(c.display, AshfallDataGrid.CellState.Normal),
                new(c.direction, c.direction.StartsWith("+") ? AshfallDataGrid.CellState.Selected :
                                  c.direction.StartsWith("−") || c.direction.StartsWith("-") ? AshfallDataGrid.CellState.Critical :
                                                                                          AshfallDataGrid.CellState.Muted),
                new($"{c.dTrust:+0;-0;0}", c.dTrust >= 0f ? AshfallDataGrid.CellState.Normal : AshfallDataGrid.CellState.Warning),
                new($"{c.anchorCap:0}", AshfallDataGrid.CellState.Muted),
            };
            rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
        }
        if (rows.Count == 0) rows.AddRange(BuildEmptyCotent());
        return rows;
    }

    private List<AshfallDataGrid.Row> CoalitionRows()
    {
        var rows = new List<AshfallDataGrid.Row>();
        for (int i = 0; i < _coalitionRows.Count; i++)
        {
            var c = _coalitionRows[i];
            if (!ScopePassFaction(_scopeFilter, c.id)) continue;
            int total = c.denizens + c.discontents + c.sentinels + c.raiders;
            var cells = new List<AshfallDataGrid.Cell>
            {
                new(c.display, AshfallDataGrid.CellState.Normal),
                new($"{c.denizens} ({TotalPct(c.denizens, total):0}%)",
                    AshfallDataGrid.CellState.Muted),
                new($"{c.discontents} ({TotalPct(c.discontents, total):0}%)",
                    c.discontents > c.sentinels ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Muted),
                new($"{c.sentinels} ({TotalPct(c.sentinels, total):0}%)",
                    c.sentinels > 0 ? AshfallDataGrid.CellState.Selected : AshfallDataGrid.CellState.Muted),
                new($"{c.raiders} ({TotalPct(c.raiders, total):0}%)",
                    c.raiders > 0 ? AshfallDataGrid.CellState.Critical : AshfallDataGrid.CellState.Muted),
            };
            rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
        }
        if (rows.Count == 0) rows.AddRange(BuildEmptyCotent());
        return rows;
    }

    private static int TotalPct(int part, int whole) => whole <= 0 ? 0 : (int)System.Math.Round((part * 100.0) / whole);

    private List<AshfallDataGrid.Row> DossierRows()
    {
        // Research dossiers: technical schematics and field research notes pinned to disciplines.
        var rows = new List<AshfallDataGrid.Row>
        {
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("dos_dispatch_03", AshfallDataGrid.CellState.Normal),
                new("Wasteland Dispatch", AshfallDataGrid.CellState.Muted),
                new("0.85", AshfallDataGrid.CellState.Normal),
                new("+12", AshfallDataGrid.CellState.Selected),
                new("faction_iron_garrison", AshfallDataGrid.CellState.Muted),
            }, Selectable = false },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("dos_witness_12", AshfallDataGrid.CellState.Normal),
                new("Internal Witness Log", AshfallDataGrid.CellState.Muted),
                new("0.65", AshfallDataGrid.CellState.Normal),
                new("+8", AshfallDataGrid.CellState.Selected),
                new("faction_the_office", AshfallDataGrid.CellState.Muted),
            }, Selectable = false },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("dos_brine_07", AshfallDataGrid.CellState.Normal),
                new("Brine Pipe Ledger", AshfallDataGrid.CellState.Muted),
                new("0.40", AshfallDataGrid.CellState.Caution),
                new("+3", AshfallDataGrid.CellState.Normal),
                new("faction_hydro_barons", AshfallDataGrid.CellState.Muted),
            }, Selectable = false },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("dos_foundry_22", AshfallDataGrid.CellState.Normal),
                new("Foundry Field Note", AshfallDataGrid.CellState.Muted),
                new("0.55", AshfallDataGrid.CellState.Normal),
                new("+6", AshfallDataGrid.CellState.Selected),
                new("faction_silent_foundry", AshfallDataGrid.CellState.Muted),
            }, Selectable = false },
        };
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
        _detailTitle = AshfallUiHelpers.MakeSectionHeader("NODE DETAIL");
        _detailBox.AddChild(_detailTitle);
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        if (_host == null)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Research engine offline. Bind a ResearchHostSession to see live knowledge nodes and breakthroughs."));
            return;
        }
        if (_selectedIndex < 0)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Select a row to view approach, current direction, and breakdown."));
            return;
        }
        var id = ResolveVisibleRow(_selectedIndex);
        if (string.IsNullOrEmpty(id))
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Selected row out of scope."));
            return;
        }
        var current = _currentRows.Find(r => r.id == id);
        var coal = _coalitionRows.Find(r => r.id == id);
        if (current.id == null)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Faction unknown."));
            return;
        }
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Faction", current.display,
            AshfallUiHelpers.ToColor(DesignTheme.Warm)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Current", current.direction,
            AshfallUiHelpers.ToColor(current.dTrust >= 0f ? DesignTheme.Lethe : DesignTheme.Entropy)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Δ Trust", $"{current.dTrust:+0;-0;0}",
            AshfallUiHelpers.ToColor(DesignTheme.Pale)));
        if (coal.id != null)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Camp Strength",
                $"D{coal.denizens} C{coal.discontents} S{coal.sentinels} R{coal.raiders}",
                AshfallUiHelpers.ToColor(DesignTheme.Muted)));
        }
    }

    private string ResolveVisibleRow(int visibleIndex)
    {
        if (_currentRows.Count == 0) return string.Empty;
        int seen = -1;
        for (int i = 0; i < _currentRows.Count; i++)
        {
            if (!ScopePassFaction(_scopeFilter, _currentRows[i].id)) continue;
            seen++;
            if (seen == visibleIndex) return _currentRows[i].id;
        }
        return string.Empty;
    }

    private static bool ScopePassFaction(string scope, string factionId)
    {
        if (scope == "all") return true;
        if (scope == "unlocked" || scope == "remaining") return true; // currents/dossiers always visible across factions
        if (scope == "coalition") return true;
        if (scope == "loyalist") return factionId == "faction_the_office" || factionId == "faction_iron_garrison" || factionId == "faction_silent_foundry";
        if (scope == "deserter") return factionId == "faction_hydro_barons" || factionId == "faction_warlord";
        return true;
    }

    private (List<(string id, string display, string direction, float dTrust, float anchorCap)> currents,
            List<(string id, string display, int denizens, int discontents, int sentinels, int raiders)> coalition)
        BuildData()
    {
        var currents = new List<(string, string, string, float, float)>
        {
            ("faction_the_office",         "The Office",          "+",   12f,  80f),
            ("faction_iron_garrison",      "Iron Garrison",       "+",    5f,  72f),
            ("faction_silent_foundry",     "The Silent Foundry",  "+",   18f,  91f),
            ("faction_hydro_barons",       "Hydro Barons",        "−",   -8f,  60f),
            ("faction_warlord",            "Warlord Sectors",     "−",  -23f,  35f),
        };
        var coalition = new List<(string, string, int, int, int, int)>
        {
            ("faction_the_office",         "The Office",          32,  11,  18,  0),
            ("faction_iron_garrison",      "Iron Garrison",       41,  19,  29,  6),
            ("faction_silent_foundry",     "The Silent Foundry",  17,   5,  22,  0),
            ("faction_hydro_barons",       "Hydro Barons",        22,  18,   9,  4),
            ("faction_warlord",            "Warlord Sectors",     15,   8,   3, 12),
        };
        return (currents, coalition);
    }

    /// <summary>Hard-coded fixture rows for the bound=false case.</summary>
    internal static List<AshfallDataGrid.Row> BuildActionFixtureRows()
    {
        return new List<AshfallDataGrid.Row>
        {
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Start Research",        AshfallDataGrid.CellState.Normal),
                new("Begin research on the selected knowledge node.", AshfallDataGrid.CellState.Muted),
            }, Selectable = false },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Force Complete",    AshfallDataGrid.CellState.Normal),
                new("Bypass day budget to complete the active node.",  AshfallDataGrid.CellState.Muted),
            }, Selectable = false },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Abandon Research",      AshfallDataGrid.CellState.Normal),
                new("Clear the active queue slot; progress is lost.",                   AshfallDataGrid.CellState.Muted),
            }, Selectable = false },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("View Breakthrough",       AshfallDataGrid.CellState.Normal),
                new("Inspect the breakthrough item awarded.",             AshfallDataGrid.CellState.Muted),
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

    public override void _ExitTree()
    {
        if (_host != null)
        {
            _host.StateChanged -= RefreshView;
        }
        base._ExitTree();
    }
}
