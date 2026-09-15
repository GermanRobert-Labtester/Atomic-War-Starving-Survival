// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Tasks 9–12 Wave 3 — follow-up chaining contract tests.
    ///
    /// TEST CONTRACT: Follow-up transmissions are delayed authored radio events
    /// created by a qualifying parent-signal outcome. They are distinct from
    /// time-based message stages and must be scheduled, persisted, and
    /// deduplicated exactly once.
    /// </summary>
    public sealed class DistressFollowUpTests : CatalogTestBase
    {
        private const string GenuineQuest = "quest_distress_trapped_mechanic";   // freq_distress_88_3, deadline 5, survival 5
        private const string GenuineSignal = "freq_distress_88_3";
        private const string LegacyExpirySignal = "freq_distress_445_2";         // quest_distress_family_shelter, deadline 4
        private const string TrapQuest = "quest_distress_raider_trap";           // freq_distress_192_4, IsTrap
        private const string TrapSignal = "freq_distress_192_4";

        private sealed class Harness
        {
            public RadioDistressSystem Distress;
            public DistressRescueMissionManager Missions;
            public DistressFollowUpScheduler Scheduler;
            public List<(string Parent, SignalFollowUpDefinition FollowUp, int Day)> Fired = new();

            public Harness(params (string signalId, SignalFollowUpDefinition[] followUps)[] authored)
            {
                Distress = new RadioDistressSystem();
                foreach (var (signalId, followUps) in authored)
                {
                    var def = Distress.GetDefinition(signalId);
                    if (def == null)
                    {
                        def = new DistressSignalDefinition { FrequencyId = signalId, FrequencyMhzStr = "199.8" };
                        Distress.RegisterSignal(def);
                    }
                    def.FollowUpSignals.AddRange(followUps);
                }
                Missions = new DistressRescueMissionManager(null, Distress);
                Scheduler = new DistressFollowUpScheduler(Distress, Missions);
                Scheduler.BindToMissionEvents();
                Scheduler.OnFollowUpFired += (parent, followUp, day) => Fired.Add((parent, followUp, day));
            }
        }

        private static SignalFollowUpDefinition FollowUp(string id, string trigger, int delay, string text)
            => new SignalFollowUpDefinition { Id = id, TriggerCondition = trigger, DelayDays = delay, Text = text, Clarity = 0.9f };

        // ── Required: qualifying triggers, timing, content ────────────────────

        [Fact]
        public void SignalChainsIntoFollowUpAfterInitialContact()
        {
            var h = new Harness((GenuineSignal, new[] { FollowUp("fu_arrival", SignalFollowUpTriggers.Answered, 2, "We can see your team on the road.") }));

            h.Scheduler.SetDay(1);
            Assert.True(h.Missions.RecordSignalHeard(GenuineSignal, 1));
            Assert.True(h.Missions.RecordExpeditionDispatched(GenuineQuest, "exp_1"));
            Assert.Empty(h.Fired); // scheduled, not yet due

            h.Scheduler.TickDaily(2); // due day 3 — not yet
            Assert.Empty(h.Fired);
            h.Scheduler.TickDaily(3); // due
            Assert.Single(h.Fired);
            Assert.Equal(GenuineSignal, h.Fired[0].Parent);
            Assert.Equal("We can see your team on the road.", h.Fired[0].FollowUp.Text);
            Assert.Equal(3, h.Fired[0].Day);
        }

        [Fact]
        public void RescueSuccessSchedulesFollowUp()
        {
            var h = new Harness((GenuineSignal, new[] { FollowUp("fu_thanks", SignalFollowUpTriggers.RescueSuccess, 5, "The survivor sends word from the shelter.") }));

            h.Scheduler.SetDay(1);
            h.Missions.RecordSignalHeard(GenuineSignal, 1);
            h.Missions.RecordExpeditionDispatched(GenuineQuest, "exp_1");
            h.Scheduler.SetDay(2);
            h.Missions.RecordDestinationReached(GenuineQuest, 2); // TerminalRescued
            Assert.Empty(h.Fired);
            h.Scheduler.TickDaily(7); // day 2 + 5
            Assert.Single(h.Fired);
            Assert.Equal(7, h.Fired[0].Day);
        }

        [Fact]
        public void FollowUpContentDiffersFromInitial()
        {
            var h = new Harness((GenuineSignal, new[] { FollowUp("fu_distinct", SignalFollowUpTriggers.Answered, 1, "Generator is failing again — please hurry.") }));

            // The builtin fixture signal has no authored fragments; give it one
            // so the "differs from the initial transmission" comparison is real.
            var def = h.Distress.GetDefinition(GenuineSignal)!;
            def.MessageFragments.Add(new DistressMessageFragment { Day = 1, Clarity = 0.3f, Text = "Original distress plea." });

            h.Scheduler.SetDay(1);
            h.Missions.RecordSignalHeard(GenuineSignal, 1);
            h.Missions.RecordExpeditionDispatched(GenuineQuest, "exp_1");
            h.Scheduler.TickDaily(2);

            var initial = def.MessageFragments;
            Assert.Single(h.Fired);
            // Semantic payload differs: the follow-up text is its own authored
            // payload with a stable identity — not a replay of any stage text.
            Assert.DoesNotContain(h.Fired[0].FollowUp.Text, initial.Select(f => f.Text), StringComparer.Ordinal);
            Assert.NotEqual(initial[0].Text, h.Fired[0].FollowUp.Text);
        }

        [Fact]
        public void FollowUpTimingIsDeterministic()
        {
            List<string> Trace()
            {
                var h = new Harness(
                    (GenuineSignal, new[] { FollowUp("fu_a", SignalFollowUpTriggers.Answered, 2, "A"), FollowUp("fu_b", SignalFollowUpTriggers.RescueSuccess, 3, "B") }),
                    (TrapSignal, new[] { FollowUp("fu_trap", SignalFollowUpTriggers.AmbushEncountered, 1, "T") }));
                var lines = new List<string>();
                h.Scheduler.SetDay(1);
                h.Missions.RecordSignalHeard(GenuineSignal, 1);
                h.Missions.RecordExpeditionDispatched(GenuineQuest, "exp_x");
                h.Scheduler.SetDay(2);
                h.Missions.RecordDestinationReached(GenuineQuest, 2);
                h.Missions.RecordSignalHeard(TrapSignal, 1);
                h.Missions.RecordExpeditionDispatched(TrapQuest, "exp_t");
                h.Missions.RecordDestinationReached(TrapQuest, 2);
                for (int day = 1; day <= 10; day++) h.Scheduler.TickDaily(day);
                foreach (var (parent, followUp, day) in h.Fired)
                    lines.Add($"{day}|{parent}|{followUp.Id}|{followUp.Text}");
                return lines;
            }

            var a = Trace();
            var b = Trace();
            Assert.Equal(a, b);
            Assert.Equal(3, a.Count);
            // Exact timing: fu_trap day 3 (2+1), fu_a day 3 (1+2), fu_b day 5 (2+3);
            // same-day order is parent-ID ordinal (192_4 before 88_3).
            Assert.Equal("3|freq_distress_192_4|fu_trap|T", a[0]);
            Assert.Equal("3|freq_distress_88_3|fu_a|A", a[1]);
            Assert.Equal("5|freq_distress_88_3|fu_b|B", a[2]);
        }

        [Fact]
        public void FollowUpSurvivesSaveLoad()
        {
            var json = new Ashfall.Core.SystemTextJsonSerializer();
            List<string> Continuous()
            {
                var h = new Harness((GenuineSignal, new[] { FollowUp("fu_c", SignalFollowUpTriggers.RescueSuccess, 5, "Aftermath.") }));
                h.Scheduler.SetDay(1);
                h.Missions.RecordSignalHeard(GenuineSignal, 1);
                h.Missions.RecordExpeditionDispatched(GenuineQuest, "exp_c");
                h.Scheduler.SetDay(2);
                h.Missions.RecordDestinationReached(GenuineQuest, 2);
                for (int day = 3; day <= 8; day++) h.Scheduler.TickDaily(day);
                var lines = new List<string>();
                foreach (var (parent, followUp, day) in h.Fired) lines.Add($"{day}|{followUp.Id}");
                return lines;
            }
            List<string> Interrupted()
            {
                var h = new Harness((GenuineSignal, new[] { FollowUp("fu_c", SignalFollowUpTriggers.RescueSuccess, 5, "Aftermath.") }));
                h.Scheduler.SetDay(1);
                h.Missions.RecordSignalHeard(GenuineSignal, 1);
                h.Missions.RecordExpeditionDispatched(GenuineQuest, "exp_c");
                h.Scheduler.SetDay(2);
                h.Missions.RecordDestinationReached(GenuineQuest, 2);
                h.Scheduler.TickDaily(4); // before due day 7
                // Save / load round-trip of the scheduler state.
                var captured = h.Scheduler.CaptureState();
                string encoded = RadioSaveCodec.Encode(new RadioSaveState { day = 4, signalFollowUps = captured }, json);
                Assert.True(RadioSaveCodec.TryDecode(encoded, json, out var restored));
                var h2 = new Harness((GenuineSignal, new[] { FollowUp("fu_c", SignalFollowUpTriggers.RescueSuccess, 5, "Aftermath.") }));
                h2.Scheduler.RestoreState(restored!.signalFollowUps);
                for (int day = 5; day <= 8; day++) h2.Scheduler.TickDaily(day);
                var lines = new List<string>();
                foreach (var (parent, followUp, day) in h2.Fired) lines.Add($"{day}|{followUp.Id}");
                return lines;
            }

            Assert.Equal(Continuous(), Interrupted());
        }

        [Fact]
        public void FollowUpDoesNotFireWhenInitialSignalIgnored()
        {
            var h = new Harness((LegacyExpirySignal, new[] { FollowUp("fu_never", SignalFollowUpTriggers.Answered, 1, "Should never fire.") }));
            // No follow-up authored with the "expired" trigger — an ignored
            // signal schedules nothing.
            h.Missions.RecordSignalHeard(LegacyExpirySignal, 1);
            h.Missions.TickDaily(100); // deadline expiry
            Assert.Empty(h.Fired);
            Assert.Empty(h.Scheduler.PendingKeys);
        }

        [Fact]
        public void FollowUpDoesNotFireWhenInitialSignalWasTrap()
        {
            var h = new Harness((TrapSignal, new[] { FollowUp("fu_no_rescue", SignalFollowUpTriggers.RescueSuccess, 1, "Wrong trigger for the trap path.") }));
            // A genuine-path (rescue_success) follow-up must never fire from a
            // trap resolution: the ambush outcome schedules only
            // ambush_encountered-triggered content.
            h.Scheduler.SetDay(1);
            h.Missions.RecordSignalHeard(TrapSignal, 1);
            h.Missions.RecordExpeditionDispatched(TrapQuest, "exp_t");
            h.Scheduler.SetDay(2);
            h.Missions.RecordDestinationReached(TrapQuest, 2); // TerminalAmbush
            Assert.Empty(h.Fired); // no ambush_encountered follow-up authored
            Assert.Empty(h.Scheduler.PendingKeys);
        }

        [Fact]
        public void TrapAftermathFollowUpFiresOnAmbushEncountered()
        {
            var h = new Harness((TrapSignal, new[] { FollowUp("fu_aftermath", SignalFollowUpTriggers.AmbushEncountered, 1, "Word spreads of the ambush at the substation.") }));
            h.Scheduler.SetDay(1);
            h.Missions.RecordSignalHeard(TrapSignal, 1);
            h.Missions.RecordExpeditionDispatched(TrapQuest, "exp_t");
            h.Scheduler.SetDay(2);
            h.Missions.RecordDestinationReached(TrapQuest, 2); // TerminalAmbush
            h.Scheduler.TickDaily(3); // day 2 + 1
            Assert.Single(h.Fired);
        }

        // ── Exactly-once scheduling and firing ────────────────────────────────

        [Fact]
        public void NoDuplicateSchedulingOrFiring()
        {
            var h = new Harness((GenuineSignal, new[] { FollowUp("fu_once", SignalFollowUpTriggers.RescueSuccess, 2, "Once.") }));
            h.Scheduler.SetDay(1);
            h.Missions.RecordSignalHeard(GenuineSignal, 1);
            h.Missions.RecordExpeditionDispatched(GenuineQuest, "exp_1");
            h.Scheduler.SetDay(2);
            h.Missions.RecordDestinationReached(GenuineQuest, 2);
            // Replay attempts (guard replays, repeated ticks) must not duplicate.
            h.Missions.RecordDestinationReached(GenuineQuest, 2);
            h.Scheduler.TickDaily(4);
            h.Scheduler.TickDaily(4);
            h.Scheduler.TickDaily(5);
            Assert.Single(h.Fired);
        }

        [Fact]
        public void FiredFollowUpDoesNotRefireAfterReload()
        {
            var h = new Harness((GenuineSignal, new[] { FollowUp("fu_reload", SignalFollowUpTriggers.Answered, 1, "Fired once.") }));
            h.Scheduler.SetDay(1);
            h.Missions.RecordSignalHeard(GenuineSignal, 1);
            h.Missions.RecordExpeditionDispatched(GenuineQuest, "exp_1");
            h.Scheduler.TickDaily(2); // fires
            Assert.Single(h.Fired);

            // Save after fire, restore into a fresh scheduler, tick again.
            var captured = h.Scheduler.CaptureState();
            var h2 = new Harness((GenuineSignal, new[] { FollowUp("fu_reload", SignalFollowUpTriggers.Answered, 1, "Fired once.") }));
            h2.Scheduler.RestoreState(captured);
            h2.Scheduler.TickDaily(3);
            h2.Scheduler.TickDaily(100);
            Assert.Empty(h2.Fired); // fired ledger persisted — no refire
        }

        [Fact]
        public void SameDayOrderingIsDeterministic()
        {
            var h = new Harness(
                (GenuineSignal, new[] { FollowUp("z_last", SignalFollowUpTriggers.Answered, 1, "Z"), FollowUp("a_first", SignalFollowUpTriggers.Answered, 1, "A") }));
            h.Scheduler.SetDay(1);
            h.Missions.RecordSignalHeard(GenuineSignal, 1);
            h.Missions.RecordExpeditionDispatched(GenuineQuest, "exp_1");
            h.Scheduler.TickDaily(2);
            Assert.Equal(2, h.Fired.Count);
            Assert.Equal("a_first", h.Fired[0].FollowUp.Id); // ordinal ID order within the same day
            Assert.Equal("z_last", h.Fired[1].FollowUp.Id);
        }

        [Fact]
        public void ResolvedParentCannotRescheduleOldFollowUp()
        {
            var h = new Harness((GenuineSignal, new[] { FollowUp("fu_done", SignalFollowUpTriggers.RescueSuccess, 1, "Done.") }));
            h.Scheduler.SetDay(1);
            h.Missions.RecordSignalHeard(GenuineSignal, 1);
            h.Missions.RecordExpeditionDispatched(GenuineQuest, "exp_1");
            h.Scheduler.SetDay(2);
            h.Missions.RecordDestinationReached(GenuineQuest, 2);
            h.Scheduler.TickDaily(3); // fires
            // A later rescue_failed-style transition on the same (terminal) mission
            // cannot reschedule — terminal missions emit no further matching events,
            // and the fired ledger blocks re-scheduling even if one occurred.
            Assert.Equal(0, h.Scheduler.ScheduleForTrigger(GenuineSignal, SignalFollowUpTriggers.RescueSuccess));
        }

        // ── Validation ────────────────────────────────────────────────────────

        private static string WriteTempCatalog(string json)
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall-followup-validator-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            File.WriteAllText(Path.Combine(dir, "radio_distress_signals.json"), json);
            return dir;
        }

        private static void DisposeTemp(string dir)
        {
            try { Directory.Delete(dir, recursive: true); } catch { /* best effort */ }
        }

        [Fact]
        public void Validator_RejectsUnsupportedTriggerAndNegativeDelay()
        {
            string json = """
            {
              "schema_version": 1,
              "radio_broadcasts": [
                {
                  "frequency_id": "freq_test_fu",
                  "frequency_mhz": "101.5",
                  "source_name": "FollowUp Fixture",
                  "outcome_type": "survivor_isolated",
                  "revealed_location": "loc_test_validator",
                  "message_fragments": [{"day":1,"clarity":0.2,"text":"a"}],
                  "follow_up_signals": [
                    { "id": "fu_bad", "trigger_condition": "player_sneezed", "delay_days": -3, "text": "x" }
                  ]
                }
              ]
            }
            """;
            var dir = WriteTempCatalog(json);
            try
            {
                var report = new Ashfall.Core.CatalogIntegrityReport();
                Ashfall.Core.CatalogIntegrityValidator.ValidateDistressSignalStages(dir, new Ashfall.Core.FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("fu_bad") && e.Contains("unsupported trigger_condition"));
                Assert.Contains(report.Errors, e => e.Contains("fu_bad") && e.Contains("delay_days=-3"));
            }
            finally { DisposeTemp(dir); }
        }

        [Fact]
        public void Validator_RejectsDuplicateFollowUpIdsAcrossCorpus()
        {
            string json = """
            {
              "schema_version": 1,
              "radio_broadcasts": [
                {
                  "frequency_id": "freq_test_fu_a",
                  "frequency_mhz": "101.5",
                  "source_name": "FU A",
                  "outcome_type": "survivor_isolated",
                  "revealed_location": "loc_test_validator",
                  "message_fragments": [{"day":1,"clarity":0.2,"text":"a"}],
                  "follow_up_signals": [
                    { "id": "fu_dupe", "trigger_condition": "answered", "delay_days": 1, "text": "x" }
                  ]
                },
                {
                  "frequency_id": "freq_test_fu_b",
                  "frequency_mhz": "102.5",
                  "source_name": "FU B",
                  "outcome_type": "survivor_isolated",
                  "revealed_location": "loc_test_validator",
                  "message_fragments": [{"day":1,"clarity":0.2,"text":"b"}],
                  "follow_up_signals": [
                    { "id": "fu_dupe", "trigger_condition": "answered", "delay_days": 2, "text": "y" }
                  ]
                }
              ]
            }
            """;
            var dir = WriteTempCatalog(json);
            try
            {
                var report = new Ashfall.Core.CatalogIntegrityReport();
                Ashfall.Core.CatalogIntegrityValidator.ValidateDistressSignalStages(dir, new Ashfall.Core.FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("duplicate follow-up id 'fu_dupe'"));
            }
            finally { DisposeTemp(dir); }
        }

        [Fact]
        public void Validator_AcceptsWellFormedFollowUpsAndRealCatalogsStayClean()
        {
            string json = """
            {
              "schema_version": 1,
              "radio_broadcasts": [
                {
                  "frequency_id": "freq_test_fu_ok",
                  "frequency_mhz": "101.5",
                  "source_name": "FU OK",
                  "outcome_type": "survivor_isolated",
                  "revealed_location": "loc_test_validator",
                  "message_fragments": [{"day":1,"clarity":0.2,"text":"a"}],
                  "follow_up_signals": [
                    { "id": "fu_ok_1", "trigger_condition": "rescue_success", "delay_days": 5, "text": "Aftermath.", "clarity": 0.95, "outcome_hint": "A new voice." }
                  ]
                }
              ]
            }
            """;
            var dir = WriteTempCatalog(json);
            try
            {
                var report = new Ashfall.Core.CatalogIntegrityReport();
                Ashfall.Core.CatalogIntegrityValidator.ValidateDistressSignalStages(dir, new Ashfall.Core.FileSystemIO(), report);
                Assert.Empty(report.Errors);
            }
            finally { DisposeTemp(dir); }

            // Real catalogs: no follow-ups authored yet → zero follow-up errors.
            var real = new Ashfall.Core.CatalogIntegrityReport();
            Ashfall.Core.CatalogIntegrityValidator.ValidateDistressSignalStages(DataDirectory, new Ashfall.Core.FileSystemIO(), real);
            Assert.DoesNotContain(real.Errors, e => e.Contains("follow_up_signals") || e.Contains("follow-up"));
        }

        // ── DTO binding + V6 save ─────────────────────────────────────────────

        [Fact]
        public void FollowUpSignalsBindFromJson()
        {
            string json = """
            {
              "schema_version": 1,
              "radio_broadcasts": [
                {
                  "frequency_id": "freq_test_fu_bind",
                  "frequency_mhz": "101.5",
                  "source_name": "FU Bind",
                  "outcome_type": "survivor_isolated",
                  "revealed_location": "loc_test_validator",
                  "message_fragments": [{"day":1,"clarity":0.2,"text":"a"}],
                  "follow_up_signals": [
                    { "id": "fu_bind_1", "trigger_condition": "answered", "delay_days": 2, "text": "We can see your team on the road." }
                  ]
                }
              ]
            }
            """;
            var system = new RadioDistressSystem();
            Assert.Equal(1, system.LoadFromJson(json));
            var def = system.GetDefinition("freq_test_fu_bind");
            Assert.NotNull(def);
            Assert.Single(def!.FollowUpSignals);
            Assert.Equal("answered", def.FollowUpSignals[0].TriggerCondition);
            Assert.Equal(2, def.FollowUpSignals[0].DelayDays);
        }

        [Fact]
        public void RadioSaveV6RoundTripsFollowUpState()
        {
            var json = new Ashfall.Core.SystemTextJsonSerializer();
            var state = new RadioSaveState
            {
                day = 42,
                signalFollowUps = new SignalFollowUpSaveState
                {
                    pending = { new PendingSignalFollowUpEntry { parentSignalId = GenuineSignal, followUpId = "fu_x", dueDay = 44 } },
                    firedKeys = { $"{GenuineSignal}:fu_done" }
                }
            };
            string encoded = RadioSaveCodec.Encode(state, json);
            Assert.Contains("\"saveVersion\":6", encoded);
            Assert.True(RadioSaveCodec.TryDecode(encoded, json, out var restored));
            Assert.NotNull(restored!.signalFollowUps);
            Assert.Equal(SignalTrustPolicy.NeutralScore, restored.signalTrust!.score); // EnsureCollections neutral fill
            Assert.Single(restored.signalFollowUps.pending);
            Assert.Equal(44, restored.signalFollowUps.pending[0].dueDay);
            Assert.Contains($"{GenuineSignal}:fu_done", restored.signalFollowUps.firedKeys);
        }

        [Fact]
        public void RadioSaveV5MigratesToEmptyFollowUpState()
        {
            var json = new Ashfall.Core.SystemTextJsonSerializer();
            var v5 = new RadioSaveStateFrozenV5 { day = 30, currentFrequency = 100.0f };
            v5.Checksum = Ashfall.Core.SaveChecksum.Compute(v5);
            string encoded = json.Serialize(v5);

            Assert.True(RadioSaveCodec.TryDecode(encoded, json, out var migrated));
            Assert.Equal(RadioSaveCodec.CurrentSaveVersion, migrated!.saveVersion);
            Assert.NotNull(migrated.signalFollowUps);
            Assert.Empty(migrated.signalFollowUps!.pending);
            Assert.Empty(migrated.signalFollowUps.firedKeys);
        }

        [Fact]
        public void PendingViewProjectionIsDeterministicAndReadOnly()
        {
            var h = new Harness(
                (GenuineSignal, new[] { FollowUp("fu_view_b", SignalFollowUpTriggers.Answered, 3, "B"), FollowUp("fu_view_a", SignalFollowUpTriggers.Answered, 2, "A") }),
                (TrapSignal, new[] { FollowUp("fu_view_t", SignalFollowUpTriggers.AmbushEncountered, 1, "T") }));
            h.Scheduler.SetDay(1);
            h.Missions.RecordSignalHeard(GenuineSignal, 1);
            h.Missions.RecordExpeditionDispatched(GenuineQuest, "exp_1");
            h.Scheduler.SetDay(2);
            h.Missions.RecordSignalHeard(TrapSignal, 1);
            h.Missions.RecordExpeditionDispatched(TrapQuest, "exp_t");
            h.Missions.RecordDestinationReached(TrapQuest, 2);

            var view = h.Scheduler.GetPendingView();
            Assert.Equal(3, view.Count);
            // Ordered by (dueDay, parentSignalId, followUpId).
            Assert.Equal(("freq_distress_192_4", "fu_view_t", 3), (view[0].ParentSignalId, view[0].FollowUpId, view[0].DueDay));
            Assert.Equal(("freq_distress_88_3", "fu_view_a", 3), (view[1].ParentSignalId, view[1].FollowUpId, view[1].DueDay));
            Assert.Equal(("freq_distress_88_3", "fu_view_b", 4), (view[2].ParentSignalId, view[2].FollowUpId, view[2].DueDay));
            // Read-only projection: consuming it does not consume the queue.
            var again = h.Scheduler.GetPendingView();
            Assert.Equal(view.Count, again.Count);
            h.Scheduler.TickDaily(3); // due days 3 and 3 fire; fu_view_b (due 4) remains
            Assert.Single(h.Scheduler.GetPendingView());
        }

        [Fact]
        public void LastFiredProjectionRecordsMostRecentTransmission()
        {
            var h = new Harness((GenuineSignal, new[] { FollowUp("fu_last", SignalFollowUpTriggers.Answered, 1, "Latest.") }));
            h.Scheduler.SetDay(1);
            h.Missions.RecordSignalHeard(GenuineSignal, 1);
            h.Missions.RecordExpeditionDispatched(GenuineQuest, "exp_1");
            Assert.Null(h.Scheduler.LastFired);
            h.Scheduler.TickDaily(2);
            Assert.NotNull(h.Scheduler.LastFired);
            Assert.Equal(GenuineSignal, h.Scheduler.LastFired!.Value.ParentSignalId);
            Assert.Equal("fu_last", h.Scheduler.LastFired.Value.FollowUp.Id);
            Assert.Equal(2, h.Scheduler.LastFired.Value.Day);
        }

        [Fact]
        public void StageAndFollowUpSemanticsAreDistinct()
        {
            // A time-based stage change is not a follow-up: advancing days with
            // no player action schedules nothing and fires nothing.
            var h = new Harness((GenuineSignal, new[] { FollowUp("fu_never_2", SignalFollowUpTriggers.Answered, 1, "No action taken.") }));
            for (int day = 1; day <= 30; day++) h.Scheduler.TickDaily(day);
            Assert.Empty(h.Fired);
            Assert.Empty(h.Scheduler.PendingKeys);
        }
    }
}
