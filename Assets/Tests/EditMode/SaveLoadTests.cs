using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Round-trip save/load test: set a nasty mid-storm state (rad 73, filter 12%,
    /// chronic-illness survivor), save, reload into fresh systems, assert every
    /// value matches within float epsilon.
    /// </summary>
    [TestFixture]
    public class SaveLoadTests
    {
        private const float Eps = 1e-3f;

        private string _testDir;
        private NeedsProfile _profile;
        private SeasonProfile _seasonProfile;

        [SetUp]
        public void SetUp()
        {
            _testDir = Path.Combine(Path.GetTempPath(), "ashfall_save_test_" + Path.GetRandomFileName());
            Directory.CreateDirectory(_testDir);

            _profile = ScriptableObject.CreateInstance<NeedsProfile>();
            _profile.hungerPerHour = 2f;
            _profile.thirstPerHour = 3f;
            _profile.fatiguePerHour = 1.5f;
            _profile.warmthLossPerHourInCold = 4f;
            _profile.warmthRestorePerHourNearHeat = 6f;
            _profile.hungerCritical = 100f;
            _profile.thirstCritical = 100f;
            _profile.warmthCritical = 10f;
            _profile.healthLossFromHunger = 3f;
            _profile.healthLossFromThirst = 4f;
            _profile.healthLossFromCold = 2f;
            _profile.moraleLossPerHourWhileCritical = 1f;

            _seasonProfile = ScriptableObject.CreateInstance<SeasonProfile>();
            _seasonProfile.campaignLengthDays = 90;
            _seasonProfile.ambientTemperatureCurve = AnimationCurve.Linear(0f, 5f, 1f, -35f);
            _seasonProfile.weatherCheckIntervalHours = 6f;
            _seasonProfile.seasons = new[]
            {
                new SeasonWindow
                {
                    id = "nuclear_winter", displayName = "Nuclear Winter", startDay = 0,
                    clearWeight = 1f, ashfallWeight = 2f, falloutStormWeight = 2f, blizzardWeight = 1f
                }
            };
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_profile);
            Object.DestroyImmediate(_seasonProfile);
            if (Directory.Exists(_testDir))
                Directory.Delete(_testDir, true);
        }

        // -----------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------

        private static Survivor MakeSurvivor(string id, string name, bool chronicIllness = false, float dose = 0f, float lifetime = 0f)
        {
            return new Survivor
            {
                Id = id,
                DisplayName = name,
                RadiationDose = dose,
                LifetimeRadiationExposure = lifetime,
                HasChronicIllness = chronicIllness
            };
        }

        private static ShelterModuleInstance MakeModule(string id, int level, float filterHealth, float fuel)
        {
            return new ShelterModuleInstance(id, level)
            {
                FilterHealth = filterHealth,
                Fuel = fuel
            };
        }

        private ItemDefinition MakeItem(string id)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.stackMax = 10;
            item.weight = 1f;
            return item;
        }

        /// <summary>
        /// Build a "nasty" mid-storm state: fallout storm active, day 15,
        /// ambient -20C, rad 73 on survivor, filter at 12%, chronic illness,
        /// items in inventory, world flags set.
        /// </summary>
        private (GameState gs, WeatherSystem ws, TemperatureSystem ts, NeedsSystem ns,
                 RadiationSystem rs, Shelter shelter, Survivor survivor,
                 Dictionary<string, ItemDefinition> items)
            BuildNastyState()
        {
            var gs = new GameState { Phase = GamePhase.Running, Day = 15 };

            var ws = new WeatherSystem(_seasonProfile, seed: 42);
            ws.ForceWeather(WeatherKind.FalloutStorm);

            var ts = new TemperatureSystem(_seasonProfile, ws);
            ts.SetElapsedHours(15f * 24f); // day 15

            var ns = new NeedsSystem(_profile);
            var rs = new RadiationSystem(ns);

            var survivor = MakeSurvivor("sv_elena", "Elena Vasquez",
                chronicIllness: true, dose: 73f, lifetime: 420f);
            survivor.State = SurvivorState.Sick;
            survivor.Needs.Hunger = 68f;
            survivor.Needs.Thirst = 55f;
            survivor.Needs.Fatigue = 80f;
            survivor.Needs.Warmth = 25f;
            survivor.Needs.Morale = 30f;
            survivor.Needs.Health = 45f;
            survivor.Needs.WasHungerCritical = true;
            survivor.Needs.WasWarmthCritical = true;
            survivor.HasAcuteRadiationSickness = true;
            survivor.HasRadResistance = true;
            survivor.RadResistanceHoursRemaining = 2.5f;
            survivor.HasFullSuitEquipped = true;

            ns.Register(survivor);
            rs.Register(survivor);

            // Set up dosimeter
            var dos = rs.GetDosimeter(survivor.Id);
            dos.CurrentRate = 8.5f;
            dos.RecentExposure = 42f;
            dos.LifetimeDose = survivor.LifetimeRadiationExposure;

            // Shelter: air filter at 12%, shielding level 3, heater with fuel
            var shelter = new Shelter();
            shelter.AddModule(MakeModule("air_filtration", level: 2, filterHealth: 12f, fuel: 0f));
            shelter.AddModule(MakeModule("radiation_shielding", level: 3, filterHealth: 100f, fuel: 0f));
            shelter.AddModule(MakeModule("heater", level: 2, filterHealth: 100f, fuel: 45f));

            // Items
            var items = new Dictionary<string, ItemDefinition>();
            var beans = MakeItem("canned_beans");
            var water = MakeItem("clean_water");
            var iodine = MakeItem("iodine_pills");
            items[beans.id] = beans;
            items[water.id] = water;
            items[iodine.id] = iodine;

            return (gs, ws, ts, ns, rs, shelter, survivor, items);
        }

        private SaveSystem MakeSaveSystem(
            GameState gs, WeatherSystem ws, TemperatureSystem ts,
            NeedsSystem ns, RadiationSystem rs, Shelter shelter,
            Survivor survivor, Dictionary<string, ItemDefinition> items)
        {
            var survivorsList = new List<Survivor> { survivor };
            return new SaveSystem(
                gs, ws, ts, ns, rs, shelter,
                () => survivorsList,
                id => items.TryGetValue(id, out var item) ? item : null,
                id => null,
                _testDir);
        }

        // -----------------------------------------------------------------
        // Test: full round-trip
        // -----------------------------------------------------------------

        [Test]
        public void SaveLoad_RoundTripsNastyMidStormState()
        {
            // Arrange: build nasty state
            var (gs, ws, ts, ns, rs, shelter, survivor, items) = BuildNastyState();
            var saveSys = MakeSaveSystem(gs, ws, ts, ns, rs, shelter, survivor, items);
            saveSys.SetWorldFlag("stranger_sheltered", true);
            saveSys.SetWorldFlag("filter_breach_active", true);

            // Act: save
            Assert.IsTrue(saveSys.Save("test_slot"));

            // Verify file exists
            Assert.IsTrue(saveSys.SlotExists("test_slot"));
            Assert.Contains("test_slot", saveSys.ListSlots());

            // Arrange: build fresh systems for load
            var (gs2, ws2, ts2, ns2, rs2, shelter2, survivor2, items2) = BuildNastyState();
            // Reset fresh systems to clean defaults
            survivor2.Needs.Hunger = 0f;
            survivor2.Needs.Thirst = 0f;
            survivor2.Needs.Health = 100f;
            survivor2.RadiationDose = 0f;
            survivor2.LifetimeRadiationExposure = 0f;
            survivor2.HasChronicIllness = false;
            survivor2.HasAcuteRadiationSickness = false;
            survivor2.State = SurvivorState.Idle;

            var loadSys = MakeSaveSystem(gs2, ws2, ts2, ns2, rs2, shelter2, survivor2, items2);

            // Act: load
            Assert.IsTrue(loadSys.Load("test_slot"));

            // Assert: game state
            Assert.AreEqual(GamePhase.Running, gs2.Phase);
            Assert.AreEqual(15, gs2.Day);

            // Assert: weather
            Assert.AreEqual(WeatherKind.FalloutStorm, ws2.Current);
            Assert.AreEqual(42, ws2.Seed);

            // Assert: temperature
            Assert.AreEqual(ts.TotalElapsedHours, ts2.TotalElapsedHours, Eps);
            Assert.AreEqual(ts.AmbientCelsius, ts2.AmbientCelsius, Eps);

            // Assert: survivor needs
            Assert.AreEqual(68f, survivor2.Needs.Hunger, Eps);
            Assert.AreEqual(55f, survivor2.Needs.Thirst, Eps);
            Assert.AreEqual(80f, survivor2.Needs.Fatigue, Eps);
            Assert.AreEqual(25f, survivor2.Needs.Warmth, Eps);
            Assert.AreEqual(30f, survivor2.Needs.Morale, Eps);
            Assert.AreEqual(45f, survivor2.Needs.Health, Eps);
            Assert.IsTrue(survivor2.Needs.WasHungerCritical);
            Assert.IsTrue(survivor2.Needs.WasWarmthCritical);

            // Assert: survivor status
            Assert.AreEqual(SurvivorState.Sick, survivor2.State);
            Assert.AreEqual(73f, survivor2.RadiationDose, Eps);
            Assert.AreEqual(420f, survivor2.LifetimeRadiationExposure, Eps);
            Assert.IsTrue(survivor2.HasAcuteRadiationSickness);
            Assert.IsTrue(survivor2.HasChronicIllness);
            Assert.IsTrue(survivor2.HasRadResistance);
            Assert.AreEqual(2.5f, survivor2.RadResistanceHoursRemaining, Eps);
            Assert.IsTrue(survivor2.HasFullSuitEquipped);

            // Assert: shelter modules
            var airMod = shelter2.GetModule("air_filtration");
            Assert.IsNotNull(airMod);
            Assert.AreEqual(12f, airMod.FilterHealth, Eps);
            Assert.AreEqual(2, airMod.Level);

            var shieldMod = shelter2.GetModule("radiation_shielding");
            Assert.IsNotNull(shieldMod);
            Assert.AreEqual(3, shieldMod.Level);

            var heaterMod = shelter2.GetModule("heater");
            Assert.IsNotNull(heaterMod);
            Assert.AreEqual(45f, heaterMod.Fuel, Eps);
            Assert.AreEqual(2, heaterMod.Level);

            // Assert: dosimeter
            var dos2 = rs2.GetDosimeter("sv_elena");
            Assert.AreEqual(8.5f, dos2.CurrentRate, Eps);
            Assert.AreEqual(42f, dos2.RecentExposure, Eps);
            Assert.AreEqual(420f, dos2.LifetimeDose, Eps);

            // Assert: world flags
            Assert.IsTrue(loadSys.GetWorldFlag("stranger_sheltered"));
            Assert.IsTrue(loadSys.GetWorldFlag("filter_breach_active"));
            Assert.IsFalse(loadSys.GetWorldFlag("nonexistent_flag"));
        }

        // -----------------------------------------------------------------
        // Test: corrupt save detection
        // -----------------------------------------------------------------

        [Test]
        public void Load_DetectsCorruptSave_ReturnsFalse()
        {
            var (gs, ws, ts, ns, rs, shelter, survivor, items) = BuildNastyState();
            var saveSys = MakeSaveSystem(gs, ws, ts, ns, rs, shelter, survivor, items);
            saveSys.Save("corrupt_slot");

            // Corrupt the file
            string path = Path.Combine(_testDir, "save_corrupt_slot.json");
            string json = File.ReadAllText(path);
            json = json.Replace("Day\": 15", "Day\": 999");
            File.WriteAllText(path, json);

            // Load should fail
            var (gs2, ws2, ts2, ns2, rs2, shelter2, survivor2, items2) = BuildNastyState();
            survivor2.Needs.Hunger = 0f;
            var loadSys = MakeSaveSystem(gs2, ws2, ts2, ns2, rs2, shelter2, survivor2, items2);
            UnityEngine.TestTools.LogAssert.Expect(LogType.Error, new System.Text.RegularExpressions.Regex(".*corrupt.*"));
            Assert.IsFalse(loadSys.Load("corrupt_slot"));

            // Fresh state should be unchanged
            Assert.AreEqual(0f, survivor2.Needs.Hunger, Eps);
        }

        // -----------------------------------------------------------------
        // Test: missing slot
        // -----------------------------------------------------------------

        [Test]
        public void Load_NonexistentSlot_ReturnsFalse()
        {
            var (gs, ws, ts, ns, rs, shelter, survivor, items) = BuildNastyState();
            var saveSys = MakeSaveSystem(gs, ws, ts, ns, rs, shelter, survivor, items);
            Assert.IsFalse(saveSys.Load("ghost_slot"));
        }

        // -----------------------------------------------------------------
        // Test: idempotent re-save
        // -----------------------------------------------------------------

        [Test]
        public void Save_TwiceToSameSlot_OverwritesCleanly()
        {
            var (gs, ws, ts, ns, rs, shelter, survivor, items) = BuildNastyState();
            var saveSys = MakeSaveSystem(gs, ws, ts, ns, rs, shelter, survivor, items);

            Assert.IsTrue(saveSys.Save("overwrite_slot"));
            survivor.Needs.Hunger = 99f;
            Assert.IsTrue(saveSys.Save("overwrite_slot"));

            // Load and verify the updated value
            var (gs2, ws2, ts2, ns2, rs2, shelter2, survivor2, items2) = BuildNastyState();
            survivor2.Needs.Hunger = 0f;
            var loadSys = MakeSaveSystem(gs2, ws2, ts2, ns2, rs2, shelter2, survivor2, items2);
            Assert.IsTrue(loadSys.Load("overwrite_slot"));
            Assert.AreEqual(99f, survivor2.Needs.Hunger, Eps);
        }

        // -----------------------------------------------------------------
        // Test: delete slot
        // -----------------------------------------------------------------

        [Test]
        public void Delete_RemovesSaveFile()
        {
            var (gs, ws, ts, ns, rs, shelter, survivor, items) = BuildNastyState();
            var saveSys = MakeSaveSystem(gs, ws, ts, ns, rs, shelter, survivor, items);

            saveSys.Save("delete_me");
            Assert.IsTrue(saveSys.SlotExists("delete_me"));

            Assert.IsTrue(saveSys.Delete("delete_me"));
            Assert.IsFalse(saveSys.SlotExists("delete_me"));
            Assert.IsFalse(saveSys.Delete("delete_me")); // already gone
        }

        // -----------------------------------------------------------------
        // Test: version field present
        // -----------------------------------------------------------------

        [Test]
        public void Save_FileContainsSaveVersion()
        {
            var (gs, ws, ts, ns, rs, shelter, survivor, items) = BuildNastyState();
            var saveSys = MakeSaveSystem(gs, ws, ts, ns, rs, shelter, survivor, items);
            saveSys.Save("version_check");

            string path = Path.Combine(_testDir, "save_version_check.json");
            string json = File.ReadAllText(path);
            Assert.IsTrue(json.Contains("\"SaveVersion\": 2"), "Save file should contain SaveVersion field");
            Assert.IsTrue(json.Contains("\"Checksum\":"), "Save file should contain Checksum field");
        }
    }
}
