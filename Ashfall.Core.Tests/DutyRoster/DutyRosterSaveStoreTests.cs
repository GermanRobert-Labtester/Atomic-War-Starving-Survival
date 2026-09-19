// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.DutyRoster
{
    /// <summary>
    /// Tests for the try-catch error handling and persistence contract of DutyRosterSaveStore:
    /// validating that corrupt JSON, truncated payloads, tampered checksums, invalid versions,
    /// missing files, and I/O exceptions are caught, logged to ILog.Error, and return fallback
    /// values (null or false) without throwing unhandled exceptions to callers.
    /// </summary>
    public class DutyRosterSaveStoreTests : IDisposable
    {
        private sealed class CapturingLog : ILog
        {
            public List<string> Messages { get; } = new List<string>();
            public List<string> ErrorMessages { get; } = new List<string>();
            public void Info(string message) => Messages.Add("[INFO] " + message);
            public void Warn(string message) => Messages.Add("[WARN] " + message);
            public void Error(string message)
            {
                Messages.Add("[ERROR] " + message);
                ErrorMessages.Add(message);
            }
        }

        private sealed class FaultyFileIo : IFileIO
        {
            private readonly IFileIO _inner;
            public bool ThrowOnRead { get; set; }
            public bool ThrowOnWrite { get; set; }

            public FaultyFileIo(IFileIO inner) => _inner = inner;

            public bool DirectoryExists(string path) => _inner.DirectoryExists(path);
            public bool FileExists(string path) => _inner.FileExists(path);
            public string ReadAllText(string path)
            {
                if (ThrowOnRead) throw new IOException("Simulated disk read fault");
                return _inner.ReadAllText(path);
            }
            public void WriteAllText(string path, string contents)
            {
                if (ThrowOnWrite) throw new IOException("Simulated disk write fault");
                _inner.WriteAllText(path, contents);
            }
            public string Combine(params string[] parts) => _inner.Combine(parts);
            public void CreateDirectory(string path) => _inner.CreateDirectory(path);
            public void DeleteFile(string path) => _inner.DeleteFile(path);
            public string[] EnumerateFiles(string directory, string searchPattern, SearchOption searchOption) =>
                _inner.EnumerateFiles(directory, searchPattern, searchOption);
            public string[] GetDirectories(string path, string searchPattern = "*") =>
                _inner.GetDirectories(path, searchPattern);
        }

        private readonly string _tempDir;
        private readonly CapturingLog _log = new CapturingLog();
        private readonly SystemTextJsonSerializer _json = new SystemTextJsonSerializer();
        private readonly FileSystemIO _fileIo = new FileSystemIO();

        public DutyRosterSaveStoreTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "ashfall_duty_roster_store_tests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDir))
            {
                try { Directory.Delete(_tempDir, true); } catch { /* cleanup test scratch */ }
            }
        }

        /// <summary>
        /// Creates a SaveStore configured identically to DutyRosterSaveStore:
        /// same file name ("duty_roster_save.json"), log tag ("DutyRosterSaveStore"),
        /// and DutyRosterSaveCodec encode/decode delegates.
        /// </summary>
        private SaveStore<DutyRosterSave> CreateStore(IFileIO? fileIo = null, ILog? log = null)
        {
            return SaveStore<DutyRosterSave>.FromCodec(
                "duty_roster_save.json",
                fileIo ?? _fileIo,
                _json,
                log ?? _log,
                () => _tempDir,
                "DutyRosterSaveStore",
                (save, json) => DutyRosterSaveCodec.Encode(save, json),
                (raw, json) => DutyRosterSaveCodec.Decode(raw, json),
                createBackup: false);
        }

        private static DutyRosterSave CreateSampleSave(int day = 7)
        {
            var roster = new DutyRosterSystem(908);
            roster.Unlock(day);
            var marks = new MoraleMarkSystem();
            marks.SetMark("mark_ration_protocol", "payload", day);
            var encounters = new ShelterEncounterSystem(908);
            encounters.Unlock(day);
            var clock = new SimClock(day);
            var quests = new DutyRosterQuestRuntime();
            return DutyRosterSaveCodec.Capture(roster, marks, encounters, clock, quests);
        }

        [Fact]
        public void TryLoad_NonExistentFile_ReturnsNullWithoutLoggingError()
        {
            var store = CreateStore();
            string nonExistentPath = Path.Combine(_tempDir, "absent_file.json");

            var result = store.TryLoad(nonExistentPath);

            Assert.Null(result);
            Assert.Empty(_log.ErrorMessages);
        }

        [Fact]
        public void TryLoad_EmptyOrWhitespaceFile_ReturnsNullWithoutLoggingError()
        {
            var store = CreateStore();
            string emptyFile = Path.Combine(_tempDir, "empty.json");
            File.WriteAllText(emptyFile, "   \n\t  ");

            var result = store.TryLoad(emptyFile);

            Assert.Null(result);
            Assert.Empty(_log.ErrorMessages);
        }

        [Fact]
        public void TryLoad_MalformedJson_CatchesException_LogsErrorAndReturnsNull()
        {
            var store = CreateStore();
            string corruptPath = Path.Combine(_tempDir, "corrupt.json");
            File.WriteAllText(corruptPath, "{ \"saveVersion\": 3, invalid_json: [ }");

            var result = store.TryLoad(corruptPath);

            Assert.Null(result);
            Assert.Single(_log.ErrorMessages);
            Assert.Contains("[DutyRosterSaveStore] load failed:", _log.ErrorMessages[0]);
            Assert.Contains("malformed save payload", _log.ErrorMessages[0]);
        }

        [Fact]
        public void TryLoad_TruncatedJson_CatchesException_LogsErrorAndReturnsNull()
        {
            var store = CreateStore();
            string truncatedPath = Path.Combine(_tempDir, "truncated.json");
            File.WriteAllText(truncatedPath, "{\"saveVersion\":3,\"simDay\":");

            var result = store.TryLoad(truncatedPath);

            Assert.Null(result);
            Assert.Single(_log.ErrorMessages);
            Assert.Contains("[DutyRosterSaveStore] load failed:", _log.ErrorMessages[0]);
        }

        [Fact]
        public void TryLoad_TamperedPayloadChecksumMismatch_CatchesException_LogsErrorAndReturnsNull()
        {
            var store = CreateStore();
            var save = CreateSampleSave(day: 5);
            Assert.True(store.TrySave(save));

            string savedPath = store.SavePath;
            string raw = File.ReadAllText(savedPath);
            // Tamper with simDay without updating checksum
            string tampered = raw.Replace("\"simDay\":5", "\"simDay\":99");
            Assert.NotEqual(raw, tampered);
            File.WriteAllText(savedPath, tampered);

            var loaded = store.TryLoad();

            Assert.Null(loaded);
            Assert.Single(_log.ErrorMessages);
            Assert.Contains("[DutyRosterSaveStore] load failed:", _log.ErrorMessages[0]);
            Assert.Contains("checksum mismatch", _log.ErrorMessages[0]);
        }

        [Fact]
        public void TryLoad_MissingChecksum_CatchesException_LogsErrorAndReturnsNull()
        {
            var store = CreateStore();
            string missingChecksumPath = Path.Combine(_tempDir, "missing_checksum.json");
            string payload = "{\"saveVersion\":3,\"simDay\":5,\"Checksum\":\"\"}";
            File.WriteAllText(missingChecksumPath, payload);

            var loaded = store.TryLoad(missingChecksumPath);

            Assert.Null(loaded);
            Assert.Single(_log.ErrorMessages);
            Assert.Contains("[DutyRosterSaveStore] load failed:", _log.ErrorMessages[0]);
            Assert.Contains("carries no checksum", _log.ErrorMessages[0]);
        }

        [Fact]
        public void TryLoad_UnsupportedFutureSaveVersion_CatchesException_LogsErrorAndReturnsNull()
        {
            var store = CreateStore();
            string futureVersionPath = Path.Combine(_tempDir, "future_version.json");
            string payload = "{\"saveVersion\":999,\"simDay\":5,\"Checksum\":\"fake_checksum\"}";
            File.WriteAllText(futureVersionPath, payload);

            var loaded = store.TryLoad(futureVersionPath);

            Assert.Null(loaded);
            Assert.Single(_log.ErrorMessages);
            Assert.Contains("[DutyRosterSaveStore] load failed:", _log.ErrorMessages[0]);
            Assert.Contains("newer than this build supports", _log.ErrorMessages[0]);
        }

        [Fact]
        public void TryLoad_InvalidNegativeSaveVersion_CatchesException_LogsErrorAndReturnsNull()
        {
            var store = CreateStore();
            string negativeVersionPath = Path.Combine(_tempDir, "negative_version.json");
            string payload = "{\"saveVersion\":-1,\"simDay\":5,\"Checksum\":\"fake_checksum\"}";
            File.WriteAllText(negativeVersionPath, payload);

            var loaded = store.TryLoad(negativeVersionPath);

            Assert.Null(loaded);
            Assert.Single(_log.ErrorMessages);
            Assert.Contains("[DutyRosterSaveStore] load failed:", _log.ErrorMessages[0]);
            Assert.Contains("not a valid version", _log.ErrorMessages[0]);
        }

        [Fact]
        public void TryLoad_FileIoThrowsIOException_CatchesException_LogsErrorAndReturnsNull()
        {
            var faultyIo = new FaultyFileIo(_fileIo) { ThrowOnRead = true };
            var store = CreateStore(fileIo: faultyIo);
            string filePath = Path.Combine(_tempDir, "duty_roster_save.json");
            File.WriteAllText(filePath, "{}");

            var loaded = store.TryLoad(filePath);

            Assert.Null(loaded);
            Assert.Single(_log.ErrorMessages);
            Assert.Contains("[DutyRosterSaveStore] load failed: Simulated disk read fault", _log.ErrorMessages[0]);
        }

        [Fact]
        public void TrySave_NullState_ReturnsFalseWithoutThrowing()
        {
            var store = CreateStore();

            bool result = store.TrySave(null!);

            Assert.False(result);
            Assert.Equal(0, store.WriteCount);
        }

        [Fact]
        public void TrySave_FileIoThrowsIOException_CatchesException_LogsErrorAndReturnsFalse()
        {
            var faultyIo = new FaultyFileIo(_fileIo) { ThrowOnWrite = true };
            var store = CreateStore(fileIo: faultyIo);
            var save = CreateSampleSave();

            bool result = store.TrySave(save);

            Assert.False(result);
            Assert.Equal(0, store.WriteCount);
            Assert.Single(_log.ErrorMessages);
            Assert.Contains("[DutyRosterSaveStore] Save failed", _log.ErrorMessages[0]);
        }

        [Fact]
        public void TryRestoreBare_MalformedJson_CatchesException_LogsErrorAndReturnsNull()
        {
            var store = CreateStore();

            var result = store.RestoreBare("{ invalid JSON syntax }}}");

            Assert.Null(result);
            Assert.Single(_log.ErrorMessages);
            Assert.Contains("[DutyRosterSaveStore] restore failed:", _log.ErrorMessages[0]);
        }

        [Fact]
        public void TryRestoreBare_NullOrWhitespace_ReturnsNullWithoutLoggingError()
        {
            var store = CreateStore();

            Assert.Null(store.RestoreBare(null!));
            Assert.Null(store.RestoreBare(""));
            Assert.Null(store.RestoreBare("   \t\n "));
            Assert.Empty(_log.ErrorMessages);
        }

        [Fact]
        public void CaptureBare_NullState_ReturnsEmptyString()
        {
            var store = CreateStore();

            string captured = store.CaptureBare(null!);

            Assert.Equal(string.Empty, captured);
        }

        [Fact]
        public void RoundTrip_ValidSave_EncodesAndDecodesSuccessfully_IncrementsWriteCount()
        {
            var store = CreateStore();
            var original = CreateSampleSave(day: 12);

            bool saved = store.TrySave(original);
            Assert.True(saved);
            Assert.Equal(1, store.WriteCount);

            var loaded = store.TryLoad();
            Assert.NotNull(loaded);
            Assert.Equal(12, loaded!.simDay);
            Assert.Equal(DutyRosterSave.CurrentSaveVersion, loaded.saveVersion);
            Assert.False(string.IsNullOrEmpty(loaded.Checksum));
            Assert.True(loaded.roster.expansionUnlocked);
            Assert.True(loaded.encounters.expansionUnlocked);
            Assert.Single(loaded.marks.marks);
            Assert.Equal("mark_ration_protocol", loaded.marks.marks[0].id);
            Assert.Empty(_log.ErrorMessages);
        }
    }
}
