using System;
using System.Collections.Generic;
using Ashfall.Core.IO;
#pragma warning disable CS8618

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class PersonalLetterEntry
    {
        public string letter_id;
        public int day;
        public string sender;
        public string recipient;
        public string location;
        public string letter_type;
        public string tone;
        public string[] key_themes;
        public string content;
        public string[] cross_refs;
        public string[] tags;
    }

    [Serializable]
    public sealed class PersonalLetterFile
    {
        public int schema_version;
        public string collection_id;
        public string description;
        public List<PersonalLetterEntry> letters = new List<PersonalLetterEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for Personal Letters & Unsent Correspondence
    /// (narrative/letters_expansion.json and narrative/unsent_letters_batch_2.json).
    /// </summary>
    public sealed class PersonalLetterCatalog
    {
        private readonly Dictionary<string, PersonalLetterEntry> _byId =
            new Dictionary<string, PersonalLetterEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<PersonalLetterEntry> _allLetters = new List<PersonalLetterEntry>();

        public IReadOnlyList<PersonalLetterEntry> AllLetters => _allLetters;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<PersonalLetterFile>(json);
            if (file?.letters == null) return;

            foreach (var l in file.letters)
            {
                if (l == null || string.IsNullOrEmpty(l.letter_id)) continue;
                if (_byId.ContainsKey(l.letter_id)) continue; // Idempotent: prevent duplicate additions

                _byId[l.letter_id] = l;
                _allLetters.Add(l);
            }
        }

        public void Clear()
        {
            _byId.Clear();
            _allLetters.Clear();
        }

        /// <summary>
        /// Canonical directory loader that ingests narrative/letters_expansion.json and optional batches.
        /// </summary>
        public static PersonalLetterCatalog LoadFromDirectory(string dataDir, IFileIO fileIo, IJsonSerializer serializer)
        {
            var catalog = new PersonalLetterCatalog();
            if (string.IsNullOrWhiteSpace(dataDir) || fileIo == null || serializer == null)
                return catalog;

            string canonicalPath = fileIo.Combine(dataDir, "narrative", "letters_expansion.json");
            if (fileIo.FileExists(canonicalPath))
            {
                try
                {
                    catalog.Load(fileIo.ReadAllText(canonicalPath), serializer);
                }
                catch (Exception ex)
                {
                    CatalogDiagnostics.Warn(canonicalPath, "PersonalLetterCatalog", ex);
                }
            }

            string batch2Path = fileIo.Combine(dataDir, "narrative", "unsent_letters_batch_2.json");
            if (fileIo.FileExists(batch2Path))
            {
                try
                {
                    catalog.Load(fileIo.ReadAllText(batch2Path), serializer);
                }
                catch (Exception ex)
                {
                    CatalogDiagnostics.Warn(batch2Path, "PersonalLetterCatalog", ex);
                }
            }

            return catalog;
        }

        public PersonalLetterEntry? GetById(string letterId)
        {
            if (string.IsNullOrEmpty(letterId)) return null;
            _byId.TryGetValue(letterId, out var entry);
            return entry;
        }

        public List<PersonalLetterEntry> GetByType(string letterType)
        {
            var results = new List<PersonalLetterEntry>();
            if (string.IsNullOrEmpty(letterType)) return results;

            for (int i = 0; i < _allLetters.Count; i++)
            {
                var l = _allLetters[i];
                if (string.Equals(l.letter_type, letterType, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(l);
                }
            }
            return results;
        }

        public List<PersonalLetterEntry> GetBySender(string senderSnippet)
        {
            var results = new List<PersonalLetterEntry>();
            if (string.IsNullOrEmpty(senderSnippet)) return results;

            for (int i = 0; i < _allLetters.Count; i++)
            {
                var l = _allLetters[i];
                if (l.sender != null && l.sender.IndexOf(senderSnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(l);
                }
            }
            return results;
        }

        public List<PersonalLetterEntry> GetByRecipient(string recipientSnippet)
        {
            var results = new List<PersonalLetterEntry>();
            if (string.IsNullOrEmpty(recipientSnippet)) return results;

            for (int i = 0; i < _allLetters.Count; i++)
            {
                var l = _allLetters[i];
                if (l.recipient != null && l.recipient.IndexOf(recipientSnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(l);
                }
            }
            return results;
        }

        public List<PersonalLetterEntry> GetByTag(string tag)
        {
            var results = new List<PersonalLetterEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allLetters.Count; i++)
            {
                var l = _allLetters[i];
                if (l.tags == null) continue;
                for (int j = 0; j < l.tags.Length; j++)
                {
                    if (string.Equals(l.tags[j], tag, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(l);
                        break;
                    }
                }
            }
            return results;
        }

        public List<PersonalLetterEntry> GetByTheme(string theme)
        {
            var results = new List<PersonalLetterEntry>();
            if (string.IsNullOrEmpty(theme)) return results;

            for (int i = 0; i < _allLetters.Count; i++)
            {
                var l = _allLetters[i];
                if (l.key_themes == null) continue;
                for (int j = 0; j < l.key_themes.Length; j++)
                {
                    if (string.Equals(l.key_themes[j], theme, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(l);
                        break;
                    }
                }
            }
            return results;
        }

        public List<PersonalLetterEntry> GetBySearch(string query)
        {
            var results = new List<PersonalLetterEntry>();
            if (string.IsNullOrWhiteSpace(query)) return results;

            for (int i = 0; i < _allLetters.Count; i++)
            {
                var l = _allLetters[i];
                if (MatchesSearch(l, query))
                {
                    results.Add(l);
                }
            }
            return results;
        }

        public List<PersonalLetterEntry> GetSortedByDay(bool ascending = true)
        {
            var copy = new List<PersonalLetterEntry>(_allLetters);
            copy.Sort((a, b) =>
            {
                int cmp = ascending ? a.day.CompareTo(b.day) : b.day.CompareTo(a.day);
                if (cmp != 0) return cmp;
                return string.Compare(a.letter_id, b.letter_id, StringComparison.Ordinal);
            });
            return copy;
        }

        private static bool MatchesSearch(PersonalLetterEntry l, string query)
        {
            if (ContainsIgnoreCase(l.letter_id, query)) return true;
            if (ContainsIgnoreCase(l.sender, query)) return true;
            if (ContainsIgnoreCase(l.recipient, query)) return true;
            if (ContainsIgnoreCase(l.location, query)) return true;
            if (ContainsIgnoreCase(l.tone, query)) return true;
            if (ContainsIgnoreCase(l.content, query)) return true;

            if (l.tags != null)
            {
                for (int i = 0; i < l.tags.Length; i++)
                {
                    if (ContainsIgnoreCase(l.tags[i], query)) return true;
                }
            }

            if (l.key_themes != null)
            {
                for (int i = 0; i < l.key_themes.Length; i++)
                {
                    if (ContainsIgnoreCase(l.key_themes[i], query)) return true;
                }
            }

            return false;
        }

        private static bool ContainsIgnoreCase(string? source, string query)
        {
            if (string.IsNullOrEmpty(source)) return false;
            return source.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
