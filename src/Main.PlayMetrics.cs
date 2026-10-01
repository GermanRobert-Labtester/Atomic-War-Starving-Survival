// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 46 / C2[20] — Playable metrics & measurement-driven difficulty tuning.
//
// Host composition for Ashfall.Core.Telemetry:
//   Setup   — one PlayMetricsHostSession per campaign, funnel restored from the
//             registered "playable_metrics" section.
//   Tick    — every canonical day advance is recorded as a JSONL day_join row
//             (day, living population) and drives the first-hour funnel.
//   Events  — canonical producers feed the recorder: survivor death (fate
//             owner), rations policy change (economy owner), expedition
//             completion (expedition owner), save/quit, and the onboarding
//             sigil seam (ObserveSigil).
//   Verdict — the campaign's end snapshot is aggregated through the pure engine
//             and journaled; the report is persisted with the section.
//   Save    — "playable_metrics" section via SaveSectionRegistry + SaveStore.
//
// The recorder is an audit read model. It never feeds gameplay, never writes to
// the deterministic campaign state, and never reaches a network: the buffer is
// bounded in Core and drained to local JSONL text only.
// ============================================================================
using System;
using System.Globalization;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Difficulty;
using Ashfall.Core.Economy;
using Ashfall.Core.Onboarding;
using Ashfall.Core.PlayerCommand;
using Ashfall.Core.Shelter;
using Ashfall.Core.Telemetry;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private const string PlayMetricsBuildVersion = "1.0.0";

        private PlayMetricsHostSession? _playMetrics;
        private bool _playMetricsDirty;

        public PlayMetricsHostSession? PlayMetrics => _playMetrics;

        /// <summary>
        /// Constructs the playable-metrics audit session on first use. Guarded
        /// so both the fresh-game and restore paths share one construction
        /// path and every producer call lands on a live recorder.
        /// </summary>
        private PlayMetricsHostSession EnsurePlayMetrics()
        {
            if (_playMetrics != null) return _playMetrics;

            var session = new PlayMetricsHostSession("local_session", PlayMetricsBuildVersion);
            session.RestoreState(PlayMetricsSaveStore.TryLoad());
            session.StateChanged += () => _playMetricsDirty = true;
            // Local JSONL audit sink: every recorded event is appended to
            // user://play_metrics.jsonl for the first-hour funnel tool. The sink
            // never drains the recorder buffer, so the playable_metrics section
            // and end-of-campaign report keep their rows.
            session.Recorder.PlaySessionEventRecordedSeam += evt =>
                PlayMetricJsonlSink.AppendLine(session.Recorder.ToJsonLine(evt));

            _playMetrics = session;
            return _playMetrics;
        }

        private void SetupPlayMetrics()
        {
            var session = EnsurePlayMetrics();
            session.BeginSession(CampaignSeedForGeneration(), _difficultyPresetId);
            _playMetricsDirty = true;
        }

        private void SavePlayMetrics()
        {
            if (_playMetrics == null) return;
            CaptureSection(
                PlayMetricsSaveStore.SectionName,
                PlayMetricsSaveStore.TryCapturePersisted(_playMetrics.CaptureState()));
            _playMetricsDirty = false;
        }

        private void ResetPlayMetrics()
        {
            _playMetrics?.Dispose();
            _playMetrics = null;
            _playMetricsDirty = false;
        }

        // ── Canonical producers ────────────────────────────────────────────

        /// <summary>One canonical day advance: JSONL row + funnel + peak population.</summary>
        private void RecordPlayMetricsDayJoined(int day)
        {
            var session = EnsurePlayMetrics();
            int living = _survivors?.Roster?.LivingCount ?? (_survivors?.RosterState?.Count ?? 0);
            session.RecordDayJoined(
                Math.Max(1, day),
                "campaign_day_coordinator",
                "day_advanced",
                living,
                secondaryId: _difficultyPresetId);
            _playMetricsDirty = true;
        }

        private void RecordPlayMetricSurvivorPerished(string survivorId)
        {
            var session = EnsurePlayMetrics();
            session.RecordAction("survivor_perished", survivorId ?? string.Empty, "consequence", Math.Max(1, _simDay));
            _playMetricsDirty = true;
        }

        private void RecordPlayMetricSigil(string sigil)
        {
            if (string.IsNullOrWhiteSpace(sigil)) return;
            var session = EnsurePlayMetrics();
            session.RecordSigil(sigil, Math.Max(1, _simDay));
            _playMetricsDirty = true;
        }

        /// <summary>
        /// P002 — record the published first-hour verb (a
        /// <see cref="PlayerCommandCode"/> value) for the stage whose sigil was
        /// just observed. target_id carries the stage localization id so the
        /// funnel can attribute the verb to exactly one stage.
        /// </summary>
        private void RecordPlayMetricFirstHourVerb(string verb, string stageId)
        {
            if (string.IsNullOrWhiteSpace(verb)) return;
            var session = EnsurePlayMetrics();
            session.RecordAction(verb, stageId ?? string.Empty, "observed", Math.Max(1, _simDay));
            _playMetricsDirty = true;
        }

        /// <summary>
        /// P004 — record a hint presentation or dismissal for a stage. The
        /// action is one of <see cref="PlaySessionActions.HintShown"/> /
        /// <see cref="PlaySessionActions.HintDismissed"/>; target_id is the
        /// stage localization id, so hint→action drop-off is measurable.
        /// </summary>
        private void RecordPlayMetricHint(string action, string stageId)
        {
            if (string.IsNullOrWhiteSpace(action) || string.IsNullOrWhiteSpace(stageId)) return;
            var session = EnsurePlayMetrics();
            session.RecordAction(action, stageId, "observed", Math.Max(1, _simDay));
            _playMetricsDirty = true;
        }

        private void RecordPlayMetricRationPolicyChanged(RationTarget target)
        {
            if (target == null) return;
            var session = EnsurePlayMetrics();
            session.RecordAction("ration_policy_set", target.ResourceId ?? string.Empty, target.Tier.ToString(), Math.Max(1, _simDay));
            _playMetricsDirty = true;
        }

        private void RecordPlayMetricExpeditionReturned(int lootUnits)
        {
            var session = EnsurePlayMetrics();
            session.RecordExpeditionReturned(lootUnits, Math.Max(1, _simDay));
            _playMetricsDirty = true;
        }

        /// <summary>Plan 46 — observe a failed sortie in the play-session stream.</summary>
        private void RecordPlayMetricExpeditionFailed(string reason)
        {
            var session = EnsurePlayMetrics();
            session.RecordExpeditionFailed(reason, Math.Max(1, _simDay));
            _playMetricsDirty = true;
        }

        private void RecordPlayMetricsSessionEnded()
        {
            var session = EnsurePlayMetrics();
            session.RecordSaveOutcome(Math.Max(1, _simDay));
            session.RecordQuit(Math.Max(1, _simDay));
            _playMetricsDirty = true;

        }

        // ── Aggregation ────────────────────────────────────────────────────

        /// <summary>
        /// Difficulty scalar in permille derived from the canonical provider's
        /// survival-pressure scalars; difficulty_standard maps to exactly 1000.
        /// This is the single documented read-model mapping — the scalar
        /// authority itself stays the difficulty preset catalog.
        /// </summary>
        private static int DeriveDifficultyScalarPermille(DifficultyScalarsProvider provider)
        {
            if (provider == null) return 1000;
            double sum =
                provider.HungerMult + provider.ThirstMult + provider.RadiationMult +
                provider.DiseaseMult + provider.HostileEncounterMult;
            if (sum <= 0.0) return 1000;
            double permille = 1000.0 * (sum / 5.0);
            return Math.Clamp((int)Math.Round(permille, MidpointRounding.AwayFromZero), 500, 3000);
        }

        /// <summary>
        /// Aggregate the current session through the pure engine and journal the
        /// readiness verdict. Inputs come from the existing owners — no second
        /// counter is created for any of them.
        /// </summary>
        private void PublishPlayMetricsReport(int day, string context)
        {
            var session = EnsurePlayMetrics();

            int living = _survivors?.Roster?.LivingCount ?? (_survivors?.RosterState?.Count ?? 0);
            int crisesResolved = 0;
            int crisesFailed = 0;
            if (_disasterResponse != null)
            {
                foreach (var disaster in _disasterResponse.Disasters)
                {
                    if (disaster == null) continue;
                    if (disaster.Status == DisasterStatus.Resolved)
                    {
                        crisesResolved++;
                    }
                    else if (day > disaster.StartedDay + Math.Max(1, disaster.DurationDays))
                    {
                        // Overdue and never resolved by its owner.
                        crisesFailed++;
                    }
                }
            }

            var inputs = new PlayMetricsAggregationInputs(
                daysSurvived: Math.Max(1, day),
                peakPopulation: Math.Max(1, Math.Max(session.PeakPopulation, living)),
                casualtiesCount: _survivorFate?.DeathCount ?? 0,
                totalScavengeSorties: _expeditions?.Engine?.CompletedCount ?? 0,
                // No canonical lifetime harvest counter exists in Core; the
                // recorder-side delivered-loot read model is used and named.
                totalResourcesHarvested: session.HarvestedResourceCount,
                totalWaterPurifiedLiters: (int)(_waterTreatment?.System?.TotalWater ?? 0f),
                crisesResolved: crisesResolved,
                crisesFailed: crisesFailed,
                difficultyScalarPermille: DeriveDifficultyScalarPermille(_difficultyScalars));

            var report = session.Aggregate(inputs.ToEngineInputs());
            _playMetricsDirty = true;

            _journal?.TryAddRawEntry(
                "play_metrics_report",
                $"Session readiness {report.Grade.ToString().Replace("_", " ", StringComparison.Ordinal)} "
                + $"after {inputs.DaysSurvived} day(s) at {DeriveDifficultyScalarPermille(_difficultyScalars)} permille difficulty "
                + $"(hardship {report.HardshipIndexPermille}, efficiency {report.EfficiencyRatingPermille}, "
                + $"stability {report.SurvivalStabilityScorePermille}"
                + (report.HasCriticalFailure ? ", critical failure" : string.Empty)
                + $"). {context}. {report.SummaryDescription}",
                null!, Math.Max(1, day));
        }
    }
}
