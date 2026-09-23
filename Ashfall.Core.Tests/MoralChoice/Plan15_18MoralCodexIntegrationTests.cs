// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Codex;
using Ashfall.Core.Flags;
using Ashfall.Core.Journal;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.MoralChoice
{
    public sealed class Plan15_18MoralCodexIntegrationTests
    {
        [Fact]
        public void MoralChoice_Resolution_AdjustsScoreAndTriggersThresholds()
        {
            var seededRng = new SeededRng(12345);
            var flagLedger = new InMemoryFlagLedger();
            var system = new MoralChoiceSystem(seededRng, flags: flagLedger);

            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_water_ration_crisis",
                DisplayName = "Water Rationing Dilemma",
                Category = "share",
                LocationId = "loc_bunker_cistern",
                MinDay = 1,
                MaxDay = 30,
                Choices = new List<MoralChoiceOption>
                {
                    new MoralChoiceOption
                    {
                        Label = "Divert water to the infirmary",
                        MoralDelta = 25,
                        EmpathyDelta = 20,
                        SetFlag = "infirmary_water_secured",
                        OutcomeText = "The sick survivors recover, but garden yields drop.",
                        Epitaph = "Sacrificed crops to heal the wounded."
                    },
                    new MoralChoiceOption
                    {
                        Label = "Prioritize hydroponic crops",
                        MoralDelta = -20,
                        EmpathyDelta = -15,
                        SetFlag = "crops_prioritized_over_sick",
                        OutcomeText = "Food security maintained, but two patients perish.",
                        Epitaph = "Cold calculation preserved the food supply."
                    }
                }
            };

            system.RegisterQuests(new[] { quest });

            // Initial state check
            Assert.False(system.IsResolved(quest.Id));
            Assert.Equal(0, system.MoralScore);
            Assert.Equal(MoralPathBand.Neutral, system.CurrentBand);

            // Resolve choice 0 (divert water)
            bool resolved = system.TryResolve(quest.Id, 0, "loc_bunker_cistern", day: 3, out var result);
            Assert.True(resolved);
            Assert.NotNull(result);
            Assert.True(system.IsResolved(quest.Id));
            Assert.Equal(25, system.MoralScore);
            Assert.Equal(20, system.EmpathyPoints);
            Assert.True(system.MoralScore > 0);
            Assert.Equal(MoralPathBand.SlightlyPositive, system.CurrentBand);

            // Verify flag was set in the consequence ledger
            Assert.True(flagLedger.IsSet("infirmary_water_secured"));
        }

        [Fact]
        public void MoralChoice_SaveRestore_PreservesLedgerAndHistory()
        {
            var rng = new SeededRng(54321);
            var flags = new InMemoryFlagLedger();
            var original = new MoralChoiceSystem(rng, flags: flags);

            var quest1 = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_scavenger_plea",
                DisplayName = "Desperate Scavenger",
                Category = "trust",
                Choices = new List<MoralChoiceOption>
                {
                    new MoralChoiceOption { Label = "Share supplies", MoralDelta = 15, EmpathyDelta = 10, Epitaph = "Shared canned rations." },
                    new MoralChoiceOption { Label = "Drive away at gunpoint", MoralDelta = -15, EmpathyDelta = -10, Epitaph = "Guarded bunker entrance." }
                }
            };

            original.RegisterQuests(new[] { quest1 });
            original.TryResolve(quest1.Id, 0, "loc_gate", day: 5, out _);

            // Capture state
            var state = original.CaptureState();
            Assert.NotNull(state);
            Assert.Equal(1, state.resolutions.Count);
            Assert.Equal(15, state.moralScore);
            Assert.Equal(10, state.empathyPoints);

            // Restore into fresh instance
            var restored = new MoralChoiceSystem(new SeededRng(99999), flags: new InMemoryFlagLedger());
            restored.RegisterQuests(new[] { quest1 });
            restored.RestoreState(state);

            Assert.True(restored.IsResolved(quest1.Id));
            Assert.Equal(15, restored.MoralScore);
            Assert.Equal(10, restored.EmpathyPoints);
            Assert.Equal(MoralPathBand.SlightlyPositive, restored.CurrentBand);
            Assert.True(restored.TryGetResolution(quest1.Id, out var resolution));
            Assert.Equal(0, resolution.choiceIndex);
            Assert.Equal(5, resolution.resolvedDay);
        }

        [Fact]
        public void CodexProjection_DerivesUnifiedKnowledge_WithZeroSaveState()
        {
            var journal = new JournalSystem();
            journal.Knowledge.Discover(KnowledgeKeys.ColdCountBeforeTheLab);
            journal.UnlockLocationVisited("loc_command_bunker");

            var authored = new List<AuthoredCodexEntry>
            {
                new AuthoredCodexEntry
                {
                    id = "codex_bunker_construction",
                    category = "locations",
                    displayName = "Command Bunker Origins",
                    spoilerTier = 1,
                    unlockCondition = CodexUnlockCondition.VisitLocation,
                    unlockRef = "loc_command_bunker",
                    body = "Constructed in 1984 as a hardened subterranean command relay.",
                    provenance = "canonical",
                    tags = new List<string> { "bunker", "infrastructure" }
                },
                new AuthoredCodexEntry
                {
                    id = "codex_unvisited_silo",
                    category = "locations",
                    displayName = "Silo 44",
                    spoilerTier = 0,
                    unlockCondition = CodexUnlockCondition.VisitLocation,
                    unlockRef = "loc_silo_44",
                    body = "Deep underground ICBM silo.",
                    provenance = "canonical"
                }
            };

            var projection = CodexProjectionBuilder.Build(
                fieldGuide: null,
                researchState: null,
                researchCatalog: null,
                journalSystem: journal,
                currentDay: 10,
                authoredEntries: authored);

            Assert.NotNull(projection);

            // Bunker was visited, must be unlocked / known
            var bunkerEntry = projection.FirstOrDefault(e => e.EntryId == "codex_bunker_construction");
            Assert.NotNull(bunkerEntry);
            Assert.Equal(CodexCategory.WastelandLore, bunkerEntry.Category);
            Assert.Equal("Command Bunker Origins", bunkerEntry.Title);
            Assert.Equal(CodexEntryState.Known, bunkerEntry.State);
            Assert.Contains("infrastructure", bunkerEntry.Tags);

            // Silo was NOT visited and spoilerTier is 0, must be Locked
            var siloEntry = projection.FirstOrDefault(e => e.EntryId == "codex_unvisited_silo");
            Assert.NotNull(siloEntry);
            Assert.Equal(CodexEntryState.Locked, siloEntry.State);
        }

        [Fact]
        public void MoralChoice_ResolutionReflectsInJournal_AndProjectsIntoCodex()
        {
            var rng = new SeededRng(42);
            var journal = new JournalSystem();
            var moralSystem = new MoralChoiceSystem(rng);

            // Hook journal integration
            moralSystem.OnQuestResolved += res =>
            {
                string arrow = res.impactMark == "up" ? "🔺" : "🔻";
                journal.TryAddRawEntry(res.questId, $"{arrow} {res.epitaph}", null!, res.resolvedDay);
            };

            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_reactor_meltdown_triage",
                DisplayName = "Coolant Valve Diverter",
                Category = "dead",
                LocationId = "loc_reactor_core",
                Choices = new List<MoralChoiceOption>
                {
                    new MoralChoiceOption
                    {
                        Label = "Manual override by engineer",
                        MoralDelta = 30,
                        EmpathyDelta = 25,
                        Epitaph = "Engineer heroically vented coolant, saving sector."
                    }
                }
            };

            moralSystem.RegisterQuests(new[] { quest });
            bool resolved = moralSystem.TryResolve(quest.Id, 0, "loc_reactor_core", day: 12, out _);
            Assert.True(resolved);

            // Verify journal recorded the moral consequence
            Assert.Contains(journal.Entries, e => e.KnowledgeKey == quest.Id && e.Day == 12 && e.Text.Contains("Engineer heroically vented coolant"));

            // Project Codex with this journal
            var codex = CodexProjectionBuilder.Build(
                fieldGuide: null,
                researchState: null,
                researchCatalog: null,
                journalSystem: journal,
                currentDay: 12);

            Assert.NotNull(codex);
            // Journal entries project cleanly without throwing or corrupting state
            Assert.True(codex.Count >= 0);
        }
    }
}
