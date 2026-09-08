// SPDX-License-Identifier: MIT
// Plan 70 — Shelter Schedules Expansion: 3 -> 12 Duty Rhythms
// Pinned contract tests for the expanded shelter_schedules.json catalog, schedule uniqueness,
// shift patterns, trigger conditions, emergency override gates, and save round-trip.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests;

public class ShelterSchedulesPlan70CatalogTests : CatalogTestBase
{
    private static ShelterScheduleSystem CreateSystemWithCatalog(out List<ScheduleDefinition> defs)
    {
        var io = new FileSystemIO();
        var json = new SystemTextJsonSerializer();
        defs = ShelterScheduleCatalogLoader.Load(DataDirectory, io, json);

        var state = new PowerGridState
        {
            GenerationWatts = 1000,
            FuelUnits = 100,
            BatteryCapacityWh = 5000,
            BatteryReserveWh = 2500
        };
        var rooms = new List<PowerGridRoom> { new PowerGridRoom("room_main", "Vault Core", 100f) };
        var grid = new PowerGridSystem(state, rooms, new SeededRng(42));
        var system = new ShelterScheduleSystem(grid);
        system.LoadCatalog(defs);
        return system;
    }

    [Fact]
    public void Catalog_LoadsSuccessfully_HasExactCountOf12()
    {
        var io = new FileSystemIO();
        var json = new SystemTextJsonSerializer();
        var defs = ShelterScheduleCatalogLoader.Load(DataDirectory, io, json);

        Assert.NotNull(defs);
        Assert.Equal(12, defs.Count);
    }

    [Fact]
    public void Catalog_JsonDocument_HasSchemaVersionAndCollectionId()
    {
        var path = Path.Combine(DataDirectory, "shelter_schedules.json");
        Assert.True(File.Exists(path), $"Catalog file not found: {path}");
        var text = File.ReadAllText(path);
        using var doc = JsonDocument.Parse(text);
        var root = doc.RootElement;

        Assert.True(root.TryGetProperty("schema_version", out var schemaProp));
        Assert.Equal(1, schemaProp.GetInt32());

        Assert.True(root.TryGetProperty("collection_id", out var collProp));
        Assert.Equal("shelter_schedules", collProp.GetString());

        Assert.True(root.TryGetProperty("schedules", out var schedsProp));
        Assert.Equal(12, schedsProp.GetArrayLength());
    }

    [Fact]
    public void Catalog_PreservesThreeBaselineSchedules()
    {
        var io = new FileSystemIO();
        var json = new SystemTextJsonSerializer();
        var defs = ShelterScheduleCatalogLoader.Load(DataDirectory, io, json);

        var standard = defs.FirstOrDefault(d => d.schedule_id == "schedule_standard");
        Assert.NotNull(standard);
        Assert.Equal("Standard Rotation", standard.display_name);
        Assert.Equal(6.0f, standard.dayStartHour);
        Assert.Equal(22.0f, standard.dayEndHour);
        Assert.Equal(22.0f, standard.curfewStartHour);
        Assert.Equal(6.0f, standard.curfewEndHour);
        Assert.Equal(1.0f, standard.fatigueRecoveryModifier);
        Assert.True(standard.allowEmergencyOverride);

        var night = defs.FirstOrDefault(d => d.schedule_id == "schedule_night_shift");
        Assert.NotNull(night);
        Assert.Equal("Night Rotation", night.display_name);
        Assert.Equal(18.0f, night.dayStartHour);
        Assert.Equal(6.0f, night.dayEndHour);
        Assert.Equal(6.0f, night.curfewStartHour);
        Assert.Equal(18.0f, night.curfewEndHour);
        Assert.Equal(0.9f, night.fatigueRecoveryModifier);
        Assert.True(night.allowEmergencyOverride);

        var curfew = defs.FirstOrDefault(d => d.schedule_id == "schedule_curfew_locked");
        Assert.NotNull(curfew);
        Assert.Equal("Locked-Down Curfew", curfew.display_name);
        Assert.Equal(8.0f, curfew.dayStartHour);
        Assert.Equal(20.0f, curfew.dayEndHour);
        Assert.Equal(20.0f, curfew.curfewStartHour);
        Assert.Equal(8.0f, curfew.curfewEndHour);
        Assert.Equal(1.2f, curfew.fatigueRecoveryModifier);
        Assert.False(curfew.allowEmergencyOverride);
    }

    [Fact]
    public void Catalog_ContainsAllNineNewSchedules()
    {
        var io = new FileSystemIO();
        var json = new SystemTextJsonSerializer();
        var defs = ShelterScheduleCatalogLoader.Load(DataDirectory, io, json);

        var expectedIds = new[]
        {
            "schedule_emergency_shifts",
            "schedule_siege_watch",
            "schedule_winter_hibernation",
            "schedule_mourning",
            "schedule_festival_day",
            "schedule_rationing",
            "schedule_quarantine",
            "schedule_construction_push",
            "schedule_scout_rotation"
        };

        foreach (var id in expectedIds)
        {
            Assert.Contains(defs, d => d.schedule_id == id);
        }
    }

    [Fact]
    public void Catalog_AllSchedules_HaveUniqueValidIdsWithPrefix()
    {
        var io = new FileSystemIO();
        var json = new SystemTextJsonSerializer();
        var defs = ShelterScheduleCatalogLoader.Load(DataDirectory, io, json);

        var seenIds = new HashSet<string>(StringComparer.Ordinal);
        foreach (var def in defs)
        {
            Assert.False(string.IsNullOrWhiteSpace(def.schedule_id));
            Assert.StartsWith("schedule_", def.schedule_id);
            Assert.True(seenIds.Add(def.schedule_id), $"Duplicate schedule_id: {def.schedule_id}");
        }
        Assert.Equal(12, seenIds.Count);
    }

    [Fact]
    public void Catalog_AllSchedules_HaveValidHoursAndModifiers()
    {
        var io = new FileSystemIO();
        var json = new SystemTextJsonSerializer();
        var defs = ShelterScheduleCatalogLoader.Load(DataDirectory, io, json);

        foreach (var def in defs)
        {
            Assert.InRange(def.dayStartHour, 0.0f, 24.0f);
            Assert.InRange(def.dayEndHour, 0.0f, 24.0f);
            Assert.InRange(def.curfewStartHour, 0.0f, 24.0f);
            Assert.InRange(def.curfewEndHour, 0.0f, 24.0f);

            Assert.InRange(def.fatigueRecoveryModifier, 0.5f, 1.5f);
            Assert.InRange(def.lightingDemandDay, 0.1f, 1.0f);
            Assert.InRange(def.lightingDemandNight, 0.1f, 1.0f);
            Assert.InRange(def.lightingDemandCurfew, 0.1f, 1.0f);
        }
    }

    [Fact]
    public void Catalog_AllSchedules_HaveValidShiftPatternsAndTriggerConditions()
    {
        var io = new FileSystemIO();
        var json = new SystemTextJsonSerializer();
        var defs = ShelterScheduleCatalogLoader.Load(DataDirectory, io, json);

        var allowedPatterns = new HashSet<string>(StringComparer.Ordinal)
        {
            "single_shift", "double_shift", "triple_shift", "all_hands", "skeleton_crew"
        };

        foreach (var def in defs)
        {
            Assert.Contains(def.shiftPattern, allowedPatterns);
            Assert.False(string.IsNullOrWhiteSpace(def.triggerCondition), $"Empty triggerCondition for {def.schedule_id}");
            Assert.False(string.IsNullOrWhiteSpace(def.description), $"Empty description for {def.schedule_id}");
            Assert.False(string.IsNullOrWhiteSpace(def.display_name), $"Empty display_name for {def.schedule_id}");
        }
    }

    [Fact]
    public void System_SetSchedule_SwitchesToAll12Schedules()
    {
        var sys = CreateSystemWithCatalog(out var defs);

        foreach (var def in defs)
        {
            var res = sys.SetSchedule(def.schedule_id);
            Assert.True(res.IsSuccess, $"Failed to set schedule: {def.schedule_id}");
            Assert.Equal(def.schedule_id, sys.ActiveScheduleId);

            var active = sys.GetActiveSchedule();
            Assert.NotNull(active);
            Assert.Equal(def.schedule_id, active.schedule_id);
            Assert.Equal(def.display_name, active.display_name);
        }
    }

    [Fact]
    public void System_EmergencyOverride_RespectsScheduleAllowFlag()
    {
        var sys = CreateSystemWithCatalog(out _);

        // Schedules that disallow emergency override
        var disallowed = new[] { "schedule_curfew_locked", "schedule_siege_watch", "schedule_quarantine" };
        foreach (var id in disallowed)
        {
            sys.SetSchedule(id);
            var res = sys.SetEmergencyOverride(true);
            Assert.False(res.IsSuccess, $"Emergency override should be blocked for {id}");
            Assert.False(sys.IsEmergencyOverride);
        }

        // Schedules that allow emergency override
        var allowed = new[] { "schedule_standard", "schedule_emergency_shifts", "schedule_winter_hibernation" };
        foreach (var id in allowed)
        {
            sys.SetSchedule(id);
            var res = sys.SetEmergencyOverride(true);
            Assert.True(res.IsSuccess, $"Emergency override should be allowed for {id}");
            Assert.True(sys.IsEmergencyOverride);
            sys.SetEmergencyOverride(false);
            Assert.False(sys.IsEmergencyOverride);
        }
    }

    [Fact]
    public void System_TryActivateScheduleByTrigger_ActivatesTargetSchedule()
    {
        var sys = CreateSystemWithCatalog(out _);

        // Incident triggers
        Assert.True(sys.TryActivateScheduleByTrigger("incident_shelter_crisis"));
        Assert.Equal("schedule_emergency_shifts", sys.ActiveScheduleId);

        Assert.True(sys.TryActivateScheduleByTrigger("incident_faction_siege"));
        Assert.Equal("schedule_siege_watch", sys.ActiveScheduleId);

        Assert.True(sys.TryActivateScheduleByTrigger("incident_survivor_fatality"));
        Assert.Equal("schedule_mourning", sys.ActiveScheduleId);

        Assert.True(sys.TryActivateScheduleByTrigger("incident_epidemic_outbreak"));
        Assert.Equal("schedule_quarantine", sys.ActiveScheduleId);

        // Seasonal triggers
        Assert.True(sys.TryActivateScheduleByTrigger("season_second_winter"));
        Assert.Equal("schedule_winter_hibernation", sys.ActiveScheduleId);

        Assert.True(sys.TryActivateScheduleByTrigger("seasonal_solstice_commemoration"));
        Assert.Equal("schedule_festival_day", sys.ActiveScheduleId);

        // Room and shortage triggers
        Assert.True(sys.TryActivateScheduleByTrigger("room_workshop"));
        Assert.Equal("schedule_construction_push", sys.ActiveScheduleId);

        Assert.True(sys.TryActivateScheduleByTrigger("shortage_food_reserves"));
        Assert.Equal("schedule_rationing", sys.ActiveScheduleId);

        Assert.True(sys.TryActivateScheduleByTrigger("expedition_active_sorties"));
        Assert.Equal("schedule_scout_rotation", sys.ActiveScheduleId);

        // Unknown trigger fails gracefully
        Assert.False(sys.TryActivateScheduleByTrigger("nonexistent_trigger_xyz"));
    }

    [Fact]
    public void System_SaveAndRestore_PreservesActiveSchedule()
    {
        var sys1 = CreateSystemWithCatalog(out _);
        sys1.SetSchedule("schedule_winter_hibernation");
        sys1.SetCurfew(true);
        sys1.AssignBed("survivor_dweller_01", "bunk_01");

        var state = sys1.CaptureState();
        Assert.Equal("schedule_winter_hibernation", state.activeScheduleId);

        var sys2 = CreateSystemWithCatalog(out _);
        sys2.RestoreState(state);

        Assert.Equal("schedule_winter_hibernation", sys2.ActiveScheduleId);
        Assert.True(sys2.State.curfewActive);
        Assert.Single(sys2.State.assignments);
        Assert.Equal("bunk_01", sys2.State.assignments[0].bedId);

        var activeDef = sys2.GetActiveSchedule();
        Assert.NotNull(activeDef);
        Assert.Equal("Winter Hibernation", activeDef.display_name);
    }

    [Fact]
    public void System_TickDay_AppliesActiveScheduleModifiers()
    {
        var sys = CreateSystemWithCatalog(out _);
        sys.SetSchedule("schedule_winter_hibernation");

        // Day phase, curfew inactive
        sys.TickDay(1);
        Assert.Equal(1.3f, sys.FatigueRecoveryModifier);
        Assert.Equal(0.35f, sys.LightingDemand);

        // Curfew active
        sys.SetCurfew(true);
        sys.TickDay(2);
        Assert.Equal(1.3f, sys.FatigueRecoveryModifier);
        Assert.Equal(0.2f, sys.LightingDemand);
    }
}
