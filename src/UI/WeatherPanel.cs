using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Weather panel.
    /// Shows current weather conditions, forecasts, and environmental hazards.
    /// </summary>
    public partial class WeatherPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblCurrentTitle;
        private VBoxContainer _currentWeather;
        private Label _lblForecastTitle;
        private VBoxContainer _forecastList;
        private Label _lblHazardsTitle;
        private VBoxContainer _hazardList;

        // Placeholder weather data
        private readonly string[] _placeholderCurrentWeather = {
            "Temperature: -5°C (Nuclear Winter)",
            "Wind: 15 km/h (West)",
            "Visibility: 200m (Dust storm)",
            "Radiation: Elevated (0.8 mSv/hr)",
            "Precipitation: None"
        };

        private readonly string[] _placeholderForecast = {
            "[Day 16] Clear skies, -10°C, low radiation",
            "[Day 17] Fallout dust, -8°C, elevated rad",
            "[Day 18] Snow storm, -15°C, reduced visibility",
            "[Day 19] Clearing, -12°C, radiation dropping"
        };

        private readonly string[] _placeholderHazards = {
            "Fallout zone nearby (Sector 7)",
            "Radiation spike expected (Day 17)",
            "Nuclear winter conditions persisting",
            "Dust storm reducing visibility"
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
            if (_currentWeather == null || _forecastList == null || _hazardList == null) return;

            // Clear existing lists
            while (_currentWeather.GetChildCount() > 0)
                _currentWeather.RemoveChild(_currentWeather.GetChild(0));
            while (_forecastList.GetChildCount() > 0)
                _forecastList.RemoveChild(_forecastList.GetChild(0));
            while (_hazardList.GetChildCount() > 0)
                _hazardList.RemoveChild(_hazardList.GetChild(0));

            // Display placeholder current weather
            foreach (string condition in _placeholderCurrentWeather)
            {
                var label = new Label { Text = condition };
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

            // Display placeholder hazards
            foreach (string hazard in _placeholderHazards)
            {
                var label = new Label { Text = hazard };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
                _hazardList.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("WEATHER & ENVIRONMENT", Ashfall.Core.UI.Theme.FontSizeH1);
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
            _lblForecastTitle = AshfallUiHelpers.MakeSectionHeader("FORECAST (4 DAYS)");
            vbox.AddChild(_lblForecastTitle);

            _forecastList = new VBoxContainer();
            _forecastList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _forecastList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_forecastList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Hazards section
            _lblHazardsTitle = AshfallUiHelpers.MakeSectionHeader("ENVIRONMENTAL HAZARDS");
            vbox.AddChild(_lblHazardsTitle);

            _hazardList = new VBoxContainer();
            _hazardList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _hazardList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_hazardList);

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
