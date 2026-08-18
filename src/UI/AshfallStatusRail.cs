using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Status Rail.
/// A horizontal strip of AshfallMetricCard slots rendered above (or below)
/// the content area of a dashboard-style surface. Owns no event semantics —
/// callers update each card's value/criticality through Set* APIs.
///
/// Presentation only.
/// </summary>
public partial class AshfallStatusRail : PanelContainer
{
    private readonly HBoxContainer _strip;
    private readonly Dictionary<string, AshfallMetricCard> _cards = new(StringComparer.Ordinal);

    public int CardCount => _cards.Count;

    public AshfallStatusRail()
    {
        CustomMinimumSize = new Vector2(0, 56);
        SizeFlagsHorizontal = SizeFlags.ExpandFill;

        var sb = new StyleBoxFlat
        {
            BgColor = new Color(DesignTheme.Ink.r, DesignTheme.Ink.g, DesignTheme.Ink.b, 0.55f),
        };
        sb.SetBorderWidthAll(0);
        AddThemeStyleboxOverride("panel", sb);

        var margin = new MarginContainer();
        margin.AddThemeConstantOverride("margin_left", DesignTheme.SpacingSm);
        margin.AddThemeConstantOverride("margin_top", DesignTheme.SpacingXs);
        margin.AddThemeConstantOverride("margin_right", DesignTheme.SpacingSm);
        margin.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingXs);
        AddChild(margin);

        _strip = new HBoxContainer();
        _strip.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _strip.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        margin.AddChild(_strip);
    }

    public AshfallMetricCard AddCard(string key, string label, string value,
        AshfallMetricCard.Criticality criticality = AshfallMetricCard.Criticality.Normal,
        int minWidth = 120)
    {
        if (string.IsNullOrEmpty(key)) return null!;
        if (_cards.ContainsKey(key)) return _cards[key];

        var card = new AshfallMetricCard(label, value, criticality, minWidth);
        _cards[key] = card;
        _strip.AddChild(card);
        return card;
    }

    /// <summary>
    /// Adds a vertical separator between cards in the rail. Useful for
    /// grouping "resource" vs "morale" pairs the way the Stitch dashboards do.
    /// </summary>
    public void AddSeparator()
    {
        if (_strip == null) return;
        var sep = new VSeparator();
        sep.CustomMinimumSize = new Vector2(1, 30);
        _strip.AddChild(sep);
    }

    public AshfallMetricCard? GetCard(string key)
    {
        if (string.IsNullOrEmpty(key)) return null;
        return _cards.TryGetValue(key, out var c) ? c : null;
    }

    public void Set(string key, string value, AshfallMetricCard.Criticality c)
    {
        var card = GetCard(key);
        card?.SetValue(value);
        card?.SetCriticality(c);
    }

    public void SetLabel(string key, string label)
    {
        GetCard(key)?.SetLabel(label);
    }
}
