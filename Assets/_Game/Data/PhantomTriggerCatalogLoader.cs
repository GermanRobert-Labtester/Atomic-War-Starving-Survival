using System;
using System.IO;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Data
{
    /// <summary>Loads phantom_triggers.json into PhantomMemorySystem rules.</summary>
    public static class PhantomTriggerCatalogLoader
    {
        [Serializable]
        private class PhantomTriggerFileEntry
        {
            public string background_id;
            public PhantomTriggerJsonRule[] triggers;
        }

        [Serializable]
        private class PhantomTriggerJsonRule
        {
            public string item_category;
            public float motivation_chance;
            public string description;
            public string motivation_text;
            public string breakdown_text;
        }

        public static void LoadInto(PhantomMemorySystem system)
        {
            if (system == null) return;
            string path = Path.Combine(Application.streamingAssetsPath, "Data/phantom_triggers.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var wrapped = "{\"entries\":" + json + "}";
            var container = JsonUtility.FromJson<PhantomTriggerContainer>(wrapped);
            if (container?.entries == null) return;

            for (int i = 0; i < container.entries.Length; i++)
            {
                var entry = container.entries[i];
                if (entry?.triggers == null || string.IsNullOrEmpty(entry.background_id)) continue;
                for (int t = 0; t < entry.triggers.Length; t++)
                {
                    var rule = entry.triggers[t];
                    if (rule == null || string.IsNullOrEmpty(rule.item_category)) continue;
                    system.RegisterRule(
                        entry.background_id,
                        rule.item_category,
                        rule.motivation_chance,
                        rule.description ?? rule.item_category,
                        rule.motivation_text,
                        rule.breakdown_text);
                }
            }
        }

        [Serializable]
        private class PhantomTriggerContainer
        {
            public PhantomTriggerFileEntry[] entries;
        }
    }
}
