using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// The game clock is the one piece of state everything else is dated against:
    /// ~30 systems read <see cref="TimeSystem.CurrentDay"/> through injected day
    /// providers (medical, economy, trade, AI, world phase, the day-30 flashpoint).
    ///
    /// It was not persisted. <c>TimeSystemSave</c> carried nothing but its own id,
    /// <c>CaptureState</c> returned an empty DTO, <c>RestoreState</c> discarded its
    /// argument, and the only object registered under the "time_system" save key was
    /// a second, never-ticked <c>TimeSystem</c> instance. Loading a day-40 save put
    /// the player back on day 1 hour 0 with every day-gated system re-armed.
    ///
    /// <see cref="GameState.Day"/> was the nominal fallback and was equally dead: set
    /// to 1 by <c>Reset()</c> and never advanced, so the save file always recorded
    /// day 1 and <c>VictoryProjectManager.DaysSurvived</c> always reported 1.
    ///
    /// These tests pin the clock end-to-end: the DTO, the SaveSystem round-trip, and
    /// the GameState mirror that the victory screen reads.
    /// </summary>
    [TestFixture]
    public class GameClockPersistenceTests
    {
        private string _testDir;

        [SetUp]
        public void SetUp()
        {
            _testDir = Path.Combine(Path.GetTempPath(), "ashfall_clock_" + System.Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_testDir);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_testDir))
            {
                try { Directory.Delete(_testDir, true); } catch { /* best-effort cleanup */ }
            }
        }

        // -----------------------------------------------------------------
        // DTO level
        // -----------------------------------------------------------------

        [Test]
        public void CaptureRestore_ReproducesDayAndHour()
        {
            var source = new TimeSystem();
            source.TickHours(11 * 24f + 7f); // day 12, hour 7

            Assume.That(source.CurrentDay, Is.EqualTo(12));
            Assume.That(source.CurrentHour, Is.EqualTo(7));

            var restored = new TimeSystem();
            restored.RestoreState(source.CaptureState());

            Assert.AreEqual(source.CurrentDay, restored.CurrentDay, "Day must survive the round-trip.");
            Assert.AreEqual(source.CurrentHour, restored.CurrentHour, "Hour must survive the round-trip.");
            Assert.AreEqual(source.TotalElapsedHours, restored.TotalElapsedHours, 1e-3f);
        }

        [Test]
        public void CaptureRestore_SurvivesJsonSerialization()
        {
            // The save file goes through JsonUtility, which only serializes public
            // fields. A DTO with auto-properties would silently round-trip as zeroes,
            // so assert against the actual serialized text rather than the object.
            var source = new TimeSystem();
            source.TickHours(29 * 24f + 13.5f);

            string json = JsonUtility.ToJson(source.CaptureState());
            var revived = JsonUtility.FromJson<TimeSystemSave>(json);

            var restored = new TimeSystem();
            restored.RestoreState(revived);

            Assert.AreEqual(30, restored.CurrentDay);
            Assert.AreEqual(13, restored.CurrentHour);
            Assert.AreEqual(13.5f, restored.CurrentHourFloat, 1e-3f,
                "The fractional hour matters: it gates the day-30 flashpoint steps.");
        }

        [Test]
        public void RestoreState_WithNull_LeavesTheClockUntouched()
        {
            var clock = new TimeSystem();
            clock.TickHours(5 * 24f);

            clock.RestoreState(null);

            Assert.AreEqual(6, clock.CurrentDay,
                "A missing section in an older save must not reset the clock to day 1.");
        }

        [Test]
        public void RestoredClock_KeepsAdvancingAndFiresDayTicks()
        {
            var source = new TimeSystem();
            source.TickHours(9 * 24f + 23f); // day 10, hour 23

            var restored = new TimeSystem();
            restored.RestoreState(source.CaptureState());

            var days = new List<int>();
            restored.OnDayTick += d => days.Add(d);
            restored.TickHours(2f); // crosses into day 11

            Assert.AreEqual(new[] { 11 }, days,
                "Day ticks must resume from the restored day, not from day 2.");
            Assert.AreEqual(11, restored.CurrentDay);
        }

        // -----------------------------------------------------------------
        // SaveSystem round-trip
        // -----------------------------------------------------------------

        [Test]
        public void SaveSystem_RoundTrip_RestoresTheGameClock()
        {
            var writeClock = new TimeSystem();
            writeClock.TickHours(39 * 24f + 6f); // day 40, hour 6

            var writer = MakeSaveSystem(writeClock, out var writeState);
            writeState.Day = writeClock.CurrentDay;
            Assert.IsTrue(writer.Save("clock_slot"), "Save must succeed.");

            var readClock = new TimeSystem();
            var reader = MakeSaveSystem(readClock, out var readState);
            Assert.IsTrue(reader.Load("clock_slot"), "Load must succeed.");

            Assert.AreEqual(40, readClock.CurrentDay,
                "Loading a day-40 save must not put the player back on day 1.");
            Assert.AreEqual(6, readClock.CurrentHour);
            Assert.AreEqual(40, readState.Day, "GameState.Day must agree with the live clock.");
        }

        /// <summary>
        /// Minimal SaveSystem with a real clock wired in. Mirrors the harness in
        /// FullSaveSystem_RoundTrip_Tests; only the clock is under test here.
        /// </summary>
        private SaveSystem MakeSaveSystem(TimeSystem clock, out GameState gameState)
        {
            gameState = new GameState();
            var ws = new WeatherSystem(null, 42);
            var ts = new TemperatureSystem(null, ws);
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var ns = new NeedsSystem(profile, sv => true);
            var rs = new RadiationSystem(ns);
            var save = new SaveSystem(new SaveSystem.CoreDeps
            {
                GameState = gameState,
                WeatherSystem = ws,
                TemperatureSystem = ts,
                NeedsSystem = ns,
                RadiationSystem = rs,
                Shelter = new ShelterClass(),
                GetSurvivors = () => new List<Survivor>(),
                ItemLookup = id => null,
                ModuleLookup = id => null,
                SavesDir = _testDir
            });
            save.SetTimeSystem(clock);
            return save;
        }

        // -----------------------------------------------------------------
        // Live wiring
        // -----------------------------------------------------------------

        [Test]
        public void GameStateDay_MirrorsTheLiveClock()
        {
            var go = new GameObject("GameClockPersistenceTests");
            go.SetActive(false);
            try
            {
                var bootstrap = go.AddComponent<GameBootstrap>();
                RegistryDispatchWiringTests.InjectBootstrapFields(bootstrap);
                typeof(GameBootstrap)
                    .GetMethod("InitializeSystems", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(bootstrap, null);

                Assert.AreEqual(bootstrap.TimeSystem.CurrentDay, bootstrap.GameState.Day,
                    "GameState.Day must start in step with the clock.");

                bootstrap.TimeSystem.TickHours(3 * 24f);

                Assert.AreEqual(4, bootstrap.TimeSystem.CurrentDay);
                Assert.AreEqual(4, bootstrap.GameState.Day,
                    "GameState.Day is what the save file and the victory screen read; " +
                    "if it does not follow the clock it reports day 1 forever.");
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        [Test]
        public void TheSaveRegisteredClock_IsTheOneTheGameActuallyTicks()
        {
            var go = new GameObject("GameClockPersistenceTests_Identity");
            go.SetActive(false);
            try
            {
                var bootstrap = go.AddComponent<GameBootstrap>();
                RegistryDispatchWiringTests.InjectBootstrapFields(bootstrap);
                typeof(GameBootstrap)
                    .GetMethod("InitializeSystems", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(bootstrap, null);

                var registered = typeof(SaveSystem)
                    .GetField("_timeSystem", BindingFlags.Instance | BindingFlags.NonPublic)
                    ?.GetValue(bootstrap.SaveSystem);

                Assert.IsNotNull(registered, "SaveSystem must hold a clock reference.");
                Assert.AreSame(bootstrap.TimeSystem, registered,
                    "The save key 'time_system' must point at the clock Update() advances, " +
                    "not at a second idle instance.");
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }
    }
}
