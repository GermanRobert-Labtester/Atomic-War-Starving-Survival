// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 12E — balance, frequency, and long-campaign simulation properties
    /// for social systems (CohortSystem, ShelterDecorSystem, RationConflictSystem,
    /// IdeologicalFrictionSystem).
    /// </summary>
    public class Plan12EBalanceSimulationTests
    {
        // ──────────────────────────────────────────────────────────────
        // 12E.1 — Deterministic simulations
        // ──────────────────────────────────────────────────────────────

        [Fact]
        public void CohortSystem_SameSeed_ProducesSameMaturationDays()
        {
            // Run 1: book 4 children, tick until all mature
            var sys1 = new CohortSystem();
            var parentIds = new List<string> { "survivor_alpha", "survivor_beta" };
            for (int i = 0; i < 4; i++)
                sys1.BookChild($"child_{i}", parentIds, "medium", 10);

            var maturationDays1 = new Dictionary<string, int>();
            for (int day = 11; day <= 500; day++)
                for (int i = 0; i < 4; i++)
                    if (sys1.TryMaturation($"child_{i}", day))
                        maturationDays1[$"child_{i}"] = day;

            // Run 2: identical setup
            var sys2 = new CohortSystem();
            for (int i = 0; i < 4; i++)
                sys2.BookChild($"child_{i}", parentIds, "medium", 10);

            var maturationDays2 = new Dictionary<string, int>();
            for (int day = 11; day <= 500; day++)
                for (int i = 0; i < 4; i++)
                    if (sys2.TryMaturation($"child_{i}", day))
                        maturationDays2[$"child_{i}"] = day;

            // Same children matured on the same days
            Assert.Equal(maturationDays1.Count, maturationDays2.Count);
            foreach (var kv in maturationDays1)
                Assert.Equal(kv.Value, maturationDays2[kv.Key]);
        }

        [Fact]
        public void ShelterDecorSystem_MoraleCalculation_IsDeterministic()
        {
            float RunOnce()
            {
                var sys = new ShelterDecorSystem();
                sys.RegisterItemModifier(new ShelterDecorItemModifier
                {
                    ItemId = "item_photo_frame", LocalizedMoraleDelta = 2.0f, Category = "personal"
                });
                sys.RegisterItemModifier(new ShelterDecorItemModifier
                {
                    ItemId = "item_painted_stone", LocalizedMoraleDelta = 1.5f, Category = "craft"
                });
                sys.RegisterItemModifier(new ShelterDecorItemModifier
                {
                    ItemId = "item_dried_flowers", LocalizedMoraleDelta = 1.0f, Category = "nature"
                });

                sys.Assign("room_a", "slot_1", "item_photo_frame", 1);
                sys.Assign("room_a", "slot_2", "item_painted_stone", 1);
                sys.Assign("room_a", "slot_3", "item_dried_flowers", 1);

                return sys.GetRoomMoraleDelta("room_a");
            }

            float first = RunOnce();
            float second = RunOnce();
            Assert.Equal(first, second);
            Assert.True(first > 0f, "Sum of positive deltas should be positive");
        }

        [Fact]
        public void RationConflictSystem_EqualAllocations_NoResentment()
        {
            var rng = new SeededRng(42);
            var sys = new RationConflictSystem(rng);

            sys.RegisterSurvivor("surv_a");
            sys.RegisterSurvivor("surv_b");
            sys.RegisterSurvivor("surv_c");

            // All get the same allocation
            sys.SetAllocation("surv_a", 0.6f);
            sys.SetAllocation("surv_b", 0.6f);
            sys.SetAllocation("surv_c", 0.6f);

            // Tick 30 days (720 hours)
            for (int day = 0; day < 30; day++)
            {
                sys.Tick("surv_a", 24f);
                sys.Tick("surv_b", 24f);
                sys.Tick("surv_c", 24f);
            }

            foreach (var id in new[] { "surv_a", "surv_b", "surv_c" })
            {
                var state = sys.GetState(id);
                Assert.NotNull(state);
                Assert.Equal(0f, state.resentmentLevel);
                Assert.Equal(string.Empty, state.resentmentTargetId);
            }
        }

        [Fact]
        public void IdeologicalFrictionSystem_SameBeliefPairs_SameMultiplier()
        {
            float RunOnce()
            {
                var sys = new IdeologicalFrictionSystem();
                sys.RegisterBelief("surv_x", "military_discipline");
                sys.RegisterBelief("surv_y", "pacifist");
                return sys.GetRoommateCompatibilityMultiplier("surv_x", "surv_y");
            }

            float first = RunOnce();
            float second = RunOnce();
            Assert.Equal(first, second);
            Assert.True(first < 1f, "Conflicting beliefs should produce a penalty multiplier < 1");
        }

        // ──────────────────────────────────────────────────────────────
        // 12E.2 — Frequency bounds
        // ──────────────────────────────────────────────────────────────

        [Fact]
        public void CohortSystem_Maturation_IsOneWay_Idempotent()
        {
            var sys = new CohortSystem();
            sys.BookChild("child_01", new List<string> { "parent_a" }, "medium", 280);

            // Maturation requires sufficient age gap (same pattern as Plan12A tests)
            Assert.True(sys.TryMaturation("child_01", 374));
            int maturationDay = 374;

            // Trying again on the same day should fail (already matured)
            Assert.False(sys.TryMaturation("child_01", maturationDay),
                "TryMaturation should return false on already-matured child");

            // Trying on a later day should also fail
            Assert.False(sys.TryMaturation("child_01", maturationDay + 100),
                "TryMaturation should return false on already-matured child for any later day");

            // Verify the child record shows maturation
            var child = sys.GetChild("child_01");
            Assert.NotNull(child);
            Assert.True(child.isMatured);
            Assert.Equal(maturationDay, child.maturationDay);
        }

        [Fact]
        public void ShelterDecorSystem_GetRoomMoraleDelta_ReturnsBoundedValues()
        {
            var sys = new ShelterDecorSystem();

            // Register 12 items with moderate deltas
            for (int i = 0; i < 12; i++)
            {
                sys.RegisterItemModifier(new ShelterDecorItemModifier
                {
                    ItemId = $"decor_item_{i}",
                    LocalizedMoraleDelta = 2.5f,
                    Category = "test"
                });
            }

            // Assign all 12 to one room
            for (int i = 0; i < 12; i++)
                sys.Assign("room_max", $"slot_{i}", $"decor_item_{i}", 1);

            float delta = sys.GetRoomMoraleDelta("room_max");

            // 12 items × 2.5 = 30.0 — but the system should still be bounded
            // The test verifies the sum is computed correctly and is finite
            Assert.True(delta > 0f, "Morale delta should be positive with positive items");
            Assert.True(float.IsFinite(delta), "Morale delta must be finite");
            Assert.Equal(12 * 2.5f, delta, precision: 2);
        }

        [Fact]
        public void RationConflictSystem_ResentmentDecays_WhenAllocationsFair()
        {
            var rng = new SeededRng(99);
            var sys = new RationConflictSystem(rng);

            sys.RegisterSurvivor("surv_a");
            sys.RegisterSurvivor("surv_b");

            // Build resentment: unequal allocations
            sys.SetAllocation("surv_a", 0.3f);
            sys.SetAllocation("surv_b", 0.8f);

            for (int day = 0; day < 20; day++)
            {
                sys.Tick("surv_a", 24f);
                sys.Tick("surv_b", 24f);
            }

            var stateA = sys.GetState("surv_a");
            Assert.NotNull(stateA);
            float resentmentAfterUnequal = stateA.resentmentLevel;
            Assert.True(resentmentAfterUnequal > 0f,
                "Resentment should build under unequal allocations");

            // Now switch to equal allocations
            sys.SetAllocation("surv_a", 0.55f);
            sys.SetAllocation("surv_b", 0.55f);

            float prevResentment = resentmentAfterUnequal;
            for (int day = 0; day < 30; day++)
            {
                sys.Tick("surv_a", 24f);
                sys.Tick("surv_b", 24f);
            }

            var stateAfter = sys.GetState("surv_a");
            Assert.NotNull(stateAfter);
            Assert.True(stateAfter.resentmentLevel < prevResentment,
                "Resentment should decay when allocations become fair");
        }

        // ──────────────────────────────────────────────────────────────
        // 12E.3 — Morale balance (decor is small and localized)
        // ──────────────────────────────────────────────────────────────

        [Fact]
        public void ShelterDecorSystem_EmptyRoom_ReturnsZeroMorale()
        {
            var sys = new ShelterDecorSystem();
            Assert.Equal(0f, sys.GetRoomMoraleDelta("empty_room"));
        }

        [Fact]
        public void ShelterDecorSystem_SingleItem_ReturnsSmallPositiveDelta()
        {
            var sys = new ShelterDecorSystem();
            sys.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_candle", LocalizedMoraleDelta = 2.0f, Category = "lighting"
            });
            sys.Assign("room_1", "slot_1", "item_candle", 1);

            float delta = sys.GetRoomMoraleDelta("room_1");
            Assert.InRange(delta, 1.0f, 3.0f);
        }

        [Fact]
        public void ShelterDecorSystem_TwelveItems_NoticeableButNotGameBreaking()
        {
            var sys = new ShelterDecorSystem();

            // Register 12 items with moderate deltas (1.5–2.5 each)
            for (int i = 0; i < 12; i++)
            {
                sys.RegisterItemModifier(new ShelterDecorItemModifier
                {
                    ItemId = $"item_decor_{i}",
                    LocalizedMoraleDelta = 1.5f + (i % 3) * 0.5f, // 1.5, 2.0, 2.5 cycling
                    Category = "decor"
                });
            }

            for (int i = 0; i < 12; i++)
                sys.Assign("room_full", $"slot_{i}", $"item_decor_{i}", 1);

            float delta = sys.GetRoomMoraleDelta("room_full");

            // Should be noticeable but not game-breaking (< 30.0)
            Assert.True(delta > 0f, "Should be positive");
            Assert.True(delta < 30.0f, $"Delta {delta} should be < 30.0 to stay balanced");
        }

        [Fact]
        public void ShelterDecorSystem_MoraleCalculatedIndependentlyPerRoom()
        {
            var sys = new ShelterDecorSystem();
            sys.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_banner", LocalizedMoraleDelta = 2.0f, Category = "flag"
            });

            // Only assign to room_a
            sys.Assign("room_a", "slot_1", "item_banner", 1);

            float deltaA = sys.GetRoomMoraleDelta("room_a");
            float deltaB = sys.GetRoomMoraleDelta("room_b");

            Assert.True(deltaA > 0f, "room_a should have positive morale from the banner");
            Assert.Equal(0f, deltaB);
            Assert.NotEqual(deltaA, deltaB);
        }

        // ──────────────────────────────────────────────────────────────
        // 12E.4 — Social failure recovery
        // ──────────────────────────────────────────────────────────────

        [Fact]
        public void RationConflictSystem_ResentmentBuilds_ThenRecoverWithEqualAlloc()
        {
            var rng = new SeededRng(777);
            var sys = new RationConflictSystem(rng);

            sys.RegisterSurvivor("surv_x");
            sys.RegisterSurvivor("surv_y");

            // Phase 1: build resentment
            sys.SetAllocation("surv_x", 0.2f);
            sys.SetAllocation("surv_y", 0.9f);

            for (int day = 0; day < 15; day++)
            {
                sys.Tick("surv_x", 24f);
                sys.Tick("surv_y", 24f);
            }

            var stateX = sys.GetState("surv_x");
            Assert.NotNull(stateX);
            Assert.True(stateX.resentmentLevel > 0f, "Resentment should have built up");
            Assert.NotEqual(string.Empty, stateX.resentmentTargetId);

            // Phase 2: switch to equal allocations → resentment should decay
            sys.SetAllocation("surv_x", 0.55f);
            sys.SetAllocation("surv_y", 0.55f);

            for (int day = 0; day < 60; day++)
            {
                sys.Tick("surv_x", 24f);
                sys.Tick("surv_y", 24f);
            }

            var recoveredState = sys.GetState("surv_x");
            Assert.NotNull(recoveredState);
            Assert.Equal(0f, recoveredState.resentmentLevel);
            Assert.Equal(string.Empty, recoveredState.resentmentTargetId);
        }

        [Fact]
        public void IdeologicalFrictionSystem_ConflictingBeliefs_PenaltyRemoved_WhenBeliefCleared()
        {
            var sys = new IdeologicalFrictionSystem();

            // Phase 1: conflicting beliefs → penalty
            sys.RegisterBelief("surv_m", "military_discipline");
            sys.RegisterBelief("surv_n", "pacifist");

            float conflictMult = sys.GetRoommateCompatibilityMultiplier("surv_m", "surv_n");
            Assert.True(conflictMult < 1f,
                "Conflicting beliefs should produce multiplier < 1");

            // Phase 2: clear one belief → penalty gone
            sys.RegisterBelief("surv_n", "");

            float clearedMult = sys.GetRoommateCompatibilityMultiplier("surv_m", "surv_n");
            Assert.Equal(1f, clearedMult);
        }

        [Fact]
        public void ShelterDecorSystem_RemoveItem_RoomReturnsToZero()
        {
            var sys = new ShelterDecorSystem();
            sys.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_cracked_vase", LocalizedMoraleDelta = 1.5f, Category = "broken"
            });

            sys.Assign("room_b", "slot_1", "item_cracked_vase", 1);
            Assert.True(sys.GetRoomMoraleDelta("room_b") > 0f,
                "Room should have positive morale with item assigned");

            bool removed = sys.Remove("room_b", "slot_1");
            Assert.True(removed, "Remove should succeed for an existing placement");
            Assert.Equal(0f, sys.GetRoomMoraleDelta("room_b"));
        }

        // ──────────────────────────────────────────────────────────────
        // 12E.5 — Save/load stability
        // ──────────────────────────────────────────────────────────────

        [Fact]
        public void CohortSystem_SaveLoad_ProducesSameStateAsUninterrupted()
        {
            // Reference run: no save/load (use age gap matching Plan12A pattern)
            var refSys = new CohortSystem();
            var parents = new List<string> { "p1", "p2" };
            for (int i = 0; i < 5; i++)
                refSys.BookChild($"kid_{i}", parents, "medium", 280);

            for (int day = 374; day <= 424; day++)
                for (int i = 0; i < 5; i++)
                    refSys.TryMaturation($"kid_{i}", day);

            // Test run: book → tick 25 days → save → restore → tick remaining → compare
            var testSys = new CohortSystem();
            for (int i = 0; i < 5; i++)
                testSys.BookChild($"kid_{i}", parents, "medium", 280);

            for (int day = 374; day <= 398; day++)
                for (int i = 0; i < 5; i++)
                    testSys.TryMaturation($"kid_{i}", day);

            var saved = testSys.CaptureState();

            var restoredSys = new CohortSystem();
            restoredSys.RestoreState(saved);

            for (int day = 399; day <= 424; day++)
                for (int i = 0; i < 5; i++)
                    restoredSys.TryMaturation($"kid_{i}", day);

            // Compare final state of every child
            for (int i = 0; i < 5; i++)
            {
                var refChild = refSys.GetChild($"kid_{i}");
                var testChild = restoredSys.GetChild($"kid_{i}");
                Assert.NotNull(refChild);
                Assert.NotNull(testChild);
                Assert.Equal(refChild.isMatured, testChild.isMatured);
                Assert.Equal(refChild.maturationDay, testChild.maturationDay);
                Assert.Equal(refChild.guessBand, testChild.guessBand);
                Assert.Equal(refChild.birthDay, testChild.birthDay);
            }
        }

        [Fact]
        public void ShelterDecorSystem_SaveLoad_MoraleDeltaIdentical()
        {
            // Reference run
            var refSys = new ShelterDecorSystem();
            refSys.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_lantern", LocalizedMoraleDelta = 2.0f, Category = "light"
            });
            refSys.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_sketch", LocalizedMoraleDelta = 1.5f, Category = "art"
            });

            for (int i = 0; i < 6; i++)
            {
                string itemId = i % 2 == 0 ? "item_lantern" : "item_sketch";
                refSys.Assign("room_save", $"slot_{i}", itemId, i + 1);
            }

            float refDelta = refSys.GetRoomMoraleDelta("room_save");

            // Test run: assign → save → restore → check
            var testSys = new ShelterDecorSystem();
            testSys.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_lantern", LocalizedMoraleDelta = 2.0f, Category = "light"
            });
            testSys.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_sketch", LocalizedMoraleDelta = 1.5f, Category = "art"
            });

            for (int i = 0; i < 6; i++)
            {
                string itemId = i % 2 == 0 ? "item_lantern" : "item_sketch";
                testSys.Assign("room_save", $"slot_{i}", itemId, i + 1);
            }

            var saved = testSys.CaptureState();

            var restoredSys = new ShelterDecorSystem();
            restoredSys.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_lantern", LocalizedMoraleDelta = 2.0f, Category = "light"
            });
            restoredSys.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_sketch", LocalizedMoraleDelta = 1.5f, Category = "art"
            });
            restoredSys.RestoreState(saved);

            float restoredDelta = restoredSys.GetRoomMoraleDelta("room_save");
            Assert.Equal(refDelta, restoredDelta);
        }

        [Fact]
        public void RationConflictSystem_SaveLoad_SameResentmentAsUninterrupted()
        {
            // Reference run: 20 ticks unequal, no save/load
            var refRng = new SeededRng(12345);
            var refSys = new RationConflictSystem(refRng);
            refSys.RegisterSurvivor("s1");
            refSys.RegisterSurvivor("s2");

            // 10 ticks unequal
            refSys.SetAllocation("s1", 0.3f);
            refSys.SetAllocation("s2", 0.8f);
            for (int t = 0; t < 10; t++)
            {
                refSys.Tick("s1", 24f);
                refSys.Tick("s2", 24f);
            }

            // 10 ticks equal
            refSys.SetAllocation("s1", 0.55f);
            refSys.SetAllocation("s2", 0.55f);
            for (int t = 0; t < 10; t++)
            {
                refSys.Tick("s1", 24f);
                refSys.Tick("s2", 24f);
            }

            float refResentment = refSys.GetState("s1")!.resentmentLevel;

            // Test run: 10 ticks unequal → save → restore → 10 ticks equal
            var testRng = new SeededRng(12345);
            var testSys = new RationConflictSystem(testRng);
            testSys.RegisterSurvivor("s1");
            testSys.RegisterSurvivor("s2");

            testSys.SetAllocation("s1", 0.3f);
            testSys.SetAllocation("s2", 0.8f);
            for (int t = 0; t < 10; t++)
            {
                testSys.Tick("s1", 24f);
                testSys.Tick("s2", 24f);
            }

            var saved = testSys.CaptureState();

            var restoredSys = new RationConflictSystem(new SeededRng(12345));
            restoredSys.RegisterSurvivor("s1");
            restoredSys.RegisterSurvivor("s2");
            restoredSys.SetAllocation("s1", 0.3f);
            restoredSys.SetAllocation("s2", 0.8f);
            restoredSys.RestoreState(saved);

            // Now switch to equal and tick 10 more
            restoredSys.SetAllocation("s1", 0.55f);
            restoredSys.SetAllocation("s2", 0.55f);
            for (int t = 0; t < 10; t++)
            {
                restoredSys.Tick("s1", 24f);
                restoredSys.Tick("s2", 24f);
            }

            float testResentment = restoredSys.GetState("s1")!.resentmentLevel;
            Assert.Equal(refResentment, testResentment, precision: 5);
        }
    }
}
