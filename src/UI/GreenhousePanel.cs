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

    // Plan 22 UI gap register
    private string? _pendingPicker;

    public bool IsBound => _host != null;

    public void Bind(GreenhouseHostSession session)
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

        _shell = new AshfallDashboardShell("The Glass Orchard // Sub-surface Hydroponics", minWidth: 1100, minHeight: 720);
        SetContentRoot(_shell);

        var cropsItems = new[]
        {
            new AshfallSidebar.Item { Id = "all",      Label = "All Beds",      Hint = "every plot · stage / water / soil", IconPath = "" },
            new AshfallSidebar.Item { Id = "fallow",   Label = "Fallow Only",   Hint = "empty soil beds ready to seed",    IconPath = "" },
            new AshfallSidebar.Item { Id = "critical", Label = "Damaged",       Hint = "failed or blight-stricken beds",   IconPath = "" },
            new AshfallSidebar.Item { Id = "harvest",  Label = "Ready",         Hint = "beds at mature stage",            IconPath = "" },
            new AshfallSidebar.Item { Id = "apiary",   Label = "Apiary (Hives)", Hint = "colony health · pollination · honey & wax", IconPath = "" },
        };
        _sidebar = _shell.SetSidebar(cropsItems, "Filter", "all");
        _sidebar.OnSelected += HandleSidebar;

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("active", "Active Beds", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("plotcount", "Plot Count", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("harvests", "Harvests", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("vault", "Seed Vault", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);
        _statusRail.AddCard("blight", "Blighted Beds", "—", AshfallMetricCard.Criticality.Caution, minWidth: 130);

        // Plan 22 GAP-3: supply stock strip — only items that exist in the
        // current catalog (concurrent worker trimmed the supply list).
        _statusRail.AddCard("sup_glass", "Glass", "—", AshfallMetricCard.Criticality.Normal, minWidth: 80);
        _statusRail.AddCard("sup_blight", "Blight", "—", AshfallMetricCard.Criticality.Normal, minWidth: 80);
        _statusRail.AddCard("sup_medium", "Medium", "—", AshfallMetricCard.Criticality.Normal, minWidth: 80);

        // Grid columns: bed #, stage badge, water, contamination, growth %,
        // readiness, dry warning (GAP-7), seed id.
        var cols = new[]
        {
            new AshfallDataGrid.Column { Header = "Bed",       MinWidth = 60,  Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Stage",     MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Water",     MinWidth = 70,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Soil mSv",  MinWidth = 70,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Growth",    MinWidth = 70,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Ready",     MinWidth = 60,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Dry",       MinWidth = 50,  Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Seed",      MinWidth = 180, Alignment = AshfallDataGrid.ColumnAlign.Left },
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
        if (id == "fallow" || id == "critical" || id == "harvest" || id == "apiary")
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
        RefreshSupplyRail();
        BuildPlotRows();
        RefreshDetail();
    }

    /// <summary>
    /// Plan 22 GAP-3: supply stock strip. Counts from the live inventory host;
    /// zero stock renders in caution so a spent supply stays visible.
    /// </summary>
    private void RefreshSupplyRail()
    {
        if (_statusRail == null) return;
        if (_cropFilter == "apiary") return;
        var inv = _host?.InventoryHost?.Inventory;
        SetSupply("sup_glass", GreenhouseExpansionCatalog.Items.LeadGlassPane, inv);
        SetSupply("sup_blight", GreenhouseExpansionCatalog.Items.BlightTreatment, inv);
        SetSupply("sup_medium", GreenhouseExpansionCatalog.Items.GrowMedium, inv);
    }

    private void SetSupply(string cardId, string itemId, Ashfall.Core.Inventory.Inventory? inv)
    {
        if (inv == null)
        {
            _statusRail!.Set(cardId, "—", AshfallMetricCard.Criticality.Normal);
            return;
        }
        int count = inv.CountById(itemId);
        _statusRail!.Set(cardId, $"{count}",
            count > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Caution);
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

        if (_cropFilter == "apiary")
        {
            var hive = _host.Apiculture.GetHive("hive_01");
            if (hive != null && !hive.isDead)
            {
                float bonus = _host.Apiculture.GetPollinationBonus("plot_0");
                _statusRail.Set("active", $"{hive.colonyPopulation:P0}", AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("plotcount", $"{hive.queenVitality:P0}", hive.queenVitality < 0.4f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("harvests", $"+{bonus:P0}", AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("vault", $"{hive.honeyBuffer:F1} kg", AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("blight", $"{hive.waxBuffer:F1} kg", AshfallMetricCard.Criticality.Normal);
            }
            else
            {
                _statusRail.Set("active", hive == null ? "NO HIVE" : "COLLAPSED", AshfallMetricCard.Criticality.Critical);
                _statusRail.Set("plotcount", "0%", AshfallMetricCard.Criticality.Critical);
                _statusRail.Set("harvests", "+0%", AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("vault", "0 kg", AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("blight", "0 kg", AshfallMetricCard.Criticality.Normal);
            }
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

        if (_cropFilter == "apiary")
        {
            var apiaryRows = new List<AshfallDataGrid.Row>();
            if (_host != null)
            {
                var hive = _host.Apiculture.GetHive("hive_01");
                if (hive != null)
                {
                    float bonus = _host.Apiculture.GetPollinationBonus("plot_0");
                    var popState = hive.isDead ? AshfallDataGrid.CellState.Critical
                        : hive.colonyPopulation > 0.7f ? AshfallDataGrid.CellState.Positive
                        : AshfallDataGrid.CellState.Normal;
                    apiaryRows.Add(new AshfallDataGrid.Row
                    {
                        Cells = new List<AshfallDataGrid.Cell>
                        {
                            new("Hive #1", AshfallDataGrid.CellState.Normal),
                            new(hive.isDead ? "DEAD" : "ACTIVE", hive.isDead ? AshfallDataGrid.CellState.Critical : AshfallDataGrid.CellState.Positive),
                            new($"{hive.waterLevel * 100f:0.0}%", hive.waterLevel < 0.2f ? AshfallDataGrid.CellState.Caution : AshfallDataGrid.CellState.Normal),
                            new($"{hive.temperatureC:0.0}°C", (hive.temperatureC < 15f || hive.temperatureC > 32f) ? AshfallDataGrid.CellState.Caution : AshfallDataGrid.CellState.Normal),
                            new($"{hive.colonyPopulation * 100f:0.0}%", popState),
                            new("—", AshfallDataGrid.CellState.Muted),
                            new("—", AshfallDataGrid.CellState.Muted),
                            new($"Glass Orchard Apiary (+{bonus:P0} Pollination)", AshfallDataGrid.CellState.Normal),
                        },
                        Selectable = true
                    });
                }
            }
            if (apiaryRows.Count == 0)
            {
                apiaryRows.Add(new AshfallDataGrid.Row
                {
                    Cells = new List<AshfallDataGrid.Cell>
                    {
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("NO HIVE", AshfallDataGrid.CellState.Caution),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("—", AshfallDataGrid.CellState.Muted),
                        new("No active beehives installed in apiary", AshfallDataGrid.CellState.Muted),
                    }
                });
            }
            _plotGrid.SetRows(apiaryRows);
            return;
        }

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

            // Plan 22 GAP-7: readiness estimate + dry warning.
            bool dry = !GreenhouseSystem.IsFallow(p) && p.water < 25f;

            var cells = new List<AshfallDataGrid.Cell>
            {
                new($"#{i + 1}", AshfallDataGrid.CellState.Normal),
                new(badgeText, badgeState),
                new($"{p.water:0.0}", waterState),
                new($"{p.soilContamination:0.0}", soilState),
                new($"{p.growth:0.0}%", growthState),
                new(ReadyIn(p), AshfallDataGrid.CellState.Muted),
                new(dry ? "DRY" : "—", dry ? AshfallDataGrid.CellState.Caution : AshfallDataGrid.CellState.Muted),
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

        if (_cropFilter == "apiary")
        {
            _detailTitle.Text = "APIARY // HIVE CONTROL";
            var hive = _host?.Apiculture.GetHive("hive_01");
            if (hive != null && !hive.isDead)
            {
                float bonus = _host!.Apiculture.GetPollinationBonus("plot_0");
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Hive Unit", "hive_01 (Bay Orchard)", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Colony Pop", $"{hive.colonyPopulation:P0}", AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Queen Vitality", $"{hive.queenVitality:P0}",
                    hive.queenVitality < 0.4f ? AshfallUiHelpers.ToColor(DesignTheme.Critical) : AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Feed Level", $"{hive.feedLevel:P0}",
                    hive.feedLevel < 0.2f ? AshfallUiHelpers.ToColor(DesignTheme.LetheAmber) : AshfallUiHelpers.ToColor(DesignTheme.Dim)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Water Level", $"{hive.waterLevel:P0}",
                    hive.waterLevel < 0.2f ? AshfallUiHelpers.ToColor(DesignTheme.LetheAmber) : AshfallUiHelpers.ToColor(DesignTheme.Dim)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Hive Temp", $"{hive.temperatureC:F1}°C (Optimal 15-32°C)",
                    (hive.temperatureC < 15f || hive.temperatureC > 32f) ? AshfallUiHelpers.ToColor(DesignTheme.Entropy) : AshfallUiHelpers.ToColor(DesignTheme.Dim)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Pollination", $"+{bonus:P0} crop yield bonus", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Honey Reserve", $"{hive.honeyBuffer:F2} kg (-> Food Rations)", AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Wax Reserve", $"{hive.waxBuffer:F2} kg (-> Crafting Parts)", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

                if (!string.IsNullOrEmpty(_host.LastEvent))
                {
                    _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
                    _detailBox.AddChild(AshfallUiHelpers.MakeSmall(_host.LastEvent));
                }

                _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
                _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("APIARY ACTIONS"));
                var apiaryActionRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);

                var inspectBtn = AshfallUiHelpers.MakeButton("INSPECT", () => OnActionRequested?.Invoke("apiary_inspect", 0));
                inspectBtn.CustomMinimumSize = new Vector2(85, 30);
                apiaryActionRow.AddChild(inspectBtn);

                var feedBtn = AshfallUiHelpers.MakeButton("FEED/WATER", () => OnActionRequested?.Invoke("apiary_feed", 0));
                feedBtn.CustomMinimumSize = new Vector2(100, 30);
                apiaryActionRow.AddChild(feedBtn);

                var apiaryHarvestBtn = AshfallUiHelpers.MakeButton("HARVEST", () => OnActionRequested?.Invoke("apiary_harvest", 0));
                apiaryHarvestBtn.CustomMinimumSize = new Vector2(85, 30);
                apiaryActionRow.AddChild(apiaryHarvestBtn);

                _detailBox.AddChild(apiaryActionRow);
            }
            else
            {
                _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("No active beehive installed in the Orchard Apiary bay. Install a colony to provide pollination boost."));
                _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
                var installBtn = AshfallUiHelpers.MakeButton("INSTALL HIVE", () => OnActionRequested?.Invoke("apiary_install", 0));
                installBtn.CustomMinimumSize = new Vector2(140, 32);
                _detailBox.AddChild(installBtn);
            }
            return;
        }

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

        // ── Plan 22 UI gap register (trimmed to current host surface) ──
        // GAP-1 seed picker, GAP-3 supply rail (above), GAP-6 water split,
        // GAP-7 readiness + dry columns (grid above).
        // Removed (concurrent worker trimmed catalog/host): GAP-2 amend,
        // GAP-4 maintenance, GAP-5 sterilise, GAP-8 degraded copy.
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("ACTIONS"));
        var actionRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);

        var plantBtn = AshfallUiHelpers.MakeButton("PLANT",
            () => _pendingPicker = _pendingPicker == "plant" ? null : "plant");
        plantBtn.CustomMinimumSize = new Vector2(90, 30);
        actionRow.AddChild(plantBtn);

        var treatBtn = AshfallUiHelpers.MakeButton("TREAT",
            () => OnActionRequested?.Invoke("treat", _selectedIndex));
        treatBtn.CustomMinimumSize = new Vector2(90, 30);
        actionRow.AddChild(treatBtn);

        var clearBtn = AshfallUiHelpers.MakeButton("CLEAR",
            () => OnActionRequested?.Invoke("clear", _selectedIndex));
        clearBtn.CustomMinimumSize = new Vector2(90, 30);
        actionRow.AddChild(clearBtn);

        var harvestBtn = AshfallUiHelpers.MakeButton("HARVEST",
            () => OnActionRequested?.Invoke("harvest", _selectedIndex));
        harvestBtn.CustomMinimumSize = new Vector2(90, 30);
        actionRow.AddChild(harvestBtn);
        _detailBox.AddChild(actionRow);

        // GAP-6: water split — three discrete options.
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("WATERING"));
        var waterRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
        int cleanStock  = _host?.InventoryHost?.Inventory.CountById("clean_water")     ?? 0;
        int irradStock  = _host?.InventoryHost?.Inventory.CountById("irradiated_water") ?? 0;

        var c25Btn = AshfallUiHelpers.MakeButton("CLEAN 25",
            () => OnActionRequested?.Invoke("water:25:clean", _selectedIndex),
            disabled: _host?.InventoryHost != null && cleanStock < 3);
        c25Btn.CustomMinimumSize = new Vector2(100, 28);
        waterRow.AddChild(c25Btn);

        var c50Btn = AshfallUiHelpers.MakeButton("CLEAN 50",
            () => OnActionRequested?.Invoke("water:50:clean", _selectedIndex),
            disabled: _host?.InventoryHost != null && cleanStock < 5);
        c50Btn.CustomMinimumSize = new Vector2(100, 28);
        waterRow.AddChild(c50Btn);

        var t50Btn = AshfallUiHelpers.MakeButton("TAINTED 50",
            () => OnActionRequested?.Invoke("water:50:tainted", _selectedIndex),
            disabled: _host?.InventoryHost != null && irradStock < 5);
        t50Btn.CustomMinimumSize = new Vector2(110, 28);
        t50Btn.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Entropy));
        waterRow.AddChild(t50Btn);

        _detailBox.AddChild(waterRow);
        _detailBox.AddChild(AshfallUiHelpers.MakeSmall(
            $"clean ×{cleanStock}  ·  irradiated ×{irradStock}"));
        _detailBox.AddChild(AshfallUiHelpers.MakeSmall(
            "irradiated — crops remember", autowrap: true));

        // GAP-1: seed selection picker (toggled by PLANT button above).
        if (_pendingPicker == "plant" && _host != null)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("SEED SELECT"));
            var cat = GreenhouseExpansionCatalog.CropCatalog.All;
            for (int i = 0; i < cat.Length; i++)
            {
                var def = cat[i];
                bool locked  = def.RequiresUnlock && !_host.System.State.preWarWheatUnlocked;
                bool noStock = _host.InventoryHost?.Inventory.CountById(def.SeedItemId) < 1;
                bool disabled = locked || noStock;
                var seedRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingXs);
                var label = $"{FriendlySeed(def.SeedItemId)} — {def.GrowthHoursToMature / 24f:0.#}d · yield {def.BaseYield} · blight {(def.BlightResistance * 100f):0}% · {(def.WaterPerDay):0} water/day";
                if (locked)  label += "  [SEED VAULT SEALED]";
                if (noStock) label += "  [no stock]";
                var seedBtn = AshfallUiHelpers.MakeButton(label,
                    () => OnActionRequested?.Invoke($"plant:{def.SeedItemId}", _selectedIndex),
                    disabled);
                seedBtn.CustomMinimumSize = new Vector2(0, 24);
                seedBtn.SizeFlagsHorizontal = SizeFlags.Expand | SizeFlags.Fill;
                seedRow.AddChild(seedBtn);
                _detailBox.AddChild(seedRow);
            }
        }
    }

    /// <summary>Raised when the player presses an action button. Host wires to GreenhouseHostSession.</summary>
    public event Action<string, int>? OnActionRequested;

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
        GreenhouseExpansionCatalog.Items.SeedMushroom     => "Mushroom Spores",
        GreenhouseExpansionCatalog.Items.SeedTuber        => "Frost Tuber",
        GreenhouseExpansionCatalog.Items.SeedGrain        => "Winter Rye Grain",
        GreenhouseExpansionCatalog.Items.SeedWheat        => "Pre-War Heritage Wheat",
        GreenhouseExpansionCatalog.Items.SeedHardyTuber   => "Hardy Frost Tuber",
        GreenhouseExpansionCatalog.Items.SeedAshGrain     => "Ashland Grain",
        GreenhouseExpansionCatalog.Items.SeedBiolumMushroom => "Bioluminescent Mushroom",
        GreenhouseExpansionCatalog.Items.SeedNutrientAlgae  => "Nutrient Algae",
        GreenhouseExpansionCatalog.Items.SeedMedicinalHerb  => "Medicinal Herb",
        GreenhouseExpansionCatalog.Items.SeedLeafyGreen     => "Leafy Green",
        GreenhouseExpansionCatalog.Items.SeedOilseed        => "Oilseed",
        GreenhouseExpansionCatalog.Items.SeedColdLegume     => "Cold Legume",
        _ => "Cultivar",
    };

    private static string FriendlySupply(string itemId) => itemId switch
    {
        GreenhouseExpansionCatalog.Items.LeadGlassPane   => "Glass Pane",
        GreenhouseExpansionCatalog.Items.BlightTreatment => "Blight Treatment",
        GreenhouseExpansionCatalog.Items.GrowMedium      => "Grow Medium",
        _ => itemId,
    };

    /// <summary>
    /// Plan 22 GAP-7: rough readiness estimate based on crop definition
    /// and current growth. Returns "NOW" when mature, "—" for fallow or
    /// when no definition exists.
    /// </summary>
    private static string ReadyIn(GreenhousePlotState p)
    {
        if (GreenhouseSystem.IsFallow(p)) return "—";
        var stage = (GreenhouseStage)p.stage;
        if (stage == GreenhouseStage.Mature) return "NOW";
        if (stage == GreenhouseStage.Failed) return "—";
        var def = GreenhouseExpansionCatalog.CropCatalog.Get(p.seedItemId);
        if (def == null) return "—";
        float daysTotal = Math.Max(1f, def.GrowthHoursToMature / 24f);
        float daysLeft = (100f - p.growth) / 100f * daysTotal;
        return $"{Math.Max(1, (int)Math.Ceiling(daysLeft))}d";
    }

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
                    new("4d", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new(FriendlySeed(GreenhouseExpansionCatalog.Items.SeedMushroom), AshfallDataGrid.CellState.Normal),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("#2", AshfallDataGrid.CellState.Normal),
                    new("GROWING", AshfallDataGrid.CellState.Normal),
                    new("44.0", AshfallDataGrid.CellState.Caution),
                    new("38.0", AshfallDataGrid.CellState.Normal),
                    new("72.0%", AshfallDataGrid.CellState.Normal),
                    new("2d", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new(FriendlySeed(GreenhouseExpansionCatalog.Items.SeedTuber), AshfallDataGrid.CellState.Normal),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("#3", AshfallDataGrid.CellState.Normal),
                    new("READY", AshfallDataGrid.CellState.Positive),
                    new("82.0", AshfallDataGrid.CellState.Normal),
                    new("9.0", AshfallDataGrid.CellState.Normal),
                    new("100.0%", AshfallDataGrid.CellState.Positive),
                    new("NOW", AshfallDataGrid.CellState.Positive),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new(FriendlySeed(GreenhouseExpansionCatalog.Items.SeedGrain), AshfallDataGrid.CellState.Normal),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("#4", AshfallDataGrid.CellState.Normal),
                    new("FALLOW", AshfallDataGrid.CellState.Muted),
                    new("0.0", AshfallDataGrid.CellState.Critical),
                    new("0.0", AshfallDataGrid.CellState.Normal),
                    new("0.0%", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
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
