// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    public sealed class DistressRescueMissionTests
    {
        private readonly DistressRescueMissionManager _manager = new DistressRescueMissionManager();

        [Fact]
        public void AllRescueMissions_AreRegisteredWithCorrectCorrelation()
        {
            // Five flagship rescues + seven §24 expansion scenarios across
            // two content tranches (hostage, fever ward, salvage crew, ransom,
            // false evacuation, command-post beacon, winter crossing).
            var missions = _manager.AllMissions;
            Assert.Equal(12, missions.Count);
            Assert.NotNull(_manager.GetMissionByQuest("quest_distress_ransom_demand"));
            Assert.Equal("loc_dentists_row", _manager.GetMissionByQuest("quest_distress_ransom_demand")!.DestinationId);
            Assert.NotNull(_manager.GetMissionByQuest("quest_distress_false_evacuation"));
            Assert.Equal("collapsed_building", _manager.GetMissionByQuest("quest_distress_false_evacuation")!.DestinationId);
            Assert.True(_manager.GetMissionByQuest("quest_distress_false_evacuation")!.IsTrap);
            Assert.NotNull(_manager.GetMissionByQuest("quest_distress_military_beacon"));
            Assert.Equal("loc_ordnance_shoulder", _manager.GetMissionByQuest("quest_distress_military_beacon")!.DestinationId);
            Assert.NotNull(_manager.GetMissionByQuest("quest_distress_winter_crossing"));
            Assert.Equal("loc_the_shallows_market", _manager.GetMissionByQuest("quest_distress_winter_crossing")!.DestinationId);
            Assert.NotNull(_manager.GetMissionByQuest("quest_distress_hostage_call"));
            Assert.Equal("loc_motel_verity", _manager.GetMissionByQuest("quest_distress_hostage_call")!.DestinationId);
            Assert.NotNull(_manager.GetMissionByQuest("quest_distress_infected_survivor"));
            Assert.Equal("loc_st_brigids_almshouse", _manager.GetMissionByQuest("quest_distress_infected_survivor")!.DestinationId);
            Assert.NotNull(_manager.GetMissionByQuest("quest_distress_convoy_sos"));
            Assert.Equal("loc_warehouse_district", _manager.GetMissionByQuest("quest_distress_convoy_sos")!.DestinationId);

            var mechanic = _manager.GetMissionByQuest("quest_distress_trapped_mechanic");
            Assert.NotNull(mechanic);
            Assert.Equal("freq_distress_88_3", mechanic.SignalId);
            Assert.Equal("loc_recovery_yard", mechanic.DestinationId);
            Assert.False(mechanic.IsTrap);
            Assert.Equal(5, mechanic.DeadlineDays);
            Assert.Contains("scrap_metal", mechanic.RewardItems);

            var trader = _manager.GetMissionByQuest("quest_distress_injured_trader");
            Assert.NotNull(trader);
            Assert.Equal("freq_distress_156_8", trader.SignalId);
            Assert.Equal("rural_gas_station", trader.DestinationId);
            Assert.False(trader.IsTrap);
            Assert.Equal(3, trader.DeadlineDays);

            var family = _manager.GetMissionByQuest("quest_distress_family_shelter");
            Assert.NotNull(family);
            Assert.Equal("freq_distress_445_2", family.SignalId);
            Assert.Equal("family_bunker_backyard_shed", family.DestinationId);
            Assert.False(family.IsTrap);
            Assert.Equal(4, family.DeadlineDays);

            var trap = _manager.GetMissionByQuest("quest_distress_raider_trap");
            Assert.NotNull(trap);
            Assert.Equal("freq_distress_192_4", trap.SignalId);
            Assert.Equal("loc_denial_cut_substation", trap.DestinationId);
            Assert.True(trap.IsTrap);

            var military = _manager.GetMissionByQuest("quest_distress_military_patrol");
            Assert.NotNull(military);
            Assert.Equal("freq_distress_901_2", military.SignalId);
            Assert.Equal("checkpoint_kilo_armory", military.DestinationId);
            Assert.False(military.IsTrap);
        }

        [Fact]
        public void StagedProgression_MonotonicallyAdvancesToRescued()
        {
            var m = _manager.GetMissionByQuest("quest_distress_trapped_mechanic");
            Assert.NotNull(m);
            Assert.Equal(DistressRescueMissionStage.None, m.Stage);

            // 1. Heard on Day 2
            bool heard = _manager.RecordSignalHeard("freq_distress_88_3", 2);
            Assert.True(heard);
            Assert.Equal(DistressRescueMissionStage.Heard, m.Stage);
            Assert.Equal(2, m.InterceptedDay);
            Assert.Equal(7, m.ExpiryDay);

            // Cannot re-hear
            Assert.False(_manager.RecordSignalHeard("freq_distress_88_3", 3));

            // 2. Identified
            bool ided = _manager.RecordSignalIdentified("freq_distress_88_3");
            Assert.True(ided);
            Assert.Equal(DistressRescueMissionStage.Identified, m.Stage);

            // 3. Dispatched
            bool disp = _manager.RecordExpeditionDispatched("quest_distress_trapped_mechanic", "exp_sort_001");
            Assert.True(disp);
            Assert.Equal(DistressRescueMissionStage.Dispatched, m.Stage);
            Assert.Equal("exp_sort_001", m.ExpeditionId);

            // 4. Reached within deadline (Day 4 <= Day 7)
            var stage = _manager.RecordDestinationReached("quest_distress_trapped_mechanic", 4);
            Assert.Equal(DistressRescueMissionStage.TerminalRescued, stage);
            Assert.Equal(DistressRescueMissionStage.TerminalRescued, m.Stage);
            Assert.True(m.IsTerminal);
        }

        [Fact]
        public void DeadlineMath_FailsMissionWhenArrivingPastExpiry()
        {
            var m = _manager.GetMissionByQuest("quest_distress_injured_trader");
            Assert.NotNull(m);

            _manager.RecordSignalHeard("freq_distress_156_8", 1); // DeadlineDays = 3, ExpiryDay = 4
            _manager.RecordSignalIdentified("freq_distress_156_8");
            _manager.RecordExpeditionDispatched("quest_distress_injured_trader", "exp_sort_002");

            // Arrive on Day 5 — rescue-signal runtime: the arrival DAY governs
            // the outcome; the trader's sender death day is 1 + survival 3 = 4,
            // so Day-5 arrival resolves the dead branch (plan §8.6).
            var stage = _manager.RecordDestinationReached("quest_distress_injured_trader", 5);
            Assert.Equal(DistressRescueMissionStage.TerminalFailed, stage);
            Assert.Equal(DistressRescueMissionStage.TerminalFailed, m.Stage);
            Assert.False(m.SenderAlive);
            Assert.Contains("died on Day 4", m.OutcomeSummary);
        }

        [Fact]
        public void RaiderTrap_TransitionsToAmbushThenSurvived()
        {
            var m = _manager.GetMissionByQuest("quest_distress_raider_trap");
            Assert.NotNull(m);

            _manager.RecordSignalHeard("freq_distress_192_4", 1);
            _manager.RecordSignalIdentified("freq_distress_192_4");
            _manager.RecordExpeditionDispatched("quest_distress_raider_trap", "exp_sort_003");

            // Arrive on Day 3
            var stage = _manager.RecordDestinationReached("quest_distress_raider_trap", 3);
            Assert.Equal(DistressRescueMissionStage.TerminalAmbush, stage);
            Assert.Equal(DistressRescueMissionStage.TerminalAmbush, m.Stage);

            // Ambush resolved
            bool survived = _manager.ResolveAmbushSurvived("quest_distress_raider_trap", "Raiders eliminated.");
            Assert.True(survived);
            Assert.Equal(DistressRescueMissionStage.TerminalSurvived, m.Stage);
        }

        [Fact]
        public void IdempotentRewards_CannotBeClaimedTwice()
        {
            var m = _manager.GetMissionByQuest("quest_distress_military_patrol");
            Assert.NotNull(m);

            _manager.RecordSignalHeard("freq_distress_901_2", 1);
            _manager.RecordSignalIdentified("freq_distress_901_2");
            _manager.RecordExpeditionDispatched("quest_distress_military_patrol", "exp_sort_004");
            _manager.RecordDestinationReached("quest_distress_military_patrol", 3);

            Assert.False(_manager.IsReceiptClaimed("quest_distress_military_patrol", "freq_distress_901_2"));

            // First claim
            var (items1, rep1) = _manager.ClaimIdempotentRewards("quest_distress_military_patrol");
            Assert.NotEmpty(items1);
            Assert.Contains("ammo_556", items1);
            Assert.Equal(10, rep1);
            Assert.True(_manager.IsReceiptClaimed("quest_distress_military_patrol", "freq_distress_901_2"));

            // Second claim -> strictly zero duplicate grants
            var (items2, rep2) = _manager.ClaimIdempotentRewards("quest_distress_military_patrol");
            Assert.Empty(items2);
            Assert.Equal(0, rep2);
        }

        [Fact]
        public void DailyTick_ExpiresUnreachedMissionsPastDeadline()
        {
            var m = _manager.GetMissionByQuest("quest_distress_family_shelter");
            Assert.NotNull(m);

            _manager.RecordSignalHeard("freq_distress_445_2", 1); // Expiry Day 5
            _manager.TickDaily(3);
            Assert.Equal(DistressRescueMissionStage.Heard, m.Stage);

            _manager.TickDaily(6); // Past deadline
            Assert.Equal(DistressRescueMissionStage.TerminalFailed, m.Stage);
            Assert.True(m.IsTerminal);
        }

        [Fact]
        public void SaveLoad_PreservesStagesAndClaimedReceipts()
        {
            var m = _manager.GetMissionByQuest("quest_distress_trapped_mechanic");
            Assert.NotNull(m);

            _manager.RecordSignalHeard("freq_distress_88_3", 2);
            _manager.RecordSignalIdentified("freq_distress_88_3");
            _manager.RecordExpeditionDispatched("quest_distress_trapped_mechanic", "exp_save_test");
            _manager.RecordDestinationReached("quest_distress_trapped_mechanic", 4);
            _manager.ClaimIdempotentRewards("quest_distress_trapped_mechanic");

            var save = _manager.CaptureState();
            Assert.NotNull(save);
            Assert.Contains("quest_distress_trapped_mechanic:freq_distress_88_3", save.ClaimedReceipts);

            var freshManager = new DistressRescueMissionManager();
            freshManager.RestoreState(save);

            var restoredM = freshManager.GetMissionByQuest("quest_distress_trapped_mechanic");
            Assert.NotNull(restoredM);
            Assert.Equal(DistressRescueMissionStage.TerminalRescued, restoredM.Stage);
            Assert.Equal("exp_save_test", restoredM.ExpeditionId);
            Assert.True(freshManager.IsReceiptClaimed("quest_distress_trapped_mechanic", "freq_distress_88_3"));

            // Cannot re-claim on restored manager
            var (items, rep) = freshManager.ClaimIdempotentRewards("quest_distress_trapped_mechanic");
            Assert.Empty(items);
            Assert.Equal(0, rep);
        }
    }
}
