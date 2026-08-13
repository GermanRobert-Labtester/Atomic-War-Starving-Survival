using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    [Serializable]
    public class HoldfastQuestStageEntry
    {
        public string id;
        public string text;
    }

    [Serializable]
    public class HoldfastQuestChoiceEntry
    {
        public string id;
        public string text;
        public string set_flag;
    }

    [Serializable]
    public class HoldfastQuestEntry
    {
        public string id;
        public string display_name;
        public string type;
        public string briefing;
        public string prereq_quest_id;
        public int min_day;
        public HoldfastQuestStageEntry[] stages;
        public HoldfastQuestChoiceEntry[] choices;
        public string knowledge_key;
        public string target_location_id;

        public int StageCount => stages != null ? stages.Length : 0;
    }

    public static class HoldfastQuestCatalogLoader
    {
        [Serializable]
        private class Container
        {
            public HoldfastQuestEntry[] entries;
        }

        private static List<HoldfastQuestEntry> _cache;

        public static List<HoldfastQuestEntry> Load()
        {
            if (_cache != null) return _cache;
            _cache = new List<HoldfastQuestEntry>();
            string path = Path.Combine(Application.streamingAssetsPath, "Data/holdfast_quests.json");
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

        public static HoldfastQuestEntry GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var all = Load();
            for (int i = 0; i < all.Count; i++)
                if (all[i].id == id) return all[i];
            return null;
        }
    }
}
