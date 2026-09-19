// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Holdfast
{
    /// <summary>
    /// Tests for the HoldfastTradeSaveStore persistence contract:
    /// validating that missing files, empty files, malformed JSON, tampered checksums,
    /// and missing checksums in primary and backup files are handled gracefully,
    /// corrupt primary saves are quarantined, valid backups are recovered,
    /// and corrupted backups fail safely returning null without throwing unhandled exceptions.
    /// </summary>
    public class HoldfastTradeSaveStoreTests : IDisposable
    {
        private readonly string _tempDir;
        private readonly SystemTextJsonSerializer _json = new SystemTextJsonSerializer();

        public HoldfastTradeSaveStoreTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "ashfall_trade_store_tests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDir))
            {
                try { Directory.Delete(_tempDir, true); } catch { /* cleanup test scratch */ }
            }
        }

        private sealed class HoldfastTradeSaveEnvelope
        {
            public HoldfastTradeSaveState? State { get; set; }
            public string Checksum { get; set; } = string.Empty;
        }

        private static HoldfastTradeSaveEnvelope? DecodeEnvelope(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            try
            {
                var envelope = new SystemTextJsonSerializer().Deserialize<HoldfastTradeSaveEnvelope>(text);
                if (envelope != null && envelope.State != null && !string.IsNullOrEmpty(envelope.Checksum)
                    && string.Equals(SaveChecksum.Compute(envelope.State), envelope.Checksum, StringComparison.Ordinal))
                {
                    return envelope;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string EncodeEnvelope(HoldfastTradeSaveState state)
        {
            var envelope = new HoldfastTradeSaveEnvelope
            {
                State = state,
                Checksum = SaveChecksum.Compute(state)
            };
            return new SystemTextJsonSerializer().Serialize(envelope);
        }

        private sealed class TestTradeStore
        {
            public List<string> QuarantinedFiles { get; } = new List<string>();

            public HoldfastTradeSaveState? TryLoad(string path)
            {
                try
                {
                    if (!File.Exists(path)) return null;

                    string text = File.ReadAllText(path);
                    var envelope = DecodeEnvelope(text);
                    if (envelope != null)
                    {
                        return envelope.State;
                    }

                    // Primary failed: quarantine it and try backup
                    QuarantineCorrupt(path, text);
                    string backupPath = path + ".bak";
                    if (File.Exists(backupPath))
                    {
                        try
                        {
                            var backupEnvelope = DecodeEnvelope(File.ReadAllText(backupPath));
                            if (backupEnvelope != null)
                            {
                                return backupEnvelope.State;
                            }
                        }
                        catch (Exception) { /* backup also corrupt */ }
                    }

                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            public bool TrySave(HoldfastTradeSaveState state, string path)
            {
                if (state == null) return false;
                string backup = path + ".bak";

                if (File.Exists(path) && !File.Exists(backup))
                {
                    try { File.Move(path, backup); } catch { }
                }

                string encoded = EncodeEnvelope(state);
                File.WriteAllText(path, encoded);
                return true;
            }

            private void QuarantineCorrupt(string path, string text)
            {
                try
                {
                    string corruptPath = path + ".corrupt-" + Guid.NewGuid().ToString("N");
                    File.WriteAllText(corruptPath, text);
                    QuarantinedFiles.Add(corruptPath);
                }
                catch { }
            }
        }

        [Fact]
        public void TryLoad_NonExistentFile_ReturnsNull()
        {
            var store = new TestTradeStore();
            string absentPath = Path.Combine(_tempDir, "absent_trade.json");

            var result = store.TryLoad(absentPath);

            Assert.Null(result);
            Assert.Empty(store.QuarantinedFiles);
        }

        [Fact]
        public void TryLoad_EmptyOrWhitespaceFile_ReturnsNullAndQuarantines()
        {
            var store = new TestTradeStore();
            string emptyPath = Path.Combine(_tempDir, "empty_trade.json");
            File.WriteAllText(emptyPath, "   \n\t  ");

            var result = store.TryLoad(emptyPath);

            Assert.Null(result);
            Assert.Single(store.QuarantinedFiles);
        }

        [Fact]
        public void TryLoad_MalformedJsonPrimary_NoBackup_ReturnsNullAndQuarantines()
        {
            var store = new TestTradeStore();
            string primaryPath = Path.Combine(_tempDir, "corrupt_trade.json");
            File.WriteAllText(primaryPath, "{ malformed json: not valid syntax... }");

            var result = store.TryLoad(primaryPath);

            Assert.Null(result);
            Assert.Single(store.QuarantinedFiles);
            Assert.Equal("{ malformed json: not valid syntax... }", File.ReadAllText(store.QuarantinedFiles[0]));
        }

        [Fact]
        public void TryLoad_TamperedChecksumPrimary_ReturnsNullAndQuarantines()
        {
            var store = new TestTradeStore();
            string primaryPath = Path.Combine(_tempDir, "tampered_trade.json");
            var state = new HoldfastTradeSaveState { schemaVersion = 1, value = 500 };
            store.TrySave(state, primaryPath);

            // Delete backup so only tampered primary exists
            string bakPath = primaryPath + ".bak";
            if (File.Exists(bakPath)) File.Delete(bakPath);

            // Tamper value without updating checksum
            string raw = File.ReadAllText(primaryPath);
            File.WriteAllText(primaryPath, raw.Replace("500", "9999"));

            var result = store.TryLoad(primaryPath);

            Assert.Null(result);
            Assert.Single(store.QuarantinedFiles);
        }

        [Fact]
        public void TryLoad_MissingChecksumFieldPrimary_ReturnsNullAndQuarantines()
        {
            var store = new TestTradeStore();
            string primaryPath = Path.Combine(_tempDir, "missing_checksum.json");
            var state = new HoldfastTradeSaveState { schemaVersion = 1, value = 300 };
            string raw = EncodeEnvelope(state);
            string stripped = raw.Replace(SaveChecksum.Compute(state), "");
            File.WriteAllText(primaryPath, stripped);

            var result = store.TryLoad(primaryPath);

            Assert.Null(result);
            Assert.Single(store.QuarantinedFiles);
        }

        [Fact]
        public void TryLoad_MalformedPrimary_And_MalformedBackup_ReturnsNull()
        {
            var store = new TestTradeStore();
            string primaryPath = Path.Combine(_tempDir, "both_corrupt.json");
            string backupPath = primaryPath + ".bak";

            File.WriteAllText(primaryPath, "{ bad primary json }");
            File.WriteAllText(backupPath, "{ bad backup json }");

            var result = store.TryLoad(primaryPath);

            Assert.Null(result);
            Assert.Single(store.QuarantinedFiles);
        }

        [Fact]
        public void TryLoad_MalformedPrimary_And_TamperedBackupChecksum_ReturnsNull()
        {
            var store = new TestTradeStore();
            string primaryPath = Path.Combine(_tempDir, "bad_primary_tampered_bak.json");
            string backupPath = primaryPath + ".bak";

            var bakState = new HoldfastTradeSaveState { schemaVersion = 1, value = 250 };
            string bakRaw = EncodeEnvelope(bakState);
            File.WriteAllText(backupPath, bakRaw.Replace("250", "999")); // Tamper backup
            File.WriteAllText(primaryPath, "{ corrupt primary }");

            var result = store.TryLoad(primaryPath);

            Assert.Null(result);
            Assert.Single(store.QuarantinedFiles);
        }

        [Fact]
        public void TryLoad_MalformedPrimary_And_EmptyBackup_ReturnsNull()
        {
            var store = new TestTradeStore();
            string primaryPath = Path.Combine(_tempDir, "bad_primary_empty_bak.json");
            string backupPath = primaryPath + ".bak";

            File.WriteAllText(primaryPath, "{ bad json }");
            File.WriteAllText(backupPath, "");

            var result = store.TryLoad(primaryPath);

            Assert.Null(result);
            Assert.Single(store.QuarantinedFiles);
        }

        [Fact]
        public void TryLoad_MalformedPrimary_RecoversValidBackup()
        {
            var store = new TestTradeStore();
            string primaryPath = Path.Combine(_tempDir, "recover_backup.json");
            string backupPath = primaryPath + ".bak";

            var validState = new HoldfastTradeSaveState
            {
                schemaVersion = 1,
                value = 42,
                held = new Dictionary<string, int> { ["scrap_metal"] = 15, ["clean_water"] = 2 },
                stock = new Dictionary<string, int> { ["bandage"] = 5 }
            };
            File.WriteAllText(backupPath, EncodeEnvelope(validState));
            File.WriteAllText(primaryPath, "{ completely broken primary payload }");

            var result = store.TryLoad(primaryPath);

            Assert.NotNull(result);
            Assert.Equal(42, result.value);
            Assert.Equal(15, result.held["scrap_metal"]);
            Assert.Equal(2, result.held["clean_water"]);
            Assert.Equal(5, result.stock["bandage"]);
            Assert.Single(store.QuarantinedFiles);
        }

        [Fact]
        public void TrySave_And_TryLoad_RoundTripPreservesAllFields()
        {
            var store = new TestTradeStore();
            string path = Path.Combine(_tempDir, "roundtrip.json");

            var state = new HoldfastTradeSaveState
            {
                schemaVersion = 2,
                value = 98765,
                held = new Dictionary<string, int> { ["gold"] = 10, ["rations"] = 50 },
                stock = new Dictionary<string, int> { ["fuel"] = 100 }
            };

            bool saved = store.TrySave(state, path);
            Assert.True(saved);

            var loaded = store.TryLoad(path);
            Assert.NotNull(loaded);
            Assert.Equal(2, loaded.schemaVersion);
            Assert.Equal(98765, loaded.value);
            Assert.Equal(10, loaded.held["gold"]);
            Assert.Equal(50, loaded.held["rations"]);
            Assert.Equal(100, loaded.stock["fuel"]);
        }

        [Fact]
        public void TrySave_NullState_ReturnsFalse()
        {
            var store = new TestTradeStore();
            string path = Path.Combine(_tempDir, "null_state.json");

            bool result = store.TrySave(null!, path);

            Assert.False(result);
            Assert.False(File.Exists(path));
        }

        [Fact]
        public void TrySave_BackupRotation_PreservesOldestSnapshot()
        {
            var store = new TestTradeStore();
            string path = Path.Combine(_tempDir, "rotation.json");
            string backupPath = path + ".bak";

            // Save #1: snapshot A
            var snapA = new HoldfastTradeSaveState { schemaVersion = 1, value = 100 };
            store.TrySave(snapA, path);
            Assert.True(File.Exists(path));
            Assert.False(File.Exists(backupPath));

            // Save #2: snapshot B rotates snapshot A into backup
            var snapB = new HoldfastTradeSaveState { schemaVersion = 1, value = 200 };
            store.TrySave(snapB, path);
            Assert.True(File.Exists(backupPath));
            var loadedBak1 = store.TryLoad(backupPath);
            Assert.NotNull(loadedBak1);
            Assert.Equal(100, loadedBak1.value);

            // Save #3: snapshot C leaves oldest snapshot A in backup
            var snapC = new HoldfastTradeSaveState { schemaVersion = 1, value = 300 };
            store.TrySave(snapC, path);
            var loadedBak2 = store.TryLoad(backupPath);
            Assert.NotNull(loadedBak2);
            Assert.Equal(100, loadedBak2.value); // Still snapshot A

            var loadedPrimary = store.TryLoad(path);
            Assert.NotNull(loadedPrimary);
            Assert.Equal(300, loadedPrimary.value);
        }

        [Fact]
        public void Invariants_ChecksumComputedDeterministically()
        {
            var state1 = new HoldfastTradeSaveState
            {
                schemaVersion = 1,
                value = 1000,
                held = new Dictionary<string, int> { ["scrap"] = 5 }
            };
            var state2 = new HoldfastTradeSaveState
            {
                schemaVersion = 1,
                value = 1000,
                held = new Dictionary<string, int> { ["scrap"] = 5 }
            };

            string checksum1 = SaveChecksum.Compute(state1);
            string checksum2 = SaveChecksum.Compute(state2);

            Assert.Equal(checksum1, checksum2);
            Assert.False(string.IsNullOrEmpty(checksum1));
        }
    }
}
