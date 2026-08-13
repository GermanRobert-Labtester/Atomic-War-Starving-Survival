using System;
using System.Collections.Generic;
using UnityEngine;
using Ashfall.Core;

namespace AtomicWar._Game.Environment
{
    [Serializable]
    public class ForecastState
    {
        public int sensorArrayLevel = 1;       // 1 = basic barometer, 2 = doppler rad, 3 = satellite link
        public int forecastHorizonDays = 5;
        public float sensorAccuracyBonus = 0f; // 0..0.3
        public List<ForecastEntry> cachedForecast = new List<ForecastEntry>();
    }

    [Serializable]
    public class ForecastEntry
    {
        public int dayOffset;
        public WeatherKind predictedWeather;
        public float predictedRadLevel;
        public float confidence; // 0.0 to 1.0 (decays per day out)
        public string windDirection;
    }

    /// <summary>
    /// Expansion V / Spec §3.3: Fallout Forecast System.
    /// Drives predictive meteorological and radiological forecasting with uncertainty bands.
    /// Interacts with WeatherSystem and shelter sensor equipment to inform expedition routing.
    /// </summary>
    public class FalloutForecastSystem
    {
        public const int MaxForecastDays = 7;

        private ForecastState _state = new ForecastState();
        private readonly WeatherSystem _weatherSystem;

        public event Action<IReadOnlyList<ForecastEntry>> OnForecastUpdated;

        public ForecastState State => _state;
        public int HorizonDays => _state.forecastHorizonDays;
        public int SensorLevel => _state.sensorArrayLevel;

        public FalloutForecastSystem(WeatherSystem weatherSystem, ForecastState state = null)
        {
            _weatherSystem = weatherSystem;
            _state = state ?? new ForecastState();
            if (_state.cachedForecast == null)
                _state.cachedForecast = new List<ForecastEntry>();
        }

        public void UpgradeSensorArray(int level)
        {
            _state.sensorArrayLevel = Mathf.Clamp(level, 1, 3);
            _state.forecastHorizonDays = 3 + (_state.sensorArrayLevel * 1); // 4, 5, 6 days
            _state.sensorAccuracyBonus = (_state.sensorArrayLevel - 1) * 0.1f;
        }

        /// <summary>
        /// Generates or refreshes the predictive forecast.
        /// </summary>
        public IReadOnlyList<ForecastEntry> GenerateForecast(int currentDay, int worldSeed)
        {
            int days = Mathf.Clamp(_state.forecastHorizonDays, 1, MaxForecastDays);
            var entries = new List<ForecastEntry>(days);

            // Fetch perfect or deterministic preview from WeatherSystem if available
            WeatherKind[] preview = _weatherSystem?.GetPerfectForecast(days) ?? Array.Empty<WeatherKind>();

            for (int i = 0; i < days; i++)
            {
                int dayOffset = i + 1;
                // Confidence degrades with distance into the future, mitigated by sensor level
                float baseConfidence = Mathf.Clamp01(1.0f - (i * 0.15f) + _state.sensorAccuracyBonus);
                
                WeatherKind predicted = i < preview.Length ? preview[i] : WeatherKind.Clear;
                
                // If confidence is low, there's a chance of noise
                int forecastSeed = unchecked(worldSeed * 31 + currentDay * 17 + i * 101);
                var rng = new System.Random(forecastSeed);

                float radEstimate = 20f;
                switch (predicted)
                {
                    case WeatherKind.FalloutStorm:
                        radEstimate = 180f + (float)(rng.NextDouble() * 50f);
                        break;
                    case WeatherKind.BlackRain:
                        radEstimate = 280f + (float)(rng.NextDouble() * 70f);
                        break;
                    case WeatherKind.Ashfall:
                        radEstimate = 75f + (float)(rng.NextDouble() * 30f);
                        break;
                    case WeatherKind.Blizzard:
                        radEstimate = 35f;
                        break;
                    default:
                        radEstimate = 15f + (float)(rng.NextDouble() * 10f);
                        break;
                }

                string[] directions = { "N", "NE", "E", "SE", "S", "SW", "W", "NW" };
                string wind = directions[rng.Next(directions.Length)];

                entries.Add(new ForecastEntry
                {
                    dayOffset = dayOffset,
                    predictedWeather = predicted,
                    predictedRadLevel = radEstimate,
                    confidence = baseConfidence,
                    windDirection = wind
                });
            }

            _state.cachedForecast = entries;
            OnForecastUpdated?.Invoke(_state.cachedForecast);
            return _state.cachedForecast;
        }

        public ForecastState CaptureState() => _state;

        public void RestoreState(ForecastState state)
        {
            _state = state ?? new ForecastState();
            if (_state.cachedForecast == null)
                _state.cachedForecast = new List<ForecastEntry>();
        }
    }
}
