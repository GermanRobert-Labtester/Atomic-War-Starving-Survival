using System;
using System.Collections.Generic;
using Ashfall.Core.IO;
#pragma warning disable CS8618

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
    /// Engine-agnostic loader and query interface for Bunker Corridor Slate Graffiti,
    /// Wall Text, and Ambient Field Markings (Plans 17, 29, 145).
    /// Hardened for idempotent loading, multi-source ingestion, and location projection.
    /// </summary>
    public sealed class BunkerGraffitiCatalog
    {
        private readonly Dictionary<string, BunkerGraffitiEntry> _byId =
            new Dictionary<string, BunkerGraffitiEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<BunkerGraffitiEntry> _allPostings = new List<BunkerGraffitiEntry>();

        public IReadOnlyList<BunkerGraffitiEntry> AllPostings => _allPostings;
        public int Count => _allPostings.Count;

        /// <summary>
        /// Clears all loaded postings and resets internal lookup indices.
        /// </summary>
        public void Clear()
        {
            _byId.Clear();
            _allPostings.Clear();
        }

        /// <summary>
        /// Loads postings from raw JSON. Idempotent: entries with an already-registered
        /// posting_id (case-insensitive) are ignored to prevent duplication on repeated load.
        /// Postings with null/empty posting_id, empty content, or negative recorded_day are rejected.
        /// Postings are deterministically sorted by recorded_day ascending, then posting_id.
        /// </summary>
        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<BunkerGraffitiFile>(json);
            if (file?.postings == null) return;

            bool addedAny = false;
            foreach (var p in file.postings)
            {
                if (p == null) continue;
                if (string.IsNullOrWhiteSpace(p.posting_id)) continue;
                if (string.IsNullOrWhiteSpace(p.content)) continue;
                if (p.recorded_day < 0) continue;

                // Idempotent duplicate check (case-insensitive)
                if (_byId.ContainsKey(p.posting_id)) continue;

                _byId[p.posting_id] = p;
                _allPostings.Add(p);
                addedAny = true;
            }

            if (addedAny)
            {
                SortPostings();
            }
        }

        private void SortPostings()
        {
            _allPostings.Sort((a, b) =>
            {
                int dayComp = a.recorded_day.CompareTo(b.recorded_day);
                if (dayComp != 0) return dayComp;
                return string.Compare(a.posting_id, b.posting_id, StringComparison.Ordinal);
            });
        }

        /// <summary>
        /// Canonical directory loader that ingests both base canonical postings
        /// (narrative/bunker_graffiti_postings.json) and expansion postings (narrative/graffiti_expansion.json).
        /// </summary>
        public static BunkerGraffitiCatalog LoadFromDirectory(string dataDir, IFileIO fileIo, IJsonSerializer serializer)
        {
            var catalog = new BunkerGraffitiCatalog();
            if (string.IsNullOrWhiteSpace(dataDir) || fileIo == null || serializer == null)
                return catalog;

            string canonicalPath = fileIo.Combine(dataDir, "narrative", "bunker_graffiti_postings.json");
            if (fileIo.FileExists(canonicalPath))
            {
                try
                {
                    catalog.Load(fileIo.ReadAllText(canonicalPath), serializer);
                }
                catch (Exception ex)
                {
                    CatalogDiagnostics.Warn(canonicalPath, "BunkerGraffitiCatalog", ex);
                }
            }

            string expansionPath = fileIo.Combine(dataDir, "narrative", "graffiti_expansion.json");
            if (fileIo.FileExists(expansionPath))
            {
                try
                {
                    catalog.Load(fileIo.ReadAllText(expansionPath), serializer);
                }
                catch (Exception ex)
                {
                    CatalogDiagnostics.Warn(expansionPath, "GraffitiExpansionFile", ex);
                }
            }

            return catalog;
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
            if (currentDay < 0) return results;

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

        /// <summary>
        /// Returns all postings that project to the specified canonical target ID
        /// (either a shelter room like "room_kitchen" or world location like "suburban_house")
        /// and have unlocked by currentDay.
        /// </summary>
        public List<BunkerGraffitiEntry> GetPostingsForTarget(string targetId, int currentDay = int.MaxValue)
        {
            var results = new List<BunkerGraffitiEntry>();
            if (string.IsNullOrEmpty(targetId) || currentDay < 0) return results;

            for (int i = 0; i < _allPostings.Count; i++)
            {
                var p = _allPostings[i];
                if (p.recorded_day <= currentDay)
                {
                    string target = BunkerGraffitiProjection.ResolveCanonicalTarget(p);
                    if (string.Equals(target, targetId, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(p);
                    }
                }
            }
            return results;
        }

        public List<BunkerGraffitiEntry> GetPostingsForRoom(string roomId, int currentDay = int.MaxValue)
            => GetPostingsForTarget(roomId, currentDay);

        public List<BunkerGraffitiEntry> GetPostingsForLocation(string locationId, int currentDay = int.MaxValue)
            => GetPostingsForTarget(locationId, currentDay);
    }
}
