using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// District 8 factions (Office / Cutters / Fleet). Currents-shaped.
    /// Do NOT add these to faction_lore.json.
    /// </summary>
    [Serializable]
    public class HoldfastFactionEntry
    {
        public string id;
        public string display_name;
        public string alignment;
        public string home_region;
        public bool is_active;
        public float trust;
        public string[] wants;
        public string[] offers;
        public string signature_quote;
        public string access_rule;
        public string badge_asset_id;
    }

    public static class HoldfastFactionsCatalogLoader
    {
        [Serializable]
        private class Container
        {
            public HoldfastFactionEntry[] entries;
        }

        private static List<HoldfastFactionEntry> _cache;

        public static List<HoldfastFactionEntry> Load()
        {
            if (_cache != null) return _cache;
            _cache = new List<HoldfastFactionEntry>();
            string path = Path.Combine(Application.streamingAssetsPath, "Data/holdfast_factions.json");
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

        public static HoldfastFactionEntry GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var all = Load();
            for (int i = 0; i < all.Count; i++)
                if (all[i].id == id) return all[i];
            return null;
        }
    }
}
