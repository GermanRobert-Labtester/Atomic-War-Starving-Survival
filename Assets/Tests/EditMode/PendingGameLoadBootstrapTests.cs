using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Utilities;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Guards the main menu's "Continue" handoff into GameBootstrap, and in
    /// particular the ordering hazard it exists to avoid.
    ///
    /// GameBootstrap subscribes AutoSave() to GameState.OnPhaseChanged for
    /// GamePhase.Running. Awake used to end with Phase = Running, which meant
    /// booting the game immediately overwrote save_autosave.json with the
    /// fresh, empty world. A Continue that ran after that point would load the
    /// world it had just destroyed. These tests pin the fix: the pending slot
    /// is consumed and restored while autosave is suppressed, before the phase
    /// is allowed to reach Running.
    /// </summary>
    [TestFixture]
    public class PendingGameLoadBootstrapTests
    {
        private GameObject _go;
        private GameBootstrap _bootstrap;
        private string _savesDir;

        [SetUp]
        public void SetUp()
        {
            PendingGameLoad.Clear();

            // Disabled GO so AddComponent does not fire Awake before the
            // ScriptableObject fields are injected (pattern from
            // RegistryDispatchWiringTests).
            _go = new GameObject(nameof(PendingGameLoadBootstrapTests));
            _go.SetActive(false);
            _bootstrap = _go.AddComponent<GameBootstrap>();
            InjectBootstrapFields(_bootstrap);
            Invoke("InitializeSystems");

            _savesDir = Path.Combine(
                Path.GetTempPath(), "ashfall_pending_load_" + System.Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_savesDir);
            RedirectSavesDir(_savesDir);
        }

        [TearDown]
        public void TearDown()
        {
            if (_go != null) Object.DestroyImmediate(_go);
            _bootstrap = null;
            PendingGameLoad.Clear();
            if (Directory.Exists(_savesDir)) Directory.Delete(_savesDir, recursive: true);
        }

        // -------------------------------------------------------------
        // The ordering guarantee
        // -------------------------------------------------------------

        [Test]
        public void ApplyPendingGameLoad_WhileRestoring_DoesNotWriteTheAutosaveSlot()
        {
            // The regression this whole change exists for. Restoring must not
            // trip the OnPhaseChanged -> AutoSave hook, or continuing from
            // quicksave would silently destroy the player's separate autosave.
            _bootstrap.SaveGame("quicksave");
            string autosavePath = SaveSlotPaths.SlotPath(_savesDir, "autosave");
            Assert.That(File.Exists(autosavePath), Is.False,
                "Precondition: no autosave should exist yet.");

            PendingGameLoad.SlotId = "quicksave";
            Invoke("ApplyPendingGameLoad");

            Assert.That(File.Exists(autosavePath), Is.False,
                "Restoring a Continue slot must not fire AutoSave.");
        }

        [Test]
        public void ApplyPendingGameLoad_AfterRestoring_LeavesAutoSaveReEnabled()
        {
            // The suppression is scoped to the restore; normal play must still
            // autosave afterwards.
            _bootstrap.SaveGame("quicksave");
            PendingGameLoad.SlotId = "quicksave";
            Invoke("ApplyPendingGameLoad");

            Assert.That(GetPrivate<bool>("_suppressAutoSave"), Is.False);
        }

        [Test]
        public void ApplyPendingGameLoad_WithNoPendingSlot_LeavesPhaseUntouched()
        {
            // Entering play mode directly on the gameplay scene: unchanged
            // fresh-game behaviour.
            Invoke("ApplyPendingGameLoad");

            Assert.That(_bootstrap.GameState.Phase, Is.EqualTo(GamePhase.MainMenu));
        }

        // -------------------------------------------------------------
        // Slot consumption
        // -------------------------------------------------------------

        [Test]
        public void ApplyPendingGameLoad_WithPendingSlot_ConsumesIt()
        {
            _bootstrap.SaveGame("quicksave");
            PendingGameLoad.SlotId = "quicksave";

            Invoke("ApplyPendingGameLoad");

            Assert.That(PendingGameLoad.SlotId, Is.Null,
                "A consumed slot must not be re-applied on a later scene load.");
        }

        [Test]
        public void ApplyPendingGameLoad_WithMissingSlot_WarnsAndStillConsumesIt()
        {
            PendingGameLoad.SlotId = "no_such_slot";
            LogAssert.Expect(LogType.Warning, new Regex("Slot 'no_such_slot' not found"));
            LogAssert.Expect(LogType.Warning, new Regex("Continue requested slot 'no_such_slot'"));

            Invoke("ApplyPendingGameLoad");

            Assert.That(PendingGameLoad.SlotId, Is.Null);
        }

        [Test]
        public void ApplyPendingGameLoad_WithMissingSlot_LeavesPhaseAtMainMenuSoAwakeStartsFresh()
        {
            // A failed Continue must fall through to the normal fresh-game path
            // rather than leaving the world in a half-restored state.
            PendingGameLoad.SlotId = "no_such_slot";
            LogAssert.Expect(LogType.Warning, new Regex("Slot 'no_such_slot' not found"));
            LogAssert.Expect(LogType.Warning, new Regex("Continue requested slot 'no_such_slot'"));

            Invoke("ApplyPendingGameLoad");

            Assert.That(_bootstrap.GameState.Phase, Is.EqualTo(GamePhase.MainMenu));
        }

        // -------------------------------------------------------------
        // LoadGame's return contract
        // -------------------------------------------------------------

        [Test]
        public void LoadGame_WithMissingSlot_ReturnsFalse()
        {
            LogAssert.Expect(LogType.Warning, new Regex("Slot 'no_such_slot' not found"));

            Assert.That(_bootstrap.LoadGame("no_such_slot"), Is.False);
        }

        [Test]
        public void LoadGame_WithExistingSlot_ReturnsTrue()
        {
            _bootstrap.SaveGame("quicksave");

            Assert.That(_bootstrap.LoadGame("quicksave"), Is.True);
        }

        // -------------------------------------------------------------
        // Helpers
        // -------------------------------------------------------------

        private void Invoke(string methodName)
        {
            MethodInfo method = typeof(GameBootstrap).GetMethod(
                methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(method, Is.Not.Null, $"GameBootstrap must have {methodName}().");
            try
            {
                method.Invoke(_bootstrap, null);
            }
            catch (TargetInvocationException ex)
            {
                Assert.Fail($"{methodName} threw: {ex.InnerException?.ToString() ?? ex.ToString()}");
            }
        }

        private T GetPrivate<T>(string fieldName)
        {
            FieldInfo field = typeof(GameBootstrap).GetField(
                fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(field, Is.Not.Null, $"GameBootstrap must have field '{fieldName}'.");
            return (T)field.GetValue(_bootstrap);
        }

        /// <summary>
        /// Point the already-constructed SaveSystem at a temp folder so the
        /// suite never reads or writes a developer's real saves.
        /// </summary>
        private void RedirectSavesDir(string dir)
        {
            FieldInfo field = typeof(SaveSystem).GetField(
                "_savesDir", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(field, Is.Not.Null, "SaveSystem must have a '_savesDir' field.");
            field.SetValue(_bootstrap.SaveSystem, dir);
        }

        private static void InjectBootstrapFields(GameBootstrap bs)
        {
            SetPrivate(bs, "_needsProfile", ScriptableObject.CreateInstance<NeedsProfile>());
            SetPrivate(bs, "_lightProfile", ScriptableObject.CreateInstance<LightProfile>());
            SetPrivate(bs, "_seasonProfile", ScriptableObject.CreateInstance<SeasonProfile>());
            SetPrivate(bs, "_itemCatalog", ScriptableObject.CreateInstance<ItemCatalogSO>());
            SetPrivate(bs, "_recipeCatalog", ScriptableObject.CreateInstance<RecipeCatalogSO>());
            SetPrivate(bs, "_eventCatalog", ScriptableObject.CreateInstance<GameEventCatalogSO>());
            SetPrivate(bs, "_locationCatalog", ScriptableObject.CreateInstance<LocationCatalogSO>());
            SetPrivate(bs, "_radioCatalog", ScriptableObject.CreateInstance<RadioCatalogSO>());
            SetPrivate(bs, "_worldPhaseConfig", ScriptableObject.CreateInstance<WorldPhaseConfigSO>());
            SetPrivate(bs, "_flashpointSequence", ScriptableObject.CreateInstance<FlashpointSequenceSO>());
            SetPrivate(bs, "_mentalBreakCatalog", ScriptableObject.CreateInstance<MentalBreakCatalogSO>());
            SetPrivate(bs, "_lootTable", ScriptableObject.CreateInstance<LootTableSO>());
        }

        private static void SetPrivate(object instance, string fieldName, object value)
        {
            FieldInfo field = instance.GetType().GetField(
                fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.That(field, Is.Not.Null, $"{instance.GetType().Name} missing field '{fieldName}'");
            field.SetValue(instance, value);
        }
    }
}
