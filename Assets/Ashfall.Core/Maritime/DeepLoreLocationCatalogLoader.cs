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
    [Serializable]
    public class DeepLoreLocationEntry
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public float radiationUSv;
        public int dangerLevel;
        public float travelHours;
        public List<VariableLootNode> lootTable = new List<VariableLootNode>();
    }

    [Serializable]
    public class DeepLoreLocationContainer
    {
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
                                    CatalogDiagnostics.Warn("<unknown>", "unknown", ex_CATDIAG);
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
