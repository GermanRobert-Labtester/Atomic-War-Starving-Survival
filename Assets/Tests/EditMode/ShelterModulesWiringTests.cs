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
    /// ShelterModule_* wiring (CaptureState subset): 18 modules — API smoke + Capture/Restore + save slots.
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
                }).Save("slot"));

                var acid2 = new ShelterModule_AcidTrap();
                var auto2 = new ShelterModule_Autodoc();
                var class2 = new ShelterModule_Classroom();
                var pit2 = new ShelterModule_Pitfall();
                var holo2 = new ShelterModule_HoloEmitter();
                var farm2 = new ShelterModule_InsectFarm();

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
                }).Load("slot"));

                Assert.AreEqual(1, acid2.CaptureState().triggeredCount);
                Assert.AreEqual(55f, acid2.CaptureState().acidReserve, Eps);
                Assert.Contains("sv_x", auto2.CaptureState().treatedPatientIds);
                Assert.IsTrue(class2.IsEnrolled("kid_a"));
                Assert.AreEqual(2, pit2.RaidersKilled);
                Assert.IsTrue(holo2.IsActive());
                Assert.AreEqual(5f, farm2.GetTotalProteinHarvested(), Eps);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }
    }
}
