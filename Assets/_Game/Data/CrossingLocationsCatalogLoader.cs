using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — Crossing location cards.
    /// subset of holdings around the Region's viaduct gate and Scalehouse Row.
    /// Merged into the live LocationCatalogSO at boot, mirroring Holdfast.
    /// </summary>
    [Serializable]
    public class CrossingLocationEntry
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

    /// <summary>
    /// Loads crossing_locations.json. Mirrors HoldfastLocationsCatalogLoader.
    /// </summary>
    public static class CrossingLocationsCatalogLoader
    {
        [Serializable]
        private class Container
        {
            public CrossingLocationEntry[] entries;
        }

        private static List<CrossingLocationEntry> _cache;

        public static List<CrossingLocationEntry> Load()
        {
            if (_cache != null) return _cache;
            _cache = new List<CrossingLocationEntry>();
            string path = Path.Combine(Application.streamingAssetsPath, "Data/crossing_locations.json");
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

        public static CrossingLocationEntry GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var all = Load();
            for (int i = 0; i < all.Count; i++)
                if (all[i].id == id) return all[i];
            return null;
        }

        public static LocationDefinitionSO Materialise(CrossingLocationEntry e)
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

        public static string ComposeDescription(CrossingLocationEntry e)
        {
            if (e == null) return "";
            if (string.IsNullOrEmpty(e.inspect)) return e.description ?? "";
            if (string.IsNullOrEmpty(e.description)) return e.inspect;
            return e.inspect + "\n\n" + e.description;
        }

        /// <summary>Adds non-overlay Crossing locations to the live catalog.</summary>
        public static int ApplyToCatalog(LocationCatalogSO catalog)
        {
            if (catalog == null) return 0;
            if (catalog.locations == null) catalog.locations = new List<LocationDefinitionSO>();
            int changed = 0;
            var all = Load();
            for (int i = 0; i < all.Count; i++)
            {
                var e = all[i];
                if (e.overlay_on_unlock || e.recast_always) continue;

                var existing = catalog.GetById(e.id);
                if (existing != null) continue;

                catalog.locations.Add(Materialise(e));
                changed++;
            }
            return changed;
        }
    }
}