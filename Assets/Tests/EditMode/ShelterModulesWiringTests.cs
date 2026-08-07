using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// ShelterModule_* wiring (CaptureState full set): 46 modules — API smoke + Capture/Restore + save slots.
    /// </summary>
    [TestFixture]
    public class ShelterModulesWiringTests
    {
        private const float Eps = 1e-3f;

        private static string TempDir(string tag)
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_shelter_mod_" + tag + "_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            return dir;
        }

        private static SaveSystem MakeSave(string dir, Action<SaveSystem> wire)
        {
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);
            var weather = new WeatherSystem(null, 3);
            var temp = new TemperatureSystem(null, weather);
            var rad = new RadiationSystem(needs);
            var ss = new SaveSystem(new SaveSystem.CoreDeps
            {
                GameState = new GameState(),
                WeatherSystem = weather,
                TemperatureSystem = temp,
                NeedsSystem = needs,
                RadiationSystem = rad,
                Shelter = new ShelterClass(),
                GetSurvivors = () => new List<Survivor>(),
                ItemLookup = id => null,
                ModuleLookup = id => null,
                SavesDir = dir
            });
            wire(ss);
            return ss;
        }

        [Test]
        public void AcidTrap_ArmTrigger_Capture()
        {
            var m = new ShelterModule_AcidTrap();
            m.Refill(50f);
            m.Arm();
            Assert.IsTrue(m.CanTrigger());
            Assert.IsTrue(m.Trigger(0.1f)); // unarmored
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_acid_trap", save.moduleId);
            Assert.AreEqual(1, save.triggeredCount);
            Assert.AreEqual(25f, save.acidReserve, Eps);
            var m2 = new ShelterModule_AcidTrap();
            m2.RestoreState(save);
            Assert.AreEqual(1, m2.CaptureState().triggeredCount);
            Assert.AreEqual(25f, m2.CaptureState().acidReserve, Eps);
        }

        [Test]
        public void Autodoc_Surgery_Capture()
        {
            var m = new ShelterModule_Autodoc();
            Assert.IsTrue(m.PerformSurgery("sv_a", "broken_leg"));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_autodoc", save.moduleId);
            Assert.Contains("sv_a", save.treatedPatientIds);
            Assert.Contains("broken_leg", save.treatedAfflictionIds);
            var m2 = new ShelterModule_Autodoc();
            m2.RestoreState(save);
            var s2 = m2.CaptureState();
            Assert.Contains("sv_a", s2.treatedPatientIds);
            Assert.Contains("broken_leg", s2.treatedAfflictionIds);
        }

        [Test]
        public void Cctv_Activate_Capture()
        {
            var m = new ShelterModule_CCTV();
            m.Activate("op_1");
            Assert.IsTrue(m.IsActive());
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_cctv", save.moduleId);
            Assert.IsTrue(save.isActive);
            var m2 = new ShelterModule_CCTV();
            m2.RestoreState(save);
            Assert.IsTrue(m2.IsActive());
        }

        [Test]
        public void Classroom_Enroll_Capture()
        {
            var m = new ShelterModule_Classroom();
            Assert.IsTrue(m.Enroll("child_1", "teacher_1"));
            Assert.IsTrue(m.IsEnrolled("child_1"));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_classroom", save.moduleId);
            Assert.AreEqual("teacher_1", save.teacherId);
            Assert.Contains("child_1", save.enrolledChildIds);
            var m2 = new ShelterModule_Classroom();
            m2.RestoreState(save);
            Assert.IsTrue(m2.IsEnrolled("child_1"));
        }

        [Test]
        public void Confessional_Session_Capture()
        {
            var m = new ShelterModule_Confessional();
            Assert.IsTrue(m.EnterAsSpeaker("sv_sp"));
            Assert.IsTrue(m.EnterAsListener("sv_li"));
            Assert.IsTrue(m.StartSession());
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_confessional", save.moduleId);
            Assert.IsTrue(save.sessionActive);
            Assert.AreEqual("sv_sp", save.speakerId);
            var m2 = new ShelterModule_Confessional();
            m2.RestoreState(save);
            Assert.IsTrue(m2.IsSessionActive());
            Assert.AreEqual("sv_sp", m2.CaptureState().speakerId);
        }

        [Test]
        public void Conveyor_Activate_Capture()
        {
            var m = new ShelterModule_Conveyor();
            m.Activate();
            Assert.IsTrue(m.IsActive());
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_conveyor", save.moduleId);
            Assert.IsTrue(save.isActive);
            var m2 = new ShelterModule_Conveyor();
            m2.RestoreState(save);
            Assert.IsTrue(m2.IsActive());
        }

        [Test]
        public void DaylightSensor_Activate_Capture()
        {
            var m = new ShelterModule_DaylightSensor();
            m.Activate();
            Assert.IsTrue(m.IsActive());
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_daylight_sensor", save.moduleId);
            Assert.IsTrue(save.isActive);
            var m2 = new ShelterModule_DaylightSensor();
            m2.RestoreState(save);
            Assert.IsTrue(m2.IsActive());
        }

        [Test]
        public void DroneStation_Deploy_Capture()
        {
            var m = new ShelterModule_DroneStation();
            Assert.IsTrue(m.Deploy(100f));
            Assert.IsTrue(m.IsActive());
            m.TickHour(new List<string> { "room_a" }, new List<string> { "mod_x" }, new List<string> { "room_a" });
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_drone_station", save.moduleId);
            Assert.IsTrue(save.isActive);
            Assert.Contains("room_a", save.cleanedRoomIds);
            var m2 = new ShelterModule_DroneStation();
            m2.RestoreState(save);
            Assert.IsTrue(m2.IsActive());
            Assert.Contains("room_a", m2.CaptureState().cleanedRoomIds);
        }

        [Test]
        public void HoloEmitter_Activate_Capture()
        {
            var m = new ShelterModule_HoloEmitter();
            m.Activate();
            Assert.IsTrue(m.IsActive());
            Assert.AreEqual(0.2f, m.GetRaidFrequencyMultiplier("human"), Eps);
            Assert.AreEqual(1.0f, m.GetRaidFrequencyMultiplier("mutant"), Eps);
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_holo_emitter", save.moduleId);
            Assert.IsTrue(save.isActive);
            var m2 = new ShelterModule_HoloEmitter();
            m2.RestoreState(save);
            Assert.IsTrue(m2.IsActive());
        }

        [Test]
        public void InsectFarm_Harvest_Capture()
        {
            var m = new ShelterModule_InsectFarm();
            m.TickDay("shelter_1", humidity: 0.9f, heat: 0.8f, vulnerableSurvivors: new List<string> { "sv_v" });
            Assert.IsTrue(m.IsActive());
            Assert.AreEqual(5f, m.GetTotalProteinHarvested(), Eps);
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_insect_farm", save.moduleId);
            Assert.AreEqual(5f, save.totalProteinHarvested, Eps);
            var m2 = new ShelterModule_InsectFarm();
            m2.RestoreState(save);
            Assert.AreEqual(5f, m2.GetTotalProteinHarvested(), Eps);
            Assert.IsTrue(m2.IsActive());
        }

        [Test]
        public void Lathe_Convert_Capture()
        {
            var m = new ShelterModule_Lathe();
            int parts = m.ConvertRawMetal("workshop", 4, 150f);
            Assert.AreEqual(4, parts);
            Assert.IsTrue(m.CaptureState().isActive);
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_lathe", save.moduleId);
            var m2 = new ShelterModule_Lathe();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isActive);
        }

        [Test]
        public void Mortar_Build_Capture()
        {
            var m = new ShelterModule_Mortar();
            m.Build("surface_pad_1");
            Assert.IsTrue(m.IsBuilt);
            Assert.IsTrue(m.Bombard("op_2", "node_east", isAdjacent: true));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_mortar", save.moduleId);
            Assert.IsTrue(save.isBuilt);
            Assert.AreEqual("surface_pad_1", save.surfaceLocation);
            var m2 = new ShelterModule_Mortar();
            m2.RestoreState(save);
            Assert.IsTrue(m2.IsBuilt);
            Assert.AreEqual("surface_pad_1", m2.CaptureState().surfaceLocation);
        }

        [Test]
        public void PanicButton_Lockdown_Capture()
        {
            var m = new ShelterModule_PanicButton();
            m.Activate(new List<string> { "sv_1", "sv_2" });
            Assert.IsTrue(m.IsLockedDown());
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_panic_button", save.moduleId);
            Assert.IsTrue(save.isLockedDown);
            var m2 = new ShelterModule_PanicButton();
            m2.RestoreState(save);
            Assert.IsTrue(m2.IsLockedDown());
        }

        [Test]
        public void Pitfall_Kill_Capture()
        {
            var m = new ShelterModule_Pitfall();
            m.Activate();
            var (killed, lootLost) = m.TryKillRaider();
            Assert.IsTrue(killed);
            Assert.IsTrue(lootLost);
            Assert.AreEqual(1, m.RaidersKilled);
            var save = m.CaptureState();
            Assert.AreEqual(1, save.RaidersKilled);
            Assert.IsTrue(save.IsActive);
            var m2 = new ShelterModule_Pitfall();
            m2.RestoreState(save);
            Assert.AreEqual(1, m2.RaidersKilled);
            Assert.IsTrue(m2.IsActive);
        }

        [Test]
        public void Reloader_Reload_Capture()
        {
            var m = new ShelterModule_Reloader();
            // force duds off by setting chance then reloading with fixed rng
            var st = m.CaptureState();
            st.dudChance = 0f;
            m.RestoreState(st);
            var (live, duds) = m.ReloadAmmo("sv_r", 10, new System.Random(1));
            Assert.AreEqual(10, live);
            Assert.AreEqual(0, duds);
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_reloader", save.moduleId);
            Assert.AreEqual(0f, save.dudChance, Eps);
            var m2 = new ShelterModule_Reloader();
            m2.RestoreState(save);
            Assert.AreEqual(0f, m2.CaptureState().dudChance, Eps);
        }

        [Test]
        public void Sorter_Activate_Capture()
        {
            var m = new ShelterModule_Sorter();
            m.Activate();
            string dest = m.SortItem("item_1", "food");
            Assert.IsFalse(string.IsNullOrEmpty(dest));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_sorter", save.moduleId);
            Assert.IsTrue(save.isActive);
            var m2 = new ShelterModule_Sorter();
            m2.RestoreState(save);
            Assert.IsTrue(m2.IsActive());
        }

        [Test]
        public void Thermostat_Thresholds_Capture()
        {
            var m = new ShelterModule_Thermostat();
            m.TickHour("room_cold", 5f); // below low → heater on
            Assert.IsTrue(m.IsHeaterOn("room_cold"));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_thermostat", save.moduleId);
            Assert.AreEqual(10f, save.lowThreshold, Eps);
            var m2 = new ShelterModule_Thermostat();
            m2.RestoreState(save);
            Assert.AreEqual(10f, m2.CaptureState().lowThreshold, Eps);
            // heater runtime map not in Capture — only thresholds
            Assert.IsTrue(m2.ShouldHeaterBeOn(5f));
        }

        [Test]
        public void WasteChute_Activate_Capture()
        {
            var m = new ShelterModule_WasteChute();
            m.Activate();
            Assert.AreEqual("compost_bin", m.DepositWaste("sv_w", "organic"));
            Assert.AreEqual("incinerator", m.DepositWaste("sv_w", "plastic"));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_waste_chute", save.moduleId);
            Assert.IsTrue(save.isActive);
            var m2 = new ShelterModule_WasteChute();
            m2.RestoreState(save);
            Assert.IsTrue(m2.IsActive());
        }


        [Test]
        public void Autopsy_Perform_Capture()
        {
            var m = new ShelterModule_Autopsy();
            var st = m.CaptureState(); st.isBuilt = true; m.RestoreState(st);
            Assert.IsTrue(m.PerformAutopsy("doc_1", "corpse_a", new System.Random(1), out _));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_autopsy", save.moduleId);
            Assert.IsTrue(save.isBuilt);
            var m2 = new ShelterModule_Autopsy();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isBuilt);
        }

        [Test]
        public void BatteryBank_StoreSilent_Capture()
        {
            var m = new ShelterModule_BatteryBank();
            var st = m.CaptureState(); st.isBuilt = true; m.RestoreState(st);
            m.StoreExcessWattage(100f, 2f);
            Assert.IsTrue(m.ToggleSilentRunningMode(true));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_battery_bank", save.moduleId);
            Assert.AreEqual(200f, save.storedWattHours, Eps);
            Assert.IsTrue(save.isSilentRunningActive);
            var m2 = new ShelterModule_BatteryBank();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isSilentRunningActive);
            Assert.AreEqual(200f, m2.CaptureState().storedWattHours, Eps);
        }

        [Test]
        public void BioLatrine_Fertilizer_Capture()
        {
            var m = new ShelterModule_BioLatrine();
            var st = m.CaptureState(); st.isBuilt = true; st.daysSinceLastFertilizer = 9; m.RestoreState(st);
            Assert.AreEqual(10, m.TickDaily(hasSawdustOrAsh: true));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_bio_latrine", save.moduleId);
            Assert.AreEqual(0, save.daysSinceLastFertilizer);
            var m2 = new ShelterModule_BioLatrine();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isBuilt);
        }

        [Test]
        public void ChoreBoard_Buff_Capture()
        {
            var m = new ShelterModule_ChoreBoard();
            var st = m.CaptureState(); st.isBuilt = true; m.RestoreState(st);
            Assert.AreEqual(1.05f, m.GetUtilityAISpeedMultiplier(), Eps);
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_chore_board", save.moduleId);
            Assert.IsTrue(save.isBuilt);
            var m2 = new ShelterModule_ChoreBoard();
            m2.RestoreState(save);
            Assert.AreEqual(1.05f, m2.GetUtilityAISpeedMultiplier(), Eps);
        }

        [Test]
        public void DeadManSwitch_ArmTrigger_Capture()
        {
            var m = new ShelterModule_DeadManSwitch();
            m.Arm("op_dead");
            Assert.IsTrue(m.CheckTrigger(isBunkerBreached: true, isOperatorDead: true));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_dead_man_switch", save.moduleId);
            Assert.IsTrue(save.isArmed);
            Assert.IsTrue(save.isTriggered);
            var m2 = new ShelterModule_DeadManSwitch();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isTriggered);
            Assert.AreEqual("op_dead", m2.CaptureState().operatorSurvivorId);
        }

        [Test]
        public void DeconShower_Use_Capture()
        {
            var m = new ShelterModule_DeconShower();
            var st = m.CaptureState(); st.isBuilt = true; m.RestoreState(st);
            float contam = 80f; int water = 50;
            Assert.IsTrue(m.UseDeconShower("sv_d", ref contam, ref water));
            Assert.AreEqual(0f, contam, Eps);
            Assert.AreEqual(40, water);
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_decon_shower", save.moduleId);
            var m2 = new ShelterModule_DeconShower();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isBuilt);
        }

        [Test]
        public void Dialysis_StartTreatment_Capture()
        {
            var m = new ShelterModule_Dialysis();
            Assert.IsTrue(m.StartTreatment("sv_kidney", cleanWaterAvailable: 500));
            Assert.IsTrue(m.IsTreating());
            Assert.AreEqual("sv_kidney", m.GetPatientId());
            var save = m.CaptureState();
            Assert.AreEqual("dialysis", save.moduleId);
            Assert.IsTrue(save.isTreating);
            var m2 = new ShelterModule_Dialysis();
            m2.RestoreState(save);
            Assert.IsTrue(m2.IsTreating());
            Assert.AreEqual("sv_kidney", m2.GetPatientId());
        }

        [Test]
        public void DistressBeacon_Activate_Capture()
        {
            var m = new ShelterModule_DistressBeacon();
            m.Activate();
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_distress_beacon", save.moduleId);
            Assert.IsTrue(save.isActive);
            var m2 = new ShelterModule_DistressBeacon();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isActive);
        }

        [Test]
        public void DronePad_Map_Capture()
        {
            var m = new ShelterModule_DronePad();
            var st = m.CaptureState(); st.isBuilt = true; m.RestoreState(st);
            Assert.AreEqual(5, m.DeployDroneMapping(isFalloutStormActive: false));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_drone_pad", save.moduleId);
            Assert.IsTrue(save.isBuilt);
            Assert.IsFalse(save.isDroneDestroyed);
            var m2 = new ShelterModule_DronePad();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isBuilt);
        }

        [Test]
        public void Garage_Park_Capture()
        {
            var m = new ShelterModule_Garage();
            var st = m.CaptureState(); st.isBuilt = true; m.RestoreState(st);
            Assert.IsTrue(m.ParkVehicle("veh_jeep"));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_garage", save.moduleId);
            Assert.Contains("veh_jeep", save.storedVehicleIds);
            var m2 = new ShelterModule_Garage();
            m2.RestoreState(save);
            Assert.Contains("veh_jeep", m2.CaptureState().storedVehicleIds);
        }

        [Test]
        public void GunRack_StoreIssue_Capture()
        {
            var m = new ShelterModule_GunRack();
            var st = m.CaptureState(); st.isBuilt = true; m.RestoreState(st);
            m.StoreWeaponInRack("w_rifle");
            Assert.IsTrue(m.IssueWeaponToSurvivor("w_rifle", "sv_g"));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_gun_rack", save.moduleId);
            Assert.IsFalse(save.lockedWeapons.Contains("w_rifle"));
            var m2 = new ShelterModule_GunRack();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isBuilt);
        }

        [Test]
        public void Hammock_Rest_Capture()
        {
            var m = new ShelterModule_Hammock();
            var st = m.CaptureState(); st.isBuilt = true; m.RestoreState(st);
            float rate = m.RestInHammock("sv_h", 10f);
            Assert.AreEqual(6f, rate, Eps);
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_hammock", save.moduleId);
            var m2 = new ShelterModule_Hammock();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isBuilt);
        }

        [Test]
        public void HandCrank_Crank_Capture()
        {
            var m = new ShelterModule_HandCrank();
            var st = m.CaptureState(); st.isBuilt = true; m.RestoreState(st);
            float fatigue = 0f;
            float watts = m.CrankDynamo("sv_c", 1f, ref fatigue, out _);
            Assert.Greater(watts, 0f);
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_hand_crank", save.moduleId);
            var m2 = new ShelterModule_HandCrank();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isBuilt);
        }

        [Test]
        public void HotShower_Take_Capture()
        {
            var m = new ShelterModule_HotShower();
            var st = m.CaptureState(); st.isBuilt = true; m.RestoreState(st);
            int water = 20; float hyg = 0f; float mor = 0f;
            Assert.IsTrue(m.TakeHotShower("sv_s", ref water, isHeatAvailable: true, ref hyg, ref mor));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_hot_shower", save.moduleId);
            var m2 = new ShelterModule_HotShower();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isBuilt);
        }

        [Test]
        public void Incinerator_Burn_Capture()
        {
            var m = new ShelterModule_Incinerator();
            var st = m.CaptureState(); st.isBuilt = true; m.RestoreState(st);
            float hatch = 0f; float heat = 0f;
            Assert.IsTrue(m.IncinerateItem("scrap", ref hatch, ref heat));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_incinerator", save.moduleId);
            var m2 = new ShelterModule_Incinerator();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isBuilt);
        }

        [Test]
        public void MagmaTap_Install_Capture()
        {
            var m = new MagmaTapSystem("shelter_module_magma_tap");
            Assert.IsTrue(m.Install(currentDepth: 12, hasVenting: true));
            Assert.AreEqual(1000f, m.GetPowerOutput(), Eps);
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_magma_tap", save.moduleId);
            Assert.IsTrue(save.isInstalled);
            Assert.IsTrue(save.isVented);
            var m2 = new MagmaTapSystem("other");
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isInstalled);
            Assert.AreEqual("shelter_module_magma_tap", m2.CaptureState().moduleId);
        }

        [Test]
        public void MotionSensor_Ping_Capture()
        {
            var m = new ShelterModule_MotionSensor();
            var st = m.CaptureState(); st.isBuilt = true; m.RestoreState(st);
            m.DetectThreatsWithinRadius("raid_a", 1);
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_motion_sensor", save.moduleId);
            Assert.IsTrue(save.isBuilt);
            var m2 = new ShelterModule_MotionSensor();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isBuilt);
        }

        [Test]
        public void PanicRoom_BuildLock_Capture()
        {
            var m = new PanicRoomSystem("shelter_module_panic_room");
            m.Build();
            Assert.IsTrue(m.LockOccupants(new List<string> { "sv_1", "sv_2" }));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_panic_room", save.moduleId);
            Assert.IsTrue(save.isBuilt);
            Assert.IsTrue(save.isLocked);
            Assert.Contains("sv_1", save.lockedOccupantIds);
            var m2 = new PanicRoomSystem("tmp");
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isLocked);
            Assert.Contains("sv_2", m2.CaptureState().lockedOccupantIds);
        }

        [Test]
        public void PrintingPress_BuildForge_Capture()
        {
            var m = new ShelterModule_PrintingPress();
            Assert.IsTrue(m.Build());
            var st = m.CaptureState(); st.forgeryDetectionChance = 0f; m.RestoreState(st);
            var (money, detected) = m.Forge(50, new System.Random(1));
            Assert.AreEqual(50, money);
            Assert.IsFalse(detected);
            var save = m.CaptureState();
            Assert.AreEqual("module_printing_press", save.moduleId);
            Assert.IsTrue(save.isBuilt);
            Assert.AreEqual(1, save.useCount);
            var m2 = new ShelterModule_PrintingPress();
            m2.RestoreState(save);
            Assert.AreEqual(1, m2.CaptureState().useCount);
        }

        [Test]
        public void PunchingBag_Vent_Capture()
        {
            var m = new ShelterModule_PunchingBag();
            var st = m.CaptureState(); st.isBuilt = true; m.RestoreState(st);
            float anx = 80f;
            Assert.IsTrue(m.VentAngerOnBag("sv_p", ref anx));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_punching_bag", save.moduleId);
            var m2 = new ShelterModule_PunchingBag();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isBuilt);
        }

        [Test]
        public void RainBarrel_Collect_Capture()
        {
            var m = new ShelterModule_RainBarrel();
            var st = m.CaptureState(); st.isBuilt = true; m.RestoreState(st);
            m.CollectRain(8);
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_rain_barrel", save.moduleId);
            Assert.AreEqual(8, save.currentWater);
            var m2 = new ShelterModule_RainBarrel();
            m2.RestoreState(save);
            Assert.AreEqual(8, m2.CaptureState().currentWater);
        }

        [Test]
        public void RecordPlayer_Play_Capture()
        {
            var m = new ShelterModule_RecordPlayer();
            var st = m.CaptureState(); st.isBuilt = true; m.RestoreState(st);
            Assert.IsTrue(m.StartPlaying(hasPower: true, hasVinylRecord: true));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_record_player", save.moduleId);
            Assert.IsTrue(save.isPlaying);
            var m2 = new ShelterModule_RecordPlayer();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isPlaying);
        }

        [Test]
        public void Sprinklers_Suppress_Capture()
        {
            var m = new ShelterModule_Sprinklers();
            var st = m.CaptureState(); st.isBuilt = true; m.RestoreState(st);
            int water = 100;
            Assert.IsTrue(m.TriggerFireSuppression(ref water));
            Assert.AreEqual(50, water);
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_sprinklers", save.moduleId);
            var m2 = new ShelterModule_Sprinklers();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isBuilt);
        }

        [Test]
        public void Thumper_Activate_Capture()
        {
            var m = new ThumperSystem("shelter_module_thumper");
            Assert.IsTrue(m.Activate(hasPower: true));
            Assert.IsTrue(m.IsBurrowerProtected());
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_thumper", save.moduleId);
            Assert.IsTrue(save.isActive);
            var m2 = new ThumperSystem("tmp");
            m2.RestoreState(save);
            Assert.IsTrue(m2.IsBurrowerProtected());
        }

        [Test]
        public void TreadmillGen_Man_Capture()
        {
            var m = new ShelterModule_TreadmillGen();
            var st = m.CaptureState(); st.isBuilt = true; m.RestoreState(st);
            float fat = 0f, cal = 1000f, xp = 0f;
            float watts = m.ManTreadmill("sv_t", 1f, ref fat, ref cal, ref xp);
            Assert.Greater(watts, 0f);
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_treadmill_gen", save.moduleId);
            var m2 = new ShelterModule_TreadmillGen();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isBuilt);
        }

        [Test]
        public void Turret_Raid_Capture()
        {
            var m = new ShelterModule_Turret();
            var st = m.CaptureState(); st.isBuilt = true; st.hasPower = true; m.RestoreState(st);
            int ammo = 100;
            float left = m.TriggerRaidDefense(ref ammo, 50f);
            Assert.Less(left, 50f);
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_turret", save.moduleId);
            Assert.IsTrue(save.isBuilt);
            var m2 = new ShelterModule_Turret();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isBuilt);
        }

        [Test]
        public void VaultDoor_Toggle_Capture()
        {
            var m = new ShelterModule_VaultDoor();
            Assert.IsTrue(m.ToggleDoorState(hasPower: true));
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_vault_door", save.moduleId);
            Assert.IsTrue(save.isOpen);
            Assert.IsFalse(save.isStuck);
            var m2 = new ShelterModule_VaultDoor();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isOpen);
        }

        [Test]
        public void WoodStove_Light_Capture()
        {
            var m = new ShelterModule_WoodStove();
            var st = m.CaptureState(); st.isBuilt = true; st.isAdjacentToAirVent = true; m.RestoreState(st);
            Assert.IsTrue(m.LightStove(3, out string co));
            Assert.IsNull(co);
            var save = m.CaptureState();
            Assert.AreEqual("shelter_module_wood_stove", save.moduleId);
            Assert.IsTrue(save.isLit);
            var m2 = new ShelterModule_WoodStove();
            m2.RestoreState(save);
            Assert.IsTrue(m2.CaptureState().isLit);
        }


        [Test]
        public void MultiShelterModule_SaveSlot_RoundTrip()
        {
            string dir = TempDir("multi");
            try
            {
                var acid = new ShelterModule_AcidTrap();
                acid.Refill(80f);
                acid.Arm();
                acid.Trigger(0.1f);

                var auto = new ShelterModule_Autodoc();
                auto.PerformSurgery("sv_x", "infection");

                var classRoom = new ShelterModule_Classroom();
                classRoom.Enroll("kid_a", "teach_b");

                var pit = new ShelterModule_Pitfall();
                pit.Activate();
                pit.TryKillRaider();
                pit.TryKillRaider();

                var holo = new ShelterModule_HoloEmitter();
                holo.Activate();

                var farm = new ShelterModule_InsectFarm();
                farm.TickDay("sh_1", 1f, 1f, null);

                var bank = new ShelterModule_BatteryBank();
                var bst = bank.CaptureState(); bst.isBuilt = true; bank.RestoreState(bst);
                bank.StoreExcessWattage(50f, 4f);

                var garage = new ShelterModule_Garage();
                var gst = garage.CaptureState(); gst.isBuilt = true; garage.RestoreState(gst);
                garage.ParkVehicle("veh_a");

                var magma = new MagmaTapSystem("shelter_module_magma_tap");
                magma.Install(15, true);

                var thumper = new ThumperSystem("shelter_module_thumper");
                thumper.Activate(true);

                var dialysis = new ShelterModule_Dialysis();
                dialysis.StartTreatment("sv_d", 500);

                Assert.IsTrue(MakeSave(dir, ss =>
                {
                    ss.SetShelterModuleAcidTrap(acid);
                    ss.SetShelterModuleAutodoc(auto);
                    ss.SetShelterModuleClassroom(classRoom);
                    ss.SetShelterModulePitfall(pit);
                    ss.SetShelterModuleHoloEmitter(holo);
                    ss.SetShelterModuleInsectFarm(farm);
                    ss.SetShelterModuleCctv(new ShelterModule_CCTV());
                    ss.SetShelterModuleConfessional(new ShelterModule_Confessional());
                    ss.SetShelterModuleConveyor(new ShelterModule_Conveyor());
                    ss.SetShelterModuleDaylightSensor(new ShelterModule_DaylightSensor());
                    ss.SetShelterModuleDroneStation(new ShelterModule_DroneStation());
                    ss.SetShelterModuleLathe(new ShelterModule_Lathe());
                    ss.SetShelterModuleMortar(new ShelterModule_Mortar());
                    ss.SetShelterModulePanicButton(new ShelterModule_PanicButton());
                    ss.SetShelterModuleReloader(new ShelterModule_Reloader());
                    ss.SetShelterModuleSorter(new ShelterModule_Sorter());
                    ss.SetShelterModuleThermostat(new ShelterModule_Thermostat());
                    ss.SetShelterModuleWasteChute(new ShelterModule_WasteChute());
                    ss.SetShelterModuleBatteryBank(bank);
                    ss.SetShelterModuleGarage(garage);
                    ss.SetShelterModuleMagmaTap(magma);
                    ss.SetShelterModuleThumper(thumper);
                    ss.SetShelterModuleDialysis(dialysis);
                    ss.SetShelterModuleAutopsy(new ShelterModule_Autopsy());
                    ss.SetShelterModuleBioLatrine(new ShelterModule_BioLatrine());
                    ss.SetShelterModuleChoreBoard(new ShelterModule_ChoreBoard());
                    ss.SetShelterModuleDeadManSwitch(new ShelterModule_DeadManSwitch());
                    ss.SetShelterModuleDeconShower(new ShelterModule_DeconShower());
                    ss.SetShelterModuleDistressBeacon(new ShelterModule_DistressBeacon());
                    ss.SetShelterModuleDronePad(new ShelterModule_DronePad());
                    ss.SetShelterModuleGunRack(new ShelterModule_GunRack());
                    ss.SetShelterModuleHammock(new ShelterModule_Hammock());
                    ss.SetShelterModuleHandCrank(new ShelterModule_HandCrank());
                    ss.SetShelterModuleHotShower(new ShelterModule_HotShower());
                    ss.SetShelterModuleIncinerator(new ShelterModule_Incinerator());
                    ss.SetShelterModuleMotionSensor(new ShelterModule_MotionSensor());
                    ss.SetShelterModulePanicRoom(new PanicRoomSystem("shelter_module_panic_room"));
                    ss.SetShelterModulePrintingPress(new ShelterModule_PrintingPress());
                    ss.SetShelterModulePunchingBag(new ShelterModule_PunchingBag());
                    ss.SetShelterModuleRainBarrel(new ShelterModule_RainBarrel());
                    ss.SetShelterModuleRecordPlayer(new ShelterModule_RecordPlayer());
                    ss.SetShelterModuleSprinklers(new ShelterModule_Sprinklers());
                    ss.SetShelterModuleTreadmillGen(new ShelterModule_TreadmillGen());
                    ss.SetShelterModuleTurret(new ShelterModule_Turret());
                    ss.SetShelterModuleVaultDoor(new ShelterModule_VaultDoor());
                    ss.SetShelterModuleWoodStove(new ShelterModule_WoodStove());
                }).Save("slot"));

                var acid2 = new ShelterModule_AcidTrap();
                var auto2 = new ShelterModule_Autodoc();
                var class2 = new ShelterModule_Classroom();
                var pit2 = new ShelterModule_Pitfall();
                var holo2 = new ShelterModule_HoloEmitter();
                var farm2 = new ShelterModule_InsectFarm();
                var bank2 = new ShelterModule_BatteryBank();
                var garage2 = new ShelterModule_Garage();
                var magma2 = new MagmaTapSystem("tmp");
                var thumper2 = new ThumperSystem("tmp");
                var dialysis2 = new ShelterModule_Dialysis();

                Assert.IsTrue(MakeSave(dir, ss =>
                {
                    ss.SetShelterModuleAcidTrap(acid2);
                    ss.SetShelterModuleAutodoc(auto2);
                    ss.SetShelterModuleClassroom(class2);
                    ss.SetShelterModulePitfall(pit2);
                    ss.SetShelterModuleHoloEmitter(holo2);
                    ss.SetShelterModuleInsectFarm(farm2);
                    ss.SetShelterModuleCctv(new ShelterModule_CCTV());
                    ss.SetShelterModuleConfessional(new ShelterModule_Confessional());
                    ss.SetShelterModuleConveyor(new ShelterModule_Conveyor());
                    ss.SetShelterModuleDaylightSensor(new ShelterModule_DaylightSensor());
                    ss.SetShelterModuleDroneStation(new ShelterModule_DroneStation());
                    ss.SetShelterModuleLathe(new ShelterModule_Lathe());
                    ss.SetShelterModuleMortar(new ShelterModule_Mortar());
                    ss.SetShelterModulePanicButton(new ShelterModule_PanicButton());
                    ss.SetShelterModuleReloader(new ShelterModule_Reloader());
                    ss.SetShelterModuleSorter(new ShelterModule_Sorter());
                    ss.SetShelterModuleThermostat(new ShelterModule_Thermostat());
                    ss.SetShelterModuleWasteChute(new ShelterModule_WasteChute());
                    ss.SetShelterModuleBatteryBank(bank2);
                    ss.SetShelterModuleGarage(garage2);
                    ss.SetShelterModuleMagmaTap(magma2);
                    ss.SetShelterModuleThumper(thumper2);
                    ss.SetShelterModuleDialysis(dialysis2);
                    ss.SetShelterModuleAutopsy(new ShelterModule_Autopsy());
                    ss.SetShelterModuleBioLatrine(new ShelterModule_BioLatrine());
                    ss.SetShelterModuleChoreBoard(new ShelterModule_ChoreBoard());
                    ss.SetShelterModuleDeadManSwitch(new ShelterModule_DeadManSwitch());
                    ss.SetShelterModuleDeconShower(new ShelterModule_DeconShower());
                    ss.SetShelterModuleDistressBeacon(new ShelterModule_DistressBeacon());
                    ss.SetShelterModuleDronePad(new ShelterModule_DronePad());
                    ss.SetShelterModuleGunRack(new ShelterModule_GunRack());
                    ss.SetShelterModuleHammock(new ShelterModule_Hammock());
                    ss.SetShelterModuleHandCrank(new ShelterModule_HandCrank());
                    ss.SetShelterModuleHotShower(new ShelterModule_HotShower());
                    ss.SetShelterModuleIncinerator(new ShelterModule_Incinerator());
                    ss.SetShelterModuleMotionSensor(new ShelterModule_MotionSensor());
                    ss.SetShelterModulePanicRoom(new PanicRoomSystem("tmp"));
                    ss.SetShelterModulePrintingPress(new ShelterModule_PrintingPress());
                    ss.SetShelterModulePunchingBag(new ShelterModule_PunchingBag());
                    ss.SetShelterModuleRainBarrel(new ShelterModule_RainBarrel());
                    ss.SetShelterModuleRecordPlayer(new ShelterModule_RecordPlayer());
                    ss.SetShelterModuleSprinklers(new ShelterModule_Sprinklers());
                    ss.SetShelterModuleTreadmillGen(new ShelterModule_TreadmillGen());
                    ss.SetShelterModuleTurret(new ShelterModule_Turret());
                    ss.SetShelterModuleVaultDoor(new ShelterModule_VaultDoor());
                    ss.SetShelterModuleWoodStove(new ShelterModule_WoodStove());
                }).Load("slot"));

                Assert.AreEqual(1, acid2.CaptureState().triggeredCount);
                Assert.AreEqual(55f, acid2.CaptureState().acidReserve, Eps);
                Assert.Contains("sv_x", auto2.CaptureState().treatedPatientIds);
                Assert.IsTrue(class2.IsEnrolled("kid_a"));
                Assert.AreEqual(2, pit2.RaidersKilled);
                Assert.IsTrue(holo2.IsActive());
                Assert.AreEqual(5f, farm2.GetTotalProteinHarvested(), Eps);
                Assert.AreEqual(200f, bank2.CaptureState().storedWattHours, Eps);
                Assert.Contains("veh_a", garage2.CaptureState().storedVehicleIds);
                Assert.IsTrue(magma2.CaptureState().isInstalled);
                Assert.IsTrue(thumper2.IsBurrowerProtected());
                Assert.IsTrue(dialysis2.IsTreating());
                Assert.AreEqual("sv_d", dialysis2.GetPatientId());
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }
    }
}
