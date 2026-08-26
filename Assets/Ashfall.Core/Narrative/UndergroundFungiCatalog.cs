using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class UndergroundFungiEntry
    {
        public string species_id;
        public string scientific_name;
        public string common_name;
        public string habitat_substrate;
        public float optimal_depth_meters;
        public float light_requirement_lux;
        public int growth_cycle_days;
        public int spore_hazard_rating;
        public float edible_calories_per_100g;
        public string medicinal_extract;
        public string botanist_field_notes;
        public string[] tags;
    }

    [Serializable]
    public sealed class UndergroundFungiFile
    {
        public int schema_version;
        public string collection_id;
        public List<UndergroundFungiEntry> species = new List<UndergroundFungiEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 24 Subterranean Flora & Mycological Fungi Dossiers.
    /// </summary>
    public sealed class UndergroundFungiCatalog
    {
        private readonly Dictionary<string, UndergroundFungiEntry> _byId =
            new Dictionary<string, UndergroundFungiEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<UndergroundFungiEntry> _allSpecies = new List<UndergroundFungiEntry>();

        public IReadOnlyList<UndergroundFungiEntry> AllSpecies => _allSpecies;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<UndergroundFungiFile>(json);
            if (file?.species == null) return;

            foreach (var s in file.species)
            {
                if (s == null || string.IsNullOrEmpty(s.species_id)) continue;
                _byId[s.species_id] = s;
                _allSpecies.Add(s);
            }
        }

        public UndergroundFungiEntry? GetById(string speciesId)
        {
            if (string.IsNullOrEmpty(speciesId)) return null;
            _byId.TryGetValue(speciesId, out var entry);
            return entry;
        }

        public List<UndergroundFungiEntry> GetEdibleCrops()
        {
            var results = new List<UndergroundFungiEntry>();
            for (int i = 0; i < _allSpecies.Count; i++)
            {
                var s = _allSpecies[i];
                if (s.edible_calories_per_100g > 0f)
                {
                    results.Add(s);
                }
            }
            return results;
        }

        public List<UndergroundFungiEntry> GetHazardousSporeSpecies(int minHazardRating = 2)
        {
            var results = new List<UndergroundFungiEntry>();
            for (int i = 0; i < _allSpecies.Count; i++)
            {
                var s = _allSpecies[i];
                if (s.spore_hazard_rating >= minHazardRating)
                {
                    results.Add(s);
                }
            }
            return results;
        }

        public List<UndergroundFungiEntry> GetByTag(string tag)
        {
            var results = new List<UndergroundFungiEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allSpecies.Count; i++)
            {
                var s = _allSpecies[i];
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
