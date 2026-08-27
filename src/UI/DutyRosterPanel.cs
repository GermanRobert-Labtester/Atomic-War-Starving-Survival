using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Duty Roster Dashboard (Stitch #22 / Tier-A3 shift half / Phase 20).
///
/// Phase 20 prioritised lift on the shift half of the Duty Master matrix:
/// the source panel from Phase 9 had bespoke card UI; the new surface reads
/// through the same five Phase 11/12 primitives used by FactionMatrix,
/// DoseLedger, Greenhouse, SilentFoundry, ExpeditionRadar, and SkillMatrix.
///
/// Six cards on the status rail and six columns on the data grid. The
/// filter sidebar selects scope (all roles / one specific role / sick-list).
///
/// Pure presentation. Reads only from `Ashfall.Core.DutyRosterSystem`.
/// </summary>
public partial class DutyRosterPanel : Control, IBindablePanel
{
    public event Action? OnClose;
    public event Action? OnAssignmentChanged;
    public event Action<string>? OnRoleSelected;

    /// <summary>Legacy Phase 9 contract: an additional drill-down event the host wires to its detail panel.</summary>
    public event Action? OnDetailsRequested;

    /// <summary>Legacy Phase 9 contract: returns true when the read-model status strip rendered text (UI test surface).</summary>
    public bool StatusStripNonEmpty() => !string.IsNullOrEmpty(_host?.LastEvent);

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _rosterGrid;
    private VBoxContainer _detailBox = null!;
    private VBoxContainer _detailContent = null!;
    private Label _detailTitle = null!;
    private int _selectedIndex = -1;
    private string _scopeFilter = "all"; // all | night_watch | mess | hatch_opener | intake_sleeper | expedition | unassigned

    private DutyRosterHostSession? _host;
    private SurvivorsHostSession? _survivors;

    public bool IsBound => _host != null;

    public void Bind(DutyRosterHostSession host, SurvivorsHostSession? survivors = null)
    {
        Unbind();
        _host = host;
        _survivors = survivors;
        if (_host != null)
        {
            _host.Roster.OnRosterUpdated += RefreshView;
            _host.Roster.OnNameWritten += HandleSurvivorNameChanged;
            _host.Roster.OnNameErased += HandleSurvivorNameChanged;
        }
        RefreshView();
    }

    public void Unbind()
    {
        if (_host != null)
        {
            _host.Roster.OnRosterUpdated -= RefreshView;
            _host.Roster.OnNameWritten -= HandleSurvivorNameChanged;
            _host.Roster.OnNameErased -= HandleSurvivorNameChanged;
            _host = null;
        }
        _survivors = null;
    }



    private void HandleSurvivorNameChanged(string _) => RefreshView();

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("Duty Roster // Shift Coverage & Sick-List", minWidth: 1100, minHeight: 720);
        SetContentRoot(_shell);

        var scopes = new[]
        {
            new AshfallSidebar.Item { Id = "all",             Label = "All Roles",         Hint = "every assigned shift",     IconPath = "" },
            new AshfallSidebar.Item { Id = "night_watch",     Label = "Night Watch",       Hint = "perimeter + storms",       IconPath = "" },
            new AshfallSidebar.Item { Id = "mess",            Label = "Mess & Rations",    Hint = "ration efficiency",       IconPath = "" },
            new AshfallSidebar.Item { Id = "hatch_opener",    Label = "Hatch Defense",     Hint = "airlock protocols",       IconPath = "" },
            new AshfallSidebar.Item { Id = "intake_sleeper",  Label = "Intake Filtration", Hint = "HEPA filter savings",     IconPath = "" },
            new AshfallSidebar.Item { Id = "expedition",      Label = "Scavenging Sortie", Hint = "sortie readiness",        IconPath = "" },
            new AshfallSidebar.Item { Id = "unassigned",      Label = "Unassigned",        Hint = "roster entries w/o role", IconPath = "" },
        };
        _sidebar = _shell.SetSidebar(scopes, "Role Filter", "all");
        _sidebar.OnSelected += HandleSidebar;

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("assigned",    "Assigned",    "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("unassigned",  "Unassigned",  "—", AshfallMetricCard.Criticality.Caution, minWidth: 120);
        _statusRail.AddCard("sick",        "Sick-List",   "—", AshfallMetricCard.Criticality.Warn,   minWidth: 100);
        _statusRail.AddCard("rotation",    "Rotation",    "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
        _statusRail.AddCard("alerts",      "Alerts",      "—", AshfallMetricCard.Criticality.Critical, minWidth: 100);
        _statusRail.AddCard("today_day",   "Today",       "—", AshfallMetricCard.Criticality.Normal, minWidth: 90);

        // DataGrid columns: role | survivor | status | lastSlept | occupation | duration
        var cols = new[]
        {
            new AshfallDataGrid.Column { Header = "Role",        MinWidth = 200, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Survivor",    MinWidth = 180, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Status",      MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Occupation",  MinWidth = 140, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Last Slept",  MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Shift",       MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
        };
        _rosterGrid = new AshfallDataGrid(cols, showHeader: true, minWidth: 720, minHeight: 320);
        _rosterGrid.OnRowSelected += HandleRowSelected;

        var body = new HBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_rosterGrid);

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.CustomMinimumSize = new Vector2(280, 320);
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_detailBox);

        _detailTitle = AshfallUiHelpers.MakeSectionHeader("SHIFT DETAIL");
        _detailTitle.HorizontalAlignment = HorizontalAlignment.Left;
        _detailBox.AddChild(_detailTitle);
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());

        // _detailContent is the transient child container. RefreshDetail calls
        // EmptyChildren(_detailContent) every pass; the persistent header above
        // stays alive across rebuilds so RefreshDetail can keep mutating
        // _detailTitle.Text without exposing dangling references.
        _detailContent = new VBoxContainer();
        _detailContent.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailContent.SizeFlagsVertical = SizeFlags.ExpandFill;
        _detailBox.AddChild(_detailContent);
        _detailContent.AddChild(AshfallUiHelpers.MakeMetadata(
            "Select a row to view role, survivor, occupation, and last-slept day."));

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
        _scopeFilter = id ?? "all";
        _selectedIndex = -1;
        RefreshView();
    }

    private void HandleRowSelected(int idx)
    {
        _selectedIndex = idx;
        var (roleId, survivorId) = ResolveVisibleRow(idx);
        if (!string.IsNullOrEmpty(roleId))
            OnRoleSelected?.Invoke(roleId);
        OnAssignmentChanged?.Invoke();
        OnDetailsRequested?.Invoke();
        RefreshDetail();
    }

    public void RefreshView()
    {
        RefreshStatusRail();
        BuildRosterRows();
        RefreshDetail();
    }

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        if (_host == null)
        {
            _statusRail.Set("assigned",   "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("unassigned", "—", AshfallMetricCard.Criticality.Caution);
            _statusRail.Set("sick",       "—", AshfallMetricCard.Criticality.Warn);
            _statusRail.Set("rotation",   "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("alerts",     "—", AshfallMetricCard.Criticality.Critical);
            _statusRail.Set("today_day",  "—", AshfallMetricCard.Criticality.Normal);
            return;
        }

        int assigned = 0, unassigned = 0, sick = 0, rotation = 0, alerts = 0;
        var state = _host.Roster.State;

        // Count rows + assignments.
        for (int i = 0; i < state.assignments.Count; i++)
        {
            var a = state.assignments[i];
            if (a == null || string.IsNullOrEmpty(a.role)) continue;
            if (!string.IsNullOrEmpty(a.survivorId)) assigned++;
        }
        for (int i = 0; i < state.rows.Count; i++)
        {
            var r = state.rows[i];
            if (r == null) continue;
            bool roleAssigned = !string.IsNullOrEmpty(_host.Roster.GetRoleOf(r.survivorId));
            if (!roleAssigned) unassigned++;
            if (LooksSick(r.status)) sick++;
            // Rotation heuristic: any row with lastSleptDay == -1 is "first shift"
            if (r.lastSleptDay < 0) rotation++;
        }

        // Alerts: missing role + script burned = alert.
        if (!string.IsNullOrEmpty(state.endingId) || state.mutationRosterBurned) alerts++;
        if (state.assignments.Count == 0 && state.rows.Count > 0) alerts++;

        int todayDay = state.lastMorningDay >= 0 ? state.lastMorningDay : 0;

        _statusRail.Set("assigned",   $"{assigned}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("unassigned", $"{unassigned}",
            unassigned > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("sick",       $"{sick}",
            sick > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("rotation",   $"{rotation}",
            rotation > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("alerts",     $"{alerts}",
            alerts > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("today_day",  todayDay > 0 ? $"D{todayDay}" : "—",
            AshfallMetricCard.Criticality.Normal);
    }

    private static bool LooksSick(string status)
    {
        if (string.IsNullOrEmpty(status)) return false;
        return status.IndexOf("sick", StringComparison.OrdinalIgnoreCase) >= 0
            || status.IndexOf("acute", StringComparison.OrdinalIgnoreCase) >= 0
            || status.IndexOf("ill", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private void BuildRosterRows()
    {
        if (_rosterGrid == null) return;

        if (_host == null)
        {
            _rosterGrid.SetRows(BuildFixtureRows());
            return;
        }

        var rows = new List<AshfallDataGrid.Row>();
        var state = _host.Roster.State;
        // Build a row per assigned role (one per DutyRosterAssignmentEntry).
        // Plus a row per roster entry that has no role ("unassigned" filter).
        for (int i = 0; i < state.assignments.Count; i++)
        {
            var a = state.assignments[i];
            if (a == null) continue;
            if (!ScopePass(a.role)) continue;
            var occupant = FindRow(a.survivorId);
            string survivorName = occupant != null && !string.IsNullOrEmpty(occupant.displayName)
                ? occupant.displayName
                : (string.IsNullOrEmpty(a.survivorId) ? "[UNASSIGNED]" : FormatSurvivorName(a.survivorId));
            string status = occupant?.status ?? "—";
            string occupation = occupant?.occupationObserved ?? "Resident";
            int lastSlept = occupant?.lastSleptDay ?? -1;
            string shiftText = ResolveShiftText(a.role);

            var cells = new List<AshfallDataGrid.Cell>
            {
                new(RoleTitle(a.role), AshfallDataGrid.CellState.Normal),
                new(survivorName, AshfallDataGrid.CellState.Normal),
                new(status, StatusState(status)),
                new(occupation, AshfallDataGrid.CellState.Muted),
                new(lastSlept < 0 ? "—" : $"D{lastSlept}", AshfallDataGrid.CellState.Normal),
                new(shiftText, AshfallDataGrid.CellState.Muted),
            };
            rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
        }

        // Unassigned-filter: surface roster rows that have no role.
        if (_scopeFilter == "unassigned")
        {
            for (int i = 0; i < state.rows.Count; i++)
            {
                var r = state.rows[i];
                if (r == null) continue;
                if (!string.IsNullOrEmpty(_host.Roster.GetRoleOf(r.survivorId))) continue;

                var cells = new List<AshfallDataGrid.Cell>
                {
                    new("[UNASSIGNED]", AshfallDataGrid.CellState.Muted),
                    new(r.displayName ?? FormatSurvivorName(r.survivorId), AshfallDataGrid.CellState.Normal),
                    new(r.status ?? "—", StatusState(r.status ?? "—")),
                    new(r.occupationObserved ?? "Resident", AshfallDataGrid.CellState.Muted),
                    new(r.lastSleptDay < 0 ? "—" : $"D{r.lastSleptDay}", AshfallDataGrid.CellState.Normal),
                    new("—", AshfallDataGrid.CellState.Muted),
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
                    new("— no assignments match filter —", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                }
            });
        }
        _rosterGrid.SetRows(rows);
    }

    private bool ScopePass(string roleId)
    {
        if (_scopeFilter == "all") return true;
        if (_scopeFilter == "unassigned") return false; // handled by separate branch
        return _scopeFilter == roleId;
    }

    private DutyRosterRow? FindRow(string survivorId)
    {
        if (_host == null || string.IsNullOrEmpty(survivorId)) return null;
        var state = _host.Roster.State;
        for (int i = 0; i < state.rows.Count; i++)
        {
            var r = state.rows[i];
            if (r != null && r.survivorId == survivorId) return r;
        }
        return null;
    }

    private (string roleId, string survivorId) ResolveVisibleRow(int visibleIndex)
    {
        if (_host == null) return (string.Empty, string.Empty);
        int seen = -1;
        var state = _host.Roster.State;
        for (int i = 0; i < state.assignments.Count; i++)
        {
            var a = state.assignments[i];
            if (a == null) continue;
            if (!ScopePass(a.role)) continue;
            seen++;
            if (seen == visibleIndex) return (a.role ?? string.Empty, a.survivorId ?? string.Empty);
        }
        if (_scopeFilter == "unassigned")
        {
            for (int i = 0; i < state.rows.Count; i++)
            {
                var r = state.rows[i];
                if (r == null) continue;
                if (!string.IsNullOrEmpty(_host.Roster.GetRoleOf(r.survivorId))) continue;
                seen++;
                if (seen == visibleIndex) return (string.Empty, r.survivorId ?? string.Empty);
            }
        }
        return (string.Empty, string.Empty);
    }

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        AshfallUiHelpers.EmptyChildren(_detailContent);
        if (_host == null)
        {
            _detailTitle.Text = "SHIFT DETAIL";
            _detailContent.AddChild(AshfallUiHelpers.MakeMetadata(
                "Duty Roster engine offline. Bind a DutyRosterHostSession to see live shift assignments."));
            return;
        }
        if (_selectedIndex < 0)
        {
            _detailTitle.Text = "SHIFT DETAIL";
            _detailContent.AddChild(AshfallUiHelpers.MakeMetadata(
                "Select a roster row to view role, survivor, occupation, and last-slept day."));
            return;
        }
        var (roleId, survivorId) = ResolveVisibleRow(_selectedIndex);
        if (string.IsNullOrEmpty(roleId) && string.IsNullOrEmpty(survivorId))
        {
            _detailTitle.Text = "SHIFT DETAIL";
            _detailContent.AddChild(AshfallUiHelpers.MakeMetadata("Selected row is out of scope."));
            return;
        }

        var row = FindRow(survivorId);
        _detailTitle.Text = string.IsNullOrEmpty(roleId) ? "[UNASSIGNED] DETAIL" : $"{RoleTitle(roleId)} DETAIL";
        _detailContent.AddChild(AshfallUiHelpers.MakeDataRow("Role", string.IsNullOrEmpty(roleId) ? "[UNASSIGNED]" : RoleTitle(roleId),
            AshfallUiHelpers.ToColor(DesignTheme.Warm)));
        _detailContent.AddChild(AshfallUiHelpers.MakeDataRow("Survivor", row?.displayName ?? FormatSurvivorName(survivorId),
            AshfallUiHelpers.ToColor(DesignTheme.Pale)));
        _detailContent.AddChild(AshfallUiHelpers.MakeDataRow("Status", row?.status ?? "—",
            row == null ? AshfallUiHelpers.ToColor(DesignTheme.Dim) :
            StatusState(row.status) == AshfallDataGrid.CellState.Critical ? AshfallUiHelpers.ToColor(DesignTheme.Critical) :
            StatusState(row.status) == AshfallDataGrid.CellState.Warning  ? AshfallUiHelpers.ToColor(DesignTheme.Entropy) :
            AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailContent.AddChild(AshfallUiHelpers.MakeDataRow("Occupation", row?.occupationObserved ?? "—",
            AshfallUiHelpers.ToColor(DesignTheme.Muted)));
        _detailContent.AddChild(AshfallUiHelpers.MakeDataRow("Last Slept", (row?.lastSleptDay ?? -1) < 0 ? "—" : $"D{row!.lastSleptDay}",
            AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailContent.AddChild(AshfallUiHelpers.MakeDataRow("Shift", string.IsNullOrEmpty(roleId) ? "—" : ResolveShiftText(roleId),
            AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
    }

    private static string RoleTitle(string roleId) => roleId switch
    {
        DutyRosterIds.RoleNightWatch    => "Night Watch",
        DutyRosterIds.RoleMess          => "Mess & Rations",
        DutyRosterIds.RoleHatchOpener   => "Hatch Defense",
        DutyRosterIds.RoleIntakeSleeper => "Intake Filtration",
        DutyRosterIds.RoleExpedition     => "Scavenging Sortie",
        _ => roleId,
    };

    private static string ResolveShiftText(string roleId) => roleId switch
    {
        DutyRosterIds.RoleNightWatch    => "00:00–04:00",
        DutyRosterIds.RoleMess          => "06:00–10:00",
        DutyRosterIds.RoleHatchOpener   => "12:00–14:00",
        DutyRosterIds.RoleIntakeSleeper => "20:00–00:00",
        DutyRosterIds.RoleExpedition     => "04:00–20:00",
        _ => "—",
    };

    private static AshfallDataGrid.CellState StatusState(string status)
    {
        if (LooksSick(status)) return AshfallDataGrid.CellState.Critical;
        if (string.IsNullOrEmpty(status)) return AshfallDataGrid.CellState.Muted;
        return AshfallDataGrid.CellState.Normal;
    }

    private static string FormatSurvivorName(string id)
    {
        if (string.IsNullOrEmpty(id)) return "[UNNAMED]";
        return id switch
        {
            "survivor_dr_sarah_chen" or "survivor_sarah_chen" => "Dr. Sarah Chen",
            "survivor_gunner_mikhail" or "survivor_mikhail_volkov" => "Gunner Mikhail",
            "elena_vasquez" or "survivor_elena_vasquez" => "Elena Vasquez",
            _ => id.Replace("survivor_", "").Replace("_", " ").ToUpperInvariant()
        };
    }

    /// <summary>Hard-coded fixture rows for the bound=false case. Each row uses a canonical role id + a canonical roster survivor.</summary>
    internal static List<AshfallDataGrid.Row> BuildFixtureRows()
    {
        var rows = new List<AshfallDataGrid.Row>
        {
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("Intake Filtration", AshfallDataGrid.CellState.Normal),
                    new("Dr. Sarah Chen",     AshfallDataGrid.CellState.Normal),
                    new("OK",                  AshfallDataGrid.CellState.Normal),
                    new("Trauma Surgeon",     AshfallDataGrid.CellState.Muted),
                    new("D14",                 AshfallDataGrid.CellState.Normal),
                    new("20:00–00:00",         AshfallDataGrid.CellState.Muted),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("Night Watch",         AshfallDataGrid.CellState.Normal),
                    new("Gunner Mikhail",     AshfallDataGrid.CellState.Normal),
                    new("ACUTE RAD SICK",      AshfallDataGrid.CellState.Critical),
                    new("Heavy Artillery",    AshfallDataGrid.CellState.Muted),
                    new("D9",                  AshfallDataGrid.CellState.Normal),
                    new("00:00–04:00",         AshfallDataGrid.CellState.Muted),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("Mess & Rations",      AshfallDataGrid.CellState.Normal),
                    new("Elena Vasquez",      AshfallDataGrid.CellState.Normal),
                    new("OK",                  AshfallDataGrid.CellState.Normal),
                    new("Aridoculture",        AshfallDataGrid.CellState.Muted),
                    new("D14",                 AshfallDataGrid.CellState.Normal),
                    new("06:00–10:00",         AshfallDataGrid.CellState.Muted),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("Hatch Defense",       AshfallDataGrid.CellState.Normal),
                    new("Marcus Olenik",      AshfallDataGrid.CellState.Normal),
                    new("CHRONIC ILL",         AshfallDataGrid.CellState.Critical),
                    new("Watchman",            AshfallDataGrid.CellState.Muted),
                    new("D7",                  AshfallDataGrid.CellState.Normal),
                    new("12:00–14:00",         AshfallDataGrid.CellState.Muted),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("Scavenging Sortie",   AshfallDataGrid.CellState.Normal),
                    new("[UNASSIGNED]",        AshfallDataGrid.CellState.Caution),
                    new("—",                   AshfallDataGrid.CellState.Muted),
                    new("—",                   AshfallDataGrid.CellState.Muted),
                    new("—",                   AshfallDataGrid.CellState.Muted),
                    new("04:00–20:00",         AshfallDataGrid.CellState.Muted),
                }, Selectable = true },
        };
        return rows;
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
            Unbind();
            base._ExitTree();
        }
}
