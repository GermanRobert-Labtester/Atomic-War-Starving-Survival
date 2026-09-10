using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Journal;

namespace Ashfall.Core.Narrative
{
    public enum BureaucraticDocumentTruthClass
    {
        HistoricalCanonicalRecord,
        ContemporaneousAuthoredRecord,
        TemplateCompatibleRecord,
        FlavorOnlyArtifact,
        UnsafeUnresolved
    }

    /// <summary>
    /// Immutable runtime projection of one authored bureaucratic document.
    /// The transcript is presentation content; no field is a simulation delta.
    /// </summary>
    public sealed class BureaucraticDocumentDefinition
    {
        public string DocId { get; }
        public string DocType { get; }
        public string Title { get; }
        public int PostedDay { get; }
        public string PostedBy { get; }
        public string Location { get; }
        public string Material { get; }
        public string Transcript { get; }
        public IReadOnlyList<string> Tags { get; }
        public BureaucraticDocumentTruthClass TruthClass { get; }
        public string LocationId { get; }
        public IReadOnlyList<string> ProducerIds { get; }
        public IReadOnlyList<string> RelatedDocumentIds { get; }

        public BureaucraticDocumentDefinition(
            string docId,
            string docType,
            string title,
            int postedDay,
            string postedBy,
            string location,
            string material,
            string transcript,
            IEnumerable<string> tags,
            BureaucraticDocumentTruthClass truthClass,
            string locationId,
            IEnumerable<string> producerIds,
            IEnumerable<string> relatedDocumentIds)
        {
            DocId = docId ?? string.Empty;
            DocType = docType ?? string.Empty;
            Title = title ?? string.Empty;
            PostedDay = postedDay;
            PostedBy = postedBy ?? string.Empty;
            Location = location ?? string.Empty;
            Material = material ?? string.Empty;
            Transcript = transcript ?? string.Empty;
            Tags = Copy(tags);
            TruthClass = truthClass;
            LocationId = locationId ?? string.Empty;
            ProducerIds = Copy(producerIds);
            RelatedDocumentIds = Copy(relatedDocumentIds);
        }

        public string KnowledgeKey => BureaucraticDocumentDiscoverySystem.KnowledgeKey(DocId);

        public string TruthLabel
        {
            get
            {
                switch (TruthClass)
                {
                    case BureaucraticDocumentTruthClass.HistoricalCanonicalRecord:
                        return "ARCHIVED RECORD";
                    case BureaucraticDocumentTruthClass.ContemporaneousAuthoredRecord:
                        return "AUTHORED NOTICE";
                    case BureaucraticDocumentTruthClass.TemplateCompatibleRecord:
                        return "ARCHIVED FORM · TEMPLATE-COMPATIBLE";
                    case BureaucraticDocumentTruthClass.FlavorOnlyArtifact:
                        return "ARCHIVED ARTIFACT · FLAVOR";
                    default:
                        return "WITHHELD · UNRESOLVED";
                }
            }
        }

        private static IReadOnlyList<string> Copy(IEnumerable<string> values)
        {
            var result = new List<string>();
            if (values != null)
            {
                foreach (var value in values)
                {
                    if (!string.IsNullOrEmpty(value)) result.Add(value);
                }
            }
            return Array.AsReadOnly(result.ToArray());
        }
    }

    public sealed class BureaucraticDocumentCatalog
    {
        private readonly List<BureaucraticDocumentDefinition> _documents;
        private readonly IReadOnlyList<BureaucraticDocumentDefinition> _readOnlyDocuments;
        private readonly Dictionary<string, BureaucraticDocumentDefinition> _byId;

        public BureaucraticDocumentCatalog(IEnumerable<BureaucraticDocumentDefinition> documents)
        {
            _documents = new List<BureaucraticDocumentDefinition>();
            _readOnlyDocuments = _documents.AsReadOnly();
            _byId = new Dictionary<string, BureaucraticDocumentDefinition>(StringComparer.Ordinal);
            if (documents == null) return;

            foreach (var document in documents)
            {
                if (document == null || string.IsNullOrEmpty(document.DocId)) continue;
                if (_byId.ContainsKey(document.DocId)) continue;
                _documents.Add(document);
                _byId.Add(document.DocId, document);
            }

            _documents.Sort((left, right) =>
            {
                int day = left.PostedDay.CompareTo(right.PostedDay);
                return day != 0
                    ? day
                    : StringComparer.Ordinal.Compare(left.DocId, right.DocId);
            });
        }

        public IReadOnlyList<BureaucraticDocumentDefinition> Documents => _readOnlyDocuments;
        public int Count => _documents.Count;

        public bool TryGet(string docId, out BureaucraticDocumentDefinition document)
        {
            if (!string.IsNullOrEmpty(docId) && _byId.TryGetValue(docId, out document!))
                return true;
            document = null!;
            return false;
        }
    }

    public sealed class BureaucraticDocumentCatalogLoadResult
    {
        public int SchemaVersion { get; internal set; }
        public BureaucraticDocumentCatalog Catalog { get; internal set; } =
            new BureaucraticDocumentCatalog(Array.Empty<BureaucraticDocumentDefinition>());
        public List<string> Warnings { get; } = new List<string>();
        public List<string> Errors { get; } = new List<string>();
        public bool IsSuccess => Errors.Count == 0;
    }

    /// <summary>
    /// Strict loader for the authored document file plus the separate
    /// truth/provenance/producer mapping. Missing optional content is inert;
    /// malformed content is withheld rather than partially activated.
    /// </summary>
    public sealed class BureaucraticDocumentCatalogLoader
    {
        public const string DocumentsFileName = "narrative/bureaucratic_documents_expansion.json";
        public const string RuntimeMapFileName = "narrative/bureaucratic_document_runtime_map.json";
        private const int SupportedSchemaVersion = 1;

        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;

        public BureaucraticDocumentCatalogLoader(IFileIO files, IJsonSerializer json)
        {
            _files = files ?? throw new ArgumentNullException(nameof(files));
            _json = json ?? throw new ArgumentNullException(nameof(json));
        }

        public BureaucraticDocumentCatalogLoadResult Load(string dataDirectory)
        {
            var result = new BureaucraticDocumentCatalogLoadResult();
            if (string.IsNullOrEmpty(dataDirectory) || !_files.DirectoryExists(dataDirectory))
            {
                result.Warnings.Add("bureaucratic document data directory is unavailable");
                return result;
            }

            string documentsPath = _files.Combine(dataDirectory, "narrative", "bureaucratic_documents_expansion.json");
            if (!_files.FileExists(documentsPath))
            {
                result.Warnings.Add("bureaucratic document catalog is missing");
                return result;
            }

            RawDocumentRoot? source;
            try
            {
                source = _json.Deserialize<RawDocumentRoot>(_files.ReadAllText(documentsPath));
            }
            catch (Exception ex)
            {
                result.Errors.Add($"bureaucratic document catalog parse failed: {ex.Message}");
                return result;
            }

            if (source == null)
            {
                result.Errors.Add("bureaucratic document catalog is null");
                return result;
            }

            result.SchemaVersion = source.schema_version;
            if (source.schema_version != SupportedSchemaVersion)
            {
                result.Errors.Add($"unsupported bureaucratic document schema_version {source.schema_version}");
                return result;
            }
            if (source.documents == null)
            {
                result.Errors.Add("bureaucratic document catalog has no documents array");
                return result;
            }

            string mapPath = _files.Combine(dataDirectory, "narrative", "bureaucratic_document_runtime_map.json");
            var mappings = LoadMappings(mapPath, result);
            if (result.Errors.Count > 0) return result;

            var seen = new HashSet<string>(StringComparer.Ordinal);
            var definitions = new List<BureaucraticDocumentDefinition>();
            for (int i = 0; i < source.documents.Count; i++)
            {
                var raw = source.documents[i];
                if (raw == null)
                {
                    result.Errors.Add($"documents[{i}] is null");
                    continue;
                }

                string id = raw.doc_id ?? string.Empty;
                if (string.IsNullOrWhiteSpace(id))
                {
                    result.Errors.Add($"documents[{i}] has empty doc_id");
                    continue;
                }
                if (!seen.Add(id))
                {
                    result.Errors.Add($"duplicate bureaucratic document id '{id}'");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(raw.doc_type) ||
                    string.IsNullOrWhiteSpace(raw.title) ||
                    string.IsNullOrWhiteSpace(raw.transcript) ||
                    string.IsNullOrWhiteSpace(raw.posted_by) ||
                    string.IsNullOrWhiteSpace(raw.location) ||
                    string.IsNullOrWhiteSpace(raw.material))
                {
                    result.Errors.Add($"document '{id}' is missing required display fields");
                    continue;
                }
                if (raw.posted_day <= 0)
                {
                    result.Errors.Add($"document '{id}' has invalid posted_day {raw.posted_day}");
                    continue;
                }
                if (!mappings.TryGetValue(id, out var mapping))
                {
                    result.Warnings.Add($"document '{id}' has no runtime mapping and is withheld");
                    mapping = RawMapping.Unresolved(id);
                }

                definitions.Add(new BureaucraticDocumentDefinition(
                    id,
                    raw.doc_type!,
                    raw.title!,
                    raw.posted_day,
                    raw.posted_by!,
                    raw.location!,
                    raw.material!,
                    raw.transcript!,
                    raw.tags ?? Array.Empty<string>(),
                    ParseTruthClass(mapping.truth_class, id, result),
                    mapping.location_id ?? string.Empty,
                    mapping.producer_ids ?? Array.Empty<string>(),
                    mapping.related_doc_ids ?? Array.Empty<string>()));
            }

            foreach (var mapping in mappings)
            {
                if (!seen.Contains(mapping.Key))
                    result.Errors.Add($"runtime mapping references unknown document '{mapping.Key}'");
            }

            if (result.Errors.Count > 0)
            {
                result.Catalog = new BureaucraticDocumentCatalog(Array.Empty<BureaucraticDocumentDefinition>());
                return result;
            }

            var ids = new HashSet<string>(seen, StringComparer.Ordinal);
            for (int i = definitions.Count - 1; i >= 0; i--)
            {
                var doc = definitions[i];
                var related = new List<string>();
                foreach (var relatedId in doc.RelatedDocumentIds)
                {
                    if (ids.Contains(relatedId)) related.Add(relatedId);
                    else result.Warnings.Add($"document '{doc.DocId}' related target '{relatedId}' is unresolved");
                }

                if (related.Count == doc.RelatedDocumentIds.Count) continue;
                definitions[i] = new BureaucraticDocumentDefinition(
                    doc.DocId, doc.DocType, doc.Title, doc.PostedDay, doc.PostedBy,
                    doc.Location, doc.Material, doc.Transcript, doc.Tags, doc.TruthClass,
                    doc.LocationId, doc.ProducerIds, related);
            }

            result.Catalog = new BureaucraticDocumentCatalog(definitions);
            return result;
        }

        private Dictionary<string, RawMapping> LoadMappings(
            string path,
            BureaucraticDocumentCatalogLoadResult result)
        {
            var mappings = new Dictionary<string, RawMapping>(StringComparer.Ordinal);
            if (!_files.FileExists(path))
            {
                result.Warnings.Add("bureaucratic document runtime map is missing; records are withheld");
                return mappings;
            }

            RawMapRoot? root;
            try
            {
                root = _json.Deserialize<RawMapRoot>(_files.ReadAllText(path));
            }
            catch (Exception ex)
            {
                result.Errors.Add($"bureaucratic document runtime map parse failed: {ex.Message}");
                return mappings;
            }

            if (root == null || root.schema_version != SupportedSchemaVersion || root.documents == null)
            {
                result.Errors.Add("bureaucratic document runtime map has invalid schema or documents array");
                return mappings;
            }

            foreach (var mapping in root.documents)
            {
                if (mapping == null || string.IsNullOrWhiteSpace(mapping.doc_id))
                {
                    result.Errors.Add("runtime map contains an empty doc_id");
                    continue;
                }
                if (!mappings.TryAdd(mapping.doc_id!, mapping))
                    result.Errors.Add($"duplicate bureaucratic runtime mapping '{mapping.doc_id}'");
            }
            return mappings;
        }

        private static BureaucraticDocumentTruthClass ParseTruthClass(
            string? value,
            string id,
            BureaucraticDocumentCatalogLoadResult result)
        {
            switch (value)
            {
                case "historical_canonical_record": return BureaucraticDocumentTruthClass.HistoricalCanonicalRecord;
                case "contemporaneous_authored_record": return BureaucraticDocumentTruthClass.ContemporaneousAuthoredRecord;
                case "template_compatible_record": return BureaucraticDocumentTruthClass.TemplateCompatibleRecord;
                case "flavor_only_artifact": return BureaucraticDocumentTruthClass.FlavorOnlyArtifact;
                case "unsafe_unresolved": return BureaucraticDocumentTruthClass.UnsafeUnresolved;
                default:
                    result.Warnings.Add($"document '{id}' has unknown truth class and is withheld");
                    return BureaucraticDocumentTruthClass.UnsafeUnresolved;
            }
        }

        private sealed class RawDocumentRoot
        {
            public int schema_version;
            public List<RawDocument?>? documents;
        }

        private sealed class RawDocument
        {
            public string? doc_id;
            public string? doc_type;
            public string? title;
            public int posted_day;
            public string? posted_by;
            public string? location;
            public string? material;
            public string? transcript;
            public string[]? tags;
        }

        private sealed class RawMapRoot
        {
            public int schema_version;
            public List<RawMapping?>? documents;
        }

        private sealed class RawMapping
        {
            public string? doc_id;
            public string? truth_class;
            public string? location_id;
            public string[]? producer_ids;
            public string[]? related_doc_ids;

            public static RawMapping Unresolved(string id) => new RawMapping
            {
                doc_id = id,
                truth_class = "unsafe_unresolved",
                producer_ids = Array.Empty<string>(),
                related_doc_ids = Array.Empty<string>()
            };
        }
    }

    public enum BureaucraticDocumentDiscoveryStatus
    {
        Discovered,
        AlreadyDiscovered,
        BlockedBeforePostedDay,
        BlockedByTruthClass,
        UnknownDocument,
        WrongProducer,
        InvalidJournal
    }

    public sealed class BureaucraticDocumentDiscoveryResult
    {
        public string DocId { get; }
        public string ProducerId { get; }
        public BureaucraticDocumentDiscoveryStatus Status { get; }

        public BureaucraticDocumentDiscoveryResult(
            string docId,
            string producerId,
            BureaucraticDocumentDiscoveryStatus status)
        {
            DocId = docId ?? string.Empty;
            ProducerId = producerId ?? string.Empty;
            Status = status;
        }

        public bool Changed => Status == BureaucraticDocumentDiscoveryStatus.Discovered;
    }

    /// <summary>
    /// Bounded producer coordinator. Discovery state is owned by JournalSystem's
    /// existing knowledge ledger; this class owns only the explicit producer map
    /// and never mutates simulation systems.
    /// </summary>
    public sealed class BureaucraticDocumentDiscoverySystem
    {
        public const string KnowledgePrefix = "bureaucratic_document_";

        private readonly BureaucraticDocumentCatalog _catalog;

        public BureaucraticDocumentDiscoverySystem(BureaucraticDocumentCatalog catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        }

        public BureaucraticDocumentCatalog Catalog => _catalog;

        public static string KnowledgeKey(string docId) => KnowledgePrefix + (docId ?? string.Empty);

        public bool IsDiscovered(JournalSystem journal, string docId)
        {
            return journal != null &&
                _catalog.TryGet(docId, out var document) &&
                journal.Knowledge.Has(document.KnowledgeKey);
        }

        public BureaucraticDocumentDiscoveryResult Discover(
            string docId,
            string producerId,
            int currentDay,
            JournalSystem journal)
        {
            if (journal == null)
                return new BureaucraticDocumentDiscoveryResult(docId, producerId, BureaucraticDocumentDiscoveryStatus.InvalidJournal);
            if (!_catalog.TryGet(docId, out var document))
                return new BureaucraticDocumentDiscoveryResult(docId, producerId, BureaucraticDocumentDiscoveryStatus.UnknownDocument);
            if (!HasProducer(document, producerId))
                return new BureaucraticDocumentDiscoveryResult(docId, producerId, BureaucraticDocumentDiscoveryStatus.WrongProducer);
            if (currentDay < document.PostedDay)
                return new BureaucraticDocumentDiscoveryResult(docId, producerId, BureaucraticDocumentDiscoveryStatus.BlockedBeforePostedDay);
            if (document.TruthClass == BureaucraticDocumentTruthClass.UnsafeUnresolved)
                return new BureaucraticDocumentDiscoveryResult(docId, producerId, BureaucraticDocumentDiscoveryStatus.BlockedByTruthClass);
            bool changed = journal.UnlockBureaucraticDocument(document.DocId);
            return new BureaucraticDocumentDiscoveryResult(
                document.DocId,
                producerId,
                changed
                    ? BureaucraticDocumentDiscoveryStatus.Discovered
                    : BureaucraticDocumentDiscoveryStatus.AlreadyDiscovered);
        }

        public IReadOnlyList<BureaucraticDocumentDiscoveryResult> DiscoverByProducer(
            string producerId,
            int currentDay,
            JournalSystem journal)
        {
            var results = new List<BureaucraticDocumentDiscoveryResult>();
            if (string.IsNullOrEmpty(producerId)) return results;

            foreach (var document in _catalog.Documents)
            {
                if (!HasProducer(document, producerId)) continue;
                if (currentDay < document.PostedDay) continue;
                results.Add(Discover(document.DocId, producerId, currentDay, journal));
            }
            return results;
        }

        private static bool HasProducer(BureaucraticDocumentDefinition document, string producerId)
        {
            if (document == null || string.IsNullOrEmpty(producerId)) return false;
            for (int i = 0; i < document.ProducerIds.Count; i++)
            {
                if (string.Equals(document.ProducerIds[i], producerId, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }
    }
}
