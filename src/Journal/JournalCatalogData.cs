using System;
using System.Collections.Generic;
#pragma warning disable CS0649
using Ashfall.Core;

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

    /// <summary>
    /// Room-history vignette row for the Places tab (Plan 29 Task 29A).
    /// Authored in shelter_room_identities.json; gated by the journal knowledge
    /// key room_history_seen_<id>. roomDisplayName is joined at load time.
    /// </summary>
    public class RoomHistoryCodexData
    {
        public string? id;
        public string? roomId;
        public string? roomDisplayName;
        public string? title;
        public string? timePeriod;
        public string? body;
    }

    public class JournalCatalogs
    {
        public List<ItemDefinitionData> Items = new List<ItemDefinitionData>();
        public List<LocationDefinitionData> Locations = new List<LocationDefinitionData>();
        public List<SurvivorArchetypeData> Survivors = new List<SurvivorArchetypeData>();
        public List<GameEventData> Events = new List<GameEventData>();
        /// <summary>Verdict world-history ladder (verdict_data.json.world_history_ladder).</summary>
        public List<GameEventData> VerdictHistory = new List<GameEventData>();
        /// <summary>Shelter room-history vignettes (shelter_room_identities.json, Plan 29 29A).</summary>
        public List<RoomHistoryCodexData> RoomHistories = new List<RoomHistoryCodexData>();

        public bool IsEmpty =>
            Items.Count == 0 && Locations.Count == 0 && Survivors.Count == 0 && Events.Count == 0
            && VerdictHistory.Count == 0 && RoomHistories.Count == 0;
    }

    public static class CatalogJsonLoader
    {
        /// <summary>
        /// Load the four codex catalogs from a data directory. Missing files are
        /// tolerated (empty lists) so the book still opens on a partial archive.
        /// </summary>
        public static JournalCatalogs Load(IFileIO fileIO, string dataDir)
        {
            var catalogs = new JournalCatalogs();
            if (fileIO == null || string.IsNullOrEmpty(dataDir) || !fileIO.DirectoryExists(dataDir))
                return catalogs;

            catalogs.Items = LoadList<ItemDefinitionData>(fileIO, fileIO.Combine(dataDir, "items.json"));
            catalogs.Locations = LoadList<LocationDefinitionData>(fileIO, fileIO.Combine(dataDir, "locations.json"));
            catalogs.Survivors = LoadList<SurvivorArchetypeData>(fileIO, fileIO.Combine(dataDir, "survivors.json"));
            catalogs.Events = LoadList<GameEventData>(fileIO, fileIO.Combine(dataDir, "events.json"));
            catalogs.VerdictHistory = LoadVerdictHistory(fileIO, dataDir);
            catalogs.RoomHistories = LoadRoomHistories(fileIO, dataDir);
            return catalogs;
        }

        private static List<GameEventData> LoadVerdictHistory(IFileIO fileIO, string dataDir)
        {
            var result = new List<GameEventData>();
            try
            {
                string path = fileIO.Combine(dataDir, "verdict_data.json");
                if (!fileIO.FileExists(path)) return result;
                string json = fileIO.ReadAllText(path);
                var ladder = CatalogLocator.LoadWrappedList<VerdictLadderRaw>(json, SystemTextJsonSerializer.Options);
                if (ladder == null) return result;
                foreach (var l in ladder)
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

        // ── Plan 29 29A: shelter room-history vignettes ────────────────────

        private class RoomIdentityFileRaw
        {
            public List<RoomIdentityRoomRaw>? rooms;
            public List<RoomIdentityVignetteRaw>? vignettes;
        }

        private class RoomIdentityRoomRaw
        {
            public string? id;
            public string? display_name;
        }

        private class RoomIdentityVignetteRaw
        {
            public string? id;
            public string? room_id;
            public string? title;
            public string? time_period;
            public string? body;
        }

        private static List<RoomHistoryCodexData> LoadRoomHistories(IFileIO fileIO, string dataDir)
        {
            var result = new List<RoomHistoryCodexData>();
            try
            {
                string path = fileIO.Combine(dataDir, "shelter_room_identities.json");
                if (!fileIO.FileExists(path)) return result;
                string json = fileIO.ReadAllText(path);
                var file = new SystemTextJsonSerializer().Deserialize<RoomIdentityFileRaw>(json);
                if (file == null) return result;

                var roomNames = new Dictionary<string, string>(StringComparer.Ordinal);
                if (file.rooms != null)
                {
                    foreach (var room in file.rooms)
                    {
                        if (room != null && !string.IsNullOrEmpty(room.id))
                            roomNames[room.id!] = room.display_name ?? room.id!;
                    }
                }
                if (file.vignettes == null) return result;
                foreach (var v in file.vignettes)
                {
                    if (v == null || string.IsNullOrEmpty(v.id) || string.IsNullOrEmpty(v.room_id)) continue;
                    result.Add(new RoomHistoryCodexData
                    {
                        id = v.id,
                        roomId = v.room_id,
                        roomDisplayName = roomNames.TryGetValue(v.room_id!, out var name) ? name : v.room_id,
                        title = v.title,
                        timePeriod = v.time_period,
                        body = v.body
                    });
                }
            }
            catch (Exception) { /* tolerate: identity is an overlay, never a domain dependency */ }
            return result;
        }

        private static List<T> LoadList<T>(IFileIO fileIO, string path)
        {
            try
            {
                if (!fileIO.FileExists(path)) return new List<T>();
                string json = fileIO.ReadAllText(path);
                return CatalogLocator.LoadWrappedList<T>(json, SystemTextJsonSerializer.Options) ?? new List<T>();
            }
            catch (Exception)
            {
                /* cleanup: fallback on missing or corrupt journal catalog */
                return new List<T>();
            }
        }
    }
}
