using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// Expansion XI — "The Glass Orchard": StreamingAssets/Data/greenhouse_items.json.
    ///
    /// Seeds, greenhouse infrastructure (planter boxes, grow lamps, lead-glass
    /// panes, blight treatment, grow medium), and the harvested crops (clean
    /// and tainted). Mirrors <see cref="HoldfastItemsCatalogLoader"/>: the JSON
    /// is a bare array, wrapped into <c>{"entries":[...]}</c> for JsonUtility.
    /// </summary>
    [Serializable]
    public class GreenhouseItemEntry
    {
        public string id;
        public string displayName;
        [TextArea(2, 5)] public string description;
        public string type;
        public int stackMax = 1;
        public float weight = 0.4f;
        public float tradeValue = 12f;

        // On-consume effects (food yields).
        public float hungerRestore;
        public float thirstRestore;
        public float moraleEffect;
        public float healthEffect;
        public float radCleanse;

        // Contamination (0..1) for tainted harvests.
        public float contamination;

        // Equipment / durability for infrastructure items.
        public float durability;
        public bool isEquipable;
        public bool empShielded;
        public float radProtection;
    }

    public static class GreenhouseItemsCatalogLoader
    {
        [Serializable]
        private class Container
        {
            public GreenhouseItemEntry[] entries;
        }

        private static List<GreenhouseItemEntry> _cache;

        public static List<GreenhouseItemEntry> Load()
        {
            if (_cache != null) return _cache;
            _cache = new List<GreenhouseItemEntry>();
            string path = Path.Combine(Application.streamingAssetsPath, "Data/greenhouse_items.json");
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

        public static GreenhouseItemEntry GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var all = Load();
            for (int i = 0; i < all.Count; i++)
                if (all[i].id == id) return all[i];
            return null;
        }

        public static ItemDefinition Materialise(GreenhouseItemEntry e)
        {
            if (e == null) return null;
            var so = ScriptableObject.CreateInstance<ItemDefinition>();
            so.id = e.id;
            so.displayName = e.displayName;
            so.description = e.description;
            so.stackMax = e.stackMax > 0 ? e.stackMax : 1;
            so.weight = e.weight;
            so.tradeValue = e.tradeValue;
            so.hungerRestore = e.hungerRestore;
            so.thirstRestore = e.thirstRestore;
            so.moraleEffect = e.moraleEffect;
            so.healthEffect = e.healthEffect;
            so.radCleanse = e.radCleanse;
            so.contamination = Mathf.Clamp01(e.contamination);
            so.durability = e.durability;
            so.isEquipable = e.isEquipable;
            so.empShielded = e.empShielded;
            so.radProtection = e.radProtection;
            if (!Enum.TryParse(e.type, true, out ItemType parsed))
                parsed = ItemType.Material;
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
