// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Kinetic Storage Panel (Plan 80 Stitch, Tactical Telemetry).
/// Programmatic UI implementation for KineticStorageHostSession.
/// </summary>
public partial class KineticStoragePanel : Control
{
    public event Action? OnClose;

    /// <summary>Raised when the player presses an action button. Signature: (actionType, instanceId)</summary>
    public event Action<string, string>? OnActionRequested;

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _grid;
    private VBoxContainer _detailBox = null!;
    private Label _detailTitle = null!;

    private int _selectedIndex = -1;
    private string _filter = "all";

    private KineticStorageHostSession? _host;

    public bool IsBound => _host != null;

    public void Bind(KineticStorageHostSession session)
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
        Visible = false;
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("Kinetic Storage // Flywheel Control", minWidth: 1100, minHeight: 720);
        SetContentRoot(_shell);

        var sidebarItems = new[]
        {
            new AshfallSidebar.Item { Id = "all",      Label = "All Units",      Hint = "every flywheel unit", IconPath = "" },
            new AshfallSidebar.Item { Id = "online",   Label = "Online",         Hint = "spinning and ready", IconPath = "" },
            new AshfallSidebar.Item { Id = "offline",  Label = "Offline",        Hint = "spooled down or braked", IconPath = "" },
            new AshfallSidebar.Item { Id = "failed",   Label = "Failed",         Hint = "containment breaches", IconPath = "" },
        };
        _sidebar = _shell.SetSidebar(sidebarItems, "Filter", "all");
        _sidebar.OnSelected += HandleSidebar;

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("total_energy", "Stored Energy", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
        _statusRail.AddCard("capacity", "Max Discharge", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
        _statusRail.AddCard("active_units", "Active Units", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("warnings", "Warnings", "—", AshfallMetricCard.Criticality.Caution, minWidth: 110);

        var cols = new[]
        {
            new AshfallDataGrid.Column { Header = "Unit",       MinWidth = 120, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Status",     MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "RPM",        MinWidth = 70,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Energy",     MinWidth = 80,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Temp",       MinWidth = 70,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Vacuum",     MinWidth = 80,  Alignment = AshfallDataGrid.ColumnAlign.Right },
        };
        _grid = new AshfallDataGrid(cols, showHeader: true, minWidth: 640, minHeight: 320);
        _grid.OnRowSelected += HandleRowSelected;

        var body = new HBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;

        body.AddChild(_grid);

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.CustomMinimumSize = new Vector2(320, 320);
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_detailBox);

        _detailTitle = AshfallUiHelpers.MakeSectionHeader("UNIT DETAIL");
        _detailTitle.HorizontalAlignment = HorizontalAlignment.Left;
        _detailBox.AddChild(_detailTitle);
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
            "Select a flywheel unit to view tactical telemetry and operational controls."));

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
            _statusRail.Set("total_energy", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("capacity", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("active_units", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("warnings", "—", AshfallMetricCard.Criticality.Normal);
            return;
        }

        float totalKwh = _host.System.TotalStoredEnergyKwh();
        float maxDischarge = _host.System.TotalDischargeCapacityKw();

        int active = 0;
        int warnings = 0;

        foreach (var f in _host.System.Flywheels)
        {
            if (f.isInstalled && f.isOnline && !f.hasFailed) active++;
            if (f.hasFailed || f.emergencyBrakeEngaged || f.bearingTemperatureC > 80f) warnings++;
        }

        _statusRail.Set("total_energy", $"{totalKwh:F1} kWh", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("capacity", $"{maxDischarge:F0} kW", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("active_units", $"{active}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("warnings", $"{warnings}", warnings > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
    }

    private bool FilterPass(FlywheelInstance f)
    {
        if (_filter == "all") return true;
        if (_filter == "online") return f.isOnline && !f.hasFailed;
        if (_filter == "offline") return !f.isOnline && !f.hasFailed;
        if (_filter == "failed") return f.hasFailed;
        return true;
    }

    private void BuildGridRows()
    {
        if (_grid == null) return;

        if (_host == null)
        {
            _grid.SetRows(new List<AshfallDataGrid.Row>());
            return;
        }

        var rows = new List<AshfallDataGrid.Row>();
        for (int i = 0; i < _host.System.Flywheels.Count; i++)
        {
            var f = _host.System.Flywheels[i];
            if (!FilterPass(f)) continue;

            var (statusState, statusText) = GetStatusBadge(f);

            var rpmState = f.rotorRpm > 0 ? AshfallDataGrid.CellState.Normal : AshfallDataGrid.CellState.Muted;
            var energyState = f.storedEnergyJ > 0 ? AshfallDataGrid.CellState.Positive : AshfallDataGrid.CellState.Muted;
            var tempState = f.bearingTemperatureC > 90f ? AshfallDataGrid.CellState.Critical :
                            (f.bearingTemperatureC > 70f ? AshfallDataGrid.CellState.Caution : AshfallDataGrid.CellState.Normal);
            var vacState = f.vacuumPressureTorr > 1f ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Normal;

            float kwh = KineticStorageSystem.JoulesToKwh(f.storedEnergyJ);

            var cells = new List<AshfallDataGrid.Cell>
            {
                new(f.instanceId, AshfallDataGrid.CellState.Normal),
                new(statusText, statusState),
                new($"{f.rotorRpm:F0}", rpmState),
                new($"{kwh:F2} kWh", energyState),
                new($"{f.bearingTemperatureC:F1}°C", tempState),
                new($"{f.vacuumPressureTorr:F3} T", vacState),
            };

            var row = new AshfallDataGrid.Row { Cells = cells, Selectable = true };
            rows.Add(row);
        }

        if (rows.Count == 0)
        {
            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new("— no units match filter —", AshfallDataGrid.CellState.Muted),
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

    private (AshfallDataGrid.CellState, string) GetStatusBadge(FlywheelInstance f)
    {
        if (f.hasFailed) return (AshfallDataGrid.CellState.Critical, "FAILED");
        if (f.emergencyBrakeEngaged) return (AshfallDataGrid.CellState.Critical, "BRAKED");
        if (f.isOnline) return (AshfallDataGrid.CellState.Positive, "ONLINE");
        if (f.isInstalled) return (AshfallDataGrid.CellState.Muted, "OFFLINE");
        return (AshfallDataGrid.CellState.Muted, "UNKNOWN");
    }

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        AshfallUiHelpers.EmptyChildren(_detailBox);
        _detailTitle = AshfallUiHelpers.MakeSectionHeader("UNIT DETAIL");
        _detailTitle.HorizontalAlignment = HorizontalAlignment.Left;
        _detailBox.AddChild(_detailTitle);
        var separator = AshfallUiHelpers.MakeSeparator();
        separator.CustomMinimumSize = new Vector2(0, 2);
        _detailBox.AddChild(separator);

        if (_host == null)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("System offline. Bind a KineticStorageHostSession."));
            return;
        }

        if (_selectedIndex < 0 || _grid?.Rows == null || _selectedIndex >= _grid.Rows.Count)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Select a flywheel unit to view tactical telemetry and operational controls."));
            return;
        }

        var selectedRow = _grid.Rows[_selectedIndex];
        if (selectedRow.Cells[0].Text == "— no units match filter —") return;

        string instanceId = selectedRow.Cells[0].Text;
        var f = _host.System.FindFlywheel(instanceId);
        if (f == null) return;

        var fc = _host.System.FindClass(f.flywheelClassId);
        if (fc == null) return;

        _detailTitle.Text = $"UNIT: {f.instanceId}";

        var (statusState, statusText) = GetStatusBadge(f);
        var statusColor = statusState == AshfallDataGrid.CellState.Critical ? AshfallUiHelpers.ToColor(DesignTheme.Critical) :
                          statusState == AshfallDataGrid.CellState.Positive ? AshfallUiHelpers.ToColor(DesignTheme.Lethe) :
                          AshfallUiHelpers.ToColor(DesignTheme.Dim);

        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Status", statusText, statusColor));

        float kwh = KineticStorageSystem.JoulesToKwh(f.storedEnergyJ);
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Stored Energy", $"{kwh:F2} kWh", AshfallUiHelpers.ToColor(DesignTheme.Lethe)));

        float rpmRatio = f.rotorRpm / fc.max_rpm;
        Color rpmColor = rpmRatio > 0.9f ? AshfallUiHelpers.ToColor(DesignTheme.Entropy) :
                         rpmRatio > 0.7f ? AshfallUiHelpers.ToColor(DesignTheme.LetheAmber) :
                         AshfallUiHelpers.ToColor(DesignTheme.Pale);
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Rotor Speed", $"{f.rotorRpm:F0} RPM", rpmColor));

        Color tempColor = f.bearingTemperatureC > fc.max_bearing_temp_c * 0.9f ? AshfallUiHelpers.ToColor(DesignTheme.Critical) :
                          f.bearingTemperatureC > fc.safe_bearing_temp_c ? AshfallUiHelpers.ToColor(DesignTheme.LetheAmber) :
                          AshfallUiHelpers.ToColor(DesignTheme.Dim);
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Bearing Temp", $"{f.bearingTemperatureC:F1}°C", tempColor));

        Color vacColor = f.vacuumPressureTorr > fc.operational_vacuum_torr * 10f ? AshfallUiHelpers.ToColor(DesignTheme.Critical) :
                         f.vacuumPressureTorr > fc.operational_vacuum_torr * 2f ? AshfallUiHelpers.ToColor(DesignTheme.LetheAmber) :
                         AshfallUiHelpers.ToColor(DesignTheme.Dim);
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Vacuum", $"{f.vacuumPressureTorr:F4} Torr", vacColor));

        // Diagnostics
        if (f.bearingHealth < 0.5f)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("DIAGNOSTIC", "BEARING_DEGRADATION", AshfallUiHelpers.ToColor(DesignTheme.Entropy)));
        }
        if (f.bearingTemperatureC >= fc.safe_bearing_temp_c)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("DIAGNOSTIC", "THERMAL_RUNAWAY_IMMINENT", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
        }
        if (f.hasFailed)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("FAILURE", f.failureReason.ToUpperInvariant(), AshfallUiHelpers.ToColor(DesignTheme.Critical)));
        }

        if (!string.IsNullOrEmpty(_host.LastEvent))
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSmall(_host.LastEvent));
        }

        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("OPERATIONAL CONTROLS"));

        var actionRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);

        var chargeBtn = AshfallUiHelpers.MakeButton("CHARGE",
            () => OnActionRequested?.Invoke("CHARGE", f.instanceId),
            disabled: !f.isOnline || f.hasFailed || f.emergencyBrakeEngaged);
        chargeBtn.CustomMinimumSize = new Vector2(90, 30);
        actionRow.AddChild(chargeBtn);

        var dischargeBtn = AshfallUiHelpers.MakeButton("DISCHARGE",
            () => OnActionRequested?.Invoke("DISCHARGE", f.instanceId),
            disabled: !f.isOnline || f.hasFailed || f.storedEnergyJ <= 0);
        dischargeBtn.CustomMinimumSize = new Vector2(90, 30);
        actionRow.AddChild(dischargeBtn);

        var brakeBtn = AshfallUiHelpers.MakeButton("EMERGENCY BRAKE",
            () => OnActionRequested?.Invoke("EMERGENCY_BRAKE", f.instanceId),
            disabled: f.hasFailed || f.rotorRpm <= 0 || f.emergencyBrakeEngaged);
        brakeBtn.CustomMinimumSize = new Vector2(140, 30);
        brakeBtn.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Critical));
        actionRow.AddChild(brakeBtn);

        var maintBtn = AshfallUiHelpers.MakeButton("MAINTENANCE",
            () => OnActionRequested?.Invoke("MAINTENANCE", f.instanceId),
            disabled: f.rotorRpm > 0 || f.isOnline);
        maintBtn.CustomMinimumSize = new Vector2(110, 30);
        actionRow.AddChild(maintBtn);

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
