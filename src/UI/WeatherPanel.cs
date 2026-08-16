using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.World;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Weather panel.
    /// Shows current weather conditions, forecasts, environmental hazards, and temperature telemetry.
    /// </summary>
    public partial class WeatherPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _currentWeather = null!;
        private VBoxContainer _forecastList = null!;
        private VBoxContainer _hazardList = null!;

        private WorldHostSession? _worldHost;

        public bool IsBound => _worldHost != null;
        public WeatherKind? BoundWeather => _worldHost?.Weather.Current;
        public int RenderedHazardCount => _hazardList?.GetChildCount() ?? 0;

        public void Bind(WorldHostSession weather)
        {
            _worldHost = weather;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_currentWeather == null || _forecastList == null || _hazardList == null) return;

            while (_currentWeather.GetChildCount() > 0)
                _currentWeather.RemoveChild(_currentWeather.GetChild(0));
            while (_forecastList.GetChildCount() > 0)
                _forecastList.RemoveChild(_forecastList.GetChild(0));
            while (_hazardList.GetChildCount() > 0)
                _hazardList.RemoveChild(_hazardList.GetChild(0));

            if (_worldHost == null)
            {
                _currentWeather.AddChild(AshfallUiHelpers.MakeMetadata("No world session bound."));
                _forecastList.AddChild(AshfallUiHelpers.MakeMetadata("No weather state available."));
                _hazardList.AddChild(AshfallUiHelpers.MakeMetadata("No hazard readout available."));
                return;
            }

            var weather = _worldHost.Weather;
            int day = Math.Max(1, (int)Math.Floor(weather.State.totalElapsedHours / 24f) + 1);
            var season = weather.GetSeasonForDay(day);
            float temperaturePenalty = WeatherSystem.TemperaturePenaltyForWeather(weather.Current);

            _currentWeather.AddChild(AshfallUiHelpers.MakeDataRow(
                "Current Weather Pattern",
                $"{weather.Current}".ToUpperInvariant(),
                new Color(0.9f, 0.9f, 0.9f)));
            _currentWeather.AddChild(AshfallUiHelpers.MakeDataRow(
                "Atmospheric Visibility",
                $"{weather.VisibilityFactor:P0}",
                AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            _currentWeather.AddChild(AshfallUiHelpers.MakeDataRow(
                "Outdoor Radiation Modifier",
                $"+{weather.OutdoorRadModifier:0} mSv/hr",
                weather.OutdoorRadModifier > 0 ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical) : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe)));
            _currentWeather.AddChild(AshfallUiHelpers.MakeDataRow(
                "Temperature Penalty",
                $"{temperaturePenalty:+0;-0;0}°C",
                temperaturePenalty < 0 ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm) : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            _currentWeather.AddChild(AshfallUiHelpers.MakeDataRow(
                "Hazmat Gear Degradation",
                $"×{weather.HazmatDegradeMultiplier:0.0} Burn Rate",
                AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Entropy)));

            string profileName = _worldHost.Profile?.displayName ?? season.displayName;
            _forecastList.AddChild(AshfallUiHelpers.MakeDataRow("Active Season Profile", profileName, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe)));
            _forecastList.AddChild(AshfallUiHelpers.MakeDataRow("Next Weather Shift Check", $"In {weather.State.hoursUntilNextCheck:0.0} Hours", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            _forecastList.AddChild(AshfallUiHelpers.MakeDataRow("Recorded Cycle Rolls", $"{weather.State.rollCount} Rolls Complete", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim)));

            int hazardCount = 0;
            if (weather.IsScavengingBlocked(false))
            {
                var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                var icon = AshfallUiHelpers.MakeBadgeIcon("badge_corneal_burn", 22);
                row.AddChild(icon);
                var lbl = AshfallUiHelpers.MakeWarning("Scavenging expedition blocked without full hazard gear.");
                lbl.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                row.AddChild(lbl);
                _hazardList.AddChild(row);
                hazardCount++;
            }
            if (weather.OutdoorRadModifier > 0f)
            {
                var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                var icon = AshfallUiHelpers.MakeBadgeIcon("badge_radon_poisoning", 22);
                row.AddChild(icon);
                var lbl = AshfallUiHelpers.MakeCritical($"Fallout radiation elevated: +{weather.OutdoorRadModifier:0} mSv/hr.");
                lbl.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                row.AddChild(lbl);
                _hazardList.AddChild(row);
                hazardCount++;
            }
            if (temperaturePenalty < 0f)
            {
                var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                var icon = AshfallUiHelpers.MakeBadgeIcon("badge_hypothermia", 22);
                row.AddChild(icon);
                var lbl = AshfallUiHelpers.MakeWarning($"Severe cold exposure risk: {temperaturePenalty:0}°C.");
                lbl.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                row.AddChild(lbl);
                _hazardList.AddChild(row);
                hazardCount++;
            }
            if (hazardCount == 0)
                _hazardList.AddChild(AshfallUiHelpers.MakeMetadata("No acute environmental hazards detected. Outdoor scavenging permitted."));
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.88f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var panel = AshfallUiHelpers.MakePanel(700, 560);
            center.AddChild(panel);

            var margins = AshfallUiHelpers.MakeMargins(Ashfall.Core.UI.Theme.SpacingMd);
            panel.AddChild(margins);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            margins.AddChild(vbox);

            var header = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            var title = AshfallUiHelpers.MakeTitle("WEATHER & ENVIRONMENTAL TELEMETRY", Ashfall.Core.UI.Theme.FontSizeH2);
            title.HorizontalAlignment = HorizontalAlignment.Left;
            title.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            header.AddChild(title);

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(110, 32);
            header.AddChild(btnClose);
            vbox.AddChild(header);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var scroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(660, 440),
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            vbox.AddChild(scroll);

            var contentBox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            contentBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(contentBox);

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("ATMOSPHERIC CONDITIONS"));
            _currentWeather = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_currentWeather);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("SEASON CYCLE & FORECAST"));
            _forecastList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_forecastList);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("ENVIRONMENTAL HAZARD ADVISORIES"));
            _hazardList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_hazardList);

            RefreshView();
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
}
