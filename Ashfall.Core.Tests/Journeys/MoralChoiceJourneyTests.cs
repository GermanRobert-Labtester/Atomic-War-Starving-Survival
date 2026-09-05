// SPDX-License-Identifier: MIT
// ASHFALL Moral Choice Journey & Host-UI Action Path Verification (REM-004 / R07).
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Flags;
using Ashfall.Core.Journal;
using Ashfall.Core.MoralChoice;
using Xunit;

namespace Ashfall.Core.Tests.Journeys
{
    public sealed class MoralChoiceJourneyTests
    {
        [Fact]
        public void MoralChoiceJourney_Encounter_Resolve_SaveReload_PreventsDuplicateResolution()
        {
            var flags = new InMemoryFlagLedger();
            var rng = new SeededRng(20260904);
            var system = new MoralChoiceSystem(rng, flags: flags);

            // Authored dilemma definition
            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_water_share",
                DisplayName = "The Thirsty Wanderer",
                Category = "share",
                Discovery = "A desperate wanderer collapses against the exterior decontamination hatch, begging for a single canteen of water.",
                LocationId = "loc_surface_airlock",
                MinDay = 1,
                MaxDay = 10,
                Choices = new List<MoralChoiceOption>
                {
                    new()
                    {
                        Label = "Share a clean water ration",
                        MoralDelta = 15,
                        EmpathyDelta = 10,
                        OutcomeText = "The wanderer drinks with trembling gratitude and promises to spread word of the shelter's humanity.",
                        Epitaph = "Gave water to the dying."
                    },
                    new()
                    {
                        Label = "Drive them away into the ash storm",
                        MoralDelta = -15,
                        EmpathyDelta = -5,
                        OutcomeText = "The wanderer curses the bunker door before vanishing into the particulate haze.",
                        Epitaph = "Hoarded water behind sealed iron."
                    }
                }
            };

            // Track events
            var resolvedEvents = new List<MoralChoiceResolution>();
            system.OnQuestResolved += r => resolvedEvents.Add(r);

            // Step 1: Initial state verification
            Assert.False(system.IsResolved(quest.Id));
            Assert.Equal(0, system.QuestsResolved);
            Assert.Equal(0, system.MoralScore);
            Assert.Equal(0, system.EmpathyPoints);
            Assert.Equal(MoralPathBand.Neutral, system.CurrentBand);

            // Step 2: Resolve Option 0 (Compassion / Share)
            var resolution = system.Resolve(quest, choiceIndex: 0, quest.LocationId, day: 1);

            Assert.NotNull(resolution);
            Assert.Equal(quest.Id, resolution.questId);
            Assert.Equal(0, resolution.choiceIndex);
            Assert.Single(resolvedEvents);
            Assert.True(system.IsResolved(quest.Id));
            Assert.Equal(1, system.QuestsResolved);
            Assert.Equal(15, system.MoralScore);
            Assert.Equal(10, system.EmpathyPoints);
            Assert.Equal(MoralPathBand.SlightlyPositive, system.CurrentBand);

            // Step 3: Save to state DTO and serialize
            var savedState = system.CaptureState();
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(savedState);
            Assert.False(string.IsNullOrWhiteSpace(json));

            // Step 4: Restore in a fresh system instance
            var reloadedFlags = new InMemoryFlagLedger();
            var reloadedRng = new SeededRng(20260904);
            var reloadedSystem = new MoralChoiceSystem(reloadedRng, flags: reloadedFlags);
            var restoredState = serializer.Deserialize<MoralChoiceState>(json);
            Assert.NotNull(restoredState);
            reloadedSystem.RestoreState(restoredState);

            // Step 5: Verify restored state integrity
            Assert.True(reloadedSystem.IsResolved(quest.Id));
            Assert.Equal(1, reloadedSystem.QuestsResolved);
            Assert.Equal(15, reloadedSystem.MoralScore);
            Assert.Equal(10, reloadedSystem.EmpathyPoints);
            Assert.Equal(MoralPathBand.SlightlyPositive, reloadedSystem.CurrentBand);

            // Step 6: Verify lockout against duplicate selection / double application
            // Host pattern: check IsResolved before invoking Resolve
            bool canResolveAgain = !reloadedSystem.IsResolved(quest.Id);
            Assert.False(canResolveAgain, "Host must reject already resolved moral choices");

            // Even if Resolve is called directly on Core, it returns stored resolution and does not re-apply deltas
            var duplicateRes = reloadedSystem.Resolve(quest, choiceIndex: 1, quest.LocationId, day: 2);
            Assert.Equal(0, duplicateRes.choiceIndex); // Stored original choice preserved
            Assert.Equal(15, reloadedSystem.MoralScore); // Score NOT corrupted by second call
            Assert.Equal(1, reloadedSystem.QuestsResolved); // Count remains exactly 1
        }
    }
}
