using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class RelicDossierEntry
    {
        public string relic_id;
        public string name;
        public string tone;
        public string material;
        public string discovery_location;
        public string curator_note;
        public string gameplay_effect;
        public string[] tags;
    }

    [Serializable]
    public sealed class RelicProvenanceFile
    {
        public int schema_version;
        public string collection_id;
        public List<RelicDossierEntry> relics = new List<RelicDossierEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 32 Relic Provenance Master Dossiers.
    /// </summary>
    public sealed class RelicProvenanceCatalog
    {
        private readonly Dictionary<string, RelicDossierEntry> _byId =
            new Dictionary<string, RelicDossierEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<RelicDossierEntry> _allRelics = new List<RelicDossierEntry>();

        public IReadOnlyList<RelicDossierEntry> AllRelics => _allRelics;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<RelicProvenanceFile>(json);
            if (file?.relics == null) return;

            foreach (var r in file.relics)
            {
                if (r == null || string.IsNullOrEmpty(r.relic_id)) continue;
                _byId[r.relic_id] = r;
                _allRelics.Add(r);
            }
        }

        public RelicDossierEntry? GetById(string relicId)
        {
            if (string.IsNullOrEmpty(relicId)) return null;
            _byId.TryGetValue(relicId, out var entry);
            return entry;
        }

        public List<RelicDossierEntry> GetByTone(string tone)
        {
            var results = new List<RelicDossierEntry>();
            if (string.IsNullOrEmpty(tone)) return results;

            for (int i = 0; i < _allRelics.Count; i++)
            {
                var r = _allRelics[i];
                if (string.Equals(r.tone, tone, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(r);
                }
            }
            return results;
        }

        public List<RelicDossierEntry> GetByTag(string tag)
        {
            var results = new List<RelicDossierEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allRelics.Count; i++)
            {
                var r = _allRelics[i];
                if (r.tags == null) continue;
                for (int j = 0; j < r.tags.Length; j++)
                {
                    if (string.Equals(r.tags[j], tag, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(r);
                        break;
                    }
                }
            }
            return results;
        }
    }
}
