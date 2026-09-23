// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 189: Water Source Management & Contamination Network — Integration Tests
// Verifies water source catalog loading, source discovery, active source switching,
// daily environmental contamination propagation across network connections,
// water testing accuracy/type identification, maintenance/infrastructure upgrades,
// and save/restore state persistence.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Water;

namespace Ashfall.Core.Tests.Water
{
    public sealed class Plan189WaterSourceIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void LoadCatalog_LoadsWaterSourcesAndConnections()
        {
            var system = new WaterSourceSystem();
            string path = Path.Combine(DataDirectory, "water_sources.json");
            Assert.True(File.Exists(path), $"water_sources.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            var sources = system.GetAllSourceDefs();
            Assert.Equal(8, sources.Count);

            var connections = system.GetAllConnections();
            Assert.Equal(3, connections.Count);

            var deepWell = system.GetSourceDef("source_shelter_well_deep");
            Assert.NotNull(deepWell);
            Assert.Equal("well", deepWell.source_type);
            Assert.Equal(450.0f, deepWell.flow_rate_l_day);
            Assert.Equal(0.04f, deepWell.base_contamination);

            // Default state has shelter well discovered and active
            var active = system.GetActiveSource();
            Assert.NotNull(active);
            Assert.Equal("source_shelter_well_deep", active.SourceId);
            Assert.True(active.IsDiscovered);
            Assert.True(active.IsActive);
        }

        [Fact]
        public void DiscoverAndActivateSource_SwitchesIntake()
        {
            var system = new WaterSourceSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "water_sources.json")));

            string? discoveredSource = null;
            string? activatedSource = null;
            system.OnSourceDiscovered += s => discoveredSource = s;
            system.OnActiveSourceChanged += s => activatedSource = s;

            bool discovered = system.DiscoverSource("source_limestone_spring", day: 2);
            Assert.True(discovered);
            Assert.Equal("source_limestone_spring", discoveredSource);
            Assert.Equal(2, system.DiscoveredSourceCount);

            bool activated = system.SetActiveSource("source_limestone_spring");
            Assert.True(activated);
            Assert.Equal("source_limestone_spring", activatedSource);
            Assert.Equal("source_limestone_spring", system.ActiveSourceId);

            // Flow rate for limestone spring is 280.0
            Assert.Equal(280.0f, system.CalculateAvailableWater());
        }

        [Fact]
        public void TickDay_PropagatesContaminationAndAppliesWeather()
        {
            var system = new WaterSourceSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "water_sources.json")));

            var river = system.GetSource("source_pine_river_upstream");
            Assert.NotNull(river);
            float initialContamination = river.CurrentContamination;

            // Tick with heavy rain and environmental radiation
            system.TickDay(day: 3, rainMultiplier: 1.5f, environmentalContamination: 0.3f);

            Assert.True(river.CurrentContamination > initialContamination);
        }

        [Fact]
        public void ConductWaterTest_LogsResultAndUpdatesLastTested()
        {
            var system = new WaterSourceSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "water_sources.json")));

            WaterTestResult? testLogged = null;
            system.OnWaterTested += t => testLogged = t;

            var result = system.ConductWaterTest("source_district8_municipal", day: 4, survivorId: "surv_medic", kitAccuracy: 0.95f);

            Assert.NotNull(result);
            Assert.NotNull(testLogged);
            Assert.Equal("source_district8_municipal", result.SourceId);
            Assert.Equal(4, result.Day);
            Assert.Equal("surv_medic", result.TestedBySurvivorId);
            Assert.Equal("chemical", result.ContaminantType);

            var src = system.GetSource("source_district8_municipal");
            Assert.NotNull(src);
            Assert.Equal(4, src.LastTestedDay);
            Assert.Equal(result.ContaminationLevel, src.LastTestedContamination);
            Assert.Single(system.GetTestHistory());
        }

        [Fact]
        public void PerformMaintenanceAndUpgrade_ReducesContamination()
        {
            var system = new WaterSourceSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "water_sources.json")));

            var well = system.GetSource("source_shelter_well_deep");
            Assert.NotNull(well);
            well.CurrentContamination = 0.55f;
            well.MaintenanceNeeded = true;

            bool maintained = system.PerformMaintenance("source_shelter_well_deep", day: 5);
            Assert.True(maintained);
            Assert.False(well.MaintenanceNeeded);
            Assert.True(well.CurrentContamination < 0.55f);

            bool upgraded = system.UpgradeInfrastructure("source_shelter_well_deep", "advanced");
            Assert.True(upgraded);
            Assert.Equal("advanced", well.InfrastructureLevel);
        }

        [Fact]
        public void SaveRestoreState_PreservesSourcesTestsAndActiveSelection()
        {
            var system = new WaterSourceSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "water_sources.json")));

            system.DiscoverSource("source_limestone_spring", day: 2);
            system.SetActiveSource("source_limestone_spring");
            system.ConductWaterTest("source_limestone_spring", day: 3, survivorId: "surv_scout");

            var state = system.CaptureState();

            var restoredSystem = new WaterSourceSystem();
            restoredSystem.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "water_sources.json")));
            restoredSystem.RestoreState(state);

            Assert.Equal("source_limestone_spring", restoredSystem.ActiveSourceId);
            Assert.Equal(2, restoredSystem.DiscoveredSourceCount);

            var spring = restoredSystem.GetSource("source_limestone_spring");
            Assert.NotNull(spring);
            Assert.True(spring.IsActive);
            Assert.True(spring.IsDiscovered);
            Assert.Equal(3, spring.LastTestedDay);

            var history = restoredSystem.GetTestHistory();
            Assert.Single(history);
            Assert.Equal("source_limestone_spring", history[0].SourceId);
        }
    }
}
