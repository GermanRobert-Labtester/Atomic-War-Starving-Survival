using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — item cards (vouch token, calibration
    /// weight, Scale trade goods). Mirrors HoldfastItemsCatalogLoader.
    /// </summary>
    [Serializable]
    public class CrossingItemEntry
    {
        public string id;
        public string displayName;
        public string description;
        public string type;
        public int stackMax = 1;
        public float weight = 0.4f;
        public float tradeValue = 12f;
        public float thirstRestore;
        public float hungerRestore;
        public float moraleEffect;
    }

    /// <summary>Loads crossing_items.json (flat, JsonUtility-safe).</summary>
    public static class CrossingItemsCatalogLoader
    {
        [Serializable]
        private class Container
        {
            public CrossingItemEntry[] entries;
        }

        private static List<CrossingItemEntry> _cache;

        public static List<CrossingItemEntry> Load()
        {
            if (_cache != null) return _cache;
            _cache = new List<CrossingItemEntry>();
            string path = Path.Combine(Application.streamingAssetsPath, "Data/crossing_items.json");
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

        public static CrossingItemEntry GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var all = Load();
            for (int i = 0; i < all.Count; i++)
                if (all[i].id == id) return all[i];
            return null;
        }

        public static ItemDefinition Materialise(CrossingItemEntry e)
        {
            if (e == null) return null;
            var so = ScriptableObject.CreateInstance<ItemDefinition>();
            so.id = e.id;
            so.displayName = e.displayName;
            so.description = e.description;
            so.stackMax = e.stackMax > 0 ? e.stackMax : 1;
            so.weight = e.weight;
            so.tradeValue = e.tradeValue;
            so.thirstRestore = e.thirstRestore;
            so.hungerRestore = e.hungerRestore;
            so.moraleEffect = e.moraleEffect;
            if (!Enum.TryParse(e.type, true, out ItemType parsed))
                parsed = ItemType.Quest;
            so.type = parsed;
            return so;
        }

        public static List<ItemDefinition> MaterialiseAll()
        {
            var src = Load();
            var list = new List<ItemDefinition>(src.Count);
            for (int i = 0; i < src.Count; i++)
            {
                var so = Materialise(src[i]);
                if (so != null) list.Add(so);
            }
            return list;
        }
    }
}