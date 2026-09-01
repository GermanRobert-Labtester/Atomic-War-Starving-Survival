using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Memorial;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 23 deferred follow-up: deepest-wreck capstone quest arc
    /// at site_exp09_sunken_submarine ("The Half-Submerged Barrik").
    /// </summary>
    public class Plan23CapstoneTests : CatalogTestBase
    {
        [Fact]
        public void CapstoneQuest_ParsesFromCatalog_WithExpectedFields()
        {
            string dataDir = DataDirectory;
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var quests = ExpansionQuestCatalogLoader.Load(dataDir, files, json);

            var quest = quests.FirstOrDefault(q => q.id == "quest_exp09_sunken_submarine");
            Assert.NotNull(quest);
            Assert.Equal("The Submerged Asset", quest.title);
            Assert.Equal("exploration", quest.type);
            Assert.Equal(90, quest.minDay);
            Assert.Equal(300, quest.maxDay);
            Assert.Equal("black_flotilla", quest.factionTag);
            Assert.Contains("quest_black_flotilla_trade", quest.prerequisites);
            Assert.Contains("item_rebreather_canister", quest.prerequisites);
            Assert.Contains("item_descent_line", quest.prerequisites);
            Assert.Contains("item_sealed_dive_lamp", quest.prerequisites);
            Assert.Contains("flotilla_trust_deep", quest.prerequisites);
            Assert.Equal(3, quest.choices.Count);
        }

        [Fact]
        public void CapstoneQuest_Registry_IsRegisteredInQuestlineMaster()
        {
            var master = new QuestlineMasterCatalogLoader(
                new FileSystemIO(), new SystemTextJsonSerializer()).Load(DataDirectory);

            Assert.True(master.IsRegistered("quest_exp09_sunken_submarine"));
        }

        [Fact]
        public void CapstoneQuest_SuccessPath_RecordsMemorializeChoice()
        {
            var quest = new ExpansionQuestEntry
            {
                id = "quest_exp09_sunken_submarine",
                title = "The Submerged Asset",
                description = "The Black Flotilla has provided coordinates for a sunken pre-war vessel.",
                type = "exploration",
                minDay = 90,
                maxDay = 300,
                factionTag = "black_flotilla",
                synopsis = "Burial at the Barrik.",
                prerequisites = new System.Collections.Generic.List<string>(),
                choices = new System.Collections.Generic.List<ExpansionQuestChoice>
                {
                    new ExpansionQuestChoice
                    {
                        id = "barrik_dive_and_memorialize",
                        text = "Execute interior survey",
                        effects = new System.Collections.Generic.List<ExpansionQuestEffect>
                        {
                            new ExpansionQuestEffect { type = "complete_quest", target = "quest_exp09_sunken_submarine" }
                        },
                        consequences = "You penetrate the flooded hull and log the personnel tags on the bulkhead."
                    }
                }
            };

            var system = new ExpansionQuestSystem();
            system.BindCatalog(new System.Collections.Generic.List<ExpansionQuestEntry> { quest });
            system.StartQuest("quest_exp09_sunken_submarine", day: 100);
            system.MakeChoice("quest_exp09_sunken_submarine", "barrik_dive_and_memorialize", day: 101);

            Assert.True(system.IsCompleted("quest_exp09_sunken_submarine"));
            Assert.Equal("barrik_dive_and_memorialize", system.GetProgress("quest_exp09_sunken_submarine")!.currentChoiceId);
        }

        [Fact]
        public void CapstoneQuest_Memorialize_IsIdempotentAndDeterministic()
        {
            var memorial = new MemorialSystem(new MemorialState());

            var entry = memorial.Memorialize(new MemorialInput
            {
                SurvivorId = "diver_barrik_memorial",
                Cause = "war_grave",
                Day = 120,
                BirthDay = 1,
                Epitaph = "Lost at the Half-Submerged Barrik.",
                Outcome = MemorialOutcome.WallEntry,
                DeathQuality = DeathQuality.Unattended
            });

            Assert.NotNull(entry);
            Assert.Equal("diver_barrik_memorial", entry.SurvivorId);
            Assert.Equal(MemorialOutcome.WallEntry, entry.Outcome);
            Assert.Equal(DeathQuality.Unattended, entry.DeathQuality);

            // Idempotent repeat returns same entry without duplication.
            var repeat = memorial.Memorialize(new MemorialInput
            {
                SurvivorId = "diver_barrik_memorial",
                Cause = "war_grave",
                Day = 120,
                BirthDay = 1,
                Outcome = MemorialOutcome.WallEntry
            });

            Assert.Same(entry, repeat);
            Assert.Single(memorial.Entries);
        }

        [Fact]
        public void CapstoneQuest_State_RoundTrip_PreservesProgress()
        {
            var quest = new ExpansionQuestEntry
            {
                id = "quest_exp09_sunken_submarine",
                title = "The Submerged Asset",
                description = "The Black Flotilla has provided coordinates for a sunken pre-war vessel.",
                type = "exploration",
                minDay = 90,
                maxDay = 300,
                factionTag = "black_flotilla",
                synopsis = "Burial at the Barrik.",
                prerequisites = new System.Collections.Generic.List<string>(),
                choices = new System.Collections.Generic.List<ExpansionQuestChoice>
                {
                    new ExpansionQuestChoice
                    {
                        id = "barrik_dive_and_memorialize",
                        text = "Execute interior survey",
                        effects = new System.Collections.Generic.List<ExpansionQuestEffect>
                        {
                            new ExpansionQuestEffect { type = "complete_quest", target = "quest_exp09_sunken_submarine" }
                        },
                        consequences = "You penetrate the flooded hull and log the personnel tags on the bulkhead."
                    }
                }
            };

            var system = new ExpansionQuestSystem();
            system.BindCatalog(new System.Collections.Generic.List<ExpansionQuestEntry> { quest });
            system.StartQuest("quest_exp09_sunken_submarine", day: 100);
            system.MakeChoice("quest_exp09_sunken_submarine", "barrik_dive_and_memorialize", day: 101);
            system.CompleteQuest("quest_exp09_sunken_submarine", day: 101);

            var state = system.CaptureState();

            var restored = new ExpansionQuestSystem();
            restored.BindCatalog(new System.Collections.Generic.List<ExpansionQuestEntry> { quest });
            restored.RestoreState(state);

            Assert.True(restored.IsCompleted("quest_exp09_sunken_submarine"));
            Assert.Equal("barrik_dive_and_memorialize", restored.GetProgress("quest_exp09_sunken_submarine")!.currentChoiceId);
            Assert.Equal(100, restored.GetProgress("quest_exp09_sunken_submarine")!.dayStarted);
            Assert.Equal(101, restored.GetProgress("quest_exp09_sunken_submarine")!.dayResolved);
        }
    }
}
