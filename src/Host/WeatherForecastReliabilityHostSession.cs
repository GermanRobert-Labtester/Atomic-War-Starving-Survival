// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : WeatherForecastReliabilitySaveStore
// Core Engine: Ashfall.Core.World.WeatherForecastReliabilityEngine
// Host Caller: Main.WeatherForecastReliability
// Purpose    : Forecast reliability. The Core engine is the sole authority over
//              the reliability score, confidence grade, and dispatch-safety
//              verdict for a received forecast; the host owns only the per-day
//              received-forecast ledger and its persistence.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Save;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persisted record of one received forecast and the grade the Core engine gave it.
    /// </summary>
    [Serializable]
    public sealed class ForecastReliabilityEntry
    {
        public int receivedDay;
        public string forecastId = string.Empty;
        public int distanceKmFromStation;
        public int leadTimeDays;
        public int atmosphericInterferencePermille;
        public int stationCalibrationPermille;
        public int reliabilityScorePermille;
        public ForecastConfidenceGrade grade;
        public bool isReliableForDispatch;
    }

    [Serializable]
    public sealed class WeatherForecastReliabilitySaveState
    {
        public int schema_version = 1;
        public int currentDay;
        public List<ForecastReliabilityEntry> forecasts = new List<ForecastReliabilityEntry>();
    }

    public static class WeatherForecastReliabilitySaveStore
    {
        public const string FileName = "weather_forecast_reliability_save.json";
        public const string SectionName = "weather_forecast_reliability";

        private static readonly SaveStore<WeatherForecastReliabilitySaveState> s_store =
            SaveStoreHub.Checksummed<WeatherForecastReliabilitySaveState>(FileName, nameof(WeatherForecastReliabilitySaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(WeatherForecastReliabilitySaveState state) => s_store.CaptureBare(state);
        public static WeatherForecastReliabilitySaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(WeatherForecastReliabilitySaveState state) => s_store.TrySave(state);
        public static WeatherForecastReliabilitySaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Host session composing the pure Core forecast-reliability engine.</summary>
    public sealed class WeatherForecastReliabilityHostSession : HostSessionBase
    {
        private readonly WeatherForecastReliabilitySaveState _state;

        public WeatherForecastReliabilityHostSession(WeatherForecastReliabilitySaveState? state = null)
        {
            _state = state ?? new WeatherForecastReliabilitySaveState();
            _state.forecasts ??= new List<ForecastReliabilityEntry>();
        }

        public static WeatherForecastReliabilityHostSession Create(WeatherForecastReliabilitySaveState? state = null) =>
            new WeatherForecastReliabilityHostSession(state);

        public string LastEvent { get; private set; } = string.Empty;
        public int CurrentDay => _state.currentDay;
        public IReadOnlyList<ForecastReliabilityEntry> Forecasts => _state.forecasts;
        public int ForecastCount => _state.forecasts.Count;

        public ForecastReliabilityEntry? LatestForecast => _state.forecasts.Count > 0
            ? _state.forecasts[_state.forecasts.Count - 1]
            : null;

        /// <summary>
        /// Grades one received forecast. The Core engine decides reliability,
        /// confidence, and dispatch safety; the host only records the outcome.
        /// </summary>
        public ForecastReliabilityEntry ReceiveForecast(
            string forecastId,
            int distanceKmFromStation,
            int leadTimeDays,
            int atmosphericInterferencePermille,
            int stationCalibrationPermille,
            int day)
        {
            var result = WeatherForecastReliabilityEngine.EvaluateReliability(
                distanceKmFromStation,
                leadTimeDays,
                atmosphericInterferencePermille,
                stationCalibrationPermille);

            var entry = new ForecastReliabilityEntry
            {
                receivedDay = day,
                forecastId = forecastId ?? string.Empty,
                distanceKmFromStation = Math.Max(0, distanceKmFromStation),
                leadTimeDays = Math.Max(1, Math.Min(14, leadTimeDays)),
                atmosphericInterferencePermille = Math.Max(0, Math.Min(1000, atmosphericInterferencePermille)),
                stationCalibrationPermille = Math.Max(0, Math.Min(1000, stationCalibrationPermille)),
                reliabilityScorePermille = result.ReliabilityScorePermille,
                grade = result.Grade,
                isReliableForDispatch = result.IsReliableForDispatch
            };
            _state.forecasts.Add(entry);
            _state.currentDay = day;

            LastEvent = $"Forecast '{entry.forecastId}' graded {entry.grade} ({entry.reliabilityScorePermille} permille).";
            RaiseStateChanged();
            return entry;
        }

        public bool IsDispatchSafe() => LatestForecast?.isReliableForDispatch ?? false;

        public int DispatchableForecastCount
        {
            get
            {
                int n = 0;
                foreach (var f in _state.forecasts) if (f.isReliableForDispatch) n++;
                return n;
            }
        }

        public WeatherForecastReliabilitySaveState CaptureState()
        {
            var copy = new WeatherForecastReliabilitySaveState
            {
                schema_version = _state.schema_version,
                currentDay = _state.currentDay
            };
            foreach (var f in _state.forecasts)
            {
                copy.forecasts.Add(new ForecastReliabilityEntry
                {
                    receivedDay = f.receivedDay,
                    forecastId = f.forecastId,
                    distanceKmFromStation = f.distanceKmFromStation,
                    leadTimeDays = f.leadTimeDays,
                    atmosphericInterferencePermille = f.atmosphericInterferencePermille,
                    stationCalibrationPermille = f.stationCalibrationPermille,
                    reliabilityScorePermille = f.reliabilityScorePermille,
                    grade = f.grade,
                    isReliableForDispatch = f.isReliableForDispatch
                });
            }
            return copy;
        }

        public void RestoreState(WeatherForecastReliabilitySaveState state)
        {
            _state.currentDay = state?.currentDay ?? 0;
            _state.forecasts.Clear();
            if (state == null) return;
            _state.schema_version = state.schema_version;
            if (state.forecasts != null) _state.forecasts.AddRange(state.forecasts);
            LastEvent = "Restored forecast reliability state.";
            RaiseStateChanged();
        }

        public bool TrySave() => WeatherForecastReliabilitySaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = WeatherForecastReliabilitySaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
