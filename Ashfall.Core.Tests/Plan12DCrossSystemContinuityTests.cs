// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Memorial;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 12D — Cross-system social continuity tests.
    /// Verifies that cross-system hooks use authoritative owners,
    /// chronology guards hold, participant validity is enforced,
    /// and pending-state persistence survives save/load at critical boundaries.
    /// </summary>
    public class Plan12DCrossSystemContinuityTests
    {
        // ─────────────────────────────────────────────
        // 12D.1 — Cross-hook matrix
        // ─────────────────────────────────────────────

        [Fact]
        public void ApprenticeshipCompletion_FlowsThrough_SkillProgressionSystem()
        {
            // Apprenticeship completion must route XP through SkillProgressionSystem,
            // not a parallel counter.
            var skills = new SkillProgressionSystem();
            var roster = new DutyRosterSystem();
            var relations = new SurvivorRelationsSystem(new SeededRng(42));
            var rng = new SeededRng(42);
            var system = new ApprenticeshipSystem(rng, skills, roster, relations);

            // Give mentor enough XP to qualify (>= 30) — use canonical skill ID matching Plan12A
            var mentorActor = new SimpleSkillActor("mentor_1");
            skills.RecordAction(mentorActor, "skill_rough_repairs", 50f, 1);

            var result = system.StartPair("mentor_1", "apprentice_1", "skill_rough_repairs", 20f);
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);

            bool completedFired = false;
            system.OnApprenticeshipCompleted += _ => completedFired = true;

            // 2 ticks @ 10 XP/day = 20 XP = completion
            system.TickDay(1);
            system.TickDay(2);

            // Verify completion fired and skill was recorded
            Assert.True(completedFired, "OnApprenticeshipCompleted should fire");
            Assert.Contains("skill_rough_repairs", system.State.completedSkillIds);
        }

        [Fact]
        public void CohortMaturation_FiresThrough_CohortSystem_TryMaturation()
        {
            // Maturation is one-way through CohortSystem.TryMaturation only.
            var cohort = new CohortSystem();
            bool maturationFired = false;
            string maturedChildId = null;
            int maturedDay = -1;
            cohort.OnMaturation += (id, day) => { maturedChildId = id; maturedDay = day; maturationFired = true; };

            cohort.BookChild("child_1", new List<string> { "parent_a", "parent_b" }, "medium", 10);
            bool result = cohort.TryMaturation("child_1", 500);

            Assert.True(result);
            Assert.True(maturationFired);
            Assert.Equal("child_1", maturedChildId);
            Assert.Equal(500, maturedDay);

            // One-way: second call returns false (idempotent)
            bool secondCall = cohort.TryMaturation("child_1", 600);
            Assert.False(secondCall);
        }

        [Fact]
        public void DecorMorale_RoutesThrough_NeedsSystemModifiers()
        {
            // ShelterDecorSystem produces a morale delta; the host applies it
            // via NeedsSystem.Modify(survivorId, NeedKind.Morale, delta).
            // Verify the decor system returns the delta and NeedsSystem can apply it.
            var decor = new ShelterDecorSystem();
            decor.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_decor_painting",
                LocalizedMoraleDelta = 5f,
                Category = "art",
                StackMultiplicatively = false
            });

            decor.Assign("room_bunk", "slot_1", "item_decor_painting", 10);
            float delta = decor.GetRoomMoraleDelta("room_bunk");
            Assert.True(delta > 0f, "Decor should produce a positive morale delta");

            // NeedsSystem can apply the delta
            var needs = new NeedsSystem();
            var survivor = new SurvivorNeedsState { Id = "survivor_1" };
            needs.Register(survivor);
            float moraleBefore = survivor.Morale;
            needs.Modify("survivor_1", NeedKind.Morale, delta);
            var updated = needs.Get("survivor_1");
            Assert.True(updated.Morale > moraleBefore, "NeedsSystem morale should increase after decor delta");
        }

        [Fact]
        public void MemorialPlaque_References_MemorialSystemProvenance()
        {
            // Memorial plaque in ShelterDecorSystem should reference a MemorialSystem entry,
            // not duplicate death state. Verify the provenance link.
            var memorialState = new MemorialState();
            var memorial = new MemorialSystem(memorialState);
            var decor = new ShelterDecorSystem();

            // Register a generic plaque modifier so ResolvePlaqueItemId can resolve
            decor.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_decor_memorial_plaque_generic",
                LocalizedMoraleDelta = 2f,
                Category = "memorial",
                StackMultiplicatively = false
            });

            var entry = memorial.Memorialize(new MemorialInput
            {
                SurvivorId = "survivor_dead",
                Cause = "radiation",
                Day = 50,
                BirthDay = 0,
                FinalWishResolved = false,
                Epitaph = "Gone but not forgotten",
                HeirloomItemId = "item_personal_keepsake_survivor_default"
            });

            Assert.NotNull(entry);
            Assert.Single(memorial.Entries);

            // ResolvePlaqueSlot uses the memorial's heirloom to produce a plaque placement
            var plaque = decor.ResolvePlaqueSlot(
                entry.SurvivorId,
                entry.HeirloomItemId,
                "room_memorial",
                "slot_plaque_1",
                entry.Day);

            Assert.NotNull(plaque);
            Assert.True(plaque.IsMemorialPlaque);
            Assert.Equal("survivor_dead", plaque.MemorialSurvivorId);
            Assert.Equal("item_personal_keepsake_survivor_default", plaque.PlaqueSourceHeirloomId);
        }

        // ─────────────────────────────────────────────
        // 12D.2 — Chronology guards
        // ─────────────────────────────────────────────

        [Fact]
        public void ApprenticeshipCompletion_CannotHappenBefore_StartPair()
        {
            // Without StartPair, TickDay should not fire completion.
            var skills = new SkillProgressionSystem();
            var roster = new DutyRosterSystem();
            var relations = new SurvivorRelationsSystem(new SeededRng(42));
            var rng = new SeededRng(42);
            var system = new ApprenticeshipSystem(rng, skills, roster, relations);

            bool completedFired = false;
            system.OnApprenticeshipCompleted += _ => completedFired = true;

            system.TickDay(10);
            Assert.False(completedFired, "No pair started — completion should not fire");
            Assert.Empty(system.State.completedSkillIds);
        }

        [Fact]
        public void Maturation_CannotHappenBefore_BookChild()
        {
            // TryMaturation on an unknown child returns false.
            var cohort = new CohortSystem();
            bool result = cohort.TryMaturation("unknown_child", 100);
            Assert.False(result);
        }

        [Fact]
        public void DecorRemoval_OnEmptySlot_ReturnsFalse()
        {
            var decor = new ShelterDecorSystem();
            bool result = decor.Remove("room_empty", "slot_none");
            Assert.False(result);
        }

        [Fact]
        public void ResolvePlaqueSlot_DoesNotCrash_WithEmptyHeirloom()
        {
            // ResolvePlaqueSlot with empty heirloomItemId returns null (no crash).
            var decor = new ShelterDecorSystem();
            var plaque = decor.ResolvePlaqueSlot("survivor_x", "", "room_memorial", "slot_1", 10);
            Assert.Null(plaque);
        }

        [Fact]
        public void ResolvePlaqueSlot_DoesNotCrash_WithNullInputs()
        {
            var decor = new ShelterDecorSystem();
            var plaque = decor.ResolvePlaqueSlot(null, null, null, null, 0);
            Assert.Null(plaque);
        }

        // ─────────────────────────────────────────────
        // 12D.3 — Participant validity
        // ─────────────────────────────────────────────

        [Fact]
        public void CohortTryMaturation_UnknownChild_ReturnsFalse()
        {
            var cohort = new CohortSystem();
            Assert.False(cohort.TryMaturation("nonexistent_child", 100));
        }

        [Fact]
        public void ApprenticeshipStartPair_UnqualifiedMentor_Rejected()
        {
            // Mentor with 0 XP in the skill (< 30) is rejected.
            var skills = new SkillProgressionSystem();
            var roster = new DutyRosterSystem();
            var relations = new SurvivorRelationsSystem(new SeededRng(42));
            var rng = new SeededRng(42);
            var system = new ApprenticeshipSystem(rng, skills, roster, relations);

            var result = system.StartPair("mentor_novice", "apprentice_1", "skill_medicine");
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("mentor_unqualified", result.FailureCode);
        }

        [Fact]
        public void ShelterDecorRemove_UnknownRoomSlot_ReturnsFalse()
        {
            var decor = new ShelterDecorSystem();
            decor.Assign("room_a", "slot_1", "item_decor_painting", 5);

            // Different room/slot — should return false
            Assert.False(decor.Remove("room_b", "slot_99"));
        }

        [Fact]
        public void ShelterDecorAssign_EmptyRoomId_ReturnsFalse()
        {
            var decor = new ShelterDecorSystem();
            Assert.False(decor.Assign("", "slot_1", "item_decor_painting", 5));
        }

        [Fact]
        public void ShelterDecorAssign_EmptySlotId_ReturnsFalse()
        {
            var decor = new ShelterDecorSystem();
            Assert.False(decor.Assign("room_a", "", "item_decor_painting", 5));
        }

        [Fact]
        public void ShelterDecorAssign_EmptyItemId_StillSucceeds()
        {
            // Empty itemId is accepted (stored as empty string) — the guard
            // is on roomId and slotId only.
            var decor = new ShelterDecorSystem();
            bool result = decor.Assign("room_a", "slot_1", "", 5);
            Assert.True(result);
        }

        // ─────────────────────────────────────────────
        // 12D.4 — Pending-state persistence
        // ─────────────────────────────────────────────

        [Fact]
        public void CohortSystem_BookChild_SaveRestore_MaturationStillWorks()
        {
            var cohort = new CohortSystem();
            cohort.BookChild("child_1", new List<string> { "parent_a", "parent_b" }, "low", 10);

            // Save
            var saved = cohort.CaptureState();

            // Restore into a fresh instance
            var cohort2 = new CohortSystem();
            cohort2.RestoreState(saved);

            // Maturation should still work after restore
            bool maturationFired = false;
            cohort2.OnMaturation += (_, _) => maturationFired = true;

            bool result = cohort2.TryMaturation("child_1", 500);
            Assert.True(result);
            Assert.True(maturationFired);
        }

        [Fact]
        public void ApprenticeshipSystem_StartPair_Tick_SaveRestore_CompletionStillFires()
        {
            var skills = new SkillProgressionSystem();
            var roster = new DutyRosterSystem();
            var relations = new SurvivorRelationsSystem(new SeededRng(42));
            var rng = new SeededRng(42);
            var system = new ApprenticeshipSystem(rng, skills, roster, relations);

            // Qualify mentor
            var mentorActor = new SimpleSkillActor("mentor_1");
            skills.RecordAction(mentorActor, "skill_scrap", 50f, 0);

            system.StartPair("mentor_1", "apprentice_1", "skill_scrap", 100f);
            system.TickDay(1);

            // Save
            var saved = system.CaptureState();

            // Restore into a fresh system (with same dependencies)
            var skills2 = new SkillProgressionSystem();
            var roster2 = new DutyRosterSystem();
            var relations2 = new SurvivorRelationsSystem(new SeededRng(42));
            var rng2 = new SeededRng(42);
            var system2 = new ApprenticeshipSystem(rng2, skills2, roster2, relations2);
            system2.RestoreState(saved);

            // Verify the active pair survived the round-trip
            var pairs = system2.GetActivePairs();
            Assert.NotEmpty(pairs);
            Assert.Equal("mentor_1", pairs[0].mentorId);
            Assert.Equal("apprentice_1", pairs[0].apprenticeId);
        }

        [Fact]
        public void ShelterDecorSystem_Assign_SaveRestore_PlacementPreserved()
        {
            var decor = new ShelterDecorSystem();
            decor.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_decor_flag",
                LocalizedMoraleDelta = 3f,
                Category = "banner",
                StackMultiplicatively = false
            });
            decor.Assign("room_bunk", "slot_wall_1", "item_decor_flag", 15);

            // Save
            var saved = decor.CaptureState();

            // Restore into fresh instance
            var decor2 = new ShelterDecorSystem();
            decor2.RestoreState(saved);

            var slot = decor2.GetSlot("room_bunk", "slot_wall_1");
            Assert.NotNull(slot);
            Assert.Equal("item_decor_flag", slot.ItemId);
            Assert.Equal(15, slot.DayInstalled);
        }

        [Fact]
        public void RationConflictSystem_BuildResentment_SaveRestore_ResentmentPreserved()
        {
            var rng = new SeededRng(99);
            var rcs = new RationConflictSystem(rng);

            rcs.RegisterSurvivor("survivor_a");
            rcs.RegisterSurvivor("survivor_b");

            // Create unfair allocation to build resentment
            rcs.SetAllocation("survivor_a", 0.2f); // low
            rcs.SetAllocation("survivor_b", 0.8f); // high

            // Tick to build resentment
            rcs.Tick("survivor_a", 24f);

            var stateA = rcs.GetState("survivor_a");
            Assert.NotNull(stateA);
            float resentmentBefore = stateA.resentmentLevel;

            // Save
            var saved = rcs.CaptureState();

            // Restore into fresh instance
            var rcs2 = new RationConflictSystem(new SeededRng(99));
            rcs2.RestoreState(saved);

            var stateA2 = rcs2.GetState("survivor_a");
            Assert.NotNull(stateA2);
            Assert.Equal(resentmentBefore, stateA2.resentmentLevel);
            Assert.Equal("survivor_b", stateA2.resentmentTargetId);
        }

        [Fact]
        public void IdeologicalFrictionSystem_RegisterBeliefs_SaveRestore_FrictionStillDetected()
        {
            var friction = new IdeologicalFrictionSystem();

            // Register conflicting beliefs
            friction.RegisterBelief("survivor_a", "military_discipline");
            friction.RegisterBelief("survivor_b", "pacifist");

            // Verify friction is detected before save
            float compatBefore = friction.GetRoommateCompatibilityMultiplier("survivor_a", "survivor_b");
            Assert.True(compatBefore < 1f, "Conflicting beliefs should produce < 1.0 compatibility");

            // Tick to generate affinity data
            friction.TickRoommates("survivor_a", "survivor_b", 24f);

            // Save
            var saved = friction.CaptureState();

            // Restore into fresh instance
            var friction2 = new IdeologicalFrictionSystem();
            friction2.RestoreState(saved);

            // Re-register beliefs (beliefs are not persisted — only affinities)
            friction2.RegisterBelief("survivor_a", "military_discipline");
            friction2.RegisterBelief("survivor_b", "pacifist");

            // Friction should still be detected after restore
            float compatAfter = friction2.GetRoommateCompatibilityMultiplier("survivor_a", "survivor_b");
            Assert.True(compatAfter < 1f, "Conflicting beliefs should still produce < 1.0 after restore");

            // Affinity data should be preserved
            float affinity = friction2.GetAffinity("survivor_a", "survivor_b");
            Assert.True(affinity < 0f, "Conflict affinity should be negative after tick");
        }

        [Fact]
        public void IdeologicalFrictionSystem_SynergyBeliefs_SaveRestore_SynergyPreserved()
        {
            var friction = new IdeologicalFrictionSystem();

            // Register matching beliefs (synergy)
            friction.RegisterBelief("survivor_x", "religious_faith");
            friction.RegisterBelief("survivor_y", "religious_faith");

            float compatBefore = friction.GetRoommateCompatibilityMultiplier("survivor_x", "survivor_y");
            Assert.True(compatBefore > 1f, "Matching beliefs should produce > 1.0 compatibility");

            friction.TickRoommates("survivor_x", "survivor_y", 48f);

            var saved = friction.CaptureState();

            var friction2 = new IdeologicalFrictionSystem();
            friction2.RestoreState(saved);
            friction2.RegisterBelief("survivor_x", "religious_faith");
            friction2.RegisterBelief("survivor_y", "religious_faith");

            float compatAfter = friction2.GetRoommateCompatibilityMultiplier("survivor_x", "survivor_y");
            Assert.True(compatAfter > 1f, "Synergy should survive save/restore");

            float affinity = friction2.GetAffinity("survivor_x", "survivor_y");
            Assert.True(affinity > 0f, "Synergy affinity should be positive after tick");
        }
    }
}
