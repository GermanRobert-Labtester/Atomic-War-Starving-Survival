using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AtomicWar._Game.Events;
using Ashfall.Core.Journal;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// Runtime index over StreamingAssets/Data/world_history.json.
    ///
    /// Turns "located knowledge" (lore bible) into actual discovery calls:
    /// every world_history entry pairs a knowledge_key with a
    /// discovery_location_id, so arriving at a location should discover
    /// its lore. The UI-side WorldHistoryCatalogLoader already reads the
    /// same file for display; this class exists so the *gameplay* side can
    /// unlock entries in the Codex via JournalSystem.TryAddRawEntry
    /// (which fires OnEntryAdded → LoreCodexPanel.UnlockHistoryEntry).
    ///
    /// trigger semantics:
    ///   location_explore … discovered on first arrival at discovery_location_id
    ///   inspection        … discovered when the player inspects the bunker
    ///                        (player_shelter) — see DiscoverShelterInspection
    ///   journal / radio_intercept / survivor_dialogue … not location-bound;
    ///                        out of scope for this index (wired elsewhere)
    /// </summary>
    public static class LoreDiscoveryIndex
    {
        [Serializable]
        private class HistoryEntry
        {
            public string era;
            public string year_month;
            public string title;
            public string body;
            public string discovery_location_id;
            public string discovery_trigger;
            public string knowledge_key;
        }

        [Serializable]
        private class HistoryContainer
        {
            public HistoryEntry[] entries;
        }

        public const string PlayerShelterLocationId = "player_shelter";

        private static Dictionary<string, List<HistoryEntry>> _byLocation;

        private static void EnsureLoaded()
        {
            if (_byLocation != null) return;
            _byLocation = new Dictionary<string, List<HistoryEntry>>(StringComparer.Ordinal);

            string path = Path.Combine(Application.streamingAssetsPath, "Data/world_history.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var wrapped = "{\"entries\":" + json + "}";
            var container = JsonUtility.FromJson<HistoryContainer>(wrapped);
            if (container?.entries == null) return;

            for (int i = 0; i < container.entries.Length; i++)
            {
                var e = container.entries[i];
                if (e == null || string.IsNullOrEmpty(e.knowledge_key)) continue;

                string loc = string.IsNullOrEmpty(e.discovery_location_id)
                    ? PlayerShelterLocationId
                    : e.discovery_location_id;
                if (!_byLocation.TryGetValue(loc, out var list))
                {
                    list = new List<HistoryEntry>();
                    _byLocation[loc] = list;
                }
                list.Add(e);
            }
        }

        /// <summary>
        /// Every (knowledge_key, body) pair bound to the given location and
        /// discoverable on arrival (location_explore triggers). Fire-once is
        /// handled by KnowledgeBase; callers may invoke this repeatedly.
        /// </summary>
        public static List<KeyValuePair<string, string>> EntriesAtLocation(string locationId)
        {
            EnsureLoaded();
            var result = new List<KeyValuePair<string, string>>();
            if (string.IsNullOrEmpty(locationId) || _byLocation == null) return result;

            if (!_byLocation.TryGetValue(locationId, out var list)) return result;
            for (int i = 0; i < list.Count; i++)
            {
                var e = list[i];
                if (e.discovery_trigger != "location_explore") continue;
                result.Add(new KeyValuePair<string, string>(e.knowledge_key, e.body));
            }
            return result;
        }

        /// <summary>
        /// Bunker-inspection discoveries (trigger == "inspection", located in
        /// player_shelter): the Layer-1 wrongnesses, the open door, the
        /// nameplates, the claim. Called once from the lore HUD wiring —
        /// the act of opening the bunker's own history is the inspection.
        /// </summary>
        public static List<KeyValuePair<string, string>> ShelterInspectionEntries()
        {
            EnsureLoaded();
            var result = new List<KeyValuePair<string, string>>();
            if (_byLocation == null || !_byLocation.TryGetValue(PlayerShelterLocationId, out var list))
                return result;

            for (int i = 0; i < list.Count; i++)
            {
                var e = list[i];
                if (e.discovery_trigger != "inspection") continue;
                result.Add(new KeyValuePair<string, string>(e.knowledge_key, e.body));
            }
            return result;
        }
    }
}
