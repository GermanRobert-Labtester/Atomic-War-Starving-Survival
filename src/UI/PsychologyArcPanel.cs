// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Psychology Arc Panel (Plan 164).
/// Observation + intervention surface for breakdown arcs: stages, treatment
/// progress, work eligibility, and private-stash search/return. Therapy itself
/// is administered by the sanatorium — this panel never invents treatment.
/// </summary>
public partial class PsychologyArcPanel : Control
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
    private string _filter = "all";
    private readonly List<string> _rowSurvivorIds = new List<string>();

    private PsychologyArcHostSession? _host;
    private Func<NeedsSystem?>? _needs;

    public bool IsBound => _host != null;

    public void Bind(PsychologyArcHostSession session, Func<NeedsSystem?>? needsProvider)
    {
        if (_host != null)
            _host.StateChanged -= RefreshView;
        _host = session;
        _needs = needsProvider;
        if (_host != null)
            _host.StateChanged += RefreshView;
        RefreshView();
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("Psychological Arcs // Watch", minWidth: 1100, minHeight: 700);
        AddChild(_shell);
        _shell.SetAnchorsPreset(LayoutPreset.FullRect);
        _shell.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        _shell.SizeFlagsVertical = SizeFlags.ExpandFill;

        var sidebarItems = new[]
        {
            new AshfallSidebar.Item { Id = "all",        Label = "Everyone",   Hint = "the whole roster", IconPath = "" },
            new AshfallSidebar.Item { Id = "active",     Label = "Active Arc", Hint = "crisis in progress", IconPath = "" },
            new AshfallSidebar.Item { Id = "crisis",     Label = "Crisis",     Hint = "highest stage", IconPath = "" },
            new AshfallSidebar.Item { Id = "recovering", Label = "Recovering", Hint = "in treatment", IconPath = "" },
        };
        _sidebar = _shell.SetSidebar(sidebarItems, "Filter", "all");
        _sidebar.OnSelected += id => { _filter = id; _selectedIndex = -1; RefreshView(); };

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("active", "Active Arcs", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("crisis", "In Crisis", "—", AshfallMetricCard.Criticality.Caution, minWidth: 100);
        _statusRail.AddCard("gated", "Work-Gated", "—", AshfallMetricCard.Criticality.Warn, minWidth: 110);
        _statusRail.AddCard("stash", "Hidden Stashes", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);

        var cols = new[]
        {
            new AshfallDataGrid.Column { Header = "Survivor",  MinWidth = 140, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Arc",       MinWidth = 170, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Stage",     MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Exposure",  MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Treatment", MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Work",      MinWidth = 80,  Alignment = AshfallDataGrid.ColumnAlign.Left },
        };
        _grid = new AshfallDataGrid(cols, showHeader: true, minWidth: 720, minHeight: 300);
        _grid.OnRowSelected += idx =>
        {
            _selectedIndex = idx;
            RefreshDetail();
        };

        var body = new HBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_grid);

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.CustomMinimumSize = new Vector2(340, 300);
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_detailBox);

        _shell.SetContent(body);
        RefreshView();
    }

    public void RefreshView()
    {
        RefreshStatusRail();
        BuildGridRows();
        RefreshDetail();
    }

    private List<SurvivorArcState> Roster()
    {
        var list = new List<SurvivorArcState>();
        if (_host == null || _needs == null) return list;
        var needs = _needs();
        if (needs == null) return list;
        foreach (var n in needs.Registered)
        {
            if (n == null) continue;
            var s = _host.System.State.survivors.Find(x =>
                x != null && string.Equals(x.survivor_id, n.Id, StringComparison.Ordinal));
            list.Add(s ?? new SurvivorArcState { survivor_id = n.Id });
        }
        return list;
    }

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        if (_host == null)
        {
            _statusRail.Set("active", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("crisis", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("gated", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("stash", "—", AshfallMetricCard.Criticality.Normal);
            return;
        }
        int active = 0, crisis = 0, gated = 0, hiddenStash = 0;
        foreach (var s in Roster())
        {
            if (!string.IsNullOrEmpty(s.arc_id)) active++;
            if ((ArcStage)s.stage == ArcStage.Crisis) crisis++;
            if (!_host.System.IsEligibleForWork(s.survivor_id)) gated++;
            foreach (var e in s.stash)
                if (e != null && !e.discovered) hiddenStash++;
        }
        _statusRail.Set("active", $"{active}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("crisis", $"{crisis}", crisis > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("gated", $"{gated}", gated > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("stash", hiddenStash > 0 ? $"{hiddenStash} item(s) unaccounted" : "none",
            hiddenStash > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
    }

    private void BuildGridRows()
    {
        if (_grid == null) return;
        _rowSurvivorIds.Clear();
        if (_host == null)
        {
            _grid.SetRows(new List<AshfallDataGrid.Row>());
            return;
        }

        var rows = new List<AshfallDataGrid.Row>();
        foreach (var s in Roster())
        {
            var stage = (ArcStage)s.stage;
            bool hasArc = !string.IsNullOrEmpty(s.arc_id);
            bool pass = _filter switch
            {
                "active" => hasArc,
                "crisis" => stage == ArcStage.Crisis,
                "recovering" => stage == ArcStage.Recovering,
                _ => true
            };
            if (!pass) continue;

            var arcDef = hasArc ? _host.System.Arc(s.arc_id) : null;
            var stageCell = stage == ArcStage.Crisis ? AshfallDataGrid.CellState.Critical
                : stage == ArcStage.Recovering ? AshfallDataGrid.CellState.Positive
                : hasArc ? AshfallDataGrid.CellState.Warning
                : AshfallDataGrid.CellState.Muted;

            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new(s.survivor_id, AshfallDataGrid.CellState.Normal),
                    new(arcDef?.display_name ?? "—", hasArc ? AshfallDataGrid.CellState.Normal : AshfallDataGrid.CellState.Muted),
                    new(hasArc ? stage.ToString() : "steady", stageCell),
                    new($"{s.exposure_days}", s.exposure_days >= 5 ? AshfallDataGrid.CellState.Caution : AshfallDataGrid.CellState.Normal),
                    new(hasArc ? $"{s.treatment_progress}/{PsychologicalArcSystem.TreatmentProgressToResolve}" : "—", AshfallDataGrid.CellState.Normal),
                    new(_host.System.IsEligibleForWork(s.survivor_id) ? "yes" : "GATED",
                        _host.System.IsEligibleForWork(s.survivor_id) ? AshfallDataGrid.CellState.Muted : AshfallDataGrid.CellState.Warning),
                },
                Selectable = true
            });
            _rowSurvivorIds.Add(s.survivor_id);
        }

        if (rows.Count == 0)
        {
            var empty = new List<AshfallDataGrid.Cell>();
            for (int i = 0; i < 6; i++) empty.Add(new AshfallDataGrid.Cell("—", AshfallDataGrid.CellState.Muted));
            empty[0] = new AshfallDataGrid.Cell("— no survivors match filter —", AshfallDataGrid.CellState.Muted);
            rows.Add(new AshfallDataGrid.Row { Cells = empty });
        }
        _grid.SetRows(rows);
    }

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        AshfallUiHelpers.EmptyChildren(_detailBox);
        _detailTitle = AshfallUiHelpers.MakeSectionHeader("SURVIVOR DETAIL");
        _detailTitle.HorizontalAlignment = HorizontalAlignment.Left;
        _detailBox.AddChild(_detailTitle);
        var sep = AshfallUiHelpers.MakeSeparator();
        sep.CustomMinimumSize = new Vector2(0, 2);
        _detailBox.AddChild(sep);

        if (_host == null)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("System offline. Bind a PsychologyArcHostSession."));
            return;
        }

        if (_selectedIndex < 0 || _selectedIndex >= _rowSurvivorIds.Count)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Select a survivor. Arc stages, treatment, and stashes show here."));
            return;
        }

        string id = _rowSurvivorIds[_selectedIndex];
        var sys = _host.System;
        _detailTitle.Text = $"SURVIVOR: {id}";
        var stage = sys.StageOf(id);
        var arcDef = sys.Arc(sys.State.survivors.Find(x => string.Equals(x.survivor_id, id, StringComparison.Ordinal))?.arc_id);

        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Arc", arcDef?.display_name ?? "none",
            arcDef != null ? AshfallUiHelpers.ToColor(DesignTheme.Entropy) : AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Stage", stage.ToString(),
            stage == ArcStage.Crisis ? AshfallUiHelpers.ToColor(DesignTheme.Critical) : AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Work",
            sys.IsEligibleForWork(id) ? "eligible" : "GATED (shutdown arc)",
            sys.IsEligibleForWork(id) ? AshfallUiHelpers.ToColor(DesignTheme.Dim) : AshfallUiHelpers.ToColor(DesignTheme.LetheAmber)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Resilience", $"{sys.ResilienceBonus(id) * 100f:F0}%",
            AshfallUiHelpers.ToColor(DesignTheme.Lethe)));

        // Treatment is administered by the sanatorium; this surface reports
        // progress and offers the stash intervention only.
        _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
            arcDef == null
                ? "No active arc. Sustained high stress (90+ morale) over many days is what starts one."
                : $"Treatment: sanatorium therapy. Progress {StateOf(id)?.treatment_progress ?? 0}/{PsychologicalArcSystem.TreatmentProgressToResolve}."));

        var stash = sys.StashOf(id);
        if (stash.Count > 0)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("STASH"));
            foreach (var e in stash)
                _detailBox.AddChild(AshfallUiHelpers.MakeSmall(
                    $"D{e.day}: {e.count}× {e.item_id} {(e.discovered ? "(found)" : "(hidden)")}"));
            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var searchBtn = AshfallUiHelpers.MakeButton("SEARCH BELONGINGS",
                () => OnActionRequested?.Invoke("SEARCH_STASH", id));
            searchBtn.CustomMinimumSize = new Vector2(170, 30);
            row.AddChild(searchBtn);
            bool anyDiscovered = false;
            foreach (var e in stash) if (e.discovered) { anyDiscovered = true; break; }
            var returnBtn = AshfallUiHelpers.MakeButton("RETURN TO STORES",
                () => OnActionRequested?.Invoke("RETURN_STASH", id),
                disabled: !anyDiscovered);
            returnBtn.CustomMinimumSize = new Vector2(160, 30);
            row.AddChild(returnBtn);
            _detailBox.AddChild(row);
        }

        if (!string.IsNullOrEmpty(_host.LastEvent))
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSmall(_host.LastEvent));
        }
    }

    private SurvivorArcState? StateOf(string id) =>
        _host!.System.State.survivors.Find(x => string.Equals(x.survivor_id, id, StringComparison.Ordinal));

    public void Open()
    {
        Visible = true;
        RefreshView();
        QueueRedraw();
    }

    public void Close()
    {
        Visible = false;
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
