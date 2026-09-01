using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.IO;
using Ashfall.Core.Verdict;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// H4 regression coverage: the YearOfAsh and Verdict catalog loaders must
    /// never swallow a malformed authored file silently. Every parse failure
    /// routes through CatalogDiagnostics (an ILog sink) carrying the file path
    /// and the attempted JSON shape, while valid and missing-file behavior stays
    /// identical to the pre-H4 baseline.
    /// </summary>
    public sealed class CatalogLoaderHardeningTests : IDisposable
    {
        private readonly string _scratch;
        private readonly List<string> _messages;

        public CatalogLoaderHardeningTests()
        {
            _scratch = Path.Combine(Path.GetTempPath(), "ashfall_catalog_loader_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_scratch);
            _messages = new List<string>();
            CatalogDiagnostics.RegisterLog(new RecordingLog(_messages));
        }

        public void Dispose()
        {
            CatalogDiagnostics.RegisterLog(null);
            if (Directory.Exists(_scratch)) Directory.Delete(_scratch, true);
        }

        private static Ashfall.Core.FileSystemIO FileIO => new Ashfall.Core.FileSystemIO();
        private static Ashfall.Core.SystemTextJsonSerializer Json => new Ashfall.Core.SystemTextJsonSerializer();

        // ── (a) malformed present file ⇒ error logged with path + shape, empty result ──

        [Fact]
        public void YearOfAshMalformedOptionalCatalog_IsObservableWithPathAndShape()
        {
            string path = Path.Combine(_scratch, YearOfAshCatalogLoader.ItemsFile);
            File.WriteAllText(path, "{\"schema_version\":1,\"items\":[}");

            var loaded = YearOfAshCatalogLoader.LoadItems(_scratch, FileIO, Json);

            Assert.Empty(loaded);
            Assert.NotEmpty(_messages);
            Assert.Contains(_messages, m =>
                m.Contains(path, StringComparison.Ordinal)
                && m.Contains("YearOfAshItemContainer", StringComparison.Ordinal));
        }

        [Fact]
        public void VerdictMalformedOptionalCatalog_IsObservableWithPathAndShape()
        {
            string path = Path.Combine(_scratch, VerdictCatalogLoader.ItemsFile);
            File.WriteAllText(path, "{\"schema_version\":1,\"items\":[}");

            var loaded = VerdictCatalogLoader.LoadItems(_scratch, FileIO, Json);

            Assert.Empty(loaded);
            Assert.NotEmpty(_messages);
            Assert.Contains(_messages, m =>
                m.Contains(path, StringComparison.Ordinal)
                && m.Contains("VerdictItemEntry list", StringComparison.Ordinal));
        }

        // ── (b) valid file ⇒ identical load result to the pre-H4 baseline ────────────

        [Fact]
        public void YearOfAshValidCatalog_LoadsExpectedEntries()
        {
            string path = Path.Combine(_scratch, YearOfAshCatalogLoader.ItemsFile);
            File.WriteAllText(path,
                "{\"schema_version\":1,\"items\":[" +
                "{\"id\":\"yoa_char\",\"name\":\"Charcoal\",\"category\":\"fuel\"," +
                "\"description\":\"burns hot\",\"tradeValue\":5.0,\"weightKg\":0.5}," +
                "{\"id\":\"yoa_can\",\"name\":\"Tin Can\",\"category\":\"scrap\"," +
                "\"description\":\"dented\",\"tradeValue\":1.0,\"weightKg\":0.2}]}");

            var loaded = YearOfAshCatalogLoader.LoadItems(_scratch, FileIO, Json);

            Assert.Equal(2, loaded.Count);
            Assert.Equal("yoa_char", loaded[0].id);
            Assert.Equal("Charcoal", loaded[0].name);
            Assert.Equal("fuel", loaded[0].category);
            Assert.Equal(5.0f, loaded[0].tradeValue, 3);
            Assert.Equal(0.5f, loaded[0].weightKg, 3);
            Assert.Equal("yoa_can", loaded[1].id);
            // A valid file must not emit any diagnostic.
            Assert.Empty(_messages);
        }

        [Fact]
        public void VerdictValidCatalog_LoadsExpectedEntries()
        {
            string path = Path.Combine(_scratch, VerdictCatalogLoader.ItemsFile);
            File.WriteAllText(path,
                "{\"schema_version\":1,\"items\":[" +
                "{\"id\":\"vd_evidence_01\",\"displayName\":\"Case File\"," +
                "\"weightKg\":0.2,\"tradeValue\":0,\"category\":\"story_item\"," +
                "\"tier\":\"minor\",\"description\":\"a folder\"}]}");

            var loaded = VerdictCatalogLoader.LoadItems(_scratch, FileIO, Json);

            Assert.Single(loaded);
            Assert.Equal("vd_evidence_01", loaded[0].id);
            Assert.Equal("Case File", loaded[0].displayName);
            Assert.Equal("story_item", loaded[0].category);
            Assert.Equal(0.2f, loaded[0].weightKg, 3);
            Assert.Empty(_messages);
        }

        // ── (c) missing optional file ⇒ no throw, empty result, no error ──────────────
        //
        // Missing optional catalog files are by design silent-empty (see the Verdict
        // loader docstring: "missing file => empty list"). A missing file is not a
        // failure, so H4 does not route it through CatalogDiagnostics; pinning the
        // pre-H4 behavior keeps a host boot with an absent optional catalog quiet.

        [Fact]
        public void YearOfAshMissingOptionalFile_ReturnsEmptyWithoutThrowingOrLogging()
        {
            var loaded = YearOfAshCatalogLoader.LoadItems(_scratch, FileIO, Json);

            Assert.Empty(loaded);
            Assert.Empty(_messages);
        }

        [Fact]
        public void VerdictMissingOptionalFile_ReturnsEmptyWithoutThrowingOrLogging()
        {
            var loaded = VerdictCatalogLoader.LoadItems(_scratch, FileIO, Json);

            Assert.Empty(loaded);
            Assert.Empty(_messages);
        }

        private sealed class RecordingLog : ILog
        {
            private readonly List<string> _messages;

            public RecordingLog(List<string> messages) => _messages = messages;

            public void Info(string message) { _messages.Add(message); }
            public void Warn(string message) { _messages.Add(message); }
            public void Error(string message) { _messages.Add(message); }
        }
    }
}
