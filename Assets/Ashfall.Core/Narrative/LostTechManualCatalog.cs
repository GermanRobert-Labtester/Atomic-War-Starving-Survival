using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class LostTechManualEntry
    {
        public string manual_id;
        public string manual_code;
        public string title;
        public string origin_facility;
        public string engineering_discipline;
        public int technical_complexity_tier;
        public float repair_failure_risk_pct;
        public string schematic_summary;
        public string dmitri_engineering_notes;
        public string[] tags;
    }

    [Serializable]
    public sealed class LostTechManualsFile
    {
        public int schema_version;
        public string collection_id;
        public List<LostTechManualEntry> manuals = new List<LostTechManualEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 24 Pre-War Technology Manuals & Lost Engineering Schematics.
    /// </summary>
    public sealed class LostTechManualCatalog
    {
        private readonly Dictionary<string, LostTechManualEntry> _byId =
            new Dictionary<string, LostTechManualEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<LostTechManualEntry> _allManuals = new List<LostTechManualEntry>();

        public IReadOnlyList<LostTechManualEntry> AllManuals => _allManuals;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<LostTechManualsFile>(json);
            if (file?.manuals == null) return;

            foreach (var m in file.manuals)
            {
                if (m == null || string.IsNullOrEmpty(m.manual_id)) continue;
                _byId[m.manual_id] = m;
                _allManuals.Add(m);
            }
        }

        public LostTechManualEntry GetById(string manualId)
        {
            if (string.IsNullOrEmpty(manualId)) return null;
            _byId.TryGetValue(manualId, out var entry);
            return entry;
        }

        public List<LostTechManualEntry> GetByComplexityTier(int tier)
        {
            var results = new List<LostTechManualEntry>();
            for (int i = 0; i < _allManuals.Count; i++)
            {
                var m = _allManuals[i];
                if (m.technical_complexity_tier == tier)
                {
                    results.Add(m);
                }
            }
            return results;
        }

        public List<LostTechManualEntry> GetByDiscipline(string disciplineSubstring)
        {
            var results = new List<LostTechManualEntry>();
            if (string.IsNullOrEmpty(disciplineSubstring)) return results;

            for (int i = 0; i < _allManuals.Count; i++)
            {
                var m = _allManuals[i];
                if (m.engineering_discipline != null &&
                    m.engineering_discipline.IndexOf(disciplineSubstring, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(m);
                }
            }
            return results;
        }

        public List<LostTechManualEntry> GetByTag(string tag)
        {
            var results = new List<LostTechManualEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allManuals.Count; i++)
            {
                var m = _allManuals[i];
                if (m.tags == null) continue;
                for (int j = 0; j < m.tags.Length; j++)
                {
                    if (string.Equals(m.tags[j], tag, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(m);
                        break;
                    }
                }
            }
            return results;
        }
    }
}
