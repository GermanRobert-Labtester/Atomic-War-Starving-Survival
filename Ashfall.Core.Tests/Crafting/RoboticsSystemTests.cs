// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Xunit;

namespace Ashfall.Core.Tests.Robotics
{
    public class RoboticsSystemTests
    {
        private static string GetCatalogJson()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Assets/StreamingAssets/Data/robotics.json");
            if (!File.Exists(path))
            {
                path = Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data/robotics.json");
            }
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }

            return @"{
  ""schema_version"": 1,
  ""robots"": [
    {
      ""id"": ""robot_security_sentry_v1"",
      ""display_name"": ""Aegis-IV Autonomous Sentry"",
      ""role"": ""security"",
      ""armor_rating"": 0.45,
      ""max_chassis_integrity"": 1000,
      ""max_core_charge_wh"": 6000,
      ""labor_drain_w"": 350,
      ""charging_rate_w"": 750,
      ""emp_disable_hours"": 24,
      ""reactivation_materials"": [
        { ""item_id"": ""scrap_electronic"", ""quantity"": 8 }
      ],
      ""compatible_tasks"": [ ""guard"", ""patrol"" ],
      ""description"": ""Test sentry""
    },
    {
      ""id"": ""robot_heavy_loader_mark4"",
      ""display_name"": ""Titan-VII Hydraulic Cargo Loader"",
      ""role"": ""hauling"",
      ""armor_rating"": 0.30,
      ""max_chassis_integrity"": 1000,
      ""max_core_charge_wh"": 8000,
      ""labor_drain_w"": 500,
      ""charging_rate_w"": 1000,
      ""emp_disable_hours"": 18,
      ""reactivation_materials"": [
        { ""item_id"": ""scrap_metal"", ""quantity"": 10 }
      ],
      ""compatible_tasks"": [ ""haul"" ],
      ""description"": ""Test loader""
    }
  ]
}";
        }

        [Fact]
        public void ReactivateRobot_ValidArchetype_CreatesUnitWithSkillScaledLogic()
        {
            var sys = new RoboticsSystem(new SeededRng(400));
            sys.LoadCatalog(GetCatalogJson());

            // High programmer skill (0.90) -> logic = 600 + 0.9*400 = 960
            var unit = sys.ReactivateRobot("robot_security_sentry_v1", 0.90f, out string error);
            Assert.NotNull(unit);
            Assert.True(string.IsNullOrEmpty(error));
            Assert.Equal("robot_security_sentry_v1", unit.DefinitionId);
            Assert.Equal(1000, unit.ChassisIntegrity);
            Assert.Equal(960, unit.LogicIntegrity);
            Assert.Equal(3000, unit.CoreChargeWh); // 50% of 6000
            Assert.Single(sys.Units);
        }

        [Fact]
        public void ReactivateRobot_InvalidArchetype_Fails()
        {
            var sys = new RoboticsSystem(new SeededRng(401));
            sys.LoadCatalog(GetCatalogJson());

            var unit = sys.ReactivateRobot("robot_unknown_void", 0.5f, out string error);
            Assert.Null(unit);
            Assert.Contains("Unknown robot archetype", error);
        }

        [Fact]
        public void ProgramDirective_ValidDirective_UpdatesDirective()
        {
            var sys = new RoboticsSystem(new SeededRng(402));
            sys.LoadCatalog(GetCatalogJson());

            var unit = sys.ReactivateRobot("robot_security_sentry_v1", 0.8f, out _);
            Assert.NotNull(unit);

            bool success = sys.ProgramDirective(unit.UnitId, "directive_guard_vault", 0.8f, out string error);
            Assert.True(success);
            Assert.True(string.IsNullOrEmpty(error));
            Assert.Equal("directive_guard_vault", unit.AssignedDirective);
        }

        [Fact]
        public void ProgramDirective_EmpDisabledUnit_RejectsDirective()
        {
            var sys = new RoboticsSystem(new SeededRng(403));
            sys.LoadCatalog(GetCatalogJson());

            var unit = sys.ReactivateRobot("robot_heavy_loader_mark4", 0.8f, out _);
            Assert.NotNull(unit);

            sys.ApplyEmpShock(12);
            Assert.True(unit.IsEmpDisabled);

            bool success = sys.ProgramDirective(unit.UnitId, "directive_haul", 0.8f, out string error);
            Assert.False(success);
            Assert.Contains("EMP-disabled", error);
        }

        [Fact]
        public void ApplyEmpShock_DisablesAllUnitsForDuration()
        {
            var sys = new RoboticsSystem(new SeededRng(404));
            sys.LoadCatalog(GetCatalogJson());

            var u1 = sys.ReactivateRobot("robot_security_sentry_v1", 0.8f, out _);
            var u2 = sys.ReactivateRobot("robot_heavy_loader_mark4", 0.8f, out _);

            sys.ApplyEmpShock(24);

            Assert.True(u1!.IsEmpDisabled);
            Assert.True(u2!.IsEmpDisabled);
            Assert.Equal(24, u1.EmpDisableHoursRemaining);
            Assert.Equal(24, u2.EmpDisableHoursRemaining);
        }

        [Fact]
        public void TickLabor_EmpDisabled_RecoversHoursRemaining()
        {
            var sys = new RoboticsSystem(new SeededRng(405));
            sys.LoadCatalog(GetCatalogJson());

            var unit = sys.ReactivateRobot("robot_security_sentry_v1", 0.8f, out _);
            sys.ApplyEmpShock(12);

            sys.TickLabor(8, false, 0f);
            Assert.True(unit!.IsEmpDisabled);
            Assert.Equal(4, unit.EmpDisableHoursRemaining);

            sys.TickLabor(4, false, 0f);
            Assert.False(unit.IsEmpDisabled);
            Assert.Equal(0, unit.EmpDisableHoursRemaining);
        }

        [Fact]
        public void TickLabor_DockedWithPower_RechargesBattery()
        {
            var sys = new RoboticsSystem(new SeededRng(406));
            sys.LoadCatalog(GetCatalogJson());

            var unit = sys.ReactivateRobot("robot_security_sentry_v1", 0.8f, out _);
            unit!.CoreChargeWh = 1000;

            // 750W charging rate, 4 hours -> +3000Wh
            sys.TickLabor(4, isDockedToGrid: true, gridPowerAvailableWatts: 1000f);

            Assert.Equal(4000, unit.CoreChargeWh);
        }

        [Fact]
        public void TickLabor_AssignedToLabor_DrainsPowerAndAppliesChassisWear()
        {
            var sys = new RoboticsSystem(new SeededRng(407));
            sys.LoadCatalog(GetCatalogJson());

            var unit = sys.ReactivateRobot("robot_heavy_loader_mark4", 0.8f, out _);
            unit!.AssignedDirective = "directive_haul";
            unit.CoreChargeWh = 4000;
            unit.ChassisIntegrity = 1000;

            // 500W labor drain, 4 hours -> -2000Wh
            sys.TickLabor(4, isDockedToGrid: false, gridPowerAvailableWatts: 0f);

            Assert.Equal(2000, unit.CoreChargeWh);
            Assert.True(unit.ChassisIntegrity < 1000);
        }

        [Fact]
        public void RepairRobot_RestoresChassisIntegrity()
        {
            var sys = new RoboticsSystem(new SeededRng(408));
            sys.LoadCatalog(GetCatalogJson());

            var unit = sys.ReactivateRobot("robot_heavy_loader_mark4", 0.8f, out _);
            unit!.ChassisIntegrity = 800;

            bool repaired = sys.RepairRobot(unit.UnitId, 150);
            Assert.True(repaired);
            Assert.Equal(950, unit.ChassisIntegrity);
        }

        [Fact]
        public void SaveRestore_PreservesUnitsDirectivesAndCharges()
        {
            var sys1 = new RoboticsSystem(new SeededRng(409));
            sys1.LoadCatalog(GetCatalogJson());

            var u = sys1.ReactivateRobot("robot_security_sentry_v1", 0.75f, out _);
            u!.AssignedDirective = "directive_patrol";
            u.CoreChargeWh = 4500;

            var saved = sys1.CaptureState();

            var sys2 = new RoboticsSystem(new SeededRng(410));
            sys2.RestoreState(saved);

            Assert.Single(sys2.Units);
            Assert.Equal(u.UnitId, sys2.Units[0].UnitId);
            Assert.Equal("directive_patrol", sys2.Units[0].AssignedDirective);
            Assert.Equal(4500, sys2.Units[0].CoreChargeWh);
            Assert.Equal(1, sys2.State.TotalUnitsReactivated);
        }

        [Fact]
        public void DeterministicReplay_SameSeedProducesIdenticalLaborOutputs()
        {
            var sysA = new RoboticsSystem(new SeededRng(888));
            var sysB = new RoboticsSystem(new SeededRng(888));
            sysA.LoadCatalog(GetCatalogJson());
            sysB.LoadCatalog(GetCatalogJson());

            var uA = sysA.ReactivateRobot("robot_heavy_loader_mark4", 0.5f, out _);
            var uB = sysB.ReactivateRobot("robot_heavy_loader_mark4", 0.5f, out _);

            uA!.AssignedDirective = "directive_haul";
            uB!.AssignedDirective = "directive_haul";

            for (int i = 0; i < 5; i++)
            {
                sysA.TickLabor(8, false, 0f);
                sysB.TickLabor(8, false, 0f);
            }

            Assert.Equal(uA.CoreChargeWh, uB.CoreChargeWh);
            Assert.Equal(uA.ChassisIntegrity, uB.ChassisIntegrity);
            Assert.Equal(uA.LogicIntegrity, uB.LogicIntegrity);
        }
    }
}
