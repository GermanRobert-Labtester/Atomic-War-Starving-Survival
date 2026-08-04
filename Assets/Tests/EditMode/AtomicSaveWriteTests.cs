using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Core;
using AtomicWar._Game.Survivors;
using System.IO;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// A-1: Tests for atomic save write + backup recovery.
    /// Verifies that:
    /// - Save writes to .tmp then renames atomically
    /// - Previous save is backed up to .bak
    /// - If the main save is corrupt, Load falls back to .bak
    /// </summary>
    [TestFixture]
    public class AtomicSaveWriteTests
    {
        private string _tempDir;

        [SetUp]
        public void SetUp()
        {
            _tempDir = Path.Combine(System.IO.Path.GetTempPath(), $"ashfall_test_{System.Guid.NewGuid():N}");
            Directory.CreateDirectory(_tempDir);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_tempDir))
                Directory.Delete(_tempDir, true);
        }

        private SaveSystem CreateSaveSystem()
        {
            var gs = new GameState();
            return new SaveSystem(new SaveSystem.CoreDeps
            {
                GameState = gs,
                WeatherSystem = null,
                TemperatureSystem = null,
                NeedsSystem = null,
                RadiationSystem = null,
                Shelter = null,
                GetSurvivors = () => new System.Collections.Generic.List<Survivor>(),
                ItemLookup = _ => null,
                ModuleLookup = _ => null,
                SavesDir = _tempDir
            });
        }

        [Test]
        public void Save_CreatesBackupOfPreviousSave()
        {
            LogAssert.Expect(LogType.Log, "[SaveSystem] Saved to slot 'test_slot' (atomic write + .bak backup).");
            var save = CreateSaveSystem();
            save.Save("test_slot");

            LogAssert.Expect(LogType.Log, "[SaveSystem] Saved to slot 'test_slot' (atomic write + .bak backup).");
            save.Save("test_slot");

            string bakPath = Path.Combine(_tempDir, "save_test_slot.json.bak");
            Assert.IsTrue(File.Exists(bakPath),
                "Backup file should exist after second save.");
        }

        [Test]
        public void Save_DoesNotLeaveTempFile()
        {
            LogAssert.Expect(LogType.Log, "[SaveSystem] Saved to slot 'test_slot' (atomic write + .bak backup).");
            var save = CreateSaveSystem();
            save.Save("test_slot");

            string tmpPath = Path.Combine(_tempDir, "save_test_slot.json.tmp");
            Assert.IsFalse(File.Exists(tmpPath),
                "Temp file should not remain after successful save.");
        }

        [Test]
        public void Load_RecoverFromBackup_WhenMainSaveCorrupt()
        {
            var save = CreateSaveSystem();

            // Save twice so we get a .bak of the first valid save
            LogAssert.Expect(LogType.Log, "[SaveSystem] Saved to slot 'test_slot' (atomic write + .bak backup).");
            save.Save("test_slot");
            LogAssert.Expect(LogType.Log, "[SaveSystem] Saved to slot 'test_slot' (atomic write + .bak backup).");
            save.Save("test_slot");

            // Corrupt the main save file
            string mainPath = Path.Combine(_tempDir, "save_test_slot.json");
            File.WriteAllText(mainPath, "{ CORRUPT JSON }}}");

            // AUDIT-005: parse/null/checksum failure is logged before recovery.
            LogAssert.Expect(LogType.Warning, new System.Text.RegularExpressions.Regex(
                @"\[SaveSystem\] (Failed to parse save file|Corrupt save parse|Checksum mismatch)"));
            // Expect the recovery warnings
            LogAssert.Expect(LogType.Warning, "[SaveSystem] Slot 'test_slot' main save failed. Attempting recovery from backup...");
            LogAssert.Expect(LogType.Warning, "[SaveSystem] Backup recovered successfully for slot 'test_slot'.");
            LogAssert.Expect(LogType.Log, "[SaveSystem] Loaded slot 'test_slot' (version 3).");

            // Attempt load — should fall back to .bak
            bool loaded = save.Load("test_slot");

            Assert.IsTrue(loaded,
                "Load should succeed by falling back to .bak when main save is corrupt.");
        }

        [Test]
        public void Load_ReturnsFalse_WhenNoBackupAndCorrupt()
        {
            var save = CreateSaveSystem();

            // Write a corrupt save with no .bak
            string mainPath = Path.Combine(_tempDir, "save_test_slot.json");
            File.WriteAllText(mainPath, "{ CORRUPT JSON }}}");

            // AUDIT-005: corrupt parse is logged (not silent).
            LogAssert.Expect(LogType.Warning, new System.Text.RegularExpressions.Regex(
                @"\[SaveSystem\] (Failed to parse save file|Corrupt save parse|Checksum mismatch)"));
            LogAssert.Expect(LogType.Error, "[SaveSystem] Slot 'test_slot' corrupt and no backup available. Load aborted.");

            bool loaded = save.Load("test_slot");

            Assert.IsFalse(loaded,
                "Load should fail when save is corrupt and no backup exists.");
        }

        [Test]
        public void Load_CorruptSave_LogsParseFailure_Audit005()
        {
            var save = CreateSaveSystem();
            string mainPath = Path.Combine(_tempDir, "save_audit005.json");
            File.WriteAllText(mainPath, "{ truncated");

            LogAssert.Expect(LogType.Warning, new System.Text.RegularExpressions.Regex(
                @"\[SaveSystem\] (Failed to parse save file|Corrupt save parse|Checksum mismatch).*save_audit005"));
            LogAssert.Expect(LogType.Error, "[SaveSystem] Slot 'audit005' corrupt and no backup available. Load aborted.");

            bool loaded = save.Load("audit005");
            Assert.IsFalse(loaded, "Corrupt save without backup must fail Load.");
        }
    }
}
