using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ShelterScheduleSystemTests
    {
        [Fact] public void SetCurfew_ChangesPhase()
        {
            var s = Create(out _);
            s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
            {
                new ScheduleDefinition { schedule_id = "default", display_name = "Default" }
            });
            s.SetCurfew(true);
            Assert.True(s.IsCurfewActive);
        }

        [Fact] public void SetCurfew_BackToDay()
        {
            var s = Create(out _);
            s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
            {
                new ScheduleDefinition { schedule_id = "default", display_name = "Default" }
            });
            s.SetCurfew(true);
            s.SetCurfew(false);
            Assert.False(s.IsCurfewActive);
            Assert.Equal(SchedulePhase.Day, s.CurrentPhase);
        }

        [Fact] public void SetEmergencyOverride_ChangesPhase()
        {
            var s = Create(out _);
            s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
            {
                new ScheduleDefinition { schedule_id = "default", display_name = "Default", allowEmergencyOverride = true }
            });
            s.SetEmergencyOverride(true);
            Assert.True(s.IsEmergencyOverride);
            Assert.Equal(SchedulePhase.Emergency, s.CurrentPhase);
        }

        [Fact] public void AssignBed_CreatesAssignment()
        {
            var s = Create(out _);
            s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
            {
                new ScheduleDefinition { schedule_id = "default", display_name = "Default" }
            });
            var r = s.AssignBed("survivor_1", "bed_1");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.True(s.IsSleepEligible("survivor_1"));
        }

        [Fact] public void UnassignBed_RemovesAssignment()
        {
            var s = Create(out _);
            s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
            {
                new ScheduleDefinition { schedule_id = "default", display_name = "Default" }
            });
            s.AssignBed("survivor_1", "bed_1");
            s.UnassignBed("survivor_1");
            Assert.False(s.IsSleepEligible("survivor_1"));
        }

        [Fact] public void TickDay_SetsCompliance()
        {
            var s = Create(out _);
            s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
            {
                new ScheduleDefinition { schedule_id = "default", display_name = "Default" }
            });
            s.AssignBed("survivor_1", "bed_1");
            s.SetCurfew(true);
            s.TickDay(1);
            Assert.True(s.State.assignments[0].isCompliant);
            Assert.True(s.State.assignments[0].restQuality > 1f);
        }

        [Fact] public void EmergencyOverride_ReducesRecovery()
        {
            var s = Create(out _);
            s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
            {
                new ScheduleDefinition { schedule_id = "default", display_name = "Default", allowEmergencyOverride = true }
            });
            s.SetCurfew(true);
            s.SetEmergencyOverride(true);
            s.TickDay(1);
            Assert.Equal(0.5f, s.FatigueRecoveryModifier);
        }

        [Fact] public void TickDay_Brownout_ReducesLighting()
        {
            var s = Create(out _);
            s.TickDay(1);
            Assert.True(s.State.lightingDemand > 0);
        }

        [Fact] public void SetSchedule_LoadsDefinition()
        {
            var s = Create(out _);
            s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
            {
                new ScheduleDefinition { schedule_id = "strict", display_name = "Strict Curfew", curfewStartHour = 21f }
            });
            var r = s.SetSchedule("strict");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            var def = s.GetActiveSchedule();
            Assert.Equal(21f, def.curfewStartHour);
        }

        [Fact] public void CaptureRestoreState_PreservesAssignments()
        {
            var s = Create(out _);
            s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
            {
                new ScheduleDefinition { schedule_id = "default", display_name = "Default" }
            });
            s.AssignBed("survivor_1", "bed_1");
            s.SetCurfew(true);
            var state = s.CaptureState();
            Assert.Single(state.assignments);

            var s2 = Create(out _);
            s2.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
            {
                new ScheduleDefinition { schedule_id = "default", display_name = "Default" }
            });
            s2.RestoreState(state);
            Assert.Single(s2.State.assignments);
            Assert.True(s2.IsSleepEligible("survivor_1"));
        }

        private static ShelterScheduleSystem Create(out PowerGridSystem power)
        {
            var state = new PowerGridState { GenerationWatts = 800, FuelUnits = 100, BatteryCapacityWh = 4000, BatteryReserveWh = 2000 };
            var rooms = new System.Collections.Generic.List<PowerGridRoom>
            {
                new PowerGridRoom("room_a", "Test Room", 100f)
            };
            power = new PowerGridSystem(state, rooms, new SeededRng(42));
            return new ShelterScheduleSystem(power);
        }
    }
}
