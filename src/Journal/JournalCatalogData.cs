using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AtomicWar.Journal
{
    /// <summary>
    /// JSON-authored catalog DTOs + loader for the journal codex tabs. These
    /// mirror the StreamingAssets/Data files the Unity side ships; text is
    /// rendered verbatim — never paraphrased (house rule: text is the asset).
    /// </summary>
    public class ItemDefinitionData
    {
        public string? id;
        public string? displayName;
        public string? description;
        public string? type;
        public float weight;
        public float tradeValue;
        public float durability;
    }

    public class LocationDefinitionData
    {
        public string? id;
        public string? displayName;
        public string? description;
        public float dangerLevel;
        public float baseRadsPerHour;
    }

    public class SurvivorArchetypeData
    {
        public string? id;
        public string? displayName;
        public string? profession;
        public string? bio;
    }

    public class GameEventData
    {
        public string? id;
        public string? title;
        public string? bodyText;
    }

    public class JournalCatalogs
    {
        public List<ItemDefinitionData> Items = new List<ItemDefinitionData>();
        public List<LocationDefinitionData> Locations = new List<LocationDefinitionData>();
        public List<SurvivorArchetypeData> Survivors = new List<SurvivorArchetypeData>();
        public List<GameEventData> Events = new List<GameEventData>();
        /// <summary>Verdict world-history ladder (verdict_data.json.world_history_ladder).</summary>
        public List<GameEventData> VerdictHistory = new List<GameEventData>();

        public bool IsEmpty =>
            Items.Count == 0 && Locations.Count == 0 && Survivors.Count == 0 && Events.Count == 0
            && VerdictHistory.Count == 0;
    }

    public static class CatalogJsonLoader
    {
        private static readonly JsonSerializerOptions s_options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true
        };

        /// <summary>
        /// Load the four codex catalogs from a data directory. Missing files are
        /// tolerated (empty lists) so the book still opens on a partial archive.
        /// </summary>
        public static JournalCatalogs Load(string dataDir)
        {
            var catalogs = new JournalCatalogs();
            if (string.IsNullOrEmpty(dataDir) || !Directory.Exists(dataDir))
                return catalogs;

            catalogs.Items = LoadList<ItemDefinitionData>(Path.Combine(dataDir, "items.json"));
            catalogs.Locations = LoadList<LocationDefinitionData>(Path.Combine(dataDir, "locations.json"));
            catalogs.Survivors = LoadList<SurvivorArchetypeData>(Path.Combine(dataDir, "survivors.json"));
            catalogs.Events = LoadList<GameEventData>(Path.Combine(dataDir, "events.json"));
            catalogs.VerdictHistory = LoadVerdictHistory(dataDir);
            return catalogs;
        }

        /// <summary>Map verdict_data.json.world_history_ladder to codex event rows
        /// (knowledge_key → id, title, body_summary → body).</summary>
        private static List<GameEventData> LoadVerdictHistory(string dataDir)
        {
            var result = new List<GameEventData>();
            try
            {
                string path = Path.Combine(dataDir, "verdict_data.json");
                if (!File.Exists(path)) return result;
                string json = File.ReadAllText(path);
                var root = JsonSerializer.Deserialize<VerdictDataRaw>(json, s_options);
                if (root?.world_history_ladder == null) return result;
                foreach (var l in root.world_history_ladder)
                {
                    if (l == null || string.IsNullOrEmpty(l.knowledge_key)) continue;
                    result.Add(new GameEventData
                    {
                        id = l.knowledge_key,
                        title = l.title,
                        bodyText = l.body_summary
                    });
                }
            }
            catch (Exception) { /* tolerate */ }
            return result;
        }

        private class VerdictLadderRaw
        {
            public string? knowledge_key;
            public string? title;
            public string? body_summary;
            public string? discovery_location_id;
        }

        private class VerdictDataRaw
        {
            public List<VerdictLadderRaw>? world_history_ladder;
        }

        private static List<T> LoadList<T>(string path)
        {
            try
            {
                if (!File.Exists(path)) return new List<T>();
                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<List<T>>(json, s_options) ?? new List<T>();
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }
    }
}
