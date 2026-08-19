using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Weather Forecast panel.
    /// Shows detailed 7-day weather forecast with temperature trends, precipitation, and wind patterns.
    /// </summary>
    public partial class WeatherForecastPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblForecastTitle;
        private VBoxContainer _forecastData;
        private Label _lblTemperatureTitle;
        private VBoxContainer _temperatureTrend;
        private Label _lblPrecipitationTitle;
        private VBoxContainer _precipitationData;
        private Label _lblWindTitle;
        private VBoxContainer _windForecast;

        private readonly string[] _placeholderForecast = {
            "[Day 26] Clear, -8°C, low wind, radiation dropping",
            "[Day 27] Overcast, -6°C, wind increasing to 20 km/h",
            "[Day 28] Dust storm, -4°C, visibility <100m, rad spike",
            "[Day 29] Clearing, -7°C, wind calming, radiation falling",
            "[Day 30] Clear, -10°C, low wind, radiation normal",
            "[Day 31] Sunny, -12°C, calm conditions, optimal visibility",
            "[Day 32] Clear, -9°C, light breeze, radiation stable"
        };

        private readonly string[] _placeholderTemperature = {
            "Day 26: -8°C (Clear)",
            "Day 27: -6°C (Overcast)",
            "Day 28: -4°C (Storm)",
            "Day 29: -7°C (Clearing)",
            "Day 30: -10°C (Clear)",
            "Day 31: -12°C (Sunny)",
            "Day 32: -9°C (Clear)",
            "Trend: Cooling trend, nuclear winter persisting"
        };

        private readonly string[] _placeholderPrecipitation = {
            "Day 26: None (Clear)",
            "Day 27: Light snow expected",
            "Day 28: Heavy dust storm",
            "Day 29: Light snow clearing",
            "Day 30: None (Clear)",
            "Day 31: None (Sunny)",
            "Day 32: Light snow possible",
            "Total precipitation: Low (Nuclear winter conditions)"
        };

        private readonly string[] _placeholderWind = {
            "Day 26: 10 km/h West",
            "Day 27: 20 km/h West (Increasing)",
            "Day 28: 30 km/h NW (Storm)",
            "Day 29: 15 km/h NW (Calming)",
            "Day 30: 8 km/h West (Calm)",
            "Day 31: 5 km/h West (Calm)",
            "Day 32: 10 km/h West (Light breeze)",
            "Pattern: Western flow, storm cycle Day 27-29"
        };

        public void Bind(object weatherForecast)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_forecastData == null || _temperatureTrend == null || _precipitationData == null || _windForecast == null) return;

            AshfallUiHelpers.EmptyChildren(_forecastData);
            AshfallUiHelpers.EmptyChildren(_temperatureTrend);
            AshfallUiHelpers.EmptyChildren(_precipitationData);
            AshfallUiHelpers.EmptyChildren(_windForecast);

            foreach (string forecast in _placeholderForecast)
            {
                var label = new Label { Text = forecast };
                label.CustomMinimumSize = new Vector2(350, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
                _forecastData.AddChild(label);
            }

            foreach (string temp in _placeholderTemperature)
            {
                var label = new Label { Text = temp };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _temperatureTrend.AddChild(label);
            }

            foreach (string precip in _placeholderPrecipitation)
            {
                var label = new Label { Text = precip };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _precipitationData.AddChild(label);
            }

            foreach (string wind in _placeholderWind)
            {
                var label = new Label { Text = wind };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _windForecast.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("WEATHER FORECAST", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblForecastTitle = AshfallUiHelpers.MakeSectionHeader("7-DAY FORECAST");
            vbox.AddChild(_lblForecastTitle);

            _forecastData = new VBoxContainer();
            _forecastData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _forecastData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_forecastData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblTemperatureTitle = AshfallUiHelpers.MakeSectionHeader("TEMPERATURE TREND");
            vbox.AddChild(_lblTemperatureTitle);

            _temperatureTrend = new VBoxContainer();
            _temperatureTrend.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _temperatureTrend.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_temperatureTrend);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblPrecipitationTitle = AshfallUiHelpers.MakeSectionHeader("PRECIPITATION");
            vbox.AddChild(_lblPrecipitationTitle);

            _precipitationData = new VBoxContainer();
            _precipitationData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _precipitationData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_precipitationData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblWindTitle = AshfallUiHelpers.MakeSectionHeader("WIND FORECAST");
            vbox.AddChild(_lblWindTitle);

            _windForecast = new VBoxContainer();
            _windForecast.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _windForecast.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_windForecast);

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
