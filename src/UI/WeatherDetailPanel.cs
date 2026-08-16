using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Weather Detail panel.
    /// Shows detailed weather breakdown with hourly forecast, wind patterns, and temperature trends.
    /// </summary>
    public partial class WeatherDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblCurrentTitle;
        private VBoxContainer _currentWeather;
        private Label _lblForecastTitle;
        private VBoxContainer _forecastList;
        private Label _lblWindTitle;
        private VBoxContainer _windData;
        private Label _lblTrendTitle;
        private VBoxContainer _trendData;

        // Placeholder weather detail data
        private readonly string[] _placeholderCurrent = {
            "Temperature: -5°C (Nuclear Winter)",
            "Wind: 15 km/h West (Moderate)",
            "Visibility: 200m (Dust storm)",
            "Radiation: 0.8 mSv/hr (Elevated)",
            "Humidity: 65% (High)",
            "Pressure: 1013 hPa (Normal)"
        };

        private readonly string[] _placeholderForecast = {
            "[Hour 0-6] Clear, -8°C, low wind, radiation dropping",
            "[Hour 6-12] Overcast, -6°C, wind increasing to 20 km/h",
            "[Hour 12-18] Dust storm, -4°C, visibility <100m, rad spike",
            "[Hour 18-24] Clearing, -7°C, wind calming, radiation falling"
        };

        private readonly string[] _placeholderWind = {
            "Current Direction: West (270°)",
            "Speed: 15 km/h (Moderate)",
            "Gusts: Up to 25 km/h",
            "Pattern: Stable western flow",
            "Expected Change: Shifting to NW by hour 12"
        };

        private readonly string[] _placeholderTrend = {
            "Temperature Trend: Stable at -5°C",
            "Radiation Trend: Elevated but stable",
            "Wind Trend: Increasing then decreasing",
            "Visibility Trend: Poor then improving",
            "7-Day Forecast: Nuclear winter conditions persisting",
            "Expected Improvement: Day 30+ (weather clearing)"
        };

        // Real data from host session
        // private WeatherHostSession? _weatherHost;

        public void Bind(object weather) // placeholder for WeatherHostSession
        {
            // _weatherHost = (WeatherHostSession)weather;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_currentWeather == null || _forecastList == null || _windData == null || _trendData == null) return;

            // Clear existing lists
            while (_currentWeather.GetChildCount() > 0)
                _currentWeather.RemoveChild(_currentWeather.GetChild(0));
            while (_forecastList.GetChildCount() > 0)
                _forecastList.RemoveChild(_forecastList.GetChild(0));
            while (_windData.GetChildCount() > 0)
                _windData.RemoveChild(_windData.GetChild(0));
            while (_trendData.GetChildCount() > 0)
                _trendData.RemoveChild(_trendData.GetChild(0));

            // Display placeholder current weather
            foreach (string data in _placeholderCurrent)
            {
                var label = new Label { Text = data };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _currentWeather.AddChild(label);
            }

            // Display placeholder forecast
            foreach (string forecast in _placeholderForecast)
            {
                var label = new Label { Text = forecast };
                label.CustomMinimumSize = new Vector2(350, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
                _forecastList.AddChild(label);
            }

            // Display placeholder wind data
            foreach (string wind in _placeholderWind)
            {
                var label = new Label { Text = wind };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _windData.AddChild(label);
            }

            // Display placeholder trend data
            foreach (string trend in _placeholderTrend)
            {
                var label = new Label { Text = trend };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _trendData.AddChild(label);
            }
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

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(550, 0);
            container.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("WEATHER DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Current weather section
            _lblCurrentTitle = AshfallUiHelpers.MakeSectionHeader("CURRENT CONDITIONS");
            vbox.AddChild(_lblCurrentTitle);

            _currentWeather = new VBoxContainer();
            _currentWeather.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _currentWeather.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_currentWeather);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Forecast section
            _lblForecastTitle = AshfallUiHelpers.MakeSectionHeader("HOURLY FORECAST");
            vbox.AddChild(_lblForecastTitle);

            _forecastList = new VBoxContainer();
            _forecastList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _forecastList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_forecastList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Wind data section
            _lblWindTitle = AshfallUiHelpers.MakeSectionHeader("WIND PATTERN");
            vbox.AddChild(_lblWindTitle);

            _windData = new VBoxContainer();
            _windData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _windData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_windData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Trend section
            _lblTrendTitle = AshfallUiHelpers.MakeSectionHeader("WEATHER TREND");
            vbox.AddChild(_lblTrendTitle);

            _trendData = new VBoxContainer();
            _trendData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _trendData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_trendData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);

            var hint = AshfallUiHelpers.MakeSmall("[Esc] to close");
            hint.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeLabel);
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            vbox.AddChild(hint);
        }

        public void Open()
        {
            Visible = true;
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
}
