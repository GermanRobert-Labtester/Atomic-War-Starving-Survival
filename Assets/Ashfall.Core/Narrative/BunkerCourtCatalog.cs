using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class BunkerCourtCaseEntry
    {
        public string case_id;
        public string docket_number;
        public string defendant_name;
        public string presiding_magistrate;
        public string charge_summary;
        public string evidence_presented;
        public string verdict_outcome;
        public string disciplinary_penalty;
        public string clerk_margin_notes;
        public string[] tags;
    }

    [Serializable]
    public sealed class BunkerCourtFile
    {
        public int schema_version;
        public string collection_id;
        public List<BunkerCourtCaseEntry> cases = new List<BunkerCourtCaseEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 24 Bunker Popular Tribunal Court Records & Decrees.
    /// </summary>
    public sealed class BunkerCourtCatalog
    {
        private readonly Dictionary<string, BunkerCourtCaseEntry> _byId =
            new Dictionary<string, BunkerCourtCaseEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<BunkerCourtCaseEntry> _allCases = new List<BunkerCourtCaseEntry>();

        public IReadOnlyList<BunkerCourtCaseEntry> AllCases => _allCases;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<BunkerCourtFile>(json);
            if (file?.cases == null) return;

            foreach (var c in file.cases)
            {
                if (c == null || string.IsNullOrEmpty(c.case_id)) continue;
                _byId[c.case_id] = c;
                _allCases.Add(c);
            }
        }

        public BunkerCourtCaseEntry GetById(string caseId)
        {
            if (string.IsNullOrEmpty(caseId)) return null;
            _byId.TryGetValue(caseId, out var entry);
            return entry;
        }

        public List<BunkerCourtCaseEntry> GetByDefendant(string nameSnippet)
        {
            var results = new List<BunkerCourtCaseEntry>();
            if (string.IsNullOrEmpty(nameSnippet)) return results;

            for (int i = 0; i < _allCases.Count; i++)
            {
                var c = _allCases[i];
                if (c.defendant_name != null &&
                    c.defendant_name.IndexOf(nameSnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(c);
                }
            }
            return results;
        }

        public List<BunkerCourtCaseEntry> GetByVerdict(string verdictSnippet)
        {
            var results = new List<BunkerCourtCaseEntry>();
            if (string.IsNullOrEmpty(verdictSnippet)) return results;

            for (int i = 0; i < _allCases.Count; i++)
            {
                var c = _allCases[i];
                if (c.verdict_outcome != null &&
                    c.verdict_outcome.IndexOf(verdictSnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(c);
                }
            }
            return results;
        }

        public List<BunkerCourtCaseEntry> GetByTag(string tag)
        {
            var results = new List<BunkerCourtCaseEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allCases.Count; i++)
            {
                var c = _allCases[i];
                if (c.tags == null) continue;
                for (int j = 0; j < c.tags.Length; j++)
                {
                    if (string.Equals(c.tags[j], tag, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(c);
                        break;
                    }
                }
            }
            return results;
        }
    }
}
