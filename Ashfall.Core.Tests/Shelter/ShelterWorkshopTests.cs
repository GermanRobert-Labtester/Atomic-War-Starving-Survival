// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public class ShelterWorkshopTests
    {
        private static string GetWorkshopCatalogJson()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Assets/StreamingAssets/Data/workshop_recipes.json");
            if (!File.Exists(path))
            {
                path = Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data/workshop_recipes.json");
            }
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }

            return @"{
  ""schema_version"": 1,
  ""recipes"": [
    {
      ""id"": ""recipe_workshop_reload_9x19"",
      ""display_name"": ""Reload 9x19mm FMJ"",
      ""category"": ""ammunition_reload"",
      ""required_room_ids"": [""room_armory_munitions""],
      ""required_rule_ids"": [""rule_armory_service""],
      ""inputs"": [
        { ""item_id"": ""scrap_metal"", ""amount"": 2 },
        { ""item_id"": ""scrap_chemical"", ""amount"": 1 },
        { ""item_id"": ""spent_casing"", ""amount"": 1 }
      ],
      ""outputs"": [
        { ""item_id"": ""ammo_9x19"", ""amount"": 20 }
      ],
      ""base_labor_ticks"": 60,
      ""base_scrap_waste_permille"": 50,
      ""tool_wear_permille"": 5,
      ""calibration_requirement"": 0.50,
      ""skill_weights"": { ""skill_workshop_sense"": 0.8, ""skill_watchful"": 1.0 },
      ""tags"": [""munitions""]
    },
    {
      ""id"": ""recipe_workshop_weapon_service_clean"",
      ""display_name"": ""Firearm Decoking & Deep Clean"",
      ""category"": ""weapon_service"",
      ""required_room_ids"": [""room_armory_munitions"", ""room_workshop""],
      ""required_rule_ids"": [""rule_armory_service""],
      ""inputs"": [
        { ""item_id"": ""machine_oil"", ""amount"": 1 },
        { ""item_id"": ""cloth"", ""amount"": 1 }
      ],
      ""outputs"": [],
      ""base_labor_ticks"": 45,
      ""base_scrap_waste_permille"": 10,
      ""tool_wear_permille"": 2,
      ""calibration_requirement"": 0.30,
      ""skill_weights"": { ""skill_rough_repairs"": 0.8, ""skill_watchful"": 1.0 },
      ""tags"": [""weapon_service""]
    },
    {
      ""id"": ""recipe_workshop_weapon_service_refurbish"",
      ""display_name"": ""Firearm Precision Refurbishing"",
      ""category"": ""weapon_service"",
      ""required_room_ids"": [""room_workshop_precision"", ""room_armory_munitions""],
      ""required_rule_ids"": [""rule_workshop_precision""],
      ""inputs"": [
        { ""item_id"": ""mechanical_parts"", ""amount"": 1 },
        { ""item_id"": ""machine_oil"", ""amount"": 1 },
        { ""item_id"": ""scrap_metal"", ""amount"": 2 }
      ],
      ""outputs"": [],
      ""base_labor_ticks"": 90,
      ""base_scrap_waste_permille"": 30,
      ""tool_wear_permille"": 8,
      ""calibration_requirement"": 0.65,
      ""skill_weights"": { ""skill_workshop_sense"": 1.0 },
      ""tags"": [""weapon_service""]
    },
    {
      ""id"": ""recipe_workshop_electronics_soldering"",
      ""display_name"": ""Circuit Micro-Soldering & Refit"",
      ""category"": ""electronics_refit"",
      ""required_room_ids"": [""room_workshop_precision""],
      ""required_rule_ids"": [""rule_workshop_precision""],
      ""inputs"": [
        { ""item_id"": ""scrap_electronic"", ""amount"": 3 },
        { ""item_id"": ""soldering_kit"", ""amount"": 1 }
      ],
      ""outputs"": [
        { ""item_id"": ""calibration_kit"", ""amount"": 1 }
      ],
      ""base_labor_ticks"": 130,
      ""base_scrap_waste_permille"": 40,
      ""tool_wear_permille"": 10,
      ""calibration_requirement"": 0.75,
      ""skill_weights"": { ""skill_workshop_sense"": 1.0 },
      ""tags"": [""electronics""]
    },
    {
      ""id"": ""recipe_workshop_tool_overhaul"",
      ""display_name"": ""Lathe & Press Tooling Overhaul"",
      ""category"": ""tool_overhaul"",
      ""required_room_ids"": [""room_workshop_precision"", ""room_workshop_heavy""],
      ""required_rule_ids"": [""rule_workshop_precision""],
      ""inputs"": [
        { ""item_id"": ""scrap_metal"", ""amount"": 6 },
        { ""item_id"": ""mechanical_parts"", ""amount"": 2 },
        { ""item_id"": ""machine_oil"", ""amount"": 1 }
      ],
      ""outputs"": [],
      ""base_labor_ticks"": 150,
      ""base_scrap_waste_permille"": 0,
      ""tool_wear_permille"": 0,
      ""calibration_requirement"": 0.0,
      ""skill_weights"": { ""skill_workshop_sense"": 1.2 },
      ""tags"": [""overhaul""]
    },
    {
      ""id"": ""recipe_workshop_heavy_vehicle_service"",
      ""display_name"": ""Heavy Vehicle Powertrain Rebuild"",
      ""category"": ""heavy_workshop_service"",
      ""required_room_ids"": [""room_workshop_heavy""],
      ""required_rule_ids"": [""rule_workshop_machinist""],
      ""inputs"": [
        { ""item_id"": ""mechanical_parts"", ""amount"": 4 },
        { ""item_id"": ""scrap_metal"", ""amount"": 8 },
        { ""item_id"": ""machine_oil"", ""amount"": 2 }
      ],
      ""outputs"": [],
      ""base_labor_ticks"": 200,
      ""base_scrap_waste_permille"": 20,
      ""tool_wear_permille"": 25,
      ""calibration_requirement"": 0.50,
      ""skill_weights"": { ""skill_rough_repairs"": 1.2 },
      ""tags"": [""heavy_machinery""]
    }
  ]
}";
        }

        private static ShelterWorkshopSystem CreateSystem(
            out Inventory.Inventory inv,
            out EquipmentConditionSystem equip,
            out ExpeditionVehicleSystem vehicles,
            int seed = 42)
        {
            var rng = new SeededRng(seed);
            inv = new Inventory.Inventory { Capacity = 100, MaxWeight = 500f };
            var crafting = new CraftingSystem(inv);
            equip = new EquipmentConditionSystem(rng, inv, crafting);
            vehicles = new ExpeditionVehicleSystem(rng);
            vehicles.LoadCatalog(new VehicleCatalog
            {
                vehicles = new List<VehicleDefinition>
                {
                    new() { vehicle_id = "veh_quad", display_name = "Scout Quad", condition_max = 100f, max_fuel = 50f }
                }
            });

            var system = new ShelterWorkshopSystem(inv, rng, equip, vehicles);
            system.LoadCatalog(GetWorkshopCatalogJson());
            return system;
        }

        [Fact]
        public void RecipeEligibility_FiltersByRoom()
        {
            var system = CreateSystem(out _, out _, out _);
            var armoryRecipes = system.GetAvailableRecipes("room_armory_munitions");
            var precisionRecipes = system.GetAvailableRecipes("room_workshop_precision");

            Assert.Contains(armoryRecipes, r => r.Id == "recipe_workshop_reload_9x19");
            Assert.DoesNotContain(armoryRecipes, r => r.Id == "recipe_workshop_electronics_soldering");
            Assert.Contains(precisionRecipes, r => r.Id == "recipe_workshop_electronics_soldering");
        }

        [Fact]
        public void AmmoReload_ConsumesMaterialsAtomically_AndYieldsAmmunition()
        {
            var system = CreateSystem(out var inv, out _, out _);
            inv.Add(new ItemDefinition { id = "scrap_metal" }, 10);
            inv.Add(new ItemDefinition { id = "scrap_chemical" }, 5);
            inv.Add(new ItemDefinition { id = "spent_casing" }, 5);

            var res = system.TryStartJob("recipe_workshop_reload_9x19", "room_armory_munitions", null, null, out string jobId);
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.NotEmpty(jobId);

            // Verified materials consumed
            Assert.Equal(8, inv.CountById("scrap_metal"));
            Assert.Equal(4, inv.CountById("scrap_chemical"));
            Assert.Equal(4, inv.CountById("spent_casing"));

            // Advance ticks to complete job
            system.AdvanceLaborTicks(100, 1);
            Assert.Equal(20, inv.CountById("ammo_9x19"));
        }

        [Fact]
        public void MissingMaterial_FailsAtomicallyWithoutPartialConsumption()
        {
            var system = CreateSystem(out var inv, out _, out _);
            inv.Add(new ItemDefinition { id = "scrap_metal" }, 10);
            inv.Add(new ItemDefinition { id = "spent_casing" }, 5);
            // Missing scrap_chemical

            var res = system.TryStartJob("recipe_workshop_reload_9x19", "room_armory_munitions", null, null, out _);
            Assert.NotEqual(ActionResult.StatusKind.Success, res.Status);
            Assert.Equal(10, inv.CountById("scrap_metal"));
            Assert.Equal(5, inv.CountById("spent_casing"));
        }

        [Fact]
        public void WeaponServicing_DelegatesToEquipmentAuthority()
        {
            var system = CreateSystem(out var inv, out var equip, out _);
            equip.RegisterItem("wep_sidearm_1", "weapon_pistol_9mm", "survivor_1", EquipmentFamily.Weapon, 100f);
            equip.UseItem("wep_sidearm_1", 50f); // Degrade to 50

            inv.Add(new ItemDefinition { id = "machine_oil" }, 2);
            inv.Add(new ItemDefinition { id = "cloth" }, 2);

            var res = system.TryStartJob("recipe_workshop_weapon_service_clean", "room_armory_munitions", "wep_sidearm_1", null, out string jobId);
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);

            system.AdvanceLaborTicks(60, 1);

            var instance = equip.State.items.Find(i => i.instanceId == "wep_sidearm_1");
            Assert.NotNull(instance);
            Assert.Equal(70f, instance.condition); // 50 + 20 clean
        }

        [Fact]
        public void ToolWear_DegradesMachine_OverhaulRestoresIt()
        {
            var system = CreateSystem(out var inv, out _, out _);
            var machine = system.GetOrCreateMachineState("room_armory_munitions");
            Assert.Equal(1.0f, machine.ToolingHealth);

            inv.Add(new ItemDefinition { id = "scrap_metal" }, 20);
            inv.Add(new ItemDefinition { id = "scrap_chemical" }, 20);
            inv.Add(new ItemDefinition { id = "spent_casing" }, 20);

            for (int i = 0; i < 5; i++)
            {
                system.TryStartJob("recipe_workshop_reload_9x19", "room_armory_munitions", null, null, out _);
                system.AdvanceLaborTicks(100, 1);
            }

            Assert.True(machine.ToolingHealth < 1.0f);

            // Overhaul machine
            inv.Add(new ItemDefinition { id = "mechanical_parts" }, 5);
            inv.Add(new ItemDefinition { id = "machine_oil" }, 5);
            var overhaulRes = system.TryOverhaulTooling("room_armory_munitions");
            Assert.Equal(ActionResult.StatusKind.Success, overhaulRes.Status);
            Assert.Equal(1.0f, machine.ToolingHealth);
            Assert.Equal(1.0f, machine.Calibration);
        }

        [Fact]
        public void HeavyWorkshop_DelegatesToVehicleAuthority()
        {
            var system = CreateSystem(out var inv, out _, out var vehicles);
            vehicles.AcquireVehicle("veh_quad");
            var veh = vehicles.GetVehicle("veh_quad");
            Assert.NotNull(veh);
            veh.condition = 40f;
            veh.isBrokenDown = true;

            inv.Add(new ItemDefinition { id = "mechanical_parts" }, 10);
            inv.Add(new ItemDefinition { id = "scrap_metal" }, 20);
            inv.Add(new ItemDefinition { id = "machine_oil" }, 5);

            var res = system.TryStartJob("recipe_workshop_heavy_vehicle_service", "room_workshop_heavy", "veh_quad", null, out _);
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);

            system.AdvanceLaborTicks(300, 1);

            Assert.Equal(75f, veh.condition); // 40 + 35
            Assert.False(veh.isBrokenDown);
        }

        [Fact]
        public void WorkerSkill_ReducesLaborTicksAndWaste()
        {
            var system = CreateSystem(out var inv, out _, out _);
            system.BindWorkerSkillProvider((workerId, skillId) => workerId == "expert_artisan" ? 1.0f : 0.0f);

            inv.Add(new ItemDefinition { id = "scrap_metal" }, 10);
            inv.Add(new ItemDefinition { id = "scrap_chemical" }, 5);
            inv.Add(new ItemDefinition { id = "spent_casing" }, 5);

            system.TryStartJob("recipe_workshop_reload_9x19", "room_armory_munitions", null, new[] { "expert_artisan" }, out string jobId);
            var job = system.State.jobs.Find(j => j.JobId == jobId);
            Assert.NotNull(job);

            // Base is 60, skilled worker gives reduction
            Assert.True(job.TotalLaborTicks < 60);
        }

        [Fact]
        public void SaveRestore_PreservesActiveJobsAndMachines()
        {
            var system = CreateSystem(out var inv, out _, out _);
            inv.Add(new ItemDefinition { id = "scrap_metal" }, 10);
            inv.Add(new ItemDefinition { id = "scrap_chemical" }, 5);
            inv.Add(new ItemDefinition { id = "spent_casing" }, 5);

            system.TryStartJob("recipe_workshop_reload_9x19", "room_armory_munitions", null, null, out string jobId);
            system.AdvanceLaborTicks(20, 1);

            var save = system.CaptureState();
            var system2 = CreateSystem(out _, out _, out _);
            system2.RestoreState(save);

            var restoredJob = system2.State.jobs.Find(j => j.JobId == jobId);
            Assert.NotNull(restoredJob);
            Assert.Equal(WorkshopJobStatus.Active, restoredJob.Status);
            Assert.Equal(40, restoredJob.RemainingLaborTicks); // 60 - 20
        }

        [Fact]
        public void DeterministicReplay_YieldsIdenticalOutcomes()
        {
            var sysA = CreateSystem(out var invA, out _, out _, seed: 12345);
            var sysB = CreateSystem(out var invB, out _, out _, seed: 12345);

            invA.Add(new ItemDefinition { id = "scrap_metal" }, 20);
            invA.Add(new ItemDefinition { id = "scrap_chemical" }, 10);
            invA.Add(new ItemDefinition { id = "spent_casing" }, 10);

            invB.Add(new ItemDefinition { id = "scrap_metal" }, 20);
            invB.Add(new ItemDefinition { id = "scrap_chemical" }, 10);
            invB.Add(new ItemDefinition { id = "spent_casing" }, 10);

            sysA.TryStartJob("recipe_workshop_reload_9x19", "room_armory_munitions", null, null, out _);
            sysB.TryStartJob("recipe_workshop_reload_9x19", "room_armory_munitions", null, null, out _);

            sysA.AdvanceLaborTicks(100, 2);
            sysB.AdvanceLaborTicks(100, 2);

            Assert.Equal(invA.CountById("ammo_9x19"), invB.CountById("ammo_9x19"));
            Assert.Equal(sysA.GetOrCreateMachineState("room_armory_munitions").ToolingHealth,
                         sysB.GetOrCreateMachineState("room_armory_munitions").ToolingHealth);
        }
    }
}
