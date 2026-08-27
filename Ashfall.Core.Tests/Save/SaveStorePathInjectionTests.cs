// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: Save Store Path Injectability & Scoped Overrides.
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class SaveStorePathInjectionTests
    {
        [Serializable]
        public class TestSavePayload
        {
            public int Day { get; set; }
            public string Note { get; set; } = string.Empty;
        }

        [Fact]
        public void SaveStore_WithBaseDirectory_RoutesToCustomDirectory()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "ashfall_test_savedir_" + Guid.NewGuid().ToString("N"));
            try
            {
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                var log = new ConsoleLog();

                var defaultStore = new SaveStore<TestSavePayload>(
                    "test_save.json",
                    files,
                    json,
                    log,
                    () => "/default/path",
                    "TestTag");

                Assert.Equal(Path.Combine("/default/path", "test_save.json"), defaultStore.SavePath);

                var customStore = defaultStore.WithBaseDirectory(() => tempDir);
                Assert.Equal(Path.Combine(tempDir, "test_save.json"), customStore.SavePath);

                var payload = new TestSavePayload { Day = 42, Note = "Injectable Path" };
                bool saved = customStore.TrySave(payload);
                Assert.True(saved);
                Assert.True(File.Exists(customStore.SavePath));

                var loaded = customStore.TryLoad();
                Assert.NotNull(loaded);
                Assert.Equal(42, loaded!.Day);
                Assert.Equal("Injectable Path", loaded.Note);
            }
            finally
            {
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, recursive: true);
            }
        }

        [Fact]
        public void SaveStore_PathOverride_OverridesDefaultSavePathPerCall()
        {
            string tempFile = Path.Combine(Path.GetTempPath(), "ashfall_override_" + Guid.NewGuid().ToString("N") + ".json");
            try
            {
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                var log = new ConsoleLog();

                var store = new SaveStore<TestSavePayload>(
                    "test_save.json",
                    files,
                    json,
                    log,
                    () => "/unused/default/path",
                    "TestTag");

                var payload = new TestSavePayload { Day = 101, Note = "Per-call override" };
                bool saved = store.TrySave(payload, pathOverride: tempFile);
                Assert.True(saved);
                Assert.True(File.Exists(tempFile));

                var loaded = store.TryLoad(pathOverride: tempFile);
                Assert.NotNull(loaded);
                Assert.Equal(101, loaded!.Day);
                Assert.Equal("Per-call override", loaded.Note);
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        [Fact]
        public void SaveStore_CodecDelegation_SupportsWithBaseDirectory()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "ashfall_codec_dir_" + Guid.NewGuid().ToString("N"));
            try
            {
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                var log = new ConsoleLog();

                var defaultStore = SaveStore<TestSavePayload>.FromCodec(
                    "codec_test.json",
                    files,
                    json,
                    log,
                    () => "/default/codec/path",
                    "CodecTag",
                    encode: (state, s) => s.Serialize(state),
                    decode: (raw, s) => s.Deserialize<TestSavePayload>(raw));

                Assert.Equal(Path.Combine("/default/codec/path", "codec_test.json"), defaultStore.SavePath);

                var customStore = defaultStore.WithBaseDirectory(() => tempDir);
                Assert.Equal(Path.Combine(tempDir, "codec_test.json"), customStore.SavePath);

                var payload = new TestSavePayload { Day = 77, Note = "Codec state" };
                bool saved = customStore.TrySave(payload);
                Assert.True(saved);
                Assert.True(File.Exists(customStore.SavePath));

                var loaded = customStore.TryLoad();
                Assert.NotNull(loaded);
                Assert.Equal(77, loaded!.Day);
                Assert.Equal("Codec state", loaded.Note);
            }
            finally
            {
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, recursive: true);
            }
        }
    }
}
