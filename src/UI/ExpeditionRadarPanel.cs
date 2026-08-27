using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Expedition Radar Dashboard (#10 Stitch / Tier-A5).
///
/// Phase 17 sub-card sibling of the existing ExpeditionPanel (Sterling, the
/// Sortie Planner). Reads from the project's own ExpeditionHostSession,
/// surfaces active sorties + known wasteland destinations in a HYBRID shell.
///
/// Two-tab visualization:
///   • Active sorties: phase, distance bars, current loot, push-luck toggle.
///   • Targets grid: location, distance, danger level, loot categories,
///                  blocked status.
///
/// Reads only — never mutates ExpeditionSystem state. The host session's
/// own StartDemoExpedition / PushLuckDemo / RetreatDemo APIs are surfaced
/// to the player through buttons that route back to the same host session.
/// </summary>
public partial class ExpeditionRadarPanel : Control, IBindablePanel
{
    public event Action? OnClose;
    public event Action<string>? OnDispatchRequested;
    public event Action<string>? OnSurvivorSelected;

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _activeGrid;
    private AshfallDataGrid? _targetGrid;
    private VBoxContainer _detailBox = null!;
    private Label _detailTitle = null!;
    private int _selectedActiveIndex = -1;
    private int _selectedTargetIndex = -1;
    private string _rangeFilter = "all"; // all | near | medium | far
    private ExpeditionHostSession? _host;
    private SurvivorsHostSession? _survivors;

    public bool IsBound => _host != null;

    public void Bind(ExpeditionHostSession expeditions, SurvivorsHostSession? survivors = null)
    {
        _host = expeditions;
        _survivors = survivors;
        if (_host != null)
            _host.StateChanged += RefreshView;
        RefreshView();
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("Sortie Radar // Wasteland Movement Network", minWidth: 1100, minHeight: 720);
        SetContentRoot(_shell);

        var categories = new[]
        {
            new AshfallSidebar.Item { Id = "all", Label = "All Targets", Hint = "every wasteland destination", IconPath = "" },
            new AshfallSidebar.Item { Id = "near", Label = "Near Range", Hint = "<= 6 legs / short sortie", IconPath = "" },
            new AshfallSidebar.Item { Id = "medium", Label = "Mid Range", Hint = "7 .. 12 legs / day trip", IconPath = "" },
            new AshfallSidebar.Item { Id = "far", Label = "Far Range", Hint = "> 12 legs / multi-day", IconPath = "" },
        };
        _sidebar = _shell.SetSidebar(categories, "Range Filter", "all");
        _sidebar.OnSelected += HandleSidebar;

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("active", "Active", "—", AshfallMetricCard.Criticality.Normal, minWidth: 80);
        _statusRail.AddCard("queued", "Queued", "—", AshfallMetricCard.Criticality.Normal, minWidth: 80);
        _statusRail.AddCard("blocked", "Blocked", "—", AshfallMetricCard.Criticality.Caution, minWidth: 90);
        _statusRail.AddCard("median", "Median", "— legs", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("danger", "Max Danger", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("enc", "Encounter %", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);

        // ── Active sorties DataGrid ──
        var activeCols = new[]
        {
            new AshfallDataGrid.Column { Header = "Survivor", MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Phase",    MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Distance", MinWidth = 140, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Stamina",  MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Loot",     MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Encounters", MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
        };
        _activeGrid = new AshfallDataGrid(activeCols, showHeader: true, minWidth: 720, minHeight: 220);
        _activeGrid.OnRowSelected += HandleActiveRowSelected;

        // ── Targets DataGrid ──
        var targetCols = new[]
        {
            new AshfallDataGrid.Column { Header = "Location", MinWidth = 200, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Legs",     MinWidth = 70,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Danger",   MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Enc %",    MinWidth = 70,  Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Loot",     MinWidth = 280, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Status",   MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Left },
        };
        _targetGrid = new AshfallDataGrid(targetCols, showHeader: true, minWidth: 720, minHeight: 220);
        _targetGrid.OnRowSelected += HandleTargetRowSelected;

        // Stack the two grids vertically with a tab label between them.
        var body = new VBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;

        var activeLabel = AshfallUiHelpers.MakeSectionHeader("Active Sorties In The Field");
        body.AddChild(activeLabel);
        body.AddChild(_activeGrid);

        body.AddChild(AshfallUiHelpers.MakeSeparator());

        var targetLabel = AshfallUiHelpers.MakeSectionHeader("Known Wasteland Destinations");
        body.AddChild(targetLabel);

        var targetRow = new HBoxContainer();
        targetRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        targetRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        targetRow.SizeFlagsVertical = SizeFlags.ExpandFill;
        targetRow.AddChild(_targetGrid);

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.CustomMinimumSize = new Vector2(280, 220);
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        targetRow.AddChild(_detailBox);

        _detailTitle = AshfallUiHelpers.MakeSectionHeader("TARGET DETAIL");
        _detailTitle.HorizontalAlignment = HorizontalAlignment.Left;
        _detailBox.AddChild(_detailTitle);
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
            "Select a sortie or destination row to view phase / cargo / blocking state."));

        body.AddChild(targetRow);
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
        _rangeFilter = id ?? "all";
        _selectedTargetIndex = -1;
        RefreshView();
    }

    private void HandleActiveRowSelected(int idx)
    {
        _selectedActiveIndex = idx;
        OnSurvivorSelected?.Invoke(SurvivorIdForActiveRow(idx));
        RefreshDetail();
    }

    private void HandleTargetRowSelected(int idx)
    {
        _selectedTargetIndex = idx;
        OnDispatchRequested?.Invoke(TargetIdForVisibleRow(idx));
        RefreshDetail();
    }

    public void RefreshView()
    {
        RefreshStatusRail();
        BuildActiveRows();
        BuildTargetRows();
        RefreshDetail();
    }

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        if (_host == null)
        {
            _statusRail.Set("active", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("queued", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("blocked", "—", AshfallMetricCard.Criticality.Caution);
            _statusRail.Set("median", "— legs", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("danger", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("enc", "—", AshfallMetricCard.Criticality.Normal);
            return;
        }

        int active = _host.Engine.ActiveCount;
        var defs = _host.Definitions;
        int blocked = 0;
        int maxDanger = 0;
        int encTotal = 0;
        int encCount = 0;
        int legTotal = 0;
        for (int i = 0; i < defs.Count; i++)
        {
            var d = defs[i];
            if (d == null) continue;
            legTotal += d.distanceTicks;
            if (d.dangerLevel > maxDanger) maxDanger = d.dangerLevel;
            encTotal += (int)(d.encounterChancePerTick * 100f);
            encCount++;
            if (_host.IsLocationBlocked(d.id)) blocked++;
        }

        int medianLegs = defs.Count > 0 ? legTotal / defs.Count : 0;
        int avgEnc = encCount > 0 ? encTotal / encCount : 0;

        _statusRail.Set("active", $"{active}", active > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Caution);
        _statusRail.Set("queued", $"{defs.Count}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("blocked", $"{blocked}",
            blocked > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("median", $"{medianLegs} legs", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("danger", $"LVL {maxDanger}",
            maxDanger >= 4 ? AshfallMetricCard.Criticality.Critical :
            maxDanger >= 2 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("enc", $"{avgEnc}%",
            avgEnc > 25 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
    }

    private void BuildActiveRows()
    {
        if (_activeGrid == null) return;

        if (_host == null)
        {
            _activeGrid.SetRows(BuildActiveFixtureRows());
            return;
        }

        var rows = new List<AshfallDataGrid.Row>();
        var active = _host.Engine.Active;
        foreach (var kv in active)
        {
            var exp = kv.Value;
            if (exp == null) continue;

            var phase = (ExpeditionPhase)exp.phase;
            var phaseText = phase.ToString().ToUpperInvariant();

            int totalLegs = Math.Max(1, exp.distanceTicks);
            int traveled = Math.Clamp(exp.travelTicksCompleted, 0, totalLegs);
            int remaining = Math.Max(0, totalLegs - traveled);
            string distanceCell = $"{traveled}/{totalLegs} (Δ{remaining})";

            var staminaState = exp.stamina < 30f ? AshfallDataGrid.CellState.Critical
                              : exp.stamina < 60f ? AshfallDataGrid.CellState.Caution
                                                 : AshfallDataGrid.CellState.Normal;
            var phaseState = phase == ExpeditionPhase.Outbound ? AshfallDataGrid.CellState.Normal
                           : phase == ExpeditionPhase.Looting ? AshfallDataGrid.CellState.Positive
                           : phase == ExpeditionPhase.Inbound ? AshfallDataGrid.CellState.Caution
                           : AshfallDataGrid.CellState.Normal;

            var cells = new List<AshfallDataGrid.Cell>
            {
                new(FormatSurvivor(exp.survivorId), AshfallDataGrid.CellState.Normal),
                new(phaseText, phaseState),
                new($"{traveled}/{totalLegs} \u0394{remaining}", AshfallDataGrid.CellState.Normal),
                new($"{exp.stamina:0}%", staminaState),
                new($"{(exp.loot != null ? exp.loot.Count : 0)} \u00b7 {exp.currentWeightKg:0}/{exp.maxLootCapacityKg:0} kg", AshfallDataGrid.CellState.Normal),
                new($"{exp.encounterCount}{(exp.isPushingLuck ? " [PUSH]" : "")}", AshfallDataGrid.CellState.Normal),
            };
            rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
        }
        if (rows.Count == 0)
        {
            _activeGrid.SetRows(BuildActiveEmptyRow());
            return;
        }
        _activeGrid.SetRows(rows);
    }

    private void BuildTargetRows()
    {
        if (_targetGrid == null) return;

        if (_host == null)
        {
            _targetGrid.SetRows(BuildTargetFixtureRows());
            return;
        }

        var rows = new List<AshfallDataGrid.Row>();
        var defs = _host.Definitions;
        for (int i = 0; i < defs.Count; i++)
        {
            var d = defs[i];
            if (d == null) continue;
            if (!RangePass(d.distanceTicks)) continue;

            int encPct = (int)(d.encounterChancePerTick * 100f);
            bool blocked = _host.IsLocationBlocked(d.id);
            string status = blocked ? "BLOCKED" : (IsEscortedAvailable() ? "READY" : "NEEDS SURVIVOR");
            var statusState = blocked ? AshfallDataGrid.CellState.Critical
                           : status == "READY" ? AshfallDataGrid.CellState.Positive
                           : AshfallDataGrid.CellState.Caution;
            var dangerState = d.dangerLevel >= 4 ? AshfallDataGrid.CellState.Critical
                            : d.dangerLevel >= 2 ? AshfallDataGrid.CellState.Caution
                                                  : AshfallDataGrid.CellState.Normal;
            string lootList = d.lootCategories != null && d.lootCategories.Count > 0
                ? string.Join(", ", d.lootCategories)
                : "—";

            var cells = new List<AshfallDataGrid.Cell>
            {
                new(d.displayName ?? d.id, AshfallDataGrid.CellState.Normal),
                new($"{d.distanceTicks}", AshfallDataGrid.CellState.Normal),
                new($"LVL {d.dangerLevel}", dangerState),
                new($"{encPct}%", AshfallDataGrid.CellState.Normal),
                new(lootList, AshfallDataGrid.CellState.Muted),
                new(status, statusState),
            };
            rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
        }
        if (rows.Count == 0)
        {
            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new("— no targets in range —", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                }
            });
        }
        _targetGrid.SetRows(rows);
    }

    private bool RangePass(int legs) => _rangeFilter switch
    {
        "near" => legs <= 6,
        "medium" => legs > 6 && legs <= 12,
        "far" => legs > 12,
        _ => true,
    };

    private bool IsEscortedAvailable()
    {
        if (_survivors == null) return true;
        foreach (var s in _survivors.RosterState)
        {
            if (s != null && s.IsAliveState)
                return true;
        }
        return false;
    }

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        AshfallUiHelpers.EmptyChildren(_detailBox);
        if (_host == null)
        {
            _detailTitle.Text = "TARGET DETAIL";
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Expedition engine offline. Bind an ExpeditionHostSession to see alive sorties + destinations."));
            return;
        }

        // Active selection takes precedence.
        if (_selectedActiveIndex >= 0)
        {
            ExpeditionState? exp = ActiveExpAt(_selectedActiveIndex);
            if (exp != null) RenderActiveDetail(exp);
            return;
        }
        if (_selectedTargetIndex >= 0)
        {
            var def = TargetDefAt(_selectedTargetIndex);
            if (def != null) RenderTargetDetail(def);
            return;
        }

        _detailTitle.Text = "TARGET DETAIL";
        _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
            "Select a sortie row to view phase and stamina, or a destination row to view loot categories and blocking status."));
    }

    private ExpeditionState? ActiveExpAt(int visibleIndex)
    {
        if (_host == null) return null;
        int seen = -1;
        var active = _host.Engine.Active;
        foreach (var kv in active)
        {
            seen++;
            if (seen == visibleIndex) return kv.Value;
        }
        return null;
    }

    private ExpeditionDefinition? TargetDefAt(int visibleIndex)
    {
        if (_host == null) return null;
        int seen = -1;
        var defs = _host.Definitions;
        for (int i = 0; i < defs.Count; i++)
        {
            var d = defs[i];
            if (d == null) continue;
            if (!RangePass(d.distanceTicks)) continue;
            seen++;
            if (seen == visibleIndex) return d;
        }
        return null;
    }

    private string SurvivorIdForActiveRow(int visibleIndex)
    {
        var exp = ActiveExpAt(visibleIndex);
        return exp?.survivorId ?? string.Empty;
    }

    private string TargetIdForVisibleRow(int visibleIndex)
    {
        var def = TargetDefAt(visibleIndex);
        return def?.id ?? string.Empty;
    }

    private void RenderActiveDetail(ExpeditionState exp)
    {
        _detailTitle.Text = $"{(string.IsNullOrEmpty(exp.displayName) ? exp.locationId : exp.displayName).ToUpperInvariant()} SORTIE";

        var phase = (ExpeditionPhase)exp.phase;
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Phase", phase.ToString().ToUpperInvariant(),
            AshfallUiHelpers.ToColor(DesignTheme.Warm)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Scout", FormatSurvivor(exp.survivorId),
            AshfallUiHelpers.ToColor(DesignTheme.Pale)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Stamina", $"{exp.stamina:0}%",
            exp.stamina < 30f ? AshfallUiHelpers.ToColor(DesignTheme.Critical) :
            exp.stamina < 60f ? AshfallUiHelpers.ToColor(DesignTheme.Entropy) :
                                  AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Travel", $"{exp.travelTicksCompleted}/{exp.distanceTicks} legs",
            AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Cargo", $"{(exp.loot != null ? exp.loot.Count : 0)} items \u00b7 {exp.currentWeightKg:0}/{exp.maxLootCapacityKg:0} kg",
            AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Encounters", $"{exp.encounterCount}{(exp.isPushingLuck ? " [PUSHING LUCK]" : string.Empty)}",
            exp.encounterCount > 0 ? AshfallUiHelpers.ToColor(DesignTheme.Entropy) : AshfallUiHelpers.ToColor(DesignTheme.Dim)));
    }

    private void RenderTargetDetail(ExpeditionDefinition def)
    {
        _detailTitle.Text = $"{(string.IsNullOrEmpty(def.displayName) ? def.id : def.displayName).ToUpperInvariant()} DETAIL";
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Legs", $"{def.distanceTicks}",
            AshfallUiHelpers.ToColor(DesignTheme.Pale)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Danger", $"LVL {def.dangerLevel}",
            def.dangerLevel >= 4 ? AshfallUiHelpers.ToColor(DesignTheme.Critical) :
            def.dangerLevel >= 2 ? AshfallUiHelpers.ToColor(DesignTheme.Entropy) :
                                    AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Encounter/hr", $"{(def.encounterChancePerTick * 100f):0}%",
            AshfallUiHelpers.ToColor(DesignTheme.Dim)));

        if (def.lootCategories != null && def.lootCategories.Count > 0)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSubsectionHeader("Loot Categories"));
            for (int i = 0; i < def.lootCategories.Count; i++)
            {
                _detailBox.AddChild(AshfallUiHelpers.MakeDataRow(def.lootCategories[i], "—", AshfallUiHelpers.ToColor(DesignTheme.Muted)));
            }
        }

        bool blocked = _host != null && _host.IsLocationBlocked(def.id);
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeSubsectionHeader("Status"));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Route",
            blocked ? "BLOCKED" : (IsEscortedAvailable() ? "READY" : "NEEDS SURVIVOR"),
            blocked ? AshfallUiHelpers.ToColor(DesignTheme.Critical) : AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
    }

    private static string FormatSurvivor(string id)
    {
        if (string.IsNullOrEmpty(id)) return "[UNNAMED]";
        return id switch
        {
            "survivor_dr_sarah_chen" or "survivor_sarah_chen" => "Dr. Sarah Chen",
            "survivor_gunner_mikhail" or "survivor_mikhail_volkov" => "Gunner Mikhail",
            "elena_vasquez" or "survivor_elena_vasquez" => "Elena Vasquez",
            _ => id.Replace("survivor_", "").Replace("_", " ").ToUpperInvariant()
        };
    }

    /// <summary>Fixture for the active grid when no host session is bound.</summary>
    internal static List<AshfallDataGrid.Row> BuildActiveFixtureRows()
    {
        return new List<AshfallDataGrid.Row>
        {
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("GUNNER MIKHAIL", AshfallDataGrid.CellState.Normal),
                    new("LOOTING", AshfallDataGrid.CellState.Positive),
                    new("4/5 (Δ1)", AshfallDataGrid.CellState.Normal),
                    new("72%", AshfallDataGrid.CellState.Normal),
                    new("2 · 4/13 kg", AshfallDataGrid.CellState.Normal),
                    new("1", AshfallDataGrid.CellState.Normal),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("ELENA VASQUEZ", AshfallDataGrid.CellState.Normal),
                    new("RETURN", AshfallDataGrid.CellState.Caution),
                    new("7/8 (Δ1)", AshfallDataGrid.CellState.Normal),
                    new("24%", AshfallDataGrid.CellState.Critical),
                    new("6 · 11/15 kg", AshfallDataGrid.CellState.Normal),
                    new("3", AshfallDataGrid.CellState.Normal),
                }, Selectable = true },
        };
    }

    internal static List<AshfallDataGrid.Row> BuildActiveEmptyRow()
    {
        return new List<AshfallDataGrid.Row>
        {
            new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new("— no sorties active —", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                }
            }
        };
    }

    /// <summary>Fixture for the target grid when no host session is bound.</summary>
    internal static List<AshfallDataGrid.Row> BuildTargetFixtureRows()
    {
        return new List<AshfallDataGrid.Row>
        {
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("The Works Allotment Commune", AshfallDataGrid.CellState.Normal),
                    new("5", AshfallDataGrid.CellState.Normal),
                    new("LVL 2", AshfallDataGrid.CellState.Caution),
                    new("12%", AshfallDataGrid.CellState.Normal),
                    new("scrap_metal, clean_water, bandages, food_rations", AshfallDataGrid.CellState.Muted),
                    new("READY", AshfallDataGrid.CellState.Positive),
                }, Selectable = true },
            new AshfallDataGrid.Row { Cells = new List<AshfallDataGrid.Cell>
                {
                    new("Denial Cut Substation", AshfallDataGrid.CellState.Normal),
                    new("8", AshfallDataGrid.CellState.Normal),
                    new("LVL 4", AshfallDataGrid.CellState.Critical),
                    new("18%", AshfallDataGrid.CellState.Normal),
                    new("dosimeter, copper_wire, fuel, item_hydro_baron_queue_chit", AshfallDataGrid.CellState.Muted),
                    new("READY", AshfallDataGrid.CellState.Positive),
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
