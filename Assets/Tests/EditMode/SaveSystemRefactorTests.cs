using System;
using System.Collections.Generic;
using NUnit.Framework;
using AtomicWar._Game.Core;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Environment;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// H-4: ISaveable refactor tests. Verifies that the Register/ISaveable
    /// infrastructure works correctly: systems registered via Register() are
    /// iterated during capture/restore, duplicates are handled, unknown systems
    /// don't throw, and the V2→V3 migration preserves data.
    /// </summary>
    [TestFixture]
    public class SaveSystemRefactorTests
    {
        // -----------------------------------------------------------------
        // Stub saveable for testing
        // -----------------------------------------------------------------

        private sealed class StubSaveable : ISaveable
        {
            public string SaveId { get; }
            public object LastCapturedState { get; private set; }
            public object LastRestoredState { get; private set; }
            public object CapturedStateToReturn { get; set; }
            public int CaptureCallCount { get; private set; }
            public int RestoreCallCount { get; private set; }

            public StubSaveable(string saveId)
            {
                SaveId = saveId ?? throw new ArgumentNullException(nameof(saveId));
                CapturedStateToReturn = new StubState { Value = saveId };
            }

            public object CaptureState()
            {
                CaptureCallCount++;
                LastCapturedState = CapturedStateToReturn;
                return CapturedStateToReturn;
            }

            public void RestoreState(object state)
            {
                RestoreCallCount++;
                LastRestoredState = state;
            }
        }

        [Serializable]
        public sealed class StubState
        {
            public string Value;
        }

        // -----------------------------------------------------------------
        // ISaveable.Register — remembers systems
        // -----------------------------------------------------------------

        [Test]
        public void Register_AddsSystem_ToSaveableCount()
        {
            var saveSystem = CreateMinimalSaveSystem();
            var stub = new StubSaveable("test_system");

            saveSystem.Register(stub);

            Assert.AreEqual(1, saveSystem.SaveableCount);
        }

        [Test]
        public void Register_SameIdTwice_LastOneWins()
        {
            var saveSystem = CreateMinimalSaveSystem();
            var first = new StubSaveable("duplicate_id");
            var second = new StubSaveable("duplicate_id");

            saveSystem.Register(first);
            saveSystem.Register(second);

            Assert.AreEqual(1, saveSystem.SaveableCount,
                "Duplicate SaveIds should replace, not append.");
        }

        [Test]
        public void Register_DifferentIds_AddsAll()
        {
            var saveSystem = CreateMinimalSaveSystem();
            saveSystem.Register(new StubSaveable("a"));
            saveSystem.Register(new StubSaveable("b"));
            saveSystem.Register(new StubSaveable("c"));

            Assert.AreEqual(3, saveSystem.SaveableCount);
        }

        [Test]
        public void Register_Null_DoesNotThrow()
        {
            var saveSystem = CreateMinimalSaveSystem();

            Assert.DoesNotThrow(() => saveSystem.Register(null));
            Assert.AreEqual(0, saveSystem.SaveableCount);
        }

        // -----------------------------------------------------------------
        // ISaveable.Capture — iterates all registered
        // -----------------------------------------------------------------

        [Test]
        public void Save_CallsCapture_OnAllRegistered()
        {
            var saveSystem = CreateMinimalSaveSystem();
            var a = new StubSaveable("sys_a");
            var b = new StubSaveable("sys_b");
            saveSystem.Register(a);
            saveSystem.Register(b);

            saveSystem.Save("test_capture_all");

            Assert.AreEqual(1, a.CaptureCallCount,
                "CaptureState should be called once per registered saveable on Save.");
            Assert.AreEqual(1, b.CaptureCallCount);
        }

        [Test]
        public void Save_StoresState_InSaveData()
        {
            var saveSystem = CreateMinimalSaveSystem();
            var stub = new StubSaveable("pets");
            stub.CapturedStateToReturn = new StubState { Value = "three_cats" };
            saveSystem.Register(stub);

            bool saved = saveSystem.Save("test_store_state");
            Assert.IsTrue(saved, "Save should succeed.");

            // Load into a fresh SaveSystem to verify the state was stored.
            var saveSystem2 = CreateMinimalSaveSystem();
            var stub2 = new StubSaveable("pets");
            saveSystem2.Register(stub2);

            bool loaded = saveSystem2.Load("test_store_state");
            Assert.IsTrue(loaded, "Load should succeed.");
            Assert.IsNotNull(stub2.LastRestoredState, "RestoreState should be called on Load.");
            var restored = stub2.LastRestoredState as StubState;
            Assert.IsNotNull(restored);
            Assert.AreEqual("three_cats", restored.Value,
                "Round-tripped state should preserve the value.");
        }

        // -----------------------------------------------------------------
        // ISaveable.Restore — iterates all registered
        // -----------------------------------------------------------------

        [Test]
        public void Load_RestoreCalled_ForAllRegistered()
        {
            var saveSystem = CreateMinimalSaveSystem();
            var a = new StubSaveable("aa");
            var b = new StubSaveable("bb");
            saveSystem.Register(a);
            saveSystem.Register(b);
            saveSystem.Save("test_restore_all");

            var saveSystem2 = CreateMinimalSaveSystem();
            var a2 = new StubSaveable("aa");
            var b2 = new StubSaveable("bb");
            saveSystem2.Register(a2);
            saveSystem2.Register(b2);

            saveSystem2.Load("test_restore_all");

            Assert.AreEqual(1, a2.RestoreCallCount);
            Assert.AreEqual(1, b2.RestoreCallCount);
        }

        [Test]
        public void Load_UnknownSaveId_DoesNotThrow()
        {
            var saveSystem = CreateMinimalSaveSystem();
            var known = new StubSaveable("known_sys");
            saveSystem.Register(known);
            saveSystem.Save("test_unknown_id");

            // Load into a system that does NOT have "known_sys" registered.
            var saveSystem2 = CreateMinimalSaveSystem();
            var other = new StubSaveable("other_sys");
            saveSystem2.Register(other);

            Assert.DoesNotThrow(() => saveSystem2.Load("test_unknown_id"),
                "Unknown SaveIds in the save file should be silently skipped.");
        }

        // -----------------------------------------------------------------
        // V2 → V3 migration (H-4)
        // -----------------------------------------------------------------

        [Test]
        public void Save_WritesVersion3()
        {
            var saveSystem = CreateMinimalSaveSystem();
            saveSystem.Save("test_version3");

            // The save file should contain version 3.
            string path = System.IO.Path.Combine(
                UnityEngine.Application.persistentDataPath, "saves", "save_test_version3.json");
            Assert.IsTrue(System.IO.File.Exists(path), "Save file should exist.");
            string json = System.IO.File.ReadAllText(path);
            Assert.IsTrue(json.Contains("\"SaveVersion\": 3") || json.Contains("\"SaveVersion\":3"),
                "Save file should be version 3 after H-4 refactor.");
        }

        [Test]
        public void Load_V2Save_MigratesToV3()
        {
            // Audit M-1: this test used to give up on hand-crafting a valid V2
            // fixture (checksum verification blocked a naive JSON string edit)
            // and Assert.Pass a comment instead. VerifyChecksum re-serializes the
            // *deserialized* SaveData rather than diffing raw file bytes, so a
            // hand-built SaveData object — checksummed the same way Save() does
            // it — round-trips through Load() exactly like a real V2 file would.
            var v2 = new SaveData
            {
                SaveVersion = 2,
                GameState = new GameStateSave { Day = 77, Phase = GamePhase.Running }
            };

            v2.Checksum = "";
            string body = UnityEngine.JsonUtility.ToJson(v2, true);
            v2.Checksum = ComputeTestChecksum(body);
            string finalJson = UnityEngine.JsonUtility.ToJson(v2, true);

            string dir = SaveSystemTestFactory.TempDir("v2migrate");
            const string slotId = "v2migrate";
            string path = AtomicWar._Game.Utilities.SaveSlotPaths.SlotPath(dir, slotId);
            System.IO.File.WriteAllText(path, finalJson);

            try
            {
                var gameState = new GameState();
                var weather = new WeatherSystem(null, 42);
                var temperature = new TemperatureSystem(null, weather);
                var profile = UnityEngine.ScriptableObject.CreateInstance<NeedsProfile>();
                var needs = new NeedsSystem(profile, sv => true);
                var radiation = new AtomicWar._Game.Radiation.RadiationSystem(needs);
                var shelter = new AtomicWar._Game.Shelter.Shelter();
                var saveSystem = new SaveSystem(new SaveSystem.CoreDeps
                {
                    GameState = gameState,
                    WeatherSystem = weather,
                    TemperatureSystem = temperature,
                    NeedsSystem = needs,
                    RadiationSystem = radiation,
                    Shelter = shelter,
                    GetSurvivors = () => new List<Survivor>(),
                    ItemLookup = id => null,
                    ModuleLookup = id => null,
                    SavesDir = dir
                });
                var stub = new StubSaveable("some_v3_only_system");
                saveSystem.Register(stub);

                bool loaded = saveSystem.Load(slotId);

                Assert.IsTrue(loaded,
                    "A well-formed V2 save must load successfully — checksum and version gates must accept it.");
                Assert.IsTrue(saveSystem.LastLoadSucceeded);
                Assert.AreEqual(77, gameState.Day,
                    "Positional GameState fields must survive the V2->V3 migration untouched.");
                Assert.AreEqual(0, stub.RestoreCallCount,
                    "MigrateV2toV3 resets SubsystemSaveIds/SubsystemSaveJsons to empty lists, " +
                    "so a migrated V2 save must restore zero ISaveable subsystems.");
            }
            finally
            {
                try { System.IO.Directory.Delete(dir, true); } catch { /* best-effort cleanup */ }
            }
        }

        // -----------------------------------------------------------------
        // Multiple registrations — last one wins (by SaveId)
        // -----------------------------------------------------------------

        [Test]
        public void Register_SameIdTwice_CaptureUsesSecond()
        {
            var saveSystem = CreateMinimalSaveSystem();
            var first = new StubSaveable("idem");
            first.CapturedStateToReturn = new StubState { Value = "first" };
            var second = new StubSaveable("idem");
            second.CapturedStateToReturn = new StubState { Value = "second" };

            saveSystem.Register(first);
            saveSystem.Register(second);
            saveSystem.Save("test_last_wins");

            var saveSystem2 = CreateMinimalSaveSystem();
            var reader = new StubSaveable("idem");
            saveSystem2.Register(reader);
            saveSystem2.Load("test_last_wins");

            var restored = reader.LastRestoredState as StubState;
            Assert.IsNotNull(restored);
            Assert.AreEqual("second", restored.Value,
                "Last registered system with the same SaveId should win.");
        }

        // -----------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------

        private static SaveSystem CreateMinimalSaveSystem()
        {
            var gameState = new GameState();
            var weather = new WeatherSystem(null, 42);
            var temperature = new TemperatureSystem(null, weather);
            var profile = UnityEngine.ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);
            var radiation = new AtomicWar._Game.Radiation.RadiationSystem(needs);
            var shelter = new AtomicWar._Game.Shelter.Shelter();

            return new SaveSystem(new SaveSystem.CoreDeps
            {
                GameState = gameState,
                WeatherSystem = weather,
                TemperatureSystem = temperature,
                NeedsSystem = needs,
                RadiationSystem = radiation,
                Shelter = shelter,
                GetSurvivors = () => new List<Survivor>(),
                ItemLookup = id => null,
                ModuleLookup = id => null
            });
        }

        /// <summary>Mirrors SaveSystem.IO.cs's private ComputeChecksum exactly, so a
        /// hand-built SaveData fixture can carry a checksum Load() will accept.</summary>
        private static string ComputeTestChecksum(string json)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            byte[] hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(json));
            var sb = new System.Text.StringBuilder(hash.Length * 2);
            foreach (byte b in hash) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
