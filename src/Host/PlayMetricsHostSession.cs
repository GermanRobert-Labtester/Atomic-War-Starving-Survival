// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 46 / C2[20] — Playable metrics, first-hour funnel & measurement-driven
// tuning host adapter.
//
// Authority boundary (deliberate): PlaySessionRecorder remains the sole session
// event authority (bounded buffer, JSONL serialization, typed events);
// FirstHourFunnel remains the pure evaluator of the seven canonical onboarding
// steps; PlayableMetricsAggregationEngine remains the pure readiness scoring
// authority. This session invents no balance rule. It binds those three Core
// authorities to canonical host events and keeps the durable artifacts the plan
// requires: the aggregate report and the funnel completion map.
//
// Aggregation inputs are read from existing gameplay owners, never duplicated:
//   days survived        -> campaign day
//   peak population      -> living roster count observed at each day advance
//   casualties           -> SurvivorFateSystem death count
//   scavenge sorties     -> ExpeditionSystem completed count
//   water purified       -> WaterTreatmentSystem total produced
//   crises resolved      -> DisasterResponseSystem resolved count
//   crises failed        -> DisasterResponseSystem disasters overdue + unresolved
//   difficulty scalar    -> derived (in this adapter) from the canonical
//                          DifficultyScalarsProvider survival-pressure scalars;
//                          the standard preset maps to exactly 1000 permille.
// No canonical lifetime "total resources harvested" counter exists in Core, so
// that input is read from delivered expedition loot lines through the recorder
// stream and named honestly below rather than shadowing an inventory counter.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Telemetry;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host persistence projection for Plan 46. The recorder's event buffer is
    /// deliberately transient (bounded, drains to JSONL); the durable artifact
    /// is the last aggregated readiness report plus funnel completion times.
    /// </summary>
    public sealed class PlayMetricsSaveState
    {
        public int schema_version { get; set; } = 1;
        public string session_id { get; set; } = string.Empty;
        public int aggregated_day_count { get; set; }
        public string last_grade { get; set; } = string.Empty;
        public int last_hardship_index_permille { get; set; }
        public int last_efficiency_rating_permille { get; set; }
        public int last_survival_stability_permille { get; set; }
        public bool last_has_critical_failure { get; set; }
        public string last_summary { get; set; } = string.Empty;
        public Dictionary<string, long> funnel_completed_steps { get; set; } = new(StringComparer.Ordinal);
    }

    public sealed class PlayMetricsHostSession : HostSessionBase
    {
        public PlaySessionRecorder Recorder { get; }
        public FirstHourFunnel Funnel { get; }

        /// <summary>Last aggregated readiness report produced in this session.</summary>
        public AggregatedMetricsResult? LastAggregate { get; private set; }

        /// <summary>Campaign days whose metrics have been aggregated.</summary>
        public int AggregatedDayCount { get; private set; }

        /// <summary>Peak living population observed across recorded day advances.</summary>
        public int PeakPopulation { get; private set; } = 1;

        /// <summary>Delivered expedition loot lines (the recorder-side harvest proxy).</summary>
        public int HarvestedResourceCount { get; private set; }

        public PlayMetricsHostSession(string sessionId, string buildVersion = "1.0.0", FirstHourFunnel? funnel = null)
        {
            if (string.IsNullOrWhiteSpace(sessionId)) sessionId = "local_session";
            Recorder = new PlaySessionRecorder(sessionId, string.IsNullOrWhiteSpace(buildVersion) ? "1.0.0" : buildVersion);
            Funnel = funnel ?? new FirstHourFunnel();
            Recorder.PlaySessionEventRecordedSeam += OnEventRecorded;
        }

        private void OnEventRecorded(PlaySessionEvent evt)
        {
            if (evt == null) return;
            if (Funnel.ProcessEvent(evt)) RaiseStateChanged();
        }

        /// <summary>Bind campaign identity to the recorder (seed + difficulty preset).</summary>
        public void BeginSession(long seed, string presetId)
        {
            Recorder.SetContext(seed, string.IsNullOrWhiteSpace(presetId) ? DifficultyScalarsFallbackPresetId : presetId);
            RecordAction(PlaySessionActions.SessionStart, "campaign", "ok", 1);
        }

        private const string DifficultyScalarsFallbackPresetId = "difficulty_standard";

        public void RecordAction(string action, string targetId = "", string outcome = "ok", int day = 1, long tSessionMs = 0)
        {
            Recorder.Record(action, targetId, outcome, day, tSessionMs);
            RaiseStateChanged();
        }

        /// <summary>Record one of the canonical onboarding/UI sigils.</summary>
        public void RecordSigil(string sigil, int day = 1, long tSessionMs = 0)
        {
            if (string.IsNullOrWhiteSpace(sigil)) return;
            Recorder.RecordSigil(sigil, day, tSessionMs);
            RaiseStateChanged();
        }

        /// <summary>Record one canonical campaign day advance as a JSONL row.</summary>
        public void RecordDayJoined(
            int day,
            string ownerId,
            string kind,
            int livingPopulation,
            string primaryId = "",
            string secondaryId = "",
            float numeric = 0f,
            long tSessionMs = 0)
        {
            Recorder.JoinDay(Math.Max(1, day), 0L, ownerId, kind, primaryId, secondaryId, numeric);
            ObservePopulation(livingPopulation);
            RaiseStateChanged();
        }

        /// <summary>Observe the living roster count for the peak-population read model.</summary>
        public void ObservePopulation(int living)
        {
            if (living > PeakPopulation) PeakPopulation = living;
        }

        /// <summary>Record delivered loot units from a completed expedition.</summary>
        public void RecordExpeditionReturned(int lootUnits, int day = 1, long tSessionMs = 0)
        {
            if (lootUnits > 0) HarvestedResourceCount += lootUnits;
            Recorder.Record("expedition.returned", "expedition", "ok", day, tSessionMs);
            RaiseStateChanged();
        }

        public void RecordSaveOutcome(int day, long tSessionMs = 0)
            => RecordAction(PlaySessionActions.Save, "campaign", "ok", day, tSessionMs);

        public void RecordQuit(int day, long tSessionMs = 0)
            => RecordAction(PlaySessionActions.Quit, "campaign", "quit", day, tSessionMs);

        /// <summary>
        /// Aggregate the session through the pure engine and persist the report.
        /// </summary>
        public AggregatedMetricsResult Aggregate(SessionMetricInputs inputs)
        {
            var result = PlayableMetricsAggregationEngine.Evaluate(inputs);
            LastAggregate = result;
            AggregatedDayCount = inputs.DaysSurvived;
            RaiseStateChanged();
            return result;
        }

        /// <summary>
        /// Drill from saved metrics for the sessions that never reached an endgame.
        /// </summary>
        public void SampleDay(int day, int livingPopulation)
        {
            ObservePopulation(livingPopulation);
            AggregatedDayCount = Math.Max(AggregatedDayCount, day);
        }

        /// <summary>Buffered events for the JSONL drain on session end.</summary>
        public IReadOnlyList<PlaySessionEvent> DrainEvents() => Recorder.Drain();

        public PlayMetricsSaveState CaptureState()
        {
            var state = new PlayMetricsSaveState
            {
                schema_version = 1,
                session_id = Recorder.SessionId,
                aggregated_day_count = AggregatedDayCount
            };

            if (LastAggregate.HasValue)
            {
                var r = LastAggregate.Value;
                state.last_grade = r.Grade.ToString();
                state.last_hardship_index_permille = r.HardshipIndexPermille;
                state.last_efficiency_rating_permille = r.EfficiencyRatingPermille;
                state.last_survival_stability_permille = r.SurvivalStabilityScorePermille;
                state.last_has_critical_failure = r.HasCriticalFailure;
                state.last_summary = r.SummaryDescription ?? string.Empty;
            }

            foreach (var pair in Funnel.CompletedSteps)
                state.funnel_completed_steps[pair.Key] = pair.Value;

            return state;
        }

        public void RestoreState(PlayMetricsSaveState? state)
        {
            if (state == null) return;
            AggregatedDayCount = Math.Max(0, state.aggregated_day_count);
            Funnel.RestoreCompletedSteps(state.funnel_completed_steps);
            // The aggregate report is copied into the live session so an endgame
            // without fresh aggregation still reports the last measured run.
            if (!string.IsNullOrWhiteSpace(state.last_grade))
            {
                LastAggregate = new AggregatedMetricsResult(
                    (Ashfall.Core.Telemetry.SessionReadinessGrade)Enum.Parse(
                        typeof(Ashfall.Core.Telemetry.SessionReadinessGrade), state.last_grade),
                    state.last_hardship_index_permille,
                    state.last_efficiency_rating_permille,
                    state.last_survival_stability_permille,
                    state.last_has_critical_failure,
                    state.last_summary ?? string.Empty);
            }
            RaiseStateChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            if (PlayMetricsSaveStore.TrySave(CaptureState()))
                base.Save();
        }

        public override void Dispose()
        {
            if (Recorder != null)
            {
                Recorder.PlaySessionEventRecordedSeam -= OnEventRecorded;
            }
            base.Dispose();
        }
    }

    public static class PlayMetricsSaveStore
    {
        public const string FileName = "playable_metrics_save.json";
        public const string SectionName = "playable_metrics";

        private static readonly SaveStore<PlayMetricsSaveState> s_store =
            SaveStoreHub.Checksummed<PlayMetricsSaveState>(FileName, nameof(PlayMetricsSaveStore));

        public static bool TrySave(PlayMetricsSaveState state) => s_store.TrySave(state);
        public static PlayMetricsSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(PlayMetricsSaveState state) => s_store.CapturePersisted(state);
        public static PlayMetricsSaveState? TryRestore(string json) => s_store.RestoreBare(json);
    }

    /// <summary>
    /// Host aggregation inputs gathered from the current campaign owners.
    /// </summary>
    public readonly struct PlayMetricsAggregationInputs
    {
        public int DaysSurvived { get; }
        public int PeakPopulation { get; }
        public int CasualtiesCount { get; }
        public int TotalScavengeSorties { get; }
        public int TotalResourcesHarvested { get; }
        public int TotalWaterPurifiedLiters { get; }
        public int CrisesResolved { get; }
        public int CrisesFailed { get; }
        public int DifficultyScalarPermille { get; }

        public PlayMetricsAggregationInputs(
            int daysSurvived,
            int peakPopulation,
            int casualtiesCount,
            int totalScavengeSorties,
            int totalResourcesHarvested,
            int totalWaterPurifiedLiters,
            int crisesResolved,
            int crisesFailed,
            int difficultyScalarPermille)
        {
            DaysSurvived = daysSurvived;
            PeakPopulation = peakPopulation;
            CasualtiesCount = casualtiesCount;
            TotalScavengeSorties = totalScavengeSorties;
            TotalResourcesHarvested = totalResourcesHarvested;
            TotalWaterPurifiedLiters = totalWaterPurifiedLiters;
            CrisesResolved = crisesResolved;
            CrisesFailed = crisesFailed;
            DifficultyScalarPermille = difficultyScalarPermille;
        }

        public SessionMetricInputs ToEngineInputs() => new SessionMetricInputs(
            DaysSurvived,
            PeakPopulation,
            CasualtiesCount,
            TotalScavengeSorties,
            TotalResourcesHarvested,
            TotalWaterPurifiedLiters,
            CrisesResolved,
            CrisesFailed,
            DifficultyScalarPermille);
    }
}
