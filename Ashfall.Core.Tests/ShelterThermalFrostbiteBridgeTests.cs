using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ShelterThermalFrostbiteBridgeTests
    {
        private static List<ShelterRoom> DefaultRooms() => new List<ShelterRoom>
        {
            new ShelterRoom("room_a", "Test Room A", 4),
            new ShelterRoom("room_b", "Test Room B", 4)
        };

        private static ShelterAssignmentSystem CreateAssignments()
        {
            return new ShelterAssignmentSystem(new ShelterAssignmentState(), DefaultRooms(), new SeededRng(1));
        }

        private static ShelterThermalSystem CreateColdSystem(ShelterAssignmentSystem assignments, float indoorTemp = -20f)
        {
            var rng = new SeededRng(42);
            var needs = new NeedsSystem();
            var starting = new StartingLevelSystem();
            var deepFreezeState = new YearOfAshDeepFreezeState { indoorTemperatureCelsius = indoorTemp };
            var deepFreeze = new YearOfAshDeepFreezeSystem(deepFreezeState);
            var sys = new ShelterThermalSystem(rng, needs, starting, deepFreeze);
            sys.SetAssignments(assignments);
            sys.AddRoom("room_a", "Test Room A", 50f, 0.3f, true);
            // Ensure boiler off and fuel low so room stays cold
            sys.SetBoilerActive(false);
            // Force room target low and valve open so no heat
            var room = sys.State.rooms.Find(r => r.roomId == "room_a");
            if (room != null)
            {
                room.targetTempC = 5f;
                room.radiatorValveOpen = 0f;
                room.currentTempC = indoorTemp; // start cold
            }
            return sys;
        }

        [Fact]
        public void ColdRoom_WithOccupant_FiresFrostbiteRisk()
        {
            var assignments = CreateAssignments();
            assignments.Assign("survivor_a", "room_a", null, 1);
            var sys = CreateColdSystem(assignments, -20f);
            // Ensure assignment is present
            Assert.Single(assignments.GetAssignmentsForRoom("room_a"));

            int frostbiteCount = 0;
            string lastRoom = null!, lastSurvivor = null!;
            sys.OnFrostbiteRisk += (roomId, survivorId) => { frostbiteCount++; lastRoom = roomId; lastSurvivor = survivorId; };

            sys.TickDay(10);

            Assert.True(frostbiteCount >= 1, "Expected frostbite risk event for occupant in <5°C room");
            Assert.Equal("room_a", lastRoom);
            Assert.Equal("survivor_a", lastSurvivor);
        }

        [Fact]
        public void WarmRoom_WithOccupant_DoesNotFireFrostbiteRisk()
        {
            var assignments = CreateAssignments();
            assignments.Assign("survivor_a", "room_a", null, 1);
            var sys = CreateColdSystem(assignments, 18f); // warm indoor
            // Warm the room manually to above threshold
            var room = sys.State.rooms.Find(r => r.roomId == "room_a");
            room!.currentTempC = 22f;
            // Keep boiler active to stay warm
            sys.SetBoilerActive(true, 70f);

            int frostbiteCount = 0;
            sys.OnFrostbiteRisk += (_, __) => frostbiteCount++;

            sys.TickDay(10);

            Assert.Equal(0, frostbiteCount);
        }

        [Fact]
        public void ColdRoom_WithoutOccupant_DoesNotFireFrostbiteRisk()
        {
            var assignments = CreateAssignments();
            // No assignment
            var sys = CreateColdSystem(assignments, -20f);

            int frostbiteCount = 0;
            sys.OnFrostbiteRisk += (_, __) => frostbiteCount++;

            sys.TickDay(10);

            Assert.Equal(0, frostbiteCount);
        }

        [Fact]
        public void SaveRoundTrip_PreservesThermalStateWithColdRoom()
        {
            var assignments = CreateAssignments();
            assignments.Assign("survivor_a", "room_a", null, 1);
            var sys1 = CreateColdSystem(assignments, -20f);
            sys1.TickDay(5);
            var state = sys1.CaptureState();
            var sys2 = CreateColdSystem(CreateAssignments(), -20f);
            sys2.RestoreState(state);
            Assert.Single(sys2.State.rooms);
            Assert.Equal("room_a", sys2.State.rooms[0].roomId);
            Assert.Equal(sys1.State.rooms[0].currentTempC, sys2.State.rooms[0].currentTempC, 2);
        }
    }
}
