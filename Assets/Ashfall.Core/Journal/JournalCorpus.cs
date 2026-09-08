using System;
using System.Collections.Generic;

namespace Ashfall.Core.Journal
{
    /// <summary>
    /// One source-normalized authored journal record. This is immutable catalog
    /// input, not a second persisted journal state object.
    /// </summary>
    [Serializable]
    public sealed class JournalCorpusRecord
    {
        public string Id = string.Empty;
        public string Text = string.Empty;
        public string Title = string.Empty;
        public string AuthorName = string.Empty;
        public string AuthorId = string.Empty;
        public string KnowledgeKey = string.Empty;
        public string Timestamp = string.Empty;
        public int Day;
        public float Hour = -1f;
        public string Type = string.Empty;
        public string[] Tags = Array.Empty<string>();
        public string SourcePath = string.Empty;
        public int SourceIndex = -1;
    }

    /// <summary>
    /// Validated, read-only index over the authored journal corpus.
    /// </summary>
    public sealed class JournalCorpusCatalog
    {
        private readonly List<JournalCorpusRecord> _records =
            new List<JournalCorpusRecord>();
        private readonly Dictionary<string, JournalCorpusRecord> _byId =
            new Dictionary<string, JournalCorpusRecord>(StringComparer.Ordinal);
        private readonly Dictionary<string, JournalCorpusRecord> _byKnowledgeKey =
            new Dictionary<string, JournalCorpusRecord>(StringComparer.Ordinal);

        public IReadOnlyList<JournalCorpusRecord> Records => _records;
        public int Count => _records.Count;

        public void Add(JournalCorpusRecord record)
        {
            if (record == null)
                throw new JournalCorpusFormatException("Authored journal record is null.");
            if (string.IsNullOrEmpty(record.Id))
                throw new JournalCorpusFormatException("Authored journal record has no id.");
            if (string.IsNullOrEmpty(record.KnowledgeKey))
                throw new JournalCorpusFormatException(
                    $"Authored journal record '{record.Id}' has no knowledge key.");
            if (_byId.ContainsKey(record.Id))
                throw new JournalCorpusFormatException(
                    $"Duplicate authored journal id '{record.Id}'.");
            if (_byKnowledgeKey.ContainsKey(record.KnowledgeKey))
                throw new JournalCorpusFormatException(
                    $"Duplicate authored journal knowledge key '{record.KnowledgeKey}'.");

            _records.Add(record);
            _byId.Add(record.Id, record);
            _byKnowledgeKey.Add(record.KnowledgeKey, record);
        }

        public bool TryGet(string key, out JournalCorpusRecord record)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (_byKnowledgeKey.TryGetValue(key, out record!))
                    return true;
                if (_byId.TryGetValue(key, out record!))
                    return true;
            }

            record = null!;
            return false;
        }

        public bool ContainsId(string id) =>
            !string.IsNullOrEmpty(id) && _byId.ContainsKey(id);
    }

    /// <summary>
    /// Raised when an authored journal source is present but cannot be
    /// normalized without inventing or discarding runtime meaning.
    /// </summary>
    public sealed class JournalCorpusFormatException : Exception
    {
        public JournalCorpusFormatException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Loads the two existing journal source shapes and normalizes both to
    /// <see cref="JournalCorpusRecord"/>. Missing optional files are ignored;
    /// malformed or conflicting present files fail deterministically.
    /// </summary>
    public sealed class JournalCorpusCatalogLoader
    {
        public const string Expansion05File = "journal_entries_expansion_05.json";
        public const string AmbientNarrativeFile = "narrative/journals_expansion.json";
        public const string Batch1File = "narrative/journal_entries_batch_1.json";
        public const string Batch2File = "narrative/journal_entries_batch_2.json";
        public const string Batch3File = "narrative/journal_entries_batch_3.json";

        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;

        public JournalCorpusCatalogLoader(IFileIO files, IJsonSerializer json)
        {
            _files = files ?? throw new ArgumentNullException(nameof(files));
            _json = json ?? throw new ArgumentNullException(nameof(json));
        }

        public JournalCorpusCatalog Load(string dataDirectory)
        {
            var catalog = new JournalCorpusCatalog();
            if (string.IsNullOrEmpty(dataDirectory) ||
                !_files.DirectoryExists(dataDirectory))
                return catalog;

            LoadAmbient(catalog, _files.Combine(dataDirectory, Expansion05File));
            LoadAmbient(catalog, _files.Combine(dataDirectory, "narrative", "journals_expansion.json"));
            LoadCanonical(catalog, _files.Combine(dataDirectory, "narrative", "journal_entries_batch_1.json"));
            LoadCanonical(catalog, _files.Combine(dataDirectory, "narrative", "journal_entries_batch_2.json"));
            LoadCanonical(catalog, _files.Combine(dataDirectory, "narrative", "journal_entries_batch_3.json"));
            return catalog;
        }

        private void LoadAmbient(JournalCorpusCatalog catalog, string path)
        {
            if (!_files.FileExists(path)) return;

            AmbientRoot? root = Deserialize<AmbientRoot>(path);
            if (root?.journal_entries == null)
                throw new JournalCorpusFormatException(
                    $"Authored journal source '{path}' has no journal_entries array.");

            for (int i = 0; i < root.journal_entries.Count; i++)
            {
                AmbientRow? row = root.journal_entries[i];
                if (row == null)
                    throw Invalid(path, i, "record is null");

                string id = row.id ?? string.Empty;
                string body = row.bodyText ?? string.Empty;
                RequireText(path, i, id, id, "id");
                RequireText(path, i, id, body, "bodyText");
                if (row.day <= 0)
                    throw Invalid(path, i, $"record '{id}' has invalid day {row.day}");

                catalog.Add(new JournalCorpusRecord
                {
                    Id = id,
                    Text = body,
                    Title = row.title ?? string.Empty,
                    AuthorName = row.author ?? string.Empty,
                    AuthorId = row.author ?? string.Empty,
                    KnowledgeKey = id,
                    Timestamp = JournalVoice.FormatTimestamp(row.day, -1f),
                    Day = row.day,
                    Hour = -1f,
                    Type = row.type ?? string.Empty,
                    Tags = row.tags ?? Array.Empty<string>(),
                    SourcePath = path,
                    SourceIndex = i
                });
            }
        }

        private void LoadCanonical(JournalCorpusCatalog catalog, string path)
        {
            if (!_files.FileExists(path)) return;

            CanonicalRoot? root = Deserialize<CanonicalRoot>(path);
            if (root?.entries == null)
                throw new JournalCorpusFormatException(
                    $"Authored journal source '{path}' has no entries array.");

            for (int i = 0; i < root.entries.Count; i++)
            {
                CanonicalRow? row = root.entries[i];
                if (row == null)
                    throw Invalid(path, i, "record is null");

                string id = row.id ?? string.Empty;
                string body = row.text ?? string.Empty;
                string key = row.knowledge_key ?? string.Empty;
                RequireText(path, i, id, id, "id");
                RequireText(path, i, id, body, "text");
                RequireText(path, i, id, key, "knowledge_key");
                if (row.day <= 0)
                    throw Invalid(path, i, $"record '{id}' has invalid day {row.day}");
                if (row.hour < 0f || row.hour >= 24f)
                    throw Invalid(path, i, $"record '{id}' has invalid hour {row.hour}");

                string expectedTimestamp = JournalVoice.FormatTimestamp(row.day, row.hour);
                if (!string.Equals(row.timestamp, expectedTimestamp, StringComparison.Ordinal))
                    throw Invalid(path, i,
                        $"record '{id}' timestamp '{row.timestamp}' does not match '{expectedTimestamp}'");

                catalog.Add(new JournalCorpusRecord
                {
                    Id = id,
                    Text = body,
                    Title = string.Empty,
                    AuthorName = row.author_name ?? string.Empty,
                    AuthorId = row.author_id ?? string.Empty,
                    KnowledgeKey = key,
                    Timestamp = row.timestamp ?? string.Empty,
                    Day = row.day,
                    Hour = row.hour,
                    Type = string.Empty,
                    Tags = Array.Empty<string>(),
                    SourcePath = path,
                    SourceIndex = i
                });
            }
        }

        private T? Deserialize<T>(string path) where T : class
        {
            try
            {
                return _json.Deserialize<T>(_files.ReadAllText(path));
            }
            catch (Exception ex)
            {
                throw new JournalCorpusFormatException(
                    $"Could not parse authored journal source '{path}': {ex.Message}");
            }
        }

        private static void RequireText(string path, int index, string id, string value, string field)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw Invalid(path, index, $"record '{id}' has empty {field}");
        }

        private static JournalCorpusFormatException Invalid(
            string path,
            int index,
            string message) =>
            new JournalCorpusFormatException($"{path} entry {index}: {message}");

        private sealed class AmbientRoot
        {
            public List<AmbientRow>? journal_entries;
        }

        private sealed class AmbientRow
        {
            public string? id;
            public string? title;
            public string? bodyText;
            public int day;
            public string? type;
            public string? author;
            public string[]? tags;
        }

        private sealed class CanonicalRoot
        {
            public List<CanonicalRow>? entries;
        }

        private sealed class CanonicalRow
        {
            public string? id;
            public string? text;
            public string? timestamp;
            public string? author_name;
            public string? author_id;
            public string? knowledge_key;
            public int day;
            public float hour;
        }
    }

    /// <summary>
    /// Resolves authored source attribution without guessing. Exact canonical
    /// survivor IDs use the supplied survivor author; unresolved source names
    /// remain display-only and never become survivor IDs.
    /// </summary>
    public sealed class JournalCorpusAdapter
    {
        private readonly JournalCorpusCatalog _catalog;
        private readonly Dictionary<string, ISurvivorAuthor> _authors =
            new Dictionary<string, ISurvivorAuthor>(StringComparer.Ordinal);

        public JournalCorpusAdapter(
            JournalCorpusCatalog catalog,
            IEnumerable<ISurvivorAuthor>? authors = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            if (authors == null) return;

            foreach (var author in authors)
            {
                if (author == null || string.IsNullOrEmpty(author.Id)) continue;
                _authors[author.Id] = author;
            }
        }

        public JournalCorpusCatalog Catalog => _catalog;

        public bool TryGet(string key, out JournalCorpusRecord record) =>
            _catalog.TryGet(key, out record);

        public ISurvivorAuthor ResolveAuthor(
            JournalCorpusRecord record,
            ISurvivorAuthor? fallback)
        {
            if (!string.IsNullOrEmpty(record.AuthorId) &&
                _authors.TryGetValue(record.AuthorId, out var exact))
                return exact;
            if (!string.IsNullOrEmpty(record.AuthorName) &&
                _authors.TryGetValue(record.AuthorName, out exact))
                return exact;

            if (!string.IsNullOrEmpty(record.AuthorName))
                return new CorpusDisplayAuthor(
                    record.AuthorName,
                    fallback != null ? fallback.RiskBias : RiskBiasTrait.Realist);

            return fallback ?? new CorpusDisplayAuthor(
                string.Empty,
                RiskBiasTrait.Realist);
        }

        private sealed class CorpusDisplayAuthor : ISurvivorAuthor
        {
            public CorpusDisplayAuthor(string displayName, RiskBiasTrait bias)
            {
                Id = string.Empty;
                DisplayName = displayName;
                RiskBias = bias;
            }

            public string Id { get; }
            public string DisplayName { get; }
            public RiskBiasTrait RiskBias { get; }
        }
    }
}
