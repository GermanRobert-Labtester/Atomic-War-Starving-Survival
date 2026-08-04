using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Audit P0 EditMode coverage:
    /// - AUDIT-005: corrupt save parse is logged
    /// - AUDIT-004: FailFastRestore aborts Load on ISaveable restore failure
    /// - AUDIT-003: foundation null-assert helper rejects missing systems
    /// </summary>
    [TestFixture]
    public class AuditP0SaveAndFoundationTests
    {
        private string _tempDir;

        [SetUp]
        public void SetUp()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), $"ashfall_p0_{Guid.NewGuid():N}");
            Directory.CreateDirectory(_tempDir);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_tempDir))
                Directory.Delete(_tempDir, true);
        }

        // -----------------------------------------------------------------
        // Stubs
        // -----------------------------------------------------------------

        [Serializable]
        public sealed class StubState
        {
            public string Value;
        }

        private sealed class StubSaveable : ISaveable
        {
            public string SaveId { get; }
            public object CapturedStateToReturn { get; set; }
            public int RestoreCallCount { get; private set; }
            public bool ThrowOnRestore { get; set; }

            public StubSaveable(string saveId, string value = null)
            {
                SaveId = saveId;
                CapturedStateToReturn = new StubState { Value = value ?? saveId };
            }

            public object CaptureState() => CapturedStateToReturn;

            public void RestoreState(object state)
            {
                RestoreCallCount++;
                if (ThrowOnRestore)
                    throw new InvalidOperationException($"restore_failed:{SaveId}");
            }
        }

        // -----------------------------------------------------------------
        // AUDIT-005 — corrupt parse logged
        // -----------------------------------------------------------------

        [Test]
        public void Audit005_CorruptSave_LogsWarning_AndLoadFails()
        {
            var save = CreateMinimalSaveSystem();
            string path = Path.Combine(_tempDir, "save_corrupt_p0.json");
            File.WriteAllText(path, "{ not valid json at all }}}");

            LogAssert.Expect(LogType.Warning, new Regex(
                @"\[SaveSystem\] (Failed to parse save file|Corrupt save parse|Checksum mismatch)"));
            LogAssert.Expect(LogType.Error,
                "[SaveSystem] Slot 'corrupt_p0' corrupt and no backup available. Load aborted.");

            Assert.IsFalse(save.Load("corrupt_p0"));
        }

        [Test]
        public void Audit005_TruncatedSave_LogsWarning()
        {
            var save = CreateMinimalSaveSystem();
            string path = Path.Combine(_tempDir, "save_trunc_p0.json");
            // Truncated mid-object — classic crash-during-write residue.
            File.WriteAllText(path, "{\n  \"SaveVersion\": 3,\n  \"Checksum\": \"abc");

            LogAssert.Expect(LogType.Warning, new Regex(
                @"\[SaveSystem\] (Failed to parse save file|Corrupt save parse|Checksum mismatch)"));
            LogAssert.Expect(LogType.Error,
                "[SaveSystem] Slot 'trunc_p0' corrupt and no backup available. Load aborted.");

            Assert.IsFalse(save.Load("trunc_p0"));
        }

        // -----------------------------------------------------------------
        // AUDIT-004 — FailFastRestore
        // -----------------------------------------------------------------

        [Test]
        public void Audit004_FailFastRestore_False_ContinuesAfterThrowingSaveable()
        {
            var writer = CreateMinimalSaveSystem();
            writer.Register(new StubSaveable("good_a", "A"));
            writer.Register(new StubSaveable("bad_sys", "B"));
            writer.Register(new StubSaveable("good_c", "C"));
            LogAssert.Expect(LogType.Log, new Regex(@"\[SaveSystem\] Saved to slot"));
            Assert.IsTrue(writer.Save("failfast_soft"));

            var reader = CreateMinimalSaveSystem();
            reader.FailFastRestore = false;
            var goodA = new StubSaveable("good_a");
            var bad = new StubSaveable("bad_sys") { ThrowOnRestore = true };
            var goodC = new StubSaveable("good_c");
            reader.Register(goodA);
            reader.Register(bad);
            reader.Register(goodC);

            LogAssert.Expect(LogType.Error, new Regex(
                @"\[SaveSystem\] ISaveable\.RestoreState failed for 'bad_sys'"));
            LogAssert.Expect(LogType.Log, new Regex(@"\[SaveSystem\] Loaded slot"));

            bool loaded = reader.Load("failfast_soft");
            Assert.IsTrue(loaded, "Soft mode should still report Load success after one subsystem error.");
            Assert.AreEqual(1, goodA.RestoreCallCount, "good_a should restore before the throw.");
            Assert.AreEqual(1, bad.RestoreCallCount, "bad_sys RestoreState is attempted.");
            Assert.AreEqual(1, goodC.RestoreCallCount, "good_c continues after soft failure.");
        }

        [Test]
        public void Audit004_FailFastRestore_True_AbortsLoad_OnThrowingSaveable()
        {
            var writer = CreateMinimalSaveSystem();
            writer.Register(new StubSaveable("keep_a", "A"));
            writer.Register(new StubSaveable("boom", "B"));
            writer.Register(new StubSaveable("keep_c", "C"));
            LogAssert.Expect(LogType.Log, new Regex(@"\[SaveSystem\] Saved to slot"));
            Assert.IsTrue(writer.Save("failfast_hard"));

            var reader = CreateMinimalSaveSystem();
            reader.FailFastRestore = true;
            var keepA = new StubSaveable("keep_a");
            var boom = new StubSaveable("boom") { ThrowOnRestore = true };
            var keepC = new StubSaveable("keep_c");
            reader.Register(keepA);
            reader.Register(boom);
            reader.Register(keepC);

            LogAssert.Expect(LogType.Error, new Regex(
                @"\[SaveSystem\] ISaveable\.RestoreState failed for 'boom' \(FailFastRestore\)"));
            // Outer Load catch logs the rethrown exception.
            LogAssert.Expect(LogType.Error, new Regex(@"\[SaveSystem\] Load from 'failfast_hard' failed:"));

            bool loaded = reader.Load("failfast_hard");
            Assert.IsFalse(loaded, "FailFastRestore must abort Load when any ISaveable throws.");
            Assert.AreEqual(0, keepC.RestoreCallCount,
                "Later ISaveables must not restore after fail-fast abort (two-phase apply order).");
        }

        [Test]
        public void Audit004_FailFastRestore_PropertyDefaultsFalse()
        {
            var save = CreateMinimalSaveSystem();
            Assert.IsFalse(save.FailFastRestore,
                "Instance default is false until bootstrap/policy applies.");
        }

        [Test]
        public void AuditP1_DefaultFailFastRestoreForEnvironment_IsTrueInEditor()
        {
            // game-ci and local EditMode compile with UNITY_EDITOR.
            Assert.IsTrue(
                SaveSystem.DefaultFailFastRestoreForEnvironment(),
                "P1: Editor/CI must default to fail-fast ISaveable restore.");
        }

        [Test]
        public void AuditP1_ApplyingEnvironmentPolicy_EnablesFailFast()
        {
            var save = CreateMinimalSaveSystem();
            Assert.IsFalse(save.FailFastRestore);
            save.FailFastRestore = SaveSystem.DefaultFailFastRestoreForEnvironment();
            Assert.IsTrue(save.FailFastRestore,
                "Bootstrap wiring pattern must enable fail-fast under UNITY_EDITOR.");
        }

        // -----------------------------------------------------------------
        // AUDIT-003 — foundation null-assert
        // -----------------------------------------------------------------

        [Test]
        public void Audit003_CollectMissingFoundation_ReportsAllNulls()
        {
            var missing = GameBootstrap.CollectMissingFoundationSystems(
                null, null, null, null, null, null, null, null);

            Assert.AreEqual(GameBootstrap.FoundationSystemNames.Length, missing.Count);
            CollectionAssert.AreEquivalent(GameBootstrap.FoundationSystemNames, missing);
        }

        [Test]
        public void Audit003_CollectMissingFoundation_EmptyWhenAllPresent()
        {
            var gs = new GameState();
            var weather = new WeatherSystem(null, 7);
            var temperature = new TemperatureSystem(null, weather);
            var photo = new PhotoperiodSystem();
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, _ => true);
            var radiation = new RadiationSystem(needs);
            var shelter = new AtomicWar._Game.Shelter.Shelter();
            var time = new TimeSystem();

            var missing = GameBootstrap.CollectMissingFoundationSystems(
                gs, time, weather, temperature, photo, needs, radiation, shelter);

            Assert.AreEqual(0, missing.Count,
                "Full foundation set must produce no missing names.");
        }

        [Test]
        public void Audit003_CollectMissingFoundation_ReportsPartialSet()
        {
            var gs = new GameState();
            var weather = new WeatherSystem(null, 1);

            var missing = GameBootstrap.CollectMissingFoundationSystems(
                gs, null, weather, null, null, null, null, null);

            CollectionAssert.Contains(missing, nameof(GameBootstrap.TimeSystem));
            CollectionAssert.Contains(missing, nameof(GameBootstrap.NeedsSystem));
            CollectionAssert.Contains(missing, nameof(GameBootstrap.Shelter));
            CollectionAssert.DoesNotContain(missing, nameof(GameBootstrap.GameState));
            CollectionAssert.DoesNotContain(missing, nameof(GameBootstrap.WeatherSystem));
        }

        [Test]
        public void Audit003_AssertFoundationSystems_ThrowsWhenMissing()
        {
            // Keep inactive so Awake/InitializeSystems does not run — we only
            // exercise the public assert helper against a null foundation set.
            var go = new GameObject("audit003_bootstrap");
            go.SetActive(false);
            try
            {
                var bootstrap = go.AddComponent<GameBootstrap>();
                LogAssert.Expect(LogType.Error, new Regex(
                    @"\[GameBootstrap\] Foundation systems missing after InitializeSystems:"));

                var ex = Assert.Throws<InvalidOperationException>(() => bootstrap.AssertFoundationSystems());
                Assert.That(ex.Message, Does.Contain("Foundation systems missing"));
                Assert.That(ex.Message, Does.Contain(nameof(GameBootstrap.WeatherSystem)));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(go);
            }
        }

        // -----------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------

        private SaveSystem CreateMinimalSaveSystem()
        {
            var gameState = new GameState();
            var weather = new WeatherSystem(null, 42);
            var temperature = new TemperatureSystem(null, weather);
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);
            var radiation = new RadiationSystem(needs);
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
                ModuleLookup = id => null,
                SavesDir = _tempDir
            });
        }
    }
}
