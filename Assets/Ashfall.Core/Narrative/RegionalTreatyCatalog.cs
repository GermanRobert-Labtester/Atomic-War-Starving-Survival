using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class RegionalTreatyEntry
    {
        public string treaty_id;
        public int ratified_day;
        public string treaty_title;
        public string[] signatory_factions;
        public string demarcated_territory;
        public float water_allocation_lpm;
        public float power_quota_kw;
        public string tariff_schedule;
        public string treaty_articles;
        public string penalties;
        public string[] tags;
    }

    [Serializable]
    public sealed class RegionalTreatiesFile
    {
        public int schema_version;
        public string collection_id;
        public List<RegionalTreatyEntry> treaties = new List<RegionalTreatyEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 16-Faction Regional Treaty & Border Protocols.
    /// </summary>
    public sealed class RegionalTreatyCatalog
    {
        private readonly Dictionary<string, RegionalTreatyEntry> _byId =
            new Dictionary<string, RegionalTreatyEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<RegionalTreatyEntry> _allTreaties = new List<RegionalTreatyEntry>();

        public IReadOnlyList<RegionalTreatyEntry> AllTreaties => _allTreaties;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<RegionalTreatiesFile>(json);
            if (file?.treaties == null) return;

            foreach (var t in file.treaties)
            {
                if (t == null || string.IsNullOrEmpty(t.treaty_id)) continue;
                _byId[t.treaty_id] = t;
                _allTreaties.Add(t);
            }
        }

        public RegionalTreatyEntry? GetById(string treatyId)
        {
            if (string.IsNullOrEmpty(treatyId)) return null;
            _byId.TryGetValue(treatyId, out var entry);
            return entry;
        }

        public List<RegionalTreatyEntry> GetRatifiedByDay(int currentDay)
        {
            var results = new List<RegionalTreatyEntry>();
            for (int i = 0; i < _allTreaties.Count; i++)
            {
                var t = _allTreaties[i];
                if (t.ratified_day <= currentDay)
                {
                    results.Add(t);
                }
            }
            return results;
        }

        public List<RegionalTreatyEntry> GetBySignatoryFaction(string factionIdSnippet)
        {
            var results = new List<RegionalTreatyEntry>();
            if (string.IsNullOrEmpty(factionIdSnippet)) return results;

            for (int i = 0; i < _allTreaties.Count; i++)
            {
                var t = _allTreaties[i];
                if (t.signatory_factions == null) continue;
                for (int j = 0; j < t.signatory_factions.Length; j++)
                {
                    if (t.signatory_factions[j] != null &&
                        t.signatory_factions[j].IndexOf(factionIdSnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        results.Add(t);
                        break;
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// Exact-faction lookup. The substring API above can collide across the
        /// `current_` vocabulary (e.g. current_1 matches current_10..16), so
        /// treaty wiring MUST use this exact match against the canonical faction
        /// id, never a snippet.
        /// </summary>
        public List<RegionalTreatyEntry> GetByExactSignatoryFaction(string factionId)
        {
            var results = new List<RegionalTreatyEntry>();
            if (string.IsNullOrEmpty(factionId)) return results;

            for (int i = 0; i < _allTreaties.Count; i++)
            {
                var t = _allTreaties[i];
                if (t.signatory_factions == null) continue;
                for (int j = 0; j < t.signatory_factions.Length; j++)
                {
                    if (string.Equals(t.signatory_factions[j], factionId, StringComparison.Ordinal))
                    {
                        results.Add(t);
                        break;
                    }
                }
            }
            return results;
        }

        public List<RegionalTreatyEntry> GetByTag(string tag)
        {
            var results = new List<RegionalTreatyEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allTreaties.Count; i++)
            {
                var t = _allTreaties[i];
                if (t.tags == null) continue;
                for (int j = 0; j < t.tags.Length; j++)
                {
                    if (string.Equals(t.tags[j], tag, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(t);
                        break;
                    }
                }
            }
            return results;
        }
    }
}
