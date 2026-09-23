// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class Plan70_77ScheduleSeasonIntegrationTests
    {
        private static string FindDataDir()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return string.Empty;
        }

        private static ShelterScheduleSystem CreateScheduleSystem(out List<ScheduleDefinition> defs)
        {
            string dataDir = FindDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            defs = ShelterScheduleCatalogLoader.Load(dataDir, io, json);

            var powerState = new PowerGridState
            {
                GenerationWatts = 2000,
                FuelUnits = 50,
                BatteryCapacityWh = 10000,
                BatteryReserveWh = 8000
            };
            var rooms = new List<PowerGridRoom>
            {
                new PowerGridRoom("room_central", "Central Vault", 150f),
                new PowerGridRoom("room_barracks", "Living Barracks", 100f)
            };
            var grid = new PowerGridSystem(powerState, rooms, new SeededRng(12345));
            var scheduleSystem = new ShelterScheduleSystem(grid);
            scheduleSystem.LoadCatalog(defs);
            return scheduleSystem;
        }

        private static DutyRosterCatalog CreateDutyRosterCatalog()
        {
            string dataDir = FindDataDir();
            var loader = new DutyRosterCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            return loader.Load(dataDir);
        }

        [Fact]
        public void Plan70_ShelterSchedulesExpansion_EndToEnd()
        {
            // 1. Verify 12 authored duty rhythms
            var scheduleSystem = CreateScheduleSystem(out var defs);
            Assert.Equal(12, defs.Count);

            // 2. Trigger activation for incident, season, and shortage rhythms
            Assert.True(scheduleSystem.TryActivateScheduleByTrigger("incident_faction_siege"));
            Assert.Equal("schedule_siege_watch", scheduleSystem.ActiveScheduleId);

            Assert.True(scheduleSystem.TryActivateScheduleByTrigger("season_second_winter"));
            Assert.Equal("schedule_winter_hibernation", scheduleSystem.ActiveScheduleId);

            Assert.True(scheduleSystem.TryActivateScheduleByTrigger("shortage_food_reserves"));
            Assert.Equal("schedule_rationing", scheduleSystem.ActiveScheduleId);

            Assert.True(scheduleSystem.TryActivateScheduleByTrigger("incident_epidemic_outbreak"));
            Assert.Equal("schedule_quarantine", scheduleSystem.ActiveScheduleId);

            // 3. Operational parameter verification
            scheduleSystem.SetSchedule("schedule_winter_hibernation");
            var winterDef = scheduleSystem.GetActiveSchedule();
            Assert.NotNull(winterDef);
            Assert.Equal("Winter Hibernation", winterDef.display_name);
            Assert.Equal(1.3f, winterDef.fatigueRecoveryModifier);
            Assert.Equal(0.35f, winterDef.lightingDemandDay);
            Assert.Equal(18f, winterDef.curfewStartHour);
            Assert.Equal(8f, winterDef.curfewEndHour);

            // 4. Save/Restore roundtrip
            scheduleSystem.SetCurfew(true);
            scheduleSystem.AssignBed("survivor_dweller_alpha", "bunk_top_01");

            var state = scheduleSystem.CaptureState();
            var restoredSystem = CreateScheduleSystem(out _);
            restoredSystem.RestoreState(state);

            Assert.Equal("schedule_winter_hibernation", restoredSystem.ActiveScheduleId);
            Assert.True(restoredSystem.State.curfewActive);
            Assert.Single(restoredSystem.State.assignments);
            Assert.Equal("bunk_top_01", restoredSystem.State.assignments[0].bedId);
        }

        [Fact]
        public void Plan77_DutyRosterSeasonsExpansion_EndToEnd()
        {
            // 1. Verify 8 contiguous seasons
            var rosterCatalog = CreateDutyRosterCatalog();
            Assert.Equal(8, rosterCatalog.Seasons.Count);

            // 2. Verify exact day resolution across 365 days
            var s0 = rosterCatalog.GetSeasonForDay(0);
            Assert.NotNull(s0);
            Assert.Equal("season_first_ashfall", s0.id);
            Assert.Equal(1.45f, s0.encounterWeight);

            var s8 = rosterCatalog.GetSeasonForDay(8);
            Assert.NotNull(s8);
            Assert.Equal("season_second_winter", s8.id);
            Assert.Equal(1.6f, s8.encounterWeight);

            var s31 = rosterCatalog.GetSeasonForDay(31);
            Assert.NotNull(s31);
            Assert.Equal("season_spring_thaw", s31.id);
            Assert.Equal(0.75f, s31.encounterWeight); // relief floor

            var s121 = rosterCatalog.GetSeasonForDay(121);
            Assert.NotNull(s121);
            Assert.Equal("season_first_siege", s121.id);
            Assert.Equal(1.75f, s121.encounterWeight); // crisis peak

            var s365 = rosterCatalog.GetSeasonForDay(365);
            Assert.NotNull(s365);
            Assert.Equal("season_long_winter", s365.id);

            // 3. Overflow carry-forward
            var s400 = rosterCatalog.GetSeasonForDay(400);
            Assert.NotNull(s400);
            Assert.Equal("season_long_winter", s400.id);
        }

        [Fact]
        public void Plan70_Plan77_CombinedEcosystem_SeasonalScheduleCoordination()
        {
            // Scenario:
            // 1. Campaign begins on Day 0 (first ashfall). Schedule defaults to standard rotation.
            // 2. On Day 8, the season shifts to second_winter. Shelter adapts to winter hibernation schedule.
            // 3. On Day 121, first_siege begins. Shelter shifts to siege_watch emergency shifts.
            // 4. Persistence roundtrip preserves both the seasonal era and the active duty schedule.

            var scheduleSystem = CreateScheduleSystem(out _);
            var rosterCatalog = CreateDutyRosterCatalog();

            // Day 0: Opening Era
            int currentDay = 0;
            var seasonDay0 = rosterCatalog.GetSeasonForDay(currentDay);
            Assert.Equal("season_first_ashfall", seasonDay0!.id);
            scheduleSystem.SetSchedule("schedule_standard");
            Assert.Equal("schedule_standard", scheduleSystem.ActiveScheduleId);

            // Day 8: Second Winter arrives -> coordinate with schedule_winter_hibernation
            currentDay = 8;
            var seasonDay8 = rosterCatalog.GetSeasonForDay(currentDay);
            Assert.Equal("season_second_winter", seasonDay8!.id);

            // Activate matching winter schedule via seasonal trigger
            bool activatedWinter = scheduleSystem.TryActivateScheduleByTrigger(seasonDay8.id);
            Assert.True(activatedWinter);
            Assert.Equal("schedule_winter_hibernation", scheduleSystem.ActiveScheduleId);

            scheduleSystem.TickDay(currentDay);
            Assert.Equal(1.3f, scheduleSystem.FatigueRecoveryModifier);
            Assert.Equal(0.35f, scheduleSystem.LightingDemand);

            // Day 121: First Siege arrives -> coordinate with schedule_siege_watch
            currentDay = 121;
            var seasonDay121 = rosterCatalog.GetSeasonForDay(currentDay);
            Assert.Equal("season_first_siege", seasonDay121!.id);

            bool activatedSiege = scheduleSystem.TryActivateScheduleByTrigger("incident_faction_siege");
            Assert.True(activatedSiege);
            Assert.Equal("schedule_siege_watch", scheduleSystem.ActiveScheduleId);

            var siegeDef = scheduleSystem.GetActiveSchedule();
            Assert.NotNull(siegeDef);
            Assert.Equal("Siege Watch", siegeDef.display_name);
            Assert.Equal(0.7f, siegeDef.fatigueRecoveryModifier);
            Assert.False(siegeDef.allowEmergencyOverride);

            // Persist coordinated state
            var capture = scheduleSystem.CaptureState();
            var restored = CreateScheduleSystem(out _);
            restored.RestoreState(capture);

            Assert.Equal("schedule_siege_watch", restored.ActiveScheduleId);
            Assert.Equal("season_first_siege", rosterCatalog.GetSeasonForDay(currentDay)!.id);
        }
    }
}
