using System;
using System.Collections.Generic;
using System.Text;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Foundry;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Silent Foundry Dashboard (Stitch #1, Heavy Metallurgy / Foundry).
///
/// Phase 16 prioritised lift: same `SilentFoundryHostSession` source as the
/// legacy Phase 10 modal, but routed through the Phase 11 dashboard shell
/// (AshfallDashboardShell + AshfallSidebar + AshfallStatusRail) and the
/// Phase 12 data-grid primitive (AshfallDataGrid).
///
/// Six columns of the active cast queue + condition bars + treaty roll-up.
/// Pure presentation; reads only from `Ashfall.Core.Foundry.*`.
/// </summary>
public partial class SilentFoundryPanel : Control
{
    public event Action? OnClose;
    public event Action<int>? OnProductSelected;

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _productGrid;
    private VBoxContainer _detailBox = null!;
    private Label _detailTitle = null!;
    private int _selectedIndex = -1;
    private string _categoryFilter = "all"; // all | agricultural_tool | structural_beam | water_component | heavy_alloy_part | repair_plate

    private SilentFoundryHostSession? _host;
    private int _currentDay = 4;

    public bool IsBound => _host != null;

    public void Bind(SilentFoundryHostSession session, int currentDay)
    {
        _host = session;
        _currentDay = currentDay;
        if (_host != null)
            _host.StateChanged += RefreshView;
        RefreshView();
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("The Silent Foundry // Cupola & Casting Bay", minWidth: 1100, minHeight: 720);
        SetContentRoot(_shell);

        // Sidebar: filter by sink category, plus status shortcuts.
        var sidebarItems = new[]
        {
            new AshfallSidebar.Item { Id = "all", Label = "All Heats", Hint = "every castable product", IconPath = "" },
            new AshfallSidebar.Item { Id = "agricultural_tool", Label = "Agricultural Tools", Hint = "shovels, harrows, plowshares", IconPath = "" },
            new AshfallSidebar.Item { Id = "structural_beam", Label = "Structural Beams", Hint = "rods, plates, fasteners", IconPath = "" },
            new AshfallSidebar.Item { Id = "water_component", Label = "Water / Brine Items", Hint = "pipes, winches", IconPath = "" },
            new AshfallSidebar.Item { Id = "heavy_alloy_part", Label = "Heavy Alloy Parts", Hint = "defense plates, brackets", IconPath = "" },
            new AshfallSidebar.Item { Id = "repair_plate", Label = "Repair Plates", Hint = "expedition spares", IconPath = "" },
        };
        _sidebar = _shell.SetSidebar(sidebarItems, "Heat Categories", "all");
        _sidebar.OnSelected += HandleSidebar;

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("sealed", "Furnace", "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
        _statusRail.AddCard("hearth", "Hearth", "—/100", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("lining", "Lining", "—/100", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("casts", "Casts", "—", AshfallMetricCard.Criticality.Normal, minWidth: 80);
        _statusRail.AddCard("labor", "Labor", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("treaty", "Treaties", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);

        // DataGrid: product, sink, charge (count + ids), fuel, water, quality target.
        var cols = new[]
        {
            new AshfallDataGrid.Column { Header = "Product", MinWidth = 200, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Sink",    MinWidth = 140, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Charge",  MinWidth = 200, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Fuel",    MinWidth = 70,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Water L", MinWidth = 80,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Quality", MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right },
        };
        _productGrid = new AshfallDataGrid(cols, showHeader: true, minWidth: 720, minHeight: 320);
        _productGrid.OnRowSelected += HandleRowSelected;

        var body = new HBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_productGrid);

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.CustomMinimumSize = new Vector2(320, 320);
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_detailBox);

        _detailTitle = AshfallUiHelpers.MakeSectionHeader("CAST DETAIL");
        _detailTitle.HorizontalAlignment = HorizontalAlignment.Left;
        _detailBox.AddChild(_detailTitle);
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
            "Select a heat row to view charge costs, treaty obligations, and quality target."));

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
        _categoryFilter = id ?? "all";
        _selectedIndex = -1;
        RefreshView();
    }

    private void HandleRowSelected(int idx)
    {
        _selectedIndex = idx;
        OnProductSelected?.Invoke(idx);
        RefreshDetail();
    }

    public void RefreshView()
    {
        RefreshStatusRail();
        BuildProductRows();
        RefreshDetail();
    }

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        if (_host == null)
        {
            _statusRail.Set("sealed", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("hearth", "—/100", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("lining", "—/100", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("casts", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("labor", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("treaty", "—", AshfallMetricCard.Criticality.Normal);
            return;
        }

        var s = _host.Engine.State;
        int casts = _host.Engine.TotalProductionCount;
        int failed = _host.Engine.TotalFailedCount;

        _statusRail.Set("sealed", s.unlocked ? "OPEN" : "SEALED",
            s.unlocked ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Caution);
        _statusRail.Set("hearth", $"{s.hearthTuyeres:0}/100",
            s.hearthTuyeres < 30f ? AshfallMetricCard.Criticality.Critical :
            s.hearthTuyeres < 60f ? AshfallMetricCard.Criticality.Caution :
                                    AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("lining", $"{s.refractoryLining:0}/100",
            s.refractoryLining < 30f ? AshfallMetricCard.Criticality.Critical :
            s.refractoryLining < 60f ? AshfallMetricCard.Criticality.Caution :
                                       AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("casts", $"{casts}", failed > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("labor", LaborLabel(s.laborDispute),
            s.laborDispute == FoundryLaborDispute.StrikeActive ? AshfallMetricCard.Criticality.Warn :
            s.laborDispute == FoundryLaborDispute.Tensions     ? AshfallMetricCard.Criticality.Caution :
                                                                    AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("treaty", TreatySummary(s),
            s.treatyCompliance != null && AnyTreatyViolated(s.treatyCompliance) ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
    }

    private static string LaborLabel(FoundryLaborDispute d) => d switch
    {
        FoundryLaborDispute.None => "CALM",
        FoundryLaborDispute.Tensions => "TENSIONS",
        FoundryLaborDispute.StrikeActive => "STRIKE",
        FoundryLaborDispute.Resolved => "RESOLVED",
        _ => "—",
    };

    private static bool AnyTreatyViolated(List<FoundryTreatyCompliance> comps)
    {
        if (comps == null) return false;
        for (int i = 0; i < comps.Count; i++)
            if (comps[i] != null && comps[i].missedCount > comps[i].metCount) return true;
        return false;
    }

    private static string TreatySummary(SilentFoundryState s)
    {
        if (s.treatyCompliance == null || s.treatyCompliance.Count == 0) return "—";
        int met = 0, missed = 0;
        for (int i = 0; i < s.treatyCompliance.Count; i++)
        {
            if (s.treatyCompliance[i] == null) continue;
            met += s.treatyCompliance[i].metCount;
            missed += s.treatyCompliance[i].missedCount;
        }
        return missed > 0 ? $"{met}m {missed}miss" : $"{met}m";
    }

    private void BuildProductRows()
    {
        if (_productGrid == null) return;

        if (_host == null)
        {
            _productGrid.SetRows(BuildFixtureRows());
            return;
        }

        var rows = new List<AshfallDataGrid.Row>();
        var products = _host.Catalog.AllProducts;
        for (int i = 0; i < products.Count; i++)
        {
            var p = products[i];
            if (p == null) continue;
            if (!FilterPass(p)) continue;

            string qualityTarget = p.quality_target > 0f ? $"{p.quality_target:0}" : "—";
            string charge = SummariseIngredients(p);
            var cells = new List<AshfallDataGrid.Cell>
            {
                new(p.display_name ?? p.product_id, AshfallDataGrid.CellState.Normal),
                new(string.IsNullOrEmpty(p.sink) ? p.category : p.sink, AshfallDataGrid.CellState.Muted),
                new(charge, AshfallDataGrid.CellState.Normal),
                new($"{p.fuel_units}", AshfallDataGrid.CellState.Normal),
                new($"{p.water_litres}", AshfallDataGrid.CellState.Normal),
                new(qualityTarget, AshfallDataGrid.CellState.Normal),
            };
            rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
        }
        if (rows.Count == 0)
        {
            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new("— no products match filter —", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                }
            });
        }
        _productGrid.SetRows(rows);
    }

    private static string SummariseIngredients(FoundryProductEntry p)
    {
        if (p.ingredients == null || p.ingredients.Count == 0) return "—";
        var sb = new StringBuilder();
        for (int i = 0; i < p.ingredients.Count; i++)
        {
            var ing = p.ingredients[i];
            if (ing == null) continue;
            if (sb.Length > 0) sb.Append(" + ");
            sb.Append(ing.amount).Append(' ').Append(ing.item_id);
        }
        return sb.ToString();
    }

    private bool FilterPass(FoundryProductEntry p)
    {
        if (p == null) return false;
        if (_categoryFilter == "all") return true;
        return string.Equals(p.category, _categoryFilter, StringComparison.Ordinal);
    }

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        while (_detailBox.GetChildCount() > 0)
        {
            var c = _detailBox.GetChild(0);
            _detailBox.RemoveChild(c);
            c.QueueFree();
        }
        if (_host == null || _selectedIndex < 0 || _productGrid == null || _selectedIndex >= _productGrid.Rows.Count)
        {
            _detailTitle.Text = "CAST DETAIL";
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                _host == null
                    ? "Foundry engine offline. Bind a SilentFoundryHostSession to see live cast rows."
                    : "Select a heat row to view charge costs, treaty obligations, and quality target."));
            return;
        }
        var products = _host.Catalog.AllProducts;
        var visibleIdx = ResolveVisibleIndex(_selectedIndex);
        if (visibleIdx < 0 || visibleIdx >= products.Count) return;
        var p = products[visibleIdx];

        _detailTitle.Text = $"{(string.IsNullOrEmpty(p.display_name) ? p.product_id : p.display_name).ToUpperInvariant()} DETAIL";
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Sink", string.IsNullOrEmpty(p.sink) ? p.category : p.sink,
            AshfallUiHelpers.ToColor(DesignTheme.Pale)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Yields", $"{p.result_amount}× {p.result_item_id}",
            AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Labor", $"{p.labor_hours:0.0} h",
            AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Cast", $"{p.cast_hours:0.0} h",
            AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Skill target", $"{p.skill_target:0.00}",
            AshfallUiHelpers.ToColor(DesignTheme.Dim)));

        if (p.ingredients != null && p.ingredients.Count > 0)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSubsectionHeader("Charge Manifest"));
            for (int i = 0; i < p.ingredients.Count; i++)
            {
                var ing = p.ingredients[i];
                if (ing == null) continue;
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow(ing.item_id, $"× {ing.amount}",
                    AshfallUiHelpers.ToColor(DesignTheme.Muted)));
            }
        }
        if (!string.IsNullOrEmpty(p.treaty_id) && p.quota_amount > 0)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSubsectionHeader("Treaty Obligation"));
            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow(p.treaty_id, $"quota {p.quota_amount}/cycle",
                AshfallUiHelpers.ToColor(DesignTheme.LetheAmber)));
        }
        if (!string.IsNullOrEmpty(p.notes))
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSubsectionHeader("Provenance"));
            _detailBox.AddChild(AshfallUiHelpers.MakeSmall(p.notes, autowrap: true));
        }
    }

    private int ResolveVisibleIndex(int selected)
    {
        // Walk the catalog up to selected, skipping those filtered out.
        var products = _host?.Catalog.AllProducts;
        if (products == null) return -1;
        int seen = 0;
        for (int i = 0; i < products.Count; i++)
        {
            if (!FilterPass(products[i])) continue;
            if (seen == selected) return i;
            seen++;
        }
        return -1;
    }

    /// <summary>
    /// Hard-coded fixture rows used when no host session is bound. All product
    /// ids route through the catalog's own `product_id` strings — never invented.
    /// </summary>
    internal static List<AshfallDataGrid.Row> BuildFixtureRows()
    {
        var rows = new List<AshfallDataGrid.Row>
        {
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("Heavy Ploughshare", AshfallDataGrid.CellState.Normal),
                    new("agricultural_tool", AshfallDataGrid.CellState.Muted),
                    new("4 scrap_metal + 2 item_foundry_flux", AshfallDataGrid.CellState.Normal),
                    new("6", AshfallDataGrid.CellState.Normal),
                    new("40", AshfallDataGrid.CellState.Normal),
                    new("70", AshfallDataGrid.CellState.Normal),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("I-Beam Bracket", AshfallDataGrid.CellState.Normal),
                    new("structural_beam", AshfallDataGrid.CellState.Muted),
                    new("6 scrap_metal + 1 item_foundry_alloy_additive", AshfallDataGrid.CellState.Normal),
                    new("8", AshfallDataGrid.CellState.Normal),
                    new("55", AshfallDataGrid.CellState.Normal),
                    new("75", AshfallDataGrid.CellState.Normal),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("Brine-Resistant Pipe", AshfallDataGrid.CellState.Normal),
                    new("water_component", AshfallDataGrid.CellState.Muted),
                    new("3 scrap_metal + 1 charcoal", AshfallDataGrid.CellState.Normal),
                    new("5", AshfallDataGrid.CellState.Normal),
                    new("60", AshfallDataGrid.CellState.Normal),
                    new("80", AshfallDataGrid.CellState.Normal),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("Defense Plate", AshfallDataGrid.CellState.Normal),
                    new("heavy_alloy_part", AshfallDataGrid.CellState.Muted),
                    new("8 scrap_metal + 1 item_foundry_alloy_additive", AshfallDataGrid.CellState.Normal),
                    new("9", AshfallDataGrid.CellState.Normal),
                    new("70", AshfallDataGrid.CellState.Normal),
                    new("85", AshfallDataGrid.CellState.Normal),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("Road-Iron Spike", AshfallDataGrid.CellState.Normal),
                    new("repair_plate", AshfallDataGrid.CellState.Muted),
                    new("5 scrap_metal", AshfallDataGrid.CellState.Normal),
                    new("7", AshfallDataGrid.CellState.Normal),
                    new("50", AshfallDataGrid.CellState.Normal),
                    new("72", AshfallDataGrid.CellState.Normal),
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
