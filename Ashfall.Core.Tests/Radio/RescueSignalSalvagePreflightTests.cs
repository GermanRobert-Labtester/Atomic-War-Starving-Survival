// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Expansion wave — dead-arrival salvage/recovery (plan §8.5) and the
    /// structured authenticity dispatch preflight (plan §10).
    /// </summary>
    public sealed class RescueSignalSalvagePreflightTests
    {
        private readonly DistressRescueMissionManager _manager = new DistressRescueMissionManager();

        // ── Dead-arrival salvage (plan §8.5) ─────────────────────────────────

        [Fact]
        public void DeadArrival_GrantsSalvageOnce_NoReputation()
        {
            _manager.RecordSignalHeard("freq_distress_156_8", 1); // trader, death day 4
            _manager.RecordExpeditionDispatched("quest_distress_injured_trader", "exp_salv");
            var stage = _manager.RecordDestinationReached("quest_distress_injured_trader", 5);
            Assert.Equal(DistressRescueMissionStage.TerminalFailed, stage);

            var (items, rep) = _manager.ClaimIdempotentRewards("quest_distress_injured_trader");
            Assert.NotEmpty(items); // the trader's goods are recovered
            Assert.Equal(0, rep);   // a late arrival earns no standing

            var (items2, rep2) = _manager.ClaimIdempotentRewards("quest_distress_injured_trader");
            Assert.Empty(items2); // recovery claimed exactly once
            Assert.Equal(0, rep2);
        }

        [Fact]
        public void LegacyDeadlineExpiry_NeverGrantsSalvage()
        {
            // family_shelter: no survival model, no expedition — the legacy
            // "beacon dead" expiry grants nothing.
            _manager.RecordSignalHeard("freq_distress_445_2", 1);
            _manager.TickDaily(6);
            var m = _manager.GetMissionByQuest("quest_distress_family_shelter")!;
            Assert.Equal(DistressRescueMissionStage.TerminalFailed, m.Stage);
            var (items, rep) = _manager.ClaimIdempotentRewards("quest_distress_family_shelter");
            Assert.Empty(items);
            Assert.Equal(0, rep);
        }

        [Fact]
        public void IgnoredSenderDeath_WithoutExpedition_GrantsNothing()
        {
            // Ignored calls kill the sender but grant no salvage until an
            // expedition actually reaches the site.
            _manager.RecordSignalHeard("freq_distress_88_3", 2);
            _manager.TickDaily(8); // past deadline → expired + sender dead
            var m = _manager.GetMissionByQuest("quest_distress_trapped_mechanic")!;
            Assert.False(m.SenderAlive);
            Assert.False(m.ArrivalResolved);
            var (items, rep) = _manager.ClaimIdempotentRewards("quest_distress_trapped_mechanic");
            Assert.Empty(items);
            Assert.Equal(0, rep);
        }

        [Fact]
        public void SalvageClaim_SurvivesSaveLoad_OnceOnly()
        {
            _manager.RecordSignalHeard("freq_distress_156_8", 1);
            _manager.RecordExpeditionDispatched("quest_distress_injured_trader", "exp_salv_rt");
            _manager.RecordDestinationReached("quest_distress_injured_trader", 5);

            var fresh = new DistressRescueMissionManager();
            fresh.RestoreState(_manager.CaptureState());
            var (items, rep) = fresh.ClaimIdempotentRewards("quest_distress_injured_trader");
            Assert.NotEmpty(items);
            Assert.Equal(0, rep);

            // Reload again after the claim — never re-granted.
            var again = new DistressRescueMissionManager();
            again.RestoreState(fresh.CaptureState());
            var (items2, rep2) = again.ClaimIdempotentRewards("quest_distress_injured_trader");
            Assert.Empty(items2);
            Assert.Equal(0, rep2);
        }

        // ── Dispatch preflight (plan §10) ────────────────────────────────────

        private static void InjectDetectedTrap(DistressRescueMission m, string authenticityAssessment)
        {
            // Simulates a persisted high-skill detection result.
            m.AuthenticityChecked = true;
            m.AuthenticityAssessment = authenticityAssessment == "false_flag"
                ? (int)SignalAuthenticityCategory.FalseFlag
                : (int)SignalAuthenticityCategory.Trap;
            m.AssessmentThreatDetected = true;
        }

        [Fact]
        public void Preflight_UnknownSignal_ReturnsNull()
        {
            Assert.Null(_manager.GetDispatchPreflight("freq_unknown_999"));
        }

        [Fact]
        public void Preflight_UnanalyzedGenuine_RecommendsNormalDispatch()
        {
            _manager.RecordSignalHeard("freq_distress_88_3", 2);
            var p = _manager.GetDispatchPreflight("freq_distress_88_3")!;
            Assert.False(p.Analyzed);
            Assert.Equal(RescueDispatchRecommendation.DispatchRescue, p.Recommendation);
        }

        [Fact]
        public void Preflight_DetectedTrap_WarnsButDoesNotGateDispatch()
        {
            _manager.RecordSignalHeard("freq_distress_192_4", 2);
            InjectDetectedTrap(_manager.GetMissionByQuest("quest_distress_raider_trap")!, "trap");
            var p = _manager.GetDispatchPreflight("freq_distress_192_4")!;
            Assert.Equal(RescueDispatchRecommendation.DispatchWithWarning, p.Recommendation);
            Assert.True(p.ThreatDetected);
            // Agency preserved: dispatch still goes through the normal pipeline.
            Assert.True(_manager.RecordExpeditionDispatched("quest_distress_raider_trap", "exp_risky"));
        }

        [Fact]
        public void Preflight_DetectedFalseFlag_Warns()
        {
            _manager.RecordSignalHeard("freq_distress_192_4", 2);
            var m = _manager.GetMissionByQuest("quest_distress_raider_trap")!;
            m.AuthenticityChecked = true;
            m.AuthenticityAssessment = (int)SignalAuthenticityCategory.FalseFlag;
            m.AssessmentThreatDetected = true;
            var p = _manager.GetDispatchPreflight("freq_distress_192_4")!;
            Assert.Equal(RescueDispatchRecommendation.DispatchWithWarning, p.Recommendation);
            Assert.Contains("false flag", p.Note);
        }

        [Fact]
        public void Preflight_ExpiredSignal_RecommendsRecoveryInvestigation()
        {
            _manager.RecordSignalHeard("freq_distress_88_3", 2);
            _manager.TickDaily(8);
            var p = _manager.GetDispatchPreflight("freq_distress_88_3")!;
            Assert.True(p.Expired);
            Assert.False(p.SenderAlive);
            Assert.Equal(RescueDispatchRecommendation.RecoveryInvestigation, p.Recommendation);
        }

        [Fact]
        public void Preflight_StaleConfirmed_RecommendsInvestigation()
        {
            _manager.RecordSignalHeard("freq_distress_88_3", 2);
            var m = _manager.GetMissionByQuest("quest_distress_trapped_mechanic")!;
            m.AuthenticityChecked = true;
            m.AuthenticityAssessment = (int)SignalAuthenticityCategory.Stale;
            m.AssessmentStalenessDetected = true;
            var p = _manager.GetDispatchPreflight("freq_distress_88_3")!;
            Assert.Equal(RescueDispatchRecommendation.RecoveryInvestigation, p.Recommendation);
        }

        [Fact]
        public void Preflight_TerminalMission_NotApplicable()
        {
            _manager.RecordSignalHeard("freq_distress_88_3", 2);
            _manager.RecordExpeditionDispatched("quest_distress_trapped_mechanic", "exp_pf");
            _manager.RecordDestinationReached("quest_distress_trapped_mechanic", 6);
            var p = _manager.GetDispatchPreflight("freq_distress_88_3")!;
            Assert.Equal(RescueDispatchRecommendation.NotApplicable, p.Recommendation);
        }

        [Fact]
        public void Preflight_IsSideEffectFreeProjection()
        {
            _manager.RecordSignalHeard("freq_distress_156_8", 1);
            var before = DistressMissionSaveState.ComputeFingerprint(_manager.CaptureState());
            for (int i = 0; i < 3; i++)
                _ = _manager.GetDispatchPreflight("freq_distress_156_8");
            var after = DistressMissionSaveState.ComputeFingerprint(_manager.CaptureState());
            Assert.Equal(before, after);
        }

        [Fact]
        public void Preflight_AvailableForEveryRegisteredRescueSignal()
        {
            foreach (var mission in _manager.AllMissions)
                Assert.NotNull(_manager.GetDispatchPreflight(mission.SignalId));
        }
    }
}
