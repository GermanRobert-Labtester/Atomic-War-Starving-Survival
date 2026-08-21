using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class WireConfessionEntry
    {
        public string confession_id;
        public string author_id;
        public string author_name;
        public int recorded_day;
        public string device_type;
        public string acoustic_environment;
        public string title;
        public string transcript;
        public string[] tags;
    }

    [Serializable]
    public sealed class WireConfessionBatchFile
    {
        public int schema_version;
        public string archive_id;
        public List<WireConfessionEntry> confessions = new List<WireConfessionEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query service for The Black Wire Archive (30 intimate confessions).
    /// </summary>
    public sealed class WireConfessionCatalog
    {
        private readonly Dictionary<string, WireConfessionEntry> _byConfessionId =
            new Dictionary<string, WireConfessionEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, List<WireConfessionEntry>> _byAuthorId =
            new Dictionary<string, List<WireConfessionEntry>>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyCollection<WireConfessionEntry> AllConfessions => _byConfessionId.Values;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var batch = serializer.Deserialize<WireConfessionBatchFile>(json);
            if (batch?.confessions == null) return;

            foreach (var conf in batch.confessions)
            {
                if (conf == null || string.IsNullOrEmpty(conf.confession_id)) continue;
                _byConfessionId[conf.confession_id] = conf;

                if (!string.IsNullOrEmpty(conf.author_id))
                {
                    if (!_byAuthorId.TryGetValue(conf.author_id, out var list))
                    {
                        list = new List<WireConfessionEntry>();
                        _byAuthorId[conf.author_id] = list;
                    }
                    list.Add(conf);
                }
            }
        }

        public WireConfessionEntry? GetById(string confessionId)
        {
            if (string.IsNullOrEmpty(confessionId)) return null;
            _byConfessionId.TryGetValue(confessionId, out var entry);
            return entry;
        }

        public IReadOnlyList<WireConfessionEntry> GetByAuthor(string authorId)
        {
            if (string.IsNullOrEmpty(authorId)) return Array.Empty<WireConfessionEntry>();
            if (_byAuthorId.TryGetValue(authorId, out var list)) return list;
            return Array.Empty<WireConfessionEntry>();
        }

        public bool Contains(string confessionId)
        {
            if (string.IsNullOrEmpty(confessionId)) return false;
            return _byConfessionId.ContainsKey(confessionId);
        }
    }
}
