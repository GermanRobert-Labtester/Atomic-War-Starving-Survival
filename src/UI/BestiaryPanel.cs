// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using Ashfall.Core.World;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Bestiary Panel (Plan 165).
/// Knowledge-gated species ledger: observation level, ecology status, apex
/// activity, and tamed animals. Exact hidden populations stay hidden until a
/// species is documented; TAME routes through the ecosystem's authority
/// transfer (never a parallel population).
/// </summary>
public partial class BestiaryPanel : Control
{
    public event Action? OnClose;
    public event Action<string, string>? OnActionRequested;

    private AshfallDashboardShell _shell = null!;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _grid;
    private VBoxContainer _detailBox = null!;
    private Label _detailTitle = null!;

    private int _selectedIndex = -1;
    private readonly List<string> _rowSpeciesIds = new List<string>();

    private WildlifeEcosystemHostSession? _host;
    private Func<WildlifeMigrationSystem?>? _migration;

    public bool IsBound => _host != null;

    public void Bind(WildlifeEcosystemHostSession session, Func<WildlifeMigrationSystem?>? migrationProvider)
    {
        if (_host != null)
            _host.StateChanged -= RefreshView;
        _host = session;
        _migration = migrationProvider;
        if (_host != null)
            _host.StateChanged += RefreshView;
        RefreshView();
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("Wasteland Bestiary // Field Ledger", minWidth: 1000, minHeight: 660);
        AddChild(_shell);
        _shell.SetAnchorsPreset(LayoutPreset.FullRect);
        _shell.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        _shell.SizeFlagsVertical = SizeFlags.ExpandFill;

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("documented", "Documented", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
        _statusRail.AddCard("extinct", "Local Extinctions", "—", AshfallMetricCard.Criticality.Caution, minWidth: 150);
        _statusRail.AddCard("apex", "Apex Active", "—", AshfallMetricCard.Criticality.Warn, minWidth: 120);
        _statusRail.AddCard("tamed", "Tamed", "—", AshfallMetricCard.Criticality.Normal, minWidth: 90);

        var cols = new[]
        {
            new AshfallDataGrid.Column { Header = "Species",  MinWidth = 170, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Knowledge",MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Diet",     MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Status",   MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Tameable", MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Left },
        };
        _grid = new AshfallDataGrid(cols, showHeader: true, minWidth: 640, minHeight: 300);
        _grid.OnRowSelected += idx => { _selectedIndex = idx; RefreshDetail(); };

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

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        if (_host == null)
        {
            _statusRail.Set("documented", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("extinct", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("apex", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("tamed", "—", AshfallMetricCard.Criticality.Normal);
            return;
        }
        int documented = 0;
        foreach (var s in _host.System.Catalog.species)
            if (s != null && _host.System.KnowledgeLevel(s.id) == "documented") documented++;
        _statusRail.Set("documented", $"{documented}/{_host.System.Catalog.species.Count}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("extinct", $"{_host.System.State.extinct_species_sectors.Count}",
            _host.System.State.extinct_species_sectors.Count > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("apex", $"{_host.System.ApexActivities.Count}",
            _host.System.ApexActivities.Count > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("tamed", $"{_host.System.DomesticAnimals.Count}", AshfallMetricCard.Criticality.Normal);
    }

    private void BuildGridRows()
    {
        if (_grid == null) return;
        _rowSpeciesIds.Clear();
        if (_host == null)
        {
            _grid.SetRows(new List<AshfallDataGrid.Row>());
            return;
        }

        var rows = new List<AshfallDataGrid.Row>();
        foreach (var s in _host.System.Catalog.species)
        {
            if (s == null) continue;
            string knowledge = _host.System.KnowledgeLevel(s.id);
            bool apexActive = _host.System.ApexActivities.Any(a =>
                a != null && string.Equals(a.species_id, s.id, StringComparison.Ordinal));

            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new(knowledge == "unknown" ? "unidentified species" : s.display_name,
                        knowledge == "unknown" ? AshfallDataGrid.CellState.Muted : AshfallDataGrid.CellState.Normal),
                    new(knowledge, knowledge == "documented" ? AshfallDataGrid.CellState.Positive
                        : knowledge == "unknown" ? AshfallDataGrid.CellState.Muted : AshfallDataGrid.CellState.Normal),
                    new(knowledge == "unknown" ? "—" : s.diet_type, AshfallDataGrid.CellState.Muted),
                    new(apexActive ? "APEX ACTIVE" : "—",
                        apexActive ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Muted),
                    new(s.tameable && knowledge != "unknown" ? "yes" : "—",
                        s.tameable ? AshfallDataGrid.CellState.Positive : AshfallDataGrid.CellState.Muted),
                },
                Selectable = true
            });
            _rowSpeciesIds.Add(s.id);
        }
        _grid.SetRows(rows);
    }

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        AshfallUiHelpers.EmptyChildren(_detailBox);
        _detailTitle = AshfallUiHelpers.MakeSectionHeader("FIELD NOTES");
        _detailTitle.HorizontalAlignment = HorizontalAlignment.Left;
        _detailBox.AddChild(_detailTitle);
        var sep = AshfallUiHelpers.MakeSeparator();
        sep.CustomMinimumSize = new Vector2(0, 2);
        _detailBox.AddChild(sep);

        if (_host == null)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("System offline. Bind a WildlifeEcosystemHostSession."));
            return;
        }

        if (_selectedIndex < 0 || _selectedIndex >= _rowSpeciesIds.Count)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Select a species. Knowledge grows with every verified sighting."));
            return;
        }

        string speciesId = _rowSpeciesIds[_selectedIndex];
        var species = _host.System.Species(speciesId);
        string knowledge = _host.System.KnowledgeLevel(speciesId);
        _detailTitle.Text = $"FIELD NOTES: {species?.display_name ?? speciesId}";

        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Knowledge", knowledge,
            knowledge == "documented" ? AshfallUiHelpers.ToColor(DesignTheme.Lethe) : AshfallUiHelpers.ToColor(DesignTheme.Dim)));

        // Hidden populations stay hidden unless documented (plan §8.23).
        var migration = _migration?.Invoke();
        if (knowledge == "documented" && migration != null && !string.IsNullOrEmpty(_worldSector))
        {
            int pop = _host.System.SectorSpeciesPopulation(migration, _worldSector, speciesId);
            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow($"Population ({_worldSector})",
                _host.System.IsLocallyExtinct(_worldSector, speciesId) ? "locally extinct" : $"{pop}",
                _host.System.IsLocallyExtinct(_worldSector, speciesId)
                    ? AshfallUiHelpers.ToColor(DesignTheme.Critical)
                    : AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        }
        else
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Exact populations require a fully documented entry."));
        }

        var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
        var observeBtn = AshfallUiHelpers.MakeButton("LOG SIGHTING",
            () => OnActionRequested?.Invoke("OBSERVE", speciesId));
        observeBtn.CustomMinimumSize = new Vector2(130, 30);
        row.AddChild(observeBtn);
        if (species is { tameable: true } && knowledge != "unknown" && migration != null && !string.IsNullOrEmpty(_worldSector))
        {
            var tameBtn = AshfallUiHelpers.MakeButton("ATTEMPT TAME",
                () => OnActionRequested?.Invoke("TAME", $"{speciesId}|{_worldSector}|"));
            tameBtn.CustomMinimumSize = new Vector2(140, 30);
            row.AddChild(tameBtn);
        }
        _detailBox.AddChild(row);

        if (_host.System.DomesticAnimals.Count > 0)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("PENS"));
            foreach (var a in _host.System.DomesticAnimals)
                _detailBox.AddChild(AshfallUiHelpers.MakeSmall(
                    $"{a.animal_id}: {a.species_id.Replace("species_", "").Replace('_', ' ')} (tamed D{a.tamed_day})"));
        }

        if (!string.IsNullOrEmpty(_host.LastEvent))
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSmall(_host.LastEvent));
        }
    }

    private string? _worldSector;

    /// <summary>Host injects the shelter sector for population reads/taming.</summary>
    public void SetWorldSector(string sectorId) => _worldSector = sectorId;

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
