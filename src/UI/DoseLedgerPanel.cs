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
/// ASHFALL — Dose Ledger (#59 Stitch, Decontamination & Dose Ledger Terminal).
///
/// Per-survivor cumulative radiation readout. Pure presentation. Reads only
/// from <see cref="DoseLedgerSystem"/> via the host session.
///
/// The ledger deliberately ONLY shows survivors with a booked dosimeter.
/// Unbound survivors are not part of the ledger — that's per
/// `DoseLedgerSystem`'s design intent ("readings are only booked against
/// survivors with an assigned dosimeter tag; unbooked rads are the
/// shelter's silence").
/// </summary>
public partial class DoseLedgerPanel : Control
{
    public event Action? OnClose;
    public event Action<string>? OnSurvivorSelected;

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _doseGrid;
    private VBoxContainer _detailBox = null!;
    private Label _detailTitle = null!;
    private int _selectedIndex = -1;
    private string _activeFactionFilter = "all"; // all | amber | red | black | unbound

    private DoseLedgerHostSession? _doseSession;
    private SurvivorsHostSession? _survivorsHost;
    private List<DoseEntry> _visibleEntries = new();

    public bool IsBound => _doseSession != null;

    public void Bind(DoseLedgerHostSession session, SurvivorsHostSession? survivors = null)
    {
        _doseSession = session;
        _survivorsHost = survivors;
        if (_doseSession?.Ledger != null)
        {
            _doseSession.Ledger.OnStateChanged -= HandleLedgerChanged;
            _doseSession.Ledger.OnStateChanged += HandleLedgerChanged;
        }
        RefreshView();
    }

    private void HandleLedgerChanged(DoseLedgerSystemState _) => RefreshView();

    public void RefreshView()
    {
        RefreshStatusRail();
        BuildDoseRows();
        RefreshDetail();
    }

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        if (_doseSession == null)
        {
            _statusRail.Set("entries", "0", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("ceiling", "0 mSv", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("amber",   "0", AshfallMetricCard.Criticality.Caution);
            _statusRail.Set("red",     "0", AshfallMetricCard.Criticality.Warn);
            _statusRail.Set("black",   "0", AshfallMetricCard.Criticality.Critical);
            _statusRail.Set("cal",     "—", AshfallMetricCard.Criticality.Normal);
            return;
        }
        var ledger = _doseSession.Ledger;
        int amber = 0, red = 0, black = 0;
        foreach (var entry in ledger.Entries)
        {
            float cum = ledger.GetCumulative(entry.survivorId);
            if (cum >= DoseLedgerSystem.BlackMsv) black++;
            else if (cum >= DoseLedgerSystem.RedMsv) red++;
            else if (cum >= DoseLedgerSystem.AmberMsv) amber++;
        }
        _statusRail.Set("entries", $"{ledger.Entries.Count}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("ceiling", $"{ledger.State.ceilingMsv:0} mSv", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("amber",   amber > 0 ? $"{amber}" : "0", amber > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("red",     red   > 0 ? $"{red}"   : "0", red   > 0 ? AshfallMetricCard.Criticality.Warn   : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("black",   black > 0 ? $"{black}" : "0", black > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("cal",     ledger.State.calibrationOverdue ? "OVERDUE" : "OK",
            ledger.State.calibrationOverdue ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
    }

    private void BuildDoseRows()
    {
        if (_doseGrid == null) return;
        if (_doseSession == null)
        {
            _doseGrid.SetRows(BuildFixtureRows());
            return;
        }

        var rows = new List<AshfallDataGrid.Row>();
        _visibleEntries.Clear();
        var ledger = _doseSession.Ledger;
        foreach (var entry in ledger.Entries)
        {
            if (entry == null) continue;
            float cum = ledger.GetCumulative(entry.survivorId);
            var band = MapBand(cum);
            if (!FilterPass(band)) continue;
            _visibleEntries.Add(entry);

            var lastReading = entry.readingsHistory != null && entry.readingsHistory.Count > 0
                ? entry.readingsHistory[entry.readingsHistory.Count - 1] : null;

            string lastTxt = lastReading != null
                ? $"D{lastReading.day} · {lastReading.bookedMsv:0.0} mSv"
                : "—";

            string tag = string.IsNullOrEmpty(entry.assignedDosimeterTag) ? "—" : entry.assignedDosimeterTag;
            string lastAntiRad = entry.lastAntiRadDay < 0 ? "—" : $"D{entry.lastAntiRadDay}";

            var cells = new List<AshfallDataGrid.Cell>
            {
                new(FormatSurvivor(entry.survivorId), AshfallDataGrid.CellState.Normal),
                new($"{cum:0.0} mSv", band),
                new(BandName(band), band),
                new(tag, string.IsNullOrEmpty(entry.assignedDosimeterTag) ? AshfallDataGrid.CellState.Muted : AshfallDataGrid.CellState.Normal),
                new(lastAntiRad, lastReading == null ? AshfallDataGrid.CellState.Muted : AshfallDataGrid.CellState.Normal),
            };
            rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
        }
        if (rows.Count == 0 && _activeFactionFilter != "all")
        {
            // Surface the empty state explicitly.
            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new("— no matches —", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                }
            });
        }
        _doseGrid.SetRows(rows);
    }

    private bool FilterPass(AshfallDataGrid.CellState band) => _activeFactionFilter switch
    {
        "amber"  => band == AshfallDataGrid.CellState.Caution,
        "red"    => band == AshfallDataGrid.CellState.Warning,
        "black"  => band == AshfallDataGrid.CellState.Critical,
        "ok"     => band == AshfallDataGrid.CellState.Normal || band == AshfallDataGrid.CellState.Positive,
        _ => true,
    };

    internal static AshfallDataGrid.CellState MapBand(float cumulativeMsv)
    {
        if (cumulativeMsv >= DoseLedgerSystem.BlackMsv) return AshfallDataGrid.CellState.Critical;
        if (cumulativeMsv >= DoseLedgerSystem.RedMsv)   return AshfallDataGrid.CellState.Warning;
        if (cumulativeMsv >= DoseLedgerSystem.AmberMsv) return AshfallDataGrid.CellState.Caution;
        if (cumulativeMsv > 0f)                          return AshfallDataGrid.CellState.Normal;
        return AshfallDataGrid.CellState.Positive;
    }

    internal static string BandName(AshfallDataGrid.CellState s) => s switch
    {
        AshfallDataGrid.CellState.Critical => "BLACK",
        AshfallDataGrid.CellState.Warning => "RED",
        AshfallDataGrid.CellState.Caution => "AMBER",
        AshfallDataGrid.CellState.Normal => "GREEN",
        AshfallDataGrid.CellState.Positive => "ZERO",
        _ => "—",
    };

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        AshfallUiHelpers.EmptyChildren(_detailBox);
        if (_selectedIndex < 0 || _selectedIndex >= _visibleEntries.Count || _doseSession == null)
        {
            _detailTitle.Text = "DOSIMETRY DETAIL";
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Select a survivor row to view cumulative dose, baseline, shielding, and recent readings."));
            return;
        }
        var entry = _visibleEntries[_selectedIndex];
        var ledger = _doseSession.Ledger;
        float cum = ledger.GetCumulative(entry.survivorId);
        var band = MapBand(cum);

        _detailTitle.Text = FormatSurvivor(entry.survivorId).ToUpperInvariant() + " · DOSIMETRY";

        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("CUMULATIVE",
            $"{cum:0.0} mSv",
            AshfallUiHelpers.ToColor(BandToken(band))));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("BAND", BandName(band), AshfallUiHelpers.ToColor(BandToken(band))));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("BASELINE", $"{entry.baselineMsv:0.0} mSv", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("SHIELDING", $"{entry.shieldingFactor:0.00}", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("TAG",
            string.IsNullOrEmpty(entry.assignedDosimeterTag) ? "— UNBOUND —" : entry.assignedDosimeterTag,
            AshfallUiHelpers.ToColor(string.IsNullOrEmpty(entry.assignedDosimeterTag) ? DesignTheme.Muted : DesignTheme.Warm)));

        if (entry.readingsHistory != null && entry.readingsHistory.Count > 0)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
            _detailBox.AddChild(AshfallUiHelpers.MakeSectionHeader("RECENT READINGS"));
            int take = Math.Min(5, entry.readingsHistory.Count);
            for (int i = entry.readingsHistory.Count - 1; i >= entry.readingsHistory.Count - take; i--)
            {
                var r = entry.readingsHistory[i];
                if (r == null) continue;
                string source = string.IsNullOrEmpty(r.source) ? "— exposure —" : r.source;
                string line = $"D{r.day:00} · {source} · {r.nominalMsv:0.0}/{r.bookedMsv:0.0} mSv"
                    + (r.fluxAmbiguous ? " · FLUX" : "")
                    + (r.antiRadAfter ? " · ANTI-RAD" : "");
                _detailBox.AddChild(AshfallUiHelpers.MakeSmall(line));
            }
        }

        OnSurvivorSelected?.Invoke(entry.survivorId);
    }

    private static (float r, float g, float b, float a) BandToken(AshfallDataGrid.CellState s) => s switch
    {
        AshfallDataGrid.CellState.Critical => DesignTheme.Critical,
        AshfallDataGrid.CellState.Warning => DesignTheme.Entropy,
        AshfallDataGrid.CellState.Caution => DesignTheme.Lethe,
        AshfallDataGrid.CellState.Normal => DesignTheme.Warm,
        AshfallDataGrid.CellState.Positive => DesignTheme.Lethe,
        _ => DesignTheme.Pale,
    };

    private static string FormatSurvivor(string id)
    {
        if (string.IsNullOrEmpty(id)) return "[UNNAMED]";
        if (id == "survivor_dr_sarah_chen" || id == "survivor_sarah_chen") return "Dr. Sarah Chen";
        if (id == "survivor_gunner_mikhail" || id == "survivor_mikhail_volkov") return "Gunner Mikhail";
        if (id == "elena_vasquez" || id == "survivor_elena_vasquez") return "Elena Vasquez";
        return id.Replace("survivor_", "").Replace("_", " ").ToUpperInvariant();
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);
        Visible = false;

        var bg = new ColorRect { Color = new Color(0.04f, 0.04f, 0.05f, 0.92f) };
        bg.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(bg);

        _shell = new AshfallDashboardShell(
            "DOSE LEDGER — DECONTAMINATION_TERMINAL",
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
            new AshfallSidebar.Item { Id = "filter_all",    Label = "Filter: All",     Hint = "every entry" },
            new AshfallSidebar.Item { Id = "filter_zero",   Label = "Filter: GREEN",   Hint = "< Amber" },
            new AshfallSidebar.Item { Id = "filter_amber",  Label = "Filter: AMBER",   Hint = "≥ 100 mSv" },
            new AshfallSidebar.Item { Id = "filter_red",    Label = "Filter: RED",     Hint = "≥ 300 mSv" },
            new AshfallSidebar.Item { Id = "filter_black",  Label = "Filter: BLACK",   Hint = "≥ 600 mSv" },
        }, "DOSE LEDGER OPS", "filter_all");

        if (_sidebar != null)
        {
            _sidebar.OnSelected += id =>
            {
                _activeFactionFilter = id switch
                {
                    "filter_zero" => "ok",
                    "filter_amber" => "amber",
                    "filter_red" => "red",
                    "filter_black" => "black",
                    _ => "all",
                };
                BuildDoseRows();
            };
        }

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("entries", "BOOKED",  "0",       AshfallMetricCard.Criticality.Normal, 100);
        _statusRail.AddCard("ceiling", "CEILING", "0 mSv",  AshfallMetricCard.Criticality.Normal, 110);
        _statusRail.AddCard("amber",   "AMBER",   "0",       AshfallMetricCard.Criticality.Caution, 100);
        _statusRail.AddCard("red",     "RED",     "0",       AshfallMetricCard.Criticality.Warn,   90);
        _statusRail.AddCard("black",   "BLACK",   "0",       AshfallMetricCard.Criticality.Critical, 100);
        _statusRail.AddCard("cal",     "CAL",     "OK",      AshfallMetricCard.Criticality.Normal,   90);

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
        gridCol.AddChild(AshfallUiHelpers.MakeSectionHeader("BOOKED DOSIMETERS"));

        var columns = new[]
        {
            new AshfallDataGrid.Column { Header = "Survivor",  MinWidth = 200, Alignment = AshfallDataGrid.ColumnAlign.Left   },
            new AshfallDataGrid.Column { Header = "Cumul.",    MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right  },
            new AshfallDataGrid.Column { Header = "Band",      MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Center },
            new AshfallDataGrid.Column { Header = "Tag",       MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left   },
            new AshfallDataGrid.Column { Header = "Anti-rad",  MinWidth = 90,  Alignment = AshfallDataGrid.ColumnAlign.Right  },
        };
        _doseGrid = new AshfallDataGrid(columns, showHeader: true, minWidth: 600, minHeight: 360);
        _doseGrid.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        _doseGrid.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        _doseGrid.OnRowSelected += idx =>
        {
            _selectedIndex = idx;
            RefreshDetail();
        };
        gridCol.AddChild(_doseGrid);

        // Band legend strip
        var legend = new HBoxContainer();
        legend.AddThemeConstantOverride("separation", DesignTheme.SpacingLg);
        LegendChip(legend, "AMBER", DesignTheme.Lethe,    $"{DoseLedgerSystem.AmberMsv:0} mSv");
        LegendChip(legend, "RED",   DesignTheme.Entropy,  $"{DoseLedgerSystem.RedMsv:0} mSv");
        LegendChip(legend, "BLACK", DesignTheme.Critical, $"{DoseLedgerSystem.BlackMsv:0} mSv");
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

        _detailTitle = new Label { Text = "DOSIMETRY DETAIL" };
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

    private static void LegendChip(HBoxContainer host, string label, (float r, float g, float b, float a) token, string value)
    {
        var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingXs);
        var dot = new ColorRect { Color = AshfallUiHelpers.ToColor(token), CustomMinimumSize = new Vector2(10, 10) };
        row.AddChild(dot);
        var lbl = AshfallUiHelpers.MakeSmall($"{label} ({value})");
        lbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
        row.AddChild(lbl);
        host.AddChild(row);
    }

    private static List<AshfallDataGrid.Row> BuildFixtureRows()
    {
        var rows = new List<AshfallDataGrid.Row>
        {
            new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new("Gunner Mikhail", AshfallDataGrid.CellState.Normal),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("UNBOUND", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                }
            },
            new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new("Elena Vasquez", AshfallDataGrid.CellState.Normal),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("UNBOUND", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                }
            }
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

    public override void _ExitTree()
    {
        if (_doseSession?.Ledger != null)
        {
            _doseSession.Ledger.OnStateChanged -= HandleLedgerChanged;
        }
        base._ExitTree();
    }
}
