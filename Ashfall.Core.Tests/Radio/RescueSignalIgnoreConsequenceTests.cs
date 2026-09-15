// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Task 3 — ignore consequence enforcement. Discovered, actionable signals
    /// expire and apply their configured consequence exactly once; resolved,
    /// dispatched, and undiscovered signals are exempt; state survives reload.
    /// </summary>
    public sealed class RescueSignalIgnoreConsequenceTests
    {
        private readonly DistressRescueMissionManager _manager = new DistressRescueMissionManager();

        private static (DistressRescueMission Mission, List<string> EventLog) Heard(
            DistressRescueMissionManager m, string signalId, string questId, int day)
        {
            var log = new List<string>();
            m.OnIgnoreConsequence += (mission, tokens, factionId, day2) =>
                log.Add($"{mission.QuestId}:{string.Join(",", tokens)}:{factionId}:{day2}");
            Assert.True(m.RecordSignalHeard(signalId, day));
            var mission = m.GetMissionByQuest(questId)!;
            return (mission, log);
        }

        [Fact]
        public void DiscoveredSignal_ReachesDeadline_ConsequenceFires()
        {
            var (mission, log) = Heard(_manager, "freq_distress_88_3", "quest_distress_trapped_mechanic", 2);
            _manager.TickDaily(7); // ExpiryDay = 2 + 5
            Assert.True(mission.Expired);
            Assert.True(mission.IgnoreConsequenceApplied);
            Assert.Equal(7, mission.IgnoreConsequenceAppliedDay);
            Assert.False(mission.SenderAlive);
            Assert.Equal(7, mission.SenderDeathDay);
            Assert.Single(log);
            Assert.Contains("sender_death", log[0]);
        }

        [Fact]
        public void ConsequenceFiresOnce_AcrossRepeatedTicks()
        {
            var (mission, log) = Heard(_manager, "freq_distress_88_3", "quest_distress_trapped_mechanic", 2);
            _manager.TickDaily(7);
            _manager.TickDaily(8);
            _manager.TickDaily(9);
            Assert.Single(log);
            Assert.False(mission.SenderAlive);
        }

        [Fact]
        public void ResolvedBeforeDeadline_NoConsequence()
        {
            var (mission, log) = Heard(_manager, "freq_distress_88_3", "quest_distress_trapped_mechanic", 2);
            Assert.True(_manager.RecordExpeditionDispatched("quest_distress_trapped_mechanic", "exp_1"));
            _manager.TickDaily(10);
            Assert.False(mission.Expired);
            Assert.False(mission.IgnoreConsequenceApplied);
            Assert.Empty(log);
        }

        [Fact]
        public void UndiscoveredSignal_NeverPunished()
        {
            var log = new List<string>();
            _manager.OnIgnoreConsequence += (mission, tokens, factionId, day) => log.Add(mission.QuestId);
            _manager.TickDaily(1000);
            var mission = _manager.GetMissionByQuest("quest_distress_trapped_mechanic")!;
            Assert.False(mission.Expired);
            Assert.Equal(DistressRescueMissionStage.None, mission.Stage);
            Assert.Empty(log);
        }

        [Fact]
        public void SenderDeathConsequence_StillPermitsRecoveryDispatch()
        {
            var (mission, _) = Heard(_manager, "freq_distress_156_8", "quest_distress_injured_trader", 1);
            _manager.TickDaily(5); // ExpiryDay = 4 → expired + sender dead
            Assert.True(mission.Expired);
            Assert.False(mission.SenderAlive);
            // A later expedition toward the remains is still allowed.
            Assert.True(_manager.RecordExpeditionDispatched("quest_distress_injured_trader", "exp_recovery_01"));
            var stage = _manager.RecordDestinationReached("quest_distress_injured_trader", 6);
            Assert.Equal(DistressRescueMissionStage.TerminalFailed, stage);
            Assert.Contains("died on Day", mission.OutcomeSummary);
        }

        [Fact]
        public void FactionStandingLoss_UsesLiveEventWithCanonicalFaction()
        {
            var (mission, log) = Heard(_manager, "freq_distress_901_2", "quest_distress_military_patrol", 1);
            _manager.TickDaily(7); // ExpiryDay = 6
            Assert.Single(log);
            Assert.Contains("faction_standing_loss", log[0]);
            Assert.Contains("faction_civil_defense", log[0]);
            Assert.False(mission.SenderAlive);
        }

        [Fact]
        public void AppliedState_SurvivesSaveLoad_NoRepeatAfterReload()
        {
            var (mission, log) = Heard(_manager, "freq_distress_88_3", "quest_distress_trapped_mechanic", 2);
            _manager.TickDaily(7);
            Assert.Single(log);

            var fresh = new DistressRescueMissionManager();
            var log2 = new List<string>();
            fresh.OnIgnoreConsequence += (mission, tokens, factionId, day) => log2.Add(mission.QuestId);
            fresh.RestoreState(_manager.CaptureState());
            fresh.TickDaily(20);
            fresh.TickDaily(21);
            Assert.Empty(log2);
            var restored = fresh.GetMissionByQuest("quest_distress_trapped_mechanic")!;
            Assert.True(restored.IgnoreConsequenceApplied);
            Assert.Equal(7, restored.IgnoreConsequenceAppliedDay);
            Assert.Equal(7, restored.SenderDeathDay);
        }

        [Fact]
        public void FirstHeardDay_InitializesOnlyOnce_ReplayDoesNotExtendDeadline()
        {
            var (mission, log) = Heard(_manager, "freq_distress_88_3", "quest_distress_trapped_mechanic", 2);
            // Replaying the transmission must not move the deadline.
            Assert.False(_manager.RecordSignalHeard("freq_distress_88_3", 9));
            Assert.Equal(2, mission.InterceptedDay);
            Assert.Equal(7, mission.ExpiryDay);
            _manager.TickDaily(7);
            Assert.Single(log);
        }

        [Fact]
        public void NoConsequenceSignal_PreservesLegacyExpiry()
        {
            var (mission, log) = Heard(_manager, "freq_distress_445_2", "quest_distress_family_shelter", 1);
            _manager.TickDaily(3);
            Assert.Equal(DistressRescueMissionStage.Heard, mission.Stage);
            _manager.TickDaily(6); // ExpiryDay = 5
            Assert.Equal(DistressRescueMissionStage.TerminalFailed, mission.Stage);
            Assert.True(mission.IsTerminal);
            Assert.False(mission.Expired);
            Assert.False(mission.IgnoreConsequenceApplied);
            Assert.Empty(log);
        }

        [Fact]
        public void ExactDeadlineBoundary_IsTested()
        {
            // Plan §7.4: the deadline day itself expires the signal.
            var (mission, log) = Heard(_manager, "freq_distress_156_8", "quest_distress_injured_trader", 1);
            _manager.TickDaily(3); // ExpiryDay = 4 — one day before
            Assert.False(mission.Expired);
            Assert.Empty(log);
            _manager.TickDaily(4); // boundary day: fires exactly here, once
            Assert.True(mission.Expired);
            Assert.Single(log);
        }

        [Fact]
        public void FactionAmbushConsequence_UsesLiveEventWithVictimFaction()
        {
            // Ransom demand: ignoring lets the holders' toll expire with the
            // courier — sender death AND a standing event for the victim's
            // faction (faction_ambush token), exactly once.
            var (mission, log) = Heard(_manager, "freq_distress_555_0", "quest_distress_ransom_demand", 1);
            _manager.TickDaily(5); // ExpiryDay = 5 (deadline day boundary)
            Assert.Single(log);
            Assert.Contains("sender_death", log[0]);
            Assert.Contains("faction_ambush", log[0]);
            Assert.Contains("faction_river_nomads", log[0]);
            Assert.False(mission.SenderAlive);
        }

        [Fact]
        public void FalseEvacuation_IgnoringItIsWiselyUnpunished()
        {
            // Trap-class signal: the ambush happens to those who HEED the fake
            // evacuation; ignoring it must never fire a consequence.
            var (mission, log) = Heard(_manager, "freq_distress_380_2", "quest_distress_false_evacuation", 1);
            _manager.TickDaily(10);
            Assert.Empty(log);
            Assert.Equal(DistressRescueMissionStage.TerminalFailed, mission.Stage);
            Assert.False(mission.Expired);
            Assert.False(mission.IgnoreConsequenceApplied);
        }

        [Fact]
        public void ConsequenceTokenVocabulary_IsClosedAndValidated()
        {
            Assert.True(DistressRescueMissionManager.AreConsequenceTokensValid(new[] { "sender_death" }));
            Assert.True(DistressRescueMissionManager.AreConsequenceTokensValid(new[] { "sender_death", "faction_standing_loss" }));
            Assert.True(DistressRescueMissionManager.AreConsequenceTokensValid(new[] { "faction_ambush" }));
            Assert.True(DistressRescueMissionManager.AreConsequenceTokensValid(null));
            Assert.False(DistressRescueMissionManager.AreConsequenceTokensValid(new[] { "bogus_token" }));
            // Authored rows must all carry valid tokens (constructor validates).
            var m = new DistressRescueMissionManager();
            foreach (var mission in m.AllMissions)
                Assert.True(DistressRescueMissionManager.AreConsequenceTokensValid(mission.IgnoreConsequenceTokens),
                    mission.QuestId);
        }

        [Fact]
        public void CatalogAudit_AllRescueQuests_MapUnambiguously()
        {
            // Deterministic audit table (plan §11): quest ↔ signal ↔ destination.
            var expected = new (string Quest, string Signal, string Destination, string? Consequence)[]
            {
                ("quest_distress_trapped_mechanic", "freq_distress_88_3", "loc_recovery_yard", "sender_death"),
                ("quest_distress_injured_trader", "freq_distress_156_8", "rural_gas_station", "sender_death"),
                ("quest_distress_family_shelter", "freq_distress_445_2", "family_bunker_backyard_shed", null),
                ("quest_distress_raider_trap", "freq_distress_192_4", "loc_denial_cut_substation", null),
                ("quest_distress_military_patrol", "freq_distress_901_2", "checkpoint_kilo_armory", "sender_death"),
                // Expansion wave (§24): hostage call, fever ward, salvage crew.
                ("quest_distress_hostage_call", "freq_distress_726_5", "loc_motel_verity", "sender_death"),
                ("quest_distress_infected_survivor", "freq_distress_609_4", "loc_st_brigids_almshouse", "sender_death"),
                ("quest_distress_convoy_sos", "freq_distress_455_7", "loc_warehouse_district", "sender_death"),
                // Second tranche (§24): ransom, false evacuation, beacon, crossing.
                ("quest_distress_ransom_demand", "freq_distress_555_0", "loc_dentists_row", "sender_death"),
                ("quest_distress_false_evacuation", "freq_distress_380_2", "collapsed_building", null),
                ("quest_distress_military_beacon", "freq_distress_318_0", "loc_ordnance_shoulder", "sender_death"),
                ("quest_distress_winter_crossing", "freq_distress_269_3", "loc_the_shallows_market", "sender_death"),
            };
            Assert.Equal(expected.Length, _manager.AllMissions.Count);
            foreach (var (quest, signal, dest, consequence) in expected)
            {
                var byQuest = _manager.GetMissionByQuest(quest);
                var bySignal = _manager.GetMissionBySignal(signal);
                Assert.NotNull(byQuest);
                Assert.NotNull(bySignal);
                Assert.Same(byQuest, bySignal); // unambiguous one-to-one mapping
                Assert.Equal(dest, byQuest!.DestinationId);
                Assert.Equal(signal, byQuest.SignalId);
                if (consequence == null)
                    Assert.Empty(byQuest.IgnoreConsequenceTokens);
                else
                    Assert.Contains(consequence, byQuest.IgnoreConsequenceTokens);
            }
        }
    }
}
