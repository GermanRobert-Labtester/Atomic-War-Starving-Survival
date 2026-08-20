using System;
using System.Collections.Generic;
using Ashfall.Core.World;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class WeatherStationState
    {
        public string systemId = WeatherStationSystem.SystemId;
        public bool isInstalled;
        public bool isCalibrated;
        public int installDay = -1;
        public int calibrationDay = -1;
        public int forecastHorizonDays = 3;
        public float accuracy = 0.7f;
        public int lastForecastDay = -1;
        public List<ForecastEntry> cachedForecast = new List<ForecastEntry>();
    }

    [Serializable]
    public sealed class ForecastEntry
    {
        public int day;
        public WeatherKind weather;
        public float confidence;
        public bool isRouteSafe;
        public float temperature;
        public string warning = string.Empty;
    }

    public sealed class WeatherStationSystem
    {
        public const string SystemId = "weather_station";

        private WeatherStationState _state = new WeatherStationState();
        private readonly WeatherSystem _weatherSystem;
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        public WeatherStationState State => _state;
        public bool IsOperational => _state.isInstalled && _state.isCalibrated;
        public event Action OnForecastUpdated;
        public event Action OnStationStateChanged;

        public WeatherStationSystem(WeatherSystem weatherSystem, ISeededRng rng, ILog log = null)
        {
            _weatherSystem = weatherSystem ?? throw new ArgumentNullException(nameof(weatherSystem));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public ActionResult Install(int day)
        {
            if (_state.isInstalled) return ActionResult.Blocked("already_installed", "weather.already_installed");
            _state.isInstalled = true;
            _state.installDay = day;
            _log.Info($"[WeatherStation] installed on day {day}");
            OnStationStateChanged?.Invoke();
            return ActionResult.Success("weather.installed");
        }

        public ActionResult Calibrate(int day)
        {
            if (!_state.isInstalled) return ActionResult.Blocked("not_installed", "weather.not_installed");
            if (_state.isCalibrated) return ActionResult.Blocked("already_calibrated", "weather.already_calibrated");
            _state.isCalibrated = true;
            _state.calibrationDay = day;
            _state.accuracy = 0.7f + (float)_rng.NextDouble() * 0.2f;
            _log.Info($"[WeatherStation] calibrated (accuracy={_state.accuracy:F2})");
            OnStationStateChanged?.Invoke();
            return ActionResult.Success("weather.calibrated", new Dictionary<string, double> { { "accuracy", _state.accuracy } });
        }

        public ActionResult GenerateForecast(int currentDay)
        {
            if (!IsOperational) return ActionResult.Blocked("not_operational", "weather.not_operational");
            _state.cachedForecast.Clear();
            _state.lastForecastDay = currentDay;

            var rawForecast = _weatherSystem.PeekForecast(_state.forecastHorizonDays);
            for (int i = 0; i < rawForecast.Count && i < _state.forecastHorizonDays; i++)
            {
                var f = rawForecast[i];
                float confidence = Math.Max(0.1f, Math.Min(1f, _state.accuracy * (1f - i * 0.2f)));
                bool isRouteSafe = f.Kind switch
                {
                    WeatherKind.Clear or WeatherKind.Overcast or WeatherKind.Rain => true,
                    WeatherKind.FalloutStorm or WeatherKind.BlackRain or WeatherKind.Blizzard => false,
                    _ => confidence > 0.5f
                };

                _state.cachedForecast.Add(new ForecastEntry
                {
                    day = f.Day,
                    weather = f.Kind,
                    confidence = confidence,
                    isRouteSafe = isRouteSafe,
                    temperature = 5f,
                    warning = (!isRouteSafe && confidence > 0.4f)
                        ? $"WARNING: {f.Kind} expected — travel not recommended"
                        : string.Empty
                });
            }

            OnForecastUpdated?.Invoke();
            return ActionResult.Success("weather.forecast_generated",
                new Dictionary<string, double> { { "days", _state.forecastHorizonDays } });
        }

        public IReadOnlyList<ForecastEntry> GetForecast() => _state.cachedForecast.AsReadOnly();
        public bool IsRouteSafe(int day)
        {
            foreach (var e in _state.cachedForecast)
                if (e.day == day) return e.isRouteSafe;
            return false;
        }

        public float GetConfidence(int day)
        {
            foreach (var e in _state.cachedForecast)
                if (e.day == day) return e.confidence;
            return 0f;
        }

        public WeatherStationState CaptureState() => _state;
        public void RestoreState(WeatherStationState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnStationStateChanged?.Invoke();
        }
    }
}
