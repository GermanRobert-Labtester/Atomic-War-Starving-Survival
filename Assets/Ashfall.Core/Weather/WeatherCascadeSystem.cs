// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Weather
{
    /// <summary>
    /// Contract for an external weather provider to notify the cascade system of new weather fronts.
    /// </summary>
    public interface IWeatherCascadeSource
    {
        event Action<WeatherKind, float, int> OnWeatherFrontArrived;
    }

    /// <summary>
    /// Subsystem coordinator managing the weather-to-gameplay cascade across the campaign (Plan 135 / C2[27]).
    /// Wraps and delegates to <see cref="WeatherGameplayCascadeEngine"/> while providing lifecycle and persistence hooks.
    /// </summary>
    public sealed class WeatherCascadeSystem
    {
        public const string SystemId = "weather_cascade_system";

        private readonly WeatherGameplayCascadeEngine _engine;
        private readonly ISeededRng? _rng;
        private int _currentDay = 1;
        private int _fortificationLevel = 0;

        public event Action<WeatherEvent, IReadOnlyList<WeatherEffect>>? OnCascadeEvaluated;

        public WeatherCascadeSystem(WeatherCascadeState? state = null, ISeededRng? rng = null)
        {
            _engine = new WeatherGameplayCascadeEngine(state);
            _rng = rng;
            _engine.OnWeatherCascadeEvaluatedSeam = (ev, effs) => OnCascadeEvaluated?.Invoke(ev, effs);
        }

        public WeatherGameplayCascadeEngine Engine => _engine;
        public WeatherCascadeState State => _engine.State;

        public int CurrentDay
        {
            get => _currentDay;
            set => _currentDay = value;
        }

        public int FortificationLevel
        {
            get => _fortificationLevel;
            set => _fortificationLevel = Math.Max(0, value);
        }

        public void BindWeatherSource(IWeatherCascadeSource source)
        {
            if (source == null) return;
            source.OnWeatherFrontArrived += (kind, severity, day) =>
            {
                TriggerCascade(kind, severity, day);
            };
        }

        public WeatherEvent TriggerCascade(WeatherKind kind, float severity, int day, IReadOnlyList<string>? regions = null)
        {
            _currentDay = day;
            return _engine.EvaluateWeatherCascade(kind, severity, day, regions, _fortificationLevel, _rng);
        }

        public void TickDay(int newDay)
        {
            _currentDay = newDay;
            _engine.TickDay(newDay);
        }

        public WeatherCascadeState CaptureState() => _engine.CaptureState();

        /// <summary>
        /// Restores cascade state through the engine. A wrong schema version
        /// throws (the host catches, journals, and keeps live state);
        /// <c>null</c> resets to an empty, unbounded-cascade state.
        /// </summary>
        public void RestoreState(WeatherCascadeState? state) => _engine.RestoreState(state);
    }
}
