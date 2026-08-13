using System;
using System.IO;
using UnityEngine;
using AtomicWar._Game.Events;
using Ashfall.Core.Journal;

namespace AtomicWar._Game.UI
{
    /// <summary>Loads world_history.json into LoreCodexPanel's history tab.</summary>
    public static class WorldHistoryCatalogLoader
    {
        [Serializable]
        private class WorldHistoryEntry
        {
            public string era;
            public string year_month;
            public string title;
            public string body;
            public string discovery_location_id;
            public string discovery_trigger;
            public string knowledge_key;
        }

        [Serializable]
        private class WorldHistoryContainer
        {
            public WorldHistoryEntry[] entries;
        }

        public static void LoadInto(LoreCodexPanel panel, KnowledgeBase knowledge)
        {
            if (panel == null) return;
            string path = Path.Combine(Application.streamingAssetsPath, "Data/world_history.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var wrapped = "{\"entries\":" + json + "}";
            var container = JsonUtility.FromJson<WorldHistoryContainer>(wrapped);
            if (container?.entries == null) return;

            for (int i = 0; i < container.entries.Length; i++)
            {
                var entry = container.entries[i];
                if (entry == null || string.IsNullOrEmpty(entry.knowledge_key)) continue;

                bool isDiscovered = knowledge != null && knowledge.Has(entry.knowledge_key);
                panel.AddHistoryEntry(
                    entry.knowledge_key,
                    entry.era,
                    entry.year_month,
                    entry.title,
                    entry.body,
                    entry.discovery_trigger,
                    isDiscovered);
            }
        }
    }
}
