using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class WastelandSettlementEntry
    {
        public string settlement_id;
        public string settlement_name;
        public string colloquial_name;
        public string geographic_coordinates;
        public int estimated_population;
        public string controlling_faction;
        public string primary_export;
        public string defense_fortifications;
        public float water_source_radiation_mrh;
        public string harlan_scout_survey;
        public string[] tags;
    }

    [Serializable]
    public sealed class WastelandGazetteerFile
    {
        public int schema_version;
        public string collection_id;
        public List<WastelandSettlementEntry> settlements = new List<WastelandSettlementEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 20 Wasteland Outpost & Ruined Settlement Gazetteers.
    /// </summary>
    public sealed class WastelandGazetteerCatalog
    {
        private readonly Dictionary<string, WastelandSettlementEntry> _byId =
            new Dictionary<string, WastelandSettlementEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<WastelandSettlementEntry> _allSettlements = new List<WastelandSettlementEntry>();

        public IReadOnlyList<WastelandSettlementEntry> AllSettlements => _allSettlements;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<WastelandGazetteerFile>(json);
            if (file?.settlements == null) return;

            foreach (var s in file.settlements)
            {
                if (s == null || string.IsNullOrEmpty(s.settlement_id)) continue;
                _byId[s.settlement_id] = s;
                _allSettlements.Add(s);
            }
        }

        public WastelandSettlementEntry GetById(string settlementId)
        {
            if (string.IsNullOrEmpty(settlementId)) return null;
            _byId.TryGetValue(settlementId, out var entry);
            return entry;
        }

        public List<WastelandSettlementEntry> GetByFaction(string factionSnippet)
        {
            var results = new List<WastelandSettlementEntry>();
            if (string.IsNullOrEmpty(factionSnippet)) return results;

            for (int i = 0; i < _allSettlements.Count; i++)
            {
                var s = _allSettlements[i];
                if (s.controlling_faction != null &&
                    s.controlling_faction.IndexOf(factionSnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(s);
                }
            }
            return results;
        }

        public List<WastelandSettlementEntry> GetMajorHubs(int minPopulation = 100)
        {
            var results = new List<WastelandSettlementEntry>();
            for (int i = 0; i < _allSettlements.Count; i++)
            {
                var s = _allSettlements[i];
                if (s.estimated_population >= minPopulation)
                {
                    results.Add(s);
                }
            }
            return results;
        }

        public List<WastelandSettlementEntry> GetByTag(string tag)
        {
            var results = new List<WastelandSettlementEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allSettlements.Count; i++)
            {
                var s = _allSettlements[i];
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
