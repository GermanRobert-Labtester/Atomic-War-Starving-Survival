using System;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Sidebar Nav Rail.
/// Vertical left rail for dashboard surfaces. Each item is a small button-like
/// row containing a label and an ID; selection is local-state only (the rail
/// raises an event with the selected ID, the host wires the panel switcher).
/// Default width 180px; content area gets the rest.
///
/// Presentation only. Routing belongs to the host.
///
/// Visual language matches the existing modal idiom (warm amber accent,
/// dot-bracket markers, frame_9slice fallback) — Stitch's sidebar is treated
/// as inspiration, not contract.
/// </summary>
public partial class AshfallSidebar : PanelContainer
{
    public sealed class Item
    {
        public string Id;           // routing key (snake_case)
        public string Label;        // display row, upper-cased by widget
        public string? Hint;        // optional muted sub-label
        public string IconPath;     // res:// path; falls back to a square chip
        public string? Tooltip;     // optional player-facing guidance tooltip
    }

    public event Action<string>? OnSelected;

    public string SelectedId { get; private set; } = string.Empty;
    public int ItemCount => _list?.GetChildCount() ?? 0;

    private readonly VBoxContainer _list;
    private readonly Label _railHeader;
    private readonly FontFile? _monoFont;

    public AshfallSidebar(Item[] items, string headerLabel, string initialSelectedId)
    {
        CustomMinimumSize = new Vector2(180, 0);
        SizeFlagsVertical = SizeFlags.ExpandFill;

        AddThemeStyleboxOverride("panel", AshfallUiHelpers.MakePanelFrameStyleBox());

        var margin = new MarginContainer();
        margin.AddThemeConstantOverride("margin_left", DesignTheme.SpacingSm);
        margin.AddThemeConstantOverride("margin_top", DesignTheme.SpacingSm);
        margin.AddThemeConstantOverride("margin_right", DesignTheme.SpacingSm);
        margin.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingSm);
        AddChild(margin);

        var vbox = new VBoxContainer();
        vbox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        margin.AddChild(vbox);

        _monoFont = AshfallUiHelpers.LoadFont("res://assets/fonts/ShareTechMono-Regular.ttf");

        _railHeader = new Label
        {
            Text = string.IsNullOrWhiteSpace(headerLabel) ? "NAV" : headerLabel.ToUpperInvariant(),
            HorizontalAlignment = HorizontalAlignment.Left,
        };
        _railHeader.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeLabel);
        _railHeader.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
        if (_monoFont != null) _railHeader.AddThemeFontOverride("font", _monoFont);
        vbox.AddChild(_railHeader);

        vbox.AddChild(new HSeparator());

        _list = new VBoxContainer();
        _list.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
        _list.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        vbox.AddChild(_list);

        if (items != null)
        {
            foreach (var item in items)
                AddRow(item);
        }

        if (!string.IsNullOrEmpty(initialSelectedId))
            Select(initialSelectedId);
    }

    public void Select(string id)
    {
        if (string.IsNullOrEmpty(id)) return;
        if (SelectedId == id) return;

        if (!string.IsNullOrEmpty(SelectedId))
            SetRowHighlight(SelectedId, false);

        SelectedId = id;
        SetRowHighlight(id, true);
        OnSelected?.Invoke(id);
    }

    private void AddRow(Item item)
    {
        if (item == null || _list == null) return;

        var row = new PanelContainer();
        row.Name = $"row_{item.Id}";
        row.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        var sb = new StyleBoxFlat
        {
            BgColor = new Color(DesignTheme.Ink.r, DesignTheme.Ink.g, DesignTheme.Ink.b, 0.40f),
        };
        sb.SetBorderWidthAll(0);
        row.AddThemeStyleboxOverride("panel", sb);

        string tooltipText = !string.IsNullOrEmpty(item.Tooltip) ? item.Tooltip : (!string.IsNullOrEmpty(item.Hint) ? item.Hint : string.Empty);
        if (!string.IsNullOrEmpty(tooltipText))
        {
            row.TooltipText = tooltipText;
        }

        var rowMargin = new MarginContainer();
        rowMargin.AddThemeConstantOverride("margin_left", DesignTheme.SpacingSm);
        rowMargin.AddThemeConstantOverride("margin_top", DesignTheme.SpacingXs);
        rowMargin.AddThemeConstantOverride("margin_right", DesignTheme.SpacingSm);
        rowMargin.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingXs);
        row.AddChild(rowMargin);

        var rowVbox = new VBoxContainer();
        rowVbox.AddThemeConstantOverride("separation", 0);
        rowMargin.AddChild(rowVbox);

        var lbl = new Label
        {
            Text = string.IsNullOrEmpty(item.Label) ? item.Id.ToUpperInvariant() : item.Label.ToUpperInvariant()
        };
        lbl.Name = "label";
        if (!string.IsNullOrEmpty(tooltipText))
        {
            lbl.TooltipText = tooltipText;
        }
        lbl.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeSmall);
        lbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
        var barlow = AshfallUiHelpers.LoadFont("res://assets/fonts/BarlowCondensed-SemiBold.ttf");
        if (barlow != null) lbl.AddThemeFontOverride("font", barlow);
        rowVbox.AddChild(lbl);

        if (!string.IsNullOrEmpty(item.Hint))
        {
            var hint = new Label { Text = item.Hint };
            hint.Name = "hint";
            if (!string.IsNullOrEmpty(tooltipText))
            {
                hint.TooltipText = tooltipText;
            }
            hint.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeLabel);
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
            if (_monoFont != null) hint.AddThemeFontOverride("font", _monoFont);
            rowVbox.AddChild(hint);
        }

        row.GuiInput += evt =>
        {
            if (evt is InputEventMouseButton mb && mb.Pressed && mb.ButtonIndex == MouseButton.Left)
            {
                Select(item.Id);
                AcceptEvent();
            }
        };

        _list.AddChild(row);
    }

    private void SetRowHighlight(string id, bool active)
    {
        if (_list == null) return;
        foreach (var child in _list.GetChildren())
        {
            if (child is PanelContainer row && row.Name == $"row_{id}")
            {
                var sb = row.GetThemeStylebox("panel") as StyleBoxFlat;
                if (sb == null) continue;
                if (active)
                {
                    sb.BgColor = new Color(DesignTheme.Warm.r, DesignTheme.Warm.g, DesignTheme.Warm.b, 0.20f);
                    sb.BorderColor = AshfallUiHelpers.ToColor(DesignTheme.Warm);
                    sb.SetBorderWidthAll(1);
                }
                else
                {
                    sb.BgColor = new Color(DesignTheme.Ink.r, DesignTheme.Ink.g, DesignTheme.Ink.b, 0.40f);
                    sb.BorderColor = new Color(0, 0, 0, 0);
                    sb.SetBorderWidthAll(0);
                }

                var labelNode = row.FindChild("label", recursive: false, owned: false);
                if (labelNode is Label lbl)
                {
                    lbl.AddThemeColorOverride("font_color",
                        AshfallUiHelpers.ToColor(active ? DesignTheme.Hot : DesignTheme.Pale));
                }
            }
        }
    }
}
