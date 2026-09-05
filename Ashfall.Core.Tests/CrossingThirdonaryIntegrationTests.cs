using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Crossing;
using Ashfall.Core.MoralChoice;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CrossingThirdonaryIntegrationTests
    {
        private static readonly string DataDir = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));

        private static (CrossingQuestSystem quests, CrossingArbitrationSystem arbitration, CrossingThirdonaryIntegration integration) CreateFixture()
        {
            var quests = new CrossingQuestSystem();
            var catalog = CrossingQuestCatalogLoader.Load(DataDir);
            quests.BindCatalog(catalog);

            var arbitration = new CrossingArbitrationSystem();
            var integration = new CrossingThirdonaryIntegration(arbitration, quests);
            return (quests, arbitration, integration);
        }

        private static void CompleteOpeningQuest(CrossingQuestSystem quests)
        {
            quests.StartQuest(CrossingQuestSystem.OpeningQuest, 75);
            quests.MakeChoice(CrossingQuestSystem.OpeningQuest, "vouch_ostrowski_reluctant");
            var def = quests.GetDef(CrossingQuestSystem.OpeningQuest)!;
            for (int i = 0; i < def.stages.Count; i++)
            {
                quests.AdvanceStage(CrossingQuestSystem.OpeningQuest);
            }
        }

        private static void CompleteQuestWithChoice(CrossingQuestSystem quests, string questId, string choiceId)
        {
            quests.StartQuest(questId, 75);
            quests.MakeChoice(questId, choiceId);
            var def = quests.GetDef(questId)!;
            for (int i = 0; i < def.stages.Count; i++)
            {
                quests.AdvanceStage(questId);
            }
        }

        [Fact]
        public void GetCovenantEligibility_BeforeOpeningQuest_NotEligible()
        {
            var (_, _, integration) = CreateFixture();
            var result = integration.GetCovenantEligibility("covenant_salvaged_accord", 20);
            Assert.NotEqual(CovenantStatus.Eligible, result.Status);
        }

        [Fact]
        public void GetCovenantEligibility_AfterOpeningQuest_Eligible()
        {
            var (quests, _, integration) = CreateFixture();
            CompleteOpeningQuest(quests);
            var result = integration.GetCovenantEligibility("covenant_salvaged_accord", 75);
            Assert.Equal(CovenantStatus.Eligible, result.Status);
        }

        [Fact]
        public void GetCovenantEligibility_BreachedFlag_ReturnsBreached()
        {
            var (quests, _, integration) = CreateFixture();
            CompleteOpeningQuest(quests);
            // Simulate breach flag
            CompleteQuestWithChoice(quests, "quest_crossing_the_salvaged_accord", "choice_repudiate_accord");
            Assert.True(quests.HasFlag("flag_covenant_salvaged_accord_breached"));

            var result = integration.GetCovenantEligibility("covenant_salvaged_accord", 75);
            Assert.Equal(CovenantStatus.Breached, result.Status);
        }

        [Fact]
        public void GetDisputeEligibility_BeforeOpeningQuest_NotEligible()
        {
            var (_, _, integration) = CreateFixture();
            var result = integration.GetDisputeEligibility("dispute_registry_claim", 20);
            Assert.NotEqual(DisputeStatus.Eligible, result.Status);
        }

        [Fact]
        public void GetDisputeEligibility_AfterOpeningQuest_Eligible()
        {
            var (quests, _, integration) = CreateFixture();
            CompleteOpeningQuest(quests);
            var result = integration.GetDisputeEligibility("dispute_registry_claim", 75);
            Assert.Equal(DisputeStatus.Eligible, result.Status);
        }

        [Fact]
        public void GetVisibleQuests_PrereqMet_ShowsLockedQuests()
        {
            var (quests, _, _) = CreateFixture();
            // Opening quest has no prereq, min_day = 70.
            var visibleDay70 = quests.GetVisibleQuests(70);
            Assert.Contains(visibleDay70, q => q.id == CrossingQuestSystem.OpeningQuest);

            // Complete opening quest, now its dependents whose min_day is reached become visible
            CompleteOpeningQuest(quests);
            var visible = quests.GetVisibleQuests(75);
            Assert.Contains(visible, q => q.id == "quest_crossing_the_salvaged_accord");
        }

        [Fact]
        public void GetEligibleQuests_NoVouch_OnlyShowsOpeningQuest()
        {
            var (quests, _, _) = CreateFixture();
            var eligible = quests.GetEligibleQuests(100, hasVouchAccess: false);
            // Before opening quest is completed and with no external vouch access,
            // only the OpeningQuest is eligible to be started.
            foreach (var q in eligible)
            {
                Assert.Equal(CrossingQuestSystem.OpeningQuest, q.id);
            }
        }

        [Fact]
        public void GetEligibleQuests_WithVouch_ShowsAllEligible()
        {
            var (quests, _, _) = CreateFixture();
            var visible = quests.GetVisibleQuests(70);
            var eligible = quests.GetEligibleQuests(70, hasVouchAccess: true);
            // With external vouch access, all visible quests are eligible to start
            Assert.Equal(visible.Count, eligible.Count);
        }

        [Fact]
        public void MakeChoice_CovenantFlag_RoutesMoralDelta()
        {
            var (quests, _, _) = CreateFixture();
            var rng = new SeededRng(12345);
            var moral = new MoralChoiceSystem(rng);
            quests.BindMoralSystem(moral);

            CompleteOpeningQuest(quests);
            quests.StartQuest("quest_crossing_the_salvaged_accord", 75);
            quests.MakeChoice("quest_crossing_the_salvaged_accord", "choice_ratify_accord");
            Assert.True(quests.HasFlag("flag_covenant_salvaged_accord_active"));
            Assert.True(moral.HasFlag("flag_covenant_salvaged_accord_active"));
        }

        [Fact]
        public void CrossingArcReplay_ThreeQuestSequence_AllComplete()
        {
            var (quests, _, integration) = CreateFixture();
            CompleteOpeningQuest(quests);

            // 1. Ratify the salvaged accord
            CompleteQuestWithChoice(quests, "quest_crossing_the_salvaged_accord", "choice_ratify_accord");
            Assert.True(quests.IsQuestCompleted("quest_crossing_the_salvaged_accord"));
            var covResult = integration.GetCovenantEligibility("covenant_salvaged_accord", 50);
            Assert.Equal(CovenantStatus.Active, covResult.Status);

            // 2. Resolve registry dispute
            CompleteQuestWithChoice(quests, "quest_crossing_the_registry_dispute", "choice_uphold_registry");
            Assert.True(quests.IsQuestCompleted("quest_crossing_the_registry_dispute"));
            var dispResult = integration.GetDisputeEligibility("dispute_registry_claim", 50);
            Assert.Equal(DisputeStatus.Resolved, dispResult.Status);

            // 3. Complete long toll covenant
            CompleteQuestWithChoice(quests, "quest_crossing_the_long_toll", "choice_endow_toll");
            Assert.True(quests.IsQuestCompleted("quest_crossing_the_long_toll"));
            var tollResult = integration.GetCovenantEligibility("covenant_bridge_toll", 55);
            Assert.Equal(CovenantStatus.Active, tollResult.Status);
        }
    }
}
