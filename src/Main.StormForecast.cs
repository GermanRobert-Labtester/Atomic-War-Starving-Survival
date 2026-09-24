// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Expansion 33 — The Weather: storm forecast readiness host wiring.
// The signed pure StormForecastReadinessEngine (DEC-87) is the forecast/readiness
// authority. Weather itself stays with WeatherSystem and WeatherGate; this host
// owns only the observation-post skill, drill recency, and warning counters.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        /// <summary>Authored instrument quality fed to the daily calibration-drift tick.</summary>
        public const int StormForecastInstrumentQualityPermille = 600;

        private StormForecastHostSession? _stormForecast;
        private bool _stormForecastDirty;

        public StormForecastHostSession? StormForecast => _stormForecast;

        public void SetupStormForecast()
        {
            if (_stormForecast != null) return;

            var saved = StormForecastSaveStore.TryLoad();
            _stormForecast = StormForecastHostSession.Create(saved);
            _stormForecast.StateChanged += () => _stormForecastDirty = true;
        }

        /// <summary>Daily owner tick: observation-post calibration drift and drill-recency decay.</summary>
        public void TickStormForecast(int day)
        {
            SetupStormForecast();
            _stormForecast?.TickDay(day, StormForecastInstrumentQualityPermille);
        }

        public StormForecastResult IssueStormForecast(int leadTimeHours, StormSeverityClass severity, int forecastSeed)
        {
            SetupStormForecast();
            return _stormForecast?.EvaluateForecast(leadTimeHours, severity, forecastSeed)
                ?? StormForecastReadinessEngine.EvaluateForecastReliability(700, leadTimeHours, severity, forecastSeed);
        }

        /// <summary>Issues a public warning when the forecast qualifies; returns true when issued.</summary>
        public bool PublishStormWarning(StormForecastResult forecast)
        {
            SetupStormForecast();
            return _stormForecast?.IssueWarning(forecast) ?? false;
        }

        /// <summary>
        /// Read-only readiness assessment. Inputs are supplied by their canonical
        /// owners (shelter seal, filter stock, medical bay, drill recency).
        /// </summary>
        public SeasonalReadinessResult AssessStormReadiness(
            int sealedAirlockPermille, int filterStockPermille, int medicalReadinessPermille,
            int drillRecencyPermille, StormSeverityClass targetSeverity)
        {
            SetupStormForecast();
            return _stormForecast?.AssessReadiness(sealedAirlockPermille, filterStockPermille, medicalReadinessPermille, drillRecencyPermille, targetSeverity)
                ?? StormForecastReadinessEngine.AssessSeasonalReadiness(sealedAirlockPermille, filterStockPermille, medicalReadinessPermille, drillRecencyPermille, targetSeverity);
        }

        /// <summary>Read-only readiness using only this host's drill recency and the supplied seal/filter/medical facts.</summary>
        public SeasonalReadinessResult AssessStormReadiness(
            int sealedAirlockPermille, int filterStockPermille, int medicalReadinessPermille, StormSeverityClass targetSeverity)
        {
            SetupStormForecast();
            int drill = _stormForecast?.Ledger.DrillRecencyPermille ?? 0;
            return AssessStormReadiness(sealedAirlockPermille, filterStockPermille, medicalReadinessPermille, drill, targetSeverity);
        }

        public void RunStormResponseDrill()
        {
            SetupStormForecast();
            _stormForecast?.RunDrill();
        }

        public void AddStormObservationSkill(int delta)
        {
            SetupStormForecast();
            _stormForecast?.AddObservationSkill(delta);
        }

        public StormForecastCensus GetStormForecastCensus() => _stormForecast?.Census ?? default;

        public void SaveStormForecast()
        {
            if (_stormForecast == null) return;
            var state = _stormForecast.CaptureState();
            StormForecastSaveStore.TrySave(state);
            if (CaptureSection(StormForecastSaveStore.SectionName, StormForecastSaveStore.TryCapturePersisted(state)))
                _stormForecastDirty = false;
        }

        public void FlushStormForecastIfDirty()
        {
            if (_stormForecastDirty)
                SaveStormForecast();
        }

        public void ResetStormForecast()
        {
            _stormForecast = null;
            _stormForecastDirty = false;
        }
    }
}
