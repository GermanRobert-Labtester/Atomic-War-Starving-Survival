using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class BunkerGraffitiEntry
    {
        public string posting_id;
        public int recorded_day;
        public string location;
        public string medium;
        public string author_signature;
        public string category;
        public string content;
        public string morale_effect;
        public string[] tags;
    }

    [Serializable]
    public sealed class BunkerGraffitiFile
    {
        public int schema_version;
        public string collection_id;
        public List<BunkerGraffitiEntry> postings = new List<BunkerGraffitiEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 36 Bunker Corridor Slate Graffiti & Daily Gossip Postings.
    /// </summary>
    public sealed class BunkerGraffitiCatalog
    {
        private readonly Dictionary<string, BunkerGraffitiEntry> _byId =
            new Dictionary<string, BunkerGraffitiEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<BunkerGraffitiEntry> _allPostings = new List<BunkerGraffitiEntry>();

        public IReadOnlyList<BunkerGraffitiEntry> AllPostings => _allPostings;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<BunkerGraffitiFile>(json);
            if (file?.postings == null) return;

            foreach (var p in file.postings)
            {
                if (p == null || string.IsNullOrEmpty(p.posting_id)) continue;
                _byId[p.posting_id] = p;
                _allPostings.Add(p);
            }
        }

        public BunkerGraffitiEntry? GetById(string postingId)
        {
            if (string.IsNullOrEmpty(postingId)) return null;
            _byId.TryGetValue(postingId, out var entry);
            return entry;
        }

        public List<BunkerGraffitiEntry> GetUnlockedByDay(int currentDay)
        {
            var results = new List<BunkerGraffitiEntry>();
            for (int i = 0; i < _allPostings.Count; i++)
            {
                var p = _allPostings[i];
                if (p.recorded_day <= currentDay)
                {
                    results.Add(p);
                }
            }
            return results;
        }

        public List<BunkerGraffitiEntry> GetByCategory(string categorySnippet)
        {
            var results = new List<BunkerGraffitiEntry>();
            if (string.IsNullOrEmpty(categorySnippet)) return results;

            for (int i = 0; i < _allPostings.Count; i++)
            {
                var p = _allPostings[i];
                if (p.category != null && p.category.IndexOf(categorySnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(p);
                }
            }
            return results;
        }
    }
}
