// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : StormForecastSaveStore
// Core State : Ashfall.Core.World.StormForecastState
// Host Caller: Main.StormForecast
// Purpose    : Expansion 33 — observation-post skill, drill recency, and warning
//              counters. Weather authority remains with WeatherSystem.
// ============================================================================

using System;
using Ashfall.Core.Save;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static class StormForecastSaveStore
    {
        public const string FileName = "storm_forecast_save.json";
        public const string SectionName = "storm_forecast";

        private static readonly SaveStore<StormForecastState> s_store =
            SaveStoreHub.Checksummed<StormForecastState>(FileName, nameof(StormForecastSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(StormForecastState state) => s_store.CaptureBare(state);
        public static StormForecastState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(StormForecastState state) => s_store.TrySave(state);
        public static StormForecastState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Expansion 33 host session. Wraps the stateful <see cref="StormForecastLedger"/>
    /// over the signed pure <see cref="StormForecastReadinessEngine"/>. It owns the
    /// observation-post skill, drill recency, and warning counters — never the weather.
    /// </summary>
    public sealed class StormForecastHostSession : HostSessionBase
    {
        private readonly StormForecastLedger _ledger;

        public StormForecastLedger Ledger => _ledger;
        public StormForecastCensus Census => _ledger.GetCensus();
        public int ObservationPostSkillPermille => _ledger.ObservationPostSkillPermille;
        public int WarningsIssued => _ledger.WarningsIssued;
        public string LastEvent { get; private set; } = string.Empty;

        public StormForecastHostSession(StormForecastState? state = null)
        {
            _ledger = new StormForecastLedger(state);
        }

        public static StormForecastHostSession Create(StormForecastState? state = null) =>
            new StormForecastHostSession(state);

        public void TickDay(int day, int instrumentQualityPermille)
        {
            _ledger.TickDay(day, instrumentQualityPermille);
            RaiseStateChanged();
        }

        public StormForecastResult EvaluateForecast(int leadTimeHours, StormSeverityClass severity, int forecastSeed)
        {
            var result = _ledger.EvaluateForecast(leadTimeHours, severity, forecastSeed);
            LastEvent = $"Forecast {result.ConfidenceTier} ({result.ConfidencePermille}\u2030) for {severity}, lead {result.LeadTimeHours}h.";
            RaiseStateChanged();
            return result;
        }

        public bool IssueWarning(StormForecastResult forecast)
        {
            bool issued = _ledger.IssueWarning(forecast);
            if (issued)
            {
                LastEvent = $"Storm warning issued for {forecast.PredictedSeverity}.";
                RaiseStateChanged();
            }
            return issued;
        }

        public SeasonalReadinessResult AssessReadiness(
            int sealedAirlockPermille, int filterStockPermille, int medicalReadinessPermille,
            int drillRecencyPermille, StormSeverityClass targetSeverity) =>
            _ledger.AssessReadiness(sealedAirlockPermille, filterStockPermille, medicalReadinessPermille, drillRecencyPermille, targetSeverity);

        public void RunDrill()
        {
            _ledger.RunDrill();
            LastEvent = "Storm-response drill completed.";
            RaiseStateChanged();
        }

        public void SetSeason(int season) => _ledger.SetSeason(season);
        public void AddObservationSkill(int delta) => _ledger.AddObservationSkill(delta);

        public StormForecastState CaptureState() => _ledger.CaptureState();
        public void RestoreState(StormForecastState? state) => _ledger.RestoreState(state);
        public void Clear() => _ledger.Clear();
    }
}
