// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 46 / C2[20] host probe.
//
// --playable-metrics-selftest proves, headlessly, the local measurement stack
// the host binds to canonical producers: the bounded recorder stream, the
// first-hour funnel steps, JSONL serialization, aggregation grading and the
// capture/restore parity of the persisted report.
// ============================================================================
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Telemetry;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunPlayMetricsSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; GD.Print($"[PASS] playable-metrics/{gate}"); }
                else { fail++; GD.Print($"[FAIL] playable-metrics/{gate}{(note.Length > 0 ? " — " + note : "")}"); }
            }

            // 1 — session start binds campaign identity to the recorder.
            var session = new PlayMetricsHostSession("probe_session", "probe");
            session.BeginSession(4242, "difficulty_standard");
            Check("session_context", session.Recorder.Seed == 4242 && session.Recorder.PresetId == "difficulty_standard");

            // 2 — bounded buffer: never grows past Core's cap.
            for (int i = 0; i < PlaySessionRecorder.MaxBufferedEvents + 25; i++)
            {
                session.RecordAction("panel_opened", "panel_" + (i % 7), "ok", 1 + (i / 5));
            }
            Check("bounded_buffer", session.Recorder.BufferedCount == PlaySessionRecorder.MaxBufferedEvents,
                session.Recorder.BufferedCount.ToString());

            // 3 — canonical onboarding sigils drive the funnel.
            session.RecordSigil("protocol.ration", 1);
            session.RecordSigil("inventory.used", 1);
            session.RecordSigil("weather.read", 2);
            Check("funnel_sigil_steps",
                session.Funnel.IsStepCompleted("guidance_opened")
                && session.Funnel.IsStepCompleted("first_craft")
                && session.Funnel.IsStepCompleted("first_storm_survived"));

            // 4 — day advances arrive as day_join rows and complete their step.
            for (int day = 1; day <= 3; day++) session.RecordDayJoined(day, "campaign_day_coordinator", "day_advanced", 4 + day);
            Check("funnel_day_step", session.Funnel.IsStepCompleted("first_day_past_tutorial"));
            Check("day_join_peak_population", session.PeakPopulation == 7, session.PeakPopulation.ToString());

            // 5 — expedition completion feeds the harvest read model.
            session.RecordExpeditionReturned(12, 3);
            Check("harvest_read_model", session.HarvestedResourceCount == 12);

            // 6 — aggregation grades are deterministic and documented.
            var strong = session.Aggregate(new SessionMetricInputs(
                daysSurvived: 60, peakPopulation: 8, casualtiesCount: 1, totalScavengeSorties: 25,
                totalResourcesHarvested: 200, totalWaterPurifiedLiters: 400, crisesResolved: 4,
                crisesFailed: 0, difficultyScalarPermille: 1000));
            Check("aggregate_deterministic", session.Aggregate(new SessionMetricInputs(
                60, 8, 1, 25, 200, 400, 4, 0, 1000)).HardshipIndexPermille == strong.HardshipIndexPermille);
            Check("aggregate_no_critical_failure", !strong.HasCriticalFailure, strong.SummaryDescription);

            var collapsed = session.Aggregate(new SessionMetricInputs(
                daysSurvived: 2, peakPopulation: 1, casualtiesCount: 12, totalScavengeSorties: 0,
                totalResourcesHarvested: 0, totalWaterPurifiedLiters: 0, crisesResolved: 0,
                crisesFailed: 5, difficultyScalarPermille: 1670));
            Check("aggregate_critical_failure_flagged", collapsed.HasCriticalFailure || collapsed.Grade >= strong.Grade,
                $"{collapsed.Grade} vs {strong.Grade}");

            // 7 — JSONL serialization of a drained event round-trips its identity.
            var drained = session.DrainEvents();
            Check("drain_non_empty", drained.Count > 0);
            string jsonl = session.Recorder.ToJsonLine(drained[0]);
            var reparsed = System.Text.Json.JsonSerializer.Deserialize<PlaySessionEvent>(jsonl);
            Check("jsonl_roundtrip", reparsed != null && reparsed.SessionId == "probe_session");

            // 8 — capture/restore parity of the persisted report + funnel.
            var captured = session.CaptureState();
            var restoredSession = new PlayMetricsHostSession("probe_session", "probe");
            restoredSession.RestoreState(captured);
            Check("restore_funnel_preserved",
                restoredSession.Funnel.CompletedStepCount == session.Funnel.CompletedStepCount
                && restoredSession.Funnel.IsStepCompleted("guidance_opened"));
            Check("restore_report_preserved",
                restoredSession.LastAggregate.HasValue
                && restoredSession.LastAggregate.Value.Grade == session.LastAggregate!.Value.Grade
                && restoredSession.AggregatedDayCount == session.AggregatedDayCount);

            // 9 — save store round-trip through the canonical checksummed store.
            Check("save_store_roundtrip", PlayMetricsSaveStore.TrySave(captured));
            var reloaded = PlayMetricsSaveStore.TryLoad();
            Check("save_store_reload", reloaded != null && reloaded.funnel_completed_steps.Count == captured.funnel_completed_steps.Count);

            GD.Print($"[playable-metrics-selftest] {pass} passed, {fail} failed");
            return fail == 0 ? 0 : 1;
        }
    }
}
