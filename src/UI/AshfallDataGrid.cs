using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Data Grid.
/// Tabular / matrix presentation primitive used by dashboards and ledgers
/// that need structured columns + rows of mixed text / numeric / icon / status
/// cells. Pure presentation: the widget consumes pre-formatted strings and
/// semantic state, never reaches into the simulation.
///
/// Intended reuse (Phase 12):
///   • Survival Workstation recipe/ingredient matrix (#19)
///   • Trade offer / ask ledger (#35)
///   • Future Tier-2: Skill Matrix, Faction Matrix, Dose Ledger
///
/// Cell semantics map to existing design tokens via a small stable enum
/// (Normal / Positive / Warning / Critical / Muted / Selected). Callers do
/// not pass colours directly.
///
/// This is not a generic spreadsheet framework.
public partial class AshfallDataGrid : PanelContainer
{
    public enum CellState
    {
        Normal,
        Positive,
        Caution,
        Warning,
        Critical,
        Muted,
        Selected,
    }

    public enum ColumnAlign
    {
        Left,
        Center,
        Right,
    }

    public sealed class Column
    {
        public string Header;
        public int MinWidth = 60;
        public ColumnAlign Alignment = ColumnAlign.Left;
    }

    public sealed class Cell
    {
        public string Text = string.Empty;
        public CellState State = CellState.Normal;
        public Texture2D? IconTexture = null;

        public Cell() { }
        public Cell(string text, CellState state = CellState.Normal)
        {
            Text = text ?? string.Empty;
            State = state;
        }
    }

    public sealed class Row
    {
        public List<Cell> Cells = new();
        public bool Selectable = true;
        public Action? OnSelected = null;
    }

    public event Action<int>? OnRowSelected;

    public int RowCount => _rows.Count;
    public int SelectedIndex { get; private set; } = -1;
    public List<Row> Rows => _rows;

    private readonly List<Column> _columns = new();
    private readonly List<Row> _rows = new();
    private readonly VBoxContainer _body;
    private readonly PanelContainer _headerBar;
    private readonly Label _emptyLabel;
    // Godot.Node.SetMeta only accepts Variant payloads, so we cannot round-
    // trip arbitrary Row data through Meta. Track row panels -> rows with a
    // plain Dictionary.
    private readonly Dictionary<PanelContainer, Row> _rowLookup = new();
    private bool _showHeader = true;

    public AshfallDataGrid(IEnumerable<Column> columns, bool showHeader = true,
        int minWidth = 480, int minHeight = 180)
    {
        CustomMinimumSize = new Vector2(minWidth, minHeight);
        AddThemeStyleboxOverride("panel", AshfallUiHelpers.MakePanelFrameStyleBox());

        var vbox = new VBoxContainer();
        vbox.AddThemeConstantOverride("separation", 0);
        vbox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        vbox.SizeFlagsVertical = SizeFlags.ExpandFill;
        AddChild(vbox);

        _headerBar = new PanelContainer();
        _headerBar.AddThemeStyleboxOverride("panel", AshfallUiHelpers.MakeHeaderFrameStyleBox());
        vbox.AddChild(_headerBar);

        var scroll = new ScrollContainer
        {
            SizeFlagsVertical = SizeFlags.ExpandFill,
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
            VerticalScrollMode = ScrollContainer.ScrollMode.Auto,
        };
        vbox.AddChild(scroll);

        _body = new VBoxContainer();
        _body.AddThemeConstantOverride("separation", 0);
        _body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        scroll.AddChild(_body);

        _emptyLabel = new Label
        {
            Text = "— no entries —",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            CustomMinimumSize = new Vector2(0, 64),
        };
        _emptyLabel.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeSmall);
        _emptyLabel.AddThemeColorOverride("font_color",
            AshfallUiHelpers.ToColor(DesignTheme.Muted));

        if (columns != null)
        {
            foreach (var c in columns)
                _columns.Add(c);
        }
        _showHeader = showHeader;
        Rebuild();
    }

    /// <summary>Replace all rows. Pass null or empty for the empty placeholder.</summary>
    public void SetRows(IReadOnlyList<Row>? rows)
    {
        _rows.Clear();
        if (rows != null)
        {
            foreach (var r in rows)
                _rows.Add(r);
        }
        Rebuild();
    }

    public void SetSelected(int index)
    {
        if (index < -1 || index >= _rows.Count) return;
        SelectedIndex = index;
        RefreshRowHighlights();
        OnRowSelected?.Invoke(index);
    }

    public void Clear()
    {
        _rows.Clear();
        SelectedIndex = -1;
        Rebuild();
    }

    private void Rebuild()
    {
        _rowLookup.Clear();

        // Header row
        while (_headerBar.GetChildCount() > 0)
        {
            var child = _headerBar.GetChild(0);
            _headerBar.RemoveChild(child);
            // Header controls are generated on every rebuild. They are removed
            // before disposal, so QueueFree() can leave them orphaned during a
            // rapid rebind or headless shutdown; dispose them synchronously.
            child.Free();
        }
        if (_showHeader && _columns.Count > 0)
        {
            _headerBar.Visible = true;
            var headerMargin = new MarginContainer();
            headerMargin.AddThemeConstantOverride("margin_left", DesignTheme.SpacingSm);
            headerMargin.AddThemeConstantOverride("margin_top", DesignTheme.SpacingXs);
            headerMargin.AddThemeConstantOverride("margin_right", DesignTheme.SpacingSm);
            headerMargin.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingXs);
            _headerBar.AddChild(headerMargin);

            var headerRow = new HBoxContainer();
            headerRow.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            headerRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            headerMargin.AddChild(headerRow);

            foreach (var col in _columns)
                headerRow.AddChild(MakeHeaderLabel(col));
        }
        else
        {
            _headerBar.Visible = false;
        }

        // Body
        while (_body.GetChildCount() > 0)
        {
            var child = _body.GetChild(0);
            _body.RemoveChild(child);
            if (child != _emptyLabel)
                child.Free();
        }

        if (_rows.Count == 0)
        {
            _body.AddChild(_emptyLabel);
            return;
        }

        for (int i = 0; i < _rows.Count; i++)
            _body.AddChild(BuildRowContainer(_rows[i], i));

        RefreshRowHighlights();
    }

    private Control BuildRowContainer(Row row, int rowIndex)
    {
        var panel = new PanelContainer();
        panel.Name = $"row_{rowIndex}";
        panel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        ApplyRowStyle(panel, row);

        var margin = new MarginContainer();
        margin.AddThemeConstantOverride("margin_left", DesignTheme.SpacingSm);
        margin.AddThemeConstantOverride("margin_top", DesignTheme.SpacingXs);
        margin.AddThemeConstantOverride("margin_right", DesignTheme.SpacingSm);
        margin.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingXs);
        panel.AddChild(margin);

        var hbox = new HBoxContainer();
        hbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        hbox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        margin.AddChild(hbox);

        for (int c = 0; c < _columns.Count; c++)
        {
            var col = _columns[c];
            Cell cell = c < row.Cells.Count ? row.Cells[c] : new Cell();
            hbox.AddChild(MakeCellControl(cell, col));
        }

        _rowLookup[panel] = row;

        if (row.OnSelected != null || row.Selectable)
        {
            int captured = rowIndex;
            Action capturedHandler = row.OnSelected ?? (() => { });
            panel.GuiInput += evt =>
            {
                if (evt is InputEventMouseButton mb && mb.Pressed && mb.ButtonIndex == MouseButton.Left)
                {
                    SetSelected(captured);
                    capturedHandler();
                    AcceptEvent();
                }
            };
        }

        return panel;
    }

    private static void ApplyRowStyle(PanelContainer panel, Row row)
    {
        CellState dominant = CellState.Normal;
        bool anySelected = false;
        foreach (var cell in row.Cells)
        {
            if (cell.State == CellState.Selected) { anySelected = true; break; }
            if ((int)cell.State > (int)dominant) dominant = cell.State;
        }
        CellState s = anySelected ? CellState.Selected : dominant;
        var sb = new StyleBoxFlat
        {
            BgColor = s switch
            {
                CellState.Selected => new Color(DesignTheme.Warm.r, DesignTheme.Warm.g, DesignTheme.Warm.b, 0.14f),
                CellState.Critical => new Color(DesignTheme.Critical.r, DesignTheme.Critical.g, DesignTheme.Critical.b, 0.10f),
                CellState.Warning => new Color(DesignTheme.Entropy.r, DesignTheme.Entropy.g, DesignTheme.Entropy.b, 0.08f),
                CellState.Positive => new Color(DesignTheme.Lethe.r, DesignTheme.Lethe.g, DesignTheme.Lethe.b, 0.06f),
                CellState.Caution => new Color(DesignTheme.Lethe.r, DesignTheme.Lethe.g, DesignTheme.Lethe.b, 0.04f),
                _ => new Color(DesignTheme.Ink.r, DesignTheme.Ink.g, DesignTheme.Ink.b, 0.40f),
            },
            BorderColor = s == CellState.Selected
                ? AshfallUiHelpers.ToColor(DesignTheme.Warm)
                : new Color(0, 0, 0, 0),
        };
        sb.SetBorderWidthAll(s == CellState.Selected ? 1 : 0);
        panel.AddThemeStyleboxOverride("panel", sb);
    }

    private void RefreshRowHighlights()
    {
        if (_body == null) return;
        for (int i = 0; i < _body.GetChildCount(); i++)
        {
            if (_body.GetChild(i) is PanelContainer panel && _rowLookup.TryGetValue(panel, out var row))
            {
                if (i == SelectedIndex)
                {
                    bool anySelected = false;
                    foreach (var c in row.Cells)
                        if (c.State == CellState.Selected) { anySelected = true; break; }
                    if (!anySelected)
                    {
                        ApplyRowStyle(panel, MakeSelectionOverrideRow(row.Cells));
                        continue;
                    }
                }
                ApplyRowStyle(panel, row);
            }
        }
    }

    private static Row MakeSelectionOverrideRow(List<Cell> source)
    {
        var row = new Row
        {
            Selectable = false,
            OnSelected = null,
            Cells = new List<Cell>(source),
        };
        // Selection state overrides
        for (int i = 0; i < row.Cells.Count; i++) row.Cells[i] = new Cell(row.Cells[i].Text, CellState.Selected);
        return row;
    }

    private static Label MakeHeaderLabel(Column col)
    {
        var lbl = new Label
        {
            Text = (col.Header ?? string.Empty).ToUpperInvariant(),
            CustomMinimumSize = new Vector2(col.MinWidth, 0),
            HorizontalAlignment = col.Alignment switch
            {
                ColumnAlign.Right => HorizontalAlignment.Right,
                ColumnAlign.Center => HorizontalAlignment.Center,
                _ => HorizontalAlignment.Left,
            },
        };
        lbl.SizeFlagsHorizontal = Control.SizeFlags.Fill;
        lbl.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeLabel);
        lbl.AddThemeColorOverride("font_color",
            AshfallUiHelpers.ToColor(DesignTheme.Dim));
        var mono = AshfallUiHelpers.LoadFont("res://assets/fonts/ShareTechMono-Regular.ttf");
        if (mono != null) lbl.AddThemeFontOverride("font", mono);
        return lbl;
    }

    private static Control MakeCellControl(Cell cell, Column col)
    {
        var container = new HBoxContainer();
        container.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        container.CustomMinimumSize = new Vector2(col.MinWidth, 0);
        container.SizeFlagsHorizontal = Control.SizeFlags.Fill;

        if (cell.IconTexture != null)
        {
            var rect = new TextureRect
            {
                Texture = cell.IconTexture,
                CustomMinimumSize = new Vector2(18, 18),
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
            };
            container.AddChild(rect);
        }

        var lbl = new Label
        {
            Text = cell.Text ?? string.Empty,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = col.Alignment switch
            {
                ColumnAlign.Right => HorizontalAlignment.Right,
                ColumnAlign.Center => HorizontalAlignment.Center,
                _ => HorizontalAlignment.Left,
            },
        };
        lbl.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        lbl.AddThemeFontSizeOverride("font_size",
            col.Alignment == ColumnAlign.Right ? DesignTheme.FontSizeMono : DesignTheme.FontSizeSmall);
        lbl.AddThemeColorOverride("font_color",
            AshfallUiHelpers.ToColor(StateToken(cell.State)));

        var font = col.Alignment == ColumnAlign.Right
            ? (AshfallUiHelpers.LoadFont("res://assets/fonts/ShareTechMono-Regular.ttf")
                ?? AshfallUiHelpers.LoadFont("res://assets/fonts/BarlowCondensed-Regular.ttf"))
            : (AshfallUiHelpers.LoadFont("res://assets/fonts/BarlowCondensed-Regular.ttf")
                ?? AshfallUiHelpers.LoadFont("res://assets/fonts/ShareTechMono-Regular.ttf"));
        if (font != null) lbl.AddThemeFontOverride("font", font);

        container.AddChild(lbl);
        return container;
    }

    private static (float r, float g, float b, float a) StateToken(CellState state) => state switch
    {
        CellState.Positive => DesignTheme.Lethe,
        CellState.Caution => DesignTheme.Lethe,
        CellState.Warning => DesignTheme.Entropy,
        CellState.Critical => DesignTheme.Critical,
        CellState.Muted => DesignTheme.Dim,
        CellState.Selected => DesignTheme.Hot,
        _ => DesignTheme.Pale,
    };
}
