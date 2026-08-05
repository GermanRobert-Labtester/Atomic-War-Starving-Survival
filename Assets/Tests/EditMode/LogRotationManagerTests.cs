using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Core;
using System.IO;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// A-11: Tests for LogRotationManager archive + cleanup logic.
    /// </summary>
    [TestFixture]
    public class LogRotationManagerTests
    {
        private string _tempDir;

        [SetUp]
        public void SetUp()
        {
            _tempDir = Path.Combine(System.IO.Path.GetTempPath(), $"ashfall_logrotate_{System.Guid.NewGuid():N}");
            Directory.CreateDirectory(_tempDir);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_tempDir))
                Directory.Delete(_tempDir, true);
        }

        [Test]
        public void ArchiveLog_CreatesTimestampedArchive()
        {
            string logPath = Path.Combine(_tempDir, "Player.log");
            File.WriteAllText(logPath, "FAKE LOG CONTENT");

            LogRotationManager.ArchiveLog(logPath, 1024);

            string[] archives = Directory.GetFiles(_tempDir, "Player_archive_*.log");
            Assert.AreEqual(1, archives.Length, "One archive file should be created.");
            Assert.AreEqual("FAKE LOG CONTENT", File.ReadAllText(archives[0]),
                "Archive should contain the original log content.");
        }

        [Test]
        public void TruncateLog_ClearsFileContent()
        {
            string logPath = Path.Combine(_tempDir, "Player.log");
            File.WriteAllText(logPath, "SOME LOG CONTENT");

            LogRotationManager.TruncateLog(logPath);

            Assert.AreEqual(0, new FileInfo(logPath).Length,
                "Log file should be truncated to zero bytes.");
        }

        [Test]
        public void CleanOldArchives_DeletesOldFiles()
        {
            string oldArchive = Path.Combine(_tempDir, "Player_archive_20250101_000000.log");
            File.WriteAllText(oldArchive, "OLD");
            string recentArchive = Path.Combine(_tempDir, "Player_archive_20260804_120000.log");
            File.WriteAllText(recentArchive, "RECENT");

            File.SetLastWriteTime(oldArchive, System.DateTime.Now.AddDays(-30));

            LogRotationManager.CleanOldArchives(_tempDir, maxAgeDays: 7, maxArchives: 5, baseName: "Player");

            Assert.IsFalse(File.Exists(oldArchive), "Old archive (>7 days) should be deleted.");
            Assert.IsTrue(File.Exists(recentArchive), "Recent archive should be kept.");
        }

        [Test]
        public void CleanOldArchives_KeepsMaxArchiveCount()
        {
            for (int i = 0; i < 10; i++)
            {
                string path = Path.Combine(_tempDir, $"Player_archive_2026080{i}_120000.log");
                File.WriteAllText(path, $"LOG{i}");
                File.SetLastWriteTime(path, System.DateTime.Now.AddDays(-i));
            }

            LogRotationManager.CleanOldArchives(_tempDir, maxAgeDays: 365, maxArchives: 3, baseName: "Player");

            string[] remaining = Directory.GetFiles(_tempDir, "Player_archive_*.log");
            Assert.AreEqual(3, remaining.Length, "Should keep only MaxArchiveCount archives.");
        }
    }
}
