// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Tasks 9–12 Wave 5 — unified deterministic replay harness (plan §13).
    ///
    /// Exercises the complete lifecycle over one fixture campaign:
    /// detect → listen → stage progression → answer → rescue → follow-up
    /// scheduled → follow-up fires → trust recorded → audio cue resolved,
    /// with save/load round-trips through the REAL radio save codec
    /// (RadioSaveCodec V6) and byte-for-byte trace comparison.
    ///
    /// TEST CONTRACT: the same seed and the same action trace reproduce the
    /// same result, field by field — stage, trust, pending/fired follow-ups,
    /// and the resolved audio cue are unchanged across a save/load boundary.
    /// </summary>
    public sealed class DistressSignalTasks912ReplayTests
    {
        private const string SignalId = "freq_test_replay_main";     // genuine, multi-stage, follow-ups
        private const string QuestId = "quest_test_replay_main";
        private const string TrapSignalId = "freq_test_replay_trap"; // trap, aftermath follow-up
        private const string TrapQuestId = "quest_test_replay_trap";
        private const int Seed = 42; // campaign seed (documented fixture constant)

        private sealed class World
        {
            public RadioDistressSystem Distress = null!;
            public DistressRescueMissionManager Missions = null!;
            public SignalTrustLedger Trust = null!;
            public DistressFollowUpScheduler FollowUps = null!;
            public int Day;

            public static World Create()
            {
                var w = new World();
                w.Distress = new RadioDistressSystem();
                w.RegisterFixtures();
                w.Trust = new SignalTrustLedger();
                w.Missions = new DistressRescueMissionManager(null, w.Distress, w.Trust);
                w.FollowUps = new DistressFollowUpScheduler(w.Distress, w.Missions);
                w.FollowUps.BindToMissionEvents();
                return w;
            }

            private void RegisterFixtures()
            {
                // Genuine multi-stage signal: 3 stages (day 1/3/5, clarity
                // 0.25/0.55/0.9), hint only on the final stage, per-stage audio
                // overrides, one answered follow-up (delay 2) and one
                // rescue_success follow-up (delay 5).
                var main = new DistressSignalDefinition
                {
                    FrequencyId = SignalId,
                    FrequencyMhzStr = "199.5",
                    SourceName = "Replay Main Fixture",
                    OutcomeTypeStr = "survivor_isolated",
                    DaysToTrace = 4,
                    DeadlineDays = 5,
                    SenderSurvivalDays = 5,
                    AudioCue = "radio_static"
                };
                main.MessageFragments.Add(new DistressMessageFragment { Day = 1, Clarity = 0.25f, Text = "weak carrier" });
                main.MessageFragments.Add(new DistressMessageFragment { Day = 3, Clarity = 0.55f, Text = "clearer plea", AudioCue = "radio_morse" });
                main.MessageFragments.Add(new DistressMessageFragment { Day = 5, Clarity = 0.9f, Text = "coordinates known", OutcomeHint = "The coordinates repeat every 30 seconds.", AudioCue = "radio_distress_beacon" });
                main.FollowUpSignals.Add(new SignalFollowUpDefinition { Id = "fu_replay_answered", TriggerCondition = SignalFollowUpTriggers.Answered, DelayDays = 2, Text = "We can see your team on the road.", Clarity = 0.9f, AudioCue = "radio_vo_ch3_ash_road" });
                main.FollowUpSignals.Add(new SignalFollowUpDefinition { Id = "fu_replay_rescued", TriggerCondition = SignalFollowUpTriggers.RescueSuccess, DelayDays = 5, Text = "The survivor sends word from the shelter.", Clarity = 0.95f });
                Distress.RegisterSignal(main);

                // Trap signal: same shape, authored trap authenticity, a
                // ambush_encountered aftermath and a rescue_success follow-up that
                // must never fire from the trap path.
                var trap = new DistressSignalDefinition
                {
                    FrequencyId = TrapSignalId,
                    FrequencyMhzStr = "188.2",
                    SourceName = "Replay Trap Fixture",
                    OutcomeTypeStr = "bait_trap",
                    Authenticity = "trap",
                    DaysToTrace = 3,
                    DeadlineDays = 5,
                    AudioCue = "radio_static"
                };
                trap.MessageFragments.Add(new DistressMessageFragment { Day = 1, Clarity = 0.3f, Text = "urgent plea" });
                trap.MessageFragments.Add(new DistressMessageFragment { Day = 3, Clarity = 0.8f, Text = "plea repeats", AudioCue = "radio_morse" });
                trap.FollowUpSignals.Add(new SignalFollowUpDefinition { Id = "fu_replay_trap_aftermath", TriggerCondition = SignalFollowUpTriggers.AmbushEncountered, DelayDays = 1, Text = "Word spreads of the ambush.", Clarity = 0.85f });
                trap.FollowUpSignals.Add(new SignalFollowUpDefinition { Id = "fu_replay_trap_rescue", TriggerCondition = SignalFollowUpTriggers.RescueSuccess, DelayDays = 1, Text = "Must never fire.", Clarity = 0.9f });
                Distress.RegisterSignal(trap);

                // Authoritative mission registrations (the lifecycle events the
                // scheduler and trust ledger consume).
                typeof(DistressRescueMissionManager)
                    .GetField("dummy", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance); // no-op; missions registered via ctor fixtures below
            }

            /// <summary>Registers the harness missions through the public
            /// capture/restore seam: build a manager, capture its authored
            /// missions, then inject the two harness missions by restoring a
            /// merged save state. Keeps the harness entirely on public APIs.</summary>
            public void RegisterMissions()
            {
                // The authored manager fixtures already contain the raider trap
                // (quest_distress_raider_trap / freq_distress_192_4). For the
                // harness signals we drive the lifecycle through the same
                // public transitions the authored missions use, so the harness
                // missions are registered by restoring a DistressMissionSaveState
                // that contains them.
                var state = Missions.CaptureState();
                state.Missions.Add(new DistressRescueMission
                {
                    QuestId = QuestId,
                    SignalId = SignalId,
                    DestinationId = "loc_replay_site",
                    DaysToTrace = 4,
                    DeadlineDays = 5,
                    SenderSurvivalDays = 5,
                    SenderAlive = true
                });
                state.Missions.Add(new DistressRescueMission
                {
                    QuestId = TrapQuestId,
                    SignalId = TrapSignalId,
                    DestinationId = "loc_replay_trap_site",
                    DaysToTrace = 3,
                    DeadlineDays = 5,
                    IsTrap = true,
                    SenderAlive = true
                });
                state.RefreshFingerprint();
                Missions.RestoreState(state);
            }

            public void SetDay(int day)
            {
                Day = Math.Max(1, day);
                // Host contract: the scheduler learns the day BEFORE the
                // lifecycle ticks that can raise scheduling events.
                FollowUps.SetDay(Day);
            }

            /// <summary>Captures the complete Tasks 9–12 state through the REAL
            /// radio save codec (V6) — the same path the host uses.</summary>
            public string Save()
            {
                var state = new RadioSaveState
                {
                    day = Day,
                    distressSignals = Distress.CaptureState(),
                    rescueMissions = Missions.CaptureState(),
                    signalTrust = Trust.CaptureState(),
                    signalFollowUps = FollowUps.CaptureState()
                };
                return RadioSaveCodec.Encode(state, new Ashfall.Core.SystemTextJsonSerializer());
            }

            public static World Load(string json)
            {
                Assert.True(RadioSaveCodec.TryDecode(json, new Ashfall.Core.SystemTextJsonSerializer(), out var state));
                var w = new World();
                w.Distress = new RadioDistressSystem();
                w.RegisterFixtures();
                w.Trust = new SignalTrustLedger();
                w.Missions = new DistressRescueMissionManager(null, w.Distress, w.Trust);
                w.FollowUps = new DistressFollowUpScheduler(w.Distress, w.Missions);
                w.FollowUps.BindToMissionEvents();
                w.Day = state!.day;
                w.Distress.RestoreState(state.distressSignals);
                w.Missions.RestoreState(state.rescueMissions);
                w.Trust.RestoreState(state.signalTrust);
                w.FollowUps.RestoreState(state.signalFollowUps);
                w.FollowUps.SetDay(w.Day);
                return w;
            }
        }

        /// <summary>One trace line per day: stage, clarity, text/hint hashes,
        /// trust, pending/fired follow-ups, resolved audio cue.</summary>
        private static List<string> Trace(World w, int fromDay, int toDay)
        {
            var lines = new List<string>();
            for (int day = fromDay; day <= toDay; day++)
            {
                w.SetDay(day);
                w.Distress.TickDaily(day);
                w.FollowUps.TickDaily(day);

                var def = w.Distress.GetDefinition(SignalId)!;
                var stage = DistressStageResolver.Resolve(def, day);
                string cue = DistressAudioCueResolver.ResolveForDay(def, day);
                string pending = string.Join(";", w.FollowUps.PendingKeys.OrderBy(k => k, StringComparer.Ordinal));
                string fired = string.Join(";", w.FollowUps.FiredKeys.OrderBy(k => k, StringComparer.Ordinal));
                lines.Add(
                    $"d{day}" +
                    $"|stage={stage?.StageIndex ?? -1}" +
                    $"|clarity={stage?.Fragment.Clarity.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) ?? "-"}" +
                    $"|text={Ashfall.Core.StableHash.Of(stage?.Fragment.Text ?? string.Empty)}" +
                    $"|hint={Ashfall.Core.StableHash.Of(stage?.Fragment.OutcomeHint ?? string.Empty)}" +
                    $"|cue={cue}" +
                    $"|trust={w.Trust.Score}" +
                    $"|a={w.Trust.SignalsAnswered},i={w.Trust.SignalsIgnored},t={w.Trust.TrapsFallenFor},r={w.Trust.RescuesSuccessful}" +
                    $"|pending={pending}" +
                    $"|fired={fired}");
            }
            return lines;
        }

        // ── Scenario A/B: rescue lifecycle, continuous vs interrupted ─────────

        private static void RunRescueActions(World w)
        {
            w.SetDay(1);
            Assert.True(w.Distress.Intercept(SignalId, 1)); // detect
            Assert.True(w.Missions.RecordSignalHeard(SignalId, 1));
            // Days 2–4: stage progression only — no trust change may occur.
            w.SetDay(4);
            w.Distress.TickDaily(4);
            Assert.Equal(SignalTrustPolicy.NeutralScore, w.Trust.Score);
            // Answer (authoritative dispatch).
            w.SetDay(4);
            Assert.True(w.Missions.RecordExpeditionDispatched(QuestId, "exp_replay"));
            // Successful rescue: arrival day 5 < sender death day 6.
            w.SetDay(5);
            Assert.Equal(DistressRescueMissionStage.TerminalRescued,
                w.Missions.RecordDestinationReached(QuestId, 5));
        }

        [Fact]
        public void ScenarioA_ContinuousRescueLifecycleTrace()
        {
            var w = World.Create();
            w.RegisterMissions();
            RunRescueActions(w);
            var trace = Trace(w, 1, 14);

            // Stage progression: 0 → 1 (day 3) → 2 (day 5), monotonic.
            Assert.Contains("d1|stage=0", trace[0]);
            Assert.Contains("d3|stage=1", trace[2]);
            Assert.Contains("d5|stage=2", trace[4]);
            // Hint reveals only at the final stage: empty-text hash (0) before,
            // non-zero authored hint hash from day 5.
            Assert.Contains("|hint=0", trace[0]);
            Assert.Contains("|hint=0", trace[2]);
            Assert.NotEqual(0, Ashfall.Core.StableHash.Of("The coordinates repeat every 30 seconds."));
            Assert.DoesNotContain("|hint=0", trace[4]);
            // Answer on day 4 → answered follow-up due day 6; rescue day 5 →
            // rescue follow-up due day 10: both pending at day 5.
            Assert.Contains("freq_test_replay_main:fu_replay_answered", trace[4].Split("pending=")[1].Split("|fired=")[0]);
            Assert.Contains("freq_test_replay_main:fu_replay_rescued", trace[4].Split("pending=")[1].Split("|fired=")[0]);
            // The answered follow-up fires exactly on day 6, the rescue
            // follow-up exactly on day 10 (fired list is ordinal-sorted, so
            // the rescue key is matched within the fired segment).
            Assert.Contains("fired=freq_test_replay_main:fu_replay_answered", trace[5]);
            Assert.Contains("fu_replay_rescued", trace[9].Split("fired=")[1]);
            // Trust: +2 answered, +5 rescue = 57.
            Assert.Contains("|trust=57|a=1,i=0,t=0,r=1", trace[5]);
            // Audio cue follows the stage overrides.
            Assert.Contains("|cue=radio_static", trace[0]);
            Assert.Contains("|cue=radio_morse", trace[2]);
            Assert.Contains("|cue=radio_distress_beacon", trace[4]);
            // Final state stable: last two days identical except the day tag.
            Assert.Equal(trace[13][3..], trace[12][3..]);
        }

        [Fact]
        public void ScenarioB_SaveLoadMidLifecycle_ProducesIdenticalTrace()
        {
            List<string> Continuous()
            {
                var w = World.Create();
                w.RegisterMissions();
                RunRescueActions(w);
                return Trace(w, 1, 14);
            }


            // The interrupted trace (days 7–14 after reload) must equal the
            // continuous trace for the same days, field by field.
            var continuous = Continuous();
            var w1 = World.Create();
            w1.RegisterMissions();
            RunRescueActions(w1);
            w1.SetDay(6);
            w1.Distress.TickDaily(6);
            w1.FollowUps.TickDaily(6);
            string referenceSave = w1.Save();
            var w2 = World.Load(referenceSave);
            var afterReload = Trace(w2, 7, 14);
            var continuousTail = Trace(w1, 7, 14);
            Assert.Equal(continuousTail, afterReload);

            // And the continuous full trace is stable run-to-run.
            Assert.Equal(Continuous(), Continuous());
        }

        private static List<string> TracePrefixFromSave(World w, int from, int to)
            => Trace(w, from, to);

        // ── Scenario C: ignore path ───────────────────────────────────────────

        [Fact]
        public void ScenarioC_IgnorePath_DiffersExactlyAsAuthored()
        {
            var w = World.Create();
            w.RegisterMissions();
            w.SetDay(1);
            Assert.True(w.Distress.Intercept(SignalId, 1));
            Assert.True(w.Missions.RecordSignalHeard(SignalId, 1));
            // Never dispatch; the response window expires (deadline 5 → expiry day 6).
            w.SetDay(6);
            w.Distress.TickDaily(6);
            w.Missions.TickDaily(6);
            w.FollowUps.TickDaily(6);

            // Trust path differs: ignored −2 (score 48), no rescue, no answer.
            Assert.Equal(SignalTrustPolicy.NeutralScore + SignalTrustPolicy.DeltaIgnored, w.Trust.Score);
            Assert.Equal(1, w.Trust.SignalsIgnored);
            Assert.Equal(0, w.Trust.SignalsAnswered);
            // No forbidden follow-up fires: neither authored follow-up triggers
            // on the expired path.
            Assert.Empty(w.FollowUps.FiredKeys);
            Assert.Empty(w.FollowUps.PendingKeys);
            // Stage behavior before the ignore is identical to the rescue path:
            // stage progression is a pure function of the day.
            var stageDay5 = DistressStageResolver.Resolve(w.Distress.GetDefinition(SignalId), 5);
            Assert.Equal(2, stageDay5!.Value.StageIndex);
        }

        // ── Scenario D: trap response ─────────────────────────────────────────

        [Fact]
        public void ScenarioD_TrapResponse_PenaltyApplies_GenuineFollowUpSuppressed()
        {
            var w = World.Create();
            w.RegisterMissions();
            w.SetDay(1);
            Assert.True(w.Distress.Intercept(TrapSignalId, 1));
            Assert.True(w.Missions.RecordSignalHeard(TrapSignalId, 1));
            w.SetDay(2);
            Assert.True(w.Missions.RecordExpeditionDispatched(TrapQuestId, "exp_trap"));
            var stage = w.Missions.RecordDestinationReached(TrapQuestId, 2);
            Assert.Equal(DistressRescueMissionStage.TerminalAmbush, stage);
            w.FollowUps.TickDaily(3); // trap aftermath due day 3

            // Trap penalty applies (+2 answered, −5 trap = 47).
            Assert.Equal(SignalTrustPolicy.NeutralScore + SignalTrustPolicy.DeltaAnswered + SignalTrustPolicy.DeltaAmbushEncountered, w.Trust.Score);
            Assert.Equal(1, w.Trust.TrapsFallenFor);
            // The genuine rescue_success follow-up does NOT schedule.
            Assert.DoesNotContain($"{TrapSignalId}:fu_replay_trap_rescue", w.FollowUps.FiredKeys);
            Assert.DoesNotContain($"{TrapSignalId}:fu_replay_trap_rescue", w.FollowUps.PendingKeys);
            // The authored trap aftermath DOES fire.
            Assert.Contains($"{TrapSignalId}:fu_replay_trap_aftermath", w.FollowUps.FiredKeys);
            // Authored signal identity never changes.
            var def = w.Distress.GetDefinition(TrapSignalId)!;
            Assert.True(def.IsTrapOrDeception);
            Assert.False(def.IsGenuineRescue);
        }

        // ── Full-lifecycle fingerprint (same seed → same bytes) ──────────────

        [Fact]
        public void FullLifecycleFingerprintIsStableAcrossRuns()
        {
            string Fingerprint()
            {
                var w = World.Create();
                w.RegisterMissions();
                RunRescueActions(w);
                var lines = Trace(w, 1, 14);
                // Add terminal mission + save-state fields to the fingerprint.
                var mission = w.Missions.GetMissionByQuest(QuestId)!;
                lines.Add($"mission={mission.Stage}|arrival={mission.ArrivalResolved}");
                lines.Add($"save={w.Save()}");
                return Ashfall.Core.StableHash.Of(string.Join("\n", lines))
                    .ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            Assert.Equal(Fingerprint(), Fingerprint());
        }
    }
}
