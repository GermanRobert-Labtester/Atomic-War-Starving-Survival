// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Vehicles;

namespace Ashfall.Core.Tests.Plan152Vehicles
{
    public class Plan152VehicleCustomizationIntegrationTests
    {
        private static string GetCatalogJson()
        {
            string filename = "vehicle_modules.json";
            string[] candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates)
            {
                if (File.Exists(c)) return File.ReadAllText(c);
            }
            throw new FileNotFoundException($"Could not find {filename} in candidate paths.");
        }

        [Fact]
        public void CatalogLoad_LoadsAllAuthoredVehicleModules()
        {
            string json = GetCatalogJson();
            var catalog = VehicleCustomizationCatalog.LoadFromJson(json);

            Assert.NotNull(catalog);
            Assert.True(catalog.AllModules.Count >= 10, $"Expected >= 10 modules, found {catalog.AllModules.Count}");

            Assert.True(catalog.TryGetModule("reinforced_hull", out var hull));
            Assert.Equal("armor", hull.ModuleType);
            Assert.Equal(20.0f, hull.DefenseBonus);
            Assert.Equal(-0.05f, hull.SpeedModifier);

            Assert.True(catalog.TryGetModule("extended_cargo_bed", out var bed));
            Assert.Equal("cargo", bed.ModuleType);
            Assert.Equal(50.0f, bed.CargoBonus);

            Assert.True(catalog.TryGetModule("bunk_beds_module", out var bunks));
            Assert.Equal("living", bunks.ModuleType);
            Assert.Equal(4, bunks.BunkCapacity);

            Assert.True(catalog.TryGetModule("mounted_gun", out var gun));
            Assert.Equal("weapon", gun.ModuleType);

            Assert.True(catalog.TryGetModule("heavy_winch", out var winch));
            Assert.Equal("utility", winch.ModuleType);
        }

        [Fact]
        public void ModuleInstallation_UpdatesInstalledList_AndEnforcesSlotLimits()
        {
            string json = GetCatalogJson();
            var catalog = VehicleCustomizationCatalog.LoadFromJson(json);
            var system = new VehicleCustomizationSystem(catalog);

            bool installFired = false;
            string lastInstalledVehicle = string.Empty;
            string lastInstalledModule = string.Empty;

            system.OnModuleInstalledSeam += (vId, mId) =>
            {
                installFired = true;
                lastInstalledVehicle = vId;
                lastInstalledModule = mId;
            };

            // Install up to slot limit (max 3 slots for this test)
            Assert.True(system.InstallModule("rig_01", "reinforced_hull", maxSlots: 3));
            Assert.True(installFired);
            Assert.Equal("rig_01", lastInstalledVehicle);
            Assert.Equal("reinforced_hull", lastInstalledModule);

            Assert.True(system.InstallModule("rig_01", "extended_cargo_bed", maxSlots: 3));
            Assert.True(system.InstallModule("rig_01", "bunk_beds_module", maxSlots: 3));

            // 4th module exceeds slot limit of 3
            Assert.False(system.InstallModule("rig_01", "mounted_gun", maxSlots: 3));

            var installed = system.GetInstalledModules("rig_01");
            Assert.Equal(3, installed.Count);

            // Removing module frees slot
            bool removeFired = false;
            system.OnModuleRemovedSeam += (vId, mId) =>
            {
                removeFired = true;
                Assert.Equal("rig_01", vId);
                Assert.Equal("extended_cargo_bed", mId);
            };

            Assert.True(system.RemoveModule("rig_01", "extended_cargo_bed"));
            Assert.True(removeFired);
            Assert.Equal(2, system.GetInstalledModules("rig_01").Count);

            // Now installing mounted_gun succeeds
            Assert.True(system.InstallModule("rig_01", "mounted_gun", maxSlots: 3));
            Assert.Equal(3, system.GetInstalledModules("rig_01").Count);
        }

        [Fact]
        public void EffectiveStatsCalculation_AggregatesBonusesAndPenalties()
        {
            string json = GetCatalogJson();
            var catalog = VehicleCustomizationCatalog.LoadFromJson(json);
            var system = new VehicleCustomizationSystem(catalog);

            system.InstallModule("truck_01", "reinforced_hull"); // def +20, speed -0.05
            system.InstallModule("truck_01", "bulletproof_glass"); // def +15, speed 0.0
            system.InstallModule("truck_01", "extended_cargo_bed"); // cargo +50, speed -0.05

            var stats = system.CalculateEffectiveStats("truck_01", baseSpeedMult: 1.0f, baseCargoCapacity: 80.0f, baseDefense: 0.0f);

            Assert.Equal(0.90f, stats.speedMult, 2);
            Assert.Equal(130.0f, stats.cargoCapacity, 1);
            Assert.Equal(35.0f, stats.defense, 1);
        }

        [Fact]
        public void MobileBaseLivingModules_EnableBaseCampDeployment()
        {
            string json = GetCatalogJson();
            var catalog = VehicleCustomizationCatalog.LoadFromJson(json);
            var system = new VehicleCustomizationSystem(catalog);

            // Without living modules, vehicle is not mobile base capable
            system.InstallModule("convoy_rig", "reinforced_hull");
            Assert.False(system.IsMobileBaseCapable("convoy_rig"));
            Assert.False(system.DeployBaseCamp("convoy_rig", "loc_quarry"));

            // Install living module (Sleeper Bunks)
            system.InstallModule("convoy_rig", "bunk_beds_module");
            Assert.True(system.IsMobileBaseCapable("convoy_rig"));

            bool deployFired = false;
            system.OnBaseCampDeployedSeam += (vId, locId) =>
            {
                deployFired = true;
                Assert.Equal("convoy_rig", vId);
                Assert.Equal("loc_quarry", locId);
            };

            Assert.True(system.DeployBaseCamp("convoy_rig", "loc_quarry"));
            Assert.True(deployFired);
            Assert.True(system.IsBaseCampDeployed("convoy_rig"));
            Assert.Equal("loc_quarry", system.GetBaseCampLocation("convoy_rig"));

            // Packing base camp restores mobile status
            bool packFired = false;
            system.OnBaseCampPackedSeam += (vId) =>
            {
                packFired = true;
                Assert.Equal("convoy_rig", vId);
            };

            Assert.True(system.PackBaseCamp("convoy_rig"));
            Assert.True(packFired);
            Assert.False(system.IsBaseCampDeployed("convoy_rig"));
        }

        [Fact]
        public void RestSurvivorsInVehicle_RestoresFatigueWithinBunkCapacity()
        {
            string json = GetCatalogJson();
            var catalog = VehicleCustomizationCatalog.LoadFromJson(json);
            var system = new VehicleCustomizationSystem(catalog);

            system.InstallModule("sleeper_truck", "bunk_beds_module"); // 4 bunks
            system.InstallModule("sleeper_truck", "mobile_medbay"); // 1 triage bed -> total 5 bunks

            Assert.Equal(5, system.GetBunkCapacity("sleeper_truck"));

            var squad = new List<string> { "surv_01", "surv_02", "surv_03", "surv_04", "surv_05", "surv_06", "surv_07" };
            int restedCount = system.RestSurvivorsInVehicle("sleeper_truck", squad);

            // Out of 7 survivors, only 5 can rest simultaneously in the bunks
            Assert.Equal(5, restedCount);
        }

        [Fact]
        public void Persistence_CaptureAndRestoreRoundtrip()
        {
            string json = GetCatalogJson();
            var catalog = VehicleCustomizationCatalog.LoadFromJson(json);
            var system1 = new VehicleCustomizationSystem(catalog);

            system1.InstallModule("war_rig", "reinforced_hull");
            system1.InstallModule("war_rig", "bunk_beds_module");
            system1.InstallModule("war_rig", "mounted_gun");
            system1.DeployBaseCamp("war_rig", "loc_iron_ridge");

            string saved = system1.CaptureState();
            Assert.Contains("war_rig", saved);
            Assert.Contains("reinforced_hull", saved);
            Assert.Contains("loc_iron_ridge", saved);

            var system2 = new VehicleCustomizationSystem(catalog);
            system2.RestoreState(saved);

            Assert.Equal(3, system2.GetInstalledModules("war_rig").Count);
            Assert.True(system2.IsMobileBaseCapable("war_rig"));
            Assert.True(system2.IsBaseCampDeployed("war_rig"));
            Assert.Equal("loc_iron_ridge", system2.GetBaseCampLocation("war_rig"));
        }
    }
}
