// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ShelterAssignmentProximityTests
    {
        private static ShelterAssignmentSystem CreateSystem(out List<ShelterRoom> rooms)
        {
            rooms = new List<ShelterRoom>
            {
                new ShelterRoom("room_bunks", "Bunks", 4),
                new ShelterRoom("room_kitchen", "Kitchen", 2),
                new ShelterRoom("room_clinic", "Clinic", 2),
                new ShelterRoom("room_workshop", "Workshop", 2)
            };
            return new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, new SeededRng(42));
        }

        [Fact]
        public void AreInSameRoom_NullOrEmptyOrSelf_ReturnsFalse()
        {
            var system = CreateSystem(out _);
            system.Assign("survivor_a", "room_bunks");
            system.Assign("survivor_b", "room_bunks");

            Assert.False(system.AreInSameRoom(null, "survivor_b"));
            Assert.False(system.AreInSameRoom("survivor_a", null));
            Assert.False(system.AreInSameRoom("", "survivor_b"));
            Assert.False(system.AreInSameRoom("survivor_a", ""));
            Assert.False(system.AreInSameRoom(null, null));
            Assert.False(system.AreInSameRoom("survivor_a", "survivor_a")); // Self is not a companion
        }

        [Fact]
        public void AreInSameRoom_BothAssignedToSameRoom_ReturnsTrue()
        {
            var system = CreateSystem(out _);
            system.Assign("survivor_a", "room_bunks");
            system.Assign("survivor_b", "room_bunks");

            Assert.True(system.AreInSameRoom("survivor_a", "survivor_b"));
            Assert.True(system.AreInSameRoom("survivor_b", "survivor_a"));
        }

        [Fact]
        public void AreInSameRoom_DifferentRooms_ReturnsFalse()
        {
            var system = CreateSystem(out _);
            system.Assign("survivor_a", "room_bunks");
            system.Assign("survivor_b", "room_kitchen");

            Assert.False(system.AreInSameRoom("survivor_a", "survivor_b"));
            Assert.False(system.AreInSameRoom("survivor_b", "survivor_a"));
        }

        [Fact]
        public void AreInSameRoom_UnassignedSurvivor_ReturnsFalse()
        {
            var system = CreateSystem(out _);
            system.Assign("survivor_a", "room_bunks");
            // survivor_unassigned has no assignment

            Assert.False(system.AreInSameRoom("survivor_a", "survivor_unassigned"));
            Assert.False(system.AreInSameRoom("survivor_unassigned", "survivor_a"));
            Assert.False(system.AreInSameRoom("survivor_unassigned_1", "survivor_unassigned_2"));
        }

        [Fact]
        public void AreInSameRoom_InactiveOrDecommissionedAssignment_ReturnsFalse()
        {
            var state = new ShelterAssignmentState
            {
                Assignments = new List<ShelterAssignment>
                {
                    new ShelterAssignment
                    {
                        SurvivorId = "survivor_a",
                        RoomId = "room_bunks",
                        Status = ShelterAssignmentStatus.Active
                    },
                    new ShelterAssignment
                    {
                        SurvivorId = "survivor_b",
                        RoomId = "room_bunks",
                        Status = ShelterAssignmentStatus.Decommissioned // Inactive
                    }
                }
            };
            var rooms = new List<ShelterRoom>
            {
                new ShelterRoom("room_bunks", "Bunks", 4)
            };
            var system = new ShelterAssignmentSystem(state, rooms, new SeededRng(42));

            Assert.False(system.AreInSameRoom("survivor_a", "survivor_b"));
        }

        [Fact]
        public void Flashback_GroundedWhenCompanionInSameRoom()
        {
            var system = CreateSystem(out _);
            system.Assign("survivor_a", "room_bunks");
            system.Assign("survivor_b", "room_bunks");

            string? groundedBy = null;
            var flashback = new SomaticFlashbackSystem
            {
                Rng = new SeededRng(10),
                GetAliveSurvivorIds = () => new[] { "survivor_a", "survivor_b" },
                IsCompanionInSameRoom = system.AreInSameRoom
            };
            flashback.OnFlashbackGrounded += (sv, orig, reduced) => groundedBy = sv;

            flashback.IncreaseSusceptibility("survivor_a", 1f);
            flashback.OnAudioEvent("siren", 10f);

            Assert.True(flashback.HasActiveFlashback("survivor_a"));
            Assert.Equal("survivor_a", groundedBy);
            Assert.Equal(SomaticFlashbackSystem.GroundedWorkEfficiencyPenalty,
                flashback.GetWorkEfficiencyPenalty("survivor_a"));
        }

        [Fact]
        public void Flashback_UngroundedWhenCompanionsInDifferentRooms()
        {
            var system = CreateSystem(out _);
            system.Assign("survivor_a", "room_bunks");
            system.Assign("survivor_b", "room_kitchen"); // Apart

            bool groundedFired = false;
            var flashback = new SomaticFlashbackSystem
            {
                Rng = new SeededRng(10),
                GetAliveSurvivorIds = () => new[] { "survivor_a", "survivor_b" },
                IsCompanionInSameRoom = system.AreInSameRoom
            };
            flashback.OnFlashbackGrounded += (sv, orig, reduced) => groundedFired = true;

            flashback.IncreaseSusceptibility("survivor_a", 1f);
            flashback.OnAudioEvent("siren", 10f);

            Assert.True(flashback.HasActiveFlashback("survivor_a"));
            Assert.False(groundedFired);
            Assert.Equal(SomaticFlashbackSystem.FlashbackWorkEfficiencyPenalty,
                flashback.GetWorkEfficiencyPenalty("survivor_a"));
        }

        [Fact]
        public void Flashback_UngroundedWhenCompanionsUnassigned()
        {
            var system = CreateSystem(out _);
            system.Assign("survivor_a", "room_bunks");
            // survivor_b unassigned

            var flashback = new SomaticFlashbackSystem
            {
                Rng = new SeededRng(10),
                GetAliveSurvivorIds = () => new[] { "survivor_a", "survivor_b" },
                IsCompanionInSameRoom = system.AreInSameRoom
            };

            flashback.IncreaseSusceptibility("survivor_a", 1f);
            flashback.OnAudioEvent("siren", 10f);

            Assert.Equal(SomaticFlashbackSystem.FlashbackWorkEfficiencyPenalty,
                flashback.GetWorkEfficiencyPenalty("survivor_a"));
        }

        [Fact]
        public void Flashback_UngroundedWhenCompanionIsDead()
        {
            var system = CreateSystem(out _);
            system.Assign("survivor_a", "room_bunks");
            system.Assign("survivor_b", "room_bunks");

            var flashback = new SomaticFlashbackSystem
            {
                Rng = new SeededRng(10),
                // survivor_b is deceased (only survivor_a alive)
                GetAliveSurvivorIds = () => new[] { "survivor_a" },
                IsCompanionInSameRoom = system.AreInSameRoom
            };

            flashback.IncreaseSusceptibility("survivor_a", 1f);
            flashback.OnAudioEvent("siren", 10f);

            // Cannot be grounded by a dead companion
            Assert.Equal(SomaticFlashbackSystem.FlashbackWorkEfficiencyPenalty,
                flashback.GetWorkEfficiencyPenalty("survivor_a"));
        }

        [Fact]
        public void Flashback_Reassignment_UpdatesProximityDynamically()
        {
            var system = CreateSystem(out _);
            system.Assign("survivor_a", "room_bunks");
            system.Assign("survivor_b", "room_kitchen"); // Initially apart

            var flashback = new SomaticFlashbackSystem
            {
                Rng = new SeededRng(10),
                GetAliveSurvivorIds = () => new[] { "survivor_a", "survivor_b" },
                IsCompanionInSameRoom = system.AreInSameRoom
            };

            // 1. Apart: ungrounded
            flashback.IncreaseSusceptibility("survivor_a", 1f);
            flashback.OnAudioEvent("siren", 10f);
            Assert.Equal(SomaticFlashbackSystem.FlashbackWorkEfficiencyPenalty,
                flashback.GetWorkEfficiencyPenalty("survivor_a"));

            // Clear flashback
            flashback.Tick("survivor_a", 24f);

            // 2. Reassign survivor_b to room_bunks (together)
            system.Unassign("survivor_b");
            system.Assign("survivor_b", "room_bunks");

            flashback.IncreaseSusceptibility("survivor_a", 1f);
            flashback.OnAudioEvent("siren", 10f);
            Assert.Equal(SomaticFlashbackSystem.GroundedWorkEfficiencyPenalty,
                flashback.GetWorkEfficiencyPenalty("survivor_a"));

            // Clear flashback
            flashback.Tick("survivor_a", 24f);

            // 3. Unassign survivor_b: apart again
            system.Unassign("survivor_b");
            flashback.IncreaseSusceptibility("survivor_a", 1f);
            flashback.OnAudioEvent("siren", 10f);
            Assert.Equal(SomaticFlashbackSystem.FlashbackWorkEfficiencyPenalty,
                flashback.GetWorkEfficiencyPenalty("survivor_a"));
        }

        [Fact]
        public void Flashback_SaveRestoreRoundtrip_PreservesGroundingBehavior()
        {
            var system = CreateSystem(out var rooms);
            system.Assign("survivor_a", "room_bunks");
            system.Assign("survivor_b", "room_bunks");
            system.Assign("survivor_c", "room_kitchen");

            var capturedState = system.CaptureState();

            // Restore into a fresh system instance
            var restoredSystem = new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, new SeededRng(99));
            restoredSystem.State.RestoreInto(capturedState, rooms);

            var flashback = new SomaticFlashbackSystem
            {
                Rng = new SeededRng(10),
                GetAliveSurvivorIds = () => new[] { "survivor_a", "survivor_b", "survivor_c" },
                IsCompanionInSameRoom = restoredSystem.AreInSameRoom
            };

            Assert.True(restoredSystem.AreInSameRoom("survivor_a", "survivor_b"));
            Assert.False(restoredSystem.AreInSameRoom("survivor_a", "survivor_c"));

            flashback.IncreaseSusceptibility("survivor_a", 1f);
            flashback.OnAudioEvent("siren", 10f);

            Assert.Equal(SomaticFlashbackSystem.GroundedWorkEfficiencyPenalty,
                flashback.GetWorkEfficiencyPenalty("survivor_a"));
        }
    }
}
