// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// CF-P1 population seal. Every authored follow-up is driven through the
    /// real mission/scheduler lifecycle and the V6 radio codec. The harness
    /// injects missions for signals that are content-valid but not yet
    /// registered by production, so this test proves content/mechanism
    /// compatibility without claiming runtime reachability for those rows.
    /// </summary>
    public sealed class DistressFollowUpPopulationReplayTests
    {
        private const string PrimaryCatalog = "radio_distress_signals.json";
        private const string ExpansionCatalog = "radio_distress_signals_expansion.json";

        private sealed class CensusRow
        {
            public CensusRow(string catalog, string signalId, string followUpId,
                string trigger, int delay, string cue)
            {
                Catalog = catalog;
                SignalId = signalId;
                FollowUpId = followUpId;
                Trigger = trigger;
                Delay = delay;
                Cue = cue;
            }

            public string Catalog { get; }
            public string SignalId { get; }
            public string FollowUpId { get; }
            public string Trigger { get; }
            public int Delay { get; }
            public string Cue { get; }
            public string Text { get; set; } = string.Empty;
        }

        private static CensusRow Row(string catalog, string signalId, string followUpId,
            string trigger, int delay, string cue)
            => new CensusRow(catalog, signalId, followUpId, trigger, delay, cue);

        // TEST-AGGREGATION: source_rows=40 aggregate_cases=40 saved_cases=40
        private static readonly IReadOnlyList<CensusRow> ExpectedRows = new[]
        {
            Row(PrimaryCatalog, "freq_distress_217_4", "fu_217_4_answered", "answered", 2, "radio_distress_beacon"),
            Row(PrimaryCatalog, "freq_distress_217_4", "fu_217_4_rescue_success", "rescue_success", 4, "radio_distress_beacon"),
            Row(PrimaryCatalog, "freq_distress_148_2", "fu_148_2_trap_fallen_for", "ambush_encountered", 1, "radio_static"),
            Row(PrimaryCatalog, "freq_distress_55_1", "fu_55_1_answered", "answered", 2, "radio_vinyl_broadcast"),
            Row(PrimaryCatalog, "freq_distress_401_9", "fu_401_9_answered", "answered", 2, "radio_distress_beacon"),
            Row(PrimaryCatalog, "freq_distress_401_9", "fu_401_9_rescue_success", "rescue_success", 4, "radio_distress_beacon"),
            Row(PrimaryCatalog, "freq_distress_88_3", "fu_88_3_answered", "answered", 2, ""),
            Row(PrimaryCatalog, "freq_distress_88_3", "fu_88_3_rescue_success", "rescue_success", 5, ""),
            Row(PrimaryCatalog, "freq_distress_156_8", "fu_156_8_answered", "answered", 2, ""),
            Row(PrimaryCatalog, "freq_distress_156_8", "fu_156_8_rescue_success", "rescue_success", 4, ""),
            Row(PrimaryCatalog, "freq_distress_203_1", "fu_203_1_answered", "answered", 2, "radio_distress_beacon"),
            Row(PrimaryCatalog, "freq_distress_203_1", "fu_203_1_rescue_success", "rescue_success", 4, "radio_distress_beacon"),
            Row(PrimaryCatalog, "freq_distress_311_5", "fu_311_5_answered", "answered", 2, "radio_distress_beacon"),
            Row(PrimaryCatalog, "freq_distress_311_5", "fu_311_5_rescue_success", "rescue_success", 4, "radio_distress_beacon"),
            Row(PrimaryCatalog, "freq_distress_445_2", "fu_445_2_answered", "answered", 2, ""),
            Row(PrimaryCatalog, "freq_distress_445_2", "fu_445_2_rescue_success", "rescue_success", 5, ""),
            Row(PrimaryCatalog, "freq_distress_192_4", "fu_192_4_trap_fallen_for", "ambush_encountered", 1, ""),
            Row(PrimaryCatalog, "freq_distress_410_7", "fu_410_7_trap_fallen_for", "ambush_encountered", 1, "radio_static"),
            Row(PrimaryCatalog, "freq_distress_288_1", "fu_288_1_trap_fallen_for", "ambush_encountered", 1, "radio_static"),
            Row(PrimaryCatalog, "freq_distress_333_6", "fu_333_6_trap_fallen_for", "ambush_encountered", 1, "radio_static"),
            Row(PrimaryCatalog, "freq_distress_478_2", "fu_478_2_trap_fallen_for", "ambush_encountered", 1, "radio_static"),
            Row(PrimaryCatalog, "freq_distress_812_5", "fu_812_5_rescue_success", "rescue_success", 4, "radio_distress_beacon"),
            Row(PrimaryCatalog, "freq_distress_812_5", "fu_812_5_expired", "expired", 3, "radio_static"),
            Row(PrimaryCatalog, "freq_distress_867_9", "fu_867_9_rescue_success", "rescue_success", 4, "radio_distress_beacon"),
            Row(PrimaryCatalog, "freq_distress_867_9", "fu_867_9_expired", "expired", 3, "radio_static"),
            Row(PrimaryCatalog, "freq_distress_901_2", "fu_901_2_answered", "answered", 2, ""),
            Row(PrimaryCatalog, "freq_distress_901_2", "fu_901_2_rescue_success", "rescue_success", 5, ""),
            Row(ExpansionCatalog, "freq_distress_726_5", "fu_726_5_answered", "answered", 2, ""),
            Row(ExpansionCatalog, "freq_distress_726_5", "fu_726_5_rescue_success", "rescue_success", 4, ""),
            Row(ExpansionCatalog, "freq_distress_609_4", "fu_609_4_answered", "answered", 2, ""),
            Row(ExpansionCatalog, "freq_distress_609_4", "fu_609_4_rescue_success", "rescue_success", 5, ""),
            Row(ExpansionCatalog, "freq_distress_455_7", "fu_455_7_answered", "answered", 2, ""),
            Row(ExpansionCatalog, "freq_distress_455_7", "fu_455_7_rescue_success", "rescue_success", 4, ""),
            Row(ExpansionCatalog, "freq_distress_555_0", "fu_555_0_answered", "answered", 2, ""),
            Row(ExpansionCatalog, "freq_distress_555_0", "fu_555_0_rescue_success", "rescue_success", 4, ""),
            Row(ExpansionCatalog, "freq_distress_380_2", "fu_380_2_trap_fallen_for", "ambush_encountered", 1, ""),
            Row(ExpansionCatalog, "freq_distress_318_0", "fu_318_0_answered", "answered", 2, ""),
            Row(ExpansionCatalog, "freq_distress_318_0", "fu_318_0_rescue_success", "rescue_success", 5, ""),
            Row(ExpansionCatalog, "freq_distress_269_3", "fu_269_3_answered", "answered", 2, ""),
            Row(ExpansionCatalog, "freq_distress_269_3", "fu_269_3_rescue_success", "rescue_success", 4, "")
        };

        private sealed class World
        {
            public RadioDistressSystem Distress = null!;
            public DistressRescueMissionManager Missions = null!;
            public SignalTrustLedger Trust = null!;
            public DistressFollowUpScheduler FollowUps = null!;
            public int Day { get; private set; }
            public List<(string Parent, SignalFollowUpDefinition FollowUp, int Day)> Fired { get; } = new();

            public static World Create()
            {
                string dataDir = DataDirectory();
                var world = new World
                {
                    Distress = new RadioDistressSystem()
                };
                // RadioHostSession uses expansion-first, primary-last loading.
                world.Distress.LoadFromJson(File.ReadAllText(Path.Combine(dataDir, ExpansionCatalog)));
                world.Distress.LoadFromJson(File.ReadAllText(Path.Combine(dataDir, PrimaryCatalog)));
                world.Trust = new SignalTrustLedger();
                world.Missions = new DistressRescueMissionManager(null, world.Distress, world.Trust);
                world.FollowUps = new DistressFollowUpScheduler(world.Distress, world.Missions);
                world.FollowUps.BindToMissionEvents();
                world.FollowUps.OnFollowUpFired += (parent, followUp, day) =>
                    world.Fired.Add((parent, followUp, day));
                world.RegisterHarnessMissions();
                return world;
            }

            private void RegisterHarnessMissions()
            {
                var state = Missions.CaptureState();
                foreach (string signalId in ExpectedRows.Select(r => r.SignalId).Distinct(StringComparer.Ordinal))
                {
                    if (Missions.GetMissionBySignal(signalId) != null) continue;
                    var definition = Distress.GetDefinition(signalId);
                    Assert.NotNull(definition);
                    var tokens = new List<string>();
                    if (!string.IsNullOrWhiteSpace(definition!.IgnoreConsequence))
                        tokens.Add(definition.IgnoreConsequence);
                    state.Missions.Add(new DistressRescueMission
                    {
                        QuestId = "quest_replay_" + signalId,
                        SignalId = signalId,
                        DestinationId = "loc_replay_" + signalId,
                        DaysToTrace = Math.Max(1, definition.DaysToTrace),
                        DeadlineDays = Math.Max(1, definition.DeadlineDays),
                        SenderSurvivalDays = definition.SenderSurvivalDays,
                        IsTrap = definition.IsTrapOrDeception,
                        IgnoreConsequenceTokens = tokens,
                        SenderAlive = true
                    });
                }
                state.RefreshFingerprint();
                Missions.RestoreState(state);
            }

            public void SetDay(int day)
            {
                Day = Math.Max(0, day);
                FollowUps.SetDay(Day);
            }

            public string Save()
            {
                return RadioSaveCodec.Encode(new RadioSaveState
                {
                    day = Day,
                    distressSignals = Distress.CaptureState(),
                    rescueMissions = Missions.CaptureState(),
                    signalTrust = Trust.CaptureState(),
                    signalFollowUps = FollowUps.CaptureState()
                }, new Ashfall.Core.SystemTextJsonSerializer());
            }

            public static World Load(string json)
            {
                Assert.True(RadioSaveCodec.TryDecode(json,
                    new Ashfall.Core.SystemTextJsonSerializer(), out var state));
                var world = Create();
                world.SetDay(state!.day);
                world.Distress.RestoreState(state.distressSignals);
                world.Missions.RestoreState(state.rescueMissions);
                world.Trust.RestoreState(state.signalTrust);
                world.FollowUps.RestoreState(state.signalFollowUps);
                world.FollowUps.SetDay(world.Day);
                return world;
            }
        }

        private sealed class ReplayObservation
        {
            public int EventDay { get; set; }
            public int FireDay { get; set; }
            public string Text { get; set; } = string.Empty;
            public string Cue { get; set; } = string.Empty;
            public int TrustScore { get; set; }
            public int Answered { get; set; }
            public int Ignored { get; set; }
            public int Traps { get; set; }
            public int Rescues { get; set; }
            public string FiredKey { get; set; } = string.Empty;
        }

        public static IEnumerable<object[]> PopulationRows()
        {
            foreach (CensusRow row in ExpectedRows)
                yield return new object[] { row.Catalog, row.SignalId, row.FollowUpId };
        }

        [Fact]
        public void PopulationCensus_MatchesThePostRemediationAuthoredTable()
        {
            List<CensusRow> actual = LoadAuthoredRows();
            Assert.Equal(24, actual.Select(r => r.SignalId).Distinct(StringComparer.Ordinal).Count());
            Assert.Equal(40, actual.Count);
            Assert.Equal(15, actual.Count(r => r.Trigger.Equals(SignalFollowUpTriggers.Answered, StringComparison.OrdinalIgnoreCase)));
            Assert.Equal(16, actual.Count(r => r.Trigger.Equals(SignalFollowUpTriggers.RescueSuccess, StringComparison.OrdinalIgnoreCase)));
            Assert.Equal(7, actual.Count(r => r.Trigger.Equals(SignalFollowUpTriggers.AmbushEncountered, StringComparison.OrdinalIgnoreCase)));
            Assert.Equal(2, actual.Count(r => r.Trigger.Equals(SignalFollowUpTriggers.Expired, StringComparison.OrdinalIgnoreCase)));
            Assert.DoesNotContain(actual, r => r.Trigger.Equals(SignalFollowUpTriggers.RescueFailed, StringComparison.OrdinalIgnoreCase));
            Assert.Equal(18, actual.Count(r => !string.IsNullOrEmpty(r.Cue)));
            Assert.Equal(22, actual.Count(r => string.IsNullOrEmpty(r.Cue)));

            Assert.Equal(ExpectedRows.Count, actual.Count);
            for (int i = 0; i < ExpectedRows.Count; i++)
            {
                CensusRow expected = ExpectedRows[i];
                CensusRow observed = actual[i];
                Assert.True(expected.Catalog == observed.Catalog && expected.SignalId == observed.SignalId
                    && expected.FollowUpId == observed.FollowUpId && expected.Trigger == observed.Trigger
                    && expected.Delay == observed.Delay && expected.Cue == observed.Cue,
                    $"census drift at row {i}: expected {expected.SignalId}/{expected.FollowUpId}, observed {observed.SignalId}/{observed.FollowUpId}");
            }
        }

        [Theory]
        [MemberData(nameof(PopulationRows))]
        public void EveryAuthoredFollowUp_ReplaysThroughV6ExactlyOnce(
            string catalog, string signalId, string followUpId)
        {
            CensusRow row = LoadAuthoredRows().Single(r => r.Catalog == catalog
                && r.SignalId == signalId && r.FollowUpId == followUpId);
            ReplayRow(row);
        }

        [Fact]
        public void ConsequenceExpiry_AppliesOnceAndStillSchedulesTheAuthoredFollowUp()
        {
            CensusRow row = ExpectedRows.Single(r => r.FollowUpId == "fu_812_5_expired");
            var world = World.Create();
            world.SetDay(1);
            Assert.True(world.Distress.Intercept(row.SignalId, 1));
            Assert.True(world.Missions.RecordSignalHeard(row.SignalId, 1));
            string questId = world.Missions.GetMissionBySignal(row.SignalId)!.QuestId;
            var mission = world.Missions.GetMissionByQuest(questId)!;
            int expiryDay = mission.ExpiryDay;

            world.SetDay(expiryDay);
            world.Missions.TickDaily(expiryDay);
            mission = world.Missions.GetMissionByQuest(questId)!;
            Assert.False(mission.SenderAlive);
            Assert.True(mission.IgnoreConsequenceApplied);
            Assert.Single(world.FollowUps.PendingKeys);
            int eventCountAfterFirstExpiry = world.Fired.Count;

            world.Missions.TickDaily(expiryDay);
            Assert.Equal(eventCountAfterFirstExpiry, world.Fired.Count);
            Assert.True(mission.IgnoreConsequenceApplied);

            int dueDay = expiryDay + row.Delay;
            world.SetDay(dueDay);
            world.FollowUps.TickDaily(dueDay);
            Assert.Single(GetTargetEvents(world, row));
        }

        [Fact]
        public void UndiscoveredPopulationSample_RemainsSilentAndUnscheduled()
        {
            foreach (string signalId in new[]
            {
                "freq_distress_217_4",
                "freq_distress_148_2",
                "freq_distress_55_1",
                "freq_distress_812_5",
                "freq_distress_867_9"
            })
            {
                var world = World.Create();
                world.SetDay(30);
                world.Missions.TickDaily(30);
                world.FollowUps.TickDaily(30);
                Assert.Empty(world.Fired);
                Assert.Empty(world.FollowUps.PendingKeys);
                Assert.Equal(50, world.Trust.Score);
            }
        }

        [Fact]
        public void PopulationReplay_IsStableAcrossTwoRuns()
        {
            string first = PopulationFingerprint();
            string second = PopulationFingerprint();
            Assert.Equal(first, second);
        }

        [Fact]
        public void SameDayPopulationFiresInOrdinalParentOrder()
        {
            var world = World.Create();
            foreach (string signalId in new[] { "freq_distress_401_9", "freq_distress_217_4" })
            {
                world.SetDay(1);
                Assert.True(world.Distress.Intercept(signalId, 1));
                Assert.True(world.Missions.RecordSignalHeard(signalId, 1));
                string questId = world.Missions.GetMissionBySignal(signalId)!.QuestId;
                Assert.True(world.Missions.RecordExpeditionDispatched(questId, "exp_" + signalId));
            }

            world.SetDay(3);
            world.FollowUps.TickDaily(3);
            Assert.Equal(2, world.Fired.Count);
            Assert.Equal("freq_distress_217_4", world.Fired[0].Parent);
            Assert.Equal("freq_distress_401_9", world.Fired[1].Parent);
        }

        [Fact]
        public void RemovedFollowUpIdsInV6PendingStateExpireWithoutPresentation()
        {
            const string key = "freq_distress_55_1:fu_55_1_expired";
            string save = RadioSaveCodec.Encode(new RadioSaveState
            {
                day = 1,
                signalFollowUps = new SignalFollowUpSaveState
                {
                    pending = new List<PendingSignalFollowUpEntry>
                    {
                        new PendingSignalFollowUpEntry
                        {
                            parentSignalId = "freq_distress_55_1",
                            followUpId = "fu_55_1_expired",
                            dueDay = 2
                        }
                    }
                }
            }, new Ashfall.Core.SystemTextJsonSerializer());

            var world = World.Load(save);
            world.SetDay(2);
            world.FollowUps.TickDaily(2);
            Assert.Empty(world.Fired);
            Assert.Contains(key, world.FollowUps.FiredKeys);
            Assert.Empty(world.FollowUps.PendingKeys);
        }

        private static void ReplayRow(CensusRow row)
        {
            World continuous = World.Create();
            int eventDay = DriveToTrigger(continuous, row);
            int dueDay = eventDay + row.Delay;
            string saveBeforeDue = continuous.Save();

            continuous.SetDay(dueDay);
            continuous.FollowUps.TickDaily(dueDay);
            var fired = GetTargetEvents(continuous, row);
            Assert.Single(fired);
            var target = fired[0];
            Assert.Equal(row.SignalId, target.Parent);
            Assert.Equal(row.FollowUpId, target.FollowUp.Id);
            Assert.Equal(dueDay, target.Day);
            Assert.Equal(row.Text, target.FollowUp.Text);
            Assert.Equal(row.Cue, target.FollowUp.AudioCue);
            string key = DistressFollowUpScheduler.Key(row.SignalId, row.FollowUpId);
            Assert.Contains(key, continuous.FollowUps.FiredKeys);
            Assert.Equal(1, continuous.FollowUps.FiredKeys.Count(k => k == key));

            continuous.FollowUps.TickDaily(dueDay);
            continuous.FollowUps.TickDaily(dueDay + 30);
            Assert.Single(GetTargetEvents(continuous, row));

            World interrupted = World.Load(saveBeforeDue);
            interrupted.SetDay(dueDay);
            interrupted.FollowUps.TickDaily(dueDay);
            var interruptedTarget = GetTargetEvents(interrupted, row);
            Assert.Single(interruptedTarget);
            Assert.Equal(target.Day, interruptedTarget[0].Day);
            Assert.Equal(target.FollowUp.Text, interruptedTarget[0].FollowUp.Text);
            Assert.Equal(target.FollowUp.AudioCue, interruptedTarget[0].FollowUp.AudioCue);
            Assert.Equal(continuous.Trust.Score, interrupted.Trust.Score);
            Assert.Equal(continuous.Trust.SignalsAnswered, interrupted.Trust.SignalsAnswered);
            Assert.Equal(continuous.Trust.SignalsIgnored, interrupted.Trust.SignalsIgnored);
            Assert.Equal(continuous.Trust.TrapsFallenFor, interrupted.Trust.TrapsFallenFor);
            Assert.Equal(continuous.Trust.RescuesSuccessful, interrupted.Trust.RescuesSuccessful);

            AssertTrustForTrigger(continuous, row.Trigger);

            string saveAfterFire = continuous.Save();
            World afterReload = World.Load(saveAfterFire);
            afterReload.SetDay(dueDay + 30);
            afterReload.FollowUps.TickDaily(dueDay + 30);
            Assert.Empty(afterReload.Fired);
        }

        private static List<(string Parent, SignalFollowUpDefinition FollowUp, int Day)> GetTargetEvents(
            World world, CensusRow row)
        {
            return world.Fired
                .Where(x => x.Parent == row.SignalId && x.FollowUp.Id == row.FollowUpId)
                .ToList();
        }

        private static int DriveToTrigger(World world, CensusRow row)
        {
            world.SetDay(1);
            Assert.True(world.Distress.Intercept(row.SignalId, 1));
            Assert.True(world.Missions.RecordSignalHeard(row.SignalId, 1));
            string questId = world.Missions.GetMissionBySignal(row.SignalId)!.QuestId;

            if (row.Trigger.Equals(SignalFollowUpTriggers.Answered, StringComparison.OrdinalIgnoreCase))
            {
                world.SetDay(2);
                Assert.True(world.Missions.RecordExpeditionDispatched(questId, "exp_" + row.SignalId));
                return 2;
            }

            if (row.Trigger.Equals(SignalFollowUpTriggers.RescueSuccess, StringComparison.OrdinalIgnoreCase))
            {
                world.SetDay(1);
                Assert.True(world.Missions.RecordExpeditionDispatched(questId, "exp_" + row.SignalId));
                world.SetDay(2);
                Assert.Equal(DistressRescueMissionStage.TerminalRescued,
                    world.Missions.RecordDestinationReached(questId, 2));
                return 2;
            }

            if (row.Trigger.Equals(SignalFollowUpTriggers.AmbushEncountered, StringComparison.OrdinalIgnoreCase))
            {
                world.SetDay(1);
                Assert.True(world.Missions.RecordExpeditionDispatched(questId, "exp_" + row.SignalId));
                world.SetDay(2);
                Assert.Equal(DistressRescueMissionStage.TerminalAmbush,
                    world.Missions.RecordDestinationReached(questId, 2));
                return 2;
            }

            Assert.Equal(SignalFollowUpTriggers.Expired, row.Trigger,
                StringComparer.OrdinalIgnoreCase);
            var mission = world.Missions.GetMissionByQuest(questId)!;
            int expiryDay = mission.ExpiryDay;
            world.SetDay(expiryDay);
            world.Missions.TickDaily(expiryDay);
            return expiryDay;
        }

        private static void AssertTrustForTrigger(World world, string trigger)
        {
            if (trigger.Equals(SignalFollowUpTriggers.Answered, StringComparison.OrdinalIgnoreCase))
            {
                Assert.Equal(52, world.Trust.Score);
                Assert.Equal(1, world.Trust.SignalsAnswered);
                return;
            }
            if (trigger.Equals(SignalFollowUpTriggers.RescueSuccess, StringComparison.OrdinalIgnoreCase))
            {
                Assert.Equal(57, world.Trust.Score);
                Assert.Equal(1, world.Trust.SignalsAnswered);
                Assert.Equal(1, world.Trust.RescuesSuccessful);
                return;
            }
            if (trigger.Equals(SignalFollowUpTriggers.AmbushEncountered, StringComparison.OrdinalIgnoreCase))
            {
                Assert.Equal(47, world.Trust.Score);
                Assert.Equal(1, world.Trust.SignalsAnswered);
                Assert.Equal(1, world.Trust.TrapsFallenFor);
                return;
            }
            Assert.Equal(48, world.Trust.Score);
            Assert.Equal(1, world.Trust.SignalsIgnored);
        }

        private static string PopulationFingerprint()
        {
            var lines = new List<string>();
            foreach (CensusRow row in LoadAuthoredRows())
            {
                var world = World.Create();
                int eventDay = DriveToTrigger(world, row);
                int dueDay = eventDay + row.Delay;
                world.SetDay(dueDay);
                world.FollowUps.TickDaily(dueDay);
                string pending = string.Join(",", world.FollowUps.PendingKeys.OrderBy(k => k, StringComparer.Ordinal));
                string fired = string.Join(",", world.FollowUps.FiredKeys.OrderBy(k => k, StringComparer.Ordinal));
                lines.Add($"{row.SignalId}|{row.FollowUpId}|d={dueDay}|trust={world.Trust.Score}|a={world.Trust.SignalsAnswered}|i={world.Trust.SignalsIgnored}|t={world.Trust.TrapsFallenFor}|r={world.Trust.RescuesSuccessful}|pending={pending}|fired={fired}");
            }
            return StableHash.Of(string.Join("\n", lines)).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        private static List<CensusRow> LoadAuthoredRows()
        {
            var rows = new List<CensusRow>();
            string dataDir = DataDirectory();
            foreach (string catalog in new[] { PrimaryCatalog, ExpansionCatalog })
            {
                using var doc = JsonDocument.Parse(File.ReadAllText(Path.Combine(dataDir, catalog)));
                if (!doc.RootElement.TryGetProperty("radio_broadcasts", out var broadcasts)) continue;
                foreach (var broadcast in broadcasts.EnumerateArray())
                {
                    string signalId = broadcast.GetProperty("frequency_id").GetString() ?? string.Empty;
                    if (!broadcast.TryGetProperty("follow_up_signals", out var followUps)) continue;
                    foreach (var followUp in followUps.EnumerateArray())
                    {
                        var row = new CensusRow(
                            catalog,
                            signalId,
                            followUp.GetProperty("id").GetString() ?? string.Empty,
                            followUp.GetProperty("trigger_condition").GetString() ?? string.Empty,
                            followUp.GetProperty("delay_days").GetInt32(),
                            followUp.TryGetProperty("audio_cue", out var cue) && cue.ValueKind == JsonValueKind.String
                                ? cue.GetString() ?? string.Empty
                                : string.Empty)
                        {
                            Text = followUp.GetProperty("text").GetString() ?? string.Empty
                        };
                        rows.Add(row);
                    }
                }
            }
            return rows;
        }

        private static string DataDirectory()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new InvalidOperationException("StreamingAssets/Data directory was not found.");
        }
    }
}
