using System;
using Godot;
using Ashfall.Core.World;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Weather Sonde panel.
    /// Thin presentation layer for radiosonde management.
    /// Shows launch status, telemetry, altitude, battery, forecast confidence.
    /// All gameplay logic delegates to WeatherHostSession → WeatherSondeSystem.
    /// </summary>
    public partial class WeatherSondePanel : Control
    {
        public event Action? OnClose;

        private WeatherHostSession? _weatherHost;

        private Label _headerLabel = null!;
        private Label _statusLabel = null!;
        private Label _altitudeLabel = null!;
        private Label _batteryLabel = null!;
        private Label _hydrogenLabel = null!;
        private Label _sensorLabel = null!;
        private Label _qualityLabel = null!;
        private VBoxContainer _telemetryContainer = null!;
        private VBoxContainer _forecastContainer = null!;
        private Label _feedbackLabel = null!;
        private Button _launchButton = null!;
        private Button _tickButton = null!;
        private Button _closeButton = null!;

        public bool IsBound => _weatherHost != null;

        public void Bind(WeatherHostSession weatherHost)
        {
            if (_weatherHost != null)
            {
                _weatherHost.StateChanged -= RefreshView;
            }

            _weatherHost = weatherHost;

            if (_weatherHost != null)
            {
                _weatherHost.StateChanged += RefreshView;
                RefreshView();
            }
        }

        public override void _Ready()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            var margin = AshfallUiHelpers.MakeMargins(16);
            AddChild(margin);

            var root = new VBoxContainer();
            margin.AddChild(root);

            _headerLabel = AshfallUiHelpers.MakeLabel("WEATHER RADIOSONDE", 20, true);
            root.AddChild(_headerLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            _statusLabel = AshfallUiHelpers.MakeBody("Status: —");
            root.AddChild(_statusLabel);

            _altitudeLabel = AshfallUiHelpers.MakeBody("Altitude: —");
            root.AddChild(_altitudeLabel);

            _batteryLabel = AshfallUiHelpers.MakeBody("Battery: —");
            root.AddChild(_batteryLabel);

            _hydrogenLabel = AshfallUiHelpers.MakeBody("Hydrogen: —");
            root.AddChild(_hydrogenLabel);

            _sensorLabel = AshfallUiHelpers.MakeBody("Sensor: —");
            root.AddChild(_sensorLabel);

            _qualityLabel = AshfallUiHelpers.MakeBody("Observation Quality: —");
            root.AddChild(_qualityLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            root.AddChild(AshfallUiHelpers.MakeBody("Telemetry:"));
            _telemetryContainer = new VBoxContainer();
            root.AddChild(_telemetryContainer);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            root.AddChild(AshfallUiHelpers.MakeBody("Forecast:"));
            _forecastContainer = new VBoxContainer();
            root.AddChild(_forecastContainer);

            _feedbackLabel = AshfallUiHelpers.MakeBody("");
            root.AddChild(_feedbackLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            var buttonRow = new HBoxContainer();
            root.AddChild(buttonRow);

            _launchButton = AshfallUiHelpers.MakeButton("Launch Sonde", OnLaunch);
            buttonRow.AddChild(_launchButton);

            _tickButton = AshfallUiHelpers.MakeButton("Advance Tick", OnTick);
            buttonRow.AddChild(_tickButton);

            _closeButton = AshfallUiHelpers.MakeButton("Close", () => OnClose?.Invoke());
            buttonRow.AddChild(_closeButton);
        }

        private void OnLaunch()
        {
            if (_weatherHost == null) return;
            string result = _weatherHost.LaunchSondeDemo(40); // TODO: get real day
            _feedbackLabel.Text = result;
            RefreshView();
        }

        private void OnTick()
        {
            if (_weatherHost == null) return;
            string result = _weatherHost.TickSondeDemo();
            _feedbackLabel.Text = result;
            RefreshView();
        }

        private void RefreshView()
        {
            if (_weatherHost == null) return;

            var state = _weatherHost.Sonde.State;
            _statusLabel.Text = _weatherHost.SondeStatusLine();

            if (!state.isLaunched)
            {
                _altitudeLabel.Text = "Altitude: —";
                _batteryLabel.Text = "Battery: —";
                _hydrogenLabel.Text = "Hydrogen: —";
                _sensorLabel.Text = "Sensor: —";
                _qualityLabel.Text = "Observation Quality: —";
                _launchButton.Disabled = false;
                _tickButton.Disabled = true;
                return;
            }

            _altitudeLabel.Text = $"Altitude: {_weatherHost.Sonde.GetCurrentAltitude():F1} km";
            _batteryLabel.Text = $"Battery: {state.batteryLevel:P0}";
            _hydrogenLabel.Text = $"Hydrogen: {state.hydrogenLevel:P0}";
            _sensorLabel.Text = $"Sensor Quality: {state.sensorQuality:P0}";
            _qualityLabel.Text = $"Observation Quality: {state.observationQuality:F2}";

            // Telemetry
            foreach (var child in _telemetryContainer.GetChildren())
                child.QueueFree();
            foreach (var sample in state.samples)
            {
                string lost = sample.isLost ? " [LOST]" : "";
                var label = AshfallUiHelpers.MakeBody(
                    $"  #{sample.sampleIndex}: alt={sample.altitudeKm:F1}km temp={sample.temperatureC:F1}C " +
                    $"rad={sample.radiationMsv:F2}mSv wind={sample.windSpeedKmh:F0}km/h{lost}");
                _telemetryContainer.AddChild(label);
            }

            // Forecast
            foreach (var child in _forecastContainer.GetChildren())
                child.QueueFree();
            foreach (var entry in state.forecast)
            {
                var label = AshfallUiHelpers.MakeBody(
                    $"  Day +{entry.dayOffset}: {entry.predictedKind} (confidence {entry.confidence:P0}, uncertainty ±{entry.uncertaintyRadius:F2})");
                _forecastContainer.AddChild(label);
            }

            _launchButton.Disabled = true;
            _tickButton.Disabled = state.isRecovered || state.isFailed;
        }

        public override void _ExitTree()
        {
            if (_weatherHost != null)
            {
                _weatherHost.StateChanged -= RefreshView;
            }
        }
    }
}
