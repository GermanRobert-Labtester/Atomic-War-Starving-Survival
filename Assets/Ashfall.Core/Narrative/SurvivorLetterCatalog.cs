using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class SurvivorLetterEntry
    {
        public string letter_id;
        public string pigeonhole_number;
        public string author_dweller;
        public string intended_recipient;
        public string destination_address;
        public string dispatch_attempt_date;
        public string envelope_condition;
        public string letter_text;
        public string galina_dead_letter_note;
        public string[] tags;
    }

    [Serializable]
    public sealed class SurvivorLetterFile
    {
        public int schema_version;
        public string collection_id;
        public List<SurvivorLetterEntry> letters = new List<SurvivorLetterEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 25 Unopened Letters to Lost Kin & Surface Postboxes.
    /// </summary>
    public sealed class SurvivorLetterCatalog
    {
        private readonly Dictionary<string, SurvivorLetterEntry> _byId =
            new Dictionary<string, SurvivorLetterEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<SurvivorLetterEntry> _allLetters = new List<SurvivorLetterEntry>();

        public IReadOnlyList<SurvivorLetterEntry> AllLetters => _allLetters;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<SurvivorLetterFile>(json);
            if (file?.letters == null) return;

            foreach (var l in file.letters)
            {
                if (l == null || string.IsNullOrEmpty(l.letter_id)) continue;
                _byId[l.letter_id] = l;
                _allLetters.Add(l);
            }
        }

        public SurvivorLetterEntry? GetById(string letterId)
        {
            if (string.IsNullOrEmpty(letterId)) return null;
            _byId.TryGetValue(letterId, out var entry);
            return entry;
        }

        public List<SurvivorLetterEntry> GetByAuthor(string authorSnippet)
        {
            var results = new List<SurvivorLetterEntry>();
            if (string.IsNullOrEmpty(authorSnippet)) return results;

            for (int i = 0; i < _allLetters.Count; i++)
            {
                var l = _allLetters[i];
                if (l.author_dweller != null &&
                    l.author_dweller.IndexOf(authorSnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(l);
                }
            }
            return results;
        }

        public List<SurvivorLetterEntry> GetByDestination(string destinationSnippet)
        {
            var results = new List<SurvivorLetterEntry>();
            if (string.IsNullOrEmpty(destinationSnippet)) return results;

            for (int i = 0; i < _allLetters.Count; i++)
            {
                var l = _allLetters[i];
                if (l.destination_address != null &&
                    l.destination_address.IndexOf(destinationSnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(l);
                }
            }
            return results;
        }

        public List<SurvivorLetterEntry> GetByTag(string tag)
        {
            var results = new List<SurvivorLetterEntry>();
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
    }
}
