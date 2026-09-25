// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Defense;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Defense Grid Panel (Plan 163).
/// Bound surface for DefenseHostSession: trap installations, perimeter
/// strength breakdown, reset/repair commands (kept separate), and the
/// structured raid log. Emplacements/turrets live in the perimeter system —
/// their strength contribution is shown here without owning it.
/// </summary>
public partial class DefenseGridPanel : Control
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
    private readonly List<string> _rowInstallationIds = new List<string>();
    private OptionButton? _trapPicker;

    private DefenseHostSession? _host;

    public bool IsBound => _host != null;

    public void Bind(DefenseHostSession session)
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

        _shell = new AshfallDashboardShell("Defense Grid // Wire & Pit", minWidth: 1100, minHeight: 720);
        AddChild(_shell);
        _shell.SetAnchorsPreset(LayoutPreset.FullRect);
        _shell.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        _shell.SizeFlagsVertical = SizeFlags.ExpandFill;

        var sidebarItems = new[]
        {
            new AshfallSidebar.Item { Id = "all",     Label = "All Traps",   Hint = "every installation", IconPath = "" },
            new AshfallSidebar.Item { Id = "armed",   Label = "Armed",       Hint = "set and waiting", IconPath = "" },
            new AshfallSidebar.Item { Id = "sprung",  Label = "Sprung",      Hint = "needs reset", IconPath = "" },
            new AshfallSidebar.Item { Id = "broken",  Label = "Broken",      Hint = "needs repair", IconPath = "" },
        };
        _sidebar = _shell.SetSidebar(sidebarItems, "Filter", "all");
        _sidebar.OnSelected += id => { _filter = id; _selectedIndex = -1; RefreshView(); };

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("armed", "Armed Traps", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("sprung", "Sprung", "—", AshfallMetricCard.Criticality.Caution, minWidth: 90);
        _statusRail.AddCard("strength", "Perimeter Strength", "—", AshfallMetricCard.Criticality.Normal, minWidth: 160);
        _statusRail.AddCard("captures", "Total Captures", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);

        var cols = new[]
        {
            new AshfallDataGrid.Column { Header = "Installation", MinWidth = 170, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Trap",         MinWidth = 150, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Placement",    MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "State",        MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "HP",           MinWidth = 70,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Sprungs",      MinWidth = 70,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Captures",     MinWidth = 80,  Alignment = AshfallDataGrid.ColumnAlign.Right },
        };
        _grid = new AshfallDataGrid(cols, showHeader: true, minWidth: 760, minHeight: 300);
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
        _detailBox.CustomMinimumSize = new Vector2(330, 300);
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

    private static string StateOf(TrapInstallationState i) =>
        i.broken ? "BROKEN" : i.sprung ? "SPRUNG" : i.armed ? "ARMED" : "IDLE";

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        if (_host == null)
        {
            _statusRail.Set("armed", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("sprung", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("strength", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("captures", "—", AshfallMetricCard.Criticality.Normal);
            return;
        }
        int armed = 0, sprung = 0, captures = 0;
        foreach (var i in _host.System.Installations)
        {
            if (i.broken) continue;
            if (i.sprung) sprung++;
            else if (i.armed) armed++;
            captures += i.total_captures;
        }
        // CORE-MECH W5: project the SAME truth the raid resolver uses — the attached
        // perimeter and the real power state. The previous call passed (null, null),
        // so walls, turrets, and powered state were invisible to the player who
        // built them.
        var strength = _host.System.CalculatePerimeterStrength(
            _host.Perimeter, _host.EmplacementPoweredProvider);
        _statusRail.Set("armed", $"{armed}", armed > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Caution);
        _statusRail.Set("sprung", $"{sprung}", sprung > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("strength",
            $"{strength.Total} (traps {strength.Traps} · walls {strength.Walls} · turrets {strength.Turrets} · power {strength.Power} · penalties -{strength.DamagePenalties})",
            AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("captures", $"{captures}", AshfallMetricCard.Criticality.Normal);

        // CORE-MECH W5: report what the defenses actually did last time.
        var last = _host.LastEngagement;
        if (last != null)
        {
            _statusRail.Set("last raid",
                last.Repelled
                    ? $"repelled · {last.RaidersNeutralizedByTraps} down · {last.RaidersCaptured} captured"
                    : $"breached · {last.RemainingRaiders} reached the shelter",
                last.Repelled ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Critical);
        }
    }

    private void BuildGridRows()
    {
        if (_grid == null) return;
        _rowInstallationIds.Clear();
        if (_host == null)
        {
            _grid.SetRows(new List<AshfallDataGrid.Row>());
            return;
        }

        var rows = new List<AshfallDataGrid.Row>();
        foreach (var i in _host.System.Installations)
        {
            if (i == null) continue;
            bool pass = _filter switch
            {
                "armed" => !i.broken && !i.sprung && i.armed,
                "sprung" => !i.broken && i.sprung,
                "broken" => i.broken,
                _ => true
            };
            if (!pass) continue;

            var def = _host.System.FindTrapDefinition(i.trap_id);
            var stateCell = i.broken ? AshfallDataGrid.CellState.Critical
                : i.sprung ? AshfallDataGrid.CellState.Warning
                : AshfallDataGrid.CellState.Positive;
            var hpCell = i.current_hp <= i.max_hp / 2 ? AshfallDataGrid.CellState.Caution
                : AshfallDataGrid.CellState.Normal;

            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new(i.installation_id, AshfallDataGrid.CellState.Normal),
                    new(def?.display_name ?? i.trap_id, AshfallDataGrid.CellState.Normal),
                    new(i.placement_id, AshfallDataGrid.CellState.Muted),
                    new(StateOf(i), stateCell),
                    new($"{i.current_hp}/{i.max_hp}", hpCell),
                    new($"{i.total_activations}", AshfallDataGrid.CellState.Normal),
                    new($"{i.total_captures}", i.total_captures > 0 ? AshfallDataGrid.CellState.Positive : AshfallDataGrid.CellState.Muted),
                },
                Selectable = true
            });
            _rowInstallationIds.Add(i.installation_id);
        }

        if (rows.Count == 0)
        {
            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new("— no installations match filter —", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
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
        _trapPicker = null;
        _detailTitle = AshfallUiHelpers.MakeSectionHeader("DEFENSE DETAIL");
        _detailTitle.HorizontalAlignment = HorizontalAlignment.Left;
        _detailBox.AddChild(_detailTitle);
        var sep = AshfallUiHelpers.MakeSeparator();
        sep.CustomMinimumSize = new Vector2(0, 2);
        _detailBox.AddChild(sep);

        if (_host == null)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("System offline. Bind a DefenseHostSession."));
            return;
        }

        // ── Plan 203: perimeter sector grid (attached perimeter authority) ──
        if (_host.Perimeter != null)
        {
            var perimeter = _host.Perimeter;
            _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("PERIMETER GRID"));

            var snapshot = perimeter.GetEncounterSnapshot();
            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Approach delay", $"×{snapshot.movement_delay_multiplier:0.0#}", AshfallUiHelpers.ColorText));
            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Stealth denied", snapshot.stealth_denied ? "YES" : "no",
                snapshot.stealth_denied ? AshfallUiHelpers.ColorSuccess : AshfallUiHelpers.ColorMuted));

            foreach (var sector in perimeter.Sectors)
            {
                string alarmState = !sector.alarm_armed ? "DISARMED"
                    : sector.alarm_spent ? "SPENT — NEEDS RESET" : "ARMED";
                var stateColor = sector.alarm_spent ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorText;

                var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                var label = AshfallUiHelpers.MakeBody($"{sector.sector_id.ToUpperInvariant()} — {alarmState} · {sector.emplacement_ids.Count} emplacement(s) · false alarms {sector.false_alarm_count}", autowrap: false);
                label.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                label.AddThemeColorOverride("font_color", stateColor);
                row.AddChild(label);

                if (sector.alarm_spent)
                {
                    var resetBtn = AshfallUiHelpers.MakeButton("RESET", () =>
                    {
                        _host.ResetSectorAlarm(sector.sector_id);
                        RefreshView();
                    });
                    resetBtn.TooltipText = "Rearm the triggered alert device. No cost.";
                    resetBtn.CustomMinimumSize = new Vector2(0, 26);
                    row.AddChild(resetBtn);
                }
                var armBtn = AshfallUiHelpers.MakeButton(sector.alarm_armed ? "DISARM" : "ARM", () =>
                {
                    _host.ToggleSectorArm(sector.sector_id);
                    RefreshView();
                });
                armBtn.TooltipText = "Toggle the sector's alert device standby.";
                armBtn.CustomMinimumSize = new Vector2(0, 26);
                row.AddChild(armBtn);

                _detailBox.AddChild(row);
            }

            var logTail = perimeter.IntrusionLog;
            if (logTail.Count > 0)
            {
                _detailBox.AddChild(AshfallUiHelpers.MakeSubsectionHeader("RECENT PERIMETER EVENTS"));
                int entriesShown = 0;
                for (int i = logTail.Count - 1; i >= 0 && entriesShown < 5; i--, entriesShown++)
                {
                    var e = logTail[i];
                    _detailBox.AddChild(AshfallUiHelpers.MakeMetadata($"Day {e.day} — {e.sector_id}: {e.kind.Replace('_', ' ')}"));
                }
            }

            var sep203 = AshfallUiHelpers.MakeSeparator();
            _detailBox.AddChild(sep203);
        }

        // Install section (always available — costs shown per trap).
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("INSTALL NEW TRAP"));
        var installRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
        var picker = new OptionButton();
        foreach (var t in _host.System.TrapDefinitions)
        {
            if (t == null) continue;
            picker.AddItem(t.display_name);
            picker.SetItemMetadata(picker.ItemCount - 1, t.id);
        }
        picker.CustomMinimumSize = new Vector2(190, 30);
        if (picker.ItemCount > 0) picker.Selected = 0;
        _trapPicker = picker;
        installRow.AddChild(picker);

        string selectedTrapId = string.Empty;
        if (_trapPicker != null && _trapPicker.Selected >= 0)
            selectedTrapId = _trapPicker.GetItemMetadata(_trapPicker.Selected).AsString();
        var installDef = string.IsNullOrEmpty(selectedTrapId) ? null : _host.System.FindTrapDefinition(selectedTrapId);
        if (installDef != null)
        {
            var costs = new List<string>();
            foreach (var c in installDef.build_costs) costs.Add($"{c.Value}× {c.Key}");
            _detailBox.AddChild(AshfallUiHelpers.MakeSmall($"Cost: {string.Join(", ", costs)} · strength {installDef.base_strength} · capture {installDef.capture_chance * 100f:F0}%"));
        }
        var installBtn = AshfallUiHelpers.MakeButton("INSTALL (pay costs)", () =>
        {
            string trapId = selectedTrapId;
            if (_trapPicker != null && _trapPicker.Selected >= 0)
                trapId = _trapPicker.GetItemMetadata(_trapPicker.Selected).AsString();
            OnActionRequested?.Invoke("INSTALL", $"{trapId}|perimeter");
        }, disabled: installDef == null);
        installBtn.CustomMinimumSize = new Vector2(160, 30);
        installRow.AddChild(installBtn);
        _detailBox.AddChild(installRow);
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());

        // Selected installation.
        if (_selectedIndex < 0 || _selectedIndex >= _rowInstallationIds.Count)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Select an installation to inspect and service it."));
        }
        else
        {
            string id = _rowInstallationIds[_selectedIndex];
            var inst = _host.System.FindInstallation(id);
            if (inst != null)
            {
                var def = _host.System.FindTrapDefinition(inst.trap_id);
                _detailTitle.Text = $"INSTALLATION: {inst.installation_id}";
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("State", StateOf(inst),
                    inst.broken ? AshfallUiHelpers.ToColor(DesignTheme.Critical)
                        : inst.sprung ? AshfallUiHelpers.ToColor(DesignTheme.Entropy)
                        : AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Integrity", $"{inst.current_hp}/{inst.max_hp}",
                    inst.current_hp <= inst.max_hp / 2 ? AshfallUiHelpers.ToColor(DesignTheme.LetheAmber) : AshfallUiHelpers.ToColor(DesignTheme.Dim)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Record", $"{inst.total_activations} sprungs · {inst.total_captures} captures",
                    AshfallUiHelpers.ToColor(DesignTheme.Dim)));

                var actionRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                var resetBtn = AshfallUiHelpers.MakeButton("RESET (pay costs)",
                    () => OnActionRequested?.Invoke("RESET", id),
                    disabled: !_host.System.CanResetTrap(id));
                resetBtn.CustomMinimumSize = new Vector2(140, 30);
                actionRow.AddChild(resetBtn);
                var repairBtn = AshfallUiHelpers.MakeButton("REPAIR (pay costs)",
                    () => OnActionRequested?.Invoke("REPAIR", id),
                    disabled: !_host.System.CanRepairTrap(id));
                repairBtn.CustomMinimumSize = new Vector2(150, 30);
                actionRow.AddChild(repairBtn);
                _detailBox.AddChild(actionRow);
            }
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        }

        // Raid log (bounded, newest last).
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("RAID LOG"));
        int shown = 0;
        for (int i = _host.System.RaidLog.Count - 1; i >= 0 && shown < 8; i--, shown++)
        {
            var r = _host.System.RaidLog[i];
            _detailBox.AddChild(AshfallUiHelpers.MakeSmall(
                $"D{r.day} · {r.installation_id} · {r.outcome}" +
                (r.raiders_neutralized > 0 ? $" · -{r.raiders_neutralized} raiders" : "")));
        }
        if (shown == 0)
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("No raid engagements on record yet."));

        if (!string.IsNullOrEmpty(_host.LastEvent))
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSmall(_host.LastEvent));
        }

        var drillRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
        var drillBtn = AshfallUiHelpers.MakeButton("DRILL (4 raiders)",
            () => OnActionRequested?.Invoke("SIMULATE", "4"));
        drillBtn.CustomMinimumSize = new Vector2(150, 28);
        drillRow.AddChild(drillBtn);
        _detailBox.AddChild(drillRow);

        // ── B5–B8 Phase 7 (Plan 67 §10.5): emplacement construction ──
        if (_host.Perimeter != null)
        {
            var perimeter = _host.Perimeter;
            _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("CONSTRUCT EMPLACEMENT"));

            var buildPicker = new OptionButton();
            buildPicker.CustomMinimumSize = new Vector2(260, 30);
            for (int i = 0; i < perimeter.Definitions.Count; i++)
            {
                var def = perimeter.Definitions[i];
                buildPicker.AddItem(def.display_name);
                buildPicker.SetItemMetadata(i, def.defense_id);
            }
            if (buildPicker.ItemCount > 0) buildPicker.Selected = 0;
            _detailBox.AddChild(buildPicker);

            string selectedDefenseId = string.Empty;
            if (buildPicker.Selected >= 0)
                selectedDefenseId = buildPicker.GetItemMetadata(buildPicker.Selected).AsString();
            var buildDef = string.IsNullOrEmpty(selectedDefenseId) ? null : perimeter.FindDefinition(selectedDefenseId);
            if (buildDef != null)
            {
                var buildCosts = new List<string>();
                foreach (var c in buildDef.build_costs) buildCosts.Add($"{c.Value}× {c.Key}");
                if (!string.IsNullOrEmpty(buildDef.required_knowledge))
                    buildCosts.Add($"requires {buildDef.required_knowledge}");
                _detailBox.AddChild(AshfallUiHelpers.MakeSmall($"Cost: {string.Join(", ", buildCosts)} · {buildDef.max_hp} HP"));
            }

            var buildRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var buildBtn = AshfallUiHelpers.MakeButton("BUILD (pay costs)", () =>
            {
                string defenseId = selectedDefenseId;
                OnActionRequested?.Invoke("BUILD", defenseId);
            }, disabled: buildDef == null);
            buildBtn.CustomMinimumSize = new Vector2(170, 28);
            buildRow.AddChild(buildBtn);
            _detailBox.AddChild(buildRow);
        }
    }

    public void Open()
    {
        Visible = true;
        RefreshView();
        QueueRedraw();
    }

    public void Close() {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
                Visible = false;
        }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!Visible) return;
        if (AshfallInputActions.IsCloseOrCancel(@event))
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
