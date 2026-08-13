using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — quest cards for the Crossing's opening
    /// arcs. Mirrors HoldfastQuestCatalogLoader so the same host-side pattern
    /// drives them. Quest ids are registered in CrossingIds.Quests; this file
    /// is the data (briefings, stages, resolution choices).
    /// </summary>
    [Serializable]
    public class CrossingQuestStageEntry
    {
        public string id;
        public string text;
    }

    [Serializable]
    public class CrossingQuestChoiceEntry
    {
        public string id;
        public string text;
        public string set_flag;
    }

    [Serializable]
    public class CrossingQuestEntry
    {
        public string id;
        public string display_name;
        public string type;
        public string briefing;
        public string prereq_quest_id;
        public int min_day;
        public CrossingQuestStageEntry[] stages;
        public CrossingQuestChoiceEntry[] choices;
        public string knowledge_key;
        public string target_location_id;

        public int StageCount => stages != null ? stages.Length : 0;
    }

    /// <summary>Loads crossing_quests.json (flat, JsonUtility-safe).</summary>
    public static class CrossingQuestCatalogLoader
    {
        [Serializable]
        private class Container
        {
            public CrossingQuestEntry[] entries;
        }

        private static List<CrossingQuestEntry> _cache;

        public static List<CrossingQuestEntry> Load()
        {
            if (_cache != null) return _cache;
            _cache = new List<CrossingQuestEntry>();
            string path = Path.Combine(Application.streamingAssetsPath, "Data/crossing_quests.json");
            if (!File.Exists(path)) return _cache;

            string json = File.ReadAllText(path);
            var wrapped = "{\"entries\":" + json + "}";
            var container = JsonUtility.FromJson<Container>(wrapped);
            if (container?.entries == null) return _cache;

            for (int i = 0; i < container.entries.Length; i++)
            {
                var e = container.entries[i];
                if (e == null || string.IsNullOrEmpty(e.id)) continue;
                _cache.Add(e);
            }
            return _cache;
        }

        public static CrossingQuestEntry GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var all = Load();
            for (int i = 0; i < all.Count; i++)
                if (all[i].id == id) return all[i];
            return null;
        }
    }
}