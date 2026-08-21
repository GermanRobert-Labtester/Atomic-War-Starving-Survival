using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class GeologicalStrataEntry
    {
        public string strata_id;
        public string strata_name;
        public float depth_start_meters;
        public float depth_end_meters;
        public string rock_type;
        public float compressive_strength_mpa;
        public float radon_emission_bq_m3;
        public float fault_shear_stress_kpa;
        public float water_permeability_lpm;
        public string mining_hazard_type;
        public string geologist_field_notes;
        public string[] tags;
    }

    [Serializable]
    public sealed class GeologicalStrataFile
    {
        public int schema_version;
        public string collection_id;
        public List<GeologicalStrataEntry> strata = new List<GeologicalStrataEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 24 Subterranean Geological Strata & Seismology Logs.
    /// </summary>
    public sealed class GeologicalStrataCatalog
    {
        private readonly Dictionary<string, GeologicalStrataEntry> _byId =
            new Dictionary<string, GeologicalStrataEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<GeologicalStrataEntry> _allStrata = new List<GeologicalStrataEntry>();

        public IReadOnlyList<GeologicalStrataEntry> AllStrata => _allStrata;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<GeologicalStrataFile>(json);
            if (file?.strata == null) return;

            foreach (var s in file.strata)
            {
                if (s == null || string.IsNullOrEmpty(s.strata_id)) continue;
                _byId[s.strata_id] = s;
                _allStrata.Add(s);
            }
        }

        public GeologicalStrataEntry? GetById(string strataId)
        {
            if (string.IsNullOrEmpty(strataId)) return null;
            _byId.TryGetValue(strataId, out var entry);
            return entry;
        }

        public GeologicalStrataEntry? GetByDepth(float depthMeters)
        {
            float absDepth = Math.Abs(depthMeters);
            for (int i = 0; i < _allStrata.Count; i++)
            {
                var s = _allStrata[i];
                if (absDepth >= s.depth_start_meters && absDepth <= s.depth_end_meters)
                {
                    return s;
                }
            }
            return null;
        }

        public List<GeologicalStrataEntry> GetHighRadonHazards(float minRadonBqM3 = 1000.0f)
        {
            var results = new List<GeologicalStrataEntry>();
            for (int i = 0; i < _allStrata.Count; i++)
            {
                var s = _allStrata[i];
                if (s.radon_emission_bq_m3 >= minRadonBqM3)
                {
                    results.Add(s);
                }
            }
            return results;
        }

        public List<GeologicalStrataEntry> GetByTag(string tag)
        {
            var results = new List<GeologicalStrataEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allStrata.Count; i++)
            {
                var s = _allStrata[i];
                if (s.tags == null) continue;
                for (int j = 0; j < s.tags.Length; j++)
                {
                    if (string.Equals(s.tags[j], tag, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(s);
                        break;
                    }
                }
            }
            return results;
        }
    }
}
