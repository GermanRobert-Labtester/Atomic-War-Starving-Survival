using System;
using System.Collections.Generic;
using Ashfall.Core.IO;
#pragma warning disable CS8618

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

        /// <summary>
        /// Extracts the chronological day from the docket number format (e.g. TRIB-084-MOONSHINE -> 84).
        /// Returns 0 if unparseable.
        /// </summary>
        public int GetDocketDay()
        {
            if (string.IsNullOrEmpty(docket_number)) return 0;
            var parts = docket_number.Split('-');
            if (parts.Length >= 2 && int.TryParse(parts[1], out int day))
                return day;
            return 0;
        }
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
    /// Fully idempotent, deterministically ordered, with read-only query semantics.
    /// </summary>
    public sealed class BunkerCourtCatalog
    {
        private readonly Dictionary<string, BunkerCourtCaseEntry> _byId =
            new Dictionary<string, BunkerCourtCaseEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, BunkerCourtCaseEntry> _byDocket =
            new Dictionary<string, BunkerCourtCaseEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<BunkerCourtCaseEntry> _allCases = new List<BunkerCourtCaseEntry>();

        public IReadOnlyList<BunkerCourtCaseEntry> AllCases => _allCases;
        public int Count => _allCases.Count;

        /// <summary>
        /// Clears all loaded cases and resets internal lookup dictionaries.
        /// </summary>
        public void Clear()
        {
            _byId.Clear();
            _byDocket.Clear();
            _allCases.Clear();
        }

        /// <summary>
        /// Loads tribunal cases from raw JSON. Idempotent: entries with an already-registered
        /// case_id (case-insensitive) are ignored to prevent duplication on repeated load.
        /// Cases with null/empty case_id or docket_number are safely rejected.
        /// Entries are deterministically sorted by historical day parsed from docket, then case_id.
        /// </summary>
        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<BunkerCourtFile>(json);
            if (file?.cases == null) return;

            bool addedAny = false;
            foreach (var c in file.cases)
            {
                if (c == null) continue;
                if (string.IsNullOrWhiteSpace(c.case_id)) continue;
                if (string.IsNullOrWhiteSpace(c.docket_number)) continue;

                // Idempotent duplicate check (case-insensitive)
                if (_byId.ContainsKey(c.case_id)) continue;

                _byId[c.case_id] = c;
                _byDocket[c.docket_number] = c;
                _allCases.Add(c);
                addedAny = true;
            }

            if (addedAny)
            {
                SortCases();
            }
        }

        private void SortCases()
        {
            _allCases.Sort((a, b) =>
            {
                int dayComp = a.GetDocketDay().CompareTo(b.GetDocketDay());
                if (dayComp != 0) return dayComp;
                return string.Compare(a.case_id, b.case_id, StringComparison.Ordinal);
            });
        }

        /// <summary>
        /// Canonical directory loader that ingests narrative/bunker_court_verdicts_codex.json.
        /// </summary>
        public static BunkerCourtCatalog LoadFromDirectory(string dataDir, IFileIO fileIo, IJsonSerializer serializer)
        {
            var catalog = new BunkerCourtCatalog();
            if (string.IsNullOrWhiteSpace(dataDir) || fileIo == null || serializer == null)
                return catalog;

            string canonicalPath = fileIo.Combine(dataDir, "narrative", "bunker_court_verdicts_codex.json");
            if (fileIo.FileExists(canonicalPath))
            {
                try
                {
                    catalog.Load(fileIo.ReadAllText(canonicalPath), serializer);
                }
                catch (Exception ex)
                {
                    CatalogDiagnostics.Warn(canonicalPath, "BunkerCourtCatalog", ex);
                }
            }

            return catalog;
        }

        public BunkerCourtCaseEntry? GetById(string caseId)
        {
            if (string.IsNullOrEmpty(caseId)) return null;
            _byId.TryGetValue(caseId, out var entry);
            return entry;
        }

        public BunkerCourtCaseEntry? GetByDocket(string docketNumber)
        {
            if (string.IsNullOrEmpty(docketNumber)) return null;
            _byDocket.TryGetValue(docketNumber, out var entry);
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

        public List<BunkerCourtCaseEntry> GetByMagistrate(string magistrateSnippet)
        {
            var results = new List<BunkerCourtCaseEntry>();
            if (string.IsNullOrEmpty(magistrateSnippet)) return results;

            for (int i = 0; i < _allCases.Count; i++)
            {
                var c = _allCases[i];
                if (c.presiding_magistrate != null &&
                    c.presiding_magistrate.IndexOf(magistrateSnippet, StringComparison.OrdinalIgnoreCase) >= 0)
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

        public List<BunkerCourtCaseEntry> GetUnlockedByDay(int currentDay)
        {
            var results = new List<BunkerCourtCaseEntry>();
            for (int i = 0; i < _allCases.Count; i++)
            {
                var c = _allCases[i];
                if (c.GetDocketDay() <= currentDay)
                {
                    results.Add(c);
                }
            }
            return results;
        }

        public List<BunkerCourtCaseEntry> GetBySearch(string query)
        {
            var results = new List<BunkerCourtCaseEntry>();
            if (string.IsNullOrWhiteSpace(query)) return results;

            for (int i = 0; i < _allCases.Count; i++)
            {
                var c = _allCases[i];
                if ((c.case_id != null && c.case_id.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (c.docket_number != null && c.docket_number.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (c.defendant_name != null && c.defendant_name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (c.presiding_magistrate != null && c.presiding_magistrate.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (c.charge_summary != null && c.charge_summary.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (c.verdict_outcome != null && c.verdict_outcome.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (c.disciplinary_penalty != null && c.disciplinary_penalty.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (c.clerk_margin_notes != null && c.clerk_margin_notes.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    results.Add(c);
                    continue;
                }

                if (c.tags != null)
                {
                    for (int j = 0; j < c.tags.Length; j++)
                    {
                        if (c.tags[j] != null && c.tags[j].IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            results.Add(c);
                            break;
                        }
                    }
                }
            }
            return results;
        }
    }
}
