using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Research Atlas Dashboard.
/// Authoritative live knowledge atlas reading ResearchHostSession.
/// Displays all 56 knowledge nodes, discipline filters, prerequisite trees,
/// downstream dependent unlocks, and interactive research commands with zero fixture data.
/// </summary>
public partial class ResearchAtlasPanel : Control, IBindablePanel
{
    public event Action? OnClose;
    public event Action<string>? OnNodeSelected;

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _nodesGrid;
    private VBoxContainer _detailBox = null!;
    private int _selectedIndex = -1;
    private string _scopeFilter = "all";

    private readonly List<ResearchKnowledgeDef> _visibleNodes = new();
    private ResearchHostSession? _host;

    public bool IsBound => _host != null;
    public int VisibleRowCount => _visibleNodes.Count;

    public void Bind(ResearchHostSession host)
    {
        if (_host != null)
            _host.StateChanged -= RefreshView;

        _host = host;
        if (_host != null)
            _host.StateChanged += RefreshView;

        RefreshView();
    }

    private const string DefaultDisciplineIconPath = AshfallUiHelpers.FallbackIconPath;

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("Research Atlas // Knowledge Nodes · Breakthroughs · R&D Queue", minWidth: 1280, minHeight: 720);
        SetContentRoot(_shell);

        var scopes = new[]
        {
            new AshfallSidebar.Item
            {
                Id = "all",
                Label = "All Disciplines",
                Hint = "all disciplines",
                Tooltip = "Display all knowledge nodes across all disciplines.",
                IconPath = DefaultDisciplineIconPath
            },
            new AshfallSidebar.Item
            {
                Id = "survival",
                Label = "Survival",
                Hint = "water · food · habitability",
                Tooltip = "Filter survival life-support nodes: water purification, agriculture, and life support.",
                IconPath = DefaultDisciplineIconPath
            },
            new AshfallSidebar.Item
            {
                Id = "engineering",
                Label = "Engineering",
                Hint = "power · shielding · hardware",
                Tooltip = "Filter engineering & hardware nodes: power generation, radiation shielding, and tools.",
                IconPath = DefaultDisciplineIconPath
            },
            new AshfallSidebar.Item
            {
                Id = "medical",
                Label = "Medical",
                Hint = "radiation · triage · medicine",
                Tooltip = "Filter medical nodes: radiation therapy, triage surgical procedures, and medicine.",
                IconPath = DefaultDisciplineIconPath
            },
            new AshfallSidebar.Item
            {
                Id = "science",
                Label = "Science",
                Hint = "radio · cipher · spectrometry",
                Tooltip = "Filter science & intelligence nodes: directional radio, signal analysis, and optics.",
                IconPath = DefaultDisciplineIconPath
            },
            new AshfallSidebar.Item
            {
                Id = "scavenging",
                Label = "Scavenging",
                Hint = "salvage · logistics · routes",
                Tooltip = "Filter scavenging & logistics nodes: route mapping, salvage yield, and load bearing.",
                IconPath = DefaultDisciplineIconPath
            },
            new AshfallSidebar.Item
            {
                Id = "combat",
                Label = "Combat",
                Hint = "ballistics · tactics · defense",
                Tooltip = "Filter combat doctrine nodes: perimeter defense tactics and weapons handling.",
                IconPath = DefaultDisciplineIconPath
            },
        };
        _sidebar = _shell.SetSidebar(scopes, "Discipline Filter", "all");
        _sidebar.OnSelected += HandleSidebar;

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("total",         "Total",         "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("unlocked",      "Unlocked",      "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("active",        "Active",        "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
        _statusRail.AddCard("completed",     "Completed",     "—", AshfallMetricCard.Criticality.Caution, minWidth: 110);
        _statusRail.AddCard("remaining",     "Remaining",     "—", AshfallMetricCard.Criticality.Warn, minWidth: 130);
        _statusRail.AddCard("breakthroughs", "Breakthroughs", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);

        var body = new HBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;

        var gridCol = new VBoxContainer();
        gridCol.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        gridCol.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        gridCol.SizeFlagsVertical = SizeFlags.ExpandFill;
        gridCol.AddChild(AshfallUiHelpers.MakeSectionHeader("KNOWLEDGE CATALOG"));

        var colsNodes = new[]
        {
            new AshfallDataGrid.Column { Header = "Name",         MinWidth = 220, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Category",     MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Days",         MinWidth = 60,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Status",       MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Breakthrough", MinWidth = 200, Alignment = AshfallDataGrid.ColumnAlign.Left },
        };
        _nodesGrid = new AshfallDataGrid(colsNodes, showHeader: true, minWidth: 720, minHeight: 400);
        _nodesGrid.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        _nodesGrid.SizeFlagsVertical = SizeFlags.ExpandFill;
        _nodesGrid.OnRowSelected += HandleRowSelected;
        gridCol.AddChild(_nodesGrid);

        body.AddChild(gridCol);

        var detailScroll = new ScrollContainer();
        detailScroll.CustomMinimumSize = new Vector2(360, 400);
        detailScroll.SizeFlagsVertical = SizeFlags.ExpandFill;

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        detailScroll.AddChild(_detailBox);

        body.AddChild(detailScroll);

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
        if (idx >= 0 && idx < _visibleNodes.Count)
        {
            var node = _visibleNodes[idx];
            OnNodeSelected?.Invoke(node.id);
        }
        RefreshDetail();
    }

    public void RefreshView()
    {
        RefreshStatusRail();
        BuildNodesGrid();
        RefreshDetail();
    }

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        if (_host == null)
        {
            _statusRail.Set("total",         "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("unlocked",      "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("active",        "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("completed",     "—", AshfallMetricCard.Criticality.Caution);
            _statusRail.Set("remaining",     "—", AshfallMetricCard.Criticality.Warn);
            _statusRail.Set("breakthroughs", "—", AshfallMetricCard.Criticality.Normal);
            return;
        }

        int breakthroughNodes = 0;
        foreach (var kv in _host.Catalog)
        {
            if (!string.IsNullOrEmpty(kv.Value.breakthroughItem))
                breakthroughNodes++;
        }

        _statusRail.Set("total",         _host.CatalogCount.ToString(), AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("unlocked",      _host.UnlockedCount.ToString(), AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("active",        _host.ActiveResearchDays > 0 ? _host.ActiveResearchId : "idle", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("completed",     _host.CompletedCount.ToString(), AshfallMetricCard.Criticality.Caution);
        _statusRail.Set("remaining",     System.Math.Max(0, _host.CatalogCount - _host.CompletedCount).ToString(), AshfallMetricCard.Criticality.Warn);
        _statusRail.Set("breakthroughs", breakthroughNodes.ToString(), AshfallMetricCard.Criticality.Normal);
    }

    private void BuildNodesGrid()
    {
        if (_nodesGrid == null) return;
        _visibleNodes.Clear();

        if (_host == null)
        {
            _nodesGrid.SetRows(new List<AshfallDataGrid.Row>
            {
                new AshfallDataGrid.Row
                {
                    Cells = new List<AshfallDataGrid.Cell>
                    {
                        new("— engine offline —", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                    },
                    Selectable = false
                }
            });
            return;
        }

        var sorted = _host.Catalog.Values
            .OrderBy(n => n.category, StringComparer.Ordinal)
            .ThenBy(n => n.id, StringComparer.Ordinal);

        var rows = new List<AshfallDataGrid.Row>();

        foreach (var def in sorted)
        {
            if (_scopeFilter != "all" && !string.Equals(def.category, _scopeFilter, StringComparison.OrdinalIgnoreCase))
                continue;

            _visibleNodes.Add(def);

            bool isCompleted = def.isCompleted || _host.Engine.State.completedIds.Contains(def.id);
            bool isActive = string.Equals(_host.ActiveResearchId, def.id, StringComparison.Ordinal);
            var elig = _host.GetEligibility(def.id);

            string statusText;
            AshfallDataGrid.CellState statusState;

            if (isCompleted)
            {
                statusText = "COMPLETED";
                statusState = AshfallDataGrid.CellState.Positive;
            }
            else if (isActive)
            {
                statusText = $"ACTIVE ({_host.ActiveResearchDays}d)";
                statusState = AshfallDataGrid.CellState.Caution;
            }
            else if (elig.CanStart)
            {
                statusText = "AVAILABLE";
                statusState = AshfallDataGrid.CellState.Normal;
            }
            else
            {
                statusText = "LOCKED";
                statusState = AshfallDataGrid.CellState.Muted;
            }

            string daysText = isActive ? $"{_host.ActiveResearchDays}d" : $"{def.daysToComplete}d";
            string breakthroughText = string.IsNullOrEmpty(def.breakthroughItem) ? "—" : def.breakthroughItem;
            var breakthroughState = string.IsNullOrEmpty(def.breakthroughItem) ? AshfallDataGrid.CellState.Muted : AshfallDataGrid.CellState.Selected;

            var cells = new List<AshfallDataGrid.Cell>
            {
                new(def.displayName, AshfallDataGrid.CellState.Normal),
                new(def.category.ToUpperInvariant(), AshfallDataGrid.CellState.Muted),
                new(daysText, AshfallDataGrid.CellState.Muted),
                new(statusText, statusState),
                new(breakthroughText, breakthroughState),
            };

            rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
        }

        if (rows.Count == 0)
        {
            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new("— no nodes match filter —", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                },
                Selectable = false
            });
        }

        _nodesGrid.SetRows(rows);
    }

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        AshfallUiHelpers.EmptyChildren(_detailBox);

        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("NODE DETAIL"));
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());

        if (_host == null)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Research engine offline. Bind a ResearchHostSession to see live knowledge nodes and breakthroughs."));
            return;
        }

        if (_selectedIndex < 0 || _selectedIndex >= _visibleNodes.Count)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Select a knowledge node from the catalog to inspect requirements, rewards, and dependents."));
            return;
        }

        var node = _visibleNodes[_selectedIndex];
        var elig = _host.GetEligibility(node.id);
        var dependents = _host.GetDependents(node.id);
        bool isCompleted = node.isCompleted || _host.Engine.State.completedIds.Contains(node.id);
        bool isActive = string.Equals(_host.ActiveResearchId, node.id, StringComparison.Ordinal);

        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Name", node.displayName, AshfallUiHelpers.ToColor(DesignTheme.Warm)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("ID", node.id, AshfallUiHelpers.ToColor(DesignTheme.Muted)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Category", node.category.ToUpperInvariant(), AshfallUiHelpers.ToColor(DesignTheme.Pale)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Duration", $"{node.daysToComplete} days", AshfallUiHelpers.ToColor(DesignTheme.Pale)));

        string statusDesc = isCompleted ? "COMPLETED" :
                            isActive ? $"IN PROGRESS ({_host.ActiveResearchDays}d remaining)" :
                            elig.CanStart ? "AVAILABLE TO RESEARCH" :
                            $"LOCKED ({_host.FormatFailureCode(elig)})";
        var statusColor = isCompleted ? DesignTheme.Success :
                          isActive ? DesignTheme.Warning :
                          elig.CanStart ? DesignTheme.Pale : DesignTheme.Entropy;

        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Status", statusDesc, AshfallUiHelpers.ToColor(statusColor)));

        if (!string.IsNullOrEmpty(node.description))
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("DESCRIPTION"));
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(node.description));
        }

        if (!string.IsNullOrEmpty(node.breakthroughItem))
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("BREAKTHROUGH REWARD"));
            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Item", node.breakthroughItem, AshfallUiHelpers.ToColor(DesignTheme.Cyan)));
        }

        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("PREREQUISITES"));
        if (node.prerequisites == null || node.prerequisites.Length == 0)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("None (Foundational Node)"));
        }
        else
        {
            foreach (var prereq in node.prerequisites)
            {
                bool pDone = _host.Catalog.TryGetValue(prereq, out var pDef) && (pDef.isCompleted || _host.Engine.State.completedIds.Contains(prereq));
                string pName = pDef?.displayName ?? prereq;
                string check = pDone ? "[✓]" : "[✗]";
                var pColor = pDone ? DesignTheme.Success : DesignTheme.Entropy;
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow(check, pName, AshfallUiHelpers.ToColor(pColor)));
            }
        }

        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("DOWNSTREAM DEPENDENTS"));
        if (dependents.Count == 0)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("None (Terminal / Capstone Technology)"));
        }
        else
        {
            foreach (var depId in dependents)
            {
                _host.Catalog.TryGetValue(depId, out var depDef);
                string depName = depDef?.displayName ?? depId;
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("↳", depName, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            }
        }

        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());

        var startBtn = new Button
        {
            CustomMinimumSize = new Vector2(0, 36),
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
        };

        if (elig.CanStart)
        {
            startBtn.Text = "START RESEARCH";
            startBtn.Disabled = false;
            startBtn.Pressed += () =>
            {
                if (_host.TryStart(node.id))
                {
                    RefreshView();
                }
            };
        }
        else
        {
            startBtn.Disabled = true;
            startBtn.Text = elig.Code switch
            {
                ResearchEligibilityCode.AlreadyCompleted => "RESEARCH COMPLETED",
                ResearchEligibilityCode.AlreadyActive => "IN PROGRESS",
                ResearchEligibilityCode.AnotherResearchActive => "ANOTHER RESEARCH ACTIVE",
                ResearchEligibilityCode.MissingPrerequisites => "PREREQUISITES REQUIRED",
                _ => "NOT ELIGIBLE"
            };
        }
        _detailBox.AddChild(startBtn);
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
        }
    }

    public override void _ExitTree()
    {
        Unbind();
        base._ExitTree();
    }
}
