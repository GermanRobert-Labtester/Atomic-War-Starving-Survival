// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Rescue-signal runtime persistence (Wave 1): the DistressRescueMission
    /// state — stages, deadlines, expedition association, claimed reward
    /// receipts, sender survival, ignore consequence, authenticity — rides the
    /// checksummed RadioSaveState (V4) and survives a full codec round-trip.
    /// V3 saves migrate cleanly with neutral mission defaults.
    /// </summary>
    public sealed class RescueSignalRuntimePersistenceTests
    {
        private static IJsonSerializer Serializer => new SystemTextJsonSerializer();

        /// <summary>Manager wired with the flagship signal definitions so
        /// authenticity analysis can resolve its ground truth.</summary>
        private static DistressRescueMissionManager ManagerWithDefs()
        {
            var distress = new RadioDistressSystem();
            distress.RegisterSignal(new DistressSignalDefinition
            {
                FrequencyId = "freq_distress_88_3",
                Authenticity = "genuine",
                OutcomeTypeStr = "survivor_community"
            });
            return new DistressRescueMissionManager(null, distress);
        }

        [Fact]
        public void MissionState_RoundTrips_ThroughRadioSaveState()
        {
            var source = ManagerWithDefs();
            source.RecordSignalHeard("freq_distress_88_3", 2);
            source.RecordSignalIdentified("freq_distress_88_3");
            source.RecordExpeditionDispatched("quest_distress_trapped_mechanic", "exp_rt_01");
            source.RecordAuthenticityCheck("freq_distress_88_3", "survivor_rt", 4, 99);
            source.RecordDestinationReached("quest_distress_trapped_mechanic", 6); // before death day 7
            source.ClaimIdempotentRewards("quest_distress_trapped_mechanic");

            var state = new RadioSaveState { day = 9, currentFrequency = 88.3f };
            state.rescueMissions = source.CaptureState();
            string json = RadioSaveCodec.Encode(state, Serializer);

            Assert.True(RadioSaveCodec.TryDecode(json, Serializer, out var decoded));
            Assert.NotNull(decoded.rescueMissions);

            var restored = new DistressRescueMissionManager();
            restored.RestoreState(decoded.rescueMissions);
            var m = restored.GetMissionByQuest("quest_distress_trapped_mechanic")!;
            Assert.Equal(DistressRescueMissionStage.TerminalRescued, m.Stage);
            Assert.Equal("exp_rt_01", m.ExpeditionId);
            Assert.Equal(2, m.InterceptedDay);
            Assert.True(m.ArrivalResolved);
            Assert.True(m.AuthenticityChecked);
            Assert.True(restored.IsReceiptClaimed("quest_distress_trapped_mechanic", "freq_distress_88_3"));
        }

        [Fact]
        public void IgnoreConsequenceState_RoundTrips_ExactlyOnceAfterReload()
        {
            var source = new DistressRescueMissionManager();
            source.RecordSignalHeard("freq_distress_156_8", 1);
            source.TickDaily(5); // expired + sender death applied once

            var state = new RadioSaveState { day = 5, currentFrequency = 156.8f };
            state.rescueMissions = source.CaptureState();
            string json = RadioSaveCodec.Encode(state, Serializer);
            Assert.True(RadioSaveCodec.TryDecode(json, Serializer, out var decoded));

            var restored = new DistressRescueMissionManager();
            var log = new List<string>();
            restored.OnIgnoreConsequence += (mission, tokens, factionId, day) => log.Add(mission.QuestId);
            restored.RestoreState(decoded.rescueMissions);
            restored.TickDaily(10);
            restored.TickDaily(11);
            Assert.Empty(log); // consequence never replays after restore
            var m = restored.GetMissionByQuest("quest_distress_injured_trader")!;
            Assert.True(m.IgnoreConsequenceApplied);
            Assert.Equal(5, m.IgnoreConsequenceAppliedDay);
            Assert.False(m.SenderAlive);
        }

        [Fact]
        public void V3Payload_MigratesToV4_WithNeutralMissionDefaults()
        {
            // A frozen V3 shape (no rescueMissions field) must migrate cleanly:
            // old saves are never retroactively punished or double-resolved.
            var v3 = new RadioSaveStateFrozenV3
            {
                day = 33,
                currentFrequency = 88.5f,
                history = new List<RadioInterceptEntry>
                {
                    new RadioInterceptEntry
                    {
                        factionId = "faction_civil_defense",
                        callsign = "CIVIL DEFENSE 88.5",
                        frequencyMhz = 88.5f,
                        kind = 0,
                        message = "Morning weather clear.",
                        signalStrength = 7,
                        day = 33
                    }
                },
                playedBroadcastKeys = new List<string> { "33:88.50:legacy" },
                distressSignals = new List<DistressSignalSaveEntry>
                {
                    new DistressSignalSaveEntry
                    {
                        signalId = "freq_distress_88_3",
                        status = (int)DistressSignalStatus.Intercepted,
                        interceptedDay = 33,
                        daysRemaining = 5
                    }
                }
            };
            v3.Checksum = SaveChecksum.Compute(v3);
            string json = Serializer.Serialize(v3);

            Assert.True(RadioSaveCodec.TryDecode(json, Serializer, out var decoded));
            Assert.Equal(RadioSaveCodec.CurrentSaveVersion, decoded.saveVersion);
            Assert.Null(decoded.rescueMissions);
            Assert.Equal(33, decoded.day);

            var fresh = new DistressRescueMissionManager();
            fresh.RestoreState(decoded.rescueMissions); // null → neutral defaults
            var m = fresh.GetMissionByQuest("quest_distress_trapped_mechanic")!;
            Assert.Equal(DistressRescueMissionStage.None, m.Stage);
            Assert.False(m.Expired);
            Assert.False(m.IgnoreConsequenceApplied);
        }

        [Fact]
        public void LegacyManagerState_MissingNewFields_DeserializesNeutral()
        {
            // A pre-extension DistressMissionSaveState JSON (no runtime-extension
            // fields) deserializes into safe legacy behavior.
            var legacy = new DistressMissionSaveState
            {
                Version = 1,
                Missions = new List<DistressRescueMission>
                {
                    new DistressRescueMission
                    {
                        QuestId = "quest_distress_trapped_mechanic",
                        SignalId = "freq_distress_88_3",
                        Stage = DistressRescueMissionStage.Heard,
                        InterceptedDay = 2
                    }
                }
            };
            string json = Serializer.Serialize(legacy);
            var decoded = Serializer.Deserialize<DistressMissionSaveState>(json);
            Assert.NotNull(decoded);

            var m = new DistressRescueMissionManager();
            m.RestoreState(decoded);
            var mission = m.GetMissionByQuest("quest_distress_trapped_mechanic")!;
            Assert.Equal(DistressRescueMissionStage.Heard, mission.Stage);
            Assert.False(mission.AuthenticityChecked);
            Assert.False(mission.IgnoreConsequenceApplied);
            Assert.False(mission.ArrivalResolved);
        }

        [Fact]
        public void ChecksumCoversMissionState_TamperedMissionFailsDecode()
        {
            var source = new DistressRescueMissionManager();
            source.RecordSignalHeard("freq_distress_88_3", 2);
            var state = new RadioSaveState { day = 2, currentFrequency = 88.3f };
            state.rescueMissions = source.CaptureState();
            string json = RadioSaveCodec.Encode(state, Serializer);

            // Tamper with the mission payload — checksum must reject.
            var tampered = RadioSaveCodec.TryDecode(json, Serializer, out var decoded)
                ? decoded!
                : throw new InvalidOperationException("baseline decode failed");
            tampered.rescueMissions!.Missions[0].Stage = DistressRescueMissionStage.TerminalRescued;
            string tamperedJson = Serializer.Serialize(tampered);
            Assert.False(RadioSaveCodec.TryDecode(tamperedJson, Serializer, out _));
        }
    }
}
