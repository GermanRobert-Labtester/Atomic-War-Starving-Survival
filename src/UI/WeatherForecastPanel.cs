using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.World;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Weather Forecast panel (wired).
/// Shows real 7-day forecast from WeatherSystem.PeekForecast().
/// Replaces hardcoded placeholder strings with live data binding.
/// </summary>
public partial class WeatherForecastPanel : Control
{
    public event Action? OnClose;

    private WeatherSystem? _weather;
    private Action<WeatherKind>? _onWeatherChanged;

    private VBoxContainer _forecastData = null!;
    private VBoxContainer _temperatureTrend = null!;
    private VBoxContainer _precipitationData = null!;
    private VBoxContainer _windForecast = null!;

    public void Bind(WeatherSystem weather)
    {
        if (_weather != null && _onWeatherChanged != null)
            _weather.OnWeatherChanged -= _onWeatherChanged;

        _weather = weather;
        _onWeatherChanged ??= _ => RefreshView();

        if (_weather != null)
            _weather.OnWeatherChanged += _onWeatherChanged;

        RefreshView();
    }

    public override void _ExitTree()
    {
        if (_weather != null && _onWeatherChanged != null)
        {
            _weather.OnWeatherChanged -= _onWeatherChanged;
            _weather = null;
        }
        base._ExitTree();
    }

    public void RefreshView()
    {
        if (_weather == null || _forecastData == null) return;

        AshfallUiHelpers.EmptyChildren(_forecastData);
        AshfallUiHelpers.EmptyChildren(_temperatureTrend);
        AshfallUiHelpers.EmptyChildren(_precipitationData);
        AshfallUiHelpers.EmptyChildren(_windForecast);

        var forecast = _weather.PeekForecast(7);
        if (forecast.Count == 0)
        {
            AddEmptyHint(_forecastData, "No forecast data available.");
            return;
        }

        foreach (var f in forecast)
        {
            AddForecastRow(_forecastData, f);
            AddTemperatureRow(_temperatureTrend, f);
            AddPrecipRow(_precipitationData, f);
            AddWindRow(_windForecast, f);
        }
    }

    private void AddForecastRow(VBoxContainer container, WeatherForecastEntry f)
    {
        var when = f.Day > 0 ? $"Day {f.Day}" : "Today";
        var rad = f.OutdoorRad > 0f ? $", RAD +{f.OutdoorRad:0}" : "";
        var vis = f.Visibility is < 1f and > 0f ? $", VIS {f.Visibility:P0}" : "";
        var text = $"{when}: {f.Kind}{rad}{vis}";

        var label = new Label { Text = text };
        label.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
        label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
        label.CustomMinimumSize = new Vector2(350, 30);
        label.AutowrapMode = TextServer.AutowrapMode.WordSmart;
        container.AddChild(label);
    }

    private void AddTemperatureRow(VBoxContainer container, WeatherForecastEntry f)
    {
        // Temperature is implicit in weather kind; show the kind with hazard flag.
        string tempNote = f.Kind switch
        {
            Ashfall.Core.WeatherKind.Blizzard => "−15 °C wind chill",
            Ashfall.Core.WeatherKind.FalloutStorm => "−5 °C wind chill",
            Ashfall.Core.WeatherKind.BlackRain => "−8 °C wind chill",
            _ => "Baseline winter temperature"
        };

        var when = f.Day > 0 ? $"Day {f.Day}" : "Today";
        var text = $"{when}: {f.Kind} ({tempNote})";

        var label = new Label { Text = text };
        label.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
        label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
        label.CustomMinimumSize = new Vector2(350, 30);
        label.AutowrapMode = TextServer.AutowrapMode.WordSmart;
        container.AddChild(label);
    }

    private void AddPrecipRow(VBoxContainer container, WeatherForecastEntry f)
    {
        string precip = f.Kind switch
        {
            Ashfall.Core.WeatherKind.Rain => "Light rain",
            Ashfall.Core.WeatherKind.Ashfall => "Ashfall (radioactive dust)",
            Ashfall.Core.WeatherKind.FalloutStorm => "Heavy fallout storm",
            Ashfall.Core.WeatherKind.BlackRain => "Black rain (highly radioactive)",
            Ashfall.Core.WeatherKind.Blizzard => "Blizzard (snow + wind)",
            _ => "None"
        };

        var when = f.Day > 0 ? $"Day {f.Day}" : "Today";
        var text = $"{when}: {precip}";

        var label = new Label { Text = text };
        label.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
        label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
        label.CustomMinimumSize = new Vector2(350, 30);
        label.AutowrapMode = TextServer.AutowrapMode.WordSmart;
        container.AddChild(label);
    }

    private void AddWindRow(VBoxContainer container, WeatherForecastEntry f)
    {
        string wind = f.Kind switch
        {
            Ashfall.Core.WeatherKind.Blizzard => "High (storm-force)",
            Ashfall.Core.WeatherKind.FalloutStorm => "Gale (20–40 km/h)",
            Ashfall.Core.WeatherKind.BlackRain => "Heavy (30+ km/h)",
            Ashfall.Core.WeatherKind.Ashfall => "Moderate (15–25 km/h)",
            _ => "Light (< 10 km/h)"
        };

        var vis = f.Visibility is < 1f and > 0f ? $", visibility {f.Visibility:P0}" : "";
        var when = f.Day > 0 ? $"Day {f.Day}" : "Today";
        var text = $"{when}: {wind}{vis}";

        var label = new Label { Text = text };
        label.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
        label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
        label.CustomMinimumSize = new Vector2(350, 30);
        label.AutowrapMode = TextServer.AutowrapMode.WordSmart;
        container.AddChild(label);
    }

    private static void AddEmptyHint(VBoxContainer container, string hint)
    {
        var lbl = new Label { Text = hint };
        lbl.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeSmall);
        lbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
        lbl.AutowrapMode = TextServer.AutowrapMode.WordSmart;
        container.AddChild(lbl);
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);
        Visible = false;

        var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.92f) };
        bg.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(bg);

        var container = new CenterContainer();
        container.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(container);

        var vbox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingLg);
        vbox.CustomMinimumSize = new Vector2(550, 0);
        container.AddChild(vbox);

        var title = AshfallUiHelpers.MakeTitle("WEATHER FORECAST", DesignTheme.FontSizeH1);
        title.HorizontalAlignment = HorizontalAlignment.Center;
        vbox.AddChild(title);

        vbox.AddChild(AshfallUiHelpers.MakeSeparator());

        _forecastData = new VBoxContainer();
        _forecastData.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _forecastData.CustomMinimumSize = new Vector2(400, 0);
        vbox.AddChild(_forecastData);

        vbox.AddChild(AshfallUiHelpers.MakeSeparator());

        _temperatureTrend = new VBoxContainer();
        _temperatureTrend.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _temperatureTrend.CustomMinimumSize = new Vector2(400, 0);
        vbox.AddChild(_temperatureTrend);

        vbox.AddChild(AshfallUiHelpers.MakeSeparator());

        _precipitationData = new VBoxContainer();
        _precipitationData.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _precipitationData.CustomMinimumSize = new Vector2(400, 0);
        vbox.AddChild(_precipitationData);

        vbox.AddChild(AshfallUiHelpers.MakeSeparator());

        _windForecast = new VBoxContainer();
        _windForecast.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _windForecast.CustomMinimumSize = new Vector2(400, 0);
        vbox.AddChild(_windForecast);

        vbox.AddChild(AshfallUiHelpers.MakeSeparator());

        var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
        btnClose.CustomMinimumSize = new Vector2(200, 40);
        vbox.AddChild(btnClose);

        var hint = AshfallUiHelpers.MakeSmall("[Esc] to close");
        hint.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeLabel);
        hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
        vbox.AddChild(hint);
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
