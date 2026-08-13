using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// District 8 location cards. inspect + description from the Holdfast creative pack.
    /// overlay_on_unlock = existing Sector 4 ids whose copy changes when the expansion unlocks.
    /// </summary>
    [Serializable]
    public class HoldfastLocationEntry
    {
        public string id;
        public string displayName;
        public string inspect;
        public string description;
        public float dangerLevel;
        public float travelHours;
        public float baseRadsPerHour;
        public string region;
        public bool overlay_on_unlock;
        public bool recast_always;
    }

    public static class HoldfastLocationsCatalogLoader
    {
        [Serializable]
        private class Container
        {
            public HoldfastLocationEntry[] entries;
        }

        private static List<HoldfastLocationEntry> _cache;

        public static List<HoldfastLocationEntry> Load()
        {
            if (_cache != null) return _cache;
            _cache = new List<HoldfastLocationEntry>();
            string path = Path.Combine(Application.streamingAssetsPath, "Data/holdfast_locations.json");
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

        public static HoldfastLocationEntry GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var all = Load();
            for (int i = 0; i < all.Count; i++)
                if (all[i].id == id) return all[i];
            return null;
        }

        public static LocationDefinitionSO Materialise(HoldfastLocationEntry e)
        {
            if (e == null) return null;
            var so = ScriptableObject.CreateInstance<LocationDefinitionSO>();
            so.id = e.id;
            so.displayName = e.displayName;
            so.dangerLevel = e.dangerLevel;
            so.travelHours = e.travelHours;
            so.baseRadsPerHour = e.baseRadsPerHour;
            so.description = ComposeDescription(e);
            return so;
        }

        public static string ComposeDescription(HoldfastLocationEntry e)
        {
            if (e == null) return "";
            if (string.IsNullOrEmpty(e.inspect)) return e.description ?? "";
            if (string.IsNullOrEmpty(e.description)) return e.inspect;
            return e.inspect + "\n\n" + e.description;
        }

        /// <summary>Always-on recasts (desalination / barge / convoy) plus unlock overlays.</summary>
        public static int ApplyToCatalog(LocationCatalogSO catalog, bool expansionUnlocked)
        {
            if (catalog == null) return 0;
            if (catalog.locations == null) catalog.locations = new List<LocationDefinitionSO>();
            int changed = 0;
            var all = Load();
            for (int i = 0; i < all.Count; i++)
            {
                var e = all[i];
                if (e.overlay_on_unlock && !expansionUnlocked) continue;

                var existing = catalog.GetById(e.id);
                if (existing != null)
                {
                    if (e.recast_always || e.overlay_on_unlock || expansionUnlocked)
                    {
                        existing.displayName = string.IsNullOrEmpty(e.displayName) ? existing.displayName : e.displayName;
                        existing.description = ComposeDescription(e);
                        if (e.travelHours > 0f) existing.travelHours = e.travelHours;
                        if (e.dangerLevel > 0f) existing.dangerLevel = e.dangerLevel;
                        if (e.baseRadsPerHour > 0f) existing.baseRadsPerHour = e.baseRadsPerHour;
                        changed++;
                    }
                    continue;
                }

                if (e.overlay_on_unlock || e.recast_always) continue;
                catalog.locations.Add(Materialise(e));
                changed++;
            }
            return changed;
        }
    }
}
