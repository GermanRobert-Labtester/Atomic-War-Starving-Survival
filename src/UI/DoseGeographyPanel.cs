// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Radiation;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Dose Geography (Plan 81 radiation-cartography surface).
///
/// The sector-aware view over <see cref="Assets/Ashfall.Core DoseContentCatalog"/>
/// locations: every dose location with its canonical sector, authored risk
/// level (0–8, numeral + tier word — never color-only) and its baseline
/// exposure rate in truthful µSv/h. Row selection shows the environmental
/// description and the dose actually booked at that place, from ledger
/// provenance.
///
/// Pure presentation — reads only from <see cref="DoseLedgerHostSession"/>.
/// Unbound renders an honest empty state (no fixture rows; UI-19 discipline).
/// Registered as the 'dose_geography' expanded route (PanelRegistryBootstrap)
/// and reachable from the dashboard navigation rail.
/// </summary>
public partial class DoseGeographyPanel : Control
{
    public event Action? OnClose;

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _grid;
    private VBoxContainer _detailBox = null!;
    private Label _detailTitle = null!;
    private int _selectedIndex = -1;
    private string _sectorFilter = "all";

    private DoseLedgerHostSession? _dose;
    private RadiationHotspotPolicy? _hotspotPolicy;
    private readonly List<DoseLocationDef> _visibleLocations = new();
    private readonly StringBuilder _renderDump = new();

    /// <summary>Optional authored hotspot policy; falls back to the Core default
    /// bands (mirrors radiation_hotspot_policy.json) when unbound.</summary>
    public void BindHotspotPolicy(RadiationHotspotPolicy? policy)
    {
        _hotspotPolicy = policy;
        RefreshView();
    }

    private RadiationHotspotPolicy HotspotPolicy => _hotspotPolicy ?? RadiationHotspotPolicy.Default;

    private IReadOnlyList<HotspotRow> HotspotWatch()
    {
        var locations = _dose?.Content?.locations;
        if (locations == null) return Array.Empty<HotspotRow>();
        return RadiationHotspotSurvey.Classify(HotspotPolicy, locations);
    }

    /// <summary>Alpha feature G4 — pre-departure check over the sectors the
    /// current filter shows (the player's planned route). Read-only.</summary>
    private RouteDoseCheck RouteCheck()
    {
        var locations = _dose?.Content?.locations;
        if (locations == null) return default;

        var planned = new List<DoseLocationDef>();
        foreach (var location in locations)
        {
            if (location == null || !SectorPass(location.sector)) continue;
            planned.Add(location);
        }
        return RouteDoseCheckService.Evaluate(HotspotPolicy, planned);
    }

    public bool IsBound => _dose != null;

    /// <summary>Rendered row/rail text from the last RefreshView, for
    /// headless UI assertions (route→bind→visible→rendered-strings).</summary>
    internal string RenderDump => _renderDump.ToString();

    public void Bind(DoseLedgerHostSession? session)
    {
        Unbind();
        _dose = session;
        if (_dose?.Ledger != null)
            _dose.Ledger.OnStateChanged += HandleLedgerChanged;
        RefreshView();
    }

    public void Unbind()
    {
        if (_dose?.Ledger != null)
            _dose.Ledger.OnStateChanged -= HandleLedgerChanged;
        _dose = null;
    }

    private void HandleLedgerChanged(DoseLedgerSystemState _) => RefreshView();

    public void RefreshView()
    {
        RefreshStatusRail();
        BuildRows();
        RefreshDetail();
    }

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        var locations = _dose?.Content?.locations;
        if (locations == null || locations.Count == 0)
        {
            _statusRail.Set("places", "0", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("sectors", "0", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("peak", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("risk", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("hot", "0", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("events", "0", AshfallMetricCard.Criticality.Normal);
            return;
        }

        int maxRisk = 0;
        float maxRate = 0f;
        var sectors = new HashSet<string>(StringComparer.Ordinal);
        foreach (var l in locations)
        {
            if (l == null) continue;
            sectors.Add(l.sector);
            if (l.riskLevel > maxRisk) maxRisk = l.riskLevel;
            if (l.radiationUsv > maxRate) maxRate = l.radiationUsv;
        }
        int hot = 0;
        foreach (var l in locations)
            if (l != null && l.riskLevel >= 5) hot++;

        _statusRail.Set("places", $"{locations.Count}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("sectors", $"{sectors.Count}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("peak", $"{maxRate:0.00} µSv/h", maxRate >= 10f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("risk", $"{maxRisk} · {RiskTier(maxRisk)}", maxRisk >= 5 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("hot", $"{hot}", hot > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("events", $"{CountProvenanceEvents()}", AshfallMetricCard.Criticality.Normal);

        // Alpha feature F4 — hotspot watch list (read-only severity projection).
        var watch = HotspotWatch();
        _statusRail.Set("watch", $"{watch.Count}",
            watch.Count > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
        _renderDump.Append("WATCH ").Append(watch.Count).Append(" | ");
        foreach (var row in watch.Take(3))
        {
            _renderDump.Append(row.DisplayName).Append(" ").Append(row.RadiationUsv.ToString("0.0"))
                .Append(" µSv/h ").Append(row.Severity).Append(" | ");
        }

        // Alpha feature G4 — route exposure check for the filtered sectors.
        var route = RouteCheck();
        if (route.Clear)
        {
            _statusRail.Set("route", "clear", AshfallMetricCard.Criticality.Normal);
            _renderDump.Append("ROUTE clear | ");
        }
        else
        {
            var worst = route.Worst!.Value;
            _statusRail.Set("route", $"{worst.DisplayName} · {worst.Severity.ToUpperInvariant()}",
                worst.Severity is "extreme" or "high" ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Warn);
            _renderDump.Append("ROUTE worst ").Append(worst.DisplayName).Append(" ").Append(worst.RadiationUsv.ToString("0.0"))
                .Append(" µSv/h; safest ").Append(route.Safest?.DisplayName ?? "—").Append(" | ");
        }
    }

    private void BuildRows()
    {
        if (_grid == null) return;
        _visibleLocations.Clear();
        _renderDump.Clear();

        var locations = _dose?.Content?.locations;
        if (_dose == null || locations == null || locations.Count == 0)
        {
            // Honest empty state — no fixture rows (UI-19 discipline).
            _grid.SetRows(new List<AshfallDataGrid.Row>());
            _renderDump.AppendLine("No dose content loaded.");
            return;
        }

        var rows = new List<AshfallDataGrid.Row>();
        foreach (var l in locations)
        {
            if (l == null || string.IsNullOrEmpty(l.id)) continue;
            if (!SectorPass(l.sector)) continue;
            int idx = _visibleLocations.Count;
            _visibleLocations.Add(l);

            int events = ProvenanceEventCount(l.id);
            var state = l.riskLevel >= 5 ? AshfallDataGrid.CellState.Warning
                : l.riskLevel >= 3 ? AshfallDataGrid.CellState.Caution
                : AshfallDataGrid.CellState.Normal;

            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new(l.displayName, state),
                    new(l.sector, AshfallDataGrid.CellState.Muted),
                    new($"{l.riskLevel} · {RiskTier(l.riskLevel)}", state),
                    new($"{l.radiationUsv:0.00} µSv/h", state),
                    new(events > 0 ? $"{events}" : "—", events > 0 ? AshfallDataGrid.CellState.Normal : AshfallDataGrid.CellState.Muted),
                },
                Selectable = true,
            });

            _renderDump.Append(l.displayName).Append(" | ").Append(l.sector)
                .Append(" | risk ").Append(l.riskLevel)
                .Append(" | ").Append(l.radiationUsv.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture))
                .Append(" µSv/h\n");
        }
        _grid.SetRows(rows);
    }

    private bool SectorPass(string sector) => _sectorFilter switch
    {
        "bunker" => sector == "bunker",
        "surface" => sector == "surface",
        "expedition" => sector == "expedition",
        "external" => sector == "external",
        "faction" => sector == "faction",
        _ => true,
    };

    private int CountProvenanceEvents()
    {
        int total = 0;
        var ledger = _dose?.Ledger;
        if (ledger == null) return 0;
        foreach (var e in ledger.Entries)
        {
            if (e?.readingsHistory == null) continue;
            foreach (var r in e.readingsHistory)
            {
                if (r != null && ProvenanceEventCount(r.source) >= 0 && MatchesAnyVisibleLocation(r.source))
                    total++;
            }
        }
        return total;
    }

    private bool MatchesAnyVisibleLocation(string source)
    {
        var locations = _dose?.Content?.locations;
        if (locations == null) return false;
        foreach (var l in locations)
            if (l != null && l.id == source) return true;
        return false;
    }

    private int ProvenanceEventCount(string locationId)
    {
        int count = 0;
        var ledger = _dose?.Ledger;
        if (ledger == null) return 0;
        foreach (var e in ledger.Entries)
        {
            if (e?.readingsHistory == null) continue;
            foreach (var r in e.readingsHistory)
                if (r != null && r.source == locationId) count++;
        }
        return count;
    }

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        AshfallUiHelpers.EmptyChildren(_detailBox);

        if (_selectedIndex < 0 || _selectedIndex >= _visibleLocations.Count || _dose == null)
        {
            _detailTitle.Text = "EXPOSURE GEOGRAPHY";
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Select a place to read why it is radioactive, and what the ledger has booked there."));

            // Alpha feature F4 — hotspot watch list, worst first.
            var watch = HotspotWatch();
            if (watch.Count > 0)
            {
                _detailBox.AddChild(AshfallUiHelpers.MakeSubsectionHeader("HOTSPOT WATCH LIST"));
                foreach (var row in watch.Take(5))
                {
                    _detailBox.AddChild(AshfallUiHelpers.MakeDataRow(
                        row.DisplayName, $"{row.RadiationUsv:0.0} µSv/h · {row.Label}",
                        AshfallUiHelpers.ToColor(row.Severity switch
                        {
                            "extreme" => DesignTheme.Entropy,
                            "high" => DesignTheme.Critical,
                            "elevated" => DesignTheme.Hot,
                            _ => DesignTheme.Warm,
                        })));
                }
                _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                    "Every reading is already booked in the dose ledger — survey it before you travel, not after."));
            }
            return;
        }

        var l = _visibleLocations[_selectedIndex];
        _detailTitle.Text = l.displayName.ToUpperInvariant() + " · " + l.sector.ToUpperInvariant();

        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("SECTOR", l.sector, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("RISK LEVEL", $"{l.riskLevel} · {RiskTier(l.riskLevel)}",
            AshfallUiHelpers.ToColor(l.riskLevel >= 5 ? DesignTheme.Entropy : DesignTheme.Pale)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("BASELINE RATE", $"{l.radiationUsv:0.00} µSv/h",
            AshfallUiHelpers.ToColor(l.radiationUsv >= 10f ? DesignTheme.Entropy : DesignTheme.Warm)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("1-HOUR DWELL", $"{l.radiationUsv:0.00} µSv",
            AshfallUiHelpers.ToColor(DesignTheme.Pale)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("4-HOUR SORTIE", $"{l.radiationUsv * 4f:0.0} µSv",
            AshfallUiHelpers.ToColor(DesignTheme.Pale)));

        if (!string.IsNullOrEmpty(l.description))
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("FIELD NOTE"));
            var desc = AshfallUiHelpers.MakeSmall(l.description);
            desc.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _detailBox.AddChild(desc);
        }

        // Ledger provenance for this place — where the dose actually came from.
        float bookedHere = 0f;
        int readings = 0;
        int lastDay = -1;
        var ledger = _dose.Ledger;
        foreach (var e in ledger.Entries)
        {
            if (e?.readingsHistory == null) continue;
            foreach (var r in e.readingsHistory)
            {
                if (r == null || r.source != l.id) continue;
                readings++;
                bookedHere += r.bookedMsv;
                if (r.day > lastDay) lastDay = r.day;
            }
        }
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("LEDGER PROVENANCE"));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("BOOKED HERE",
            readings > 0 ? $"{AshfallUiHelpers.FormatDoseMsv(bookedHere)} across {readings} reading(s)" : "nothing booked yet",
            AshfallUiHelpers.ToColor(DesignTheme.Warm)));
        if (lastDay >= 0)
            _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("LAST READING", $"day {lastDay}",
                AshfallUiHelpers.ToColor(DesignTheme.Pale)));
    }

    /// <summary>Player-facing risk tier vocabulary (0–8). Numeral + word —
    /// risk is never communicated by color alone (81AW).</summary>
    internal static string RiskTier(int risk) => risk switch
    {
        0 => "Shielded",
        1 => "Transition",
        2 => "Exposed",
        3 => "Contaminated",
        4 => "Corridor",
        5 => "Hot Perimeter",
        6 => "Severe Ruins",
        7 => "Hot Zone",
        8 => "Extreme",
        _ => "—",
    };

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);
        Visible = false;

        var bg = new ColorRect { Color = new Color(0.04f, 0.04f, 0.05f, 0.92f) };
        bg.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(bg);

        _shell = new AshfallDashboardShell("DOSE GEOGRAPHY — EXPOSURE MAP", 1180, 720);

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
            new AshfallSidebar.Item { Id = "all",        Label = "All sectors",   Hint = "every standing place" },
            new AshfallSidebar.Item { Id = "bunker",     Label = "Bunker",        Hint = "shielded interior" },
            new AshfallSidebar.Item { Id = "surface",    Label = "Surface",       Hint = "the threshold above" },
            new AshfallSidebar.Item { Id = "expedition", Label = "Expedition",    Hint = "destination hot zones" },
            new AshfallSidebar.Item { Id = "external",   Label = "External",      Hint = "travel corridors" },
            new AshfallSidebar.Item { Id = "faction",    Label = "Faction",       Hint = "checkpoint approaches" },
        }, "SECTORS", "all");

        if (_sidebar != null)
        {
            _sidebar.OnSelected += id =>
            {
                _sectorFilter = id;
                // UI/UX wave: sector filter swaps animate in place.
                UiPanelFlow.TransitionSwap(_grid, () => { BuildRows(); RefreshDetail(); });
            };
        }

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("places", "PLACES", "0", AshfallMetricCard.Criticality.Normal, 100);
        _statusRail.AddCard("sectors", "SECTORS", "0", AshfallMetricCard.Criticality.Normal, 100);
        _statusRail.AddCard("peak", "PEAK RATE", "—", AshfallMetricCard.Criticality.Normal, 130);
        _statusRail.AddCard("risk", "PEAK RISK", "—", AshfallMetricCard.Criticality.Normal, 130);
        _statusRail.AddCard("hot", "HOT ZONES", "0", AshfallMetricCard.Criticality.Normal, 100);
        _statusRail.AddCard("watch", "WATCH LIST", "0", AshfallMetricCard.Criticality.Normal, 110);
        _statusRail.AddCard("route", "ROUTE CHECK", "—", AshfallMetricCard.Criticality.Normal, 170);
        _statusRail.AddCard("events", "READINGS", "0", AshfallMetricCard.Criticality.Normal, 100);

        _shell.AttachHeaderCloseButton("CLOSE [Esc]", () => OnClose?.Invoke());
        BuildContent();
        RefreshView();
    }

    private void BuildContent()
    {
        var contentStack = new HBoxContainer();
        contentStack.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        contentStack.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        contentStack.SizeFlagsVertical = Control.SizeFlags.ExpandFill;

        var gridCol = new VBoxContainer();
        gridCol.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        gridCol.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        gridCol.SizeFlagsStretchRatio = 1.5f;
        gridCol.AddChild(AshfallUiHelpers.MakeSectionHeader("STANDING PLACES"));

        var columns = new[]
        {
            new AshfallDataGrid.Column { Header = "Location",   MinWidth = 220, Alignment = AshfallDataGrid.ColumnAlign.Left   },
            new AshfallDataGrid.Column { Header = "Sector",     MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Left   },
            new AshfallDataGrid.Column { Header = "Risk",       MinWidth = 150, Alignment = AshfallDataGrid.ColumnAlign.Left   },
            new AshfallDataGrid.Column { Header = "Baseline",   MinWidth = 120, Alignment = AshfallDataGrid.ColumnAlign.Right  },
            new AshfallDataGrid.Column { Header = "Readings",   MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right  },
        };
        _grid = new AshfallDataGrid(columns, showHeader: true, minWidth: 640, minHeight: 380);
        _grid.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        _grid.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        _grid.OnRowSelected += idx =>
        {
            _selectedIndex = idx;
            RefreshDetail();
        };
        gridCol.AddChild(_grid);

        var legend = AshfallUiHelpers.MakeSmall(
            "Baseline is the place's own exposure rate in µSv/h. Weather, gear and time on site change the real dose — the ledger keeps the provenance.");
        legend.AutowrapMode = TextServer.AutowrapMode.WordSmart;
        legend.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
        gridCol.AddChild(legend);

        contentStack.AddChild(gridCol);

        var detailPanel = AshfallUiHelpers.MakePanel();
        detailPanel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        detailPanel.SizeFlagsStretchRatio = 0.95f;
        var detailMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
        detailPanel.AddChild(detailMargin);

        var detailVBox = new VBoxContainer();
        detailVBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        detailMargin.AddChild(detailVBox);

        _detailTitle = new Label { Text = "EXPOSURE GEOGRAPHY" };
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

    public void Open()
    {
        Visible = true;
        RefreshView();
        QueueRedraw();
    }

    public void Close() => Visible = false;

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!Visible) return;
        if (AshfallInputActions.IsCloseOrCancel(@event))
        {
            OnClose?.Invoke();
            GetViewport().SetInputAsHandled();
        }
    }

    public override void _ExitTree()
    {
        Unbind();
        base._ExitTree();
    }
}
