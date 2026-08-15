using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — the Crossing's three blocs
    /// (Scale / Underwrite / Compact). Currents-shaped DTO, deliberately NOT
    /// added to faction_lore.json (bible §5 compliance: a gap in power, not a
    /// fifth Power). Loaded from StreamingAssets/Data/crossing_factions.json.
    /// </summary>
    [Serializable]
    public class CrossingFactionEntry
    {
        public string id;
        public string display_name;
        /// <summary>peaceful | conditional | dangerous.</summary>
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

    /// <summary>
    /// Loads crossing_factions.json (flat, JsonUtility-safe). Mirrors
    /// HoldfastFactionsCatalogLoader.
    /// </summary>
    public static class CrossingFactionsCatalogLoader
    {
        [Serializable]
        private class Container
        {
            public CrossingFactionEntry[] entries;
        }

        private static List<CrossingFactionEntry> _cache;

        public static List<CrossingFactionEntry> Load()
        {
            if (_cache != null) return _cache;
            _cache = new List<CrossingFactionEntry>();
            string path = Path.Combine(Application.streamingAssetsPath, "Data/crossing_factions.json");
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

        public static CrossingFactionEntry GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var all = Load();
            for (int i = 0; i < all.Count; i++)
                if (all[i].id == id) return all[i];
            return null;
        }
    }
}