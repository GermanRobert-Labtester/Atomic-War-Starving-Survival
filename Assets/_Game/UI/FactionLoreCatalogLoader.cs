using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>Loads faction_lore.json into LoreCodexPanel's faction tab.</summary>
    public static class FactionLoreCatalogLoader
    {
        [Serializable]
        private class FactionRelationships
        {
            public string iron_garrison;
            public string ash_militia;
            public string cult_of_ash_sign;
            public string warlords_sector_4;
            public string faction_rebuilders;
            public string faction_black_ops;
        }

        [Serializable]
        private class FactionLoreEntry
        {
            public string faction_id;
            public string display_name;
            public string ideology;
            public string origin_story;
            public string[] key_beliefs;
            public string signature_quote;
            public FactionRelationships relationships;
        }

        [Serializable]
        private class FactionLoreContainer
        {
            public FactionLoreEntry[] entries;
        }

        // FactionRelationships is a fixed-field DTO (JsonUtility can't deserialize into a
        // Dictionary), so it only has slots for these 6 factions. A faction id outside this
        // list would have any relationship data referencing it silently dropped by JsonUtility.
        private static readonly string[] KnownRelationshipFactionIds =
        {
            "iron_garrison", "ash_militia", "cult_of_ash_sign", "warlords_sector_4",
            "faction_rebuilders", "faction_black_ops"
        };

        public static void LoadInto(LoreCodexPanel panel)
        {
            if (panel == null) return;
            string path = Path.Combine(Application.streamingAssetsPath, "Data/faction_lore.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var wrapped = "{\"entries\":" + json + "}";
            var container = JsonUtility.FromJson<FactionLoreContainer>(wrapped);
            if (container?.entries == null) return;

            for (int i = 0; i < container.entries.Length; i++)
            {
                var entry = container.entries[i];
                if (entry == null || string.IsNullOrEmpty(entry.faction_id)) continue;

                if (Array.IndexOf(KnownRelationshipFactionIds, entry.faction_id) < 0)
                    Debug.LogWarning($"[FactionLoreCatalogLoader] '{entry.faction_id}' is not one of the " +
                        $"{KnownRelationshipFactionIds.Length} factions FactionRelationships has fields for; " +
                        "relationships involving it will be silently dropped. Add a field for it to that DTO.");

                panel.AddFactionEntry(
                    entry.faction_id,
                    entry.display_name,
                    entry.ideology,
                    entry.origin_story,
                    entry.key_beliefs,
                    entry.signature_quote,
                    ToRelationshipMap(entry.faction_id, entry.relationships),
                    isDiscovered: true);
            }
        }

        private static Dictionary<string, string> ToRelationshipMap(string factionId, FactionRelationships rel)
        {
            var map = new Dictionary<string, string>();
            if (rel == null) return map;

            AddIfPresent(map, factionId, "iron_garrison", rel.iron_garrison);
            AddIfPresent(map, factionId, "ash_militia", rel.ash_militia);
            AddIfPresent(map, factionId, "cult_of_ash_sign", rel.cult_of_ash_sign);
            AddIfPresent(map, factionId, "warlords_sector_4", rel.warlords_sector_4);
            AddIfPresent(map, factionId, "faction_rebuilders", rel.faction_rebuilders);
            AddIfPresent(map, factionId, "faction_black_ops", rel.faction_black_ops);
            return map;
        }

        private static void AddIfPresent(Dictionary<string, string> map, string selfId, string otherId, string value)
        {
            if (string.IsNullOrEmpty(value) || otherId == selfId) return;
            map[otherId] = value;
        }
    }
}
