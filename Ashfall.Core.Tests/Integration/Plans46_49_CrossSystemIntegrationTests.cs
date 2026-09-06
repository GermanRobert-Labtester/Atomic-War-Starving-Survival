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

        [Fact]
        public void ScenarioA_ArmoryScarcityLoop_ExecutesReloadServiceAndSaveRoundtrip()
        {
            var ctx1 = new TestCampaignContext(301);

            // 1. Shelter owns damaged sidearm and spent casings
            ctx1.Equipment.RegisterItem("wep_sidearm_test", "weapon_pistol_9mm", "survivor_guard", EquipmentFamily.Weapon, 100f);
            ctx1.Equipment.UseItem("wep_sidearm_test", 75f); // Condition is 25%
            var weaponToken = ctx1.Equipment.State.items.Find(i => i.instanceId == "wep_sidearm_test")!;
            Assert.Equal(25f, weaponToken.condition);

            // Provision materials
            ctx1.Inventory.Add(new ItemDefinition { id = "scrap_metal" }, 10);
            ctx1.Inventory.Add(new ItemDefinition { id = "scrap_chemical" }, 5);
            ctx1.Inventory.Add(new ItemDefinition { id = "spent_casing" }, 5);
            ctx1.Inventory.Add(new ItemDefinition { id = "machine_oil" }, 3);
            ctx1.Inventory.Add(new ItemDefinition { id = "cloth" }, 3);

            int startScrap = ctx1.Inventory.CountById("scrap_metal");

            // 2. Reload ammunition in room_armory_munitions
            var reloadRes = ctx1.Workshop.TryStartJob(
                "recipe_workshop_reload_9x19",
                "room_armory_munitions",
                null,
                new[] { "worker_gunsmith" },
                out string reloadJobId);
            Assert.Equal(ActionResult.StatusKind.Success, reloadRes.Status);

            // Materials deducted atomically at job start
            Assert.Equal(startScrap - 2, ctx1.Inventory.CountById("scrap_metal"));
            Assert.Equal(4, ctx1.Inventory.CountById("spent_casing"));

            // Complete reload job
            ctx1.Workshop.AdvanceLaborTicks(60, 1);
            Assert.Equal(20, ctx1.Inventory.CountById("ammo_9x19"));

            // 3. Service weapon (clean & decoke: +20 condition)
            var serviceRes = ctx1.Workshop.TryStartJob(
                "recipe_workshop_weapon_service_clean",
                "room_armory_munitions",
                "wep_sidearm_test",
                new[] { "worker_gunsmith" },
                out _);
            Assert.Equal(ActionResult.StatusKind.Success, serviceRes.Status);
            ctx1.Workshop.AdvanceLaborTicks(45, 1);

            // Verify condition restored through equipment authority
            Assert.Equal(45f, weaponToken.condition);

            // 4. Save/restore roundtrip
            var wsSave = ctx1.Workshop.CaptureState();
            var eqSave = ctx1.Equipment.CaptureState();
            var invSave = ctx1.Inventory.CaptureState();

            var ctx2 = new TestCampaignContext(301);
            ctx2.Workshop.RestoreState(wsSave);
            ctx2.Equipment.RestoreState(eqSave);
            ctx2.Inventory.RestoreState(invSave, id => new ItemDefinition { id = id });

            // 5. Verify no phantom jobs, no duplicated outputs, condition preserved
            Assert.All(ctx2.Workshop.State.jobs, j => Assert.Equal(WorkshopJobStatus.Completed, j.Status));
            Assert.DoesNotContain(ctx2.Workshop.State.jobs, j => j.Status == WorkshopJobStatus.Active || j.Status == WorkshopJobStatus.Queued);
            Assert.Equal(2, ctx2.Workshop.State.jobs.Count);
            Assert.Equal(20, ctx2.Inventory.CountById("ammo_9x19"));
            var restoredWeapon = ctx2.Equipment.State.items.Find(i => i.instanceId == "wep_sidearm_test")!;
            Assert.Equal(45f, restoredWeapon.condition);
        }

        [Fact]
        public void ScenarioB_RadioToExpeditionDiscovery_TriangulatesSOSAndSchedulesExpiry()
        {
            var ctx1 = new TestCampaignContext(302);

            // 1. Tune to SOS broadcast frequency 3850 kHz
            ctx1.RadioStation.TuneTo(3850, "hf");
            var scan = ctx1.RadioStation.ScanFrequency(1);
            Assert.True(scan.FoundSignal);
            Assert.Equal("radio_intercept_sos_quarry_shelter_02", scan.InterceptId);

            // 2. Record 2 required bearings (0 and 90 deg)
            ctx1.RadioStation.RecordBearing("radio_intercept_sos_quarry_shelter_02", 0);
            bool unlocked = ctx1.RadioStation.RecordBearing("radio_intercept_sos_quarry_shelter_02", 90);
            Assert.True(unlocked);

            // 3. World location discovered & expiry set
            Assert.Contains("loc_recovery_yard", ctx1.RadioStation.State.discoveredLocationIds);
            var progress = ctx1.RadioStation.GetOrCreateInterceptProgress("radio_intercept_sos_quarry_shelter_02");
            Assert.True(progress.Resolved);
            Assert.Equal(4, progress.ExpiresOnDay); // Day 1 + 3 days expiry

            // 4. Save / Restore roundtrip
            var radioSave = ctx1.RadioStation.CaptureState();

            var ctx2 = new TestCampaignContext(302);
            ctx2.RadioStation.RestoreState(radioSave);

            Assert.Contains("loc_recovery_yard", ctx2.RadioStation.State.discoveredLocationIds);
            var restoredProgress = ctx2.RadioStation.GetOrCreateInterceptProgress("radio_intercept_sos_quarry_shelter_02");
            Assert.Equal(4, restoredProgress.ExpiresOnDay);
            Assert.False(restoredProgress.IsExpired);
            Assert.True(restoredProgress.Resolved);

            // Also detect an unresolved distress signal with expiry
            ctx2.RadioStation.TuneTo(1850, "hf");
            var scanDistress = ctx2.RadioStation.ScanFrequency(1);
            Assert.True(scanDistress.FoundSignal);
            Assert.Equal("radio_intercept_pumphouse_distress_11", scanDistress.InterceptId);
            var unresolvedProgress = ctx2.RadioStation.GetOrCreateInterceptProgress("radio_intercept_pumphouse_distress_11");
            Assert.False(unresolvedProgress.Resolved);
            Assert.Equal(3, unresolvedProgress.ExpiresOnDay); // Day 1 + 2 days expiry

            // 5. Advance past expiry (Day 5) -> unresolved distress expires, resolved location remains intact
            ctx2.RadioStation.TickDay(5);
            Assert.False(restoredProgress.IsExpired); // Resolved signal does not expire
            Assert.True(unresolvedProgress.IsExpired); // Unresolved signal expires
        }

        [Fact]
        public void ScenarioC_SocialPressureFromShelterCapacity_BunkFrictionToPrivateRelief()
        {
            var ctx1 = new TestCampaignContext(303);
            ctx1.SocialDynamics.BindMediatorSkillProvider((m, s) => m == "counselor_dan" ? 1.0f : 0.0f);

            // 1. Incompatible survivors in crowded bunks
            ctx1.SocialDynamics.RegisterSurvivorRoom("survivor_grumpy", "room_bunks_crowded");
            ctx1.SocialDynamics.RegisterSurvivorRoom("survivor_loud", "room_bunks_crowded");

            var incident = ctx1.SocialDynamics.EvaluateRoomDynamics("room_bunks_crowded", new[] { "survivor_grumpy", "survivor_loud" }, 1);
            Assert.NotNull(incident);

            // Relationship affinity deteriorated
            var rel = ctx1.Relations.GetOrCreateRelationship("survivor_grumpy", "survivor_loud");
            Assert.True(rel.affinity < 0f);

            // 2. Successful mediation by skilled counselor
            var medResult = ctx1.SocialDynamics.TryMediateIncident(incident.IncidentId, "counselor_dan");
            Assert.Equal(ActionResult.StatusKind.Success, medResult.Status);
            Assert.True(incident.Resolved);

            // 3. Move survivor to private quarters -> relieves privacy fatigue
            ctx1.SocialDynamics.RegisterSurvivorRoom("survivor_grumpy", "room_quarters_private");
            var grumpyProfile = ctx1.SocialDynamics.GetOrCreatePrivacyProfile("survivor_grumpy");
            grumpyProfile.PrivacyFatiguePermille = 600;

            ctx1.SocialDynamics.EvaluateRoomDynamics("room_quarters_private", new[] { "survivor_grumpy" }, 2);
            Assert.Equal(350, grumpyProfile.PrivacyFatiguePermille); // 600 - 250

            // 4. Save/Restore preserves state
            var socialSave = ctx1.SocialDynamics.CaptureState();
            var ctx2 = new TestCampaignContext(303);
            ctx2.SocialDynamics.RestoreState(socialSave);

            Assert.Equal(350, ctx2.SocialDynamics.GetOrCreatePrivacyProfile("survivor_grumpy").PrivacyFatiguePermille);
            Assert.Equal("room_quarters_private", ctx2.SocialDynamics.GetOrCreatePrivacyProfile("survivor_grumpy").AssignedRoomId);
        }

        [Fact]
        public void ScenarioD_DeepStrataEmergency_MitigationAndCaveInRescueLifecycle()
        {
            var ctx1 = new TestCampaignContext(304);

            // 1. Deep sector setup
            var sector = ctx1.ExcavationHazards.GetOrCreateSector("sector_abyss_1");
            sector.MethanePpm = 3500;
            sector.ShoringHealthPermille = 450;

            // Provision emergency mitigation materials
            ctx1.Inventory.Add(new ItemDefinition { id = "iron_pipe" }, 4);
            ctx1.Inventory.Add(new ItemDefinition { id = "mechanical_parts" }, 4);
            ctx1.Inventory.Add(new ItemDefinition { id = "scrap_wood" }, 8);
            ctx1.Inventory.Add(new ItemDefinition { id = "scrap_metal" }, 6);
            ctx1.Inventory.Add(new ItemDefinition { id = "cloth" }, 4);

            // 2. Install ventilation blower -> methane decreases
            var ventRes = ctx1.ExcavationHazards.TryApplyMitigation("sector_abyss_1", "mitigation_ventilation_blower_install");
            Assert.Equal(ActionResult.StatusKind.Success, ventRes.Status);
            Assert.True(sector.MethanePpm < 3500);

            // 3. Reinforce shoring -> health increases
            var shoreRes = ctx1.ExcavationHazards.TryApplyMitigation("sector_abyss_1", "mitigation_timber_shoring_reinforcement");
            Assert.Equal(ActionResult.StatusKind.Success, shoreRes.Status);
            Assert.True(sector.ShoringHealthPermille > 450);

            // 4. Cave-in traps 2 miners
            ctx1.ExcavationHazards.TriggerCaveInRescue("sector_abyss_1", new[] { "miner_ed", "miner_frank" }, deadlineDays: 3, requiredLabor: 200);
            Assert.Equal(2, sector.ActiveTrappedMiners.Count);

            // Progress half of required rescue labor (100 ticks)
            ctx1.ExcavationHazards.ProgressRescueLabor("sector_abyss_1", 100);
            Assert.Equal(100, sector.RescueLaborRemaining);
            Assert.False(sector.RescueCompleted);

            // 5. Save mid-rescue and restore into fresh context
            var hazSave = ctx1.ExcavationHazards.CaptureState();
            var ctx2 = new TestCampaignContext(304);
            ctx2.ExcavationHazards.RestoreState(hazSave);

            var restoredSector = ctx2.ExcavationHazards.GetOrCreateSector("sector_abyss_1");
            Assert.Equal(2, restoredSector.ActiveTrappedMiners.Count);
            Assert.Equal(100, restoredSector.RescueLaborRemaining);

            // 6. Complete remaining rescue labor
            ctx2.ExcavationHazards.ProgressRescueLabor("sector_abyss_1", 100);
            Assert.True(restoredSector.RescueCompleted);
            Assert.Empty(restoredSector.ActiveTrappedMiners);
        }

        [Fact]
        public void ScenarioE_CrossSystemShelterCrisis_DeterministicSimulationAcrossAllFourDomains()
        {
            string RunCrisisSimulation(int seed)
            {
                var ctx = new TestCampaignContext(seed);

                // Initial inventory across domains
                ctx.Inventory.Add(new ItemDefinition { id = "scrap_metal" }, 30);
                ctx.Inventory.Add(new ItemDefinition { id = "scrap_chemical" }, 15);
                ctx.Inventory.Add(new ItemDefinition { id = "spent_casing" }, 10);
                ctx.Inventory.Add(new ItemDefinition { id = "scrap_electronic" }, 10);
                ctx.Inventory.Add(new ItemDefinition { id = "iron_pipe" }, 10);
                ctx.Inventory.Add(new ItemDefinition { id = "mechanical_parts" }, 10);
                ctx.Inventory.Add(new ItemDefinition { id = "scrap_wood" }, 20);

                // Plan 46: Start reload & radio component refit jobs
                ctx.Workshop.TryStartJob("recipe_workshop_reload_9x19", "room_armory_munitions", null, null, out _);
                ctx.Workshop.TryStartJob("recipe_workshop_radio_component_refit", "room_workshop_precision", null, null, out _);

                // Plan 47: Tune to early warning frequency
                ctx.RadioStation.TuneTo(14220, "hf");
                ctx.RadioStation.ScanFrequency(1);

                // Plan 48: Social assignment
                ctx.SocialDynamics.RegisterSurvivorRoom("dweller_alpha", "room_bunks_crowded");
                ctx.SocialDynamics.RegisterSurvivorRoom("dweller_beta", "room_bunks_crowded");
                ctx.SocialDynamics.EvaluateRoomDynamics("room_bunks_crowded", new[] { "dweller_alpha", "dweller_beta" }, 1);

                // Plan 49: Deep excavation emergency
                var sec = ctx.ExcavationHazards.GetOrCreateSector("sector_crisis_deep");
                sec.MethanePpm = 3000;
                ctx.ExcavationHazards.TryApplyMitigation("sector_crisis_deep", "mitigation_ventilation_blower_install");

                // Advance multi-day campaign progression
                for (int day = 1; day <= 3; day++)
                {
                    ctx.Workshop.TickDay(day);
                    ctx.RadioStation.TickDay(day);
                    ctx.SocialDynamics.TickDay(day);
                    ctx.ExcavationHazards.TickDay(day);
                }

                // Mid-crisis save and restore
                var wsSave = ctx.Workshop.CaptureState();
                var radSave = ctx.RadioStation.CaptureState();
                var socSave = ctx.SocialDynamics.CaptureState();
                var hazSave = ctx.ExcavationHazards.CaptureState();

                var restoredCtx = new TestCampaignContext(seed);
                restoredCtx.Workshop.RestoreState(wsSave);
                restoredCtx.RadioStation.RestoreState(radSave);
                restoredCtx.SocialDynamics.RestoreState(socSave);
                restoredCtx.ExcavationHazards.RestoreState(hazSave);

                // Continue 2 more days in restored context
                for (int day = 4; day <= 5; day++)
                {
                    restoredCtx.Workshop.TickDay(day);
                    restoredCtx.RadioStation.TickDay(day);
                    restoredCtx.SocialDynamics.TickDay(day);
                    restoredCtx.ExcavationHazards.TickDay(day);
                }

                var composite = new
                {
                    Workshop = restoredCtx.Workshop.CaptureState(),
                    Radio = restoredCtx.RadioStation.CaptureState(),
                    Social = restoredCtx.SocialDynamics.CaptureState(),
                    Hazards = restoredCtx.ExcavationHazards.CaptureState()
                };

                var serializer = new SystemTextJsonSerializer();
                return serializer.Serialize(composite);
            }

            string result1 = RunCrisisSimulation(5555);
            string result2 = RunCrisisSimulation(5555);
            string result3 = RunCrisisSimulation(5555);

            Assert.Equal(result1, result2);
            Assert.Equal(result2, result3);
        }
    }
}
