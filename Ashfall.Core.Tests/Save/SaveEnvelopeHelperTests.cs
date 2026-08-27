using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Save
{
    public class SampleSaveState
    {
        public int Day;
        public string Name = string.Empty;
        public int Score;
    }

    public class SaveEnvelopeHelperTests : IDisposable
    {
        private readonly string _tempDir;

        public SaveEnvelopeHelperTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "AshfallSaveHelperTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDir))
            {
                try { Directory.Delete(_tempDir, true); } catch { }
            }
        }

        [Fact]
        public void RoundTrip_SaveAndLoad_Succeeds()
        {
            string path = Path.Combine(_tempDir, "test_save.json");
            var original = new SampleSaveState { Day = 42, Name = "Bunker Alpha", Score = 999 };

            bool saved = SaveEnvelopeHelper.TrySaveAtomic(path, original);
            Assert.True(saved);
            Assert.True(File.Exists(path));

            var (success, loaded, error) = SaveEnvelopeHelper.TryLoad<SampleSaveState>(path);
            Assert.True(success);
            Assert.Null(error);
            Assert.NotNull(loaded);
            Assert.Equal(42, loaded!.Day);
            Assert.Equal("Bunker Alpha", loaded.Name);
            Assert.Equal(999, loaded.Score);
        }

        [Fact]
        public void TamperedEnvelope_IsRejectedWithChecksumMismatch()
        {
            string path = Path.Combine(_tempDir, "tampered_save.json");
            var original = new SampleSaveState { Day = 10, Name = "Shelter", Score = 100 };

            Assert.True(SaveEnvelopeHelper.TrySaveAtomic(path, original));

            // Tamper file content (modify score while keeping old checksum)
            string raw = File.ReadAllText(path);
            string tampered = raw.Replace("100", "999999");
            Assert.NotEqual(raw, tampered);
            File.WriteAllText(path, tampered);

            var (success, loaded, error) = SaveEnvelopeHelper.TryLoad<SampleSaveState>(path);
            Assert.False(success);
            Assert.Null(loaded);
            Assert.NotNull(error);
            Assert.Contains("Checksum mismatch", error, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void MissingChecksum_IsRejected()
        {
            string path = Path.Combine(_tempDir, "missing_checksum.json");
            string jsonWithoutChecksum = "{\"State\": {\"Day\": 5, \"Name\": \"Test\", \"Score\": 50}, \"Checksum\": \"\"}";
            File.WriteAllText(path, jsonWithoutChecksum);

            var (success, loaded, error) = SaveEnvelopeHelper.TryLoad<SampleSaveState>(path);
            Assert.False(success);
            Assert.Null(loaded);
            Assert.Contains("missing", error, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void LegacyBareState_LoadsGracefullyViaFallback()
        {
            string path = Path.Combine(_tempDir, "legacy_save.json");
            string bareJson = "{\"Day\": 7, \"Name\": \"Legacy Bunker\", \"Score\": 250}";
            File.WriteAllText(path, bareJson);

            var (success, loaded, error) = SaveEnvelopeHelper.TryLoad<SampleSaveState>(path);
            Assert.True(success);
            Assert.Null(error);
            Assert.NotNull(loaded);
            Assert.Equal(7, loaded!.Day);
            Assert.Equal("Legacy Bunker", loaded.Name);
            Assert.Equal(250, loaded.Score);
        }

        [Fact]
        public void BackupFile_IsCreatedOnOverwrite()
        {
            string path = Path.Combine(_tempDir, "backup_test.json");
            var v1 = new SampleSaveState { Day = 1, Name = "V1", Score = 10 };
            var v2 = new SampleSaveState { Day = 2, Name = "V2", Score = 20 };

            Assert.True(SaveEnvelopeHelper.TrySaveAtomic(path, v1));
            Assert.True(File.Exists(path));
            Assert.False(File.Exists(path + ".bak"));

            Assert.True(SaveEnvelopeHelper.TrySaveAtomic(path, v2, createBackup: true));
            Assert.True(File.Exists(path));
            Assert.True(File.Exists(path + ".bak"));

            var (bakSuccess, bakState, _) = SaveEnvelopeHelper.TryLoad<SampleSaveState>(path + ".bak");
            Assert.True(bakSuccess);
            Assert.Equal("V1", bakState!.Name);

            var (v2Success, v2State, _) = SaveEnvelopeHelper.TryLoad<SampleSaveState>(path);
            Assert.True(v2Success);
            Assert.Equal("V2", v2State!.Name);
        }

        [Fact]
        public void InMenuCaptureAndRestore_SucceedsWithoutDisk()
        {
            var original = new SampleSaveState { Day = 100, Name = "Memory Bunker", Score = 5000 };

            string json = SaveEnvelopeHelper.CaptureEnvelope(original);
            Assert.False(string.IsNullOrWhiteSpace(json));
            Assert.Contains("Checksum", json);

            var (success, restored, error) = SaveEnvelopeHelper.RestoreEnvelope<SampleSaveState>(json);
            Assert.True(success);
            Assert.Null(error);
            Assert.NotNull(restored);
            Assert.Equal(100, restored!.Day);
            Assert.Equal("Memory Bunker", restored.Name);
            Assert.Equal(5000, restored.Score);
        }
    }
}
