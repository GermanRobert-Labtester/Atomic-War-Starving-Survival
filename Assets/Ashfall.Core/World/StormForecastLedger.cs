// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 33 — The Weather
// Subsystem    : Storm Forecast Ledger (stateful owner over the signed pure
//                StormForecastReadinessEngine, DEC-87)
// ============================================================================
using System;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Persisted observation-post and shelter-readiness ledger. It owns only the
    /// forecast skill / drill / warning counters; the weather itself remains owned
    /// by WeatherSystem, and readiness inputs are supplied by their canonical owners.
    /// </summary>
    [Serializable]
    public sealed class StormForecastState
    {
        public int SchemaVersion { get; set; } = 1;
        public int Season { get; set; }
        public int ObservationPostSkillPermille { get; set; } = 700;
        public int DrillRecencyPermille { get; set; }
        public int WarningsIssued { get; set; }
        public int LastForecastDay { get; set; } = -1;

        public StormForecastState Clone() => new StormForecastState
        {
            SchemaVersion = SchemaVersion,
            Season = Season,
            ObservationPostSkillPermille = ObservationPostSkillPermille,
            DrillRecencyPermille = DrillRecencyPermille,
            WarningsIssued = WarningsIssued,
            LastForecastDay = LastForecastDay
        };
    }

    /// <summary>Bounded read model of the storm forecast ledger.</summary>
    public struct StormForecastCensus
    {
        public int ObservationPostSkillPermille { get; }
        public int DrillRecencyPermille { get; }
        public int WarningsIssued { get; }
        public ForecastConfidenceTier LastTier { get; }
        public int Season { get; }

        public StormForecastCensus(int observationPostSkillPermille, int drillRecencyPermille, int warningsIssued, ForecastConfidenceTier lastTier, int season)
        {
            ObservationPostSkillPermille = observationPostSkillPermille;
            DrillRecencyPermille = drillRecencyPermille;
            WarningsIssued = warningsIssued;
            LastTier = lastTier;
            Season = season;
        }
    }

    /// <summary>
    /// Owns the observation-post skill, drill recency, and warning counters, and
    /// delegates all forecast/readiness arithmetic to the signed pure
    /// <see cref="StormForecastReadinessEngine"/>. Weather authority is untouched.
    /// </summary>
    public sealed class StormForecastLedger
    {
        private readonly StormForecastState _state;
        private ForecastConfidenceTier _lastTier = ForecastConfidenceTier.Unknown;

        public StormForecastLedger(StormForecastState? state = null)
        {
            _state = state ?? new StormForecastState();
        }

        public int ObservationPostSkillPermille => _state.ObservationPostSkillPermille;
        public int DrillRecencyPermille => _state.DrillRecencyPermille;
        public int WarningsIssued => _state.WarningsIssued;
        public int Season => _state.Season;
        public ForecastConfidenceTier LastTier => _lastTier;

        public StormForecastState CaptureState() => _state.Clone();

        public void RestoreState(StormForecastState? saved)
        {
            if (saved == null) return;
            if (saved.SchemaVersion > _state.SchemaVersion)
                throw new InvalidOperationException(
                    $"storm forecast schema {saved.SchemaVersion} is newer than supported {_state.SchemaVersion}.");

            _state.SchemaVersion = saved.SchemaVersion <= 0 ? _state.SchemaVersion : saved.SchemaVersion;
            _state.Season = saved.Season;
            _state.ObservationPostSkillPermille = Math.Clamp(saved.ObservationPostSkillPermille, 0, 1000);
            _state.DrillRecencyPermille = Math.Clamp(saved.DrillRecencyPermille, 0, 1000);
            _state.WarningsIssued = Math.Max(0, saved.WarningsIssued);
            _state.LastForecastDay = saved.LastForecastDay;
        }

        /// <summary>
        /// Advances the daily observation-post calibration drift and drill-recency
        /// decay. Deterministic; no RNG.
        /// </summary>
        public void TickDay(int day, int instrumentQualityPermille)
        {
            int decay = StormForecastReadinessEngine.CalculateObservationPostDecay(instrumentQualityPermille);
            _state.ObservationPostSkillPermille = Math.Max(0, _state.ObservationPostSkillPermille - decay);
            _state.DrillRecencyPermille = Math.Max(0, _state.DrillRecencyPermille - 50);
            _state.LastForecastDay = day;
        }

        /// <summary>Evaluates a forecast from the live observation-post skill.</summary>
        public StormForecastResult EvaluateForecast(int leadTimeHours, StormSeverityClass severity, int forecastSeed)
        {
            var result = StormForecastReadinessEngine.EvaluateForecastReliability(
                _state.ObservationPostSkillPermille, leadTimeHours, severity, forecastSeed);
            _lastTier = result.ConfidenceTier;
            return result;
        }

        /// <summary>Records that a public warning was issued for a forecast.</summary>
        public bool IssueWarning(StormForecastResult forecast)
        {
            if (!forecast.IssueWarning) return false;
            _state.WarningsIssued++;
            return true;
        }

        /// <summary>Read-only readiness assessment from canonical owner inputs.</summary>
        public SeasonalReadinessResult AssessReadiness(
            int sealedAirlockPermille, int filterStockPermille, int medicalReadinessPermille,
            int drillRecencyPermille, StormSeverityClass targetSeverity) =>
            StormForecastReadinessEngine.AssessSeasonalReadiness(
                sealedAirlockPermille, filterStockPermille, medicalReadinessPermille, drillRecencyPermille, targetSeverity);

        /// <summary>Runs a storm-response drill, resetting recency to full.</summary>
        public void RunDrill() => _state.DrillRecencyPermille = 1000;

        public void SetSeason(int season) => _state.Season = season;

        public void AddObservationSkill(int delta) =>
            _state.ObservationPostSkillPermille = Math.Clamp(_state.ObservationPostSkillPermille + delta, 0, 1000);

        public StormForecastCensus GetCensus() =>
            new StormForecastCensus(_state.ObservationPostSkillPermille, _state.DrillRecencyPermille, _state.WarningsIssued, _lastTier, _state.Season);

        public void Clear()
        {
            _state.ObservationPostSkillPermille = 700;
            _state.DrillRecencyPermille = 0;
            _state.WarningsIssued = 0;
            _lastTier = ForecastConfidenceTier.Unknown;
        }
    }
}
