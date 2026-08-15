using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    [Serializable]
    public class CrossingChoiceEntry
    {
        public string text;
        public string[] cost_items;
        public string result;
    }

    [Serializable]
    public class CrossingEncounterEntry
    {
        public string id;
        public string name;
        public string target_location;
        public string description;
        public string threat_level;
        public CrossingChoiceEntry[] choices;
    }

    [Serializable]
    public class CrossingCrisisEntry
    {
        public string id;
        public string name;
        public string[] phases;
        public string description;
        public string resolution;
    }

    [Serializable]
    public class CrossingEncountersContainer
    {
        public CrossingEncounterEntry[] encounters;
        public CrossingCrisisEntry[] crises;
    }

    /// <summary>
    /// Loads crossing_encounters.json containing the 10 canonical encounters
    /// and 5 multi-phase crises of Nobody's Charter.
    /// </summary>
    public static class CrossingEncountersCatalogLoader
    {
        private static CrossingEncountersContainer _cache;

        public static CrossingEncountersContainer Load()
        {
            if (_cache != null) return _cache;
            string path = Path.Combine(Application.streamingAssetsPath, "Data/crossing_encounters.json");
            if (!File.Exists(path))
            {
                _cache = new CrossingEncountersContainer
                {
                    encounters = Array.Empty<CrossingEncounterEntry>(),
                    crises = Array.Empty<CrossingCrisisEntry>()
                };
                return _cache;
            }

            string json = File.ReadAllText(path);
            _cache = JsonUtility.FromJson<CrossingEncountersContainer>(json) ?? new CrossingEncountersContainer
            {
                encounters = Array.Empty<CrossingEncounterEntry>(),
                crises = Array.Empty<CrossingCrisisEntry>()
            };
            return _cache;
        }

        public static CrossingEncounterEntry GetEncounter(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var data = Load();
            if (data?.encounters == null) return null;
            for (int i = 0; i < data.encounters.Length; i++)
            {
                if (data.encounters[i].id == id) return data.encounters[i];
            }
            return null;
        }

        public static CrossingCrisisEntry GetCrisis(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var data = Load();
            if (data?.crises == null) return null;
            for (int i = 0; i < data.crises.Length; i++)
            {
                if (data.crises[i].id == id) return data.crises[i];
            }
            return null;
        }
    }
}
