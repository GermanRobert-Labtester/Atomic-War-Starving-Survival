using System;
using Godot;
using Ashfall.Core.UI;
using Ashfall.Core.World;
using Ashfall.Core;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Weather Detail panel. Shows detailed weather breakdown bound
    /// to the live WeatherSystem. Unbound renders "NOT MONITORED".
    ///
    /// Ticket #125: layout chrome is owned by
    /// <c>res://assets/ui/panels/WeatherDetailPanel.tscn</c>. The binder resolves
    /// the four typed content VBoxContainers via <see cref="SceneBinder"/>; the
    /// C# class projects the current WeatherSystem snapshot into them.
    /// </summary>
    public partial class WeatherDetailPanel : Control
    {
        public event Action? OnClose;

        private SceneBinder? _binder;
        private VBoxContainer _currentWeather = null!;
        private VBoxContainer _forecastList = null!;
        private VBoxContainer _windData = null!;
        private VBoxContainer _trendData = null!;
        private Button _closeButton = null!;

        private WeatherSystem? _weather;

        public bool IsBound => _weather != null;
        public int RenderedRowCount { get; private set; }

        public void Bind(WeatherSystem? weather)
        {
            _weather = weather;
            RefreshView();
        }

        public override void _Ready()
        {
            _binder = new SceneBinder(this, typeof(WeatherDetailPanel));
            _binder.Require<VBoxContainer>("CurrentList");
            _binder.Require<VBoxContainer>("ForecastList");
            _binder.Require<VBoxContainer>("WindList");
            _binder.Require<VBoxContainer>("TrendList");
            _binder.Require<Button>("CloseButton");

            _currentWeather = _binder.Get<VBoxContainer>("CurrentList");
            _forecastList = _binder.Get<VBoxContainer>("ForecastList");
            _windData = _binder.Get<VBoxContainer>("WindList");
            _trendData = _binder.Get<VBoxContainer>("TrendList");
            _closeButton = _binder.Get<Button>("CloseButton");
            _closeButton.Pressed += () => OnClose?.Invoke();

            Visible = false;
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
