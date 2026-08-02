using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    // =====================================================================
    // Balance tracker: records per-survivor stats during a simulation run
    // =====================================================================

    public class BalanceRecord
    {
        public string SurvivorId;
        public float DaysSurvived;
        public string CauseOfDeath;
        public float TotalRadiationExposure;
        public float PeakRadiation;
        public int HoursStarving;
        public int HoursDehydrated;
        public int HoursFreezing;
        public bool DevelopedChronicIllness;
        public bool DevelopedARS;
    }

    public class BalanceTracker
    {
        public readonly List<BalanceRecord> Records = new List<BalanceRecord>();
        private readonly Dictionary<string, BalanceRecord> _active = new Dictionary<string, BalanceRecord>();

        public void Register(Survivor sv)
        {
            _active[sv.Id] = new BalanceRecord
            {
                SurvivorId = sv.Id,
                DaysSurvived = 0f,
                CauseOfDeath = "alive",
                TotalRadiationExposure = sv.LifetimeRadiationExposure
            };
        }

        public void Tick(float gameHours, float currentDay, IReadOnlyList<Survivor> survivors)
        {
            foreach (var sv in survivors)
            {
                if (!_active.TryGetValue(sv.Id, out var rec)) continue;

                rec.TotalRadiationExposure = sv.LifetimeRadiationExposure;
                rec.PeakRadiation = Mathf.Max(rec.PeakRadiation, sv.RadiationDose);

                if (sv.Needs.Hunger >= 100f) rec.HoursStarving++;
                if (sv.Needs.Thirst >= 100f) rec.HoursDehydrated++;
                if (sv.Needs.Warmth <= 10f) rec.HoursFreezing++;

                if (sv.HasChronicIllness) rec.DevelopedChronicIllness = true;
                if (sv.HasAcuteRadiationSickness) rec.DevelopedARS = true;

                if (sv.State == SurvivorState.Dead && rec.CauseOfDeath == "alive")
                {
                    rec.DaysSurvived = currentDay;
                    rec.CauseOfDeath = DetermineCause(sv);
                    Records.Add(rec);
                }
            }
        }

        public void FinalizeAlive(float finalDay, IReadOnlyList<Survivor> survivors)
        {
            foreach (var sv in survivors)
            {
                if (!_active.TryGetValue(sv.Id, out var rec)) continue;
                if (rec.CauseOfDeath == "alive")
                {
                    rec.DaysSurvived = finalDay;
                    Records.Add(rec);
                }
            }
        }

        public void LogReport()
        {
            Debug.Log("=== BALANCE REPORT ===");
            Debug.Log($"{"ID",-20} {"Days",6} {"Cause",-18} {"AvgRad",8} {"PeakRad",8} {"Starv",6} {"Dehyd",6} {"Freez",6} {"CI",4} {"ARS",5}");
            foreach (var r in Records)
            {
                float avgRad = r.DaysSurvived > 0 ? r.TotalRadiationExposure / (r.DaysSurvived * 24f) : 0f;
                Debug.Log($"{r.SurvivorId,-20} {r.DaysSurvived,6:F1} {r.CauseOfDeath,-18} {avgRad,8:F2} {r.PeakRadiation,8:F1} {r.HoursStarving,6} {r.HoursDehydrated,6} {r.HoursFreezing,6} {r.DevelopedChronicIllness,4} {r.DevelopedARS,5}");
            }
            Debug.Log("======================");
        }

        private static string DetermineCause(Survivor sv)
        {
            if (sv.Needs.Health <= 0f)
            {
                if (sv.Needs.Hunger >= 100f && sv.Needs.Thirst >= 100f) return "starvation+dehydration";
                if (sv.Needs.Hunger >= 100f) return "starvation";
                if (sv.Needs.Thirst >= 100f) return "dehydration";
                if (sv.Needs.Warmth <= 10f) return "hypothermia";
                if (sv.HasAcuteRadiationSickness) return "acute_radiation";
                return "health_depleted";
            }
            if (sv.RadiationDose >= 100f) return "radiation_overdose";
            return "unknown";
        }
    }

    // =====================================================================
    // Smoke test: 30-day headless simulation
    // =====================================================================

    [TestFixture]
    public class SurvivalSmokeTest
    {
        private const float Eps = 1e-3f;
        private const float HoursPerDay = 24f;
        private const int SimDays = 30;
        private const float TotalHours = SimDays * HoursPerDay;

        private NeedsProfile _profile;
        private SeasonProfile _seasonProfile;
        private List<Survivor> _survivors;
        private List<SurvivorAction> _actions;
        private NeedsSystem _needsSystem;
        private RadiationSystem _radSystem;
        private WeatherSystem _weatherSystem;
        private TemperatureSystem _tempSystem;
        private Shelter.Shelter _shelter;
        private UtilityAI _ai;
        private EventRunner _eventRunner;
        private GameState _gameState;
        private Inventory _inventory;
        private Random _rng;
        private BalanceTracker _tracker;

        [SetUp]
        public void SetUp()
        {
            // NeedsProfile: moderate rates that guarantee death by ~day 2 for
            // unprotected survivors, survival for the sheltered one.
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
                    clearWeight = 1f, ashfallWeight = 2f, falloutStormWeight = 1f, blizzardWeight = 1f
                }
            };
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_profile);
            Object.DestroyImmediate(_seasonProfile);
            foreach (var a in _actions)
                if (a != null) Object.DestroyImmediate(a);
        }

        private void InitSystems(int seed)
        {
            _rng = new Random(seed);
            _gameState = new GameState { Phase = GamePhase.Running, Day = 1 };

            _weatherSystem = new WeatherSystem(_seasonProfile, seed);
            _tempSystem = new TemperatureSystem(_seasonProfile, _weatherSystem);
            _needsSystem = new NeedsSystem(_profile, sv => true); // always near heat
            _radSystem = new RadiationSystem(_needsSystem);

            _shelter = new Shelter.Shelter();
            _shelter.AddModule(new ShelterModuleInstance("air_filtration", 2) { FilterHealth = 100f });
            _shelter.AddModule(new ShelterModuleInstance("radiation_shielding", 3));
            _shelter.AddModule(new ShelterModuleInstance("heater", 2) { Fuel = 500f });

            _inventory = new Inventory { Capacity = 50, MaxWeight = 200f };

            // Seed inventory with supplies
            var food = ScriptableObject.CreateInstance<ItemDefinition>();
            food.id = "canned_beans"; food.hungerRestore = 25f; food.stackMax = 20; food.weight = 0.5f;
            var water = ScriptableObject.CreateInstance<ItemDefinition>();
            water.id = "clean_water"; water.thirstRestore = 40f; water.stackMax = 20; water.weight = 1f;
            var iodine = ScriptableObject.CreateInstance<ItemDefinition>();
            iodine.id = "iodine_pills"; iodine.type = ItemType.Iodine; iodine.stackMax = 20; iodine.weight = 0.1f;

            _inventory.Add(food, 30);
            _inventory.Add(water, 30);
            _inventory.Add(iodine, 10);

            // AI actions
            _actions = new List<SurvivorAction>
            {
                ScriptableObject.CreateInstance<EatActionSO>(),
                ScriptableObject.CreateInstance<DrinkActionSO>(),
                ScriptableObject.CreateInstance<SleepActionSO>(),
                ScriptableObject.CreateInstance<RestActionSO>(),
                ScriptableObject.CreateInstance<WarmUpActionSO>(),
                ScriptableObject.CreateInstance<TakeIodineActionSO>()
            };

            _ai = new UtilityAI();
            _eventRunner = new EventRunner();
            _tracker = new BalanceTracker();
        }

        private void CreateSurvivors()
        {
            _survivors = new List<Survivor>();

            // Survivor 1: "Doomed" - high starting radiation, will develop
            // chronic illness and die from combined health loss.
            // Rad: 8/hr → ARS at hour 10 (dose 80), chronic at hour 50 (lifetime 400).
            // Health: dies ~hour 50 from hunger+thirst+ARS health drain.
            var doomed = new Survivor
            {
                Id = "sv_doomed",
                DisplayName = "Doomed Scavenger",
                RadiationDose = 0f,
                LifetimeRadiationExposure = 0f,
                State = SurvivorState.Idle
            };
            _survivors.Add(doomed);

            // Survivor 2: "Sickly" - starts with high rad, will get ARS quickly,
            // dies from radiation-accelerated health loss.
            // Rad: 20/hr → ARS at hour 4 (dose 80), dies ~hour 10.
            var sickly = new Survivor
            {
                Id = "sv_sickly",
                DisplayName = "Sickly Refugee",
                RadiationDose = 0f,
                LifetimeRadiationExposure = 0f,
                State = SurvivorState.Idle
            };
            _survivors.Add(sickly);

            // Survivor 3: "Survivor" - low rad exposure, will live through 30 days.
            // Rad: 0.5/hr → lifetime 360 over 30 days (below chronic threshold 400).
            // Health loss from hunger/thirst starts at hour 50 but rate is manageable.
            var survivor = new Survivor
            {
                Id = "sv_survivor",
                DisplayName = "Steady Survivor",
                RadiationDose = 0f,
                LifetimeRadiationExposure = 0f,
                State = SurvivorState.Idle
            };
            _survivors.Add(survivor);

            foreach (var sv in _survivors)
            {
                _needsSystem.Register(sv);
                _radSystem.Register(sv);
                _tracker.Register(sv);
            }
        }

        private float GetRadRate(Survivor sv)
        {
            // Doom: 8/hr, Sickly: 20/hr, Survivor: 0.5/hr
            if (sv.Id == "sv_doomed") return 8f;
            if (sv.Id == "sv_sickly") return 20f;
            return 0.5f;
        }

        private void SimulateHour(float hour, float day)
        {
            float gameHours = 1f;

            // Tick weather & temperature
            _weatherSystem.Tick(gameHours);
            _tempSystem.Tick(gameHours);
            _gameState.Day = Mathf.FloorToInt(day);

            // Tick shelter modules
            _shelter.Tick(gameHours);

            // Apply radiation to each survivor
            foreach (var sv in _survivors)
            {
                if (!sv.IsAlive) continue;
                float radRate = GetRadRate(sv);
                _radSystem.Expose(sv, radRate, gameHours);
            }

            // Tick needs
            _needsSystem.Tick(gameHours);

            // AI evaluation
            foreach (var sv in _survivors)
            {
                if (!sv.IsAlive) continue;
                var context = new AIContext(sv, _shelter, _inventory, _rng)
                {
                    IsFalloutStorm = _weatherSystem.Current == WeatherKind.FalloutStorm,
                    AmbientRadRate = GetRadRate(sv),
                    IsRadiationRising = GetRadRate(sv) > 2f
                };
                var action = _ai.SelectAction(context, _actions);
                action?.Execute(context);
            }

            // Track balance
            _tracker.Tick(gameHours, day, _survivors);
        }

        // -----------------------------------------------------------------
        // Main smoke test
        // -----------------------------------------------------------------

        [TestCase(1)]
        [TestCase(42)]
        [TestCase(1337)]
        public void Simulate30Days_NoNullRef_NoNaN(int seed)
        {
            InitSystems(seed);
            CreateSurvivors();

            int nullRefCount = 0;
            int nanCount = 0;

            for (float hour = 0; hour < TotalHours; hour++)
            {
                float day = hour / HoursPerDay + 1f;

                try
                {
                    SimulateHour(hour, day);
                }
                catch (NullReferenceException)
                {
                    nullRefCount++;
                }
                catch (Exception ex) when (ex is not AssertionException)
                {
                    nullRefCount++;
                }

                // Check for NaN in needs
                foreach (var sv in _survivors)
                {
                    if (float.IsNaN(sv.Needs.Hunger) || float.IsNaN(sv.Needs.Thirst) ||
                        float.IsNaN(sv.Needs.Health) || float.IsNaN(sv.Needs.Warmth) ||
                        float.IsNaN(sv.RadiationDose) || float.IsNaN(sv.LifetimeRadiationExposure))
                    {
                        nanCount++;
                    }
                }
            }

            _tracker.FinalizeAlive(SimDays, _survivors);
            _tracker.LogReport();

            Assert.AreEqual(0, nullRefCount, $"Null refs during simulation: {nullRefCount}");
            Assert.AreEqual(0, nanCount, $"NaN values detected during simulation: {nanCount}");
        }

        // -----------------------------------------------------------------
        // Chronic illness + death guarantees
        // -----------------------------------------------------------------

        [TestCase(1)]
        [TestCase(42)]
        [TestCase(1337)]
        public void Simulate30Days_AtLeastOneChronicIllnessAndOneDeath(int seed)
        {
            InitSystems(seed);
            CreateSurvivors();

            for (float hour = 0; hour < TotalHours; hour++)
            {
                float day = hour / HoursPerDay + 1f;
                SimulateHour(hour, day);
            }

            _tracker.FinalizeAlive(SimDays, _survivors);

            // At least one chronic illness
            bool anyChronic = false;
            foreach (var r in _tracker.Records)
                if (r.DevelopedChronicIllness) anyChronic = true;
            Assert.IsTrue(anyChronic, "Expected at least one survivor to develop chronic illness");

            // At least one death
            bool anyDeath = false;
            foreach (var r in _tracker.Records)
                if (r.CauseOfDeath != "alive") anyDeath = true;
            Assert.IsTrue(anyDeath, "Expected at least one survivor to die");

            // Survivor 3 should still be alive
            var survivorRec = _tracker.Records.Find(r => r.SurvivorId == "sv_survivor");
            Assert.IsNotNull(survivorRec);
            Assert.AreEqual("alive", survivorRec.CauseOfDeath, "Steady Survivor should survive 30 days");
        }

        // -----------------------------------------------------------------
        // Save/load mid-run
        // -----------------------------------------------------------------

        [TestCase(1)]
        [TestCase(42)]
        [TestCase(1337)]
        public void SaveLoadMidRun_PreservesState(int seed)
        {
            InitSystems(seed);
            CreateSurvivors();

            // Simulate 15 days
            float midHours = 15f * HoursPerDay;
            for (float hour = 0; hour < midHours; hour++)
            {
                float day = hour / HoursPerDay + 1f;
                SimulateHour(hour, day);
            }

            // Capture mid-run state
            float doomedDose = _survivors[0].RadiationDose;
            float sicklyDose = _survivors[1].RadiationDose;
            float survivorHunger = _survivors[2].Needs.Hunger;
            WeatherKind weather = _weatherSystem.Current;

            // Save
            string testDir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "ashfall_smoke_" + seed);
            var saveSys = new SaveSystem(
                _gameState, _weatherSystem, _tempSystem, _needsSystem,
                _radSystem, _shelter, () => _survivors,
                id => null, id => null, testDir);
            Assert.IsTrue(saveSys.Save("midrun"));

            // Build fresh systems and load
            InitSystems(seed);
            CreateSurvivors();
            var loadSys = new SaveSystem(
                _gameState, _weatherSystem, _tempSystem, _needsSystem,
                _radSystem, _shelter, () => _survivors,
                id => null, id => null, testDir);
            Assert.IsTrue(loadSys.Load("midrun"));

            // Assert restored values
            Assert.AreEqual(doomedDose, _survivors[0].RadiationDose, 0.1f,
                "Doomed survivor rad dose should survive save/load");
            Assert.AreEqual(sicklyDose, _survivors[1].RadiationDose, 0.1f,
                "Sickly survivor rad dose should survive save/load");
            Assert.AreEqual(survivorHunger, _survivors[2].Needs.Hunger, 0.1f,
                "Survivor hunger should survive save/load");
            Assert.AreEqual(weather, _weatherSystem.Current,
                "Weather state should survive save/load");

            // Cleanup
            System.IO.Directory.Delete(testDir, true);
        }
    }
}
