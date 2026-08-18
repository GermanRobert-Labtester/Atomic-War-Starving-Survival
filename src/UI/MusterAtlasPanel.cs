using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — The Muster (Expansion 06) Dashboard. Tier-3 HYBRID sub-card
/// sibling of the legacy Phase-9 modal `MusterPanel.cs`.
///
/// Reads the user's own `MusterSystem` (Core) through `MusterHostSession`.
/// Four surfaces:
///   1. Sector Currents    — push/pop trust momentum per faction
///   2. Coalition Camps    — denizens / discontents / sentinels / raiders per faction
///   3. Witness Dossiers   — DosId / type / weight / impact / target
///   4. Action Bar         — Muster Vote / Schedule Muster / Recall Action
///
/// Plus six-card status rail and right-side detail inspector.
/// </summary>
public partial class MusterAtlasPanel : Control
{
    public event Action? OnClose;
    public event Action<string>? OnFactionSelected;

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
    private string _scopeFilter = "all"; // all | currents | coalition | dossiers | loyalist | deserter

    private MusterHostSession? _host;

    public bool IsBound => _host != null;

    public void Bind(MusterHostSession host)
    {
        _host = host;
        if (_host != null)
            _host.StateChanged += RefreshView;
        RefreshView();
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("The Muster // Sector Currents & Coalition Camps", minWidth: 1280, minHeight: 720);
        SetContentRoot(_shell);

        var scopes = new[]
        {
            new AshfallSidebar.Item { Id = "all",       Label = "All Muster",     Hint = "every faction",                  IconPath = "" },
            new AshfallSidebar.Item { Id = "currents",  Label = "Sector Currents", Hint = "trust momentum",                IconPath = "" },
            new AshfallSidebar.Item { Id = "coalition", Label = "Coalition Camps", Hint = "denizens + sentinels",          IconPath = "" },
            new AshfallSidebar.Item { Id = "dossiers",  Label = "Witness Dossiers", Hint = "evidence weight + impact",      IconPath = "" },
            new AshfallSidebar.Item { Id = "loyalist",  Label = "Loyalist Cap",    Hint = "approaches the muster",          IconPath = "" },
            new AshfallSidebar.Item { Id = "deserter",  Label = "Deserter Cap",   Hint = "defection approaches",           IconPath = "" },
        };
        _sidebar = _shell.SetSidebar(scopes, "Muster Filter", "all");
        _sidebar.OnSelected += HandleSidebar;

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("factions",  "Factions",     "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("currents",  "Currents",     "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("denizens",  "Denizens",     "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
        _statusRail.AddCard("sentinels", "Sentinels",    "—", AshfallMetricCard.Criticality.Caution, minWidth: 110);
        _statusRail.AddCard("dossiers",  "Witness Dossiers","—", AshfallMetricCard.Criticality.Warn, minWidth: 130);
        _statusRail.AddCard("approach",  "Approach",     "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);

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
            new AshfallDataGrid.Column { Header = "Denizens",   MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Discontents",MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Sentinels",  MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
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
        currentsCol.AddChild(AshfallUiHelpers.MakeSectionHeader("Sector Currents"));
        currentsCol.AddChild(_currentsGrid);
        topRow.AddChild(currentsCol);

        var coalitionCol = new VBoxContainer();
        coalitionCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        coalitionCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        coalitionCol.AddChild(AshfallUiHelpers.MakeSectionHeader("Coalition Camps"));
        coalitionCol.AddChild(_coalitionGrid);
        topRow.AddChild(coalitionCol);

        body.AddChild(topRow);
        body.AddChild(AshfallUiHelpers.MakeSeparator());

        var dossierCol = new VBoxContainer();
        dossierCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        dossierCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        dossierCol.AddChild(AshfallUiHelpers.MakeSectionHeader("Witness Dossiers"));
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
        actionCol.AddChild(AshfallUiHelpers.MakeSectionHeader("Muster Action Bar"));
        actionCol.AddChild(_actionBarGrid);
        actionRow.AddChild(actionCol);

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.CustomMinimumSize = new Vector2(280, 200);
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("MUSTER DETAIL"));
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
            "Bind a MusterHostSession to see live currents, coalition camp composition, and witness dossier weight."));
        actionRow.AddChild(_detailBox);

        body.AddChild(actionRow);
        _shell.SetContent(body);
        _detailTitle = AshfallUiHelpers.MakeSectionHeader("MUSTER DETAIL");
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
            OnFactionSelected?.Invoke(factionId);
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
            _statusRail.Set("factions",  "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("currents",  "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("denizens",  "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("sentinels", "—", AshfallMetricCard.Criticality.Caution);
            _statusRail.Set("dossiers",  "—", AshfallMetricCard.Criticality.Warn);
            _statusRail.Set("approach",  "—", AshfallMetricCard.Criticality.Normal);
            return;
        }
        // Note: MusterSystem currently exposes a single MusterRecord; the
        // count surface will track the size of the sector-tracking set
        // once the engine extends its read surface. Until then we surface
        // the mixed bound=true / seed-1401 narrative line.
        _statusRail.Set("factions",  "5", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("currents",  "5", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("denizens",  "127", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("sentinels", "44", AshfallMetricCard.Criticality.Caution);
        _statusRail.Set("dossiers",  "12", AshfallMetricCard.Criticality.Warn);
        _statusRail.Set("approach", _host.Engine.SelectedApproach?.ToString() ?? "—",
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
        _coalitionGrid.SetRows(_scopeFilter == "currents" ? BuildEmptyCotent() : CoalitionRows());
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
        // Witness dossiers: author-level pieces of evidence pinned to subsectors
        // and factions. Their weight influences the muster approach choice.
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
        while (_detailBox.GetChildCount() > 0)
        {
            var c = _detailBox.GetChild(0);
            _detailBox.RemoveChild(c);
            c.QueueFree();
        }
        _detailTitle = AshfallUiHelpers.MakeSectionHeader("MUSTER DETAIL");
        _detailBox.AddChild(_detailTitle);
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        if (_host == null)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Muster engine offline. Bind a MusterHostSession to see live currents and coalition."));
            return;
        }
        if (_selectedIndex < 0)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Select a faction row to view approach, current direction, and coalition breakdown."));
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
        if (scope == "currents" || scope == "dossiers") return true; // currents/dossiers always visible across factions
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
                new("Muster Vote",        AshfallDataGrid.CellState.Normal),
                new("Roll the desertion threshold · select faction card", AshfallDataGrid.CellState.Muted),
            }, Selectable = false },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Schedule Muster",    AshfallDataGrid.CellState.Normal),
                new("Set the muster date · it determines desertion turn",  AshfallDataGrid.CellState.Muted),
            }, Selectable = false },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Recall Action",      AshfallDataGrid.CellState.Normal),
                new("Re-muster before coalition breaks",                   AshfallDataGrid.CellState.Muted),
            }, Selectable = false },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Sign Witness",       AshfallDataGrid.CellState.Normal),
                new("Promote a dossier into the muster vote",             AshfallDataGrid.CellState.Muted),
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
