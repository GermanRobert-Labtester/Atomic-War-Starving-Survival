using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Survival Workstation (#19 Stitch).
///
/// Coherent workstation surface that stitches together Inventory + Crafting.
/// Uses the dashboard shell + sidebar (Storage / Recipes / Queue) + status
/// rail (capacity / slots / active crafts / operational stations) and an
/// AshfallDataGrid for the recipe matrix — the first production consumer.
///
/// The existing InventoryPanel + CraftingPanel remain authoritative and
/// un-touched (preserving their interaction behaviour and the existing
/// modal panels used by other entry points). This surface composes them
/// into the dashboard HYBRID architecture the Stitch reference describes.
///
/// Pure presentation. All recipe/craft authority stays in
/// CraftingHostSession / CraftingSystem and InventoryHostSession.
/// </summary>
public partial class SurvivalWorkstationPanel : Control
{
    public event Action? OnClose;
    public event Action? OnOpenInventoryOverlay;
    public event Action? OnOpenCraftingOverlay;
    public event Action? OnCraftStarted;

    public bool IsBound => _craftingHost != null;

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _recipeGrid;

    private Label _detailTitle = null!;
    private VBoxContainer _detailBox = null!;
    private Label _queueFooter = null!;
    private Button _btnOpenInventory = null!;
    private Button _btnOpenCrafting = null!;
    private Button _btnStartSelected = null!;

    private CraftingHostSession? _craftingHost;
    private InventoryHostSession? _inventoryHost;
    private int _selectedRecipeIndex = -1;
    private string _activeFilter = "all"; // "all" | "craftable" | "queued"

    public void Bind(CraftingHostSession crafting, InventoryHostSession? inventory = null)
    {
        _craftingHost = crafting;
        _inventoryHost = inventory;
        if (_craftingHost != null)
        {
            _craftingHost.Engine.OnCraftStarted -= OnEngineCraftStarted;
            _craftingHost.Engine.OnCraftCompleted -= OnEngineCraftCompleted;
            _craftingHost.Engine.OnCraftStarted += OnEngineCraftStarted;
            _craftingHost.Engine.OnCraftCompleted += OnEngineCraftCompleted;
        }
        RefreshView();
    }

    public void RefreshView()
    {
        RefreshStatusRail();
        RefreshQueueFooter();
        BuildRecipeRows();
        RefreshDetail();
    }

    private void OnEngineCraftStarted(Recipe _) { RefreshView(); OnCraftStarted?.Invoke(); }
    private void OnEngineCraftCompleted(Recipe _) => RefreshView();

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);
        Visible = false;

        var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.90f) };
        bg.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(bg);

        var center = new CenterContainer
        {
            Name = "CenterContainer"
        };
        center.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(center);

        _shell = new AshfallDashboardShell(
            "SURVIVAL WORKSTATION — STORAGE_CACHE_A / ASSEMBLY_MODE",
            1100, 720);
        center.AddChild(_shell);

        _sidebar = _shell.SetSidebar(new[]
        {
            new AshfallSidebar.Item { Id = "storage",  Label = "Storage",        Hint = "Inventory · Gear" },
            new AshfallSidebar.Item { Id = "recipes",  Label = "Recipe Matrix",  Hint = "Craftable Queue" },
            new AshfallSidebar.Item { Id = "queue",    Label = "Active Queue",   Hint = "In Progress" },
            new AshfallSidebar.Item { Id = "filter_all",       Label = "Filter: All",         Hint = "all recipes" },
            new AshfallSidebar.Item { Id = "filter_craftable", Label = "Filter: Craftable",   Hint = "ingredients met" },
        }, "WORKSTATION", "recipes");

        if (_sidebar != null)
        {
            _sidebar.OnSelected += id =>
            {
                if (id == "storage")
                    OnOpenInventoryOverlay?.Invoke();
                else if (id == "queue")
                    OnOpenCraftingOverlay?.Invoke();
                else if (id == "filter_all")
                { _activeFilter = "all"; BuildRecipeRows(); }
                else if (id == "filter_craftable")
                { _activeFilter = "craftable"; BuildRecipeRows(); }
            };
        }

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("slots",   "STORAGE SLOTS",  "—", AshfallMetricCard.Criticality.Normal, 140);
        _statusRail.AddCard("capacity","CAPACITY",       "— kg", AshfallMetricCard.Criticality.Normal, 130);
        _statusRail.AddCard("active",  "ACTIVE CRAFTS",  "0", AshfallMetricCard.Criticality.Normal, 140);
        _statusRail.AddCard("station", "WORKBENCH",      "OK", AshfallMetricCard.Criticality.Normal, 130);
        _statusRail.AddCard("ready",   "READY RECIPES",  "0", AshfallMetricCard.Criticality.Normal, 130);

        _shell.AttachHeaderCloseButton("CLOSE [Esc]", () => OnClose?.Invoke());

        BuildContent();
    }

    private void BuildContent()
    {
        var content = new HBoxContainer();
        content.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        content.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        content.SizeFlagsVertical = Control.SizeFlags.ExpandFill;

        // ── Left: recipe grid (#19 Stitch primary element) ──
        var leftCol = new VBoxContainer();
        leftCol.SizeFlagsHorizontal = Control.SizeFlags.Expand;
        leftCol.SizeFlagsStretchRatio = 1.45f;
        leftCol.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);

        var recipeHeader = new HBoxContainer();
        recipeHeader.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        var recipeTitle = new Label
        {
            Text = "AVAILABLE RECIPES",
            VerticalAlignment = VerticalAlignment.Center
        };
        recipeTitle.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeH3);
        recipeTitle.AddThemeColorOverride("font_color",
            AshfallUiHelpers.ToColor(DesignTheme.Warm));
        var recipeFont = AshfallUiHelpers.LoadFont("res://assets/fonts/BarlowCondensed-SemiBold.ttf");
        if (recipeFont != null) recipeTitle.AddThemeFontOverride("font", recipeFont);
        recipeTitle.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        recipeHeader.AddChild(recipeTitle);

        var btnAll = AshfallUiHelpers.MakeButton("ALL", () =>
        {
            _activeFilter = "all";
            BuildRecipeRows();
        });
        btnAll.CustomMinimumSize = new Vector2(72, 28);
        recipeHeader.AddChild(btnAll);

        var btnCraftable = AshfallUiHelpers.MakeButton("CRAFTABLE NOW", () =>
        {
            _activeFilter = "craftable";
            BuildRecipeRows();
        });
        btnCraftable.CustomMinimumSize = new Vector2(132, 28);
        recipeHeader.AddChild(btnCraftable);

        leftCol.AddChild(recipeHeader);

        var columns = new[]
        {
            new AshfallDataGrid.Column { Header = "Recipe",   MinWidth = 200, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Output",   MinWidth = 140, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Need",     MinWidth = 70,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Met",      MinWidth = 70,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Duration", MinWidth = 80,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Status",   MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Center },
        };
        _recipeGrid = new AshfallDataGrid(columns, showHeader: true, minWidth: 600, minHeight: 360);
        _recipeGrid.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        _recipeGrid.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        _recipeGrid.OnRowSelected += idx => { _selectedRecipeIndex = idx; RefreshDetail(); };
        leftCol.AddChild(_recipeGrid);

        // Action row pinned above bottom: open Inventory / open Crafting / start
        var actionRow = new HBoxContainer();
        actionRow.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _btnOpenInventory = AshfallUiHelpers.MakeButton("OPEN INVENTORY", () => OnOpenInventoryOverlay?.Invoke());
        _btnOpenInventory.CustomMinimumSize = new Vector2(170, 32);
        actionRow.AddChild(_btnOpenInventory);

        _btnOpenCrafting = AshfallUiHelpers.MakeButton("OPEN CRAFTING PANEL", () => OnOpenCraftingOverlay?.Invoke());
        _btnOpenCrafting.CustomMinimumSize = new Vector2(220, 32);
        actionRow.AddChild(_btnOpenCrafting);

        actionRow.AddChild(new Control { SizeFlagsHorizontal = Control.SizeFlags.ExpandFill });

        _btnStartSelected = AshfallUiHelpers.MakeButton("START SELECTED", () =>
        {
            if (_selectedRecipeIndex < 0) return;
            if (_craftingHost == null) return;
            var recipe = GetRecipeAtVisibleRow(_selectedRecipeIndex);
            if (recipe == null) return;
            _craftingHost?.Start(recipe.id);
        });
        _btnStartSelected.CustomMinimumSize = new Vector2(160, 32);
        actionRow.AddChild(_btnStartSelected);
        leftCol.AddChild(actionRow);

        content.AddChild(leftCol);

        // ── Right: detail + queue footer ──
        var rightCol = new VBoxContainer();
        rightCol.SizeFlagsHorizontal = Control.SizeFlags.Expand;
        rightCol.SizeFlagsStretchRatio = 1.0f;
        rightCol.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);

        var rightPanel = AshfallUiHelpers.MakePanel();
        rightPanel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        rightPanel.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        var rightMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
        rightPanel.AddChild(rightMargin);

        var rightVBox = new VBoxContainer();
        rightVBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        rightMargin.AddChild(rightVBox);

        _detailTitle = new Label { Text = "RECIPE DETAIL" };
        _detailTitle.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeH3);
        _detailTitle.AddThemeColorOverride("font_color",
            AshfallUiHelpers.ToColor(DesignTheme.Warm));
        if (recipeFont != null) _detailTitle.AddThemeFontOverride("font", recipeFont);
        rightVBox.AddChild(_detailTitle);
        rightVBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        _detailBox.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        rightVBox.AddChild(_detailBox);

        rightCol.AddChild(rightPanel);

        var queuePanel = AshfallUiHelpers.MakePanel();
        queuePanel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        var queueMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
        queuePanel.AddChild(queueMargin);

        var queueVBox = new VBoxContainer();
        queueVBox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        queueMargin.AddChild(queueVBox);

        var queueTitle = new Label
        {
            Text = "CRAFTING IN PROGRESS",
            VerticalAlignment = VerticalAlignment.Center
        };
        queueTitle.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
        queueTitle.AddThemeColorOverride("font_color",
            AshfallUiHelpers.ToColor(DesignTheme.Hot));
        queueVBox.AddChild(queueTitle);
        queueVBox.AddChild(AshfallUiHelpers.MakeSeparator());

        _queueFooter = new Label
        {
            Text = "— idle —",
            VerticalAlignment = VerticalAlignment.Center
        };
        _queueFooter.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeSmall);
        _queueFooter.AddThemeColorOverride("font_color",
            AshfallUiHelpers.ToColor(DesignTheme.Pale));
        var mono = AshfallUiHelpers.LoadFont("res://assets/fonts/ShareTechMono-Regular.ttf");
        if (mono != null) _queueFooter.AddThemeFontOverride("font", mono);
        queueVBox.AddChild(_queueFooter);

        rightCol.AddChild(queuePanel);
        content.AddChild(rightCol);

        _shell.SetContent(content);
        BuildRecipeRows();
        RefreshStatusRail();
        RefreshQueueFooter();
        RefreshDetail();
    }

    private Recipe? GetRecipeAtVisibleRow(int visibleIndex)
    {
        if (_craftingHost == null) return null;
        int seen = 0;
        foreach (var r in _craftingHost.Recipes)
        {
            if (r == null) continue;
            bool canCraft = _craftingHost.Engine.CanCraft(r);
            if (_activeFilter == "craftable" && !canCraft) continue;
            if (_activeFilter == "queued") continue;
            if (seen == visibleIndex) return r;
            seen++;
        }
        return null;
    }

    private void BuildRecipeRows()
    {
        if (_recipeGrid == null) return;
        if (_craftingHost == null)
        {
            // Host not bound — render a deterministic fixture so the surface
            // remains inspectable. The fixture mirrors the canonical seed
            // catalog from CraftingHostSession.BuildSeedCatalog.
            _recipeGrid.SetRows(BuildFixtureRows());
            return;
        }

        var rows = new List<AshfallDataGrid.Row>();
        foreach (var recipe in _craftingHost.Recipes)
        {
            if (recipe == null) continue;
            bool canCraft = _craftingHost.Engine.CanCraft(recipe);
            if (_activeFilter == "craftable" && !canCraft) continue;
            if (_activeFilter == "queued") continue;

            int needTotal = 0;
            int metCount = 0;
            int missingTotal = 0;
            var cellList = new List<AshfallDataGrid.Cell>();
            cellList.Add(new AshfallDataGrid.Cell(recipe.recipeName, AshfallDataGrid.CellState.Normal));

            string outputName = recipe.result != null ? recipe.result.displayName : "[?]";
            cellList.Add(new AshfallDataGrid.Cell(
                $"×{recipe.resultAmount} {outputName}",
                canCraft ? AshfallDataGrid.CellState.Positive : AshfallDataGrid.CellState.Normal));

            foreach (var ing in recipe.ingredients)
            {
                if (ing?.item == null) continue;
                needTotal += ing.amount;
                int held = _inventoryHost?.Inventory.CountById(ing.item.id) ?? 0;
                if (held >= ing.amount) metCount++;
                else missingTotal += (ing.amount - held);
            }

            var needCellState = needTotal == 0 ? AshfallDataGrid.CellState.Muted
                : missingTotal == 0 ? AshfallDataGrid.CellState.Positive
                : AshfallDataGrid.CellState.Warning;
            cellList.Add(new AshfallDataGrid.Cell(needTotal == 0 ? "—" : needTotal.ToString(), needCellState));

            var metCellState = recipe.ingredients.Count == 0 ? AshfallDataGrid.CellState.Muted
                : metCount == recipe.ingredients.Count ? AshfallDataGrid.CellState.Positive
                : metCount >= recipe.ingredients.Count / 2 ? AshfallDataGrid.CellState.Warning
                : AshfallDataGrid.CellState.Critical;
            string metText = recipe.ingredients.Count == 0 ? "—" : $"{metCount}/{recipe.ingredients.Count}";
            cellList.Add(new AshfallDataGrid.Cell(metText, metCellState));

            cellList.Add(new AshfallDataGrid.Cell($"{recipe.craftingTimeHours:F0}h",
                AshfallDataGrid.CellState.Muted));

            string statusText = canCraft ? "READY" : (missingTotal > 0 ? $"MISSING ×{missingTotal}" : "STATION OFF");
            var statusState = canCraft ? AshfallDataGrid.CellState.Positive
                : missingTotal > 0 ? AshfallDataGrid.CellState.Warning
                : AshfallDataGrid.CellState.Critical;
            cellList.Add(new AshfallDataGrid.Cell(statusText, statusState));

            var capturedRecipe = recipe;
            rows.Add(new AshfallDataGrid.Row
            {
                Cells = cellList,
                Selectable = true,
                OnSelected = () => { /* RefreshDetail is invoked via grid row selection */ }
            });
        }
        _recipeGrid.SetRows(rows);
    }

    /// <summary>
    /// Deterministic fixture used when no CraftingHostSession is bound
    /// (snapshot harness default state). Mirrors CraftingHostSession seed.
    /// </summary>
    private static List<AshfallDataGrid.Row> BuildFixtureRows()
    {
        var rows = new List<AshfallDataGrid.Row>
        {
            NewFixtureRow("Water Filter (charcoal)", "×1 water_filter", 3, 0, 2, "4h", AshfallDataGrid.CellState.Normal, "MISSING ×2"),
            NewFixtureRow("Bandage (clean cloth)",   "×2 bandage",     1, 0, 1, "1h", AshfallDataGrid.CellState.Normal, "MISSING ×1"),
            NewFixtureRow("Iodine Kit",              "×1 iodine_pills",2, 0, 2, "2h", AshfallDataGrid.CellState.Normal, "MISSING ×2"),
            NewFixtureRow("Rad-Away (chelators)",    "×1 rad_away",    3, 0, 3, "6h", AshfallDataGrid.CellState.Normal, "MISSING ×3"),
            NewFixtureRow("Filter Pack (gas mask)",  "×1 filter_pack", 3, 0, 3, "3h", AshfallDataGrid.CellState.Normal, "MISSING ×3"),
            NewFixtureRow("Improvised Inhaler",      "×1 inhaler",     3, 0, 3, "3h", AshfallDataGrid.CellState.Normal, "MISSING ×3"),
            NewFixtureRow("Herbal Tea",              "×2 herbal_tea",  1, 0, 1, "0h", AshfallDataGrid.CellState.Normal, "MISSING ×1"),
        };
        return rows;
    }

    private static AshfallDataGrid.Row NewFixtureRow(string recipe, string output, int need, int met, int missing, string dur, AshfallDataGrid.CellState baseState, string status)
    {
        var cells = new List<AshfallDataGrid.Cell>
        {
            new(recipe, baseState),
            new(output, baseState),
            new(need.ToString(), baseState),
            new($"{met}/{need}", baseState),
            new(dur, AshfallDataGrid.CellState.Muted),
            new(status, AshfallDataGrid.CellState.Warning),
        };
        return new AshfallDataGrid.Row { Cells = cells, Selectable = true };
    }

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        if (_inventoryHost == null && _craftingHost == null)
        {
            _statusRail.Set("slots",    "—",          AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("capacity", "— kg",       AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("active",   "0",          AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("station",  "—",          AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("ready",    "0",          AshfallMetricCard.Criticality.Normal);
            return;
        }
        int slots = _inventoryHost?.Inventory.Slots.Count ?? 0;
        var maxWeight = _inventoryHost?.Inventory.MaxWeight ?? 0f;
        var currentWeight = _inventoryHost?.Inventory.GetCurrentWeight() ?? 0f;

        int active = _craftingHost?.Engine.ActiveCraftCount ?? 0;

        string stationState = "OK";
        var station = _craftingHost?.Engine.GetStation("workbench");
        if (station == null) stationState = "—";
        else if (!station.IsOperational) stationState = "DOWN";
        else if (station.condition < 25f) stationState = $"{station.condition:0}%";

        int ready = 0;
        if (_craftingHost != null)
        {
            foreach (var r in _craftingHost.Recipes)
            {
                if (r == null) continue;
                if (_craftingHost.Engine.CanCraft(r)) ready++;
            }
        }

        AshfallMetricCard.Criticality capacityCrit =
            maxWeight <= 0 ? AshfallMetricCard.Criticality.Normal
            : currentWeight >= maxWeight ? AshfallMetricCard.Criticality.Critical
            : currentWeight >= maxWeight * 0.8 ? AshfallMetricCard.Criticality.Warn
            : AshfallMetricCard.Criticality.Normal;
        AshfallMetricCard.Criticality stationCrit =
            stationState == "OK" ? AshfallMetricCard.Criticality.Normal
            : stationState == "DOWN" ? AshfallMetricCard.Criticality.Critical
            : AshfallMetricCard.Criticality.Warn;

        _statusRail.Set("slots",    slots > 0 ? $"{slots}" : "0",          AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("capacity", $"{currentWeight:0.0}/{maxWeight:0} kg", capacityCrit);
        _statusRail.Set("active",   active > 0 ? $"{active}" : "0",         AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("station",  stationState,                          stationCrit);
        _statusRail.Set("ready",    ready > 0 ? $"{ready}" : "0",          AshfallMetricCard.Criticality.Normal);
    }

    private void RefreshQueueFooter()
    {
        if (_queueFooter == null) return;
        if (_craftingHost == null || _craftingHost.Engine.ActiveCraftCount == 0)
        {
            _queueFooter.Text = "— idle —";
            return;
        }
        var sb = new System.Text.StringBuilder();
        foreach (var c in _craftingHost.Engine.ActiveCrafts)
        {
            if (c?.Recipe == null) continue;
            if (sb.Length > 0) sb.Append(" · ");
            sb.Append(c.Recipe.recipeName).Append(": ").Append(c.HoursRemaining.ToString("F1")).Append("h");
        }
        _queueFooter.Text = sb.ToString();
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
        var recipe = GetRecipeAtVisibleRow(_selectedRecipeIndex);
        if (recipe == null || _craftingHost == null)
        {
            _detailTitle.Text = "RECIPE DETAIL";
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Select a recipe row to inspect requirements, station, and craft duration."));
            _btnStartSelected.Disabled = true;
            return;
        }
        _detailTitle.Text = recipe.recipeName.ToUpperInvariant();
        bool canCraft = _craftingHost.Engine.CanCraft(recipe);
        _btnStartSelected.Disabled = !canCraft;
        _btnStartSelected.Text = canCraft ? $"START {recipe.result?.displayName ?? "item"}" : "INGREDIENTS NEEDED";

        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("OUTPUT",
            recipe.result != null ? $"{recipe.result.displayName} ×{recipe.resultAmount}" : "—",
            canCraft ? AshfallUiHelpers.ToColor(DesignTheme.Pale) : AshfallUiHelpers.ToColor(DesignTheme.Warm)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("STATION",
            string.IsNullOrEmpty(recipe.requiredStationId) ? "[any]" : recipe.requiredStationId,
            AshfallUiHelpers.ToColor(DesignTheme.Pale)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("DURATION",
            $"{recipe.craftingTimeHours:F0}h (in-game)",
            AshfallUiHelpers.ToColor(DesignTheme.Pale)));

        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("REQUIREMENTS"));

        foreach (var ing in recipe.ingredients)
        {
            if (ing?.item == null) continue;
            int held = _inventoryHost?.Inventory.CountById(ing.item.id) ?? 0;
            bool sufficient = held >= ing.amount;
            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingXs);

            var statusMark = AshfallUiHelpers.MakeMono(sufficient ? "[OK] " : "[!!] ");
            statusMark.AddThemeColorOverride("font_color",
                AshfallUiHelpers.ToColor(
                    sufficient ? DesignTheme.Lethe : DesignTheme.Critical));
            row.AddChild(statusMark);

            var ingLabel = AshfallUiHelpers.MakeSmall(
                $"{ing.item.displayName} ×{ing.amount}  (held: {held})");
            ingLabel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            row.AddChild(ingLabel);
            _detailBox.AddChild(row);
        }

        if (!canCraft && _craftingHost != null)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                $"BLOCKED: {GetCraftBlockReason(recipe)}"));
        }
    }

    private string GetCraftBlockReason(Recipe recipe)
    {
        if (recipe.ingredients != null)
        {
            foreach (var ing in recipe.ingredients)
            {
                if (ing?.item == null) continue;
                if ((_inventoryHost?.Inventory.CountById(ing.item.id) ?? 0) < ing.amount)
                    return $"Need {ing.item.displayName} ×{ing.amount}";
            }
        }
        if (!string.IsNullOrEmpty(recipe.requiredStationId))
        {
            var station = _craftingHost?.Engine.GetStation(recipe.requiredStationId);
            if (station == null || !station.IsOperational)
                return $"Station '{recipe.requiredStationId}' unavailable";
        }
        return "Cannot craft";
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
