using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.IO;

namespace Ashfall.Core.World
{
    [Serializable]
    public sealed class SeasonalEventDef
    {
        public string id = string.Empty;
        public string season_id = string.Empty;
        public string name = string.Empty;
        public string description = string.Empty;
        public float trigger_chance = 0.35f;
        public int cooldown_days = 14;
        public string severity = "Medium";
        public string category = "Shelter";
        public string impact_summary = string.Empty;
        public string mitigation_item_id = string.Empty;
        public int mitigation_cost = 0;
    }

    [Serializable]
    public sealed class SeasonalEventCatalogFile
    {
        public int schema_version = 1;
        public List<SeasonalEventDef> events = new List<SeasonalEventDef>();
    }

    public static class SeasonalEventCatalogLoader
    {
        public const string FileName = "seasonal_events.json";

        public static List<SeasonalEventDef> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var list = new List<SeasonalEventDef>();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return list;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return list;

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return list;

            try
            {
                var parsed = json.Deserialize<SeasonalEventCatalogFile>(raw);
                if (parsed?.events != null)
                {
                    list.AddRange(parsed.events);
                }
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(path, "SeasonalEventCatalogFile", ex);
            }

            return list;
        }
    }
}
