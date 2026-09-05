using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class MoralChoiceFactionReactionsExpansionTests : CatalogTestBase
    {
        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();

        private static readonly string[] CanonicalEventIds = new[]
        {
            MoralChoiceSystem.EventBountyIssued,
            MoralChoiceSystem.EventContractTaken,
            MoralChoiceSystem.EventContractRaised,
            MoralChoiceSystem.EventPatrolDefense,
            MoralChoiceSystem.EventLegendPositive,
            MoralChoiceSystem.EventLegendNegative
        };

        [Fact]
        public void Catalog_LoadsExactSixCanonicalReactions()
        {
            var data = MoralChoiceFactionReactionsCatalogLoader.Load(DataDirectory, s_files, s_json);
            Assert.NotNull(data);
            Assert.Equal(6, data.ThresholdReactions.Count);
        }

        [Fact]
        public void Catalog_ContainsAllSixCanonicalEventIds()
        {
            var data = MoralChoiceFactionReactionsCatalogLoader.Load(DataDirectory, s_files, s_json);
            foreach (var expectedId in CanonicalEventIds)
            {
                Assert.True(data.ThresholdReactions.ContainsKey(expectedId),
                    $"Expected threshold reaction '{expectedId}' to be present in catalog.");
            }
        }

        [Fact]
        public void Catalog_EveryReactionHasNonEmptyEventDescription()
        {
            var data = MoralChoiceFactionReactionsCatalogLoader.Load(DataDirectory, s_files, s_json);
            foreach (var kvp in data.ThresholdReactions)
            {
                Assert.False(string.IsNullOrWhiteSpace(kvp.Value.EventDescription),
                    $"Event '{kvp.Key}' has missing or empty EventDescription.");
            }
        }

        [Fact]
        public void Catalog_EveryReactionHasAllThreeFactionDialogues()
        {
            var data = MoralChoiceFactionReactionsCatalogLoader.Load(DataDirectory, s_files, s_json);
            foreach (var kvp in data.ThresholdReactions)
            {
                var reaction = kvp.Value;
                Assert.NotEmpty(reaction.PeacekeeperDialogue);
                Assert.NotEmpty(reaction.RaiderDialogue);
                Assert.NotEmpty(reaction.KnowledgeKeeperDialogue);
            }
        }

        [Fact]
        public void Catalog_AllDialogueBlocksHaveValidSpeakerLocationAndLines()
        {
            var data = MoralChoiceFactionReactionsCatalogLoader.Load(DataDirectory, s_files, s_json);
            foreach (var kvp in data.ThresholdReactions)
            {
                var factionLists = new[]
                {
                    ("Peacekeeper", kvp.Value.PeacekeeperDialogue),
                    ("Raider", kvp.Value.RaiderDialogue),
                    ("KnowledgeKeeper", kvp.Value.KnowledgeKeeperDialogue),
                    ("Civilian", kvp.Value.CivilianDialogue)
                };

                foreach (var (factionName, blocks) in factionLists)
                {
                    if (blocks == null) continue;
                    foreach (var block in blocks)
                    {
                        Assert.False(string.IsNullOrWhiteSpace(block.Speaker),
                            $"Event '{kvp.Key}' [{factionName}] has an empty Speaker.");
                        Assert.False(string.IsNullOrWhiteSpace(block.Location),
                            $"Event '{kvp.Key}' [{factionName}] has an empty Location.");
                        Assert.InRange(block.Lines.Count, 3, 6);
                        foreach (var line in block.Lines)
                        {
                            Assert.False(string.IsNullOrWhiteSpace(line),
                                $"Event '{kvp.Key}' [{factionName}] has an empty dialogue line.");
                        }
                    }
                }
            }
        }

        [Fact]
        public void Catalog_EveryReactionHasNonEmptyJournalEntry()
        {
            var data = MoralChoiceFactionReactionsCatalogLoader.Load(DataDirectory, s_files, s_json);
            foreach (var kvp in data.ThresholdReactions)
            {
                Assert.False(string.IsNullOrWhiteSpace(kvp.Value.JournalEntry),
                    $"Event '{kvp.Key}' is missing JournalEntry.");
            }
        }

        [Fact]
        public void Catalog_PreservesBountyIssuedBaselineParity()
        {
            var data = MoralChoiceFactionReactionsCatalogLoader.Load(DataDirectory, s_files, s_json);
            var bounty = data.ThresholdReactions[MoralChoiceSystem.EventBountyIssued];

            Assert.Equal("Fires when the player enters VeryEvil band (-100 or below). Peacekeepers issue a bounty.", bounty.EventDescription);
            var pkSergeant = bounty.PeacekeeperDialogue.FirstOrDefault(d => d.Speaker.Contains("Veill"));
            Assert.NotNull(pkSergeant);
            Assert.Contains("Your face is on the board now. I put it there myself.", pkSergeant.Lines);

            var raiderLookout = bounty.RaiderDialogue.FirstOrDefault();
            Assert.NotNull(raiderLookout);
            Assert.Contains("The Peacekeepers put a price on you. You know what that means to us?", raiderLookout.Lines);

            var archivist = bounty.KnowledgeKeeperDialogue.FirstOrDefault();
            Assert.NotNull(archivist);
            Assert.Contains("We record everything. You know that.", archivist.Lines);
        }

        [Fact]
        public void Runtime_BandCrossings_TriggerAllCanonicalReactionsOnce()
        {
            var rng = new SeededRng(42);
            var sys = new MoralChoiceSystem(rng);
            var firedEvents = new List<string>();
            sys.OnThresholdEventFired += firedEvents.Add;

            // Day 1: Drive into Positive (+60)
            sys.Resolve(CreateQuest("quest_moral_pos_1", 60), 0, "loc_a", 1);
            sys.Reconcile(1);
            Assert.Single(firedEvents);
            Assert.Contains(MoralChoiceSystem.EventContractTaken, firedEvents);

            // Day 2: Drive into VeryPositive (+120)
            sys.Resolve(CreateQuest("quest_moral_pos_2", 60), 0, "loc_b", 2);
            sys.Reconcile(2);
            Assert.Equal(3, firedEvents.Count);
            Assert.Contains(MoralChoiceSystem.EventContractRaised, firedEvents);
            Assert.Contains(MoralChoiceSystem.EventPatrolDefense, firedEvents);

            // Day 3: Reconcile again without score change — no new events
            sys.Reconcile(3);
            Assert.Equal(3, firedEvents.Count);

            // Day 4: Drive into VeryEvil (-120 total)
            sys.Resolve(CreateQuest("quest_moral_neg_1", -240), 0, "loc_c", 4);
            sys.Reconcile(4);
            Assert.Equal(4, firedEvents.Count);
            Assert.Contains(MoralChoiceSystem.EventBountyIssued, firedEvents);

            // Day 5: Re-crossing back into VeryPositive does NOT refire already fired events
            sys.Resolve(CreateQuest("quest_moral_pos_3", 240), 0, "loc_d", 5);
            sys.Reconcile(5);
            Assert.Equal(4, firedEvents.Count);
        }

        [Fact]
        public void Runtime_OverflowLegends_TriggerOnceOvernight()
        {
            var rng = new SeededRng(1337);
            var sys = new MoralChoiceSystem(rng);
            var firedEvents = new List<string>();
            sys.OnThresholdEventFired += firedEvents.Add;

            // Push past +200 MaxScore
            sys.Resolve(CreateQuest("quest_moral_overflow_pos", 250), 0, "loc_sanctum", 1);
            Assert.Equal(MoralChoiceSystem.MaxScore, sys.MoralScore);
            Assert.Empty(firedEvents); // Pending until overnight Reconcile

            sys.Reconcile(2);
            Assert.Contains(MoralChoiceSystem.EventLegendPositive, firedEvents);

            // Second positive overflow does not refire
            sys.Resolve(CreateQuest("quest_moral_overflow_pos_2", 50), 0, "loc_sanctum", 3);
            sys.Reconcile(4);
            Assert.Equal(1, firedEvents.Count(id => id == MoralChoiceSystem.EventLegendPositive));

            // Push past -200 MinScore
            sys.Resolve(CreateQuest("quest_moral_overflow_neg", -500), 0, "loc_ruins", 5);
            Assert.Equal(MoralChoiceSystem.MinScore, sys.MoralScore);
            sys.Reconcile(6);
            Assert.Contains(MoralChoiceSystem.EventLegendNegative, firedEvents);

            // Second negative overflow does not refire
            sys.Resolve(CreateQuest("quest_moral_overflow_neg_2", -50), 0, "loc_ruins", 7);
            sys.Reconcile(8);
            Assert.Equal(1, firedEvents.Count(id => id == MoralChoiceSystem.EventLegendNegative));
        }

        [Fact]
        public void Runtime_StateCaptureAndRestore_PreservesFiredEvents()
        {
            var rng = new SeededRng(999);
            var original = new MoralChoiceSystem(rng);
            original.Resolve(CreateQuest("quest_moral_init_a", 150), 0, "loc_1", 1);
            original.Reconcile(2);

            var state = original.CaptureState();
            Assert.Contains(MoralChoiceSystem.EventContractTaken, state.firedThresholdEvents);
            Assert.Contains(MoralChoiceSystem.EventContractRaised, state.firedThresholdEvents);
            Assert.Contains(MoralChoiceSystem.EventPatrolDefense, state.firedThresholdEvents);

            var restored = new MoralChoiceSystem(new SeededRng(999));
            restored.RestoreState(state);

            var firedInRestored = new List<string>();
            restored.OnThresholdEventFired += firedInRestored.Add;

            // Reconcile on future day with same band
            restored.Reconcile(3);
            Assert.Empty(firedInRestored);
        }

        private static MoralChoiceQuestDefinition CreateQuest(string id, int moralDelta)
        {
            return new MoralChoiceQuestDefinition
            {
                Id = id,
                DisplayName = $"Test Quest {id}",
                Category = "trust",
                Choices = new List<MoralChoiceOption>
                {
                    new MoralChoiceOption
                    {
                        MoralDelta = moralDelta,
                        EmpathyDelta = 0,
                        Epitaph = "tested choice"
                    }
                }
            };
        }
    }
}
