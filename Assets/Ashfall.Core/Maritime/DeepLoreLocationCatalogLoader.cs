using System;
using System.Collections.Generic;

using Ashfall.Core.IO;
namespace Ashfall.Core.Maritime
{
    /// <summary>
    /// ASHFALL: THE BLACK FLOTILLA (Expansion 09) — loader for deep-lore
    /// locations with variable loot tables. Reads deep_lore_locations.json
    /// via the engine-agnostic IFileIO/IJsonSerializer ports.
    /// </summary>
    /// <summary>
    /// Data Transfer Object representing a deep lore dive/salvage location in JSON.
    /// </summary>
    [Serializable]
    public class DeepLoreLocationEntry
    {
        /// <summary>Unique location identifier (e.g. loc_deep_lore_01).</summary>
        public string id = string.Empty;

        /// <summary>Player-visible location name.</summary>
        public string displayName = string.Empty;

        /// <summary>Ambient radiation level in micro-Sieverts per hour (µSv/h).</summary>
        public float radiationUSv;

        /// <summary>Relative danger level tier (1-5).</summary>
        public int dangerLevel;

        /// <summary>Round-trip expedition travel time in hours.</summary>
        public float travelHours;

        /// <summary>List of variable loot nodes available for salvage rolls at this location.</summary>
        public List<VariableLootNode> lootTable = new List<VariableLootNode>();
    }

    /// <summary>
    /// Root JSON container for deep_lore_locations.json.
    /// </summary>
    [Serializable]
    public class DeepLoreLocationContainer
    {
        /// <summary>Collection of deep lore location definitions.</summary>
        public List<DeepLoreLocationEntry> locations = new List<DeepLoreLocationEntry>();
    }

    public static class DeepLoreLocationCatalogLoader
    {
        public const string FileName = "deep_lore_locations.json";

        public static List<DeepLoreLocationEntry> Load(
            string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new List<DeepLoreLocationEntry>();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return result;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path)) return result;

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw)) return result;

            try
            {
                var container = json.Deserialize<DeepLoreLocationContainer>(raw);
                if (container?.locations == null) return result;
                foreach (var loc in container.locations)
                {
                    if (loc == null || string.IsNullOrEmpty(loc.id)) continue;
                    result.Add(loc);
                }
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(path, "DeepLoreLocationContainer", ex_CATDIAG);
                return result;
            }
            return result;
        }

        public static DeepLoreLocationEntry? FindById(
            List<DeepLoreLocationEntry> locations, string id)
        {
            if (locations == null || string.IsNullOrEmpty(id)) return null;
            foreach (var loc in locations)
                if (loc.id == id) return loc;
            return null;
        }
    }
}
