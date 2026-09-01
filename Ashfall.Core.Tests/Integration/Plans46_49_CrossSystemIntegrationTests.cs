// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Excavation;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Memorial;
using Ashfall.Core.Radio;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Integration
{
    public class Plans46_49_CrossSystemIntegrationTests
    {
        private static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(Path.GetFullPath(AppContext.BaseDirectory));
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            return AppContext.BaseDirectory;
        }

        private static string LoadDataFile(string relativePath)
        {
            string root = FindRepoRoot();
            string path = Path.Combine(root, relativePath);
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return string.Empty;
        }

        private sealed class TestCampaignContext
        {
            public ISeededRng Rng { get; }
            public Inventory.Inventory Inventory { get; }
            public CraftingSystem Crafting { get; }
            public EquipmentConditionSystem Equipment { get; }
            public ExpeditionVehicleSystem Vehicles { get; }
            public SkyLayerArmorSystem SkyArmor { get; }
            public OrbitalHarrowTelemetrySystem Harrow { get; }
            public ExcavationSystem Excavation { get; }
            public SurvivorRelationsSystem Relations { get; }
            public MemorialSystem Memorial { get; }

            public ShelterWorkshopSystem Workshop { get; }
            public ShelterRadioStationSystem RadioStation { get; }
            public ShelterSocialDynamicsSystem SocialDynamics { get; }
            public ExcavationHazardSystem ExcavationHazards { get; }

            public TestCampaignContext(int seed)
            {
                Rng = new SeededRng(seed);
                Inventory = new Inventory.Inventory { Capacity = 100, MaxWeight = 500f };
                Crafting = new CraftingSystem(Inventory);
                Equipment = new EquipmentConditionSystem(Rng, Inventory, Crafting);
                Vehicles = new ExpeditionVehicleSystem(Rng);
                SkyArmor = new SkyLayerArmorSystem();
                Harrow = new OrbitalHarrowTelemetrySystem(SkyArmor, Rng);
                Excavation = new ExcavationSystem(Rng);
                Relations = new SurvivorRelationsSystem(Rng);
                Memorial = new MemorialSystem(new MemorialState());

                Workshop = new ShelterWorkshopSystem(Inventory, Rng, Equipment, Vehicles);
                RadioStation = new ShelterRadioStationSystem(Rng, Harrow);
                SocialDynamics = new ShelterSocialDynamicsSystem(Rng, Relations, null, Memorial);
                ExcavationHazards = new ExcavationHazardSystem(Inventory, Rng, Excavation, SkyArmor);

                // Load catalogs
                string workshopJson = LoadDataFile("Assets/StreamingAssets/Data/workshop_recipes.json");
                if (!string.IsNullOrEmpty(workshopJson)) Workshop.LoadCatalog(workshopJson);

                string radioJson = LoadDataFile("Assets/StreamingAssets/Data/radio_intercepts.json");
                if (!string.IsNullOrEmpty(radioJson)) RadioStation.LoadCatalog(radioJson);

                string socialJson = LoadDataFile("Assets/StreamingAssets/Data/shelter_social_events.json");
                if (!string.IsNullOrEmpty(socialJson)) SocialDynamics.LoadCatalog(socialJson);

                string excavationJson = LoadDataFile("Assets/StreamingAssets/Data/excavation_hazard_mitigation.json");
                if (!string.IsNullOrEmpty(excavationJson)) ExcavationHazards.LoadCatalog(excavationJson);
            }
        }

        [Fact]
        public void WorkshopAndEquipmentBridge_WeaponRefurbishment_RestoresCombatReadiness()
        {
            var ctx = new TestCampaignContext(101);

            // Register damaged firearm
            ctx.Equipment.RegisterItem("wep_carbine_alpha", "weapon_rifle_556", "survivor_scout", EquipmentFamily.Weapon, 100f);
            ctx.Equipment.UseItem("wep_carbine_alpha", 60f); // Condition drops to 40%

            // High jam risk at low condition
            var token = ctx.Equipment.State.items.Find(i => i.instanceId == "wep_carbine_alpha");
            Assert.NotNull(token);
            Assert.Equal(40f, token.condition);

            // Add required materials for weapon refurbishment
            ctx.Inventory.Add(new ItemDefinition { id = "mechanical_parts" }, 2);
            ctx.Inventory.Add(new ItemDefinition { id = "machine_oil" }, 2);
            ctx.Inventory.Add(new ItemDefinition { id = "scrap_metal" }, 4);

            var startRes = ctx.Workshop.TryStartJob(
                "recipe_workshop_weapon_service_refurbish",
                "room_workshop_precision",
                "wep_carbine_alpha",
                null,
                out string jobId);

            Assert.Equal(ActionResult.StatusKind.Success, startRes.Status);

            // Complete workshop labor
            ctx.Workshop.AdvanceLaborTicks(100, 1);

            Assert.Equal(80f, token.condition); // 40 + 40 refurbishment
            Assert.Equal(1, token.lastMaintainedDay);
        }

        [Fact]
        public void HeavyWorkshopAndVehicleGarage_PowertrainRebuild_EnablesExpeditionReadiness()
        {
            var ctx = new TestCampaignContext(102);

            ctx.Vehicles.LoadCatalog(new VehicleCatalog
            {
                vehicles = new List<VehicleDefinition>
                {
                    new() { vehicle_id = "veh_recon_truck", display_name = "Recon Truck", condition_max = 100f, max_fuel = 80f }
                }
            });
            ctx.Vehicles.AcquireVehicle("veh_recon_truck");
            var veh = ctx.Vehicles.GetVehicle("veh_recon_truck");
            Assert.NotNull(veh);
            veh.condition = 20f;
            veh.isBrokenDown = true;

            // Supply rebuild parts
            ctx.Inventory.Add(new ItemDefinition { id = "mechanical_parts" }, 10);
            ctx.Inventory.Add(new ItemDefinition { id = "scrap_metal" }, 20);
            ctx.Inventory.Add(new ItemDefinition { id = "machine_oil" }, 5);

            var startRes = ctx.Workshop.TryStartJob(
                "recipe_workshop_heavy_vehicle_service",
                "room_workshop_heavy",
                "veh_recon_truck",
                null,
                out _);

            Assert.Equal(ActionResult.StatusKind.Success, startRes.Status);
            ctx.Workshop.AdvanceLaborTicks(250, 1);

            Assert.Equal(55f, veh.condition); // 20 + 35
            Assert.False(veh.isBrokenDown);
        }

        [Fact]
        public void RadioIntelligenceAndMap_TriangulatesHiddenDepotLocation()
        {
            var ctx = new TestCampaignContext(103);
            ctx.RadioStation.TuneTo(7115, "hf");

            var scan = ctx.RadioStation.ScanFrequency(1);
            Assert.True(scan.FoundSignal);

            // Record 3 distinct bearings (separated by >= 20 deg)
            ctx.RadioStation.RecordBearing("radio_intercept_meridian_supply_column_01", 0);
            ctx.RadioStation.RecordBearing("radio_intercept_meridian_supply_column_01", 90);
            bool unlocked = ctx.RadioStation.RecordBearing("radio_intercept_meridian_supply_column_01", 180);

            Assert.True(unlocked);
            Assert.Contains("loc_diesel_tank_farm", ctx.RadioStation.State.discoveredLocationIds);
        }

        [Fact]
        public void CrowdedSleepingQuartersAndMediation_TracksAffinityDriftAndAccord()
        {
            var ctx = new TestCampaignContext(104);
            ctx.SocialDynamics.BindMediatorSkillProvider((mediator, skill) => mediator == "dweller_counselor" ? 1.0f : 0.0f);

            // 2 dwellers assigned to crowded bunks
            ctx.SocialDynamics.RegisterSurvivorRoom("dweller_alice", "room_bunks_crowded");
            ctx.SocialDynamics.RegisterSurvivorRoom("dweller_bob", "room_bunks_crowded");

            var incident = ctx.SocialDynamics.EvaluateRoomDynamics("room_bunks_crowded", new[] { "dweller_alice", "dweller_bob" }, 1);
            Assert.NotNull(incident);

            var rel = ctx.Relations.GetOrCreateRelationship("dweller_alice", "dweller_bob");
            Assert.True(rel.affinity < 0f);

            // Attempt mediation
            var medRes = ctx.SocialDynamics.TryMediateIncident(incident.IncidentId, "dweller_counselor");
            Assert.Equal(ActionResult.StatusKind.Success, medRes.Status);
            Assert.True(incident.IsMediated);
            Assert.True(incident.Resolved);
        }

        [Fact]
        public void SubterraneanHazardsAndRescue_EmergencyClearance_SavesTrappedMiners()
        {
            var ctx = new TestCampaignContext(105);

            // Sector with hazardous methane & spore buildup
            var sector = ctx.ExcavationHazards.GetOrCreateSector("sector_sublevel_2");
            sector.MethanePpm = 3000;
            sector.SporeConcentrationPermille = 400;

            // Apply ventilation blower
            ctx.Inventory.Add(new ItemDefinition { id = "iron_pipe" }, 2);
            ctx.Inventory.Add(new ItemDefinition { id = "mechanical_parts" }, 2);
            var ventRes = ctx.ExcavationHazards.TryApplyMitigation("sector_sublevel_2", "mitigation_ventilation_blower_install");
            Assert.Equal(ActionResult.StatusKind.Success, ventRes.Status);
            Assert.True(sector.MethanePpm < 3000);

            // Trigger cave-in rescue
            ctx.ExcavationHazards.TriggerCaveInRescue("sector_sublevel_2", new[] { "miner_carter", "miner_davis" }, deadlineDays: 3, requiredLabor: 200);
            Assert.Equal(2, sector.ActiveTrappedMiners.Count);

            // Progress rescue operation
            ctx.ExcavationHazards.ProgressRescueLabor("sector_sublevel_2", 200);
            Assert.True(sector.RescueCompleted);
            Assert.Empty(sector.ActiveTrappedMiners);
        }

        [Fact]
        public void FullCampaignSaves_RoundTripCaptureAndRestoreAllFourSubsystems()
        {
            var ctx1 = new TestCampaignContext(200);

            // Setup state across all 4 systems
            ctx1.Inventory.Add(new ItemDefinition { id = "scrap_metal" }, 10);
            ctx1.Inventory.Add(new ItemDefinition { id = "scrap_chemical" }, 5);
            ctx1.Inventory.Add(new ItemDefinition { id = "spent_casing" }, 5);
            ctx1.Workshop.TryStartJob("recipe_workshop_reload_9x19", "room_armory_munitions", null, null, out _);

            ctx1.RadioStation.TuneTo(7115, "hf");
            ctx1.RadioStation.ScanFrequency(1);
            ctx1.RadioStation.RecordBearing("radio_intercept_meridian_supply_column_01", 60);

            ctx1.SocialDynamics.RegisterSurvivorRoom("dweller_1", "room_quarters_private");
            ctx1.SocialDynamics.GetOrCreatePrivacyProfile("dweller_1").PrivacyFatiguePermille = 400;

            var sector = ctx1.ExcavationHazards.GetOrCreateSector("sector_deep");
            sector.MethanePpm = 2500;
            sector.ShoringHealthPermille = 750;

            // Capture state from all 4 systems
            var workshopSave = ctx1.Workshop.CaptureState();
            var radioSave = ctx1.RadioStation.CaptureState();
            var socialSave = ctx1.SocialDynamics.CaptureState();
            var hazardSave = ctx1.ExcavationHazards.CaptureState();

            // Restore in fresh context
            var ctx2 = new TestCampaignContext(200);
            ctx2.Workshop.RestoreState(workshopSave);
            ctx2.RadioStation.RestoreState(radioSave);
            ctx2.SocialDynamics.RestoreState(socialSave);
            ctx2.ExcavationHazards.RestoreState(hazardSave);

            // Assertions
            Assert.Single(ctx2.Workshop.State.jobs);
            Assert.Equal(7115, ctx2.RadioStation.State.tunedFrequencyKhz);
            Assert.Equal(400, ctx2.SocialDynamics.GetOrCreatePrivacyProfile("dweller_1").PrivacyFatiguePermille);
            Assert.Equal(2500, ctx2.ExcavationHazards.GetOrCreateSector("sector_deep").MethanePpm);
        }

        [Fact]
        public void CrossSystemDeterminism_PairedRunsYieldIdenticalStateSnapshots()
        {
            var runA = new TestCampaignContext(9999);
            var runB = new TestCampaignContext(9999);

            // Run identical simulated 5-day cycle on both contexts
            for (int day = 1; day <= 5; day++)
            {
                runA.Workshop.TickDay(day);
                runB.Workshop.TickDay(day);

                runA.RadioStation.TickDay(day);
                runB.RadioStation.TickDay(day);

                runA.SocialDynamics.TickDay(day);
                runB.SocialDynamics.TickDay(day);

                runA.ExcavationHazards.TickDay(day);
                runB.ExcavationHazards.TickDay(day);
            }

            var serializer = new SystemTextJsonSerializer();
            string jsonA_ws = serializer.Serialize(runA.Workshop.CaptureState());
            string jsonB_ws = serializer.Serialize(runB.Workshop.CaptureState());
            Assert.Equal(jsonA_ws, jsonB_ws);

            string jsonA_rad = serializer.Serialize(runA.RadioStation.CaptureState());
            string jsonB_rad = serializer.Serialize(runB.RadioStation.CaptureState());
            Assert.Equal(jsonA_rad, jsonB_rad);

            string jsonA_soc = serializer.Serialize(runA.SocialDynamics.CaptureState());
            string jsonB_soc = serializer.Serialize(runB.SocialDynamics.CaptureState());
            Assert.Equal(jsonA_soc, jsonB_soc);

            string jsonA_haz = serializer.Serialize(runA.ExcavationHazards.CaptureState());
            string jsonB_haz = serializer.Serialize(runB.ExcavationHazards.CaptureState());
            Assert.Equal(jsonA_haz, jsonB_haz);
        }
    }
}
