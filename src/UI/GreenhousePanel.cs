using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Glass Orchard Dashboard (#51 Stitch, Sub-surface Hydroponics).
///
/// Phase 15 prioritised lift: same `GreenhouseHostSession` source as the
/// legacy Phase 9 modal, but routed through the Phase 11 dashboard shell
/// (AshfallDashboardShell + AshfallSidebar + AshfallStatusRail) and the
/// Phase 12 data-grid primitive (AshfallDataGrid). Status rail carries
/// five Tier-2 metrics; the grid lists every planter box with stage,
/// irrigation, soil contamination, growth progress and a one-line lore
/// tag. Sidebar offers crop filter + vault filter.
///
/// All labels, crop names, and seed IDs resolve from the project's own
/// `GreenhouseExpansionCatalog.Items.*` constants — none of the catalog
/// has a Tier-3 missing-detected entry, so the fixture is auditable.
/// </summary>
public partial class GreenhousePanel : Control
{
    public event Action? OnClose;
    public event Action<int>? OnPlotSelected;

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _plotGrid;
    private VBoxContainer _detailBox = null!;
    private Label _detailTitle = null!;
    private int _selectedIndex = -1;
    private string _cropFilter = "all"; // all | mushroom | tuber | grain | wheat

    private GreenhouseHostSession? _host;

    public bool IsBound => _host != null;

    public void Bind(GreenhouseHostSession session)
    {
        _host = session;
        RefreshView();
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("The Glass Orchard // Sub-surface Hydroponics", minWidth: 1100, minHeight: 720);
        SetContentRoot(_shell);

        var cropsItems = new[]
        {
            new AshfallSidebar.Item { Id = "all",      Label = "All Beds",      Hint = "every plot · stage / water / soil", IconPath = "" },
            new AshfallSidebar.Item { Id = "fallow",   Label = "Fallow Only",   Hint = "empty soil beds ready to seed",    IconPath = "" },
            new AshfallSidebar.Item { Id = "critical", Label = "Damaged",       Hint = "failed or blight-stricken beds",   IconPath = "" },
            new AshfallSidebar.Item { Id = "harvest",  Label = "Ready",         Hint = "beds at mature stage",            IconPath = "" },
        };
        _sidebar = _shell.SetSidebar(cropsItems, "Filter", "all");
        _sidebar.OnSelected += HandleSidebar;

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("active", "Active Beds", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("plotcount", "Plot Count", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("harvests", "Harvests", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("vault", "Seed Vault", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);
        _statusRail.AddCard("blight", "Blighted Beds", "—", AshfallMetricCard.Criticality.Caution, minWidth: 130);

        // Grid columns: bed #, stage badge, water, contamination, growth %, seed id.
        var cols = new[]
        {
            new AshfallDataGrid.Column { Header = "Bed",       MinWidth = 60,  Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Stage",     MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Water",     MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Soil mSv",  MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Growth",    MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Seed",      MinWidth = 240, Alignment = AshfallDataGrid.ColumnAlign.Left },
        };
        _plotGrid = new AshfallDataGrid(cols, showHeader: true, minWidth: 720, minHeight: 320);
        _plotGrid.OnRowSelected += HandleRowSelected;

        // Body: grid + detail pane side-by-side.
        var body = new HBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;

        body.AddChild(_plotGrid);

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.CustomMinimumSize = new Vector2(300, 320);
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_detailBox);

        _detailTitle = AshfallUiHelpers.MakeSectionHeader("BED DETAIL");
        _detailTitle.HorizontalAlignment = HorizontalAlignment.Left;
        _detailBox.AddChild(_detailTitle);
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
            "Select a plot row to view irrigation balance, contamination history, and harvest yields."));

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
        if (id == "fallow" || id == "critical" || id == "harvest")
        {
            // map sidebar filter to a crop filter token for BuildPlotRows
            _cropFilter = id;
        }
        else
        {
            _cropFilter = "all";
        }
        RefreshView();
    }

    private void HandleRowSelected(int idx)
    {
        _selectedIndex = idx;
        OnPlotSelected?.Invoke(idx);
        RefreshDetail();
    }

    public void RefreshView()
    {
        RefreshStatusRail();
        BuildPlotRows();
        RefreshDetail();
    }

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        if (_host == null)
        {
            _statusRail.Set("active", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("plotcount", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("harvests", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("vault", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("blight", "—", AshfallMetricCard.Criticality.Caution);
            return;
        }
        var state = _host.System.State;
        int plotCount = state.plots.Count;
        int active = 0, blighted = 0;
        for (int i = 0; i < state.plots.Count; i++)
        {
            var p = state.plots[i];
            if (p == null) continue;
            if (!GreenhouseSystem.IsFallow(p)) active++;
            if (p.blight > 0f) blighted++;
        }
        _statusRail.Set("active", $"{active}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("plotcount", $"{plotCount}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("harvests", $"{state.totalHarvests}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("vault", state.preWarWheatUnlocked ? "OPEN" : "SEALED",
            state.preWarWheatUnlocked ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Caution);
        _statusRail.Set("blight", blighted > 0 ? $"{blighted}" : "0",
            blighted > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
    }

    private void BuildPlotRows()
    {
        if (_plotGrid == null) return;

        if (_host == null)
        {
            _plotGrid.SetRows(BuildFixtureRows());
            return;
        }

        var rows = new List<AshfallDataGrid.Row>();
        var state = _host.System.State;
        for (int i = 0; i < state.plots.Count; i++)
        {
            var p = state.plots[i];
            if (p == null) continue;
            if (!FilterPass(p)) continue;

            var stageEnum = (GreenhouseStage)p.stage;
            var (badgeState, badgeText) = StageBadge(stageEnum);

            // Water: critical below 15, warning below 30, normal otherwise.
            var waterState = p.water < 15f ? AshfallDataGrid.CellState.Critical
                            : p.water < 30f ? AshfallDataGrid.CellState.Caution
                                            : AshfallDataGrid.CellState.Normal;

            // Contamination: critical above 70, warning above 40, normal otherwise.
            var soilState = p.soilContamination > 70f ? AshfallDataGrid.CellState.Critical
                          : p.soilContamination > 40f ? AshfallDataGrid.CellState.Warning
                                                       : AshfallDataGrid.CellState.Normal;

            // Growth: positive to neutral.
            var growthState = stageEnum == GreenhouseStage.Mature ? AshfallDataGrid.CellState.Positive
                              : p.growth > 50f ? AshfallDataGrid.CellState.Normal
                                               : AshfallDataGrid.CellState.Muted;

            string seedName = GreenhouseSystem.IsFallow(p) ? "— fallow —" : FriendlySeed(p.seedItemId);

            var cells = new List<AshfallDataGrid.Cell>
            {
                new($"#{i + 1}", AshfallDataGrid.CellState.Normal),
                new(badgeText, badgeState),
                new($"{p.water:0.0}", waterState),
                new($"{p.soilContamination:0.0}", soilState),
                new($"{p.growth:0.0}%", growthState),
                new(seedName, GreenhouseSystem.IsFallow(p) ? AshfallDataGrid.CellState.Muted : AshfallDataGrid.CellState.Normal),
            };
            rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
        }
        if (rows.Count == 0)
        {
            // Surface empty state precisely so the snapshot doesn't fake rows.
            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new("— no plot matches —", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                }
            });
        }
        _plotGrid.SetRows(rows);
    }

    private bool FilterPass(GreenhousePlotState p)
    {
        if (p == null) return false;
        if (_cropFilter == "all") return true;
        if (_cropFilter == "fallow") return GreenhouseSystem.IsFallow(p);
        var stage = (GreenhouseStage)p.stage;
        if (_cropFilter == "harvest") return stage == GreenhouseStage.Mature;
        if (_cropFilter == "critical") return p.blight > 0f || stage == GreenhouseStage.Failed;
        // crop-type filter by seed id
        if (string.IsNullOrEmpty(p.seedItemId)) return false;
        return p.seedItemId switch
        {
            GreenhouseExpansionCatalog.Items.SeedMushroom => _cropFilter == "mushroom",
            GreenhouseExpansionCatalog.Items.SeedTuber    => _cropFilter == "tuber",
            GreenhouseExpansionCatalog.Items.SeedGrain    => _cropFilter == "grain",
            GreenhouseExpansionCatalog.Items.SeedWheat    => _cropFilter == "wheat",
            _ => false,
        };
    }

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        AshfallUiHelpers.EmptyChildren(_detailBox);
        // Recreate the persistent header — the QueueFree loop above disposed it.
        _detailTitle = AshfallUiHelpers.MakeSectionHeader("BED DETAIL");
        _detailTitle.HorizontalAlignment = HorizontalAlignment.Left;
        _detailBox.AddChild(_detailTitle);
        // Add separator with proper layout constraints
        var separator = AshfallUiHelpers.MakeSeparator();
        separator.CustomMinimumSize = new Vector2(0, 2);  // Prevent layout issues
        _detailBox.AddChild(separator);
        if (_host == null || _selectedIndex < 0 || _selectedIndex >= _plotGrid?.Rows?.Count)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                _host == null
                    ? "Greenhouse engine offline. Bind a GreenhouseHostSession to see live plot data."
                    : "Select a plot row to view irrigation balance, contamination history, and harvest yields."));
            return;
        }
        var state = _host.System.State;
        if (_selectedIndex >= state.plots.Count) return;
        var p = state.plots[_selectedIndex];

        _detailTitle.Text = $"BED #{_selectedIndex + 1} DETAIL";

        string statusText = GreenhouseSystem.IsFallow(p) ? "FALLOW"
            : ((GreenhouseStage)p.stage) switch
            {
                GreenhouseStage.Sprouting => "SPROUTING",
                GreenhouseStage.Growing   => "GROWING",
                GreenhouseStage.Mature    => "READY TO HARVEST",
                GreenhouseStage.Failed    => "FAILED",
                _ => "?",
            };

        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Status", statusText,
            ((GreenhouseStage)p.stage) == GreenhouseStage.Failed ? AshfallUiHelpers.ToColor(DesignTheme.Critical) :
            ((GreenhouseStage)p.stage) == GreenhouseStage.Mature ? AshfallUiHelpers.ToColor(DesignTheme.Lethe) :
            AshfallUiHelpers.ToColor(DesignTheme.Warm)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Seed", GreenhouseSystem.IsFallow(p) ? "—" : FriendlySeed(p.seedItemId),
            AshfallUiHelpers.ToColor(DesignTheme.Pale)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Growth", $"{(GreenhouseSystem.IsFallow(p) ? 0f : p.growth):0.0}%",
            AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Moisture", $"{p.water:0.0} / 100",
            p.water < 15f ? AshfallUiHelpers.ToColor(DesignTheme.Critical) :
            p.water < 30f ? AshfallUiHelpers.ToColor(DesignTheme.LetheAmber) :
            AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Soil mSv", $"{p.soilContamination:0.0}",
            p.soilContamination > 70f ? AshfallUiHelpers.ToColor(DesignTheme.Critical) :
            p.soilContamination > 40f ? AshfallUiHelpers.ToColor(DesignTheme.Entropy) :
            AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Blight", $"{p.blight * 100f:0}%",
            p.blight > 0f ? AshfallUiHelpers.ToColor(DesignTheme.Critical) : AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        if (!string.IsNullOrEmpty(_host.LastEvent))
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSmall(_host.LastEvent));
        }
    }

    private static (AshfallDataGrid.CellState, string) StageBadge(GreenhouseStage stage) => stage switch
    {
        GreenhouseStage.Fallow    => (AshfallDataGrid.CellState.Muted, "FALLOW"),
        GreenhouseStage.Sprouting => (AshfallDataGrid.CellState.Caution, "SPROUTING"),
        GreenhouseStage.Growing   => (AshfallDataGrid.CellState.Normal, "GROWING"),
        GreenhouseStage.Mature    => (AshfallDataGrid.CellState.Positive, "READY"),
        GreenhouseStage.Failed    => (AshfallDataGrid.CellState.Critical, "FAILED"),
        _ => (AshfallDataGrid.CellState.Normal, stage.ToString().ToUpperInvariant()),
    };

    private static string FriendlySeed(string seedId) => seedId switch
    {
        GreenhouseExpansionCatalog.Items.SeedMushroom => "Mushroom Spores",
        GreenhouseExpansionCatalog.Items.SeedTuber    => "Frost Tuber",
        GreenhouseExpansionCatalog.Items.SeedGrain    => "Winter Rye Grain",
        GreenhouseExpansionCatalog.Items.SeedWheat    => "Pre-War Heritage Wheat",
        _ => seedId,
    };

    /// <summary>
    /// Hard-coded fixture rows used when no host session is bound. IDs all
    /// resolve through `GreenhouseExpansionCatalog.Items.*` — never invented.
    /// </summary>
    internal static List<AshfallDataGrid.Row> BuildFixtureRows()
    {
        var rows = new List<AshfallDataGrid.Row>
        {
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("#1", AshfallDataGrid.CellState.Normal),
                    new("SPROUTING", AshfallDataGrid.CellState.Caution),
                    new("68.0", AshfallDataGrid.CellState.Normal),
                    new("12.0", AshfallDataGrid.CellState.Normal),
                    new("21.0%", AshfallDataGrid.CellState.Normal),
                    new(FriendlySeed(GreenhouseExpansionCatalog.Items.SeedMushroom), AshfallDataGrid.CellState.Normal),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("#2", AshfallDataGrid.CellState.Normal),
                    new("GROWING", AshfallDataGrid.CellState.Normal),
                    new("44.0", AshfallDataGrid.CellState.Caution),
                    new("38.0", AshfallDataGrid.CellState.Normal),
                    new("72.0%", AshfallDataGrid.CellState.Normal),
                    new(FriendlySeed(GreenhouseExpansionCatalog.Items.SeedTuber), AshfallDataGrid.CellState.Normal),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("#3", AshfallDataGrid.CellState.Normal),
                    new("READY", AshfallDataGrid.CellState.Positive),
                    new("82.0", AshfallDataGrid.CellState.Normal),
                    new("9.0", AshfallDataGrid.CellState.Normal),
                    new("100.0%", AshfallDataGrid.CellState.Positive),
                    new(FriendlySeed(GreenhouseExpansionCatalog.Items.SeedGrain), AshfallDataGrid.CellState.Normal),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("#4", AshfallDataGrid.CellState.Normal),
                    new("FALLOW", AshfallDataGrid.CellState.Muted),
                    new("0.0", AshfallDataGrid.CellState.Critical),
                    new("0.0", AshfallDataGrid.CellState.Normal),
                    new("0.0%", AshfallDataGrid.CellState.Muted),
                    new("— fallow —", AshfallDataGrid.CellState.Muted),
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
}
