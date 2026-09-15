// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Task 4 — sender survival time-dependent model. Sender death day is
    /// computed from first-heard day; the actual arrival day decides
    /// live-rescue vs remains; transitions are idempotent and persist.
    /// </summary>
    public sealed class RescueSignalSenderSurvivalTests
    {
        private readonly DistressRescueMissionManager _manager = new DistressRescueMissionManager();

        // Mechanic: heard + N, survival 5 → death day = heard + 5.
        private const string MechanicSignal = "freq_distress_88_3";
        private const string MechanicQuest = "quest_distress_trapped_mechanic";

        [Fact]
        public void DeathDay_ComputedFromFirstHeardDay()
        {
            Assert.True(_manager.RecordSignalHeard(MechanicSignal, 5));
            var m = _manager.GetMissionByQuest(MechanicQuest)!;
            Assert.True(m.SenderAlive);
            Assert.Equal(10, m.SenderDeathDay); // 5 + survival 5
        }

        [Fact]
        public void Arrival_BeforeDeathDay_AliveRescue()
        {
            _manager.RecordSignalHeard(MechanicSignal, 5);
            _manager.RecordExpeditionDispatched(MechanicQuest, "exp_a");
            var stage = _manager.RecordDestinationReached(MechanicQuest, 9);
            Assert.Equal(DistressRescueMissionStage.TerminalRescued, stage);
            var m = _manager.GetMissionByQuest(MechanicQuest)!;
            Assert.True(m.SenderAlive);
            Assert.True(m.ArrivalResolved);
        }

        [Fact]
        public void Arrival_ExactlyOnDeathDay_Dead()
        {
            _manager.RecordSignalHeard(MechanicSignal, 5);
            _manager.RecordExpeditionDispatched(MechanicQuest, "exp_b");
            var stage = _manager.RecordDestinationReached(MechanicQuest, 10);
            Assert.Equal(DistressRescueMissionStage.TerminalFailed, stage);
            var m = _manager.GetMissionByQuest(MechanicQuest)!;
            Assert.False(m.SenderAlive);
            Assert.Contains("died on Day 10", m.OutcomeSummary);
        }

        [Fact]
        public void Arrival_AfterDeathDay_Dead()
        {
            _manager.RecordSignalHeard(MechanicSignal, 5);
            _manager.RecordExpeditionDispatched(MechanicQuest, "exp_c");
            var stage = _manager.RecordDestinationReached(MechanicQuest, 14);
            Assert.Equal(DistressRescueMissionStage.TerminalFailed, stage);
            Assert.False(_manager.GetMissionByQuest(MechanicQuest)!.SenderAlive);
        }

        [Fact]
        public void DispatchedBeforeDeath_ArrivedAfterDeath_DeadBranch()
        {
            // Dispatch day 7 (sender alive), travel 2 days, arrival day 9 ≥ death day 8.
            _manager.RecordSignalHeard(MechanicSignal, 3); // death day 8
            _manager.RecordExpeditionDispatched(MechanicQuest, "exp_late");
            var stage = _manager.RecordDestinationReached(MechanicQuest, 9);
            Assert.Equal(DistressRescueMissionStage.TerminalFailed, stage);
            Assert.False(_manager.GetMissionByQuest(MechanicQuest)!.SenderAlive);
        }

        [Fact]
        public void Replay_DoesNotRecomputeDeathDayLater()
        {
            _manager.RecordSignalHeard(MechanicSignal, 5);
            // Replaying the transmission never recomputes a later death day.
            Assert.False(_manager.RecordSignalHeard(MechanicSignal, 9));
            Assert.Equal(10, _manager.GetMissionByQuest(MechanicQuest)!.SenderDeathDay);
        }

        [Fact]
        public void SenderState_SurvivesSaveLoad()
        {
            _manager.RecordSignalHeard(MechanicSignal, 5);
            var fresh = new DistressRescueMissionManager();
            fresh.RestoreState(_manager.CaptureState());
            var m = fresh.GetMissionByQuest(MechanicQuest)!;
            Assert.True(m.SenderAlive);
            Assert.Equal(10, m.SenderDeathDay);
            Assert.Equal(5, m.SenderSurvivalDays);
        }

        [Fact]
        public void StaleSignal_HasNoLiveSenderDeathProcessing()
        {
            // family_shelter (survival 0 = no live-sender model) keeps legacy
            // deadline behavior and never gets a computed death day.
            _manager.RecordSignalHeard("freq_distress_445_2", 1);
            var m = _manager.GetMissionByQuest("quest_distress_family_shelter")!;
            Assert.Equal(0, m.SenderDeathDay);
            _manager.TickDaily(6);
            Assert.Equal(DistressRescueMissionStage.TerminalFailed, m.Stage);
            Assert.DoesNotContain("died on Day", m.OutcomeSummary);
        }

        [Fact]
        public void LiveRescue_ClaimsRewardsExactlyOnce()
        {
            _manager.RecordSignalHeard(MechanicSignal, 5);
            _manager.RecordExpeditionDispatched(MechanicQuest, "exp_d");
            _manager.RecordDestinationReached(MechanicQuest, 9);
            var (items1, rep1) = _manager.ClaimIdempotentRewards(MechanicQuest);
            var (items2, rep2) = _manager.ClaimIdempotentRewards(MechanicQuest);
            Assert.NotEmpty(items1);
            Assert.Equal(5, rep1);
            Assert.Empty(items2); // duplicate recruit/grant guarded
            Assert.Equal(0, rep2);
        }

        [Fact]
        public void DeadArrival_GrantsSalvageOnce_NeverRescueReputation()
        {
            // Plan §8.5: a dead-arrival recovery grants the remains/salvage
            // outcome exactly once — with zero reputation (never the rescue
            // standing, never a recruit).
            _manager.RecordSignalHeard(MechanicSignal, 5);
            _manager.RecordExpeditionDispatched(MechanicQuest, "exp_e");
            _manager.RecordDestinationReached(MechanicQuest, 12); // past death day
            var (items, rep) = _manager.ClaimIdempotentRewards(MechanicQuest);
            Assert.NotEmpty(items);  // salvage = the mission's authored goods
            Assert.Equal(0, rep);    // never rescue reputation

            var (items2, rep2) = _manager.ClaimIdempotentRewards(MechanicQuest);
            Assert.Empty(items2);    // claimed exactly once
            Assert.Equal(0, rep2);
        }

        [Fact]
        public void ArrivalResolution_IsIdempotentAgainstRepeatCallbacks()
        {
            _manager.RecordSignalHeard(MechanicSignal, 5);
            _manager.RecordExpeditionDispatched(MechanicQuest, "exp_f");
            var first = _manager.RecordDestinationReached(MechanicQuest, 9);
            var second = _manager.RecordDestinationReached(MechanicQuest, 9);
            var third = _manager.RecordDestinationReached(MechanicQuest, 20); // late callback
            Assert.Equal(DistressRescueMissionStage.TerminalRescued, first);
            Assert.Equal(first, second);
            Assert.Equal(first, third);
        }

        [Fact]
        public void ResolvedRescue_StopsFutureSurvivalConsequences()
        {
            // Arrival resolves live before the deadline: a later ignore-consequence
            // tick must never fire and never kill the (rescued) sender.
            var log = new List<string>();
            _manager.OnIgnoreConsequence += (mission, tokens, factionId, day) => log.Add(mission.QuestId);
            _manager.RecordSignalHeard(MechanicSignal, 5);
            _manager.RecordExpeditionDispatched(MechanicQuest, "exp_g");
            _manager.RecordDestinationReached(MechanicQuest, 9);
            _manager.TickDaily(20);
            _manager.TickDaily(21);
            Assert.Empty(log);
            Assert.True(_manager.GetMissionByQuest(MechanicQuest)!.SenderAlive);
        }

        [Fact]
        public void SaveReloadWhileEnRoute_PreservesFinalOutcome()
        {
            _manager.RecordSignalHeard(MechanicSignal, 5);
            _manager.RecordExpeditionDispatched(MechanicQuest, "exp_enroute");

            var fresh = new DistressRescueMissionManager();
            fresh.RestoreState(_manager.CaptureState());
            Assert.Equal("exp_enroute", fresh.GetMissionByQuest(MechanicQuest)!.ExpeditionId);

            var stage = fresh.RecordDestinationReached(MechanicQuest, 12); // past death day 10
            Assert.Equal(DistressRescueMissionStage.TerminalFailed, stage);
            Assert.False(fresh.GetMissionByQuest(MechanicQuest)!.SenderAlive);

            // Reload again after resolution — no re-resolution, and the salvage
            // claim survives reload exactly once (plan §8.5 / §15).
            var again = new DistressRescueMissionManager();
            again.RestoreState(fresh.CaptureState());
            Assert.Equal(DistressRescueMissionStage.TerminalFailed,
                again.RecordDestinationReached(MechanicQuest, 13));
            var (items, rep) = again.ClaimIdempotentRewards(MechanicQuest);
            Assert.NotEmpty(items);  // salvage granted once across all reloads
            Assert.Equal(0, rep);
            var (items2, rep2) = again.ClaimIdempotentRewards(MechanicQuest);
            Assert.Empty(items2);
            Assert.Equal(0, rep2);
        }
    }
}
