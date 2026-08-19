using NUnit.Framework;
using System.IO;
using AtomicWar.GodotApp;
using Ashfall.Core;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class HoldfastTradeSaveStoreTests
    {
        private string _tempDir;
        private string _testSavePath;
        private string _testBackupPath;

        [SetUp]
        public void SetUp()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), $"holdfast_test_{System.Guid.NewGuid():N}");
            Directory.CreateDirectory(_tempDir);
            _testSavePath = Path.Combine(_tempDir, HoldfastTradeSaveStore.FileName);
            _testBackupPath = _testSavePath + ".bak";
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_tempDir))
            {
                Directory.Delete(_tempDir, true);
            }
        }

        [Test]
        public void Load_ReturnsNull_WhenMainSaveIsCorruptAndNoBackup()
        {
            File.WriteAllText(_testSavePath, "{ corrupted json... }");

            var state = HoldfastTradeSaveStore.TryLoad(_testSavePath);

            Assert.IsNull(state, "Load should return null when the main save is corrupt and there is no backup.");
        }

        [Test]
        public void Load_ReturnsNull_WhenMainSaveAndBackupAreCorrupt()
        {
            File.WriteAllText(_testSavePath, "{ corrupted json... }");
            File.WriteAllText(_testBackupPath, "{ also corrupted json... }");

            var state = HoldfastTradeSaveStore.TryLoad(_testSavePath);

            Assert.IsNull(state, "Load should return null when both main and backup saves are corrupt.");
        }

        [Test]
        public void Load_RecoversFromBackup_WhenMainSaveIsCorrupt()
        {
            // Create a valid state
            var state = new HoldfastTradeSaveState { PlayerValue = 42 };
            // Save it to backup
            HoldfastTradeSaveStore.TrySave(state, _testBackupPath);
            // Replace the main save with corrupt data
            File.WriteAllText(_testSavePath, "{ corrupted json... }");

            var loadedState = HoldfastTradeSaveStore.TryLoad(_testSavePath);

            Assert.IsNotNull(loadedState, "Load should recover from backup.");
            Assert.AreEqual(42, loadedState.PlayerValue, "Recovered state should match the backup data.");
        }
    }
}
