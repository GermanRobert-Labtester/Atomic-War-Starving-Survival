using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Decontamination Airlock Dashboard.
/// Bound to DecontaminationHostSession.
/// </summary>
public partial class DeconAirlockPanel : Control
{
    public event Action? OnClose;
    public event Action<string, string>? OnActionRequested;

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _queueGrid;
    private VBoxContainer _detailBox = null!;
    private Label _detailTitle = null!;
    private int _selectedIndex = -1;
    private string _currentTab = "cases";

    private DecontaminationHostSession? _host;

    public bool IsBound => _host != null;

    public void Bind(DecontaminationHostSession session)
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
        {
            _host.StateChanged -= RefreshView;
            _host = null;
        }
        RefreshView();
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("Decontamination Airlock // Baseline Interlock", minWidth: 1100, minHeight: 720);
        SetContentRoot(_shell);

        var sidebarItems = new[]
        {
            new AshfallSidebar.Item { Id = "cases", Label = "Airlock & Queue", Hint = "Active and pending decon cases", IconPath = "" },
            new AshfallSidebar.Item { Id = "effluent", Label = "Effluent System", Hint = "Tank volume and filters", IconPath = "" },
        };
        _sidebar = _shell.SetSidebar(sidebarItems, "Subsystem", "cases");
        _sidebar.OnSelected += id => { _currentTab = id; _selectedIndex = -1; RefreshView(); };

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("active", "Active Case", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
        _statusRail.AddCard("queue", "Queued", "0", AshfallMetricCard.Criticality.Normal, minWidth: 90);
        _statusRail.AddCard("tank", "Effluent Tank", "0%", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("filter", "Filter Life", "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
        _statusRail.AddCard("shelter", "Shelter Contam", "0%", AshfallMetricCard.Criticality.Normal, minWidth: 120);

        var cols = new[]
        {
            new AshfallDataGrid.Column { Header = "ID", MinWidth = 60, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Subject", MinWidth = 140, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Gear", MinWidth = 120, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Surface Dust", MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Status", MinWidth = 120, Alignment = AshfallDataGrid.ColumnAlign.Left },
        };
        _queueGrid = new AshfallDataGrid(cols, showHeader: true, minWidth: 600, minHeight: 320);
        _queueGrid.OnRowSelected += idx => { _selectedIndex = idx; RefreshDetail(); };

        var body = new HBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;

        body.AddChild(_queueGrid);

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.CustomMinimumSize = new Vector2(340, 320);
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_detailBox);

        _detailTitle = AshfallUiHelpers.MakeSectionHeader("CASE DETAIL");
        _detailTitle.HorizontalAlignment = HorizontalAlignment.Left;
        _detailBox.AddChild(_detailTitle);

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
            _statusRail.Set("active", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("queue", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("tank", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("filter", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("shelter", "—", AshfallMetricCard.Criticality.Normal);
            return;
        }

        var state = _host.System.State;

        string activeName = state.activeCase != null ? state.activeCase.survivorId : "NONE";
        var activeCrit = state.activeCase != null ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal;
        _statusRail.Set("active", activeName, activeCrit);

        _statusRail.Set("queue", state.queue.Count.ToString(), AshfallMetricCard.Criticality.Normal);

        float tankPct = state.effluentTankCapacity > 0 ? (state.effluentTankVolume / state.effluentTankCapacity) : 0f;
        var tankCrit = tankPct > 0.9f ? AshfallMetricCard.Criticality.Critical : (tankPct > 0.7f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("tank", $"{tankPct:P0}", tankCrit);

        if (state.effluentFilterInstalled)
        {
            var filterCrit = state.effluentFilterRemainingLiters < 50 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal;
            _statusRail.Set("filter", $"{state.effluentFilterRemainingLiters:F0} L", filterCrit);
        }
        else
        {
            _statusRail.Set("filter", "MISSING", AshfallMetricCard.Criticality.Critical);
        }

        var shelterCrit = state.shelterContaminationLevel > 0.1f ? AshfallMetricCard.Criticality.Critical : (state.shelterContaminated ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("shelter", $"{state.shelterContaminationLevel:P1}", shelterCrit);
    }

    private void BuildGridRows()
    {
        if (_queueGrid == null) return;
        var rows = new List<AshfallDataGrid.Row>();
        if (_host == null)
        {
            _queueGrid.SetRows(rows);
            return;
        }

        var state = _host.System.State;

        if (_currentTab == "cases")
        {
            if (state.activeCase != null) rows.Add(MakeCaseRow(state.activeCase, true));
            foreach (var c in state.queue) rows.Add(MakeCaseRow(c, false));
        }
        else if (_currentTab == "effluent")
        {
            foreach (var inc in state.incidentLog)
            {
                rows.Add(new AshfallDataGrid.Row {
                    Cells = new List<AshfallDataGrid.Cell> {
                        new($"Day {inc.day}", AshfallDataGrid.CellState.Muted),
                        new(inc.caseId.Length > 8 ? inc.caseId.Substring(0, 8) : inc.caseId, AshfallDataGrid.CellState.Normal),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new(inc.description, AshfallDataGrid.CellState.Muted)
                    },
                    Selectable = false
                });
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
        _queueGrid.SetRows(rows);
    }

    private AshfallDataGrid.Row MakeCaseRow(DeconCase c, bool isActive)
    {
        var statusState = isActive ? AshfallDataGrid.CellState.Caution : AshfallDataGrid.CellState.Normal;
        if (c.status == DeconStatus.RewashRequired) statusState = AshfallDataGrid.CellState.Critical;

        var dustState = c.surfaceContamination > 0.5f ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Normal;

        return new AshfallDataGrid.Row
        {
            Cells = new List<AshfallDataGrid.Cell>
            {
                new(c.caseId.Length > 8 ? c.caseId.Substring(0, 8) : c.caseId, AshfallDataGrid.CellState.Normal),
                new(c.survivorId, AshfallDataGrid.CellState.Normal),
                new(c.gearId, AshfallDataGrid.CellState.Normal),
                new($"{c.surfaceContamination:P1}", dustState),
                new(c.status.ToString().ToUpper(), statusState)
            },
            Selectable = true
        };
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
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Decontamination system offline."));
            return;
        }
        var state = _host.System.State;

        if (_currentTab == "effluent")
        {
            _detailTitle.Text = "EFFLUENT & REAGENTS";

            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Tank Volume", $"{state.effluentTankVolume:F1} / {state.effluentTankCapacity:F1} L", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Tank Contamination", $"{state.effluentTankContamination:P1}",
                state.effluentTankContamination > 0.8f ? AshfallUiHelpers.ToColor(DesignTheme.Critical) : AshfallUiHelpers.ToColor(DesignTheme.Entropy)));
            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Sludge Volume", $"{state.effluentSludgeVolume:F2} L", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Filter Installed", state.effluentFilterInstalled ? "YES" : "NO",
                state.effluentFilterInstalled ? AshfallUiHelpers.ToColor(DesignTheme.Lethe) : AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Filter Remaining", $"{state.effluentFilterRemainingLiters:F0} L", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("SYSTEM ACTIONS"));

            var actionRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var treatBtn = AshfallUiHelpers.MakeButton("TREAT EFFLUENT", () => OnActionRequested?.Invoke("treat_effluent", ""));
            actionRow.AddChild(treatBtn);

            var installBtn = AshfallUiHelpers.MakeButton("INSTALL FILTER", () => OnActionRequested?.Invoke("install_filter", ""));
            actionRow.AddChild(installBtn);

            _detailBox.AddChild(actionRow);

            if (!string.IsNullOrEmpty(_host.LastEvent))
            {
                _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
                _detailBox.AddChild(AshfallUiHelpers.MakeSmall(_host.LastEvent));
            }
            return;
        }

        DeconCase? selected = null;
        if (_selectedIndex == 0 && state.activeCase != null)
        {
            selected = state.activeCase;
        }
        else if (state.activeCase != null && _selectedIndex > 0 && _selectedIndex - 1 < state.queue.Count)
        {
            selected = state.queue[_selectedIndex - 1];
        }
        else if (state.activeCase == null && _selectedIndex >= 0 && _selectedIndex < state.queue.Count)
        {
            selected = state.queue[_selectedIndex];
        }

        if (selected == null)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Select a case to view details."));
            return;
        }

        _detailTitle.Text = $"CASE: {selected.survivorId}";

        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Status", selected.status.ToString().ToUpper(), AshfallUiHelpers.ToColor(DesignTheme.Warm)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Gear", selected.gearId, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Surface Dust", $"{selected.surfaceContamination:P1}", AshfallUiHelpers.ToColor(DesignTheme.Entropy)));

        if (selected.status == DeconStatus.InProgress || selected.status == DeconStatus.RewashRequired)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());

            string segments = "";
            for (int i = 0; i < selected.totalStages; i++)
            {
                segments += (i < selected.currentStageIndex) ? "█ " : (i == selected.currentStageIndex ? "▒ " : "□ ");
            }
            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("DECON_SEQUENCE_ACTIVE", segments, AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Current Stage", string.IsNullOrEmpty(selected.currentStageId) ? "—" : selected.currentStageId, AshfallUiHelpers.ToColor(DesignTheme.Dim)));

            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("RADIOMETRIC_GATE_SIG", $"{selected.radiometricGateReading:F2} mSv/h",
                selected.radiometricGateReading > 10f ? AshfallUiHelpers.ToColor(DesignTheme.Critical) : AshfallUiHelpers.ToColor(DesignTheme.Warm)));

            if (!_host.System.CanOpenInnerDoor())
            {
                var lockLabel = AshfallUiHelpers.MakeSmall("INNER DOOR LOCKED");
                lockLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Critical));
                _detailBox.AddChild(lockLabel);
            }
            if (selected.radiometricGateReading > 10f)
            {
                var limitLabel = AshfallUiHelpers.MakeSmall("CONTAMINATION ABOVE LIMIT");
                limitLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Critical));
                _detailBox.AddChild(limitLabel);
            }
            if (selected.status == DeconStatus.RewashRequired)
            {
                var rewashLabel = AshfallUiHelpers.MakeSmall("REWASH REQUIRED");
                rewashLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Critical));
                _detailBox.AddChild(rewashLabel);
            }

            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("ACTIONS"));
            var actionRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);

            var tickBtn = AshfallUiHelpers.MakeButton("TICK STAGE", () => OnActionRequested?.Invoke("tick_stage", selected.caseId));
            actionRow.AddChild(tickBtn);

            var overrideBtn = AshfallUiHelpers.MakeButton("MANUAL OVERRIDE", () => OnActionRequested?.Invoke("manual_override", selected.caseId));
            actionRow.AddChild(overrideBtn);

            var disposeBtn = AshfallUiHelpers.MakeButton("DISPOSE GEAR", () => OnActionRequested?.Invoke("dispose_gear", selected.gearId));
            actionRow.AddChild(disposeBtn);

            _detailBox.AddChild(actionRow);
        }
        else if (selected.status == DeconStatus.Queued)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            var startBtn = AshfallUiHelpers.MakeButton("START DECON", () => OnActionRequested?.Invoke("start_decon", selected.caseId));
            _detailBox.AddChild(startBtn);
        }

        if (!string.IsNullOrEmpty(_host.LastEvent))
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSmall(_host.LastEvent));
        }
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
