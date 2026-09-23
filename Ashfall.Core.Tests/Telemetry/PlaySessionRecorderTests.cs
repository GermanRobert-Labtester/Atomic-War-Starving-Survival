// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using Ashfall.Core.Telemetry;
using Xunit;

namespace Ashfall.Core.Tests.Telemetry
{
    public sealed class PlaySessionRecorderTests
    {
        [Fact]
        public void Record_CapturesAction_PreservesMonotonicTimeAndContext()
        {
            var recorder = new PlaySessionRecorder("sess_12345", "0.9.1");
            recorder.SetContext(987654321L, "difficulty_austere");

            PlaySessionEvent? capturedSeamEvent = null;
            recorder.PlaySessionEventRecordedSeam = evt => capturedSeamEvent = evt;

            var evt = recorder.Record(PlaySessionActions.PanelOpened, "roster_panel", "ok", day: 2, tSessionMs: 45000);

            Assert.Equal("sess_12345", evt.SessionId);
            Assert.Equal("0.9.1", evt.BuildVersion);
            Assert.Equal(987654321L, evt.Seed);
            Assert.Equal("difficulty_austere", evt.PresetId);
            Assert.Equal(2, evt.Day);
            Assert.Equal(45000, evt.TSessionMs);
            Assert.Equal(PlaySessionActions.PanelOpened, evt.Action);
            Assert.Equal("roster_panel", evt.TargetId);
            Assert.Equal(1, recorder.BufferedCount);
            Assert.NotNull(capturedSeamEvent);
            Assert.Equal("roster_panel", capturedSeamEvent.TargetId);
        }

        [Fact]
        public void RecordSigil_MapsToSigilAction()
        {
            var recorder = new PlaySessionRecorder("sess_test");
            var evt = recorder.RecordSigil("protocol.ration", day: 1, tSessionMs: 12000);

            Assert.Equal(PlaySessionActions.Sigil, evt.Action);
            Assert.Equal("protocol.ration", evt.Sigil);
            Assert.Equal("observed", evt.Outcome);
        }

        [Fact]
        public void JoinDay_StoresSimulationConsequenceEvent()
        {
            var recorder = new PlaySessionRecorder("sess_day_test");
            var evt = recorder.JoinDay(
                day: 5,
                tSessionMs: 120000,
                ownerId: "needs_system",
                kind: "survivor_starving",
                primaryId: "dweller_10",
                secondaryId: "hunger",
                numeric: 95.5f
            );

            Assert.Equal("day_join", evt.RecordType);
            Assert.Equal("needs_system", evt.SourceOwnerId);
            Assert.Equal("survivor_starving", evt.Kind);
            Assert.Equal("dweller_10", evt.PrimaryId);
            Assert.Equal(95.5f, evt.Numeric);
        }

        [Fact]
        public void Drain_EmptiesBufferAndReturnsAllEvents()
        {
            var recorder = new PlaySessionRecorder("sess_drain");
            recorder.Record(PlaySessionActions.SessionStart);
            recorder.Record(PlaySessionActions.Consume, "ration_pack");
            recorder.Record(PlaySessionActions.SessionEnd);

            Assert.Equal(3, recorder.BufferedCount);

            var drained = recorder.Drain();
            Assert.Equal(3, drained.Count);
            Assert.Equal(0, recorder.BufferedCount);
        }

        [Fact]
        public void ToJsonLine_SerializesValidJson()
        {
            var recorder = new PlaySessionRecorder("sess_json");
            var evt = recorder.Record(PlaySessionActions.Save, "slot_1");

            var jsonLine = recorder.ToJsonLine(evt);
            Assert.False(string.IsNullOrWhiteSpace(jsonLine));

            using var doc = JsonDocument.Parse(jsonLine);
            var root = doc.RootElement;
            Assert.Equal(1, root.GetProperty("schema_version").GetInt32());
            Assert.Equal("sess_json", root.GetProperty("session_id").GetString());
            Assert.Equal("save", root.GetProperty("action").GetString());
            Assert.Equal("slot_1", root.GetProperty("target_id").GetString());
        }

        [Fact]
        public void FirstHourFunnel_TracksProgressionAcrossSigilsAndActions()
        {
            var funnel = new FirstHourFunnel();
            var recorder = new PlaySessionRecorder("sess_funnel");

            var e1 = recorder.RecordSigil("protocol.ration", 1, 5000);
            var e2 = recorder.Record(PlaySessionActions.Consume, "inventory.used", "ok", 1, 10000);
            var e3 = recorder.RecordSigil("expedition.dispatched", 1, 15000);

            Assert.True(funnel.ProcessEvent(e1));
            Assert.True(funnel.ProcessEvent(e2));
            Assert.True(funnel.ProcessEvent(e3));

            Assert.True(funnel.IsStepCompleted("guidance_opened"));
            Assert.True(funnel.IsStepCompleted("first_craft"));
            Assert.True(funnel.IsStepCompleted("first_dispatch"));
            Assert.False(funnel.IsStepCompleted("first_death_witnessed"));
            Assert.Equal(3, funnel.CompletedStepCount);
        }

        [Fact]
        public void PlaySessionReport_AggregatesHistogramAndFunnelMetrics()
        {
            var recorder = new PlaySessionRecorder("sess_report");
            recorder.Record(PlaySessionActions.SessionStart, "", "ok", 1, 0);
            recorder.Record(PlaySessionActions.PanelOpened, "status_panel", "ok", 1, 500);
            recorder.Record(PlaySessionActions.PanelOpened, "roster_panel", "ok", 1, 1500);
            recorder.Record(PlaySessionActions.PanelOpened, "status_panel", "ok", 1, 2500);
            recorder.RecordSigil("protocol.ration", 1, 3000);
            recorder.JoinDay(2, 60000, "weather", "storm_passed");

            var events = recorder.Drain();
            var report = PlaySessionReport.Aggregate(events);

            Assert.Equal(6, report.TotalEvents);
            Assert.Equal(5, report.ActionEventsCount);
            Assert.Equal(1, report.DayJoinEventsCount);
            Assert.Equal(2, report.MaxDayReached);
            Assert.Equal(3, report.ActionHistogram[PlaySessionActions.PanelOpened]);
            Assert.Equal(2, report.PanelsOpened["status_panel"]);
            Assert.Equal(1, report.PanelsOpened["roster_panel"]);
            Assert.Equal(2, report.FunnelCompletedCount); // "protocol.ration" (guidance_opened) + JoinDay "day_advanced" (first_day_past_tutorial)

            var markdown = report.ToMarkdown();
            Assert.Contains("ASHFALL Local Play Session Telemetry Report", markdown);
            Assert.Contains("status_panel", markdown);
            Assert.Contains("**Total Events Recorded:** 6", markdown);
        }
    }
}
