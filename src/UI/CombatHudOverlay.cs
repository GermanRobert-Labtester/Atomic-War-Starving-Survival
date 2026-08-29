using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Combat HUD Overlay (Stitch #58, Phase 22, Tier-3).
///
/// Phase 22 ships a live combat HUD as a HUD-style sub-card sibling of the
/// existing `CombatPanel.cs` (Phase 9 modal). The HUD reads from
/// `Ashfall.Core.Combat.TacticalCombatSystem` via the user's own
/// `CombatHostSession`. Four tiles:
///
///   1. Left lane tile     — combatants in lane 0
///   2. Center lane tile   — combatants in lane 1
///   3. Right lane tile    — combatants in lane 2
///   4. Action bar tile    — Fire / Suppress / Clear Jam / End Turn
///
/// Anchored to viewport (not modal). The modal feel is preserved by the
/// legacy `CombatPanel.cs`; this HUD adds the always-on-top mini-monitor
/// the existing `ExpeditionPanel.cs` hud-overlay pattern already uses.
/// </summary>
public partial class CombatHudOverlay : Control, IBindablePanel
{
    public event Action? OnClose;
    public event Action? OnFireRequested;
    public event Action? OnSuppressRequested;
    public event Action? OnClearJamRequested;
    public event Action? OnEndTurnRequested;

    private AshfallDashboardShell _shell = null!;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _leftLaneGrid;
    private AshfallDataGrid? _centerLaneGrid;
    private AshfallDataGrid? _rightLaneGrid;
    private AshfallDataGrid? _actionBarGrid;
    private VBoxContainer _detailBox = null!;
    private Label _detailTitle = null!;

    private CombatHostSession? _host;

    public bool IsBound => _host != null;

    public void Bind(CombatHostSession host)
    {
        _host = host;
        if (_host != null)
        {
            _host.StateChanged += RefreshView;
        }
        RefreshView();
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("Combat HUD Overlay // Live Encounter Mini-Monitor", minWidth: 1280, minHeight: 720);
        SetContentRoot(_shell);

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("phase",     "Phase",    "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("turn",      "Turn",     "—", AshfallMetricCard.Criticality.Normal, minWidth: 80);
        _statusRail.AddCard("round",     "Round",    "—", AshfallMetricCard.Criticality.Normal, minWidth: 80);
        _statusRail.AddCard("stance",    "Stance",   "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);
        _statusRail.AddCard("players",   "Players",  "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
        _statusRail.AddCard("hostiles",  "Hostiles", "—", AshfallMetricCard.Criticality.Warn,   minWidth: 100);
        _statusRail.AddCard("downed",    "Downed",   "—", AshfallMetricCard.Criticality.Critical, minWidth: 100);
        _statusRail.AddCard("pinned",    "Pinned",   "—", AshfallMetricCard.Criticality.Caution, minWidth: 90);

        // Lane column: lane-tag (Left/Center/Right), 4-col data grid:
        // Combatant, Health, Cover, Armor. Each lane gets its own data grid
        // so the snapshot fixture is row-deterministic.
        var laneCols = new[]
        {
            new AshfallDataGrid.Column { Header = "Combatant", MinWidth = 180, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Faction",  MinWidth = 120, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Health",   MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Cover",    MinWidth = 80,  Alignment = AshfallDataGrid.ColumnAlign.Right },
        };
        _leftLaneGrid = new AshfallDataGrid(laneCols, showHeader: true, minWidth: 330, minHeight: 220);
        _centerLaneGrid = new AshfallDataGrid(laneCols, showHeader: true, minWidth: 330, minHeight: 220);
        _rightLaneGrid = new AshfallDataGrid(laneCols, showHeader: true, minWidth: 330, minHeight: 220);

        // Action bar: 4 action tiles with hint.
        var actionCols = new[]
        {
            new AshfallDataGrid.Column { Header = "Action", MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Stance", MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Hint",   MinWidth = 240, Alignment = AshfallDataGrid.ColumnAlign.Left },
        };
        _actionBarGrid = new AshfallDataGrid(actionCols, showHeader: true, minWidth: 380, minHeight: 220);
        _actionBarGrid.OnRowSelected += HandleActionRowSelected;

        var body = new VBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;

        var laneRow = new HBoxContainer();
        laneRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        laneRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        laneRow.SizeFlagsVertical = SizeFlags.ExpandFill;

        var leftCol = new VBoxContainer();
        leftCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        leftCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        leftCol.AddChild(AshfallUiHelpers.MakeSectionHeader("Lane 0 — Left Flank"));
        leftCol.AddChild(_leftLaneGrid);
        laneRow.AddChild(leftCol);

        var centerCol = new VBoxContainer();
        centerCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        centerCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        centerCol.AddChild(AshfallUiHelpers.MakeSectionHeader("Lane 1 — Center"));
        centerCol.AddChild(_centerLaneGrid);
        laneRow.AddChild(centerCol);

        var rightCol = new VBoxContainer();
        rightCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        rightCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        rightCol.AddChild(AshfallUiHelpers.MakeSectionHeader("Lane 2 — Right Flank"));
        rightCol.AddChild(_rightLaneGrid);
        laneRow.AddChild(rightCol);

        body.AddChild(laneRow);
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
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("EVENT LOG"));
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
            "Bind a CombatHostSession to see live combat event stream."));
        actionRow.AddChild(_detailBox);

        body.AddChild(actionRow);
        _shell.SetContent(body);

        _detailTitle = AshfallUiHelpers.MakeSectionHeader("EVENT LOG");
        // title re-anchor for refresh-detail
        RefreshView();
    }

    private void SetContentRoot(Control root)
    {
        AddChild(root);
        root.SetAnchorsPreset(LayoutPreset.FullRect);
        root.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        root.SizeFlagsVertical = SizeFlags.ExpandFill;
    }

    private void HandleActionRowSelected(int idx)
    {
        switch (idx)
        {
            case 0: OnFireRequested?.Invoke(); break;
            case 1: OnSuppressRequested?.Invoke(); break;
            case 2: OnClearJamRequested?.Invoke(); break;
            case 3: OnEndTurnRequested?.Invoke(); break;
        }
    }

    public void RefreshView()
    {
        RefreshStatusRail();
        BuildLaneRows();
        BuildActionRows();
        RefreshDetail();
    }

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        if (_host == null || _host.Engine == null)
        {
            _statusRail.Set("phase",    "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("turn",     "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("round",    "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("stance",   "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("players",  "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("hostiles", "—", AshfallMetricCard.Criticality.Warn);
            _statusRail.Set("downed",   "—", AshfallMetricCard.Criticality.Critical);
            _statusRail.Set("pinned",   "—", AshfallMetricCard.Criticality.Caution);
            return;
        }
        var state = _host.Engine.State;
        int players = 0, hostiles = 0, downed = 0, pinned = 0;
        for (int i = 0; i < state.Combatants.Count; i++)
        {
            var c = state.Combatants[i];
            if (c == null) continue;
            if (c.IsPlayer) players++;
            else hostiles++;
            if (c.IsDowned) downed++;
            if (c.IsPinned) pinned++;
        }

        _statusRail.Set("phase",    state.Phase.ToString().ToUpperInvariant(), AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("turn",     $"T{state.Turn}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("round",    $"R{state.RoundNumber}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("stance",   StanceName(state.PlayerStance), AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("players",  $"{players}", players > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Caution);
        _statusRail.Set("hostiles", $"{hostiles}", hostiles > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("downed",   $"{downed}", downed > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("pinned",   $"{pinned}", pinned > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
    }

    private void BuildLaneRows()
    {
        if (_leftLaneGrid == null || _centerLaneGrid == null || _rightLaneGrid == null) return;

        if (_host == null || _host.Engine == null)
        {
            _leftLaneGrid.SetRows(BuildLaneFixtureRows(0));
            _centerLaneGrid.SetRows(BuildLaneFixtureRows(1));
            _rightLaneGrid.SetRows(BuildLaneFixtureRows(2));
            return;
        }

        var state = _host.Engine.State;
        _leftLaneGrid.SetRows(LaneRowsFor(state, (int)CombatLane.Left));
        _centerLaneGrid.SetRows(LaneRowsFor(state, (int)CombatLane.Center));
        _rightLaneGrid.SetRows(LaneRowsFor(state, (int)CombatLane.Right));
    }

    private static List<AshfallDataGrid.Row> LaneRowsFor(CombatState state, int lane)
    {
        var rows = new List<AshfallDataGrid.Row>();
        for (int i = 0; i < state.Combatants.Count; i++)
        {
            var c = state.Combatants[i];
            if (c == null || c.Lane != lane) continue;
            if (c.HasFled) continue;
            var cells = new List<AshfallDataGrid.Cell>
            {
                new(c.Name, AshfallDataGrid.CellState.Normal),
                new(c.IsPlayer ? "Player" : c.FactionId, AshfallDataGrid.CellState.Muted),
                new($"{c.Health:0}/{c.MaxHealth:0}", c.IsDowned ? AshfallDataGrid.CellState.Critical :
                                                c.Health < c.MaxHealth * 0.5f ? AshfallDataGrid.CellState.Warning :
                                                                                 AshfallDataGrid.CellState.Normal),
                new($"{c.CoverRating * 100f:0}%", AshfallDataGrid.CellState.Muted),
            };
            rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = false });
        }
        if (rows.Count == 0)
        {
            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new("— lane empty —", AshfallDataGrid.CellState.Muted),
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
        if (_host == null || _host.Engine == null)
        {
            _detailTitle = AshfallUiHelpers.MakeSectionHeader("EVENT LOG");
            _detailBox.AddChild(_detailTitle);
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Combat HUD offline. Bind a CombatHostSession to see live combat event stream."));
            return;
        }
        _detailTitle = AshfallUiHelpers.MakeSectionHeader("EVENT LOG");
        _detailBox.AddChild(_detailTitle);
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        var state = _host.Engine.State;
        int shown = 0;
        for (int i = state.Events.Count - 1; i >= 0 && shown < 8; i--)
        {
            var ev = state.Events[i];
            if (ev == null) continue;
            string line = $"D{ev.Day} T{ev.Turn} · {ev.Kind} · {ev.SubjectId}";
            if (!string.IsNullOrEmpty(ev.TargetId)) line += $" → {ev.TargetId}";
            if (!string.IsNullOrEmpty(ev.Detail)) line += $" · {ev.Detail}";
            _detailBox.AddChild(AshfallUiHelpers.MakeSmall(line, autowrap: true));
            shown++;
        }
        if (shown == 0)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("No events yet — combat just started."));
        }
    }

    private static string StanceName(string stanceId)
    {
        if (string.IsNullOrEmpty(stanceId)) return "—";
        return stanceId switch
        {
            "stance_hold_position" => "Hold Position",
            "stance_advance" => "Advance",
            "stance_suppressive_fire" => "Suppressive Fire",
            "stance_retreat" => "Retreat",
            "stance_last_stand" => "Last Stand",
            _ => stanceId,
        };
    }

    /// <summary>Hard-coded fixture rows for the bound=false case. Per-lane combatants drawn from canonical combat catalog ids.</summary>
    internal static List<AshfallDataGrid.Row> BuildLaneFixtureRows(int lane)
    {
        // Lane 0 = Left Flank, Lane 1 = Center, Lane 2 = Right Flank.
        var rows = new List<AshfallDataGrid.Row>();
        if (lane == 0)
        {
            rows.Add(new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Gunner Mikhail", AshfallDataGrid.CellState.Normal),
                new("Player",        AshfallDataGrid.CellState.Muted),
                new("82/100",        AshfallDataGrid.CellState.Normal),
                new("40%",            AshfallDataGrid.CellState.Muted),
            }, Selectable = false });
        }
        else if (lane == 1)
        {
            rows.Add(new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Dr. Sarah Chen",  AshfallDataGrid.CellState.Normal),
                new("Player",          AshfallDataGrid.CellState.Muted),
                new("60/90",            AshfallDataGrid.CellState.Warning),
                new("25%",              AshfallDataGrid.CellState.Muted),
            }, Selectable = false });
            rows.Add(new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Wasteland Raider",  AshfallDataGrid.CellState.Normal),
                new("Warlord Sectors",   AshfallDataGrid.CellState.Muted),
                new("55/100",             AshfallDataGrid.CellState.Normal),
                new("0%",                AshfallDataGrid.CellState.Muted),
            }, Selectable = false });
        }
        else
        {
            rows.Add(new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Elena Vasquez",     AshfallDataGrid.CellState.Normal),
                new("Player",            AshfallDataGrid.CellState.Muted),
                new("40/85",              AshfallDataGrid.CellState.Warning),
                new("55%",                AshfallDataGrid.CellState.Muted),
            }, Selectable = false });
            rows.Add(new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Wasteland Sniper",   AshfallDataGrid.CellState.Normal),
                new("Warlord Sectors",     AshfallDataGrid.CellState.Muted),
                new("78/100",              AshfallDataGrid.CellState.Normal),
                new("20%",                 AshfallDataGrid.CellState.Muted),
            }, Selectable = false });
        }
        return rows;
    }

    internal static List<AshfallDataGrid.Row> BuildActionFixtureRows()
    {
        return new List<AshfallDataGrid.Row>
        {
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Fire",        AshfallDataGrid.CellState.Normal),
                new("—",           AshfallDataGrid.CellState.Muted),
                new("Select lane target · roll vs cover+armor", AshfallDataGrid.CellState.Muted),
            }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Suppress",     AshfallDataGrid.CellState.Normal),
                new("Suppressive",  AshfallDataGrid.CellState.Muted),
                new("Area fire · pins targets · heavy ammo + jam risk", AshfallDataGrid.CellState.Muted),
            }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("Clear Jam",    AshfallDataGrid.CellState.Normal),
                new("—",             AshfallDataGrid.CellState.Muted),
                new("Spend 1 turn · roll to clear weapon jam",   AshfallDataGrid.CellState.Muted),
            }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
            {
                new("End Turn",     AshfallDataGrid.CellState.Normal),
                new("—",             AshfallDataGrid.CellState.Muted),
                new("Resolve enemy round · ash & rad tick",         AshfallDataGrid.CellState.Muted),
            }, Selectable = true },
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


    public void Unbind()
    {
        if (_host != null)
        {
            _host.StateChanged -= RefreshView;
        }
    }

    public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
}
