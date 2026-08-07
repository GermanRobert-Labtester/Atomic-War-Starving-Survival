using System;
using System.IO;
using NUnit.Framework;
using AtomicWar._Game.Utilities;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Covers the save-slot naming convention and the "which slot does
    /// Continue resume" decision. These run against a temp directory with
    /// hand-set write times rather than the real persistentDataPath, so they
    /// never touch a developer's actual saves.
    /// </summary>
    [TestFixture]
    public class SaveSlotPathsTests
    {
        private string _tempDir;

        [SetUp]
        public void SetUp()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "ashfall_slot_tests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempDir);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_tempDir)) Directory.Delete(_tempDir, recursive: true);
        }

        /// <summary>Write a slot file and stamp it with a deterministic write time.</summary>
        private string WriteSlot(string slotId, DateTime writtenUtc)
        {
            string path = SaveSlotPaths.SlotPath(_tempDir, slotId);
            File.WriteAllText(path, "{}");
            File.SetLastWriteTimeUtc(path, writtenUtc);
            return path;
        }

        // -------------------------------------------------------------
        // Naming convention
        // -------------------------------------------------------------

        [Test]
        public void SlotFileName_ForSlot_UsesSavePrefixAndJsonExtension()
        {
            Assert.That(SaveSlotPaths.SlotFileName("autosave"), Is.EqualTo("save_autosave.json"));
        }

        [Test]
        public void BakPath_ForSlot_AppendsBakToTheSaveFileName()
        {
            string slot = SaveSlotPaths.SlotPath(_tempDir, "quicksave");
            Assert.That(SaveSlotPaths.BakPath(_tempDir, "quicksave"), Is.EqualTo(slot + ".bak"));
        }

        [Test]
        public void SlotIdFromFileName_ForGeneratedName_RoundTripsBackToTheSlotId()
        {
            Assert.That(SaveSlotPaths.SlotIdFromFileName(SaveSlotPaths.SlotFileName("autosave")),
                Is.EqualTo("autosave"));
        }

        [Test]
        public void SlotIdFromFileName_ForUnrelatedFile_ReturnsNull()
        {
            Assert.That(SaveSlotPaths.SlotIdFromFileName("notes.txt"), Is.Null);
            Assert.That(SaveSlotPaths.SlotIdFromFileName("save_autosave.json.bak"), Is.Null);
            Assert.That(SaveSlotPaths.SlotIdFromFileName("save_.json"), Is.Null);
            Assert.That(SaveSlotPaths.SlotIdFromFileName(null), Is.Null);
        }

        [Test]
        public void DefaultSavesDir_Always_EndsWithTheSavesFolderName()
        {
            Assert.That(Path.GetFileName(SaveSlotPaths.DefaultSavesDir),
                Is.EqualTo(SaveSlotPaths.SavesFolderName));
        }

        // -------------------------------------------------------------
        // Continue-slot selection
        // -------------------------------------------------------------

        [Test]
        public void NewestExistingSlot_WhenNoSavesExist_ReturnsNull()
        {
            Assert.That(SaveSlotPaths.NewestExistingSlot(_tempDir, "autosave", "quicksave"), Is.Null);
        }

        [Test]
        public void NewestExistingSlot_WhenOnlyOneSlotExists_ReturnsThatSlot()
        {
            WriteSlot("quicksave", new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            Assert.That(SaveSlotPaths.NewestExistingSlot(_tempDir, "autosave", "quicksave"),
                Is.EqualTo("quicksave"));
        }

        [Test]
        public void NewestExistingSlot_WhenBothExist_ReturnsTheMoreRecentlyWrittenOne()
        {
            WriteSlot("autosave", new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            WriteSlot("quicksave", new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc));

            Assert.That(SaveSlotPaths.NewestExistingSlot(_tempDir, "autosave", "quicksave"),
                Is.EqualTo("quicksave"));
        }

        [Test]
        public void NewestExistingSlot_WhenBothExist_IgnoresTheOrderArgumentsArePassedIn()
        {
            WriteSlot("autosave", new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc));
            WriteSlot("quicksave", new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc));

            // The newer slot wins regardless of which position it occupies.
            Assert.That(SaveSlotPaths.NewestExistingSlot(_tempDir, "autosave", "quicksave"),
                Is.EqualTo("autosave"));
            Assert.That(SaveSlotPaths.NewestExistingSlot(_tempDir, "quicksave", "autosave"),
                Is.EqualTo("autosave"));
        }

        [Test]
        public void NewestExistingSlot_WhenWriteTimesTie_PrefersTheFirstCandidate()
        {
            var sameMoment = new DateTime(2026, 4, 4, 12, 0, 0, DateTimeKind.Utc);
            WriteSlot("autosave", sameMoment);
            WriteSlot("quicksave", sameMoment);

            Assert.That(SaveSlotPaths.NewestExistingSlot(_tempDir, "autosave", "quicksave"),
                Is.EqualTo("autosave"));
        }

        [Test]
        public void NewestExistingSlot_WhenABackupExistsButNoSave_IgnoresTheBackup()
        {
            // A .bak on its own must not enable Continue: SaveSystem.Load only
            // falls back to the backup after the main file fails to parse, so a
            // slot with no main file is not resumable.
            File.WriteAllText(SaveSlotPaths.BakPath(_tempDir, "autosave"), "{}");

            Assert.That(SaveSlotPaths.NewestExistingSlot(_tempDir, "autosave", "quicksave"), Is.Null);
        }

        [Test]
        public void NewestExistingSlot_WhenDirectoryDoesNotExist_ReturnsNull()
        {
            string missing = Path.Combine(_tempDir, "no_such_dir");

            Assert.That(SaveSlotPaths.NewestExistingSlot(missing, "autosave"), Is.Null);
        }

        [Test]
        public void NewestExistingSlot_WithNullOrEmptyInput_ReturnsNullWithoutThrowing()
        {
            Assert.That(SaveSlotPaths.NewestExistingSlot(null, "autosave"), Is.Null);
            Assert.That(SaveSlotPaths.NewestExistingSlot(_tempDir, (string[])null), Is.Null);
            Assert.That(SaveSlotPaths.NewestExistingSlot(_tempDir), Is.Null);
        }
    }
}
