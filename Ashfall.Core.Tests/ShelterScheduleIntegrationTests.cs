using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ShelterScheduleIntegrationTests
    {
        private static ShelterScheduleSystem CreateSystem()
        {
            var state = new PowerGridState { GenerationWatts = 800, FuelUnits = 100, BatteryCapacityWh = 4000, BatteryReserveWh = 2000 };
            var rooms = new List<PowerGridRoom> { new PowerGridRoom("room_a", "Test Room", 100f) };
            var power = new PowerGridSystem(state, rooms, new SeededRng(42));
            var sched = new ShelterScheduleSystem(power);
            sched.LoadCatalog(new List<ScheduleDefinition>
            {
                new ScheduleDefinition { schedule_id = "default", display_name = "Default", allowEmergencyOverride = true }
            });
            return sched;
        }

        [Fact]
        public void CurfewAndEmergency_TogglesState()
        {
            var sys = CreateSystem();
            var c = sys.SetCurfew(true);
            Assert.True(c.IsSuccess);
            Assert.True(sys.IsCurfewActive);

            var em = sys.SetEmergencyOverride(true);
            Assert.True(em.IsSuccess);
            Assert.False(sys.IsCurfewActive); // Emergency overrides curfew
        }

        [Fact]
        public void AssignBed_TracksAssignment()
        {
            var sys = CreateSystem();
            var assign = sys.AssignBed("survivor_dweller", "bunk_01");
            Assert.True(assign.IsSuccess);
            Assert.Single(sys.State.assignments);
            Assert.Equal("survivor_dweller", sys.State.assignments[0].survivorId);
            Assert.Equal("bunk_01", sys.State.assignments[0].bedId);
        }

        [Fact]
        public void SaveAndRestore_PreservesSchedule()
        {
            var sys1 = CreateSystem();
            sys1.SetCurfew(true);
            sys1.AssignBed("dweller_1", "bunk_a");

            var state = sys1.CaptureState();
            var sys2 = CreateSystem();
            sys2.RestoreState(state);

            Assert.True(sys2.State.curfewActive);
            Assert.Single(sys2.State.assignments);
            Assert.Equal("bunk_a", sys2.State.assignments[0].bedId);
        }
    }
}
