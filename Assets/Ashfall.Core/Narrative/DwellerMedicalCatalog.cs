using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class DwellerMedicalCaseEntry
    {
        public string case_id;
        public int recorded_day;
        public string attending_physician;
        public string patient_id;
        public string patient_name;
        public string category;
        public float dose_estimate_msv;
        public string symptoms;
        public string intervention;
        public string outcome;
        public string doctor_margin_note;
        public string[] tags;
    }

    [Serializable]
    public sealed class DwellerMedicalCasebookFile
    {
        public int schema_version;
        public string collection_id;
        public List<DwellerMedicalCaseEntry> cases = new List<DwellerMedicalCaseEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 40 Dweller Medical Casebook & Trauma Profiles.
    /// </summary>
    public sealed class DwellerMedicalCatalog
    {
        private readonly Dictionary<string, DwellerMedicalCaseEntry> _byId =
            new Dictionary<string, DwellerMedicalCaseEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<DwellerMedicalCaseEntry> _allCases = new List<DwellerMedicalCaseEntry>();

        public IReadOnlyList<DwellerMedicalCaseEntry> AllCases => _allCases;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<DwellerMedicalCasebookFile>(json);
            if (file?.cases == null) return;

            foreach (var c in file.cases)
            {
                if (c == null || string.IsNullOrEmpty(c.case_id)) continue;
                _byId[c.case_id] = c;
                _allCases.Add(c);
            }
        }

        public DwellerMedicalCaseEntry? GetById(string caseId)
        {
            if (string.IsNullOrEmpty(caseId)) return null;
            _byId.TryGetValue(caseId, out var entry);
            return entry;
        }

        public List<DwellerMedicalCaseEntry> GetUnlockedByDay(int currentDay)
        {
            var results = new List<DwellerMedicalCaseEntry>();
            for (int i = 0; i < _allCases.Count; i++)
            {
                var c = _allCases[i];
                if (c.recorded_day <= currentDay)
                {
                    results.Add(c);
                }
            }
            return results;
        }

        public List<DwellerMedicalCaseEntry> GetByCategory(string categorySnippet)
        {
            var results = new List<DwellerMedicalCaseEntry>();
            if (string.IsNullOrEmpty(categorySnippet)) return results;

            for (int i = 0; i < _allCases.Count; i++)
            {
                var c = _allCases[i];
                if (c.category != null && c.category.IndexOf(categorySnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(c);
                }
            }
            return results;
        }

        public List<DwellerMedicalCaseEntry> GetByPhysician(string physicianSnippet)
        {
            var results = new List<DwellerMedicalCaseEntry>();
            if (string.IsNullOrEmpty(physicianSnippet)) return results;

            for (int i = 0; i < _allCases.Count; i++)
            {
                var c = _allCases[i];
                if (c.attending_physician != null && c.attending_physician.IndexOf(physicianSnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(c);
                }
            }
            return results;
        }
    }
}
