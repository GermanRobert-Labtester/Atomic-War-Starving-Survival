using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Metric Card.
/// A small labeled-value chip used in the top status rail of dashboard-style
/// surfaces. Pure presentation: caller supplies text + optional criticality,
/// the widget renders a 9-slice framed chip with label (top, muted) +
/// value (bottom, mono, colored by criticality).
///
/// Criticality:
///   0 = Normal   (warm/positive baseline)
///   1 = Caution  (lethe cyan-grey)
///   2 = Warn     (entropy corroded amber)
///   3 = Critical (critical restrained red)
///
/// Uses DesignTheme.* tokens via AshfallUiHelpers; no hard-coded colors.
/// </summary>
public partial class AshfallMetricCard : PanelContainer
{
    public enum Criticality { Normal = 0, Caution = 1, Warn = 2, Critical = 3 }

    private readonly Label _labelLbl;
    private readonly Label _valueLbl;
    private string _labelText = string.Empty;
    private string _valueText = string.Empty;
    private Criticality _criticality = Criticality.Normal;

    public AshfallMetricCard(string label, string value, Criticality criticality = Criticality.Normal,
        int minWidth = 120)
    {
        // Two stacked rows (label above value); 44px could not hold both
        // lines once margins were paid, so the mono value painted over the
        // Barlow label wherever the text runs met.
        CustomMinimumSize = new Vector2(minWidth, 56);
        SizeFlagsHorizontal = SizeFlags.ShrinkCenter;

        var sb = new StyleBoxFlat
        {
            BgColor = new Color(DesignTheme.Ink.r, DesignTheme.Ink.g, DesignTheme.Ink.b, 0.72f),
            BorderColor = AshfallUiHelpers.ToColor(DesignTheme.LineSoft),
        };
        sb.SetBorderWidthAll(1);
        AddThemeStyleboxOverride("panel", sb);

        var margin = new MarginContainer();
        margin.AddThemeConstantOverride("margin_left", DesignTheme.SpacingSm);
        margin.AddThemeConstantOverride("margin_top", DesignTheme.SpacingXs);
        margin.AddThemeConstantOverride("margin_right", DesignTheme.SpacingSm);
        margin.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingXs);
        AddChild(margin);

        var rows = new VBoxContainer();
        rows.AddThemeConstantOverride("separation", 0);
        margin.AddChild(rows);

        _labelLbl = new Label
        {
            Text = label?.ToUpperInvariant() ?? string.Empty,
            VerticalAlignment = VerticalAlignment.Center
        };
        _labelLbl.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeLabel);
        _labelLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
        var labelFont = AshfallUiHelpers.LoadFont("res://assets/fonts/BarlowCondensed-Regular.ttf");
        if (labelFont != null) _labelLbl.AddThemeFontOverride("font", labelFont);
        rows.AddChild(_labelLbl);

        _valueLbl = new Label
        {
            Text = value ?? string.Empty,
            VerticalAlignment = VerticalAlignment.Center
        };
        _valueLbl.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeMono);
        _valueLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
        var monoFont = AshfallUiHelpers.LoadFont("res://assets/fonts/ShareTechMono-Regular.ttf");
        if (monoFont != null) _valueLbl.AddThemeFontOverride("font", monoFont);
        rows.AddChild(_valueLbl);

        _labelText = label ?? string.Empty;
        _valueText = value ?? string.Empty;
        _criticality = criticality;
        ApplyCriticality();
    }

    public void SetValue(string value)
    {
        _valueText = value ?? string.Empty;
        _valueLbl.Text = _valueText;
    }

    public void SetLabel(string label)
    {
        _labelText = label ?? string.Empty;
        _labelLbl.Text = _labelText.ToUpperInvariant();
    }

    public void SetCriticality(Criticality c)
    {
        _criticality = c;
        ApplyCriticality();
    }

    public void Set(string label, string value, Criticality c)
    {
        SetLabel(label);
        SetValue(value);
        SetCriticality(c);
    }

    public string Label => _labelText;
    public string Value => _valueText;
    public Criticality CurrentCriticality => _criticality;

    private void ApplyCriticality()
    {
        var token = _criticality switch
        {
            Criticality.Critical => DesignTheme.Critical,
            Criticality.Warn => DesignTheme.Entropy,
            Criticality.Caution => DesignTheme.Lethe,
            _ => DesignTheme.Warm,
        };
        _valueLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(token));

        // Border also reflects criticality so the chip frames the urgency.
        if (_criticality == Criticality.Critical || _criticality == Criticality.Warn)
        {
            var sb = GetThemeStylebox("panel") as StyleBoxFlat;
            if (sb != null)
            {
                sb.BorderColor = AshfallUiHelpers.ToColor(token);
            }
        }
    }
}
