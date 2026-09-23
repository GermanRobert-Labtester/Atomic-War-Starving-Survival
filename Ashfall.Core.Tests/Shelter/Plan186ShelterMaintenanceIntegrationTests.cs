// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 186: Shelter Maintenance & Degradation System — Integration Tests
// Verifies shelter components catalog loading, daily wear and tear,
// environmental stress multipliers, warning and failure thresholds,
// maintenance action logging, and save/restore roundtrips.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class Plan186ShelterMaintenanceIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void LoadCatalog_LoadsComponents()
        {
            var system = new ShelterMaintenanceSystem();
            string path = Path.Combine(DataDirectory, "shelter_components.json");
            Assert.True(File.Exists(path), $"shelter_components.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            Assert.Equal(10, system.TrackedComponentCount);
            var defs = system.GetAllDefinitions();
            Assert.Equal(10, defs.Count);

            var airFilter = system.GetDefinition("comp_air_filter_pre");
            Assert.NotNull(airFilter);
            Assert.Equal("Intake Pre-Filter", airFilter.display_name);
            Assert.Equal("Air", airFilter.component_type);
            Assert.Equal(100.0f, airFilter.max_condition);
            Assert.Equal(2.0f, airFilter.base_degradation_rate);

            var state = system.GetComponent("comp_air_filter_pre");
            Assert.NotNull(state);
            Assert.Equal(100.0f, state.Condition);
            Assert.True(state.IsOperational);
            Assert.False(state.HasWarning);
        }

        [Fact]
        public void TickDay_DegradesComponents()
        {
            var system = new ShelterMaintenanceSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "shelter_components.json")));

            string? degradedId = null;
            float degradedCondition = 0f;
            system.OnComponentDegraded += (id, cond) =>
            {
                if (id == "comp_wall_north")
                {
                    degradedId = id;
                    degradedCondition = cond;
                }
            };

            system.TickDay(1, weatherStressMult: 1.0f, radiationStressMult: 1.0f);

            var wall = system.GetComponent("comp_wall_north");
            Assert.NotNull(wall);
            // base rate is 0.5, weather mult is 1.0 -> 99.5
            Assert.Equal(99.5f, wall.Condition);
            Assert.Equal("comp_wall_north", degradedId);
            Assert.Equal(99.5f, degradedCondition);
        }

        [Fact]
        public void WarningAndFailure_TriggerStateAndEvents()
        {
            var system = new ShelterMaintenanceSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "shelter_components.json")));

            string? warningTriggered = null;
            string? failureTriggered = null;
            system.OnComponentWarning += (id, _) => warningTriggered = id;
            system.OnComponentFailed += id => failureTriggered = id;

            var comp = system.GetComponent("comp_air_filter_pre");
            Assert.NotNull(comp);
            // warning threshold is 50.0, failure threshold is 20.0
            comp.Condition = 48.0f;
            system.TickDay(2);

            Assert.True(comp.HasWarning);
            Assert.Equal("comp_air_filter_pre", warningTriggered);
            Assert.Contains(system.GetWarningComponents(), c => c.ComponentId == "comp_air_filter_pre");

            // Now push past failure threshold
            comp.Condition = 19.0f;
            system.TickDay(3);

            Assert.False(comp.IsOperational);
            Assert.Equal("comp_air_filter_pre", failureTriggered);
            Assert.Contains(system.GetFailedComponents(), c => c.ComponentId == "comp_air_filter_pre");
        }

        [Fact]
        public void PerformMaintenance_RestoresConditionAndClearsWarnings()
        {
            var system = new ShelterMaintenanceSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "shelter_components.json")));

            var comp = system.GetComponent("comp_power_generator");
            Assert.NotNull(comp);
            comp.Condition = 10.0f; // in failure and warning
            comp.IsOperational = false;
            comp.HasWarning = true;

            MaintenanceActionRecord? loggedAction = null;
            system.OnMaintenanceCompleted += rec => loggedAction = rec;

            // Skill 50 -> skillEfficiency = 1.0 -> Overhaul restores full condition
            bool success = system.PerformMaintenance("comp_power_generator", "Overhaul", skillLevel: 50f, day: 5);

            Assert.True(success);
            Assert.Equal(100.0f, comp.Condition);
            Assert.True(comp.IsOperational);
            Assert.False(comp.HasWarning);
            Assert.Equal(5, comp.LastMaintainedDay);
            Assert.NotNull(loggedAction);
            Assert.Equal("Overhaul", loggedAction.ActionType);
            Assert.Equal("comp_power_generator", loggedAction.ComponentId);
            Assert.Equal(90.0f, loggedAction.ConditionRestored);
        }

        [Fact]
        public void GetAverageIntegrity_CalculatesShelterCondition()
        {
            var system = new ShelterMaintenanceSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "shelter_components.json")));

            Assert.Equal(100.0f, system.GetAverageIntegrity());

            var comp = system.GetComponent("comp_blast_doors");
            Assert.NotNull(comp);
            comp.Condition = 50.0f;

            // 9 components at 100 + 1 component at 50 = 950 / 10 = 95.0
            Assert.Equal(95.0f, system.GetAverageIntegrity());
        }

        [Fact]
        public void SaveRestoreState_PreservesWearAndMaintenanceLog()
        {
            var system = new ShelterMaintenanceSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "shelter_components.json")));

            var c1 = system.GetComponent("comp_water_recycler");
            Assert.NotNull(c1);
            c1.Condition = 62.5f;
            c1.LastMaintainedDay = 4;
            c1.HasWarning = false;
            c1.IsOperational = true;

            system.PerformMaintenance("comp_water_recycler", "Clean", skillLevel: 50f, day: 4);

            var state = system.CaptureState();

            var restoredSystem = new ShelterMaintenanceSystem();
            restoredSystem.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "shelter_components.json")));
            restoredSystem.RestoreState(state);

            var restoredC1 = restoredSystem.GetComponent("comp_water_recycler");
            Assert.NotNull(restoredC1);
            Assert.Equal(c1.Condition, restoredC1.Condition);
            Assert.Equal(4, restoredC1.LastMaintainedDay);
            Assert.True(restoredC1.IsOperational);
            Assert.False(restoredC1.HasWarning);

            var capturedAction = state.MaintenanceLog.FirstOrDefault();
            Assert.NotNull(capturedAction);
            Assert.Equal("comp_water_recycler", capturedAction.ComponentId);
            Assert.Equal("Clean", capturedAction.ActionType);
        }
    }
}
