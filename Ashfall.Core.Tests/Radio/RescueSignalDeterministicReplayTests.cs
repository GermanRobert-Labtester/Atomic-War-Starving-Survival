// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Wave 7 cross-system hardening — same-day deadline/death boundary,
    /// trap-detection → dispatch → ambush interaction, and a deterministic
    /// continuous-vs-mid-reload replay over the full signal runtime.
    /// </summary>
    public sealed class RescueSignalDeterministicReplayTests
    {
        private const int SeedBase = 20260913;

        // ── Same-day deadline == sender-death boundary (plan §9) ─────────────

        [Fact]
        public void SameDayDeadlineAndSenderDeath_EachResolveExactlyOnce()
        {
            var m = new DistressRescueMissionManager();
            var log = new List<string>();
            m.OnIgnoreConsequence += (mission, tokens, factionId, day) => log.Add($"{mission.QuestId}:{day}");

            // Military patrol: heard day 1, deadline 6 == sender death day 6.
            m.RecordSignalHeard("freq_distress_901_2", 1);
            var mission = m.GetMissionByQuest("quest_distress_military_patrol")!;
            Assert.Equal(6, mission.ExpiryDay);
            Assert.Equal(6, mission.SenderDeathDay);

            m.TickDaily(6); // boundary day: both fire, exactly once
            Assert.True(mission.Expired);
            Assert.True(mission.IgnoreConsequenceApplied);
            Assert.False(mission.SenderAlive);
            Assert.Equal(6, mission.SenderDeathDay); // death recorded once
            Assert.Single(log);

            m.TickDaily(7);
            m.TickDaily(8);
            Assert.Single(log);                       // no duplicate consequence
            Assert.Equal(6, mission.SenderDeathDay);  // no duplicate death effects

            // A later recovery expedition finds the patrol's remains exactly once.
            Assert.True(m.RecordExpeditionDispatched("quest_distress_military_patrol", "exp_recov"));
            Assert.Equal(DistressRescueMissionStage.TerminalFailed,
                m.RecordDestinationReached("quest_distress_military_patrol", 9));
            var (items, rep) = m.ClaimIdempotentRewards("quest_distress_military_patrol");
            Assert.NotEmpty(items);
            Assert.Equal(0, rep);
        }

        // ── Trap detection → dispatch → ambush (plan §10 / §17) ──────────────

        [Fact]
        public void DetectedTrap_DispatchRemainsPlayersChoice_AmbushPipelineRuns()
        {
            var m = new DistressRescueMissionManager();
            m.RecordSignalHeard("freq_distress_192_4", 2);
            var mission = m.GetMissionByQuest("quest_distress_raider_trap")!;
            mission.AuthenticityChecked = true;
            mission.AuthenticityAssessment = (int)SignalAuthenticityCategory.Trap;
            mission.AssessmentThreatDetected = true;

            // Player chooses to dispatch despite the warning.
            Assert.True(m.RecordExpeditionDispatched("quest_distress_raider_trap", "exp_bait"));
            var stage = m.RecordDestinationReached("quest_distress_raider_trap", 5);
            Assert.Equal(DistressRescueMissionStage.TerminalAmbush, stage);
            Assert.True(m.ResolveAmbushSurvived("quest_distress_raider_trap", "Bait ambush repelled."));
            Assert.Equal(DistressRescueMissionStage.TerminalSurvived,
                m.GetMissionByQuest("quest_distress_raider_trap")!.Stage);

            // Survival of a trap earns no reputation (authored 0).
            var (items, rep) = m.ClaimIdempotentRewards("quest_distress_raider_trap");
            Assert.Empty(items);
            Assert.Equal(0, rep);
        }

        // ── Deterministic replay: continuous == mid-reload, field by field ───

        /// <summary>Manager wired with the mechanic's ground-truth definition
        /// so the authenticity call resolves instead of silently no-oping.</summary>
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

        /// <summary>Runs the canonical rescue scenario; returns the effective
        /// manager (mid-reload continues on the restored runtime).</summary>
        private static DistressRescueMissionManager RunScenario(
            DistressRescueMissionManager m, bool reloadAfterDay3, bool withAuthenticity)
        {
            m.RecordSignalHeard("freq_distress_88_3", 2);          // mechanic, death day 7
            m.TickDaily(3);
            if (withAuthenticity)
                m.RecordAuthenticityCheck("freq_distress_88_3", "survivor_replay", 3, SeedBase);
            m.TickDaily(4);
            m.RecordExpeditionDispatched("quest_distress_trapped_mechanic", "exp_replay");
            m.TickDaily(5);

            if (reloadAfterDay3)
            {
                var mid = m.CaptureState();
                var restored = ManagerWithDefs();
                restored.RestoreState(mid);
                m = restored; // replay continues on the restored runtime
            }

            m.TickDaily(6);
            var stage = m.RecordDestinationReached("quest_distress_trapped_mechanic", 6); // before death
            Assert.Equal(DistressRescueMissionStage.TerminalRescued, stage);
            var (items, rep) = m.ClaimIdempotentRewards("quest_distress_trapped_mechanic");
            Assert.NotEmpty(items);
            Assert.Equal(5, rep);
            return m;
        }

        [Fact]
        public void ContinuousRun_EqualsMidReloadReplay_FieldByField()
        {
            var continuous = RunScenario(ManagerWithDefs(), reloadAfterDay3: false, withAuthenticity: true);
            var replay = RunScenario(ManagerWithDefs(), reloadAfterDay3: true, withAuthenticity: true);

            var fpA = DistressMissionSaveState.ComputeFingerprint(continuous.CaptureState());
            var fpB = DistressMissionSaveState.ComputeFingerprint(replay.CaptureState());
            Assert.Equal(fpA, fpB);
        }

        [Fact]
        public void Fingerprint_TracksRuntimeState_AnalyzedRunsDivergeFromUnanalyzed()
        {
            var analyzed = RunScenario(ManagerWithDefs(), reloadAfterDay3: false, withAuthenticity: true);
            var unanalyzed = RunScenario(ManagerWithDefs(), reloadAfterDay3: false, withAuthenticity: false);

            Assert.NotEqual(
                DistressMissionSaveState.ComputeFingerprint(analyzed.CaptureState()),
                DistressMissionSaveState.ComputeFingerprint(unanalyzed.CaptureState()));
        }

        [Fact]
        public void MidReloadExpiry_CausesConsequenceExactlyOnce_AtTheSameBoundary()
        {
            // Save one tick before the deadline → load → next tick fires once.
            var m = new DistressRescueMissionManager();
            var log = new List<string>();
            m.RecordSignalHeard("freq_distress_156_8", 1); // expiry day 4
            m.TickDaily(3);

            var fresh = new DistressRescueMissionManager();
            fresh.OnIgnoreConsequence += (mission, tokens, factionId, day) => log.Add(day.ToString());
            fresh.RestoreState(m.CaptureState());
            fresh.TickDaily(4); // boundary fires exactly once after restore
            fresh.TickDaily(5);
            fresh.TickDaily(6);
            Assert.Single(log);
        }

        [Fact]
        public void LongRunUndiscoveredSignals_NeverPunished_NeverInitialized()
        {
            var m = new DistressRescueMissionManager();
            m.TickDaily(30);
            m.TickDaily(300);
            m.TickDaily(3000);
            foreach (var mission in m.AllMissions)
            {
                Assert.Equal(DistressRescueMissionStage.None, mission.Stage);
                Assert.False(mission.Expired);
                Assert.False(mission.IgnoreConsequenceApplied);
                Assert.False(mission.SenderAlive && mission.SenderDeathDay > 0);
            }
        }
    }
}
