using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

using Ashfall.Core.IO;
namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Factions Narrative Dashboard (#49 / #53 Stitch, Phase 21).
///
/// Phase 21 closes the narrative half of the Factions Trust Tree. The data
/// half is already covered by `FactionMatrixPanel.cs`; this surface focuses on
/// the lore, ideology, and neighbor-faction relations that drive the
/// player's diplomatic decision-making.
///
/// Six cards on the status rail and six columns on the data grid. The
/// filter sidebar selects scope (all / by stance / by neighborhood).
/// Reads only from `Ashfall.Core.Economy.IFactionStanceProvider` and the
/// canonical `faction_lore.json`.
///
/// Sibling sub-card of the legacy `FactionsPanel.cs` (Phase 9 modal),
/// mirroring the same pattern as `ExpeditionRadarPanel.cs` and
/// `ExpeditionPanel.cs`. The legacy modal remains the focused interaction
/// surface; this dashboard adds the dashboard view as a Tier-2 sibling.
/// </summary>
public partial class FactionsNarrativePanel : Control
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
    private string _scopeFilter = "all"; // all | hostile | rival | trade | ally | internal

    private IFactionStanceProvider? _stance;
    private List<(string id, string display, string lore, string ideology, string neighbors)> _factions = new();

    public bool IsBound => _stance != null && _factions.Count > 0;

    public void Bind(IFactionStanceProvider stance)
    {
        _stance = stance;
        LoadFactionsFromCatalog();
        RefreshView();
    }

    /// <summary>
    /// Loads the canonical faction manifest from <c>StreamingAssets/Data/faction_lore.json</c>.
    /// The matrix survives even when the catalog has not loaded — the snapshot
    /// harness ships a deterministic fixture instead.
    /// </summary>
    private static readonly List<(string Id, string Display, string Lore, string Ideology, string Neighbors)> s_cachedFactions = new();

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
                string neighbors = entry.TryGetProperty("neighbors", out var nb) && nb.ValueKind == JsonValueKind.Array
                    ? string.Join(", ", System.Linq.Enumerable.Select(nb.EnumerateArray(), e => e.GetString() ?? ""))
                    : "—";
                s_cachedFactions.Add((id, display, lore, ideology, neighbors));
            }
            _factions.AddRange(s_cachedFactions);
        }
        catch (Exception ex_CATDIAG)
        {
            CatalogDiagnostics.Warn("<unknown>", "unknown", ex_CATDIAG);
            // ignored — fixture data will be used at row render time
        }
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("Factions Narrative // Trust Tree & Diplomacy", minWidth: 1100, minHeight: 720);
        SetContentRoot(_shell);

        var scopes = new[]
        {
            new AshfallSidebar.Item { Id = "all",      Label = "All Factions",     Hint = "every registered faction",          IconPath = "" },
            new AshfallSidebar.Item { Id = "hostile",  Label = "Hostile (Raid)",   Hint = "trust below raid threshold",         IconPath = "" },
            new AshfallSidebar.Item { Id = "rival",    Label = "Rival (Rob)",      Hint = "trust below rob threshold",          IconPath = "" },
            new AshfallSidebar.Item { Id = "trade",    Label = "Trade",            Hint = "willing to trade (no intel share)",  IconPath = "" },
            new AshfallSidebar.Item { Id = "ally",     Label = "Ally (Intel)",     Hint = "willing to share intel",             IconPath = "" },
            new AshfallSidebar.Item { Id = "internal", Label = "Internal",         Hint = "internal/internal-only factions",    IconPath = "" },
        };
        _sidebar = _shell.SetSidebar(scopes, "Trust Filter", "all");
        _sidebar.OnSelected += HandleSidebar;

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("tracked",   "Tracked",    "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("hostile",   "Hostile",    "—", AshfallMetricCard.Criticality.Critical, minWidth: 110);
        _statusRail.AddCard("trade",     "Trade",      "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
        _statusRail.AddCard("ally",      "Ally",       "—", AshfallMetricCard.Criticality.Caution, minWidth: 90);
        _statusRail.AddCard("rival",     "Rival",      "—", AshfallMetricCard.Criticality.Warn,   minWidth: 90);
        _statusRail.AddCard("avg",       "Avg Trust",  "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);

        // DataGrid columns: faction | lore | stance | trust | aggression | neighbors.
        var cols = new[]
        {
            new AshfallDataGrid.Column { Header = "Faction",     MinWidth = 200, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Lore Tag",    MinWidth = 240, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Stance",      MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Trust",       MinWidth = 80,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Aggression",  MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Neighbors",   MinWidth = 220, Alignment = AshfallDataGrid.ColumnAlign.Left },
        };
        _factionGrid = new AshfallDataGrid(cols, showHeader: true, minWidth: 720, minHeight: 320);
        _factionGrid.OnRowSelected += HandleRowSelected;

        var body = new HBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_factionGrid);

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.CustomMinimumSize = new Vector2(280, 320);
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_detailBox);

        _detailTitle = AshfallUiHelpers.MakeSectionHeader("FACTION DETAIL");
        _detailTitle.HorizontalAlignment = HorizontalAlignment.Left;
        _detailBox.AddChild(_detailTitle);
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
            "Select a faction row to view ideology, current stance, trust, and neighbor-faction relations."));

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
        var factionId = ResolveVisibleRow(idx);
        if (!string.IsNullOrEmpty(factionId))
            OnFactionSelected?.Invoke(factionId);
        RefreshDetail();
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
        if (_stance == null || _factions.Count == 0)
        {
            _statusRail.Set("tracked",  "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("hostile",  "—", AshfallMetricCard.Criticality.Critical);
            _statusRail.Set("trade",    "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("ally",     "—", AshfallMetricCard.Criticality.Caution);
            _statusRail.Set("rival",    "—", AshfallMetricCard.Criticality.Warn);
            _statusRail.Set("avg",      "—", AshfallMetricCard.Criticality.Normal);
            return;
        }

        int hostile = 0, trade = 0, ally = 0, rival = 0;
        float total = 0f;
        int count = 0;
        foreach (var f in _factions)
        {
            var stance = _stance.GetStance(f.id);
            switch (stance)
            {
                case TradeStance.HostileRaid: hostile++; break;
                case TradeStance.Rob: rival++; break;
                case TradeStance.Trade: trade++; break;
                case TradeStance.ShareIntel: ally++; trade++; break;
                case TradeStance.Refuse: break;
            }
            total += _stance.GetEffectiveTrust(f.id);
            count++;
        }
        float avg = count > 0 ? total / count : 0f;

        _statusRail.Set("tracked",  $"{_factions.Count}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("hostile",  $"{hostile}", hostile > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("trade",    $"{trade}", trade > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("ally",     $"{ally}", ally > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("rival",    $"{rival}", rival > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("avg",      $"{avg:+0;-0;0}", avg >= 0f ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Warn);
    }

    private void BuildFactionRows()
    {
        if (_factionGrid == null) return;

        if (_stance == null || _factions.Count == 0)
        {
            _factionGrid.SetRows(BuildFixtureRows());
            return;
        }

        var rows = new List<AshfallDataGrid.Row>();
        foreach (var f in _factions)
        {
            var stance = _stance.GetStance(f.id);
            if (!ScopePass(stance, f.id)) continue;
            float trust = _stance.GetEffectiveTrust(f.id);
            float aggression = ResolveAggression(f.id);
            string stanceText = StanceName(stance);
            string loreShort = string.IsNullOrEmpty(f.lore) ? "—" : Truncate(f.lore, 80);

            var cells = new List<AshfallDataGrid.Cell>
            {
                new(f.display, AshfallDataGrid.CellState.Normal),
                new(loreShort, AshfallDataGrid.CellState.Muted),
                new(stanceText, StanceState(stance)),
                new($"{trust:+0;-0;0}", trust >= 0f ? AshfallDataGrid.CellState.Normal : AshfallDataGrid.CellState.Warning),
                new($"{aggression:0.00}", AshfallDataGrid.CellState.Normal),
                new(f.neighbors, AshfallDataGrid.CellState.Muted),
            };
            rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
        }
        if (rows.Count == 0)
        {
            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new("— no factions match filter —", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                }
            });
        }
        _factionGrid.SetRows(rows);
    }

    private bool ScopePass(TradeStance stance, string factionId)
    {
        if (_scopeFilter == "all") return true;
        if (_scopeFilter == "hostile") return stance == TradeStance.HostileRaid;
        if (_scopeFilter == "rival")   return stance == TradeStance.Rob;
        if (_scopeFilter == "trade")   return stance == TradeStance.Trade;
        if (_scopeFilter == "ally")    return stance == TradeStance.ShareIntel;
        if (_scopeFilter == "internal")
        {
            if (factionId == "faction_the_office" || factionId == "faction_silent_foundry") return true;
            return false;
        }
        return true;
    }

    private string ResolveVisibleRow(int visibleIndex)
    {
        if (_stance == null) return string.Empty;
        int seen = -1;
        foreach (var f in _factions)
        {
            var stance = _stance.GetStance(f.id);
            if (!ScopePass(stance, f.id)) continue;
            seen++;
            if (seen == visibleIndex) return f.id;
        }
        return string.Empty;
    }

    private float ResolveAggression(string factionId) => 0.5f;

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        AshfallUiHelpers.EmptyChildren(_detailBox);
        if (_stance == null || _factions.Count == 0)
        {
            _detailTitle.Text = "FACTION DETAIL";
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Faction stance engine offline. Bind an IFactionStanceProvider to see live faction lore."));
            return;
        }
        if (_selectedIndex < 0)
        {
            _detailTitle.Text = "FACTION DETAIL";
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Select a faction row to view ideology, current stance, trust, and neighbor-faction relations."));
            return;
        }
        var factionId = ResolveVisibleRow(_selectedIndex);
        if (string.IsNullOrEmpty(factionId))
        {
            _detailTitle.Text = "FACTION DETAIL";
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Selected row is out of scope."));
            return;
        }
        var row = FindFaction(factionId);
        if (row == null)
        {
            _detailTitle.Text = "FACTION DETAIL";
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Faction not found."));
            return;
        }
        var rowValue = row.Value;
        var stance = _stance.GetStance(factionId);
        float trust = _stance.GetEffectiveTrust(factionId);
        _detailTitle.Text = $"{rowValue.display.ToUpperInvariant()} DETAIL";
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Stance", StanceName(stance),
            AshfallUiHelpers.ToColor(StanceToTheme(stance))));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Trust", $"{trust:+0;-0;0}",
            trust >= 0f ? AshfallUiHelpers.ToColor(DesignTheme.Lethe) : AshfallUiHelpers.ToColor(DesignTheme.Entropy)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Aggression", $"{ResolveAggression(factionId):0.00}",
            AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeSubsectionHeader("Ideology"));
        _detailBox.AddChild(AshfallUiHelpers.MakeSmall(rowValue.ideology, autowrap: true));
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeSubsectionHeader("Signature Quote"));
        _detailBox.AddChild(AshfallUiHelpers.MakeSmall(rowValue.lore, autowrap: true));
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeSubsectionHeader("Neighbors"));
        _detailBox.AddChild(AshfallUiHelpers.MakeSmall(rowValue.neighbors, autowrap: true));
    }

    private (string id, string display, string lore, string ideology, string neighbors)? FindFaction(string id)
    {
        foreach (var f in _factions)
        {
            if (f.id == id) return f;
        }
        return null;
    }

    private static string StanceName(TradeStance s) => s switch
    {
        TradeStance.Refuse     => "Refuse",
        TradeStance.HostileRaid => "Hostile (Raid)",
        TradeStance.Rob         => "Rival (Rob)",
        TradeStance.Trade       => "Trade",
        TradeStance.ShareIntel  => "Ally (Intel)",
        _ => s.ToString(),
    };

    private static AshfallDataGrid.CellState StanceState(TradeStance s) => s switch
    {
        TradeStance.Refuse     => AshfallDataGrid.CellState.Muted,
        TradeStance.HostileRaid => AshfallDataGrid.CellState.Critical,
        TradeStance.Rob         => AshfallDataGrid.CellState.Warning,
        TradeStance.Trade       => AshfallDataGrid.CellState.Normal,
        TradeStance.ShareIntel  => AshfallDataGrid.CellState.Positive,
        _ => AshfallDataGrid.CellState.Normal,
    };

    private static (float r, float g, float b, float a) StanceToTheme(TradeStance s) => s switch
    {
        TradeStance.Refuse     => DesignTheme.Muted,
        TradeStance.HostileRaid => DesignTheme.Critical,
        TradeStance.Rob         => DesignTheme.Entropy,
        TradeStance.Trade       => DesignTheme.Lethe,
        TradeStance.ShareIntel  => DesignTheme.Lethe,
        _ => DesignTheme.Dim,
    };

    private static string Truncate(string text, int max)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        if (text.Length <= max) return text;
        return text.Substring(0, Math.Max(0, max - 1)) + "…";
    }

    /// <summary>Hard-coded fixture rows for the bound=false case. Each row uses a canonical faction id and lore drawn from the user's own ASHFALL faction_lore.json catalog.</summary>
    internal static List<AshfallDataGrid.Row> BuildFixtureRows()
    {
        return new List<AshfallDataGrid.Row>
        {
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("The Office",               AshfallDataGrid.CellState.Normal),
                    new("By the warm bulb, the stand-down paper is still wet.", AshfallDataGrid.CellState.Muted),
                    new("Ally (Intel)",             AshfallDataGrid.CellState.Positive),
                    new("+62",                       AshfallDataGrid.CellState.Normal),
                    new("0.20",                      AshfallDataGrid.CellState.Normal),
                    new("Iron Garrison, Caravan Barons", AshfallDataGrid.CellState.Muted),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("Iron Garrison",            AshfallDataGrid.CellState.Normal),
                    new("Hold the line. The ash is on us.", AshfallDataGrid.CellState.Muted),
                    new("Trade",                    AshfallDataGrid.CellState.Normal),
                    new("+12",                       AshfallDataGrid.CellState.Normal),
                    new("0.30",                      AshfallDataGrid.CellState.Normal),
                    new("The Office, Hydro Barons", AshfallDataGrid.CellState.Muted),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("Hydro Barons",             AshfallDataGrid.CellState.Normal),
                    new("Brine is a currency; the rest is bookkeeping.", AshfallDataGrid.CellState.Muted),
                    new("Rival (Rob)",               AshfallDataGrid.CellState.Warning),
                    new("-23",                       AshfallDataGrid.CellState.Warning),
                    new("0.55",                      AshfallDataGrid.CellState.Normal),
                    new("Iron Garrison, Caravan Barons", AshfallDataGrid.CellState.Muted),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("Warlord Sectors",          AshfallDataGrid.CellState.Normal),
                    new("Take what we cannot keep. The rest is fuel.", AshfallDataGrid.CellState.Muted),
                    new("Hostile (Raid)",            AshfallDataGrid.CellState.Critical),
                    new("-67",                       AshfallDataGrid.CellState.Critical),
                    new("0.85",                      AshfallDataGrid.CellState.Normal),
                    new("Caravan Barons, the Office", AshfallDataGrid.CellState.Muted),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("The Silent Foundry",       AshfallDataGrid.CellState.Normal),
                    new("We are a guild. We do not stand down.", AshfallDataGrid.CellState.Muted),
                    new("Ally (Intel)",              AshfallDataGrid.CellState.Positive),
                    new("+38",                       AshfallDataGrid.CellState.Normal),
                    new("0.20",                      AshfallDataGrid.CellState.Normal),
                    new("Iron Garrison, The Office", AshfallDataGrid.CellState.Muted),
                }, Selectable = true },
        };
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
