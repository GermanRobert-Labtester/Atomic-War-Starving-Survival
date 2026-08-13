using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// A Current is a non-territorial faction (lore bible 05_FACTIONS).
    /// Unlike the four territorial Powers in faction_lore.json, Currents have
    /// no relationships and no diplomacy — the player holds access, granted or
    /// withdrawn, rather than trust-based standing. Loaded from
    /// StreamingAssets/Data/currents.json.
    /// </summary>
    [Serializable]
    public class CurrentEntry
    {
        public string id;
        public string display_name;
        /// <summary>peaceful | conditional | dangerous.</summary>
        public string alignment;
        public string home_region;
        /// <summary>True when a live NPC_* class already implements behaviour.</summary>
        public bool is_active;
        public float trust;
        public string[] wants;
        public string[] offers;
        public string signature_quote;
        public string access_rule;
        /// <summary>Resources path id (Art/Factions) adopted per the bible's badge table; "" = none.</summary>
        public string badge_asset_id;
    }

    /// <summary>
    /// Loads currents.json (flat, JsonUtility-safe — no dictionaries).
    /// Follows the PhantomTriggerCatalogLoader pattern.
    /// </summary>
    public static class CurrentsCatalogLoader
    {
        [Serializable]
        private class CurrentsContainer
        {
            public CurrentEntry[] entries;
        }

        private static List<CurrentEntry> _cache;

        /// <summary>All Current entries in file order. Cached after first load.</summary>
        public static List<CurrentEntry> Load()
        {
            if (_cache != null) return _cache;
            _cache = new List<CurrentEntry>();

            string path = Path.Combine(Application.streamingAssetsPath, "Data/currents.json");
            if (!File.Exists(path)) return _cache;

            string json = File.ReadAllText(path);
            var wrapped = "{\"entries\":" + json + "}";
            var container = JsonUtility.FromJson<CurrentsContainer>(wrapped);
            if (container?.entries == null) return _cache;

            for (int i = 0; i < container.entries.Length; i++)
            {
                var e = container.entries[i];
                if (e == null || string.IsNullOrEmpty(e.id)) continue;
                _cache.Add(e);
            }
            return _cache;
        }

        /// <summary>Look up a Current by snake_case id (null-safe).</summary>
        public static CurrentEntry GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var all = Load();
            for (int i = 0; i < all.Count; i++)
                if (all[i].id == id) return all[i];
            return null;
        }
    }
}
