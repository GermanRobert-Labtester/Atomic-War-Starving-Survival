using System;
using System.Linq;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using Ashfall.Core.World;
using Ashfall.Core;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Weather Detail panel.
    /// Shows detailed weather breakdown bound to the live WeatherSystem.
    /// Unbound renders "NOT MONITORED".
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

        private WeatherSystem? _weather;

        public bool IsBound => _weather != null;
        public int RenderedRowCount { get; private set; }

        public void Bind(WeatherSystem? weather)
        {
            _weather = weather;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_currentWeather == null || _forecastList == null || _windData == null || _trendData == null) return;

            AshfallUiHelpers.EmptyChildren(_currentWeather);
            AshfallUiHelpers.EmptyChildren(_forecastList);
            AshfallUiHelpers.EmptyChildren(_windData);
            AshfallUiHelpers.EmptyChildren(_trendData);

            RenderedRowCount = 0;

            if (_weather == null)
            {
                _currentWeather.AddChild(MakeDimLine("No weather system bound."));
                return;
            }

            var kind = _weather.Current;
            bool hazard = kind == WeatherKind.FalloutStorm || kind == WeatherKind.BlackRain || kind == WeatherKind.Blizzard;
            float tempPenalty = WeatherSystem.TemperaturePenaltyForWeather(kind);
            float outdoorRad = _weather.OutdoorRadModifier;

            AddRow(_currentWeather, $"Current Weather: {kind}", Ashfall.Core.UI.Theme.Pale);
            AddRow(_currentWeather, $"Temperature Penalty: {tempPenalty:0}°C", Ashfall.Core.UI.Theme.Lethe);
            AddRow(_currentWeather, $"Outdoor Rad Modifier: {(outdoorRad > 0 ? "+" : "")}{outdoorRad:0} mSv/h",
                outdoorRad > 0 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe);
            AddRow(_currentWeather, $"Elapsed Hours: {_weather.State.totalElapsedHours:0}", Ashfall.Core.UI.Theme.Pale);
            AddRow(_currentWeather, $"Next Check In: {_weather.State.hoursUntilNextCheck:0.0} h", Ashfall.Core.UI.Theme.Pale);
            AddRow(_currentWeather, $"Roll Count: {_weather.State.rollCount}", Ashfall.Core.UI.Theme.Dim);
            RenderedRowCount += 6;

            if (hazard)
            {
                AddRow(_forecastList, $"HAZARD ACTIVE — {kind} conditions", Ashfall.Core.UI.Theme.Critical);
                AddRow(_forecastList, "Keep survivors indoors; outdoor exposure dangerous", Ashfall.Core.UI.Theme.Warm);
            }
            else
            {
                AddRow(_forecastList, "No hazard weather active", Ashfall.Core.UI.Theme.Lethe);
                AddRow(_forecastList, $"Next weather check in {_weather.State.hoursUntilNextCheck:0.0} hours", Ashfall.Core.UI.Theme.Dim);
            }

            AddRow(_windData, "Wind data not modeled in Core weather system", Ashfall.Core.UI.Theme.Dim);
            AddRow(_trendData, $"Weather roll #{_weather.State.rollCount} — deterministic from seed", Ashfall.Core.UI.Theme.Dim);
            AddRow(_trendData, _weather.State.restrictToNonHazardWeather ? "Restricted to non-hazard weather" : "All weather kinds enabled",
                Ashfall.Core.UI.Theme.Dim);
        }

        private void AddRow(VBoxContainer parent, string text, (float r, float g, float b, float a) col)
        {
            var label = new Label { Text = text };
            label.CustomMinimumSize = new Vector2(350, 0);
            label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(col));
            parent.AddChild(label);
        }

        private Label MakeDimLine(string text)
        {
            var l = new Label { Text = text };
            l.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            l.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            return l;
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

            _lblCurrentTitle = AshfallUiHelpers.MakeSectionHeader("CURRENT CONDITIONS");
            vbox.AddChild(_lblCurrentTitle);
            _currentWeather = new VBoxContainer();
            _currentWeather.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _currentWeather.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_currentWeather);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblForecastTitle = AshfallUiHelpers.MakeSectionHeader("HAZARD FORECAST");
            vbox.AddChild(_lblForecastTitle);
            _forecastList = new VBoxContainer();
            _forecastList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _forecastList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_forecastList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblWindTitle = AshfallUiHelpers.MakeSectionHeader("WIND PATTERN");
            vbox.AddChild(_lblWindTitle);
            _windData = new VBoxContainer();
            _windData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _windData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_windData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

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
