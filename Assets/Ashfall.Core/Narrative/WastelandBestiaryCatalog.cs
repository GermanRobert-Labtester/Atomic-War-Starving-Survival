using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class WastelandCreatureEntry
    {
        public string creature_id;
        public string common_name;
        public string colloquial_name;
        public string primary_habitat;
        public int threat_level;
        public string pack_size_range;
        public float acoustic_lure_frequency_hz;
        public string[] harvestable_materials;
        public float butchered_meat_calories;
        public string harlan_scout_notes;
        public string[] tags;
    }

    [Serializable]
    public sealed class WastelandBestiaryFile
    {
        public int schema_version;
        public string collection_id;
        public List<WastelandCreatureEntry> creatures = new List<WastelandCreatureEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 24 Irradiated Wasteland Fauna & Cryptid Bestiary.
    /// </summary>
    public sealed class WastelandBestiaryCatalog
    {
        private readonly Dictionary<string, WastelandCreatureEntry> _byId =
            new Dictionary<string, WastelandCreatureEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<WastelandCreatureEntry> _allCreatures = new List<WastelandCreatureEntry>();

        public IReadOnlyList<WastelandCreatureEntry> AllCreatures => _allCreatures;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<WastelandBestiaryFile>(json);
            if (file?.creatures == null) return;

            foreach (var c in file.creatures)
            {
                if (c == null || string.IsNullOrEmpty(c.creature_id)) continue;
                _byId[c.creature_id] = c;
                _allCreatures.Add(c);
            }
        }

        public WastelandCreatureEntry? GetById(string creatureId)
        {
            if (string.IsNullOrEmpty(creatureId)) return null;
            _byId.TryGetValue(creatureId, out var entry);
            return entry;
        }

        public List<WastelandCreatureEntry> GetByThreatLevel(int threatLevel)
        {
            var results = new List<WastelandCreatureEntry>();
            for (int i = 0; i < _allCreatures.Count; i++)
            {
                var c = _allCreatures[i];
                if (c.threat_level == threatLevel)
                {
                    results.Add(c);
                }
            }
            return results;
        }

        public List<WastelandCreatureEntry> GetGameYields(float minCalories = 1000.0f)
        {
            var results = new List<WastelandCreatureEntry>();
            for (int i = 0; i < _allCreatures.Count; i++)
            {
                var c = _allCreatures[i];
                if (c.butchered_meat_calories >= minCalories)
                {
                    results.Add(c);
                }
            }
            return results;
        }

        public List<WastelandCreatureEntry> GetByTag(string tag)
        {
            var results = new List<WastelandCreatureEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allCreatures.Count; i++)
            {
                var c = _allCreatures[i];
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
