// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.UI;
using Ashfall.Core.World;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — canonical wasteland map-intelligence dashboard.
///
/// This panel is presentation-only. It projects the live WastelandMapSystem
/// read model and uses ExpeditionHostSession only for active-sortie count.
/// It never invents sector IDs, radiation rates, route reachability, or
/// dispatch commands. Selecting a row routes to the existing map-detail
/// surface through <see cref="OnLocationSelected"/>.
/// </summary>
public partial class MapAtlasPanel : Control, IBindablePanel
{
    public event Action? OnClose;
    public event Action<string>? OnLocationSelected;

    private sealed class AtlasLocation
    {
        public string Id = string.Empty;
        public string Display = string.Empty;
        public MapFogState FogState;
        public MapNodeStatusKind Status;
        public MapNodeDanger Danger;
        public float PositionX;
        public float PositionY;
        public string Intel = string.Empty;
        public bool Routable;
    }

    private AshfallDashboardShell _shell = null!;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _grid;
    private VBoxContainer _detailBox = null!;
    private int _selectedIndex = -1;

    private ExpeditionHostSession? _expeditionHost;
    private WorldHostSession? _worldHost;
    private readonly List<AtlasLocation> _locations = new();

    public bool IsBound => _expeditionHost != null && _worldHost != null;

    public void Bind(ExpeditionHostSession expeditionHost, WorldHostSession? worldHost = null)
    {
        Unsubscribe();

        _expeditionHost = expeditionHost;
        _worldHost = worldHost;

        if (_expeditionHost != null)
            _expeditionHost.StateChanged += HandleSourceChanged;

        if (_worldHost?.WastelandMap != null)
        {
            var map = _worldHost.WastelandMap;
            map.OnNodeDiscovered += HandleMapNode;
            map.OnNodeKnowledgeChanged += HandleMapKnowledge;
            map.OnNodeCompleted += HandleMapNode;
            map.OnNodeLockChanged += HandleMapLock;
            map.OnMarkersChanged += HandleMarkersChanged;
        }

        ReloadLocations();
        RefreshView();
    }

    private void Unsubscribe()
    {
        if (_expeditionHost != null)
            _expeditionHost.StateChanged -= HandleSourceChanged;

        if (_worldHost?.WastelandMap != null)
        {
            var map = _worldHost.WastelandMap;
            map.OnNodeDiscovered -= HandleMapNode;
            map.OnNodeKnowledgeChanged -= HandleMapKnowledge;
            map.OnNodeCompleted -= HandleMapNode;
            map.OnNodeLockChanged -= HandleMapLock;
            map.OnMarkersChanged -= HandleMarkersChanged;
        }
    }

    private void HandleSourceChanged()
    {
        ReloadLocations();
        RefreshView();
    }

    private void HandleMapNode(string _)
        => HandleSourceChanged();

    private void HandleMapKnowledge(string _, MapFogState __)
        => HandleSourceChanged();

    private void HandleMapLock(string _, bool __)
        => HandleSourceChanged();

    private void HandleMarkersChanged()
        => HandleSourceChanged();

    private void ReloadLocations()
    {
        _locations.Clear();
        var map = _worldHost?.WastelandMap;
        if (map == null) return;

        for (int i = 0; i < map.Nodes.Count; i++)
        {
            var node = map.Nodes[i];
            if (node == null || string.IsNullOrEmpty(node.Id)) continue;

            var intel = map.GetNodeIntel(node.Id);
            if (intel == null || intel.FogState == MapFogState.Unknown)
                continue; // Unknown geography stays hidden; no presentation leak.

            _locations.Add(new AtlasLocation
            {
                Id = intel.NodeId,
                Display = intel.DisplayName,
                FogState = intel.FogState,
                Status = map.ResolveNodeStatus(node.Id),
                Danger = intel.Danger,
                PositionX = intel.PositionX,
                PositionY = intel.PositionY,
                Intel = intel.LootDescription ?? string.Empty,
                Routable = intel.Routable
            });
        }

        _locations.Sort((a, b) =>
        {
            int y = a.PositionY.CompareTo(b.PositionY);
            if (y != 0) return y;
            int x = a.PositionX.CompareTo(b.PositionX);
            if (x != 0) return x;
            return string.CompareOrdinal(a.Id, b.Id);
        });

        if (_selectedIndex >= _locations.Count)
            _selectedIndex = -1;
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell(
            "Map Atlas // Canonical Wasteland Intelligence",
            minWidth: 1280,
            minHeight: 720);
        AddChild(_shell);
        _shell.SetAnchorsPreset(LayoutPreset.FullRect);
        _shell.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        _shell.SizeFlagsVertical = SizeFlags.ExpandFill;

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("known", "Known Nodes", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
        _statusRail.AddCard("routes", "Known Route Edges", "—", AshfallMetricCard.Criticality.Normal, minWidth: 145);
        _statusRail.AddCard("active", "Active Sorties", "—", AshfallMetricCard.Criticality.Caution, minWidth: 120);
        _statusRail.AddCard("hazards", "Hazard Nodes", "—", AshfallMetricCard.Criticality.Warn, minWidth: 120);
        _statusRail.AddCard("locked", "Locked Nodes", "—", AshfallMetricCard.Criticality.Critical, minWidth: 115);

        var columns = new[]
        {
            new AshfallDataGrid.Column { Header = "Location", MinWidth = 220, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Knowledge", MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Status", MinWidth = 105, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Danger", MinWidth = 85, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Map Position", MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Intel", MinWidth = 260, Alignment = AshfallDataGrid.ColumnAlign.Left },
        };
        _grid = new AshfallDataGrid(columns, showHeader: true, minWidth: 820, minHeight: 520);
        _grid.OnRowSelected += HandleRowSelected;

        var body = new HBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;

        var mapColumn = new VBoxContainer();
        mapColumn.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        mapColumn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        mapColumn.SizeFlagsVertical = SizeFlags.ExpandFill;
        mapColumn.AddChild(AshfallUiHelpers.MakeSectionHeader("KNOWN MAP INTELLIGENCE"));
        mapColumn.AddChild(AshfallUiHelpers.MakeMetadata(
            "Rows come from WastelandMapSystem.GetNodeIntel. Unknown nodes are intentionally hidden."));
        mapColumn.AddChild(_grid);
        body.AddChild(mapColumn);

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.CustomMinimumSize = new Vector2(330, 420);
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_detailBox);

        _shell.SetContent(body);
        RefreshView();
    }

    private void HandleRowSelected(int index)
    {
        if (index < 0 || index >= _locations.Count) return;
        _selectedIndex = index;
        RefreshDetail();
        OnLocationSelected?.Invoke(_locations[index].Id);
    }

    public void RefreshView()
    {
        RefreshStatusRail();
        BuildRows();
        RefreshDetail();
    }

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        var map = _worldHost?.WastelandMap;
        if (map == null)
        {
            SetStatusDashes();
            return;
        }

        var knownIds = new HashSet<string>(StringComparer.Ordinal);
        int hazardCount = 0;
        int lockedCount = 0;
        for (int i = 0; i < _locations.Count; i++)
        {
            var location = _locations[i];
            knownIds.Add(location.Id);
            if (location.Danger == MapNodeDanger.Medium || location.Danger == MapNodeDanger.High)
                hazardCount++;
            if (location.Status == MapNodeStatusKind.Locked)
                lockedCount++;
        }

        int knownRouteEdges = 0;
        for (int i = 0; i < map.Routes.Count; i++)
        {
            var route = map.Routes[i];
            if (route != null && knownIds.Contains(route.From) && knownIds.Contains(route.To))
                knownRouteEdges++;
        }

        int active = _expeditionHost?.Engine?.ActiveCount ?? 0;
        _statusRail.Set("known", _locations.Count.ToString(), AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("routes", knownRouteEdges.ToString(), AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("active", active.ToString(), active > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("hazards", hazardCount.ToString(), hazardCount > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("locked", lockedCount.ToString(), lockedCount > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
    }

    private void SetStatusDashes()
    {
        if (_statusRail == null) return;
        _statusRail.Set("known", "—", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("routes", "—", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("active", "—", AshfallMetricCard.Criticality.Caution);
        _statusRail.Set("hazards", "—", AshfallMetricCard.Criticality.Warn);
        _statusRail.Set("locked", "—", AshfallMetricCard.Criticality.Critical);
    }

    private void BuildRows()
    {
        if (_grid == null) return;
        if (_worldHost?.WastelandMap == null)
        {
            _grid.SetRows(BuildFixtureRows());
            return;
        }

        var rows = new List<AshfallDataGrid.Row>();
        for (int i = 0; i < _locations.Count; i++)
        {
            var location = _locations[i];
            rows.Add(new AshfallDataGrid.Row
            {
                Selectable = true,
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new(location.Display, AshfallDataGrid.CellState.Normal),
                    new(location.FogState.ToString(), KnowledgeState(location.FogState)),
                    new(location.Status.ToString(), StatusState(location.Status)),
                    new(location.Danger.ToString(), DangerState(location.Danger)),
                    new($"({location.PositionX:0}, {location.PositionY:0})", AshfallDataGrid.CellState.Muted),
                    new(string.IsNullOrEmpty(location.Intel) ? "—" : location.Intel, AshfallDataGrid.CellState.Muted),
                }
            });
        }

        if (rows.Count == 0)
            rows = BuildFixtureRows();

        _grid.SetRows(rows);
    }

    private static AshfallDataGrid.CellState KnowledgeState(MapFogState state)
        => state switch
        {
            MapFogState.Rumored => AshfallDataGrid.CellState.Warning,
            MapFogState.Surveyed => AshfallDataGrid.CellState.Normal,
            MapFogState.Visited => AshfallDataGrid.CellState.Normal,
            _ => AshfallDataGrid.CellState.Muted
        };

    private static AshfallDataGrid.CellState StatusState(MapNodeStatusKind status)
        => status switch
        {
            MapNodeStatusKind.Locked => AshfallDataGrid.CellState.Critical,
            MapNodeStatusKind.Available => AshfallDataGrid.CellState.Warning,
            MapNodeStatusKind.Completed => AshfallDataGrid.CellState.Muted,
            _ => AshfallDataGrid.CellState.Normal
        };

    private static AshfallDataGrid.CellState DangerState(MapNodeDanger danger)
        => danger switch
        {
            MapNodeDanger.High => AshfallDataGrid.CellState.Critical,
            MapNodeDanger.Medium => AshfallDataGrid.CellState.Warning,
            MapNodeDanger.Locked => AshfallDataGrid.CellState.Critical,
            _ => AshfallDataGrid.CellState.Normal
        };

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        AshfallUiHelpers.EmptyChildren(_detailBox);
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("LOCATION DETAIL"));
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());

        if (_worldHost?.WastelandMap == null)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Map atlas offline. Bind WorldHostSession and ExpeditionHostSession."));
            return;
        }

        if (_selectedIndex < 0 || _selectedIndex >= _locations.Count)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Select a known map row to open the canonical location detail."));
            return;
        }

        var location = _locations[_selectedIndex];
        var map = _worldHost.WastelandMap;
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("ID", location.Id,
            AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Knowledge", location.FogState.ToString(),
            AshfallUiHelpers.ToColor(DesignTheme.Warm)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Status", location.Status.ToString(),
            location.Status == MapNodeStatusKind.Locked
                ? AshfallUiHelpers.ToColor(DesignTheme.Critical)
                : AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Danger", location.Danger.ToString(),
            location.Danger == MapNodeDanger.High || location.Danger == MapNodeDanger.Locked
                ? AshfallUiHelpers.ToColor(DesignTheme.Critical)
                : location.Danger == MapNodeDanger.Medium
                    ? AshfallUiHelpers.ToColor(DesignTheme.Entropy)
                    : AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow(
            "Map Position",
            $"({location.PositionX:0}, {location.PositionY:0})",
            AshfallUiHelpers.ToColor(DesignTheme.Dim)));

        var routes = map.GetRoutesFrom(location.Id);
        float totalDistanceKm = 0f;
        for (int i = 0; i < routes.Count; i++)
            totalDistanceKm += routes[i].DistanceKm;

        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow(
            "Known outgoing routes",
            location.Routable ? $"{routes.Count} / {totalDistanceKm:0.0} km authored" : "unconfirmed",
            AshfallUiHelpers.ToColor(location.Routable ? DesignTheme.Ozone : DesignTheme.Dim)));

        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeSubsectionHeader("CANONICAL INTEL"));
        _detailBox.AddChild(AshfallUiHelpers.MakeSmall(
            string.IsNullOrEmpty(location.Intel) ? "No confirmed loot intelligence." : location.Intel,
            autowrap: true));
        _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
            "Dispatch remains in Wasteland Expeditions; this atlas does not fabricate command affordances."));
    }

    /// <summary>
    /// Truthful unbound/empty-state fixture. It intentionally contains no
    /// canonical-looking locations, sector IDs, radiation numbers, or commands.
    /// </summary>
    internal static List<AshfallDataGrid.Row> BuildFixtureRows()
        => new()
        {
            new AshfallDataGrid.Row
            {
                Selectable = false,
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new("— map intelligence unavailable —", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("Bind live world state", AshfallDataGrid.CellState.Muted),
                }
            }
        };

    public void Open()
    {
        Visible = true;
        ReloadLocations();
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
        Unsubscribe();
        _expeditionHost = null;
        _worldHost = null;
        _locations.Clear();
        _selectedIndex = -1;
    }

    public override void _ExitTree()
    {
        Unbind();
        base._ExitTree();
    }
}
