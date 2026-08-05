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

        /// <summary>
        /// Mirrors SaveSystem.ComputeChecksum so a test can forge a save file
        /// that passes checksum validation. Pinning the algorithm here is
        /// deliberate: if the checksum format changes, this fixture should
        /// fail loudly rather than silently stop exercising the guard.
        /// </summary>
        private static string ComputeChecksum(string json)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            byte[] hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(json));
            var sb = new System.Text.StringBuilder(hash.Length * 2);
            foreach (byte b in hash) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        /// <summary>
        /// A save written by a newer build must be refused outright. Restoring
        /// it would produce a half-initialised world that the next autosave
        /// writes back over the player's good save.
        /// </summary>
        [Test]
        public void Load_RefusesSaveWrittenByNewerBuild()
        {
            var save = CreateSaveSystem();
            LogAssert.Expect(LogType.Log, "[SaveSystem] Saved to slot 'future' (atomic write + .bak backup).");
            save.Save("future");

            // Forge a valid-but-newer save the same way a future build would:
            // bump the version, then recompute the checksum over the new body.
            string mainPath = Path.Combine(_tempDir, "save_future.json");
            var data = JsonUtility.FromJson<SaveData>(File.ReadAllText(mainPath));
            data.SaveVersion = SaveSystem.CurrentSaveVersion + 1;
            data.Checksum = "";
            data.Checksum = ComputeChecksum(JsonUtility.ToJson(data, true));
            File.WriteAllText(mainPath, JsonUtility.ToJson(data, true));

            LogAssert.Expect(LogType.Error, new System.Text.RegularExpressions.Regex(
                @"\[SaveSystem\] Slot 'future' was written by a newer build"));

            bool loaded = save.Load("future");

            Assert.IsFalse(loaded,
                "A save from a newer build must be refused, not partially restored.");
            Assert.IsTrue(File.Exists(mainPath),
                "Refusing the load must leave the file intact for a newer build to open.");
        }

        /// <summary>
        /// The replace step must always leave two independently loadable
        /// generations on disk. The previous implementation deleted the main
        /// save before moving the temp file into place, so a crash in that
        /// window left only the .bak.
        /// </summary>
        [Test]
        public void Save_LeavesTwoLoadableGenerations()
        {
            var save = CreateSaveSystem();
            LogAssert.Expect(LogType.Log, "[SaveSystem] Saved to slot 'gen' (atomic write + .bak backup).");
            save.Save("gen");
            LogAssert.Expect(LogType.Log, "[SaveSystem] Saved to slot 'gen' (atomic write + .bak backup).");
            save.Save("gen");

            string mainPath = Path.Combine(_tempDir, "save_gen.json");
            string bakPath = Path.Combine(_tempDir, "save_gen.json.bak");
            Assert.IsTrue(File.Exists(mainPath), "Main save must exist after the replace.");
            Assert.IsTrue(File.Exists(bakPath), "Backup generation must exist after the replace.");
            Assert.IsFalse(File.Exists(mainPath + ".tmp"), "Temp file must not survive the replace.");

            LogAssert.Expect(LogType.Log, "[SaveSystem] Loaded slot 'gen' (version 3).");
            Assert.IsTrue(save.Load("gen"), "The current generation must be loadable.");

            // The backup must be a complete save in its own right, not a partial file.
            File.Copy(bakPath, Path.Combine(_tempDir, "save_genprev.json"));
            LogAssert.Expect(LogType.Log, "[SaveSystem] Loaded slot 'genprev' (version 3).");
            Assert.IsTrue(save.Load("genprev"), "The backup generation must be independently loadable.");
        }
    }
}
