// SPDX-License-Identifier: MIT
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
/// ASHFALL — Chemical Reconnaissance Panel (Plan 81).
/// </summary>
public partial class ChemicalReconPanel : Control
{
    public event Action? OnClose;
    public event Action<string>? OnRowSelected;

    // As explicitly requested: Action<string, string>
    public event Action<string, string>? OnActionRequested;

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _dataGrid;
    private VBoxContainer _detailBox = null!;
    private Label _detailTitle = null!;
    private string _filter = "all";
    private string _selectedObservationId = "";

    private ChemicalReconHostSession? _host;

    public bool IsBound => _host != null;

    public void Bind(ChemicalReconHostSession session)
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

        _shell = new AshfallDashboardShell("Chemical Reconnaissance // CBRN Survey", minWidth: 1100, minHeight: 720);
        SetContentRoot(_shell);

        var sidebarItems = new[]
        {
            new AshfallSidebar.Item { Id = "all", Label = "All Observations", Hint = "All logged hazards", IconPath = "" },
            new AshfallSidebar.Item { Id = "identified", Label = "Identified", Hint = "Confirmed hazard profiles", IconPath = "" },
            new AshfallSidebar.Item { Id = "unknown", Label = "Unknown", Hint = "Unidentified threats", IconPath = "" },
        };
        _sidebar = _shell.SetSidebar(sidebarItems, "Filter", "all");
        _sidebar.OnSelected += HandleSidebar;

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("battery", "Detector Battery", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
        _statusRail.AddCard("band", "Sensor Band", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("samples", "Samples Stored", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("hazards", "Known Hazards", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("alerts", "Critical Alerts", "—", AshfallMetricCard.Criticality.Normal, minWidth: 200);

        var cols = new[]
        {
            new AshfallDataGrid.Column { Header = "Loc ID", MinWidth = 80, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Discovery", MinWidth = 90, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Class", MinWidth = 90, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Readings", MinWidth = 80, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Exposure", MinWidth = 80, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Confidence", MinWidth = 80, Alignment = AshfallDataGrid.ColumnAlign.Right },
        };
        _dataGrid = new AshfallDataGrid(cols, showHeader: true, minWidth: 600, minHeight: 320);
        _dataGrid.OnRowSelected += HandleRowSelected;

        var body = new HBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;

        body.AddChild(_dataGrid);

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.CustomMinimumSize = new Vector2(320, 320);
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_detailBox);

        _detailTitle = AshfallUiHelpers.MakeSectionHeader("HAZARD DETAIL");
        _detailTitle.HorizontalAlignment = HorizontalAlignment.Left;
        _detailBox.AddChild(_detailTitle);
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Select an observation to view details."));

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
        _selectedObservationId = "";
        RefreshView();
    }

    private void HandleRowSelected(int idx)
    {
        if (_host == null) return;
        var filtered = GetFilteredObservations();
        if (idx >= 0 && idx < filtered.Count)
        {
            _selectedObservationId = filtered[idx].observationId;
            OnRowSelected?.Invoke(_selectedObservationId);
        }
        else
        {
            _selectedObservationId = "";
        }
        RefreshDetail();
    }

    public void RefreshView()
    {
        RefreshStatusRail();
        BuildDataRows();
        RefreshDetail();
    }

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        if (_host == null)
        {
            _statusRail.Set("battery", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("band", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("samples", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("hazards", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("alerts", "—", AshfallMetricCard.Criticality.Normal);
            return;
        }

        var state = _host.System.State;

        bool lowBattery = state.detectorBatteryRemaining < 20;
        _statusRail.Set("battery", $"{state.detectorBatteryRemaining} / {_host.System.Catalog.detector_equipment.battery_ticks_per_charge}", lowBattery ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);

        _statusRail.Set("band", state.activeSensorBand.ToUpperInvariant(), AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("samples", $"{state.collectedSamples.Count} / {_host.System.Catalog.sample_collection.max_samples_per_mission}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("hazards", $"{state.discoveredHazardIds.Count}", AshfallMetricCard.Criticality.Normal);

        // Check for any critical alerts across observations
        bool hasUnknown = false;
        bool hasBreakthrough = false;
        bool hasCriticalExposure = false;

        foreach (var obs in state.hazardObservations)
        {
            if (obs.discoveryState == "unknown") hasUnknown = true;
            if (obs.safeExposureBand == "critical") hasCriticalExposure = true;
            // Simplified breakthrough risk assessment for UI
            if (obs.recommendedFilterCategory != "particulate_only" && obs.normalizedLevel > 0.8f) hasBreakthrough = true;
        }

        string alertText = "ALL CLEAR";
        var alertSeverity = AshfallMetricCard.Criticality.Normal;

        if (hasCriticalExposure)
        {
            alertText = "CRITICAL EXPOSURE DETECTED";
            alertSeverity = AshfallMetricCard.Criticality.Critical;
        }
        else if (hasBreakthrough)
        {
            alertText = "FILTER BREAKTHROUGH IMMINENT";
            alertSeverity = AshfallMetricCard.Criticality.Critical;
        }
        else if (hasUnknown)
        {
            alertText = "UNKNOWN HAZARD";
            alertSeverity = AshfallMetricCard.Criticality.Warn;
        }

        _statusRail.Set("alerts", alertText, alertSeverity);
    }

    private List<HazardObservation> GetFilteredObservations()
    {
        var list = new List<HazardObservation>();
        if (_host == null) return list;
        foreach (var obs in _host.System.Observations)
        {
            if (_filter == "all") list.Add(obs);
            else if (_filter == "identified" && (obs.discoveryState == "identified" || obs.discoveryState == "quantified")) list.Add(obs);
            else if (_filter == "unknown" && (obs.discoveryState == "unknown" || obs.discoveryState == "suspected")) list.Add(obs);
        }
        return list;
    }

    private void BuildDataRows()
    {
        if (_dataGrid == null) return;
        var rows = new List<AshfallDataGrid.Row>();
        if (_host == null)
        {
            rows.Add(new AshfallDataGrid.Row { Cells = CreateEmptyCells("— Offline —"), Selectable = false });
            _dataGrid.SetRows(rows);
            return;
        }

        var filtered = GetFilteredObservations();
        foreach (var obs in filtered)
        {
            var discoveryState = obs.discoveryState == "unknown" ? AshfallDataGrid.CellState.Critical :
                                 obs.discoveryState == "suspected" ? AshfallDataGrid.CellState.Caution :
                                 AshfallDataGrid.CellState.Normal;

            var exposureState = obs.safeExposureBand == "critical" ? AshfallDataGrid.CellState.Critical :
                                obs.safeExposureBand == "safe" ? AshfallDataGrid.CellState.Positive :
                                AshfallDataGrid.CellState.Warning;

            string hazardClass = string.IsNullOrEmpty(obs.hazardId) ? "???" : obs.hazardId;
            if (obs.discoveryState == "unknown") hazardClass = "UNKNOWN";

            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new(obs.locationNodeId, AshfallDataGrid.CellState.Normal),
                    new(obs.discoveryState.ToUpperInvariant(), discoveryState),
                    new(hazardClass, AshfallDataGrid.CellState.Normal),
                    new($"{obs.normalizedLevel * 100f:0.0}%", AshfallDataGrid.CellState.Normal),
                    new(obs.safeExposureBand.ToUpperInvariant(), exposureState),
                    new($"{obs.confidence * 100f:0}%", AshfallDataGrid.CellState.Muted),
                },
                Selectable = true
            });
        }

        if (rows.Count == 0)
        {
            rows.Add(new AshfallDataGrid.Row { Cells = CreateEmptyCells("— No observations match filter —"), Selectable = false });
        }

        _dataGrid.SetRows(rows);
    }

    private List<AshfallDataGrid.Cell> CreateEmptyCells(string message)
    {
        var cells = new List<AshfallDataGrid.Cell> { new(message, AshfallDataGrid.CellState.Muted) };
        for (int i = 1; i < 6; i++) cells.Add(new("—", AshfallDataGrid.CellState.Muted));
        return cells;
    }

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        AshfallUiHelpers.EmptyChildren(_detailBox);

        _detailTitle = AshfallUiHelpers.MakeSectionHeader("HAZARD DETAIL");
        _detailTitle.HorizontalAlignment = HorizontalAlignment.Left;
        _detailBox.AddChild(_detailTitle);

        var separator = AshfallUiHelpers.MakeSeparator();
        separator.CustomMinimumSize = new Vector2(0, 2);
        _detailBox.AddChild(separator);

        if (_host == null || string.IsNullOrEmpty(_selectedObservationId))
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Select an observation to view details."));
            // Add default wind telemetry or generic data
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Wind Telemetry", "Awaiting sensor data...", AshfallUiHelpers.ToColor(DesignTheme.Muted)));
            return;
        }

        HazardObservation? selected = null;
        foreach (var obs in _host.System.Observations)
        {
            if (obs.observationId == _selectedObservationId)
            {
                selected = obs;
                break;
            }
        }

        if (selected == null) return;

        _detailTitle.Text = $"LOC: {selected.locationNodeId.ToUpperInvariant()}";

        // Explicit error labels requested for Critical Alerts
        if (selected.discoveryState == "unknown")
        {
            var unknownLabel = AshfallUiHelpers.MakeDataRow("! ALERT !", "UNKNOWN HAZARD", AshfallUiHelpers.ToColor(DesignTheme.Entropy));
            _detailBox.AddChild(unknownLabel);
        }
        if (selected.safeExposureBand == "critical")
        {
            var exposureLabel = AshfallUiHelpers.MakeDataRow("! ALERT !", "CRITICAL EXPOSURE DETECTED", AshfallUiHelpers.ToColor(DesignTheme.Critical));
            _detailBox.AddChild(exposureLabel);
        }

        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Hazard ID", selected.hazardId, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Discovery", selected.discoveryState.ToUpperInvariant(), AshfallUiHelpers.ToColor(DesignTheme.Dim)));

        Color expColor = selected.safeExposureBand == "critical" ? AshfallUiHelpers.ToColor(DesignTheme.Critical) :
                         selected.safeExposureBand == "safe" ? AshfallUiHelpers.ToColor(DesignTheme.Lethe) :
                         AshfallUiHelpers.ToColor(DesignTheme.Entropy);
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Exposure", selected.safeExposureBand.ToUpperInvariant(), expColor));

        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Filter Rec.", selected.recommendedFilterCategory, AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Detector Band", selected.detectorBand, AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Norm. Level", $"{selected.normalizedLevel * 100f:0.0}%", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Confidence", $"{selected.confidence * 100f:0}%", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

        // Wind telemetry and Filter saturation tracking
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("TELEMETRY"));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Filter Saturation", selected.normalizedLevel > 0.8f ? "HIGH RISK" : "NOMINAL", selected.normalizedLevel > 0.8f ? AshfallUiHelpers.ToColor(DesignTheme.Critical) : AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Wind Telemetry", "Stable (No prevailing drift)", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

        if (!string.IsNullOrEmpty(_host.LastEvent))
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSmall(_host.LastEvent));
        }

        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("ACTIONS"));
        var actionRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);

        var deployBtn = AshfallUiHelpers.MakeButton("DEPLOY SENSOR",
            () => OnActionRequested?.Invoke("deploy_sensor", selected.locationNodeId));
        deployBtn.CustomMinimumSize = new Vector2(120, 30);
        actionRow.AddChild(deployBtn);

        var sampleBtn = AshfallUiHelpers.MakeButton("SAMPLE",
            () => OnActionRequested?.Invoke("sample", selected.hazardId));
        sampleBtn.CustomMinimumSize = new Vector2(90, 30);
        actionRow.AddChild(sampleBtn);

        var filterBtn = AshfallUiHelpers.MakeButton("CHANGE FILTER",
            () => OnActionRequested?.Invoke("change_filter", selected.locationNodeId));
        filterBtn.CustomMinimumSize = new Vector2(120, 30);
        actionRow.AddChild(filterBtn);

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
