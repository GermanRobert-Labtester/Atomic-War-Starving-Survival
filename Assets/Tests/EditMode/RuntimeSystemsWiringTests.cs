using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Post–Core-families audit: runtime systems that had Capture/Restore but no
    /// SaveSystem.RegisterSystem slot (Cooking / Bicycle / WaterEconomy / BlackRain).
    /// Temperature + Radiation use legacy SaveData paths (ElapsedHours / survivor DTOs).
    /// </summary>
    [TestFixture]
    public class RuntimeSystemsWiringTests
    {
        private static string TempDir(string tag)
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_runtime_" + tag + "_" + Guid.NewGuid().ToString("N"));
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

        private static bool HasSaveable(SaveSystem ss, string saveId)
        {
            var field = typeof(SaveSystem).GetField("_saveables", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, "SaveSystem._saveables field missing");
            var list = field.GetValue(ss) as System.Collections.IList;
            Assert.IsNotNull(list);
            foreach (var item in list)
            {
                var prop = item.GetType().GetProperty("SaveId");
                if (prop != null && string.Equals(prop.GetValue(item) as string, saveId, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }

        [Test]
        public void Cooking_CaptureRestore_And_Register()
        {
            var inv = new InventoryClass();
            var cooking = new CookingSystem(inv);
            var save = cooking.CaptureState();
            Assert.IsNotNull(save);
            Assert.AreEqual("cooking_system", save.systemId);
            cooking.RestoreState(save);
            Assert.IsNotNull(cooking.CaptureState());

            string dir = TempDir("cooking");
            try
            {
                var ss = MakeSave(dir, s => s.SetCookingSystem(cooking));
                Assert.IsTrue(HasSaveable(ss, "cooking"), "cooking save slot missing");
            }
            finally { if (Directory.Exists(dir)) Directory.Delete(dir, true); }
        }

        [Test]
        public void Bicycle_CaptureRestore_And_Register()
        {
            var bike = new BicycleSystem();
            var save = bike.CaptureState();
            Assert.IsNotNull(save);
            Assert.AreEqual("bicycle_system", save.systemId);
            bike.RestoreState(save);

            string dir = TempDir("bicycle");
            try
            {
                var ss = MakeSave(dir, s => s.SetBicycleSystem(bike));
                Assert.IsTrue(HasSaveable(ss, "bicycle"), "bicycle save slot missing");
            }
            finally { if (Directory.Exists(dir)) Directory.Delete(dir, true); }
        }

        [Test]
        public void WaterEconomy_CaptureRestore_And_Register()
        {
            var we = new WaterEconomySystem();
            var save = we.CaptureState();
            Assert.IsNotNull(save);
            Assert.AreEqual("water_economy_system", save.systemId);
            we.RestoreState(save);

            string dir = TempDir("water");
            try
            {
                var ss = MakeSave(dir, s => s.SetWaterEconomySystem(we));
                Assert.IsTrue(HasSaveable(ss, "water_economy"), "water_economy save slot missing");
            }
            finally { if (Directory.Exists(dir)) Directory.Delete(dir, true); }
        }

        [Test]
        public void BlackRain_CaptureRestore_And_Register()
        {
            var weather = new WeatherSystem(null, 3);
            var br = new BlackRainHazardSystem(weather);
            var save = br.CaptureState();
            Assert.IsNotNull(save);
            Assert.AreEqual("black_rain_hazard_system", save.systemId);
            br.RestoreState(save);

            string dir = TempDir("blackrain");
            try
            {
                var ss = MakeSave(dir, s => s.SetBlackRainHazardSystem(br));
                Assert.IsTrue(HasSaveable(ss, "black_rain"), "black_rain save slot missing");
            }
            finally { if (Directory.Exists(dir)) Directory.Delete(dir, true); }
        }

        [Test]
        public void Temperature_And_Radiation_Use_Legacy_SavePaths()
        {
            // Temperature: SaveSystem.Capture/Restore uses ElapsedHours + SetElapsedHours (CoreDeps).
            // Radiation: dose/resistance live on Survivor DTOs; dosimeters rebuild on Register.
            var weather = new WeatherSystem(null, 3);
            var temp = new TemperatureSystem(null, weather);
            Assert.AreEqual(0f, temp.TotalElapsedHours, 1e-3f);
            temp.SetElapsedHours(24f); // no-op in legacy/manual mode (null season profile)
            Assert.AreEqual(0f, temp.TotalElapsedHours, 1e-3f);

            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);
            var rad = new RadiationSystem(needs);
            Assert.IsFalse(rad.IsPaused);
            rad.IsPaused = true;
            Assert.IsTrue(rad.IsPaused);
        }

        [Test]
        public void ClothingSystem_SetInjectsWithoutCrash()
        {
            var clothing = new ClothingDegradationSystem();
            string dir = TempDir("clothing");
            try
            {
                var ss = MakeSave(dir, s => s.SetClothingSystem(clothing));
                Assert.IsNotNull(ss);
            }
            finally { if (Directory.Exists(dir)) Directory.Delete(dir, true); }
        }
    }
}
