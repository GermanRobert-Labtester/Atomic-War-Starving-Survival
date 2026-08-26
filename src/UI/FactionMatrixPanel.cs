using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;
using Ashfall.Core.Economy;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

using Ashfall.Core.IO;
namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Faction Matrix (#49 / #53 Stitch).
///
/// Cross-faction dashboard. Reads every cell directly from authoritative
/// sources: <see cref="IFactionStanceProvider"/> for live trust / stance /
/// aggression; <c>faction_lore.json</c> for one-line lore.
///
/// The matrix surfaces *all* registered factions rather than only the
/// currently active trader — that is the difference from Phase 12's
/// `CaravanBarterLedgerPanel`.
///
/// Pure presentation. Reads only; never mutates trust or thresholds.
/// </summary>
public partial class FactionMatrixPanel : Control
{
    public event Action? OnClose;
    public event Action<string>? OnFactionSelected;

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _factionGrid;
    private VBoxContainer _detailBox = null!;
    private Label _detailTitle = null!;
    private int _selectedIndex = -1;

    private IFactionStanceProvider? _stanceProvider;
    private List<(string id, string display, string lore, string ideology)> _factions = new();
    private string _selectedFactionId = string.Empty;

    public bool IsBound => _stanceProvider != null && _factions.Count > 0;

    public void Bind(IFactionStanceProvider stanceProvider)
    {
        _stanceProvider = stanceProvider;
        LoadFactionsFromCatalog();
        RefreshView();
    }

    public void BindWithFactions(IFactionStanceProvider stanceProvider,
        IEnumerable<(string id, string display, string lore, string ideology)> factions)
    {
        _stanceProvider = stanceProvider;
        _factions.Clear();
        _factions.AddRange(factions);
        RefreshView();
    }

    /// <summary>
    /// Loads the canonical faction manifest from <c>StreamingAssets/Data/faction_lore.json</c>.
    /// The matrix survives even when the catalog has not loaded — the snapshot
    /// harness ships a deterministic fixture instead.
    /// </summary>
    private static readonly List<(string Id, string Display, string Lore, string Ideology)> s_cachedFactions = new();

    private void LoadFactionsFromCatalog()
    {
        _factions.Clear();
        if (s_cachedFactions.Count > 0)
        {
            _factions.AddRange(s_cachedFactions);
            return;
        }

        try
        {
            string osPath = ProjectSettings.GlobalizePath("res://Assets/StreamingAssets/Data/faction_lore.json");
            if (!File.Exists(osPath)) return;
            using var stream = File.OpenRead(osPath);
            using var doc = JsonDocument.Parse(stream);
            var root = doc.RootElement;
            if (root.ValueKind != JsonValueKind.Array) return;
            foreach (var entry in root.EnumerateArray())
            {
                string id = entry.TryGetProperty("faction_id", out var fid) ? fid.GetString() ?? "" : "";
                if (string.IsNullOrEmpty(id)) continue;
                string display = entry.TryGetProperty("display_name", out var dn) ? dn.GetString() ?? id : id;
                string lore = entry.TryGetProperty("signature_quote", out var sq) ? sq.GetString() ?? "" : "";
                string ideology = entry.TryGetProperty("ideology", out var ideo) ? ideo.GetString() ?? "" : "";
                s_cachedFactions.Add((id, display, lore, ideology));
            }
            _factions.AddRange(s_cachedFactions);
        }
        catch (Exception ex_CATDIAG)
        {
            CatalogDiagnostics.Warn("<unknown>", "unknown", ex_CATDIAG);
            // ignored — fixture data will be used at row render time
        }
    }

    public void RefreshView()
    {
        RefreshStatusRail();
        BuildFactionRows();
        RefreshDetail();
    }

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        if (_stanceProvider == null || _factions.Count == 0)
        {
            _statusRail.Set("tracked", "0", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("trade",    "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("hostile",  "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("avg_trust","0", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("max_agg",  "0.00", AshfallMetricCard.Criticality.Normal);
            return;
        }

        int trade = 0, hostile = 0;
        float totalTrust = 0f, maxAgg = 0f;
        foreach (var f in _factions)
        {
            var s = _stanceProvider.GetStance(f.id);
            if (s == TradeStance.Trade || s == TradeStance.ShareIntel) trade++;
            if (s == TradeStance.HostileRaid || s == TradeStance.Rob) hostile++;
            totalTrust += _stanceProvider.GetEffectiveTrust(f.id);
            float a = _stanceProvider.GetRaidAggression(f.id);
            if (a > maxAgg) maxAgg = a;
        }
        float avgTrust = _factions.Count > 0 ? totalTrust / _factions.Count : 0f;
        _statusRail.Set("tracked",   $"{_factions.Count}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("trade",     trade > 0 ? $"{trade}" : "0",    trade > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Critical);
        _statusRail.Set("hostile",   hostile > 0 ? $"{hostile}" : "0", hostile > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("avg_trust", $"{avgTrust:+#;-#;0}",            AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("max_agg",   $"{maxAgg:0.00}",                  AshfallMetricCard.Criticality.Normal);
    }

    private void BuildFactionRows()
    {
        if (_factionGrid == null) return;
        if (_stanceProvider == null || _factions.Count == 0)
        {
            _factionGrid.SetRows(BuildFixtureRows());
            return;
        }

        var rows = new List<AshfallDataGrid.Row>();
        foreach (var f in _factions)
        {
            var stance = _stanceProvider.GetStance(f.id);
            float trust = _stanceProvider.GetEffectiveTrust(f.id);
            float aggression = _stanceProvider.GetRaidAggression(f.id);
            bool active = IsActive(f.id);

            var stanceState = MapStanceState(stance);
            var trustState = MapTrustState(trust);
            var aggState = aggression > 0.66f ? AshfallDataGrid.CellState.Critical
                : aggression > 0.4f ? AshfallDataGrid.CellState.Warning
                : aggression > 0.2f ? AshfallDataGrid.CellState.Caution
                : AshfallDataGrid.CellState.Normal;

            var cells = new List<AshfallDataGrid.Cell>
            {
                new(f.display, AshfallDataGrid.CellState.Normal),
                new(!active ? "DORMANT" : stance.ToString().ToUpperInvariant(), stanceState),
                new(trust > 0 ? $"+{trust:0}" : trust.ToString("0"), trustState),
                new($"{aggression:0.00}", aggState),
                new(active ? "ACTIVE" : "INACTIVE", active ? AshfallDataGrid.CellState.Positive : AshfallDataGrid.CellState.Muted),
            };
            rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
        }
        _factionGrid.SetRows(rows);
    }

    private bool IsActive(string factionId)
    {
        if (_stanceProvider == null) return true;
        try
        {
            var t = _stanceProvider.GetType();
            var prop = t.GetProperty("IsFactionActive");
            if (prop?.GetValue(_stanceProvider) is bool b) return b;
        }
        catch (Exception ex) { GD.PrintErr($"[FactionMatrix] IsFactionActive probe failed: {ex.Message}"); }
        return true;
    }

    internal static AshfallDataGrid.CellState MapStanceState(TradeStance s) => s switch
    {
        TradeStance.ShareIntel => AshfallDataGrid.CellState.Positive,
        TradeStance.Trade => AshfallDataGrid.CellState.Normal,
        TradeStance.Refuse => AshfallDataGrid.CellState.Caution,
        TradeStance.Rob => AshfallDataGrid.CellState.Warning,
        TradeStance.HostileRaid => AshfallDataGrid.CellState.Critical,
        _ => AshfallDataGrid.CellState.Muted,
    };

    internal static AshfallDataGrid.CellState MapTrustState(float trust) => trust switch
    {
        >= 60f => AshfallDataGrid.CellState.Positive,
        >= 20f => AshfallDataGrid.CellState.Normal,
        >= -20f => AshfallDataGrid.CellState.Caution,
        >= -60f => AshfallDataGrid.CellState.Warning,
        _ => AshfallDataGrid.CellState.Critical,
    };

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        AshfallUiHelpers.EmptyChildren(_detailBox);
        var (id, display, lore, ideology) = GetFactionAtVisibleRow(_selectedIndex);
        if (id == null)
        {
            _detailTitle.Text = "FACTION DETAIL";
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Select a row to inspect ideology, lore, and live stance."));
            return;
        }
        _detailTitle.Text = display.ToUpperInvariant();
        var stance = _stanceProvider?.GetStance(id) ?? TradeStance.Refuse;
        float trust = _stanceProvider?.GetEffectiveTrust(id) ?? 0f;
        float aggression = _stanceProvider?.GetRaidAggression(id) ?? 0f;
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("STANCE",  stance.ToString().ToUpperInvariant(), AshfallUiHelpers.ToColor(DashStanceToken(stance))));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("TRUST",   trust > 0 ? $"+{trust:0}" : trust.ToString("0"), AshfallUiHelpers.ToColor(DashTrustToken(trust))));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("AGGRESSION", $"{aggression:0.00}",
            aggression > 0.66f ? AshfallUiHelpers.ToColor(DesignTheme.Critical)
            : aggression > 0.4f ? AshfallUiHelpers.ToColor(DesignTheme.Entropy)
            : AshfallUiHelpers.ToColor(DesignTheme.Pale)));

        if (!string.IsNullOrWhiteSpace(ideology))
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("IDEOLOGY"));
            _detailBox.AddChild(AshfallUiHelpers.MakeBody(ideology));
        }
        if (!string.IsNullOrWhiteSpace(lore))
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("SIGNATURE QUOTE"));
            _detailBox.AddChild(AshfallUiHelpers.MakeBody("\"" + lore + "\""));
        }
    }

    private (string? id, string display, string lore, string ideology) GetFactionAtVisibleRow(int idx)
    {
        if (idx < 0) return (null, "", "", "");
        var src = _factions.Count > 0 ? _factions : BuildFixtureFactions();
        if (idx >= src.Count) return (null, "", "", "");
        var (id, display, lore, ideology) = src[idx];
        _selectedFactionId = id;
        return (id, display, lore, ideology);
    }

    private static (float r, float g, float b, float a) DashStanceToken(TradeStance s) => MapStanceState(s) switch
    {
        AshfallDataGrid.CellState.Positive => DesignTheme.Lethe,
        AshfallDataGrid.CellState.Caution => DesignTheme.Lethe,
        AshfallDataGrid.CellState.Warning => DesignTheme.Entropy,
        AshfallDataGrid.CellState.Critical => DesignTheme.Critical,
        _ => DesignTheme.Pale,
    };

    private static (float r, float g, float b, float a) DashTrustToken(float trust) => MapTrustState(trust) switch
    {
        AshfallDataGrid.CellState.Positive => DesignTheme.Lethe,
        AshfallDataGrid.CellState.Caution => DesignTheme.Lethe,
        AshfallDataGrid.CellState.Warning => DesignTheme.Entropy,
        AshfallDataGrid.CellState.Critical => DesignTheme.Critical,
        _ => DesignTheme.Pale,
    };

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);
        Visible = false;

        var bg = new ColorRect { Color = new Color(0.04f, 0.04f, 0.05f, 0.92f) };
        bg.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(bg);

        _shell = new AshfallDashboardShell(
            "FACTION MATRIX — CROSS_FACTION_REGISTRY",
            1180, 720);

        var hostContainer = new MarginContainer();
        hostContainer.AddThemeConstantOverride("margin_left", DesignTheme.SpacingLg);
        hostContainer.AddThemeConstantOverride("margin_top", DesignTheme.SpacingLg);
        hostContainer.AddThemeConstantOverride("margin_right", DesignTheme.SpacingLg);
        hostContainer.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingLg);
        hostContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        hostContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        hostContainer.AddChild(_shell);
        AddChild(hostContainer);

        _sidebar = _shell.SetSidebar(new[]
        {
            new AshfallSidebar.Item { Id = "sort_default",       Label = "Sort: Default",      Hint = "as registered" },
            new AshfallSidebar.Item { Id = "filter_active",     Label = "Filter: Active",     Hint = "currently active" },
            new AshfallSidebar.Item { Id = "filter_hostile",    Label = "Filter: Hostile",    Hint = "Rob / Raid" },
            new AshfallSidebar.Item { Id = "filter_tradeable",  Label = "Filter: Tradeable",  Hint = "Trade / Intel" },
        }, "FACTION OPS", "sort_default");

        if (_sidebar != null)
        {
            _sidebar.OnSelected += id =>
            {
                if (_factionGrid == null) return;
                if (id == "sort_default")      { _activeFilter = "all";        BuildFactionRows(); }
                else if (id == "filter_active")     { _activeFilter = "active";    BuildFactionRows(); }
                else if (id == "filter_hostile")    { _activeFilter = "hostile";   BuildFactionRows(); }
                else if (id == "filter_tradeable")  { _activeFilter = "tradeable"; BuildFactionRows(); }
            };
        }

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("tracked",   "TRACKED",   "0", AshfallMetricCard.Criticality.Normal, 110);
        _statusRail.AddCard("trade",     "TRADEABLE", "0", AshfallMetricCard.Criticality.Normal, 130);
        _statusRail.AddCard("hostile",   "HOSTILE",   "0", AshfallMetricCard.Criticality.Normal, 110);
        _statusRail.AddCard("avg_trust", "AVG TRUST", "0", AshfallMetricCard.Criticality.Normal, 130);
        _statusRail.AddCard("max_agg",   "MAX AGGR",  "0.00", AshfallMetricCard.Criticality.Normal, 130);

        _shell.AttachHeaderCloseButton("CLOSE [Esc]", () => OnClose?.Invoke());

        BuildContent();
        if (_factions.Count == 0) LoadFactionsFromCatalog();
        RefreshView();
    }

    private string _activeFilter = "all"; // all | active | hostile | tradeable

    private void BuildContent()
    {
        var contentStack = new HBoxContainer();
        contentStack.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        contentStack.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        contentStack.SizeFlagsVertical = Control.SizeFlags.ExpandFill;

        var gridCol = new VBoxContainer();
        gridCol.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        gridCol.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        gridCol.SizeFlagsStretchRatio = 1.45f;
        gridCol.AddChild(AshfallUiHelpers.MakeSectionHeader("FACTION REGISTRY"));

        var columns = new[]
        {
            new AshfallDataGrid.Column { Header = "Faction",    MinWidth = 200, Alignment = AshfallDataGrid.ColumnAlign.Left  },
            new AshfallDataGrid.Column { Header = "Stance",     MinWidth = 120, Alignment = AshfallDataGrid.ColumnAlign.Center },
            new AshfallDataGrid.Column { Header = "Trust",      MinWidth = 80,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Aggression", MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "State",      MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Center },
        };
        _factionGrid = new AshfallDataGrid(columns, showHeader: true, minWidth: 600, minHeight: 360);
        _factionGrid.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        _factionGrid.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        _factionGrid.OnRowSelected += idx =>
        {
            _selectedIndex = idx;
            RefreshDetail();
            var (id, _, _, _) = GetFactionAtVisibleRow(idx);
            if (!string.IsNullOrEmpty(id)) OnFactionSelected?.Invoke(id);
        };
        gridCol.AddChild(_factionGrid);

        var legendRow = new HBoxContainer();
        legendRow.AddThemeConstantOverride("separation", DesignTheme.SpacingLg);
        LegendChip(legendRow, "TRADE",   DesignTheme.Lethe);
        LegendChip(legendRow, "ROB",     DesignTheme.Entropy);
        LegendChip(legendRow, "RAID",    DesignTheme.Critical);
        LegendChip(legendRow, "SHARE",   DesignTheme.Hot);
        gridCol.AddChild(legendRow);

        contentStack.AddChild(gridCol);

        var detailPanel = AshfallUiHelpers.MakePanel();
        detailPanel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        detailPanel.SizeFlagsStretchRatio = 1.0f;
        var detailMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
        detailPanel.AddChild(detailMargin);

        var detailVBox = new VBoxContainer();
        detailVBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        detailMargin.AddChild(detailVBox);

        _detailTitle = new Label { Text = "FACTION DETAIL" };
        _detailTitle.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeH3);
        _detailTitle.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
        var font = AshfallUiHelpers.LoadFont("res://assets/fonts/BarlowCondensed-SemiBold.ttf");
        if (font != null) _detailTitle.AddThemeFontOverride("font", font);
        detailVBox.AddChild(_detailTitle);
        detailVBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        _detailBox.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        detailVBox.AddChild(_detailBox);

        contentStack.AddChild(detailPanel);
        _shell.SetContent(contentStack);
    }

    private static void LegendChip(HBoxContainer host, string label, (float r, float g, float b, float a) token)
    {
        var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingXs);
        var dot = new ColorRect { Color = AshfallUiHelpers.ToColor(token), CustomMinimumSize = new Vector2(8, 8) };
        row.AddChild(dot);
        var lbl = AshfallUiHelpers.MakeSmall(label);
        lbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
        row.AddChild(lbl);
        host.AddChild(row);
    }

    private static IList<(string id, string display, string lore, string ideology)> BuildFixtureFactions() => new List<(string, string, string, string)>
    {
        ("faction_central_garrison", "The Iron Garrison",
            "Civilization isn't built on sympathy; it's maintained by supply chains and ammunition counts.",
            "Military continuity, strict resource rationing, absolute martial law"),
        ("warlords_sector_4", "The Warlords of Sector 4",
            "Clean water costs blood or bullets. Pick which one you're paying with today.",
            "Mercenary opportunism, trade control, survival of the fittest"),
        ("ash_militia", "The Ash Militia",
            "If we turn into monsters just to survive the fallout, then the war already won.",
            "Local democracy, resource sharing, mutual defense, civilian autonomy"),
        ("cult_of_ash_sign", "The Cult of the Ash Sign",
            "Do not fear the glow, child. The fire burned away the old world's lies. Drink the ash and be renewed.",
            "Apocalyptic purification, radiation worship, ascetic martyrdom"),
        ("faction_rebuilders", "The Rebuilders",
            "The world went to fire once. It shall not again if we hold fast to the rule of law and to one another.",
            "Civil restructuring, democratic continuity, restoration of utilities"),
    };

    private static List<AshfallDataGrid.Row> BuildFixtureRows()
    {
        var rows = new List<AshfallDataGrid.Row>();
        foreach (var f in BuildFixtureFactions())
        {
            var cells = new List<AshfallDataGrid.Cell>
            {
                new(f.display, AshfallDataGrid.CellState.Normal),
                new("—", AshfallDataGrid.CellState.Muted),
                new("—", AshfallDataGrid.CellState.Muted),
                new("—", AshfallDataGrid.CellState.Muted),
                new("UNBIND", AshfallDataGrid.CellState.Muted),
            };
            rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
        }
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

    public override void _ExitTree()
    {
        _factions.Clear();
        base._ExitTree();
    }
}
