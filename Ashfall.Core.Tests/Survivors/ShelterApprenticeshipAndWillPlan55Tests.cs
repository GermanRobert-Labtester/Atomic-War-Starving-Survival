// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ShelterApprenticeshipAndWillPlan55Tests
    {
        private static (ApprenticeshipSystem system, SkillProgressionSystem skills, ShelterAssignmentSystem assign, InventoryContainer inv) CreateFixture()
        {
            var rng = new SeededRng(42);
            var skills = new SkillProgressionSystem();
            var roster = new DutyRosterSystem();
            var relations = new SurvivorRelationsSystem(rng);
            var inv = new InventoryContainer();

            var rooms = new List<ShelterRoom>
            {
                new ShelterRoom("room_workshop", "Main Workshop", 4),
                new ShelterRoom("room_medical", "Clinic Ward", 4),
                new ShelterRoom("room_living", "Living Quarters", 4)
            };
            var assignState = new ShelterAssignmentState();
            var assign = new ShelterAssignmentSystem(assignState, rooms, rng);

            var system = new ApprenticeshipSystem(rng, skills, roster, relations, assign, inv);
            return (system, skills, assign, inv);
        }

        [Fact]
        public void CoOccupancy_GrantsFullXpWhenInSameRoom_ReducedWhenSeparated()
        {
            var (system, skills, assign, _) = CreateFixture();

            // Mentor qualification: 50 XP
            skills.RecordAction(new SimpleSkillActor("mentor_bob"), "engineering", 50f, 1);

            // Pair A: Same room
            assign.Assign("mentor_bob", "room_workshop", null, 1);
            assign.Assign("apprentice_tim", "room_workshop", null, 1);

            var startRes = system.StartPair("mentor_bob", "apprentice_tim", "engineering", 100f, "mentorship_generator_maintenance");
            Assert.Equal(ActionResult.StatusKind.Success, startRes.Status);

            system.TickDay(1);

            var pair = system.State.activePairs[0];
            Assert.Equal(15f, pair.progressXp); // Full rate: 15 XP

            // Now move apprentice to another room
            assign.Unassign("apprentice_tim", 2);
            assign.Assign("apprentice_tim", "room_living", null, 2);

            system.TickDay(2);
            // Day 2 adds 15 * 0.5 = 7.5 -> 22.5 XP
            Assert.Equal(22.5f, pair.progressXp);
        }

        [Fact]
        public void ManualTranscription_ProducesManualItemOnCompletion()
        {
            var (system, skills, _, inv) = CreateFixture();

            // Scribe qualification: 40 XP in medicine
            skills.RecordAction(new SimpleSkillActor("doc_alice"), "medicine", 40f, 1);

            var startRes = system.StartTranscription("doc_alice", "mentorship_field_medicine", inv);
            Assert.Equal(ActionResult.StatusKind.Success, startRes.Status);
            Assert.Single(system.State.transcriptionTasks);

            var task = system.State.transcriptionTasks[0];
            Assert.Equal(5, task.daysRequired);

            // Tick 5 days to complete
            for (int day = 1; day <= 5; day++)
            {
                system.TickDay(day);
            }

            Assert.True(task.isComplete);
            Assert.Equal(1, inv.CountById("item_manual_field_medicine"));
        }

        [Fact]
        public void SurvivorWill_RegistersAndExecutesAtomically()
        {
            var (system, _, _, inv) = CreateFixture();

            var will = new SurvivorWill
            {
                willId = "will_old_pete",
                testatorSurvivorId = "survivor_pete",
                primaryBeneficiaryId = "survivor_sarah",
                fallbackBeneficiaryId = "survivor_mark",
                bequeathedItemIds = new List<string> { "item_blowtorch", "item_scrap_metal" },
                finalWords = "Keep the fires burning."
            };

            var regRes = system.RegisterWill(will);
            Assert.Equal(ActionResult.StatusKind.Success, regRes.Status);
            Assert.Single(system.State.registeredWills);

            // Execute upon Pete's death
            var living = new HashSet<string> { "survivor_sarah", "survivor_mark" };
            var execRes = system.ExecuteWill("survivor_pete", inv, living);

            Assert.Equal(ActionResult.StatusKind.Success, execRes.Status);
            Assert.Empty(system.State.registeredWills);
            Assert.Single(system.State.executedWills);
            Assert.True(system.State.executedWills[0].isExecuted);

            // Sarah inherits items
            Assert.Equal(1, inv.CountById("item_blowtorch"));
            Assert.Equal(1, inv.CountById("item_scrap_metal"));
        }

        [Fact]
        public void SurvivorWill_FallsBackWhenPrimaryBeneficiaryDeceased()
        {
            var (system, _, _, inv) = CreateFixture();

            var will = new SurvivorWill
            {
                testatorSurvivorId = "survivor_pete",
                primaryBeneficiaryId = "survivor_sarah",
                fallbackBeneficiaryId = "survivor_mark",
                bequeathedItemIds = new List<string> { "item_blowtorch" }
            };
            system.RegisterWill(will);

            // Sarah is dead; only Mark is alive
            var living = new HashSet<string> { "survivor_mark" };
            string actualRecipient = string.Empty;
            system.OnWillExecuted += (w, rec) => actualRecipient = rec;

            var execRes = system.ExecuteWill("survivor_pete", inv, living);
            Assert.Equal(ActionResult.StatusKind.Success, execRes.Status);
            Assert.Equal("survivor_mark", actualRecipient);
            Assert.Equal(1, inv.CountById("item_blowtorch"));
        }

        [Fact]
        public void MentorDeath_AwardsLegacyTraitAndBonusXp()
        {
            var (system, skills, _, _) = CreateFixture();

            skills.RecordAction(new SimpleSkillActor("mentor_pete"), "engineering", 60f, 1);
            system.StartPair("mentor_pete", "apprentice_jack", "engineering", 100f, "mentorship_generator_maintenance");

            string inheritedTrait = string.Empty;
            float inheritedXp = 0f;
            system.OnMentorLegacyInherited += (appId, trait, xp) =>
            {
                inheritedTrait = trait;
                inheritedXp = xp;
            };

            // Pete dies
            system.NotifyMentorDeath("mentor_pete");

            Assert.Equal("trait_stoic_craftsman", inheritedTrait);
            Assert.Equal(150f, inheritedXp);
            Assert.Contains("trait_stoic_craftsman", system.State.legacyTraitsGranted["apprentice_jack"]);
            Assert.True(system.State.activePairs[0].isComplete);
            Assert.True(system.State.activePairs[0].isLegacyInherited);
        }

        [Fact]
        public void StatePreservation_RoundTripsWillsAndTasks()
        {
            var (system, _, _, _) = CreateFixture();

            system.RegisterWill(new SurvivorWill
            {
                testatorSurvivorId = "s1",
                primaryBeneficiaryId = "s2",
                bequeathedItemIds = new List<string> { "item_fuel" }
            });
            system.State.legacyTraitsGranted["s2"] = new List<string> { "trait_hardened_disciple" };

            var state = system.CaptureState();

            var (restored, _, _, _) = CreateFixture();
            restored.RestoreState(state);

            Assert.Single(restored.State.registeredWills);
            Assert.Equal("s1", restored.State.registeredWills[0].testatorSurvivorId);
            Assert.Contains("trait_hardened_disciple", restored.State.legacyTraitsGranted["s2"]);
        }
    }
}
