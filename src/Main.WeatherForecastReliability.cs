// SPDX-License-Identifier: MIT
// ============================================================================
// Forecast reliability host composition. The Core WeatherForecastReliabilityEngine
// is the sole authority over the reliability score, confidence grade, and
// dispatch-safety verdict for a received forecast. The host owns only the
// received-forecast ledger and supplies the station facts (distance, lead time,
// interference, calibration) from the existing weather-station owner.
// ============================================================================

using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private WeatherForecastReliabilityHostSession? _forecast;
        private bool _forecastDirty;

        public WeatherForecastReliabilityHostSession? WeatherForecastReliabilitySession => _forecast;

        public void SetupWeatherForecastReliability()
        {
            if (_forecast != null) return;
            var saved = WeatherForecastReliabilitySaveStore.TryLoad();
            _forecast = WeatherForecastReliabilityHostSession.Create(saved);
            _forecast.StateChanged += () => _forecastDirty = true;
        }

        /// <summary>Grades a received forecast. Station facts come from the weather owner.</summary>
        public ForecastReliabilityEntry ReceiveWeatherForecast(
            string forecastId,
            int distanceKmFromStation,
            int leadTimeDays,
            int atmosphericInterferencePermille,
            int stationCalibrationPermille,
            int day)
        {
            SetupWeatherForecastReliability();
            var entry = _forecast!.ReceiveForecast(
                forecastId, distanceKmFromStation, leadTimeDays,
                atmosphericInterferencePermille, stationCalibrationPermille, day);
            _forecastDirty = true;
            return entry;
        }

        public bool IsForecastDispatchSafe()
        {
            SetupWeatherForecastReliability();
            return _forecast!.IsDispatchSafe();
        }

        public (int Forecasts, int Dispatchable) GetForecastReliabilityReadout()
        {
            SetupWeatherForecastReliability();
            return (_forecast!.ForecastCount, _forecast.DispatchableForecastCount);
        }

        public void SaveWeatherForecastReliability()
        {
            if (_forecast == null) return;
            var state = _forecast.CaptureState();
            if (CaptureSection(WeatherForecastReliabilitySaveStore.SectionName, WeatherForecastReliabilitySaveStore.TryCapturePersisted(state)))
                _forecastDirty = false;
        }

        public void ResetWeatherForecastReliability()
        {
            _forecast = null;
            _forecastDirty = false;
        }
    }
}
