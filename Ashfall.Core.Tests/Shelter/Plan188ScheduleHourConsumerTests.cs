// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Plan188Schedule
{
    /// <summary>
    /// DEBT-188-SCHEDULE-HOUR-CONSUMER — the shelter schedule consumes the
    /// campaign hour and derives Day / Night / Curfew from its authored windows.
    /// This makes <see cref="SchedulePhase.Night"/> reachable, with no second
    /// survivor scheduler and no per-survivor block ledger.
    /// </summary>
    public sealed class Plan188ScheduleHourConsumerTests
    {
        private static ShelterScheduleSystem CreateSystem()
        {
            var schedule = new ShelterScheduleSystem(CreateGrid());
            schedule.LoadCatalog(new List<ScheduleDefinition>
            {
                new ScheduleDefinition
                {
                    schedule_id = "default",
                    display_name = "Default",
                    dayStartHour = 6f, dayEndHour = 22f,
                    curfewStartHour = 22f, curfewEndHour = 6f,
                    lightingDemandDay = 0.5f,
                    lightingDemandNight = 0.8f,
                    lightingDemandCurfew = 0.3f
                }
            });
            return schedule;
        }

        private static Ashfall.Core.Shelter.PowerGridSystem CreateGrid()
        {
            var state = new PowerGridState
            {
                GenerationWatts = 800,
                FuelUnits = 100,
                BatteryCapacityWh = 4000,
                BatteryReserveWh = 2000
            };
            var rooms = new List<PowerGridRoom> { new PowerGridRoom("room_main", "Main Vault", 100f) };
            return new Ashfall.Core.Shelter.PowerGridSystem(state, rooms, new SeededRng(42));
        }

        [Theory]
        [InlineData(6, SchedulePhase.Day)]
        [InlineData(12, SchedulePhase.Day)]
        [InlineData(21, SchedulePhase.Day)]
        [InlineData(22, SchedulePhase.Curfew)]
        [InlineData(23, SchedulePhase.Curfew)]
        [InlineData(0, SchedulePhase.Curfew)]
        [InlineData(5, SchedulePhase.Curfew)]
        public void Hour_maps_to_the_authored_window(int hour, SchedulePhase expected)
        {
            Assert.Equal(expected, CreateSystem().PhaseForHour(hour));
        }

        [Fact]
        public void Night_is_reachable_when_the_catalog_leaves_a_gap()
        {
            // Windows: day 8–18, curfew 23–6 → 18,19,20,21,22 are Night.
            var schedule = new ShelterScheduleSystem(CreateGrid());
            schedule.LoadCatalog(new List<ScheduleDefinition>
            {
                new ScheduleDefinition
                {
                    schedule_id = "default",
                    dayStartHour = 8f, dayEndHour = 18f,
                    curfewStartHour = 23f, curfewEndHour = 6f
                }
            });

            Assert.Equal(SchedulePhase.Night, schedule.PhaseForHour(20));
        }

        [Fact]
        public void TickHour_applies_the_phase_and_its_lighting_demand()
        {
            var schedule = CreateSystem();
            schedule.TickHour(20); // within day window
            Assert.Equal(SchedulePhase.Day, schedule.CurrentPhase);
            Assert.Equal(0.5f, schedule.LightingDemand, 3);

            schedule.TickHour(23);
            Assert.Equal(SchedulePhase.Curfew, schedule.CurrentPhase);
            Assert.Equal(0.3f, schedule.LightingDemand, 3);
        }

        [Fact]
        public void Hour_wraps_and_emergency_override_still_wins()
        {
            var schedule = CreateSystem();
            Assert.Equal(SchedulePhase.Curfew, schedule.PhaseForHour(24 + 23));

            schedule.SetEmergencyOverride(true);
            Assert.Equal(SchedulePhase.Emergency, schedule.PhaseForHour(12));
            Assert.Equal(SchedulePhase.Emergency, schedule.PhaseForHour(23));
        }

        [Fact]
        public void Phase_change_event_fires_on_hour_transition()
        {
            var schedule = CreateSystem();
            var seen = new List<SchedulePhase>();
            schedule.OnPhaseChanged += p => seen.Add(p);

            schedule.TickHour(12); // Day
            schedule.TickHour(23); // Curfew
            schedule.TickHour(12); // Day again

            Assert.Contains(SchedulePhase.Day, seen);
            Assert.Contains(SchedulePhase.Curfew, seen);
        }

        [Fact]
        public void Schedules_without_an_hour_read_keep_legacy_day_behaviour()
        {
            // No TickHour call → the hour is unknown and the phase must not become
            // Night from nothing (old saves and headless fixtures stay valid).
            var schedule = CreateSystem();
            schedule.TickDay(1);
            Assert.NotEqual(SchedulePhase.Night, schedule.CurrentPhase);
        }
    }
}
