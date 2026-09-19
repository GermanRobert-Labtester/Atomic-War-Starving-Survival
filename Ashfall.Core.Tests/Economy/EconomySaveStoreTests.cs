// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    /// <summary>
    /// Tests for the persistence and error-handling contract of EconomySaveStore:
    /// validating that missing files, empty files, malformed JSON, truncated payloads,
    /// tampered checksums, missing checksums, legacy bare migrations, and disk I/O faults
    /// are caught safely, logged to ILog.Error, and return fallback values (null / false)
    /// without throwing unhandled exceptions to callers.
    /// </summary>
    public class EconomySaveStoreTests : IDisposable
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

        [Serializable]
        public class EconomySaveEnvelope
        {
            public string Checksum = string.Empty;
            public MarketState State;
        }

        private readonly string _tempDir;
        private readonly CapturingLog _log = new CapturingLog();
        private readonly SystemTextJsonSerializer _json = new SystemTextJsonSerializer();
        private readonly FileSystemIO _fileIo = new FileSystemIO();

        public EconomySaveStoreTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "ashfall_economy_store_tests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDir))
            {
                try { Directory.Delete(_tempDir, true); } catch { /* cleanup test scratch */ }
            }
        }

        private static string EncodeState(MarketState state, IJsonSerializer json)
        {
            var envelope = new EconomySaveEnvelope
            {
                Checksum = SaveChecksum.Compute(state),
                State = state
            };
            return json.Serialize(envelope);
        }

        private static MarketState? DecodeState(string raw, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;
            var envelope = json.Deserialize<EconomySaveEnvelope>(raw);
            if (envelope != null && envelope.State != null)
            {
                if (string.IsNullOrEmpty(envelope.Checksum)) return null;
                // Tamper gate: recompute over the state; mismatch refuses the save.
                if (!string.Equals(SaveChecksum.Compute(envelope.State), envelope.Checksum,
                        StringComparison.Ordinal))
                    return null;
                return envelope.State;
            }

            // An envelope with a checksum field or state property is an envelope save;
            // if it failed the above validation it is corrupt/tampered, not a legacy save.
            if (envelope != null && (!string.IsNullOrEmpty(envelope.Checksum) || envelope.State != null))
                return null;
            if (raw.Contains("\"Checksum\"", StringComparison.OrdinalIgnoreCase))
                return null;

            // Legacy migration: a bare MarketState (pre-checksum store shape)
            // has no envelope; accept it so an upgrade never silently loses
            // the economy. Legacy saves carry no checksum by definition.
            if (!raw.Contains("\"demand\"", StringComparison.OrdinalIgnoreCase)
                && !raw.Contains("\"tickCount\"", StringComparison.OrdinalIgnoreCase)
                && !raw.Contains("\"systemId\"", StringComparison.OrdinalIgnoreCase))
                return null;

            var legacy = json.Deserialize<MarketState>(raw);
            if (legacy != null && !string.IsNullOrEmpty(legacy.systemId))
                return legacy;
            return null;
        }

        /// <summary>
        /// Creates a SaveStore configured identically to EconomySaveStore:
        /// same file name ("economy_save.json"), log tag ("EconomySaveStore"),
        /// and EncodeState/DecodeState delegates.
        /// </summary>
        private SaveStore<MarketState> CreateStore(IFileIO? fileIo = null, ILog? log = null)
        {
            return SaveStore<MarketState>.FromCodec(
                "economy_save.json",
                fileIo ?? _fileIo,
                _json,
                log ?? _log,
                () => _tempDir,
                "EconomySaveStore",
                EncodeState,
                DecodeState,
                createBackup: false);
        }

        private static MarketState CreateSampleState(int day = 5, long tickCount = 12)
        {
            return new MarketState
            {
                systemId = MarketSystem.SystemId,
                version = MarketState.Version,
                day = day,
                tickCount = tickCount,
                demand = new List<DemandEntry>
                {
                    new DemandEntry { itemId = "clean_water", multiplier = 1.25f },
                    new DemandEntry { itemId = "scrap_metal", multiplier = 0.90f }
                },
                ledger = new List<LedgerEntry>
                {
                    new LedgerEntry { day = day, itemId = "clean_water", quantity = 2, unitPrice = 10f, totalValue = 20f }
                }
            };
        }

        [Fact]
        public void TryLoad_NonExistentFile_ReturnsNullWithoutLoggingError()
        {
            var store = CreateStore();
            string nonExistentPath = Path.Combine(_tempDir, "absent_economy_save.json");

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
        public void TryLoad_MalformedJsonSyntax_CatchesException_LogsErrorAndReturnsNull()
        {
            var store = CreateStore();
            string corruptPath = Path.Combine(_tempDir, "corrupt.json");
            File.WriteAllText(corruptPath, "{ \"Checksum\": \"abc\", \"State\": malformed_syntax: [ }");

            var result = store.TryLoad(corruptPath);

            Assert.Null(result);
            Assert.Single(_log.ErrorMessages);
            Assert.Contains("[EconomySaveStore] load failed:", _log.ErrorMessages[0]);
        }

        [Fact]
        public void TryLoad_TruncatedJson_CatchesException_LogsErrorAndReturnsNull()
        {
            var store = CreateStore();
            string truncatedPath = Path.Combine(_tempDir, "truncated.json");
            File.WriteAllText(truncatedPath, "{\"Checksum\":\"abc\",\"State\":{\"day\":");

            var result = store.TryLoad(truncatedPath);

            Assert.Null(result);
            Assert.Single(_log.ErrorMessages);
            Assert.Contains("[EconomySaveStore] load failed:", _log.ErrorMessages[0]);
        }

        [Fact]
        public void TryLoad_InvalidJsonTokens_CatchesException_LogsErrorAndReturnsNull()
        {
            var store = CreateStore();
            string arrayPath = Path.Combine(_tempDir, "array.json");
            File.WriteAllText(arrayPath, "[ 1, 2, 3, \"unexpected_array\" ]");

            var result = store.TryLoad(arrayPath);

            Assert.Null(result);
            Assert.Single(_log.ErrorMessages);
            Assert.Contains("[EconomySaveStore] load failed:", _log.ErrorMessages[0]);
        }

        [Fact]
        public void TryLoad_TamperedPayload_ChecksumMismatch_ReturnsNull()
        {
            var store = CreateStore();
            var state = CreateSampleState(day: 5);
            Assert.True(store.TrySave(state));

            string savedPath = store.SavePath;
            string raw = File.ReadAllText(savedPath);
            // Tamper with day without updating checksum
            string tampered = raw.Replace("\"day\":5", "\"day\":99");
            Assert.NotEqual(raw, tampered);
            File.WriteAllText(savedPath, tampered);

            var loaded = store.TryLoad();

            Assert.Null(loaded);
        }

        [Fact]
        public void TryLoad_TamperedChecksum_ReturnsNull()
        {
            var store = CreateStore();
            var state = CreateSampleState(day: 3);
            Assert.True(store.TrySave(state));

            string savedPath = store.SavePath;
            string raw = File.ReadAllText(savedPath);
            string tampered = System.Text.RegularExpressions.Regex.Replace(
                raw, "\"Checksum\":\"[^\"]+\"", "\"Checksum\":\"forged_checksum_12345\"");
            Assert.NotEqual(raw, tampered);
            File.WriteAllText(savedPath, tampered);

            var loaded = store.TryLoad();

            Assert.Null(loaded);
        }

        [Fact]
        public void TryLoad_MissingChecksumField_ReturnsNull()
        {
            var store = CreateStore();
            string missingChecksumPath = Path.Combine(_tempDir, "missing_checksum.json");
            // Checksum property is empty
            string payload = "{\"Checksum\":\"\",\"State\":{\"systemId\":\"market_main\",\"day\":5,\"tickCount\":5}}";
            File.WriteAllText(missingChecksumPath, payload);

            var loaded = store.TryLoad(missingChecksumPath);

            Assert.Null(loaded);
        }

        [Fact]
        public void TryLoad_NullStateInEnvelope_ReturnsNull()
        {
            var store = CreateStore();
            string nullStatePath = Path.Combine(_tempDir, "null_state.json");
            string payload = "{\"Checksum\":\"some_hash\",\"State\":null}";
            File.WriteAllText(nullStatePath, payload);

            var loaded = store.TryLoad(nullStatePath);

            Assert.Null(loaded);
        }

        [Fact]
        public void TryLoad_LegacyBareState_WithValidSystemId_LoadsSuccessfully()
        {
            var store = CreateStore();
            string legacyPath = Path.Combine(_tempDir, "legacy.json");
            var legacy = new MarketState
            {
                systemId = MarketSystem.SystemId,
                version = MarketState.Version,
                day = 7,
                tickCount = 7,
                demand = new List<DemandEntry>
                {
                    new DemandEntry { itemId = "legacy_good", multiplier = 1.4f }
                }
            };
            File.WriteAllText(legacyPath, _json.Serialize(legacy));

            var loaded = store.TryLoad(legacyPath);

            Assert.NotNull(loaded);
            Assert.Equal(7, loaded!.day);
            Assert.Equal(7, loaded.tickCount);
            Assert.Single(loaded.demand);
            Assert.Equal("legacy_good", loaded.demand[0].itemId);
            Assert.Equal(1.4f, loaded.demand[0].multiplier);
        }

        [Fact]
        public void TryLoad_LegacyBareState_MissingOrEmptySystemId_ReturnsNull()
        {
            var store = CreateStore();
            string emptySystemIdPath = Path.Combine(_tempDir, "empty_sysid_legacy.json");
            string payload1 = "{\"systemId\":\"\",\"day\":7,\"tickCount\":7,\"demand\":[]}";
            File.WriteAllText(emptySystemIdPath, payload1);

            var loaded1 = store.TryLoad(emptySystemIdPath);
            Assert.Null(loaded1);

            string nonMarketPath = Path.Combine(_tempDir, "non_market.json");
            string payload2 = "{\"version\":3,\"some_arbitrary_key\":\"some_value\"}";
            File.WriteAllText(nonMarketPath, payload2);

            var loaded2 = store.TryLoad(nonMarketPath);
            Assert.Null(loaded2);
        }

        [Fact]
        public void TryLoad_FileIoThrowsIOException_CatchesException_LogsErrorAndReturnsNull()
        {
            var faultyIo = new FaultyFileIo(_fileIo) { ThrowOnRead = true };
            var store = CreateStore(fileIo: faultyIo);
            string filePath = Path.Combine(_tempDir, "economy_save.json");
            File.WriteAllText(filePath, "{}");

            var loaded = store.TryLoad(filePath);

            Assert.Null(loaded);
            Assert.Single(_log.ErrorMessages);
            Assert.Contains("[EconomySaveStore] load failed: Simulated disk read fault", _log.ErrorMessages[0]);
        }

        [Fact]
        public void TryLoad_CleanRoundTrip_PreservesMarketStateAndChecksum()
        {
            var store = CreateStore();
            var original = CreateSampleState(day: 10, tickCount: 25);

            bool saved = store.TrySave(original);
            Assert.True(saved);
            Assert.Equal(1, store.WriteCount);

            var loaded = store.TryLoad();
            Assert.NotNull(loaded);
            Assert.Equal(original.day, loaded!.day);
            Assert.Equal(original.tickCount, loaded.tickCount);
            Assert.Equal(original.demand.Count, loaded.demand.Count);
            Assert.Equal(original.demand[0].itemId, loaded.demand[0].itemId);
            Assert.Equal(original.demand[0].multiplier, loaded.demand[0].multiplier);
            Assert.Equal(original.ledger.Count, loaded.ledger.Count);
            Assert.Equal(original.ledger[0].quantity, loaded.ledger[0].quantity);
            Assert.Equal(original.ledger[0].totalValue, loaded.ledger[0].totalValue);
        }

        [Fact]
        public void TrySave_NullState_ReturnsFalse()
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
            var state = CreateSampleState(day: 1);

            bool saved = store.TrySave(state);

            Assert.False(saved);
            Assert.Equal(0, store.WriteCount);
            Assert.NotEmpty(_log.ErrorMessages);
            Assert.Contains("Simulated disk write fault", _log.ErrorMessages[0]);
        }

        [Fact]
        public void RestoreBare_MalformedJson_LogsErrorAndReturnsNull()
        {
            var store = CreateStore();
            var restored = store.RestoreBare("{ invalid json ");

            Assert.Null(restored);
            Assert.Single(_log.ErrorMessages);
            Assert.Contains("[EconomySaveStore] restore failed:", _log.ErrorMessages[0]);
        }

        [Fact]
        public void CaptureBare_NullState_ReturnsEmptyString()
        {
            var store = CreateStore();
            string json = store.CaptureBare(null!);
            Assert.Equal(string.Empty, json);
        }
    }
}
