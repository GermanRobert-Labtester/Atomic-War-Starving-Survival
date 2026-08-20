using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public class ShelterAssignmentSystemTests
    {
        private static ShelterAssignmentSystem MakeGrid(ISeededRng rng = null)
        {
            rng ??= new SeededRng(7);
            var rooms = new List<ShelterRoom>
            {
                new ShelterRoom("room_bunks", "Bunks", 4),
                new ShelterRoom("room_kitchen", "Kitchen", 2, "skill_cooking"),
                new ShelterRoom("room_clinic", "Clinic", 2, "skill_medic"),
                new ShelterRoom("room_workshop", "Workshop", 2, "skill_crafting")
            };
            return new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, rng);
        }

        [Fact]
        public void Assign_AddsAssignment()
        {
            var sys = MakeGrid();
            var result = sys.Assign("elena_vasquez", "room_bunks", day: 5);
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Assignment);
            Assert.Single(sys.GetAssignments());
        }

        [Fact]
        public void Assign_UnknownRoomFails()
        {
            var sys = MakeGrid();
            var result = sys.Assign("elena_vasquez", "room_does_not_exist");
            Assert.False(result.Succeeded);
            Assert.Equal("unknown_room", result.ReasonCode);
        }

        [Fact]
        public void Assign_AlreadyAssignedFails()
        {
            var sys = MakeGrid();
            sys.Assign("elena_vasquez", "room_bunks");
            var result = sys.Assign("elena_vasquez", "room_kitchen");
            Assert.False(result.Succeeded);
            Assert.Equal("already_assigned", result.ReasonCode);
        }

        [Fact]
        public void Assign_RoomFullFails()
        {
            var sys = MakeGrid();
            sys.Assign("s1", "room_bunks");
            sys.Assign("s2", "room_bunks");
            sys.Assign("s3", "room_bunks");
            sys.Assign("s4", "room_bunks");
            var result = sys.Assign("s5", "room_bunks");
            Assert.False(result.Succeeded);
            Assert.Equal("room_full", result.ReasonCode);
        }

        [Fact]
        public void CanAssign_FalseWhenFull()
        {
            var sys = MakeGrid();
            sys.Assign("s1", "room_bunks");
            sys.Assign("s2", "room_bunks");
            Assert.True(sys.CanAssign("s3", "room_bunks"));
            sys.Assign("s3", "room_bunks");
            sys.Assign("s4", "room_bunks");
            Assert.False(sys.CanAssign("s5", "room_bunks"));
        }

        [Fact]
        public void Unassign_RemovesAssignment()
        {
            var sys = MakeGrid();
            sys.Assign("elena_vasquez", "room_bunks");
            var result = sys.Unassign("elena_vasquez", day: 6);
            Assert.True(result.Succeeded);
            Assert.Empty(sys.GetAssignments());
        }

        [Fact]
        public void Unassign_NotAssignedFails()
        {
            var sys = MakeGrid();
            var result = sys.Unassign("ghost");
            Assert.False(result.Succeeded);
            Assert.Equal("not_assigned", result.ReasonCode);
        }

        [Fact]
        public void GetOccupancy_AfterMultipleAssigns()
        {
            var sys = MakeGrid();
            sys.Assign("s1", "room_bunks");
            sys.Assign("s2", "room_bunks");
            Assert.Equal(2, sys.GetRoomOccupancy("room_bunks"));
            Assert.Equal(0, sys.GetRoomOccupancy("room_kitchen"));
        }

        [Fact]
        public void Events_FireOnAssignAndUnassign()
        {
            var sys = MakeGrid();
            var fired = new List<ShelterAssignmentEvent>();
            sys.OnAssignmentChanged += e => fired.Add(e);
            sys.Assign("elena_vasquez", "room_bunks", day: 3);
            sys.Unassign("elena_vasquez", day: 4);
            Assert.Equal(2, fired.Count);
            Assert.Equal(ShelterAssignmentEventKind.Assigned, fired[0].Kind);
            Assert.Equal(ShelterAssignmentEventKind.Unassigned, fired[1].Kind);
        }

        [Fact]
        public void CaptureRestore_RoundTrip()
        {
            var sys = MakeGrid();
            sys.Assign("elena_vasquez", "room_bunks");
            sys.Assign("marcus_olejnik", "room_clinic");
            var save = sys.CaptureState();
            var fresh = MakeGrid();
            fresh.RestoreState(save);
            Assert.Equal(2, fresh.GetAssignments().Count);
            Assert.NotNull(fresh.GetAssignmentForSurvivor("elena_vasquez"));
            Assert.NotNull(fresh.GetAssignmentForSurvivor("marcus_olejnik"));
        }

        [Fact]
        public void Save_RoundTrip_ChecksumStable()
        {
            var sys = MakeGrid();
            sys.Assign("elena_vasquez", "room_bunks");
            var save = new ShelterAssignmentSave
            {
                simDay = 1,
                Rooms = new List<ShelterRoomSave>
                {
                    new ShelterRoomSave { RoomId = "room_bunks", DisplayName = "Bunks", Capacity = 4 }
                },
                State = sys.CaptureState()
            };
            var json = new SystemTextJsonSerializer();
            string text = ShelterAssignmentSaveCodec.EncodeToString(save, json);
            var loaded = ShelterAssignmentSaveCodec.Decode(text, json);
            Assert.Equal(save.Checksum, loaded.Checksum);
            Assert.Single(loaded.State.Assignments);
        }

        [Fact]
        public void Save_TamperedChecksumRejected()
        {
            var json = new SystemTextJsonSerializer();
            var save = new ShelterAssignmentSave
            {
                simDay = 1,
                Rooms = new List<ShelterRoomSave>
                {
                    new ShelterRoomSave { RoomId = "x", DisplayName = "y", Capacity = 1 }
                },
                State = new ShelterAssignmentState()
            };
            string text = ShelterAssignmentSaveCodec.EncodeToString(save, json);
            int idx = text.IndexOf("simDay", StringComparison.Ordinal);
            char[] arr = text.ToCharArray();
            arr[idx + 8] = arr[idx + 8] == '1' ? '9' : '1';
            string tampered = new string(arr);
            Assert.Throws<InvalidOperationException>(() => ShelterAssignmentSaveCodec.Decode(tampered, json));
        }

        [Fact]
        public void Save_EmptyChecksumRejected()
        {
            var json = new SystemTextJsonSerializer();
            var save = new ShelterAssignmentSave { simDay = 1, Checksum = string.Empty };
            string text = json.Serialize(save);
            Assert.Throws<InvalidOperationException>(() => ShelterAssignmentSaveCodec.Decode(text, json));
        }

        [Fact]
        public void DeDuplicates_SurvivorAssignments_OnNormalize()
        {
            var state = new ShelterAssignmentState
            {
                Assignments = new List<ShelterAssignment>
                {
                    new ShelterAssignment { SurvivorId = "elena_vasquez", RoomId = "room_bunks" },
                    new ShelterAssignment { SurvivorId = "elena_vasquez", RoomId = "room_kitchen" }
                }
            };
            var sys = new ShelterAssignmentSystem(state, new List<ShelterRoom>
            {
                new ShelterRoom("room_bunks", "Bunks", 4),
                new ShelterRoom("room_kitchen", "Kitchen", 2)
            }, new SeededRng(1));
            // Last-write-wins: the kitchen assignment should survive.
            Assert.Single(sys.GetAssignments());
            Assert.Equal("room_kitchen", sys.GetAssignmentForSurvivor("elena_vasquez")?.RoomId);
        }
    }
}
