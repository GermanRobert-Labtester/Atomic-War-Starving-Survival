using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class BureaucraticDocumentCatalogTests
    {
        private static string FindDataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string probe = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(probe)) return probe;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate Assets/StreamingAssets/Data from " + AppContext.BaseDirectory);
        }

        private static BureaucraticDocumentCatalogLoadResult LoadActual()
        {
            return new BureaucraticDocumentCatalogLoader(
                new FileSystemIO(),
                new SystemTextJsonSerializer()).Load(FindDataDir());
        }

        [Fact]
        public void ActualCatalog_LoadsCompleteInventoryWithExplicitSafeMappings()
        {
            var result = LoadActual();

            Assert.True(result.IsSuccess, string.Join(" | ", result.Errors));
            Assert.Equal(1, result.SchemaVersion);
            Assert.Equal(27, result.Catalog.Count);
            Assert.Equal(14, result.Catalog.Documents.Select(d => d.DocType).Distinct(StringComparer.Ordinal).Count());
            Assert.DoesNotContain(result.Catalog.Documents, d => d.TruthClass == BureaucraticDocumentTruthClass.UnsafeUnresolved);
            Assert.All(result.Catalog.Documents, document =>
            {
                Assert.False(string.IsNullOrWhiteSpace(document.DocId));
                Assert.False(string.IsNullOrWhiteSpace(document.Title));
                Assert.False(string.IsNullOrWhiteSpace(document.Transcript));
                Assert.True(document.PostedDay > 0);
                Assert.NotEmpty(document.ProducerIds);
                Assert.Empty(document.LocationId);
                Assert.All(document.RelatedDocumentIds, relatedId => Assert.True(result.Catalog.TryGet(relatedId, out _)));
            });

            var ordered = result.Catalog.Documents
                .OrderBy(d => d.PostedDay)
                .ThenBy(d => d.DocId, StringComparer.Ordinal)
                .Select(d => d.DocId)
                .ToArray();
            Assert.Equal(ordered, result.Catalog.Documents.Select(d => d.DocId).ToArray());
        }

        [Fact]
        public void Discovery_IsExplicitDayGatedAndIdempotent()
        {
            var catalog = LoadActual().Catalog;
            var discovery = new BureaucraticDocumentDiscoverySystem(catalog);
            var journal = new JournalSystem();

            var dayOne = discovery.DiscoverByProducer("gate_house_notice_board", 1, journal);
            Assert.Single(dayOne);
            Assert.Equal("bunker_doc_transfer_bram_02", dayOne[0].DocId);
            Assert.Equal(BureaucraticDocumentDiscoveryStatus.Discovered, dayOne[0].Status);

            var dayFour = discovery.DiscoverByProducer("gate_house_notice_board", 4, journal);
            Assert.Equal(2, dayFour.Count);
            Assert.Contains(dayFour, r => r.DocId == "bunker_doc_transfer_bram_02" && !r.Changed);
            Assert.Contains(dayFour, r => r.DocId == "bunker_doc_evacuation_dam" && r.Changed);

            var repeated = discovery.Discover(
                "bunker_doc_evacuation_dam",
                "gate_house_notice_board",
                4,
                journal);
            Assert.Equal(BureaucraticDocumentDiscoveryStatus.AlreadyDiscovered, repeated.Status);
            Assert.Equal(2, journal.CodexUnlockCount);
        }

        [Fact]
        public void SaveRestore_PreservesStableDocumentIdentityWithoutReplay()
        {
            var catalog = LoadActual().Catalog;
            var discovery = new BureaucraticDocumentDiscoverySystem(catalog);
            var source = new JournalSystem();
            Assert.Equal(
                BureaucraticDocumentDiscoveryStatus.Discovered,
                discovery.Discover("bunker_doc_transfer_bram_02", "gate_house_notice_board", 1, source).Status);

            var save = source.CaptureState();
            var restored = new JournalSystem();
            int restoredUnlocks = 0;
            restored.OnCodexUnlocked += _ => restoredUnlocks++;
            restored.RestoreState(save);

            Assert.True(restored.IsBureaucraticDocumentDiscovered("bunker_doc_transfer_bram_02"));
            Assert.Equal(1, restored.CodexUnlockCount);
            Assert.Equal(0, restoredUnlocks);
            Assert.Equal(
                BureaucraticDocumentDiscoveryStatus.AlreadyDiscovered,
                discovery.Discover("bunker_doc_transfer_bram_02", "gate_house_notice_board", 1, restored).Status);
            Assert.Equal(1, restored.CodexUnlockCount);
        }

        [Fact]
        public void MissingMapping_IsWithheldAndCannotBecomeAProducer()
        {
            var files = new MemoryFileIO();
            files.Set("narrative/bureaucratic_documents_expansion.json", DocumentJson("doc_missing_map"));
            files.Set("narrative/bureaucratic_document_runtime_map.json", "{\"schema_version\":1,\"documents\":[]}");

            var result = new BureaucraticDocumentCatalogLoader(files, new SystemTextJsonSerializer()).Load("data");
            Assert.True(result.IsSuccess);
            Assert.Single(result.Catalog.Documents);
            var document = result.Catalog.Documents[0];
            Assert.Equal(BureaucraticDocumentTruthClass.UnsafeUnresolved, document.TruthClass);
            Assert.Empty(document.ProducerIds);
            Assert.Contains(result.Warnings, warning => warning.Contains("no runtime mapping", StringComparison.Ordinal));

            var attempt = new BureaucraticDocumentDiscoverySystem(result.Catalog)
                .Discover(document.DocId, "archive_desk", 10, new JournalSystem());
            Assert.Equal(BureaucraticDocumentDiscoveryStatus.WrongProducer, attempt.Status);
        }

        [Fact]
        public void DuplicateIdsAndMalformedFields_FailClosed()
        {
            var files = new MemoryFileIO();
            files.Set(
                "narrative/bureaucratic_documents_expansion.json",
                "{\"schema_version\":1,\"documents\":[" + DocumentJsonObject("duplicate") + "," + DocumentJsonObject("duplicate") + "," +
                "{\"doc_id\":\"bad_day\",\"doc_type\":\"notice\",\"title\":\"Bad\",\"posted_day\":0,\"posted_by\":\"Clerk\",\"location\":\"Desk\",\"material\":\"Paper\",\"transcript\":\"Text\"}," +
                "{\"doc_id\":\"missing_title\",\"doc_type\":\"notice\",\"title\":\"\",\"posted_day\":1,\"posted_by\":\"Clerk\",\"location\":\"Desk\",\"material\":\"Paper\",\"transcript\":\"Text\"}]}" );
            files.Set(
                "narrative/bureaucratic_document_runtime_map.json",
                "{\"schema_version\":1,\"documents\":[" + MappingJsonObject("duplicate") + "," + MappingJsonObject("duplicate") + "]}");

            var result = new BureaucraticDocumentCatalogLoader(files, new SystemTextJsonSerializer()).Load("data");
            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, error => error.Contains("duplicate bureaucratic runtime mapping", StringComparison.Ordinal));
            Assert.Empty(result.Catalog.Documents);
        }

        [Fact]
        public void UnknownRelatedTarget_IsWarningOnlyAndDoesNotBreakReader()
        {
            var files = new MemoryFileIO();
            files.Set("narrative/bureaucratic_documents_expansion.json", DocumentJson("doc_with_edge"));
            files.Set(
                "narrative/bureaucratic_document_runtime_map.json",
                "{\"schema_version\":1,\"documents\":[{\"doc_id\":\"doc_with_edge\",\"truth_class\":\"historical_canonical_record\",\"producer_ids\":[\"archive_desk\"],\"related_doc_ids\":[\"missing_target\"]}]}");

            var result = new BureaucraticDocumentCatalogLoader(files, new SystemTextJsonSerializer()).Load("data");
            Assert.True(result.IsSuccess);
            Assert.Single(result.Catalog.Documents);
            Assert.Empty(result.Catalog.Documents[0].RelatedDocumentIds);
            Assert.Contains(result.Warnings, warning => warning.Contains("missing_target", StringComparison.Ordinal));
        }

        [Fact]
        public void UnknownDocumentAndProducer_FailClosedWithoutJournalMutation()
        {
            var discovery = new BureaucraticDocumentDiscoverySystem(LoadActual().Catalog);
            var journal = new JournalSystem();

            Assert.Equal(
                BureaucraticDocumentDiscoveryStatus.UnknownDocument,
                discovery.Discover("does_not_exist", "archive_desk", 99, journal).Status);
            Assert.Equal(
                BureaucraticDocumentDiscoveryStatus.WrongProducer,
                discovery.Discover("bunker_doc_req_fuel_03", "unknown_source", 99, journal).Status);
            Assert.Equal(0, journal.Knowledge.Count);
            Assert.Equal(0, journal.CodexUnlockCount);
        }

        private static string DocumentJson(string id)
        {
            return "{\"schema_version\":1,\"documents\":[" + DocumentJsonObject(id) + "]}";
        }

        private static string DocumentJsonObject(string id)
        {
            return "{\"doc_id\":\"" + id + "\",\"doc_type\":\"notice\",\"title\":\"A record\",\"posted_day\":1,\"posted_by\":\"Clerk\",\"location\":\"Desk\",\"material\":\"Paper\",\"transcript\":\"A fixed authored transcript.\",\"tags\":[\"notice\"]}";
        }

        private static string MappingJsonObject(string id)
        {
            return "{\"doc_id\":\"" + id + "\",\"truth_class\":\"historical_canonical_record\",\"producer_ids\":[\"archive_desk\"],\"related_doc_ids\":[]}";
        }

        private sealed class MemoryFileIO : IFileIO
        {
            private readonly Dictionary<string, string> _files = new Dictionary<string, string>(StringComparer.Ordinal);

            public void Set(string relativePath, string contents) => _files[Path.Combine("data", relativePath.Replace('/', Path.DirectorySeparatorChar))] = contents;
            public string Combine(params string[] parts) => Path.Combine(parts);
            public bool DirectoryExists(string path) => path == "data" || path.StartsWith("data" + Path.DirectorySeparatorChar, StringComparison.Ordinal);
            public bool FileExists(string path) => _files.ContainsKey(path);
            public string ReadAllText(string path) => _files[path];
            public void WriteAllText(string path, string contents) => _files[path] = contents;
        }
    }
}
