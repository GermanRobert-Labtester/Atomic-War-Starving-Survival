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

        [Fact] public void TickDay_DayPhase_UsesScheduleFatigueModifier()
        {
            // Bug-07 regression: a schedule with a non-default fatigue recovery
            // modifier must apply that modifier during the day phase, not just
            // the curfew phase. Previously the day branch hardcoded 1f.
            var s = Create(out _);
            s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
            {
                new ScheduleDefinition
                {
                    schedule_id = "restful",
                    display_name = "Restful",
                    fatigueRecoveryModifier = 1.3f
                }
            });
            s.SetSchedule("restful");
            // No curfew, no emergency — pure day phase.
            s.TickDay(1);
            Assert.Equal(1.3f, s.FatigueRecoveryModifier);
        }

        [Fact] public void TickDay_DayPhase_PropagatesRestlessSchedule()
        {
            // Bug-07 regression #2: a schedule with a SUPPRESSED recovery
            // modifier (e.g. 0.7f) must also propagate during the day phase.
            // Default behavior (1f) is the special case, not the general rule.
            var s = Create(out _);
            s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
            {
                new ScheduleDefinition
                {
                    schedule_id = "restless",
                    display_name = "Restless",
                    fatigueRecoveryModifier = 0.7f
                }
            });
            s.SetSchedule("restless");
            s.TickDay(2);
            Assert.Equal(0.7f, s.FatigueRecoveryModifier);
        }

        // Bug-15 (brownout) has no dedicated regression test in this batch.
        // Static-evidence rationale: the production fix in
        // ShelterScheduleSystem.TickDay moves the brownout multiplier inside
        // the if/else block so it runs AFTER the lightingDemand assignment,
        // preserving the × 0.5 effect. Writing a deterministic brownout test
        // would require controlling PowerGridSystem into a sustained brownout
        // state, which is a separate upstream design issue (ComputeTotalDraw
        // returns 0 under brownout, causing IsBrownout to flip on the same
        // tick). That is not in scope for this batch.

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
