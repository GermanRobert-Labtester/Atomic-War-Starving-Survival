// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Narrative;
using Ashfall.Core.Quests;
using Xunit;

namespace Ashfall.Core.Tests.Quests
{
    public sealed class DynamicQuestGeneratorTests
    {
        [Fact]
        public void GenerateQuests_GeneratesQuestsWithinActiveLimit_AndRespectsCooldown()
        {
            var gen = new DynamicQuestGenerator();

            var generated = gen.GenerateQuests(currentDay: 1);
            Assert.NotEmpty(generated);
            Assert.True(generated.Count <= 3);
            Assert.Equal(generated.Count, gen.AvailableCount);

            // Attempt generation on day 2 (cooldown is 3 days)
            var day2Gen = gen.GenerateQuests(currentDay: 2);
            Assert.Empty(day2Gen);

            // Day 5 (after 3 day cooldown from day 1)
            var day5Gen = gen.GenerateQuests(currentDay: 5);
            // Since max is 3 and we already have 3, should be empty
            Assert.Empty(day5Gen);
        }

        [Fact]
        public void AcceptQuest_SetsStatusToActive_AndAssignsSurvivor()
        {
            var gen = new DynamicQuestGenerator();
            var quests = gen.GenerateQuests(currentDay: 1);
            var target = quests.First();

            ProceduralQuest? accepted = null;
            gen.OnQuestAccepted += q => accepted = q;

            bool ok = gen.AcceptQuest(target.QuestId, assignedSurvivorId: "dweller_scout");

            Assert.True(ok);
            Assert.Equal(target, accepted);
            Assert.Equal(ProceduralQuestStatus.Active, target.Status);
            Assert.Equal("dweller_scout", target.AssignedSurvivorId);
            Assert.Equal(1, gen.ActiveCount);
        }

        [Fact]
        public void ProgressQuest_IncrementsCurrentQuantity()
        {
            var gen = new DynamicQuestGenerator();
            var quests = gen.GenerateQuests(currentDay: 1);
            var target = quests.First();
            gen.AcceptQuest(target.QuestId);

            gen.ProgressQuest(target.QuestId, 1);
            Assert.Equal(1, target.CurrentQuantity);
        }

        [Fact]
        public void CompleteQuest_MarksCompleted_OnlyWhenFulfilled()
        {
            var gen = new DynamicQuestGenerator();
            var quests = gen.GenerateQuests(currentDay: 1);
            var target = quests.First();
            gen.AcceptQuest(target.QuestId);

            // Incomplete try
            bool completeBeforeFulfill = gen.CompleteQuest(target.QuestId, currentDay: 2);
            Assert.False(completeBeforeFulfill);

            // Progress to required
            gen.ProgressQuest(target.QuestId, target.RequiredQuantity);
            Assert.True(target.IsFulfilled);

            bool ok = gen.CompleteQuest(target.QuestId, currentDay: 2);
            Assert.True(ok);
            Assert.Equal(ProceduralQuestStatus.Completed, target.Status);
            Assert.Equal(1, gen.CompletedCount);
        }

        [Fact]
        public void CheckDeadlines_ExpiresOrFailsQuestsPastDeadline()
        {
            var gen = new DynamicQuestGenerator();
            var quests = gen.GenerateQuests(currentDay: 1);
            var q1 = quests[0];
            var q2 = quests[1];

            // Accept q1, leave q2 available
            gen.AcceptQuest(q1.QuestId);

            // Advance past deadline (default is currentDay + 7 = 8)
            gen.CheckDeadlines(currentDay: 12);

            Assert.Equal(ProceduralQuestStatus.Failed, q1.Status);
            Assert.Equal(ProceduralQuestStatus.Expired, q2.Status);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var gen1 = new DynamicQuestGenerator();
            var quests = gen1.GenerateQuests(currentDay: 1);
            gen1.AcceptQuest(quests[0].QuestId, "dweller_alpha");
            gen1.ProgressQuest(quests[0].QuestId, 1);

            var state = gen1.CaptureState();
            Assert.Equal(quests.Count, state.Quests.Count);

            var gen2 = new DynamicQuestGenerator();
            gen2.RestoreState(state);

            Assert.Equal(gen1.ActiveCount, gen2.ActiveCount);
            Assert.Equal(gen1.AvailableCount, gen2.AvailableCount);
        }

        [Fact]
        public void CanonicalAdapter_RegistersJsonBackedCandidate_WithoutGeneratorLifecycleState()
        {
            var narrative = new ProceduralNarrativeSystem();
            narrative.LoadTemplateCatalog(new[]
            {
                new QuestTemplateDef
                {
                    Id = "quest_template_canonical",
                    Weight = 1f,
                    MinimumDay = 1,
                    ActorRoleSlots = new List<string> { "scout" },
                    LocationConstraints = new List<string> { "ruin" },
                    ObjectiveModules = new List<string> { "repair_pipe" },
                    TitleKey = "quest.canonical.title",
                    DescriptionKey = "quest.canonical.description",
                    TimeLimitDays = 3
                }
            });
            var runtime = new QuestRuntimeCoordinator();
            var snapshot = new NarrativeWorldSnapshot
            {
                day = 5,
                aliveSurvivorIds = new List<string> { "survivor_scout" },
                knownLocationIds = new List<string> { "loc_ruin" }
            };

            bool registered = DynamicQuestGenerator.TryGenerateAndRegisterCanonicalCandidate(
                narrative,
                runtime,
                snapshot,
                new SeededRng(171),
                out var draft);

            Assert.True(registered);
            Assert.Equal("quest_template_canonical", draft.quest.definitionId);
            Assert.Single(runtime.BuildReadModel());
        }
    }
}
