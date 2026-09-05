// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.Quests;
using Xunit;

namespace Ashfall.Core.Tests.Quests
{
    public sealed class DynamicQuestlineTests
    {
        [Fact]
        public void RescueMinersQuest_TriggersAndAdvancesStages()
        {
            var system = new DynamicQuestlineSystem();
            var trapped = new List<string> { "survivor_miner_1", "survivor_miner_2" };

            var quest = system.TriggerRescueMinersQuest("inc_cavein_sector_c", "sec_deep_quarry", trapped, triggerDay: 10, deadlineDays: 3, requiredLabor: 200);

            Assert.NotNull(quest);
            Assert.Equal(DynamicQuestlineSystem.RescueMinersQuestId, quest.QuestId);
            Assert.Equal("sec_deep_quarry", quest.TargetLocationId);
            Assert.Equal(2, quest.TargetSurvivorIds.Count);
            Assert.Equal(10, quest.TriggerDay);
            Assert.Equal(13, quest.DeadlineDay);
            Assert.Equal(DynamicQuestStatus.Active, quest.Status);
            Assert.Equal(0, quest.CurrentStageIndex);
            Assert.Equal(4, quest.Stages.Count);

            // Cannot trigger same incident twice
            var duplicate = system.TriggerRescueMinersQuest("inc_cavein_sector_c", "sec_deep_quarry", trapped, 10);
            Assert.Null(duplicate);

            // Advance progress halfway
            system.AdvanceQuestProgress(quest.QuestId, 100);
            Assert.Equal(100, quest.ProgressCurrent);
            Assert.True(quest.CurrentStageIndex >= 1);

            // Complete progress
            system.AdvanceQuestProgress(quest.QuestId, 100);
            Assert.Equal(DynamicQuestStatus.Completed, quest.Status);
            Assert.Empty(system.ActiveQuests);
            Assert.Contains(quest.QuestId, system.CompletedIds);
        }

        [Fact]
        public void RescueMinersQuest_FailsWhenDeadlineExpires()
        {
            var system = new DynamicQuestlineSystem();
            var trapped = new List<string> { "survivor_miner_1" };
            var quest = system.TriggerRescueMinersQuest("inc_cavein_sec_a", "sec_a", trapped, triggerDay: 5, deadlineDays: 2);

            Assert.NotNull(quest);
            Assert.Equal(7, quest.DeadlineDay);

            system.TickDay(6);
            Assert.Equal(DynamicQuestStatus.Active, quest.Status);

            system.TickDay(7); // Deadline reached
            Assert.Equal(DynamicQuestStatus.Failed, quest.Status);
            Assert.Empty(system.ActiveQuests);
            Assert.Contains(quest.QuestId, system.FailedIds);
        }

        [Fact]
        public void InvestigateRadioDepotQuest_TriggersAndCompletes()
        {
            var system = new DynamicQuestlineSystem();

            var quest = system.TriggerInvestigateRadioDepotQuest("intercept_depot_delta", "loc_military_depot", triggerDay: 12);
            Assert.NotNull(quest);
            Assert.Equal(DynamicQuestlineSystem.InvestigateRadioDepotQuestId, quest.QuestId);
            Assert.Equal("loc_military_depot", quest.TargetLocationId);

            // Advance stages
            Assert.True(system.AdvanceQuestStage(quest.QuestId));
            Assert.Equal(1, quest.CurrentStageIndex);
            Assert.True(system.AdvanceQuestStage(quest.QuestId));
            Assert.Equal(2, quest.CurrentStageIndex);
            Assert.True(system.AdvanceQuestStage(quest.QuestId)); // Final stage completes
            Assert.Equal(DynamicQuestStatus.Completed, quest.Status);
            Assert.Empty(system.ActiveQuests);
        }

        [Fact]
        public void ArmoryMunitionsRefurbishQuest_ProgressAndStateRoundtrip()
        {
            var system = new DynamicQuestlineSystem();
            var quest = system.TriggerArmoryMunitionsRefurbishQuest("inc_armory_wear_1", triggerDay: 8, weaponsNeedingRepair: 4);

            Assert.NotNull(quest);
            Assert.Equal(DynamicQuestlineSystem.ArmoryMunitionsRefurbishQuestId, quest.QuestId);
            Assert.Equal(4, quest.ProgressRequired);

            system.AdvanceQuestProgress(quest.QuestId, 2);
            Assert.Equal(2, quest.ProgressCurrent);

            // Capture and restore
            var captured = system.CaptureState();
            Assert.NotNull(captured);

            var system2 = new DynamicQuestlineSystem();
            system2.RestoreState(captured);

            var restoredQuest = system2.GetActiveQuest(DynamicQuestlineSystem.ArmoryMunitionsRefurbishQuestId);
            Assert.NotNull(restoredQuest);
            Assert.Equal(2, restoredQuest.ProgressCurrent);
            Assert.Equal(4, restoredQuest.ProgressRequired);
            Assert.Equal(restoredQuest.CurrentStageIndex, quest.CurrentStageIndex);

            // Ensure incident is still recorded as triggered
            Assert.True(system2.HasIncidentTriggered("inc_armory_wear_1"));
            var dupe = system2.TriggerArmoryMunitionsRefurbishQuest("inc_armory_wear_1", 9, 4);
            Assert.Null(dupe);
        }
    }
}
