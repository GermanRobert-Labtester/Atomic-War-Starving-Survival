using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class DwellerHeirloomEntry
    {
        public string heirloom_id;
        public string owner_id;
        public string owner_name;
        public string item_name;
        public string pre_war_origin;
        public string physical_condition;
        public int daily_morale_modifier;
        public int trauma_trigger_risk_percent;
        public string sensory_memory_text;
        public string item_loss_event_text;
        public string[] tags;
    }

    [Serializable]
    public sealed class DwellerHeirloomsFile
    {
        public int schema_version;
        public string collection_id;
        public List<DwellerHeirloomEntry> heirlooms = new List<DwellerHeirloomEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 30 Survivor Personal Keepsakes & Heirloom Registry.
    /// </summary>
    public sealed class DwellerHeirloomCatalog
    {
        private readonly Dictionary<string, DwellerHeirloomEntry> _byId =
            new Dictionary<string, DwellerHeirloomEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<DwellerHeirloomEntry> _allHeirlooms = new List<DwellerHeirloomEntry>();

        public IReadOnlyList<DwellerHeirloomEntry> AllHeirlooms => _allHeirlooms;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<DwellerHeirloomsFile>(json);
            if (file?.heirlooms == null) return;

            foreach (var h in file.heirlooms)
            {
                if (h == null || string.IsNullOrEmpty(h.heirloom_id)) continue;
                _byId[h.heirloom_id] = h;
                _allHeirlooms.Add(h);
            }
        }

        public DwellerHeirloomEntry? GetById(string heirloomId)
        {
            if (string.IsNullOrEmpty(heirloomId)) return null;
            _byId.TryGetValue(heirloomId, out var entry);
            return entry;
        }

        public List<DwellerHeirloomEntry> GetByOwner(string ownerId)
        {
            var results = new List<DwellerHeirloomEntry>();
            if (string.IsNullOrEmpty(ownerId)) return results;

            for (int i = 0; i < _allHeirlooms.Count; i++)
            {
                var h = _allHeirlooms[i];
                if (string.Equals(h.owner_id, ownerId, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(h);
                }
            }
            return results;
        }

        public List<DwellerHeirloomEntry> GetByTag(string tag)
        {
            var results = new List<DwellerHeirloomEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allHeirlooms.Count; i++)
            {
                var h = _allHeirlooms[i];
                if (h.tags == null) continue;
                for (int j = 0; j < h.tags.Length; j++)
                {
                    if (string.Equals(h.tags[j], tag, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(h);
                        break;
                    }
                }
            }
            return results;
        }
    }
}
